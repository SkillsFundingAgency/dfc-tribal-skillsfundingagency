using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public class AddressController : BaseController
    {
        const String MessageArea = "ABImport";
        readonly String cancelImportMessage;

        public AddressController() : base()
        {
            cancelImportMessage = AppGlobal.Language.GetText(this, "CancellingImport", "Cancelling import...");
        }

        [HttpPost]
        public NewtonsoftJsonResult GetAddresses(String postcode)
        {
            if (!postcode.Contains(" ") && postcode.Length > 3)
            {
                postcode = postcode.Substring(0, postcode.Length - 3) + " " + postcode.Substring(postcode.Length - 3, 3);
            }

            var filteredAddresses = (from a in db.AddressBases where a.Postcode.Equals(postcode) select a).ToList();

            var addresses = filteredAddresses.Select(address => new AddressBaseModel(address)).ToList();

            return Json(addresses);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadAddressBaseData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult Index()
        {
            UploadAddressBaseModel model = new UploadAddressBaseModel();
            GetViewData();
            GetLastUploadDetails(model);

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanUploadAddressBaseData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult Index(UploadAddressBaseModel model)
        {
            ViewBag.IsComplete = true;

            // Check if import is already in progress.
            ProgressMessage pm = db.ProgressMessages.Find(MessageArea);
            if (pm != null)
            {
                if (!pm.IsComplete)
                {
                    ViewBag.IsComplete = false;
                    ModelState.AddModelError("", AppGlobal.Language.GetText(this, "ImportInProgress", "Import Already In Progress"));
                }
                else
                {
                    DeleteProgressMessage();
                }
            }

            if (ModelState.IsValid)
            {
                String[] validFileTypes = {".zip"};
                String AddressBaseFolder = Constants.ConfigSettings.AddressBaseUploadVirtualDirectoryName;
                if (AddressBaseFolder.EndsWith(@"\"))
                {
                    AddressBaseFolder = AddressBaseFolder.Substring(0, AddressBaseFolder.Length - 1);
                }

                // Check if config setting is valid
                if (String.IsNullOrEmpty(AddressBaseFolder) || !Directory.Exists(AddressBaseFolder))
                {
                    ModelState.AddModelError("", AppGlobal.Language.GetText(this, "AddressBaseFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringAddressBaseFiles is not set or is incorrect"));
                    DeleteProgressMessage();
                }

                String importErrorMessageText = AppGlobal.Language.GetText(this, "ImportError", "Error Importing Address Base : {0}");
                try
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase uploadedFile = Request.Files[i];
                        if (uploadedFile == null)
                        {
                            continue;
                        }
                        String ext = Path.GetExtension(uploadedFile.FileName);
                        if (!validFileTypes.Contains(ext ?? "", StringComparer.CurrentCultureIgnoreCase))
                        {
                            ModelState.AddModelError("File",
                                AppGlobal.Language.GetText(this, "ZIPFilesOnly", "Please upload a ZIP file"));
                        }

                        if (ModelState.IsValid)
                        {
                            String ZIPFile = Path.Combine(AddressBaseFolder, uploadedFile.FileName);

                            // Delete the file if it exists
                            if (System.IO.File.Exists(ZIPFile))
                            {
                                TryToDeleteFile(ZIPFile);
                            }

                            // Save the zip file
                            uploadedFile.SaveAs(ZIPFile);

                            // Sometimes it uploads the file as zero bytes.
                            // If so log the error and delete the file
                            FileInfo fi = new FileInfo(ZIPFile);
                            if (fi.Length == 0)
                            {
                                ModelState.AddModelError("", String.Format(AppGlobal.Language.GetText("FileUploadFailed", "Error uploading file: {0}"), uploadedFile.FileName));
                                fi.Delete();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Create an error
                    AddOrReplaceProgressMessage(String.Format(importErrorMessageText, ex.Message), true);
                }
            }

            // No Errors so redirect to index which will show messages
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            GetViewData();
            GetLastUploadDetails(model);

            return View(model);
        }

        [NonAction]
        private void GetLastUploadDetails(UploadAddressBaseModel model)
        {
            MetadataUpload dataUpload = db.MetadataUploads.Where(m => m.MetadataUploadTypeId == (Int32)Constants.MetadataUploadType.AddressBase).OrderByDescending(m => m.CreatedDateTimeUtc).FirstOrDefault();
            if (dataUpload != null)
            {
                if (dataUpload.AspNetUser == null)
                {
                    AspNetUser user = db.AspNetUsers.Find(dataUpload.CreatedByUserId);
                    if (user != null)
                    {
                        model.LastUploadedBy = user.Name;
                    }
                }
                else
                {
                    model.LastUploadedBy = dataUpload.AspNetUser.Name;
                }
                model.LastUploadDateTimeUtc = dataUpload.CreatedDateTimeUtc;
                DateTime.SpecifyKind(model.LastUploadDateTimeUtc.Value, DateTimeKind.Utc);
                try
                {
                    model.LastUploadFileName = String.Join("<br />", dataUpload.FileName.Split(';'));
                }
                catch {}
            }
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadAddressBaseData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult StartImport()
        {
            // Check if import is already in progress.
            ProgressMessage pm = db.ProgressMessages.Find(MessageArea);
            if (pm != null)
            {
                if (!pm.IsComplete)
                {
                    ModelState.AddModelError("", AppGlobal.Language.GetText(this, "ImportInProgress", "Import Already In Progress"));
                }
                else
                {
                    DeleteProgressMessage();
                }
            }

            String AddressBaseFolder = Constants.ConfigSettings.AddressBaseUploadVirtualDirectoryName;
            if (AddressBaseFolder.EndsWith(@"\"))
            {
                AddressBaseFolder = AddressBaseFolder.Substring(0, AddressBaseFolder.Length - 1);
            }

            // Check if config setting is valid
            if (String.IsNullOrEmpty(AddressBaseFolder) || !Directory.Exists(AddressBaseFolder))
            {
                ModelState.AddModelError("", AppGlobal.Language.GetText(this, "AddressBaseFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringAddressBaseFiles is not set or is incorrect"));
                DeleteProgressMessage();
            }

            if (ModelState.IsValid)
            {
                // Get the CSV Filenames
                String[] zipFiles = Directory.GetFiles(AddressBaseFolder, "*.zip");
                if (zipFiles.GetLength(0) == 0)
                {
                    ModelState.AddModelError("", AppGlobal.Language.GetText(this, "UnableToFindZIPFile", "Unable to find ZIP file(s) to import"));
                    DeleteProgressMessage();
                }
                else
                {
                    AddOrReplaceProgressMessage(AppGlobal.Language.GetText(this, "StartingImport", "Starting Import..."));
                    Boolean cancellingImport = false;
                    String importingMessageText = AppGlobal.Language.GetText(this, "ImportingFileXOfY", "Importing file {0} of {1}...");
                    String unzippingMessageText = AppGlobal.Language.GetText(this, "UnzippingFileXOfY", "Unzipping file {0} of {1}...");
                    String mergingMessageText = AppGlobal.Language.GetText(this, "MergeData", "Merging Data...");
                    String removingTempDataMessageText = AppGlobal.Language.GetText(this, "RemovingTemporaryData", "Removing Temporary Data...");
                    String importSuccessfulMessageText = AppGlobal.Language.GetText(this, "ImportSuccessful", "Address Base Data Successfully Imported");
                    String importCancelledMessageText = AppGlobal.Language.GetText(this, "ImportCancelled", "Address Base Data Import Cancelled");
                    String importErrorMessageText = AppGlobal.Language.GetText(this, "ImportError", "Error Importing Address Base : {0}");
                    String userId = Permission.GetCurrentUserId();
                    new Thread(() =>                    
                    {
                        try
                        {
                            ProviderPortalEntities _db = new ProviderPortalEntities();

                            const Int32 UPRN = 0;
                            const Int32 ORGANISATION_NAME = 3;
                            const Int32 DEPARTMENT_NAME = 4;
                            const Int32 PO_BOX_NUMBER = 5;
                            const Int32 SUB_BUILDING_NAME = 6;
                            const Int32 BUILDING_NAME = 7;
                            const Int32 BUILDING_NUMBER = 8;
                            const Int32 DEPENDENT_THOROUGHFARE_NAME = 9;
                            const Int32 THOROUGHFARE_NAME = 10;
                            const Int32 POST_TOWN = 11;
                            const Int32 DOUBLE_DEPENDENT_LOCALITY = 12;
                            const Int32 DEPENDENT_LOCALITY = 13;
                            const Int32 POSTCODE = 14;
                            const Int32 CHANGE_TYPE = 22;
                            const Int32 LATITUDE = 18;
                            const Int32 LONGITUDE = 19;

                            var totalSize = 0;
                            var fileNames = String.Empty;
                            foreach (var item in zipFiles)
                            {
                                totalSize += (int)new FileInfo(item).Length;
                                fileNames += Path.GetFileName(item) + ";";
                            }

                            var metadataUpload = new MetadataUpload
                            {
                                MetadataUploadTypeId = (int)Constants.MetadataUploadType.AddressBase,
                                CreatedByUserId = userId,
                                CreatedDateTimeUtc = DateTime.UtcNow,
                                FileName = fileNames.TrimEnd(';'),
                                FileSizeInBytes = totalSize,
                                RowsBefore = _db.AddressBases.Count()
                            };
                            var sw = new Stopwatch();
                            sw.Start();

                            // Open the database
                            SqlConnection conn = new SqlConnection(_db.Database.Connection.ConnectionString);
                            conn.Open();

                            // Truncate the temporary import table just incase there's still data in there.
                            SqlCommand comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_AddressBase];", conn);
                            comm.ExecuteNonQuery();

                            // Setup the DataTable
                            DataTable dt = new DataTable();
                            dt.Columns.Add(new DataColumn { ColumnName = "UPRN", AllowDBNull = false, DataType = typeof(Int64) });
                            dt.Columns.Add(new DataColumn { ColumnName = "Postcode", AllowDBNull = false, DataType = typeof(String), MaxLength = 8 });
                            dt.Columns.Add(new DataColumn { ColumnName = "OrganisationName", AllowDBNull = true, DataType = typeof(String), MaxLength = 60 });
                            dt.Columns.Add(new DataColumn { ColumnName = "DepartmentName", AllowDBNull = true, DataType = typeof(String), MaxLength = 60 });
                            dt.Columns.Add(new DataColumn { ColumnName = "POBoxNumber", AllowDBNull = true, DataType = typeof(String), MaxLength = 6 });
                            dt.Columns.Add(new DataColumn { ColumnName = "BuildingName", AllowDBNull = true, DataType = typeof(String), MaxLength = 50 });
                            dt.Columns.Add(new DataColumn { ColumnName = "SubBuildingName", AllowDBNull = true, DataType = typeof(String), MaxLength = 30 });
                            dt.Columns.Add(new DataColumn { ColumnName = "BuildingNumber", AllowDBNull = true, DataType = typeof(Int32) });
                            dt.Columns.Add(new DataColumn { ColumnName = "DependentThoroughfareName", AllowDBNull = true, DataType = typeof(String), MaxLength = 80 });
                            dt.Columns.Add(new DataColumn { ColumnName = "ThoroughfareName", AllowDBNull = true, DataType = typeof(String), MaxLength = 80 });
                            dt.Columns.Add(new DataColumn { ColumnName = "Town", AllowDBNull = true, DataType = typeof(String), MaxLength = 30 });
                            dt.Columns.Add(new DataColumn { ColumnName = "DoubleDependentLocality", AllowDBNull = true, DataType = typeof(String), MaxLength = 35 });
                            dt.Columns.Add(new DataColumn { ColumnName = "DependentLocality", AllowDBNull = true, DataType = typeof(String), MaxLength = 35 });
                            dt.Columns.Add(new DataColumn { ColumnName = "Latitude", AllowDBNull = true, DataType = typeof(Decimal) });
                            dt.Columns.Add(new DataColumn { ColumnName = "Longitude", AllowDBNull = true, DataType = typeof(Decimal) });

                            Int32 i = 1;
                            foreach (String zipFile in zipFiles)
                            {
                                // Write the progress message
                                AddOrReplaceProgressMessage(_db, String.Format(unzippingMessageText, i, zipFiles.GetLength(0)));

                                // Unzip the file
                                System.IO.Compression.ZipFile.ExtractToDirectory(zipFile, AddressBaseFolder);

                                // Delete the zip file
                                TryToDeleteFile(zipFile);

                                String[] csvFiles = Directory.GetFiles(AddressBaseFolder, "*.csv");
                                if (csvFiles.GetLength(0) == 0)
                                {
                                    ModelState.AddModelError("", AppGlobal.Language.GetText(this, "UnableToFindCSVFile", "Unable to find CSV file to import"));
                                    DeleteProgressMessage();
                                }

                                foreach (String csvFile in csvFiles)
                                {
                                    // Check if we have stopped the import
                                    if (IsCancellingImport(new ProviderPortalEntities()))
                                    {
                                        break;
                                    }

                                    // Remove all the rows
                                    dt.Clear();

                                    // Write the progress message
                                    AddOrReplaceProgressMessage(_db, String.Format(importingMessageText, i++, zipFiles.GetLength(0)));

                                    // Import the CSV file
                                    using (CsvReader csv = new CsvReader(new StreamReader(csvFile)))
                                    {
                                        csv.Configuration.HasHeaderRecord = false;
                                        while (csv.Read())
                                        {
                                            if (csv[CHANGE_TYPE] == "D")
                                            {
                                                AddressBase addressBase = _db.AddressBases.Find(Convert.ToInt32(csv[UPRN]));
                                                if (addressBase != null)
                                                {
                                                    _db.Entry(addressBase).State = EntityState.Deleted;
                                                    _db.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                DataRow dr = dt.NewRow();
                                                dr["UPRN"] = Convert.ToInt64(csv[UPRN]);
                                                dr["Postcode"] = csv[POSTCODE];
                                                dr["OrganisationName"] = csv[ORGANISATION_NAME];
                                                dr["DepartmentName"] = csv[DEPARTMENT_NAME];
                                                dr["POBoxNumber"] = csv[PO_BOX_NUMBER];
                                                dr["BuildingName"] = csv[BUILDING_NAME];
                                                dr["SubBuildingName"] = csv[SUB_BUILDING_NAME];
                                                dr["BuildingNumber"] = csv[BUILDING_NUMBER] == "" ? DBNull.Value : (Object) Convert.ToInt32(csv[BUILDING_NUMBER]);
                                                dr["DependentThoroughfareName"] = csv[DEPENDENT_THOROUGHFARE_NAME];
                                                dr["ThoroughfareName"] = csv[THOROUGHFARE_NAME];
                                                dr["Town"] = csv[POST_TOWN];
                                                dr["DoubleDependentLocality"] = csv[DOUBLE_DEPENDENT_LOCALITY];
                                                dr["DependentLocality"] = csv[DEPENDENT_LOCALITY];
                                                dr["Latitude"] = csv[LATITUDE] == "" ? (Object) DBNull.Value : Convert.ToDecimal(csv[LATITUDE]);
                                                dr["Longitude"] = csv[LONGITUDE] == "" ? (Object) DBNull.Value : Convert.ToDecimal(csv[LONGITUDE]);
                                                dt.Rows.Add(dr);
                                            }

                                            // Every 100 rows, check whether we are cancelling the import
                                            if (csv.Row%100 == 0 && IsCancellingImport(new ProviderPortalEntities()))
                                            {
                                                cancellingImport = true;
                                                break;
                                            }
                                        }
                                        csv.Dispose();

                                        // Delete the file to tidy up space as quickly as possible
                                        TryToDeleteFile(csvFile);
                                    }

                                    if (!cancellingImport)
                                    {
                                        // Copy the data to the Import_BaseAddress Table                                
                                        BulkImportData(conn, dt);
                                    }
                                }
                            }

                            cancellingImport = IsCancellingImport(new ProviderPortalEntities());
                            if (!cancellingImport)
                            {
                                // Merge the data into the AddressBase Table
                                AddOrReplaceProgressMessage(_db, mergingMessageText);
                                comm = new SqlCommand("MERGE [dbo].[AddressBase] dest USING [dbo].[Import_AddressBase] source ON dest.UPRN = source.UPRN WHEN MATCHED THEN UPDATE SET dest.Postcode = source.Postcode, dest.OrganisationName = source.OrganisationName, dest.DepartmentName = source.DepartmentName, dest.POBoxNumber = source.POBoxNumber, dest.BuildingName = source.BuildingName, dest.SubBuildingname = source.SubBuildingName, dest.BuildingNumber = source.BuildingNumber, dest.DependentThoroughfareName = source.DependentThoroughfareName, dest.ThoroughfareName = source.ThoroughfareName, dest.Town = source.Town, dest.DoubleDependentLocality = source.DoubleDependentLocality, dest.DependentLocality = source.DependentLocality, dest.Latitude = source.Latitude, dest.Longitude = source.Longitude WHEN NOT MATCHED THEN INSERT (UPRN, Postcode, OrganisationName, DepartmentName, POBoxNumber, BuildingName, SubBuildingName, BuildingNumber, DependentThoroughfareName, ThoroughfareName, Town, DoubleDependentLocality, DependentLocality, Latitude, Longitude) VALUES (source.UPRN, source.Postcode, source.OrganisationName, source.DepartmentName, source.POBoxNumber, source.BuildingName, source.SubBuildingName, source.BuildingNumber, source.DependentThoroughfareName, source.ThoroughfareName, source.Town, source.DoubleDependentLocality, source.DependentLocality, source.Latitude, source.Longitude);", conn)
                                {
                                    CommandTimeout = 7200 /* 2 Hours */
                                };
                                comm.ExecuteNonQuery();
                            }

                            // Truncate the temporary import table
                            if (!cancellingImport)
                            {
                                AddOrReplaceProgressMessage(_db, removingTempDataMessageText);
                            }
                            comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_AddressBase];", conn)
                            {
                                CommandTimeout = 3600 /* 1 Hours */
                            };

                            comm.ExecuteNonQuery();

                            // Close the database
                            conn.Close();

                            // Save timings
                            if (!cancellingImport)
                            {
                                sw.Stop();
                                _db.Database.CommandTimeout = 3600; /* 1 Hour */
                                metadataUpload.DurationInMilliseconds = (int) sw.ElapsedMilliseconds;
                                metadataUpload.RowsAfter = _db.AddressBases.Count();
                                _db.MetadataUploads.Add(metadataUpload);
                                _db.SaveChanges();
                            }

                            // Delete all the uploaded and expanded files (if any still exist)
                            try
                            {
                                foreach (FileInfo file in new DirectoryInfo(AddressBaseFolder).GetFiles())
                                {
                                    file.Delete();
                                }
                            }
                            catch {}

                            // Write Success or Cancelled message
                            AddOrReplaceProgressMessage(_db, cancellingImport ? importCancelledMessageText : importSuccessfulMessageText, true);
                        }
                        catch (Exception ex)
                        {
                            // Log the error
                            AppGlobal.Log.WriteError(String.Format("Error importing AddressBase: Message:{0}, Inner Message:{1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : ""));

                            // Write Error to Display to User
                            AddOrReplaceProgressMessage(new ProviderPortalEntities(), String.Format(importErrorMessageText, ex.InnerException != null ? ex.InnerException.Message : ex.Message), true);

                            // Delete all the uploaded and expanded files (if any still exist)
                            try
                            {
                                foreach (FileInfo file in new DirectoryInfo(AddressBaseFolder).GetFiles())
                                {
                                    file.Delete();
                                }
                            }
                            catch {}
                        }
                    }).Start();
                }
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            GetViewData();

            return View("Index", new UploadAddressBaseModel());
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadAddressBaseData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult CancelImport()
        {
            ProgressMessage pm = db.ProgressMessages.Find(MessageArea);
            if (pm != null && !pm.IsComplete)
            {
                AddOrReplaceProgressMessage(cancelImportMessage);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadAddressBaseData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult ForceCancelImport()
        {
            DeleteProgressMessage();

            String AddressBaseFolder = Constants.ConfigSettings.AddressBaseUploadVirtualDirectoryName;
            if (AddressBaseFolder.EndsWith(@"\"))
            {
                AddressBaseFolder = AddressBaseFolder.Substring(0, AddressBaseFolder.Length - 1);
            }

            // Check if config setting is valid
            if (String.IsNullOrEmpty(AddressBaseFolder) || !Directory.Exists(AddressBaseFolder))
            {
                ModelState.AddModelError("", AppGlobal.Language.GetText(this, "AddressBaseFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringAddressBaseFiles is not set or is incorrect"));
                DeleteProgressMessage();
            } 
            
            foreach (FileInfo file in new DirectoryInfo(AddressBaseFolder).GetFiles())
            {
                file.Delete();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadAddressBaseData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult DeleteFile(String fileName)
        {
            // If the file passed in has a \ in it then return nothing.
            if (!fileName.IsPathSafe())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            String AddressBaseFolder = Constants.ConfigSettings.AddressBaseUploadVirtualDirectoryName;
            if (AddressBaseFolder.EndsWith(@"\"))
            {
                AddressBaseFolder = AddressBaseFolder.Substring(0, AddressBaseFolder.Length - 1);
            }

            // Check if config setting is valid
            if (String.IsNullOrEmpty(AddressBaseFolder) || !Directory.Exists(AddressBaseFolder))
            {
                AddOrReplaceProgressMessage(AppGlobal.Language.GetText(this, "AddressBaseFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringAddressBaseFiles is not set or is incorrect"), true);
            }
            else
            {
                if (!fileName.ToLower().EndsWith(".zip"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                try
                {
                    System.IO.File.Delete(Path.Combine(AddressBaseFolder, fileName));
                }
                catch {}
            }

            return RedirectToAction("Index");
        }

        [NonAction]
        private void GetViewData()
        {
            ViewBag.IsComplete = true;
            ViewBag.IsCancelling = false;

            ProgressMessage pm = db.ProgressMessages.Find(MessageArea);
            if (pm != null)
            {
                ViewBag.Message = pm.MessageText;
                ViewBag.IsComplete = pm.IsComplete;
                ViewBag.IsCancelling = pm.MessageText.Equals(cancelImportMessage);

                if (pm.IsComplete)
                {
                    db.Entry(pm).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }

            String AddressBaseFolder = Constants.ConfigSettings.AddressBaseUploadVirtualDirectoryName;
            if (AddressBaseFolder.EndsWith(@"\"))
            {
                AddressBaseFolder = AddressBaseFolder.Substring(0, AddressBaseFolder.Length - 1);
            }

            // Check if config setting is valid
            if (String.IsNullOrEmpty(AddressBaseFolder) || !Directory.Exists(AddressBaseFolder))
            {
                ModelState.AddModelError("", AppGlobal.Language.GetText(this, "AddressBaseFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringAddressBaseFiles is not set or is incorrect"));
                DeleteProgressMessage();
            }
            else
            {
                String[] zipFiles = Directory.GetFiles(AddressBaseFolder, "*.zip");
                ViewBag.NumberOfFiles = zipFiles.GetLength(0);
                ViewBag.Files = zipFiles;
            }
        }

        [NonAction]
        private Boolean IsCancellingImport(ProviderPortalEntities _db)
        {
            // Check if we have stopped the import
            ProgressMessage pm = _db.ProgressMessages.FirstOrDefault(x => x.MessageArea == MessageArea);
            return pm == null || pm.MessageText == cancelImportMessage;
        }

        [NonAction]
        private void DeleteProgressMessage()
        {
            ProgressMessage pm = db.ProgressMessages.Find(MessageArea);
            if (pm != null)
            {
                db.Entry(pm).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        [NonAction]
        private void AddOrReplaceProgressMessage(String message, Boolean isComplete = false)
        {
            AddOrReplaceProgressMessage(db, message, isComplete);
        }

        [NonAction]
        private static void AddOrReplaceProgressMessage(ProviderPortalEntities _db, String message, Boolean isComplete = false)
        {
            ProgressMessage pm = _db.ProgressMessages.Find(MessageArea);
            if (pm != null)
            {
                _db.Entry(pm).State = EntityState.Modified;
            }
            else
            {
                pm = new ProgressMessage
                {
                    MessageArea = MessageArea
                };
                _db.Entry(pm).State = EntityState.Added;
            }
            pm.MessageText = message;
            pm.IsComplete = isComplete;

            _db.SaveChanges();
        }

        [NonAction]
        private static void BulkImportData(SqlConnection conn, DataTable dt)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
            {
                // The table I'm loading the data to
                bulkCopy.DestinationTableName = "[dbo].[Import_AddressBase]";

                // How many records to send to the database in one go (all of them)
                bulkCopy.BatchSize = dt.Rows.Count;

                // Set the timeout to 10 minutes - should be plenty
                bulkCopy.BulkCopyTimeout = 600;

                // Load the data to the database
                bulkCopy.WriteToServer(dt);

                // Close up          
                bulkCopy.Close();
            }
        }

        [NonAction]
        private static Boolean TryToDeleteFile(String fileName)
        {
            try
            {
                System.IO.File.Delete(fileName);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}