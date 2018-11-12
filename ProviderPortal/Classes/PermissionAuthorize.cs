// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PermissionAuthorize.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Checks the permission and allows access to the controller where the user has at least one permission in the supplied list
// </summary>
// --------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

    /// <summary>
    /// Checks the permission and allows access to the controller where the user has at least one permission in the supplied list
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PermissionAuthorize : AuthorizeAttribute
    {
        /// <summary>
        /// The permissions to be checked
        /// </summary>
        private readonly Permission.PermissionName[] permissions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionAuthorize"/> class. 
        /// Checks the permission and allows access to the controller where the user has at least one permission in the supplied list
        /// </summary>
        /// <param name="permissionRequired">
        /// The permission list, if the user has at least one permission from the list access is granted
        /// </param>
        public PermissionAuthorize(params Permission.PermissionName[] permissionRequired)
        {
            this.permissions = permissionRequired;
        }

        /// <summary>
        /// Overridden authorize
        /// </summary>
        /// <param name="httpContext">The HttpContext</param>
        /// <returns>True if has permissions, and false if no permissions</returns>
        /// <exception cref="System.Security.SecurityException">A System.Security.SecurityException is thrown if authorize fails</exception>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Permission.HasPermission(true, false, this.permissions);
        }
    }
}