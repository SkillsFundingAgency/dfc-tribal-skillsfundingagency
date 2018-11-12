using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.Mvc.Html;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes.Content;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class ContentViewModelExtensions
    {
        public static readonly int PublishedVersion = 0;

        #region SiteContent/Index

        /// <summary>
        /// Gets a list of all live and pending items.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="db">The database.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static ContentListViewModel Populate(this ContentListViewModel model, ProviderPortalEntities db)
        {
            model = new ContentListViewModel
            {
                Items = new List<ContentListViewModelItem>(),
                DisplayMode = ContentListDisplayMode.Index
            };

            bool canManageContent = Permission.HasPermission(false, true, Permission.PermissionName.CanManageContent);
            if (!canManageContent) return model;

            model.Items = db.Contents
                .Where(
                    x =>
                        x.RecordStatusId == (int) Constants.RecordStatus.Live ||
                        x.RecordStatusId == (int) Constants.RecordStatus.Pending)
                .Select(x => new ContentListViewModelItem
                {
                    ContentId = x.ContentId,
                    Path = x.Path,
                    Title = x.Title,
                    UserContext = (UserContext.UserContextName) x.UserContext,
                    RecordStatus = (Constants.RecordStatus) x.RecordStatusId,
                    Embed = x.Embed,
                    LastModifiedDateTimeUtc = x.ModifiedDateTimeUtc ?? x.CreatedDateTimeUtc,
                    LastModifiedBy = x.AspNetUser1.Name ?? x.AspNetUser.Name,
                    Language = x.Language.DefaultText,
                    Version = x.Version,
                    Summary = x.Summary
                }).OrderBy(x => x.Path).ThenByDescending(x => x.Version).ToList();
            return model;
        }

        #endregion
        
        #region SiteContent / History

        /// <summary>
        /// Gets a list of all changes for a specified path.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="db">The database.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static ContentListViewModel Populate(this ContentListViewModel model, ProviderPortalEntities db,
            string path)
        {
            model = new ContentListViewModel
            {
                Items = new List<ContentListViewModelItem>(),
                DisplayMode = ContentListDisplayMode.History
            };

            bool canManageContent = Permission.HasPermission(false, true, Permission.PermissionName.CanManageContent);
            if (!canManageContent) return model;

            path = ContentManager.TrimPath(path);
            model.Items = db.Contents
                .Where(x => x.Path.Equals(path) && x.RecordStatusId != (int)Constants.RecordStatus.Deleted)
                .Select(x => new ContentListViewModelItem
                {
                    ContentId = x.ContentId,
                    Path = x.Path,
                    Title = x.Title,
                    UserContext = (UserContext.UserContextName) x.UserContext,
                    RecordStatus = (Constants.RecordStatus) x.RecordStatusId,
                    Embed = x.Embed,
                    LastModifiedDateTimeUtc = x.ModifiedDateTimeUtc ?? x.CreatedDateTimeUtc,
                    LastModifiedBy = x.AspNetUser1.Name ?? x.AspNetUser.Name,
                    Language = x.Language.DefaultText,
                    Version = x.Version,
                    Summary = x.Summary

                }).OrderByDescending(x => x.Version).ToList();
            return model;
        }

        #endregion

        #region SiteContent/Index

        /// <summary>
        /// Gets a list of all archived items.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="db">The database.</param>
        /// <returns></returns>
        public static ContentListViewModel PopulateArchived(this ContentListViewModel model, ProviderPortalEntities db)
        {
            model = new ContentListViewModel
            {
                Items = new List<ContentListViewModelItem>(),
                DisplayMode = ContentListDisplayMode.Index
            };

            bool canManageContent = Permission.HasPermission(false, true, Permission.PermissionName.CanManageContent);
            if (!canManageContent) return model;

            var notArchived = db.Contents
                .Where(
                    x =>
                        x.RecordStatusId == (int) Constants.RecordStatus.Live ||
                        x.RecordStatusId == (int) Constants.RecordStatus.Pending)
                .Select(x => x.Path.ToLower()).ToList();

            model.Items = db.Contents
                .Where(
                    x => !notArchived.Contains(x.Path.ToLower()))
                .Select(x => new ContentListViewModelItem
                {
                    ContentId = x.ContentId,
                    Path = x.Path,
                    Title = x.Title,
                    UserContext = (UserContext.UserContextName)x.UserContext,
                    RecordStatus = (Constants.RecordStatus)x.RecordStatusId,
                    Embed = x.Embed,
                    LastModifiedDateTimeUtc = x.ModifiedDateTimeUtc ?? x.CreatedDateTimeUtc,
                    LastModifiedBy = x.AspNetUser1.Name ?? x.AspNetUser.Name,
                    Language = x.Language.DefaultText,
                    Version = x.Version,
                    Summary = x.Summary
                }).OrderBy(x => x.Path).ThenByDescending(x => x.Version).ToList();
            return model;
        }

        #endregion
 

        #region SiteContent/Create

        public static AddEditContentViewModel PopulateAsNew(this AddEditContentViewModel model, string path,
            ProviderPortalEntities db)
       { 
            path = ContentManager.TrimPath(path);
            var availableContexts = GetAvailableContexts(path, db);
            model.Path = path;
            model.Title = (path ?? String.Empty).Replace('/', ' ');
            model.Embed = db.Contents.Any(x => x.Embed && x.Path.Equals(model.Path, StringComparison.CurrentCultureIgnoreCase));
            model.ContextsInUse = UserContext.UserContextName.All ^ availableContexts;
            model.UserContext = availableContexts;

            return model;
       } 

        public static AddEditContentViewModel Populate(this AddEditContentViewModel model, string path,
            UserContext.UserContextName contextName, int version, ProviderPortalEntities db, UserContext.UserContextInfo userContext)
        {
            var content = new ContentViewModel();
            model = content.Populate(path, contextName, version, db, userContext).Content;
            var availableContexts = GetAvailableContexts(path, db);
            model.ContextsInUse = UserContext.UserContextName.All ^ availableContexts;
            return model;
        }
        
        private static UserContext.UserContextName GetAvailableContexts(string path, ProviderPortalEntities db)
        {
            // Get a list of user contexts for published articles in this path
            var otherContexts = db.Contents.Where(x =>
                x.Path.Equals(path, StringComparison.CurrentCultureIgnoreCase)
                && x.RecordStatusId == (int) Constants.RecordStatus.Live)
                .Select(x => (UserContext.UserContextName) x.UserContext)
                .ToList();

            // Work out what other contexts the content is available in
            var availableContexts = UserContext.UserContextName.All;
            foreach (var item in otherContexts)
            {
                availableContexts ^= item;
            }
            return availableContexts;
        }

        #endregion

        #region AddEditContentViewModel (Display, Edit, Create)

        public static ContentViewModel Populate(this ContentViewModel model, string path,
            UserContext.UserContextName contextName, int version, ProviderPortalEntities db,
            UserContext.UserContextInfo userContext)
        {
            model = model.PopulateInner(path, contextName, version, db, userContext);
            
            // Safely embed if required to prevent infinite recursion
            if (model.SafeEmbed && model.Status != ContentStatus.ExistingPage)
            {
                return new ContentViewModel
                {
                    Status = ContentStatus.ExistingPage,
                    Content = new AddEditContentViewModel
                    {
                        Path = path,
                        Embed = true,
                        UserContext = userContext.ContextName
                    },
                    SafeEmbed = model.SafeEmbed
                };
            }

            return model;
        }
        
        private static ContentViewModel PopulateInner(this ContentViewModel model, string path,
            UserContext.UserContextName contextName, int version, ProviderPortalEntities db,
            UserContext.UserContextInfo userContext)
        {
            bool canManageContent = Permission.HasPermission(false, true, Permission.PermissionName.CanManageContent);
            version = canManageContent ? version : PublishedVersion;
            contextName = canManageContent && contextName != UserContext.UserContextName.None
                ? contextName
                : userContext.ContextName;

            if (String.IsNullOrWhiteSpace(path))
            {
                return new ContentViewModel {Content = null, Status = ContentStatus.NotFound};
            }
            
            var cachedVersion = version == PublishedVersion
                ? ContentCache.Get(path, contextName)
                : null;
            if (cachedVersion != null) return cachedVersion;

            var query = db.Contents.Where(x =>
                x.Path.Equals(path, StringComparison.CurrentCultureIgnoreCase))
                .AsQueryable();
            var content = version == PublishedVersion
                ? query.FirstOrDefault(x => x.RecordStatusId == (int)Constants.RecordStatus.Live && ((int)contextName & x.UserContext) != 0)
                : query.FirstOrDefault(x => x.Version == version);

            // No content exists for the current user context return an error
            if (content == null && !canManageContent)
            {
                return new ContentViewModel
                {
                    Content = null,
                    Status = query.Any() && !HttpContext.Current.Request.IsAuthenticated
                        ? ContentStatus.AuthenticationRequired
                        : ContentStatus.NotFound,
                    SafeEmbed = model.SafeEmbed
                };
            }

            var otherAvailableContexts = UserContext.UserContextName.None;
            if (canManageContent)
            {
                // Work out what other contexts the content is available in
                var availableContexts = query
                    .Where(x => x.RecordStatusId == (int) Constants.RecordStatus.Live)
                    .Select(x => new {x.ContentId, x.UserContext}).ToList();
                foreach (var item in availableContexts)
                {
                    if (content == null || content.ContentId != item.ContentId)
                    {
                        otherAvailableContexts |= (UserContext.UserContextName) item.UserContext;
                    }
                }
                otherAvailableContexts ^= content == null
                    ? UserContext.UserContextName.None
                    : (UserContext.UserContextName) content.UserContext;

                // The page doesn't exist, offer to create a new one
                if (content == null)
                {
                    var defaultContent = db.Contents.FirstOrDefault(x => x.Path == "DefaultContent") ?? new Content();
                    return new ContentViewModel
                    {
                        Content = new AddEditContentViewModel
                        {
                            Version = 1,
                            Path = path,
                            Title = defaultContent.Title,
                            Body = defaultContent.Body,
                            Scripts = defaultContent.Scripts,
                            Styles = defaultContent.Styles,
                            Summary = null,
                            UserContext = UserContext.UserContextName.All ^ otherAvailableContexts,
                            Embed = false,
                            RecordStatusId = (int) Constants.RecordStatus.Pending,
                            LanguageId = defaultContent.LanguageId,
                            ContextsInUse = otherAvailableContexts
                        },
                        Status = ContentStatus.NewPage,
                        SafeEmbed = model.SafeEmbed
                    };
                }
            }

            // Page exists and isn't new
            var result = new ContentViewModel
            {
                Content = new AddEditContentViewModel
                {
                    ContentId = content.ContentId,
                    Version = content.Version,
                    Path = content.Path,
                    Title = content.Title,
                    Body = content.Body,
                    Scripts = content.Scripts,
                    Styles = content.Styles,
                    Summary = null,
                    UserContext = (UserContext.UserContextName) content.UserContext,
                    Embed = content.Embed,
                    RecordStatusId = content.RecordStatusId,
                    LanguageId = content.LanguageId,
                    ContextsInUse = otherAvailableContexts
                },
                Status = ContentStatus.ExistingPage,
                SafeEmbed = model.SafeEmbed
            };

            if (result.Content.ContentId != 0 && version == PublishedVersion)
            {
                ContentCache.Add(result);
            }
            return result;
        }

        public static bool ContextInUse(this AddEditContentViewModel model, UserContext.UserContextName context)
        {
            return (model.ContextsInUse & context) != 0;
        }

        public static void Validate(this AddEditContentViewModel model, ProviderPortalEntities db,
            System.Web.Mvc.ModelStateDictionary modelState)
        {
            if (model.ContentId == 0)
            {
                if (!ContentManager.IsPathValid(model.Path))
                {
                    modelState.AddModelError("Path",
                        AppGlobal.Language.GetText("SiteContent_Error_InvalidPath",
                            "Please specify a valid path."));
                }
            }
            if (model.UserContext == UserContext.UserContextName.None)
            {
                modelState.AddModelError("UserContext",
                        AppGlobal.Language.GetText("SiteContent_Error_SelectAContext",
                            "Select one or more user contexts to make this content available in."));
            }
        }
        
        public static Content ToEntity(this AddEditContentViewModel model, ProviderPortalEntities db)
        {
            var currentInDb =
                db.Contents.FirstOrDefault(x => x.ContentId == model.ContentId);

            // Create if this doesn't already exist or it's not pending
            if (currentInDb == null || currentInDb.RecordStatusId != (int) Constants.RecordStatus.Pending)
            {
                var stats = db.Contents.Where(x => x.Path.Equals(model.Path, StringComparison.CurrentCultureIgnoreCase))
                    .Select(x => new {ContentId = x.ContentId, Version = x.Version, Embed = x.Embed});

                return new Content
                {
                    Version = stats.Any() ? stats.Max(x => x.Version) + 1 : 1,
                    Path = Uri.UnescapeDataString(model.Path),
                    Title = model.Title,
                    Body = model.Body,
                    Scripts = model.Scripts,
                    Styles = model.Styles,
                    Summary = model.Summary,
                    UserContext = (int) model.UserContext,
                    Embed = stats.Any(x => x.Embed),
                    RecordStatusId = (int) Constants.RecordStatus.Pending,
                    LanguageId = AppGlobal.Language.GetLanguageIdForThisRequest(),
                    CreatedByUserId = Permission.GetCurrentUserId(),
                    CreatedDateTimeUtc = DateTime.UtcNow,
                    ModifiedByUserId = Permission.GetCurrentUserId(),
                    ModifiedDateTimeUtc = DateTime.UtcNow,
                };
            }

            // It's pending so just return the existing
            currentInDb.Path = Uri.UnescapeDataString(model.Path);
            currentInDb.Title = model.Title;
            currentInDb.Body = model.Body;
            currentInDb.Scripts = model.Scripts;
            currentInDb.Styles = model.Styles;
            currentInDb.Summary = model.Summary;
            currentInDb.UserContext = (int) model.UserContext;
            //currentInDb.RecordStatusId = (int)Constants.RecordStatus.Pending;
            currentInDb.LanguageId = AppGlobal.Language.GetLanguageIdForThisRequest();
            currentInDb.CreatedByUserId = Permission.GetCurrentUserId();
            currentInDb.CreatedDateTimeUtc = DateTime.UtcNow;
            currentInDb.ModifiedByUserId = Permission.GetCurrentUserId();
            currentInDb.ModifiedDateTimeUtc = DateTime.UtcNow;

            return currentInDb;
        }

        #endregion
    }
}