using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;

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

namespace Tribal.SkillsFundingAgency.CacheManagement
{

    #region Constants

    public class WebConfig
    {
        public const string ServerFarmUrls = "ServerFarmUrls";
    }

    public class QueryString
    {
        public const string Key = "key";
        public const string Notify = "notify";
    }

    #endregion

    public class CacheHandler : IHttpHandler
    {
        /// <summary>
        ///     Use this object to lock the cache whilst checking / adding
        /// </summary>
        public static object LockObject = new object();

        //Public Static Methods

        #region Public Static Method - Invalidate

        /// <summary>
        ///     Clears the cache for the specified key, or clears the entire cache if key is blank - also notifies all other
        ///     servers in the farm to do the same
        /// </summary>
        /// <param name="keyStartsWith">The cache key to be invalidated</param>
        public static void Invalidate(string keyStartsWith)
        {
            //remove key from this server's cache
            ClearCache(keyStartsWith);
        }

        #endregion

        #region Public Static Method - NotifyServers

        /// <summary>
        ///     Notifies other servers of cache invalidation (comma separated list in the ServerFarmURLs setting in the web.config)
        /// </summary>
        /// <param name="keyStartsWith">The cache key to be cleared</param>
        public static void NotifyServers(string keyStartsWith)
        {
            var otherServers = WebConfigurationManager.AppSettings[WebConfig.ServerFarmUrls];
            if (!string.IsNullOrEmpty(otherServers))
            {
                var listUrls = otherServers.Split(',');
                foreach (var url in listUrls)
                {
                    try
                    {
                        var newUrl = url + "/clear.cache?notify=false&key=" + HttpUtility.UrlEncode(keyStartsWith);
                        var webRequest = (HttpWebRequest) WebRequest.Create(newUrl);
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

        #region Public Static Method - Get

        /// <summary>
        ///     Retrieves the specified item from the cache
        /// </summary>
        /// <param name="key">The identifier for the cache item to retrieve</param>
        /// <returns>The cached item</returns>
        public static object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        #endregion

        #region Public Static Method - ListForSiteTools

        public static Dictionary<string, string> ListForSiteTools()
        {
            var list = new Dictionary<string, string>();
            var cacheEnumerater = HttpRuntime.Cache.GetEnumerator();

            while (cacheEnumerater.MoveNext())
            {
                if (!cacheEnumerater.Key.ToString().StartsWith("System.Web.Optimization.Bundle")
                    && !cacheEnumerater.Key.ToString().StartsWith("mini-profiler-")
                    && !cacheEnumerater.Key.ToString().StartsWith("__AppStartPage__"))
                {
                    list.Add(cacheEnumerater.Key.ToString(), cacheEnumerater.Value.ToString());
                }
            }

            return list;
        }

        #endregion

        //Private Static Methods

        #region Private Static Method - ClearCache

        /// <summary>
        ///     Clears the cache for the specified key, or clears the entire cache if key is blank
        /// </summary>
        /// <param name="keyStartsWith">The cache key to be cleared</param>
        /// <param name="notify"></param>
        private static void ClearCache(string keyStartsWith, bool notify = true)
        {
            lock (LockObject)
            {
                var cache = HttpRuntime.Cache;
                var cacheEnumerater = cache.GetEnumerator();

                if (string.IsNullOrEmpty(keyStartsWith))
                {
                    //empty key - clear everything from the cache
                    while (cacheEnumerater.MoveNext())
                    {
                        cache.Remove(cacheEnumerater.Key.ToString());
                    }
                }
                else
                {
                    //key given - clear an individual item
                    while (cacheEnumerater.MoveNext())
                    {
                        if (cacheEnumerater.Key.ToString().ToLower().StartsWith(keyStartsWith.ToLower()))
                        {
                            cache.Remove(cacheEnumerater.Key.ToString());
                        }
                    }
                }
            }
            if (notify)
            {
                NotifyServers(keyStartsWith);
            }
        }

        #endregion

        #region IHttpHandler Members

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            var keyStartsWith = context.Request.QueryString[QueryString.Key];
            var notify = Convert.ToBoolean(context.Request.QueryString[QueryString.Notify]);
            ClearCache(keyStartsWith, notify);
        }

        #endregion

        #region Public Static Method - Add (Overloaded for no expiry, sliding expiration and absolute expiration)

        /// <summary>
        ///     Add a cache entry with no expiry
        /// </summary>
        /// <param name="key">The cache key used to reference the item</param>
        /// <param name="value">The item to be added to the cache</param>
        public static object Add(string key, object value)
        {
            return HttpRuntime.Cache.Add(key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
                CacheItemPriority.Normal, null);
        }

        /// <summary>
        ///     Add a cache entry with a sliding expiration
        /// </summary>
        /// <param name="key">The cache key used to reference the item</param>
        /// <param name="value">The item to be added to the cache</param>
        /// <param name="slidingExpirationTimespan">
        ///     The interval between the time the added object was last accessed and the time
        ///     at which that object expires. If this value is the equivalent of 20 minutes, the object expires and is removed from
        ///     the cache 20 minutes after it is last accessed.
        /// </param>
        public static object Add(string key, object value, TimeSpan slidingExpirationTimespan)
        {
            return HttpRuntime.Cache.Add(key, value, null, Cache.NoAbsoluteExpiration, slidingExpirationTimespan,
                CacheItemPriority.Normal, null);
        }

        /// <summary>
        ///     Add a cache entry with an expiration datetime
        /// </summary>
        /// <param name="key">The cache key used to reference the item</param>
        /// <param name="value">The item to be added to the cache</param>
        /// <param name="absoluteExpirationDatetime">The time at which the added object expires and is removed from the cache. </param>
        public static object Add(string key, object value, DateTime absoluteExpirationDatetime)
        {
            return HttpRuntime.Cache.Add(key, value, null, absoluteExpirationDatetime, Cache.NoSlidingExpiration,
                CacheItemPriority.Normal, null);
        }

        #endregion
    }
}