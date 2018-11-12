using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class ApprenticeshipSearchViewModelExtensions
    {
        public static ApprenticeshipSearchViewModel Populate(this ApprenticeshipSearchViewModel model,
            ProviderPortalEntities db)
        {
            var userContext = UserContext.GetUserContext();

            if (model == null) model = new ApprenticeshipSearchViewModel();

            // Populate delivery modes
            model.DeliveryModes = db.DeliveryModes
                .Where(x => x.RecordStatusId == (int) Constants.RecordStatus.Live)
                .Select(x => new SelectListItem
                {
                    Text = x.DeliveryModeName,
                    // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                    Value = x.DeliveryModeId.ToString()
                }).OrderBy(x => x.Text).ToList();

            // Populate Locations
            if (userContext.IsProvider())
            {
                model.Locations = db.Locations
                    .Where(x => x.ProviderId == userContext.ItemId.Value)
                    .Select(x => new SelectListItem
                    {
                        Text = x.LocationName,
                        // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                        Value = x.LocationId.ToString()
                    }).OrderBy(x => x.Text).ToList();
            }
            else
            {
                model.Locations = new List<SelectListItem>();
            }
            return model;
        }
    }

    public static class ApprenticeshipListViewModelExtensions
    {
        public static ApprenticeshipListViewModel Populate(this ApprenticeshipListViewModel model,
            ProviderPortalEntities db)
        {
            var userContext = UserContext.GetUserContext();

            if (model.Search == null) model.Search = new ApprenticeshipSearchViewModel();
            model.Search.Populate(db);

            if (!userContext.IsProvider()) return model;

            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return model;
            }

            model.DisplayNotPublishedBanner = !provider.ApprenticeshipContract;
            
            var apprenticeships = db.Apprenticeships
                .Where(x => x.ProviderId == userContext.ItemId.Value);
            if (model.Search.DeliveryModeId != null)
            {
                apprenticeships =
                    apprenticeships.Where(
                        x => x.ApprenticeshipLocations.Any(
                            y => y.DeliveryModes.Any(
                                z => z.DeliveryModeId == model.Search.DeliveryModeId)));
            }
            if (model.Search.LocationId != null)
            {
                apprenticeships =
                    apprenticeships.Where(
                        x => x.ApprenticeshipLocations.Any(y => y.LocationId == model.Search.LocationId));
            }
            if (!String.IsNullOrWhiteSpace(model.Search.FrameworkOrStandardId)
                && !String.IsNullOrWhiteSpace(model.Search.FrameworkOrStandard))
            {
                var decodedValues = ApprenticeshipExtensions.DecodeSearchFrameworkOrStandard(model.Search.FrameworkOrStandardId);
                if (decodedValues.FrameworkCode != null)
                {
                    apprenticeships =
                        apprenticeships.Where(x =>
                            x.FrameworkCode == decodedValues.FrameworkCode
                            && x.ProgType == decodedValues.ProgType
                            && x.PathwayCode == decodedValues.PathwayCode);
                }
                if (decodedValues.StandardCode != null)
                {
                    apprenticeships =
                        apprenticeships.Where(x =>
                            x.StandardCode == decodedValues.StandardCode
                            && x.Version == decodedValues.Version);
                }
            }

            if (Permission.HasPermission(false, false, Permission.PermissionName.CanQAApprenticeships))
            {
                model.ApprenticeshipsQAed = provider.GetQualityAssuredApprenticeshipCount();
                model.NumberOfApprenticeshipsRequiredToQA = provider.GetNumberOfApprenticeshipsRequiredToQA();
                model.DataReadyForQA = provider.DataReadyToQA;
            }

            model.Apprenticeships = apprenticeships.ToList()
                .Select(x => new ApprenticeshipListViewModelItem
                {
                    // [FrameworkCode], [ProgType], [PathwayCode]
                    // [StandardCode], [Version]
                    ApprenticeshipId = x.ApprenticeshipId,
                    ApprenticeshipDetails = x.ApprenticeshipDetails(),
                    LastUpdate = x.ModifiedDateTimeUtc ?? x.CreatedDateTimeUtc,
                    Status = x.RecordStatu.RecordStatusName,
                    ComplianceQAResult = !x.ApprenticeshipQACompliances.Any() ? "" : x.ApprenticeshipQACompliances.OrderByDescending(o => o.CreatedDateTimeUtc).First().Passed ? AppGlobal.Language.GetText("Apprenticeship_QAResult_Passed", "Passed") : AppGlobal.Language.GetText("Apprenticeship_QAResult_Failed", "Failed"),
                    StyleQAResult = !x.ApprenticeshipQAStyles.Any() ? "" : x.ApprenticeshipQAStyles.OrderByDescending(o => o.CreatedDateTimeUtc).First().Passed ? AppGlobal.Language.GetText("Apprenticeship_QAResult_Passed", "Passed") : AppGlobal.Language.GetText("Apprenticeship_QAResult_Failed", "Failed")                    
                }).OrderByDescending(x => x.LastUpdate).ToList();

            return model;
        }

        public static String ApprenticeshipDetails(this Apprenticeship apprenticeship)
        {
            return apprenticeship.Framework != null
                ? apprenticeship.Framework.Details()
                : apprenticeship.Standard != null
                    ? apprenticeship.Standard.Details()
                    : String.Empty;
        }

        public static String Details(this Framework framework)
        {
            return (String.IsNullOrEmpty(framework.PathwayName)
                ? framework.NasTitle + " - " + framework.ProgType1.ProgTypeDesc
                : framework.NasTitle + " - " + framework.PathwayName + " - " + framework.ProgType1.ProgTypeDesc);

        }

        public static String Details(this Standard standard)
        {
            return standard.StandardSectorCode1.StandardSectorCodeDesc + " - "
                   + standard.StandardName + " - "
                   + AppGlobal.Language.GetText("Apprenticeship_Standard_Level", "Level") + " " + standard.NotionalEndLevel;
        }
    }

    /// <summary>
    /// Add/edit apprenticeship view model extensions.
    /// </summary>
    public static class AddEditApprenticeshipViewModelExtensions
    {
        public static AddEditApprenticeshipViewModel Populate(this AddEditApprenticeshipViewModel model,
            ProviderPortalEntities db)
        {
            // Create new apprenticeship
            if (model == null) model = new AddEditApprenticeshipViewModel();
            model.RecordStatusId = (int) Constants.RecordStatus.Pending;

            UserContext.UserContextInfo uc = UserContext.GetUserContext();
            if (uc.IsProvider())
            {
                Provider provider = db.Providers.Find(uc.ItemId);
                if (provider != null)
                {
                    model.DisplayNotPublishedBanner = !provider.ApprenticeshipContract;
                }
            }

            return model;
        }

        public static AddEditApprenticeshipViewModel Populate(this AddEditApprenticeshipViewModel model, int id,
            ProviderPortalEntities db)
        {
            var apprenticeship = new Apprenticeship();
            if (id > 0)
            {
                var userContext = UserContext.GetUserContext();
                if (userContext.IsProvider())
                {
                    apprenticeship = db.Apprenticeships.FirstOrDefault(
                        x =>
                            x.ApprenticeshipId == id &&
                            x.ProviderId == userContext.ItemId.Value);
                }
            }
            model = new AddEditApprenticeshipViewModel
            {
                ApprenticeshipId = apprenticeship.ApprenticeshipId,
                RecordStatusId = apprenticeship.RecordStatusId,
                FrameworkOrStandardId = ApprenticeshipExtensions.GetFrameworkOrStandardId(apprenticeship),
                FrameworkOrStandard = apprenticeship.ApprenticeshipDetails(),
                MarketingInformation = apprenticeship.MarketingInformation,
                Url = UrlHelper.GetFullUrl(apprenticeship.Url),
                ContactEmail = apprenticeship.ContactEmail,
                ContactTelephone = apprenticeship.ContactTelephone,
                ContactWebsite = UrlHelper.GetFullUrl(apprenticeship.ContactWebsite),
                DisplayNotPublishedBanner = !apprenticeship.Provider.ApprenticeshipContract,
                HasBeenQAdForCompliance = apprenticeship.ApprenticeshipQACompliances.Any(),
                LastQAdForComplianceBy = apprenticeship.ApprenticeshipQACompliances.Count == 0 ? null : apprenticeship.ApprenticeshipQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().AspNetUser.Name,
                LastQAdForComplianceOn = apprenticeship.ApprenticeshipQACompliances.Count == 0 ? (DateTime?)null : apprenticeship.ApprenticeshipQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().CreatedDateTimeUtc,
                HasPassedComplianceChecks = apprenticeship.ApprenticeshipQACompliances.Count != 0 && apprenticeship.ApprenticeshipQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().Passed,
                HasBeenQAdForStyle = apprenticeship.ApprenticeshipQAStyles.Any(),
                LastQAdForStyleBy = apprenticeship.ApprenticeshipQAStyles.Count == 0 ? null : apprenticeship.ApprenticeshipQAStyles.OrderByDescending(m => m.CreatedDateTimeUtc).First().AspNetUser.Name,
                LastQAdForStyleOn = apprenticeship.ApprenticeshipQAStyles.Count == 0 ? (DateTime?)null : apprenticeship.ApprenticeshipQAStyles.OrderByDescending(m => m.CreatedDateTimeUtc).First().CreatedDateTimeUtc,
                HasPassedStyleChecks = apprenticeship.ApprenticeshipQAStyles.Count != 0 && apprenticeship.ApprenticeshipQAStyles.OrderByDescending(m => m.CreatedDateTimeUtc).First().Passed
            };
            var deliveryLocations = new DeliveryLocationListViewModel();
            model.DeliveryLocations = deliveryLocations.Populate(id, db);

            return model;
        }

        public static List<String> GetWarningMessages(this AddEditApprenticeshipViewModel model, ProviderPortalEntities db)
        {
            List<String> messages = new List<String>();

            if (!String.IsNullOrWhiteSpace(model.Url) && !UrlHelper.UrlIsReachable(model.Url))
            {
                messages.Add(String.Format(AppGlobal.Language.GetText("AddEditApprenticeshipViewModel_Edit_UrlNotReachable", "The web address for {0} returns a response that suggests this page may not exist. Please check that the web address entered is correct."), AppGlobal.Language.GetText("AddEditApprenticeshipViewModel_DisplayName_Url", "Your Apprenticeship Website Page")));
            }

            if (!String.IsNullOrWhiteSpace(model.ContactWebsite) && !UrlHelper.UrlIsReachable(model.ContactWebsite))
            {
                messages.Add(String.Format(AppGlobal.Language.GetText("AddEditApprenticeshipViewModel_Edit_UrlNotReachable", "The web address for {0} returns a response that suggests this page may not exist. Please check that the web address entered is correct."), AppGlobal.Language.GetText("AddEditApprenticeshipViewModel_DisplayName_ContactWebsite", @"Your Apprenticeship Website ""Contact Us"" Page")));
            }

            return messages;
        }

        public static bool Validate(this AddEditApprenticeshipViewModel model, ProviderPortalEntities db, ModelStateDictionary modelState)
        {
            if (modelState["FrameworkOrStandard"].Errors.Count == 0)
            {
                Apprenticeship frameworkOrStandard = ApprenticeshipExtensions.DecodeSearchFrameworkOrStandard(model.FrameworkOrStandardId) ?? ApprenticeshipExtensions.DecodeSearchFrameworkOrStandardByName(model.FrameworkOrStandard);
                if (frameworkOrStandard == null)
                {
                    modelState.AddModelError("FrameworkOrStandard", AppGlobal.Language.GetText("Apprenticeship_Edit_FrameworkOrStandardRequired", "The Framework / Standard Name field is required."));
                }
                else
                {
                    Int32 providerId = UserContext.GetUserContext().ItemId ?? 0;
                    Apprenticeship app = db.Apprenticeships.FirstOrDefault(x => x.ProviderId == providerId && x.StandardCode == frameworkOrStandard.StandardCode && x.Version == frameworkOrStandard.Version && x.FrameworkCode == frameworkOrStandard.FrameworkCode && x.ProgType == frameworkOrStandard.ProgType && x.PathwayCode == frameworkOrStandard.PathwayCode && (model.ApprenticeshipId == 0 || x.ApprenticeshipId != model.ApprenticeshipId));
                    if (app != null)
                    {
                        modelState.AddModelError("FrameworkOrStandard", AppGlobal.Language.GetText("Apprenticeship_Create_FrameworkOrStandardMustBeUnique", "The Framework / Standard Name supplied is already in use."));
                    }
                }
            }
            return modelState.IsValid;
        }

        public static Apprenticeship ToEntity(this AddEditApprenticeshipViewModel model, ProviderPortalEntities db)
        {
            var userContext = UserContext.GetUserContext();
            var userId = Permission.GetCurrentUserId();
            if (!userContext.IsProvider()) return null;
            Apprenticeship apprenticeship = model.ApprenticeshipId != 0
                ? db.Apprenticeships.FirstOrDefault(
                    x =>
                        x.ApprenticeshipId == model.ApprenticeshipId &&
                        x.ProviderId == userContext.ItemId.Value)
                : null;
            apprenticeship = apprenticeship ?? new Apprenticeship
            {
                CreatedByUserId = userId,
                CreatedDateTimeUtc = DateTime.UtcNow,
                ProviderId = userContext.ItemId.Value,
                RecordStatusId = (int) Constants.RecordStatus.Pending
            };
            apprenticeship.ModifiedByUserId = userId;
            apprenticeship.ModifiedDateTimeUtc = DateTime.UtcNow;
            apprenticeship.AddedByApplicationId = (int) Constants.Application.Portal;
            apprenticeship.MarketingInformation = Markdown.Sanitize(model.MarketingInformation);
            apprenticeship.Url = UrlHelper.GetFullUrl(model.Url);
            apprenticeship.ContactTelephone = model.ContactTelephone;
            apprenticeship.ContactEmail = model.ContactEmail;
            apprenticeship.ContactWebsite = UrlHelper.GetFullUrl(model.ContactWebsite);

            Apprenticeship frameworkOrStandard = ApprenticeshipExtensions.DecodeSearchFrameworkOrStandard(model.FrameworkOrStandardId) ?? ApprenticeshipExtensions.DecodeSearchFrameworkOrStandardByName(model.FrameworkOrStandard);
            if (frameworkOrStandard != null)
            {
                apprenticeship.StandardCode = frameworkOrStandard.StandardCode;
                apprenticeship.Version = frameworkOrStandard.Version;
                apprenticeship.FrameworkCode = frameworkOrStandard.FrameworkCode;
                apprenticeship.ProgType = frameworkOrStandard.ProgType;
                apprenticeship.PathwayCode = frameworkOrStandard.PathwayCode;
            }
            return apprenticeship;
        }
        
        public static ApprenticeshipQACompliance ToEntity(this AddEditApprenticeshipQAForComplianceModel model, ProviderPortalEntities db)
        {
            Apprenticeship apprenticeship = db.Apprenticeships.Find(model.ApprenticeshipId);

            ApprenticeshipQACompliance QA;
            if (model.ApprenticeshipQAComplianceId.HasValue)
            {
                QA = db.ApprenticeshipQACompliances.Find(model.ApprenticeshipQAComplianceId);
            }
            else
            {
                QA = new ApprenticeshipQACompliance
                {
                    ApprenticeshipId = model.ApprenticeshipId,
                    CreatedByUserId = Permission.GetCurrentUserId(),
                    CreatedDateTimeUtc = DateTime.UtcNow,
                    TextQAd = apprenticeship.MarketingInformation
                };
            }

            // If adding new properties here please also add them to the bulk upload
            // otherwise data loss will occur.  Bulk upload deletes and re-creates
            // apprenticeships.  It also deletes all QA records associated with
            // apprenticeships and these have to be re-added after the bulk upload has finished.
            // This is handled in the LoadApprenticeships method of the ApprenticeshipConverter class

            QA.DetailsOfUnverifiableClaim = model.DetailsOfUnverifiableClaim;
            QA.DetailsOfComplianceFailure = model.DetailsOfComplianceFailure;
            QA.Passed = model.Passed == "1";

            // Add the failure reasons
            foreach (Int32 frId in model.SelectedComplianceFailureReasons)
            {
                QAComplianceFailureReason fr = db.QAComplianceFailureReasons.Find(frId);
                if (fr != null)
                {
                    QA.QAComplianceFailureReasons.Add(fr);
                }
            }

            return QA;
        }

        public static ApprenticeshipQAStyle ToEntity(this AddEditApprenticeshipQAForStyleModel model, ProviderPortalEntities db)
        {
            Apprenticeship apprenticeship = db.Apprenticeships.Find(model.ApprenticeshipId);

            ApprenticeshipQAStyle QA;
            if (model.ApprenticeshipQAStyleId.HasValue)
            {
                QA = db.ApprenticeshipQAStyles.Find(model.ApprenticeshipQAStyleId);
            }
            else
            {
                QA = new ApprenticeshipQAStyle
                {
                    ApprenticeshipId = model.ApprenticeshipId,
                    CreatedByUserId = Permission.GetCurrentUserId(),
                    CreatedDateTimeUtc = DateTime.UtcNow,
                    TextQAd = apprenticeship.MarketingInformation
                };
            }

            // If adding new properties here please also add them to the bulk upload
            // otherwise data loss will occur.  Bulk upload deletes and re-creates
            // apprenticeships.  It also deletes all QA records associated with
            // apprenticeships and these have to be re-added after the bulk upload has finished.
            // This is handled in the LoadApprenticeships method of the ApprenticeshipConverter class

            QA.Passed = model.Passed == "1";
            QA.DetailsOfQA = model.DetailsOfQA;

            // Add the failure reasons
            foreach (Int32 frId in model.SelectedStyleFailureReasons)
            {
                QAStyleFailureReason fr = db.QAStyleFailureReasons.Find(frId);
                if (fr != null)
                {
                    QA.QAStyleFailureReasons.Add(fr);
                }
            }

            return QA;
        }
    }
}

