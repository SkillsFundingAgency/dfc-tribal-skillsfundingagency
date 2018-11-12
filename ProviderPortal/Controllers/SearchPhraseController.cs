using System;
using System.Linq;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    [PermissionAuthorize(Permission.PermissionName.CanEditAPISearchPhrases)]
    [ContextAuthorize(UserContext.UserContextName.Administration)]
    public class SearchPhraseController : BaseController
    {
        // GET: /SearchPhrase
        [HttpGet]
        public ActionResult Index()
        {
            SearchPhraseListModel model = new SearchPhraseListModel();
            model.Populate(db);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            AddEditSearchPhraseModel model = new AddEditSearchPhraseModel();
            AddListsToModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AddEditSearchPhraseModel model)
        {
            SearchPhrase searchPhrase = model.ToEntity(db);
            if (searchPhrase == null)
            {
                return HttpNotFound();
            }

            SearchPhrase phrase = db.SearchPhrases.Where(x => x.Phrase == searchPhrase.Phrase && x.SearchPhraseId != searchPhrase.SearchPhraseId).FirstOrDefault();
            if (phrase != null)
            {
                ModelState.AddModelError("SearchPhrase", AppGlobal.Language.GetText(this, "SearchPhraseNotUnique", "Search Phrase is already in use"));
            }

            if (model.SelectedQualificationLevels.Count() == 0 && model.SelectedStudyModes.Count() == 0 && model.SelectedAttendanceTypes.Count() == 0 && model.SelectedAttendancePatterns.Count() == 0)
            {
                ModelState.AddModelError("", AppGlobal.Language.GetText(this, "MustSelectReplacement", "You must select at least 1 Qualification Level, Study Mode, Attendance Mode or Attendance Pattern."));
            }

            if (ModelState.IsValid)
            {
                db.Entry(searchPhrase).State = !model.SearchPhraseId.HasValue ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ShowGenericSavedMessage();
                return RedirectToAction("Index");
            }

            AddListsToModel(model);
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Int32 id)
        {
            SearchPhrase searchPhrase = db.SearchPhrases.Find(id);
            if (searchPhrase == null)
            {
                return HttpNotFound();
            }

            AddEditSearchPhraseModel model = new AddEditSearchPhraseModel(searchPhrase);
            AddListsToModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AddEditSearchPhraseModel model)
        {
            SearchPhrase searchPhrase = model.ToEntity(db);
            if (searchPhrase == null)
            {
                return HttpNotFound();
            }

            SearchPhrase phrase = db.SearchPhrases.Where(x => x.Phrase == searchPhrase.Phrase && x.SearchPhraseId != searchPhrase.SearchPhraseId).FirstOrDefault();
            if (phrase != null)
            {
                ModelState.AddModelError("SearchPhrase", AppGlobal.Language.GetText(this, "SearchPhraseNotUnique", "Search Phrase is already in use"));
            }

            if (model.SelectedQualificationLevels.Count() == 0 && model.SelectedStudyModes.Count() == 0 && model.SelectedAttendanceTypes.Count() == 0 && model.SelectedAttendancePatterns.Count() == 0)
            {
                ModelState.AddModelError("", AppGlobal.Language.GetText(this, "MustSelectReplacement", "You must select at least 1 Qualification Level, Study Mode, Attendance Mode or Attendance Pattern."));
            }

            if (ModelState.IsValid)
            {
                db.Entry(searchPhrase).State = !model.SearchPhraseId.HasValue ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ShowGenericSavedMessage();
                return RedirectToAction("Index");
            }

            AddListsToModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Archive(Int32 id)
        {
            SearchPhrase searchPhrase = db.SearchPhrases.Find(id);
            if (searchPhrase == null)
            {
                return HttpNotFound();
            }

            searchPhrase.Archive(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Edit", "SearchPhrase", new { Id = searchPhrase.SearchPhraseId });
        }

        [HttpPost]
        public ActionResult Unarchive(Int32 id)
        {
            SearchPhrase searchPhrase = db.SearchPhrases.Find(id);
            if (searchPhrase == null)
            {
                return HttpNotFound();
            }

            searchPhrase.Unarchive(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Edit", "SearchPhrase", new { Id = searchPhrase.SearchPhraseId });
        }

        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            SearchPhrase searchPhrase = db.SearchPhrases.Find(id);
            if (searchPhrase == null)
            {
                return HttpNotFound();
            }

            searchPhrase.Delete(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public NewtonsoftJsonResult MoveUp(Int32 id)
        {
            SearchPhrase searchPhrase = db.SearchPhrases.Find(id);
            if (searchPhrase == null)
            {
                return Json(new { Success = 0, Message = AppGlobal.Language.GetText(this, "UnableToMoveUp", "Unable to move search phrase") });
            }

            if (searchPhrase.Ordinal > 1)
            {
                SearchPhrase swapPhrase = db.SearchPhrases.Where(x => x.Ordinal < searchPhrase.Ordinal).OrderByDescending(x => x.Ordinal).FirstOrDefault();
                if (swapPhrase != null)
                {
                    Int32 swapOrdinal = swapPhrase.Ordinal;
                    swapPhrase.Ordinal = searchPhrase.Ordinal;
                    searchPhrase.Ordinal = swapOrdinal;

                    db.Entry(searchPhrase).State = System.Data.Entity.EntityState.Modified;
                    db.Entry(swapPhrase).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();

                    return Json(new { Success = 1, Message = String.Format(AppGlobal.Language.GetText(this, "MovedUpToX", "Moved search phrase up to number: {0}"), swapOrdinal) });
                }
            }

            return Json(new { Success = 0, Message = AppGlobal.Language.GetText(this, "UnableToMoveUp", "Unable to move search phrase") });
        }

        [HttpPost]
        public NewtonsoftJsonResult MoveDown(Int32 id)
        {
            SearchPhrase searchPhrase = db.SearchPhrases.Find(id);
            if (searchPhrase == null)
            {
                return Json(new { Success = 0, Message = AppGlobal.Language.GetText(this, "UnableToMoveUp", "Unable to move search phrase") });
            }

            SearchPhrase swapPhrase = db.SearchPhrases.Where(x => x.Ordinal > searchPhrase.Ordinal).OrderBy(x => x.Ordinal).FirstOrDefault();
            if (swapPhrase != null)
            {
                Int32 swapOrdinal = swapPhrase.Ordinal;
                swapPhrase.Ordinal = searchPhrase.Ordinal;
                searchPhrase.Ordinal = swapOrdinal;

                db.Entry(searchPhrase).State = System.Data.Entity.EntityState.Modified;
                db.Entry(swapPhrase).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return Json(new { Success = 1, Message = String.Format(AppGlobal.Language.GetText(this, "MovedDownToX", "Moved search phrase down to number: {0}"), swapOrdinal) });
            }

            return Json(new { Success = 0, Message = AppGlobal.Language.GetText(this, "UnableToMoveUp", "Unable to move search phrase") });
        }

        [NonAction]
        private void AddListsToModel(AddEditSearchPhraseModel model)
        {
            model.QualificationLevels = db.QualificationLevels.OrderBy(x => x.DisplayOrder);
            model.StudyModes = db.StudyModes.OrderBy(x => x.DisplayOrder);
            model.AttendanceTypes = db.AttendanceTypes.OrderBy(x => x.DisplayOrder);
            model.AttendancePatterns = db.AttendancePatterns.OrderBy(x => x.DisplayOrder);
        }
    }
}