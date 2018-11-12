// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Tribal" file="ConfigurationSettings.cs">
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

namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes
{
    /// <summary>
    /// Creates the configuration object
    /// </summary>
    public class ConfigurationSettings : Dictionary<string, ConfigurationSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSettings"/> class. 
        /// </summary>
        public ConfigurationSettings()
        {
            // Load the settings from the database on first creating
            LoadSettings();
        }

        /// <summary>
        /// Gets the name of the application
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
        /// Gets the name of virutal directory for saving bulk upload csv files.  This location may be mapped to local/ network drive.
        /// </summary>
        public string NightlyCsvFilesDirectoryLocation
        {
            get
            {
                return base["NightlyCsvFilesLocation"].ToString().EndsWith(@"\") ?
                       base["NightlyCsvFilesLocation"].ToString() :
                       string.Concat(base["NightlyCsvFilesLocation"].ToString(), @"\");

            }
        }

        public Int32 NumberOfZipFilesToRetain
        {
            get
            {
                Int32 numberOfFiles;
                if (!Int32.TryParse(base["NumberOfNightlyCsvFilesToRetain"].ToString(), out numberOfFiles))
                {
                    numberOfFiles = 4;
                }
                return numberOfFiles;
            }
        }

        /// <summary>
        /// Gets the short date format used on the site as set in the web config file, e.g. 31/01/2005 or 31-Jan-05 
        /// </summary>
        public string ShortDateFormat
        {
            get
            {
                return base["ShortDateFormat"].ToString();
            }
        }

        public string ShortDateFormatFileName
        {
            get
            {
                return base["ShortDateFormatFileName"].ToString();
            }
        }

        /// <summary>
        /// Add a configuration setting to the base collection
        /// </summary>
        /// <param name="configurationSetting">The configuration Setting</param>
        public void Add(ConfigurationSetting configurationSetting)
        {
            base.Add(configurationSetting.Name, configurationSetting);
        }

        /// <summary>
        /// Returns a configuration setting
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
        /// Loads the settings.
        /// </summary>
        /// <exception cref="ConfigurationErrorsException">Throws a configuration Exception if settings can not be retrieved.
        /// </exception>
        private void LoadSettings()
        {
            Clear();
            try
            {
                List<Tribal.SkillsFundingAgency.ProviderPortal.Entities.ConfigurationSetting> configSettings = null;
                // ReSharper disable once SuggestUseVarKeywordEvident
                using (ProviderPortalEntities databaseContext = new ProviderPortalEntities())
                {
                    configSettings = databaseContext.ConfigurationSettings.ToList();
                }

                foreach (var setting in configSettings)
                {
                    DateTime dateUpdated;
                    if (setting.LastUpdated == null)
                    {
                        dateUpdated = DateTime.MinValue;
                    }
                    else
                    {
                        dateUpdated = setting.LastUpdated.Value;
                    }

                    Add(setting.Name, new ConfigurationSetting(setting.Name,
                                                               setting.Value,
                                                               setting.ValueDefault,
                                                               setting.DataType,
                                                               setting.Description,
                                                               dateUpdated,
                                                               setting.LastUpdatedBy.ToString()));
                }
            }
            catch
            {
                throw new ConfigurationErrorsException("Failed to retrieve configuration settings from database, please check that the database is started and that the connection string in the web.config file is correct.");
            }
        }
    }
}