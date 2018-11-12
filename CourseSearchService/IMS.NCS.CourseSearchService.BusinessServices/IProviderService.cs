using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ims.Schemas.Alse.CourseSearch.Contract;

namespace IMS.NCS.CourseSearchService.BusinessServices
{
    /// <summary>
    /// Interface for providing all Provider releated functions.
    /// </summary>
    public interface IProviderService
    {
        /// <summary>
        /// Executes a search for Providers matching the search keyword.
        /// </summary>
        /// <param name="request">ProviderSearchInput request object.</param>
        /// <returns>A ProviderSearchOuput object.</returns>
        ProviderSearchOutput GetProviders(ProviderSearchInput request);

        /// <summary>
        /// Gets Provider details.
        /// </summary>
        /// <param name="request">ProviderDetailsInput request object.</param>
        /// <returns>ProviderDetailsOutput object.</returns>
        ProviderDetailsOutput GetProviderDetails(ProviderDetailsInput request);
    }
}
