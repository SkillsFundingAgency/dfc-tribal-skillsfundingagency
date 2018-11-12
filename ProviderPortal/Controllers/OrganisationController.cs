using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Models;
using Tribal.SkillsFundingAgency.ProviderPortal;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;
using TribalTechnology.InformationManagement.Net.Mail;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public class OrganisationController : BaseController
    {
        [PermissionAuthorize(Permission.PermissionName.CanViewOrganisationHomePage)]
        // GET: /Organisation/Index
        public ActionResult Index()
        {
            if (userContext.ContextName == UserContext.UserContextName.DeletedOrganisation)
            {
                return Permission.HasPermission(false, true, Permission.PermissionName.CanEditOrganisation)
                    ? RedirectToAction("Edit")
                    : RedirectToAction("Details");
            }

            return RedirectToActionPermanent("Dashboard");
        }

        [PermissionAuthorize(Permission.PermissionName.CanViewOrganisationHomePage)]
        [ContextAuthorize(UserContext.UserContextName.Organisation)]
        // GET: /Organisation/Dashboard
        public async Task<ActionResult> Dashboard()
        {
            Organisation organisation = await db.Organisations.FindAsync(userContext.ItemId);
            if (organisation == null)
            {
                return HttpNotFound();
            }

            var model = new OrganisationDashboardViewModel(organisation);
            return View(model);
        }

        [PermissionAuthorize(Permission.PermissionName.CanViewOrganisationHomePage)]
        [ContextAuthorize(UserContext.UserContextName.Organisation)]
        [HttpPost]
        // POST: /Organisation/Dashboard
        public async Task<ActionResult> Dashboard(OrganisationMembershipActionViewModel model)
        {
            Organisation organisation = await db.Organisations.FindAsync(userContext.ItemId);
            if (organisation == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                OrganisationProvider item =
                    organisation.OrganisationProviders.FirstOrDefault(x => x.ProviderId == model.Id);

                if (model.Action != "switchto"
                    && !Permission.HasPermission(false, true,
                        Permission.PermissionName.CanManageOrganisationProviderMembership))
                {
                    model.Action = "notpermitted";
                }

                switch (model.Action)
                {
                    case "invite":

                        if (item != null)
                        {
                            ViewBag.Message = AppGlobal.Language.GetText(this, "AlreadyAssociated",
                                "This provider is already associated with your organisation.");
                            break;
                        }

                        OrganisationProvider op = db.OrganisationProviders.Create();
                        op.OrganisationId = organisation.OrganisationId;
                        op.ProviderId = model.Id;
                        op.IsAccepted = false;
                        op.IsRejected = false;
                        op.CanOrganisationEditProvider = false;
                        op.Reason = null;
                        organisation.OrganisationProviders.Add(op);

                        ProvisionUtilities.SendProviderMembershipEmail(
                            db,
                            Constants.EmailTemplates.ProviderInviteNotification,
                            model.Id,
                            userContext.ItemId.Value,
                            null);
                        ShowGenericSavedMessage();
                        break;

                    case "remove":

                        ProvisionUtilities.SendProviderMembershipEmail(
                            db,
                            Constants.EmailTemplates.ProviderRemovedFromOrganisation,
                            model.Id,
                            userContext.ItemId.Value,
                            null);

                        db.OrganisationProviders.Remove(item);
                        ShowGenericSavedMessage();
                        break;

                    case "withdraw":

                        ProvisionUtilities.SendProviderMembershipEmail(
                            db,
                            Constants.EmailTemplates.ProviderInviteWithdrawn,
                            model.Id,
                            userContext.ItemId.Value,
                            null);

                        db.OrganisationProviders.Remove(item);
                        ShowGenericSavedMessage();
                        break;

                    case "switchto":

                        if (Permission.HasPermission(false, true, Permission.PermissionName.CanViewAdministratorHomePage))
                        {
                            new RecentProvisions(Permission.GetCurrentUserId()).Add("P" + model.Id, model.Name);
                        }

                        bool success = item != null
                                       && item.CanOrganisationEditProvider
                                       && UserContext.SetUserContext(db, UserContext.UserContextName.Provider, model.Id);
                        if (!success)
                        {
                            ViewBag.Message = AppGlobal.Language.GetText(this, "UnableToSwitch",
                                "Unable to switch to the selected provider.");
                        }
                        else
                        {
                            return RedirectToAction("Dashboard", "Provider");
                        }
                        break;

                    default:

                        ViewBag.Message = AppGlobal.Language.GetText(this, "NotPermitted",
                            "Unable to perform that action.");
                        break;
                }

                await db.SaveChangesAsync();
            }

            // Something happened so return the model
            var viewModel = new OrganisationDashboardViewModel(organisation);
            return View(viewModel);
        }

        // GET: /Organisation/Membership
        [PermissionAuthorize(Permission.PermissionName.CanManageProviderOrganisationMembership)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public async Task<ActionResult> Membership()
        {
            Provider provider = await db.Providers.FindAsync(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            var model = new ProviderOrganisationsViewModel(provider);
            return View(model);
        }

        // POST: /Organisation/Membership
        [PermissionAuthorize(Permission.PermissionName.CanManageProviderOrganisationMembership)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Membership(OrganisationMembershipActionViewModel model)
        {
            Provider provider = await db.Providers.FindAsync(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                OrganisationProvider item =
                    provider.OrganisationProviders.FirstOrDefault(x => x.OrganisationId == model.Id);
                if (item == null)
                {
                    return HttpNotFound();
                }

                switch (model.Action)
                {
                    case "accept":
                        item.IsAccepted = true;
                        item.IsRejected = false;
                        item.CanOrganisationEditProvider = model.Flag.HasValue && !model.Flag.Value;
                        item.RespondedByUserId = Permission.GetCurrentUserId();
                        item.RespondedByDateTimeUtc = DateTime.UtcNow;

                        ProvisionUtilities.SendOrganisationMembershipEmail(
                            db,
                            item.CanOrganisationEditProvider
                                ? Constants.EmailTemplates.ProviderInviteAcceptedCanEdit
                                : Constants.EmailTemplates.ProviderInviteAcceptedCannotEdit,
                            userContext.ItemId.Value,
                            model.Id,
                            null);
                        ShowGenericSavedMessage();
                        break;

                    case "reject":
                        item.IsAccepted = false;
                        item.IsRejected = true;
                        item.Reason = model.Reason;
                        item.RespondedByUserId = Permission.GetCurrentUserId();
                        item.RespondedByDateTimeUtc = DateTime.UtcNow;

                        ProvisionUtilities.SendOrganisationMembershipEmail(
                            db,
                            Constants.EmailTemplates.ProviderInviteRejected,
                            userContext.ItemId.Value,
                            model.Id,
                            new List<EmailParameter>
                            {
                                new EmailParameter("%REASONS%", model.Reason)
                            });
                        ShowGenericSavedMessage();
                        break;

                    case "leave":
                        item.IsAccepted = true;
                        item.IsRejected = true;
                        item.Reason = model.Reason;
                        item.RespondedByUserId = Permission.GetCurrentUserId();
                        item.RespondedByDateTimeUtc = DateTime.UtcNow;

                        ProvisionUtilities.SendOrganisationMembershipEmail(
                            db,
                            Constants.EmailTemplates.ProviderLeftOrganisation,
                            userContext.ItemId.Value,
                            model.Id,
                            new List<EmailParameter>
                            {
                                new EmailParameter("%REASONS%", model.Reason)
                            });
                        ShowGenericSavedMessage();
                        break;

                    case "toggleorg":
                        item.CanOrganisationEditProvider = model.Flag.HasValue && model.Flag.Value;

                        ProvisionUtilities.SendOrganisationMembershipEmail(
                            db,
                            item.CanOrganisationEditProvider
                                ? Constants.EmailTemplates.ProviderAllowedOrganisationToManageData
                                : Constants.EmailTemplates.ProviderDisallowedOrganisationToManageData,
                            userContext.ItemId.Value,
                            model.Id,
                            null);
                        ShowGenericSavedMessage();
                        break;

                    default:
                        ViewBag.Message = AppGlobal.Language.GetText(this, "NotPermitted",
                            "Unable to perform that action.");
                        break;
                }

                await db.SaveChangesAsync();
            }

            var viewModel = new ProviderOrganisationsViewModel(provider);
            return View(viewModel);
        }

        // GEt: /Organisation/Create
        [PermissionAuthorize(Permission.PermissionName.CanAddOrganisation)]
        [ContextAuthorize(UserContext.UserContextName.AdministrationProvider)]
        public ActionResult Create()
        {
            var model = new AddEditOrganisationModel();
            model.Address.Populate(db);

            // Populate the drop downs
            GetLookups(model);

            return View(model);
        }

        // POST: /Organisation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanAddOrganisation)]
        [ContextAuthorize(UserContext.UserContextName.AdministrationProvider)]
        public async Task<ActionResult> Create(AddEditOrganisationModel model)
        {
            if (ModelState.IsValid)
            {
                Ukrlp ukrlp = db.Ukrlps.Find(model.UKPRN);
                if (ukrlp == null)
                {
                    // Don't tie error message to field as this will happen anyway when the script looks it up
                    ModelState.AddModelError("", AppGlobal.Language.GetText(this, "UKRLPNotFound", "UKRLP Not Found"));
                }
                else if (ukrlp.UkrlpStatus.HasValue && ukrlp.UkrlpStatus.Value == (Int32) Constants.RecordStatus.Deleted)
                {
                    // Don't tie error message to field as this will happen anyway when the script looks it up
                    ModelState.AddModelError("",
                        AppGlobal.Language.GetText(this, "UKRLPDeactived", "UKRLP Has Been Deactivated"));
                }

                if (ProvisionUtilities.IsNameInUse(db, model.OrganisationName, null, null))
                {
                    ModelState.AddModelError("OrganisationName",
                        AppGlobal.Language.GetText(this, "NameInUse",
                            "This Organisation name already exists in the database, please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk if you need any help."));
                }

                if (ModelState.IsValid)
                {
                    Organisation organisation = model.ToEntity(db);
                    organisation.RecordStatu = db.RecordStatus.Find((int) Constants.RecordStatus.Live);
                    Address providerAddress = model.Address.ToEntity(db);
                    organisation.Address = providerAddress;
                    organisation.Address.ProviderRegionId = null;
                    if (userContext.IsProvider())
                    {
                        organisation.OrganisationProviders.Add(new OrganisationProvider
                        {
                            ProviderId = userContext.ItemId.Value,
                            IsAccepted = false,
                            IsRejected = false,
                        });
                    }
                    db.Addresses.Add(providerAddress);
                    db.Organisations.Add(organisation);
                    await db.SaveChangesAsync();
                    ShowGenericSavedMessage();
                    ProvisionUtilities.SendNewOrganisationEmail(organisation);
                    return RedirectToAction("Index", "Home");
                }
            }

            // Populate the drop downs
            GetLookups(model);

            return View(model);
        }

        // GET: /Organisation/Edit
        [PermissionAuthorize(Permission.PermissionName.CanEditOrganisation)]
        [ContextAuthorize(
            new[] {UserContext.UserContextName.Organisation, UserContext.UserContextName.DeletedOrganisation})]
        public async Task<ActionResult> Edit()
        {
            //if (userContext.ContextName != UserContext.UserContextName.Organisation)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            Organisation organisation = await db.Organisations.FindAsync(userContext.ItemId);
            if (organisation == null)
            {
                return HttpNotFound();
            }

            var model = new AddEditOrganisationModel(organisation);
            Ukrlp ukrlp =
                db.Ukrlps.Include("PrimaryAddress")
                    .Include("LegalAddress")
                    .FirstOrDefault(x => x.Ukprn == organisation.UKPRN);
            if (ukrlp != null)
            {
                model.UKRLPData = new UKRLPDataModel(ukrlp);
            }

            // Populate the dropdowns
            GetLookups(model);

            return View(model);
        }

        // POST: /Organisation/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanEditOrganisation)]
        [ContextAuthorize(
            new[] {UserContext.UserContextName.Organisation, UserContext.UserContextName.DeletedOrganisation})]
        public async Task<ActionResult> Edit(AddEditOrganisationModel model)
        {
            if (Permission.HasPermission(false, true, Permission.PermissionName.CanEditProviderSpecialFields))
            {
                Ukrlp ukrlp = db.Ukrlps.Find(model.UKPRN);
                if (ukrlp == null)
                {
                    // Don't tie error message to field as this will happen anyway when the script looks it up
                    ModelState.AddModelError("", AppGlobal.Language.GetText(this, "UKRLPNotFound", "UKRLP Not Found"));
                }
                else if (ukrlp.UkrlpStatus.HasValue && ukrlp.UkrlpStatus.Value == (Int32) Constants.RecordStatus.Deleted &&
                         model.RecordStatusId.HasValue &&
                         model.RecordStatusId.Value != (Int32) Constants.RecordStatus.Deleted)
                {
                    // Don't tie error message to field as this will happen anyway when the script looks it up
                    ModelState.AddModelError("",
                        AppGlobal.Language.GetText(this, "UKRLPDeactived", "UKRLP Has Been Deactivated"));
                }
            }

            if (ProvisionUtilities.IsNameInUse(db, model.OrganisationName, null, model.OrganisationId))
            {
                ModelState.AddModelError("OrganisationName",
                    AppGlobal.Language.GetText(this, "NameInUse",
                        "This Organisation name already exists in the database, please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk if you need any help."));
            }

            if (ModelState.IsValid)
            {
                Organisation organisation = model.ToEntity(db);
                if (organisation == null || organisation.OrganisationId != userContext.ItemId)
                {
                    return HttpNotFound();
                }

                bool recordStatuschanged = model.RecordStatusId != organisation.RecordStatusId;
                if (
                    !(model.RecordStatusId == (int) Constants.RecordStatus.Live ||
                      model.RecordStatusId == (int) Constants.RecordStatus.Deleted))
                {
                    ModelState.AddModelError(
                        "RecordStatusId",
                        AppGlobal.Language.GetText("RecordStatusNotFound", "Status Is Invalid"));
                }

                if (recordStatuschanged
                    && model.RecordStatusId == (int) Constants.RecordStatus.Deleted
                    && organisation.OrganisationProviders.Any(x => x.IsAccepted && !x.IsRejected))
                {
                    ModelState.AddModelError(
                        "RecordStatusId",
                        AppGlobal.Language.GetText(this, "CannotDeleteActiveOrganisations",
                            "You may not delete this Organisation until all Providers have left or been removed."));
                }

                if (ModelState.IsValid)
                {
                    if (Permission.HasPermission(false, true, Permission.PermissionName.CanEditProviderSpecialFields))
                    {
                        organisation.UKPRN = model.UKPRN.HasValue ? model.UKPRN.Value : 0;
                        organisation.QualityEmailsPaused = model.QualityEmailsPaused;
                        organisation.QualityEmailStatusId = model.QualityEmailStatusId;
                    }

                    organisation.ModifiedByUserId = Permission.GetCurrentUserId();
                    organisation.ModifiedDateTimeUtc = DateTime.UtcNow;
                    // ReSharper disable once PossibleInvalidOperationException
                    organisation.RecordStatusId = model.RecordStatusId.Value;
                    Address organisationAddress = model.Address.ToEntity(db);

                    db.Entry(organisationAddress).State = EntityState.Modified;
                    db.Entry(organisation).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    // Reset the user context if the status has changed
                    if (recordStatuschanged)
                    {
                        UserContext.SetUserContext(db, model.RecordStatusId == (int) Constants.RecordStatus.Deleted
                            ? UserContext.UserContextName.DeletedOrganisation
                            : UserContext.UserContextName.Organisation, userContext.ItemId, true);
                    }
                    ShowGenericSavedMessage();
                    return RedirectToAction("Index", "Organisation");
                }
            }

            // Populate the dropdowns
            GetLookups(model);

            return View(model);
        }

        // POST: /Organisation/Delete
        [PermissionAuthorize(Permission.PermissionName.CanDeleteOrganisation)]
        [ContextAuthorize(UserContext.UserContextName.Organisation)]
        public ActionResult Delete()
        {
            if (userContext.ItemId == null)
            {
                return HttpNotFound();
            }
            var model = new DeleteOrganisationViewModel(userContext.ItemId.Value);
            model.Populate(db);
            return View(model);
        }


        // POST: /Organisation/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanDeleteOrganisation)]
        [ContextAuthorize(UserContext.UserContextName.Organisation)]
        public async Task<ActionResult> Delete(DeleteOrganisationViewModel model)
        {
            Organisation organisation = await db.Organisations.FindAsync(userContext.ItemId);
            if (organisation.OrganisationProviders.Any(x => x.IsAccepted && !x.IsRejected))
            {
                return Delete();
            }

            organisation.RecordStatusId = (int) Constants.RecordStatus.Deleted;
            await db.SaveChangesAsync();

            // Reset the user's current context and find out what it is
            UserContext.UserContextInfo defaultContext = UserContext.InstantiateSession();
            if (defaultContext.ContextName != UserContext.UserContextName.Administration)
            {
                IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignOut();
                SessionManager.End();
            }
            ShowGenericSavedMessage();
            return RedirectToAction("Index", "Home");
        }

        [ContextAuthorize(UserContext.UserContextName.ProviderOrganisation)]
        [PermissionAuthorize(Permission.PermissionName.CanViewOrganisation)]
        public async Task<ActionResult> Details(int? id)
        {
            if (userContext.IsOrganisation())
            {
                id = id == userContext.ItemId || id == null ? userContext.ItemId : null;
            }

            if (id == null)
            {
                return HttpNotFound();
            }

            var organisation = await db.Organisations.FindAsync(id);
            if (organisation == null || (userContext.IsProvider()
                                         &&
                                         !organisation.OrganisationProviders.Any(
                                             x =>
                                                 x.OrganisationId == id && x.ProviderId == userContext.ItemId &&
                                                 !x.IsRejected)))
            {
                return HttpNotFound();
            }

            var model = new AddEditOrganisationModel(organisation, true);
            model.Populate(db);
            Ukrlp ukrlp =
                db.Ukrlps.Include("PrimaryAddress")
                    .Include("LegalAddress")
                    .FirstOrDefault(x => x.Ukprn == organisation.UKPRN);
            if (ukrlp != null)
            {
                model.UKRLPData = new UKRLPDataModel(ukrlp);
            }

            return View(model);
        }

        [NonAction]
        private void GetLookups(AddEditOrganisationModel model)
        {
            model.Address.Populate(db);
            ViewBag.OrganisationTypes = new SelectList(
                db.OrganisationTypes,
                "OrganisationTypeId",
                "OrganisationTypeName",
                model.OrganisationTypeId);
            ViewBag.RecordStatuses = new SelectList(
                db.RecordStatus.Where(
                    x =>
                        x.RecordStatusId == (int) Constants.RecordStatus.Live ||
                        x.RecordStatusId == (int) Constants.RecordStatus.Deleted),
                "RecordStatusId",
                "RecordStatusName",
                model.RecordStatusId);
            ViewBag.QualityEmailStatuses = new SelectList(
                db.QualityEmailStatuses,
                "QualityEmailStatusId",
                "QualityEmailStatusDesc",
                model.QualityEmailStatusId);
        }
    }
}