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
    /// Implementation of Provider Query functions.
    /// </summary>
    public class ProviderQuery : IProviderQuery
    {
        #region Variables

        private IProviderGateway _providerGateway = null;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Unity Constructor.
        /// </summary>
        /// <param name="providerGateway">IProviderGateway object.</param>
        public ProviderQuery(IProviderGateway providerGateway)
        {
            _providerGateway = providerGateway;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Executes a search for Providers matching the search keyword.
        /// </summary>
        /// <param name="providerSearchKeyword">Keyword for Provider search.</param>
        /// <returns>A collection of matching Providers.</returns>
        public List<Provider> GetProviders(string providerSearchKeyword, String APIKey)
        {
            return _providerGateway.GetProviders(providerSearchKeyword, APIKey);
        }

        /// <summary>
        /// Gets Provider details.
        /// </summary>
        /// <param name="providerId">Id of Provider to get details for.</param>
        /// <returns>Provider Details.</returns>
        public Provider GetProviderDetails(string providerId, String APIKey)
        {
            return _providerGateway.GetProviderDetails(providerId, APIKey);
        }

        #endregion Public Methods
    }
}
