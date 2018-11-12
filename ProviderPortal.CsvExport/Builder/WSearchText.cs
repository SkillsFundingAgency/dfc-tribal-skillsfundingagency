using System;
using System.IO;
using System.Linq;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder
{
    public class WSearchText : BuilderBase
    {
        protected readonly ProviderPortalEntities _db;
        private Action<string> _logger;

        public WSearchText(ProviderPortalEntities db, Action<string> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override void GenerateCsv()
        {
            _logger("Starting Search Text CSV creation.");

            var courseSearchListItems = _db.up_CourseSearchTextForCsvExport();

            using (Stream stream = File.Open(Constants.W_Search_Text_CsvFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    var csv = new CsvWriter(writer);

                    csv.WriteField<string>("COURSE_ID");

                    csv.WriteField<string>("OPPORTUNITY_ID");

                    csv.WriteField<string>("STUDY_MODE");

                    csv.WriteField<string>("ATTENDANCE_MODE");

                    csv.WriteField<string>("ATTENDANCE_PATTERN");

                    csv.WriteField<string>("QUAL_TYPE_CODE");

                    csv.WriteField<string>("XMIN");

                    csv.WriteField<string>("XMAX");

                    csv.WriteField<string>("YMIN");

                    csv.WriteField<string>("YMAX");

                    csv.WriteField<string>("X_COORD");

                    csv.WriteField<string>("Y_COORD");

                    csv.WriteField<string>("SEARCH_TEXT");

                    csv.WriteField<string>("NO_OF_OPPS");

                    csv.WriteField<string>("LAST_UPDATE_DATE");

                    csv.WriteField<string>("PROVIDER_ID");

                    csv.WriteField<string>("SEARCH_LDCS");

                    csv.WriteField<string>("PROVIDER_NAME");

                    csv.WriteField<string>("IES_FLAG");

                    csv.WriteField<string>("TTG_FLAG");

                    csv.WriteField<string>("TQS_FLAG");

                    csv.WriteField<string>("SFL_FLAG");

                    csv.WriteField<string>("APP_DEADLINE");

                    csv.WriteField<string>("APP_STATUS");

                    csv.WriteField<string>("TTG_STATUS");

                    csv.WriteField<string>("ADULT_LEARNER");

                    csv.WriteField<string>("OTHER_FUNDING");

                    csv.WriteField<string>("ILS_FLAG");

                    csv.WriteField<string>("FLEXIBLE_START_FLAG");

                    csv.WriteField<string>("SEARCH_REGION");

                    csv.WriteField<string>("SEARCH_TOWN");

                    csv.WriteField<string>("SEARCH_POSTCODE");

                    csv.NextRecord();

                    foreach (var courseSearchListItem in courseSearchListItems.ToList())
                    {

                        csv.WriteField(courseSearchListItem.COURSE_ID);

                        csv.WriteField(courseSearchListItem.OPPORTUNITY_ID);

                        csv.WriteField(courseSearchListItem.STUDY_MODE);

                        csv.WriteField(courseSearchListItem.ATTENDANCE_MODE);

                        csv.WriteField(courseSearchListItem.ATTENDANCE_PATTERN);

                        csv.WriteField(courseSearchListItem.QUAL_TYPE_CODE);

                        csv.WriteField(courseSearchListItem.XMIN);

                        csv.WriteField(courseSearchListItem.XMAX);

                        csv.WriteField(courseSearchListItem.YMIN);

                        csv.WriteField(courseSearchListItem.YMAX);

                        csv.WriteField(courseSearchListItem.X_COORD);

                        csv.WriteField(courseSearchListItem.Y_COORD);

                        csv.WriteField(courseSearchListItem.SEARCH_TEXT);

                        csv.WriteField(courseSearchListItem.NO_OF_OPPS);

                        csv.WriteField(courseSearchListItem.LAST_UPDATE_DATE);

                        csv.WriteField(courseSearchListItem.PROVIDER_ID);

                        csv.WriteField(courseSearchListItem.SEARCH_LDCS);

                        csv.WriteField(courseSearchListItem.PROVIDER_NAME);

                        csv.WriteField(courseSearchListItem.IES_FLAG);

                        csv.WriteField(courseSearchListItem.TTG_FLAG);

                        csv.WriteField(courseSearchListItem.TQS_FLAG);

                        csv.WriteField(courseSearchListItem.SFL_FLAG);

                        csv.WriteField(courseSearchListItem.APP_DEADLINE);

                        csv.WriteField(courseSearchListItem.APP_STATUS);

                        csv.WriteField(courseSearchListItem.TTG_STATUS);

                        csv.WriteField(courseSearchListItem.ADULT_LEARNER);

                        csv.WriteField(courseSearchListItem.OTHER_FUNDING);

                        csv.WriteField(courseSearchListItem.ILS_FLAG);

                        csv.WriteField(courseSearchListItem.FLEXIBLE_START_FLAG);

                        csv.WriteField(courseSearchListItem.SEARCH_REGION);

                        csv.WriteField(courseSearchListItem.SEARCH_TOWN);

                        csv.WriteField(courseSearchListItem.SEARCH_POSTCODE);

                        csv.NextRecord();
                    }
                }
            }
        }
    }
}
