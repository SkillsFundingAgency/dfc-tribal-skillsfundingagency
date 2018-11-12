using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IMS.NCS.Dashboard.Entities;
using IMS.NCS.Dashboard.Gateways;

namespace IMS.NCS.Dashboard.Queries
{
    /// <summary>
    /// Implementation of Dashboard query functions.
    /// </summary>
    public class DashboardQuery : IDashboardQuery
    {
        /// <summary>
        /// Gets a list of Jobs for the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>A DashboardJob of data.</returns>
        DashboardJob IDashboardQuery.GetJobList(JobSearchCriteria searchCriteria)
        {
            IDashboardGateway gateway = new DashboardGateway();
            return gateway.GetJobList(searchCriteria);
        }


        /// <summary>
        /// Gets the Job and Step details for a Job.
        /// </summary>
        /// <param name="jobId">The id of the Job.</param>
        /// <returns>A Job and list of Steps for that Job.</returns>
        DashboardDetailJob IDashboardQuery.GetJobDetails(int jobId)
        {
            IDashboardGateway gateway = new DashboardGateway();
            return gateway.GetJobDetails(jobId);
        }
    }
}
