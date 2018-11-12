using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public class UCASImportController : BaseController
    {
        private enum UCASUpdateType
        {
            Full = 0,
            Incremental = 1,
            PG = 2
        }

        private const String MessageArea = "UCASImport";
        private readonly String cancelImportMessage;

        // ReSharper disable once RedundantBaseConstructorCall
        public UCASImportController() : base()
        {
            cancelImportMessage = AppGlobal.Language.GetText(this, "CancellingImport", "Cancelling import...");
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadUCASData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult Index()
        {
            GetViewData();

            UploadUCASDataModel model = new UploadUCASDataModel();
            GetLastUploadDetails(model);

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanUploadUCASData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult Index(UploadUCASDataModel model)
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
                Boolean validFileType = false;
                String UCASImportFolder = Constants.ConfigSettings.UCASImportUploadVirtualDirectoryName;
                if (UCASImportFolder.EndsWith(@"\"))
                {
                    UCASImportFolder = UCASImportFolder.Substring(0, UCASImportFolder.Length - 1);
                }

                // Check if config setting is valid
                if (String.IsNullOrEmpty(UCASImportFolder) || !Directory.Exists(UCASImportFolder))
                {
                    ModelState.AddModelError("", AppGlobal.Language.GetText(this, "UCASImportFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringUCASImportFiles is not set or is incorrect"));
                    DeleteProgressMessage();
                }

                String importErrorMessageText = AppGlobal.Language.GetText(this, "ImportError", "Error Importing UCAS Data : {0}");
                try
                {
                    foreach (String fileType in validFileTypes)
                    {
                        if (model.File.FileName.ToLower().EndsWith(fileType))
                        {
                            validFileType = true;
                            break;
                        }
                    }
                    if (!validFileType)
                    {
                        ModelState.AddModelError("File", AppGlobal.Language.GetText(this, "ZIPFilesOnly", "Please upload a ZIP file"));
                    }

                    if (ModelState.IsValid)
                    {
                        String ZIPFile = String.Format(@"{0}\{1}", UCASImportFolder, model.File.FileName);

                        // Save the zip file
                        model.File.SaveAs(ZIPFile);

                        // Import the file
                        ImportData();
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
        private void ImportData()
        {
            String importErrorMessageText = AppGlobal.Language.GetText(this, "ImportError", "Error Importing UCAS Data : {0}");

            String UCASImportFolder = Constants.ConfigSettings.UCASImportUploadVirtualDirectoryName;
            if (UCASImportFolder.EndsWith(@"\"))
            {
                UCASImportFolder = UCASImportFolder.Substring(0, UCASImportFolder.Length - 1);
            }

            String fileName = "";
            Int32 fileSize = 0;
            foreach (FileInfo file in new DirectoryInfo(UCASImportFolder).GetFiles("*.zip"))
            {
                // Get filename and size for meta data upload history
                fileName = file.Name;
                fileSize = (Int32) file.Length;

                // Unzip the file
                System.IO.Compression.ZipFile.ExtractToDirectory(file.FullName, UCASImportFolder);

                // Delete the zip file
                file.Delete();
            }

            AddOrReplaceProgressMessage(AppGlobal.Language.GetText(this, "Importing", "Importing..."));
            String importSuccessfulMessageText = AppGlobal.Language.GetText(this, "ImportSuccessful", "UCAS Data Successfully Imported");
            String importCancelledMessageText = AppGlobal.Language.GetText(this, "ImportCancelled", "UCAS Data Import Cancelled");

            String importingTownsMessage = AppGlobal.Language.GetText(this, "ImportingTowns", "Importing Towns...");
            String importingCurrenciesMessage = AppGlobal.Language.GetText(this, "ImportingCurrencies", "Importing Currencies...");
            String importingProvidersMessage = AppGlobal.Language.GetText(this, "ImportingProviders", "Importing Providers...");
            String importingVenuesMessage = AppGlobal.Language.GetText(this, "ImportingVenues", "Importing Venues...");
            String importingCoursesMessage = AppGlobal.Language.GetText(this, "ImportingCourses", "Importing Courses...");
            String importingDeletionsMessage = AppGlobal.Language.GetText(this, "ImportingDeletions", "Importing Deletions...");
            String importingCourseEntryMessage = AppGlobal.Language.GetText(this, "ImportingCourseEntries", "Importing CourseEntries...");
            String importingCourseIndexesMessage = AppGlobal.Language.GetText(this, "ImportingCourseIndexes", "Importing CourseIndexes...");
            String importingFeesMessage = AppGlobal.Language.GetText(this, "ImportingFees", "Importing FeeYears...");
            String importingFeeYearsMessage = AppGlobal.Language.GetText(this, "ImportingFeeYears", "Importing Fees...");
            String importingStartsMessage = AppGlobal.Language.GetText(this, "ImportingStarts", "Importing Starts...");
            String importingStartsIndexMessage = AppGlobal.Language.GetText(this, "ImportingStartsIndex", "Importing StartsIndex...");
            String importingQualificationsMessage = AppGlobal.Language.GetText(this, "ImportingQualifications", "Importing Qualifications...");
            String handlingDeletionsMessage = AppGlobal.Language.GetText(this, "HandlingDeletions", "Deleting Deleted Entities...");

            String importingPGCoursesMessage = AppGlobal.Language.GetText(this, "ImportingPGCourses", "Importing Courses and Providers...");
            String importingPGCourseOptionsMessage = AppGlobal.Language.GetText(this, "ImportingPGCourseOptions", "Importing Course Options and Locations...");
            String importingPGCourseOptionFeesMessage = AppGlobal.Language.GetText(this, "ImportingPGCourseOptionFees", "Importing Course Options and Locations...");
            Boolean cancellingImport = false;
            String userId = Permission.GetCurrentUserId();
            new Thread(() =>
            {
                try
                {
                    ProviderPortalEntities _db = new ProviderPortalEntities();
                    UCASUpdateType updateType = Directory.GetFiles(UCASImportFolder, "Full_*.csv").Any() ? UCASUpdateType.Full : Directory.GetFiles(UCASImportFolder, "Incremental_*.csv").Any() ? UCASUpdateType.Incremental : UCASUpdateType.PG;

                    // Create the database connection
                    using (SqlConnection conn = new SqlConnection(AddOrReplaceConnectionTimeout(_db.Database.Connection.ConnectionString)))
                    {
                        // Open the connection
                        conn.Open();

                        using (SqlTransaction transaction = conn.BeginTransaction())
                        {
                            // Log Current Record Counts

                            #region Get Current Record Counts

                            // UG Data
                            MetadataUpload mduCourseEntry = new MetadataUpload();
                            MetadataUpload mduCourses = new MetadataUpload();
                            MetadataUpload mduCoursesIndex = new MetadataUpload();
                            MetadataUpload mduCurrencies = new MetadataUpload();
                            MetadataUpload mduFees = new MetadataUpload();
                            MetadataUpload mduFeeYears = new MetadataUpload();
                            MetadataUpload mduOrgs = new MetadataUpload();
                            MetadataUpload mduPlacesOfStudy = new MetadataUpload();
                            MetadataUpload mduStarts = new MetadataUpload();
                            MetadataUpload mduStartsIndex = new MetadataUpload();
                            MetadataUpload mduTowns = new MetadataUpload();
                            MetadataUpload mduQualifications = new MetadataUpload();

                            // PG Data
                            MetadataUpload mduPGCourses = new MetadataUpload();
                            MetadataUpload mduPGProviders = new MetadataUpload();
                            MetadataUpload mduPGLocations = new MetadataUpload();
                            MetadataUpload mduPGCourseOptions = new MetadataUpload();
                            MetadataUpload mduPGCourseOptionFees = new MetadataUpload();


                            switch (updateType)
                            {
                                case UCASUpdateType.Full:
                                case UCASUpdateType.Incremental:

                                    mduCourseEntry = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCASCourseEntry,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName, // updateType + "_CourseEntry.csv",
                                        FileSizeInBytes = fileSize, // (int)new FileInfo(UCASImportFolder + @"\" + updateType + "_CourseEntry.csv").Length,
                                        RowsBefore = _db.UCAS_CourseEntries.Count()
                                    };
                                    _db.MetadataUploads.Add(mduCourseEntry);

                                    mduCourses = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCASCourses,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName, // updateType + "_Courses.csv",
                                        FileSizeInBytes = fileSize, // (int)new FileInfo(UCASImportFolder + @"\" + updateType + "_Courses.csv").Length,
                                        RowsBefore = _db.UCAS_Courses.Count()
                                    };
                                    _db.MetadataUploads.Add(mduCourses);

                                    mduCoursesIndex = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCASCoursesIndex,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName, // updateType + "_CoursesIndex.csv",
                                        FileSizeInBytes = fileSize, // (int)new FileInfo(UCASImportFolder + @"\" + updateType + "_CoursesIndex.csv").Length,
                                        RowsBefore = _db.UCAS_CoursesIndexes.Count()
                                    };
                                    _db.MetadataUploads.Add(mduCoursesIndex);

                                    mduCurrencies = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCASCurrencies,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName, // updateType + "_Currencies.csv",
                                        FileSizeInBytes = fileSize, // (int)new FileInfo(UCASImportFolder + @"\" + updateType + "_Currencies.csv").Length,
                                        RowsBefore = _db.UCAS_Currencies.Count()
                                    };
                                    _db.MetadataUploads.Add(mduCurrencies);

                                    mduFees = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCASFees,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName, // updateType + "_Fees.csv",
                                        FileSizeInBytes = fileSize, // (int)new FileInfo(UCASImportFolder + @"\" + updateType + "_Fees.csv").Length,
                                        RowsBefore = _db.UCAS_Fees.Count()
                                    };
                                    _db.MetadataUploads.Add(mduFees);

                                    mduFeeYears = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCASFeeYears,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName, // updateType + "_FeeYear.csv",
                                        FileSizeInBytes = fileSize, // (int)new FileInfo(UCASImportFolder + @"\" + updateType + "_FeeYear.csv").Length,
                                        RowsBefore = _db.UCAS_FeeYears.Count()
                                    };
                                    _db.MetadataUploads.Add(mduFeeYears);

                                    mduOrgs = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCASOrgs,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName, // updateType + "_Orgs.csv",
                                        FileSizeInBytes = fileSize, // (int)new FileInfo(UCASImportFolder + @"\" + updateType + "_Orgs.csv").Length,
                                        RowsBefore = _db.UCAS_Orgs.Count()
                                    };
                                    _db.MetadataUploads.Add(mduOrgs);

                                    mduPlacesOfStudy = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCASPlacesOfStudy,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName, // updateType + "_PlacesOfStudy.csv",
                                        FileSizeInBytes = fileSize, // (int)new FileInfo(UCASImportFolder + @"\" + updateType + "_PlacesOfStudy.csv").Length,
                                        RowsBefore = _db.UCAS_PlacesOfStudy.Count()
                                    };
                                    _db.MetadataUploads.Add(mduPlacesOfStudy);

                                    mduStarts = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCASStarts,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName, // updateType + "_Starts.csv",
                                        FileSizeInBytes = fileSize, // (int)new FileInfo(UCASImportFolder + @"\" + updateType + "_Starts.csv").Length,
                                        RowsBefore = _db.UCAS_Starts.Count()
                                    };
                                    _db.MetadataUploads.Add(mduStarts);

                                    mduStartsIndex = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCASStartsIndex,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName, // updateType + "_StartsIndex.csv",
                                        FileSizeInBytes = fileSize, // (int)new FileInfo(UCASImportFolder + @"\" + updateType + "_StartsIndex.csv").Length,
                                        RowsBefore = _db.UCAS_StartsIndexes.Count()
                                    };
                                    _db.MetadataUploads.Add(mduStartsIndex);

                                    mduTowns = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCASTowns,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName, // updateType + "_Towns.csv",
                                        FileSizeInBytes = fileSize, // (int)new FileInfo(UCASImportFolder + @"\" + updateType + "_Towns.csv").Length,
                                        RowsBefore = _db.UCAS_Towns.Count()
                                    };
                                    _db.MetadataUploads.Add(mduTowns);

                                    mduQualifications = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCASQualifications,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName, // updateType + "_Towns.csv",
                                        FileSizeInBytes = fileSize, // (int)new FileInfo(UCASImportFolder + @"\" + updateType + "_Towns.csv").Length,
                                        RowsBefore = _db.UCAS_Qualifications.Count()
                                    };
                                    _db.MetadataUploads.Add(mduQualifications);

                                    break;

                                case UCASUpdateType.PG:

                                    mduPGCourses = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCAS_PG_Courses,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName,
                                        FileSizeInBytes = fileSize,
                                        RowsBefore = _db.UCAS_PG_Courses.Count()
                                    };
                                    _db.MetadataUploads.Add(mduPGCourses);

                                    mduPGProviders = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCAS_PG_Providers,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName,
                                        FileSizeInBytes = fileSize,
                                        RowsBefore = _db.UCAS_PG_Providers.Count()
                                    };
                                    _db.MetadataUploads.Add(mduPGProviders);

                                    mduPGLocations = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCAS_PG_Locations,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName,
                                        FileSizeInBytes = fileSize,
                                        RowsBefore = _db.UCAS_PG_Locations.Count()
                                    };
                                    _db.MetadataUploads.Add(mduPGLocations);

                                    mduPGCourseOptions = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCAS_PG_CourseOptions,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName,
                                        FileSizeInBytes = fileSize,
                                        RowsBefore = _db.UCAS_PG_CourseOptions.Count()
                                    };
                                    _db.MetadataUploads.Add(mduPGCourseOptions);

                                    mduPGCourseOptionFees = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.UCAS_PG_CourseOptionFees,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = fileName,
                                        FileSizeInBytes = fileSize,
                                        RowsBefore = _db.UCAS_PG_CourseOptionFees.Count()
                                    };
                                    _db.MetadataUploads.Add(mduPGCourseOptionFees);

                                    break;
                            }

                            #endregion

                            // Prepare Database
                            // Full updates are handled by the SqlConnection transaction
                            SqlCommand comm;
                            switch (updateType)
                            {
                                case UCASUpdateType.Full:

                                    // Full updates are handled by the SqlConnection transaction
                                    comm = new SqlCommand
                                    {
                                        Connection = conn,
                                        CommandType = CommandType.StoredProcedure,
                                        CommandText = "[UCAS].[up_UCAS_PrepareForImport]",
                                        CommandTimeout = 360,
                                        Transaction = transaction
                                    };
                                    comm.Parameters.Add("@Incremental", SqlDbType.Bit).Value = 0;
                                    comm.ExecuteNonQuery();

                                    break;

                                case UCASUpdateType.Incremental:

                                    // Incremental updates are handled by the transaction within the _db object
                                    _db.up_UCAS_PrepareForImport(updateType == UCASUpdateType.Incremental);

                                    break;

                                case UCASUpdateType.PG:
                                    comm = new SqlCommand
                                    {
                                        Connection = conn,
                                        CommandType = CommandType.StoredProcedure,
                                        CommandText = "[UCAS_PG].[up_UCAS_PrepareForImport]",
                                        CommandTimeout = 360,
                                        Transaction = transaction
                                    };
                                    comm.ExecuteNonQuery();

                                    break;
                            }

                            switch (updateType)
                            {
                                case UCASUpdateType.Full:
                                case UCASUpdateType.Incremental:

                                    #region Import Deletions

                                    if (updateType == UCASUpdateType.Incremental)
                                    {
                                        AddOrReplaceProgressMessage(importingDeletionsMessage);
                                        DataTable dtDeletions = new DataTable();
                                        dtDeletions.Columns.Add(new DataColumn {ColumnName = "TableName", AllowDBNull = false, DataType = typeof (String), MaxLength = 25});
                                        dtDeletions.Columns.Add(new DataColumn {ColumnName = "RecordId", AllowDBNull = false, DataType = typeof (Int64)});

                                        // The name of the tables that we are importing (must be upper case)
                                        String[] TablesToImport = {"COURSEENTRY", "COURSES", "COURSESINDEX", "CURRENCIES", "FEES", "FEEYEAR", "ORGS", "PLACESOFSTUDY", "STARTS", "STARTSINDEX", "TOWNS"};

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + GetFilePrefix(updateType) + "_Deletions.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (TablesToImport.Contains(csv["TableName"].ToUpper()))
                                                {
                                                    UCAS_Deletion del = new UCAS_Deletion
                                                    {
                                                        TableName = csv["TableName"],
                                                        RecordId = Convert.ToInt32(csv["RecordId"])
                                                    };
                                                    _db.UCAS_Deletions.Add(del);
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    #endregion

                                    #region Import Towns

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingTownsMessage);
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add(new DataColumn {ColumnName = "TownId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Town", AllowDBNull = false, DataType = typeof (String), MaxLength = 50});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + GetFilePrefix(updateType) + "_Towns.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (updateType == UCASUpdateType.Full)
                                                {
                                                    DataRow dr = dt.NewRow();
                                                    dr["TownId"] = Convert.ToInt32(csv["fldTownId"]);
                                                    dr["Town"] = csv["fldTown"];
                                                    dt.Rows.Add(dr);
                                                }
                                                else if (csv["RecordStatus"] != "")
                                                {
                                                    UCAS_Town town = _db.UCAS_Towns.Find(Convert.ToInt32(csv["fldTownId"]));
                                                    if (town == null)
                                                    {
                                                        town = new UCAS_Town
                                                        {
                                                            TownId = Convert.ToInt32(csv["fldTownId"])
                                                        };
                                                        _db.Entry(town).State = EntityState.Added;
                                                    }
                                                    else
                                                    {
                                                        _db.Entry(town).State = EntityState.Modified;
                                                    }

                                                    town.Town = csv["fldTown"];
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduTowns.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport && updateType == UCASUpdateType.Full)
                                        {
                                            BulkImportData(conn, dt, "Towns", transaction);
                                        }
                                    }

                                    #endregion

                                    #region Import Currencies

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingCurrenciesMessage);
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add(new DataColumn {ColumnName = "CurrencyId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Currency", AllowDBNull = false, DataType = typeof (String), MaxLength = 10});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + GetFilePrefix(updateType) + "_Currencies.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (updateType == UCASUpdateType.Full)
                                                {
                                                    DataRow dr = dt.NewRow();
                                                    dr["CurrencyId"] = Convert.ToInt32(csv["fldCurrencyId"]);
                                                    dr["Currency"] = csv["fldCurrency"];
                                                    dt.Rows.Add(dr);
                                                }
                                                else if (csv["RecordStatus"] != "")
                                                {
                                                    UCAS_Currency currency = _db.UCAS_Currencies.Find(Convert.ToInt32(csv["fldCurrencyId"]));
                                                    if (currency == null)
                                                    {
                                                        currency = new UCAS_Currency
                                                        {
                                                            CurrencyId = Convert.ToInt32(csv["fldCurrencyId"])
                                                        };
                                                        _db.Entry(currency).State = EntityState.Added;
                                                    }
                                                    else
                                                    {
                                                        _db.Entry(currency).State = EntityState.Modified;
                                                    }

                                                    currency.Currency = csv["fldCurrency"];
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduCurrencies.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport && updateType == UCASUpdateType.Full)
                                        {
                                            BulkImportData(conn, dt, "Currencies", transaction);
                                        }
                                    }

                                    #endregion

                                    #region Import Qualifications

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingQualificationsMessage);
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add(new DataColumn {ColumnName = "QualificationId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Qualification", AllowDBNull = false, DataType = typeof (String), MaxLength = 100});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + GetFilePrefix(updateType) + "_Qualifications.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (updateType == UCASUpdateType.Full)
                                                {
                                                    DataRow dr = dt.NewRow();
                                                    dr["QualificationId"] = Convert.ToInt32(csv["fldQualificationID"]);
                                                    dr["Qualification"] = csv["fldQualification"];
                                                    dt.Rows.Add(dr);
                                                }
                                                else if (csv["RecordStatus"] != "")
                                                {
                                                    UCAS_Qualification qualification = _db.UCAS_Qualifications.Find(Convert.ToInt32(csv["fldQualificationID"]));
                                                    if (qualification == null)
                                                    {
                                                        qualification = new UCAS_Qualification
                                                        {
                                                            QualificationId = Convert.ToInt32(csv["fldQualificationID"])
                                                        };
                                                        _db.Entry(qualification).State = EntityState.Added;
                                                    }
                                                    else
                                                    {
                                                        _db.Entry(qualification).State = EntityState.Modified;
                                                    }

                                                    qualification.Qualification = csv["fldQualification"];
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduQualifications.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport && updateType == UCASUpdateType.Full)
                                        {
                                            BulkImportData(conn, dt, "Qualifications", transaction);
                                        }
                                    }

                                    #endregion

                                    #region Import Providers

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingProvidersMessage);
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add(new DataColumn {ColumnName = "OrgId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "OrgName", AllowDBNull = false, DataType = typeof (String), MaxLength = 150});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Address1", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Address2", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Address3", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dt.Columns.Add(new DataColumn {ColumnName = "TownId", AllowDBNull = true, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Postcode", AllowDBNull = true, DataType = typeof (String), MaxLength = 10});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Phone", AllowDBNull = true, DataType = typeof (String), MaxLength = 50});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Fax", AllowDBNull = true, DataType = typeof (String), MaxLength = 50});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Email", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Web", AllowDBNull = false, DataType = typeof (String), MaxLength = 100});
                                        dt.Columns.Add(new DataColumn {ColumnName = "UKPRN", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CreatedDateTimeUtc", AllowDBNull = false, DataType = typeof (DateTime)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CreatedByUserId", AllowDBNull = false, DataType = typeof (String), MaxLength = 128});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + GetFilePrefix(updateType) + "_Orgs.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (updateType == UCASUpdateType.Full)
                                                {
                                                    DataRow dr = dt.NewRow();
                                                    dr["OrgId"] = Convert.ToInt32(csv["fldOrgId"]);
                                                    dr["OrgName"] = csv["fldOrgName"];
                                                    dr["Address1"] = csv["fldAddress1"];
                                                    dr["Address2"] = csv["fldAddress2"];
                                                    dr["Address3"] = csv["fldAddress3"];
                                                    dr["TownId"] = csv["fldTownId"] != "" ? Convert.ToInt32(csv["fldTownId"]) : 0;
                                                    dr["Postcode"] = csv["fldPostcode"];
                                                    dr["Phone"] = csv["fldPhone"];
                                                    dr["Fax"] = csv["fldFax"];
                                                    dr["Email"] = csv["fldEmail"];
                                                    dr["Web"] = csv["fldWeb"];
                                                    dr["UKPRN"] = csv["fldUKPRN"] != "" ? Convert.ToInt32(csv["fldUKPRN"]) : 0;
                                                    dr["CreatedDateTimeUtc"] = DateTime.UtcNow;
                                                    dr["CreatedByUserId"] = userId;
                                                    dt.Rows.Add(dr);
                                                }
                                                else if (csv["RecordStatus"] != "")
                                                {
                                                    UCAS_Org org = _db.UCAS_Orgs.Find(Convert.ToInt32(csv["fldOrgID"]));
                                                    if (org == null)
                                                    {
                                                        org = new UCAS_Org
                                                        {
                                                            OrgId = Convert.ToInt32(csv["fldOrgId"])
                                                        };
                                                        _db.Entry(org).State = EntityState.Added;
                                                    }
                                                    else
                                                    {
                                                        _db.Entry(org).State = EntityState.Modified;
                                                    }

                                                    org.OrgName = csv["fldOrgName"];
                                                    org.Address1 = csv["fldAddress1"];
                                                    org.Address2 = csv["fldAddress2"];
                                                    org.Address3 = csv["fldAddress3"];
                                                    org.TownId = csv["fldTownId"] != "" ? Convert.ToInt32(csv["fldTownId"]) : 0;
                                                    org.Postcode = csv["fldPostcode"];
                                                    org.Phone = csv["fldPhone"];
                                                    org.Fax = csv["fldFax"];
                                                    org.Email = csv["fldEmail"];
                                                    org.Web = csv["fldWeb"];
                                                    org.UKPRN = csv["fldUKPRN"] != "" ? Convert.ToInt32(csv["fldUKPRN"]) : 0;
                                                    org.CreatedByUserId = userId;
                                                    org.CreatedDateTimeUtc = DateTime.UtcNow;
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduOrgs.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport && updateType == UCASUpdateType.Full)
                                        {
                                            BulkImportData(conn, dt, "Orgs", transaction);
                                        }
                                    }

                                    #endregion

                                    #region Import Venues

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingVenuesMessage);
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add(new DataColumn {ColumnName = "PlaceOfStudyId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "OrgId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "PlaceOfStudy", AllowDBNull = false, DataType = typeof (String), MaxLength = 150});
                                        dt.Columns.Add(new DataColumn {ColumnName = "PlaceOfStudyDescription", AllowDBNull = true, DataType = typeof (String), MaxLength = 2000});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Address1", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Address2", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Address3", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dt.Columns.Add(new DataColumn {ColumnName = "TownId", AllowDBNull = true, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Postcode", AllowDBNull = true, DataType = typeof (String), MaxLength = 10});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CreatedDateTimeUtc", AllowDBNull = false, DataType = typeof (DateTime)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CreatedByUserId", AllowDBNull = false, DataType = typeof (String), MaxLength = 128});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + GetFilePrefix(updateType) + "_PlacesOfStudy.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (updateType == UCASUpdateType.Full)
                                                {
                                                    DataRow dr = dt.NewRow();
                                                    dr["PlaceOfStudyId"] = Convert.ToInt32(csv["fldPlaceOfStudyId"]);
                                                    dr["OrgId"] = Convert.ToInt32(csv["fldOrgId"]);
                                                    dr["PlaceOfStudy"] = csv["fldPlaceOfStudy"];
                                                    dr["PlaceOfStudyDescription"] = csv["fldPlaceOfStudyDescription"];
                                                    dr["Address1"] = csv["fldAddress1"];
                                                    dr["Address2"] = csv["fldAddress2"];
                                                    dr["Address3"] = csv["fldAddress3"];
                                                    dr["TownId"] = csv["fldTownId"] != "" ? Convert.ToInt32(csv["fldTownId"]) : 0;
                                                    dr["Postcode"] = csv["fldPostcode"];
                                                    dr["CreatedDateTimeUtc"] = DateTime.UtcNow;
                                                    dr["CreatedByUserId"] = userId;
                                                    dt.Rows.Add(dr);
                                                }
                                                else if (csv["RecordStatus"] != "")
                                                {
                                                    UCAS_PlaceOfStudy pos = _db.UCAS_PlacesOfStudy.Find(Convert.ToInt32(csv["fldPlaceOfStudyId"]));
                                                    if (pos == null)
                                                    {
                                                        pos = new UCAS_PlaceOfStudy
                                                        {
                                                            PlaceOfStudyId = Convert.ToInt32(csv["fldPlaceOfStudyId"])
                                                        };
                                                        _db.Entry(pos).State = EntityState.Added;
                                                    }
                                                    else
                                                    {
                                                        _db.Entry(pos).State = EntityState.Modified;
                                                    }

                                                    pos.OrgId = Convert.ToInt32(csv["fldPlaceOfStudyId"]);
                                                    pos.PlaceOfStudy = csv["fldPlaceOfStudy"];
                                                    pos.PlaceOfStudyDescription = csv["fldPlaceOfStudyDescription"];
                                                    pos.Address1 = csv["fldAddress1"];
                                                    pos.Address2 = csv["fldAddress2"];
                                                    pos.Address3 = csv["fldAddress3"];
                                                    pos.TownId = csv["fldTownId"] != "" ? Convert.ToInt32(csv["fldTownId"]) : 0;
                                                    pos.Postcode = csv["fldPostcode"];
                                                    pos.CreatedByUserId = userId;
                                                    pos.CreatedDateTimeUtc = DateTime.UtcNow;
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduPlacesOfStudy.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport && updateType == UCASUpdateType.Full)
                                        {
                                            BulkImportData(conn, dt, "PlacesOfStudy", transaction);
                                        }
                                    }

                                    #endregion

                                    #region Import Courses

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingCoursesMessage);
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add(new DataColumn {ColumnName = "CourseId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "OrgId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CourseTitle", AllowDBNull = false, DataType = typeof (String), MaxLength = 150});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Summary", AllowDBNull = true, DataType = typeof (String)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Modules", AllowDBNull = true, DataType = typeof (String)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "AssessmentMethods", AllowDBNull = true, DataType = typeof (String)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "AdditionalEntry", AllowDBNull = true, DataType = typeof (String)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "NoOfPlaces", AllowDBNull = true, DataType = typeof (String), MaxLength = 4});
                                        dt.Columns.Add(new DataColumn {ColumnName = "HESA1", AllowDBNull = true, DataType = typeof (String), MaxLength = 10});
                                        dt.Columns.Add(new DataColumn {ColumnName = "HESA2", AllowDBNull = true, DataType = typeof (String), MaxLength = 10});
                                        dt.Columns.Add(new DataColumn {ColumnName = "HESA3", AllowDBNull = true, DataType = typeof (String), MaxLength = 10});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CreatedDateTimeUtc", AllowDBNull = false, DataType = typeof (DateTime)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CreatedByUserId", AllowDBNull = false, DataType = typeof (String), MaxLength = 128});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + updateType + "_Courses.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (updateType == UCASUpdateType.Full)
                                                {
                                                    DataRow dr = dt.NewRow();
                                                    dr["CourseId"] = Convert.ToInt32(csv["fldCourseId"]);
                                                    dr["OrgId"] = Convert.ToInt32(csv["fldOrgId"]);
                                                    dr["CourseTitle"] = csv["fldCourseTitle"];
                                                    dr["Summary"] = csv["fldSummary"];
                                                    dr["Modules"] = csv["fldModules"];
                                                    dr["AssessmentMethods"] = csv["fldAssessmentMethods"];
                                                    dr["AdditionalEntry"] = csv["fldAdditionalEntry"];
                                                    dr["NoOfPlaces"] = csv["fldNoOfPlaces"];
                                                    dr["HESA1"] = csv["fldHesa1"];
                                                    dr["HESA2"] = csv["fldHesa2"];
                                                    dr["HESA3"] = csv["fldHesa3"];
                                                    dr["CreatedDateTimeUtc"] = DateTime.UtcNow;
                                                    dr["CreatedByUserId"] = userId;
                                                    dt.Rows.Add(dr);
                                                }
                                                else if (csv["RecordStatus"] != "")
                                                {
                                                    UCAS_Course course = _db.UCAS_Courses.Find(Convert.ToInt32(csv["fldCourseId"]));
                                                    if (course == null)
                                                    {
                                                        course = new UCAS_Course
                                                        {
                                                            CourseId = Convert.ToInt32(csv["fldCourseId"])
                                                        };
                                                        _db.Entry(course).State = EntityState.Added;
                                                    }
                                                    else
                                                    {
                                                        _db.Entry(course).State = EntityState.Modified;
                                                    }

                                                    course.OrgId = Convert.ToInt32(csv["fldOrgId"]);
                                                    course.CourseTitle = csv["fldCourseTitle"];
                                                    course.Summary = csv["fldSummary"];
                                                    course.Modules = csv["fldModules"];
                                                    course.AssessmentMethods = csv["fldAssessmentMethods"];
                                                    course.AdditionalEntry = csv["fldAdditionalEntry"];
                                                    course.NoOfPlaces = csv["fldNoOfPlaces"];
                                                    course.HESA1 = csv["fldHesa1"];
                                                    course.HESA2 = csv["fldHesa1"];
                                                    course.HESA3 = csv["fldHesa1"];
                                                    course.CreatedByUserId = userId;
                                                    course.CreatedDateTimeUtc = DateTime.UtcNow;
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduCourses.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport && updateType == UCASUpdateType.Full)
                                        {
                                            BulkImportData(conn, dt, "Courses", transaction);
                                        }
                                    }

                                    #endregion

                                    #region Import CourseEntry

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingCourseEntryMessage);
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add(new DataColumn {ColumnName = "CourseEntryId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CourseId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "EntryId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "MinPoints", AllowDBNull = true, DataType = typeof (String), MaxLength = 20});
                                        dt.Columns.Add(new DataColumn {ColumnName = "MaxPoints", AllowDBNull = true, DataType = typeof (String), MaxLength = 20});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Subjects", AllowDBNull = true, DataType = typeof (String), MaxLength = 500});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + updateType + "_CourseEntry.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (updateType == UCASUpdateType.Full)
                                                {
                                                    DataRow dr = dt.NewRow();
                                                    dr["CourseEntryId"] = Convert.ToInt32(csv["fldCourseEntryId"]);
                                                    dr["CourseId"] = Convert.ToInt32(csv["fldCourseId"]);
                                                    dr["EntryId"] = Convert.ToInt32(csv["fldEntryId"]);
                                                    dr["MinPoints"] = csv["fldMinPoints"];
                                                    dr["MaxPoints"] = csv["fldMaxPoints"];
                                                    dr["Subjects"] = csv["fldSubjects"];
                                                    dt.Rows.Add(dr);
                                                }
                                                else if (csv["RecordStatus"] != "")
                                                {
                                                    UCAS_CourseEntry courseEntry = _db.UCAS_CourseEntries.Find(Convert.ToInt32(csv["fldCourseEntryId"]));
                                                    if (courseEntry == null)
                                                    {
                                                        courseEntry = new UCAS_CourseEntry
                                                        {
                                                            CourseEntryId = Convert.ToInt32(csv["fldCourseEntryId"])
                                                        };
                                                        _db.Entry(courseEntry).State = EntityState.Added;
                                                    }
                                                    else
                                                    {
                                                        _db.Entry(courseEntry).State = EntityState.Modified;
                                                    }

                                                    courseEntry.CourseId = Convert.ToInt32(csv["fldCourseId"]);
                                                    courseEntry.EntryId = Convert.ToInt32(csv["fldEntryId"]);
                                                    courseEntry.MinPoints = csv["fldMinPoints"];
                                                    courseEntry.MaxPoints = csv["fldMaxPoints"];
                                                    courseEntry.Subjects = csv["fldSubjects"];
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduCourseEntry.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport && updateType == UCASUpdateType.Full)
                                        {
                                            BulkImportData(conn, dt, "CourseEntry", transaction);
                                        }
                                    }

                                    #endregion

                                    #region Import CoursesIndex

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingCourseIndexesMessage);
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add(new DataColumn {ColumnName = "CourseIndexId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CourseId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "MinDuration", AllowDBNull = true, DataType = typeof (Double)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "MaxDuration", AllowDBNull = true, DataType = typeof (Double)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "DurationId", AllowDBNull = true, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "QualificationId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "StudyModeId", AllowDBNull = true, DataType = typeof (Int64)});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + updateType + "_CoursesIndex.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (updateType == UCASUpdateType.Full)
                                                {
                                                    DataRow dr = dt.NewRow();
                                                    dr["CourseIndexId"] = Convert.ToInt32(csv["fldCourseIndexId"]);
                                                    dr["CourseId"] = Convert.ToInt32(csv["fldCourseId"]);
                                                    dr["QualificationId"] = Convert.ToInt32(csv["fldQualificationId"]);
                                                    dr["MinDuration"] = csv["fldMin"] != "" ? (Object) Convert.ToDouble(csv["fldMin"]) : DBNull.Value;
                                                    dr["MaxDuration"] = csv["fldMax"] != "" ? (Object) Convert.ToDouble(csv["fldMax"]) : DBNull.Value;
                                                    dr["DurationId"] = csv["fldDurationId"] != "" ? (Object) Convert.ToInt32(csv["fldDurationId"]) : DBNull.Value;
                                                    dr["StudyModeId"] = csv["fldStudyModeId"] != "" ? (Object) Convert.ToInt32(csv["fldStudyModeId"]) : DBNull.Value;
                                                    dt.Rows.Add(dr);
                                                }
                                                else if (csv["RecordStatus"] != "")
                                                {
                                                    UCAS_CoursesIndex courseIndex = _db.UCAS_CoursesIndexes.Find(Convert.ToInt32(csv["fldCourseIndexId"]));
                                                    if (courseIndex == null)
                                                    {
                                                        courseIndex = new UCAS_CoursesIndex
                                                        {
                                                            CourseIndexId = Convert.ToInt32(csv["fldCourseIndexId"])
                                                        };
                                                        _db.Entry(courseIndex).State = EntityState.Added;
                                                    }
                                                    else
                                                    {
                                                        _db.Entry(courseIndex).State = EntityState.Modified;
                                                    }

                                                    courseIndex.CourseId = Convert.ToInt32(csv["fldCourseId"]);
                                                    courseIndex.QualificationId = Convert.ToInt32(csv["fldQualificationId"]);
                                                    courseIndex.MinDuration = csv["fldMin"] != "" ? Convert.ToDouble(csv["fldMin"]) : (Double?) null;
                                                    courseIndex.MaxDuration = csv["fldMax"] != "" ? Convert.ToDouble(csv["fldMax"]) : (Double?) null;
                                                    courseIndex.DurationId = csv["fldDurationId"] != "" ? Convert.ToInt32(csv["fldDurationId"]) : (Int32?) null;
                                                    courseIndex.StudyModeId = csv["fldStudyModeId"] != "" ? Convert.ToInt32(csv["fldStudyModeId"]) : (Int32?) null;
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduCoursesIndex.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport && updateType == UCASUpdateType.Full)
                                        {
                                            BulkImportData(conn, dt, "CoursesIndex", transaction);
                                        }
                                    }

                                    #endregion

                                    #region Import Starts

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingStartsMessage);
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add(new DataColumn {ColumnName = "StartId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CourseId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "StartDate", AllowDBNull = true, DataType = typeof (DateTime)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "StartDescription", AllowDBNull = true, DataType = typeof (String), MaxLength = 150});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + GetFilePrefix(updateType) + "_Starts.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (updateType == UCASUpdateType.Full)
                                                {
                                                    DateTime? startDate = ConvertToDate(csv["fldStartDate"]);
                                                    DataRow dr = dt.NewRow();
                                                    dr["StartId"] = Convert.ToInt32(csv["fldStartId"]);
                                                    dr["CourseId"] = Convert.ToInt32(csv["fldCourseId"]);
                                                    dr["StartDate"] = startDate.HasValue ? (Object) startDate.Value : DBNull.Value;
                                                    dr["StartDescription"] = csv["fldStartDescription"];
                                                    dt.Rows.Add(dr);
                                                }
                                                else if (csv["RecordStatus"] != "")
                                                {
                                                    UCAS_Start starts = _db.UCAS_Starts.Find(Convert.ToInt32(csv["fldStartId"]));
                                                    if (starts == null)
                                                    {
                                                        starts = new UCAS_Start
                                                        {
                                                            StartId = Convert.ToInt32(csv["fldStartId"])
                                                        };
                                                        _db.Entry(starts).State = EntityState.Added;
                                                    }
                                                    else
                                                    {
                                                        _db.Entry(starts).State = EntityState.Modified;
                                                    }

                                                    starts.CourseId = Convert.ToInt32(csv["fldCourseId"]);
                                                    starts.StartDate = ConvertToDate(csv["fldStartDate"]);
                                                    starts.StartDescription = csv["fldStartDescription"];
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduStarts.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport && updateType == UCASUpdateType.Full)
                                        {
                                            BulkImportData(conn, dt, "Starts", transaction);
                                        }
                                    }

                                    #endregion

                                    #region Import StartsIndex

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingStartsIndexMessage);
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add(new DataColumn {ColumnName = "StartIndexId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "StartId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CourseIndexId", AllowDBNull = true, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "PlaceOfStudyId", AllowDBNull = true, DataType = typeof (Int64)});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + GetFilePrefix(updateType) + "_StartsIndex.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (updateType == UCASUpdateType.Full)
                                                {
                                                    DataRow dr = dt.NewRow();
                                                    dr["StartIndexId"] = Convert.ToInt32(csv["fldStartIndexId"]);
                                                    dr["StartId"] = Convert.ToInt32(csv["fldStartId"]);
                                                    dr["CourseIndexId"] = csv["fldCourseIndexId"] != "" ? (Object) Convert.ToInt32(csv["fldCourseIndexId"]) : DBNull.Value;
                                                    dr["PlaceOfStudyId"] = csv["fldPlaceOfStudyId"] != "" ? (Object) Convert.ToInt32(csv["fldPlaceOfStudyId"]) : DBNull.Value;
                                                    dt.Rows.Add(dr);
                                                }
                                                else if (csv["RecordStatus"] != "")
                                                {
                                                    UCAS_StartsIndex startsIndex = _db.UCAS_StartsIndexes.Find(Convert.ToInt32(csv["fldStartIndexId"]));
                                                    if (startsIndex == null)
                                                    {
                                                        startsIndex = new UCAS_StartsIndex
                                                        {
                                                            StartIndexId = Convert.ToInt32(csv["fldStartIndexId"])
                                                        };
                                                        _db.Entry(startsIndex).State = EntityState.Added;
                                                    }
                                                    else
                                                    {
                                                        _db.Entry(startsIndex).State = EntityState.Modified;
                                                    }

                                                    startsIndex.StartId = Convert.ToInt32(csv["fldStartId"]);
                                                    startsIndex.CourseIndexId = csv["fldCourseIndexId"] != "" ? Convert.ToInt32(csv["fldCourseIndexId"]) : (Int32?) null;
                                                    startsIndex.PlaceOfStudyId = csv["fldPlaceOfStudyId"] != "" ? Convert.ToInt32(csv["fldPlaceOfStudyId"]) : (Int32?) null;
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduStartsIndex.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport && updateType == UCASUpdateType.Full)
                                        {
                                            BulkImportData(conn, dt, "StartsIndex", transaction);
                                        }
                                    }

                                    #endregion

                                    #region Import Fees

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingFeesMessage);
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add(new DataColumn {ColumnName = "FeeId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CourseIndexId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "CurrencyId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "StudyPeriodId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "FeeYearId", AllowDBNull = true, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "Fee", AllowDBNull = true, DataType = typeof (Decimal)});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + updateType + "_Fees.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (updateType == UCASUpdateType.Full)
                                                {
                                                    DataRow dr = dt.NewRow();
                                                    dr["FeeId"] = Convert.ToInt32(csv["fldFeeId"]);
                                                    dr["CourseIndexId"] = Convert.ToInt32(csv["fldCourseIndexId"]);
                                                    dr["CurrencyId"] = Convert.ToInt32(csv["fldCurrencyId"]);
                                                    dr["StudyPeriodId"] = Convert.ToInt32(csv["fldStudyPeriodId"]);
                                                    dr["FeeYearId"] = csv["fldFeeYearId"] != "" ? (Object) Convert.ToInt32(csv["fldFeeYearId"]) : DBNull.Value;
                                                    dr["Fee"] = Convert.ToDecimal(csv["fldFee"]);
                                                    dt.Rows.Add(dr);
                                                }
                                                else if (csv["RecordStatus"] != "")
                                                {
                                                    UCAS_Fee fee = _db.UCAS_Fees.Find(Convert.ToInt32(csv["fldFeeId"]));
                                                    if (fee == null)
                                                    {
                                                        fee = new UCAS_Fee
                                                        {
                                                            FeeId = Convert.ToInt32(csv["fldFeeId"])
                                                        };
                                                        _db.Entry(fee).State = EntityState.Added;
                                                    }
                                                    else
                                                    {
                                                        _db.Entry(fee).State = EntityState.Modified;
                                                    }

                                                    fee.CourseIndexId = Convert.ToInt32(csv["fldCourseIndexId"]);
                                                    fee.CurrencyId = Convert.ToInt32(csv["fldCurrencyId"]);
                                                    fee.StudyPeriodId = Convert.ToInt32(csv["fldStudyPeriodId"]);
                                                    fee.FeeYearId = csv["fldFeeYearId"] != "" ? Convert.ToInt32(csv["fldFeeYearId"]) : (Int32?) null;
                                                    fee.Fee = Convert.ToDecimal(csv["fldFee"]);
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduFees.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport && updateType == UCASUpdateType.Full)
                                        {
                                            BulkImportData(conn, dt, "Fees", transaction);
                                        }
                                    }

                                    #endregion

                                    #region Import FeeYears

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingFeeYearsMessage);
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add(new DataColumn {ColumnName = "FeeYearId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dt.Columns.Add(new DataColumn {ColumnName = "FeeYear", AllowDBNull = false, DataType = typeof (String), MaxLength = 50});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + GetFilePrefix(updateType) + "_FeeYear.csv"))
                                        {
                                            while (csv.Read())
                                            {
                                                if (updateType == UCASUpdateType.Full)
                                                {
                                                    DataRow dr = dt.NewRow();
                                                    dr["FeeYearId"] = Convert.ToInt32(csv["fldFeeYearId"]);
                                                    dr["FeeYear"] = csv["fldFeeYear"];
                                                    dt.Rows.Add(dr);
                                                }
                                                else if (csv["RecordStatus"] != "")
                                                {
                                                    UCAS_FeeYear feeYear = _db.UCAS_FeeYears.Find(Convert.ToInt32(csv["fldFeeYearId"]));
                                                    if (feeYear == null)
                                                    {
                                                        feeYear = new UCAS_FeeYear
                                                        {
                                                            FeeYearId = Convert.ToInt32(csv["fldFeeYearId"])
                                                        };
                                                        _db.Entry(feeYear).State = EntityState.Added;
                                                    }
                                                    else
                                                    {
                                                        _db.Entry(feeYear).State = EntityState.Modified;
                                                    }

                                                    feeYear.FeeYear = csv["fldFeeYear"];
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduFeeYears.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport && updateType == UCASUpdateType.Full)
                                        {
                                            BulkImportData(conn, dt, "FeeYears", transaction);
                                        }
                                    }

                                    #endregion

                                    break;

                                case UCASUpdateType.PG:

                                    #region Import Courses and Providers

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingPGCoursesMessage);
                                        DataTable dtProvider = new DataTable();
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "ProviderId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "UniqueId", AllowDBNull = false, DataType = typeof (String), MaxLength = 50});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "ProviderName", AllowDBNull = false, DataType = typeof (String), MaxLength = 150});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "Website", AllowDBNull = true, DataType = typeof (String), MaxLength = 255});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "Address1", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "Address2", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "Address3", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "Address4", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "Postcode", AllowDBNull = true, DataType = typeof (String), MaxLength = 10});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "ContactTitle", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "ContactEmail", AllowDBNull = true, DataType = typeof (String), MaxLength = 255});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "ContactPhone", AllowDBNull = true, DataType = typeof (String), MaxLength = 50});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "ContactFax", AllowDBNull = true, DataType = typeof (String), MaxLength = 50});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "CreatedDateTimeUtc", AllowDBNull = false, DataType = typeof (DateTime)});
                                        dtProvider.Columns.Add(new DataColumn {ColumnName = "CreatedByUserId", AllowDBNull = false, DataType = typeof (String), MaxLength = 128});

                                        DataTable dtCourse = new DataTable();
                                        dtCourse.Columns.Add(new DataColumn {ColumnName = "CourseId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dtCourse.Columns.Add(new DataColumn {ColumnName = "UniqueId", AllowDBNull = false, DataType = typeof (String), MaxLength = 50});
                                        dtCourse.Columns.Add(new DataColumn {ColumnName = "ProviderId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dtCourse.Columns.Add(new DataColumn {ColumnName = "CourseTitle", AllowDBNull = false, DataType = typeof (String), MaxLength = 150});
                                        dtCourse.Columns.Add(new DataColumn {ColumnName = "CourseSummary", AllowDBNull = false, DataType = typeof (String), MaxLength = 2000});
                                        dtCourse.Columns.Add(new DataColumn {ColumnName = "CreatedDateTimeUtc", AllowDBNull = false, DataType = typeof (DateTime)});
                                        dtCourse.Columns.Add(new DataColumn {ColumnName = "CreatedByUserId", AllowDBNull = false, DataType = typeof (String), MaxLength = 128});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + "Courses.csv"))
                                        {
                                            Int32 courseId = Int32.MaxValue;
                                            Int32 providerId = Int32.MaxValue;

                                            while (csv.Read())
                                            {
                                                // Get Provider Data
                                                if (dtProvider.Select("UniqueId = '" + csv["ProviderId"] + "'").GetLength(0) == 0)
                                                {
                                                    DataRow drProvider = dtProvider.NewRow();
                                                    drProvider["ProviderId"] = providerId--;
                                                    drProvider["UniqueId"] = csv["ProviderId"].TruncateAt(50);
                                                    drProvider["ProviderName"] = csv["ProviderName"].TruncateAt(150);
                                                    drProvider["Website"] = csv["ProviderWebsite"].TruncateAt(255);
                                                    drProvider["Address1"] = csv["ProviderAddress1"].TruncateAt(100);
                                                    drProvider["Address2"] = csv["ProviderAddress2"].TruncateAt(100);
                                                    drProvider["Address3"] = csv["ProviderAddress3"].TruncateAt(100);
                                                    drProvider["Address4"] = csv["ProviderAddress4"].TruncateAt(100);
                                                    drProvider["Postcode"] = csv["ProviderPostcode"].TruncateAt(10);
                                                    drProvider["ContactTitle"] = csv["ProviderContactTitle"].TruncateAt(100);
                                                    drProvider["ContactEmail"] = csv["ProviderContactEmail"].TruncateAt(50);
                                                    drProvider["ContactPhone"] = csv["ProviderContactPhone"].TruncateAt(50);
                                                    drProvider["ContactFax"] = csv["ProviderContactFax"].TruncateAt(50);
                                                    drProvider["CreatedDateTimeUtc"] = DateTime.UtcNow;
                                                    drProvider["CreatedByUserId"] = userId;
                                                    dtProvider.Rows.Add(drProvider);
                                                }

                                                DataRow[] drProviders = dtProvider.Select(String.Format("UniqueId = '{0}'", csv["ProviderId"]));
                                                if (drProviders.GetLength(0) == 1)
                                                {
                                                    // Get Course Data
                                                    DataRow drCourse = dtCourse.NewRow();
                                                    drCourse["CourseId"] = courseId--;
                                                    drCourse["UniqueId"] = csv["CourseId"].TruncateAt(50);
                                                    drCourse["ProviderId"] = Convert.ToInt32(drProviders[0]["ProviderId"]);
                                                    drCourse["CourseTitle"] = csv["CourseTitle"].TruncateAt(150);
                                                    drCourse["CourseSummary"] = csv["Summary"].TruncateAt(2000);
                                                    drCourse["CreatedDateTimeUtc"] = DateTime.UtcNow;
                                                    drCourse["CreatedByUserId"] = userId;
                                                    dtCourse.Rows.Add(drCourse);
                                                }
                                                else
                                                {
                                                    // Log an error finding provider
                                                    throw new Exception(String.Format("Unable to find provider '{0}'", csv["ProviderId"]));
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduPGProviders.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;
                                        mduPGCourses.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport)
                                        {
                                            BulkImportData(conn, dtProvider, "Provider", transaction, true);
                                            BulkImportData(conn, dtCourse, "Course", transaction, true);
                                        }
                                    }

                                    #endregion

                                    #region Import Courses Options and Locations

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingPGCourseOptionsMessage);
                                        DataTable dtLocation = new DataTable();
                                        dtLocation.Columns.Add(new DataColumn {ColumnName = "LocationId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dtLocation.Columns.Add(new DataColumn {ColumnName = "ProviderId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dtLocation.Columns.Add(new DataColumn {ColumnName = "LocationName", AllowDBNull = false, DataType = typeof (String), MaxLength = 150});
                                        dtLocation.Columns.Add(new DataColumn {ColumnName = "Address1", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dtLocation.Columns.Add(new DataColumn {ColumnName = "Address2", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dtLocation.Columns.Add(new DataColumn {ColumnName = "Address3", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dtLocation.Columns.Add(new DataColumn {ColumnName = "Address4", AllowDBNull = true, DataType = typeof (String), MaxLength = 100});
                                        dtLocation.Columns.Add(new DataColumn {ColumnName = "Postcode", AllowDBNull = true, DataType = typeof (String), MaxLength = 10});
                                        dtLocation.Columns.Add(new DataColumn {ColumnName = "CreatedDateTimeUtc", AllowDBNull = false, DataType = typeof (DateTime)});
                                        dtLocation.Columns.Add(new DataColumn {ColumnName = "CreatedByUserId", AllowDBNull = false, DataType = typeof (String), MaxLength = 128});

                                        DataTable dtCourseOption = new DataTable();
                                        dtCourseOption.Columns.Add(new DataColumn {ColumnName = "CourseOptionId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dtCourseOption.Columns.Add(new DataColumn {ColumnName = "UniqueId", AllowDBNull = false, DataType = typeof (String), MaxLength = 50});
                                        dtCourseOption.Columns.Add(new DataColumn {ColumnName = "CourseId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dtCourseOption.Columns.Add(new DataColumn {ColumnName = "LocationId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dtCourseOption.Columns.Add(new DataColumn {ColumnName = "Qualification", AllowDBNull = false, DataType = typeof (String), MaxLength = 100});
                                        dtCourseOption.Columns.Add(new DataColumn {ColumnName = "QualificationLevel", AllowDBNull = false, DataType = typeof (String), MaxLength = 4000});
                                        dtCourseOption.Columns.Add(new DataColumn {ColumnName = "EntryRequirements", AllowDBNull = false, DataType = typeof (String), MaxLength = 4000});
                                        dtCourseOption.Columns.Add(new DataColumn {ColumnName = "AssessmentMethods", AllowDBNull = false, DataType = typeof (String), MaxLength = 4000});
                                        dtCourseOption.Columns.Add(new DataColumn {ColumnName = "StudyMode", AllowDBNull = false, DataType = typeof (String), MaxLength = 50});
                                        dtCourseOption.Columns.Add(new DataColumn {ColumnName = "StartDate", AllowDBNull = true, DataType = typeof (DateTime)});
                                        dtCourseOption.Columns.Add(new DataColumn {ColumnName = "DurationValue", AllowDBNull = false, DataType = typeof (Int32)});
                                        dtCourseOption.Columns.Add(new DataColumn {ColumnName = "DurationType", AllowDBNull = false, DataType = typeof (String), MaxLength = 25});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + "CoursesOptions.csv"))
                                        {
                                            Int32 courseOptionId = Int32.MaxValue;
                                            Int32 locationId = Int32.MaxValue;

                                            while (csv.Read())
                                            {
                                                // Get Provider for this course
                                                String uniqueId = csv["CourseId"];

                                                // Have to use SqlConnetion here rather than EF because of the transaction
                                                DataTable dtCourse = GetSqlData(conn, transaction, "SELECT C.CourseId, C.ProviderId, P.ProviderName, P.Address1, P.Address2, P.Address3, P.Address4, P.Postcode FROM [UCAS_PG].[Course] C INNER JOIN [UCAS_PG].[Provider] P ON P.ProviderId = C.ProviderId WHERE C.UniqueId = '" + uniqueId + "';");
                                                if (dtCourse.Rows.Count == 1)
                                                {
                                                    Int32 providerId = Convert.ToInt32(dtCourse.Rows[0]["ProviderId"]);
                                                    Int32 courseId = Convert.ToInt32(dtCourse.Rows[0]["CourseId"]);
                                                    String locationName = (String.IsNullOrWhiteSpace(csv["LocationName"]) ? dtCourse.Rows[0]["ProviderName"].ToString() : csv["LocationName"]);

                                                    // Get Location Data
                                                    if (dtLocation.Select("ProviderId = " + providerId + " AND LocationName = '" + locationName.Replace("'", "''") + "'").GetLength(0) == 0)
                                                    {
                                                        DataRow drLocation = dtLocation.NewRow();
                                                        drLocation["LocationId"] = locationId--;
                                                        drLocation["ProviderId"] = providerId;
                                                        drLocation["LocationName"] = locationName;
                                                        drLocation["Address1"] = String.IsNullOrWhiteSpace(csv["LocationAddress1"]) ? dtCourse.Rows[0]["Address1"] : csv["LocationAddress1"].TruncateAt(100);
                                                        drLocation["Address2"] = String.IsNullOrWhiteSpace(csv["LocationAddress2"]) ? dtCourse.Rows[0]["Address2"] : csv["LocationAddress2"].TruncateAt(100);
                                                        drLocation["Address3"] = String.IsNullOrWhiteSpace(csv["LocationAddress3"]) ? dtCourse.Rows[0]["Address3"] : csv["LocationAddress3"].TruncateAt(100);
                                                        drLocation["Address4"] = String.IsNullOrWhiteSpace(csv["LocationAddress4"]) ? dtCourse.Rows[0]["Address4"] : csv["LocationAddress4"].TruncateAt(100);
                                                        drLocation["Postcode"] = String.IsNullOrWhiteSpace(csv["LocationPostcode"]) ? dtCourse.Rows[0]["Postcode"] : csv["LocationPostcode"].TruncateAt(10);
                                                        drLocation["CreatedDateTimeUtc"] = DateTime.UtcNow;
                                                        drLocation["CreatedByUserId"] = userId;
                                                        dtLocation.Rows.Add(drLocation);
                                                    }

                                                    DataRow[] drLocations = dtLocation.Select("ProviderId = " + providerId + " AND LocationName = '" + locationName.Replace("'", "''") + "'");
                                                    if (drLocations.GetLength(0) == 1)
                                                    {
                                                        // Get Course Option Data
                                                        DataRow drCourseOption = dtCourseOption.NewRow();
                                                        drCourseOption["CourseOptionId"] = courseOptionId--;
                                                        drCourseOption["UniqueId"] = csv["CourseOptionId"].TruncateAt(50);
                                                        drCourseOption["CourseId"] = courseId;
                                                        drCourseOption["LocationId"] = Convert.ToInt32(drLocations[0]["LocationId"]);
                                                        drCourseOption["Qualification"] = csv["OutcomeQualification"].TruncateAt(100);
                                                        drCourseOption["QualificationLevel"] = csv["QualificationLevel"].TruncateAt(100);
                                                        drCourseOption["EntryRequirements"] = csv["EntryRequirements"].TruncateAt(4000);
                                                        drCourseOption["AssessmentMethods"] = csv["AssessmentMethods"].TruncateAt(4000);
                                                        drCourseOption["StudyMode"] = csv["StudyMode"].TruncateAt(50);
                                                        drCourseOption["StartDate"] = ConvertPGToDate(csv["StartDate"]);
                                                        Double duration = Convert.ToDouble(csv["DurationValue"]);
                                                        String durationType = csv["DurationType"].TruncateAt(25);
                                                        if (duration != Convert.ToDouble(Convert.ToInt32(duration)))
                                                        {
                                                            switch (durationType)
                                                            {
                                                                case "Years":
                                                                    drCourseOption["DurationValue"] = Convert.ToInt32(duration * 12);
                                                                    drCourseOption["DurationType"] = "Months";
                                                                    break;

                                                                case "Months":
                                                                    drCourseOption["DurationValue"] = Convert.ToInt32(duration * 4);
                                                                    drCourseOption["DurationType"] = "Weeks";
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            drCourseOption["DurationValue"] = Convert.ToInt32(csv["DurationValue"]);
                                                            drCourseOption["DurationType"] = csv["DurationType"].TruncateAt(25);
                                                        }
                                                        dtCourseOption.Rows.Add(drCourseOption);
                                                    }
                                                    else
                                                    {
                                                        // Log an error finding location (venue)
                                                        throw new Exception(String.Format("Unable to find location '{0}' for provider {1}", locationName, providerId));
                                                    }
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduPGLocations.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;
                                        mduPGCourseOptions.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport)
                                        {
                                            BulkImportData(conn, dtLocation, "Location", transaction, true);
                                            BulkImportData(conn, dtCourseOption, "CourseOption", transaction, true);
                                        }
                                    }

                                    #endregion

                                    #region Import Courses Option Fees

                                    if (!IsCancellingImport())
                                    {
                                        AddOrReplaceProgressMessage(importingPGCourseOptionFeesMessage);
                                        DataTable dtFees = new DataTable();
                                        dtFees.Columns.Add(new DataColumn {ColumnName = "CourseOptionFeeId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dtFees.Columns.Add(new DataColumn {ColumnName = "CourseOptionId", AllowDBNull = false, DataType = typeof (Int64)});
                                        dtFees.Columns.Add(new DataColumn {ColumnName = "CourseFees", AllowDBNull = false, DataType = typeof (Double)});
                                        dtFees.Columns.Add(new DataColumn {ColumnName = "Currency", AllowDBNull = false, DataType = typeof (String), MaxLength = 10});
                                        dtFees.Columns.Add(new DataColumn {ColumnName = "FeeDurationPeriod", AllowDBNull = false, DataType = typeof (String), MaxLength = 50});
                                        dtFees.Columns.Add(new DataColumn {ColumnName = "Locale", AllowDBNull = false, DataType = typeof (String), MaxLength = 50});

                                        Stopwatch sw = new Stopwatch();
                                        sw.Start();

                                        using (UCASCsvReader csv = UCASCsvReader.OpenCsvFile(UCASImportFolder + @"\" + "CoursesOptionFees.csv"))
                                        {
                                            Int32 courseOptionFeeId = Int32.MaxValue;

                                            while (csv.Read())
                                            {
                                                // Get CourseOptionId for this course
                                                String strCourseId = csv["CourseId"];
                                                String strCourseOptionId = csv["CourseOptionId"];

                                                // Have to use SqlConnetion here rather than EF because of the transaction
                                                DataTable dtCourseOption = GetSqlData(conn, transaction, "SELECT CO.CourseOptionId FROM [UCAS_PG].[Course] C INNER JOIN [UCAS_PG].[CourseOption] CO ON CO.CourseId = C.CourseId WHERE C.UniqueId = '" + strCourseId + "' AND CO.UniqueId = '" + strCourseOptionId + "';");
                                                if (dtCourseOption.Rows.Count == 1)
                                                {
                                                    Int32 courseOptionId = Convert.ToInt32(dtCourseOption.Rows[0]["CourseOptionId"]);

                                                    // Get Course Option Data
                                                    DataRow drCourseOptionFee = dtFees.NewRow();
                                                    drCourseOptionFee["CourseOptionFeeId"] = courseOptionFeeId--;
                                                    drCourseOptionFee["CourseOptionId"] = courseOptionId;
                                                    drCourseOptionFee["CourseFees"] = Convert.ToDouble(String.IsNullOrWhiteSpace(csv["CourseFees"]) ? "0" : csv["CourseFees"]);
                                                    drCourseOptionFee["Currency"] = csv["Currency"].TruncateAt(10);
                                                    drCourseOptionFee["FeeDurationPeriod"] = csv["FeeDurationPeriod"].TruncateAt(50);
                                                    drCourseOptionFee["Locale"] = csv["Locale"].TruncateAt(50);
                                                    dtFees.Rows.Add(drCourseOptionFee);
                                                }
                                                else
                                                {
                                                    throw new Exception(String.Format("Unable to find CourseOption record for CourseId '{0}' and CourseOptionId '{1}'", strCourseId, strCourseOptionId));
                                                }

                                                // Every 100 rows, check whether we are cancelling the import
                                                if (csv.Row%100 == 0 && IsCancellingImport())
                                                {
                                                    cancellingImport = true;
                                                    break;
                                                }
                                            }
                                        }

                                        sw.Stop();
                                        mduPGCourseOptionFees.DurationInMilliseconds = (Int32) sw.ElapsedMilliseconds;

                                        if (!cancellingImport)
                                        {
                                            BulkImportData(conn, dtFees, "CourseOptionFee", transaction, true);
                                        }
                                    }

                                    #endregion

                                    break;
                            }

                            if (!IsCancellingImport())
                            {
                                // Commit the transaction
                                transaction.Commit();

                                #region Update After Row Counts

                                // Add the current row count to MetadataUpload
                                switch (updateType)
                                {
                                    case UCASUpdateType.Full:
                                    case UCASUpdateType.Incremental:

                                        mduCourseEntry.RowsAfter = _db.UCAS_CourseEntries.Count();
                                        mduCourses.RowsAfter = _db.UCAS_Courses.Count();
                                        mduCoursesIndex.RowsAfter = _db.UCAS_CoursesIndexes.Count();
                                        mduCurrencies.RowsAfter = _db.UCAS_Currencies.Count();
                                        mduFees.RowsAfter = _db.UCAS_Fees.Count();
                                        mduFeeYears.RowsAfter = _db.UCAS_FeeYears.Count();
                                        mduOrgs.RowsAfter = _db.UCAS_Orgs.Count();
                                        mduPlacesOfStudy.RowsAfter = _db.UCAS_PlacesOfStudy.Count();
                                        mduStarts.RowsAfter = _db.UCAS_Starts.Count();
                                        mduStartsIndex.RowsAfter = _db.UCAS_StartsIndexes.Count();
                                        mduTowns.RowsAfter = _db.UCAS_Towns.Count();
                                        mduQualifications.RowsAfter = _db.UCAS_Qualifications.Count();

                                        break;

                                    case UCASUpdateType.PG:

                                        mduPGCourses.RowsAfter = _db.UCAS_PG_Courses.Count();
                                        mduPGProviders.RowsAfter = _db.UCAS_PG_Providers.Count();
                                        mduPGLocations.RowsAfter = _db.UCAS_PG_Locations.Count();
                                        mduPGCourseOptions.RowsAfter = _db.UCAS_PG_CourseOptions.Count();
                                        mduPGCourseOptionFees.RowsAfter = _db.UCAS_PG_CourseOptionFees.Count();

                                        break;
                                }

                                #endregion

                                _db.SaveChanges();
                            }
                            else
                            {
                                // Rollback the transaction
                                try
                                {
                                    transaction.Rollback();
                                    _db.Dispose();
                                }
                                catch
                                {
                                }
                            }

                            #region Handle Deletions

                            if (!IsCancellingImport() && updateType == UCASUpdateType.Incremental)
                            {
                                AddOrReplaceProgressMessage(handlingDeletionsMessage);
                                _db.up_UCAS_HandleDeletions();
                            }

                            #endregion
                        }

                        // Close the SQL Server connection
                        conn.Close();
                    }

                    // Delete all the uploaded and expanded files
                    try
                    {
                        foreach (FileInfo file in new DirectoryInfo(UCASImportFolder).GetFiles())
                        {
                            file.Delete();
                        }
                    }
                    catch
                    {
                    }

                    // Write Success or Cancelled message
                    AddOrReplaceProgressMessage(IsCancellingImport() ? importCancelledMessageText : importSuccessfulMessageText, true);
                }
                catch (Exception ex)
                {
                    AddOrReplaceProgressMessage(String.Format(importErrorMessageText, ex.Message), true);

                    // Delete all the uploaded and expanded files
                    try
                    {
                        foreach (FileInfo file in new DirectoryInfo(UCASImportFolder).GetFiles())
                        {
                            file.Delete();
                        }
                    }
                    catch
                    {
                    }
                }
            }).Start();
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanUploadUCASData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult DownloadAndImportLatestUCASData()
        {
            try
            {
                Byte[] bytes = downloadUCASData();

                // Save to disk
                String UCASImportFolder = Constants.ConfigSettings.UCASImportUploadVirtualDirectoryName;
                if (UCASImportFolder.EndsWith(@"\"))
                {
                    UCASImportFolder = UCASImportFolder.Substring(0, UCASImportFolder.Length - 1);
                }

                // Write file to disk (overwrite it if it exists)
                using (FileStream f = System.IO.File.Open(Path.Combine(UCASImportFolder, "UCAS_PG.zip"), FileMode.Create))
                {
                    f.Write(bytes, 0, bytes.Length);
                }

                ImportData();
                return null; // Return null here as the js does a window.reload and we don't want to remove any error messages generated by redirecting twice
            }
            catch (Exception ex)
            {
                AddOrReplaceProgressMessage(String.Format(AppGlobal.Language.GetText(this, "ImportError", "Error Importing UCAS Data : {0}<br /><br />Please check that the UCAS API URL, Client Id and Client Secret are configured correctly."), ex.Message), true);
            }

            return null; // Return null here as the js does a window.reload and we don't want to remove any error messages generated by redirecting twice
        }

        [NonAction]
        private static Byte[] downloadUCASData()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;

            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add("client_id", Constants.ConfigSettings.UCASAPIClientId);
                webClient.Headers.Add("client_secret", Constants.ConfigSettings.UCASAPIClientSecret);
                return webClient.DownloadData(Constants.ConfigSettings.UCASAPIURL);
            }
        }

        [NonAction]
        private void GetLastUploadDetails(UploadUCASDataModel model)
        {
            MetadataUpload dataUpload = db.MetadataUploads.Where(m => m.MetadataUploadTypeId == (Int32)Constants.MetadataUploadType.UCAS_PG_Courses || m.MetadataUploadTypeId == (Int32)Constants.MetadataUploadType.UCASCourses).OrderByDescending(m => m.CreatedDateTimeUtc).FirstOrDefault();
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
                model.LastUploadFileName = dataUpload.FileName;
            }
        }
        
        [NonAction]
        private static DateTime? ConvertToDate(String dateToConvert)
        {
            if (String.IsNullOrWhiteSpace(dateToConvert))
            {
                return null;
            }

            DateTime? dt = null;
            try
            {
                dt = new DateTime(Convert.ToInt32(dateToConvert.Substring(0, 4)), Convert.ToInt32(dateToConvert.Substring(4, 2)), dateToConvert.Length == 8 ? Convert.ToInt32(dateToConvert.Substring(6, 2)) : 1);
            }
            catch { }

            return dt;
        }

        [NonAction]
        private static Object ConvertPGToDate(String dateToConvert)
        {
            if (String.IsNullOrWhiteSpace(dateToConvert))
            {
                return DBNull.Value;
            }

            Int32 yearStart = dateToConvert.Length == 7 ? 3 : 6;
            Int32 monthStart = dateToConvert.Length == 7 ? 0 : 3;
            Int32 dayStart = dateToConvert.Length == 7 ? -1 : 0;

            DateTime? dt = null;
            try
            {
                dt = new DateTime(Convert.ToInt32(dateToConvert.Substring(yearStart, 4)), Convert.ToInt32(dateToConvert.Substring(monthStart, 2)), dayStart == -1 ? 1 : Convert.ToInt32(dateToConvert.Substring(dayStart, 2)));
            }
            catch { }

            return dt == null ? DBNull.Value : (Object)dt;
        }

        [NonAction]
        private static DataTable GetSqlData(SqlConnection conn, SqlTransaction transaction, String query)
        {           
            DataSet ds = new DataSet();
            using (SqlCommand comm = new SqlCommand(query, conn))
            {
                comm.Transaction = transaction;
                comm.CommandType = CommandType.Text;
                //comm.CommandTimeout = 360;
                using (SqlDataAdapter adapter = new SqlDataAdapter(comm))
                {
                    adapter.Fill(ds);
                }
            }

            return ds.Tables[0];
        }

        [NonAction]
        private static String GetFilePrefix(UCASUpdateType updateType)
        {
            return updateType == UCASUpdateType.Full ? "Full" : "Incremental";
        }
        
        [NonAction]
        private static void BulkImportData(SqlConnection conn, DataTable dt, String tableName, SqlTransaction transaction, Boolean postGraduate = false)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                // The table I'm loading the data to
                bulkCopy.DestinationTableName = "[UCAS" + (postGraduate ? "_PG" : "") + "].[" + tableName + "]";

                // How many records to send to the database in one go (all of them)
                bulkCopy.BatchSize = dt.Rows.Count;

                // Set the timeout to 30 minutes - should be plenty
                bulkCopy.BulkCopyTimeout = 1800;

                // Load the data to the database
                bulkCopy.WriteToServer(dt);

                // Close up          
                bulkCopy.Close();
            }
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadUCASData)]
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
        [PermissionAuthorize(Permission.PermissionName.CanUploadUCASData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult ForceCancelImport()
        {
            DeleteProgressMessage();

            String UCASDataFolder = Constants.ConfigSettings.UCASImportUploadVirtualDirectoryName;
            if (UCASDataFolder.EndsWith(@"\"))
            {
                UCASDataFolder = UCASDataFolder.Substring(0, UCASDataFolder.Length - 1);
            }

            // Check if config setting is valid
            if (String.IsNullOrEmpty(UCASDataFolder) || !Directory.Exists(UCASDataFolder))
            {
                ModelState.AddModelError("", AppGlobal.Language.GetText(this, "UCASImportFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringUCASImportFiles is not set or is incorrect"));
                DeleteProgressMessage();
            }

            foreach (FileInfo file in new DirectoryInfo(UCASDataFolder).GetFiles())
            {
                file.Delete();
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

            String UCASImportFolder = Constants.ConfigSettings.UCASImportUploadVirtualDirectoryName;
            if (UCASImportFolder.EndsWith(@"\"))
            {
                UCASImportFolder = UCASImportFolder.Substring(0, UCASImportFolder.Length - 1);
            }

            // Check if config setting is valid
            if (String.IsNullOrEmpty(UCASImportFolder) || !Directory.Exists(UCASImportFolder))
            {
                ModelState.AddModelError("", AppGlobal.Language.GetText(this, "UCASImportFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringUCASImportFiles is not set or is incorrect"));
                DeleteProgressMessage();
            }
            else
            {
                String[] csvFiles = Directory.GetFiles(UCASImportFolder, "*.csv");
                ViewBag.NumberOfFiles = csvFiles.GetLength(0);
                ViewBag.Files = csvFiles;
            }
        }

        [NonAction]
        private Boolean IsCancellingImport()
        {
            // Create a new ProviderPortalEntities object so that it doesn't get messed up with the transaction
            using (ProviderPortalEntities _db = new ProviderPortalEntities())
            {
                // Check if we have stopped the import
                ProgressMessage pm = _db.ProgressMessages.FirstOrDefault(x => x.MessageArea == MessageArea);
                return pm == null || pm.MessageText == cancelImportMessage;
            }
        }

        [NonAction]
        private static void DeleteProgressMessage()
        {
            using (ProviderPortalEntities _db = new ProviderPortalEntities())
            {
                ProgressMessage pm = _db.ProgressMessages.Find(MessageArea);
                if (pm != null)
                {
                    _db.Entry(pm).State = EntityState.Deleted;
                    _db.SaveChanges();
                }
            }
        }

        [NonAction]
        private static void AddOrReplaceProgressMessage(String message, Boolean isComplete = false)
        {
            using (ProviderPortalEntities _db = new ProviderPortalEntities())
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
        }

        [NonAction]
        private static String AddOrReplaceConnectionTimeout(String connectionString)
        {
            // Rollback can take a while and it uses the connection timeout as a timeout value
            // so set it to 30 minutes (unless it's already greater than 30 minutes).
            Boolean hasTimeout = false;
            const Int32 newTimeout = 360; // 30 Minutes

            String retValue = connectionString;

            String[] options = retValue.Split(';');
            for (Int32 i = 0; i < options.GetLength(0); i++)
            {
                if (options[i].ToLower().StartsWith("connection timeout"))
                {
                    String[] values = options[i].Split('=');
                    Int32 currentTimeout;
                    Int32.TryParse(values[1], out currentTimeout);
                    if (currentTimeout < newTimeout)
                    {
                        options[i] = "Connection Timeout=" + newTimeout;
                    }
                    hasTimeout = true;
                    break;
                }
            }

            return !hasTimeout ? String.Concat(retValue, ";Connection Timeout=", newTimeout) : String.Join(";", options);
        }
    }

    sealed class UCASCsvReader : CsvReader
    {
        private static readonly System.Text.Encoding encoding = System.Text.Encoding.UTF7;

        public static UCASCsvReader OpenCsvFile(String filename)
        {
            StreamReader sr = new StreamReader(filename, encoding);
            return new UCASCsvReader(sr);
        }

        public UCASCsvReader(TextReader sr) : base(sr)
        {
            Configuration.Encoding = encoding;
            Configuration.IgnoreBlankLines = true;
            Configuration.IsHeaderCaseSensitive = false;
        }
    }

    internal static class csvStringExtensions
    {
        /// <summary>
        /// Returns the string truncated to the number of characters specified
        /// </summary>
        /// <param name="str">The String to truncate</param>
        /// <param name="numberOfCharacters">The maximum number of characters to return</param>
        /// <returns></returns>
        public static String TruncateAt(this String str, Int32 numberOfCharacters)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return str.Substring(0, Math.Min(str.Length, numberOfCharacters));
        }
    }
}