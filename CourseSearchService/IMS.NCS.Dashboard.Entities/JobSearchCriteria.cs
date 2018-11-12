using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.Dashboard.Entities
{
    /// <summary>
    /// Search criteria for the Data Import Dashboard
    /// </summary>
    public class JobSearchCriteria
    {
        /// <summary>
        /// The Job ProcessStart date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The Job ProcessEnd date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// True to get InProgress jobs, else false.
        /// </summary>
        public bool InProgressJobs { get; set; }

        /// <summary>
        /// True to get Completed jobs, else false.
        /// </summary>
        public bool CompletedJobs { get; set; }

        /// <summary>
        /// True to get Failed jobs, else false.
        /// </summary>
        public bool FailedJobs { get; set; }

        /// <summary>
        /// The name of the column to sort by.
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// No of records per page.
        /// </summary>
        public int RecordsPerPage { get; set; }

        /// <summary>
        /// The page number of the data to retrieve.
        /// </summary>
        public int NextPage { get; set; }
    }
}
