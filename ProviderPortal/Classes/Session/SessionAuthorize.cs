// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionAuthorize.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Checks whether an authenticated user session has timed out.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.Owin.Security;

    /// <summary>
    /// Checks whether a user has timed out and manages expiration of their session.
    /// The default ASP.Net Identity session expiry cannot be used due to the different way SA
    /// user sessions time out.
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionAuthorize : AuthorizeAttribute
    {
        /// <summary>
        /// Overridden authorize
        /// </summary>
        /// <param name="httpContext">The HttpContext</param>
        /// <returns>True if is not logged in user or is one that has not timed out,
        /// and false if the session has timed out.</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Not logged in, pass-through
            if (!httpContext.Request.IsAuthenticated)
                return base.AuthorizeCore(httpContext);

            var session = HttpContext.Current.Session;

            // Somehow the time out session variable is missing, deny access
            if (session[Constants.SessionFieldNames.SessionLastAccessDateTimeUtc] == null)
                return false;

            var sessionValidMinutes = SingleSignOn.SingleSignOn.IsSecureAccessUser()
                ? Constants.ConfigSettings.SALoginValidPeriod
                : Constants.ConfigSettings.LoginValidPeriod;

            // User has exceeded the timeout period, deny access
            var lastAccess =
                (DateTime) HttpContext.Current.Session[Constants.SessionFieldNames.SessionLastAccessDateTimeUtc];
            var minutesSinceLastHit = (DateTime.UtcNow - lastAccess).TotalMinutes;
            if (minutesSinceLastHit >= sessionValidMinutes)
                return false;

            // Always slide the timer
            session[Constants.SessionFieldNames.SessionLastAccessDateTimeUtc] = DateTime.UtcNow;

            return true;
        }

        /// <summary>
        /// Log out the user and redirect them to the appropriate page.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var isSecureAccessUser = HttpContext.Current.Session[Constants.SessionFieldNames.IsSecureAccessUser] != null &&
                                     (bool) HttpContext.Current.Session[Constants.SessionFieldNames.IsSecureAccessUser];
            SessionManager.SessionHasTimedOut = true;
            var authenticationManager = filterContext.HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            SessionManager.End();

            if (isSecureAccessUser)
            {
                filterContext.Result = new RedirectResult(Constants.ConfigSettings.SATimedOutNotification);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"action", "LogIn"},
                        {"controller", "Account"}
                    });
            }
        }
    }
}