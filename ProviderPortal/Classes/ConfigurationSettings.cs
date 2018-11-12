// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Tribal" file="bnConfigurationSettings.cs">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//  Creates the configuration settings 
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    /// <summary>
    ///     Creates the configuration object
    /// </summary>
    public class ConfigurationSettings : Dictionary<string, ConfigurationSetting>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfigurationSettings" /> class.
        /// </summary>
        public ConfigurationSettings()
        {
            // Load the settings from the database on first creating
            LoadSettings();
        }

        /// <summary>
        ///     Gets the name of the application
        /// </summary>
        public string ApplicationName
        {
            get
            {
                ConfigurationSetting value;
                if (TryGetValue("ApplicationName", out value))
                {
                    return base["ApplicationName"].ToString();
                }
                return "Application name undefined";
            }
        }

        /// <summary>
        ///     Gets the short date format used on the site as set in the web config file, e.g. 31/01/2005 or 31-Jan-05
        /// </summary>
        public string ShortDateFormat
        {
            get { return base["ShortDateFormat"].ToString(); }
        }

        /// <summary>
        ///     Gets the short date and time format used on the site as set in the configuration settings e.g. 31/01/2005 23:30 or
        ///     31-Jan-05 11:30pm
        /// </summary>
        public string ShortDateTimeFormat
        {
            get { return base["ShortDateTimeFormat"].ToString(); }
        }

        /// <summary>
        ///     Gets the long date and time format used on the site as set in the configuration settings e.g. 31 January 2005 or
        ///     January 31, 2005
        /// </summary>
        public string LongDateFormat
        {
            get { return base["LongDateFormat"].ToString(); }
        }

        /// <summary>
        ///     Gets the long date and time format used on the site as set in the configuration settings  e.g. 31 January 2005
        ///     23:30 or January 31, 2005, 11:30pm
        /// </summary>
        public string LongDateTimeFormat
        {
            get { return base["LongDateTimeFormat"].ToString(); }
        }

        /// <summary>
        ///     Gets the database name
        /// </summary>
        public string DatabaseName
        {
            get { return base["DatabaseName"].ToString(); }
        }

        /// <summary>
        ///     Gets the number of items that are listed before they a split onto one or more pages
        /// </summary>
        public int ListPerPage
        {
            get { return (int) base["ListPerPage"].Value; }
        }

        /// <summary>
        ///     Gets a value indicating whether true if language selection is available to the user
        /// </summary>
        public bool LanguageAllowSelection
        {
            get { return (bool) base["LanguageAllowSelection"].Value; }
        }

        /// <summary>
        ///     Gets the site name
        /// </summary>
        public string SiteName
        {
            get { return (string) base["SiteName"].Value; }
        }

        /// <summary>
        ///     Gets the email address to use as the from email address for emails sent from the site
        /// </summary>
        public string AutomatedFromEmailAddress
        {
            get { return (string) base["AutomatedFromEmailAddress"].Value; }
        }

        /// <summary>
        ///     Gets the email from name to use as the from email from name for emails sent from the site
        /// </summary>
        public string AutomatedFromEmailName
        {
            get { return (string) base["AutomatedFromEmailName"].Value; }
        }

        /// <summary>
        ///     Gets whether the user is able at login to tick an option so they are remembered on future visits
        /// </summary>
        public bool AutoSiteLoginAllow
        {
            get { return (bool) base["AutoSiteLoginAllow"].Value; }
        }

        /// <summary>
        ///     Gets whether new users are allowed to register accounts on the site
        /// </summary>
        public bool AllowSelfRegistration
        {
            get { return (bool) base["AllowSelfRegistration"].Value; }
        }

        /// <summary>
        ///     Gets the SMTP server address
        /// </summary>
        public string SmtpServer
        {
            get { return (string) base["SMTPServer"].Value; }
        }

        /// <summary>
        ///     Gets the SMTP port
        /// </summary>
        public int SmtpPort
        {
            get { return (int) base["SMTPServerPort"].Value; }
        }

        /// <summary>
        ///     Gets the SMTP User name
        /// </summary>
        public string SmtpUserName
        {
            get { return (string) base["SMTPUserName"].Value; }
        }

        /// <summary>
        ///     Gets the SMTP password
        /// </summary>
        public string SmtpPassword
        {
            get { return (string) base["SMTPPassword"].Value; }
        }

        /// <summary>
        ///     Gets a value indicating whether secure sockets are used
        /// </summary>
        public bool SmtpIsSecure
        {
            get { return (bool) base["SMTPIsSecure"].Value; }
        }

        /// <summary>
        ///     Gets the Email Sender Username
        /// </summary>
        public string EmailSenderUsername
        {
            get { return (string)base["EmailSenderUsername"].Value; }
        }

        /// <summary>
        ///     Gets the EmailSender's EmailAddress  
        /// </summary>
        public string EmailSenderEmailAddress
        {
            get { return (string)base["EmailSenderEmailAddress"].Value; }
        }

        /// <summary>
        ///     Gets a value indicating whether EmailService is Enabled
        /// </summary>
        public bool EmailServiceEnabled
        {
            get { return (bool)base["EmailServiceEnabled"].Value; }
        }

        /// <summary>
        ///     Gets a value indicating whether a retry policy is enabled
        /// </summary>
        public bool SmtpIsRetryPolicyEnabled
        {
            get { return (bool) base["SMTPIsRetryPolicyEnabled"].Value; }
        }

        /// <summary>
        ///     Gets a list of email recipients to use when email override is enabled.
        /// </summary>
        public string[] EmailOverrideRecipients
        {
            get { return base["EmailOverrideRecipients"].ToString().Split(';'); }
        }

        /// <summary>
        ///     Gets whether email override is currently enabled.
        /// </summary>
        public bool EmailOverrideEnabled
        {
            get { return (bool) base["EmailOverrideEnabled"].Value; }
        }

        /// <summary>
        ///     Gets the domain which is exempt from email overriding.
        /// </summary>
        public string EmailOverrideSafeDomain
        {
            get { return (string) base["EmailOverrideSafeDomain"].Value; }
        }

        /// <summary>
        ///     Gets whether to always redirect users to the home page on log in.
        ///     When set the ReturnUrl parameter on the log in page has no effect.
        /// </summary>
        public bool LoginRedirectsToHomePage
        {
            get { return (bool) base["LoginRedirectsToHomePage"].Value; }
        }


        /// <summary>
        ///     Gets the name of virutal directory for saving bulk upload csv files.  This location may be mapped to local/ network
        ///     drive.
        /// </summary>
        public string BulkUploadVirtualDirectoryName
        {
            get { return base["VirtualDirectoryNameForStoringBulkUploadFiles"].ToString(); }
        }

        public int BulkUploadThresholdAcceptableLimit
        {
            get { return (int) base["BulkUploadThresholdAcceptablePercent"].Value; }
        }

        public int BulkUploadApprenticeshipThresholdAcceptableLimit
        {
            get
            {
                return (int)base["BulkUploadThresholdApprenticeshipAcceptablePercent"].Value;
            }
        }

        /// <summary>
        ///     Gets the name of virutal directory for saving LARS upload zip files.  This location may be mapped to local/ network
        ///     drive.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string LARSUploadVirtualDirectoryName
        {
            get { return base["VirtualDirectoryNameForStoringLARSFiles"].ToString(); }
        }

        /// <summary>
        ///     Gets the name of virutal directory for saving Code Point upload zip files.  This location may be mapped to local/ network
        ///     drive.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string CodePointUploadVirtualDirectoryName
        {
            get { return base["VirtualDirectoryNameForStoringCodePointFiles"].ToString(); }
        }
        
        /// <summary>
        ///     Gets the name of virutal directory for saving addressbase upload zip/csv files.  This location may be mapped to
        ///     local/ network drive.
        /// </summary>
        public string AddressBaseUploadVirtualDirectoryName
        {
            get { return base["VirtualDirectoryNameForStoringAddressBaseFiles"].ToString(); }
        }

        // ReSharper disable once InconsistentNaming
        public string UCASImportUploadVirtualDirectoryName
        {
            get { return base["VirtualDirectoryNameForStoringUCASImportFiles"].ToString(); }
        }

        /// <summary>
        /// Gets the email address to send the "Data Ready for QA" email to
        /// </summary>
        public string DataReadyForQAEmailAddress
        {
            get { return base["DataReadyForQAEmailAddress"].ToString(); }
        }

        /// <summary>
        /// Gets the email address to send the "Data Ready for QA" email to
        /// </summary>
        public string SubmitNewTextForQAAddress
        {
            get { return base["SubmitNewTextForQAAddress"].ToString(); }
        }  

        /// <summary>
        ///     Gets the name of virutal directory for saving addressbase upload zip/csv files.  This location may be mapped to
        ///     local/ network drive.
        /// </summary>
        public string ProviderImportFilesVirtualDirectoryName
        {
            get { return base["VirtualDirectoryNameForStoringProviderImportFiles"].ToString(); }
        }

        /// <summary>
        ///     Default system-wide setting for the maximum number of apprenticeship locations per provider
        /// </summary>
        public Int32 MaxLocations
        {
            get { return (Int32)base["MaxLocations"].Value; }
        }
        

        /// <summary>
        ///     Gets ths number of decimal places to use for percentages in the daily report
        /// </summary>
        public int DailyReportDecimalPlaces
        {
            get
            {
                if (ContainsKey("DailyReportDecimalPlaces"))
                {
                    int retVal;
                    int.TryParse(base["DailyReportDecimalPlaces"].ToString(), out retVal);
                    if (retVal > 5)
                    {
                        retVal = 5;
                    }
                    return retVal;
                }

                return 0;
            }
        }

        /// <summary>
        /// </summary>
        public string ShortDateFormatFileName
        {
            get { return base["ShortDateFormatFileName"].ToString(); }
        }

        /// <summary>
        ///     Gets the name of virutal directory for saving bulk upload csv files.  This location may be mapped to local/ network
        ///     drive.
        /// </summary>
        public string NightlyCsvFilesDirectoryLocation
        {
            get
            {
                return base["NightlyCsvFilesLocation"].ToString().EndsWith(@"\")
                    ? base["NightlyCsvFilesLocation"].ToString()
                    : string.Concat(base["NightlyCsvFilesLocation"].ToString(), @"\");
            }
        }

        /// <summary>
        ///     Gets the base part of the URL to use for constructing links to UsageStatistics.
        /// </summary>
        public string UsageStatisticsVirtualDirectory
        {
            get { return base["UsageStatisticsVirtualDirectory"].ToString(); }
        }

        /// <summary>
        ///     Gets the path to the usage statistics reports on the file system.
        /// </summary>
        public string UsageStatisticsFilesLocation
        {
            get { return base["UsageStatisticsFilesLocation"].ToString(); }
        }

        /// <summary>
        ///     Gets a semi-colon separated list of email addresses to send support team alerts to.
        /// </summary>
        public string[] SupportTeamEmailRecipients
        {
            get { return base["SupportTeamEmailRecipients"].ToString().Split(';'); }
        }

        /// <summary>
        ///     Gets the name to address support team alerts to
        /// </summary>
        public string SupportTeamEmailName
        {
            get { return base["SupportTeamEmailName"].ToString(); }
        }

        public int ProviderDashboardPdfRenderDelay
        {
            get { return (int) base["ProviderDashboardPdfRenderDelay"].Value; }
        }


        /// <summary>
        ///     The URI to post authentication requests to.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SADestination
        {
            get { return base["SADestination"].ToString(); }
        }

        /// <summary>
        ///     The URI of the page that receives the SAML2 response from the SSO server.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SAAssertionConsumerServiceUrl
        {
            get { return base["SAAssertionConsumerServiceUrl"].ToString(); }
        }

        /// <summary>
        ///     The URI of the landing page for SSO users.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SALandingPage
        {
            get { return base["SALandingPage"].ToString(); }
        }

        /// <summary>
        ///     The URI of the service identifier.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SAServiceProviderEntityId
        {
            get { return base["SAServiceProviderEntityId"].ToString(); }
        }

        /// <summary>
        ///     The location of the X.509 certificate for decoding SAML2 responses.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SAX509Certificate
        {
            get { return base["SAX509Certificate"].ToString(); }
        }

        /// <summary>
        ///     Whether SSO is enabled.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool SAEnabled
        {
            get { return Convert.ToBoolean(base["SAEnabled"].ToString()); }
        }

        /// <summary>
        ///     The URI of the page to show when a SSO user logs out.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SALoggedOutNotification
        {
            get { return base["SALoggedOutNotification"].ToString(); }
        }

        /// <summary>
        ///     The URI of the page to show when a SSO user times out.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SATimedOutNotification
        {
            get { return base["SATimedOutNotification"].ToString(); }
        }

        /// <summary>
        ///     The number of minutes the credentials are valid for before they timeout for inactivity meaning the user needs to
        ///     log in again.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int SALoginValidPeriod
        {
            get { return Convert.ToInt32(base["SALoginValidPeriod"].ToString()); }
        }

        /// <summary>
        ///     The URI of the page where a user can view and manage their account details.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SAUserAccountManagement
        {
            get { return base["SAUserAccountManagement"].ToString(); }
        }

        /// <summary>
        ///     The URI of the page where a user can manage their account password.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SAUserChangePassword
        {
            get { return base["SAUserChangePassword"].ToString(); }
        }

        /// <summary>
        ///     The role granted to the first user account at a Secure Access provider.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SAUserRolePrimary
        {
            get { return base["SAUserRolePrimary"].ToString(); }
        }

        /// <summary>
        ///     The role granted to any subsequent user accounts for a Secure Access provider.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SAUserRoleSecondary
        {
            get { return base["SAUserRoleSecondary"].ToString(); }
        }

        /// <summary>
        ///     The URI of the Secure Access home page.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SAHomePage
        {
            get { return base["SAHomePage"].ToString(); }
        }

        /// <summary>
        ///     The path on the server to the ClamWin executable
        /// </summary>
        public string VirusScanPath
        {
            get { return base["VirusScanPath"].ToString(); }
        }

        /// <summary>
        ///     The path on the server to the ClamWin virus definition db
        /// </summary>
        public string VirusScanDefinitionPath
        {
            get { return base["VirusScanDefinitionPath"].ToString(); }
        }

        /// <summary>
        ///     The configured virus scanner type
        /// </summary>
        public string VirusScanType
        {
            get { return base["VirusScanType"].ToString(); }
        }

        /// <value>
        ///     The number of minutes the credentials are valid for before they timeout for inactivity meaning the user needs to
        ///     log in again.  This figure only applies when a user has logged in without using the 'remember me' option.
        /// </value>
        public int LoginValidPeriod
        {
            get { return Convert.ToInt32(base["LoginValidPeriod"].ToString()); }
        }

        /// <summary>
        ///     When true this option requires all providers and organisations to have unique names.
        /// </summary>
        public bool RequireUniqueProvisionNames
        {
            get { return (bool) base["RequireUniqueProvisionNames"].Value; }
        }

        /// <summary>
        ///     When building data for export to the Apprenticeship API, do we sanity check the scale of the changes since the last
        ///     export.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool DASExportThresholdChecksEnabled
        {
            get { return Convert.ToBoolean(base["DASExportThresholdChecksEnabled"].ToString()); }
        }

        /// <summary>
        ///     Used when building data for export to the Apprenticeship API. The number of records in the export tables is
        ///     compared to the number generated during the previous export. If the number falls by more than the specified
        ///     percentage, the export is rolled back.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int DASExportThresholdCheckPercent
        {
            get { return Convert.ToInt32(base["DASExportThresholdCheckPercent"].ToString()); }
        }

        /// <summary>
        ///     Used when building data for export to the Apprenticeship API. Setting the value to True forces through an export
        ///     which has failed the threshold validation checks. The system resets the value to False after the export.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool DASOverrideThresholdCheck
        {
            get { return Convert.ToBoolean(base["DASOverrideThresholdCheck"].ToString()); }
        }

        /// <summary>
        /// Used when downloading data from the UCAS API
        /// </summary>
        public String UCASAPIClientId
        {
            get { return base["UCASAPIClientId"].ToString(); }
        }

        /// <summary>
        /// Used when downloading data from the UCAS API
        /// </summary>
        public String UCASAPIClientSecret
        {
            get { return base["UCASAPIClientSecret"].ToString(); }
        }

        /// <summary>
        /// Used when downloading data from the UCAS API
        /// </summary>
        public String UCASAPIURL
        {
            get { return base["UCASAPIURL"].ToString(); }
        }

        /// <summary>
        /// Used when downloading data from the UCAS API
        /// </summary>
        public String ApprenticeshipQABands
        {
            get { return base["ApprenticeshipQABands"].ToString(); }
        }

        public String LARSImportTime
        {
            get
            {
                return base["LARSImportTime"] == null ? String.Empty : base["LARSImportTime"].ToString();
            }
        }

        public String LARSImportUserId
        {
            get
            {
                return base["LARSImportUserId"] == null ? String.Empty : base["LARSImportUserId"].ToString();
            }
        }

        public String LARSUrlAndFileName
        {
            get
            {
                return base["LARSUrlAndFileName"] == null ? String.Empty : base["LARSUrlAndFileName"].ToString();
            }
        }

        public Int32 LARSDaysSinceLastImportBeforeSendingEmail
        {
            get
            {
                const Int32 DefaultReturnValue = 365*10; // 10 Years
                Int32 days;
                Int32.TryParse(base["LARSDaysSinceLastImportBeforeSendingEmail"].ToString(), out days);

                return days <= 0 ? DefaultReturnValue : days;
            }
        }

        public String LARSLongTimeSinceImportEmailAddress
        {
            get
            {
                return base["LARSLongTimeSinceImportEmailAddress"] == null ? String.Empty : base["LARSLongTimeSinceImportEmailAddress"].ToString();                
            }
        }

        /// <summary>
        /// Email address to send LARS import failure email
        /// </summary>
        public String LARSImportErrorEmailAddress
        {
            get { return base["LARSImportErrorEmailAddress"] == null ? String.Empty : base["LARSImportErrorEmailAddress"].ToString(); }
        }

        /// <summary>
        /// Email address to send RoATP import failure email
        /// </summary>
        public String RoATPImportErrorEmailAddress
        {
            get { return base["RoATPImportErrorEmailAddress"] == null ? String.Empty : base["RoATPImportErrorEmailAddress"].ToString(); }
        }

        /// <summary>
        ///     Configuration setting for determining short/long course date based on length. If greater than this day parameter, then course is considered long
        /// </summary>
        public Int32 LongCourseMinDurationWeeks
        {
            get { return Convert.ToInt32(base["LongCourseMinDurationWeeks"].ToString()); }
        }

        /// <summary>
        ///     Configuration setting for determining how many days after start date that a course is considered out of date.
        /// </summary>
        public Int32 LongCourseMaxStartDateInPastDays
        {
            get { return Convert.ToInt32(base["LongCourseMaxStartDateInPastDays"].ToString()); }
        }

        /// <summary>
        /// Configuration setting for determining time to import RoATP API data
        /// </summary>
        public String RoATPAPIImportTime
        {
            get
            {
                return base["RoATPAPIImportTime"] == null ? String.Empty : base["RoATPAPIImportTime"].ToString();
            }
        }

        /// <summary>
        /// Configuration setting for monthly report excel template filename
        /// </summary>
        public String MonthlyReportExcelTemplateFilename
        {
            get
            {
                return base["MonthlyReportExcelTemplateFilename"] == null ? String.Empty : base["MonthlyReportExcelTemplateFilename"].ToString();
            }
        }

        /// <summary>
        /// Configuration setting for monthly report excel template filename
        /// </summary>
        public String DFEStartDateReportExcelTemplateFilename
        {
            get
            {
                return base["DFEStartDateReportExcelTemplateFilename"] == null ? String.Empty : base["DFEStartDateReportExcelTemplateFilename"].ToString();
            }
        }



        /// <summary>
        ///     Configuration setting for the start date dd/mm/yyyy when we require providers to confirm they have refreshed their provision..
        /// </summary>
        public DateTime RoATPRefreshStartDate
        {
            get { return Convert.ToDateTime(base["RoATPRefreshStartDate"].ToString()); }
        }



        /// <summary>
        ///     Configuration setting for the end date dd/mm/yyyy by which we require providers to confirm they have refreshed their provision..
        /// </summary>
        public DateTime RoATPRefreshEndDate
        {
            get { return Convert.ToDateTime(base["RoATPRefreshEndDate"].ToString()); }
        }



        /// <summary>
        ///     Add a configuration setting to the base collection
        /// </summary>
        /// <param name="configurationSetting">The configuration Setting</param>
        public void Add(ConfigurationSetting configurationSetting)
        {
            base.Add(configurationSetting.Name, configurationSetting);
        }


        /// <summary>
        ///     Returns a configuration setting
        /// </summary>
        /// <param name="key">The name of the setting to return</param>
        /// <returns>An object which is the setting</returns>
        public object GetValue(string key)
        {
            object value = null;
            if (key != null && ContainsKey(key))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                value = Convert.ChangeType(this[key].Value, Type.GetType(this[key].DataType));
            }

            return value;
        }

        /// <summary>
        ///     Update a role name in the configuration settings.
        /// </summary>
        /// <param name="oldName">Old role name.</param>
        /// <param name="newName">New role name.</param>
        public void RenameConfiguredRoles(string oldName, string newName)
        {
            var roleConfigSettings = new List<string>
            {
                "AdminContextCanAddRoles",
                "AdminUserCanAddRoles",
                "OrganisationContextCanAddRoles",
                "OrganisationUserCanAddRoles",
                "ProviderContextCanAddRoles",
                "ProviderUserCanAddRoles",
                "SAUserRolePrimary",
                "SAUserRoleSecondary"
            };

            oldName = oldName.Trim();
            newName = newName.Trim();

            using (var db = new ProviderPortalEntities())
            {
                foreach (var settingName in roleConfigSettings)
                {
                    var configSetting = db.ConfigurationSettings.Find(settingName);

                    var roles = configSetting.Value.Trim().Split(';');
                    for (var i = 0; i < roles.Length; i++)
                    {
                        if (roles[i].Equals(oldName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            roles[i] = newName;
                        }
                    }
                    configSetting.Value = string.Join(";", roles);
                }
                db.SaveChanges();
            }
            Constants.ConfigSettings.Refresh();
        }

        /// <summary>
        ///     Reloads all the settings from the database
        /// </summary>
        public void Refresh()
        {
            LoadSettings();
        }

        /// <summary>
        ///     Loads the settings.
        /// </summary>
        /// <exception cref="ConfigurationErrorsException">
        ///     Throws a configuration Exception if settings can not be retrieved.
        /// </exception>
        private void LoadSettings()
        {
            Clear();
            apprenticeshipQABands.Clear();
            try
            {
                List<Entities.ConfigurationSetting> configSettings;
                // ReSharper disable once SuggestUseVarKeywordEvident
                using (var databaseContext = new ProviderPortalEntities())
                {
                    configSettings = databaseContext.ConfigurationSettings.ToList();
                }

                foreach (var setting in configSettings)
                {
                    var dateUpdated = setting.LastUpdated ?? DateTime.MinValue;

                    Add(
                        setting.Name,
                        new ConfigurationSetting(
                            setting.Name,
                            setting.Value,
                            setting.ValueDefault,
                            setting.DataType,
                            setting.Description,
                            dateUpdated,
                            setting.LastUpdatedBy.ToString()));

                    AppGlobal.Log.WriteDebug(
                        string.Format(
                            "Setting name {0}, setting value {1}, setting default {2}, setting data type {3} loaded into ConfigurationSettings for use in the website",
                            setting.Name,
                            setting.Value,
                            setting.ValueDefault,
                            setting.DataType));
                }

                AppGlobal.Log.WriteLog("Configuration settings loaded successfully from the database");
            }
            catch
            {
                throw new ConfigurationErrorsException(
                    "Failed to retrieve configuration settings from database, please check that the database is started and that the connection string in the web.config file is correct.");
            }
        }


        private static readonly SortedDictionary<Int32, Int32> apprenticeshipQABands = new SortedDictionary<Int32, Int32>();
        public Int32 GetNumberOfApprenticeshipsToQA(Int32 numberOfApprenticeships)
        {
            if (numberOfApprenticeships == 0)
            {
                return 0;
            }

            if (apprenticeshipQABands.Count == 0)
            {
                foreach (String setting in ApprenticeshipQABands.Split(','))
                {
                    try
                    {
                        String[] s = setting.Split('~');
                        apprenticeshipQABands.Add(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]));
                    }
                    catch {}
                }
            }

            Int32 retValue = 0;
            foreach (KeyValuePair<Int32, Int32> kvp in apprenticeshipQABands)
            {
                if (kvp.Key >= numberOfApprenticeships)
                {
                    retValue = kvp.Value;
                    break;
                }
            }
            if (retValue > numberOfApprenticeships)
            {
                retValue = numberOfApprenticeships;
            }

            return retValue;
        }
    }
}