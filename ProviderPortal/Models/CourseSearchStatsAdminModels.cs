using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using System.Web.Configuration;

    public class CourseSearchStatsFileModel
    {
        public String Filename { get; set; }
    }

    public class CourseSearchStatsFolderModel
    {
        public DateTime Name { get; set; }
        public List<CourseSearchStatsFileModel> Files { get; set; }

        public bool CanDelete
        {
            get
            {
                return this.Files.Count == 0;
            }
        }
    }

    public class CourseSearchStatsAdminModel
    {
        public String BaseUrl { get; set; }
        public List<CourseSearchStatsFolderModel> Folders { get; set; }

        [LanguageDisplay("Upload to Folder")]
        public String UploadToFolder { get; set; }

        [LanguageDisplay("New Folder")]
        public String NewFolderName { get; set; }

        [LanguageDisplay("File to Upload")]
        public HttpPostedFileBase FileUpload { get; set; }

        public Dictionary<String, String> FolderNames
        {
            get
            {
                return this.Folders.ToDictionary(
                    folder => folder.Name.ToString(UsageStatistics.FolderNameFormat), folder => folder.Name.ToLongDateString()
                );
            }
        }
    }
}