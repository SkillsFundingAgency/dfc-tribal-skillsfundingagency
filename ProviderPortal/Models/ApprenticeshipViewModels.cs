using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    /// <summary>
    /// Apprenticeship search view model.
    /// </summary>
    public class ApprenticeshipSearchViewModel
    {
        [LanguageDisplay("Framework / Standard Name")]
        public String FrameworkOrStandard { get; set; }

        public string FrameworkOrStandardId { get; set; }

        [LanguageDisplay("Location")]
        public Int32? LocationId { get; set; }

        public List<SelectListItem> Locations { get; set; }

        [LanguageDisplay("Delivery Mode")]
        public Int32? DeliveryModeId { get; set; }

        public List<SelectListItem> DeliveryModes { get; set; }
    }

    /// <summary>
    /// Apprenticeship list view model item.
    /// </summary>
    public class ApprenticeshipListViewModelItem
    {
        public Int32 ApprenticeshipId { get; set; }

        [LanguageDisplay("Last Update")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime LastUpdate { get; set; }

        [LanguageDisplay("Status")]
        public String Status { get; set; }

        [LanguageDisplay("Apprenticeship Name")]
        public String ApprenticeshipDetails { get; set; }

        [LanguageDisplay("Compliance QA Result")]
        public String ComplianceQAResult { get; set; }

        [LanguageDisplay("Style QA Result")]
        public String StyleQAResult { get; set; }
    }

    /// <summary>
    /// Apprenticeship list page view model.
    /// </summary>
    public class ApprenticeshipListViewModel
    {
        public Boolean DisplayNotPublishedBanner { get; set; }

        public Int32 ApprenticeshipsQAed { get; set; }

        public Int32 NumberOfApprenticeshipsRequiredToQA { get; set; }

        public Boolean DataReadyForQA { get; set; }

        public ApprenticeshipSearchViewModel Search { get; set; }
        public List<ApprenticeshipListViewModelItem> Apprenticeships { get; set; }
    }

    /// <summary>
    /// Add/edit apprenticeship view model.
    /// </summary>
    public class AddEditApprenticeshipViewModel : IQualityAssurance
    {
        public Boolean DisplayNotPublishedBanner { get; set; }

        public Int32 ApprenticeshipId { get; set; }

        public Int32 RecordStatusId { get; set; }

        public Int32 ProviderId { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Live Framework / Standard Name on LARS")]
        [Display(Description = "Type the framework or standard name and the full title will appear, you must make a selection from this list.")]
        public String FrameworkOrStandard { get; set; }

        public string FrameworkOrStandardId { get; set; }

        public const Int32 MarketingInformationMaxLength = 900;

        [AllowHtml]
        [LanguageRequired]
        [Display(Description = "Enter information about how your organisation delivers this apprenticeship that employers would find useful (max 750 characters).")]
        // *************************************************************************************************
        // The following 2 strings must also be changed in ApprenticeshipController.RemoveSpellCheckHTMLFromMarketingInformation
        [LanguageDisplay("Your Apprenticeship Information for Employers")]
        [LanguageStringLength(MarketingInformationMaxLength, ErrorMessage = "The maximum length of {0} is 750 characters.")]
        // *************************************************************************************************
        public String MarketingInformation { get; set; }

        [LanguageDisplay("Your Apprenticeship Website Page")]
        [Display(Description = "Enter a link to an employer focused page on your organisation's website: ideally about: 1) this specific apprenticeship or 2) your apprenticeships in general or 3) your home page.")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [ProviderPortalUrl(ErrorMessage = "Please include the full URL including http://www")]
        //[DataType(DataType.Url)]
        public String Url { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Your Apprenticeship Contact Telephone")]
        [DataType(DataType.PhoneNumber)]
        [Display(Description = "Enter your organisation's telephone number for handling employer queries about this apprenticeship.")]
        [LanguageStringLength(30, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public String ContactTelephone { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Your Apprenticeship Contact Email")]
        [Display(Description = "Enter your organisation's email address (not named individuals) for handling employer queries about this apprenticeship.")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [LanguageEmailAddress]
        [DataType(DataType.EmailAddress)]
        public String ContactEmail { get; set; }

        [LanguageDisplay(@"Your Apprenticeship Website ""Contact Us"" Page")]
        [Display(Description = @"Enter a link to a ""Contact us"" page on your organisation's website where an employer can enquire about this apprenticeship.")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [ProviderPortalUrl(ErrorMessage = "Please include the full URL including http://www")]
        //[DataType(DataType.Url)]
        public String ContactWebsite { get; set; }

        public DeliveryLocationListViewModel DeliveryLocations { get; set; }

        [LanguageDisplay("Quality Assured for Compliance")]
        [Display(Description = "Indicates whether the apprenticeship information text has been quality assured for compliance")]
        public Boolean HasBeenQAdForCompliance { get; set; }

        [LanguageDisplay("Last Quality Assured for Compliance By")]
        public String LastQAdForComplianceBy { get; set; }

        [LanguageDisplay("Last Quality Assured for Compliance On")]
        public DateTime? LastQAdForComplianceOn { get; set; }

        [LanguageDisplay("Passed Compliance Checks")]
        [Display(Description = "Indicates whether the apprenticeship information text has passed compliance quality assurance checks")]
        public Boolean HasPassedComplianceChecks { get; set; }

        [LanguageDisplay("Quality Assured for Style")]
        [Display(Description = "Indicates whether the apprenticeship information text has been quality assured for style")]
        public Boolean HasBeenQAdForStyle { get; set; }

        [LanguageDisplay("Last Quality Assured for Style By")]
        public String LastQAdForStyleBy { get; set; }

        [LanguageDisplay("Last Quality Assured for Style On")]
        public DateTime? LastQAdForStyleOn { get; set; }

        [LanguageDisplay("Passed Style Checks")]
        [Display(Description = "Indicates whether the apprenticeship information text has passed style quality assurance checks")]
        public Boolean HasPassedStyleChecks { get; set; }

        public Boolean IsLive()
        {
            return RecordStatusId == (Int32)Constants.RecordStatus.Live;
        }
    }

    public class ViewApprenticeshipModel : AddEditApprenticeshipViewModel
    {
        [LanguageDisplay("Record Status")]
        public String RecordStatusName { get; set; }

        public ViewApprenticeshipModel()
        { }

        public ViewApprenticeshipModel(Apprenticeship apprenticeship)
        {
            this.ProviderId = apprenticeship.ProviderId;
            this.FrameworkOrStandard = apprenticeship.ApprenticeshipDetails();
            this.MarketingInformation = apprenticeship.MarketingInformation;
            this.Url = apprenticeship.Url;
            this.ContactEmail = apprenticeship.ContactEmail;
            this.ContactTelephone = apprenticeship.ContactTelephone;
            this.ContactWebsite = UrlHelper.GetFullUrl(apprenticeship.ContactWebsite);
            this.RecordStatusName = apprenticeship.RecordStatu.RecordStatusName;
            this.DisplayNotPublishedBanner = !apprenticeship.Provider.ApprenticeshipContract;

            // Get delivery Locations
            if (this.DeliveryLocations == null)
            {
                this.DeliveryLocations = new DeliveryLocationListViewModel();
            }
            this.DeliveryLocations.Items = apprenticeship.ApprenticeshipLocations
                .Where(x => x.ApprenticeshipId == apprenticeship.ApprenticeshipId)
                .Select(x => new DeliveryLocationListViewModelItem
                {
                    ApprenticeshipLocationId = x.ApprenticeshipLocationId,
                    ProviderOwnLocationRef = x.Location.ProviderOwnLocationRef,
                    LocationName = x.Location.LocationName,
                    DeliveryModes = x.DeliveryModes.Select(y => y.DeliveryModeName).OrderBy(y => y),
                    Radius = x.Radius,
                    Status = x.RecordStatu.RecordStatusName,
                    LastUpdate = x.ModifiedDateTimeUtc ?? x.CreatedDateTimeUtc
                })
                .OrderByDescending(x => x.LastUpdate)
                .ToList();

            this.HasBeenQAdForCompliance = apprenticeship.ApprenticeshipQACompliances.Any();
            this.LastQAdForComplianceBy = !apprenticeship.ApprenticeshipQACompliances.Any() ? null : apprenticeship.ApprenticeshipQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().AspNetUser.Name;
            this.LastQAdForComplianceOn = !apprenticeship.ApprenticeshipQACompliances.Any() ? (DateTime?)null : DateTime.SpecifyKind(apprenticeship.ApprenticeshipQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().CreatedDateTimeUtc, DateTimeKind.Utc);
            this.HasPassedComplianceChecks = apprenticeship.ApprenticeshipQACompliances.Any() && apprenticeship.ApprenticeshipQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().Passed;

            this.HasBeenQAdForStyle = apprenticeship.ApprenticeshipQAStyles.Any();
            this.LastQAdForStyleBy = !apprenticeship.ApprenticeshipQAStyles.Any() ? null : apprenticeship.ApprenticeshipQAStyles.OrderByDescending(m => m.CreatedDateTimeUtc).First().AspNetUser.Name;
            this.LastQAdForStyleOn = !apprenticeship.ApprenticeshipQAStyles.Any() ? (DateTime?)null : DateTime.SpecifyKind(apprenticeship.ApprenticeshipQAStyles.OrderByDescending(m => m.CreatedDateTimeUtc).First().CreatedDateTimeUtc, DateTimeKind.Utc);
            this.HasPassedStyleChecks = apprenticeship.ApprenticeshipQAStyles.Any() && apprenticeship.ApprenticeshipQAStyles.OrderByDescending(m => m.CreatedDateTimeUtc).First().Passed;
        }
    }

    public class AddEditApprenticeshipQAForComplianceModel
    {
        public Int32? ApprenticeshipQAComplianceId { get; set; }

        public Int32 ApprenticeshipId { get; set; }

        [LanguageDisplay("Further details about unverifiable claim")]
        [Display(Description = "Enter details about the unverifiable claim made")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(2000, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public String DetailsOfUnverifiableClaim { get; set; }

        [LanguageDisplay("Further details about compliance fails")]
        [Display(Description = "Enter details about the compliance failure")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(2000, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public String DetailsOfComplianceFailure { get; set; }

        [LanguageDisplay("Reason for Failure")]
        [Display(Description = "Select the reason(s) for QA failure")]
        public IEnumerable<QAComplianceFailureReason> QAComplianceFailureReasons { get; set; }

        [LanguageRequired]
        public List<Int32> SelectedComplianceFailureReasons { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Passed compliance checks")]
        [Display(Description = "Select a QA check result")]
        public String Passed { get; set; }

        public AddEditApprenticeshipQAForComplianceModel()
        {
            SelectedComplianceFailureReasons = new List<Int32>();
        }

        public AddEditApprenticeshipQAForComplianceModel(ApprenticeshipQACompliance apprenticeshipQACompliance) : this()
        {
            this.ApprenticeshipQAComplianceId = apprenticeshipQACompliance.ApprenticeshipQAComplianceId;
            this.ApprenticeshipId = apprenticeshipQACompliance.ApprenticeshipId;
            this.DetailsOfUnverifiableClaim = apprenticeshipQACompliance.DetailsOfUnverifiableClaim;
            this.DetailsOfComplianceFailure = apprenticeshipQACompliance.DetailsOfComplianceFailure;
            this.Passed = apprenticeshipQACompliance.Passed ? "1" : "0";
            foreach (QAComplianceFailureReason fr in apprenticeshipQACompliance.QAComplianceFailureReasons)
            {
                this.SelectedComplianceFailureReasons.Add(fr.QAComplianceFailureReasonId);
            }
        }
    }

    public class ApprenticeshipQAForComplianceJsonModel
    {
        public Int32 ApprenticeshipQAForComplianceId { get; set; }
        public Boolean Passed { get; set; }

        public ApprenticeshipQAForComplianceJsonModel()
        {
        }

        public ApprenticeshipQAForComplianceJsonModel(ApprenticeshipQACompliance apprenticeshipQACompliance)
            : this()
        {
            this.ApprenticeshipQAForComplianceId = apprenticeshipQACompliance.ApprenticeshipQAComplianceId;
            this.Passed = apprenticeshipQACompliance.Passed;
        }
    }

    public class AddEditApprenticeshipQAForStyleModel
    {
        public Int32? ApprenticeshipQAStyleId { get; set; }

        public Int32 ApprenticeshipId { get; set; }

        [LanguageDisplay("Reason for Failure")]
        [Display(Description = "Select the reason(s) for QA failure")]
        public IEnumerable<QAStyleFailureReason> QAStyleFailureReasons { get; set; }

        [LanguageRequired]
        public List<Int32> SelectedStyleFailureReasons { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Passed style checks")]
        [Display(Description = "Select a QA check result")]
        public String Passed { get; set; }


        [LanguageDisplay("Further style fail details")]
        [Display(Description = "")]
        public String DetailsOfQA { get; set; }


        public AddEditApprenticeshipQAForStyleModel()
        {
            SelectedStyleFailureReasons = new List<Int32>();
        }

        public AddEditApprenticeshipQAForStyleModel(ApprenticeshipQAStyle apprenticeshipQAStyle)
            : this()
        {
            this.ApprenticeshipQAStyleId = apprenticeshipQAStyle.ApprenticeshipQAStyleId;
            this.ApprenticeshipId = apprenticeshipQAStyle.ApprenticeshipId;
            this.Passed = apprenticeshipQAStyle.Passed ? "1" : "0";
            foreach (QAStyleFailureReason fr in apprenticeshipQAStyle.QAStyleFailureReasons)
            {
                this.SelectedStyleFailureReasons.Add(fr.QAStyleFailureReasonId);
            }
        }
    }

    public class ApprenticeshipQAForStyleJsonModel
    {
        public Int32 ApprenticeshipQAForComplianceId { get; set; }
        public Boolean Passed { get; set; }

        public ApprenticeshipQAForStyleJsonModel()
        {
        }

        public ApprenticeshipQAForStyleJsonModel(ApprenticeshipQAStyle apprenticeshipQAStyle)
            : this()
        {
            this.ApprenticeshipQAForComplianceId = apprenticeshipQAStyle.ApprenticeshipQAStyleId;
            this.Passed = apprenticeshipQAStyle.Passed;
        }
    }
}