using System;
using System.Collections.Generic;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using System.Linq;

    using Entities;
    using Permission = Tribal.SkillsFundingAgency.ProviderPortal.Permission;

    public static class ProviderModelExtensions
    {
        /// <summary>
        /// Convert an <see cref="AddEditProviderModel"/> to an <see cref="Provider"/>.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <returns>
        /// The <see cref="Provider"/>.
        /// </returns>        
        public static Provider ToEntity(this AddEditProviderModel model, ProviderPortalEntities db)
        {
            Provider provider;

            if (model.ProviderId == null)
            {
                provider = new Provider
                {
                    Ukprn = model.UKPRN.HasValue ? model.UKPRN.Value : 0,
                    CreatedByUserId = Permission.GetCurrentUserId(), 
                    CreatedDateTimeUtc = DateTime.UtcNow,
                    BulkUploadPending = false,
                    PublishData = true,
                    ProviderTypeId = model.ProviderTypeId,
                    MarketingInformationUpdatedDateUtc = String.IsNullOrWhiteSpace(model.MarketingInformation) ? (DateTime?)null : DateTime.UtcNow
                };
            }
            else
            {
                provider = db.Providers.Find(model.ProviderId);
                if (provider == null)
                {
                    return null;
                }
            }

            provider.IsContractingBody = model.IsContractingBody;

            var canEditSpecialFields = Permission.HasPermission(false, true,
                Permission.PermissionName.CanEditProviderSpecialFields);
            if (canEditSpecialFields && model.ProviderId != null)
            {
                provider.ProviderTypeId = model.ProviderTypeId; 
                provider.DFE1619Funded = model.DFE1619Funded;
                provider.SFAFunded = model.SFAFunded;
                provider.BulkUploadPending = model.BulkUploadPending;
                provider.PublishData = model.PublishData;
                provider.SecureAccessId = model.SecureAccessId;
                provider.RoATPFFlag = model.RoATP;
                provider.TradingName = model.TradingName;
            }
            if (model.ProviderId == null)
            {
                provider.DFE1619Funded = model.DFE1619Funded;
                provider.SFAFunded = model.SFAFunded;
            }

            provider.ProviderName = model.ProviderName;
            provider.ProviderNameAlias = model.ProviderAlias;
            provider.Loans24Plus = model.Loans24Plus;
            provider.UPIN = model.UPIN;
            provider.ProviderRegionId = model.ProviderRegionId;
            provider.Email = model.Email;
            provider.Website = UrlHelper.GetFullUrl(model.Website);
            provider.Telephone = model.Telephone;
            provider.Fax = model.Fax;
            provider.ProviderTrackingUrl = model.ProviderTrackingUrl;
            provider.VenueTrackingUrl = model.VenueTrackingUrl;
            provider.CourseTrackingUrl = model.CourseTrackingUrl;
            provider.BookingTrackingUrl = model.BookingTrackingUrl;
            provider.DfEProviderTypeId = model.DfEProviderTypeId;
            provider.DfEProviderStatusId = model.DfEProviderStatusId;
            provider.DfELocalAuthorityId = model.DfELocalAuthorityId;
            provider.DfERegionId = model.DfERegionId;
            provider.DfEEstablishmentTypeId = model.DfEEstablishmentTypeId;
            provider.ApprenticeshipContract = model.ApprenticeshipContract;
            provider.TASRefreshOverride = model.TASRefreshOverride;
            provider.NationalApprenticeshipProvider = model.NationalApprenticeshipProvider;
            
            if (!provider.PassedOverallQAChecks || Permission.HasPermission(false, false, Permission.PermissionName.CanQAProviders))
            {
                String oldMarketingInformation = provider.MarketingInformation;
                provider.MarketingInformation = Markdown.Sanitize(model.MarketingInformation);
                if (provider.MarketingInformation != oldMarketingInformation)
                {
                    provider.MarketingInformationUpdatedDateUtc = DateTime.UtcNow;
                }
            }

            return provider;
        }

        public static void WithDfELookupData(this AddEditProviderModel model, ProviderPortalEntities db)
        {
            model.ProviderTypeDfE1619 = db.ProviderTypes.First(pt => pt.ProviderTypeName == "DfE 16-19").ProviderTypeId;
        }

        public static ProviderQACompliance ToEntity(this AddEditProviderQAForComplianceModel model, ProviderPortalEntities db)
        {
            Provider Provider = db.Providers.Find(model.ProviderId);

            ProviderQACompliance QA;
            if (model.ProviderQAComplianceId.HasValue)
            {
                QA = db.ProviderQACompliances.Find(model.ProviderQAComplianceId);
            }
            else
            {
                QA = new ProviderQACompliance
                {
                    ProviderId = model.ProviderId,
                    CreatedByUserId = Permission.GetCurrentUserId(),
                    CreatedDateTimeUtc = DateTime.UtcNow,
                    TextQAd = Provider.MarketingInformation
                };
            }

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

        public static ProviderQAStyle ToEntity(this AddEditProviderQAForStyleModel model, ProviderPortalEntities db)
        {
            Provider Provider = db.Providers.Find(model.ProviderId);

            ProviderQAStyle QA;
            if (model.ProviderQAStyleId.HasValue)
            {
                QA = db.ProviderQAStyles.Find(model.ProviderQAStyleId);
            }
            else
            {
                QA = new ProviderQAStyle
                {
                    ProviderId = model.ProviderId,
                    CreatedByUserId = Permission.GetCurrentUserId(),
                    CreatedDateTimeUtc = DateTime.UtcNow,
                    TextQAd = Provider.MarketingInformation
                };
            }

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


        public static ProviderUnableToComplete ToEntity(this AddEditProviderUnableToCompleteModel model, ProviderPortalEntities db)
        {
            Provider Provider = db.Providers.Find(model.ProviderId);

            ProviderUnableToComplete unableToComplete;
            if (model.ProviderUnableToCompleteId.HasValue)
            {
                unableToComplete = db.ProviderUnableToCompletes.Find(model.ProviderUnableToCompleteId);
            }
            else
            {
                unableToComplete = new ProviderUnableToComplete
                {
                    ProviderId = model.ProviderId,
                    CreatedByUserId = Permission.GetCurrentUserId(),
                    CreatedDateTimeUtc = DateTime.UtcNow,
                    TextUnableToComplete = model.TextUnableToComplete
                };
            }

            
            // Add the failure reasons
            foreach (Int32 frId in model.SelectedUnableToCompleteFailureReasons)
            {
                UnableToCompleteFailureReason fr = db.UnableToCompleteFailureReasons.Find(frId);
                if (fr != null)
                {
                    unableToComplete.UnableToCompleteFailureReasons.Add(fr);
                }
            }

            return unableToComplete;
        }


        public static Int32 GetQualityAssuredApprenticeshipCount(this Provider provider)
        {
            return provider.Apprenticeships.Count(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live && x.ApprenticeshipQACompliances.Count() > 0);
        }

        public static Int32 GetQualityAssuredApprenticeshipPassedCount(this Provider provider)
        {
            return provider.Apprenticeships.Count(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live && x.ApprenticeshipQACompliances.Count() > 0 && x.ApprenticeshipQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().Passed);
        }

        public static Int32 GetNumberOfApprenticeshipsRequiredToQA(this Provider provider)
        {
            return Constants.ConfigSettings.GetNumberOfApprenticeshipsToQA(provider.Apprenticeships.Count(x => x.RecordStatusId == (Int32) Constants.RecordStatus.Live));
        }
    }

    public static class AddEditProviderModelExtensions
    {
        public static AddEditProviderModel Populate(this AddEditProviderModel model, ProviderPortalEntities db)
        {
            model.Address = model.Address ?? new AddressViewModel();
            model.Address.Populate(db);
            return model;
        }

        public static List<String> GetWarningMessages(this AddEditProviderModel model)
        {
            List<String> messages = new List<String>();

            if (!String.IsNullOrWhiteSpace(model.Website) && !UrlHelper.UrlIsReachable(model.Website))
            {
                messages.Add(String.Format(AppGlobal.Language.GetText("AddEditProviderModel_Edit_UrlNotReachable", "The web address for {0} returns a response that suggests this page may not exist. Please check that the web address entered is correct."), AppGlobal.Language.GetText("AddEditProviderModel_DisplayName_Website", "Website")));
            }

            return messages;
        }
    }
}
