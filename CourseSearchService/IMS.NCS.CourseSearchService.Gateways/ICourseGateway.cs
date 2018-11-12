using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IMS.NCS.CourseSearchService.Entities;

namespace IMS.NCS.CourseSearchService.Gateways
{
    /// <summary>
    /// Interface providing Course gateway functions.
    /// </summary>
    public interface ICourseGateway
    {
        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A collection of Category entities.</returns>
        List<Category> GetCategories();

        /// <summary>
        /// Retrieves course details for the list of course ids provided.
        /// </summary>
        /// <param name="courseIds">Course ids to return course details for.</param>
        /// <returns>A collection of Course entities.</returns>
        List<Course> GetCourseDetails(List<long> courseIds);

        /// <summary>
        /// Retrieves courses matching the search criteria in the request.
        /// </summary>
        /// <param name="request">Course search criteria.</param>
        /// <returns>A CourseList Response entity.</returns>
        CourseListResponse GetCourseList(CourseListRequest request);

        /// <summary>
        /// Records time taken for search.
        /// </summary>
        /// <param name="columnFlag">Column flag.</param>
        /// <param name="searchHeaderId">Search header Id</param>
        void RecordSearchTime(string columnFlag, string searchHeaderId);
    }
}
