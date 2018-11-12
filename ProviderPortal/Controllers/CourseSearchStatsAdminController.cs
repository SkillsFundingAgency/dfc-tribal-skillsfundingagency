using System;
using System.Web.Mvc;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    using System.Globalization;
    using System.IO;
    using System.Net;

    using Tribal.SkillsFundingAgency.ProviderPortal.Models;

    public class CourseSearchStatsAdminController : BaseController
    {
        // GET: CourseSearchStatsAdmin/Index
        [PermissionAuthorize(new[] { Permission.PermissionName.CanEditCourseSearchStats })]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult Index()
        {
            return this.DisplayIndex();
        }

        // POST: CourseSearchStatsAdmin/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(new[] { Permission.PermissionName.CanEditCourseSearchStats })]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult Index(CourseSearchStatsAdminModel model)
        {
            if (Request.Form["CreateFolder"] != null)
            {
                DateTime date;
                if (!DateTime.TryParse(model.NewFolderName, out date))
                {
                    ModelState.AddModelError(
                        "",
                        AppGlobal.Language.GetText(this, "InvalidDateError", "Not a valid name for a new folder"));
                }
                if (ModelState.IsValid)
                {
                    UsageStatistics.CreateFolder(date);
                    ModelState.SetModelValue(
                        "NewFolderName",
                        new ValueProviderResult(null, string.Empty, CultureInfo.InvariantCulture));
                    ShowGenericSavedMessage();
                }
            }

            if (Request.Form["UploadFile"] != null)
            {
                if (model.UploadToFolder == null)
                {
                    ModelState.AddModelError(
                        "",
                        AppGlobal.Language.GetText(
                            this,
                            "NoUploadFolderError",
                            "Cannot upload a file without selecting a folder to upload into."));
                }
                if (model.FileUpload == null)
                {
                    ModelState.AddModelError(
                        "",
                        AppGlobal.Language.GetText(
                            this,
                            "NoUploadFileError",
                            "Cannot upload without selecting a file."));
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        UsageStatistics.AddFile(model.UploadToFolder, model.FileUpload);
                        ModelState.SetModelValue(
                            "UploadToFolder",
                            new ValueProviderResult(null, string.Empty, CultureInfo.InvariantCulture));
                        ShowGenericSavedMessage();
                    }
                    catch (TimeoutException)
                    {
                        AppGlobal.Log.WriteWarning("Virus scanner timed out scanning file " + model.FileUpload.FileName);
                        ModelState.AddModelError(
                            "",
                            AppGlobal.Language.GetText(
                                this,
                                "VirusScanTimeOut",
                                "File could not be uploaded, because the virus scan operation timed out."));
                    }
                    catch (InvalidDataException)
                    {
                        ModelState.AddModelError(
                            "",
                            AppGlobal.Language.GetText(
                                this,
                                "VirusDetectedWarning",
                                "File has been rejected by the virus scanner."));
                    }
                    catch (ArgumentException ex)
                    {
                        string message;
                        switch (ex.Message)
                        {
                            case "DuplicateFileWarning":
                                message = AppGlobal.Language.GetText(
                                    this,
                                    "DuplicateFileWarning",
                                    "A file with that name already exists in that folder.");
                                break;

                            case "FileExtensionNotAllowed":
                                message = AppGlobal.Language.GetText(
                                    this,
                                    "FileExtensionNotAllowed",
                                    "Invalid file type. Permitted file types are "
                                    + String.Join(", ", UsageStatistics.FileExtensionWhitelist));
                                break;

                            case "InvalidFolderName":
                                message = AppGlobal.Language.GetText(
                                    this,
                                    "InvalidFolderName",
                                    "Invalid folder specified.");
                                break;

                            default:
                                message = ex.Message;
                                break;
                        }
                        ModelState.AddModelError("", message);
                    }
                }
            }

            return this.DisplayIndex();
        }

        // GET: CourseSearchStatsAdmin/DeleteFolder/yyyymmdd
        [PermissionAuthorize(new[] { Permission.PermissionName.CanEditCourseSearchStats })]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult DeleteFolder(string id)
        {
            try
            {
                UsageStatistics.DeleteFolder(id);
                ShowGenericSavedMessage();
                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        // GET: CourseSearchStatsAdmin/DeleteFile/yyyymmdd?filename=abcd.pdf
        [PermissionAuthorize(new[] { Permission.PermissionName.CanEditCourseSearchStats })]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult DeleteFile(string id, string filename)
        {
            try
            {
                UsageStatistics.DeleteFile(id, filename);
                ShowGenericSavedMessage();
                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        // GET: CourseSearchStatsAdmin/GetFile/yyyymmdd?filename=abcd.pdf
        [ContextAuthorize(UserContext.UserContextName.Authenticated)]
        public ActionResult GetFile(string id, string filename)
        {
            try
            {
                return File(
                    UsageStatistics.GetFile(id, filename),
                    System.Net.Mime.MediaTypeNames.Application.Octet,
                    filename);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        [NonAction]
        private ActionResult DisplayIndex()
        {
            var model = new CourseSearchStatsAdminModel();
            model.Populate(UsageStatistics.GetAll(true));
            this.ViewBag.FolderNames = new SelectList(model.FolderNames, "Key", "Value");
            return this.View(model);
        }
    }
}