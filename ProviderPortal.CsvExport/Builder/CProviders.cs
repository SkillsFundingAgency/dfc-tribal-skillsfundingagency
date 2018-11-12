using System;
using System.IO;
using System.Linq;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder
{
    public class CProviders : BuilderBase
    {
        protected readonly ProviderPortalEntities _db;
        private Action<string> _logger;

        public CProviders(ProviderPortalEntities db, Action<string> logger)
        {
            _db = db;
            _logger = logger;

            if (!Directory.Exists(Constants.NightlyCsvZipFolderPath))
                Directory.CreateDirectory(Constants.NightlyCsvZipFolderPath);
        }

        public override void GenerateCsv()
        {
            _logger("Starting Providers CSV creation.");

            var providerList = _db.up_ProviderListForCsvExport();

            using (Stream stream = File.Open(Constants.C_Providers_CsvFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    var csv = new CsvWriter(writer);

                    csv.WriteField<string>("PROVIDER_ID");

                    csv.WriteField<string>("PROVIDER_NAME");

                    csv.WriteField<string>("UKPRN");

                    csv.WriteField<string>("PROVIDER_TYPE_ID");
                    csv.WriteField<string>("PROVIDER_TYPE_DESCRIPTION");

                    csv.WriteField<string>("EMAIL");

                    csv.WriteField<string>("WEBSITE");

                    csv.WriteField<string>("PHONE");

                    csv.WriteField<string>("FAX");

                    csv.WriteField<string>("PROV_TRADING_NAME");

                    csv.WriteField<string>("PROV_LEGAL_NAME");

                    csv.WriteField<string>("LSC_SUPPLIER_NO");

                    csv.WriteField<string>("PROV_ALIAS");

                    csv.WriteField<string>("DATE_CREATED");

                    csv.WriteField<string>("DATE_UPDATED");

                    csv.WriteField<string>("TTG_FLAG");

                    csv.WriteField<string>("TQS_FLAG");

                    csv.WriteField<string>("IES_FLAG");

                    csv.WriteField<string>("STATUS");

                    csv.WriteField<string>("UPDATED_BY");

                    csv.WriteField<string>("CREATED_BY");

                    csv.WriteField<string>("ADDRESS_1");

                    csv.WriteField<string>("ADDRESS_2");

                    csv.WriteField<string>("TOWN");

                    csv.WriteField<string>("COUNTY");

                    csv.WriteField<string>("POSTCODE");

                    csv.WriteField<string>("SYS_DATA_SOURCE");

                    csv.WriteField<string>("DATE_UPDATED_COPY_OVER");

                    csv.WriteField<string>("DATE_CREATED_COPY_OVER");

                    csv.WriteField<string>("DFE_PROVIDER_TYPE_ID");
                    csv.WriteField<string>("DFE_PROVIDER_TYPE_DESCRIPTION");
                    csv.WriteField<string>("DFE_LOCAL_AUTHORITY_CODE");
                    csv.WriteField<string>("DFE_LOCAL_AUTHORITY_DESCRIPTION");
                    csv.WriteField<string>("DFE_REGION_CODE");
                    csv.WriteField<string>("DFE_REGION_DESCRIPTION");
                    csv.WriteField<string>("DFE_ESTABLISHMENT_TYPE_CODE");
                    csv.WriteField<string>("DFE_ESTABLISHMENT_TYPE_DESCRIPTION");

                    csv.NextRecord();

                    foreach (var providerListItem in providerList.ToList())
                    {

                        csv.WriteField(providerListItem.PROVIDER_ID);

                        csv.WriteField(providerListItem.PROVIDER_NAME);

                        csv.WriteField(providerListItem.UKPRN);

                        csv.WriteField(providerListItem.PROVIDER_TYPE_ID);
                        csv.WriteField(providerListItem.PROVIDER_TYPE_DESCRIPTION);

                        csv.WriteField(providerListItem.EMAIL);

                        csv.WriteField(providerListItem.WEBSITE);

                        csv.WriteField(providerListItem.PHONE);

                        csv.WriteField(providerListItem.FAX);

                        csv.WriteField(providerListItem.PROV_TRADING_NAME);

                        csv.WriteField(providerListItem.PROV_LEGAL_NAME);

                        csv.WriteField(providerListItem.LSC_SUPPLIER_NO.Equals(0) ? string.Empty : providerListItem.LSC_SUPPLIER_NO.ToString());

                        csv.WriteField(providerListItem.PROV_ALIAS);

                        csv.WriteField(providerListItem.DATE_CREATED);

                        csv.WriteField(providerListItem.DATE_UPDATED);

                        csv.WriteField(providerListItem.TTG_FLAG);

                        csv.WriteField(providerListItem.TQS_FLAG);

                        csv.WriteField(providerListItem.IES_FLAG);

                        csv.WriteField(providerListItem.STATUS);

                        csv.WriteField(providerListItem.UPDATED_BY);

                        csv.WriteField(providerListItem.CREATED_BY);

                        csv.WriteField(providerListItem.ADDRESS_1);

                        csv.WriteField(providerListItem.ADDRESS_2);

                        csv.WriteField(providerListItem.TOWN);

                        csv.WriteField(providerListItem.COUNTY);

                        csv.WriteField(providerListItem.POSTCODE);

                        csv.WriteField(providerListItem.SYS_DATA_SOURCE);

                        csv.WriteField(providerListItem.DATE_UPDATED_COPY_OVER);

                        csv.WriteField(providerListItem.DATE_CREATED_COPY_OVER);

                        csv.WriteField(providerListItem.DFE_PROVIDER_TYPE_ID);
                        csv.WriteField(providerListItem.DFE_PROVIDER_TYPE_DESCRIPTION);
                        csv.WriteField(providerListItem.DFE_LOCAL_AUTHORITY_CODE);
                        csv.WriteField(providerListItem.DFE_LOCAL_AUTHORITY_DESCRIPTION);
                        csv.WriteField(providerListItem.DFE_REGION_CODE);
                        csv.WriteField(providerListItem.DFE_REGION_DESCRIPTION);
                        csv.WriteField(providerListItem.DFE_ESTABLISHMENT_TYPE_CODE);
                        csv.WriteField(providerListItem.DFE_ESTABLISHMENT_TYPE_DESCRIPTION);

                        csv.NextRecord();
                    }
                }
            }
        }
    }
}
