using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using Tribal.SkillsFundingAgency.CacheManagement;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    /// <summary>
    ///     The register view model extensions.
    /// </summary>
    public static class RegisterViewModelExtensions
    {
        /// <summary>
        ///     Populate a register user view model.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <param name="db">
        ///     The db.
        /// </param>
        /// <returns>
        ///     The <see cref="RegisterViewModel" />.
        /// </returns>
        public static RegisterViewModel Populate(this RegisterViewModel model, ProviderPortalEntities db)
        {
            model.Address = model.Address ?? new AddressViewModel();
            model.Address.Populate(db);
            return model;
        }
    }

    /// <summary>
    ///     The add user view model extensions.
    /// </summary>
    public static class AddEditAccountViewModelExtensions
    {
        /// <summary>
        ///     The cache key.
        /// </summary>
        private const string UserTypesCacheKey = "AddUserViewModel:UserTypeSelectList";

        /// <summary>
        ///     The cache key.
        /// </summary>
        private const string RolesCacheKey = "AddUserViewModel:Roles:{0}:{1}";

        /// <summary>
        ///     The cache key.
        /// </summary>
        private const string AspNetRolesCacheKey = "AddUserViewModel:AspNetRoles:{0}:{1}";

        /// <summary>
        ///     Populate an add user view model.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <param name="db">
        ///     The db.
        /// </param>
        /// <param name="userId">
        ///     The user ID or email address.
        /// </param>
        /// <returns>
        ///     The <see cref="AddEditAccountViewModel" />.
        /// </returns>
        public static AddEditAccountViewModel Populate(this AddEditAccountViewModel model, ProviderPortalEntities db, string userId = null)
        {
            model.SetPermissionFlags();

            model.UserTypes = GetUserTypes(db);

            AspNetUser user = null;

            if (userId != null)
            {
                user = db.AspNetUsers.FirstOrDefault(x => x.Id == userId || x.Email == userId || x.UserName == userId);

                if (user == null)
                {
                    throw new ArgumentOutOfRangeException("User " + userId + " not found.");
                }

                model.Roles = GetRoles();
                model.AspNetRoles = GetAspNetRoles(db, model.Roles);
            }

            if (user != null)
            {
                model.UserId = user.Id;
                model.EditingSelf = model.UserId == Permission.GetCurrentUserId();
                model.Email = user.Email;
                model.RoleId = user.AspNetRoles.First().Name;
                model.UserTypeId = user.ProviderUserTypeId;
                model.UserType = user.ProviderUserType.ProviderUserTypeName;
                model.Name = user.Name;
                model.Address = user.Address == null ? new AddressViewModel() : new AddressViewModel(user.Address);
                model.PhoneNumber = user.PhoneNumber;
                model.IsDeleted = user.IsDeleted;
                model.IsSecureAccessUser = user.IsSecureAccessUser;

                if (user.Providers2.Any())
                {
                    Provider provider = user.Providers2.First();
                    model.ProviderId = "P" + provider.ProviderId;
                    model.Provider = provider.ProviderName;
                }
                else if (user.Organisations2.Any())
                {
                    Organisation organisation = user.Organisations2.First();
                    model.ProviderId = "O" + organisation.OrganisationId;
                    model.Organisation = organisation.OrganisationName;
                }
                else
                {
                    model.ProviderId = string.Empty;
                    model.Provider = string.Empty;
                }
            }
            else
            {
                model.RoleId = string.Empty;
                model.UserTypeId = 0;
                model.Roles = GetRoles();
                model.AspNetRoles = GetAspNetRoles(db, model.Roles);
            }

            model.Address = model.Address ?? new AddressViewModel();
            model.Address.Populate(db);
            return model;
        }

        /// <summary>
        ///     Get the configuration setting key that defines the user roles available in the current context.
        /// </summary>
        /// <returns>The configuration setting name.</returns>
        private static string GetContextRolesKey()
        {
            UserContext.UserContextName context = UserContext.GetUserContext().ContextName;
            string contextRolesKey = null;
            switch (context)
            {
                case UserContext.UserContextName.Provider:
                    contextRolesKey = "ProviderContextCanAddRoles";
                    break;
                case UserContext.UserContextName.Organisation:
                    contextRolesKey = "OrganisationContextCanAddRoles";
                    break;
                case UserContext.UserContextName.Administration:
                    contextRolesKey = "AdminContextCanAddRoles";
                    break;
            }
            return contextRolesKey;
        }

        /// <summary>
        ///     Get the configuration setting key that defines the user roles available to the current user.
        /// </summary>
        /// <returns>The configuration setting name.</returns>
        private static string GetUserTypeRolesKey()
        {
            string userTypeRolesKey = null;
            if (Permission.HasPermission(false, false, new[] {Permission.PermissionName.CanAddEditAdminUsers}))
            {
                userTypeRolesKey = "AdminUserCanAddRoles";
            }
            else if (Permission.HasPermission(false, false, new[] {Permission.PermissionName.CanAddEditOrganisationUsers}))
            {
                userTypeRolesKey = "OrganisationUserCanAddRoles";
            }
            else if (Permission.HasPermission(false, false, new[] {Permission.PermissionName.CanAddEditProviderUsers}))
            {
                userTypeRolesKey = "ProviderUserCanAddRoles";
            }
            return userTypeRolesKey;
        }

        public static AddEditAccountViewModel SetPermissionFlags(this AddEditAccountViewModel model)
        {
            model.EditingSelf = model.UserId == Permission.GetCurrentUserId();
            bool permission = !model.EditingSelf &&
                              UserContext.GetUserContext().ContextName == UserContext.UserContextName.Administration;
            model.CanEditProviderOrganisation = permission;
            model.CanEditRole = !model.EditingSelf;
            model.CanEditUserType = permission;

            return model;
        }

        /// <summary>
        ///     Validate an <see cref="AddEditAccountViewModel" />. Base validation is done via model annotations this does
        ///     additional sanity checks.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <param name="db">
        ///     The db.
        /// </param>
        /// <param name="state">
        ///     The state.
        /// </param>
        public static void Validate(this AddEditAccountViewModel model, ProviderPortalEntities db,
            ModelStateDictionary state)
        {
            model.SetPermissionFlags();

            if (model.CanEditRole && model.RoleId == null)
            {
                state.AddModelError("RoleId",
                    AppGlobal.Language.GetText("Account_Role_MustSpecifyRole", "The Role field is required"));
            }

            if (!model.EditingSelf && model.CanEditRole && model.RoleId != null &&
                GetRoles().All(x => x.Value != model.RoleId.ToString(CultureInfo.InvariantCulture)))
            {
                state.AddModelError("RoleId", AppGlobal.Language.GetText("Account_Role_InvalidRole", "Invalid role"));
            }

            if (model.CanEditUserType &&
                GetUserTypes(db).All(x => x.Value != model.UserTypeId.ToString(CultureInfo.InvariantCulture)))
            {
                state.AddModelError(
                    "UserTypeId",
                    AppGlobal.Language.GetText("Account_Role_InvalidUserType", "Invalid user type"));
            }

            var user = db.AspNetUsers.FirstOrDefault(x => x.Id == model.UserId);
            model.Email = String.IsNullOrEmpty(model.Email) ? String.Empty : model.Email.Trim();
            if (model.UserId != null && user == null)
            {
                state.AddModelError("Email",
                    AppGlobal.Language.GetText("Account_Role_AccountNotFound", "User account not found"));
            }
            else if (user == null || !model.Email.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase))
            {
                var inUse = db.AspNetUsers.Any(x => x.Email == model.Email);
                if (inUse)
                {
                    state.AddModelError("Email",
                        AppGlobal.Language.GetText("Account_Role_EmailInUse",
                            "That email address is already in use for another account"));
                }
            }

            if (model.CanEditProviderOrganisation)
            {
                var roleInContext = db.AspNetRoles.FirstOrDefault(x => x.Name.Equals(model.RoleId));
                if (roleInContext != null && String.IsNullOrWhiteSpace(model.ProviderId))
                {
                    var roleContext = (UserContext.UserContextName) roleInContext.UserContextId;
                    if (roleContext == UserContext.UserContextName.Provider)
                    {
                        state.AddModelError("Provider",
                            String.IsNullOrWhiteSpace(model.Provider)
                                ? AppGlobal.Language.GetText("Account_Provider_RequiredMessage", "Provider is required")
                                : AppGlobal.Language.GetText("Account_Provider_InvalidProvider", "Invalid provider"));
                    }
                    else if (roleContext == UserContext.UserContextName.Organisation)
                    {
                        state.AddModelError("Organisation",
                            String.IsNullOrWhiteSpace(model.Provider)
                                ? AppGlobal.Language.GetText("Account_Organisation_RequiredMessage",
                                    "Organisation is required")
                                : AppGlobal.Language.GetText("Account_OPrganisation_InvalidOrganisation",
                                    "Invalid organisation"));
                    }
                }
            }

            // SA users do not have addresses
            if (!model.IsSecureAccessUser)
            {
                model.Address.Validate(db, state);
            }
        }

        /// <summary>
        ///     Get the user types.
        /// </summary>
        /// <param name="db">
        ///     The db.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        private static List<SelectListItem> GetUserTypes(ProviderPortalEntities db)
        {
            var items = (List<SelectListItem>) CacheHandler.Get(UserTypesCacheKey);
            if (items == null)
            {
                items =
                    new SelectList(db.ProviderUserTypes.OrderBy(x => x.ProviderUserTypeId), "ProviderUserTypeId",
                        "ProviderUserTypeName").ToList();
                CacheHandler.Add(UserTypesCacheKey, items);
            }
            return items;
        }

        /// <summary>
        ///     Get the available AspNetRoles that can be created by the current user in the current context.
        /// </summary>
        /// <returns>The available roles.</returns>
        private static List<SelectListItem> GetRoles()
        {
            string contextRolesKey = GetContextRolesKey();

            string userTypeRolesKey = GetUserTypeRolesKey();            
            
            string cacheKey = String.Format(RolesCacheKey, contextRolesKey, userTypeRolesKey);

            var items = (List<SelectListItem>) CacheHandler.Get(cacheKey);
            if (items == null)
            {
                List<string> contextRoles = String.IsNullOrEmpty(contextRolesKey)
                    ? new List<string>()
                    : Constants.ConfigSettings[contextRolesKey].Value.ToString().Split(';').ToList();
                List<string> userTypeRoles = String.IsNullOrEmpty(userTypeRolesKey)
                    ? new List<string>()
                    : Constants.ConfigSettings[userTypeRolesKey].Value.ToString().Split(';').ToList();
                items = contextRoles.Intersect(userTypeRoles).OrderBy(x => x)
                    .Select(x => new SelectListItem
                    {
                        Text = x.Trim(),
                        Value = x.Trim()
                    }).ToList();
                if (items.Count() > 1)
                {
                    items.Insert(0, new SelectListItem
                    {
                        Value = string.Empty,
                        Text = AppGlobal.Language.GetText("Account_AddUser_SelectRole", "Select a role")
                    });
                }
                CacheHandler.Add(cacheKey, items);
            }
            return items;
        }

        /// <summary>
        ///     Get the user roles.
        /// </summary>
        /// <param name="db">
        ///     The db.
        /// </param>
        /// <param name="roles">
        ///     The roles.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        private static List<AspNetRole> GetAspNetRoles(ProviderPortalEntities db, IEnumerable<SelectListItem> roles)
        {
            string contextRolesKey = GetContextRolesKey();
            string userTypeRolesKey = GetUserTypeRolesKey();
            string cacheKey = String.Format(AspNetRolesCacheKey, contextRolesKey, userTypeRolesKey);

            var items = (List<AspNetRole>) CacheHandler.Get(cacheKey);
            if (items == null)
            {
                IEnumerable<string> roleNames = roles.Select(x => x.Value);
                items = db.AspNetRoles.Where(x => roleNames.Contains(x.Name)).OrderBy(x => x.Name).ToList();
                CacheHandler.Add(cacheKey, items);
            }
            return items;
        }
    }

    public static class 
        AccountSearchViewModelExtensions
    {
        /// <summary>
        ///     Populate an account search view model.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <param name="db">
        ///     The db.
        /// </param>
        /// <returns>
        ///     The <see cref="AddEditAccountViewModel" />.
        /// </returns>
        public static AccountSearchViewModel Populate(this AccountSearchViewModel model, ProviderPortalEntities db)
        {
            UserContext.UserContextInfo context = UserContext.GetUserContext();
            switch (context.ContextName)
            {
                case UserContext.UserContextName.Provider:

                    model.CanAdd = Permission.HasPermission(false, true,
                        Permission.PermissionName.CanAddEditProviderUsers);
                    break;

                case UserContext.UserContextName.Organisation:

                    model.CanAdd = Permission.HasPermission(false, true,
                        Permission.PermissionName.CanAddEditOrganisationUsers);
                    break;

                case UserContext.UserContextName.Administration:

                    model.CanAdd = Permission.HasPermission(false, true,
                        Permission.PermissionName.CanAddEditAdminUsers);
                    break;
            }

            model.CanEditSecureAccessUsers = Permission.HasPermission(false, true,
                Permission.PermissionName.CanEditSecureAccessUsers);
            return model;
        }
    }
}