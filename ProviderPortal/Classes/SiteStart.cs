// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteStart.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Defines the SiteStart type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using Tribal.SkillsFundingAgency.ProviderPortal.Controllers;

// ReSharper disable once CheckNamespace


namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    using System;
    using System.Configuration;
    using System.Web;
    using TribalTechnology.InformationManagement.EventLogger;
    using TribalTechnology.InformationManagement.Web.UI.Text;

    /// <summary>
    /// Tasks that need to run on Application Start
    /// </summary>
    public static class SiteStart
    {
        /// <summary>
        /// Caches the host name.
        /// </summary>
        private static string cacheHostName = null;
        public static System.Threading.Timer larsTimer;
        public static System.Threading.Timer roATPAPITimer;

        #region Public Method - Initialise

        /// <summary>
        /// Run at site start up
        /// </summary>
        public static void Initialise()
        {
            // Fetch language debugging flag
            bool isLanguageDebugging = false;
            if (ConfigurationManager.AppSettings["LanguageInDeveloperMode"] != null)
            {
                bool.TryParse(ConfigurationManager.AppSettings["LanguageInDeveloperMode"], out isLanguageDebugging);
            }
            
            // Set up the Global event logging to the database, this has it's own connection and doesn't go via entity framework
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            AppGlobal.Log = new DatabaseEventLog(
                connectionString,
                "Provider portal",
                GetHostIpAddress(),
                "ASP.NET Website");
            AppGlobal.Log.StartAutoFlush(TimeSpan.FromMinutes(2), false);          

            // Make sure permissions in the enumerator are in the database, only run when debugging/developing.  New permissions should be scripted for deployment
            if (System.Web.HttpContext.Current.IsDebuggingEnabled)
            {
                Permission.AddMissingPermissions();
            }

            // Load configuration settings from the database
            Constants.ConfigSettings = new Tribal.SkillsFundingAgency.ProviderPortal.ConfigurationSettings();

            // Set up the database object to handle the language system
            AppGlobal.SqlDatabase = new TribalTechnology.InformationManagement.Data.SqlDatabase(connectionString, AppGlobal.Log, null);

            // Set up the languages
            AppGlobal.Language = new LanguageResource(AppGlobal.SqlDatabase, AppGlobal.Log)
            {
                KeyNamesIetf = (string)Constants.ConfigSettings["LanguageKeyNamesIetf"].Value,
                XxxxIetf = (string)Constants.ConfigSettings["LanguageXxxxIetf"].Value,
                IsDebugging = isLanguageDebugging && HttpContext.Current.IsDebuggingEnabled
            };

            // Set up the event log purge rules
            AppGlobal.Log.EventLoggingLevel = ParseEnum<DatabaseEventLog.EventLogLevel>((string)Constants.ConfigSettings["EventLoggingLevel"].Value, DatabaseEventLog.EventLogLevel.Informational);  // TODO get from settings
            AppGlobal.Log.MaximumEventsToBuffer = (int)Constants.ConfigSettings["EventLoggingBufferSize"].Value;
            AppGlobal.Log.AddPurgeRule(DatabaseEventLog.EventLogType.AuditFailure, TimeSpan.FromHours((int)Constants.ConfigSettings["EventLogPurgeAuditFailure"].Value));
            AppGlobal.Log.AddPurgeRule(DatabaseEventLog.EventLogType.AuditSuccess, TimeSpan.FromHours((int)Constants.ConfigSettings["EventLogPurgeAuditSuccess"].Value));
            AppGlobal.Log.AddPurgeRule(DatabaseEventLog.EventLogType.Debug, TimeSpan.FromHours((int)Constants.ConfigSettings["EventLogPurgeDebug"].Value));
            AppGlobal.Log.AddPurgeRule(DatabaseEventLog.EventLogType.Error, TimeSpan.FromHours((int)Constants.ConfigSettings["EventLogPurgeError"].Value));
            AppGlobal.Log.AddPurgeRule(DatabaseEventLog.EventLogType.Information, TimeSpan.FromHours((int)Constants.ConfigSettings["EventLogPurgeInformation"].Value));
            AppGlobal.Log.AddPurgeRule(DatabaseEventLog.EventLogType.Warning, TimeSpan.FromHours((int)Constants.ConfigSettings["EventLogPurgeWarning"].Value));
            AppGlobal.Log.IsPurgingEnabled = (bool)Constants.ConfigSettings["EventLogPurgingEnabled"].Value;

            // Set up the email queue
            AppGlobal.EmailQueue = new WebEmailSendQueue(
                Constants.ConfigSettings.SmtpServer,
                Constants.ConfigSettings.SmtpPort,
                Constants.ConfigSettings.SmtpIsSecure,
                Constants.ConfigSettings.SmtpUserName,
                Constants.ConfigSettings.SmtpPassword,
                Constants.ConfigSettings.AutomatedFromEmailAddress,
                AppGlobal.Log,
                Constants.ConfigSettings.SmtpIsRetryPolicyEnabled);
            AppGlobal.Log.WriteLog("Site started successfully");
            AppGlobal.Log.FlushAsync();

            // Set the application version
            Assembly ass = Assembly.GetExecutingAssembly();
            AppGlobal.Version = ass.GetName().Version.ToString();

            // Start timer to check for LARS file to download every minute
            // It will only try to download the file when it gets to the configured time
            AppGlobal.Log.WriteLog("Starting timer for Automated LARS import");
            larsTimer = new System.Threading.Timer(new System.Threading.TimerCallback(AutomationController.CheckLARSDownload), null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1));
            AppGlobal.Log.WriteLog("Automated LARS import timer started successfully");

            // Start timer to import RoATP API data every minute
            // It will only try to import the data when it gets to the configured time
            AppGlobal.Log.WriteLog("Starting timer for Automated RoATP data import");
            roATPAPITimer = new System.Threading.Timer(new System.Threading.TimerCallback(AutomationController.ImportRoATPData), null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1));
            AppGlobal.Log.WriteLog("Automated RoATP data import timer started successfully");
        }
        #endregion

        #region Private Method - GetHostIPAddress
        /// <summary>
        /// Gets the host IP address.
        /// </summary>
        /// <returns>
        /// The host name <see cref="string"/>.
        /// </returns>
        public static string GetHostIpAddress()
        {
            if (string.IsNullOrWhiteSpace(cacheHostName))
            {
                string hostName = System.Net.Dns.GetHostName();

                var hostIpEntry = System.Net.Dns.GetHostEntry(hostName);
                System.Net.IPAddress[] hostIpAddress = hostIpEntry.AddressList;

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (System.Net.IPAddress address in hostIpAddress)
                {
                    hostName += string.Concat(", ", address.ToString());
                }

                cacheHostName = hostName;
            }

            return cacheHostName;
        }
        #endregion

        #region Private Method - ParseEnum

        /// <summary>
        /// Parses an ENUM returning a default if the parse fails
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultParseFail">
        /// The default value when the parse fails.
        /// </param>
        /// <typeparam name="TEnum">The ENUM
        /// </typeparam>
        /// <returns>
        /// The <see cref="TEnum"/>.
        /// </returns>
        private static TEnum ParseEnum<TEnum>(string value, TEnum defaultParseFail) where TEnum : struct
        {
            TEnum parseEnum;
            if (Enum.TryParse(value, true, out parseEnum))
            {
                return parseEnum;
            }
            else
            {
                return defaultParseFail;
            }
        }

        #endregion
    }
}
