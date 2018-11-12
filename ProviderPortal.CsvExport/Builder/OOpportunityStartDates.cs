using System;
using System.IO;
using System.Linq;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder
{
    public class OOpportunityStartDates : BuilderBase
    {
        protected readonly ProviderPortalEntities _db;
        private Action<string> _logger;

        public OOpportunityStartDates(ProviderPortalEntities db, Action<string> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override void GenerateCsv()
        {
            _logger("Starting Opportunities Start Dates CSV creation.");

            var courseInstanceItems = _db.up_CourseInstanceStartDatesListForCsvExport();

            using (Stream stream = File.Open(Constants.O_Opportunity_StartDate, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    var csv = new CsvWriter(writer);

                    csv.WriteField<string>("OPPORTUNITY_ID");

                    csv.WriteField<string>("START_DATE");

                    csv.WriteField<string>("PLACES_AVAILABLE");

                    csv.WriteField<string>("DATE_FORMAT");

                    csv.NextRecord();

                    foreach (var courseInstanceItem in courseInstanceItems.ToList())
                    {
                        csv.WriteField(courseInstanceItem.OPPORTUNITY_ID);

                        csv.WriteField(courseInstanceItem.START_DATE);

                        csv.WriteField(courseInstanceItem.PLACES_AVAILABLE);

                        csv.WriteField(courseInstanceItem.DATE_FORMAT);

                        csv.NextRecord();
                    }
                }
            }
        }
    }
}
