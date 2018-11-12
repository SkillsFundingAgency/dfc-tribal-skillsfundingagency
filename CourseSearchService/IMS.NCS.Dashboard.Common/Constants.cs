using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.Dashboard.Common
{
    /// <summary>
    /// Set of constants for use by the Data Import Dashboard.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Database constants
        /// </summary>
        public struct Database
        {
            /// <summary>
            /// The key to the NCs ImportControl database connection string.
            /// </summary>
            public const string NCSImportControlDatabase = "ImportControl Database";
            
            /// <summary>
            /// Identifies the stored procedure for searching Jobs.
            /// </summary>
            public const string UspGetJobsByCriteria = "NCSImport.UspGetJobsByCriteria";

            /// <summary>
            /// The stored procedure to get a single Job details.
            /// </summary>
            public const string UspGetJobByJobId = "Core.UspGetJobByJobId";
        }


        /// <summary>
        /// Column Names for the return from the UspGetJobByJobId stored procedure
        /// </summary>
        public struct UspGetJobByJobIdColumns
        {
            public const string CurrentPageNo = "CurrentPageNo";
            public const string TotalPages = "TotalPages";
            public const string TotalRecords = "TotalRecords";
            
            public const string JobId = "JobId";
			public const string StepName = "StepName";
			public const string Status = "Status";
			public const string StepProcessStart = "StepProcessStart";
			public const string StepProcessEnd = "StepProcessEnd";
			public const string StepElapsedTime = "StepElapsedTime";
        }

        /// <summary>
        /// Output parameter names from the UspGetJobByJobId stored procedure
        /// </summary>
        public struct UspGetJobByJobIdParameters
        {
            public const string JobId = "@jobId";
            public const string JobName = "@jobName";
            public const string JobProcessStart = "@jobProcessStart";
            public const string JobProcessEnd = "@jobProcessEnd";
            public const string JobElapsedTime = "@jobElapsedTime";
        }


        /// <summary>
        /// Column Names for the return from the UspSelectJobsByCriteria stored procedure
        /// </summary>
        public struct UspGetJobsByCriteriaColumns
        {
            public const string JobId = "JobId";
            public const string JobName = "JobName";
            public const string ProcessStart = "ProcessStart";
            public const string ProcessEnd = "ProcessEnd";
            public const string ElapsedTime = "ElapsedTime";
            public const string CurrentStep = "CurrentStep";
            public const string Status = "Status";
        }


        /// <summary>
        /// Output parameter names from the UspGetJobsByCriteria stored procedure
        /// </summary>
        public struct UspGetJobsByCriteriaParameters
        {
            public const string StartDate = "@startDate";
            public const string EndDate = "@endDate";
            public const string InProgressJobs = "@inProgressJobs";
            public const string CompletedJobs = "@completedJobs";
            public const string FailedJobs = "@failedJobs";
            public const string SortBy = "@sortBy";
            public const string NoOfRecords = "@noOfRecords";
            public const string PageNo = "@pageNo";
            public const string TotalRows = "@totalRows";
        }
    }
}
