using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ims.Schemas.Alse.CourseSearch.Contract;

namespace IMS.NCS.CourseSearchService.BusinessServices
{
    /// <summary>
    /// Interface for providing all Course releated functions.
    /// </summary>
    public interface ICourseService
    {
        /// <summary>
        /// Gets a list of courses matching the search criteria in CourseListRequestStructure.
        /// </summary>
        /// <param name="courseListInput">Search criteria.</param>
        /// <returns>Populated CourseLisOutput containing matching courses.</returns>
        CourseListOutput GetCourseList(CourseListInput courseListInput);

        /// <summary>
        /// Gets course details for the course ids in CourseDetailInput.
        /// </summary>
        /// <param name="courseDetailInput">CourseDetailInput containing course ids to return details for.</param>
        /// <returns>Populated CourseDetailsOutput.</returns>
        CourseDetailOutput GetCourseDetails(CourseDetailInput courseDetailInput);

        /// <summary>
        /// Gets a list of categories matching hte search crietria.
        /// </summary>
        /// <param name="subjectBrowseInput">Search criteria.</param>
        /// <returns>Populated SubjectBrowseOutput containing matching categories.</returns>
        SubjectBrowseOutput GetCategories(SubjectBrowseInput subjectBrowseInput);
    }
}
