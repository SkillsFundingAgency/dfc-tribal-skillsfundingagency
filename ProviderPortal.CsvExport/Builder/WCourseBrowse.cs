using System;
using System.IO;
using System.Linq;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder
{
    public class WCourseBrowse : BuilderBase
    {
        protected readonly ProviderPortalEntities _db;
        private Action<string> _logger;

        public WCourseBrowse(ProviderPortalEntities db, Action<string> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override void GenerateCsv()
        {
            _logger("Starting Course browser CSV creation.");

            var courseItems = _db.up_CourseBrowseListForCsvExport();

            using (Stream stream = File.Open(Constants.W_Course_Browse_CsvFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    var csv = new CsvWriter(writer);

                    csv.WriteField<string>("CATEGORY_CODE");

                    csv.WriteField<string>("COURSE_COUNT");

                    csv.WriteField<string>("PARENT_CATEGORY_CODE");

                    csv.WriteField<string>("DESCRIPTION");

                    csv.WriteField<string>("SEARCHABLE_FLAG");

                    csv.NextRecord();

                    foreach (var courseItem in courseItems.ToList())
                    {
                        csv.WriteField(courseItem.CATEGORY_CODE);

                        csv.WriteField(courseItem.COURSE_COUNT);

                        csv.WriteField(courseItem.PARENT_CATEGORY_CODE);

                        csv.WriteField(courseItem.DESCRIPTION);

                        csv.WriteField(courseItem.SEARCHABLE_FLAG);

                        csv.NextRecord();
                    }
                }
            }
        }
    }
}
