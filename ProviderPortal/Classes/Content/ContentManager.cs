using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Classes
{
    public class ContentManager
    {
        public static readonly char[] InvalidCharacters = {'?', '&', '#', '"', '%', '&', '*', '|', '\\', ':', '<', '>', '.', '+'};

        public static void MapRoutes(RouteCollection routes)
        {
            foreach (var item in GetControllers())
            {
                var path = item.Name.Replace(typeof (Controller).Name, String.Empty);
                routes.MapRoute(
                    name: item.Name + "Help",
                    url: path + "/Help/{*id}",
                    defaults: new {controller = "SiteContent", action = "DisplayHelp", id = UrlParameter.Optional}
                    );
                routes.MapRoute(
                    name: item.Name,
                    url: path + "/{action}/{id}",
                    defaults: new {controller = path, action = "Index", id = UrlParameter.Optional}
                    );
            }
        }

        public static void MapCatchAll(RouteCollection routes)
        {
            routes.MapRoute(
                name: "SiteContentActions",
                url: "SiteContent/{action}/{*id}",
                defaults: new {controller = "SiteContent", action = "Edit", id = UrlParameter.Optional},
                constraints: new {action = @"Create|Edit|Delete|History|Archived"}
                );
            routes.MapRoute(
                name: "SiteContentDownload",
                url: "Content/{area}/{*id}",
                defaults: new { controller = "SiteContent", action = "Download", id = UrlParameter.Optional },
                constraints: new { area = @"Files|Thumbs" }
                );
            routes.MapRoute(
                name: "SiteContentDisplay",
                url: "{*id}",
                defaults: new {controller = "SiteContent", action = "Display", id = UrlParameter.Optional}
                );
        }

        public static string TrimPath(string path)
        {
            if (path == null) return null;
            path = path.Trim(new[] {'\r', '\n', '\t', ' ', '/', '\\'});
            return String.IsNullOrEmpty(path) ? null : path;
        }

        public static bool IsPathValid(string path)
        {
            var unescapedPath = TrimPath(path);
            if (unescapedPath == null) return false; 
            unescapedPath = Uri.UnescapeDataString(unescapedPath);
            if (InvalidCharacters.Any(c => unescapedPath.Contains(c)))
            {
                return false;
            }
            var parts = unescapedPath.Split('/');
            // Empty path, empty element, ., .. and trimmable element are invalid
            if (parts.Length == 0
                || parts.Any(x => x.Length == 0
                                  || x.Trim() != x
                                  || x == "." || x == ".."))
            {
                return false;
            }
            // Paths within a controller namespace are invalid
            var isAController =
                GetControllerNames().Any(x => x.Equals(parts[0], StringComparison.CurrentCultureIgnoreCase));
            // Unless they are {controller>/Help
            return !isAController ||
                   (parts.Length >= 2 && parts[1].Equals("Help", StringComparison.CurrentCultureIgnoreCase));
        }

        public static IEnumerable<Type> GetControllers()
        {
            var items = new List<Type>();
            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                items.AddRange(ass.GetTypes().Where(type => type.IsSubclassOf(typeof (Controller))));
            }
            return items;
        }

        public static IEnumerable<String> GetControllerNames()
        {
            var items = new List<String>();
            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                items.AddRange(
                    ass.GetTypes()
                        .Where(type => type.IsSubclassOf(typeof (Controller)))
                        .Select(type => type.Name.Replace(typeof (Controller).Name, String.Empty)));
            }
            return items;
        }

        /// <summary>
        /// Backend routine for saving, archiving, publishing and updating content availability new and existing content.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> HandleContentEdit(ProviderPortalEntities db, AddEditContentViewModel model)
        {
            var errors = new List<KeyValuePair<string, string>>();

            var invalidOperation = AppGlobal.Language.GetText("SiteContent_Edit_InvalidOperation",
                "That operation is invalid for the current content.");
            var notFound = AppGlobal.Language.GetText("SiteContent_Edit_NotFound",
                "This content is no longer available.");

            if (String.IsNullOrWhiteSpace(model.SubmitAction))
            {
                errors.Add(new KeyValuePair<string, string>("", invalidOperation));
                return errors;
            }

            // Save
            if (model.SubmitAction.Equals("Save", StringComparison.CurrentCultureIgnoreCase))
            {
                var content = model.ToEntity(db);
                if (content.ContentId == 0) db.Contents.Add(content);
                return errors;
            }

            var item = db.Contents.FirstOrDefault(x => x.ContentId == model.ContentId);
            if (item == null)
            {
                errors.Add(new KeyValuePair<string, string>("", notFound));
                return errors;
            }

            if (model.SubmitAction.Equals("UpdateAvailability", StringComparison.CurrentCultureIgnoreCase)
                && item.RecordStatusId == (int) Constants.RecordStatus.Live)
            {
                EnsureContextAvailability(db, model.ContentId, model.Path, model.UserContext);
                item.UserContext = (int) model.UserContext;
                item.ModifiedByUserId = Permission.GetCurrentUserId();
                item.ModifiedDateTimeUtc = DateTime.UtcNow;
                db.SaveChanges();
                return errors;  
            }

            if (model.SubmitAction.Equals("Publish", StringComparison.CurrentCultureIgnoreCase)
                && item.RecordStatusId != (int) Constants.RecordStatus.Live)
            {
                EnsureContextAvailability(db, model.ContentId, model.Path, model.UserContext);
                item.RecordStatusId = (int) Constants.RecordStatus.Live;
                item.ModifiedByUserId = Permission.GetCurrentUserId();
                item.ModifiedDateTimeUtc = DateTime.UtcNow;
                db.SaveChanges();
                return errors;
            }

            if (model.SubmitAction.Equals("Archive", StringComparison.CurrentCultureIgnoreCase)
                && item.RecordStatusId != (int) Constants.RecordStatus.Archived)
            {
                item.RecordStatusId = (int) Constants.RecordStatus.Archived;
                item.ModifiedByUserId = Permission.GetCurrentUserId();
                item.ModifiedDateTimeUtc = DateTime.UtcNow;
                db.SaveChanges();
                return errors;
            }

            errors.Add(new KeyValuePair<string, string>("", invalidOperation));
            return errors;
        }

        /// <summary>
        /// Go through all the published pages for the specified path and 
        /// archive the content or update its availability as appropriate.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="contentId">The content identifier.</param>
        /// <param name="path">The path.</param>
        /// <param name="context">The context.</param>
        private static void EnsureContextAvailability(ProviderPortalEntities db, int contentId, string path, UserContext.UserContextName context)
        {
            var pages =
                db.Contents.Where(
                    x =>
                        x.RecordStatusId == (int)Constants.RecordStatus.Live &&
                        x.Path.Equals(path, StringComparison.CurrentCultureIgnoreCase) &&
                        x.UserContext != (int)UserContext.UserContextName.None &&
                        x.ContentId != contentId);

            if (!pages.Any()) return;

            var userContext = (int) context;
            foreach (var page in pages)
            {
                // User context is a flag enumeration so we can use
                // bitwise arithmetic to manipulate the values.

                // Clear all bits that 
                var newContext = (userContext ^ page.UserContext) & page.UserContext;

                if (newContext == (int) UserContext.UserContextName.None)
                {
                    page.RecordStatusId = (int) Constants.RecordStatus.Archived;
                }
                else
                {
                    page.UserContext = newContext;
                }
            }
        }

    }
}
