using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using CsvHelper;
//using Tribal.SkillsFundingAgency.ProviderPortal.BulkUpload.Validators.FileValidators;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    [ContextAuthorize(UserContext.UserContextName.Administration)]
    public class FEChoicesController : BaseController
    {
        //
        // GET: /FEChoices/
        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadFEChoicesData)]
        public ActionResult Index()
        {
            FEChoicesUploadModel model = new FEChoicesUploadModel();
            GetLastUploadDetails(model);
            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanUploadFEChoicesData)]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FEChoicesUploadModel model)
        {
            if (ModelState.IsValid)
            {
                const Int32 fieldUPIN = 0;
                const Int32 fieldLearnerDestination = 1;
                const Int32 fieldEmployerSatisfaction = 2;
                const Int32 fieldLearnerSatisfaction = 3;

                try
                {
                    String[] validFileTypes = {".csv"};
                    Boolean validFileType = false;

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
                        ModelState.AddModelError("File", AppGlobal.Language.GetText(this, "CSVFilesOnly", "Please upload a CSV file"));
                    }
                    else
                    {
                        var metadataUpload = new MetadataUpload
                        {
                            MetadataUploadTypeId = (int)Constants.MetadataUploadType.FEChoices,
                            CreatedByUserId = Permission.GetCurrentUserGuid().ToString(),
                            CreatedDateTimeUtc = DateTime.UtcNow,
                            FileName = model.File.FileName,
                            FileSizeInBytes = model.File.ContentLength,
                            RowsBefore = db.FEChoices.Count()
                        };
                        var sw = new Stopwatch();
                        sw.Start();

                        // Delete the current data
                        foreach (FEChoice fe in db.FEChoices.ToList())
                        {
                            db.Entry(fe).State = EntityState.Deleted;
                        }

                        // Import the new data
                        using (TextReader csvFile = new StreamReader(model.File.InputStream))
                        {
                            using (var csvReader = new CsvReader(csvFile))
                            {
                                csvReader.Configuration.HasHeaderRecord = true;

                                while (csvReader.Read())
                                {
                                    Int32? UPIN = GetIntOrNull(csvReader.GetField<String>(fieldUPIN));
                                    if (UPIN != null)
                                    {
                                        Double? LearnerDestination = GetDoubleOrNull(csvReader.GetField<String>(fieldLearnerDestination));
                                        Double? EmployerSatisfaction = GetDoubleOrNull(csvReader.GetField<String>(fieldEmployerSatisfaction));
                                        Double? LearnerSatisfaction = GetDoubleOrNull(csvReader.GetField<String>(fieldLearnerSatisfaction));

                                        FEChoice fec = db.FEChoices.Find(UPIN);
                                        if (fec != null)
                                        {
                                            db.Entry(fec).State = EntityState.Modified;
                                        }
                                        else
                                        {
                                            fec = new FEChoice
                                            {
                                                UPIN = UPIN.Value
                                            };
                                            db.FEChoices.Add(fec);
                                        }

                                        fec.LearnerDestination = LearnerDestination;
                                        fec.LearnerSatisfaction = LearnerSatisfaction;
                                        fec.EmployerSatisfaction = EmployerSatisfaction;
                                        fec.CreatedDateTimeUtc = DateTime.UtcNow;
                                    }
                                    else if (ModelState.IsValid)
                                    {
                                        // UPIN Is null - Create a model error
                                        ModelState.AddModelError("", AppGlobal.Language.GetText(this, "UPINIsNull", "UPIN is empty"));
                                    }
                                }
                            }
                        }

                        if (ModelState.IsValid)
                        {
                            sw.Stop();
                            metadataUpload.DurationInMilliseconds = (int)sw.ElapsedMilliseconds;
                            metadataUpload.RowsAfter = db.FEChoices.Count();
                            db.MetadataUploads.Add(metadataUpload);

                            // Save the changes                
                            db.SaveChanges();

                            ViewBag.Message = AppGlobal.Language.GetText(this, "ImportSuccessful", "FE Choices Data Successfully Imported");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Create a model error
                    ModelState.AddModelError("", ex.Message);
                }
            }

            GetLastUploadDetails(model);

            return View(model);
        }

        [NonAction]
        private void GetLastUploadDetails(FEChoicesUploadModel model)
        {
            MetadataUpload dataUpload = new ProviderPortalEntities().MetadataUploads.Where(m => m.MetadataUploadTypeId == (Int32)Constants.MetadataUploadType.FEChoices).OrderByDescending(m => m.CreatedDateTimeUtc).FirstOrDefault();
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

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadFEChoicesData)]
        public FileResult DownloadCurrent()
        {
            String fileContent = "UPIN,LearnerDestination,EmployerSatisfaction,LearnerSatisfaction\r";
            foreach (FEChoice fe in db.FEChoices.ToList())
            {
                fileContent += String.Format("{0},{1},{2},{3}\r", fe.UPIN, fe.LearnerDestination, fe.EmployerSatisfaction, fe.LearnerSatisfaction);
            }

            FileContentResult response = new FileContentResult(GetBytes(fileContent), "text/csv") { FileDownloadName = "FEChoices.csv" };
            return response;
        }

        [NonAction]
        private static byte[] GetBytes(string s)
        {
            byte[] str = Encoding.UTF8.GetBytes(s);           
            byte[] bytes = new byte[3 + str.Length];
            bytes[0] = 0xEF; // UTF-8 BOM
            bytes[1] = 0xBB;
            bytes[2] = 0xBF;
            Buffer.BlockCopy(str, 0, bytes, 3, bytes.Length-3);
            return bytes;
        }

        [NonAction]
        private static Int32? GetIntOrNull(String data)
        {
            Int32 i;
            if (Int32.TryParse(data, out i))
            {
                return i;
            }

            return null;
        }

        [NonAction]
        private static Double? GetDoubleOrNull(String data)
        {
            Double dbl;
            if (Double.TryParse(data, out dbl))
            {
                return dbl;
            }

            return null;
        }
    }
}