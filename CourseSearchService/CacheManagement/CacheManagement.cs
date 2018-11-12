using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

/*
 * To use this CacheManagement project in your own MVC solution:
 *
 * Add the following to the top of AppStart/RouteConfig.cs
 * 
 *      //ignore the CacheManagement handler
 *      routes.IgnoreRoute("{*cache}", new { cache=@"clear.cache" });
 * 
 * Add the following to the <systemweb><httpHandlers> section of web.config
 * 
 *      <add verb="*" path="clear.cache" type="CacheManagement.CacheHandler, CacheManagement" />
 * 
 * Add the following to the <system.webserver><handlers> section of web.config
 * 
 *      <add name="CacheManagement" path="*.cache" verb="*" type="CacheManagement.CacheHandler, CacheManagement" />
 * 
 * 
 * To invalidate a cached item, this will notify all other servers to remove this item from the cache:
 * 
 *      CacheManagement.CacheHandler.Invalidate("key--leave-blank-to-clear-entire-cache");
 *      
 */

namespace IMS.NCS.CacheManagement
{
    #region Constants
    public class WebConfig
    {
        public const string SERVER_FARM_URLS = "ServerFarmUrls";
    }

    public class QueryString
    {
        public const string KEY = "key";
        public const string NOTIFY = "notify";
    }
    #endregion

    public class CacheHandler : IHttpHandler
    {
        /// <summary>
        /// Use this object to lock the cache whilst checking / adding
        /// </summary>
        public static object LockObject = new object();

        #region IHttpHandler Members
        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            var keyStartsWith = context.Request.QueryString[QueryString.KEY];
            var notify = Convert.ToBoolean(context.Request.QueryString[QueryString.NOTIFY]);
            ClearCache(keyStartsWith, notify);
        }
        #endregion

        //Public Static Methods

        #region Public Static Method - Invalidate
        /// <summary>
        /// Clears the cache for the specified key, or clears the entire cache if key is blank - also notifies all other servers in the farm to do the same
        /// </summary>
        /// <param name="key">The cache key to be invalidated</param>
        public static void Invalidate(string keyStartsWith)
        {
            //remove key from this server's cache
            ClearCache(keyStartsWith, true);            
        }
        #endregion

        #region Public Static Method - NotifyServers
        /// <summary>
        /// Notifies other servers of cache invalidation (comma separated list in the ServerFarmURLs setting in the web.config)
        /// </summary>
        /// <param name="key">The cache key to be cleared</param>
        public static void NotifyServers(string keyStartsWith)
        {
            string otherServers = System.Web.Configuration.WebConfigurationManager.AppSettings[WebConfig.SERVER_FARM_URLS];
            if (!string.IsNullOrEmpty(otherServers))
            {
                string[] listURLs = otherServers.Split(',');
                foreach (string url in listURLs)
                {
                    try
                    {
                        string newURL = url;
                        newURL = url + "/clear.cache?notify=false&key=" + HttpContext.Current.Server.UrlEncode(keyStartsWith);

                        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(newURL);
                        webRequest.Method = "GET";
                        webRequest.BeginGetResponse(null, null); //backgrounded - don't care about response
                    }
                    catch (Exception ex)
                    {
                        // TODO Elmah.Elmah.LogError(ex, "Error in CacheManagement.NotifyServers");
                    }
                }
            }
        }
        #endregion

        #region Public Static Method - Add (Overloaded for no expiry, sliding expiration and absolute expiration)
        /// <summary>
        /// Add a cache entry with no expiry
        /// </summary>
        /// <param name="key">The cache key used to reference the item</param>
        /// <param name="value">The item to be added to the cache</param>
        public static object Add(string key, object value)
        {
            return System.Web.HttpRuntime.Cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
        }
        /// <summary>
        /// Add a cache entry with a sliding expiration
        /// </summary>
        /// <param name="key">The cache key used to reference the item</param>
        /// <param name="value">The item to be added to the cache</param>
        /// <param name="SlidingExpirationTimespan">The interval between the time the added object was last accessed and the time at which that object expires. If this value is the equivalent of 20 minutes, the object expires and is removed from the cache 20 minutes after it is last accessed.</param>
        public static object Add(string key, object value, TimeSpan SlidingExpirationTimespan)
        {
            return System.Web.HttpRuntime.Cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, SlidingExpirationTimespan, System.Web.Caching.CacheItemPriority.Normal, null);
        }
        /// <summary>
        /// Add a cache entry with an expiration datetime
        /// </summary>
        /// <param name="key">The cache key used to reference the item</param>
        /// <param name="value">The item to be added to the cache</param>
        /// <param name="AbsoluteExpirationDatetime">The time at which the added object expires and is removed from the cache. </param>
        public static object Add(string key, object value, DateTime AbsoluteExpirationDatetime)
        {
            return System.Web.HttpRuntime.Cache.Add(key, value, null, AbsoluteExpirationDatetime, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
        }
        #endregion

        #region Public Static Method - Get
        /// <summary>
        /// Retrieves the specified item from the cache
        /// </summary>
        /// <param name="key">The identifier for the cache item to retrieve</param>
        /// <returns>The cached item</returns>
        public static object Get(string key)
        {
            return System.Web.HttpRuntime.Cache.Get(key);
        }
        #endregion

        #region Public Static Method - ListForSiteTools
        public static Dictionary<string, string> ListForSiteTools()
        {
            Dictionary<string, string> list = new Dictionary<string,string>();
            System.Collections.IDictionaryEnumerator CacheEnumerater = System.Web.HttpRuntime.Cache.GetEnumerator();

            while (CacheEnumerater.MoveNext())
            {
                if (!CacheEnumerater.Key.ToString().StartsWith("System.Web.Optimization.Bundle")
                    && !CacheEnumerater.Key.ToString().StartsWith("mini-profiler-")
                    && !CacheEnumerater.Key.ToString().StartsWith("__AppStartPage__"))
                {
                    list.Add(CacheEnumerater.Key.ToString(), CacheEnumerater.Value.ToString());
                }
            }

            return list;
        }
        #endregion

        //Private Static Methods

        #region Private Static Method - ClearCache
        /// <summary>
        /// Clears the cache for the specified key, or clears the entire cache if key is blank
        /// </summary>
        /// <param name="key">The cache key to be cleared</param>
        private static void ClearCache(string keyStartsWith, bool Notify = true)
        {
            lock (LockObject)
            {
                System.Web.Caching.Cache Cache = System.Web.HttpRuntime.Cache;
                System.Collections.IDictionaryEnumerator CacheEnumerater;
                CacheEnumerater = Cache.GetEnumerator();

                if (string.IsNullOrEmpty(keyStartsWith))
                {
                    //empty key - clear everything from the cache
                    while (CacheEnumerater.MoveNext())
                    {
                        Cache.Remove(CacheEnumerater.Key.ToString());
                    }
                }
                else
                {
                    //key given - clear an individual item
                    while (CacheEnumerater.MoveNext())
                    {
                        if (CacheEnumerater.Key.ToString().ToLower().StartsWith(keyStartsWith.ToLower()))
                        {
                            Cache.Remove(CacheEnumerater.Key.ToString());
                        }
                    }
                }
            }
            if (Notify)
            {
                NotifyServers(keyStartsWith);
            }
        }
        #endregion

    }
}
