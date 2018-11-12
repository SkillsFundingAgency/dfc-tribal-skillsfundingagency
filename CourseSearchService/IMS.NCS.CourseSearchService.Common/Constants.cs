using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.CourseSearchService.Common
{
    /// <summary>
    /// Common class containing constants.
    /// Including,
    ///     - database name
    ///     - stored procedure names
    ///     - function names
    ///     - column flags
    ///     - stored procedure parameter names
    ///     - stored procedure result set column names
    /// </summary>
    public static class Constants
    {
        // Database Connections
        public const string DATABASE_CONNECTION = "HotCoursesDb";

        // Stored procedures
        public const string USER_DETAILS_SP = "NDLPP_UTILS_PKG.AUTHENTICATE_USER_FN";
        public const string FETCH_CATEGORIES_SP = "NDLPP_SEARCH_PKG.SUBJECT_BROWSE_PRC";

        public const string FETCH_COURSE_DETAILS_SP = "NDLPP_SEARCH_WRAPPER_PKG.COURSE_DETAILS_PRC";
        public const string FETCH_PROVIDER_DETAILS_SP = "NDLPP_SEARCH_PKG.PROVIDER_DETAILS_PRC";
        public const string PROVIDER_SEARCH_SP = "NDLPP_SEARCH_PKG.PROVIDER_SEARCH_PRC";
        public const string COURSE_SEARCH_SP = "NDLPP_COURSE_SEARCH_PKG.COURSE_SEARCH_PRC";
        public const string UPDATE_TIMING_SP = "NDLPP_UTILS_PKG.UPDATE_TIMING_PRC";

        // Functions
        public const string FETCH_DB_NAME_FN = "NDLPP_UTILS_PKG.GET_DB_NAME_FN";
        public const string FETCH_ALL_SYS_VARS_FN = "NDLPP_UTILS_PKG.GET_SYS_VARS";

        // Column flags
        public const string SEARCH_TIME_COLUMN_FLAG = "T";

        // UDT Type names
        public const string NUMBER_ARRAY = "TB_NUM";
        public const string VAR_ARRAY = "TB_VAR";

        /// <summary>
        /// Provider Search SP Parameter names.
        /// </summary>
        public struct ProviderSearchParameters
        {
            public const string PROVIDER_KEYWORD = "PV_PROVIDER_KEYWORD";
            public const string EMAIL = "PV_EMAIL";
            public const string ERROR_MSG = "PV_ERROR_MESSAGE";
            public const string RESULTS_CURSOR = "PC_RESULTS_CURSOR";
        }

        /// <summary>
        /// Provider Search SP Column names.
        /// </summary>
        public struct ProviderSearchColumns
        {
            public const string PROVIDER_ID = "PROVIDER_ID";
            public const string PROVIDER_NAME = "PROVIDER_NAME";
            public const string ADDRESS_LINE1 = "ADDRESSLINE1";
            public const string ADDRESS_LINE2 = "ADDRESSLINE2";
            public const string TOWN = "TOWN";
            public const string COUNTY = "COUNTY";
            public const string POSTCODE = "POSTCODE";
            public const string PHONE = "PHONE";
            public const string EMAIL = "EMAIL";
            public const string FAX = "FAX";
            public const string WEBSITE = "WEBSITE";
            public const string UKPRN = "UKPRN";
            public const string UPIN = "UPIN";
            public const string TFPLUSLOANS = "TFPLUSLOANS";
        }

        /// <summary>
        /// Provider Details SP Parameter names.
        /// </summary>
        public struct ProviderDetailsParameters
        {
            public const string PROVIDER_ID = "PV_PROVIDER_ID";
            public const string USERNAME = "PV_USERNAME";
            public const string ERROR_MSG = "PV_ERROR_MESSAGE";
            public const string PROVIDER_DETAILS = "PROVIDERDETAILS";
        }

        /// <summary>
        /// Provider Details SP Column names.
        /// </summary>
        public struct ProviderDetailsColumns
        {
            public const string PROVIDER_ID = "PROVIDERID";
            public const string PROVIDER_NAME = "PROVIDERNAME";
            public const string ADDRESS_LINE1 = "ADDRESSLINE1";
            public const string ADDRESS_LINE2 = "ADDRESSLINE2";
            public const string TOWN = "TOWN";
            public const string COUNTY = "COUNTY";
            public const string POSTCODE = "POSTCODE";
            public const string PHONE = "PHONE";
            public const string EMAIL = "EMAIL";
            public const string FAX = "FAX";
            public const string WEBSITE = "WEBSITE";
            public const string UKPRN = "UKPRN";
            public const string UPIN = "UPIN";
            public const string TFPLUSLOANS = "TFPLUSLOANS";

        }

        /// <summary>
        /// Fetch Categories SP Parameter names.
        /// </summary>
        public struct FetchCategoriesParameters
        {
            public const string USERNAME = "PV_USERNAME";
            public const string ERROR_MSG = "PV_ERROR_MESSAGE";
            public const string CATEGORIES = "CATEGORIES";
        }

        /// <summary>
        /// Fetch Categories SP Column names.
        /// </summary>
        public struct FetchCategoriesColumns
        {
            public const string CATEGORY_CODE = "CATEGORY_CODE";
            public const string DESCRIPTION = "DESCRIPTION";
            public const string PARENT_CATEGORY_CODE = "PARENT_CATEGORY_CODE";
            public const string COURSE_COUNT = "COURSE_COUNT";
            public const string LEVEL = "LEVEL";
            public const string SEARCHABLE_FLAG = "SEARCHABLE_FLAG";
        }

        /// <summary>
        /// Fetch Course Details SP Parameter names.
        /// </summary>
        public struct CourseDetailsParameters
        {
            public const string COURSE_IDS = "PV_WRAPPER_COURSE_IDS";
            public const string USERNAME = "PV_WRAPPER_USERNAME";
            public const string ERROR_MSG = "PV_WRAPPER_ERROR_MESSAGE";
            public const string COURSE_DETAILS = "PC_WRAPPER_RESULTS_CURSOR_COU";
            public const string PROVIDER_DETAILS = "PC_WRAPPER_RESULTS_CURSOR_PRO";
            public const string OPPORTUNITY_DETAILS = "PC_WRAPPER_RESULTS_CURSOR_OPP";
            public const string VENUE_DETAILS = "PC_WRAPPER_RESULTS_CURSOR_VEN";
        }

        /// <summary>
        /// Fetch Course Details SP Column names.
        /// </summary>
        public struct CourseDetailsColumns
        {
            /// <summary>
            /// Course Cursor Column names.
            /// </summary>
            public struct CourseCursor
            {
                public const string COURSE_ID = "COURSEID";
                public const string COURSE_TITLE = "COURSETITLE";
                public const string QUALIFICATION_TYPE = "QUALTYPE";
                public const string QUALIFICATION_LEVEL = "QUALLEVEL";
                public const string LDCSCODE_1 = "LDCSCODE1";
                public const string LDCSCODE_2 = "LDCSCODE2";
                public const string LDCSCODE_3 = "LDCSCODE3";
                public const string LDCSCODE_4 = "LDCSCODE4";
                public const string LDCSCODE_5 = "LDCSCODE5";
                public const string LDCSDESC_1 = "LDCSDESC1";
                public const string LDCSDESC_2 = "LDCSDESC2";
                public const string LDCSDESC_3 = "LDCSDESC3";
                public const string LDCSDESC_4 = "LDCSDESC4";
                public const string LDCSDESC_5 = "LDCSDESC5";
                public const string NUM_OF_OPPORTUNITIES = "NOOFOPPS";
                public const string COURSE_SUMMARY = "COURSESUMMARY";
                public const string AWARDING_BODY = "AWARDINGBODY";
                public const string ASSESSMENT_METHOD = "ASSESSMENTMETHOD";
                public const string BOOKING_URL = "BOOKINGURL";
                public const string ACCREDITATION_END_DATE = "ACCREDITATIONENDDATE";
                public const string ACCREDITATION_START_DATE = "ACCREDITATIONSTARTDATE";
                public const string CERTIFICATION_END_DATE = "CERTIFICATIONENDDATE";
                public const string CREDIT_VALUE = "CREDITVALUE";
                public const string DATA_TYPE = "DATATYPE";
                public const string ER_APP_STATUS = "ERAPPSTATUS";
                public const string ER_TTG_STATUS = "ERTTGSTATUS";
                public const string ENTRY_REQUIREMENTS = "ENTRYREQUIREMENTS";
                public const string EQUIPMENT_REQUIRED = "EQUIPMENTREQUIRED";
                public const string INDEPENDENT_LIVING_SKILLS = "INDEPENDENTLIVINGSKILLS";
                public const string LAD_ID = "LADID";
                public const string LEVEL_2_ENTITLEMENT_CATEGORY_DESCRIPTION = "LEVEL2ENTITLEMENTCATEGORYDESC";
                public const string LEVEL_3_ENTITLEMENT_CATEGORY_DESCRIPTION = "LEVEL3ENTITLEMENTCATEGORYDESC";
                public const string OTHER_FUNDING_NON_FUNDED_STATUS = "OTHERFUNDINGNONFUNDEDSTATUS";
                public const string QCA_GUIDED_LEARNING_HOURS = "QCAGUIDEDLEARNINGHOURS";
                public const string QUALIFICATION_REFERENCE_AUTHORITY = "QUALREFAUTHORITY";
                public const string QUALIFICATION_REFERENCE = "QUALIFICATIONREFERENCE";
                public const string QUALIFICATION_TITLE = "QUALIFICATIONTITLE";
                public const string SECTOR_LEAD_BODY_DESCRIPTION = "SECTORLEADBODYDESC";
                public const string SKILLS_FOR_LIFE_FLAG = "SKILLSFORLIFEFLAG";
                public const string SKILLS_FOR_LIFE_TYPE_DESCRIPTION = "SKILLSFORLIFETYPEDESC";
                public const string TARIFF_REQUIRED = "TARIFFREQUIRED";
                public const string URL = "URL";
                public const string ADULT_LR_STATUS = "ADULTLRSTATUS";
            }

            /// <summary>
            /// Provider Cursor Column names.
            /// </summary>
            public struct ProviderCursor
            {
                public const string COURSE_ID = "COURSEID";
                public const string PROVIDER_ID = "PROVIDERID";
                public const string PROVIDER_NAME = "PROVIDERNAME";
                public const string ADDRESS_LINE1 = "ADDRESSLINE1";
                public const string ADDRESS_LINE2 = "ADDRESSLINE2";
                public const string TOWN = "TOWN";
                public const string COUNTY = "COUNTY";
                public const string POSTCODE = "POSTCODE";
                public const string PHONE = "PHONE";
                public const string EMAIL = "EMAIL";
                public const string FAX = "FAX";
                public const string WEBSITE = "WEBSITE";
                public const string UKPRN = "UKPRN";
                public const string UPIN = "UPIN";
                public const string TFPLUSLOANS = "TFPLUSLOANS";
            }

            /// <summary>
            /// Opportunity Cursor Column names.
            /// </summary>
            public struct OpportunityCursor
            {
                public const string COURSE_ID = "COURSEID";
                public const string ATTENDANCE_MODE = "ATTENDANCEMODE";
                public const string ATTENDANCE_PATTERN = "ATTENDANCEPATTERN";
                public const string DURATION_DESCRIPTION = "DURATIONDESCRIPTION";
                public const string DURATION_VALUE = "DURATIONVALUE";
                public const string DURATION_UNIT = "DURATIONUNIT";
                public const string PRICE = "PRICE";
                public const string PRICE_DESCRIPTION = "PRICEDESC";
                public const string START_DATE = "STARTDATE";
                public const string START_DATE_DESCRIPTION = "STARTDATEDESC";
                public const string STUDY_MODE = "STUDY_MODE";
                public const string TIMETABLE = "TIMETABLE";
                public const string REGION_NAME = "REGIONNAME";
                public const string VENUE_ID = "VENUEID";
                public const string APPLICATION_ACCEPTED_THROUGHOUT_YEAR = "APPLYTHROUGHOUTYEAR";
                public const string APPLY_FROM_DATE = "APPLYFROMDATE";
                public const string APPLY_TO = "APPLYTO";
                public const string APPLY_UNTIL_DATE = "APPLYUNTILDATE";
                public const string APPLY_UNTIL_DESCRIPTION = "APPLYUNTILDESC";
                public const string END_DATE = "ENDDATE";
                public const string ENQUIRE_TO = "ENQUIRETO";
                public const string LANGUAGE_OF_ASSESSMENT = "LANGUAGEOFASSESSMENT";
                public const string LANGUAGE_OF_INSTRUCTION = "LANGUAGEOFINSTRUCTION";
                public const string PLACES_AVAILABLE = "PLACESAVAILABLE";
                public const string PROVIDER_OPPORTUNITY_ID = "PROVIDEROPPORTUNITYID";
                public const string URL = "URL";
                public const string A10 = "A10";
                public const string OPPORTUNITY_ID = "OPPORTUNITYID";
            }

            /// <summary>
            /// Venue Cursor Column names.
            /// </summary>
            public struct VenueCursor
            {
                public const string COURSE_ID = "COURSEID";
                public const string VENUE_ID = "VENUEID";
                public const string VENUE_NAME = "VENUENAME";
                public const string ADDRESS_LINE1 = "ADDRESS1";
                public const string ADDRESS_LINE2 = "ADDRESS2";
                public const string TOWN = "TOWN";
                public const string COUNTY = "COUNTY";
                public const string POSTCODE = "POSTCODE";
                public const string PHONE = "CONTACTPHONE";
                public const string EMAIL = "EMAIL";
                public const string FAX = "FAX";
                public const string WEBSITE = "WEBSITE";
                public const string FACILITIES = "FACILITIES";
                public const string LATITUDE = "LATITUDE";
                public const string LONGITUDE = "LONGITUDE";
            }
        }

        /// <summary>
        /// Course Search SP Parameter names.
        /// </summary>
        public struct CourseListParameters
        {
            public const string COURSE_KEYWORD = "PV_COURSE_KEYWORD";
            public const string LOCATION_TEXT = "PV_LOCATION_TEXT";
            public const string MAX_DISTANCE = "PN_MAX_DISTANCE_MILES";
            public const string PROVIDER_ID = "PN_NDLPP_PROVIDER_ID";
            public const string PROVIDER_KEYWORD = "PV_PROVIDER_KEYWORD";
            public const string QUALIFICATION_TYPES = "PV_QUAL_TYPES";
            public const string QUALIFICATION_LEVELS = "PV_QUAL_LEVELS";
            public const string STUDY_MODES = "PV_STUDY_MODE";
            public const string ATTENDANCE_MODES = "PV_ATTENDANCE_MODE";
            public const string ATTENDANCE_PATTERNS = "PV_ATTENDANCE_PATTERN";
            public const string EARLIEST_START_DATE = "PV_EARLIEST_START_DATE";
            public const string FLEXIBLE_START_FLAG = "PV_INCL_FLEX_START_DATE";
            public const string APPLICATION_CLOSED_FLAG = "PV_INCL_CLOSED_APPL";
            public const string LDCS_CATEGORY_CODE = "PV_LDCS_CATEGORY_ID";
            public const string TTG_FLAG = "PV_TTG_ONLY";
            public const string TQS_FLAG = "PV_TQS_ONLY";
            public const string IES_FLAG = "PV_IES_ONLY";
            public const string A10_CODES = "PV_A10_LIST";
            public const string INDEPENDENT_LIVING_SKILLS_FLAG = "PV_INDEP_LIVING_SKILLS_FLAG";
            public const string SKILLS_FOR_LIFE_FLAG = "PV_SKILLS_FOR_LIFE_FLAG";
            public const string ER_APP_STATUS = "PV_ER_APP_STS";
            public const string ER_TTG_STATUS = "PV_ER_TTG_STS";
            public const string ADULT_LR_STATUS = "PV_ADULT_LR_STS";
            public const string OTHER_FUNDING_STATUS = "PV_OTHER_FUNDING_STS";
            public const string SORT_BY = "PV_SORT_ORDER";
            public const string PAGE_NUMBER = "PN_PAGE_NUMBER";
            public const string RECORDS_PER_PAGE = "PN_RESULTS_PER_PAGE";
            public const string USERNAME = "PV_EMAIL";
            public const string ERROR_MSG = "PV_ERROR_MESSAGE";
            public const string TOTAL_RESULTS_COUNT = "PN_TOTAL_RESULTS";
            public const string RESULTS_CURSOR = "PC_RESULTS_CURSOR";
            public const string LDCS_CURSOR = "PC_LDCS_CURSOR";
            public const string SEARCH_ID = "PN_SEARCH_ID";
        }

        /// <summary>
        /// Course Search SP Column names.
        /// </summary>
        public struct CourseListColumns
        {
            /// <summary>
            /// Course Cursor Column names.
            /// </summary>
            public struct CourseCursor
            {
                public const string PROVIDER_NAME = "PROVIDER_NAME";
                public const string COURSE_ID = "COURSEID";
                public const string COURSE_TITLE = "COURSETITLE";
                public const string QUALIFICATION_TYPE = "QUALTYPE";
                public const string QUALIFICATION_LEVEL = "QUALLEVEL";
                public const string LDCSCODE_1 = "LDCSCODE1";
                public const string LDCSCODE_2 = "LDCSCODE2";
                public const string LDCSCODE_3 = "LDCSCODE3";
                public const string LDCSCODE_4 = "LDCSCODE4";
                public const string LDCSCODE_5 = "LDCSCODE5";
                public const string LDCSDESC_1 = "LDCSDESC1";
                public const string LDCSDESC_2 = "LDCSDESC2";
                public const string LDCSDESC_3 = "LDCSDESC3";
                public const string LDCSDESC_4 = "LDCSDESC4";
                public const string LDCSDESC_5 = "LDCSDESC5";
                public const string NUM_OF_OPPORTUNITIES = "NOOFOPPS";
                public const string COURSE_SUMMARY = "COURSESUMMARY";
                public const string OPPORTUNITY_ID = "OPPORTUNITYID";
                public const string STUDY_MODE = "STUDY_MODE";
                public const string ATTENDANCE_MODE = "ATTENDANCEMODE";
                public const string ATTENDANCE_PATTERN = "ATTENDANCEPATTERN";
                public const string START_DATE = "STARTDATE";
                public const string START_DATE_DESCRIPTION = "STARTDATEDESCRIPTION";
                public const string END_DATE = "ENDDATE";
                public const string REGION_NAME = "REGIONNAME";
                public const string VENUE_NAME = "VENUENAME";
                public const string ADDRESS_LINE1 = "ADDRESS_LINE_1";
                public const string ADDRESS_LINE2 = "ADDRESS_LINE_2";
                public const string TOWN = "TOWN";
                public const string COUNTY = "COUNTY";
                public const string POSTCODE = "POSTCODE";
                public const string LATITUDE = "LATITUDE";
                public const string LONGITUDE = "LONGITUDE";
                public const string DISTANCE = "DISTANCE";
                public const string DURATION_UNIT = "DURATIONUNIT";
                public const string DURATION_VALUE = "DURATIONVALUE";
                public const string DURATION_DESCRIPTION = "DURATIONDESCRIPTION";
                public const string TFPLUSLOANS = "TFPLUSLOANS";
            }

            /// <summary>
            /// LDCS Cursor Column names.
            /// </summary>
            public struct LdcsCursor
            {
                public const string LDCS_DESCRIPTION = "LDCSDESC";
                public const string LDCS_COUNTS = "COUNTS";
                public const string LDCS_CODE = "LDCSCODE";
            }
        }
        
        /// <summary>
        /// Update Timing SP Parameter names.
        /// </summary>
        public struct UpdateTimingParameters
        {
            public const string COLUMN_FLAG = "PV_COLUMN_FLAG";
            public const string SEARCH_ID = "PN_SEARCH_ID";
        }

        /// <summary>
        /// uspGetGeoLocation stored procedure parameters
        /// </summary>
        public struct GetGeoLocationParameters
        {
            public const string POSTCODE = "postcode";
        }


        /// <summary>
        /// uspGetGeoLocation stored procedure results columns
        /// </summary>
        public struct GetGeoLocationColumns
        {
            public const string POSTCODE = "Postcode";
            public const string LATITUDE = "Latitude";
            public const string LONGITUDE = "LONGITUDE";
        }


        /// <summary>
        /// Constants for accessing configuration keys
        /// </summary>
        public struct ConfigurationKeys
        {
            /// <summary>
            /// The configuration key for the Service Account name.
            /// </summary>
            public const string ServiceAccount = "ServiceAccount";

            /// <summary>
            /// The configuration key for the Service Account domain.
            /// </summary>
            public const string ServiceAccountDomain = "ServiceAccountDomain";

            /// <summary>
            /// The configuration key for the Service Account password.
            /// </summary>
            public const string ServiceAccountPassword = "ServiceAccountPassword";
        }
    }
}
