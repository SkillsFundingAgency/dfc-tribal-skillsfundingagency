using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Controllers;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    [ContextAuthorize(UserContext.UserContextName.Administration)]
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/
        [PermissionAuthorize(Permission.PermissionName.CanViewAdministratorHomePage)]
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Dashboard");
        }

        //
        // GET: /Admin/Dashboard
        [PermissionAuthorize(Permission.PermissionName.CanViewAdministratorHomePage)]
        public ActionResult Dashboard()
        {
            var model = new AdminDashboardViewModel();
            model.Populate(db);
            return View(model);
        }

        //
        // POST: /Admin/Dashboard
        [PermissionAuthorize(Permission.PermissionName.CanViewAdministratorHomePage)]
        [HttpPost]
        public ActionResult Dashboard(AdminDashboardViewModel model)
        {
            if (ModelState.IsValidField("ProviderName") && !ModelState.IsValidField("ProviderID"))
            {
                string id;
                if (model.Provider != null)
                {
                    model.Provider = model.Provider.Trim();
                }

                if (new TypeaheadController().FindProviderOrOrganisationByName(model.Provider, out id))
                {
                    model.ProviderId = id;
                    ModelState["ProviderId"].Errors.Clear();
                }
            }

            if (ModelState.IsValid)
            {
                var requestedContext = TypeaheadController.DecodeProviderId(model.ProviderId);
                if (requestedContext == null)
                {
                    ModelState.AddModelError("Provider",
                        AppGlobal.Language.GetText(this, "InvalidSelection",
                            "Invalid provider or organisation selected."));
                }
                else
                {
                    var success = UserContext.SetUserContext(db, requestedContext.ContextName, requestedContext.ItemId);
                    if (success)
                    {
                        var userId = Permission.GetCurrentUserId();
                        new RecentProvisions(userId).Add(model.ProviderId, model.Provider);
                        if (!String.IsNullOrEmpty(model.SuccessAction) && !String.IsNullOrEmpty(model.SuccessController))
                        {
                            return RedirectToAction(model.SuccessAction, model.SuccessController);
                        }
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError("Provider",
                        AppGlobal.Language.GetText(this, "UnableToSwitch",
                            "Unable to switch to the selected provider or organisation."));
                }
            }

            if (!String.IsNullOrEmpty(model.FailureAction) && !String.IsNullOrEmpty(model.FailureController))
            {
                return RedirectToAction(model.FailureAction, model.FailureController);
            }

            // Something happened so return the model
            model.Populate(db);
            return View(model);
        }
    }
}