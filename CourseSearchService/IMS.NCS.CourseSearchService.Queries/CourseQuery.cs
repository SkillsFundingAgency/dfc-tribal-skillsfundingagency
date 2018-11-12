using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using IMS.NCS.CourseSearchService.Entities;
using IMS.NCS.CourseSearchService.Sql.Gateways;
 

namespace IMS.NCS.CourseSearchService.Queries
{
    /// <summary>
    /// Implementation of Course Query functions.
    /// </summary>
    public class CourseQuery : ICourseQuery
    {
        #region Variables

        private ICourseGateway _courseGateway = null;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Unity Constructor.
        /// </summary>
        /// <param name="courseGateway">ICourseGateway object.</param>
        public CourseQuery(ICourseGateway courseGateway)
        {
            _courseGateway= courseGateway;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A collection of Category entities.</returns>
        public List<Category> GetCategories(String APIKey)
        {
            return _courseGateway.GetCategories(APIKey);
        }

        /// <summary>
        /// Retrieves course details for the list of course ids provided.
        /// </summary>
        /// <param name="courseIds">Course ids to return course details for.</param>
        /// <param name="APIKey"></param>
        /// <returns>A collection of Course entities.</returns>
        public List<Course> GetCourseDetails(List<long> courseIds, String APIKey)
        {
            return _courseGateway.GetCourseDetails(courseIds, APIKey);
        }

        /// <summary>
        /// Retrieves courses matching the search criteria in the request.
        /// </summary>
        /// <param name="request">Course search criteria.</param>
        /// <returns>A CourseList Response entity.</returns>
        public CourseListResponse GetCourseList(CourseListRequest request)
        {
            return _courseGateway.GetCourseList(request);
        }

        /// <summary>
        /// Records time taken for search.
        /// </summary>
        /// <param name="columnFlag">Column flag.</param>
        /// <param name="searchHeaderId">Search header Id</param>
        public void RecordSearchTime(string columnFlag, string searchHeaderId)
        {
            _courseGateway.RecordSearchTime(columnFlag, searchHeaderId);
        }

        #endregion Public Methods
    }
}
