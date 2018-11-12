using System;
using System.IO;
using System.Linq;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder
{
    public class WProviderSearch : BuilderBase
    {
        protected readonly ProviderPortalEntities _db;
        private Action<string> _logger;

        public WProviderSearch(ProviderPortalEntities db, Action<string> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override void GenerateCsv()
        {
            _logger("Starting Provider Search AIM CSV creation.");

            var providerSearchItems = _db.up_ProviderSearchListForCsvExport();

            using (Stream stream = File.Open(Constants.W_Provider_Search_CsvFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    var csv = new CsvWriter(writer);

                    csv.WriteField<string>("PROVIDER_SEARCH_TEXT");

                    csv.WriteField<string>("PROVIDER_ID");

                    csv.WriteField<string>("PROVIDERNAME");

                    csv.NextRecord();

                    foreach (var providerSearchItem in providerSearchItems.ToList())
                    {
                        csv.WriteField(providerSearchItem.PROVIDER_SEARCH_TEXT);

                        csv.WriteField(providerSearchItem.PROVIDER_ID);

                        csv.WriteField(providerSearchItem.PROVIDERNAME);

                        csv.NextRecord();
                    }
                }
            }
        }
    }
}
