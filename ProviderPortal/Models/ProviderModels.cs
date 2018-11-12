using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Models;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using System.Web;

    public class AddEditProviderModel : IQualityAssurance
    {
        public AddEditProviderModel()
        {
            Address = new AddressViewModel
            {
                RegionId = 0
            };
            UKRLPData = new UKRLPDataModel();
            PublishData = true;
        }

        public AddEditProviderModel(Provider provider, bool includeDisplayFields = false)
        {
            ProviderId = provider.ProviderId;
            UKPRN = provider.Ukprn;
            RecordStatusId = provider.RecordStatusId;
            IsContractingBody = provider.IsContractingBody;
            ProviderTypeId = provider.ProviderTypeId;
            ProviderTypeName = provider.ProviderType.ProviderTypeName;
            ProviderName = provider.ProviderName;
            TradingName = provider.TradingName;
            ProviderAlias = provider.ProviderNameAlias;
            UPIN = provider.UPIN;
            Loans24Plus = provider.Loans24Plus;
            ProviderRegionId = provider.ProviderRegionId;
            Email = provider.Email;
            Website = provider.Website;
            Telephone = provider.Telephone;
            Fax = provider.Fax;
            ProviderTrackingUrl = provider.ProviderTrackingUrl;
            VenueTrackingUrl = provider.VenueTrackingUrl;
            CourseTrackingUrl = provider.CourseTrackingUrl;
            BookingTrackingUrl = provider.BookingTrackingUrl;
            DFE1619Funded = provider.DFE1619Funded;
            SFAFunded = provider.SFAFunded;
            DfEProviderTypeId = DFE1619Funded ? provider.DfEProviderTypeId : null;
            DfEProviderStatusId = DFE1619Funded ? provider.DfEProviderStatusId : null;
            DfELocalAuthorityId = DFE1619Funded ? provider.DfELocalAuthorityId : null;
            DfERegionId = DFE1619Funded ? provider.DfERegionId : null;
            DfEEstablishmentTypeId = DFE1619Funded ? provider.DfEEstablishmentTypeId : null;
            SecureAccessId = provider.SecureAccessId;

            QualityEmailsPaused = provider.QualityEmailsPaused;
            QualityEmailStatusId = provider.QualityEmailStatusId;

            Address = provider.AddressId == null
                ? new AddressViewModel {RegionId = 0}
                : new AddressViewModel(provider.Address) {RegionId = provider.ProviderRegionId ?? 0};

            UKRLPData = new UKRLPDataModel();

            BulkUploadPending = provider.BulkUploadPending;
            PublishData = provider.PublishData;

            ApprenticeshipContract = provider.ApprenticeshipContract;
            TASRefreshOverride = provider.TASRefreshOverride;
            NationalApprenticeshipProvider = provider.NationalApprenticeshipProvider;
            MarketingInformation = provider.MarketingInformation;

            if (includeDisplayFields)
            {
                RecordStatusName = provider.RecordStatu.RecordStatusName;

                DfEProviderTypeName = provider.DfEProviderType == null
                    ? null
                    : provider.DfEProviderType.DfEProviderTypeName;
                DfEProviderStatusName = provider.DfEProviderStatu == null
                    ? null
                    : provider.DfEProviderStatu.DfEProviderStatusName;
                DfELocalAuthorityName = provider.DfELocalAuthority == null
                    ? null
                    : provider.DfELocalAuthority.DfELocalAuthorityName;
                DfERegionName = provider.DfERegion == null ? null : provider.DfERegion.DfERegionName;
                DfEEstablishmentTypeName = provider.DfEEstablishmentType == null
                    ? null
                    : provider.DfEEstablishmentType.DfEEstablishmentTypeName;

                if (provider.ProviderRegion != null) ProviderRegionName = provider.ProviderRegion.RegionName;
            }

            this.RoATP = provider.RoATPFFlag;
            RoATPProviderTypeName = provider.RoATPProviderType == null ? AppGlobal.Language.GetText("AddEditProviderModel_RoATP_RoATPProviderTypeNone", "(none)") : provider.RoATPProviderType.Description;
            RoATPStartDate = provider.RoATPStartDate == null ? AppGlobal.Language.GetText("AddEditProviderModel_RoATP_RoATPStartDateNA", "n/a") : provider.RoATPStartDate.Value.ToString(Constants.ConfigSettings.ShortDateFormat);

            this.HasBeenQAdForCompliance = provider.ProviderQACompliances.Any();
            this.LastQAdForComplianceBy = !provider.ProviderQACompliances.Any() ? null : provider.ProviderQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().AspNetUser.Name;
            this.LastQAdForComplianceOn = !provider.ProviderQACompliances.Any() ? (DateTime?)null : DateTime.SpecifyKind(provider.ProviderQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().CreatedDateTimeUtc, DateTimeKind.Utc);
            this.HasPassedComplianceChecks = provider.ProviderQACompliances.Any() && provider.ProviderQACompliances.OrderByDescending(m => m.CreatedDateTimeUtc).First().Passed;

            this.HasBeenQAdForStyle = provider.ProviderQAStyles.Any();
            this.LastQAdForStyleBy = !provider.ProviderQAStyles.Any() ? null : provider.ProviderQAStyles.OrderByDescending(m => m.CreatedDateTimeUtc).First().AspNetUser.Name;
            this.LastQAdForStyleOn = !provider.ProviderQAStyles.Any() ? (DateTime?)null : DateTime.SpecifyKind(provider.ProviderQAStyles.OrderByDescending(m => m.CreatedDateTimeUtc).First().CreatedDateTimeUtc, DateTimeKind.Utc);
            this.HasPassedStyleChecks = provider.ProviderQAStyles.Any() && provider.ProviderQAStyles.OrderByDescending(m => m.CreatedDateTimeUtc).First().Passed;

            this.UnableToCompleteProcess = provider.ProviderUnableToCompletes.Where(m => m.Active == true).Count() > 0;
            if (this.UnableToCompleteProcess)
            {
                this.UnableToCompleteDate = DateTime.SpecifyKind(provider.ProviderUnableToCompletes.Where(m => m.Active == true).OrderByDescending(m => m.CreatedDateTimeUtc).First().CreatedDateTimeUtc, DateTimeKind.Utc);
                this.UnableToCompleteName = provider.ProviderUnableToCompletes.Where(m => m.Active == true).OrderByDescending(m => m.CreatedDateTimeUtc).First().AspNetUser.Name;
            }

            this.PassedOverallQAChecks = provider.PassedOverallQAChecks ? "1": "0";
            this.ApprenticeshipsQAed = provider.GetQualityAssuredApprenticeshipCount();
            this.ApprenticeshipPassedQA = provider.GetQualityAssuredApprenticeshipPassedCount();
            this.NumberOfApprenticeshipsRequiredToQA = provider.GetNumberOfApprenticeshipsRequiredToQA();

            this.NumberOfLocations = provider.Locations.Count();
            this.MaxNumberOfLocations = provider.MaxLocations;
            this.MaxNumberOfLocationsOverriddenBy = provider.MaxLocationsUser != null ? provider.MaxLocationsUser.Name : null;
            this.MaxNumberOfLocationsOverriddenOn = provider.MaxLocationsDateTimeUtc.HasValue ? DateTime.SpecifyKind(provider.MaxLocationsDateTimeUtc.Value, DateTimeKind.Utc) : (DateTime?) null;

            this.DataReadyForQA = provider.DataReadyToQA;

            this.ShowSendFailedQAEmailButton = !provider.PassedOverallQAChecks && this.DataReadyForQA && this.HasBeenQAdForCompliance;

            this.CreatedDateTimeUTC = provider.CreatedDateTimeUtc;

            ImportBatches = provider.ImportBatches.OrderByDescending(x => x.ImportBatch.ImportBatchId).ToList();

            var latestRoATPRefreshed = provider.ProviderTASRefreshes.OrderByDescending(p => p.RefreshTimeUtc).FirstOrDefault();
            if (latestRoATPRefreshed != null)
            {
                RoATPLastRefreshed = DateTime.SpecifyKind(latestRoATPRefreshed.RefreshTimeUtc, DateTimeKind.Utc);
            }

        }

        [LanguageDisplay("Provider Id")]
        public Int32? ProviderId { get; set; }

        [LanguageDisplay("Status")]
        [Display(Description = "Provider status")]
        public Int32? RecordStatusId { get; set; }

        // Just used for displaying the provider details
        public String RecordStatusName { get; set; }

        [LanguageRequired]
        [DataType(DataType.Text)]
        [Display(
            Description =
                "United Kingdom Provider Reference Number (UKPRN). This number is the unique identifier for this provider from the United Kingdom Register of Learning Providers (UKRLP). For more information see http://www.ukrlp.co.uk/."
            )]
        [LanguageDisplay("UKPRN")]
        public Int32? UKPRN { get; set; }

        [LanguageDisplay("Contracting Body")]
        [Display(
            Description =
                "This field should be checked if this Provider has a contract with the Skills Funding Agency to deliver provision."
            )]
        public Boolean IsContractingBody { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Provider Type")]
        [Display(Description = "Please select the most appropriate type from the list.")]
        public Int32 ProviderTypeId { get; set; }

        // Just used for displaying the provider details
        public String ProviderTypeName { get; set; }

        [LanguageDisplay("RoATP Provider Type")]
        [Display(Description = "Provider type from RoATP")]
        public String RoATPProviderTypeName { get; set; }

        [LanguageDisplay("RoATP Start Date")]
        [Display(Description = "Start date from RoATP")]
        public String RoATPStartDate { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Provider Name")]
        [LanguageStringLength(100, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter the name of your Provider.")]
        public String ProviderName { get; set; }

        [LanguageDisplay("Trading Name")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter the trading name of your Provider.")]
        public String TradingName { get; set; }

        [LanguageDisplay("Provider Alias")]
        [LanguageStringLength(100, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please amend the name of your Provider.")]
        public String ProviderAlias { get; set; }

        [LanguageDisplay("UPIN")]
        [Display(
            Description =
                "The Unique Provider Identification Number (UPIN) is a reference number assigned by the Provider Information Management System (PIMS) to each provider contracted by the Skills Funding Agency."
            )]
        [DataType(DataType.Text)]
        public Int32? UPIN { get; set; }

        [LanguageDisplay("24+ Loans")]
        [Display(
            Description =
                "This field should be checked if the Provider/Organisation offers loans under the 24+ Learning Loans scheme."
            )]
        public Boolean Loans24Plus { get; set; }

        [LanguageDisplay("Region")]
        [Display(Description = "Please select Region")]
        public Int32? ProviderRegionId { get; set; }

        // Just used for displaying the provider details
        public String ProviderRegionName { get; set; }

        public AddressViewModel Address { get; set; }

        [LanguageDisplay("Email Address")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter one main email address for your provider.")]
        [LanguageEmailAddress]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [LanguageDisplay("Website")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter your provider's website address.")]
        public String Website { get; set; }

        [LanguageDisplay("Telephone")]
        [LanguageStringLength(30, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter your provider's admissions telephone number.")]
        public String Telephone { get; set; }

        [LanguageDisplay("Fax")]
        [LanguageStringLength(30, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter your provider's fax number.")]
        public String Fax { get; set; }

        // Dummy item to contain the tooltip for the tracking codes
        [Display(
            Description =
                "Because the National Careers Service is behind a secure URL, it will not be listed as a referrer in your web analytics tools. However, if tracking codes are added to your URLs, you can easily identify which visits to your site came from the NCS. The Course Directory can do this automatically for your URLs by populating the fields below. The tracking codes are designed for use with Google Analytics, but will work just as well for most other analytics tools, and you can change them to any codes that are useful to you. More information on Google tracking codes can be found at: https://support.google.com/analytics/answer/1033867?hl=en-GB"
            )]
        public bool TrackingCodes { get; set; }


        // Dummy item to contain the tooltip for the submit new text button
        [Display(
            Description =
                "Submit new text for QA, to replace text currently locked."
            )]
        public bool SubmitNewTextToolTip { get; set; }

        [LanguageDisplay("Provider URLs")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public String ProviderTrackingUrl { get; set; }

        [LanguageDisplay("Venue URLs")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public String VenueTrackingUrl { get; set; }

        [LanguageDisplay("Course URLs")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public String CourseTrackingUrl { get; set; }

        [LanguageDisplay("Booking URLs")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public String BookingTrackingUrl { get; set; }

        [LanguageDisplay("Pause automated quality emails")]
        public Boolean QualityEmailsPaused { get; set; }

        [LanguageDisplay("Automated quality email status")]
        public Int32? QualityEmailStatusId { get; set; }

        public UKRLPDataModel UKRLPData { get; set; }

        [LanguageDisplay("SFA Funded")]
        public Boolean SFAFunded { get; set; }

        [LanguageDisplay("DfE EFA Funded")]
        public Boolean DFE1619Funded { get; set; }

        [LanguageDisplay("DfE Provider Type")]
        [Display(Description = "Please select the most appropriate type from the list.")]
        public Int32? DfEProviderTypeId { get; set; }

        public String DfEProviderTypeName { get; set; }

        [LanguageDisplay("DfE Provider Status")]
        [Display(Description = "Please select the most appropriate status from the list.")]
        public Int32? DfEProviderStatusId { get; set; }

        public String DfEProviderStatusName { get; set; }

        [LanguageDisplay("DfE Local Authority")]
        [Display(Description = "Please select the provider's Local Authority from the list.")]
        public Int32? DfELocalAuthorityId { get; set; }

        public String DfELocalAuthorityName { get; set; }

        [LanguageDisplay("DfE Region")]
        [Display(Description = "Please select the provider's DfE Region from the list.")]
        public Int32? DfERegionId { get; set; }

        public String DfERegionName { get; set; }

        [LanguageDisplay("DfE Estab. Type")]
        [Display(Description = "Please select the provider's type of establishment from the list.")]
        public Int32? DfEEstablishmentTypeId { get; set; }

        public String DfEEstablishmentTypeName { get; set; }

        [LanguageDisplay("Secure Access Id")]
        [Display(Description = "The provider's unique identifier within the Secure Access authentication system.")]
        [DataType(DataType.Text)]
        public Int32? SecureAccessId { get; set; }

        [LanguageDisplay("Bulk Upload Pending")]
        [Display(Description = "A bulk upload is pending.")]
        public bool BulkUploadPending { get; set; }

        public int ProviderTypeDfE1619 { get; set; }

        [LanguageDisplay("Publish Data")]
        [Display(
            Description =
                "When enabled the provider's course data will appear in both the Search API and Open Data extracts. If this option is not enabled the provider is given the opportunity to opt-into having their data published."
            )]
        public bool PublishData { get; set; }

        [LanguageDisplay("Current contract with SFA")]
        [Display(Description = "")]
        public Boolean ApprenticeshipContract { get; set; }

        [LanguageDisplay("National")]
        [Display(Description = "Large provider with extensive range of apprenticeships, flexible delivery options, providing training to a large part of England")]
        public Boolean NationalApprenticeshipProvider { get; set; }

        public const Int32 MarketingInformationMaxLength = 900;
        [AllowHtml]
        [Display(Description = "Enter a brief introductory overview of your organisation and how it provides apprenticeships training. This must be information that employers will find useful e.g. what type of training organisation you are, how long you have been providing apprenticeship training, etc. Employers will view this information on all search results you are returned in (max 750 characters).")]
        // *************************************************************************************************
        // The following 2 strings must also be changed in ProviderController.RemoveSpellCheckHTMLFromMarketingInformation
        [LanguageDisplay("Your Generic Apprenticeship Information for Employers")]
        [LanguageStringLength(MarketingInformationMaxLength, ErrorMessage = "The maximum length of {0} is 750 characters.")]
        // *************************************************************************************************
        public string MarketingInformation { get; set; }

        public string DfELookupAreaStyle
        {
            get { return DFE1619Funded ? String.Empty : "display: none;"; }
        }

        [LanguageDisplay("Quality Assured for Compliance")]
        [Display(Description = "Indicates whether the provider brief overview text has been quality assured for complicance")]
        public Boolean HasBeenQAdForCompliance { get; set; }

        [LanguageDisplay("Last Quality Assured for Compliance By")]
        public String LastQAdForComplianceBy { get; set; }

        [LanguageDisplay("Last Quality Assured for Compliance On")]
        public DateTime? LastQAdForComplianceOn { get; set; }

        [LanguageDisplay("Passed Compliance Checks")]
        [Display(Description = "Indicates whether the provider information text has passed compliance quality assurance checks")]
        public Boolean HasPassedComplianceChecks { get; set; }

        [LanguageDisplay("Quality Assured for Style")]
        [Display(Description = "Indicates whether the provider brief overview text has been quality assured for style")]
        public Boolean HasBeenQAdForStyle { get; set; }

        [LanguageDisplay("Last Quality Assured for Style By")]
        public String LastQAdForStyleBy { get; set; }

        [LanguageDisplay("Last Quality Assured for Style On")]
        public DateTime? LastQAdForStyleOn { get; set; }

        [LanguageDisplay("Passed Style Checks")]
        [Display(Description = "Indicates whether the provider brief overview text has passed style quality assurance checks")]
        public Boolean HasPassedStyleChecks { get; set; }

        [LanguageDisplay("Overall QA Passed")]
        [Display(Description = "Select overall QA check result")]
        public String PassedOverallQAChecks { get; set; }

        [LanguageDisplay("Number of Apprenticeships Quality Assured")]
        public Int32 ApprenticeshipsQAed { get; set; }

        [LanguageDisplay("Number of Apprenticeships Passed QA")]
        public Int32 ApprenticeshipPassedQA { get; set; }

        [LanguageDisplay("Number of Apprenticeships Required to QA")]
        public Int32 NumberOfApprenticeshipsRequiredToQA { get; set; }

        [LanguageDisplay("Has Provider Submitted Their Data for QA?")]
        [Display(Description = "Indicates whether the provider has submitted their data for quality assurance")]
        public Boolean DataReadyForQA { get; set; }

        [LanguageDisplay("Reported Unability To Complete On ")]
        public DateTime? UnableToCompleteDate { get; set; }

        [LanguageDisplay("Reported Unability To Complete by")]
        public String UnableToCompleteName { get; set; }

        [LanguageDisplay("Unable to complete process")]
        public Boolean UnableToCompleteProcess { get; set; }

        public Boolean ShowSendFailedQAEmailButton { get; set; }

        [LanguageDisplay("RoATP")]
        [Display(Description = "Has this provider applied to be on the Register of Apprenticeship Training Providers?")]
        public Boolean RoATP { get; set; }

        [LanguageDisplay("RoATP Import Batches")]
        [Display(Description = "The RoATP import batches that this provider has been included in")]
        public List<ImportBatchProvider> ImportBatches { get; set; }

        [LanguageDisplay("Date added")]
        public DateTime? CreatedDateTimeUTC { get; set; }

        [LanguageDisplay("Maximum Number of Locations")]
        [Display(Description = "Maximum number of location this provider is allowed to create")]
        public Int32? MaxNumberOfLocations { get; set; }

        [LanguageDisplay("Maximum Number of Locations Overridden By")]
        [Display(Description = "User who overrode the maximum number of locations for this provider")]
        public String MaxNumberOfLocationsOverriddenBy { get; set; }

        [LanguageDisplay("Maximum Number of Locations Overridden On")]
        [Display(Description = "Date and time that the maximum number of locations was overridden for this provider")]
        public DateTime? MaxNumberOfLocationsOverriddenOn { get; set; }

        [LanguageDisplay("Current Number of Locations")]
        [Display(Description = "Number of locations this provider currently has")]
        public Int32 NumberOfLocations { get; set; }

        public Boolean IsLive()
        {
            return RecordStatusId.HasValue && RecordStatusId == (Int32)Constants.RecordStatus.Live;
        }

        [LanguageDisplay("RoATP Refresh Confirmation Override")]
        [Display(Description = "Check this field to show the TAS Refresh Confirmation button outside of the refresh period")]
        public Boolean TASRefreshOverride { get; set; }

        public DateTime? RoATPLastRefreshed { get; }

    }

    public class ProviderDashboardModel
    {
        public ProviderDashboardModel()
        {
            Provider = new ProviderViewModel();
        }

        public ProviderDashboardModel(Provider provider)
        {
            Provider = new ProviderViewModel(provider);
            NumberOfPendingCourses = provider.Courses.Count(x => x.RecordStatusId == (Int32) Constants.RecordStatus.Pending);
        }

        public ProviderViewModel Provider { get; set; }

        [LanguageDisplay("Number of Pending Courses")]
        public Int32 NumberOfPendingCourses { get; set; }
    }

    public class ProviderViewModel
    {
        public ProviderViewModel()
        {
            Address = new AddressViewModel();
            Venues = new List<ListVenueModel>();
        }

        public ProviderViewModel(Provider provider)
        {
            ProviderId = provider.ProviderId;
            UKPRN = provider.Ukprn;
            RecordStatusId = provider.RecordStatusId;
            IsContractingBody = provider.IsContractingBody;
            ProviderName = provider.ProviderName;
            ProviderAlias = provider.ProviderNameAlias;
            UPIN = provider.UPIN;
            Loans24Plus = provider.Loans24Plus;
            Email = provider.Email;
            Website = provider.Website;
            Telephone = provider.Telephone;
            Fax = provider.Fax;
            ProviderTrackingUrl = provider.ProviderTrackingUrl;
            VenueTrackingUrl = provider.VenueTrackingUrl;
            CourseTrackingUrl = provider.CourseTrackingUrl;
            BookingTrackingUrl = provider.BookingTrackingUrl;
            DFE1619Funded = provider.DFE1619Funded;
            SFAFunded = provider.SFAFunded;
            DfEProviderTypeId = provider.DfEProviderTypeId;
            DfEProviderStatusId = provider.DfEProviderStatusId;
            DfELocalAuthorityId = provider.DfELocalAuthorityId;
            DfERegionId = provider.DfERegionId;
            DfEEstablishmentTypeId = provider.DfEEstablishmentTypeId;
            SecureAccessId = provider.SecureAccessId;
            BulkUploadPending = provider.BulkUploadPending;
            PublishData = provider.PublishData;

            ApprenticeshipContract = provider.ApprenticeshipContract;
            NationalApprenticeshipProvider = provider.NationalApprenticeshipProvider;
            MarketingInformation = provider.MarketingInformation;

            Address = provider.AddressId == null
                ? new AddressViewModel {RegionId = 0}
                : new AddressViewModel(provider.Address);

            Venues = new List<ListVenueModel>();

            foreach (Venue v in provider.Venues)
            {
                Venues.Add(new ListVenueModel(v));
            }

            this.PassedOverallQAChecks = provider.PassedOverallQAChecks ? "1" : "0";
        }

        public Int32? ProviderId { get; set; }
        public Int32? RecordStatusId { get; set; }
        public Int32? UKPRN { get; set; }
        public Boolean IsContractingBody { get; set; }
        public String ProviderName { get; set; }
        public String ProviderAlias { get; set; }
        public Int32? UPIN { get; set; }
        public Boolean Loans24Plus { get; set; }
        public AddressViewModel Address { get; set; }
        public String Email { get; set; }
        public String Website { get; set; }
        public String Telephone { get; set; }
        public String Fax { get; set; }
        public String ProviderTrackingUrl { get; set; }
        public String VenueTrackingUrl { get; set; }
        public String CourseTrackingUrl { get; set; }
        public String BookingTrackingUrl { get; set; }
        public Boolean DFE1619Funded { get; set; }
        public Boolean SFAFunded { get; set; }
        public Int32? DfEProviderTypeId { get; set; }
        public Int32? DfEProviderStatusId { get; set; }
        public Int32? DfELocalAuthorityId { get; set; }
        public Int32? DfERegionId { get; set; }
        public Int32? DfEEstablishmentTypeId { get; set; }
        public Int32? SecureAccessId { get; set; }
        public Boolean BulkUploadPending { get; set; }
        public Boolean PublishData { get; set; }
        public Boolean ApprenticeshipContract { get; set; }
        public Boolean NationalApprenticeshipProvider { get; set; }
        public String MarketingInformation { get; set; }

        public List<ListVenueModel> Venues { get; set; }

        public String PassedOverallQAChecks { get; set; }
    }

    public class AddEditProviderQAForComplianceModel
    {
        public Int32? ProviderQAComplianceId { get; set; }

        public Int32 ProviderId { get; set; }

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

        public AddEditProviderQAForComplianceModel()
        {
            SelectedComplianceFailureReasons = new List<Int32>();
        }

        public AddEditProviderQAForComplianceModel(ProviderQACompliance providerQACompliance)
            : this()
        {
            this.ProviderQAComplianceId = providerQACompliance.ProviderQAComplianceId;
            this.ProviderId = providerQACompliance.ProviderId;
            this.DetailsOfUnverifiableClaim = providerQACompliance.DetailsOfUnverifiableClaim;
            this.DetailsOfComplianceFailure = providerQACompliance.DetailsOfComplianceFailure;
            this.Passed = providerQACompliance.Passed ? "1" : "0";
            foreach (QAComplianceFailureReason fr in providerQACompliance.QAComplianceFailureReasons)
            {
                this.SelectedComplianceFailureReasons.Add(fr.QAComplianceFailureReasonId);
            }
        }

        
    }

    public class ProviderQAForComplianceJsonModel
    {
        public Int32 ProviderQAForComplianceId { get; set; }
        public Boolean Passed { get; set; }

        public ProviderQAForComplianceJsonModel()
        {
        }

        public ProviderQAForComplianceJsonModel(ProviderQACompliance providerQACompliance)
            : this()
        {
            this.ProviderQAForComplianceId = providerQACompliance.ProviderQAComplianceId;
            this.Passed = providerQACompliance.Passed;
        }
    }

    public class AddEditProviderUnableToCompleteModel
    {
        public Int32? ProviderUnableToCompleteId { get; set; }

        public Int32 ProviderId { get; set; }

        [LanguageDisplay("Further details about incompletion")]
        [Display(Description = "Enter details about the incompletion")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(2000, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public String TextUnableToComplete { get; set; }

        [LanguageDisplay("Reason for Failure")]
        [Display(Description = "Select the reason(s) for being Unable to Complete")]
        public IEnumerable<UnableToCompleteFailureReason> UnableToCompleteFailureReasons { get; set; }

        [LanguageRequired]
        public List<Int32> SelectedUnableToCompleteFailureReasons { get; set; }

        public AddEditProviderUnableToCompleteModel()
        {
            SelectedUnableToCompleteFailureReasons = new List<Int32>();
        }

        public AddEditProviderUnableToCompleteModel(ProviderUnableToComplete providerUnableToComplete)
            : this()
        {
            this.ProviderUnableToCompleteId = providerUnableToComplete.ProviderUnableToCompleteId;
            this.ProviderId = providerUnableToComplete.ProviderId;
            this.TextUnableToComplete = providerUnableToComplete.TextUnableToComplete;

            foreach (UnableToCompleteFailureReason fr in providerUnableToComplete.UnableToCompleteFailureReasons)
            {
                this.SelectedUnableToCompleteFailureReasons.Add(fr.UnableToCompleteFailureReasonId);
            }
        }


    }


    public class ProviderUnableToCompleteJsonModel
    {
        public Int32 ProviderUnableToCompleteId { get; set; }
        
        public ProviderUnableToCompleteJsonModel()
        {
        }

        public ProviderUnableToCompleteJsonModel(ProviderUnableToComplete providerUnableToComplete)
            : this()
        {
            this.ProviderUnableToCompleteId = providerUnableToComplete.ProviderUnableToCompleteId;
        }
    }


    public class AddEditProviderQAForStyleModel
    {
        public Int32? ProviderQAStyleId { get; set; }

        public Int32 ProviderId { get; set; }

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


        public AddEditProviderQAForStyleModel()
        {
            SelectedStyleFailureReasons = new List<Int32>();
        }

        public AddEditProviderQAForStyleModel(ProviderQAStyle providerQAStyle)
            : this()
        {
            this.ProviderQAStyleId = providerQAStyle.ProviderQAStyleId;
            this.ProviderId = providerQAStyle.ProviderId;
            this.Passed = providerQAStyle.Passed ? "1" : "0";
            foreach (QAStyleFailureReason fr in providerQAStyle.QAStyleFailureReasons)
            {
                this.SelectedStyleFailureReasons.Add(fr.QAStyleFailureReasonId);
            }
        }
    }

    public class ProviderQAForStyleJsonModel
    {
        public Int32 ProviderQAForComplianceId { get; set; }
        public Boolean Passed { get; set; }

        public ProviderQAForStyleJsonModel()
        {
        }

        public ProviderQAForStyleJsonModel(ProviderQAStyle providerQAStyle)
            : this()
        {
            this.ProviderQAForComplianceId = providerQAStyle.ProviderQAStyleId;
            this.Passed = providerQAStyle.Passed;
        }
    }

    public class ReturnValueJsonModel
    {
        public Int32 Status { get; set; }
        public String Message { get; set; }

        public ReturnValueJsonModel()
        {
        }

        public ReturnValueJsonModel(Int32 Status, String Message)
            : this()
        {
            this.Status = Status;
            this.Message = Message;
        }

        public static ReturnValueJsonModel Success()
        {
            return new ReturnValueJsonModel(1, "");
        }

        public static ReturnValueJsonModel FailWithMessage(String Message)
        {
            return new ReturnValueJsonModel(0, Message);
        }
    }

    public class ProviderImportModel
    {
        [Required(ErrorMessage = "Please specify a ZIP file.")]
        public HttpPostedFileBase FileUpload { get; set; }

        [LanguageDisplay("Import Batch")]
        public int ImportBatchId { get; set; }
    }

    public class SubmitNewMarketingInformationTextModel
    {
        public const Int32 MarketingInformationMaxLength = 900;

        [AllowHtml]
        [Display(Description = "")]
        // *************************************************************************************************
        // The following 2 strings must also be changed in ProviderController.RemoveSpellCheckHTMLFromMarketingInformation
        [LanguageDisplay("Enter Text to be Submitted for QA")]
        [LanguageStringLength(MarketingInformationMaxLength, ErrorMessage = "The maximum length of {0} is 750 characters.")]
        // *************************************************************************************************
        [LanguageRequired]
        public string NewMarketingInformation { get; set; }
    }

    public interface IQualityAssurance
    {
        Boolean HasBeenQAdForCompliance { get; set; }
        Boolean HasPassedComplianceChecks { get; set; }
        DateTime? LastQAdForComplianceOn { get; set; }
        String LastQAdForComplianceBy { get; set; }

        Boolean HasBeenQAdForStyle { get; set; }
        Boolean HasPassedStyleChecks { get; set; }
        DateTime? LastQAdForStyleOn { get; set; }
        String LastQAdForStyleBy { get; set; }

        Boolean IsLive();
    }

    public class OverrideMaximumNumberOfLocationsModel
    {
        [LanguageDisplay("Maximum Number of Locations for Provider")]
        [Display(Description = "Enter the maximum number of locations this provider is allowed to create")]
        public Int32 MaximumNumberOfLocations { get; set; }

        public Int32 NumberOfLocations { get; set; }
    }
}