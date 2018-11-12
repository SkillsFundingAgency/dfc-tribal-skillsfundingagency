using System.Collections.Generic;
using System.Configuration;
using Provider = IMS.NCS.CourseSearchService.Entities.Provider;
using IMS.NCS.CourseSearchService.DatabaseContext;
using System;
using System.Linq;
using System.Diagnostics;

namespace IMS.NCS.CourseSearchService.Sql.Gateways
{
    public class ProviderGateway : IProviderGateway
    {
        #region Public methods

        /// <summary>
        /// Called from "Get Providers" Service Method.
        /// </summary>
        /// <param name="providerSearchKeyword">string name to be searched against provider</param>
        /// <param name="APIKey"></param>
        /// <returns></returns>
        public List<Provider> GetProviders(string providerSearchKeyword, String APIKey)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                Boolean isPublicAPI = ConfigurationManager.AppSettings["IncludeUCASData"].ToLower() != "true";
                if (isPublicAPI && String.IsNullOrEmpty(APIKey.Trim()))
                {
                    return null;
                }

                var providers = CacheHelper.GetProvidersByName(providerSearchKeyword, isPublicAPI ? 1 : 0, APIKey);

                if (providers == null)
                {
                    var filteredSearchKeyword = new DBHelper().LoadStopWordFilteredList(providerSearchKeyword);

                    using (SFA_SearchAPIEntities db = new SFA_SearchAPIEntities())
                    {
                        providers = db.API_Provider_GetAll_v2(isPublicAPI ? 1 : 0, APIKey)
                            .Where(p => (
                                p.ProviderName.RemoveLeadingAndTrailingSymbols().Split(' ').Intersect(filteredSearchKeyword, StringComparer.OrdinalIgnoreCase).Count() >= filteredSearchKeyword.Count()
                                ||
                                p.ProviderNameAlias.RemoveLeadingAndTrailingSymbols().Split(' ').Intersect(filteredSearchKeyword, StringComparer.OrdinalIgnoreCase).Count() >= filteredSearchKeyword.Count()
                                ) && !filteredSearchKeyword.Count().Equals(0))
                            .Select(p => new Provider
                            {
                                ProviderId = p.ProviderId.ToString(),
                                ProviderName = p.ProviderName,
                                AddressLine1 = p.AddressLine1,
                                AddressLine2 = p.AddressLine2,
                                Town = p.Town,
                                County = p.County,
                                Postcode = p.Postcode,
                                Phone = p.Telephone,
                                Email = p.Email,
                                Website = p.Website,
                                Ukprn = p.Ukprn.ToString(),
                                Upin = p.UPIN.HasValue ? p.UPIN.ToString() : string.Empty,
                                TFPlusLoans = p.Loans24Plus,
                                DFE1619Funded = p.DFE1619Funded
                            })
                            .ToList();
                    }

                    CacheHelper.SaveProvider(providerSearchKeyword, isPublicAPI ? 1 : 0, APIKey, providers);
                }

                new DBHelper().LogProviderRequestResponseLog(DBHelper.ServiceMethodName.GetProviders, stopwatch.ElapsedMilliseconds, DBHelper.Serialize(providerSearchKeyword), DBHelper.Serialize(providers), isPublicAPI, APIKey, providers.Count);

                return providers;
            }
            catch (Exception ex)
            {
                LogException(ex, "GetProviders");
                throw;
            }
        }

        /// <summary>
        /// Called by "Get Provider Details" Service Method.
        /// </summary>
        /// <param name="providerId">Provider Id from provider table</param>
        /// <param name="APIKey"></param>
        /// <returns></returns>
        public Provider GetProviderDetails(string providerId, String APIKey)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                Boolean isPublicAPI = ConfigurationManager.AppSettings["IncludeUCASData"].ToLower() != "true";
                if (isPublicAPI && String.IsNullOrEmpty(APIKey.Trim()))
                {
                    return null;
                }

                int formattedProviderId;
                int.TryParse(providerId, out formattedProviderId);

                var searchedProvider = CacheHelper.GetProviderById(formattedProviderId, isPublicAPI ? 1 : 0, APIKey);

                if (searchedProvider == null)
                {
                    using (SFA_SearchAPIEntities db = new SFA_SearchAPIEntities())
                    {
                        searchedProvider = db.API_Provider_GetByProviderId_v2(formattedProviderId, isPublicAPI ? 1 : 0, APIKey)
                            .Select(p => new Provider
                            {
                                ProviderId = p.ProviderId.ToString(),
                                ProviderName = p.ProviderName,
                                AddressLine1 = p.AddressLine1,
                                AddressLine2 = p.AddressLine2,
                                Town = p.Town,
                                County = p.County,
                                Postcode = p.Postcode,
                                Phone = p.Telephone,
                                Email = p.Email,
                                Website = p.Website,
                                Ukprn = p.Ukprn.ToString(),
                                Upin = p.UPIN.HasValue ? p.UPIN.ToString() : string.Empty,
                                TFPlusLoans = p.Loans24Plus,
                                DFE1619Funded = p.DFE1619Funded
                            })
                            .FirstOrDefault();
                    }
                    CacheHelper.SaveProviderById(formattedProviderId, isPublicAPI ? 1 : 0, APIKey, searchedProvider);
                }

                new DBHelper().LogProviderRequestResponseLog(DBHelper.ServiceMethodName.GetProviderDetails, stopwatch.ElapsedMilliseconds, DBHelper.Serialize(providerId), DBHelper.Serialize(searchedProvider), isPublicAPI, APIKey, searchedProvider == null ? 0 : 1);

                return searchedProvider;
            }
            catch (Exception ex)
            {
                LogException(ex, "GetProviderDetails");
                throw;
            }
        }

        #endregion

        #region Private Methods

        private static void LogException(Exception ex, String method)
        {
            Log log = new Log
            {
                MachineName = Environment.MachineName,
                Method = method,
                Details = String.Format("{0}\r\rStack Trace: {1}", ex.Message, ex.StackTrace ?? String.Empty)
            };

            if (ex.InnerException != null)
            {
                log.Details += String.Format("\r\rInner Exception: {0}\r\rInner Stack Trace: {1}", ex.InnerException.Message, ex.InnerException.StackTrace ?? String.Empty);
            }

            using (SFA_SearchAPIEntities db = new SFA_SearchAPIEntities())
            {
                db.Logs.Add(log);
                db.SaveChanges();
            }
        }

        #endregion
    }
}

public static class StringExtensions
{
    public static String RemoveLeadingAndTrailingSymbols(this String inValue)
    {
        if (String.IsNullOrWhiteSpace(inValue))
        {
            return String.Empty;
        }

        String[] invalidCharacters = { "-", "+", "!", "\"", "£", "(", ")", "[", "]", "{", "}", "&", "@", "#", "~", ":", ";", ",", ".", "/", "?", "<", ">" };
        String[] words = inValue.Split(' ');

        for (Int32 i = 0; i < words.GetLength(0); i++)
        {
            while (words[i].Length > 0 && invalidCharacters.Contains(words[i].Substring(0, 1)))
            {
                words[i] = words[i].Substring(1);
            }

            while (words[i].Length > 0 && invalidCharacters.Contains(words[i].Substring(words[i].Length - 1, 1)))
            {
                words[i] = words[i].Substring(0, words[i].Length - 1);
            }
        }

        return String.Join(" ", words);
    }
}
