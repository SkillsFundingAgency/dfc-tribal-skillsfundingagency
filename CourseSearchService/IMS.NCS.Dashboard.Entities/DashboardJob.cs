using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.Dashboard.Entities
{
    /// <summary>
    /// Represents the data for the Dashboard Job results.
    /// </summary>
    public class DashboardJob
    {
        /// <summary>
        /// The total number of pages for the search criteria.
        /// </summary>
        public int TotalPages{ get; set; }

        /// <summary>
        /// The current page no.
        /// </summary>
        public int CurrentPageNo { get; set; }

        /// <summary>
        /// The total number of records for the search criteria.
        /// </summary>
        public int TotalRecords { get; set; }
        
        /// <summary>
        /// The current page worth of jobs.
        /// </summary>
        public List<Job> Jobs { get; set; }
    }
}
