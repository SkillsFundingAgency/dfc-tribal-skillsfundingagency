using System;
using System.Collections.Generic;
using System.Web.Caching;
using IMS.NCS.CourseSearchService.Entities;

namespace IMS.NCS.CourseSearchService.Sql.Gateways
{
    public static class CacheHelper
    {
        #region Private fields

        private const string CategoryCacheKey = "Cache_CategoryCode_All";
        private const string CategoryCodesListKey = "Cache_CategoryCodes_List";
        private const string ProviderSearchByNameKey = "Cache_ProviderSearch_ByName";
        private const string StopwordsKey = "StopWords";
        private const string ProviderSearchByIdKey = "Cache_ProviderSearch_ById";
        private const string CourseDetailsByIdKey = "Cache_CourseDetails_ById";
        private const string CourseListResponseKey = "Cache_CourseReponse_GetBy_Request";
        private const string ProviderIdByProviderNamekey = "Cache_Matching_Provider_IdBy_ProviderName";
        private const string CourseIdByCourseNamekey = "Cache_Matching_Course_IdBy_CourseSubject";

        private static DateTime CacheExpiryDate
        {
            get { return DateTime.UtcNow.AddHours(2); }
        }

        #endregion

        #region static methods /properties

        public static List<Category> GetCategories(String APIKey)
        {
            return (List<Category>) CacheManagement.CacheHandler.Get(CategoryCacheKey);
        }

        public static void SaveCategories(String APIKey, List<Category> categories)
        {
            CacheManagement.CacheHandler.Add(CategoryCacheKey, categories, CacheExpiryDate);
        }

        public static Dictionary<string, string> CategoryCodesList(Int32 isPublicAPI, String APIKey)
        {
            var result =
                ((Dictionary<string, string>) CacheManagement.CacheHandler.Get(CategoryCodesListKey + "_" + isPublicAPI));

            if (result == null)
            {
                result = new DBHelper().LoadCategoryCodes(isPublicAPI, APIKey);
            }

            CacheManagement.CacheHandler.Add(CategoryCodesListKey + "_" + isPublicAPI, result, CacheExpiryDate);

            return result;
        }

        public static List<Provider> GetProvidersByName(string providerName, Int32 isPublicAPI, String APIKey)
        {
            String searchKey = String.Concat(ProviderSearchByNameKey, "_", providerName + "_" + isPublicAPI);
            List<Provider> result = (List<Provider>) CacheManagement.CacheHandler.Get(searchKey);

            return result;
        }

        public static void SaveProvider(string providerName, Int32 isPublicAPI, String APIKey, List<Provider> providers)
        {
            String searchKey = String.Concat(ProviderSearchByNameKey, "_", providerName + "_" + isPublicAPI);
            CacheManagement.CacheHandler.Add(searchKey, providers, CacheExpiryDate);
        }

        public static List<string> StopWords
        {
            get { return (List<string>) CacheManagement.CacheHandler.Get(StopwordsKey); }
        }

        public static void AddStopwords(List<string> stopwords)
        {
            CacheManagement.CacheHandler.Add(StopwordsKey, stopwords, CacheExpiryDate);
        }

        public static Provider GetProviderById(int providerId, Int32 isPublicAPI, String APIKey)
        {
            String searchKey = String.Concat(ProviderSearchByIdKey, "_", providerId + "_" + isPublicAPI);
            Provider result = (Provider) CacheManagement.CacheHandler.Get(searchKey);

            return result;
        }

        public static void SaveProviderById(int providerId, Int32 isPublicAPI, String APIKey, Provider providers)
        {
            String searchKey = String.Concat(ProviderSearchByIdKey, "_", providerId + "_" + isPublicAPI);
            CacheManagement.CacheHandler.Add(searchKey, providers, CacheExpiryDate);
        }

        public static Entities.Course GetCourseDetailsByCourseId(int courseId, Int32 isPublicAPI, String APIKey)
        {
            var searchKey = string.Concat(CourseDetailsByIdKey, "_", courseId + "_" + isPublicAPI);

            var result = (Entities.Course) CacheManagement.CacheHandler.Get(searchKey);

            return result;
        }

        public static void SaveCourseDetailsByCourseId(int courseId, Int32 isPublicAPI, String APIKey,
            Entities.Course courses)
        {
            var searchKey = string.Concat(CourseDetailsByIdKey, "_", courseId + "_" + isPublicAPI);

            CacheManagement.CacheHandler.Add(searchKey, courses, CacheExpiryDate);
        }

        public static CourseListResponse GetCourseListResponse(string courseStringCacheKey)
        {
            var searchKey = string.Concat(CourseListResponseKey, "_", courseStringCacheKey);

            var result = (CourseListResponse) CacheManagement.CacheHandler.Get(searchKey);

            return result;
        }

        public static void SaveCourseListResponse(string courseStringCacheKey, CourseListResponse response)
        {
            var searchKey = string.Concat(CourseListResponseKey, "_", courseStringCacheKey);

            CacheManagement.CacheHandler.Add(searchKey, response, CacheExpiryDate);
        }

        public static string GetCourseIdsByCourseName(string key)
        {
            var searchKey = string.Concat(CourseIdByCourseNamekey, "_", key);

            var result = (string) CacheManagement.CacheHandler.Get(searchKey);

            return result;
        }

        public static void SaveCourseListResponse(string key, string courseIdsByCourseName)
        {
            var searchKey = string.Concat(CourseIdByCourseNamekey, "_", key);

            CacheManagement.CacheHandler.Add(searchKey, courseIdsByCourseName, CacheExpiryDate);
        }

        public static string GetProviderIdsByProviderName(string key)
        {
            var searchKey = string.Concat(ProviderIdByProviderNamekey, "_", key);

            var result = (string) CacheManagement.CacheHandler.Get(searchKey);

            return result;
        }

        public static void SaveProviderIdsByProviderName(string key, string courseIdsByCourseName)
        {
            var searchKey = string.Concat(ProviderIdByProviderNamekey, "_", key);

            CacheManagement.CacheHandler.Add(searchKey, courseIdsByCourseName, CacheExpiryDate);
        }

        #endregion
    }
}
