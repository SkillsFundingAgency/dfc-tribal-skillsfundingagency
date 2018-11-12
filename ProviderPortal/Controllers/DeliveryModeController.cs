using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    [ContextAuthorize(UserContext.UserContextName.Administration)]
    [PermissionAuthorize(Permission.PermissionName.CanManageDeliveryModes)]
    public class DeliveryModeController : BaseController
    {
        //
        // GET: /DeliveryMode/
        public ActionResult Index()
        {
            var model = new DeliveryModeViewModel();
            model.Populate(db);
            return View(model);
        }

        //
        // GET: /DeliveryMode/Create
        public ActionResult Create()
        {
            var model = new DeliveryModeViewModelItem();
            model = model.Populate(db);
            PopulateRecordStatusList();
            return View(model);
        }

        //
        // POST: /DeliveryMode/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DeliveryModeViewModelItem model)
        {
            model.ValidateNewEntry(db, ModelState);
            if (ModelState.IsValid)
            {
                var item = model.ToEntity();
                db.DeliveryModes.Add(item);
                db.SaveChanges();
                ShowGenericSavedMessage();
                return RedirectToAction("Index");
            }
            PopulateRecordStatusList();
            return View(model);
        }

        //
        // GET: /DeliveryMode/Edit/5
        public ActionResult Edit(int id)
        {
            var model = new DeliveryModeViewModelItem();
            model = model.Populate(db, id);
            if (model == null)
            {
                return HttpNotFound();
            }
            PopulateRecordStatusList();
            return View(model);
        }

        //
        // POST: /DeliveryMode/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DeliveryModeViewModelItem model)
        {
            model.ValidateEditedEntry(db, ModelState);
            if (ModelState.IsValid)
            {
                var item = model.ToEntity();
                db.Entry(item).State = EntityState.Modified;
                ShowGenericSavedMessage();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateRecordStatusList();
            return View(model);
        }

        #region Private methods

        private void PopulateRecordStatusList()
        {
            ViewBag.RecordStatu = db.RecordStatus
                .Where(x => x.IsPublished || x.IsDeleted)
                .OrderBy(x => x.RecordStatusId)
                .Select(x => new SelectListItem
                {
                    Value = x.RecordStatusId.ToString(),
                    Text = x.RecordStatusName
                }).ToList();
        }

        #endregion
    }
}