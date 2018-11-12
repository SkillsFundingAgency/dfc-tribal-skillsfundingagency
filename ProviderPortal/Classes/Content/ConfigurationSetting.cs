// --------------------------------------------------------------------------------------------------------------------
// <copyright company="ConfigurationSetting" file="ConfigurationSetting.cs">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   A configuration setting
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    using System;
    using System.Configuration;

    /// <summary>
    /// A class that holds a configuration setting
    /// </summary>
    public class ConfigurationSetting
    {
        /// <summary>
        /// Holds the default value
        /// </summary>
        private readonly string valueDefault = null;
        
        /// <summary>
        /// Holds the name
        /// </summary>
        private string name;

       /// <summary>
        /// The data type
        /// </summary>
        private string dataType;

        /// <summary>
        /// The description
        /// </summary>
        private string description;

        /// <summary>
        /// The last updated by user
        /// </summary>
        private string lastUpdatedBy;

        /// <summary>
        /// The date time last updated
        /// </summary>
        private DateTime dateLastUpdated;

        /// <summary>
        /// The setting value
        /// </summary>
        private object value = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSetting"/> class. 
        /// An individual setting 
        /// </summary>
        public ConfigurationSetting()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSetting"/> class. 
        /// An individual setting 
        /// </summary>
        /// <param name="name">The configuration setting name
        /// </param>
        /// <param name="value">The value of the setting
        /// </param>
        /// <param name="valueDefault">A default value that may be used
        /// </param>
        /// <param name="dataType">The data type of the setting
        /// </param>
        /// <param name="description">The description of the setting
        /// </param>
        /// <param name="lastUpdated">The date time last updated
        /// </param>
        /// <param name="lastUpdatedBy">Who last updated the setting
        /// </param>
        public ConfigurationSetting(string name, string value, string valueDefault, string dataType, string description, DateTime lastUpdated, string lastUpdatedBy)
        {
            this.name = name;
            this.dataType = dataType;
            this.description = description;
            this.dateLastUpdated = lastUpdated;
            this.lastUpdatedBy = lastUpdatedBy;
            this.valueDefault = valueDefault;

            // Cast the string value to the object using the datatype and default to ValueDefault if required
            string usingValue = null;
            if (!string.IsNullOrEmpty(value))
            {
                usingValue = value;
            }
            else
            {
                if (!string.IsNullOrEmpty(this.valueDefault))
                {
                    usingValue = valueDefault;
                }
            }

            this.value = this.ConvertStringToValue(usingValue, this.dataType);
        }

        /// <summary>
        /// Gets or sets the configuration name
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }


        /// <summary>
        /// Gets or sets Last updated by
        /// </summary>
        public string LastUpdatedBy
        {
            get
            {
                return this.lastUpdatedBy;
            }

            set
            {
                this.lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Gets or sets description
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
            }
        }

        /// <summary>
        /// Gets or sets the data type
        /// </summary>
        public string DataType
        {
            get
            {
                return this.dataType;
            }

            set
            {
                this.dataType = value;
            }
        }

        /// <summary>
        /// Gets the Value Default
        /// </summary>
        public string ValueDefault
        {
            get
            {
                return this.valueDefault;
            }
        }

        /// <summary>
        /// Gets the value, if no value is set in the database the default value is returned here, if no default value a null is returned
        /// </summary>
        public object Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Overrides ToString to return a string representation of the setting
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            if (this.value == null)
            {
                return string.Empty;
            }
            else
            {
                return this.value.ToString();
            }
        }

        /// <summary>
        /// Returns the default value, and if no default value returns the default entered
        /// </summary>
        /// <param name="defaultWhenNull">The value to return if the default value is missing</param>
        /// <returns>Returns a string</returns>
        public string ToString(string defaultWhenNull)
        {
            return this.valueDefault ?? defaultWhenNull;
        }

        /// <summary>
        /// Updates the setting value to the new value
        /// </summary>
        /// <param name="newValue">The new value</param>
        public void UpdateSetting(string newValue)
        {
            this.value = this.ConvertStringToValue(newValue, this.DataType);
            this.dateLastUpdated = System.DateTime.Now;
        }

        /// <summary>
        /// The convert string to value method
        /// </summary>
        /// <param name="stringValue">
        /// The string value.
        /// </param>
        /// <param name="settingDataType">
        /// The data type.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="ConfigurationErrorsException">An object cast to the correct type
        /// </exception>
        private object ConvertStringToValue(string stringValue, string settingDataType)
        {
            if (!string.IsNullOrEmpty(stringValue))
            {
                // May raise an exception, so trap, format a better error message for trouble shooting and throw up, can't run the site when 
                // configuration settings are in error
                try
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    return Convert.ChangeType(stringValue, Type.GetType(settingDataType));
                }
                catch
                {
                    throw new ConfigurationErrorsException(string.Format("There was an error loading the configuration settings, error on setting {0}, data type {1} and value {2}", this.name, this.dataType, stringValue));
                }
            }
            else
            {
                return null;
            }
        }

    }
}