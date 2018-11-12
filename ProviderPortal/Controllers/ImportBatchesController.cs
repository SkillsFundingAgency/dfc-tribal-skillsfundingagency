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
    [PermissionAuthorize(Permission.PermissionName.CanManageImportBatches)]
    public class ImportBatchesController : BaseController
    {
        //
        // GET: /ImportBatches/
        public ActionResult Index()
        {
            var model = new ImportBatchesViewModel();
            model.Populate(db);
            return View(model);
        }

        //
        // GET: /ImportBatches/Create
        public ActionResult Create()
        {
            var model = new ImportBatchesViewModelItem();
            model = model.Populate(db);
            return View(model);
        }

        //
        // POST: /ImportBatches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ImportBatchesViewModelItem model)
        {
            model.ValidateEntry(db, ModelState);
            if (ModelState.IsValid)
            {
                if (model.Current)
                {
                    if (model.Current)
                    {
                        ImportBatch currentBatch = db.ImportBatches.FirstOrDefault(x => x.Current);
                        if (currentBatch != null)
                        {
                            currentBatch.Current = false;
                            db.Entry(currentBatch).State = EntityState.Modified;
                        }
                    }
                }

                var item = model.ToEntity();
                db.ImportBatches.Add(item);
                db.SaveChanges();
                ShowGenericSavedMessage();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //
        // GET: /ImportBatches/Edit/5
        public ActionResult Edit(int id)
        {
            var model = new ImportBatchesViewModelItem();
            model = model.Populate(db, id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        //
        // POST: /ImportBatches/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ImportBatchesViewModelItem model)
        {
            model.ValidateEntry(db, ModelState);
            if (ModelState.IsValid)
            {
                if (model.Current)
                {
                    ImportBatch currentBatch = db.ImportBatches.FirstOrDefault(x => x.Current && x.ImportBatchId != model.ImportBatchId);
                    if (currentBatch != null)
                    {
                        currentBatch.Current = false;
                        db.Entry(currentBatch).State = EntityState.Modified;
                    }
                }

                var item = model.ToEntity();
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();

                ShowGenericSavedMessage();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        #region Private methods

        #endregion
    }
}