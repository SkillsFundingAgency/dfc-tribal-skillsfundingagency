using System;
using System.IO;
using System.Linq;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder
{
    public class OOpportunityA10 : BuilderBase
    {
        protected readonly ProviderPortalEntities _db;
        private Action<string> _logger;

        public OOpportunityA10(ProviderPortalEntities db, Action<string> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override void GenerateCsv()
        {

            _logger("Starting Opportunities A10 CSV creation.");

            var opportunityItems = _db.up_CourseInstanceA10CodesForCsvExport();

            using (Stream stream = File.Open(Constants.O_Opportunity_A10_CsvFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    var csv = new CsvWriter(writer);

                    csv.WriteField<string>("OPPORTUNITY_ID");

                    csv.WriteField<string>("A10_CODE");

                    csv.NextRecord();

                    foreach (var opportunityItem in opportunityItems.ToList())
                    {
                        csv.WriteField(opportunityItem.OPPORTUNITY_ID);

                        csv.WriteField(opportunityItem.A10_CODE);

                        csv.NextRecord();
                    }
                }
            }
        }
    }
}
