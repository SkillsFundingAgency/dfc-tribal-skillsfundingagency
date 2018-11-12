using System;
using System.IO;
using System.Linq;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder
{
    public class OOpportunities : BuilderBase
    {
        protected readonly ProviderPortalEntities _db;
        private Action<string> _logger;

        public OOpportunities(ProviderPortalEntities db, Action<string> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override void GenerateCsv()
        {
            _logger("Starting Opportunities CSV creation.");

            var opportunityItems = _db.up_CourseInstanceListForCsvExport();

            using (Stream stream = File.Open(Constants.O_Opportunities_CsvFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    var csv = new CsvWriter(writer);

                    csv.WriteField<string>("OPPORTUNITY_ID");

                    csv.WriteField<string>("PROVIDER_OPPORTUNITY_ID");

                    csv.WriteField<string>("PRICE");

                    csv.WriteField<string>("PRICE_DESCRIPTION");

                    csv.WriteField<string>("DURATION_VALUE");

                    csv.WriteField<string>("DURATION_UNITS");

                    csv.WriteField<string>("DURATION_DESCRIPTION");

                    csv.WriteField<string>("START_DATE_DESCRIPTION");

                    csv.WriteField<string>("END_DATE");

                    csv.WriteField<string>("STUDY_MODE");

                    csv.WriteField<string>("ATTENDANCE_MODE");

                    csv.WriteField<string>("ATTENDANCE_PATTERN");

                    csv.WriteField<string>("LANGUAGE_OF_INSTRUCTION");

                    csv.WriteField<string>("LANGUAGE_OF_ASSESSMENT");

                    csv.WriteField<string>("PLACES_AVAILABLE");

                    csv.WriteField<string>("ENQUIRE_TO");

                    csv.WriteField<string>("APPLY_TO");

                    csv.WriteField<string>("APPLY_FROM");

                    csv.WriteField<string>("APPLY_UNTIL");

                    csv.WriteField<string>("APPLY_UNTI_DESC");

                    csv.WriteField<string>("URL");

                    csv.WriteField<string>("TIMETABLE");

                    csv.WriteField<string>("COURSE_ID");

                    csv.WriteField<string>("VENUE_ID");

                    csv.WriteField<string>("APPLY_THROUGHOUT_YEAR");

                    csv.WriteField<string>("EIS_FLAG");

                    csv.WriteField<string>("REGION_NAME");

                    csv.WriteField<string>("DATE_CREATED");

                    csv.WriteField<string>("DATE_UPDATE");

                    csv.WriteField<string>("STATUS");

                    csv.WriteField<string>("UPDATED_BY");

                    csv.WriteField<string>("CREATED_BY");

                    csv.WriteField<string>("OPPORTUNITY_SUMMARY");

                    csv.WriteField<string>("REGION_ID");

                    csv.WriteField<string>("SYS_DATA_SOURCE");

                    csv.WriteField<string>("DATE_UPDATED_COPY_OVER");

                    csv.WriteField<string>("DATE_CREATED_COPY_OVER");

                    csv.WriteField<string>("OFFERED_BY");

                    csv.WriteField<string>("DFE_FUNDED");

                    csv.NextRecord();

                    foreach (var opportunityItem in opportunityItems.ToList())
                    {
                        csv.WriteField(opportunityItem.OPPORTUNITY_ID);

                        csv.WriteField(opportunityItem.PROVIDER_OPPORTUNITY_ID);

                        csv.WriteField(opportunityItem.PRICE);

                        csv.WriteField(opportunityItem.PRICE_DESCRIPTION);

                        csv.WriteField(opportunityItem.DURATION_VALUE);

                        csv.WriteField(opportunityItem.DURATION_UNITS);

                        csv.WriteField(opportunityItem.DURATION_DESCRIPTION);

                        csv.WriteField(opportunityItem.START_DATE_DESCRIPTION);

                        csv.WriteField(opportunityItem.END_DATE);

                        csv.WriteField(opportunityItem.STUDY_MODE);

                        csv.WriteField(opportunityItem.ATTENDANCE_MODE);

                        csv.WriteField(opportunityItem.ATTENDANCE_PATTERN);

                        csv.WriteField(opportunityItem.LANGUAGE_OF_INSTRUCTION);

                        csv.WriteField(opportunityItem.LANGUAGE_OF_ASSESSMENT);

                        csv.WriteField(opportunityItem.PLACES_AVAILABLE);

                        csv.WriteField(opportunityItem.ENQUIRE_TO);

                        csv.WriteField(opportunityItem.APPLY_TO);

                        csv.WriteField(opportunityItem.APPLY_FROM);

                        csv.WriteField(opportunityItem.APPLY_UNTIL);

                        csv.WriteField(opportunityItem.APPLY_UNTI_DESC);

                        csv.WriteField(opportunityItem.URL);

                        csv.WriteField(opportunityItem.TIMETABLE);

                        csv.WriteField(opportunityItem.COURSE_ID);

                        csv.WriteField(opportunityItem.VENUE_ID);

                        csv.WriteField(opportunityItem.APPLY_THROUGHOUT_YEAR);

                        csv.WriteField(opportunityItem.EIS_FLAG);

                        csv.WriteField(opportunityItem.REGION_NAME);

                        csv.WriteField(opportunityItem.DATE_CREATED);

                        csv.WriteField(opportunityItem.DATE_UPDATE);

                        csv.WriteField(opportunityItem.STATUS);

                        csv.WriteField(opportunityItem.UPDATED_BY);

                        csv.WriteField(opportunityItem.CREATED_BY);

                        csv.WriteField(opportunityItem.OPPORTUNITY_SUMMARY);

                        csv.WriteField(opportunityItem.REGION_ID);

                        csv.WriteField(opportunityItem.SYS_DATA_SOURCE);

                        csv.WriteField(opportunityItem.DATE_UPDATED_COPY_OVER);

                        csv.WriteField(opportunityItem.DATE_CREATED_COPY_OVER);

                        csv.WriteField(opportunityItem.OFFERED_BY);

                        csv.WriteField(opportunityItem.DFE_FUNDED);

                        csv.NextRecord();
                    }
                }
            }
        }
    }
}
