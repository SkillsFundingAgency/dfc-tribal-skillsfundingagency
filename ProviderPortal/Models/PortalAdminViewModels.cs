using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TribalTechnology.InformationManagement.Web.UI.Text;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{

    #region Cache

    public class CacheViewModel
    {
        public string Message { get; set; }
        public Dictionary<string, string> CacheItems { get; set; }
        public string CacheMemoryFree { get; set; }
    }

    #endregion

    #region Configuration

    public class ConfigurationViewModel
    {
        public List<ConfigurationSetting> Settings { get; set; }
    }

    #endregion

    #region Language resources

    /// <summary>
    /// View model for the language resources page
    /// </summary>
    public class LanguageResourcesViewModel
    {
        public List<SelectListItem> DownloadLanguageOptions { get; set; }

        [LanguageDisplay("Download Language")]
        public int DownloadLanguageId { get; set; }

        public bool UploadSuccess { get; set; }
        public string UploadMessage { get; set; }
        public List<SelectListItem> UploadLanguageOptions { get; set; }

        [LanguageDisplay("Upload Language")]
        public int UploadLanguageId { get; set; }

        [LanguageDisplay("File to Upload")]
        public HttpPostedFileBase FileUpload { get; set; }

        public bool NewLanguageSuccess { get; set; }
        public string NewLanguageMessage { get; set; }

        [LanguageDisplay("New Language Name")]
        public string NewLanguageName { get; set; }

        [LanguageDisplay("New Language IETF")]
        public string NewLanguageIETF { get; set; }
    }


    /// <summary>
    /// Language export data.
    /// </summary>
    public class LanguageExportModel
    {
        public byte[] Bytes { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
    }

    /// <summary>
    /// Language entry.
    /// </summary>
    public class LanguageEntry
    {
        public string FieldName { get; set; }
        public string LanguageText { get; set; }
    }

    #endregion

}