using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using IMS.NCS.CourseSearchService.DatabaseContext;
using IMS.NCS.CourseSearchService.Entities;

namespace IMS.NCS.CourseSearchService.Sql.Gateways
{
    public class DBHelper
    {
        #region private fields

        private static bool EnableProviderRequestResponseLogging = false;

        #endregion

        #region constructors

        static DBHelper()
        {
            if (ConfigurationManager.AppSettings["EnableProviderRequestResponseLogging"] != null)
            {
                bool.TryParse(ConfigurationManager.AppSettings["EnableProviderRequestResponseLogging"], out EnableProviderRequestResponseLogging);
            }
        }

        #endregion

        #region Public methods

        public List<string> LoadStopWordFilteredList(string providerSearchKeyword)
        {
            List<string> filteredSearchKeyword = new List<string>();

            if (!string.IsNullOrEmpty(providerSearchKeyword))
            {
                var STOP_WORDS = LoadStopWords();

                filteredSearchKeyword.AddRange(providerSearchKeyword.RemoveLeadingAndTrailingSymbols().Split(' ').Select(w => STOP_WORDS.Contains(w.Trim().ToLower()) ? "" : w.Trim())
                                                                            .Where(w => !string.IsNullOrEmpty(w))
                                                                            .ToList());
            }
            return filteredSearchKeyword;
        }

        public Dictionary<string, string> LoadCategoryCodes(Int32 isPublicAPI, String APIKey)
        {
            var result = new Dictionary<string, string>();

            using (SFA_SearchAPIEntities db = new SFA_SearchAPIEntities())
            {
                var categoryCodes = db.API_CategoryCode_GetAll_v2(isPublicAPI, APIKey).ToList();
                foreach (var categoryCode in categoryCodes)
                {
                    if (!result.ContainsKey(categoryCode.CategoryCodeName))
                        result.Add(categoryCode.CategoryCodeName, categoryCode.Description);

                }
            }
            return result;
        }

        public static string Serialize<T>(T value)
        {
            string result = string.Empty;

            if (value == null)
            {
                return result;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T), new[] { typeof(OpportunityResult) });
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, value);
                    result = stringWriter.ToString();
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = string.Concat(ex.Message, " ", ex.StackTrace ?? string.Empty);
                if (ex.InnerException != null)
                {
                    result += String.Format("\r\rInner Exception: {0}", ex.InnerException.Message);
                }
                return result;
            }
        }

        public void LogProviderRequestResponseLog(ServiceMethodName methodName, long timeInMilliseconds, string request, string response, Boolean isPublicAPI, String APIKey, Int32 recordCount)
        {
            if (EnableProviderRequestResponseLogging)
            {
                new Task(() => Log(request, response, (int)timeInMilliseconds, methodName, isPublicAPI, APIKey, recordCount)).Start();
            }
        }

        public enum ServiceMethodName
        {
            GetCategories = 0,
            GetCourseDetails = 1,
            GetCourseList = 2,
            GetProviders = 3,
            GetProviderDetails = 4
        }

        #endregion

        #region Private methods

        private static void Log(string request, string response, int timeInMilliseconds, ServiceMethodName methodName, Boolean isPublicAPI, String APIKey, Int32 recordCount)
        {
            try
            {
                using (SFA_SearchAPIEntities db = new SFA_SearchAPIEntities())
                {
                    db.ProviderRequestResponesLog_Insert_v2(methodName.ToString(), request, response, timeInMilliseconds, isPublicAPI, APIKey, recordCount);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //swallow the exception
            }
        }

        private static List<string> LoadStopWords()
        {
            var stopwords = CacheHelper.StopWords;

            if (stopwords == null)
            {
                using (SFA_SearchAPIEntities db = new SFA_SearchAPIEntities())
                {
                    stopwords = db.StopWords
                        .Select(s => s.StopWordName.ToLower())
                        .ToList();
                }

                CacheHelper.AddStopwords(stopwords);
            }

            return stopwords;
        }

        #endregion
    }
}
