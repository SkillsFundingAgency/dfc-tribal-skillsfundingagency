using System;
using System.Linq;
using System.Web;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.SingleSignOn
{
    public class SingleSignOn : ISession
    {
        /// <summary>
        /// Called when a new session is created.
        /// </summary>
        public void Session_Start()
        {
            var isSecureAccessUser = false;
            var userId = Permission.GetCurrentUserId();
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                var db = new ProviderPortalEntities();
                var user = db.AspNetUsers.FirstOrDefault(x => x.Id == userId);
                isSecureAccessUser = user != null && user.IsSecureAccessUser;
            }
            HttpContext.Current.Session[Constants.SessionFieldNames.IsSecureAccessUser] = isSecureAccessUser;
        }

        /// <summary>
        /// Called when a session expires.
        /// </summary>
        public void Session_End()
        {
            if (HttpContext.Current == null)
                return;
            HttpContext.Current.Session[Constants.SessionFieldNames.IsSecureAccessUser] = false;
        }

        /// <summary>
        /// Determines whether the current user is a secure access user.
        /// </summary>
        /// <returns>True if the current session is for a secure access user.</returns>
        public static bool IsSecureAccessUser()
        {
            return HttpContext.Current.Session[Constants.SessionFieldNames.IsSecureAccessUser] != null &&
                   (bool)HttpContext.Current.Session[Constants.SessionFieldNames.IsSecureAccessUser];
        }
    }
}