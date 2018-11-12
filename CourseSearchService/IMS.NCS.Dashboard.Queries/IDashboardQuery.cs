using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IMS.NCS.Dashboard.Entities;

namespace IMS.NCS.Dashboard.Queries
{
    /// <summary>
    /// Interface of Dashboard query functions.
    /// </summary>
    public interface IDashboardQuery
    {
        /// <summary>
        /// Gets a list of Jobs for the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>A DashboardJob of data.</returns>
        DashboardJob GetJobList(JobSearchCriteria searchCriteria);

        /// <summary>
        /// Gets the Job and Step details for a Job.
        /// </summary>
        /// <param name="jobId">The id of the Job.</param>
        /// <returns>A Job and list of Steps for that Job.</returns>
        DashboardDetailJob GetJobDetails(int jobId);
    }
}
