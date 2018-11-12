using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IMS.NCS.CourseSearchService.Entities;

namespace IMS.NCS.CourseSearchService.Queries
{
    /// <summary>
    /// Interface providing Course Query functions.
    /// </summary>
    public interface ICourseQuery
    {
        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A collection of Category entities.</returns>
        List<Category> GetCategories(String APIKey);

        /// <summary>
        /// Retrieves course details for the list of course ids provided.
        /// </summary>
        /// <param name="courseIds">Course ids to return course details for.</param>
        /// <param name="APIKey"></param>
        /// <returns>A collection of Course entities.</returns>
        List<Course> GetCourseDetails(List<long> courseIds, String APIKey);

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
