using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebGrease;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using System.Web;
    using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

    #region Provider Reports

    public class ProviderCourseReportViewModelItem
    {
        public int CourseId { get; set; }

        [LanguageDisplay("Status")]
        public string RecordStatusName { get; set; }

        [LanguageDisplay("Course Id")]
        public string ProviderOwnCourseRef { get; set; }

        [LanguageDisplay("Course Title")]
        public string CourseTitle { get; set; }

        [LanguageDisplay("Qualification Type")]
        public string QualificationTypeName { get; set; }

        [LanguageDisplay("Qualification title")]
        public string QualificationTitle { get; set; }

        [LanguageDisplay("Awarding / Accrediting Organisation")]
        public string AwardOrgName { get; set; }
    }

    public class ProviderCourseReportViewModel
    {
        public int ProviderId { get; set; }
        public List<ProviderCourseReportViewModelItem> Items { get; set; }

        public ProviderCourseReportViewModel(int providerId)
        {
            ProviderId = providerId;
        }
    }

    public class ProviderOpportunityReportViewModelItem
    {
        [LanguageDisplay("Status")]
        public string RecordStatusName { get; set; }

        [LanguageDisplay("Opportunity Id")]
        public string ProviderOwnCourseInstanceRef { get; set; }

        [LanguageDisplay("Study Mode")]
        public string StudyModeName { get; set; }

        [LanguageDisplay("Attendance Mode")]
        public string AttendanceTypeName { get; set; }

        [LanguageDisplay("Attendance Pattern")]
        public string AttendancePatternName { get; set; }

        [LanguageDisplay("Duration")]
        public string DurationForDisplay
        {
            get
            {
                string s = String.Empty;
                if (DurationUnit != null && DurationUnitName != null)
                {
                    s = String.Format("{0} {1}", DurationUnit, DurationUnitName);
                }
                if (!String.IsNullOrWhiteSpace(DurationAsText))
                {
                    if (s.Length > 0) s += " ";
                    s += DurationAsText;
                }
                return s;
            }
        }

        public int? DurationUnit { get; set; }
        public string DurationUnitName { get; set; }
        public string DurationAsText { get; set; }

        [LanguageDisplay("Start Date")]
        public string StartDateForDisplay
        {
            get
            {
                string s = String.Empty;
                if (!String.IsNullOrWhiteSpace(StartDates))
                {
                    s += StartDates;
                }
                if (!String.IsNullOrWhiteSpace(StartDateDescription))
                {
                    if (s.Length > 0) s += " ";
                    s += StartDateDescription;
                }
                return s;
            }
        }

        public string StartDates { get; set; }
        public string StartDateDescription { get; set; }

        [LanguageDisplay("End Date")]
        [DateDisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? EndDate { get; set; }

        [LanguageDisplay("Price")]
        public string PriceForDisplay
        {
            get
            {
                string s = String.Empty;
                if (Price != null)
                {
                    s += String.Format("{0:C}", Price);
                }
                if (!String.IsNullOrWhiteSpace(PriceAsText))
                {
                    if (s.Length > 0) s += " ";
                    s += PriceAsText;
                }
                return s;
            }
        }


        public decimal? Price { get; set; }

        [LanguageDisplay("Price(£)")]
        public string PriceAsText { get; set; }

        [LanguageDisplay("Venue Name or Region Name")]
        public string VenueOrRegionName
        {
            get
            {
                string s = String.Empty;
                if (!String.IsNullOrWhiteSpace(Venues))
                {
                    s = Venues;
                }
                else if (!String.IsNullOrWhiteSpace(Region))
                {
                    //s += s == String.Empty ? Region : String.Format("{0} ({1})", s, Region);
                    s = Region;
                }
                return s;
            }
        }

        public string Region { get; set; }
        public string Venues { get; set; }
    }

    public class ProviderOpportunityReportViewModel
    {
        public int ProviderId { get; set; }
        public List<ProviderOpportunityReportViewModelItem> Items { get; set; }

        public ProviderOpportunityReportViewModel(int providerId)
        {
            ProviderId = providerId;
        }
    }

    public class ProviderVenueReportViewModelItem
    {
        [LanguageDisplay("Status")]
        public string RecordStatusName { get; set; }

        [LanguageDisplay("Provider Venue Id")]
        public string ProviderOwnVenueRef { get; set; }

        public AddressViewModel Address { get; set; }
    }

    public class ProviderVenueReportViewModel
    {
        /// <summary>
        /// Gets or sets the provider identifier.
        /// </summary>
        /// <value>
        /// The provider identifier.
        /// </value>
        public int ProviderId { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<ProviderVenueReportViewModelItem> Items { get; set; }

        public ProviderVenueReportViewModel(int providerId)
        {
            ProviderId = providerId;
        }
    }

    public class ProviderDashboardReportViewModel
    {
        // Misc details
        public int ProviderId { get; set; }

        public string ProviderName { get; set; }

        [LanguageDisplay("Date provider added to portal")]
        [Display(Description = "Date provider added to the portal.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ProviderCreatedDateTimeUTC { get; set; }
 
        // Recent activity
        [LanguageDisplay("Next update due")]
        [Display(Description = "Date by which you must update your provision.")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime? DateNextUpdateDue { get
            {
                try {
                    //Rules taken from Automation/ProviderTrafficLightStatus
                    if (!this.LastActivity.HasValue)
                    {
                        return DateTime.Today;
                    }
                    else if ((bool)HttpContext.Current.Session[Constants.SessionFieldNames.ProviderIsSfaFunded])
                    {
                        var greenPeriod = QualityIndicator.GetGreenDuration(LastActivity.Value.Month);
                        //Next update date is assumed to be period when status will go red, this is green period + 1 month for Amber
                        return LastActivity.Value.AddMonths(greenPeriod + 1);
                    }
                    else if ((bool)HttpContext.Current.Session[Constants.SessionFieldNames.ProviderIsDfe1619Funded])
                    {
                        //Next update date is assumed to be period when status will go red, this is green period + 1 month for Amber
                        return QualityIndicator.GetDfeProviderNextUpdateDueDate(LastActivity.Value).Date;
                    }
                } catch (Exception ex)
                {
                    //Ingore any error
                }
                return DateTime.Today;

            }
        }

        [LanguageDisplay("Last log in (date)")]
        [Display(Description = "Date of most recent provider user login.")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime? LastProviderLoginDateTimeUtc { get; set; }

        [LanguageDisplay("Last log in (user)")]
        [Display(Description = "Name of most recent provider user.")]
        public string LastProviderLoginUserDisplayName { get; set; }

        [LanguageDisplay("Last updated (date)")]
        [Display(Description = "Date of most recently updated opportunity or course.")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime? LastUpdatingDateTimeUtc { get; set; }

        [LanguageDisplay("Last updated (user)")]
        [Display(Description = "User who made the most recent update.")]
        public string LastUpdatingUserDisplayName { get; set; }

        [LanguageDisplay("Last Activity")]
        [Display(Description = "Date of last activity.")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime? LastActivity { get; set; }

        [LanguageDisplay("Linked Parent Organisation")]
        [Display(Description = "The parent organisation(s) linked to this provider")]
        public List<ProviderDashboardOrganisationViewModel> ParentOrganisations { get; set; }

        public int NumberOfPendingCourses { get; set; }

        //// Provision
        [LanguageDisplay("Courses")]
        [Display(Description = "Total number of live courses.")]
        public int? Courses { get; set; }

        //[LanguageDisplay("Opps")]
        [Display(Description = "Total number of live opportunities.")]
        public int? CourseInstances { get; set; }

        // Quality scoring
        [LanguageDisplay("Course Summaries")]
        [Display(Description = "% courses which have an overview of course content to attract prospective students (200+ characters).")]
        public int? CoursesWithLongSummary { get; set; }

        [LanguageDisplay("Unique Summaries")]
        [Display(Description = "% courses which have a unique overview of course content to attract prospective students (200+ characters).")]
        public int? CoursesWithDistinctLongSummary { get; set; }

        [LanguageDisplay("Start Dates")]
        [Display(
            Description =
                "% opportunities with a start date in the future or with the start date description updated with any roll on / roll off options. This means the NCS is always up to date for the user and meets with their availability."
            )]
        public decimal? CoursesWithFutureStartDates { get; set; }

        [LanguageDisplay("Learning Aims / QAN")]
        [Display(Description = "% courses which have a ESFA-assigned Learning Aim/QAN code. This provides accurate Information to the user and simplifies the upload process meaning fewer fields to complete.")]
        public int? CoursesWithLearningAims { get; set; }

        [LanguageDisplay("Data Quality Score")]
        [Display(
            Description =
                "% overall quality score for your entry on the Provider Portal"
            )]
        public decimal? AutoAggregateQualityRating { get; set; }


        // Charts
        [LanguageDisplay("Courses by start date")]
        [Display(Description = "Number of courses broken down by start date")]
        public List<ProviderDashboardChartViewModel> CoursesChart { get; set; }

        [LanguageDisplay("Opportunities by start date")]
        [Display(Description = "Numbers of opportunities broken down by start date.")]
        public List<ProviderDashboardChartViewModel> OpportunitiesChart { get; set; }

        public bool? IsTASOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to render for PDF output.
        /// </summary>
        public bool RenderForPdf { get; set; }

        /// <summary>
        /// Gets or sets the amount of time (in ms) to wait after the page has rendered to begin PDF output.
        /// </summary>
        public int PdfRenderDelay { get; set; }

        public ProviderDashboardReportViewModel()
        {
            PdfRenderDelay = Constants.ConfigSettings.ProviderDashboardPdfRenderDelay;
        }

        public ProviderDashboardReportViewModel(int providerId)
        {
            ProviderId = providerId;
            PdfRenderDelay = Constants.ConfigSettings.ProviderDashboardPdfRenderDelay;
        }
    }

    public class ProviderDashboardChartViewModel
    {
        public string CourseStatusName { get; set; }
        public int CourseCount { get; set; }
        public string BarColour { get; set; }
        public string Link { get; set; }
    }


    public class ProviderDashboardOrganisationViewModel
    {
        public string OrganisationName { get; set; }
        public int OrganisationId { get; set; }
    }


    public class ProviderQAHistoryReportViewModelItem
    {
        [LanguageDisplay("Date & Time")]
        [Display(Description = "Date and time of QA check.")]
        [DateDisplayFormat(Format = DateFormat.ShortDateTime)]
        public DateTime? QADateTimeUtc { get; set; }

        [LanguageDisplay("User name")]
        [Display(Description = "Quality Assurance performed by.")]
        public String QAUserDisplayName { get; set; }

        [LanguageDisplay("Provider")]
        [Display(Description = "Provider name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("Status")]
        [Display(Description = "QA compliance status")]
        public Boolean Status { get; set; }

        [LanguageDisplay("Data")]
        [Display(Description = "Item being QA'ed")]
        public String EntityQAed { get; set; }

        [LanguageDisplay("Compliance checks")]
        [Display(Description = "Compliance checks")]
        public String ComplianceChecks { get; set; }

        [LanguageDisplay("Style checks")]
        [Display(Description = "Style checks")]
        public String StyleChecks { get; set; }

        [LanguageDisplay("Text QA'ed")]
        [Display(Description = "")]
        public String TextQAd { get; set; }

        [LanguageDisplay("Further style fail details")]
        [Display(Description = "")]
        public String DetailsOfQA { get; set; }

        [LanguageDisplay("Import Batches")]
        public String ImportBatchNames { get; set; }
    }

    public class ProviderQAHistoryReportViewModel
    {
        public int ProviderId { get; set; }
        public List<ProviderQAHistoryReportViewModelItem> Items { get; set; }

        public ProviderQAHistoryReportViewModel()
        {
        }

        public ProviderQAHistoryReportViewModel(int providerId)
        {
            ProviderId = providerId;
        }
    }

    public class ProviderUnableToCompleteHistoryReportViewModelItem
    {
        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }

        // Main Details
        [LanguageDisplay("UKPRN")]
        [Display(Description = "UKPRN")]
        public int? Ukprn { get; set; }

        [LanguageDisplay("Provider")]
        [Display(Description = "Provider name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("Reasons")]
        [Display(Description = "Reason checks")]
        public String UnableToCompleteReasonChecks { get; set; }

        [LanguageDisplay("Further information")]
        [Display(Description = "")]
        public String UnableToCompleteText { get; set; }

        [LanguageDisplay("Date & Time")]
        [Display(Description = "Unable to complete created.")]
        [DateDisplayFormat(Format = DateFormat.ShortDateTime)]
        public DateTime? UnableToCompleteDateCreated { get; set; }

        [LanguageDisplay("User name")]
        [Display(Description = "Unable to complete recorded by.")]
        public String UnableToCompleteUsername { get; set; }
    }

    public class ProviderUnableToCompleteHistoryReportViewModel
    {
        public int ProviderId { get; set; }

        public Boolean ShowAllProviders { get; set; }

        public List<ProviderUnableToCompleteHistoryReportViewModelItem> Items { get; set; }

        public ProviderUnableToCompleteHistoryReportViewModel()
        {
            ShowAllProviders = true;
            ProviderId = -1;
        }

        public ProviderUnableToCompleteHistoryReportViewModel(int providerId) : this()
        {
            ProviderId = providerId;
        }
    }

    #endregion



    #region Organisation Reports

    public class OrganisationTrafficLightReportViewModelItem
    {
        [LanguageDisplay("Status")]
        public string Status
        {
            get { return QualityIndicator.GetTrafficText(ModifiedDateTimeUtc, SFAFunded, DFE1619Funded); }
        }

        [LanguageDisplay("Provider Id")]
        public int ProviderId { get; set; }

        [LanguageDisplay("UKPRN")]
        public int? Ukprn { get; set; }

        [LanguageDisplay("Type")]
        public string ProviderTypeName { get; set; }

        [LanguageDisplay("Name")]
        public string ProviderName { get; set; }

        [LanguageDisplay("Alias")]
        public string ProviderNameAlias { get; set; }

        [LanguageDisplay("UKRLP Name")]
        public string UkrlpName { get; set; }

        [LanguageDisplay("Updated")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime? ModifiedDateTimeUtc { get; set; }

        [LanguageDisplay("Method")]
        public string LastUpdateMethod { get; set; }

        [LanguageDisplay("Primary Contacts")]
        public List<MailAddressPhoneNumber> PrimaryContacts { get; set; }

        [LanguageDisplay("SFA Funded")]
        public Boolean SFAFunded { get; set; }

        [LanguageDisplay("DfE EFA Funded")]
        public Boolean DFE1619Funded { get; set; }

        [LanguageDisplay("Funding Type")]
        public string FundingStatus
        {
            get
            {
                if (SFAFunded && this.DFE1619Funded)
                {
                    return "SFA / DFE";
                }
                else if (SFAFunded)
                {
                    return "SFA funded";
                }
                else if (DFE1619Funded)
                {
                    return "DFE Funded";
                }

                return string.Empty;
            }
        }
    }

    public class OrganisationTrafficLightReportViewModel
    {
        public int OrganisationId { get; set; }
        public List<OrganisationTrafficLightReportViewModelItem> Items { get; set; }

        public OrganisationTrafficLightReportViewModel(int organisationId)
        {
            OrganisationId = organisationId;
        }
    }

    #endregion

    #region Admin reports

    public class AdminReportMasterViewModelItem
    {
        [LanguageDisplay("Status")]
        public string Status
        {
            get { return QualityIndicator.GetTrafficText(ProvisionDataCurrent.GetLatestDate(LastProvisionUpdate, LastActivity), SFAFunded, DFE1619Funded); }
        }

        public string StatusCssClass
        {
            get { return QualityIndicator.GetTrafficBackground(ProvisionDataCurrent.GetLatestDate(LastProvisionUpdate, LastActivity), SFAFunded, DFE1619Funded); }
        }

        [LanguageDisplay("Id")]
        public int ProviderId { get; set; }

        [LanguageDisplay("UKPRN")]
        public int? Ukprn { get; set; }

        [LanguageDisplay("Contracting Body")]
        public bool IsContractingBody { get; set; }

        [LanguageDisplay("Prov or Org")]
        public bool IsProvider { get; set; }

        [LanguageDisplay("Type")]
        public string ProviderTypeName { get; set; }

        [LanguageDisplay("Name")]
        public string ProviderName { get; set; }

        [LanguageDisplay("Alias")]
        public string ProviderNameAlias { get; set; }

        [LanguageDisplay("UKRLP Name")]
        public string LegalName { get; set; }

        [LanguageDisplay("Last Activity")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime? LastActivity { get; set; }

        [LanguageDisplay("Last Provision Update")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime? LastProvisionUpdate { get; set; }

        [LanguageDisplay("Up to date Confirmations")]
        public Int32? UpToDateConfirmations { get; set; }

        [LanguageDisplay("Expired LARs")]
        public Int32? ExpiredLARs { get; set; }

        [LanguageDisplay("Method")]
        public string ApplicationName { get; set; }

        [LanguageDisplay("Quality Score")]
        [DisplayFormat(ConvertEmptyStringToNull = true, DataFormatString = "{0:##0.#}%")]
        public decimal? AutoAggregateQualityRating { get; set; }

        [LanguageDisplay("Primary Contacts")]
        public List<MailAddressPhoneNumber> PrimaryContacts { get; set; }

        [LanguageDisplay("Rating")]
        public string Rating
        {
            get
            {
                var score = AutoAggregateQualityRating == null ? 0.0m : AutoAggregateQualityRating.Value/100;
                return QualityIndicator.GetQualityText(score, false);
            }
        }

        public string RatingCssClass
        {
            get
            {
                var score = AutoAggregateQualityRating == null ? 0.0m : AutoAggregateQualityRating.Value/100;
                return QualityIndicator.GetQualityBackground(score);
            }
        }

        [LanguageDisplay("SFA Funded")]
        public Boolean SFAFunded { get; set; }

        [LanguageDisplay("DfE EFA Funded")]
        public Boolean DFE1619Funded { get; set; }

        [LanguageDisplay("Published to NCS/OD")]
        public Boolean? PublishData { get; set; }

        [LanguageDisplay("TAS Only")]
        public Boolean? IsTASOnly { get; set; }
    }

    public class AdminReportMasterViewModel
    {
        public bool IncludeProviders { get; set; }
        public bool IncludeOrganisations { get; set; }
        public bool ContractingBodiesOnly { get; set; }
        public bool SFAFunded { get; set; }
        public bool DFEFunded { get; set; }

        public List<AdminReportMasterViewModelItem> Items { get; set; }

        public AdminReportMasterViewModel(bool includeProviders, bool includeOrganisations, bool contractingBodiesOnly,
            bool sfaFunded, bool dfeFunded)
        {
            IncludeProviders = includeProviders;
            IncludeOrganisations = includeOrganisations;
            ContractingBodiesOnly = contractingBodiesOnly;
            SFAFunded = sfaFunded;
            DFEFunded = dfeFunded;
        }
    }

    public class DailyReportViewModel
    {
        public bool SFAFunded { get; set; }
        public bool DFEFunded { get; set; }
        public List<DailyReportViewModelItem> Items { get; set; }

        public DailyReportViewModel(bool sfaFunded, bool dfeFunded)
        {
            SFAFunded = sfaFunded;
            this.DFEFunded = dfeFunded;
        }
    }

    public class DailyReportViewModelItem
    {
        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }

        [LanguageDisplay("UKPRN")]
        public Int32? Ukprn { get; set; }

        [LanguageDisplay("Provider Name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("Courses")]
        public Int32 Courses { get; set; }

        [LanguageDisplay("Opps")]
        public Int32 Opportunities { get; set; }

        [LanguageDisplay("O per C")]
        public Double OpportunitiesPerCourse { get; set; }

        [LanguageDisplay("%Summs")]
        public Double Summaries { get; set; }

        [LanguageDisplay("%Dist Summs")]
        public Double DistinctSummaries { get; set; }

        [LanguageDisplay("%Aims")]
        public Double Aims { get; set; }

        [LanguageDisplay("%Dist Aims")]
        public Double DistinctAims { get; set; }

        [LanguageDisplay("%URL")]
        public Double Url { get; set; }

        [LanguageDisplay("%Dist URL")]
        public Double DistinctUrl { get; set; }

        [LanguageDisplay("%Book URL")]
        public Double BookingUrl { get; set; }

        [LanguageDisplay("%Dist Book URL")]
        public Double DistinctBookingUrl { get; set; }

        [LanguageDisplay("%Specific Starts")]
        public Double SpecificStarts { get; set; }

        [LanguageDisplay("%Future Starts")]
        public Double FutureStarts { get; set; }

        [LanguageDisplay("%Ent Reqs")]
        public Double EntryRequirements { get; set; }

        [LanguageDisplay("%Prices")]
        public Double Prices { get; set; }

        [LanguageDisplay("Last Activity")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime? LastActivity { get; set; }

        [LanguageDisplay("Last Updated")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime? LastUpdated { get; set; }

        [LanguageDisplay("Updated Score")]
        public Decimal? Autoscore { get; set; }

        [LanguageDisplay("Live Superuser?")]
        public Int32 LiveSuperuser { get; set; }

        [LanguageDisplay("Region")]
        public String Region { get; set; }

        [LanguageDisplay("DfERegion")]
        public String DfERegion { get; set; }

        [LanguageDisplay("Provider Type")]
        public String ProviderType { get; set; }

        [LanguageDisplay("DfE Provider Type")]
        public String DfEProviderType { get; set; }

        [LanguageDisplay("DfE Provider Status")]
        public String DfeProviderStatus { get; set; }

        [LanguageDisplay("DfE Local Authority")]
        public String DfeLocalAuthority { get; set; }

        [LanguageDisplay("DfE Establishment Type")]
        public String DfeEstablishmentType { get; set; }

        [LanguageDisplay("Rating")]
        public String Rating
        {
            get
            {
                Decimal score = Autoscore == null ? 0.0m : Autoscore.Value/100;
                return QualityIndicator.GetQualityText(score, false);
            }
        }

        public string RatingCssClass
        {
            get
            {
                Decimal score = Autoscore == null ? 0.0m : Autoscore.Value/100;
                return QualityIndicator.GetQualityBackground(score);
            }
        }

        [LanguageDisplay("RoATP")]
        public Boolean RoATP { get; set; }

        [LanguageDisplay("TAS Only")]
        public Boolean? IsTASOnly { get; set; }
    }

    public class WeeklyReportViewModel
    {
        public bool SFAFunded { get; set; }
        public bool DFEFunded { get; set; }
        public List<WeeklyReportViewModelItem> Items { get; set; }

        public WeeklyReportViewModel(bool sfaFunded, bool dfeFunded)
        {
            SFAFunded = sfaFunded;
            DFEFunded = dfeFunded;
        }
    }

    public class WeeklyReportViewModelItem
    {
        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }

        [LanguageDisplay("UKPRN")]
        public Int32? Ukprn { get; set; }

        [LanguageDisplay("Provider Name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("Last Activity")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime? LastActivity { get; set; }

        [LanguageDisplay("Last Update Date")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime? LastUpdate { get; set; }

        [LanguageDisplay("Last Opp Update")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime? LastOpportunityUpdate { get; set; }

        [LanguageDisplay("Last Update Method")]
        public String LastUpdateMethod { get; set; }

        [LanguageDisplay("BU Success")]
        public Int32 BulkUploadSuccess { get; set; }

        [LanguageDisplay("BU Load")]
        public Int32 BulkUploads { get; set; }

        [LanguageDisplay("No. Superusers")]
        public Int32 NumberOfSuperUsers { get; set; }

        [LanguageDisplay("No. Users")]
        public Int32 NumberOfUsers { get; set; }

        [LanguageDisplay("Courses")]
        public Int32 Courses { get; set; }

        [LanguageDisplay("BU Courses")]
        public Int32 BulkUploadCourses { get; set; }

        [LanguageDisplay("PP Courses")]
        public Int32 ProviderPortalCourses { get; set; }

        [LanguageDisplay("Plan B Courses")]
        public Int32 PlanBCourses { get; set; }

        [LanguageDisplay("With LAD")]
        public Int32 CoursesWithLearningAims { get; set; }

        [LanguageDisplay("WO LAD")]
        public Int32 CoursesWithoutLearningAims { get; set; }

        [LanguageDisplay("No. Opp")]
        public Int32 Opportunities { get; set; }

        [LanguageDisplay("In Scope Opps")]
        public Int32 InScopeOpportunities { get; set; }

        [LanguageDisplay("Out Scope Opps")]
        public Int32 OutOfScopeOpportunities { get; set; }

        [LanguageDisplay("A10 10")]
        public Int32 A1010 { get; set; }

        [LanguageDisplay("A10 22")]
        public Int32 A1022 { get; set; }

        [LanguageDisplay("A10 25")]
        public Int32 A1025 { get; set; }

        [LanguageDisplay("A10 35")]
        public Int32 A1035 { get; set; }

        [LanguageDisplay("A10 45")]
        public Int32 A1045 { get; set; }

        [LanguageDisplay("A10 46")]
        public Int32 A1046 { get; set; }

        [LanguageDisplay("A10 70")]
        public Int32 A1070 { get; set; }

        [LanguageDisplay("A10 80")]
        public Int32 A1080 { get; set; }

        [LanguageDisplay("A10 81")]
        public Int32 A1081 { get; set; }

        [LanguageDisplay("A10 21")]
        public Int32 A1021 { get; set; }

        [LanguageDisplay("A10 82")]
        public Int32 A1082 { get; set; }

        [LanguageDisplay("A10 99")]
        public Int32 A1099 { get; set; }

        [LanguageDisplay("A10 NA")]
        public Int32 A10NA { get; set; }

        [LanguageDisplay("Updated Score")]
        public Decimal? Autoscore { get; set; }

        [LanguageDisplay("Rating")]
        public String Rating
        {
            get
            {
                Decimal score = Autoscore == null ? 0.0m : Autoscore.Value/100;
                return QualityIndicator.GetQualityText(score, false);
            }
        }

        public string RatingCssClass
        {
            get
            {
                Decimal score = Autoscore == null ? 0.0m : Autoscore.Value/100;
                return QualityIndicator.GetQualityBackground(score);
            }
        }

        [LanguageDisplay("Traffic Light Status")]
        public String TrafficLightStatus
        {
            get { return QualityIndicator.GetTrafficText(ProvisionDataCurrent.GetLatestDate(this.LastActivity, this.LastUpdate), SFAFunded, DFE1619Funded); }
        }

        public string TrafficLightCssClass
        {
            get { return QualityIndicator.GetTrafficBackground(ProvisionDataCurrent.GetLatestDate(this.LastActivity, this.LastUpdate), SFAFunded, DFE1619Funded); }
        }

        [LanguageDisplay("SFA Funded")]
        public Boolean SFAFunded { get; set; }

        [LanguageDisplay("DfE EFA Funded")]
        public Boolean DFE1619Funded { get; set; }

        [LanguageDisplay("RoATP")]
        public Boolean RoATP { get; set; }

        [LanguageDisplay("TAS Only")]
        public Boolean? IsTASOnly { get; set; }
    }

    
    public class HeadlineStatsReportViewModel
    {
        
        public List<HeadlineStatsReportViewModelItem> Items { get; set; }

        public HeadlineStatsReportViewModel()
        {
           
        }
    }

    public class HeadlineStatsReportViewModelItem
    {
        [LanguageDisplay("Courses")]
        [Display(Description = "Number of Courses")]
        public Int32 NumberOfCourses { get; set; }

        // Main Details
        [LanguageDisplay("Opportunities")]
        [Display(Description = "Number of Opportunities")]
        public Int32 NumberOfOpportunities { get; set; }

        [LanguageDisplay("Poor Providers")]
        [Display(Description = "Number of Poor Providers")]
        public Int32 NumberOfPoorProviders { get; set; }

        [LanguageDisplay("Average Providers")]
        [Display(Description = "Number of Average Providers")]
        public Int32 NumberOfAverageProviders { get; set; }

        [LanguageDisplay("Good Providers")]
        [Display(Description = "Number of Good Providers")]
        public Int32 NumberOfGoodProviders { get; set; }

        [LanguageDisplay("Very Good Providers")]
        [Display(Description = "Number of Very Good Providers")]
        public Int32 NumberOfVeryGoodProviders { get; set; }

        [LanguageDisplay("Average Quality Score (%)")]
        [Display(Description = "Average Quality Score")]
        public Decimal AverageQualityScorePercent { get; set; }

        [LanguageDisplay("Average Quality Score")]
        [Display(Description = "Average Quality Score")]
        public string AverageQualityScoreText { get; set; }

        [LanguageDisplay("Zero Courses")]
        [Display(Description = "Number of Providers with 0 Courses")]
        public Int32 ZeroCourses { get; set; }
    }

    public class SFAWeeklyReportViewModel
    {
        public bool SFAFunded { get; set; }
        public bool DFEFunded { get; set; }
        public List<SFAWeeklyReportViewModelItem> Items { get; set; }

        public SFAWeeklyReportViewModel(bool sfaFunded, bool dfeFunded)
        {
            SFAFunded = sfaFunded;
            DFEFunded = dfeFunded;
        }
    }

    public class MonthlyReport_UsageModel
    {
        public String PeriodHeading { get; set; }
        public String Period { get; set; }
        public Int32 NumberOfProvidersWithValidSuperUser { get; set; }
        public Int32 NumberOfProviders { get; set; }
        public Int32 NumberOfProvidersUpdatedOpportunityInPeriod { get; set; }
        public Int32 TotalBulkUploadOpportunities { get; set; }
        public Int32 NumberOfBulkUploadOpportunitiesInPeriod { get; set; }
        public Int32 TotalManuallyUpdatedOpportunities { get; set; }
        public Int32 NumberOfManuallyUpdatedOpportunitiesInPeriod { get; set; }
        public Int32 NumberOfProvidersNotUpdatedOpportunityInPastYear { get; set; }
        public Int32 NumberOfProvidersUpdatedDuringPeriod { get; set; }
        public Int32 NumberOfProvidersUpdated1to2PeriodsAgo { get; set; }
        public Int32 NumberOfProvidersUpdated2to3PeriodsAgo { get; set; }
        public Int32 NumberOfProvidersUpdatedMoreThan3PeriodsAgo { get; set; }
    }

    public class MonthlyReport_ProvisionModel
    {
        public String Period { get; set; }
        public Int32 NumberOfCourses { get; set; }
        public Int32 NumberOfLiveCourses { get; set; }
        public Int32 NumberOfOpportunities { get; set; }
        public Int32 NumberOfLiveOpportunities { get; set; }
        public Int32 ProvidersWithNoCourses { get; set; }
        public Int32 SM_Flexible { get; set; }
        public Int32 SM_FullTime { get; set; }
        public Int32 SM_NotKnown { get; set; }
        public Int32 SM_PartOfFullTimeProgramme { get; set; }
        public Int32 SM_PartTime { get; set; }
        public Int32 AM_DistanceWithAttendance { get; set; }
        public Int32 AM_DistanceWithoutAttendance { get; set; }
        public Int32 AM_FaceToFace { get; set; }
        public Int32 AM_LocationCampus { get; set; }
        public Int32 AM_MixedMode { get; set; }
        public Int32 AM_NotKnown { get; set; }
        public Int32 AM_OnlineWithAttendance { get; set; }
        public Int32 AM_OnlineWithoutAttendance { get; set; }
        public Int32 AM_WorkBased { get; set; }
        public Int32 AP_Customised { get; set; }
        public Int32 AP_DayBlockRelease { get; set; }
        public Int32 AP_Daytime { get; set; }
        public Int32 AP_Evening { get; set; }
        public Int32 AP_NotApplicable { get; set; }
        public Int32 AP_NotKnown { get; set; }
        public Int32 AP_Twilight { get; set; }
        public Int32 AP_Weekend { get; set; }
        public Int32 DU_1WeekOrLess { get; set; }
        public Int32 DU_1To4Weeks { get; set; }
        public Int32 DU_1To3Months { get; set; }
        public Int32 DU_3To6Months { get; set; }
        public Int32 DU_6To12Months { get; set; }
        public Int32 DU_1To2Years { get; set; }
        public Int32 DU_NotKnown { get; set; }
        public Int32 QT_14To19Diploma { get; set; }
        public Int32 QT_AccessToHigherEducation { get; set; }
        public Int32 QT_Apprenticeship { get; set; }
        public Int32 QT_BasicKeySkill { get; set; }
        public Int32 QT_CertificateOfAttendance { get; set; }
        public Int32 QT_CourseProviderCertificate { get; set; }
        public Int32 QT_ExternalAwardedQualification { get; set; }
        public Int32 QT_FoundationalDegree { get; set; }
        public Int32 QT_FunctionalSkill { get; set; }
        public Int32 QT_GCEOrEquivalent { get; set; }
        public Int32 QT_GCSEOrEquivalent { get; set; }
        public Int32 QT_HncHnd { get; set; }
        public Int32 QT_InternationalBacculaureate { get; set; }
        public Int32 QT_NoQualification { get; set; }
        public Int32 QT_NVQ { get; set; }
        public Int32 QT_OtherAccreditedQualification { get; set; }
        public Int32 QT_Postgraduate { get; set; }
        public Int32 QT_Undergraduate { get; set; }
        public Int32 QT_IndustrySpecificQualification { get; set; }
        public Int32 QL_EntryLevel { get; set; }
        public Int32 QL_HigherLevel { get; set; }
        public Int32 QL_Level1 { get; set; }
        public Int32 QL_Level2 { get; set; }
        public Int32 QL_Level3 { get; set; }
        public Int32 QL_Level4 { get; set; }
        public Int32 QL_Level5 { get; set; }
        public Int32 QL_Level6 { get; set; }
        public Int32 QL_Level7 { get; set; }
        public Int32 QL_Level8 { get; set; }
        public Int32 QL_NotKnown { get; set; }
    }

    public class MonthlyReport_QualityModel
    {
        public String Period { get; set; }
        public Int32 Poor { get; set; }
        public Int32 Average { get; set; }
        public Int32 Good { get; set; }
        public Int32 VeryGood { get; set; }
    }

    public class MonthlyReportModel
    {
        public String PeriodToRun { get; set; }
        public String PeriodType { get; set; }

        public List<MonthlyReport_UsageModel> Usage { get; set; }

        public List<MonthlyReport_ProvisionModel> Provision { get; set; }

        public List<MonthlyReport_QualityModel> Quality { get; set; }

        [LanguageDisplay("Average Quality Score")]
        public Decimal? AverageQualityScore { get; set; }
    }


    public class DFEStartDateReport_COModel
    {
        public DateTime Period { get; set; }
        public Int32 NumberOfCourses { get; set; }
        public Int32 NumberOfLiveCourses { get; set; }
        public Int32 NumberOfOpportunities { get; set; }
        public Int32 NumberOfLiveOpportunities { get; set; }
    
    }

    public class DFEStartDateReportModel
    {
        public String PeriodToRun { get; set; }

        public DateTime AcademicYearStart { get; set; }

        public DateTime AcademicYearNext { get; set; }


        public List<DFEStartDateReport_COModel> Usage { get; set; }

        public List<MonthlyReport_QualityModel> Quality { get; set; }

        public String AverageQualityScore { get; set; }
        
        public Int32 FundedCoursesUploaded { get; set; }

        public Int32 FundedCoursesUploadedPostSept { get; set; }
        public Int32 TotalProviders { get; set; }
        public Int32 TotalProvidersUpdated { get; set; }
        public Int32 TotalProvidersWithZeroCourses { get; set; }

        public List<MonthlyReport_QualityModel> DFEQuality { get; set; }

        public List<MonthlyReport_QualityModel> DFESFAQuality { get; set; }

    }

    public class SFAWeeklyReportViewModelItem
    {
        [LanguageDisplay("Number of Providers With a Valid Superuser")]
        public Int32 NumberOfProvidersWithSuperusers { get; set; }

        [LanguageDisplay("Number of Providers Who Have Updated at Least One Opportunity")]
        public Int32 UpdatedAtLeast1Opportunity { get; set; }

        [LanguageDisplay("Total Number of Bulk Upload Opportunities")]
        public Int32 BulkUploadOpportunities { get; set; }

        [LanguageDisplay("Total Number of Bulk Upload Opportunities in the Last 7 Days")]
        public Int32 BulkUploadOpportunitiesLast7Days { get; set; }

        [LanguageDisplay("Total Number of Manually Updated Opportunities")]
        public Int32 ManualOpportunities { get; set; }

        [LanguageDisplay("Total Number of Manually Updated Opportunities in the Last 7 Days")]
        public Int32 ManualOpportunitiesLast7Days { get; set; }

        [LanguageDisplay("Number of Providers That Have Not Updated an Opportunity in the Last Year")]
        public Int32 NotUpdatedInLastYear { get; set; }

        [LanguageDisplay("Number of Providers That Have Updated in the Last 7 Days")]
        public Int32 UpdatedInLast7Days { get; set; }

        [LanguageDisplay("Number of Providers That Last Updated in 8 to 30 Days")]
        public Int32 UpdatedBetween8and30Days { get; set; }

        [LanguageDisplay("Number of Providers That Last Updated in 31 to 60 Days")]
        public Int32 UpdatedBetween31and60Days { get; set; }

        [LanguageDisplay("Number of Providers That Last Updated in 61 to 90 Days")]
        public Int32 UpdatedBetween61and90Days { get; set; }

        [LanguageDisplay("Number of Providers That Last Updated More Than 90 Days Ago")]
        public Int32 UpdatedMoreThan90Days { get; set; }

        [LanguageDisplay("Number of Courses (Incl. Pending, Live & Archived)")]
        public Int32 NumberOfCourses { get; set; }

        [LanguageDisplay("Number of Live Courses")]
        public Int32 NumberOfLiveCourses { get; set; }

        [LanguageDisplay("Number of Opportunities (Incl. Pending, Live & Archived)")]
        public Int32 NumberOfOpportunities { get; set; }

        [LanguageDisplay("Number of Live Opportunities")]
        public Int32 NumberOfLiveOpportunities { get; set; }

        [LanguageDisplay("Number of In Scope Courses (Searchable)")]
        public Int32 NumberOfInScopeCourses { get; set; }

        [LanguageDisplay("Number of Out of Scope Courses (Non-Searchable)")]
        public Int32 NumberOfOutOfScopeCourses { get; set; }

        [LanguageDisplay("Number of In Scope Opportunities (Searchable)")]
        public Int32 NumberOfInScopeOpportunities { get; set; }

        [LanguageDisplay("Number of Out of Scope Opportunities (Non-Searchable)")]
        public Int32 NumberOfOutOfScopeOpportunities { get; set; }

        [LanguageDisplay("Number of Type 1 Courses")]
        public Int32 NumberOfType1Courses { get; set; }

        [LanguageDisplay("Number of Type 2 Courses")]
        public Int32 NumberOfType2Courses { get; set; }

        [LanguageDisplay("Number of Type 3 Courses")]
        public Int32 NumberOfType3Courses { get; set; }

        [LanguageDisplay("Number of Providers With No Courses")]
        public Int32 NumberOfProvidersWithNoCourses { get; set; }

        [LanguageDisplay("A10 10 Community Learning")]
        public Int32 A1010 { get; set; }

        [LanguageDisplay("A10 21 16-18 Learner Responsive")]
        public Int32 A1021 { get; set; }

        [LanguageDisplay("A10 22 Adult Learner Responsive")]
        public Int32 A1022 { get; set; }

        [LanguageDisplay("A10 25 DfE EFA Funding")]
        public Int32 A1025 { get; set; }

        [LanguageDisplay("A10 35 Adult Skills Funding")]
        public Int32 A1035 { get; set; }

        [LanguageDisplay("A10 45 Employer Responsive")]
        public Int32 A1045 { get; set; }

        [LanguageDisplay("A10 46 Employer Responsive")]
        public Int32 A1046 { get; set; }

        [LanguageDisplay("A10 70 ESF Funded")]
        public Int32 A1070 { get; set; }

        [LanguageDisplay("A10 80 Other LSC Funding")]
        public Int32 A1080 { get; set; }

        [LanguageDisplay("A10 81 Other SFA Funding")]
        public Int32 A1081 { get; set; }

        [LanguageDisplay("A10 82 Other YPLA Funding")]
        public Int32 A1082 { get; set; }

        [LanguageDisplay("A10 99 No SFA or YPLA Funding")]
        public Int32 A1099 { get; set; }

        [LanguageDisplay("A10 Not Applicable")]
        public Int32 A10NA { get; set; }

        [LanguageDisplay("Study Mode Flexible")]
        public Int32 StudyModeFlexible { get; set; }

        [LanguageDisplay("Study Mode Full Time")]
        public Int32 StudyModeFullTime { get; set; }

        [LanguageDisplay("Study Mode Not Known")]
        public Int32 StudyModeNotKnown { get; set; }

        [LanguageDisplay("Study Mode Part of Full Time Programme")]
        public Int32 StudyModePartOfFullTimeProgramme { get; set; }

        [LanguageDisplay("Study Mode Part Time")]
        public Int32 StudyModePartTime { get; set; }

        [LanguageDisplay("Attendance Mode Distance with Attendance")]
        public Int32 AttendanceModeDistanceWithAttendance { get; set; }

        [LanguageDisplay("Attendance Mode Distance Without Attendance")]
        public Int32 AttendanceModeDistanceWithoutAttendance { get; set; }

        [LanguageDisplay("Attendance Mode Face-to-Face (Non-Campus)")]
        public Int32 AttendanceModeFaceToFace { get; set; }

        [LanguageDisplay("Attendance Mode Location / Campus")]
        public Int32 AttendanceModeLocation { get; set; }

        [LanguageDisplay("Attendance Mode Mixed Mode")]
        public Int32 AttendanceModeMixed { get; set; }

        [LanguageDisplay("Attendance Mode Not Known")]
        public Int32 AttendanceModeNotKnown { get; set; }

        [LanguageDisplay("Attendance Mode Online with Attendance")]
        public Int32 AttendanceModeOnlineWithAttendance { get; set; }

        [LanguageDisplay("Attendance Mode Online Without Attendance")]
        public Int32 AttendanceModeOnlineWithoutAttendance { get; set; }

        [LanguageDisplay("Attendance Mode Work Based")]
        public Int32 AttendanceModeWorkBased { get; set; }

        [LanguageDisplay("Attendance Pattern Customised")]
        public Int32 AttendancePatternCustomised { get; set; }

        [LanguageDisplay("Attendance Pattern Day / Block Release")]
        public Int32 AttendancePatternDayRelease { get; set; }

        [LanguageDisplay("Attendance Pattern Daytime / Working Hours")]
        public Int32 AttendancePatternDaytime { get; set; }

        [LanguageDisplay("Attendance Pattern Evening")]
        public Int32 AttendancePatternEvening { get; set; }

        [LanguageDisplay("Attendance Pattern Not Applicable")]
        public Int32 AttendancePatternNotApplicable { get; set; }

        [LanguageDisplay("Attendance Pattern Not Known")]
        public Int32 AttendancePatternNotKnown { get; set; }

        [LanguageDisplay("Attendance Pattern Twilight")]
        public Int32 AttendancePatternTwilight { get; set; }

        [LanguageDisplay("Attendance Pattern Weekend")]
        public Int32 AttendancePatternWeekend { get; set; }

        [LanguageDisplay("Duration 1 Week or Less")]
        public Int32 Duration1Week { get; set; }

        [LanguageDisplay("Duration 1 - 4 Weeks")]
        public Int32 Duration1To4Weeks { get; set; }

        [LanguageDisplay("Duration 1 - 3 Months")]
        public Int32 Duration1To3Months { get; set; }

        [LanguageDisplay("Duration 3 - 6 Months")]
        public Int32 Duration3To6Months { get; set; }

        [LanguageDisplay("Duration 6 - 12 Months")]
        public Int32 Duration6To12Months { get; set; }

        [LanguageDisplay("Duration 1 - 2 Years")]
        public Int32 Duration1To2Years { get; set; }

        [LanguageDisplay("Duration Not Known")]
        public Int32 DurationNotKnown { get; set; }

        [LanguageDisplay("Qualification Type 14-19 Diploma and Relevant Components")]
        public Int32 QualType14To19Diploma { get; set; }

        [LanguageDisplay("Qualification Type Access to Higher Education")]
        public Int32 QualTypeAccessToHigher { get; set; }

        [LanguageDisplay("Qualification Type Apprenticeship")]
        public Int32 QualTypeApprenticeship { get; set; }

        [LanguageDisplay("Qualification Type Basic / Key Skill")]
        public Int32 QualTypeBasicSkill { get; set; }

        [LanguageDisplay("Qualification Type Certificate of Attendance")]
        public Int32 QualTypeCertificateOfAttendance { get; set; }

        [LanguageDisplay("Qualification Type Course Provider Certificate (This Must Include an Assessed Element)")]
        public Int32 QualTypeCourseProviderCertificate { get; set; }

        [LanguageDisplay("Qualification Type External Awarded Qualification - Non-Accredited")]
        public Int32 QualTypeExternalAward { get; set; }

        [LanguageDisplay("Qualification Type Foundation Degree")]
        public Int32 QualTypeFoundationDegree { get; set; }

        [LanguageDisplay("Qualification Type Functional Skills")]
        public Int32 QualTypeFunctionalSkills { get; set; }

        [LanguageDisplay("Qualification Type GCE A/AS Level or Equivalent")]
        public Int32 QualTypeGCE { get; set; }

        [LanguageDisplay("Qualification Type GSCE or Equivalent")]
        public Int32 QualTypeGSCE { get; set; }

        [LanguageDisplay("Qualification Type HNC / HND / Higher Education Awards")]
        public Int32 QualTypeHND { get; set; }

        [LanguageDisplay("Qualification Type International Baccalaureate Diploma")]
        public Int32 QualTypeBaccalaureate { get; set; }

        [LanguageDisplay("Qualification Type No Qualification")]
        public Int32 QualTypeNoQualification { get; set; }

        [LanguageDisplay("Qualification Type NVQ and Relevant Components")]
        public Int32 QualTypeNVQ { get; set; }

        [LanguageDisplay("Qualification Type Other Regulated / Accredited Qualification")]
        public Int32 QualTypeOtherAccredited { get; set; }

        [LanguageDisplay("Qualification Type Postgraduate Qualification")]
        public Int32 QualTypePostgraduate { get; set; }

        [LanguageDisplay("Qualification Type Professional or Industry Specific Qualification")]
        public Int32 QualTypeProfessionalOrIndustrySpecific { get; set; }

        [LanguageDisplay("Qualification Type Undergraduate Qualification")]
        public Int32 QualTypeUndergraduate { get; set; }

        [LanguageDisplay("Entry Level")]
        public Int32 QualLevelEntry { get; set; }

        [LanguageDisplay("Higher Level")]
        public Int32 QualLevelHigher { get; set; }

        [LanguageDisplay("Level 1")]
        public Int32 QualLevel1 { get; set; }

        [LanguageDisplay("Level 2")]
        public Int32 QualLevel2 { get; set; }

        [LanguageDisplay("Level 3")]
        public Int32 QualLevel3 { get; set; }

        [LanguageDisplay("Level 4")]
        public Int32 QualLevel4 { get; set; }

        [LanguageDisplay("Level 5")]
        public Int32 QualLevel5 { get; set; }

        [LanguageDisplay("Level 6")]
        public Int32 QualLevel6 { get; set; }

        [LanguageDisplay("Level 7")]
        public Int32 QualLevel7 { get; set; }

        [LanguageDisplay("Level 8")]
        public Int32 QualLevel8 { get; set; }

        [LanguageDisplay("Level Unknown / Not Applicable")]
        public Int32 QualLevelUnknown { get; set; }

        [LanguageDisplay(
            "Number of Providers with a Valid Superuser That Have an Updated Opportunity (Archived or Live) in the Last Month"
            )]
        public Int32 ProvidersWithSuperusersUpdatedInLastMonth { get; set; }

        [LanguageDisplay(
            "Number of Providers with a Valid Superuser That Last Updated an Opportunity (Archived or Live) Between 2 and 3 Months Ago"
            )]
        public Int32 ProvidersWithSuperusersUpdatedBetween2and3Months { get; set; }

        [LanguageDisplay(
            "Number of Providers with a Valid Superuser That Last Updated an Opportunity (Archived or Live) More Than 3 Months Ago"
            )]
        public Int32 ProvidersWithSuperusersUpdatedMoreThan3Months { get; set; }

        [LanguageDisplay(
            "Number of Providers with a User That has Logged in That Last Updated or Archived an Opportunity Before 1st August 2010"
            )]
        public Int32 ProvidersWithLoggedInUsersNotUpdatedSince01Aug2010 { get; set; }

        [LanguageDisplay("Traffic Light Green")]
        public Int32 TrafficLightGreen { get; set; }

        [LanguageDisplay("Traffic Light Amber")]
        public Int32 TrafficLightAmber { get; set; }

        [LanguageDisplay("Traffic Light Red")]
        public Int32 TrafficLightRed { get; set; }

        [LanguageDisplay("Quality Rating Poor")]
        public Int32 QualityPoor { get; set; }

        [LanguageDisplay("Quality Rating Average")]
        public Int32 QualityAverage { get; set; }

        [LanguageDisplay("Quality Rating Good")]
        public Int32 QualityGood { get; set; }

        [LanguageDisplay("Quality Rating Very Good")]
        public Int32 QualityVeryGood { get; set; }

        [LanguageDisplay("Quality Rating Poor and Average")]
        public Int32 QualityPoorAndAverage { get; set; }

        [LanguageDisplay("Quality Rating Good and Very Good")]
        public Int32 QualityGoodAndVeryGood { get; set; }

        [LanguageDisplay("Average Score")]
        public Decimal AverageScore { get; set; }

        [LanguageDisplay("Rating")]
        public String Rating
        {
            get
            {
                Decimal score = AverageScore/100;
                return QualityIndicator.GetQualityText(score, false);
            }
        }

        public string RatingCssClass
        {
            get
            {
                Decimal score = AverageScore/100;
                return QualityIndicator.GetQualityBackground(score);
            }
        }

        [LanguageDisplay("Total User Sessions (Last 7 Days)")]
        public int TotalUserSessions { get; set; }

        [LanguageDisplay("Provider Portal User Sessions")]
        public int ProviderPortalUserSessions { get; set; }

        [LanguageDisplay("Secure Access User Sessions")]
        public int SecureAccessUserSessions { get; set; }

        [LanguageDisplay("Number of Providers Where Data is Published")]
        public int PublishedProviders { get; set; }

        [LanguageDisplay("Number of Provider Where Data is Not Published")]
        public int UnpublishedProviders { get; set; }
    }

    public class SalesForceContactsViewModelItem
    {
        [LanguageDisplay()]
        public string AccountName { get; set; }

        [LanguageDisplay()]
        public string FirstName { get; set; }

        [LanguageDisplay()]
        public string LastName { get; set; }

        [LanguageDisplay()]
        public string Email { get; set; }

        [LanguageDisplay()]
        public string MailingStreet { get; set; }

        [LanguageDisplay()]
        public string MailingCity { get; set; }

        [LanguageDisplay()]
        public string MailingState { get; set; }

        [LanguageDisplay()]
        public string MailingPostalCode { get; set; }

        [LanguageDisplay()]
        public string Phone { get; set; }

        [LanguageDisplay()]
        public string IsPrimaryContact { get; set; }

        [LanguageDisplay()]
        public string UserExternalId { get; set; }

        [LanguageDisplay()]
        public bool SFAFunded { get; set; }

        [LanguageDisplay("DfEEFAFunded")]
        public bool DFE1619Funded { get; set; }

        [LanguageDisplay]
        public String ExternalId { get; set; }
    }

    public class SalesForceContactsViewModel
    {
        public List<SalesForceContactsViewModelItem> Items;
    }

    public class SalesForceAccountsViewModelItem
    {
        [LanguageDisplay()]
        public string Name { get; set; }

        [LanguageDisplay()]
        public string Type { get; set; }

        [LanguageDisplay()]
        public string BillingStreet { get; set; }

        [LanguageDisplay()]
        public string BillingCity { get; set; }

        [LanguageDisplay()]
        public string BillingState { get; set; }

        [LanguageDisplay()]
        public string BillingPostalCode { get; set; }

        [LanguageDisplay()]
        public string Phone { get; set; }

        [LanguageDisplay()]
        public string Fax { get; set; }

        [LanguageDisplay()]
        public string Website { get; set; }

        [LanguageDisplay()]
        public int? UKPRN { get; set; }

        [LanguageDisplay()]
        public string Region { get; set; }

        [LanguageDisplay()]
        public string ExternalId { get; set; }

        [LanguageDisplay()]
        public string QualityStatus { get; set; }

        [LanguageDisplay()]
        public string PrimaryContact { get; set; }

        [LanguageDisplay()]
        public string PrimaryContactEmail { get; set; }

        [LanguageDisplay()]
        [DateDisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DatePortalLastUpdated { get; set; }

        [LanguageDisplay()]
        public bool? SFAFunded { get; set; }

        [LanguageDisplay("DfEEFAFunded")]
        public bool? DFE1619Funded { get; set; }

        [LanguageDisplay()]
        public string DfEProviderType { get; set; }

        [LanguageDisplay()]
        public string DfEProviderStatus { get; set; }

        [LanguageDisplay()]
        public string DfELocalAuthority { get; set; }

        [LanguageDisplay()]
        public string DfERegion { get; set; }

        [LanguageDisplay()]
        public string DfEEstablishmentType { get; set; }

        [LanguageDisplay("LastQualityEmail")]
        public string LastQualityEmailName { get; set; }

        [LanguageDisplay()]
        [DateDisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? LastQualityEmailDate { get; set; }

        [LanguageDisplay("NextQualityEmail")]
        public string NextQualityEmailName { get; set; }

        [LanguageDisplay()]
        [DateDisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? NextQualityEmailDate { get; set; }

        [LanguageDisplay()]
        public bool? QualityEmailSent { get; set; }

        [LanguageDisplay()]
        public bool? QualityEmailsPaused { get; set; }

        [LanguageDisplay()]
        public decimal? AutoAggregateQualityRating { get; set; }

        [LanguageDisplay()]
        public string TrafficLightStatus { get; set; }
    }

    public class SalesForceAccountsViewModel
    {
        public List<SalesForceAccountsViewModelItem> Items;
    }

    public class BulkUploadHistoryReportViewModelItem
    {
        [LanguageDisplay("Id")]
        public int BulkUploadId { get; set; }

        [LanguageDisplay("Date")]
        [DateDisplayFormat(Format = DateFormat.ShortDateTime)]
        public DateTime CreatedDateTimeUtc { get; set; }

        [LanguageDisplay("Organisation Name")]
        public string OrganisationName { get; set; }

        [LanguageDisplay("Provider Name")]
        public string ProviderName { get; set; }

        [LanguageDisplay("Uploaded By")]
        public string UserName { get; set; }

        [LanguageDisplay("Status")]
        public string BulkUploadStatusText { get; set; }

        [LanguageDisplay("File name")]
        public string FileName { get; set; }

        [LanguageDisplay("Number of courses before upload")]
        public int ExistingCourses { get; set; }

        [LanguageDisplay("Number of valid courses uploaded")]
        public int NewCourses { get; set; }

        [LanguageDisplay("Number of invalid courses uploaded")]
        public int InvalidCourses { get; set; }

        [LanguageDisplay("Number of opportunities before upload")]
        public int ExistingOpportunities { get; set; }

        [LanguageDisplay("Number of valid opportunities uploaded")]
        public int NewOpportunities { get; set; }

        [LanguageDisplay("Number of invalid opportunities uploaded")]
        public int InvalidOpportunities { get; set; }

        [LanguageDisplay("Number of venues before upload")]
        public int ExistingVenues { get; set; }

        [LanguageDisplay("Number of valid venues uploaded")]
        public int NewVenues { get; set; }

        [LanguageDisplay("Number of invalid venues uploaded")]
        public int InvalidVenues { get; set; }

        [LanguageDisplay("Number of errors")]
        public int Errors { get; set; }

        [LanguageDisplay("Number of warnings")]
        public int Warnings { get; set; }

        [LanguageDisplay("Number of system exceptions")]
        public int SystemExceptions { get; set; }

        [LanguageDisplay("Number of successes")]
        public int Successes { get; set; }

        [LanguageDisplay("Number of notices")]
        public int Notices { get; set; }
    }


    public class BulkUploadHistoryApprenticeReportViewModelItem
    {
        [LanguageDisplay("Id")]
        public int BulkUploadId { get; set; }

        [LanguageDisplay("Date")]
        [DateDisplayFormat(Format = DateFormat.ShortDateTime)]
        public DateTime CreatedDateTimeUtc { get; set; }

        [LanguageDisplay("Organisation Name")]
        public string OrganisationName { get; set; }

        [LanguageDisplay("Provider Name")]
        public string ProviderName { get; set; }

        [LanguageDisplay("Uploaded By")]
        public string UserName { get; set; }

        [LanguageDisplay("Status")]
        public string BulkUploadStatusText { get; set; }

        [LanguageDisplay("File name")]
        public string FileName { get; set; }

        [LanguageDisplay("Number of apprenticeships before upload")]
        public int ExistingApprenticeships { get; set; }

        [LanguageDisplay("Number of valid apprenticeships uploaded")]
        public int NewApprenticeships { get; set; }

        [LanguageDisplay("Number of invalid apprenticeships uploaded")]
        public int InvalidApprenticeships { get; set; }

        [LanguageDisplay("Number of delivery locations before upload")]
        public int ExistingDelivLocations { get; set; }

        [LanguageDisplay("Number of valid delivery locations uploaded")]
        public int NewDelivLocations { get; set; }

        [LanguageDisplay("Number of invalid delivery locations uploaded")]
        public int InvalidDelivLocations { get; set; }

        [LanguageDisplay("Number of errors")]
        public int Errors { get; set; }

        [LanguageDisplay("Number of warnings")]
        public int Warnings { get; set; }

        [LanguageDisplay("Number of system exceptions")]
        public int SystemExceptions { get; set; }

        [LanguageDisplay("Number of successes")]
        public int Successes { get; set; }

        [LanguageDisplay("Number of notices")]
        public int Notices { get; set; }
    }


    public class BulkUploadHistoryReportViewModel
    {
        /// <summary>
        /// The earliest date to include in the report, null for all dates.
        /// </summary>
        [LanguageDisplay("Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The latest date to include in the report, null for all dates.
        /// </summary>
        [LanguageDisplay("End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public List<BulkUploadHistoryReportViewModelItem> Items;
    }

    public class BulkUploadHistoryApprenticeReportViewModel
    {
        /// <summary>
        /// The earliest date to include in the report, null for all dates.
        /// </summary>
        [LanguageDisplay("Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The latest date to include in the report, null for all dates.
        /// </summary>
        [LanguageDisplay("End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public List<BulkUploadHistoryApprenticeReportViewModelItem> Items;
    }

    public class QualityEmailHistoryReportViewModelItem
    {
        [LanguageDisplay("UKPRN")]
        public int Ukprn { get; set; }

        [LanguageDisplay("Provider Id")]
        public int ProviderId { get; set; }

        [LanguageDisplay("Name")]
        public string ProviderName { get; set; }

        [LanguageDisplay("Type")]
        public string ProviderTypeName { get; set; }

        [LanguageDisplay("Updated")]
        public DateTime? ModifiedDateTimeUtc { get; set; }

        [LanguageDisplay("")]
        public int TrafficLightStatusId { get; set; }

        [LanguageDisplay("Status")]
        public string TrafficLightStatus
        {
            get { return QualityIndicator.GetTrafficText(TrafficLightStatusId); }
        }

        [LanguageDisplay("SFA Funded")]
        public bool SFAFunded { get; set; }

        [LanguageDisplay("DfE EFA Funded")]
        public bool DfE1619Funded { get; set; }

        [LanguageDisplay("Emails Paused")]
        public bool QualityEmailsPaused { get; set; }

        [LanguageDisplay("Sent")]
        public bool HasValidRecipients { get; set; }

        [LanguageDisplay("Email Date")]
        public DateTime? EmailDateTimeUtc { get; set; }

        [LanguageDisplay("Email")]
        public string EmailTemplateName { get; set; }

        [LanguageDisplay("Next Email Due")]
        public DateTime? NextEmailDateTimeUtc { get; set; }

        [LanguageDisplay("Next Email")]
        public string NextEmailTemplateName { get; set; }
    }

    public class QualityEmailHistoryReportViewModel
    {
        /// <summary>
        /// The provider id to filter the results by, or null for all providers
        /// </summary>
        public int? ProviderId { get; set; }

        /// <summary>
        /// The earliest date to include in the report, null for all dates.
        /// </summary>
        [LanguageDisplay("Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The latest date to include in the report, null for all dates.
        /// </summary>
        [LanguageDisplay("End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// The items.
        /// </summary>
        public List<QualityEmailHistoryReportViewModelItem> Items { get; set; }
    }

    public class MetadataUploadHistoryReportViewModelItem
    {
        [LanguageDisplay("Upload Id")]
        public int MetadataUploadId { get; set; }

        [LanguageDisplay("Type")]
        public string MetadataUploadTypeName { get; set; }

        [LanguageDisplay("Metadata Type")]
        public string MetadataUploadTypeDescription { get; set; }

        [LanguageDisplay("Rows Before Import")]
        public int RowsBefore { get; set; }

        [LanguageDisplay("Rows After Import")]
        public int RowsAfter { get; set; }

        [LanguageDisplay("Uploaded By")]
        public string CreatedByUser { get; set; }

        [LanguageDisplay("Upload Date")]
        public DateTime CreatedDateTimeUtc { get; set; }

        [LanguageDisplay("File Name")]
        public string FileName { get; set; }

        [LanguageDisplay("File Size")]
        public int? FileSizeInBytes { get; set; }

        public int DurationInMilliseconds { get; set; }

        [LanguageDisplay("Duration (h:m:s.ms)")]
        public string Duration
        {
            get { return new TimeSpan(10000*DurationInMilliseconds).ToString("c"); }
        }
    }

    public class MetadataUploadHistoryReportViewModel
    {
        public List<MetadataUploadHistoryReportViewModelItem> Items { get; set; }
    }

    #endregion

    #region Apprenticeship (RoATP) Reports

    public class ApprenticeshipOverallCountReportViewModelItem
    {
        [LanguageDisplay("Provider Count")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 ProviderCount { get; set; }

        [LanguageDisplay("Apprenticeship Count")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 ApprenticeshipCount { get; set; }

        public ApprenticeshipOverallCountReportViewModelItem(Int32 ProviderCount, Int32 ApprenticeshipCount)
        {
            this.ProviderCount = ProviderCount;
            this.ApprenticeshipCount = ApprenticeshipCount;
        }
    }

    public class ApprenticeshipOverallCountReportViewModel
    {
        public List<ApprenticeshipOverallCountReportViewModelItem> Items { get; set; }

        public ApprenticeshipOverallCountReportViewModel()
        {
            Items = new List<ApprenticeshipOverallCountReportViewModelItem>();
        }
    }

    public class ApprenticeshipDetailedListUploadedReportViewModelItem
    {
        [LanguageDisplay("UKPRN")]
        public String UKPRN { get; set; }

        [LanguageDisplay("Provider Name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("Apprenticeship Count")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 ApprenticeshipCount { get; set; }

        [LanguageDisplay("Framework Count")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 FrameworkCount { get; set; }

        [LanguageDisplay("Standard Count")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 StandardCount { get; set; }

        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }
    }

    public class ApprenticeshipDetailedListUploadedReportViewModel
    {
        public List<ApprenticeshipDetailedListUploadedReportViewModelItem> Items { get; set; }

        public ApprenticeshipDetailedListUploadedReportViewModel()
        {
            Items = new List<ApprenticeshipDetailedListUploadedReportViewModelItem>();
        }
    }

    public class ProvidersWithOver10ApprenticeshipsReportViewModelItem
    {
        [LanguageDisplay("UKPRN")]
        public String UKPRN { get; set; }

        [LanguageDisplay("Provider Name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("Number of Apprenticeships")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfApprenticeships { get; set; }

        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }
    }

    public class ProvidersWithOver10ApprenticeshipsReportViewModel
    {
        public List<ProvidersWithOver10ApprenticeshipsReportViewModelItem> Items { get; set; }

        public ProvidersWithOver10ApprenticeshipsReportViewModel()
        {
            Items = new List<ProvidersWithOver10ApprenticeshipsReportViewModelItem>();
        }
    }

    public class QAdProvidersReportViewModelItem
    {
        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }

        [LanguageDisplay("UKPRN")]
        public Int32 UKPRN { get; set; }

        [LanguageDisplay("Provider Name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("Number of Apprenticeships")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfApprenticeships { get; set; }

        [LanguageDisplay("Number of Apprenticeships Required to QA")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberRequiredToQA { get; set; }

        [LanguageDisplay("Passed Overall?")]
        public String Passed
        {
            get { return PassedRaw == 1 ? Yes : No; }
        }

        [LanguageDisplay("Passed Compliance?")]
        public String PassedCompliance
        {
            get
            {
                return PassedComplianceRaw == null
                    ? NeverQAdForCompliance
                    : PassedComplianceRaw == 1
                        ? Yes
                        : No;
            }
        }

        [LanguageDisplay("Number of Apprenticeships QAd for Compliance")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberQAdForCompliance { get; set; }

        [LanguageDisplay("Passed Style?")]
        public String PassedStyle
        {
            get
            {
                return PassedStyleRaw == null
                    ? NeverQAdForStyle
                    : PassedStyleRaw == 1
                        ? Yes
                        : No;
            }
        }

        [LanguageDisplay("Number of Apprenticeships QAd for Style")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberQAdForStyle { get; set; }

        [LanguageDisplay("Import Batches")]
        public String ImportBatchNames { get; set; }

        private Int32 PassedRaw { get; set; }
        private Int32? PassedComplianceRaw { get; set; }
        private Int32? PassedStyleRaw { get; set; }

        private String Yes;
        private String No;
        private String NeverQAdForCompliance;
        private String NeverQAdForStyle;

        public QAdProvidersReportViewModelItem()
        {
            Yes = AppGlobal.Language.GetText("Report_AdminReports_Yes", "Yes");
            No = AppGlobal.Language.GetText("Report_AdminReports_No", "No");
            NeverQAdForCompliance = AppGlobal.Language.GetText("Report_AdminReports_NeverQAdForCompliance", "");
            NeverQAdForStyle = AppGlobal.Language.GetText("Report_AdminReports_NeverQAdForStyle", "");

        }
    }

    public class QAdProvidersReportViewModel
    {
        public List<QAdProvidersReportViewModelItem> Items { get; set; }

        public QAdProvidersReportViewModel()
        {
            Items = new List<QAdProvidersReportViewModelItem>();
        }
    }

    public class ProvidersSubmittedDataForQAReportViewModelItem
    {
        [LanguageDisplay("UKPRN")]
        public String UKPRN { get; set; }

        [LanguageDisplay("Provider Name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("Number of Apprenticeships")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfApprenticeships { get; set; }

        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }

        [LanguageDisplay("Import Batches")]
        public String ImportBatchNames { get { return String.Join(", ", ImportBatchList); }  }

        public List<String> ImportBatchList { get; set; }
    }

    public class ProvidersSubmittedDataForQAReportViewModel
    {
        public List<ProvidersSubmittedDataForQAReportViewModelItem> Items { get; set; }

        public ProvidersSubmittedDataForQAReportViewModel()
        {
            Items = new List<ProvidersSubmittedDataForQAReportViewModelItem>();
        }
    }

    public class RegisterOpeningReportViewModelItem
    {
        [LanguageDisplay("Number of Providers With Provider Level Data")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfProvidersWithProviderLevelData { get; set; }

        [LanguageDisplay("Number of Providers With Apprenticeship Data")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfProvidersWithApprenticeshipLevelData { get; set; }

        [LanguageDisplay("Number of Apprenticeship Offers")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfApprenticeshipOffers { get; set; }

        [LanguageDisplay("Number of Providers Who Have Applied in This Round")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfProvidersWhoHaveAppliedInRound { get; set; }

        [LanguageDisplay("Number of Providers Without Existing Apprenticeships")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfProvidersWithoutApprenticeshipLevelData { get; set; }

        [LanguageDisplay("Number of Providers Who Have Been Overall QAd")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfProvidersWhoHaveBeenOverallQAd { get; set; }

        [LanguageDisplay("Number of Providers Who Have Passed Overall QA")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfProvidersWhoHavePassedOverallQA { get; set; }

        [LanguageDisplay("Number of Providers Who Have Failed Overall QA")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfProvidersWhoHaveFailedOverallQA { get; set; }

        [LanguageDisplay("Failure Reason: Specific Employer Named")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 SpecificEmployerNamed { get; set; }

        [LanguageDisplay("Failure Reason: Unverifiable Claim")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 UnverifiableClaim { get; set; }

        [LanguageDisplay("Failure Reason: Incorrect Ofsted Grade Used")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 IncorrectOfstedGrade { get; set; }

        [LanguageDisplay("Failure Reason: Insufficient Detail")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 InsufficientDetail { get; set; }

        [LanguageDisplay("Failure Reason: Not Aimed at Employer")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NotAimedAtEmployer { get; set; }

        [LanguageDisplay("Number of Apprenticeship Offers QAd")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfApprenticeshipOffersQAd { get; set; }

        [LanguageDisplay("Number of Apprenticeships Offers Failed QA")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfApprenticeshipsOffersFailed { get; set; }
    }

    public class RegisterOpeningReportViewModel
    {
        public List<RegisterOpeningReportViewModelItem> Items { get; set; }

        [LanguageDisplay("Import Batch")]
        public Int32? ImportBatchId { get; set; }

        public RegisterOpeningReportViewModel()
        {
            Items = new List<RegisterOpeningReportViewModelItem>();
        }
    }

    public class RegisterOpeningDetailReportViewModelItem
    {
        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }

        [LanguageDisplay("Provider Name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("UKPRN")]
        public Int32 UKPRN { get; set; }

        [LanguageDisplay("Has Provider Level Data")]
        public String HasProviderLevelData { get; set; }

        [LanguageDisplay("Has Apprenticeship Data")]
        public String HasApprenticeshipLevelData { get; set; }

        [LanguageDisplay("Number of Apprenticeship Offers")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfApprenticeshipOffers { get; set; }

        [LanguageDisplay("Had Existing Apprenticeships")]
        public String HadApprenticeshipLevelData { get; set; }

        [LanguageDisplay("Has Been Overall QAd")]
        public String HasBeenOverallQAd { get; set; }

        [LanguageDisplay("Has Passed Overall QA")]
        public String HasPassedOverallQA { get; set; }

        [LanguageDisplay("Has Failed Overall QA")]
        public String HasFailedOverallQA { get; set; }

        [LanguageDisplay("Failure Reason: Specific Employer Named")]
        public String SpecificEmployerNamed { get; set; }

        [LanguageDisplay("Failure Reason: Unverifiable Claim")]
        public String UnverifiableClaim { get; set; }

        [LanguageDisplay("Failure Reason: Incorrect Ofsted Grade Used")]
        public String IncorrectOfstedGrade { get; set; }

        [LanguageDisplay("Failure Reason: Insufficient Detail")]
        public String InsufficientDetail { get; set; }

        [LanguageDisplay("Failure Reason: Not Aimed at Employer")]
        public String NotAimedAtEmployer { get; set; }

        [LanguageDisplay("Number of Apprenticeship Offers QAd")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfApprenticeshipOffersQAd { get; set; }

        [LanguageDisplay("Number of Apprenticeships Offers Failed QA")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int32 NumberOfApprenticeshipsOffersFailed { get; set; }

        [LanguageDisplay("Import Batches for Provider")]
        public String ImportBatches { get; set; }
    }

    public class RegisterOpeningDetailReportViewModel
    {
        public List<RegisterOpeningDetailReportViewModelItem> Items { get; set; }

        [LanguageDisplay("Import Batch")]
        public Int32? ImportBatchId { get; set; }

        public RegisterOpeningDetailReportViewModel()
        {
            ImportBatchId = 0;
            Items = new List<RegisterOpeningDetailReportViewModelItem>();
        }
    }

    public class ProvidersWithArchivedApprenticeshipsReportViewModel
    {
        [LanguageDisplay("Start Date")]
        [Display(Description = "Enter the start date for the report")]
        [DateDisplayFormat(ApplyFormatInEditMode = true, format = DateFormat.LongDate)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [LanguageDisplay("End Date")]
        [Display(Description = "Enter the end date for the report")]
        [DateDisplayFormat(ApplyFormatInEditMode = true, format = DateFormat.LongDate)]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public List<ProvidersWithArchivedApprenticeshipsReportViewModelItem> Items { get; set; }
    }

    public class ProvidersWithArchivedApprenticeshipsReportViewModelItem
    {
        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }

        [LanguageDisplay("Provider Name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("UKPRN")]
        public Int32 UKPRN { get; set; }

        [LanguageDisplay("Apprenticeships Archived")]
        public Int32 NumberOfArchivedApprenticeships { get; set; }

        [LanguageDisplay("Apprenticeships Unarchived")]
        public Int32 NumberOfUnarchivedApprenticeships { get; set; }

        [LanguageDisplay("Currently Archived")]
        public Int32 NumberOfCurrentArchivedApprenticeships { get; set; }

        [LanguageDisplay("Current Live")]
        public Int32 NumberOfCurrentLiveApprenticeships { get; set; }
    }

    public class ProvidersWithArchivedApprenticeshipsDetailedReportViewModel
    {
        [LanguageDisplay("Start Date")]
        [Display(Description = "Enter the start date for the report")]
        [DateDisplayFormat(ApplyFormatInEditMode = true, format = DateFormat.LongDate)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [LanguageDisplay("End Date")]
        [Display(Description = "Enter the end date for the report")]
        [DateDisplayFormat(ApplyFormatInEditMode = true, format = DateFormat.LongDate)]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public List<ProvidersWithArchivedApprenticeshipsDetailedReportViewModelItem> Items { get; set; }
    }

    public class ProvidersWithArchivedApprenticeshipsDetailedReportViewModelItem
    {
        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }

        [LanguageDisplay("Provider Name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("UKPRN")]
        public Int32 UKPRN { get; set; }

        [LanguageDisplay("Standard / Framework")]
        public String StandardOrFramework { get; set; }

        [LanguageDisplay("Details of Apprenticeship")]
        public String ApprenticeshipDetails { get; set; }

        [LanguageDisplay("No. Times Apprenticeship Archived")]
        public Int32 NumberOfTimesArchived { get; set; }

        [LanguageDisplay("No. Times Apprenticeship Unarchived")]
        public Int32 NumberOfTimesUnarchived { get; set; }

        [LanguageDisplay("Current Status")]
        public String CurrentStatus { get; set; }
    }

    public class RoATPProvidersRefreshedReportViewModel
    {
        [LanguageDisplay("Start Date")]
        [Display(Description = "Enter the start date for the report")]
        [DateDisplayFormat(ApplyFormatInEditMode = true, format = DateFormat.LongDate)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [LanguageDisplay("End Date")]
        [Display(Description = "Enter the end date for the report")]
        [DateDisplayFormat(ApplyFormatInEditMode = true, format = DateFormat.LongDate)]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public List<RoATPProvidersRefreshedReportViewModelItem> Items { get; set; }
    }


    public class RoATPProvidersRefreshedReportViewModelItem
    {
        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }

        [LanguageDisplay("Provider Name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("UKPRN")]
        public Int32 UKPRN { get; set; }

        [LanguageDisplay("RoATP Start Date")]
        public DateTime? RoATPStartDate { get; set; }

        [LanguageDisplay("RoATP Provider Type")]
        public String RoATPProviderType { get; set; }

        [LanguageDisplay("Import Batch")]
        public String ImportBatchName { get; set; }

        [LanguageDisplay("Date Confirmed")]
        public DateTime? RefreshTimeUtc { get; set; }

        [LanguageDisplay("Confirmed By")]
        public String ConfirmedBy { get; set; }
    }

    public class RoATPProvidersNotRefreshedReportViewModel
    {
        [LanguageDisplay("Start Date")]
        [Display(Description = "Enter the start date for the report")]
        [DateDisplayFormat(ApplyFormatInEditMode = true, format = DateFormat.LongDate)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [LanguageDisplay("End Date")]
        [Display(Description = "Enter the end date for the report")]
        [DateDisplayFormat(ApplyFormatInEditMode = true, format = DateFormat.LongDate)]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public List<RoATPProvidersNotRefreshedReportViewModelItem> Items { get; set; }
    }


    public class RoATPProvidersNotRefreshedReportViewModelItem
    {
        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }

        [LanguageDisplay("Provider Name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("UKPRN")]
        public Int32 UKPRN { get; set; }

        [LanguageDisplay("RoATP Start Date")]
        public DateTime? RoATPStartDate { get; set; }

        [LanguageDisplay("RoATP Provider Type")]
        public String RoATPProviderType { get; set; }

        [LanguageDisplay("Import Batch")]
        public String ImportBatchName { get; set; }

        [LanguageDisplay("RoATP Refresh Override")]
        public bool? TASRefreshOverride { get; set; }
    }

    #endregion

    #region Misc Reports

    public class UsageStatisticsReportViewModel
    {
        public string BaseUrl { get; set; }
        public Dictionary<DateTime, List<string>> Items { get; set; }
    }

    #endregion
}