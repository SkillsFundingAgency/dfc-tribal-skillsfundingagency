using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.BulkUpload.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.BulkUploadWCFService;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;


namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public class BulkUploadController : BaseController
    {
        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationFiles, Permission.PermissionName.CanBulkUploadProviderFiles)]
        [ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        public ActionResult Courses()
        {
            if ((userContext.IsProvider() && Permission.HasPermission(false, true, Permission.PermissionName.CanBulkUploadProviderFiles)) ||
                (userContext.IsOrganisation() && Permission.HasPermission(false, true, Permission.PermissionName.CanBulkUploadOrganisationFiles)))
            {
                var model = new BulkUploadViewModel();
                model.Populate(userContext, db, Constants.BulkUpload_DataType.CourseData);
                return View(model);
            }

            return Redirect();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationFiles, Permission.PermissionName.CanBulkUploadProviderFiles)]
        [ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        public FileContentResult CourseDownload(BulkUploadViewModel model)
        {
            var transformedString = model.TransformToCsv(userContext, db, Constants.BulkUpload_DataType.CourseData);
            var fileName = model.GetCsvFileName(userContext, db, Constants.BulkUpload_DataType.CourseData);
            return File(transformedString, "text/csv", fileName);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationApprenticeshipFiles, Permission.PermissionName.CanBulkUploadProviderApprenticeshipFiles)]
        [ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        public ActionResult Apprenticeships()
        {
            if ((userContext.IsProvider() && Permission.HasPermission(false, true, Permission.PermissionName.CanBulkUploadProviderApprenticeshipFiles)) ||
                (userContext.IsOrganisation() && Permission.HasPermission(false, true, Permission.PermissionName.CanBulkUploadOrganisationApprenticeshipFiles)))
            {
                var model = new BulkUploadViewModel();
                model.Populate(userContext, db, Constants.BulkUpload_DataType.ApprenticeshipData);
                return View(model);
            }

            return Redirect();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationApprenticeshipFiles, Permission.PermissionName.CanBulkUploadProviderApprenticeshipFiles)]
        [ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        public FileContentResult ApprenticeshipDownload(BulkUploadViewModel model)
        {
            var transformedString = model.TransformToCsv(userContext, db, Constants.BulkUpload_DataType.ApprenticeshipData);
            var fileName = model.GetCsvFileName(userContext, db, Constants.BulkUpload_DataType.ApprenticeshipData);
            return File(transformedString, "text/csv", fileName);
        }


        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationFiles, Permission.PermissionName.CanBulkUploadProviderFiles)]
        [ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        public NewtonsoftJsonResult GetCourseInstanceCount(Int32 providerId)
        {
            Provider provider = db.Providers.Find(providerId);
            if (provider == null)
            {
                return null;
            }
            return Json(BulkUploadModelExtensions.GetOpportunityCount(providerId).ToString("N0"));
        }

        //[HttpPost]
        //[PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationFiles, Permission.PermissionName.CanBulkUploadProviderApprenticeshipFiles)]
        //[ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        //public NewtonsoftJsonResult GetApprenticeshipInstanceCount(Int32 providerId)
        //{
        //    Provider provider = db.Providers.Find(providerId);
        //    if (provider == null)
        //    {
        //        return null;
        //    }
        //    return Json(BulkUploadModelExtensions.GetDeliveryLocationCount(providerId).ToString("N0"));
        //}

        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationFiles, Permission.PermissionName.CanBulkUploadProviderFiles)]
        [ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        [HttpPost]
        public ActionResult CourseUpload(BulkUploadViewModel model)
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
            if (ModelState.IsValid)
            {
                model.InitiateBulkUpload(userContext, db);
                model.Populate(userContext, db, Constants.BulkUpload_DataType.CourseData);
                return View("PostCourseUpload", model);
            }
            model.Populate(userContext, db, Constants.BulkUpload_DataType.CourseData);
            return View("Courses", model);
        }

        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationFiles, Permission.PermissionName.CanBulkUploadProviderApprenticeshipFiles)]
        [ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        [HttpPost]
        public ActionResult ApprenticeshipUpload(BulkUploadViewModel model)
        {
            String[] validFileTypes = { ".csv" };
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
            if (ModelState.IsValid)
            {
                model.InitiateBulkUpload(userContext, db);
                model.Populate(userContext, db, Constants.BulkUpload_DataType.ApprenticeshipData);
                return View("PostApprenticeshipUpload", model);
            }
            model.Populate(userContext, db, Constants.BulkUpload_DataType.ApprenticeshipData);
            return View("Apprenticeships", model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationFiles, Permission.PermissionName.CanBulkUploadProviderFiles)]
        [ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        public ActionResult CourseHistory()
        {
            if ((userContext.IsProvider() && Permission.HasPermission(false, true, Permission.PermissionName.CanBulkUploadProviderFiles)) ||
                (userContext.IsOrganisation() && Permission.HasPermission(false, true, Permission.PermissionName.CanBulkUploadOrganisationFiles)))
            {
                var model = new List<BulkUploadHistoryViewModel>();
                model.Populate(userContext, db, Constants.BulkUpload_DataType.CourseData);
                ViewBag.PageType = AppGlobal.Language.GetText(this, "Course", "Course");
                return View("History", model);
            }

            return Redirect();
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationApprenticeshipFiles, Permission.PermissionName.CanBulkUploadProviderApprenticeshipFiles)]
        [ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        public ActionResult ApprenticeshipHistory()
        {
            if ((userContext.IsProvider() && Permission.HasPermission(false, true, Permission.PermissionName.CanBulkUploadProviderApprenticeshipFiles)) ||
                (userContext.IsOrganisation() && Permission.HasPermission(false, true, Permission.PermissionName.CanBulkUploadOrganisationApprenticeshipFiles)))
            {
                var model = new List<BulkUploadHistoryViewModel>();
                model.Populate(userContext, db, Constants.BulkUpload_DataType.ApprenticeshipData);
                ViewBag.PageType = AppGlobal.Language.GetText(this, "Apprenticeship", "Apprenticeship");
                return View("History", model);
            }

            return Redirect();
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationFiles, Permission.PermissionName.CanBulkUploadProviderFiles)]
        [ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        public ActionResult Download(int id)
        {
            var selectedModel = new List<BulkUploadHistoryViewModel>().GetHistoryFileUrl(userContext, db, id);
            if (selectedModel == null)
            {
                return HttpNotFound();
            }

            return File(selectedModel.DownloadUrl, "text/csv", selectedModel.FileName);
        }


        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationFiles, Permission.PermissionName.CanBulkUploadProviderFiles)]
        [ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        public ActionResult HistoryDetails(int id)
        {
            if ((userContext.IsProvider() && Permission.HasPermission(false, true, Permission.PermissionName.CanBulkUploadProviderFiles)) ||
                (userContext.IsOrganisation() && Permission.HasPermission(false, true, Permission.PermissionName.CanBulkUploadOrganisationFiles)))
            {
                var model = new BulkUploadHistoryDetailViewModel();
                model.Populate(userContext, db, id);
                if (model.AccessDenied)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
            return Redirect();
        }


        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanBulkUploadOrganisationFiles, Permission.PermissionName.CanBulkUploadProviderFiles)]
        [ContextAuthorize(UserContext.UserContextName.Organisation, UserContext.UserContextName.Provider)]
        public ActionResult ConfirmUpload(BulkUploadHistoryDetailViewModel bulkModel)
        {
            if ((userContext.IsProvider() && Permission.HasPermission(false, true, Permission.PermissionName.CanBulkUploadProviderFiles)) ||
                (userContext.IsOrganisation() && Permission.HasPermission(false, true, Permission.PermissionName.CanBulkUploadOrganisationFiles)))
            {
                var history = db.BulkUploads.FirstOrDefault(b => b.BulkUploadId.Equals(bulkModel.BulkUploadId));
                if (history == null)
                {
                    return HttpNotFound();
                }

                if (userContext.ItemId.HasValue)
                {
                    if ((userContext.IsOrganisation() && history.UserOrganisationId != userContext.ItemId)
                        || (userContext.IsProvider() && history.UserProviderId != userContext.ItemId))
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    return HttpNotFound();
                }

                // Check that the status is currently at "Needs Confirmation"
                if (history
                    .BulkUploadStatusHistories
                    .OrderByDescending(x => x.CreatedDateTimeUtc)
                    .First()
                    .BulkUploadStatusId != (Int32)Constants.BulkUploadStatus.Needs_Confirmation)
                {
                    return HttpNotFound();
                }

                var model = new BulkUploadViewModel
                {
                    Summary = new UploadSummary
                    {
                        TargetFileUrl = history.FilePath,
                        FileName = history.FileName
                    },
                    OverrideException = true
                };

                /*
                 * Matt - dev note
                 * ---------------------
                 * This section relates to replacing the in-application BulkUpload
                 * with a more efficient out-of-application service based system.
                 * 
                 */
                try
                {
                    var bulkUploadService = new BulkUploadWCFServiceClient();
                    var parameters = new ConfirmParameters
                    {
                        BulkUploadId = bulkModel.BulkUploadId,
                        UserId =
                            history
                                .BulkUploadStatusHistories
                                .OrderByDescending(x => x.CreatedDateTimeUtc)
                                .First()
                                .AspNetUser
                                .Id
                    };
                    bulkUploadService.Confirm(parameters);
                }
                catch (FaultException<BulkUploadFault> ex)
                {
                    model.Message = String.Format("The bulk upload service reported an error: {0}", ex.Detail.Message);
                }
                /*
                 * end
                 */

                //model.InitiateBulkUpload(userContext, db);
                string viewName;
                if (history.FileContentType != null && history.FileContentType == (int)Constants.FileContentType.ApprenticeshipData)
                {
                    viewName = "PostApprenticeshipUpload";
                }
                else
                {
                    viewName = "PostCourseUpload";
                }

             //   var viewName = (bulkModel.FileContentType == Constants.FileContentType.CourseData ? "PostCourseUpload" : "PostApprenticeshipUpload");

                return View(viewName, model);
            }
            return Redirect();
        }

        [NonAction]
        private ActionResult Redirect()
        {
            return RedirectToActionPermanent("Index", "Home");
        }
    }
}