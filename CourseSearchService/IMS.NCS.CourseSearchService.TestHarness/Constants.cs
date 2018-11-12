using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.NCS.CourseSearchService.TestHarness
{
    /// <summary>
    /// Provides a central repositry for constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// QueryString keys
        /// </summary>
        public struct QueryStrings
        {
            /// <summary>
            /// The SortBy QueryString text.
            /// </summary>
            public const string SortBy = "SortBy";

            /// <summary>
            /// The RecordsPerPage QueryString text.
            /// </summary>
            public const string RecordsPerPage = "RecordsPerPage";

            /// <summary>
            /// The GetPage QueryString text.
            /// </summary>
            public const string GetPage = "getPage";

            /// <summary>
            /// The CourseChk QueryString text.
            /// </summary>
            public const string CourseChk = "CourseChk";

            /// <summary>
            /// The CourseId QueryString text.
            /// </summary>
            public const string CourseId = "CourseId";

            /// <summary>
            /// The JobId QueryString text.
            /// </summary>
            public const string JobId = "JobId";
        }


        /// <summary>
        /// List of string formats for parameterised strings
        /// </summary>
        public struct StringFormats
        {
            /// <summary>
            /// The string format for displaying page and results.
            /// </summary>
            public const string ResultsOverviewStringFormat = "Page: {0} of {1} - Total records: {2}";

            /// <summary>
            /// The string format for creating query strings
            /// </summary>
            public const string QueryStringStringFormat = "{0}={1}";

            /// <summary>
            /// The string format for creating navigatePage onClick event handlers.
            /// </summary>
            public const string JavascriptNavigatePage = "javascript:navigatePage({0});";

            /// <summary>
            /// The string format for displaying the number of courses on the Course Details page.
            /// </summary>
            public const string CourseDetailNoOfCourses = "COURSE DETAILS FOR {0} COURSE(S)";
        }


        public struct ConfigurationKeys
        {
            /// <summary>
            /// The name of the configuration setting for the age of the files to delete.
            /// </summary>
            public const string ZipFileDeletionAge = "ZipFileDeletionAge";

            /// <summary>
            /// The name of the configuration setting for the location of the Data Import ZIP file share.
            /// </summary>
            public const string FILESRVLocation = "FILESRVLocation";

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

            /// <summary>
            /// The configuration key for the Environment display name.
            /// </summary>
            public const string Environment = "EnvironmentDisplayName";

            public const String Url = "Url";
        }
        

        /// <summary>
        /// Default page number when we first enter the page.
        /// </summary>
        public const string InitialPageNumber = "1";
    }
}