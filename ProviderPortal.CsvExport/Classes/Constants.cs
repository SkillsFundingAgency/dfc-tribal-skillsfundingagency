using System;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes
{

    public static class Constants
    {
        /// <summary>
        /// Initializes static members of the <see cref="Constants"/> class.
        /// </summary>
        static Constants()
        {
            ConfigSettings = null;
        }

        /// <summary>
        /// Gets or sets the configuration settings object
        /// </summary>
        public static ConfigurationSettings ConfigSettings { get; set; }

 
        /// <summary>
        /// 
        /// </summary>
        public static string C_Providers_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "C_PROVIDERS.csv");
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public static string C_Venues_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "C_VENUES.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string O_Courses_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "O_COURSES.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string O_Opportunity_A10_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "O_OPP_A10.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string O_Opportunity_StartDate
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "O_OPP_START_DATES.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string O_Opportunities_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "O_OPPORTUNITIES.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string S_Learning_AIM_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "S_LEARNING_AIMS.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string W_Course_Browse_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "W_COURSE_BROWSE.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string W_Provider_Search_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "W_PROVIDER_SEARCH.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string W_Search_Text_CsvFilename
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\",
                                     "W_SEARCH_TEXT.csv");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NightlyCsvZipFileName
        {
            get
            {
                return string.Concat(NightlyCsvFolderName, ".zip");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NightlyCsvZipFolderPath
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation,
                                     ConfigSettings.NightlyCsvFilesDirectoryLocation.EndsWith(@"\") ? string.Empty : @"\",
                                     @"\", NightlyCsvFolderName, @"\");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NightlyCsvZipFileNamewithAbsolutePath
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation, NightlyCsvZipFileName);
            }
        }

        public static string LogFileName
        {
            get
            {
                return string.Concat(ConfigSettings.NightlyCsvFilesDirectoryLocation, "Log.Txt");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NightlyCsvFolderName
        {
            get
            {
                return string.Format("COURSES_{0}", DateTime.Now.ToString(ConfigSettings.ShortDateFormatFileName));
            }
        }
    }
}