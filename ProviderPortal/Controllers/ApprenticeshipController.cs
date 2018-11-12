using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public class ApprenticeshipController : BaseController
    {
        //
        // GET: /Apprenticeship
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [PermissionAuthorize(Permission.PermissionName.CanViewProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult List(Int32? locationId)
        {
            var model = new ApprenticeshipListViewModel();
            if (locationId.HasValue)
            {
                if (model.Search == null)
                {
                    model.Search = new ApprenticeshipSearchViewModel();
                }
                model.Search.LocationId = locationId;
            }
            model.Populate(db);
            return View(model);
        }

        [PermissionAuthorize(Permission.PermissionName.CanViewProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        [HttpPost]
        public ActionResult List(ApprenticeshipListViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Populate(db);
            }
            return View(model);
        }

        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Create()
        {
            var model = new AddEditApprenticeshipViewModel().Populate(db);
            ModelState.Clear();
            return View(model);
        }

        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        [HttpPost]
        public ActionResult Create(AddEditApprenticeshipViewModel model)
        {
            model.ApprenticeshipId = 0;
            RemoveSpellCheckHTMLFromMarketingInformation(model);
            model.Validate(db, ModelState);
            if (ModelState.IsValid)
            {
                var apprenticeship = model.ToEntity(db);
                db.Apprenticeships.Add(apprenticeship);
                db.SaveChanges();
                
                List<String> messages = model.GetWarningMessages(db);
                if (messages.Count == 0)
                {
                    ShowGenericSavedMessage();
                }
                else
                {
                    // Add a blank entry at the beginning so the String.Join starts with <br /><br />
                    messages.Insert(0, "");
                    SessionMessage.SetMessage(AppGlobal.Language.GetText(this, "SaveSuccessfulWithWarnings", "Your changes were saved successfully with the following warnings:") + String.Join("<br /><br />", messages), SessionMessageType.Success);
                }

                return Request.Form["Create"] != null
                    ? RedirectToAction("List")
                    : RedirectToAction("Create", "DeliveryLocation", new {id = apprenticeship.ApprenticeshipId});
            }
            return View(model);
        }

        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Edit(Int32? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Create");
            }

            if (!db.Providers.Any(x => x.ProviderId == userContext.ItemId.Value)
                || !db.Apprenticeships.Any(x => x.ApprenticeshipId == id && x.ProviderId == userContext.ItemId.Value))
            {
                return HttpNotFound();
            }

            var model = new AddEditApprenticeshipViewModel();
            model = model.Populate(id.Value, db);
            return View(model);
        }

        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        [HttpPost]
        public ActionResult Edit(Int32 id, AddEditApprenticeshipViewModel model)
        {
            if (!db.Providers.Any(x => x.ProviderId == userContext.ItemId.Value)
                || !db.Apprenticeships.Any(x => x.ApprenticeshipId == id && x.ProviderId == userContext.ItemId.Value)
                || model.ApprenticeshipId != id)
            {
                return HttpNotFound();
            }

            RemoveSpellCheckHTMLFromMarketingInformation(model);

            model.Validate(db, ModelState);
            if (ModelState.IsValid)
            {
                model.ToEntity(db);
                db.SaveChanges();

                List<String> messages = model.GetWarningMessages(db);
                if (messages.Count == 0)
                {
                    ShowGenericSavedMessage();
                }
                else
                {
                    // Add a blank entry at the beginning so the String.Join starts with <br /><br />
                    messages.Insert(0, "");
                    SessionMessage.SetMessage(AppGlobal.Language.GetText(this, "SaveSuccessfulWithWarnings", "Your changes were saved successfully with the following warnings:") + String.Join("<br /><br />", messages), SessionMessageType.Success);
                }
                
                return RedirectToAction("List");
            }
            var deliveryLocations = new DeliveryLocationListViewModel();
            model.DeliveryLocations = deliveryLocations.Populate(id, db);
            return View(model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderApprenticeship, Permission.PermissionName.CanEditProviderApprenticeship)]
        public ActionResult View(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Apprenticeship apprenticeship = db.Apprenticeships.FirstOrDefault(x => x.ApprenticeshipId == id);
            if (apprenticeship == null || apprenticeship.ProviderId != userContext.ItemId)
            {
                return HttpNotFound();
            }

            ViewApprenticeshipModel model = new ViewApprenticeshipModel(apprenticeship);

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public NewtonsoftJsonResult ArchiveSelected(String apprenticeshipIds)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                throw new Exception("");
            }

            var archivedIds = new List<Int32>();
            foreach (String id in apprenticeshipIds.Split(','))
            {
                Int32 apprenticeshipId;
                if (Int32.TryParse(id, out apprenticeshipId))
                {
                    var apprenticeship = db.Apprenticeships.Find(apprenticeshipId);
                    if (apprenticeship.ProviderId == userContext.ItemId &&
                        apprenticeship.RecordStatusId != (Int32) Constants.RecordStatus.Archived)
                    {
                        apprenticeship.Archive(db);
                        archivedIds.Add(apprenticeshipId);
                    }
                }
            }
            if (archivedIds.Count > 0)
            {
                db.SaveChanges();
            }

            return Json(archivedIds.ToArray());
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public NewtonsoftJsonResult UnarchiveSelected(String apprenticeshipIds)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                throw new Exception("");
            }

            var unarchivedIds = new List<Int32>();
            foreach (String id in apprenticeshipIds.Split(','))
            {
                Int32 apprenticeshipId;
                if (Int32.TryParse(id, out apprenticeshipId))
                {
                    var apprenticeship = db.Apprenticeships.Find(apprenticeshipId);
                    if (apprenticeship.ProviderId == userContext.ItemId &&
                        apprenticeship.RecordStatusId == (Int32) Constants.RecordStatus.Archived)
                    {
                        apprenticeship.Unarchive(db);
                        unarchivedIds.Add(apprenticeshipId);
                    }
                }
            }
            if (unarchivedIds.Count > 0)
            {
                db.SaveChanges();
            }

            return Json(unarchivedIds.ToArray());
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Archive(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Apprenticeship apprenticeship = db.Apprenticeships.Find(id);
            if (apprenticeship == null || apprenticeship.ProviderId != userContext.ItemId ||
                apprenticeship.RecordStatusId == (Int32) Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            apprenticeship.Archive(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Edit", "Apprenticeship", new {Id = apprenticeship.ApprenticeshipId});
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Unarchive(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Apprenticeship apprenticeship = db.Apprenticeships.Find(id);
            if (apprenticeship == null || apprenticeship.ProviderId != userContext.ItemId ||
                apprenticeship.RecordStatusId == (Int32) Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            apprenticeship.Unarchive(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Edit", "Apprenticeship", new {Id = apprenticeship.ApprenticeshipId});
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Delete(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Apprenticeship apprenticeship = db.Apprenticeships.Find(id);
            if (apprenticeship == null || apprenticeship.ProviderId != userContext.ItemId ||
                apprenticeship.RecordStatusId == (Int32) Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            apprenticeship.Delete(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);
            
            return RedirectToAction("List");
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanQAApprenticeships)]
        public ActionResult QAForComplianceFromDialog(Int32 apprenticeshipId)
        {
            if (userContext.ContextName != UserContext.UserContextName.Provider)
            {
                return HttpNotFound();
            }

            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Apprenticeship apprenticeship = db.Apprenticeships.Find(apprenticeshipId);
            if (apprenticeship == null || apprenticeship.ProviderId != provider.ProviderId)
            {
                return HttpNotFound();
            }

            AddEditApprenticeshipQAForComplianceModel model = new AddEditApprenticeshipQAForComplianceModel
            {
                ApprenticeshipId = apprenticeshipId
            };

            // Get Lookups
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanQAApprenticeships)]
        public ActionResult QAForComplianceFromDialog(AddEditApprenticeshipQAForComplianceModel model)
        {
            if (userContext.ContextName != UserContext.UserContextName.Provider)
            {
                return HttpNotFound();
            }

            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Apprenticeship apprenticeship = db.Apprenticeships.Find(model.ApprenticeshipId);
            if (apprenticeship == null || apprenticeship.ProviderId != provider.ProviderId)
            {
                return HttpNotFound();
            }

            // If passed compliance checks is No then failure reasons should be mandatory
            if (model.Passed == "0" && model.SelectedComplianceFailureReasons.Count == 0)
            {
                ModelState.AddModelError("SelectedComplianceFailureReasons", AppGlobal.Language.GetText(this, "SelectedComplianceFailureReasonsMandatory", "Please select a reason for failure"));
            }
            else if (model.Passed == "1" && model.SelectedComplianceFailureReasons.Count > 0)
            {
                ModelState.AddModelError("Passed", AppGlobal.Language.GetText(this, "CannotPassComplianceQAWithFailureReasons", "Passed compliance checks should only be Yes when no failure reasons have been selected"));
            }

            if (ModelState.IsValid)
            {
                ApprenticeshipQACompliance QA = model.ToEntity(db);
                apprenticeship.ApprenticeshipQACompliances.Add(QA);
                db.Entry(apprenticeship).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new ApprenticeshipQAForComplianceJsonModel(QA));
            }            

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanQAApprenticeships)]
        public ActionResult QAForStyleFromDialog(Int32 apprenticeshipId)
        {
            if (userContext.ContextName != UserContext.UserContextName.Provider)
            {
                return HttpNotFound();
            }

            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Apprenticeship apprenticeship = db.Apprenticeships.Find(apprenticeshipId);
            if (apprenticeship == null || apprenticeship.ProviderId != provider.ProviderId)
            {
                return HttpNotFound();
            }

            AddEditApprenticeshipQAForStyleModel model = new AddEditApprenticeshipQAForStyleModel
            {
                ApprenticeshipId = apprenticeshipId
            };

            // Get Lookups
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanQAApprenticeships)]
        public ActionResult QAForStyleFromDialog(AddEditApprenticeshipQAForStyleModel model)
        {
            if (userContext.ContextName != UserContext.UserContextName.Provider)
            {
                return HttpNotFound();
            }

            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Apprenticeship apprenticeship = db.Apprenticeships.Find(model.ApprenticeshipId);
            if (apprenticeship == null || apprenticeship.ProviderId != provider.ProviderId)
            {
                return HttpNotFound();
            }

            // If passed compliance checks is No then failure reasons should be mandatory
            if (model.Passed == "0" && model.SelectedStyleFailureReasons.Count == 0)
            {
                ModelState.AddModelError("SelectedStyleFailureReasons", AppGlobal.Language.GetText(this, "SelectedStyleFailureReasonsMandatory", "Please select a reason for failure"));
            }
            else if (model.Passed == "1" && model.SelectedStyleFailureReasons.Count > 0)
            {
                ModelState.AddModelError("Passed", AppGlobal.Language.GetText(this, "CannotPassQAWithFailureReasons", "Passed style checks should only be Yes when no failure reasons have been selected"));
            }

            if (ModelState.IsValid)
            {
                ApprenticeshipQAStyle QA = model.ToEntity(db);
                apprenticeship.ApprenticeshipQAStyles.Add(QA);
                db.Entry(apprenticeship).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new ApprenticeshipQAForStyleJsonModel(QA));
            }

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public NewtonsoftJsonResult IsRegulated(string selectedStandard)
        {
            var decodedValues = ApprenticeshipExtensions.DecodeSearchFrameworkOrStandard(selectedStandard);
            if (decodedValues.StandardCode.HasValue)
            {
                var standard = db.Standards.FirstOrDefault(s => s.StandardCode == decodedValues.StandardCode.Value);
                var isRegulated = standard == null || standard.OtherBodyApprovalRequired == null || standard.OtherBodyApprovalRequired.Trim().ToUpper() != "Y" ? false : true;
                return Json(isRegulated);
            }
            return Json(false);
        }

        [NonAction]
        private void GetLookups(AddEditApprenticeshipQAForComplianceModel model)
        {
            model.QAComplianceFailureReasons = db.QAComplianceFailureReasons.Where(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live).ToList();

            List<SelectListItem> selectListItems = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = AppGlobal.Language.GetText(this, "Yes", "Yes")},
                new SelectListItem {Value = "0", Text = AppGlobal.Language.GetText(this, "No", "No")},
            };

            ViewBag.YesNo = new SelectList(selectListItems, "Value", "Text", !String.IsNullOrEmpty(model.Passed) ? null : model.Passed == "1" ? "Yes" : "No");
        }

        [NonAction]
        private void GetLookups(AddEditApprenticeshipQAForStyleModel model)
        {
            model.QAStyleFailureReasons = db.QAStyleFailureReasons.Where(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live).ToList();

            List<SelectListItem> selectListItems = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = AppGlobal.Language.GetText(this, "Yes", "Yes")},
                new SelectListItem {Value = "0", Text = AppGlobal.Language.GetText(this, "No", "No")},
            };

            ViewBag.YesNo = new SelectList(selectListItems, "Value", "Text", !String.IsNullOrEmpty(model.Passed) ? null : model.Passed == "1" ? "Yes" : "No");
        }

        [NonAction]
        public void RemoveSpellCheckHTMLFromMarketingInformation(AddEditApprenticeshipViewModel model)
        {
            model.MarketingInformation = Markdown.Sanitize(model.MarketingInformation);
            if (!ModelState.IsValidField("MarketingInformation") && model.MarketingInformation.Length <= AddEditApprenticeshipViewModel.MarketingInformationMaxLength)
            {
                // These 2 strings should match the corresponding strings in AddEditApprenticeshipViewModel (especially where language is set to developer mode)
                String errorMessage = String.Format(AppGlobal.Language.GetText("AddEditApprenticeshipViewModel_StringLength_MarketingInformation", "The maximum length of {0} is 750 characters."), AppGlobal.Language.GetText("AddEditApprenticeshipViewModel_DisplayName_MarketingInformation", "Your Apprenticeship Information for Employers"));
                foreach (ModelError me in ModelState["MarketingInformation"].Errors)
                {
                    if (me.ErrorMessage == errorMessage)
                    {
                        ModelState["MarketingInformation"].Errors.Remove(me);
                        break;
                    }
                }
                // If there are no more marketing information errors then remove the key altogether
                if (ModelState["MarketingInformation"].Errors.Count == 0)
                {
                    ModelState.Remove("MarketingInformation");
                }
            }
        }

    }
}