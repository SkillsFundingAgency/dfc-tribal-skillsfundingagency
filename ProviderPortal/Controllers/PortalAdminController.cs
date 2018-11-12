// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalAdminController.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Defines the PortalAdminController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Web;
using CsvHelper;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Tribal.SkillsFundingAgency.ProviderPortal;
    using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
    using Tribal.SkillsFundingAgency.ProviderPortal.Models;

    /// <summary>
    /// The portal admin controller.
    /// </summary>
    [ContextAuthorize(UserContext.UserContextName.Administration)]
    public class PortalAdminController : BaseController
    {
        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Index()
        {
            return new HttpNotFoundResult();
        }

        #region Permissions and roles

        /// <summary>
        /// The permission roles.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [PermissionAuthorize(Tribal.SkillsFundingAgency.ProviderPortal.Permission.PermissionName.CanAddEditRoles)]
        public ActionResult PermissionRoles(string returnUrl)
        {
            PermissionRolesViewModel model = new PermissionRolesViewModel
            {
                Roles = this.GetRoles(),
                UserContexts = GetUserContexts()
            };
            model.PermissionsInRole = this.GetPermissionsInRole(model.DropDownSelectedRoleId ?? string.Empty);
            model.PermissionsNotInRole = this.GetPermissionsNotInRole(model.SelectedRoleId ?? string.Empty);

            ViewBag.ReturnUrl = returnUrl;
            return this.View(model);
        }

        /// <summary>
        /// The permission roles.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Tribal.SkillsFundingAgency.ProviderPortal.Permission.PermissionName.CanAddEditRoles)]
        public ActionResult PermissionRoles(
            [Bind(Exclude = "PermissionsNotInRole,PermissionsInRole")] PermissionRolesViewModel model)
        {
            if (model.IsSave)
            {
                Int32 roleUserContextId = 0;
                if (String.IsNullOrEmpty(model.RoleUserContextId))
                {
                    ModelState.AddModelError("RoleUserContextId", AppGlobal.Language.GetText(this, "RoleUserContextMandatory", "Role user context is mandatory"));
                }
                else if (!Int32.TryParse(model.RoleUserContextId, out roleUserContextId))
                {
                    ModelState.AddModelError("RoleUserContextId", AppGlobal.Language.GetText(this, "RoleUserContextMandatory", "Role user context is mandatory"));
                }
                else if (roleUserContextId != 1 && roleUserContextId != 2 && roleUserContextId != 4)
                {
                    ModelState.AddModelError("RoleUserContextId", AppGlobal.Language.GetText(this, "RoleUserContextInvalid", "Role user context is invalid"));
                }

                // If true have changes to save, if false post back is simply to select another role
                if (ModelState.IsValid)
                {
                    string newRoleId = Guid.NewGuid().ToString("D").ToUpper();
                    string languageFieldName = string.Concat("Account_RoleDescription_",
                        model.RoleName.Replace(" ", string.Empty));

                    // Is this a new role, if yes create a new role entry
                    if (model.DropDownSelectedRoleId.Equals("-1"))
                    {
                        db.AspNetRoles.Add(new AspNetRole
                        {
                            Id = newRoleId,
                            Name = model.RoleName,
                            Description = model.RoleDescription,
                            LanguageFieldName = languageFieldName,
                            // ReSharper disable once PossibleInvalidOperationException
                            UserContextId = roleUserContextId
                        });
                        db.SaveChanges();

                        ModelState.Remove("DropDownSelectedRoleId");
                        ModelState.Remove("SelectedRoleId");
                        model.DropDownSelectedRoleId = newRoleId;
                        model.SelectedRoleId = newRoleId;

                        // Audit change
                        AppGlobal.WriteAudit(
                            string.Format("User (id, name) '{0},{1}' added a new role called '{2}'", User.Identity.Name,
                                Tribal.SkillsFundingAgency.ProviderPortal.Permission.GetCurrentUserId(), model.RoleName),
                            true);
                    }

                    // Save the changes             
                    var role = db.AspNetRoles.FirstOrDefault(r => r.Id == model.DropDownSelectedRoleId);
                    var permissionList = role.Permissions.ToList();

                    // Has the role name been changed
                    if (!role.Name.Equals(model.RoleName, System.StringComparison.CurrentCulture))
                    {
                        // Role name has changed, save the new name
                        string currentRoleName = role.Name;
                        role.Name = model.RoleName;

                        // Update configuration settings to reflect the new name
                        Constants.ConfigSettings.RenameConfiguredRoles(currentRoleName, model.RoleName);

                        // Audit change
                        AppGlobal.WriteAudit(
                            string.Format("User (id, name) '{0},{1}' changed the role name from '{2}' to '{3}'",
                                User.Identity.Name,
                                Tribal.SkillsFundingAgency.ProviderPortal.Permission.GetCurrentUserId(), currentRoleName,
                                role.Name), true);
                    }

                    // Has the role description name been changed
                    if (!role.Description.Equals(model.RoleDescription, System.StringComparison.CurrentCulture))
                    {
                        // Role description has changed, save the new name
                        string currentRoleDescription = role.Description;
                        role.Description = model.RoleDescription;

                        // Audit change
                        AppGlobal.WriteAudit(
                            string.Format("User (id, name) '{0},{1}' changed the role description from '{2}' to '{3}'",
                                User.Identity.Name,
                                Tribal.SkillsFundingAgency.ProviderPortal.Permission.GetCurrentUserId(),
                                currentRoleDescription, role.Description), true);
                    }

                    // Has the role user context been changed?
                    if (role.UserContextId != roleUserContextId)
                    {
                        // Audit change
                        AppGlobal.WriteAudit(
                            string.Format("User (id, name) '{0},{1}' changed the role user context from '{2}' to '{3}'",
                                User.Identity.Name,
                                Tribal.SkillsFundingAgency.ProviderPortal.Permission.GetCurrentUserId(),
                                role.UserContextId, model.RoleUserContextId), true);

                        role.UserContextId = roleUserContextId;
                    }

                    // Drop all permissions in the role and just add back the ones passed back on the form
                    foreach (Tribal.SkillsFundingAgency.ProviderPortal.Entities.Permission permission in permissionList)
                    {
                        role.Permissions.Remove(permission);
                    }

                    // Now add back only the selected permissions
                    if (!string.IsNullOrWhiteSpace(model.DelimitedListPermissionsInRole))
                    {
                        string[] selectedPermissions = model.DelimitedListPermissionsInRole.Split('|');
                        foreach (string selectedPermission in selectedPermissions)
                        {
                            int permissionId;
                            if (int.TryParse(selectedPermission, out permissionId))
                            {
                                var permission = db.Permissions.FirstOrDefault(p => p.PermissionId == permissionId);
                                role.Permissions.Add(permission);
                            }
                        }
                    }

                    db.SaveChanges();
                    ShowGenericSavedMessage();

                    // Audit changes
                    AppGlobal.WriteAudit(
                        string.Format(
                            "User (id, name) '{0},{1}' saved the following permissions (id, name) '{2}' to role '{3}'",
                            Tribal.SkillsFundingAgency.ProviderPortal.Permission.GetCurrentUserId(), User.Identity.Name,
                            model.DelimitedListPermissionsInRole, role.Name), true);
                }
            }

            if (!model.IsSave || ModelState.IsValid)
            {
                // Select the new role and display the permissions
                var selectedRole = db.AspNetRoles.FirstOrDefault(r => r.Id == model.SelectedRoleId);
                ModelState.Remove("DropDownSelectedRoleId");
                ModelState.Remove("RoleName");
                ModelState.Remove("RoleDescription");
                ModelState.Remove("RoleUserContextId");
                model.RoleName = selectedRole == null ? string.Empty : selectedRole.Name;
                model.RoleDescription = selectedRole == null ? string.Empty : selectedRole.Description;
                model.PermissionsInRole = this.GetPermissionsInRole(model.SelectedRoleId);
                model.PermissionsNotInRole = this.GetPermissionsNotInRole(model.SelectedRoleId);
                model.DropDownSelectedRoleId = model.SelectedRoleId;
                model.RoleUserContextId = selectedRole == null ? String.Empty : selectedRole.UserContextId.ToString();
            }
            else
            {
                // Persist the view as returning to page the same as received as validation errors
                model.PermissionsInRole = this.GetPermissionListFromViewState(model.DelimitedListPermissionsInRole);
                model.PermissionsNotInRole = this.GetPermissionListFromViewState(model.DelimitedListPermissionsNotInRole);
            }

            model.PermissionsInRole = model.PermissionsInRole.OrderBy(x => x.Text);
            model.PermissionsNotInRole = model.PermissionsNotInRole.OrderBy(x => x.Text);

            model.Roles = this.GetRoles();

            model.UserContexts = GetUserContexts();

            // Back to the page with the results saved
            return this.View(model);
        }

        [NonAction]
        private static IEnumerable<SelectListItem> GetUserContexts()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = AppGlobal.Language.GetText("PermissionRolesViewModel_userContextRoles_Provider", "Provider") },
                new SelectListItem { Value = "2", Text = AppGlobal.Language.GetText("PermissionRolesViewModel_userContextRoles_Organisation", "Organisation") },
                new SelectListItem { Value = "4", Text = AppGlobal.Language.GetText("PermissionRolesViewModel_userContextRoles_Administrator", "Administrator") }
            };
        }

        #endregion

        #region Cache

        [PermissionAuthorize(new[] {ProviderPortal.Permission.PermissionName.CanManageCache})]
        //TODO [ValidateAntiForgeryToken]
        public ActionResult Cache()
        {
            var model = new CacheViewModel();

            if (Request.HttpMethod == "POST")
            {

                if (!string.IsNullOrEmpty(Request.Form["btnFlushCache"]))
                {
                    //flush the entire cache
                    CacheManagement.CacheHandler.Invalidate(null);
                    model.Message = AppGlobal.Language.GetText(this, "CacheFlushed", "Cache Flushed");
                }
                else if (!string.IsNullOrEmpty(Request.Form["btnReloadLanguages"]))
                {
                    //flush cache of all languages
                    AppGlobal.Language.ReloadLanguage(0);
                    model.Message = AppGlobal.Language.GetText(this, "LanguagesFlushed", "Languages Flushed");
                }
                else if (!string.IsNullOrEmpty(Request.Form["btnReloadConfiguration"]))
                {
                    //flush cache of all configuration settings
                    Constants.ConfigSettings.Refresh();
                    model.Message = AppGlobal.Language.GetText(this, "ConfigurationReloaded",
                        "Configuration settings reloaded");
                }
            }

            model.CacheItems = CacheManagement.CacheHandler.ListForSiteTools();
            var memAvailable = AppGlobal.Language.GetText(this, "CacheMemoryAvailable",
                "Physical memory available for caching: {0}% ({1} MB)");
            model.CacheMemoryFree = string.Format(memAvailable,
                HttpRuntime.Cache.EffectivePercentagePhysicalMemoryLimit.ToString(),
                (HttpRuntime.Cache.EffectivePrivateBytesLimit/1024)/1024);

            return View(model);
        }

        #endregion

        #region Configuration settings

        [PermissionAuthorize(new[] {ProviderPortal.Permission.PermissionName.CanManageConfiguration})]
        public ActionResult Configuration()
        {
            var model = new ConfigurationViewModel();
            model.Settings =
                Constants.ConfigSettings.Where(x => x.Key != "AllowSelfRegistration")
                    .Select(x => x.Value)
                    .OrderBy(x => x.Name)
                    .ToList();
            return View(model);
        }

        [PermissionAuthorize(new[] {ProviderPortal.Permission.PermissionName.CanManageConfiguration})]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Configuration(ConfigurationViewModel model, FormCollection collection)
        {
            foreach (String formItem in collection)
            {
                if (formItem.EndsWith(".DataType"))
                {
                    switch (collection[formItem].ToLower())
                    {
                        case "system.decimal":
                            String dValue = collection[formItem.Replace(".DataType", ".Value")];
                            if (!Decimal.TryParse(dValue, out Decimal dResult))
                            {
                                String name = formItem.Replace(".DataType", ".Value");
                                String fullName = collection[formItem.Replace(".DataType", ".Name")];
                                ModelState.AddModelError(name, String.Format(AppGlobal.Language.GetText(this, "InvalidValueForX", "Invalid value for {0}."), fullName));
                            }
                            break;

                        case "system.int":
                        case "system.int32":
                            String iValue = collection[formItem.Replace(".DataType", ".Value")];
                            if (!Int32.TryParse(iValue, out Int32 iResult))
                            {
                                String name = formItem.Replace(".DataType", ".Value");
                                String fullName = collection[formItem.Replace(".DataType", ".Name")];
                                ModelState.AddModelError(name, String.Format(AppGlobal.Language.GetText(this, "InvalidValueForX", "Invalid value for {0}."), fullName));
                            }
                            break;
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var oldSettings = db.ConfigurationSettings;
                bool reloadSettings = false;
                for (var i = 0; i < oldSettings.Count(); i++)
                {
                    var name = collection["Settings[" + i + "].Name"];
                    if (name != null && name.ToLower() != "allowselfregistration")
                    {
                        var value = collection["Settings[" + i + "].Value"] ?? String.Empty;
                        var oldSetting = oldSettings.FirstOrDefault(x => x.Name == name);
                        if (oldSetting != null && !value.Equals(oldSetting.Value))
                        {
                            if (oldSetting.DataType == "System.Boolean" && value == "true,false")
                            {
                                value = "true";
                            }
                            oldSetting.Value = value;
                            reloadSettings = true;
                        }
                    }
                }
                if (reloadSettings)
                {
                    db.SaveChanges();
                    Constants.ConfigSettings.Refresh();
                }
                ShowGenericSavedMessage();
            }

            model.Settings =
                Constants.ConfigSettings.Where(x => x.Key != "AllowSelfRegistration")
                    .Select(x => x.Value)
                    .OrderBy(x => x.Name)
                    .ToList();
            return View(model);
        }

        #endregion

        #region Language

        [PermissionAuthorize(new[] {ProviderPortal.Permission.PermissionName.CanManageLanguages})]
        public ActionResult Language()
        {
            var language = new LanguageManager(db);
            var model = new LanguageResourcesViewModel();
            model.DownloadLanguageOptions = language.GetLanguageSelectList(model.DownloadLanguageId);
            model.UploadLanguageOptions = language.GetLanguageSelectList(model.UploadLanguageId);
            return View(model);
        }

        [PermissionAuthorize(new[] {ProviderPortal.Permission.PermissionName.CanManageLanguages})]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Language(LanguageResourcesViewModel model)
        {
            var language = new LanguageManager(db);

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Request.Form["btnCreate"]))
                {
                    var languages = language.GetLanguages();
                    if (string.IsNullOrWhiteSpace(model.NewLanguageName))
                    {
                        ModelState.AddModelError("NewLanguageName",
                            AppGlobal.Language.GetText(this, "NameRequred", "This field is required."));
                    }
                    if (languages.Any(
                        x => x.DefaultText.Equals(model.NewLanguageName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        ModelState.AddModelError("NewLanguageName",
                            AppGlobal.Language.GetText(this, "IETFInUse", "Name in use."));
                    }
                    if (string.IsNullOrWhiteSpace(model.NewLanguageIETF))
                    {
                        ModelState.AddModelError("NewLanguageIETF",
                            AppGlobal.Language.GetText(this, "IETFRequred", "This field is required."));
                    }
                    if (languages.Any(
                        x => x.IETF.Equals(model.NewLanguageIETF, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        ModelState.AddModelError("NewLanguageIETF",
                            AppGlobal.Language.GetText(this, "IETFInUse", "IETF code in use."));
                    }

                    model.NewLanguageSuccess = ModelState.IsValid;
                    if (ModelState.IsValid)
                    {

                        if (!string.IsNullOrEmpty(model.NewLanguageName) &&
                            !string.IsNullOrEmpty(model.NewLanguageIETF))
                        {
                            model.NewLanguageSuccess = language.CreateNewLanguage(model.NewLanguageName,
                                model.NewLanguageIETF);
                            if (model.NewLanguageSuccess)
                            {
                                model.NewLanguageMessage = AppGlobal.Language.GetText(this, "NewLanguageSuccess",
                                    "New language created successfully");
                            }
                            else
                            {
                                model.NewLanguageMessage = AppGlobal.Language.GetText(this, "NewLanguageFailed",
                                    "New language failed");
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(Request.Form["btnDownload"]))
                {
                    var data = language.GenerateCsvLanguageFileBytes(model.DownloadLanguageId);
                    return File(data.Bytes, "text/csv", data.FileName);
                }
                else if (!string.IsNullOrEmpty(Request.Form["btnUpload"]))
                {
                    if (model.FileUpload != null)
                    {
                        var extension = Path.GetExtension(model.FileUpload.FileName);
                        if (extension != null && extension.ToLower() == ".csv")
                        {
                            try
                            {
                                model.UploadSuccess = language.ProcessUploadedCsvLanguageFile(model.UploadLanguageId,
                                    model.FileUpload.InputStream);
                                if (model.UploadSuccess)
                                {
                                    model.UploadMessage = AppGlobal.Language.GetText(this, "LanguageUpdated",
                                        "Language updated");
                                }
                                else
                                {
                                    model.UploadSuccess = false;
                                    model.UploadMessage = AppGlobal.Language.GetText(this, "LanguageUpdateFailed",
                                        "Language update failed");
                                }
                            }
                            catch (CsvMissingFieldException e)
                            {
                                var field = e.Message.Substring(e.Message.IndexOf('\'') + 1);
                                field = field.Substring(0, field.IndexOf('\''));

                                model.UploadSuccess = false;
                                model.UploadMessage = String.Format(AppGlobal.Language.GetText(this, "MissingField",
                                    "Required field '{0}' is missing from the CSV file"), field);
                            }
                        }
                        else
                        {
                            model.UploadSuccess = false;
                            model.UploadMessage = AppGlobal.Language.GetText(this, "InvalidLanguageFile",
                                "Invalid language file");
                        }
                    }
                    else
                    {
                        model.UploadSuccess = false;
                        model.UploadMessage = AppGlobal.Language.GetText(this, "NoLanguageFile", "No language file");
                    }
                }
            }
            model.DownloadLanguageOptions = language.GetLanguageSelectList(model.DownloadLanguageId);
            model.UploadLanguageOptions = language.GetLanguageSelectList(model.UploadLanguageId);

            return View(model);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// The get permission list from view state, this recreates the items passed back in the hidden fields
        /// </summary>
        /// <param name="viewState">
        /// The view state from the hidden field
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/> List of SelectItems.
        /// </returns>
        private
            IEnumerable<SelectListItem> GetPermissionListFromViewState(string viewState)
        {
            var selectListItems = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(viewState))
            {
                string[] values = viewState.Split('|');
                for (int index = 0; index < values.Length; index += 2)
                {
                    var item = new SelectListItem();
                    item.Value = values[index];
                    item.Text = values[index + 1];
                    selectListItems.Add(item);
                }
            }

            return new SelectList(selectListItems, "Value", "Text");
        }

        /// <summary>
        /// Gets a select list for the roles
        /// </summary>
        /// <returns>
        /// The select list
        /// </returns>
        private IEnumerable<SelectListItem> GetRoles()
        {
            var roleList = from roles in db.AspNetRoles
                orderby roles.Name
                select new {RoleId = roles.Id, RoleName = roles.Name};

            var roleSelectList = roleList.Select(x => new SelectListItem {Value = x.RoleId, Text = x.RoleName});
            var selectList = new SelectList(roleSelectList, "Value", "Text").ToList();
            selectList.Insert(0,
                new SelectListItem
                {
                    Text = AppGlobal.Language.GetText(this, "SelectAddNewRole", "Add new role..."),
                    Value = "-1"
                });
            return selectList;
        }

        /// <summary>
        /// Gets a list of permissions in a role
        /// </summary>
        /// <param name="roleId">
        /// The role ID.
        /// </param>
        /// <returns>
        /// A select list of permissions in the role
        /// </returns>
        private IEnumerable<SelectListItem> GetPermissionsInRole(string roleId)
        {
            var permissionList = db.AspNetRoles.FirstOrDefault(r => r.Id == roleId);
            if (permissionList != null)
            {
                return
                    permissionList.Permissions.Select(
                        x => new SelectListItem {Value = x.PermissionId.ToString(), Text = x.PermissionName})
                        .OrderBy(x => x.Text);
            }

            return new List<SelectListItem>();
        }

        /// <summary>
        /// The get permissions not in role.
        /// </summary>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private IEnumerable<SelectListItem> GetPermissionsNotInRole(string roleId)
        {
            var role = db.AspNetRoles.FirstOrDefault(r => r.Id == roleId);

            if (role != null)
            {
                var hasPermissions = role.Permissions.ToList();
                var permissionList = (from p in db.Permissions.ToList()
                    where (hasPermissions.All(hp => hp.PermissionId != p.PermissionId))
                    select p).ToList();

                return
                    permissionList.Select(
                        x =>
                            new SelectListItem
                            {
                                Value = x.PermissionId.ToString(CultureInfo.InvariantCulture),
                                Text = x.PermissionName
                            });
            }

            if (roleId == "-1")
            {
                // Add a new role which doesn't exist yet, so show all permissions as available to add
                var permissions = from p in db.Permissions.ToList() select p;
                return
                    permissions.Select(
                        x =>
                            new SelectListItem
                            {
                                Value = x.PermissionId.ToString(CultureInfo.InvariantCulture),
                                Text = x.PermissionName
                            });
            }

            return new List<SelectListItem>();
        }

        #endregion
    }
}