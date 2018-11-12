// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Defines the Constants type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    /// <summary>
    /// Class holding constants values and settings
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Initializes static members of the <see cref="Constants"/> class.
        /// </summary>
        static Constants()
        {
            ConfigSettings = null;
        }

        /// <summary>
        /// Gets or sets the configuration settings object
        /// </summary>
        public static ConfigurationSettings ConfigSettings { get; set; }

        /// <summary>
        /// Field names used for saving values in the Session
        /// </summary>
        public static class SessionFieldNames
        {
            /// <summary>
            /// The user name.
            /// </summary>
            public const string UserName = "UserName";

            /// <summary>
            /// The user's real name
            /// </summary>
            public static string UserRealName = "UserRealName";

            /// <summary>
            /// The permissions.
            /// </summary>
            public const string Permissions = "Permissions";

            /// <summary>
            /// The user id.
            /// </summary>
            public const string UserId = "UserId";

            /// <summary>
            /// The current user context.
            /// </summary>
            public const string UserContext = "UserContext";

            /// <summary>
            /// The primary role of the current user.
            /// </summary>
            public static string UserRole = "UserRole";

            /// <summary>
            /// The provider last activity date string.
            /// </summary>
            public const string ProviderLastActivity = "ProviderLastActivity";

            /// <summary>
            /// The organisation last activity date string.
            /// </summary>
            public const string OrganisationLastActivity = "OrganisationLastActivity";

            /// <summary>
            /// The provider quality score rating 1 to 4.
            /// </summary>
            public const string ProviderQualityScore = "ProviderQualityScore";

            /// <summary>
            /// The last time the provider updated their provision
            /// </summary>
            public const String ProviderLastProvisionUpdate = "ProviderLastProvisionUpdate";

            /// <summary>
            /// Is the current session authenticated?
            /// </summary>
            public const string IsAuthenticated = "IsAuthenticated";

            /// <summary>
            /// The provider is SFA funded.
            /// </summary>
            public static string ProviderIsSfaFunded = "ProviderIsSfaFunded";

            /// <summary>
            /// The provider is DfE funded.
            /// </summary>
            public static string ProviderIsDfe1619Funded = "ProviderIsDfe1619Funded";

            /// <summary>
            /// The provider type (DFEProviderTypeId).
            /// </summary>
            public static string ProviderType = "ProviderType";

            /// <summary>
            /// The organisation is SFA funded.
            /// </summary>
            public static string OrganisationIsSfaFunded = "OrganisationIsSfaFunded";

            /// <summary>
            /// The organisation is DfE funded
            /// </summary>
            public static string OrganisationIsDfe1619Funded = "OrganisationIsDfe1619Funded";

            /// <summary>
            /// The user is a secure access user.
            /// </summary>
            public static string IsSecureAccessUser = "IsSecureAccessUser";

            /// <summary>
            /// The last time a user session interacted with the site.
            /// </summary>
            public static string SessionLastAccessDateTimeUtc = "SessionLastAccessDateTimeUtc";

            /// <summary>
            /// The session has timed out.
            /// </summary>
            public static string SessionTimingOut = "SessionTimingOut";


            /// <summary>
            /// Indicates whether user wizard has already been shown on login
            /// </summary>
            public const string ShowWizardOnLogin = "ShowWizardOnLogin";

            /// <summary>
            /// UTC Date Time which Provider Quality Score was last calculated.
            /// </summary>
            public const string ProviderQALastCalculated = "ProviderQALastCalculated";

        }

        /// <summary>
        /// Email templates
        /// </summary>
        public enum EmailTemplates
        {
            /// <summary>
            /// Base template used for all emails.
            /// </summary>
            Base = 1,

            /// <summary>
            /// Please confirm your account by clicking on the %URL%.
            /// </summary>
            EmailConfirmation = 2,

            /// <summary>
            /// To reset your password click on the %URL%.
            /// </summary>
            PasswordReset = 3,

            /// <summary>
            /// Welcome to the site. Please confirm your account by clicking on the %URL%.
            /// </summary>
            NewUserWelcome = 4,

            /// <summary>
            /// To create a password click on the %URL%.
            /// </summary>
            PasswordCreate = 5,

            /// <summary>
            /// Your login details have changed, an account confirmation email has been sent to %NEWEMAIL%.
            /// </summary>
            LoginDetailsChanged = 6,

            /// <summary>
            /// Obsolete - replaced by templates 38, 39 and 40
            /// </summary>
            //BulkUploadFailed = 7,

            ///// <summary>
            ///// Obsolete - replaced by templates 32, 33, 34 and 35
            ///// </summary>
            //BulkUploadConfirmationRequired = 8,

            /// <summary>
            ///  Obsolete - replaced by templates 36 and 37
            /// </summary>
            //BulkUploadSuccess = 9,

            /// <summary>
            /// A new provider has been added to the system.
            /// </summary>
            NewProviderNotification = 10,

            /// <summary>
            /// A new organisation has been added to the system.
            /// </summary>
            NewOrganisationNotification = 11,

            /// <summary>
            /// A notice they’ve gone into amber status on the day they do (SFA and SFA/DfE providers)
            /// </summary>
            SfaProviderTrafficLightIsNowAmber = 12,

            /// <summary>
            /// A notice one week after first becoming amber (SFA and SFA/DfE providers)
            /// </summary>
            SfaProviderTrafficLightIsAmberWeek1 = 13,

            /// <summary>
            /// A notice one week before becoming red (SFA and SFA/DfE providers)
            /// </summary>
            SfaProviderStatusRedInOneWeek = 14,

            /// <summary>
            /// A notice they’ve gone into red status on the day they do (SFA and SFA/DfE providers)
            /// </summary>
            SfaProviderTrafficLightIsNowRed = 15,

            /// <summary>
            /// Weekly reminder emails thereafter (SFA and SFA/DfE providers)
            /// </summary>
            SfaProviderTrafficLightIsRedWeeklyReminder = 16,

            /// <summary>
            /// A notice they’ve gone into amber status on the day they do (DfE only providers)
            /// </summary>
            Dfe1619ProviderTrafficLightIsNowAmber = 17,

            /// <summary>
            /// A notice one week after first becoming amber (DfE only providers)
            /// </summary>
            Dfe1619ProviderTrafficLightIsAmberWeek1 = 18,

            /// <summary>
            /// A notice one week before becoming red (DfE only providers)
            /// </summary>
            Dfe1619ProviderStatusRedInOneWeek = 19,

            /// <summary>
            /// A notice they’ve gone into red status on the day they do (DfE only providers)
            /// </summary>
            Dfe1619ProviderTrafficLightIsNowRed = 20,

            /// <summary>
            /// Weekly reminder emails thereafter (DfE only providers)
            /// </summary>
            Dfe1619ProviderTrafficLightIsRedWeeklyReminder = 21,

            /// <summary>
            /// Provider notification sent when they are invited to join an organisation.
            /// </summary>
            ProviderInviteNotification = 22,

            /// <summary>
            /// Provider notification sent when their invite to join an organisation is withdrawn.
            /// </summary>
            ProviderInviteWithdrawn = 23,

            /// <summary>
            /// Provider notification sent when they are removed from an organisation.
            /// </summary>
            ProviderRemovedFromOrganisation = 24,

            /// <summary>
            /// Organisation notification sent when a provider has accepted an invitation and opted to allow the organisation to manage their data.
            /// </summary>
            ProviderInviteAcceptedCanEdit = 25,
            
            /// <summary>
            /// Organisation notification sent when a provider has accepted an invitation and opted to not allow the organisation to manage their data.
            /// </summary>
            ProviderInviteAcceptedCannotEdit = 26,
            
            /// <summary>
            /// Organisation notification sent when a provider invite is rejected.
            /// </summary>
            ProviderInviteRejected = 27,

            /// <summary>
            /// Organisation notification sent when a provider is deleted.
            /// </summary>
            OrganisationProviderDeleted = 28,
            
            /// <summary>
            /// Organisation notification sent when a provider leaves the organisation.
            /// </summary>
            ProviderLeftOrganisation = 29,

            /// <summary>
            /// Organisation notification sent when a provider allows the organisation to manage their data.
            /// </summary>
            ProviderAllowedOrganisationToManageData = 30,

            /// <summary>
            /// Organisation notification sent when a provider disallows the organisation to manage their data.
            /// </summary>
            ProviderDisallowedOrganisationToManageData = 31,

            /// <summary>
            /// The bulk upload file contains significantly less course data than currently exists in the Course Directory DB
            /// </summary>
            BulkUploadCourseThresholdConfirmationRequired = 32,

            /// <summary>
            /// The bulk upload file contains significantly less apprenticeship data than currently exists in the Course Directory DB
            /// </summary>
            BulkUploadApprenticeshipThresholdConfirmationRequired = 33,

            /// <summary>
            /// Warning messages were generated against the uploaded course file
            /// </summary>
            BulkUploadCourseWarningAndNoticesConfirmationRequired = 34,

            /// <summary>
            // Warning messages were generated against the uploaded apprenticeship file
            /// </summary>
            BulkUploadApprenticeshipWarningAndNoticesConfirmationRequired = 35,

            /// <summary>
            /// Succesful import of a Bulk Upload file of course data
            /// </summary>
            BulkUploadCourseSuccess = 36,

            /// <summary>
            /// Succesful import of a Bulk Upload file of apprenticeship data
            /// </summary>
            BulkUploadApprenticeshipSuccess = 37,

            /// <summary>
            /// Errors generated during import of a Bulk Upload file of course data
            /// </summary>
            BulkUploadCourseFailure = 38,

            /// <summary>
            /// Errors generated during import of a Bulk Upload file of apprenticeship data
            /// </summary>
            BulkUploadApprenticeshipFailure = 39,

            /// <summary>
            /// Errors generated during import of a Bulk Upload file where the system cannot determine whether the file 
            /// holds course or apprenticeship data.
            /// </summary>
            BulkUploadFailureUnknownDataType = 40,

            /// <summary>
            /// Email sent when provider passes QA checks
            /// </summary>
            ProviderPassedQAChecks = 41,

            /// <summary>
            /// Email sent when provider fails QA checks
            /// </summary>
            ProviderFailedQAChecks = 42,

            /// <summary>
            /// Email sent to support desk when provider has indicated that their data is ready to QA
            /// </summary>
            ProviderApprenticeshipDataReadyToQA = 43,
            
            /// <summary>
            /// Email sent to support desk when provider has indicated that they have new text erady to submit to QA
            /// </summary>
            ProviderApprenticeshipSubmitNewTextToQA = 44,

            /// <summary>
            /// Sent when the LARS file has not been imported for x days (where x is configured in the ConfigurationSettings table)
            /// </summary>
            LARSFileNotImportSinceXDaysAgo = 45,

            /// <summary>
            /// Sent when the automated LARS import throws an exception
            /// </summary>
            LARSImportError = 46,

            /// <summary>
            /// Sent when the automated RoATP import throws an exception
            /// </summary>
            RoATPImportError = 47,

            /// <summary>
            /// Email sent when provider passes QA checks but also has style failures
            /// </summary>
            ProviderPassedQAChecksFailedStyle = 48
        }

        /// <summary>
        /// Enumeration of the usertypes in the db.ProviderUserType table. This is not automatically kept in step with the database.
        /// </summary>
        public enum ProviderUserTypes
        {
            NormalUser = 1,
            InformationOfficer = 2,
            RelationshipManager = 3,
            NcsCourseLead = 4,
            Dart = 5,
        }

        /// <summary>
        /// Enumeration of the usertypes in the db.RecordStatus table. This is not automatically kept in step with the database.
        /// </summary>
        public enum RecordStatus
        {
            /// <summary>
            /// Record is pending.
            /// </summary>
            Pending = 1,

            /// <summary>
            /// Record is live.
            /// </summary>
            Live = 2,

            /// <summary>
            /// Record is archived.
            /// </summary>
            Archived = 3,

            /// <summary>
            /// Record is deleted.
            /// </summary>
            Deleted = 4
        }

        /// <summary>
        /// Enumeration of the usertypes in the db.Application table. This is not automatically kept in step with the database.
        /// </summary>
        public enum Application
        {
            Portal = 1,
            BulkUpload = 2,
            UcasImport = 3
        }

        /// <summary>
        /// Enumeration of the Metadata Upload types
        /// </summary>
        public enum MetadataUploadType
        {
            // Address upload
            AddressBase = 1,
            // LARS
            LearnDirectClassification = 2,
            LearningAimAwardOrg = 3,
            LearningAim = 4,
            LearningAimValidity = 5,
            Standards = 9,
            Frameworks = 21,
            ProgTypes = 22,
            SectorSubjectAreaTier1 = 23,
            SectorSubjectAreaTier2 = 24,
            StandardSectorCodes = 25,
            UCASQualifications = 26,
            // FE Choices
            FEChoices = 6,
            // UKRLP
            UKRLP = 7,
            // UCAS
            UCASCourseEntry = 8,
            UCASCourses = 10,
            UCASCoursesIndex = 11,
            UCASCurrencies = 12,
            UCASFees = 14,
            UCASFeeYears = 15,
            UCASOrgs = 16,
            UCASPlacesOfStudy = 17,
            UCASStarts = 18,
            UCASStartsIndex = 19,
            UCASTowns = 20,
            // UCAS PG
            UCAS_PG_Courses = 27,
            UCAS_PG_Providers = 28,
            UCAS_PG_Locations = 29,
            UCAS_PG_CourseOptions = 30,
            UCAS_PG_CourseOptionFees = 31,
            // Code Point
            CodePoint = 32,
            //Provider Import
            ProviderImport = 33
        }

        public enum ErrorCodes
        {
            SecureAccessCreateError = 2327,
            SecureAccessCreateFailed = 2386,
            SecureAccessEmailInUse = 2181,
            SecureAccessUserNameInUse = 2493
        }

        public enum CourseSearchQAFilter
        {
            CoursesUpToDate = 1,
            CoursesPending = 2,
            CoursesExpiring = 3,
            CoursesOutOfDate = 4,
            LearningAimExpired = 5,
            LearningAimNone = 6,
            CourseShortSummary = 7,
            CourseNonDistinctSummary = 8
        }

        public enum OpportunitySearchQAFilter
        {
            OpportunitiesUpToDate = 1,
            OpportunitiesPending = 2,
            OpportunitiesExpiring = 3,
            OpportunitiesOutOfDate = 4
        }

        public enum CourseFilterDateStatus
        {
            UpToDate = 1,
            Expiring = 2,
            OutOfDate = 3
        }

        public enum OpportunityFilterDateStatus
        {
            UpToDate = 1,
            Expiring = 2,
            OutOfDate = 3
        }

        #region Bulk Upload


        /// <summary>
        /// Section name expected to be there in Bulk upload csv file.
        /// </summary>
        public enum BulkUpload_DataType
        {
            /// <summary>
            /// 
            /// </summary>
            CourseData = 1,

            /// <summary>
            /// 
            /// </summary>
            ApprenticeshipData = 2
        }

        /// <summary>
        /// Section names in Bulk upload csv file.
        /// </summary>
        public enum BulkUpload_SectionName
        {
            Providers = 1,
            Venues = 2,
            Courses = 3,
            Opportunities = 4,
            Locations = 5,
            Apprenticeships = 6,
            DeliveryLocations = 7
        }

        /// <summary>
        /// Distinguish between Errors and Warnings
        /// </summary>
        public enum BulkUpload_Validation_ErrorType
        {
            /// <summary>
            /// Errors, user not allowed to proceed in case of erros
            /// </summary>
            Error = 1,

            /// <summary>
            /// Warnings, user would only be alerted for any issue.
            /// </summary>
            Warning = 2,

            /// <summary>
            /// Use notice when you want to inform the user of something but not stop the record from being imported (still requires confirmation).
            /// </summary>
            Notice = 5
        }

        /// <summary>
        /// status of bulk upload
        /// </summary>
        public enum BulkUploadStatus
        {
            Failed_Stage_1_of_4 = 1,
            Failed_Stage_2_of_4 = 2,
            Failed_Stage_3_of_4 = 3,
            Failed_Stage_4_of_4 = 4,
            Needs_Confirmation = 5,
            Aborted = 6,
            Published = 7,
            UnknownException = 8,
            NoValidRecords = 9,
            Unvalidated = 10,
            ConfirmationReceived = 11,
            Validated = 12
        }

        public enum BulkUploadValidationStages
        {
            Stage1 = 1,
            Stage2 = 2,
            Stage3 = 3,
            Stage4 = 4
        }

        public static class BulkUpload_Headers_Provide
        {
            /// <summary>
            /// 
            /// </summary>
            public const string ProviderName = "PROVIDER_NAME";

            /// <summary>
            /// 
            /// </summary>
            public const string Alias = "PROVIDER_ALIAS";

            /// <summary>
            /// 
            /// </summary>
            public const string Address1 = "ADMIN_ADDRESS_1";

            /// <summary>
            /// 
            /// </summary>
            public const string Address2 = "ADMIN_ADDRESS_2";

            /// <summary>
            /// 
            /// </summary>
            public const string Town = "ADMIN_TOWN";

            /// <summary>
            /// 
            /// </summary>
            public const string County = "ADMIN_COUNTY";

            /// <summary>
            /// 
            /// </summary>
            public const string PostCode = "ADMIN_POSTCODE";

            /// <summary>
            /// 
            /// </summary>
            public const string Email = "ADMIN_EMAIL";

            /// <summary>
            /// 
            /// </summary>
            public const string Website = "ADMIN_WEBSITE";

            /// <summary>
            /// 
            /// </summary>
            public const string Phone = "ADMIN_PHONE";

            /// <summary>
            /// 
            /// </summary>
            public const string Fax = "ADMIN_FAX";

        }

        public static class BulkUpload_Headers_Venue
        {
            /// <summary>
            /// 
            /// </summary>
            public const string VenueId = "VENUE_ID*";

            /// <summary>
            /// 
            /// </summary>
            public const string ProviderVenueId = "PROVIDER_VENUE_ID";

            /// <summary>
            /// 
            /// </summary>
            public const string VenueName = "VENUE_NAME*";

            /// <summary>
            /// 
            /// </summary>
            public const string Address1 = "ADDRESS_1*";

            /// <summary>
            /// 
            /// </summary>
            public const string Address2 = "ADDRESS_2";

            /// <summary>
            /// 
            /// </summary>
            public const string Town = "TOWN*";

            /// <summary>
            /// 
            /// </summary>
            public const string County = "COUNTY";

            /// <summary>
            /// 
            /// </summary>
            public const string PostCode = "POSTCODE*";

            /// <summary>
            /// 
            /// </summary>
            public const string Email = "EMAIL";

            /// <summary>
            /// 
            /// </summary>
            public const string Website = "WEBSITE";

            /// <summary>
            /// 
            /// </summary>
            public const string Phone = "PHONE";

            /// <summary>
            /// 
            /// </summary>
            public const string Fax = "FAX";
            /// <summary>
            /// 
            /// </summary>
            public const string Facilities = "FACILITIES";

        }

        public static class BulkUpload_Headers_Course
        {
            /// <summary>
            /// 
            /// </summary>
            public const string CourseId = "COURSE_ID*";

            /// <summary>
            /// 
            /// </summary>
            public const string LearningAimId = "LAD_ID";

            /// <summary>
            /// 
            /// </summary>
            public const string CourseTitle = "PROVIDER_COURSE_TITLE*";

            /// <summary>
            /// 
            /// </summary>
            public const string Summary = "SUMMARY*";

            /// <summary>
            /// 
            /// </summary>
            public const string ProviderCourseId = "PROVIDER_COURSE_ID";

            /// <summary>
            /// 
            /// </summary>
            public const string URL = "URL*";

            /// <summary>
            /// 
            /// </summary>
            public const string BookingURL = "BOOKING_URL";

            /// <summary>
            /// 
            /// </summary>
            public const string EntryRequirement = "ENTRY_REQUIREMENTS*";

            /// <summary>
            /// 
            /// </summary>
            public const string AssesmentMethod = "ASSESSMENT_METHOD";

            /// <summary>
            /// 
            /// </summary>
            public const string EquipmentRequired = "EQUIPMENT_REQUIRED";

            /// <summary>
            /// 
            /// </summary>
            public const string QualificationType = "QUALIFICATION_TYPE*";

            /// <summary>
            /// 
            /// </summary>
            public const string QualificationTitle = "QUALIFICATION_TITLE";

            /// <summary>
            /// 
            /// </summary>
            public const string AwardingOrganisation = "AWARDING_ORG_NAME";

            /// <summary>
            /// 
            /// </summary>
            public const string QualificationLevel = "QUALIFICATION_LEVEL";

            /// <summary>
            /// 
            /// </summary>
            public const string LDSC1 = "LDCS1";

            /// <summary>
            /// 
            /// </summary>
            public const string LDSC2 = "LDCS2";

            /// <summary>
            /// 
            /// </summary>
            public const string LDSC3 = "LDCS3";

            /// <summary>
            /// 
            /// </summary>
            public const string LDSC4 = "LDCS4";

            /// <summary>
            /// 
            /// </summary>
            public const string LDSC5 = "LDCS5";

            /// <summary>
            /// 
            /// </summary>
            public const string Tariff = "UCAS_TARIFF";
        }

        public static class BulkUpload_Headers_Opportunity
        {
            /// <summary>
            /// 
            /// </summary>
            public const string CourseId = "COURSE_ID*";

            /// <summary>
            /// 
            /// </summary>
            public const string VenueRegionName = "VENUE_ID/REGION_NAME";

            /// <summary>
            /// 
            /// </summary>
            public const string ProviderOpportunityId = "PROVIDER_OPPORTUNITY_ID";

            /// <summary>
            /// 
            /// </summary>
            public const string StudyMode = "STUDY_MODE*";

            /// <summary>
            /// 
            /// </summary>
            public const string AttendanceMode = "ATTENDANCE_MODE*";

            /// <summary>
            /// 
            /// </summary>
            public const string AttendancePattern = "ATTENDANCE_PATTERN*";

            /// <summary>
            /// 
            /// </summary>
            public const string Duration = "DURATION*";

            /// <summary>
            /// 
            /// </summary>
            public const string DurationUnits = "DURATION_UNITS*";

            /// <summary>
            /// 
            /// </summary>
            public const string DurationDescription = "DURATION_DESCRIPTION*";

            /// <summary>
            /// 
            /// </summary>
            public const string StartDate = "START_DATE*";

            /// <summary>
            /// 
            /// </summary>
            public const string EndDate = "END_DATE*";

            /// <summary>
            /// 
            /// </summary>
            public const string StartDateDescription = "START_DATE_DESCRIPTION*";

            /// <summary>
            /// 
            /// </summary>
            public const string TimeTable = "TIMETABLE";

            /// <summary>
            /// 
            /// </summary>
            public const string Price = "PRICE*";

            /// <summary>
            /// 
            /// </summary>
            public const string PriceDescription = "PRICE_DESCRIPTION*";

            /// <summary>
            /// 
            /// </summary>
            public const string LanguageOfInstruction = "LANGUAGE_OF_INSTRUCTION";

            /// <summary>
            /// 
            /// </summary>
            public const string LanguageAssesment = "LANGUAGE_OF_ASSESSMENT";

            /// <summary>
            /// 
            /// </summary>
            public const string PlacesAvailable = "PLACES_AVAILABLE";

            /// <summary>
            /// 
            /// </summary>
            public const string ApplyFrom = "APPLY_FROM";

            /// <summary>
            /// 
            /// </summary>
            public const string ApplyUntil = "APPLY_UNTIL";

            /// <summary>
            /// 
            /// </summary>
            public const string ApplyUntilDesc = "APPLY_UNTIL_DESC";

            /// <summary>
            /// 
            /// </summary>
            public const string ApplyThroughtoutYear = "APPLY_THROUGHOUT_YEAR";

            /// <summary>
            /// 
            /// </summary>
            public const string EnquireTo = "ENQUIRE_TO";

            /// <summary>
            /// 
            /// </summary>
            public const string ApplyTo = "APPLY_TO";

            /// <summary>
            /// 
            /// </summary>
            public const string Url = "URL";

            /// <summary>
            /// 
            /// </summary>
            public const string A10 = "A10*";

            /// <summary>
            /// 
            /// </summary>
            public const string OfferedBy = "OFFERED_BY";

            /// <summary>
            /// 
            /// </summary>
            public const string DisplayName = "DISPLAY_NAME";

            /// <summary>
            /// 
            /// </summary>
            public const string BothSearchable = "BOTH_SEARCHABLE";
        }

        public const char PipeSeperator = '|';

        public static class BulkUpload_History
        {
            public static string Successful_text = "Successful";

            public static string Failure_text = "Failed Validation at Stage {0} of {1}";
        }

        public static char BulkUploadA10SplitCharacter = '|';

        public static char CommaSplitCharacter = ',';

        public static string BulkDownloadFileExtension = "csv";

        public static string ProviderIdColumneName = "ProviderId";
 

        public static class BulkUpload_DataStandardMessage_Common
        {
            /// <summary>
            /// 
            /// </summary>
            public static string Mandatory = AppGlobal.Language.GetText("BulkUpload_Constants_ThisFieldIsManndatory", "This is a mandatory field and therefore it must be completed.");

            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_Size = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidSize", "Character exceeds size limit : {0}");

            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_Date_Format = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidDateFormat", "Invalid Date entered.");

            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_Url_Characters = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidUrl", "Please check the URL provided has been entered correctly and is in the correct format. Please make sure 'http://' or 'https://' has been included in the URL.");

            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_Integer = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidInteger", "Invalid Value entered.");            

            public static string Invalid_Size_between = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidSizeBetween", "Character size should be between {0} and {1}");

            public static string InvalidPermissions = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidPermissions", "You don't have sufficient permission for bulk upload for provider id {0}.");

            public static string InvalidProviderInformation = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidProviderInfomration", "Invalid provider information found in upload file.");

            public static string MissingProviderInformation = AppGlobal.Language.GetText("BulkUpload_Constants_MissingProviderInfomration", "Missing provider information in upload file.");

            public static string InvalidFileExtension = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidFileExtension", "Invalid File Extension {0}");

            public static string InvalidSectionName = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidSectionName", "   Invalid sections in csv file.");

            public static string InvalidSectionProvider = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidSectionProvider", "Provider section not found.");

            public static string MoreThanMoreProviderInformation = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidSectionCount", "More than one provider information found.");
           
            public static string InvalidSectionCourse = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidSectionCourse", "Invalid Course section.");
           
            public static string InvalidSectionVenue = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidSectionVenue", "Invalid Venue section.");
            
            public static string InvalidSectionOpportunity = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidSectionOpportunity", "Invalid Opportunity section.");
            
            public static string ExpectingSection = AppGlobal.Language.GetText("BulkUpload_Constants_ExpectingSectionName", "   Expecting section {0} at line no {1}");

            public static string ColumnCountNotMatch = AppGlobal.Language.GetText("BulkUpload_Constants_ColumnCountNotMatch", "Columns count doesnot match for {0} section, Provider Id {1}.");

            public static string ExpectingColumnNotFound = AppGlobal.Language.GetText("BulkUpload_Constants_ExpectingColumnNotFound", "Expecting column {0} in section {1} providerId {2}.");

            public static string InvalidDataLength = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidDataLength", "Invalid data in section {0} provider {1}. It exceeds the limit of {2} characters.");
            
            public static string ThresholdValidation=AppGlobal.Language.GetText("BulkUpload_Constants_ThresholdValidation", "{0} contains considerably fewer valid records than you currently have on our database.  Acceptable threshold value defined is {1} however current accuracy is {2}.");

            public static string ProviderAlreadyExists_withName = AppGlobal.Language.GetText("BulkUpload_Constants_ProviderAlreadyExistsWithName", "Existing provider already exists with this name.");
           
            public static string SectionShouldHave_atleast_two_Rows= AppGlobal.Language.GetText("BulkUpload_Constants_SectionShouldHaveAtleastTwoRows", "{0} section must contain at least two rows - heading row followed by data.");
           
       
        }

        public static class BulkUpload_Common_DataStandards_Size
        {

            /// <summary>
            /// 
            /// </summary>
            public static int String_Empty = 0;

            /// <summary>
            /// 
            /// </summary>
            public static int String_255 = 255;

            /// <summary>
            /// 
            /// </summary>
            public static int? String_50 = 50;

            public static int? String_100 = 100;
            
            public static int String_150 = 150;

            public static int? String_200 = 200;

            public static int String_1000 = 1000;

            /// <summary>
            /// 
            /// </summary>
            public static int String_4000 = 4000;

            /// <summary>
            /// 
            /// </summary>
            public static int String_2000 = 2000;

            /// <summary>
            /// 
            /// </summary>
            public static int? ZeroLength = null;

            /// <summary>
            /// 
            /// </summary>
            public static int? Integer_Min_Range = 0; //max length of int32

            /// <summary>
            /// 
            /// </summary>
            public static int? Integer_Max_Range = 2147483647; //max length of int32

            /// <summary>
            /// 
            /// </summary>
            public static int? Address_MinLength = null;

            /// <summary>
            /// 
            /// </summary>
            public static int? Address_MaxLength = 40;

            /// <summary>
            /// 
            /// </summary>
            public static int? Email_MinLength = 0;

            /// <summary>
            /// 
            /// </summary>
            public static int? Email_MaxLength = 254;

            /// <summary>
            /// 
            /// </summary>
            public static int? Web_MinLength = 0;

            /// <summary>
            /// 
            /// </summary>
            public static int? Web_MaxLength = 255;

            /// <summary>
            /// 
            /// </summary>
            public static int? PhoneNumer_MaxLength = 30;
           
            /// <summary>
            /// 
            /// </summary>
            public static int ProviderCourseIdLength= 50;

           
        }

        public static class BulkUpload_Venue_DataStandards_Size
        {
            /// <summary>
            /// 
            /// </summary>
            public static int? Facilities_Length = 2000;

            public static int AddressLength = 40;
        }

        public static class BulkUpload_DataStandardMessage_Course
        {
            /// <summary>
            /// 
            /// </summary>
            public static string Mandatory_Course_Title = AppGlobal.Language.GetText("BulkUpload_Constants_MandatoryCourseTitle", "Provider Course Title is empty");

            /// <summary>
            /// 
            /// </summary>
            public static int? LearningAim_MinLength = null;

            /// <summary>
            /// 
            /// </summary>
            public static int? LearningAim_MaxLength = 10;

            /// <summary>
            /// 
            /// </summary>
            public static int? CourseSummary_Min_Length = 1;

            /// <summary>
            /// 
            /// </summary>
            public static int? CourseSummary_Max_Length = 2000;

            /// <summary>
            /// 
            /// </summary>
            public static int? Entry_MaxLength = 4000;

            /// <summary>
            /// 
            /// </summary>
            public static int? Entry_MinLength = 1;

            /// <summary>
            /// 
            /// </summary>
            public static int? String_Minimum = null;

            /// <summary>
            /// 
            /// </summary>
            public static int? String_255 = 255;

            /// <summary>
            /// 
            /// </summary>
            public static int? AssesmentMethod_MaxLength = 4000;

            /// <summary>
            /// 
            /// </summary>
            public static int? AssesmentMethod_MinLength = null;

            /// <summary>
            /// 
            /// </summary>
            public static string Qualification_Type_Mandatory = AppGlobal.Language.GetText("BulkUpload_Constants_QualificationTypeMandatory", "This field is mandatory, please select the type of Qualification awarded at the end of the course");

            public static string Qualification_Type_Mandatory_When_Lar_Qualification_Type_Is_Null = AppGlobal.Language.GetText("BulkUpload_Constants_QualificationTypeMandatoryWhenLarQualificationTypeIsNull", "This field is mandatory when the Learning Aim supplied does not have a Qualification Type associated with it, please select the type of Qualification awarded at the end of the course");

            /// <summary>
            /// 
            /// </summary>
            public static string Qualification_Type_Invalid = AppGlobal.Language.GetText("BulkUpload_Constants_QualificationTypeInvalid", "Invalid qualification type entered.");

            public static string Qualification_Type_No_Qualification = AppGlobal.Language.GetText("BulkUpload_Constants_QT1", "QT1");

            public static string Qualification_Type_NotSelected_With_Qualification_Title = AppGlobal.Language.GetText("BulkUpload_Constants_QualificationTypeNotSelectedWithTitle", "'No Qualification' cannot be selected if the 'Qualification Title' field has been completed. Please select the correct qualification type.");

            public static string Qualification_Type_Different_To_Lar = AppGlobal.Language.GetText("BulkUpload_Constants_QualificationTypeDifferentToLar", "Both a Learning Aim and a Qualification Type have been supplied. The Qualification Type will be taken from the Learning Aim and not the one supplied in the Qualification Type field.");

            public static string LengthNotMatchingWithExpected = AppGlobal.Language.GetText("BulkUpload_Constants_LengthNotMatchingWithExpected", "Length  for Column {0}, actual value {1}, expected length {2}, in Section {3}, Provider id : {4}.");

            public static string InvalidColIdForProvider = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidColumnId", "Invalid Column Id {0} in Section {1} Provider id : {2}.");

            public static string BlankCourseId = AppGlobal.Language.GetText("BulkUpload_Constants_BlankCourseId", "Blank Course Id for line number {0}.");

            public static string CourseIdNotUnique = AppGlobal.Language.GetText("BulkUpload_Constants_CourseIdNotUnique", "Course Id {0} not unique, section name {1} provider Id {2}.");

            public static string InvalidLearningAim = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidLearningAim", "Invalid Learning Aim Reference : {0}, section : {1}, provider : {2}.");

            public static string InvalidQualificationType = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidQualificationType", "Invalid Qualification Type : {0}, section : {1}, provider : {2}.");

            public static string InvalidQualificationLevel = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidQualificationLevel", "Invalid Qualification Level : {0}, section : {1}, provider : {2}.");

            public static string InvalidLearnDirectClassification = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidLearnDirectClassification", "Invalid Learn Direct classification : {0}, section : {1}, provider : {2}.");

            public static string DuplicateLearnDirectClassification = AppGlobal.Language.GetText("BulkUpload_Constants_DuplicateLearnDirectClassification", "Course has duplicate Learn Direct classifications : course: {0}, section: {1}, provider : {2}.");

            public static string BlankLearnDirectClassification = AppGlobal.Language.GetText("BulkUpload_Constants_BlankLearnDirectClassification", "Course has a blank Learn Direct classification with subsequent classifications : course: {0}, blank classification : LDSC{1},  section: {2}, provider : {3}.");

            public static string CourseHadNoOpportunity = AppGlobal.Language.GetText("BulkUpload_Constants_CourseHadNoOpportunity", "Course {0} has no opportunity linked to it.");

            public static string External_Awarded_Organisation = AppGlobal.Language.GetText("BulkUpload_Constants_ExternalAwardedOrganisation", "You have selected qualification type 'External Awarded Qualification - Non-Accredited', but left the Awarding Body/Accreditation Body blank. Please enter the organisation name which awards/accredits this course. This is important as it gives the student the ability to make an informed decision about their preference of course based on the Awarding Body/Accreditation Body which awards the qualification.");
        }

        public static class BulkUpload_DataStandardMessage_Venue
        {
            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_Venue_Id = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidVenueId", "Invalid Venue Id");

            public static string InvalidVenueId = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidVenueId", "Invalid Venue Id value {0}, for column {1}, provider {2}, section{3}");

            public static string BlankVenueId = AppGlobal.Language.GetText("BulkUpload_Constants_BlankVenueId", "Blank Venue Id value for column {0}, provider {1}, section{2}");

            public static string InvalidVenueName = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidVenueName", "Invalid Venue Name : {0}, Actual Length : {1} , Expected Length : {2}  provider : {3}, section : {4}");

            public static string NonNumericVenueId = AppGlobal.Language.GetText("BulkUpload_Constants_NonNumericVenueId", "Invalid Venue Id - The Venue Id must be a number");
        }

        public static class BulkUpload_DataStandardMessage_Opportunity
        {
            /// <summary>
            /// 
            /// </summary>
            public static string Mandatory_A10 = AppGlobal.Language.GetText("BulkUpload_Constants_MandatoryA10Code", "Please select the appropriate Funding code(s) for this course opportunity. If this is not relevant, please use 'Not Applicable'");

            /// <summary>
            /// 
            /// </summary>
            public static string Mandatory_StudyMode = AppGlobal.Language.GetText("BulkUpload_Constants_MandatoryStudyMode", "Please select an appropriate study mode to continue. If the study mode is unspecified please select 'Not known'");

            /// <summary>
            /// 
            /// </summary>
            public static string StudyMode_FullTime = AppGlobal.Language.GetText("BulkUpload_Constants_OpportunityFullTime", "Full time");

            /// <summary>
            /// 
            /// </summary>
            public static string AttendancePattern_Not_known = AppGlobal.Language.GetText("BulkUpload_Constants_AttendancePatternNotKnown", "Not known");

            /// <summary>
            /// 
            /// </summary>
            public static string AttendancePattern_Customised = AppGlobal.Language.GetText("BulkUpload_Constants_AttendancePatternCustomised", "Customised");

            /// <summary>
            /// 
            /// </summary>
            public static string AttendancePattern_Daytime_working_hours = AppGlobal.Language.GetText("BulkUpload_Constants_AttendancePatternsDaytimeWorking", "Daytime/working hours");

            /// <summary>
            /// 
            /// </summary>
            public static string AttendancePattern_Day_Block_release = AppGlobal.Language.GetText("BulkUpload_Constants_DatBlockRelease", "Day/Block release");

            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_Study_Mode = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidStudyMode", "If study mode is supplied as 'Full time' then attendance pattern should be: Not known, Customised, Daytime / working hours, Day/Block release");

            /// <summary>
            /// 
            /// </summary>
            public static string Mandatory_Attendance_Mode = AppGlobal.Language.GetText("BulkUpload_Constants_MandatoryAttendanceMode", "Please select an appropriate attendance mode to continue. If the attendance mode is unspecified please select 'Not known'");

            /// <summary>
            /// 
            /// </summary>
            public static string Mandatory_Attendance_Pattern = AppGlobal.Language.GetText("BulkUpload_Constants_MandatoryAttendancePattern", "Please select an appropriate attendance pattern to continue. If the attendance pattern is unspecified please select 'Not known'");

            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_Attendance_Pattern = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidAttendancePattern", "If attendance mode is supplied as 'Distance without attendance' then the attendance pattern should be: Not known, Customised");

            /// <summary>
            /// 
            /// </summary>
            public static string AttendanceMode_DistanceWithout_Attendance = AppGlobal.Language.GetText("BulkUpload_Constants_DistanceWithOutAttendance", "AM6");

            /// <summary>
            /// 
            /// </summary>
            public static string AttendanceMode_Online_Without_Attendance = AppGlobal.Language.GetText("BulkUpload_Constants_DistancePatternOnline", "AM7");

            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_Attendance_Pattern_Online = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidAttendancePatternOnline", "If attendance mode is supplied as 'Online (without attendance)' then the attendance pattern should be: Not known, Customised");

            /// <summary>
            /// 
            /// </summary>
            public static string Duration_Should_Be_Number = AppGlobal.Language.GetText("BulkUpload_Constants_DurationShouldBeANumber", "Please enter the number of hours/days/weeks/years etc in numeric form");

            /// <summary>
            /// 
            /// </summary>
            public static string Number_Should_be_above_zero = AppGlobal.Language.GetText("BulkUpload_Constants_NumberShouldBeAboveZero", "Please enter a value that is 1 or above.");

            /// <summary>
            /// 
            /// </summary>
            public static string Duration_Unit_Mandatory = AppGlobal.Language.GetText("BulkUpload_Constants_DurationUnitMandatory", "Please provide the appropriate duration unit (hours, days, weeks, months, terms, semesters or years) to go with the 'duration' you have supplied.");

            /// <summary>
            /// 
            /// </summary>
            public static string StartDate_EndDate_Description_Mandatory = AppGlobal.Language.GetText("BulkUpload_Constants_StartEndDateDescriptionMandatory", "There has been an error recording your course start details, please complete either a 'start date' or 'start month' and/or 'start date description' to continue.");

            /// <summary>
            /// 
            /// </summary>
            public static string Date_Already_passed = AppGlobal.Language.GetText("BulkUpload_Constants_DateAlreadyPassed", "The start dates entered have already passed. For courses that last less than 12 weeks start dates should always be in the future. For courses with longer durations, the system allows past start dates to be entered providing that the course didn't start more than 4 weeks ago.");

            /// <summary>
            /// 
            /// </summary>
            public static string Start_date_greater_than_end_date = AppGlobal.Language.GetText("BulkUpload_Constants_StartDateGreaterThanEnd", "The 'start date' you have entered for this course opportunity is later than the 'end date'. Please re-check the dates you have selected for these two fields.");

            /// <summary>
            /// 
            /// </summary>
            public static string Start_date_should_be_Provided_if_end_date_Provided = AppGlobal.Language.GetText("BulkUpload_Constants_StartDateprovidedIfEndDateProvided", "You have entered an end date for this course, please supply a start date to continue");

            /// <summary>
            /// 
            /// </summary>
            public static string StartDate_EndDate_Validation = AppGlobal.Language.GetText("BulkUpload_Constants_StartDateEndDateValidation", "Please enter an 'exact duration' and/or a 'duration description' and/or 'start and end dates' to continue.");

            /// <summary>
            /// 
            /// </summary>
            public static string Mandatory_end_date = AppGlobal.Language.GetText("BulkUpload_Constants_MandatoryEndDate", "Mandatory (if exact duration not completed).");

            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_Numeric_Cost = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidNumericCost", "Please enter the numeric cost of the course opportunity");

            /// <summary>
            /// 
            /// </summary>
            public static string ApplyFrom_greate_than_endDate = AppGlobal.Language.GetText("BulkUpload_Constants_ApplyFromDateGreaterThanEndDate", "The Apply Until date and Apply From date must be on or before the End date");

            /// <summary>
            /// 
            /// </summary>
            public static string BothSearchable_Message = AppGlobal.Language.GetText("BulkUpload_Constants_BothSearchableMessage", "Please enter 'Y' or 'N'");

            /// <summary>
            /// 
            /// </summary>
            public static string Enter_either_price_or_description = AppGlobal.Language.GetText("BulkUpload_Constants_EnterEitherPriceOrDescription", "Please enter either a 'price', 'price description' or both to continue.");

            /// <summary>
            /// 
            /// </summary>
            public static string Not_Applicable = AppGlobal.Language.GetText("BulkUpload_Constants_NotApplicableText", "NA");

            /// <summary>
            /// 
            /// </summary>
            public static string Cannot_add_spply_from_in_case_of_multiple_start_dates = AppGlobal.Language.GetText("BulkUpload_Constants_CannotAddSupplyFromInCaseofMultipleDate", "You have entered multiple 'start dates' so cannot enter an 'apply from' date. If you would like to attach 'apply from' dates to these course opportunities, please enter separate course opportunities with one 'start date' and 'apply from' date.");

            public static string BlankOpportunityId = AppGlobal.Language.GetText("BulkUpload_Constants_BlankOpportunityId", "Blank Opportunity Id for provider {0} section {1}");

            public static string CourseIdExistsInOpportunityNotInCourse = AppGlobal.Language.GetText("BulkUpload_Constants_CourseIdExistsInOpportunityNotInCourse", "Course Id {0} exists in Opportunity but not found in Courses.");

            public static string VenuedExistsInOpportunityNotInVenues = AppGlobal.Language.GetText("BulkUpload_Constants_VenuedExistsInOpportunityNotInVenues", "Venue Id {0} exists in Opportunity but not found in Venues.");

            public static string InvalidDateForColumn = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidDateForColumn", "Invalid date value {0}, for column {1}, provider {2}, section{3}");

            public static string InvalidStudyModeForColumn = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidStudyModeForColumn", "Invalid Study Mode : {0}, section : {1}, provider : {2}.");

            public static string InvalidAttendanceModeForColumn = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidAttendanceModeForColumn", "Invalid Attendance Mode : {0}, section : {1}, provider : {2}.");

            public static string InvalidAttendancePatternForColumn = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidAttendancePatternForColumn", "Invalid Attendance Pattern : {0}, section : {1}, provider : {2}.");

            public static string InvalidDurationUnitNumber = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidDurationUnitNumber", "Invalid Duration unit number : {0}, section : {1}, provider : {2}.");

            public static string InvalidDurationUnit = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidDurationUnit", "Invalid Duration Unit  {0}, section : {1}, provider : {2}.");

            public static string InvalidA10Code = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidA10Code", "Invalid Funding Code  {0}, section : {1}, provider : {2}.");

            public static string InvalidNumericIdValue = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidNumericId", "Please enter valid Id value.");

            public static string ProviderDoesnotBelongsToOrganisation = AppGlobal.Language.GetText("BulkUpload_Constants_ProviderDoesnotBelongsToOrganisation", "The Organisation ID {0} in this column has to belong to a related organisation.");

        }

        public static class BulkUpload_Provider_DataStandards_Size
        {

            /// <summary>
            /// 
            /// </summary>
            public static int? ProviderName_MinLength = null;

            /// <summary>
            /// 
            /// </summary>
            public static int? String_Size_255 = 255;

            /// <summary>
            /// 
            /// </summary>
            public static int? String_Size_100 = 255;
        }

        public static class BulkUpload_Provider_DataStandards_Validations
        {
            public static string RowNumber = AppGlobal.Language.GetText("BulkUpload_Constants_RowId", "RowId");

            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_Html_Characters = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidHTMLCharacters", "Invalid characters found, please check the text entered. Also please be aware that HTML tags (e.g. <BR />) will be shown as plain text on the website, and should not be used.");

            /// <summary>
            /// 
            /// </summary>
            public static string Cannot_be_a_number = AppGlobal.Language.GetText("BulkUpload_Constants_CannotBeANumber", "This field cannot be a number");

            /// <summary>
            /// 
            /// </summary>
            public static string ProviderName_Already_Exists = AppGlobal.Language.GetText("BulkUpload_Constants_ProviderAlreadyExists", "This organisation name already exists on the database, please contact ***NOTIFICATION_CONTACT*** if you need any help");

            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_PostCode = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidPostCode", "We could not find the postcode you have entered. Postcodes must be entered with a space - e.g. SW1A 1AA. If you believe your postcode is correct and are still receiving this error, please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk");

            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_PostCode_withRoyalMail = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidPostCodeRoyalMail", "The postcode entered is invalid. If you are sure that you have entered a legitimate postcode but it is not being accepted by the system, it is likely that is not available on Royal Mail yet. Please contact ***NOTIFICATION_CONTACT*** for help");

            /// <summary>
            /// 
            /// </summary>
            public static string Invalid_Email = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidEmail", "Your email address is incorrectly formatted, please check and re enter");

            public static string ProviderNotFound = AppGlobal.Language.GetText("BulkUpload_Constants_ProviderNotFoundInDataBase", "ProviderId  not Found in Database");

            public static string InvalidColumnLength = AppGlobal.Language.GetText("BulkUpload_Constants_InvalidColumnLength", "Length for Column {0}, actual value {1}, expected length {2}, in Section {3}, Provider id : {4}");
        }

        public static class BulkUpload_ExceptionSummary_Header
        {
            public static string Notification_Type = AppGlobal.Language.GetText("BlCon_ExceptionSummaryHeader_NotificationType", "Error / Warning");

            public static string Line_Number = AppGlobal.Language.GetText("BlCon_ExceptionSummaryHeader_LineNumber", "Line Number");

            public static string Column_Name = AppGlobal.Language.GetText("BlCon_ExceptionSummaryHeader_ColumnName", "Column Name");

            public static string Column_Value = AppGlobal.Language.GetText("BlCon_ExceptionSummaryHeader_ColumnValue", "Actual Column Value");

            public static string Provider_Id = AppGlobal.Language.GetText("BlCon_ExceptionSummaryHeader_ProviderId", "Provider Id");

            public static string Details = AppGlobal.Language.GetText("BlCon_ExceptionSummaryHeader_Details", "UploadSummaryDetails");

            public static string Validatation_Stage = AppGlobal.Language.GetText("BlCon_ExceptionSummaryHeader_ValidationStage", "Stage");
        }

        public static string BulkUploadPending_Provider_Prefix = "BulkUploadPending_";

        public static List<int> ScopeInCodes = new List<int> { 10, 21, 22, 45, 46, 70, 80, 81 };

        public static List<int> scopeOutCodes = new List<int> { 82, 99 };

        #endregion


        #region Bulk Upload

        /// <summary>
        /// Indicates whether the bulk upload file holds course or apprenticeship data
        /// </summary>
        public enum FileContentType
        {
            CourseData = 1,
            ApprenticeshipData = 2,
        }

        #endregion


        #region CSV Export

        public static string C_Providers_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "C_PROVIDERS.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string C_Venues_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "C_VENUES.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string O_Courses_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "O_COURSES.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string O_Opportunity_A10_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "O_OPP_A10.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string O_Opportunity_StartDate
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "O_OPP_START_DATES.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string O_Opportunities_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "O_OPPORTUNITIES.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string S_Learning_AIM_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "S_LEARNING_AIMS.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string W_Course_Browse_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "W_COURSE_BROWSE.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string W_Provider_Search_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "W_PROVIDER_SEARCH.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string W_Search_Text_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "W_SEARCH_TEXT.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NightlyCsvZipFileName
        {
            get
            {
                return string.Concat(NightlyCsvFolderName, ".zip");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NightlyCsvZipFolderPath
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NightlyCsvZipFileNamewithAbsolutePath
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation, NightlyCsvZipFileName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NightlyCsvFolderName
        {
            get
            {
                return string.Format("HOTCOURSES_{0}", DateTime.Now.ToString(ConfigSettings.ShortDateFormatFileName));
            }
        }

        #endregion
    }
}