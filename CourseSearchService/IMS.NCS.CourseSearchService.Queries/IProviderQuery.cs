using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IMS.NCS.CourseSearchService.Entities;

namespace IMS.NCS.CourseSearchService.Queries
{
    /// <summary>
    /// Interface providing Provider Query functions.
    /// </summary>
    public interface IProviderQuery
    {
        /// <summary>
        /// Executes a search for Providers matching the search keyword.
        /// </summary>
        /// <param name="providerSearchKeyword">Keyword for Provider search.</param>
        /// <param name="APIKey"></param>
        /// <returns>A collection of matching Providers.</returns>
        List<Provider> GetProviders(string providerSearchKeyword, String APIKey);

        /// <summary>
        /// Gets Provider details.
        /// </summary>
        /// <param name="providerId">Id of Provider to get details for.</param>
        /// <param name="APIKey"></param>
        /// <returns>Provider Details.</returns>
        Provider GetProviderDetails(string providerId, String APIKey);
    }
}
