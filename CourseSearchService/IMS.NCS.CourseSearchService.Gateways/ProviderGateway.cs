using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using IMS.NCS.CourseSearchService;
using IMS.NCS.CourseSearchService.Entities;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using IMS.NCS.CourseSearchService.Common;
using IMS.NCS.CourseSearchService.Exceptions;
using Provider = IMS.NCS.CourseSearchService.Entities.Provider;

namespace IMS.NCS.CourseSearchService.Gateways
{
    /// <summary>
    /// Implementaion of Provider gateway functions.
    /// </summary>
    public class ProviderGateway : IProviderGateway
    {
        #region Public Methods

        /// <summary>
        /// Executes a search for Providers matching the search keyword.
        /// </summary>
        /// <param name="providerSearchKeyword">Keyword for Provider search.</param>
        /// <returns>A collection of matching Providers.</returns>
        public List<Provider> GetProviders(string providerSearchKeyword)
        {
            OracleConnection connection = null;
            OracleCommand command = null;
            OracleParameter providerKeywordIn = null;
            OracleParameter usernameIn = null;
            OracleParameter errorMessageOut = null;
            OracleParameter resultsOut = null;
            OracleDataReader dr = null;

            List<Provider> providers = new List<Provider>();

            try
            {
                // Create Oracle connection
                connection = new OracleConnection(Utilities.GetDatabaseConnection());
                connection.Open();

                // Create Oracle command
                command = new OracleCommand(Constants.PROVIDER_SEARCH_SP, connection);
                command.CommandType = CommandType.StoredProcedure;

                // Input parameters
                providerKeywordIn = new OracleParameter(
                    Constants.ProviderSearchParameters.PROVIDER_KEYWORD, OracleDbType.Varchar2,
                    providerSearchKeyword, ParameterDirection.Input);
                command.Parameters.Add(providerKeywordIn);
                usernameIn = new OracleParameter(
                    Constants.ProviderSearchParameters.EMAIL, OracleDbType.Varchar2, "CourseSearchSvc",
                    ParameterDirection.Input);
                command.Parameters.Add(usernameIn);

                // Output parameters
                errorMessageOut = new OracleParameter(
                    Constants.ProviderSearchParameters.ERROR_MSG, OracleDbType.Varchar2, ParameterDirection.Output);
                command.Parameters.Add(errorMessageOut);
                resultsOut = new OracleParameter(
                    Constants.ProviderSearchParameters.RESULTS_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
                command.Parameters.Add(resultsOut);

                dr = command.ExecuteReader();

                // get error message
                string errorMessage = null;

                if (!((OracleString)(command.Parameters[Constants.ProviderSearchParameters.ERROR_MSG].Value)).IsNull)
                {
                    errorMessage = command.Parameters[Constants.ProviderSearchParameters.ERROR_MSG].Value.ToString();
                }

                if ((errorMessage == null || errorMessage.Length == 0) && dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Provider provider = new Provider();

                        provider.ProviderId = dr[Constants.ProviderSearchColumns.PROVIDER_ID].ToString();
                        provider.ProviderName = dr[Constants.ProviderSearchColumns.PROVIDER_NAME].ToString();
                        provider.AddressLine1 = dr[Constants.ProviderSearchColumns.ADDRESS_LINE1].ToString();
                        provider.AddressLine2 = dr[Constants.ProviderSearchColumns.ADDRESS_LINE2].ToString();
                        provider.Town = dr[Constants.ProviderSearchColumns.TOWN].ToString();
                        provider.County = dr[Constants.ProviderSearchColumns.COUNTY].ToString();
                        provider.Postcode = dr[Constants.ProviderSearchColumns.POSTCODE].ToString();
                        provider.Phone = dr[Constants.ProviderSearchColumns.PHONE].ToString();
                        provider.Email = dr[Constants.ProviderSearchColumns.EMAIL].ToString();
                        provider.Fax = dr[Constants.ProviderSearchColumns.FAX].ToString();
                        provider.Website = dr[Constants.ProviderSearchColumns.WEBSITE].ToString();
                        provider.Ukprn = dr[Constants.ProviderSearchColumns.UKPRN].ToString();
                        provider.Upin = dr[Constants.ProviderSearchColumns.UPIN].ToString();
                        provider.TFPlusLoans =
                            Convert.ToBoolean(dr[Constants.ProviderSearchColumns.TFPLUSLOANS].ToString());

                        providers.Add(provider);
                    }
                }
                else
                {
                    // throw error?
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                // clean up after call ...
                dr.Dispose();
                resultsOut.Dispose();
                errorMessageOut.Dispose();
                usernameIn.Dispose();
                providerKeywordIn.Dispose();
                command.Dispose();
                connection.Dispose();
            }

            return providers;
        }

        /// <summary>
        /// Gets Provider details.
        /// </summary>
        /// <param name="providerId">Id of Provider to get details for.</param>
        /// <returns>Provider Details.</returns>
        public Provider GetProviderDetails(string providerId)
        {
            OracleConnection connection = null;
            OracleCommand command = null;
            OracleParameter providerIdIn = null;
            OracleParameter usernameIn = null;
            OracleParameter errorMessageOut = null;
            OracleParameter providerDetailsOut = null;
            OracleDataReader dr = null;

            Provider provider = new Provider();

            try
            {
                // Create Oracle connection
                connection = new OracleConnection(Utilities.GetDatabaseConnection());
                connection.Open();

                // Create Oracle command
                command = new OracleCommand(Constants.FETCH_PROVIDER_DETAILS_SP, connection);
                command.CommandType = CommandType.StoredProcedure;

                // Input parameters
                providerIdIn = new OracleParameter(
                    Constants.ProviderDetailsParameters.PROVIDER_ID, OracleDbType.Int32,
                    providerId, ParameterDirection.Input);
                command.Parameters.Add(providerIdIn);
                usernameIn = new OracleParameter(
                    Constants.ProviderDetailsParameters.USERNAME, OracleDbType.Varchar2, "CourseSearchSvc", ParameterDirection.Input);
                command.Parameters.Add(usernameIn);

                // Output parameters
                errorMessageOut = new OracleParameter(
                    Constants.ProviderDetailsParameters.ERROR_MSG, OracleDbType.Varchar2, ParameterDirection.Output);
                command.Parameters.Add(errorMessageOut);
                providerDetailsOut = new OracleParameter(
                    Constants.ProviderDetailsParameters.PROVIDER_DETAILS, OracleDbType.RefCursor, ParameterDirection.Output);
                command.Parameters.Add(providerDetailsOut);

                 dr = command.ExecuteReader();

                // get error message
                string errorMessage = null;

                if (!((OracleString)(command.Parameters[Constants.ProviderDetailsParameters.ERROR_MSG].Value)).IsNull)
                {
                    errorMessage = command.Parameters[Constants.ProviderDetailsParameters.ERROR_MSG].Value.ToString();
                }

                if ((errorMessage == null || errorMessage.Length == 0) && dr.HasRows)
                {
                    dr.Read();
                    provider.ProviderId = dr[Constants.ProviderDetailsColumns.PROVIDER_ID].ToString();
                    provider.ProviderName = dr[Constants.ProviderDetailsColumns.PROVIDER_NAME].ToString();
                    provider.AddressLine1 = dr[Constants.ProviderDetailsColumns.ADDRESS_LINE1].ToString();
                    provider.AddressLine2 = dr[Constants.ProviderDetailsColumns.ADDRESS_LINE2].ToString();
                    provider.Town = dr[Constants.ProviderDetailsColumns.TOWN].ToString();
                    provider.County = dr[Constants.ProviderDetailsColumns.COUNTY].ToString();
                    provider.Postcode = dr[Constants.ProviderDetailsColumns.POSTCODE].ToString();
                    provider.Phone = dr[Constants.ProviderDetailsColumns.PHONE].ToString();
                    provider.Email = dr[Constants.ProviderDetailsColumns.EMAIL].ToString();
                    provider.Fax = dr[Constants.ProviderDetailsColumns.FAX].ToString();
                    provider.Website = dr[Constants.ProviderDetailsColumns.WEBSITE].ToString();
                    provider.Ukprn = dr[Constants.ProviderDetailsColumns.UKPRN].ToString();
                    provider.Upin = dr[Constants.ProviderDetailsColumns.UPIN].ToString();
                    provider.TFPlusLoans = Convert.ToBoolean(dr[Constants.ProviderDetailsColumns.TFPLUSLOANS].ToString()); 
                }
                else
                {
                    // throw error?
                }
            }
            finally
            {
                // clean up after call ...
                dr.Dispose();
                providerDetailsOut.Dispose();
                errorMessageOut.Dispose();
                usernameIn.Dispose();
                providerIdIn.Dispose();
                command.Dispose();
                connection.Dispose();
            }

            return provider;
        }

        #endregion Public Methods
    }
}
