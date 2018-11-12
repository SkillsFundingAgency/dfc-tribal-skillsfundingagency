// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PermissionAuthorize.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Checks the context and allows access to the controller where the user has at least one context in the supplied list
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Security.Policy;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;
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
    public class ContextAuthorize : AuthorizeAttribute
    {
        /// <summary>
        /// The authorized flag.
        /// </summary>
        private bool authorized;

        /// <summary>
        /// The permissions to be checked
        /// </summary>
        private readonly UserContext.UserContextName[] contexts;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextAuthorize"/> class. 
        /// Checks the user context and allows access to the controller where the user is in at least one context in the supplied list
        /// </summary>
        /// <param name="contextRequired">
        /// The permission list, if the user has at least one permission from the list access is granted
        /// </param>
        public ContextAuthorize(params UserContext.UserContextName[] contextRequired)
        {
            this.contexts = contextRequired;
        }

        /// <summary>
        /// Overridden authorize
        /// </summary>
        /// <param name="httpContext">The HttpContext</param>
        /// <returns>True if in the required context, and false if not in the required context</returns> <exception cref="System.Security.SecurityException">A System.Security.SecurityException is thrown if authorize fails</exception>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return (this.authorized = UserContext.HasContext(false, this.contexts));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //base.HandleUnauthorizedRequest(filterContext);

            if (!this.authorized)
            {
                filterContext.RequestContext.HttpContext.Items[UserContext.InvalidContextDetected] = true;
            }
        }
    }

    public class GoHomeOnInvalidUserContextAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if ((bool?)filterContext.RequestContext.HttpContext.Items[UserContext.InvalidContextDetected] == true)
            {
                // Temporarily disable this warning
                //if (!"Dashboard".Equals(filterContext.RouteData.Values["action"].ToString(),
                //    StringComparison.CurrentCultureIgnoreCase))
                //{
                //    // No error for the dashboard, just redirect
                //    filterContext.RequestContext.HttpContext.Session[UserContext.InvalidContextDetected] = true;
                //}

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary 
                { 
                    { "controller", "Home" }, 
                    { "action", "Index" } 
                });
            }
        }
    }

}