using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    [ContextAuthorize(UserContext.UserContextName.Administration)]
    public class PublicAPIUserController : BaseController
    {
        //
        // GET: /PublicAPIUser/
        [PermissionAuthorize(Permission.PermissionName.CanViewPublicAPIUsers)]
        public ActionResult Index()
        {
            List<ListPublicAPIUserModel> model = new List<ListPublicAPIUserModel>();
            foreach (PublicAPIUser pau in db.PublicAPIUsers.OrderBy(x => x.CompanyName))
            {
                model.Add(new ListPublicAPIUserModel(pau));
            }
            return View(model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanAddEditPublicAPIUsers)]
        public ActionResult Create()
        {
            AddEditPublicAPIUserModel model = new AddEditPublicAPIUserModel
            {
                APIKey = Guid.NewGuid().ToString()
            };

            // Get Lookups
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanAddEditPublicAPIUsers)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddEditPublicAPIUserModel model)
        {
            if (ModelState.IsValid)
            {
                PublicAPIUser pau = model.ToEntity(db);
                pau.PublicAPIUserId = Guid.Parse(model.APIKey);

                db.Entry(pau).State = EntityState.Added;
                db.SaveChanges();
                ShowGenericSavedMessage();

                return RedirectToAction("Index");
            }

            // Get Lookups
            GetLookups(model);

            return View(model);
        }
        
        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanAddEditPublicAPIUsers)]
        public ActionResult Edit(String id)
        {
            PublicAPIUser pau = db.PublicAPIUsers.Find(Guid.Parse(id));
            if (pau == null)
            {
                return new HttpNotFoundResult();
            }
            AddEditPublicAPIUserModel model = new AddEditPublicAPIUserModel(pau);

            // Get Lookups
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanAddEditPublicAPIUsers)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddEditPublicAPIUserModel model)
        {
            if (ModelState.IsValid)
            {
                PublicAPIUser pau = model.ToEntity(db);
                pau.ModifiedDateTimeUtc = DateTime.UtcNow;
                pau.ModifiedByUserId = Permission.GetCurrentUserId();

                db.Entry(pau).State = EntityState.Modified;
                db.SaveChanges();
                ShowGenericSavedMessage();

                return RedirectToAction("Index");
            }

            // Get Lookups
            GetLookups(model);

            return View(model);
        }

        [NonAction]
        private void GetLookups(AddEditPublicAPIUserModel model)
        {
            ViewBag.RecordStatuses = new SelectList(
                db.RecordStatus.Where(x => x.RecordStatusId == (int)Constants.RecordStatus.Live || x.RecordStatusId == (int)Constants.RecordStatus.Deleted),
                "RecordStatusId",
                "RecordStatusName",
                model.RecordStatusId);
        }

    }
}