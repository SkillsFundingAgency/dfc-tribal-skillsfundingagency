// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppGlobal.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Defines the AppGlobal type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Text.RegularExpressions;
using TribalTechnology.InformationManagement.Web.UI.Text;
// ReSharper disable once CheckNamespace


namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    using TribalTechnology.InformationManagement.EventLogger;

    /// <summary>
    /// Objects that are global to the site
    /// </summary>
    public static class AppGlobal
    {
        /// <summary>
        /// Initializes static members of the <see cref="AppGlobal"/> class.
        /// </summary>
        static AppGlobal()
        {
            Log = null;
            EmailQueue = null;
        }

        /// <summary>
        /// Gets or sets the email queue to add emails for immediate sending
        /// </summary>
        public static WebEmailSendQueue EmailQueue { get; set; }

        /// <summary>
        /// Gets or sets the log object for adding event logs
        /// </summary>
        public static DatabaseEventLog Log { get; set; }

        /// <summary>
        /// Gets or sets the language object
        /// </summary>
        public static LanguageResource Language { get; set; }

        /// <summary>
        /// Gets or sets the application version
        /// </summary>
        public static string Version { get; set; }

        /// <summary>
        /// Gets or sets the database object 
        /// </summary>
        public static TribalTechnology.InformationManagement.Data.SqlDatabase SqlDatabase { get; set; }
        
        /// <summary>
        /// Write an audit log
        /// </summary>
        /// <param name="auditText">The audit text</param>
        /// <param name="isAuditSuccess">True for a successful audit, false to write an audit failure</param>
        public static void WriteAudit(string auditText, bool isAuditSuccess)
        {
            var logType = DatabaseEventLog.EventLogType.AuditSuccess;
            if (!isAuditSuccess)
            {
                logType = DatabaseEventLog.EventLogType.AuditFailure;
            }

            Log.Write(auditText, SiteStart.GetHostIpAddress(), "Provider Portal Web Site", logType);
        }

        public static Boolean IsValidEmail(String email, Boolean isOptional = false)
        {
            if (isOptional && string.IsNullOrWhiteSpace(email))
            {
                return true;
            }
            return TestRegex(email, @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
        }

        /// <summary>
        /// Test a value against a regular expression, failing out if it takes longer than a second to evaluate
        /// </summary>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool TestRegex(string value, string expression)
        {
            try
            {
                var matchTimeout = TimeSpan.FromSeconds(1);
                var regex = new Regex(expression);
                return Regex.IsMatch(value.Trim(), regex.ToString(), RegexOptions.IgnoreCase, matchTimeout);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}