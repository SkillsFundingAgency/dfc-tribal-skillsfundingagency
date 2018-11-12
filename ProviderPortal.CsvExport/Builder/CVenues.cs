using System;
using System.IO;
using System.Linq;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder
{
    public class CVenues : BuilderBase
    {
        protected readonly ProviderPortalEntities _db;
        private Action<string> _logger;

        public CVenues(ProviderPortalEntities db, Action<string> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override void GenerateCsv()
        {
            _logger("Starting Venues CSV creation.");

            var venueItems = _db.up_VenueListForCsvExport();

            using (Stream stream = File.Open(Constants.C_Venues_CsvFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    var csv = new CsvWriter(writer);

                    csv.WriteField<string>("PROVIDER_ID");

                    csv.WriteField<string>("VENUE_ID");

                    csv.WriteField<string>("VENUE_NAME");

                    csv.WriteField<string>("PROV_VENUE_ID");

                    csv.WriteField<string>("PHONE");

                    csv.WriteField<string>("ADDRESS_1");

                    csv.WriteField<string>("ADDRESS_2");

                    csv.WriteField<string>("TOWN");

                    csv.WriteField<string>("COUNTY");

                    csv.WriteField<string>("POSTCODE");

                    csv.WriteField<string>("EMAIL");

                    csv.WriteField<string>("WEBSITE");

                    csv.WriteField<string>("FAX");

                    csv.WriteField<string>("FACILITIES");

                    csv.WriteField<string>("DATE_CREATED");

                    csv.WriteField<string>("DATE_UPDATED");

                    csv.WriteField<string>("STATUS");

                    csv.WriteField<string>("UPDATED_BY");

                    csv.WriteField<string>("CREATED_BY");

                    csv.WriteField<string>("XMIN");

                    csv.WriteField<string>("XMAX");

                    csv.WriteField<string>("YMIN");

                    csv.WriteField<string>("YMAX");

                    csv.WriteField<string>("X_COORD");

                    csv.WriteField<string>("Y_COORD");

                    csv.WriteField<string>("SEARCH_REGION");

                    csv.WriteField<string>("SYS_DATA_SOURCE");

                    csv.WriteField<string>("DATE_UPDATED_COPY_OVER");

                    csv.WriteField<string>("DATE_CREATED_COPY_OVER");

                    csv.NextRecord();

                    foreach (var venueItem in venueItems.ToList())
                    {
                        csv.WriteField(venueItem.PROVIDER_ID);

                        csv.WriteField(venueItem.VENUE_ID);

                        csv.WriteField(venueItem.VENUE_NAME);

                        csv.WriteField(venueItem.PROV_VENUE_ID);

                        csv.WriteField(venueItem.PHONE);

                        csv.WriteField(venueItem.ADDRESS_1);

                        csv.WriteField(venueItem.ADDRESS_2);

                        csv.WriteField(venueItem.TOWN);

                        csv.WriteField(venueItem.COUNTY);

                        csv.WriteField(venueItem.POSTCODE);

                        csv.WriteField(venueItem.EMAIL);

                        csv.WriteField(venueItem.WEBSITE);

                        csv.WriteField(venueItem.FAX);

                        csv.WriteField(venueItem.FACILITIES);

                        csv.WriteField(venueItem.DATE_CREATED);

                        csv.WriteField(venueItem.DATE_UPDATE);

                        csv.WriteField(venueItem.STATUS);

                        csv.WriteField(venueItem.UPDATED_BY);

                        csv.WriteField(venueItem.CREATED_BY);

                        csv.WriteField(venueItem.XMIN);

                        csv.WriteField(venueItem.XMAX);

                        csv.WriteField(venueItem.YMIN);

                        csv.WriteField(venueItem.YMAX);

                        csv.WriteField(venueItem.X_COORD);

                        csv.WriteField(venueItem.Y_COORD);

                        csv.WriteField(venueItem.SEARCH_REGION);

                        csv.WriteField(venueItem.SYS_DATA_SOURCE);

                        csv.WriteField(venueItem.DATE_UPDATED_COPY_OVER);

                        csv.WriteField(venueItem.DATE_CREATED_COPY_OVER);

                        csv.NextRecord();
                    }
                }
            }
        }
    }
}
