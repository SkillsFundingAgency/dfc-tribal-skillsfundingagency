using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    [ContextAuthorize(UserContext.UserContextName.Administration)]
    public class LARSController : BaseController
    {
        private static String mdbFilename = String.Empty;
        private static long mdbFileSize;

        //
        // GET: /LARS/
        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadLARSData)]
        public ActionResult Index()
        {
            LARSUploadModel model = new LARSUploadModel();
            GetLastUploadDetails(model);
            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanUploadLARSData)]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LARSUploadModel model)
        {
            if (ModelState.IsValid)
            {
                String[] validFileTypes = { ".zip" };
                Boolean validFileType = false;
                String LARSFolder = Constants.ConfigSettings.LARSUploadVirtualDirectoryName;
                if (LARSFolder.EndsWith(@"\"))
                {
                    LARSFolder = LARSFolder.Substring(0, LARSFolder.Length - 1);
                }

                // Check if config setting is valid
                if (String.IsNullOrEmpty(LARSFolder) || !Directory.Exists(LARSFolder))
                {
                    ModelState.AddModelError("", AppGlobal.Language.GetText(this, "LARSFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringLARSFiles is not set or is incorrect"));
                }

                if (ModelState.IsValid)
                {
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
                        else
                        {
                            String ZIPFile = String.Format("{0}/LARS.zip", LARSFolder);

                            // Save the zip file
                            model.File.SaveAs(ZIPFile);

                            // Import the file
                            ImportLARSFile(Permission.GetCurrentUserGuid().ToString(), AppGlobal.Language.GetLanguageIdForThisRequest());

                            if (ModelState.IsValid)
                            {
                                ViewBag.Message = AppGlobal.Language.GetText(this, "ImportSuccessful", "LARS Data Successfully Imported");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Create a model error
                        ModelState.AddModelError("", ex.Message);
                    }

                    // Delete the existing files
                    foreach (FileInfo file in new DirectoryInfo(LARSFolder).GetFiles())
                    {
                        file.Delete();
                    }
                }
            }

            GetLastUploadDetails(model);
            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanUploadLARSData)]
        public ActionResult ClearAutomationInProgressFlag()
        {
            AutomationController.CompleteAutomatedTask(AutomationController.AutomatedTaskName.LARSImport);
            return RedirectToAction("Index");
        }

        [NonAction]
        public static void ImportLARSFile(String userId, Int32 languageId)
        {
            ProviderPortalEntities db = new ProviderPortalEntities();

            String LARSFolder = Constants.ConfigSettings.LARSUploadVirtualDirectoryName;
            if (LARSFolder.EndsWith(@"\"))
            {
                LARSFolder = LARSFolder.Substring(0, LARSFolder.Length - 1);
            }

            // Check if config setting is valid
            if (String.IsNullOrEmpty(LARSFolder) || !Directory.Exists(LARSFolder))
            {
                throw new Exception(AppGlobal.Language.GetText("LARS_ImportLARSFile_LARSFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringLARSFiles is not set or is incorrect", false, languageId));
            }

            // Unzip the file(s) - Should only be 1 really
            foreach (String zipFile in Directory.GetFiles(LARSFolder, "*.zip"))
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(zipFile, LARSFolder);
                System.IO.File.Delete(zipFile);
            }

            // Get the MDB Filename
            String[] mdbFiles = Directory.GetFiles(LARSFolder, "*.mdb");
            if (mdbFiles.GetLength(0) == 0)
            {
                throw new Exception(AppGlobal.Language.GetText("LARS_ImportLARSFile_UnableToFindMDBFile", "Unable to find MDB file in ZIP file", false, languageId));
            }

            mdbFilename = mdbFiles[0];
            mdbFileSize = new FileInfo(mdbFilename).Length;

            // Open the database
            using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                // Open the database connection
                conn.Open();

                // Import the data
                ImportClassifications(conn, userId, db);
                ImportAwardingOrganisation(conn, userId, db);
                ImportLearningAims(conn, userId, db);
                ImportValidity(conn, userId, db);
                ImportFrameworksAndStandards(conn, userId, db);

                // Close the database
                conn.Close();
            }

            // Save metadata upload records
            db.SaveChanges();
        }

        [NonAction]
        private void GetLastUploadDetails(LARSUploadModel model)
        {
            String taskName = AutomationController.AutomatedTaskName.LARSImport.ToString();
            AutomatedTask at = db.AutomatedTasks.Where(x => x.TaskName == taskName).FirstOrDefault();
            if (at != null)
            {
                model.IsAutomationInProgress = at.InProgress;
            }

            MetadataUpload dataUpload = db.MetadataUploads.Where(m => m.MetadataUploadTypeId == (Int32)Constants.MetadataUploadType.LearningAim).OrderByDescending(m => m.CreatedDateTimeUtc).FirstOrDefault();
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
                    model.LastUploadedBy = dataUpload.AspNetUser.Name ?? dataUpload.AspNetUser.Email;
                }
                model.LastUploadDateTimeUtc = dataUpload.CreatedDateTimeUtc;
                DateTime.SpecifyKind(model.LastUploadDateTimeUtc.Value, DateTimeKind.Utc);
                model.LastUploadFileName = dataUpload.FileName;
            }            
        }

        [NonAction]
        private static void ImportClassifications(SqlConnection conn, String userId, ProviderPortalEntities db)
        {
            MetadataUpload metadataUpload = new MetadataUpload
            {
                MetadataUploadTypeId = (int)Constants.MetadataUploadType.LearnDirectClassification,
                CreatedByUserId = userId,
                CreatedDateTimeUtc = DateTime.UtcNow,
                FileName = Path.GetFileName(mdbFilename),
                FileSizeInBytes = (int)mdbFileSize,
                RowsBefore = db.LearnDirectClassifications.Count()
            };
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Import Classifications
            SqlCommand comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_LearnDirectClassification];", conn);
            comm.ExecuteNonQuery();
            DataTable dt = OpenDataTable("SELECT LearnDirectClassSystemCode AS LearnDirectClassificationRef, LearnDirectClassSystemCodeDesc FROM CoreReference_LARS_LearnDirectClassSystemCode_Lookup;");
            BulkImportData(conn, dt, "[dbo].[Import_LearnDirectClassification]");
            comm = new SqlCommand("MERGE [dbo].[LearnDirectClassification] dest USING [dbo].[Import_LearnDirectClassification] source ON  dest.LearnDirectClassificationRef = source.LearnDirectClassificationRef WHEN MATCHED THEN UPDATE SET dest.LearnDirectClassSystemCodeDesc = source.LearnDirectClassSystemCodeDesc WHEN NOT MATCHED THEN INSERT (LearnDirectClassificationRef, LearnDirectClassSystemCodeDesc) VALUES (source.LearnDirectClassificationRef, source.LearnDirectClassSystemCodeDesc);", conn);
            comm.ExecuteNonQuery();

            // Update LDCS Hierarchy
            SqlCommand commSP = new SqlCommand("[dbo].[up_UpdateLDCSHierarchy]", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            commSP.ExecuteNonQuery();

            sw.Stop();
            metadataUpload.DurationInMilliseconds = (int)sw.ElapsedMilliseconds;
            metadataUpload.RowsAfter = db.LearnDirectClassifications.Count();
            db.MetadataUploads.Add(metadataUpload);
        }

        [NonAction]
        private static void ImportAwardingOrganisation(SqlConnection conn, String userId, ProviderPortalEntities db)
        {
            MetadataUpload metadataUpload = new MetadataUpload
            {
                MetadataUploadTypeId = (int)Constants.MetadataUploadType.LearningAimAwardOrg,
                CreatedByUserId = userId,
                CreatedDateTimeUtc = DateTime.UtcNow,
                FileName = Path.GetFileName(mdbFilename),
                FileSizeInBytes = (int)mdbFileSize,
                RowsBefore = db.LearningAimAwardOrgs.Count()
            };
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Import Awarding Organisation
            SqlCommand comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_LearningAimAwardOrg];", conn);
            comm.ExecuteNonQuery();
            DataTable dt = OpenDataTable("SELECT AwardOrgCode AS LearningAimAwardOrgCode, AwardOrgName FROM CoreReference_LARS_AwardOrgCode_Lookup;");
            BulkImportData(conn, dt, "[dbo].[Import_LearningAimAwardOrg]");
            comm = new SqlCommand("MERGE [dbo].[LearningAimAwardOrg] dest USING [dbo].[Import_LearningAimAwardOrg] source ON  dest.LearningAimAwardOrgCode = source.LearningAimAwardOrgCode WHEN MATCHED THEN UPDATE SET dest.AwardOrgName = source.AwardOrgName WHEN NOT MATCHED THEN INSERT (LearningAimAwardOrgCode, AwardOrgName) VALUES (source.LearningAimAwardOrgCode, source.AwardOrgName);", conn);
            comm.ExecuteNonQuery();

            sw.Stop();
            metadataUpload.DurationInMilliseconds = (int)sw.ElapsedMilliseconds;
            metadataUpload.RowsAfter = db.LearningAimAwardOrgs.Count();
            db.MetadataUploads.Add(metadataUpload);
        }

        [NonAction]
        private static void ImportLearningAims(SqlConnection conn, String userId, ProviderPortalEntities db)
        {
            MetadataUpload metadataUpload = new MetadataUpload
            {
                MetadataUploadTypeId = (int)Constants.MetadataUploadType.LearningAim,
                CreatedByUserId = userId,
                CreatedDateTimeUtc = DateTime.UtcNow,
                FileName = Path.GetFileName(mdbFilename),
                FileSizeInBytes = (int)mdbFileSize,
                RowsBefore = db.LearningAims.Count()
            };
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Import Learning Aims
            SqlCommand comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_LearningAim];", conn);
            comm.ExecuteNonQuery();
            DataTable dt = OpenDataTable("SELECT LearnAimRef AS LearningAimRefId, LearnAimRefTitle AS LearningAimTitle, AwardOrgCode AS LearningAimAwardOrgCode, 0 AS IndependentLivingSkills, IIF(LearnDirectClassSystemCode1 = 'NUL', NULL, LearnDirectClassSystemCode1) AS LDCS1, IIF(LearnDirectClassSystemCode2 = 'NUL', NULL, LearnDirectClassSystemCode2) AS LDCS2, IIF(LearnDirectClassSystemCode3 = 'NUL', NULL, LearnDirectClassSystemCode3) AS LDCS3,  Switch (NotionalNVQLevelv2 = 'X',  '11', NotionalNVQLevelv2 = 'E', '10', NotionalNVQLevelv2 = '1', '1', NotionalNVQLevelv2 BETWEEN '2' AND '8', NotionalNVQLevelv2, NotionalNVQLevelv2 = 'H', '9') AS QualificationLevelId FROM Core_LARS_LearningDelivery WHERE (EffectiveTo IS NULL OR EffectiveTo > Now);");
            BulkImportData(conn, dt, "[dbo].[Import_LearningAim]");
            comm = new SqlCommand("MERGE [dbo].[LearningAim] dest USING [dbo].[Import_LearningAim] source ON  dest.LearningAimRefId = source.LearningAimRefId WHEN MATCHED THEN UPDATE SET dest.LearningAimTitle = source.LearningAimTitle, dest.LearningAimAwardOrgCode = source.LearningAimAwardOrgCode, dest.IndependentLivingSkills = source.IndependentLivingSkills, dest.LearnDirectClassSystemCode1 = source.LDCS1, dest.LearnDirectClassSystemCode2 = source.LDCS2, dest.LearnDirectClassSystemCode3 = source.LDCS3, dest.QualificationLevelId = CAST(source.QualificationLevelId AS INT), dest.RecordStatusId = 2 WHEN NOT MATCHED THEN INSERT (LearningAimRefId, Qualification, LearningAimTitle, LearningAimAwardOrgCode, IndependentLivingSkills, LearnDirectClassSystemCode1, LearnDirectClassSystemCode2, LearnDirectClassSystemCode3, QualificationLevelId, RecordStatusId) VALUES (source.LearningAimRefId, '', source.LearningAimTitle, source.LearningAimAwardOrgCode, source.IndependentLivingSkills, source.LDCS1, source.LDCS2, source.LDCS3, CAST(source.QualificationLevelId AS INT), 2);", conn);
            comm.ExecuteNonQuery();

            // Set status to delete for items not in import table
            comm = new SqlCommand("UPDATE [dbo].[LearningAim] SET RecordStatusId = 4 WHERE LearningAimRefId NOT IN (SELECT LearningAimRefId FROM [dbo].[Import_LearningAim]);", conn);
            comm.ExecuteNonQuery();

            sw.Stop();
            metadataUpload.DurationInMilliseconds = (int)sw.ElapsedMilliseconds;
            metadataUpload.RowsAfter = db.LearningAims.Count();
            db.MetadataUploads.Add(metadataUpload);
        }

        [NonAction]
        private static void ImportValidity(SqlConnection conn, String userId, ProviderPortalEntities db)
        {
            MetadataUpload metadataUpload = new MetadataUpload
            {
                MetadataUploadTypeId = (int)Constants.MetadataUploadType.LearningAimValidity,
                CreatedByUserId = userId,
                CreatedDateTimeUtc = DateTime.UtcNow,
                FileName = Path.GetFileName(mdbFilename),
                FileSizeInBytes = (int)mdbFileSize,
                RowsBefore = db.LearningAimValidities.Count()
            };
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Import Validity
            SqlCommand comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_LearningAimValidity];", conn);
            comm.ExecuteNonQuery();
            DataTable dt = OpenDataTable("SELECT LearnAimRef AS LearningAimRefId, ValidityCategory, StartDate, EndDate, LastNewStartDate FROM Core_LARS_Validity;");
            BulkImportData(conn, dt, "[dbo].[Import_LearningAimValidity]");
            comm = new SqlCommand("MERGE [dbo].[LearningAimValidity] dest USING [dbo].[Import_LearningAimValidity] source ON dest.LearningAimRefId = source.LearningAimRefId AND dest.ValidityCategory = source.ValidityCategory WHEN MATCHED THEN UPDATE SET dest.StartDate = source.StartDate, dest.EndDate = source.EndDate, dest.LastNewStartDate = source.LastNewStartDate WHEN NOT MATCHED THEN INSERT (LearningAimRefId, ValidityCategory, StartDate, EndDate, LastNewStartDate) VALUES (source.LearningAimRefId, source.ValidityCategory, source.StartDate, source.EndDate, source.LastNewStartDate);", conn);
            comm.ExecuteNonQuery();

            sw.Stop();
            metadataUpload.DurationInMilliseconds = (int)sw.ElapsedMilliseconds;
            metadataUpload.RowsAfter = db.LearningAimValidities.Count();
            db.MetadataUploads.Add(metadataUpload);
        }


        [NonAction]
        private static void ImportFrameworksAndStandards(SqlConnection conn, String userId, ProviderPortalEntities db)
        {
            MetadataUpload mduFrameworks = new MetadataUpload
            {
                MetadataUploadTypeId = (int)Constants.MetadataUploadType.Frameworks,
                CreatedByUserId = userId,
                CreatedDateTimeUtc = DateTime.UtcNow,
                FileName = Path.GetFileName(mdbFilename),
                FileSizeInBytes = (int)mdbFileSize,
                RowsBefore = db.Frameworks.Count()
            };

            MetadataUpload mduProgTypes = new MetadataUpload
            {
                MetadataUploadTypeId = (int)Constants.MetadataUploadType.ProgTypes,
                CreatedByUserId = userId,
                CreatedDateTimeUtc = DateTime.UtcNow,
                FileName = Path.GetFileName(mdbFilename),
                FileSizeInBytes = (int)mdbFileSize,
                RowsBefore = db.ProgTypes.Count()
            };

            MetadataUpload mduSectorSubjectAreaTier1 = new MetadataUpload
            {
                MetadataUploadTypeId = (int)Constants.MetadataUploadType.SectorSubjectAreaTier1,
                CreatedByUserId = userId,
                CreatedDateTimeUtc = DateTime.UtcNow,
                FileName = Path.GetFileName(mdbFilename),
                FileSizeInBytes = (int)mdbFileSize,
                RowsBefore = db.SectorSubjectAreaTier1.Count()
            };

            MetadataUpload mduSectorSubjectAreaTier2 = new MetadataUpload
            {
                MetadataUploadTypeId = (int)Constants.MetadataUploadType.SectorSubjectAreaTier2,
                CreatedByUserId = userId,
                CreatedDateTimeUtc = DateTime.UtcNow,
                FileName = Path.GetFileName(mdbFilename),
                FileSizeInBytes = (int)mdbFileSize,
                RowsBefore = db.SectorSubjectAreaTier2.Count()
            };

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Import Classifications
            SqlCommand comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_Framework];", conn);
            comm.ExecuteNonQuery();
            DataTable dt = OpenDataTable("SELECT FWorkCode AS FrameworkCode, ProgType, PwayCode AS PathwayCode, LTrim(RTrim(Left(PathwayName, 2000))) AS PwayName, LTrim(RTrim(Left(NasTitle, 2000))) AS NasTitle, EffectiveFrom, EffectiveTo, SectorSubjectAreaTier1, SectorSubjectAreaTier2 FROM Core_LARS_Framework WHERE ProgType IN (2, 3, 20, 21, 22, 23) AND FworkCode >= 400 AND (EffectiveTo IS NULL OR EffectiveTo >= Now);");
            BulkImportData(conn, dt, "[dbo].[Import_Framework]");

            // Import Sector Subject Area Tier 1s
            comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_SectorSubjectAreaTier1];", conn);
            comm.ExecuteNonQuery();
            dt = OpenDataTable("SELECT SectorSubjectAreaTier1 AS SectorSubjectAreaTier1Id, LTrim(RTrim(SectorSubjectAreaTier1Desc)) AS SectorSubjectAreaTier1Desc, LTrim(RTrim(SectorSubjectAreaTier1Desc2)) AS SectorSubjectAreaTier1Desc2, EffectiveFrom, EffectiveTo FROM CoreReference_LARS_SectorSubjectAreaTier1_Lookup;");
            BulkImportData(conn, dt, "[dbo].[Import_SectorSubjectAreaTier1]");

            // Import Sector Subject Area Tier 2s
            comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_SectorSubjectAreaTier2];", conn);
            comm.ExecuteNonQuery();
            dt = OpenDataTable("SELECT SectorSubjectAreaTier2 AS SectorSubjectAreaTier2Id, LTrim(RTrim(SectorSubjectAreaTier2Desc)) AS SectorSubjectAreaTier2Desc, LTrim(RTrim(SectorSubjectAreaTier2Desc2)) AS SectorSubjectAreaTier2Desc2, EffectiveFrom, EffectiveTo FROM CoreReference_LARS_SectorSubjectAreaTier2_Lookup;");
            BulkImportData(conn, dt, "[dbo].[Import_SectorSubjectAreaTier2]");

            // Import ProgTypes
            comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_ProgType];", conn);
            comm.ExecuteNonQuery();
            dt = OpenDataTable("SELECT ProgType AS ProgTypeId, ProgTypeDesc, ProgTypeDesc2, EffectiveFrom, EffectiveTo FROM CoreReference_LARS_ProgType_Lookup;");
            BulkImportData(conn, dt, "[dbo].[Import_ProgType]");

            // Merge Data
            comm = new SqlCommand("MERGE [dbo].[SectorSubjectAreaTier1] dest USING [dbo].[Import_SectorSubjectAreaTier1] source ON dest.SectorSubjectAreaTier1Id = source.SectorSubjectAreaTier1Id WHEN MATCHED THEN UPDATE SET dest.SectorSubjectAreaTier1Desc = source.SectorSubjectAreaTier1Desc, dest.SectorSubjectAreaTier1Desc2 = source.SectorSubjectAreaTier1Desc2, dest.EffectiveFrom = source.EffectiveFrom, dest.EffectiveTo = source.EffectiveTo, dest.ModifiedDateTimeUtc = GetUtcDate() WHEN NOT MATCHED THEN INSERT (SectorSubjectAreaTier1Id, SectorSubjectAreaTier1Desc, SectorSubjectAreaTier1Desc2, EffectiveFrom, EffectiveTo, CreatedDateTimeUtc) VALUES (source.SectorSubjectAreaTier1Id, source.SectorSubjectAreaTier1Desc, source.SectorSubjectAreaTier1Desc2, source.EffectiveFrom, source.EffectiveTo, GetUtcDate());", conn);
            comm.ExecuteNonQuery();
            comm = new SqlCommand("MERGE [dbo].[SectorSubjectAreaTier2] dest USING [dbo].[Import_SectorSubjectAreaTier2] source ON dest.SectorSubjectAreaTier2Id = source.SectorSubjectAreaTier2Id WHEN MATCHED THEN UPDATE SET dest.SectorSubjectAreaTier2Desc = source.SectorSubjectAreaTier2Desc, dest.SectorSubjectAreaTier2Desc2 = source.SectorSubjectAreaTier2Desc2, dest.EffectiveFrom = source.EffectiveFrom, dest.EffectiveTo = source.EffectiveTo, dest.ModifiedDateTimeUtc = GetUtcDate() WHEN NOT MATCHED THEN INSERT (SectorSubjectAreaTier2Id, SectorSubjectAreaTier2Desc, SectorSubjectAreaTier2Desc2, EffectiveFrom, EffectiveTo, CreatedDateTimeUtc) VALUES (source.SectorSubjectAreaTier2Id, source.SectorSubjectAreaTier2Desc, source.SectorSubjectAreaTier2Desc2, source.EffectiveFrom, source.EffectiveTo, GetUtcDate());", conn);
            comm.ExecuteNonQuery();
            comm = new SqlCommand("MERGE [dbo].[ProgType] dest USING [dbo].[Import_ProgType] source ON dest.ProgTypeId = source.ProgTypeId WHEN MATCHED THEN UPDATE SET dest.ProgTypeDesc = LTrim(RTrim(source.ProgTypeDesc)), dest.ProgTypeDesc2 = LTrim(RTrim(source.ProgTypeDesc2)), dest.EffectiveFrom = source.EffectiveFrom, dest.EffectiveTo = dest.EffectiveTo, dest.ModifiedDateTimeUtc = GetUtcDate() WHEN NOT MATCHED THEN INSERT (ProgTypeId, ProgTypeDesc, ProgTypeDesc2, EffectiveFrom, EffectiveTo, CreatedDateTimeUtc) VALUES (source.ProgTypeId, LTrim(RTrim(source.ProgTypeDesc)), LTrim(RTrim(source.ProgTypeDesc2)), source.EffectiveFrom, source.EffectiveTo, GetUtcDate());", conn);
            comm.ExecuteNonQuery();
            comm = new SqlCommand("UPDATE [dbo].[ProgType] SET ProgTypeDesc = LTrim(RTrim(ProgTypeDesc)), ProgTypeDesc2 = LTrim(RTrim(ProgTypeDesc2));", conn);
            comm.ExecuteNonQuery();
            comm = new SqlCommand("MERGE [dbo].[Framework] dest USING [dbo].[Import_Framework] source ON dest.FrameworkCode = source.FrameworkCode AND dest.ProgType = source.ProgType AND dest.PathwayCode = source.PathwayCode WHEN MATCHED THEN UPDATE SET dest.PathwayName = source.PathwayName, dest.NasTitle = source.NasTitle, dest.EffectiveFrom = source.EffectiveFrom, dest.EffectiveTo = source.EffectiveTo, dest.SectorSubjectAreaTier1 = source.SectorSubjectAreaTier1, dest.SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2, dest.ModifiedDateTimeUtc = GetUtcDate(), dest.RecordStatusId = 2 WHEN NOT MATCHED THEN INSERT (FrameworkCode, ProgType, PathwayCode, PathwayName, NasTitle, EffectiveFrom, EffectiveTo, SectorSubjectAreaTier1, SectorSubjectAreaTier2, CreatedDateTimeUtc, RecordStatusId) VALUES (source.FrameworkCode, source.ProgType, source.PathwayCode, source.PathwayName, source.NasTitle, source.EffectiveFrom, source.EffectiveTo, source.SectorSubjectAreaTier1, source.SectorSubjectAreaTier2, GetUtcDate(), 2);", conn);
            comm.ExecuteNonQuery();

            // Delete Data Not In This File 
            comm = new SqlCommand("UPDATE [dbo].[Framework] SET RecordStatusId = 4 WHERE CAST(FrameworkCode AS VARCHAR) + '~' + CAST(ProgType AS VARCHAR) + '~' + CAST(PathwayCode AS VARCHAR) NOT IN (SELECT CAST(FrameworkCode AS VARCHAR) + '~' + CAST(ProgType AS VARCHAR) + '~' + CAST(PathwayCode AS VARCHAR) FROM [dbo].[Import_Framework]);", conn);
            comm.ExecuteNonQuery();

            sw.Stop();
            mduFrameworks.DurationInMilliseconds = (int)sw.ElapsedMilliseconds;
            mduFrameworks.RowsAfter = db.Frameworks.Count();
            db.MetadataUploads.Add(mduFrameworks);

            mduProgTypes.DurationInMilliseconds = (int)sw.ElapsedMilliseconds;
            mduProgTypes.RowsAfter = db.ProgTypes.Count();
            db.MetadataUploads.Add(mduProgTypes);

            mduSectorSubjectAreaTier1.DurationInMilliseconds = (int)sw.ElapsedMilliseconds;
            mduSectorSubjectAreaTier1.RowsAfter = db.SectorSubjectAreaTier1.Count();
            db.MetadataUploads.Add(mduSectorSubjectAreaTier1);

            mduSectorSubjectAreaTier2.DurationInMilliseconds = (int)sw.ElapsedMilliseconds;
            mduSectorSubjectAreaTier2.RowsAfter = db.SectorSubjectAreaTier2.Count();
            db.MetadataUploads.Add(mduSectorSubjectAreaTier2);

            // Import Standards
            MetadataUpload mduStandards = new MetadataUpload
            {
                MetadataUploadTypeId = (int)Constants.MetadataUploadType.Standards,
                CreatedByUserId = userId,
                CreatedDateTimeUtc = DateTime.UtcNow,
                FileName = Path.GetFileName(mdbFilename),
                FileSizeInBytes = (int)mdbFileSize,
                RowsBefore = db.Standards.Count()
            };

            MetadataUpload mduStandardSectorCodes = new MetadataUpload
            {
                MetadataUploadTypeId = (int)Constants.MetadataUploadType.StandardSectorCodes,
                CreatedByUserId = userId,
                CreatedDateTimeUtc = DateTime.UtcNow,
                FileName = Path.GetFileName(mdbFilename),
                FileSizeInBytes = (int)mdbFileSize,
                RowsBefore = db.StandardSectorCodes.Count()
            };

            sw = new Stopwatch();
            sw.Start();

            // Import Standards
            comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_Standard];", conn);
            comm.ExecuteNonQuery();
            dt = OpenDataTable("SELECT StandardCode, Version, LTrim(RTrim(StandardName)) AS StandardName, LTrim(RTrim(StandardSectorCode)) AS StandardSectorCode, EffectiveFrom, EffectiveTo, LTrim(RTrim(Left(UrlLink, 1000))) AS Url, SectorSubjectAreaTier1, SectorSubjectAreaTier2, NotionalEndLevel, OtherBodyApprovalRequired FROM Core_LARS_Standard WHERE StandardCode >= 1 AND (EffectiveTo IS NULL OR EffectiveTo >= Now) AND CStr(StandardCode) + '~' + CStr(Version) IN (SELECT CStr(StandardCode) + '~' + CStr(MaxVersion) FROM (SELECT StandardCode, max(Version) AS MaxVersion FROM Core_LARS_Standard GROUP BY StandardCode));");
            BulkImportData(conn, dt, "[dbo].[Import_Standard]");

            // Import Standard Sector Codes
            comm = new SqlCommand("TRUNCATE TABLE [dbo].[Import_StandardSectorCode];", conn);
            comm.ExecuteNonQuery();
            dt = OpenDataTable("SELECT LTrim(RTrim(StandardSectorCode)) AS StandardSectorCodeId, LTrim(RTrim(StandardSectorCodeDesc)) AS StandardSectorCodeDesc, LTrim(RTrim(StandardSectorCodeDesc2)) AS StandardSectorCodeDesc2, EffectiveFrom, EffectiveTo FROM CoreReference_LARS_StandardSectorCode_Lookup;");
            BulkImportData(conn, dt, "[dbo].[Import_StandardSectorCode]");

            // Merge Data
            comm = new SqlCommand("MERGE [dbo].[StandardSectorCode] dest USING [dbo].[Import_StandardSectorCode] source ON dest.StandardSectorCodeId = source.StandardSectorCodeId WHEN MATCHED THEN UPDATE SET dest.StandardSectorCodeDesc = source.StandardSectorCodeDesc, dest.StandardSectorCodeDesc2 = source.StandardSectorCodeDesc2, dest.EffectiveFrom = source.EffectiveFrom, dest.EffectiveTo = source.EffectiveTo, dest.ModifiedDateTimeUtc = GetUtcDate() WHEN NOT MATCHED THEN INSERT (StandardSectorCodeId, StandardSectorCodeDesc, StandardSectorCodeDesc2, EffectiveFrom, EffectiveTo, CreatedDateTimeUtc) VALUES (source.StandardSectorCodeId, source.StandardSectorCodeDesc, source.StandardSectorCodeDesc2, source.EffectiveFrom, source.EffectiveTo, GetUtcDate());", conn);
            comm.ExecuteNonQuery();
            comm = new SqlCommand("MERGE [dbo].[Standard] dest USING [dbo].[Import_Standard] source ON dest.StandardCode = source.StandardCode AND dest.Version = source.Version WHEN MATCHED THEN UPDATE SET dest.StandardName = source.StandardName, dest.StandardSectorCode = source.StandardSectorCode, dest.EffectiveFrom = source.EffectiveFrom, dest.EffectiveTo = source.EffectiveTo, dest.UrlLink = source.UrlLink, dest.ModifiedDateTimeUtc = GetUtcDate(), dest.RecordStatusId = 2, dest.NotionalEndLevel = source.NotionalEndLevel, dest.OtherBodyApprovalRequired = source.OtherBodyApprovalRequired WHEN NOT MATCHED THEN INSERT (StandardCode, Version, StandardName, StandardSectorCode, EffectiveFrom, EffectiveTo, UrlLink, CreatedDateTimeUtc, RecordStatusId, NotionalEndLevel, OtherBodyApprovalRequired) VALUES (source.StandardCode, source.Version, source.StandardName, source.StandardSectorCode, source.EffectiveFrom, source.EffectiveTo, source.UrlLink, GetUtcDate(), 2, source.NotionalEndLevel, source.OtherBodyApprovalRequired);", conn);
            comm.ExecuteNonQuery();

            // Delete Data Not In This File 
            comm = new SqlCommand("UPDATE [dbo].[Standard] SET RecordStatusId = 4 WHERE CAST(StandardCode AS VARCHAR) + '~' + CAST(Version AS VARCHAR) NOT IN (SELECT CAST(StandardCode AS VARCHAR) + '~' + CAST(Version AS VARCHAR) FROM [dbo].[Import_Standard]);", conn);
            comm.ExecuteNonQuery();

            sw.Stop();
            mduStandards.DurationInMilliseconds = (int)sw.ElapsedMilliseconds;
            mduStandards.RowsAfter = db.Standards.Count();
            db.MetadataUploads.Add(mduStandards);

            mduStandardSectorCodes.DurationInMilliseconds = (int)sw.ElapsedMilliseconds;
            mduStandardSectorCodes.RowsAfter = db.StandardSectorCodes.Count();
            db.MetadataUploads.Add(mduStandardSectorCodes);
        }

        [NonAction]
        private static DataTable OpenDataTable(String query)
        {
            DataSet ds = new DataSet();
            String connectionString = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + mdbFilename;
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(query, conn))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        conn.Open();
                        adapter.Fill(ds);
                    }
                }
            }

            return ds.Tables[0];
        }

        [NonAction]
        private static void BulkImportData(SqlConnection conn, DataTable dt, String destinationTable)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
            {
                // Set the timeout to 30 minutes
                bulkCopy.BulkCopyTimeout = 360;

                // The table I'm loading the data to
                bulkCopy.DestinationTableName = destinationTable;

                // How many records to send to the database in one go (all of them)
                bulkCopy.BatchSize = dt.Rows.Count;

                // Load the data to the database
                bulkCopy.WriteToServer(dt);

                // Close up          
                bulkCopy.Close();
            }
        }
    }
}