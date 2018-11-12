using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using IMS.NCS.CourseSearchService.Entities;
using IMS.NCS.CourseSearchService.Queries;
using Ims.Schemas.Alse.CourseSearch.Contract;

namespace IMS.NCS.CourseSearchService.BusinessServices
{
    /// <summary>
    /// Service Implementation providing all provider functions.
    /// </summary>
    public class ProviderService : IProviderService
    {
        #region Variables

        private IProviderQuery _providerQuery = null;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Unity Constructor.
        /// </summary>
        /// <param name="providerQuery">IProviderQuery object.</param>
        public ProviderService(IProviderQuery providerQuery)
        {
            _providerQuery = providerQuery;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Executes a search for Providers matching the search keyword.
        /// </summary>
        /// <param name="request">ProviderSearchInput request object.</param>
        /// <returns>A ProviderSearchOuput object.</returns>
        public ProviderSearchOutput GetProviders(ProviderSearchInput request)
        {
            ProviderSearchOutput providerSearchOutput = new ProviderSearchOutput(new ProviderSearchResponseStructure());
            providerSearchOutput.ProviderSearchResponse.RequestDetails = new ProviderSearchResponseStructureRequestDetails();
            providerSearchOutput.ProviderSearchResponse.RequestDetails.ProviderSearch = request.ProviderSearchRequest;

            List<Provider> providers =
                _providerQuery.GetProviders(request.ProviderSearchRequest.ProviderKeyword, request.ProviderSearchRequest.APIKey);

            List<ProviderStructure> providerStructures = new List<ProviderStructure>();

            foreach (Provider provider in providers)
            {
                ProviderStructure providerStructure = new ProviderStructure();
                providerStructure.Provider = BuildProviderDetail(provider);

                providerStructures.Add(providerStructure);
            }

            providerSearchOutput.ProviderSearchResponse.ProviderDetails = providerStructures.ToArray();

            return providerSearchOutput;
        }

        /// <summary>
        /// Gets Provider details.
        /// </summary>
        /// <param name="request">ProviderDetailsInput request object.</param>
        /// <returns>ProviderDetailsOutput object.</returns>
        public ProviderDetailsOutput GetProviderDetails(ProviderDetailsInput request)
        {
            ProviderDetailsOutput providerDetailsOutput=null;

            var provider = _providerQuery.GetProviderDetails(request.ProviderID, request.APIKey);

            if (provider != null)
            {
                providerDetailsOutput = new ProviderDetailsOutput(new ProviderDetailsRequestStructure(), new ProviderDetail());
                providerDetailsOutput.RequestDetails.ProviderID = request.ProviderID;
                providerDetailsOutput.ProviderDetails = BuildProviderDetail(provider);
            }

            return providerDetailsOutput;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Populates a ProviderDetail object with values from a Provider entity.
        /// </summary>
        /// <param name="provider">Provider entity.</param>
        /// <returns>Populated ProviderDetail object.</returns>
        private static ProviderDetail BuildProviderDetail(Provider provider)
        {
            ProviderDetail providerDetail = new ProviderDetail
            {
                ProviderID = provider.ProviderId, 
                ProviderName = provider.ProviderName, 
                ProviderAddress = new AddressType
                {
                    Address_line_1 = provider.AddressLine1,
                    Address_line_2 = provider.AddressLine2,
                    Town = provider.Town,
                    County = provider.County,
                    PostCode = provider.Postcode
                },
                Phone = provider.Phone, 
                Email = provider.Email, 
                Fax = provider.Fax, 
                Website = provider.Website, 
                UKPRN = provider.Ukprn, 
                UPIN = provider.Upin, 
                TFPlusLoans = provider.TFPlusLoans, 
                TFPlusLoansSpecified = true, 
                DFE1619Funded = provider.DFE1619Funded,
                DFE1619FundedSpecified = true
            };

            return providerDetail;
        }

        #endregion Private Methods
    }
}
