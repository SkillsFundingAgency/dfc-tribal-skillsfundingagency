using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Models;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;
using TribalTechnology.InformationManagement.Net.Mail;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public class ProviderController : BaseController
    {
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderHomePage)]
        // GET: /Provider/Index
        public ActionResult Index()
        {
            if (userContext.ContextName == UserContext.UserContextName.DeletedProvider)
            {
                return Permission.HasPermission(false, true, Permission.PermissionName.CanEditProvider)
                    ? RedirectToAction("Edit")
                    : RedirectToAction("Details");
            }

            return RedirectToActionPermanent("Dashboard");
        }

        [PermissionAuthorize(Permission.PermissionName.CanViewProviderHomePage)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        // GET: /Provider/Dashboard
        public ActionResult Dashboard()
        {
            return RedirectToAction("Dashboard", "Report", null);
            //Provider provider =  db.Providers.Include("Venues.Address").FirstOrDefault(x => x.ProviderId == userContext.ItemId);
            //if (provider == null)
            //{
            //    return HttpNotFound();
            //}

            //ProviderDashboardModel pdm = new ProviderDashboardModel(provider);

            //return View(pdm);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanSetAllCoursesUpToDate)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public NewtonsoftJsonResult ConfirmAllCoursesUpToDate()
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return Json(new { Result = 1, Message = AppGlobal.Language.GetText(this, "UnableToConfirmCoursesProviderNotFound", "Unable to confirm all courses are up to date") });
            }

            Int32 numberOfOutOfDateCourses = ProvisionDataCurrent.GetCountOfCoursesOutOfDate();
            if (numberOfOutOfDateCourses > 0)
            {
                return Json(new {Result = 1, Message = AppGlobal.Language.GetText(this, "UnableToConfirmCoursesOutOfDateCoursesExist", "Unable to confirm all courses are up to date because there are out of date courses")});
            }

            Int32 numberOfCoursesWithExpiredLARS = ProvisionDataCurrent.GetCountOfCoursesWithExpiredLAR();
            if (numberOfCoursesWithExpiredLARS > 0)
            {
                return Json(new { Result = 1, Message = AppGlobal.Language.GetText(this, "UnableToConfirmCoursesCoursesWithExpiredLARSExist", "Unable to confirm all courses are up to date because there courses with expired learning aims") });
            }

            try
            {
                provider.LastAllDataUpToDateTimeUtc = DateTime.UtcNow;
                Provider_AllCoursesOKConfirmations allCourseConfirmations = new Provider_AllCoursesOKConfirmations
                {
                    DateTimeUtc = DateTime.UtcNow,
                    CreatedByUserId = Permission.GetCurrentUserId() 
                };
                db.Entry(allCourseConfirmations).State = EntityState.Added;
                provider.AllCoursesOKConfirmations.Add(allCourseConfirmations);

                db.Entry(provider).State = EntityState.Modified;
                db.SaveChanges();

                // Recalculate Dashboard Stats 
                Int32? oldTimeout = ((IObjectContextAdapter)db).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 540;
                db.up_ProviderUpdateQualityScore(provider.ProviderId, true);
                ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = oldTimeout;

                // Reget Session Information
                QualityIndicator.SetSessionInformation(userContext);
            }
            catch
            {
                return Json(new { Result = 1, Message = AppGlobal.Language.GetText(this, "UnableToConfirmCourses", "Unable to confirm all courses are up to date") });
            }

            return Json(new { Result = 0 });
        }

        [PermissionAuthorize(Permission.PermissionName.CanAddProvider)]
        [ContextAuthorize(new [] {UserContext.UserContextName.Administration, UserContext.UserContextName.Organisation})]
        public ActionResult Create()
        {
            AddEditProviderModel model = new AddEditProviderModel();
            model.Address.Populate(db);

            // Populate the drop downs
            GetLookups(model);

            return View(model);
        }

        // POST: /Provider/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanAddProvider)]
        [ContextAuthorize(new[] { UserContext.UserContextName.Administration, UserContext.UserContextName.Organisation })]
        public async Task<ActionResult> Create(AddEditProviderModel model)
        {
            RemoveSpellCheckHTMLFromMarketingInformation(model);

            if (model.ApprenticeshipContract && String.IsNullOrWhiteSpace(model.MarketingInformation))
            {
                ModelState.AddModelError("MarketingInformation", AppGlobal.Language.GetText(this, "MarketingInformationMandatory", "The Brief overview of your organisation for employers field is required."));
            }

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
                    ModelState.AddModelError("", AppGlobal.Language.GetText(this, "UKRLPDeactived", "UKRLP Has Been Deactivated"));
                }
                if (ProvisionUtilities.IsNameInUse(db, model.ProviderName, null, null))
                {
                    ModelState.AddModelError("ProviderName", AppGlobal.Language.GetText(this, "NameInUse", "This Provider name already exists in the database, please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk if you need any help."));
                }
                if ((model.SFAFunded || model.DFE1619Funded) == false)
                {
                    ModelState.AddModelError("SFAFunded", AppGlobal.Language.GetText(this, "FundingSourceRequired", "Please select whether this provider is funded by the SFA, DfE EFA or both."));
                }
                if (model.SecureAccessId != null)
                {
                    if (db.Providers.Any(
                            p => p.SecureAccessId == model.SecureAccessId && 
                                p.ProviderId != model.ProviderId))
                    {
                        ModelState.AddModelError(
                            "SecureAccessId",
                            AppGlobal.Language.GetText(
                                this,
                                "DuplicateSecureAccessId",
                                "Secure Access ID is already used by another provider."));
                    }
                }
                if (!model.DFE1619Funded)
                {
                    if (model.DfEProviderTypeId != null)
                    {
                        ModelState.AddModelError(
                            "DfEProviderTypeId",
                            AppGlobal.Language.GetText(
                                this,
                                "DfEProviderTypeNotAllowed",
                                "DfE Provider Type is not valid for providers without DfE EFA Funding"));
                    }
                    if (model.DfEProviderStatusId != null)
                    {
                        ModelState.AddModelError(
                            "DfEProviderStatusId",
                            AppGlobal.Language.GetText(
                                this,
                                "DfEProviderStatusNotAllowed",
                                "DfE Provider Status is not valid for providers without DfE EFA Funding"));
                    }
                    if (model.DfELocalAuthorityId != null)
                    {
                        ModelState.AddModelError(
                            "DfELocalAuthorityId",
                            AppGlobal.Language.GetText(
                                this,
                                "DfELocalAuthorityNotAllowed",
                                "DfE Local Authority is not valid for providers without DfE EFA Funding"));
                    }
                    if (model.DfERegionId != null)
                    {
                        ModelState.AddModelError(
                            "DfERegionId",
                            AppGlobal.Language.GetText(
                                this,
                                "DfERegionNotAllowed",
                                "DfE Region is not valid for providers without DfE EFA Funding"));
                    }
                    if (model.DfEEstablishmentTypeId != null)
                    {
                        ModelState.AddModelError(
                            "DfEEstablishmentTypeId",
                            AppGlobal.Language.GetText(
                                this,
                                "DfEEstablishmentTypeNotAllowed",
                                "DfE Establishment Type is not valid for providers without DfE EFA Funding"));
                    }
                }
                if (ModelState.IsValid)
                {
                    Provider provider = model.ToEntity(db);
                    provider.RecordStatu = db.RecordStatus.Find((Int32) Constants.RecordStatus.Live);
                    Address providerAddress = model.Address.ToEntity(db);
                    provider.Address = providerAddress;
                    provider.ProviderRegionId = provider.Address.ProviderRegionId;
                    db.Addresses.Add(providerAddress);

                    // If organisation is creating this provider then we need to add an invitation
                    // for the provider to join the organisation
                    UserContext.UserContextInfo uc = UserContext.GetUserContext();
                    if (uc.ContextName == UserContext.UserContextName.Organisation && uc.ItemId.HasValue)
                    {
                        OrganisationProvider op = new OrganisationProvider
                        {
                            OrganisationId = uc.ItemId.Value,
                            Provider = provider
                        };
                        db.OrganisationProviders.Add(op);
                        provider.OrganisationProviders.Add(op);
                    }

                    db.Providers.Add(provider);

                    await db.SaveChangesAsync();
                    ProvisionUtilities.SendNewProviderEmail(provider);

                    ViewBag.DFEFunded = model.DFE1619Funded;
                    ViewBag.SFAFunded = model.SFAFunded;

                    List<String> messages = model.GetWarningMessages();
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

                    return RedirectToAction("Index", "Home");
                }
            }

            // Populate the drop downs
            GetLookups(model);

            return View(model);
        }

        // GET: /Provider/Edit/5
        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanEditProvider)]
        [ContextAuthorize(new [] {UserContext.UserContextName.Provider, UserContext.UserContextName.DeletedProvider})]
        public async Task<ActionResult> Edit()
        {
            Provider provider = await db.Providers.FindAsync(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            AddEditProviderModel model = new AddEditProviderModel(provider);
            Ukrlp ukrlp = db.Ukrlps.Include("PrimaryAddress").Include("LegalAddress").FirstOrDefault(x => x.Ukprn == provider.Ukprn);
            if (ukrlp != null)
            {
                model.UKRLPData = new UKRLPDataModel(ukrlp);
            }

            // Populate the dropdowns
            GetLookups(model);

            return View(model);
        }

        // POST: /Provider/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanEditProvider)]
        [ContextAuthorize(new[] { UserContext.UserContextName.Provider, UserContext.UserContextName.DeletedProvider })]
        public async Task<ActionResult> Edit(AddEditProviderModel model)
        {
            Provider provider = null;

            RemoveSpellCheckHTMLFromMarketingInformation(model);

            if (model.ApprenticeshipContract && String.IsNullOrWhiteSpace(model.MarketingInformation))
            {
                ModelState.AddModelError("MarketingInformation", AppGlobal.Language.GetText(this, "MarketingInformationMandatory", "The Brief overview of your organisation for employers field is required."));
            }

            if (ModelState.IsValid)
            {
                provider = model.ToEntity(db);
                if (provider == null || provider.ProviderId != userContext.ItemId)
                {
                    return HttpNotFound();
                }

                bool recordStatuschanged = model.RecordStatusId != provider.RecordStatusId;
                if (!(model.RecordStatusId == (int)Constants.RecordStatus.Live || model.RecordStatusId == (int)Constants.RecordStatus.Deleted))
                {
                    ModelState.AddModelError(
                        "RecordStatusId",
                        AppGlobal.Language.GetText("RecordStatusNotFound", "Status Is Invalid"));
                }

                if (recordStatuschanged
                    && model.RecordStatusId == (int)Constants.RecordStatus.Deleted
                    && provider.OrganisationProviders.Any(x => x.IsAccepted && !x.IsRejected))
                {
                    ModelState.AddModelError(
                        "RecordStatusId",
                        AppGlobal.Language.GetText(this, "CannotDeleteWithActiveOrganisations",
                            "You may not delete this provider until you have left your Organisations."));
                }

                if (Permission.HasPermission(false, true, Permission.PermissionName.CanEditProviderSpecialFields))
                {
                    Ukrlp ukrlp = db.Ukrlps.Find(model.UKPRN);
                    if (ukrlp == null)
                    {
                        // Don't tie error message to field as this will happen anyway when the script looks it up
                        ModelState.AddModelError("", AppGlobal.Language.GetText(this, "UKRLPNotFound", "UKRLP Not Found"));
                    }
                    else if (ukrlp.UkrlpStatus.HasValue && ukrlp.UkrlpStatus.Value == (Int32)Constants.RecordStatus.Deleted && model.RecordStatusId.HasValue && model.RecordStatusId.Value != (Int32)Constants.RecordStatus.Deleted)
                    {
                        // Don't tie error message to field as this will happen anyway when the script looks it up
                        ModelState.AddModelError("", AppGlobal.Language.GetText(this, "UKRLPDeactived", "UKRLP Has Been Deactivated."));
                    }

                    if (model.SecureAccessId != null && db.Providers.Any(
                        p => p.SecureAccessId == model.SecureAccessId &&
                             p.ProviderId != model.ProviderId))
                    {
                        ModelState.AddModelError("SecureAccessId", AppGlobal.Language.GetText(this, "DuplicateSecureAccessId", "Secure Access Id is already used by another provider."));
                    }
                }

                if (ProvisionUtilities.IsNameInUse(db, model.ProviderName, model.ProviderId, null))
                {
                    ModelState.AddModelError("ProviderName", AppGlobal.Language.GetText(this, "NameInUse", "This Provider name already exists in the database, please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk if you need any help."));
                }

                if ((model.SFAFunded || model.DFE1619Funded) == false)
                {
                    ModelState.AddModelError("SFAFunded", AppGlobal.Language.GetText(this, "FundingSourceRequired", "Please select whether this provider is funded by the SFA, DfE or both."));
                }

                if (Permission.HasPermission(false, false, Permission.PermissionName.CanQAProviders))
                {
                    if (!provider.PassedOverallQAChecks && model.PassedOverallQAChecks == "1")
                    {
                        Int32 numberOfApprenticeshipsPassedQA = provider.GetQualityAssuredApprenticeshipPassedCount();
                        Int32 numberOfApprenticeshipsRequiredToQA = provider.GetNumberOfApprenticeshipsRequiredToQA();
                        if (numberOfApprenticeshipsPassedQA < numberOfApprenticeshipsRequiredToQA)
                        {
                            ModelState.AddModelError("PassedOverallQAChecks", String.Format(AppGlobal.Language.GetText(this, "InsufficientApprenticeshipsQAed", "The provider cannot pass overall QA checks until at least {0} apprenticeships have been quality assured"), numberOfApprenticeshipsRequiredToQA.ToString("N0")));
                        }
                        else if (provider.Apprenticeships.Count(x => x.ApprenticeshipQACompliances.Count() > 0 && x.ApprenticeshipQACompliances.OrderByDescending(o => o.CreatedDateTimeUtc).First().Passed) == 0)
                        {
                            ModelState.AddModelError("PassedOverallQAChecks", String.Format(AppGlobal.Language.GetText(this, "ProviderNotPassedQA", "The provider cannot pass overall QA checks until it has passed quality assurance for compliance"), numberOfApprenticeshipsRequiredToQA.ToString("N0")));
                        }
                        else if (provider.Apprenticeships.Count(x => x.ApprenticeshipQACompliances.Count() > 0 && x.ApprenticeshipQACompliances.OrderByDescending(o => o.CreatedDateTimeUtc).First().Passed == false) > 0)
                        {
                            ModelState.AddModelError("PassedOverallQAChecks", AppGlobal.Language.GetText(this, "AtLeast1ApprenticeshipFailedQA", "The provider cannot pass overall QA checks because at least 1 apprenticeship has failed quality assurance for compliance"));
                        }
                        else if (provider.ProviderQACompliances.Any() && provider.ProviderQACompliances.OrderByDescending(o => o.CreatedDateTimeUtc).First().Passed == false)
                        {
                            ModelState.AddModelError("PassedOverallQAChecks", AppGlobal.Language.GetText(this, "ProviderFailedComplianceQA", "The provider cannot pass overall QA checks because it has failed the Compliance quality assurance check"));
                        }
                        if (ModelState.IsValid)
                        {
                            //Determine if there are any style failures. If so build the reason field and use alternate template

                            string styleFailures = GetStyleQAFailText(provider);
                            List<ProviderUserContactDetails> superUsers = ProvisionUtilities.GetProviderUsers(db, provider.ProviderId, false, true);
                            if (string.IsNullOrEmpty(styleFailures))
                            {
                                foreach (ProviderUserContactDetails user in superUsers)
                                {
                                    // Send Passed QA Email to each Provider Superuser

                                    //AppGlobal.EmailQueue.AddToSendQueue(
                                    //    TemplatedEmail.EmailMessage(
                                    //        new MailAddress(user.Email, user.Name),
                                    //        null,
                                    //        null,
                                    //        Constants.EmailTemplates.ProviderPassedQAChecks,
                                    //        new List<EmailParameter>
                                    //        {
                                    //        new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                                    //        new EmailParameter("%USERNAME%", user.Name)
                                    //        }));

                                    var emailMessage = TemplatedEmail.EmailMessage(
                                            new MailAddress(user.Email, user.Name),
                                            null,
                                            null,
                                            Constants.EmailTemplates.ProviderPassedQAChecks,
                                            new List<EmailParameter>
                                            {
                                            new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                                            new EmailParameter("%USERNAME%", user.Name)
                                            });

                                    var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null);
                                }
                            }
                            else
                            {
                                foreach (ProviderUserContactDetails user in superUsers)
                                {
                                    // Send Passed QA Email with failed style checks to each Provider Superuser

                                    //AppGlobal.EmailQueue.AddToSendQueue(
                                    //    TemplatedEmail.EmailMessage(
                                    //        new MailAddress(user.Email, user.Name),
                                    //        null,
                                    //        null,
                                    //        Constants.EmailTemplates.ProviderPassedQAChecksFailedStyle,
                                    //        new List<EmailParameter>
                                    //        {
                                    //        new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                                    //        new EmailParameter("%USERNAME%", user.Name),
                                    //        new EmailParameter("%REASON%", styleFailures),
                                    //        }));

                                    var emailMessage = TemplatedEmail.EmailMessage(
                                            new MailAddress(user.Email, user.Name),
                                            null,
                                            null,
                                            Constants.EmailTemplates.ProviderPassedQAChecksFailedStyle,
                                            new List<EmailParameter>
                                            {
                                            new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                                            new EmailParameter("%USERNAME%", user.Name),
                                            new EmailParameter("%REASON%", styleFailures),
                                            });

                                    var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null);
                                }
                            }
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    if (Permission.HasPermission(false, true, Permission.PermissionName.CanEditProviderSpecialFields))
                    {
                        provider.Ukprn = model.UKPRN.HasValue ? model.UKPRN.Value : 0;
                        provider.QualityEmailsPaused = model.QualityEmailsPaused;
                        provider.QualityEmailStatusId = model.QualityEmailStatusId;
                        provider.DFE1619Funded = model.DFE1619Funded;
                        provider.SFAFunded = model.SFAFunded;
                    }

                    if (Permission.HasPermission(false, false, Permission.PermissionName.CanQAProviders))
                    {
                        if (model.PassedOverallQAChecks == "1" && !provider.PassedOverallQAChecks)
                        {
                            provider.DataReadyToQA = false;
                        }
                        provider.PassedOverallQAChecks = model.PassedOverallQAChecks == "1";
                    }

                    provider.ModifiedByUserId = Permission.GetCurrentUserId();
                    provider.ModifiedDateTimeUtc = DateTime.UtcNow;
                    // ReSharper disable once PossibleInvalidOperationException
                    provider.RecordStatusId = model.RecordStatusId.Value;

                    Address providerAddress = model.Address.ToEntity(db);
                    provider.ProviderRegionId = model.Address.RegionId;

                    db.Entry(providerAddress).State = EntityState.Modified;
                    db.Entry(provider).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    // Craig Whale 17/06/15 TFS Item: 117753
                    Session[Constants.SessionFieldNames.ProviderIsDfe1619Funded] = model.DFE1619Funded;
                    Session[Constants.SessionFieldNames.ProviderIsSfaFunded] = model.SFAFunded;
                    Session[Constants.SessionFieldNames.ProviderType] = provider.ProviderType.ProviderTypeName;

                    if (recordStatuschanged)
                    {
                        // Notify organisations
                        if (provider.RecordStatusId == (int)Constants.RecordStatus.Deleted
                            && provider.OrganisationProviders.Any())
                        {
                            foreach (var item in provider.OrganisationProviders)
                                ProvisionUtilities.SendOrganisationMembershipEmail(
                                    db, Constants.EmailTemplates.OrganisationProviderDeleted,
                                    provider.ProviderId, item.OrganisationId,
                                    null);
                        }

                        // Reset the user context if the status has changed
                        UserContext.SetUserContext(db, model.RecordStatusId == (int)Constants.RecordStatus.Deleted
                            ? UserContext.UserContextName.DeletedProvider
                            : UserContext.UserContextName.Provider, userContext.ItemId, true);
                    }
                    List<String> messages = model.GetWarningMessages();
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
                    return RedirectToAction("Index", "Provider");
                }
            }

            if (provider == null)
            {
                provider = db.Providers.Find(model.ProviderId);
            }

            // Re-get data that isn't part of the model (because these fields don't exist in the view)
            model.HasBeenQAdForCompliance = provider.ProviderQACompliances.Any();
            model.LastQAdForComplianceBy = !provider.ProviderQACompliances.Any() ? null : provider.ProviderQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().AspNetUser.Name;
            model.LastQAdForComplianceOn = !provider.ProviderQACompliances.Any() ? (DateTime?)null : provider.ProviderQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().CreatedDateTimeUtc;
            model.HasPassedComplianceChecks = provider.ProviderQACompliances.Any() && provider.ProviderQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().Passed;

            model.HasBeenQAdForStyle = provider.ProviderQAStyles.Any();
            model.LastQAdForStyleBy = !provider.ProviderQAStyles.Any() ? null : provider.ProviderQAStyles.OrderByDescending(m => m.CreatedDateTimeUtc).First().AspNetUser.Name;
            model.LastQAdForStyleOn = !provider.ProviderQAStyles.Any() ? (DateTime?)null : provider.ProviderQAStyles.OrderByDescending(m => m.CreatedDateTimeUtc).First().CreatedDateTimeUtc;
            model.HasPassedStyleChecks = provider.ProviderQAStyles.Any() && provider.ProviderQAStyles.OrderByDescending(m => m.CreatedDateTimeUtc).First().Passed;

            model.ApprenticeshipsQAed = provider.GetQualityAssuredApprenticeshipCount();
            model.ApprenticeshipPassedQA = provider.GetQualityAssuredApprenticeshipPassedCount();
            model.NumberOfApprenticeshipsRequiredToQA = provider.GetNumberOfApprenticeshipsRequiredToQA();
            model.DataReadyForQA = provider.DataReadyToQA;
            model.ShowSendFailedQAEmailButton = !provider.PassedOverallQAChecks && model.DataReadyForQA && model.HasBeenQAdForCompliance;

            model.UnableToCompleteProcess = provider.ProviderUnableToCompletes.Where(m => m.Active == true).Count() > 0;
            if (model.UnableToCompleteProcess)
            {
                model.UnableToCompleteDate = provider.ProviderUnableToCompletes.Where(m => m.Active == true).OrderByDescending(m => m.CreatedDateTimeUtc).First().CreatedDateTimeUtc;
                model.UnableToCompleteName = provider.ProviderUnableToCompletes.Where(m => m.Active == true).OrderByDescending(m => m.CreatedDateTimeUtc).First().AspNetUser.Name;
            }

            model.ImportBatches = provider.ImportBatches.ToList();

            model.NumberOfLocations = provider.Locations.Count();
            model.MaxNumberOfLocations = provider.MaxLocations;
            model.MaxNumberOfLocationsOverriddenBy = provider.MaxLocationsUser != null ? provider.MaxLocationsUser.Name : null;
            model.MaxNumberOfLocationsOverriddenOn = provider.MaxLocationsDateTimeUtc.HasValue ? DateTime.SpecifyKind(provider.MaxLocationsDateTimeUtc.Value, DateTimeKind.Utc) : (DateTime?)null;

            // Populate the dropdowns
            GetLookups(model);

            return View(model);
        }

        [ContextAuthorize(UserContext.UserContextName.ProviderOrganisation)]
        [PermissionAuthorize(Permission.PermissionName.CanViewProvider)]
        public async Task<ActionResult> Details(int? id)
        {
            if (userContext.IsProvider())
            {
                id = id == userContext.ItemId || id == null ? userContext.ItemId : null;
            }

            if (id == null)
            {
                return HttpNotFound();
            }

            var provider = await db.Providers.FindAsync(id);
            if (provider == null || (userContext.IsOrganisation()
                                     &&
                                     !db.OrganisationProviders.Any(
                                         x => x.ProviderId == id && x.OrganisationId == userContext.ItemId)))
            {
                return HttpNotFound();
            }

            var model = new AddEditProviderModel(provider, true);
            Ukrlp ukrlp =
                db.Ukrlps.Include("PrimaryAddress")
                    .Include("LegalAddress")
                    .FirstOrDefault(x => x.Ukprn == provider.Ukprn);
            if (ukrlp != null)
            {
                model.UKRLPData = new UKRLPDataModel(ukrlp);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        [PermissionAuthorize(Permission.PermissionName.CanEditProvider)]
        public async Task<ActionResult> PublishData()
        {
            var provider = await db.Providers.FirstAsync(x => x.ProviderId == userContext.ItemId);
            provider.PublishData = true;
            await db.SaveChangesAsync();
            return Json(true);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanManageUnableToComplete)]
        public ActionResult UnableToCompleteFromDialog()
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

            AddEditProviderUnableToCompleteModel model = new AddEditProviderUnableToCompleteModel
            {
                ProviderId = provider.ProviderId
            };

            // Get Lookups
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanManageUnableToComplete)]
        public ActionResult UnableToCompleteFromDialog(AddEditProviderUnableToCompleteModel model)
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

            if (ModelState.IsValid)
            {
                foreach (ProviderUnableToComplete puc in provider.ProviderUnableToCompletes.Where(x => x.Active))
                {
                    puc.Active = false;
                    db.Entry(puc).State = EntityState.Modified;
                }

                ProviderUnableToComplete providerUnableToComplete = model.ToEntity(db);
                providerUnableToComplete.Active = true;
                provider.ProviderUnableToCompletes.Add(providerUnableToComplete);
                db.Entry(provider).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new ProviderUnableToCompleteJsonModel(providerUnableToComplete));
            }

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanManageUnableToComplete)]
        public ActionResult UnableToCompleteClear()
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
                        
            foreach(ProviderUnableToComplete providerUnableToComplete in provider.ProviderUnableToCompletes)
            {
                providerUnableToComplete.Active = false;
                db.Entry(providerUnableToComplete).State = EntityState.Modified;
            }

            db.SaveChanges();

            return Json(ReturnValueJsonModel.Success());
        }



        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanQAProviders)]
        public ActionResult QAForComplianceFromDialog()
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

            AddEditProviderQAForComplianceModel model = new AddEditProviderQAForComplianceModel
            {
                ProviderId = provider.ProviderId
            };

            // Get Lookups
            GetLookups(model);

            return View(model);
        }
               

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanQAProviders)]
        public ActionResult QAForComplianceFromDialog(AddEditProviderQAForComplianceModel model)
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

            Int32 NumberApprenticeshipsQAed = provider.GetQualityAssuredApprenticeshipCount();
            Int32 NumberOfApprenticeshipsRequiredToQA = provider.GetNumberOfApprenticeshipsRequiredToQA();

            if (NumberApprenticeshipsQAed < NumberOfApprenticeshipsRequiredToQA)
            {
                ModelState.AddModelError("", String.Format(AppGlobal.Language.GetText(this, "CantQAUntilXApprenticeshipsQAed", "Unable to QA Provider Until {0} Apprenticeships Have Been QAed"), NumberOfApprenticeshipsRequiredToQA.ToString("N0")));
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
                ProviderQACompliance QA = model.ToEntity(db);
                provider.ProviderQACompliances.Add(QA);
                db.Entry(provider).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new ProviderQAForComplianceJsonModel(QA));
            }

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanQAProviders)]
        public ActionResult QAForStyleFromDialog()
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

            AddEditProviderQAForStyleModel model = new AddEditProviderQAForStyleModel
            {
                ProviderId = provider.ProviderId
            };

            // Get Lookups
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanQAProviders)]
        public ActionResult QAForStyleFromDialog(AddEditProviderQAForStyleModel model)
        {
            if (userContext.ContextName != UserContext.UserContextName.Provider)
            {
                return HttpNotFound();
            }

            Provider provider = db.Providers.Find(model.ProviderId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Int32 NumberApprenticeshipsQAed = provider.GetQualityAssuredApprenticeshipCount();
            Int32 NumberOfApprenticeshipsRequiredToQA = provider.GetNumberOfApprenticeshipsRequiredToQA();

            if (NumberApprenticeshipsQAed < NumberOfApprenticeshipsRequiredToQA)
            {
                ModelState.AddModelError("", String.Format(AppGlobal.Language.GetText(this, "CantQAUntilXApprenticeshipsQAed", "Unable to QA Provider Until {0} Apprenticeships Have Been QAed"), NumberOfApprenticeshipsRequiredToQA.ToString("N0")));
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
                ProviderQAStyle QA = model.ToEntity(db);
                provider.ProviderQAStyles.Add(QA);
                db.Entry(provider).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new ProviderQAForStyleJsonModel(QA));
            }

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProvider)]
        public ActionResult DataReadyForQA()
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

            if (provider.PassedOverallQAChecks)
            {
                return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "UnableToSetDataReadyForQAAlreadyPassed", "Unable to set apprenticeship data ready for QA as your data has already passed quality assurance checks")));
            }

            if (provider.DataReadyToQA)
            {
                return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "UnableToSetDataReadyForQAAlreadyReadyForQA", "Unable to set apprenticeship data ready for QA as your data is already ready for QA")));
            }

            if (provider.Apprenticeships.Count == 0)
            {
                return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "UnableToSetDataReadyForQANoApprenticeships", "Unable to set apprenticeship data ready for QA as you do not have any apprenticeships")));
            }

            if (provider.Apprenticeships.Count(x => x.RecordStatusId == (Int32) Constants.RecordStatus.Pending) > 0)
            {
                return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "UnableToSetDataReadyForQAPendingApprenticeships", "Unable to set apprenticeship data ready for QA as you have pending apprenticeships")));
            }

            provider.DataReadyToQA = true;
            db.SaveChanges();

            // Send email
            //AppGlobal.EmailQueue.AddToSendQueue(
            //    TemplatedEmail.EmailMessage(
            //        new MailAddress(Constants.ConfigSettings.DataReadyForQAEmailAddress, "Course Directory Provider Support"),
            //        null,
            //        null,
            //        Constants.EmailTemplates.ProviderApprenticeshipDataReadyToQA,
            //        new List<EmailParameter>
            //        {
            //            new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
            //            new EmailParameter("%POSTCODE%", provider.Address.Postcode),
            //            new EmailParameter("%UKPRN%", provider.Ukprn.ToString())
            //        }));

            var emailMessage = TemplatedEmail.EmailMessage(
                    new MailAddress(Constants.ConfigSettings.DataReadyForQAEmailAddress, "Course Directory Provider Support"),
                    null,
                    null,
                    Constants.EmailTemplates.ProviderApprenticeshipDataReadyToQA,
                    new List<EmailParameter>
                    {
                        new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                        new EmailParameter("%POSTCODE%", provider.Address.Postcode),
                        new EmailParameter("%UKPRN%", provider.Ukprn.ToString())
                    });

            var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null);

            return Json(ReturnValueJsonModel.Success());
        }



        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProvider)]
        public ActionResult TASRefreshConfirm()
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

            //Provider must already be on the RoATP register (i.e. ApprenticeshipContract has been set by overnight refresh)
            if (!provider.ApprenticeshipContract && !provider.RoATPFFlag)
            {
                return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "UnableToConfirmTASRefreshNoContract", "Unable to confirm you have refreshed your provision as you are currently not on the RoATP")));
            }

            // Check that provider is live
            if (provider.RecordStatusId != (Int32)Constants.RecordStatus.Live)
            {
                return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "UnableToConfirmTASRefreshNotLive", "Unable to confirm you have refreshed your provision as this provider is not live")));
            }

            //Find the latest batch associated with this provider, error if current batch
            var lastProviderBatch = db.ImportBatchProviders.Where(b => b.ProviderId == provider.ProviderId).OrderByDescending(d => d.ImportDateTimeUtc).FirstOrDefault();
            if (lastProviderBatch != null && lastProviderBatch.ImportBatch.Current)
            {
                return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "UnableToConfirmTASRefreshCurrentBatch", "Unable to confirm you have refreshed your provision as you are currently applying for the RoATP")));
            }

            //Create a new TAS Refresh Confirmation record in the database for this provider for the latest import batch they are associated with
            ProviderTASRefresh providerRefresh = new ProviderTASRefresh();
            providerRefresh.ImportBatchId = lastProviderBatch == null ? (int?)null : lastProviderBatch.ImportBatchId;
            providerRefresh.RefreshTimeUtc = DateTime.UtcNow;
            providerRefresh.RefreshUserId = Permission.GetCurrentUserId();
            provider.ProviderTASRefreshes.Add(providerRefresh);
            db.SaveChanges();

            return Json(ReturnValueJsonModel.Success());
        }


        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanEditProvider)]
        public ActionResult SubmitNewMarketingInformationText()
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

            SubmitNewMarketingInformationTextModel model = new SubmitNewMarketingInformationTextModel
            {
                NewMarketingInformation = provider.MarketingInformation
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanEditProvider)]
        public ActionResult SubmitNewMarketingInformationText(SubmitNewMarketingInformationTextModel model)
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

            RemoveSpellCheckHTMLFromMarketingInformation(model);

            if (ModelState.IsValid)
            {
                // Send email
                //AppGlobal.EmailQueue.AddToSendQueue(
                //    TemplatedEmail.EmailMessage(
                //        new MailAddress(Constants.ConfigSettings.SubmitNewTextForQAAddress, "Course Directory Provider Support"),
                //        null,
                //        null,
                //        Constants.EmailTemplates.ProviderApprenticeshipSubmitNewTextToQA,
                //        new List<EmailParameter>
                //        {
                //            new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                //            new EmailParameter("%UKPRN%", provider.Ukprn.ToString()),
                //            new EmailParameter("%POSTCODE%", provider.Address.Postcode),
                //            new EmailParameter("%NEWTEXT%", model.NewMarketingInformation.Replace("\r", "<br />"))
                //        }));

                var emailMessage = TemplatedEmail.EmailMessage(
                        new MailAddress(Constants.ConfigSettings.SubmitNewTextForQAAddress, "Course Directory Provider Support"),
                        null,
                        null,
                        Constants.EmailTemplates.ProviderApprenticeshipSubmitNewTextToQA,
                        new List<EmailParameter>
                        {
                            new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                            new EmailParameter("%UKPRN%", provider.Ukprn.ToString()),
                            new EmailParameter("%POSTCODE%", provider.Address.Postcode),
                            new EmailParameter("%NEWTEXT%", model.NewMarketingInformation.Replace("\r", "<br />"))
                        });

                var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null);

                return Json(ReturnValueJsonModel.Success());
            }

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanQAApprenticeships)]
        public ActionResult SendQAFailEmail()
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

            if (provider.PassedOverallQAChecks)
            {
                return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "UnableToSendQAFailEmailPassedOverallChecks", "Unable to send the QA failure email because this provider has already passed quality assurance checks")));
            }

            if (!provider.DataReadyToQA)
            {
                return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "UnableToSendQAFailEmailDataNotReady", "Unable to send the QA failure email because this provider has not yet indicated that their data is ready for QA")));
            }

            provider.DataReadyToQA = false;

            String None = AppGlobal.Language.GetText(this, "None", "None");
            String rowHtml = AppGlobal.Language.GetText(this, "QAFailEmailRowHTML", "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>");
            String tableData = String.Empty;

            String detailsOfUnverifiableClaim = String.Empty;
            String detailsOfComplianceFailure = String.Empty;
            String providerComplicanceFailureReasons = String.Empty;
            String providerStyleFailureReasons = String.Empty;
            String providerDetailsOfQA = String.Empty;

            // Get Provider Compliance
            ProviderQACompliance providerQACompliance = provider.ProviderQACompliances.OrderByDescending(x => x.CreatedDateTimeUtc).FirstOrDefault();
            if (providerQACompliance != null && !providerQACompliance.Passed)
            {
                foreach (QAComplianceFailureReason fr in providerQACompliance.QAComplianceFailureReasons)
                {
                    // Some of the reasons have the same description so only use them once
                    if (providerComplicanceFailureReasons.IndexOf(fr.FullDescription) == -1)
                    {
                        if (!String.IsNullOrEmpty(providerComplicanceFailureReasons))
                        {
                            providerComplicanceFailureReasons += "<br />";
                        }
                        providerComplicanceFailureReasons += fr.FullDescription;
                    }
                }
                detailsOfUnverifiableClaim = providerQACompliance.DetailsOfUnverifiableClaim;
                detailsOfComplianceFailure = providerQACompliance.DetailsOfComplianceFailure;
            }

            // Get Provider Style
            ProviderQAStyle providerQAStyle = provider.ProviderQAStyles.OrderByDescending(x => x.CreatedDateTimeUtc).FirstOrDefault();
            if (providerQAStyle != null && !providerQAStyle.Passed)
            {
                foreach (QAStyleFailureReason fr in providerQAStyle.QAStyleFailureReasons)
                {
                    // Some of the reasons have the same description so only use them once
                    if (providerStyleFailureReasons.IndexOf(fr.FullDescription) == -1)
                    {
                        if (!String.IsNullOrEmpty(providerStyleFailureReasons))
                        {
                            providerStyleFailureReasons += "<br />";
                        }
                        providerStyleFailureReasons += fr.FullDescription;
                    }
                }
                providerDetailsOfQA = providerQAStyle.DetailsOfQA;
            }

            tableData += String.Format(rowHtml, provider.ProviderName, String.IsNullOrWhiteSpace(providerComplicanceFailureReasons) ? None : providerComplicanceFailureReasons, 
                detailsOfUnverifiableClaim, detailsOfComplianceFailure, 
                String.IsNullOrWhiteSpace(providerStyleFailureReasons) ? None : providerStyleFailureReasons,
                String.IsNullOrWhiteSpace(providerDetailsOfQA) ? None : providerQAStyle.DetailsOfQA);

            // Get Individual Apprenticeship Failures
            foreach (Apprenticeship apprenticeship in provider.Apprenticeships.Where(x => (x.ApprenticeshipQACompliances.Count() > 0 && x.ApprenticeshipQACompliances.OrderByDescending(a => a.CreatedDateTimeUtc).First().Passed == false) || (x.ApprenticeshipQAStyles.Count() > 0 && x.ApprenticeshipQAStyles.OrderByDescending(a => a.CreatedDateTimeUtc).First().Passed == false)))
            {
                String apprenticeComplianceFailureReasons = String.Empty;
                String apprenticeStyleFailureReasons = String.Empty;
                String apprenticeDetailsOfQA = String.Empty;
                detailsOfUnverifiableClaim = String.Empty;
                detailsOfComplianceFailure = String.Empty;
                

                // Get Any Compliance Failures
                ApprenticeshipQACompliance qaCompliance = apprenticeship.ApprenticeshipQACompliances.OrderByDescending(x => x.CreatedDateTimeUtc).FirstOrDefault();
                if (qaCompliance != null && !qaCompliance.Passed)
                {
                    foreach (QAComplianceFailureReason fr in qaCompliance.QAComplianceFailureReasons)
                    {
                        // Some of the reasons have the same description so only use them once
                        if (apprenticeComplianceFailureReasons.IndexOf(fr.FullDescription) == -1)
                        {
                            if (!String.IsNullOrEmpty(apprenticeComplianceFailureReasons))
                            {
                                apprenticeComplianceFailureReasons += "<br />";
                            }
                            apprenticeComplianceFailureReasons += fr.FullDescription;
                        }
                    }
                    detailsOfUnverifiableClaim = qaCompliance.DetailsOfUnverifiableClaim;
                    detailsOfComplianceFailure = qaCompliance.DetailsOfComplianceFailure;
                }

                // Get Any Style Failures
                ApprenticeshipQAStyle qaStyle = apprenticeship.ApprenticeshipQAStyles.OrderByDescending(x => x.CreatedDateTimeUtc).FirstOrDefault();
                if (qaStyle != null && !qaStyle.Passed)
                {
                    foreach (QAStyleFailureReason fr in qaStyle.QAStyleFailureReasons)
                    {
                        // Some of the reasons have the same description so only use them once
                        if (apprenticeStyleFailureReasons.IndexOf(fr.FullDescription) == -1)
                        {
                            if (!String.IsNullOrEmpty(apprenticeStyleFailureReasons))
                            {
                                apprenticeStyleFailureReasons += "<br />";
                            }
                            apprenticeStyleFailureReasons += fr.FullDescription;
                        }
                    }
                    apprenticeDetailsOfQA = qaStyle.DetailsOfQA;
                }

                tableData += String.Format(rowHtml, apprenticeship.ApprenticeshipDetails(), 
                    String.IsNullOrWhiteSpace(apprenticeComplianceFailureReasons) ? None : apprenticeComplianceFailureReasons, 
                    detailsOfUnverifiableClaim, detailsOfComplianceFailure, 
                    String.IsNullOrWhiteSpace(apprenticeStyleFailureReasons) ? None : apprenticeStyleFailureReasons,
                String.IsNullOrWhiteSpace(apprenticeDetailsOfQA) ? None : qaStyle.DetailsOfQA);
            }

            if (!String.IsNullOrWhiteSpace(tableData))
            {
                String tableStyle = AppGlobal.Language.GetText(this, "QAFailEmailTableStyleHTML", "<style type=\"text/css\">table { border-collapse: collapse; border: 1px solid #bbb; font-family: \"Helvetica Neue\",Helvetica,Arial,sans-serif; font-size: 12px; } table th { border: 1px solid #bbb; padding: 8px; text-align: left !important; background-color: #ddd; } table td { border: 1px solid #bbb; padding: 5px; vertical-align: top; line-height: 1.5; }</style>");
                String tableHeader = AppGlobal.Language.GetText(this, "QAFailEmailHeaderRowHTML", "<table><thead><tr><th>Provider / Apprenticeship</th><th>Reason(s) for Failure</th><th>Further Details of Unverifiable Claim</th><th>Further details about compliance fails</th><th>Style Failure Reason(s)</th><th>Further style fail details</th></tr></thead><tbody>");
                String tableFooter = AppGlobal.Language.GetText(this, "QAFailEmailFooterRowHTML", "</tbody></table>");
                tableData = String.Concat(tableStyle, tableHeader, tableData, tableFooter);
            }
            else
            {
                return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "QAFailEmailNothingFailed", "Unable to send QA fail email as neither the provider nor any of it's apprenticeships have failed QA checks")));
            }

            // Send the email
            List<ProviderUserContactDetails> superUsers = ProvisionUtilities.GetProviderUsers(db, provider.ProviderId, false, true);
            foreach (ProviderUserContactDetails user in superUsers)
            {
                // Send Passed QA Email to each Provider Superuser in 
                //AppGlobal.EmailQueue.AddToSendQueue(
                //    TemplatedEmail.EmailMessage(
                //        new MailAddress(user.Email, user.Name),
                //        null,
                //        null,
                //        Constants.EmailTemplates.ProviderFailedQAChecks,
                //        new List<EmailParameter>
                //            {
                //                new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                //                new EmailParameter("%USERNAME%", user.Name),
                //                new EmailParameter("%REASON%", tableData)
                //            }));

                var emailMessage = TemplatedEmail.EmailMessage(
                        new MailAddress(user.Email, user.Name),
                        null,
                        null,
                        Constants.EmailTemplates.ProviderFailedQAChecks,
                        new List<EmailParameter>
                            {
                                new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                                new EmailParameter("%USERNAME%", user.Name),
                                new EmailParameter("%REASON%", tableData)
                            });

                var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null);
            }

            db.SaveChanges();

            return Json(ReturnValueJsonModel.Success());
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanQAProviders)]
        public ActionResult GetNumberOfQAdApprenticeshipsAndNumberRequiredToQA()
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

            Int32 NumberApprenticeshipsQAed = provider.GetQualityAssuredApprenticeshipCount();
            Int32 NumberOfApprenticeshipsRequiredToQA = provider.GetNumberOfApprenticeshipsRequiredToQA();

            return Content(String.Format(AppGlobal.Language.GetText(this, "xOutOfyApprenticeshipsHaveBeenQAed", "{0} out of the {1} required apprenticeships have been quality assured."), NumberApprenticeshipsQAed.ToString("N0"), NumberOfApprenticeshipsRequiredToQA.ToString("N0")));
        }

        [PermissionAuthorize(Permission.PermissionName.CanEditProvider)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult DeliveryInformation()
        {
            Provider provider = db.Providers.FirstOrDefault(x => x.ProviderId == userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            AddEditDeliveryInformationModel aemim = new AddEditDeliveryInformationModel(provider);

            return View(aemim);
        }

        // POST: /DeliveryInformation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanEditProvider)]
        [ContextAuthorize(new[] { UserContext.UserContextName.Provider })]
        public async Task<ActionResult> DeliveryInformation(AddEditDeliveryInformationModel model)
        {
            model.ProviderId = userContext.ItemId;

            RemoveSpellCheckHTMLFromMarketingInformation(model);

            if (ModelState.IsValid)
            {
                Provider provider = model.ToEntity(db);
                if (provider == null || provider.ProviderId != userContext.ItemId)
                {
                    return HttpNotFound();
                }

                if (ModelState.IsValid)
                {
                    db.Entry(provider).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProvider)]
        public ActionResult SetCoursesUpToDate()
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

            return Json(ReturnValueJsonModel.Success());
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanManuallyAssignImportBatches)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult AssignImportBatch(Int32 providerId, Int32 importBatchId)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (provider.ProviderId != providerId)
            {
                return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "ProviderContextChanged", "Provider context has changed.")));
            }

            ImportBatch importBatch = db.ImportBatches.Find(importBatchId);
            if (importBatch == null)
            {
                return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "ImportBatchNotFound", "Import batch not found.")));
            }

            if (!provider.ImportBatches.Any(x => x.ImportBatchId == importBatchId))
            {
                try
                {
                    ImportBatchProvider importBatchProvider = new ImportBatchProvider()
                    {
                        ImportBatch = importBatch,
                        HasProviderLevelData = !String.IsNullOrWhiteSpace(provider.MarketingInformation),
                        HasApprenticeshipLevelData = provider.Apprenticeships.Any(),
                        ImportDateTimeUtc = DateTime.UtcNow,
                        ManuallyAddedByUserId = Permission.GetCurrentUserId()
                    };
                    db.Entry(importBatchProvider).State = EntityState.Added;
                    provider.ImportBatches.Add(importBatchProvider);
                    db.Entry(provider).State = EntityState.Modified;

                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(ReturnValueJsonModel.FailWithMessage(AppGlobal.Language.GetText(this, "ErrorAddingImportBatchToProvider", "Error adding import batch to provider.")));
                }
            }

            String yes = AppGlobal.Language.GetText(this, "Yes", "Yes");
            String no = AppGlobal.Language.GetText(this, "No", "No");
            AspNetUser user = db.AspNetUsers.Find(Permission.GetCurrentUserId());

            return Json(new { Status=1, HasProviderLevelInfo = String.IsNullOrWhiteSpace(provider.MarketingInformation) ? no : yes, HasApprenticeshipsLabel = provider.Apprenticeships.Any() ? yes : no, ManuallyAddedBy = user == null ? "" : user.Name });
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanOverrideMaxLocations)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult OverrideMaximumNumberOfLocations()
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

            OverrideMaximumNumberOfLocationsModel model = new OverrideMaximumNumberOfLocationsModel
            {
                MaximumNumberOfLocations = provider.MaxLocations ?? Constants.ConfigSettings.MaxLocations,
                NumberOfLocations = provider.Locations.Count()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanOverrideMaxLocations)]
        [ContextAuthorize(new[] { UserContext.UserContextName.Provider })]
        public async Task<ActionResult> OverrideMaximumNumberOfLocations(OverrideMaximumNumberOfLocationsModel model)
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

            if (model.MaximumNumberOfLocations < 0)
            {
                ModelState.AddModelError("MaximumNumberOfLocations", "Maximum Number of Locations Must be Greater Than or Equal to Zero");
            }

            if (ModelState.IsValid)
            {
                provider.MaxLocations = model.MaximumNumberOfLocations;
                provider.MaxLocationsDateTimeUtc = DateTime.UtcNow;
                provider.MaxLocationsUserId = Permission.GetCurrentUserId();
                db.Entry(provider).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return Json(new { Status = 1 });
            }

            model.NumberOfLocations = provider.Locations.Count();

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanOverrideMaxLocations)]
        [ContextAuthorize(new[] { UserContext.UserContextName.Provider })]
        public async Task<ActionResult> ResetMaximumNumberOfLocations()
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

            provider.MaxLocations = null;
            provider.MaxLocationsDateTimeUtc = null;
            provider.MaxLocationsUserId = null;
            db.Entry(provider).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(new { Status = 1 });
        }

        [HttpPost]
        [Authorize]
        public ActionResult IsUrlValid(String url)
        {
            return Json(new { IsValid = UrlHelper.UrlIsValidFormat(url) ? 1 : 0, IsReachable = UrlHelper.UrlIsReachable(url) ? 1 : 0 });
        }

        [NonAction]
        private void GetLookups(AddEditProviderQAForComplianceModel model)
        {
            model.QAComplianceFailureReasons = db.QAComplianceFailureReasons.Where(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live).ToList();

            List<SelectListItem> selectListItems = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = AppGlobal.Language.GetText(this, "Yes", "Yes")},
                new SelectListItem {Value = "0", Text = AppGlobal.Language.GetText(this, "No", "No")},
            };

            ViewBag.YesNo = new SelectList(selectListItems, "Value", "Text");
        }

        [NonAction]
        private void GetLookups(AddEditProviderUnableToCompleteModel model)
        {
            model.UnableToCompleteFailureReasons = db.UnableToCompleteFailureReasons.Where(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live).ToList();

            List<SelectListItem> selectListItems = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = AppGlobal.Language.GetText(this, "Yes", "Yes")},
                new SelectListItem {Value = "0", Text = AppGlobal.Language.GetText(this, "No", "No")},
            };

            ViewBag.YesNo = new SelectList(selectListItems, "Value", "Text");
        }

        [NonAction]
        private void GetLookups(AddEditProviderQAForStyleModel model)
        {
            model.QAStyleFailureReasons = db.QAStyleFailureReasons.Where(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live).ToList();

            List<SelectListItem> selectListItems = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = AppGlobal.Language.GetText(this, "Yes", "Yes")},
                new SelectListItem {Value = "0", Text = AppGlobal.Language.GetText(this, "No", "No")},
            };

            ViewBag.YesNo = new SelectList(selectListItems, "Value", "Text");
        }

        [NonAction]
        private void GetLookups(AddEditProviderModel model)
        {
            if (model.Address != null) model.Address.Populate(db);
            ViewBag.ProviderTypes = new SelectList(
                db.ProviderTypes,
                "ProviderTypeId",
                "ProviderTypeName",
                model.ProviderTypeId);
            ViewBag.RecordStatuses = new SelectList(
                db.RecordStatus.Where(x => x.RecordStatusId == (int)Constants.RecordStatus.Live || x.RecordStatusId == (int)Constants.RecordStatus.Deleted),
                "RecordStatusId",
                "RecordStatusName",
                model.RecordStatusId);
            ViewBag.QualityEmailStatuses = new SelectList(
                db.QualityEmailStatuses,
                "QualityEmailStatusId",
                "QualityEmailStatusDesc",
                model.QualityEmailStatusId);
            ViewBag.DfEProviderTypes = new SelectList(
                db.DfEProviderTypes.OrderBy(pt => pt.DfEProviderTypeName),
                "DfEProviderTypeId",
                "DfEProviderTypeName",
                model.DfEProviderTypeId);
            ViewBag.DfEProviderStatuses = new SelectList(
                db.DfEProviderStatus.OrderBy(ps => ps.DfEProviderStatusName),
                "DfEProviderStatusId",
                "DfEProviderStatusName",
                model.DfEProviderStatusId);
            ViewBag.DfELocalAuthorities = new SelectList(
                db.DfELocalAuthorities.OrderBy(la => la.DfELocalAuthorityName),
                "DfELocalAuthorityId",
                "DfELocalAuthorityName",
                model.DfELocalAuthorityId);
            ViewBag.DfERegions = new SelectList(
                db.DfERegions.OrderBy(r => r.DfERegionName),
                "DfERegionId",
                "DfERegionName",
                model.DfERegionId);
            ViewBag.DfEEstablishmentTypes = new SelectList(
                db.DfEEstablishmentTypes.OrderBy(et => et.DfEEstablishmentTypeName),
                "DfEEstablishmentTypeId",
                "DfEEstablishmentTypeName",
                model.DfEEstablishmentTypeId);

            if (model.ProviderId.HasValue)
            {
                List<Int32> existingImportBatchIds = new List<Int32>();
                if (model.ImportBatches.Any())
                {
                    foreach (ImportBatchProvider ibp in model.ImportBatches)
                    {
                        existingImportBatchIds.Add(ibp.ImportBatchId);
                    }
                }
                ViewBag.ImportBatches = new SelectList(
                    db.ImportBatches.OrderByDescending(ib => ib.Current).ThenBy(ib => ib.ImportBatchName).Where(ib => existingImportBatchIds.Contains(ib.ImportBatchId) == false),
                    "ImportBatchId",
                    "ImportBatchName");
            }

            List<SelectListItem> selectListItems = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = AppGlobal.Language.GetText(this, "Yes", "Yes")},
                new SelectListItem {Value = "0", Text = AppGlobal.Language.GetText(this, "No", "No")},
            };

            ViewBag.YesNo = new SelectList(selectListItems, "Value", "Text", model.PassedOverallQAChecks == "1" ? "Yes" : "No");

            model.WithDfELookupData(db);
        }

        [NonAction]
        public void RemoveSpellCheckHTMLFromMarketingInformation(AddEditProviderModel model)
        {
            model.MarketingInformation = Markdown.Sanitize(model.MarketingInformation);
            if (!ModelState.IsValidField("MarketingInformation") && model.MarketingInformation.Length <= AddEditProviderModel.MarketingInformationMaxLength)
            {
                // These 2 strings should match the corresponding strings in AddEditProviderModel (especially where language is set to developer mode)
                String errorMessage = String.Format(AppGlobal.Language.GetText("AddEditProviderModel_StringLength_MarketingInformation", "The maximum length of {0} is 750 characters."), AppGlobal.Language.GetText("AddEditProviderModel_DisplayName_MarketingInformation", "Your Generic Apprenticeship Information for Employers"));
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

        [NonAction]
        public void RemoveSpellCheckHTMLFromMarketingInformation(AddEditDeliveryInformationModel model)
        {
            model.MarketingInformation = Markdown.Sanitize(model.MarketingInformation);
            if (!ModelState.IsValidField("MarketingInformation") && model.MarketingInformation.Length <= AddEditDeliveryInformationModel.MarketingInformationMaxLength)
            {
                // These 2 strings should match the corresponding strings in AddEditDeliveryInformationModel (especially where language is set to developer mode)
                String errorMessage = String.Format(AppGlobal.Language.GetText("AddEditDeliveryInformationModel_StringLength_MarketingInformation", "The maximum length of {0} is 750 characters."), AppGlobal.Language.GetText("AddEditDeliveryInformationModel_DisplayName_MarketingInformation", "Your Generic Apprenticeship Information for Employers"));
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

        [NonAction]
        public void RemoveSpellCheckHTMLFromMarketingInformation(SubmitNewMarketingInformationTextModel model)
        {
            model.NewMarketingInformation = Markdown.Sanitize(model.NewMarketingInformation);
            if (!ModelState.IsValidField("NewMarketingInformation") && model.NewMarketingInformation.Length <= SubmitNewMarketingInformationTextModel.MarketingInformationMaxLength)
            {
                // These 2 strings should match the corresponding strings in SubmitNewMarketingInformationTextModel (especially where language is set to developer mode)
                String errorMessage = String.Format(AppGlobal.Language.GetText("SubmitNewMarketingInformationTextModel_StringLength_MarketingInformation", "The maximum length of {0} is 750 characters."), AppGlobal.Language.GetText("SubmitNewMarketingInformationTextModel_DisplayName_MarketingInformation", "Enter Text to be Submitted for QA"));
                foreach (ModelError me in ModelState["NewMarketingInformation"].Errors)
                {
                    if (me.ErrorMessage == errorMessage)
                    {
                        ModelState["NewMarketingInformation"].Errors.Remove(me);
                        break;
                    }
                }
                // If there are no more marketing information errors then remove the key altogether
                if (ModelState["NewMarketingInformation"].Errors.Count == 0)
                {
                    ModelState.Remove("NewMarketingInformation");
                }
            }
        }

        [NonAction]
        public Boolean showDataReadyForQAButton()
        {
            // This gets called for every page so check the things that don't require database access first

            // Context must be provider
            if (!userContext.IsProvider())
            {
                return false;
            }

            // User should have CanEditProvider permission but not CanEditProviderSpecialFields
            if (!Permission.HasPermission(false, true, Permission.PermissionName.CanEditProvider))
            {
                return false;
            }

            // Check that provider exists
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return false;
            }

            // Check that provider is live, has not already clicked the button & has not passed overall QA & has live apprenticeships
            if (provider.RecordStatusId != (Int32)Constants.RecordStatus.Live || provider.DataReadyToQA || provider.PassedOverallQAChecks || provider.Apprenticeships.Count(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live) == 0)
            {
                return false;
            }

            // If we've reached here then the button should be visible
            return true;
        }



        [NonAction]
        public Boolean showTASRefreshConfirmButton()
        {
            // This gets called for every page so check the things that don't require database access first

            // Context must be provider
            if (!userContext.IsProvider())
            {
                return false;
            }

            // User should have CanEditProvider permission
            if (!Permission.HasPermission(false, true, Permission.PermissionName.CanEditProvider))
            {
                return false;
            }

            // Check that provider exists
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return false;
            }

            // We must be between start date and end date or the provider must have TAS Refresh Confirm Override Set
            DateTime today = DateTime.Today;
            if ((today < Constants.ConfigSettings.RoATPRefreshStartDate || today > Constants.ConfigSettings.RoATPRefreshEndDate.AddDays(1)) && !provider.TASRefreshOverride)
            {
                return false;
            }

            //Provider must already be on the RoATP register (i.e. ApprenticeshipContract has been set by overnight refresh AND RoATP flag is set)
            if (!provider.ApprenticeshipContract && !provider.RoATPFFlag)
            {
                return false;
            }

            // Check that provider is live, has not already clicked the button & has passed overall QA & has live apprenticeships
            if (provider.RecordStatusId != (Int32)Constants.RecordStatus.Live ||  !provider.PassedOverallQAChecks || provider.Apprenticeships.Count(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live) == 0)
            {
                return false;
            }

            //Check provider has not already clicked the button for this period
            //Strictly we have a refresh period between start and end date but late providers could have clicked after end date
            //It should be sufficient to check that a provider has clicked the button after the current start date
            var latestConfirm = db.ProviderTASRefreshes.Where(r => r.ProviderId == provider.ProviderId && r.RefreshTimeUtc > Constants.ConfigSettings.RoATPRefreshStartDate).FirstOrDefault();
            if (latestConfirm != null)
            {
                return false;
            }

            var currentBatch = db.ImportBatches.Where(b => b.Current).FirstOrDefault();
            //If provider is in current import batch return false
            if (currentBatch != null && currentBatch.ImportBatchProviders.FirstOrDefault(b => b.ProviderId == userContext.ItemId) != null)
            {
                return false;
            }

            // If we've reached here then the button should be visible
            return true;
        }



        [NonAction]
        private string GetStyleQAFailText(Provider provider)
        {
            String None = AppGlobal.Language.GetText(this, "None", "None");
            String rowHtml = AppGlobal.Language.GetText(this, "QAFailStyleEmailRowHTML", "<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>");
            String tableData = String.Empty;

            String providerStyleFailureReasons = String.Empty;

            // Get Provider Style
            ProviderQAStyle providerQAStyle = provider.ProviderQAStyles.OrderByDescending(x => x.CreatedDateTimeUtc).FirstOrDefault();
            if (providerQAStyle != null && !providerQAStyle.Passed)
            {
                foreach (QAStyleFailureReason fr in providerQAStyle.QAStyleFailureReasons)
                {
                    // Some of the reasons have the same description so only use them once
                    if (providerStyleFailureReasons.IndexOf(fr.FullDescription) == -1)
                    {
                        if (!String.IsNullOrEmpty(providerStyleFailureReasons))
                        {
                            providerStyleFailureReasons += "<br />";
                        }
                        providerStyleFailureReasons += fr.FullDescription;
                    }
                }

                tableData += String.Format(rowHtml, provider.ProviderName,
                    String.IsNullOrWhiteSpace(providerStyleFailureReasons) ? None : providerStyleFailureReasons,
                    String.IsNullOrWhiteSpace(providerQAStyle.DetailsOfQA) ? None : providerQAStyle.DetailsOfQA);
            }

            // Get Individual Apprenticeship Failures
            foreach (Apprenticeship apprenticeship in provider.Apprenticeships.Where(x => (x.ApprenticeshipQACompliances.Count() > 0 && x.ApprenticeshipQACompliances.OrderByDescending(a => a.CreatedDateTimeUtc).First().Passed == false) || (x.ApprenticeshipQAStyles.Count() > 0 && x.ApprenticeshipQAStyles.OrderByDescending(a => a.CreatedDateTimeUtc).First().Passed == false)))
            {
                String apprenticeStyleFailureReasons = String.Empty;

                // Get Any Style Failures
                ApprenticeshipQAStyle qaStyle = apprenticeship.ApprenticeshipQAStyles.OrderByDescending(x => x.CreatedDateTimeUtc).FirstOrDefault();
                if (qaStyle != null && !qaStyle.Passed)
                {
                    foreach (QAStyleFailureReason fr in qaStyle.QAStyleFailureReasons)
                    {
                        // Some of the reasons have the same description so only use them once
                        if (apprenticeStyleFailureReasons.IndexOf(fr.FullDescription) == -1)
                        {
                            if (!String.IsNullOrEmpty(apprenticeStyleFailureReasons))
                            {
                                apprenticeStyleFailureReasons += "<br />";
                            }
                            apprenticeStyleFailureReasons += fr.FullDescription;
                        }
                    }

                    tableData += String.Format(rowHtml, apprenticeship.ApprenticeshipDetails(),
                        String.IsNullOrWhiteSpace(apprenticeStyleFailureReasons) ? None : apprenticeStyleFailureReasons,
                    String.IsNullOrWhiteSpace(qaStyle.DetailsOfQA) ? None : qaStyle.DetailsOfQA);
                }
            }

            if (!String.IsNullOrWhiteSpace(tableData))
            {
                String tableStyle = AppGlobal.Language.GetText(this, "QAFailStyleEmailTableStyleHTML", "<style type=\"text/css\">table { border-collapse: collapse; border: 1px solid #bbb; font-family: \"Helvetica Neue\",Helvetica,Arial,sans-serif; font-size: 12px; } table th { border: 1px solid #bbb; padding: 8px; text-align: left !important; background-color: #ddd; } table td { border: 1px solid #bbb; padding: 5px; vertical-align: top; line-height: 1.5; }</style>");
                String tableHeader = AppGlobal.Language.GetText(this, "QAFailStyleEmailHeaderRowHTML", "<table><thead><tr><th>Provider / Apprenticeship</th><th>Style Failure Reason(s)</th><th>Further style fail details</th></tr></thead><tbody>");
                String tableFooter = AppGlobal.Language.GetText(this, "QAFailStyleEmailFooterRowHTML", "</tbody></table>");
                tableData = String.Concat(tableStyle, tableHeader, tableData, tableFooter);
                return tableData;
            }
            return string.Empty;
        }

    }
}
