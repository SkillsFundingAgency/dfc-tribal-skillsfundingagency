using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    /// <summary>
    /// Manage user session state in the application.
    /// </summary>
    public class SessionManager
    {
        /// <summary>
        /// Store of all objects that implement the <c ref="ISession"/> interface.
        /// </summary>
        private static IEnumerable<Type> _sessionHandlers;

        public static bool SessionHasTimedOut
        {
            get
            {
                return HttpContext.Current.Session[Constants.SessionFieldNames.SessionTimingOut] != null &&
                       (bool) HttpContext.Current.Session[Constants.SessionFieldNames.SessionTimingOut];
            }
            set { HttpContext.Current.Session[Constants.SessionFieldNames.SessionTimingOut] = value; }
        }

        /// <summary>
        /// Force a new session if the user authentication state or currently logged in user changes.
        /// </summary>
        public static void DetectNew()
        {
            var session = HttpContext.Current.Session;
            if (session == null) return;

            var request = HttpContext.Current.Request;
            // Invalidate session if the current authentication state has changed
            var isAuthenticated = session[Constants.SessionFieldNames.IsAuthenticated];
            if (isAuthenticated == null || (bool)isAuthenticated != request.IsAuthenticated)
            {
                session[Constants.SessionFieldNames.IsAuthenticated] = request.IsAuthenticated;
                session.Abandon();
                return;
            }
            if (!request.IsAuthenticated) return;

            // Invalidate the session if the current user has changed
            // this can occur if a logged in user logs in again as another user
            var isSameUser =
                (((string)session[Constants.SessionFieldNames.UserName] ?? String.Empty)).Equals(
                    HttpContext.Current.User.Identity.Name, StringComparison.CurrentCultureIgnoreCase);
            if (!isSameUser)
                session.Abandon();
        }

        /// <summary>
        /// Start a new session and call the Session_Start method on all objects implementing the <c ref="ISession"/> interface.
        /// </summary>
        public static void Start()
        {
            var session = HttpContext.Current.Session;
            var request = HttpContext.Current.Request;

            session[Constants.SessionFieldNames.IsAuthenticated] = request.IsAuthenticated;
            session[Constants.SessionFieldNames.SessionLastAccessDateTimeUtc] = DateTime.UtcNow;
            session[Constants.SessionFieldNames.SessionTimingOut] = false;

            InvokeInterfaceMember("Session_Start");
        }

        /// <summary>
        /// End a session and call the Session_End method on all objects implementing the <c ref="ISession"/> interface.
        /// </summary>
        public static void End()
        {
            InvokeInterfaceMember("Session_End");
        }

        /// <summary>
        /// Invoke the named method of the <c ref="ISession"/> interface.
        /// </summary>
        /// <param name="methodName"></param>
        private static void InvokeInterfaceMember(string methodName)
        {
            if (_sessionHandlers == null)
            {
                _sessionHandlers = from type in Assembly.GetExecutingAssembly().GetTypes()
                    where typeof (ISession).IsAssignableFrom(type)
                          && !type.IsAbstract
                    select type;
            }

            foreach (var type in _sessionHandlers)
            {
                var methodInfo = type.GetMethod(methodName);
                var classInstance = (ISession)Activator.CreateInstance(type, null);
                methodInfo.Invoke(classInstance, null);
            }
        }
    }
}