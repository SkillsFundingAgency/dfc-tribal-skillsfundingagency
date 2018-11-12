using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMS.NCS.Dashboard.Entities;
using IMS.NCS.Dashboard.Queries;

namespace IMS.NCS.Dashboard.BusinessServices
{
    /// <summary>
    /// Implementation of Dashboard service functions.
    /// </summary>
    public class DashboardService : IDashboardService
    {
        /// <summary>
        /// Gets a list of Jobs for the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>A DashboardJob of data.</returns>
        DashboardJob IDashboardService.GetJobList(JobSearchCriteria searchCriteria)
        {
            IDashboardQuery query = new DashboardQuery();
            return query.GetJobList(searchCriteria);
        }


        /// <summary>
        /// Gets the Job and Step details for a Job.
        /// </summary>
        /// <param name="jobId">The id of the Job.</param>
        /// <returns>A Job and list of Steps for that Job.</returns>
        DashboardDetailJob IDashboardService.GetJobDetails(int jobId)
        {
            IDashboardQuery query = new DashboardQuery();
            return query.GetJobDetails(jobId);
        }
    }
}
