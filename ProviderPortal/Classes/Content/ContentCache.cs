using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Classes.Content
{   
    public static class ContentCache
    {
        private class CachedContentItem
        {
            public ContentViewModel ViewModel { get; set; }
        }

        public static ContentViewModel Get(string path, UserContext.UserContextName userContext)
        {
            var item = Load().FirstOrDefault(x => x.ViewModel.Content.Path.Equals(path, StringComparison.CurrentCultureIgnoreCase)
                && (x.ViewModel.Content.UserContext & userContext) != 0);
            return item == null ? null : item.ViewModel;
        }

        public static void Add(ContentViewModel newItem)
        {
            var cache = Load();
            cache.RemoveAll(x => x.ViewModel.Content.Path.Equals(newItem.Content.Path, StringComparison.CurrentCultureIgnoreCase)
                && (x.ViewModel.Content.UserContext & newItem.Content.UserContext) != 0);
            cache.Add(new CachedContentItem {ViewModel = newItem});
            Save(cache);
        }

        public static void Remove(int contentId)
        {
            var cache = Load();
            cache.RemoveAll(x => x.ViewModel.Content.ContentId == contentId);
            Save(cache);
        }

        public static void Remove(string path, UserContext.UserContextName userContext)
        {
            var cache = Load();
            cache.RemoveAll(x => x.ViewModel.Content.Path.Equals(path, StringComparison.CurrentCultureIgnoreCase)
                && (x.ViewModel.Content.UserContext & userContext) != 0);
            Save(cache);
        }

        private static List<CachedContentItem> Load()
        {
            var key = GetCacheKey();
            var cache = (List<CachedContentItem>)CacheManagement.CacheHandler.Get(key);
            return cache ?? new List<CachedContentItem>();
        }

        private static void Save(List<CachedContentItem> cache)
        {
            var key = GetCacheKey();
            CacheManagement.CacheHandler.Add(key, cache);
        }

        private static string GetCacheKey()
        {
            int languageId = AppGlobal.Language.GetLanguageIdForThisRequest();
            return String.Format("Cached_Content_|{0}|", languageId);
        }
    }
}