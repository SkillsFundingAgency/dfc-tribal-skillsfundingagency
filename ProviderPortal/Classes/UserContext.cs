using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security;
using System.Web;
using Antlr.Runtime.Tree;
using Microsoft.Ajax.Utilities;
using Tribal.SkillsFundingAgency.ProviderPortal.SingleSignOn;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public class UserContext : ISession
    {
        /// <summary>
        /// The invalid context detected field key.
        /// </summary>
        public const string InvalidContextDetected = "InvalidContextDetected";

        /// <summary>
        ///     The current user context.
        /// </summary>
        [Flags]
        public enum UserContextName
        {            
            /// <summary>
            ///     Pseudo user context to represent the null context.
            /// </summary>
            None = 0,

            /// <summary>
            ///     Authenticed user in the view provider context.
            /// </summary>
            Provider = 1,

            /// <summary>
            ///     Authenticated user in the view organisation context.
            /// </summary>
            Organisation = 2,

            /// <summary>
            ///     Authenticated user is currently in the administrator context.
            /// </summary>
            Administration = 4,

            /// <summary>
            ///     User account is not set up correctly and requires assistance.
            /// </summary>
            AuthenticatedNoAccess = 8,

            /// <summary>
            ///     User is unauthenticated.
            /// </summary>
            Unauthenticated = 16,

            /// <summary>
            ///     Authenticated user in the deleted provider context.
            /// </summary>
            DeletedProvider = 32,

            /// <summary>
            ///     Authenticated user in the deleted organisation context.
            /// </summary>
            DeletedOrganisation = 64,

            /// <summary>
            ///     Pseudo user context to flag items visible in all authenticated contexts.
            /// </summary>
            All = Provider | Organisation | Administration | Unauthenticated | AuthenticatedNoAccess | DeletedProvider | DeletedOrganisation,

            /// <summary>
            ///     Pseudo user context to flag items visible in all authenticated contexts.
            /// </summary>
            Authenticated = Provider | Organisation | Administration | AuthenticatedNoAccess | DeletedProvider | DeletedOrganisation,

            /// <summary>
            ///     Pseudo user context to flag items visible in both provider and organisation contexts.
            /// </summary>
            ProviderOrganisation = Provider | Organisation,

            /// <summary>
            ///     Pseudo user context to flag items visible in administration, provider and organisation contexts.
            /// </summary>
            AdministrationProviderOrganisation = Administration | Provider | Organisation,
            
            /// <summary>
            ///      Pseudo user context to flag items visible in administration and provider contexts.
            /// </summary>
            AdministrationProvider = Administration | Provider,
        }

        public static string ToEnglishList(UserContextName context)
        {
            var items = new List<string>();
            if (context == UserContextName.All)
            {
                return AppGlobal.Language.GetText("UserContext_Name_All", "All");
            }
            if (context == UserContextName.None)
            {
                return AppGlobal.Language.GetText("UserContext_Name_None", "None");
            }
            if ((context & UserContextName.Administration) != 0)
            {
                items.Add(AppGlobal.Language.GetText("UserContext_Name_Administration", "Administration"));
            }
            if ((context & UserContextName.Provider) != 0)
            {
                items.Add(AppGlobal.Language.GetText("UserContext_Name_Provider", "Provider"));
            }
            if ((context & UserContextName.Organisation) != 0)
            {
                items.Add(AppGlobal.Language.GetText("UserContext_Name_Organisation", "Organisation"));
            }
            if ((context & UserContextName.AuthenticatedNoAccess) != 0)
            {
                items.Add(AppGlobal.Language.GetText("UserContext_Name_AuthenticatedNoAccess",
                    "AuthenticatedNoAccess"));
            }
            if ((context & UserContextName.Unauthenticated) != 0)
            {
                items.Add(AppGlobal.Language.GetText("UserContext_Name_Unauthenticated", "Unauthenticated"));
            }
            if ((context & UserContextName.DeletedProvider) != 0)
            {
                items.Add(AppGlobal.Language.GetText("UserContext_Name_DeletedProvider", "DeletedProvider"));
            }
            if ((context & UserContextName.DeletedOrganisation) != 0)
            {
                items.Add(AppGlobal.Language.GetText("UserContext_Name_DeletedOrganisation", "DeletedOrganisation"));
            }
            return String.Join(", ", items);
        }

        public class UserContextException : Exception
        {
            public UserContextException()
            {
            }

            public UserContextException(string message)
                : base(message)
            {
            }

            public UserContextException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }

        /// <summary>
        ///     Set up the application user context for a new session.
        /// </summary>
        public void Session_Start()
        {
            InstantiateSession();
        }

        /// <summary>
        ///     Clear the current application user context.
        /// </summary>
        public void Session_End()
        {
            ClearSession();
        }

        /// <summary>
        ///     Get the current user context.
        /// </summary>
        /// <returns>The current <c ref="UserContextInfo" /></returns>
        public static UserContextInfo GetUserContext()
        {
            return HttpContext.Current.Session[Constants.SessionFieldNames.UserContext] == null
                ? InstantiateSession()
                : (UserContextInfo) HttpContext.Current.Session[Constants.SessionFieldNames.UserContext];
        }

        /// <summary>
        ///     Set up the application user context for a new session.
        /// </summary>
        /// <returns>The initial <c ref="UserContextInfo" /> for the current user.</returns>
        public static UserContextInfo InstantiateSession()
        {
            UserContextInfo details = GetDefaultContextForCurrentUser();
            SetSessionContext(details);
            return details;
        }

        /// <summary>
        ///     Clear the current application user context.
        /// </summary>
        public static void ClearSession()
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null) return;
            SetSessionContext(null);
        }

        /// <summary>
        /// Change the current user context.
        /// </summary>
        /// <param name="db">A valid <c ref="ProviderPortalentities"/> context.</param>
        /// <param name="context">The desired user context.</param>
        /// <param name="itemId">The related item ID.</param>
        /// <param name="force">When true, removes the record status check.</param>
        /// <returns>Whether the context change succeeded.</returns>
        public static bool SetUserContext(ProviderPortalEntities db, UserContextName context, int? itemId = null, bool force = false)
        {
            var currentContext = GetUserContext();
            if (currentContext.ContextName == context
                && currentContext.ItemId == itemId)
                return true;

            var currentUserId = Permission.GetCurrentUserId();
            bool canViewAdmin = Permission.HasPermission(false, true,
                Permission.PermissionName.CanViewAdministratorHomePage);

            bool success = false;
            switch (context)
            {
                case UserContextName.DeletedProvider:
                case UserContextName.Provider:

                    var provider = db.Providers.FirstOrDefault(x => x.ProviderId == itemId);
                    if (provider == null) break;
                    bool canViewProvider = Permission.HasPermission(false, true,
                        Permission.PermissionName.CanViewProviderHomePage);
                    // Success if an admin, associated provider user or organisation user with edit permission
                    success = canViewProvider
                              && (canViewAdmin
                                  || provider.AspNetUsers.Any(x => x.Id == currentUserId)
                                  || provider.OrganisationProviders.Any(
                                      x =>
                                          x.CanOrganisationEditProvider
                                          && x.IsAccepted
                                          && !x.IsRejected
                                          && x.Organisation.AspNetUsers.Any(y => y.Id == currentUserId)));
                    context = force
                        ? context
                        : provider.RecordStatusId != (int) Constants.RecordStatus.Live
                            ? UserContextName.DeletedProvider
                            : UserContextName.Provider;

                    break;

                case UserContextName.DeletedOrganisation:
                case UserContextName.Organisation:

                    var organisation = db.Organisations.FirstOrDefault(x => x.OrganisationId == itemId);
                    if (organisation == null) break;
                    bool canViewOrganisation = Permission.HasPermission(false, true,
                        Permission.PermissionName.CanViewOrganisationHomePage);
                    // Success if an admin or associated organisation user
                    success = canViewOrganisation
                              && (canViewAdmin || organisation.AspNetUsers.Any(x => x.Id == currentUserId));
                    context = force
                        ? context
                        : organisation.RecordStatusId != (int) Constants.RecordStatus.Live
                            ? UserContextName.DeletedOrganisation
                            : UserContextName.Organisation;
                    break;

                case UserContextName.Administration:

                    // Success if admin
                    success = canViewAdmin;
                    break;
            }

            if (success)
            {
                var newContext = new UserContextInfo(context, itemId);
                SetSessionContext(newContext);
            }

            return success;
        }

        /// <summary>
        /// Checks if the current authenticated user has the permissions requested
        /// </summary>
        /// <param name="throwNotInContextException">When true if no permission than an exception is thrown</param>
        /// <param name="requiredContext">The context or contexts to check the user has</param>
        /// <returns>Returns true if in a required context, false if not in a required context</returns>
        /// <exception cref="System.Security.SecurityException">Thrown if throwNotInContextException is true and context check is false</exception>
        public static bool HasContext(bool throwNotInContextException,
            params UserContext.UserContextName[] requiredContext)
        {
            var userContext = GetUserContext().ContextName;

            foreach (var context in requiredContext)
            {
                if (context.HasFlag(userContext))
                    return true;
            }

            // If reaching here no context match
            if (throwNotInContextException)
            {
                try
                {
                    string listOfRequestedContexts = string.Join(",",
                        requiredContext.Select(x => x.ToString()).ToArray());
                    string url = System.Web.HttpContext.Current.Request.Url.ToString();
                    AppGlobal.WriteAudit(
                        string.Format(
                            "An incorrect context exception of type UserContextException was thrown for user '{0}' who requested an action requiring user context '{1}' at page {2}",
                            System.Web.HttpContext.Current.User.Identity.Name, listOfRequestedContexts, url), false);
                }
                    // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                    // Failsafe, if the audit code throws an exception this catch ensures we always throw the correct not in correct context exception from here 
                }

                throw new UserContextException("Incorrect context for this action");
            }

            return false;
        }

        /// <summary>
        ///     Get the initial <c ref="UserContextInfo" /> for the current user.
        /// </summary>
        /// <returns>The initial <c ref="UserContextInfo" /> for the current user.</returns>
        public static UserContextInfo GetDefaultContextForCurrentUser()
        {
            // Not authenticated
            if (!HttpContext.Current.Request.IsAuthenticated)
                return new UserContextInfo(UserContextName.Unauthenticated);

            if (Permission.HasPermission(false, true, Permission.PermissionName.CanViewAdministratorHomePage))
                return new UserContextInfo(UserContextName.Administration);

            using (var db = new ProviderPortalEntities())
            {
                string userId = Permission.GetCurrentUserId();
                AspNetUser user = db.AspNetUsers.FirstOrDefault(x => x.Id == userId);
                if (user == null)
                    return new UserContextInfo(UserContextName.Unauthenticated);

                List<EntityInfo> organisations = user.Organisations2
                    .Select(x => new EntityInfo
                    {
                        EntityId = x.OrganisationId,
                        IsDeleted = (Constants.RecordStatus) x.RecordStatusId == Constants.RecordStatus.Deleted
                    }).ToList();
                List<EntityInfo> providers = user.Providers2
                    .Select(x => new EntityInfo
                    {
                        EntityId = x.ProviderId,
                        IsDeleted = (Constants.RecordStatus) x.RecordStatusId == Constants.RecordStatus.Deleted
                    }).ToList();

                // Provider and organisation users should only be associated with one entity
                // so choosing the first
                if (organisations.Any() &&
                    Permission.HasPermission(false, true, Permission.PermissionName.CanViewOrganisationHomePage))
                    return
                        new UserContextInfo(
                            organisations.First().IsDeleted
                                ? UserContextName.DeletedOrganisation
                                : UserContextName.Organisation, organisations.First().EntityId);

                if (providers.Any() &&
                    Permission.HasPermission(false, true, Permission.PermissionName.CanViewProviderHomePage))
                    return new UserContextInfo(
                        providers.First().IsDeleted
                            ? UserContextName.DeletedProvider
                            : UserContextName.Provider, providers.First().EntityId);

                return new UserContextInfo(UserContextName.AuthenticatedNoAccess);
            }
        }

        /// <summary>
        ///     Check whether the current user is allowed to back out of their current context.
        /// </summary>
        /// <returns>True if the user can switch to a previous context.</returns>
        public static bool CanGoBack()
        {
            return (Permission.HasPermission(false, true, Permission.PermissionName.CanViewAdministratorHomePage) &&
                    HasContext(false, new[] {UserContextName.ProviderOrganisation, UserContextName.DeletedProvider, UserContextName.DeletedOrganisation}))
                        ||
                   (Permission.HasPermission(false, true, Permission.PermissionName.CanViewOrganisationHomePage) &&
                    HasContext(false, UserContextName.Provider));
        }

        /// <summary>
        /// Sets the session context and any associated variables.
        /// </summary>
        /// <param name="context">The context.</param>
        private static void SetSessionContext(UserContextInfo context)
        {
            HttpContext.Current.Session[Constants.SessionFieldNames.UserContext] = context;
            // Set or clear session data related to quality
            QualityIndicator.SetSessionInformation(context);
        }



        public static bool ShowUserWizardOnLogin()
        {
            if (HttpContext.Current.Session[Constants.SessionFieldNames.ShowWizardOnLogin] != null)
            {
                //Show wizard check has already been done this session, no need to automatically display again
                return false;
            }

            HttpContext.Current.Session[Constants.SessionFieldNames.ShowWizardOnLogin] = true;
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string thisUser = System.Web.HttpContext.Current.User.Identity.Name;
                using (var databaseContext = new ProviderPortalEntities())
                {
                    var user = databaseContext.AspNetUsers.FirstOrDefault(u => u.UserName.Equals(thisUser));
                    if (user != null)
                    {
                        return user.ShowUserWizard ?? true;
                    }
                    return false;
                }
            }
            return false;
        }



        /// <summary>
        ///     Details of the current context the user is in.
        /// </summary>
        public class UserContextInfo
        {
            /// <summary>
            ///     Create a new instance of the <c ref="UserContextInfo" /> class.
            /// </summary>
            /// <param name="contextName">The <c ref="UserContextName" /></param>
            public UserContextInfo(UserContextName contextName) : this(contextName, null)
            {
            }

            /// <summary>
            ///     Create a new instance of the <c ref="UserContextInfo" /> class.
            /// </summary>
            /// <param name="contextName">The <c ref="UserContextName" /></param>
            /// <param name="itemId">The related entity ID.</param>
            public UserContextInfo(UserContextName contextName, int? itemId)
            {
                ContextName = contextName;
                ItemId = itemId;
            }

            /// <summary>
            ///     The current application context.
            /// </summary>
            public UserContextName ContextName { get; set; }

            /// <summary>
            ///     The entity ID related to the current context.
            /// </summary>
            public int? ItemId { get; set; }

            /// <summary>
            /// Determines whether this instance is provider.
            /// </summary>
            /// <returns></returns>
            public bool IsProvider()
            {
                return ContextName == UserContextName.Provider;
            }
            /// <summary>
            /// Determines whether [is deleted provider].
            /// </summary>
            /// <returns></returns>
            public bool IsDeletedProvider()
            {
                return ContextName == UserContextName.DeletedProvider;
            }
            /// <summary>
            /// Determines whether this instance is organisation.
            /// </summary>
            /// <returns></returns>
            public bool IsOrganisation()
            {
                return ContextName == UserContextName.Organisation;
            }
            /// <summary>
            /// Determines whether [is deleted organisation].
            /// </summary>
            /// <returns></returns>
            public bool IsDeletedOrganisation()
            {
                return ContextName == UserContextName.DeletedOrganisation;
            }
            /// <summary>
            /// Determines whether this instance is administration.
            /// </summary>
            /// <returns></returns>
            public bool IsAdministration()
            {
                return ContextName == UserContextName.Administration;
            }

            public bool IsAuthenticatedNoAccess()
            {
                return ContextName == UserContextName.AuthenticatedNoAccess;
            }
        }

        /// <summary>
        /// Temporary container for provider and organisation data.
        /// </summary>
        private class EntityInfo
        {
            /// <summary>
            /// Gets or sets the entity identifier.
            /// </summary>
            /// <value>
            /// The entity identifier.
            /// </value>
            public int EntityId { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is deleted.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is deleted; otherwise, <c>false</c>.
            /// </value>
            public bool IsDeleted { get; set; }
        }
    }
}