using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class CourseSearchStatsAdminModelExtensions
    {
        public static void Populate(
            this CourseSearchStatsAdminModel model,
            Dictionary<DateTime, List<String>> folders)
        {
            model.Folders = new List<CourseSearchStatsFolderModel>();
            foreach (var folder in folders)
            {
                var folderModel = new 
                    CourseSearchStatsFolderModel
                    {
                        Name = folder.Key,
                        Files = new List<CourseSearchStatsFileModel>()
                    };
                foreach (var file in folder.Value)
                {
                    folderModel.Files.Add(new
                        CourseSearchStatsFileModel
                        {
                            Filename = file
                        });
                }
                model.Folders.Add(folderModel);
            }
        }
    }
}