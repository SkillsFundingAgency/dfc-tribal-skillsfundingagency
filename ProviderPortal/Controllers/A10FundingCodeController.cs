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
    [PermissionAuthorize(Permission.PermissionName.CanManageA10Codes)]
    public class A10FundingCodeController : BaseController
    {
        //
        // GET: /A10FundingCode/
        public ActionResult Index()
        {
            var model = new A10FundingCodeViewModel();
            model.Populate(db);
            return View(model);
        }

        //
        // GET: /A10FundingCode/Create
        public ActionResult Create()
        {
            var model = new A10FundingCodeViewModelItem();
            model = model.Populate(db);
            PopulateRecordStatusList();
            return View(model);
        }

        //
        // POST: /A10FundingCode/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(A10FundingCodeViewModelItem model)
        {
            model.ValidateNewEntry(db, ModelState);
            if (ModelState.IsValid)
            {
                var item = model.ToEntity();
                db.A10FundingCode.Add(item);
                db.SaveChanges();
                ShowGenericSavedMessage();
                return RedirectToAction("Index");
            }
            PopulateRecordStatusList();
            return View(model);
        }

        //
        // GET: /A10FundingCode/Edit/5
        public ActionResult Edit(int id)
        {
            var model = new A10FundingCodeViewModelItem();
            model = model.Populate(db, id);
            if (model == null)
            {
                return HttpNotFound();
            }
            PopulateRecordStatusList();
            return View(model);
        }

        //
        // POST: /A10FundingCode/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(A10FundingCodeViewModelItem model)
        {
            if (ModelState.IsValid)
            {
                var item = model.ToEntity();
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                ShowGenericSavedMessage();
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