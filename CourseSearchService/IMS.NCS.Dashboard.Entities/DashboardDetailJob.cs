using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.Dashboard.Entities
{
    /// <summary>
    /// Repesents the entity for the Job details
    /// </summary>
    public class DashboardDetailJob
    {
        /// <summary>
        /// The Job details.
        /// </summary>
        public Job DetailJob { get; set; }


        /// <summary>
        /// The list of Job Steps in this Job.
        /// </summary>
        public List<JobStep> Steps { get; set; }
    }
}
