// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PermissionRolesViewModel.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   The permission roles view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Web.Mvc;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The permission roles view model.
    /// </summary>
    public class PermissionRolesViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionRolesViewModel"/> class.
        /// </summary>
        public PermissionRolesViewModel()
        {
            this.DelimitedListPermissionsNotInRole = string.Empty;
            this.DelimitedListPermissionsInRole = string.Empty;
            this.DropDownSelectedRoleId = string.Empty;
            this.SelectedRoleId = string.Empty;
        }

        /// <summary>
        /// Gets or sets the selected role id.
        /// </summary>
        public string DropDownSelectedRoleId { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        [Display(Name = "Role to edit:")]
        public IEnumerable<System.Web.Mvc.SelectListItem> Roles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the changes should be saved on post back
        /// </summary>
        public bool IsSave { get; set; }

        /// <summary>
        /// Gets or sets the ID of the role being edited
        /// </summary>
        public int EditingItem { get; set; }

        /// <summary>
        /// Gets or sets a list comma separated of Permission Ids to save into the role
        /// </summary>
        public string DelimitedListPermissionsInRole { get; set; }

        /// <summary>
        /// Gets or sets a list of comma separated permission Ids as seen on the form at post back
        /// </summary>
        public string DelimitedListPermissionsNotInRole { get; set; }

        /// <summary>
        /// Gets or sets the selected role id.
        /// </summary>
        [Display(Name = "Role to edit:")]
        public string SelectedRoleId { get; set; }

        /// <summary>
        /// Gets or sets the role name.
        /// </summary>
        [Display(Name = "Edit role name if required:")]
        [LanguageRequired(ErrorMessage = "The role name is required")]
        [LanguageStringLength(100, ErrorMessage = "Role name should be less than 100 characters")]
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets a list of permissions in the selected role
        /// </summary>
        [LanguageDisplay("Permissions in selected role:")]
        public IEnumerable<System.Web.Mvc.SelectListItem> PermissionsInRole { get; set; }

        /// <summary>
        /// Gets or sets a list of permissions not in the selected role
        /// </summary>
        [LanguageDisplay("Available permissions:")]
        public IEnumerable<System.Web.Mvc.SelectListItem> PermissionsNotInRole { get; set; }

        /// <summary>
        /// Gets or sets the role description.
        /// </summary>
        [LanguageRequired(ErrorMessage = "A role description is required")]
        [LanguageDisplay("Role description:")]
        [LanguageStringLength(1000, ErrorMessage = "Role description should be less than 1000 characters")]
        public string RoleDescription { get; set; }

        /// <summary>
        /// Gets or sets the role user context.
        /// </summary>
        [LanguageDisplay("Role user context:")]
        public String RoleUserContextId { get; set; }

        public IEnumerable<SelectListItem> UserContexts { get; set; }
    }
}