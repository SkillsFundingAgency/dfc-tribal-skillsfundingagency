using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes.Content;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    // When adding actions for this controller you must add them to the SiteContentActions route in ContentManager.MapCatchAll.
    public class SiteContentController : BaseController
    {
        // GET: /SiteContent/
        public ActionResult Index()
        {
            var model = new ContentListViewModel();
            model = model.Populate(db);
            return View(model);
        }

        //
        // GET: /SiteContent/Display/id        
        [AllowAnonymous]
        public ActionResult Display(string id = null, int version = 0,
            UserContext.UserContextName context = UserContext.UserContextName.None,
            bool safeEmbed = false)
        {
            var model = new ContentViewModel {SafeEmbed = safeEmbed};
            model = model.Populate(id, context, version, db, userContext);

            switch (model.Status)
            {
                case ContentStatus.NotFound:
                    return HttpNotFound();

                case ContentStatus.AuthenticationRequired:
                    return new HttpUnauthorizedResult();

                default:
                    return View("Display", model);
            }
        }

        //
        // GET /{controller}/Help/id
        [AllowAnonymous]
        public ActionResult DisplayHelp(string id)
        {
            var path = Request.Url == null ? null : ContentManager.TrimPath(Request.Url.LocalPath);
            return Display(path);
        }

        // GET: /SiteContent/Create
        [PermissionAuthorize(Permission.PermissionName.CanManageContent)]
        public ActionResult Create(string id)
        {
            var model = new AddEditContentViewModel();
            model = model.PopulateAsNew(id, db);
            //model.Validate(db, ModelState);
            return View(model);
        }

        // POST: /SiteContent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [PermissionAuthorize(Permission.PermissionName.CanManageContent)]
        public ActionResult Create(AddEditContentViewModel model)
        {
            model.Validate(db, ModelState);
            if (ModelState.IsValid)
            {
                var content = model.ToEntity(db);
                if (content.ContentId == 0) db.Contents.Add(content);
                db.SaveChanges();
                ContentCache.Remove(model.Path, model.UserContext);
                ShowGenericSavedMessage();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /SiteContent/Edit/5
        [PermissionAuthorize(Permission.PermissionName.CanManageContent)]
        public ActionResult Edit(string id = null, int version = 0,
            UserContext.UserContextName context = UserContext.UserContextName.None)
        {
            if (id == null)
            {
                return RedirectToAction("Create");
            }
            var model = new AddEditContentViewModel();
            model = model.Populate(id, context, version, db, userContext);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: /SiteContent/Edit/5
        [PermissionAuthorize(Permission.PermissionName.CanManageContent)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(AddEditContentViewModel model)
        {
            model.Validate(db, ModelState);
            if (ModelState.IsValid
                || model.SubmitAction.Equals("Delete", StringComparison.CurrentCultureIgnoreCase)
                || model.SubmitAction.Equals("Archive", StringComparison.CurrentCultureIgnoreCase))
            {
                var errors = ContentManager.HandleContentEdit(db, model);
                if (errors.Any())
                {
                    foreach (var item in errors)
                    {
                        ModelState.AddModelError(item.Key, item.Value);
                    }
                    return View(model);
                }
                db.SaveChanges();
                ContentCache.Remove(model.Path, model.UserContext);
                ShowGenericSavedMessage();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /SiteContent/Delete/5
        [PermissionAuthorize(Permission.PermissionName.CanManageContent)]
        public ActionResult Delete(string id, int version)
        {
            var model = new ContentViewModel();
            model = model.Populate(id, UserContext.UserContextName.None, version, db, userContext);
            switch (model.Status)
            {
                case ContentStatus.NotFound:
                    return HttpNotFound();

                case ContentStatus.AuthenticationRequired:
                    return new HttpUnauthorizedResult();

                default:
                    return View(model);
            }
        }

        // POST: /SiteContent/Delete/5
        [PermissionAuthorize(Permission.PermissionName.CanManageContent)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var content = new Content {ContentId = id};
            db.Entry(content).State = EntityState.Deleted;
            db.SaveChanges();
            ContentCache.Remove(id);
            ShowGenericSavedMessage();
            return RedirectToAction("Index");
        }

        // GET: /SiteContent/History/5
        [PermissionAuthorize(Permission.PermissionName.CanManageContent)]
        public ActionResult History(string id = null)
        {
            var model = new ContentListViewModel();
            model = model.Populate(db, id);
            if (!model.Items.Any()) return HttpNotFound();
            return View(model);
        }

        // GET: /SiteContent/Archived/5
        [PermissionAuthorize(Permission.PermissionName.CanManageContent)]
        public ActionResult Archived()
        {
            var model = new ContentListViewModel();
            model = model.PopulateArchived(db);
            //if (!model.Items.Any()) return HttpNotFound();
            return View(model);
        }


        [PermissionAuthorize(Permission.PermissionName.CanManageContent)]
        public ActionResult FileManager()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Download(string area, string id)
        {
            var config = new TinyFileManager.NET.clsConfig();
            var filePath = area.Equals("Files", StringComparison.CurrentCultureIgnoreCase)
                ? config.strUploadPath + id.Replace("/", "\\")
                : area.Equals("Thumbs", StringComparison.CurrentCultureIgnoreCase)
                    ? config.strThumbPath + id.Replace("/", "\\")
                    : null;
            if (filePath != null && filePath.IsValidPath() && System.IO.File.Exists(filePath))
            {
                var mimeType = MimeMapping.GetMimeMapping(filePath);
                return new FilePathResult(filePath, mimeType);
            }
            return HttpNotFound();
        }
    }
}
