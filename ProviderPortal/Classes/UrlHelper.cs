using System;
using System.Web;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public static class UrlHelper
    {
        public static String GetFullUrl(String url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return url;
            }

            if (!url.ToLower().StartsWith("http://") && !url.ToLower().StartsWith("https://"))
            {
                url = "http://" + url;
            }

            return url;
        }

        public static Boolean UrlIsValidFormat(String url)
        {
            url = GetFullUrl(url);

            url = HttpContext.Current.Server.UrlEncode(HttpContext.Current.Server.UrlDecode(url));

            return AppGlobal.TestRegex(url, ProviderPortalUrl.urlRegularExpression);
        }

        /// <summary>
        /// This method will check a url to see that it does not return server or protocol errors
        /// </summary>
        /// <param name="url">The path to check</param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool UrlIsReachable(string url, string method = "HEAD")
        {
            try
            {
                if (!UrlIsValidFormat(url))
                {
                    return false;
                }

                url = GetFullUrl(url);
                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
                if (request != null)
                {
                    request.Timeout = 5000; //set the timeout to 5 seconds to keep the user from waiting too long for the page to load
                    request.Method = method; 

                    using (System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse)
                    {
                        if (response != null)
                        {
                            int statusCode = Convert.ToInt32(response.StatusCode);
                            if (statusCode >= 100 && statusCode < 400) // Good requests
                            {
                                return true;
                            }
                            if (statusCode == 403 && method == "HEAD") // Method Not Allowed
                            {
                                return false; UrlIsReachable(url, "GET");
                            }
                            if (statusCode >= 500 && statusCode <= 510) // Server Errors
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Status == System.Net.WebExceptionStatus.ProtocolError) // 400 errors
                {
                    return false;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}