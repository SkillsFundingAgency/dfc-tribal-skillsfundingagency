using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

using CsvHelper;
using os_latlong;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    [ContextAuthorize(UserContext.UserContextName.Administration)]
    public class CodePointController : BaseController
    {
        const String MessageArea = "CodePoint";
        readonly String cancelImportMessage;

        public CodePointController()
            : base()
        {
            cancelImportMessage = AppGlobal.Language.GetText(this, "CancellingImport", "Cancelling import...");
        }
        //
        // GET: /CodePoint/
        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadCodePointData)]
        public ActionResult Index()
        {
            CodePointUploadModel model = new CodePointUploadModel();
            GetLastUploadDetails(model);
            GetViewData();

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanUploadCodePointData)]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CodePointUploadModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    String[] validFileTypes = { ".zip" };
                    Boolean validFileType = false;

                    String CodePointFolder = Constants.ConfigSettings.CodePointUploadVirtualDirectoryName;
                    if (CodePointFolder.EndsWith(@"\"))
                    {
                        CodePointFolder = CodePointFolder.Substring(0, CodePointFolder.Length - 1);
                    }

                    // Check if config setting is valid
                    if (String.IsNullOrEmpty(CodePointFolder) || !Directory.Exists(CodePointFolder))
                    {
                        ModelState.AddModelError("", AppGlobal.Language.GetText(this, "CodePointFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringCodePointFiles is not set or is incorrect"));
                    }

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
                    else
                    {
                        String ZIPFile = Path.Combine(CodePointFolder, "CodePoint.zip");

                        // Save the zip file
                        model.File.SaveAs(ZIPFile);

                        // Unzip all the CSV files
                        using (ZipArchive za = ZipFile.OpenRead(ZIPFile))
                        {
                            foreach (ZipArchiveEntry entry in za.Entries.Where(entry => entry.Name.ToLower().EndsWith(".csv")).Where(entry => entry.Name.ToLower() != "code-point_open_column_headers.csv"))
                            {
                                entry.ExtractToFile(Path.Combine(CodePointFolder, entry.Name), true);
                            }
                        }

                        // Delete the zip file
                        System.IO.File.Delete(ZIPFile);

                        MetadataUpload metadataUpload = new MetadataUpload
                        {
                            MetadataUploadTypeId = (int)Constants.MetadataUploadType.CodePoint,
                            CreatedByUserId = Permission.GetCurrentUserGuid().ToString(),
                            CreatedDateTimeUtc = DateTime.UtcNow,
                            FileName = model.File.FileName,
                            FileSizeInBytes = model.File.ContentLength,
                            RowsBefore = db.GeoLocations.Count()
                        };
                        var sw = new Stopwatch();
                        sw.Start();

                        // Import the new data
                        String[] csvFiles = Directory.GetFiles(CodePointFolder, "*.csv");
                        if (csvFiles.GetLength(0) == 0)
                        {
                            ModelState.AddModelError("", AppGlobal.Language.GetText(this, "UnableToFindCSVFile", "Unable to find any CSV files to import"));
                            DeleteProgressMessage();
                        }
                        else
                        {
                            AddOrReplaceProgressMessage(AppGlobal.Language.GetText(this, "StartingImport", "Starting Import..."));
                            Boolean cancellingImport = false;
                            String importingMessageText = AppGlobal.Language.GetText(this, "ImportingFileXOfY", "Importing file {0} of {1}...");
                            String mergingMessageText = AppGlobal.Language.GetText(this, "MergeData", "Merging Data...");
                            String removingTempDataMessageText = AppGlobal.Language.GetText(this, "RemovingTemporaryData", "Removing Temporary Data...");
                            String importSuccessfulMessageText = AppGlobal.Language.GetText(this, "ImportSuccessful", "Code Point Data Successfully Imported");
                            String importCancelledMessageText = AppGlobal.Language.GetText(this, "ImportCancelled", "Code Point Data Import Cancelled");
                            String importErrorMessageText = AppGlobal.Language.GetText(this, "ImportError", "Error Importing Code Point Data : {0}");
                            new Thread(() =>
                            {
                                try
                                {
                                    ProviderPortalEntities _db = new ProviderPortalEntities();

                                    // Open the database
                                    using (SqlConnection conn = new SqlConnection(_db.Database.Connection.ConnectionString))
                                    {
                                        conn.Open();

                                        using (SqlTransaction transaction = conn.BeginTransaction())
                                        {
                                            // Truncate the temporary import table just incase there's still data in there.
                                            SqlCommand comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_GeoLocation];", conn, transaction);
                                            comm.ExecuteNonQuery();

                                            // Setup the DataTable
                                            DataTable dt = new DataTable();
                                            dt.Columns.Add(new DataColumn {ColumnName = "Postcode", AllowDBNull = false, DataType = typeof (String), MaxLength = 8});
                                            dt.Columns.Add(new DataColumn {ColumnName = "Lat", AllowDBNull = false, DataType = typeof (Decimal)});
                                            dt.Columns.Add(new DataColumn {ColumnName = "Lng", AllowDBNull = false, DataType = typeof (Decimal)});
                                            dt.Columns.Add(new DataColumn {ColumnName = "Northing", AllowDBNull = false, DataType = typeof (Decimal)});
                                            dt.Columns.Add(new DataColumn {ColumnName = "Easting", AllowDBNull = false, DataType = typeof (Decimal)});

                                            Int32 i = 1;
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
                                                AddOrReplaceProgressMessage(_db, String.Format(importingMessageText, i++, csvFiles.GetLength(0)));

                                                // Import the CSV file
                                                using (CsvReader csv = new CsvReader(new StreamReader(csvFile)))
                                                {
                                                    const Int32 POSTCODE = 0;
                                                    const Int32 EASTING = 2;
                                                    const Int32 NORTHING = 3;

                                                    csv.Configuration.HasHeaderRecord = false;
                                                    while (csv.Read())
                                                    {
                                                        String Postcode = CorrectPostcode(csv[POSTCODE]);
                                                        Double Northing = Convert.ToDouble(csv[NORTHING]);
                                                        Double Easting = Convert.ToDouble(csv[EASTING]);
                                                        LatLon latlon = LatLonConversions.ConvertOSToLatLon(Easting, Northing);
                                                        const Int32 decimalPlaces = 6;

                                                        if (Postcode.IndexOf(" ") == -1)
                                                        {
                                                            Postcode = Postcode.Substring(0, Postcode.Length - 3) + " " + Postcode.Substring(Postcode.Length - 3, 3);
                                                        }

                                                        DataRow dr = dt.NewRow();
                                                        dr["Postcode"] = Postcode;
                                                        dr["Lat"] = Math.Round(latlon.Latitude, decimalPlaces);
                                                        dr["Lng"] = Math.Round(latlon.Longitude, decimalPlaces);
                                                        dr["Northing"] = Northing;
                                                        dr["Easting"] = Easting;
                                                        dt.Rows.Add(dr);

                                                        // Every 100 rows, check whether we are cancelling the import
                                                        if (csv.Row%100 == 0 && IsCancellingImport(new ProviderPortalEntities()))
                                                        {
                                                            cancellingImport = true;
                                                            break;
                                                        }
                                                    }
                                                    csv.Dispose();

                                                    // Delete the file to tidy up space as quickly as possible
                                                    try
                                                    {
                                                        System.IO.File.Delete(csvFile);
                                                    }
                                                    catch { }
                                                }

                                                if (!cancellingImport)
                                                {
                                                    // Copy the data to the Import_GeoLocation Table                                
                                                    BulkImportData(conn, dt, transaction);
                                                }
                                            }

                                            cancellingImport = IsCancellingImport(new ProviderPortalEntities());
                                            if (!cancellingImport)
                                            {
                                                // Merge the data into the GeoLocation Table
                                                AddOrReplaceProgressMessage(_db, mergingMessageText);
                                                comm = new SqlCommand("MERGE [dbo].[GeoLocation] dest USING [dbo].[Import_GeoLocation] source ON dest.Postcode = source.Postcode WHEN MATCHED THEN UPDATE SET dest.Lat = source.Lat, dest.Lng = source.Lng, dest.Northing = source.Northing, dest.Easting = source.Easting WHEN NOT MATCHED THEN INSERT (Postcode, Lat, Lng, Northing, Easting) VALUES (source.Postcode, source.Lat, source.Lng, source.Northing, source.Easting);", conn, transaction)
                                                {
                                                    CommandTimeout = 3600 /* 1 Hour */
                                                };
                                                comm.ExecuteNonQuery();

                                                // Update any Address Rows that don't currently have any Latitude or Longitude
                                                try
                                                {
                                                    comm = new SqlCommand("UPDATE Address SET Address.Latitude = GeoLocation.Lat, Address.Longitude = GeoLocation.lng FROM Address INNER JOIN GeoLocation ON Address.Postcode = GeoLocation.Postcode WHERE Address.Latitude IS NULL AND GeoLocation.Lat IS NOT NULL;", conn, transaction)
                                                    {
                                                        CommandTimeout = 3600 /* 1 Hour */
                                                    };
                                                    comm.ExecuteNonQuery();
                                                }
                                                catch {}
                                            }

                                            // Truncate the temporary import table
                                            if (!cancellingImport)
                                            {
                                                AddOrReplaceProgressMessage(_db, removingTempDataMessageText);
                                                comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_GeoLocation];", conn, transaction);
                                                comm.ExecuteNonQuery();
                                            }

                                            if (!IsCancellingImport(new ProviderPortalEntities()))
                                            {
                                                // Commit the transaction
                                                transaction.Commit();

                                                #region Update After Row Counts
                                                // Add the current row count to MetadataUpload
                                                // Save timings
                                                sw.Stop();
                                                metadataUpload.DurationInMilliseconds = (int)sw.ElapsedMilliseconds;
                                                metadataUpload.RowsAfter = _db.GeoLocations.Count();
                                                _db.MetadataUploads.Add(metadataUpload);
                                                _db.SaveChanges();
                                                #endregion
                                            }
                                            else
                                            {
                                                // Rollback the transaction
                                                try
                                                {
                                                    transaction.Rollback();
                                                    _db.Dispose();
                                                }
                                                catch { }
                                            }

                                            // Close the database
                                            conn.Close();
                                        }
                                    }

                                    // Delete all the uploaded and expanded files
                                    try
                                    {
                                        foreach (FileInfo file in new DirectoryInfo(CodePointFolder).GetFiles())
                                        {
                                            file.Delete();
                                        }
                                    }
                                    catch
                                    {
                                    }

                                    // Write Success or Cancelled message
                                    AddOrReplaceProgressMessage(_db, cancellingImport ? importCancelledMessageText : importSuccessfulMessageText, true);
                                }
                                catch (Exception ex)
                                {
                                    AddOrReplaceProgressMessage(new ProviderPortalEntities(), String.Format(importErrorMessageText, ex.Message), true);

                                    // Delete all the uploaded and expanded files
                                    try
                                    {
                                        foreach (FileInfo file in new DirectoryInfo(CodePointFolder).GetFiles())
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
                    }
                }
                catch (Exception ex)
                {
                    // Create a model error
                    ModelState.AddModelError("", ex.Message);
                }
            }

            // No Errors so redirect to index which will show messages
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            GetLastUploadDetails(model);

            return View(model);
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

            String CodePointDataFolder = Constants.ConfigSettings.CodePointUploadVirtualDirectoryName;
            if (CodePointDataFolder.EndsWith(@"\"))
            {
                CodePointDataFolder = CodePointDataFolder.Substring(0, CodePointDataFolder.Length - 1);
            }

            // Check if config setting is valid
            if (String.IsNullOrEmpty(CodePointDataFolder) || !Directory.Exists(CodePointDataFolder))
            {
                ModelState.AddModelError("", AppGlobal.Language.GetText(this, "CodePointImportFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringCodePointFiles is not set or is incorrect"));
                DeleteProgressMessage();
            }

            foreach (FileInfo file in new DirectoryInfo(CodePointDataFolder).GetFiles())
            {
                file.Delete();
            }

            return RedirectToAction("Index");
        }

        [NonAction]
        private static String CorrectPostcode(String postcode)
        {
            while (postcode.IndexOf("  ") != -1)
            {
                postcode = postcode.Replace("  ", " ");
            }

            if (postcode.IndexOf(" ") == -1 && postcode.Length >= 5)
            {
                postcode = postcode.Substring(0, postcode.Length - 3) + " " + postcode.Substring(postcode.Length - 3);
            }

            return postcode.ToUpper();
        }

        [NonAction]
        private void GetLastUploadDetails(CodePointUploadModel model)
        {
            MetadataUpload dataUpload = db.MetadataUploads.Where(m => m.MetadataUploadTypeId == (Int32)Constants.MetadataUploadType.CodePoint).OrderByDescending(m => m.CreatedDateTimeUtc).FirstOrDefault();
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
        private static void BulkImportData(SqlConnection conn, DataTable dt, SqlTransaction transaction)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                // The table I'm loading the data to
                bulkCopy.DestinationTableName = "[dbo].[Import_GeoLocation]";

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

            String CodePointImportFolder = Constants.ConfigSettings.CodePointUploadVirtualDirectoryName;
            if (CodePointImportFolder.EndsWith(@"\"))
            {
                CodePointImportFolder = CodePointImportFolder.Substring(0, CodePointImportFolder.Length - 1);
            }

            // Check if config setting is valid
            if (String.IsNullOrEmpty(CodePointImportFolder) || !Directory.Exists(CodePointImportFolder))
            {
                ModelState.AddModelError("", AppGlobal.Language.GetText(this, "CodePointImportFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringCodePointFiles is not set or is incorrect"));
                DeleteProgressMessage();
            }
            else
            {
                String[] csvFiles = Directory.GetFiles(CodePointImportFolder, "*.csv");
                ViewBag.NumberOfFiles = csvFiles.GetLength(0);
                ViewBag.Files = csvFiles;
            }
        }
	}
}


namespace os_latlong
{
    public class LatLonConversions
    {
        private static double lat, lng;

        private LatLonConversions()
        {
        }

        private static double Deg2Rad(double x)
        {
            return x*(Math.PI/180);
        }

        private static double Rad2Deg(double x)
        {
            return x*(180/Math.PI);
        }

        private static double SinSquared(double x)
        {
            return Math.Sin(x)*Math.Sin(x);
        }

        private static double TanSquared(double x)
        {
            return Math.Tan(x)*Math.Tan(x);
        }

        private static double Sec(double x)
        {
            return 1.0/Math.Cos(x);
        }

        private static void OSGB36ToWGS84()
        {
            var airy1830 = new RefEll(6377563.396, 6356256.909);
            var a = airy1830.maj;
            var eSquared = airy1830.ecc;
            var phi = Deg2Rad(lat);
            var lambda = Deg2Rad(lng);
            var v = a/(Math.Sqrt(1 - eSquared*SinSquared(phi)));
            const int H = 0; // height
            var x = (v + H)*Math.Cos(phi)*Math.Cos(lambda);
            var y = (v + H)*Math.Cos(phi)*Math.Sin(lambda);
            var z = ((1 - eSquared)*v + H)*Math.Sin(phi);

            const double tx = 446.448;
            const double ty = -124.157;
            const double tz = 542.060;
            const double s = -0.0000204894;
            var rx = Deg2Rad(0.00004172222);
            var ry = Deg2Rad(0.00006861111);
            var rz = Deg2Rad(0.00023391666);

            var xB = tx + (x*(1 + s)) + (-rx*y) + (ry*z);
            var yB = ty + (rz*x) + (y*(1 + s)) + (-rx*z);
            var zB = tz + (-ry*x) + (rx*y) + (z*(1 + s));

            var wgs84 = new RefEll(6378137.000, 6356752.3141);
            a = wgs84.maj;
            eSquared = wgs84.ecc;

            var lambdaB = Rad2Deg(Math.Atan(yB/xB));
            var p = Math.Sqrt((xB*xB) + (yB*yB));
            var phiN = Math.Atan(zB/(p*(1 - eSquared)));
            for (var i = 1; i < 10; i++)
            {
                v = a/(Math.Sqrt(1 - eSquared*SinSquared(phiN)));
                double phiN1 = Math.Atan((zB + (eSquared*v*Math.Sin(phiN)))/p);
                phiN = phiN1;
            }

            var phiB = Rad2Deg(phiN);

            lat = phiB;
            lng = lambdaB;
        }

        public static LatLon ConvertOSToLatLon(double easting, double northing)
        {
            RefEll airy1830 = new RefEll(6377563.396, 6356256.909);
            const double OSGB_F0 = 0.9996012717;
            const double N0 = -100000.0;
            const double E0 = 400000.0;
            double phi0 = Deg2Rad(49.0);
            double lambda0 = Deg2Rad(-2.0);
            double a = airy1830.maj;
            double b = airy1830.min;
            double eSquared = airy1830.ecc;
            double E = easting;
            double N = northing;
            double n = (a - b)/(a + b);
            double M;
            double phiPrime = ((N - N0)/(a*OSGB_F0)) + phi0;
            do
            {
                M =
                    (b*OSGB_F0)
                    *(((1 + n + ((5.0/4.0)*n*n) + ((5.0/4.0)*n*n*n))
                       *(phiPrime - phi0))
                      - (((3*n) + (3*n*n) + ((21.0/8.0)*n*n*n))
                         *Math.Sin(phiPrime - phi0)
                         *Math.Cos(phiPrime + phi0))
                      + ((((15.0/8.0)*n*n) + ((15.0/8.0)*n*n*n))
                         *Math.Sin(2.0*(phiPrime - phi0))
                         *Math.Cos(2.0*(phiPrime + phi0)))
                      - (((35.0/24.0)*n*n*n)
                         *Math.Sin(3.0*(phiPrime - phi0))
                         *Math.Cos(3.0*(phiPrime + phi0))));
                phiPrime += (N - N0 - M)/(a*OSGB_F0);
            } while ((N - N0 - M) >= 0.001);
            var v = a*OSGB_F0*Math.Pow(1.0 - eSquared*SinSquared(phiPrime), -0.5);
            var rho =
                a
                *OSGB_F0
                *(1.0 - eSquared)
                *Math.Pow(1.0 - eSquared*SinSquared(phiPrime), -1.5);
            var etaSquared = (v/rho) - 1.0;
            var VII = Math.Tan(phiPrime)/(2*rho*v);
            var VIII =
                (Math.Tan(phiPrime)/(24.0*rho*Math.Pow(v, 3.0)))
                *(5.0
                  + (3.0*TanSquared(phiPrime))
                  + etaSquared
                  - (9.0*TanSquared(phiPrime)*etaSquared));
            var IX =
                (Math.Tan(phiPrime)/(720.0*rho*Math.Pow(v, 5.0)))
                *(61.0
                  + (90.0*TanSquared(phiPrime))
                  + (45.0*TanSquared(phiPrime)*TanSquared(phiPrime)));
            var X = Sec(phiPrime)/v;
            var XI =
                (Sec(phiPrime)/(6.0*v*v*v))
                *((v/rho) + (2*TanSquared(phiPrime)));
            var XII =
                (Sec(phiPrime)/(120.0*Math.Pow(v, 5.0)))
                *(5.0
                  + (28.0*TanSquared(phiPrime))
                  + (24.0*TanSquared(phiPrime)*TanSquared(phiPrime)));
            var XIIA =
                (Sec(phiPrime)/(5040.0*Math.Pow(v, 7.0)))
                *(61.0
                  + (662.0*TanSquared(phiPrime))
                  + (1320.0*TanSquared(phiPrime)*TanSquared(phiPrime))
                  + (720.0
                     *TanSquared(phiPrime)
                     *TanSquared(phiPrime)
                     *TanSquared(phiPrime)));
            double phi = phiPrime
                         - (VII*Math.Pow(E - E0, 2.0))
                         + (VIII*Math.Pow(E - E0, 4.0))
                         - (IX*Math.Pow(E - E0, 6.0));
            double lambda = lambda0
                            + (X*(E - E0))
                            - (XI*Math.Pow(E - E0, 3.0))
                            + (XII*Math.Pow(E - E0, 5.0))
                            - (XIIA*Math.Pow(E - E0, 7.0));


            lat = Rad2Deg(phi);
            lng = Rad2Deg(lambda);
            // convert to WGS84
            OSGB36ToWGS84();

            return new LatLon(lat, lng);
        }
    }

    public class RefEll
    {
        public double maj, min, ecc;

        public RefEll(double major, double minor)
        {
            maj = major;
            min = minor;
            ecc = ((major*major) - (minor*minor))/(major*major);
        }
    }

    public class LatLon
    {
        public double Latitude;
        public double Longitude;

        public LatLon()
        {
            Latitude = 0;
            Longitude = 0;
        }

        public LatLon(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }
    }
}