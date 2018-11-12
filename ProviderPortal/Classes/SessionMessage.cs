using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Classes
{
    public enum SessionMessageType
    {
        Success = 1,
        Info = 2, 
        Warning = 3,
        Danger = 4
    }

    public static class SessionMessage
    {
        public static readonly string SessionMessageTypeKey = "SessionMessageType";
        public static readonly string SessionMessageKey = "SessionMessage";
        public static readonly string SessionMessageStickyKey = "SessionMessageSticky";

        /// <summary>
        /// Shows a success message on the next page load.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Success(string message)
        {
            SetMessage(message, SessionMessageType.Success);
        }

        /// <summary>
        /// Shows an information message on the next page load.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Info(string message)
        {
            SetMessage(message, SessionMessageType.Info);
        }

        /// <summary>
        /// Shows a warning message on the next page load.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Warning(string message)
        {
            SetMessage(message, SessionMessageType.Warning);
        }

        /// <summary>
        /// Shows a failure message on the next page load.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Danger(string message)
        {
            SetMessage(message, SessionMessageType.Danger);
        }

        /// <summary>
        /// Shows a failure message on the next page load.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Failure(string message)
        {
            SetMessage(message, SessionMessageType.Danger);
        }

        /// <summary>
        /// Sets the message to display on the next page load.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageType">Type of the message.</param>
        public static void SetMessage(string message, SessionMessageType messageType)
        {
            SetMessage(message, messageType, 1);
        }

        /// <summary>
        /// Sets the message to display on the next page load.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="survivePageLoads">Number of pageloads for this message to survive.</param>
        public static void SetMessage(string message, SessionMessageType messageType, int survivePageLoads)
        {
            HttpContext.Current.Session[SessionMessageKey] = message;
            HttpContext.Current.Session[SessionMessageTypeKey] = messageType;
            HttpContext.Current.Session[SessionMessageStickyKey] = survivePageLoads;
        }

        /// <summary>
        /// Clears the current session message.
        /// </summary>
        public static void Clear()
        {
            if (!SurviveRedirect())
            {
                HttpContext.Current.Session.Remove(SessionMessageKey);
                HttpContext.Current.Session.Remove(SessionMessageTypeKey);
                HttpContext.Current.Session.Remove(SessionMessageStickyKey);
            }
        }

        /// <summary>
        /// Returns whether this message will survive the next redirect.
        /// </summary>
        /// <returns></returns>
        private static bool SurviveRedirect()
        {
            var sticky = (int)HttpContext.Current.Session[SessionMessageStickyKey];
            sticky --;
            HttpContext.Current.Session[SessionMessageStickyKey] = sticky;
            return sticky > 0;
        }
    }
}