using System;
using System.IO;
using System.Linq;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder
{
    public class OCourses : BuilderBase
    {
        protected readonly ProviderPortalEntities _db;
        private readonly Action<string> _logger;

        public OCourses(ProviderPortalEntities db, Action<string> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override void GenerateCsv()
        {
            _logger("Starting Courses CSV creation.");

            var couresItems = _db.up_CourseListForCsvExport();

            using (Stream stream = File.Open(Constants.O_Courses_CsvFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    var csv = new CsvWriter(writer);

                    csv.WriteField<string>("COURSE_ID");

                    csv.WriteField<string>("PROVIDER_ID");

                    csv.WriteField<string>("LAD_ID");

                    csv.WriteField<string>("PROVIDER_COURSE_TITLE");

                    csv.WriteField<string>("COURSE_SUMMARY");

                    csv.WriteField<string>("PROVIDER_COURSE_ID");

                    csv.WriteField<string>("COURSE_URL");

                    csv.WriteField<string>("BOOKING_URL");

                    csv.WriteField<string>("ENTRY_REQUIREMENTS");

                    csv.WriteField<string>("ASSESSMENT_METHOD");

                    csv.WriteField<string>("EQUIPMENT_REQUIRED");

                    csv.WriteField<string>("QUALIFICATION_TYPE");

                    csv.WriteField<string>("QUALIFICATION_TITLE");

                    csv.WriteField<string>("QUALIFICATION_LEVEL");

                    csv.WriteField<string>("LDCS1");

                    csv.WriteField<string>("LDCS2");

                    csv.WriteField<string>("LDCS3");

                    csv.WriteField<string>("LDCS4");

                    csv.WriteField<string>("LDCS5");

                    csv.WriteField<string>("DATA_SOURCE");

                    csv.WriteField<string>("UCAS_TARIFF");

                    csv.WriteField<string>("QUAL_REF_AUTHORITY");

                    csv.WriteField<string>("QUAL_REFERENCE");

                    csv.WriteField<string>("COURSE_TYPE_ID");

                    csv.WriteField<string>("DATE_CREATED");

                    csv.WriteField<string>("DATE_UPDATED");

                    csv.WriteField<string>("STATUS");

                    csv.WriteField<string>("AWARDING_ORG_NAME");

                    csv.WriteField<string>("UPDATED_BY");

                    csv.WriteField<string>("CREATED_BY");

                    csv.WriteField<string>("QUALIFICATION_TYPE_CODE");

                    csv.WriteField<string>("DATA_TYPE");

                    csv.WriteField<string>("SYS_DATA");

                    csv.WriteField<string>("DATE_UPDATED_COPY_OVER");

                    csv.WriteField<string>("DATE_CREATED_COPY_OVER");

                    csv.WriteField<string>("DFE_FUNDED");

                    csv.NextRecord();

                    foreach (var courseItem in couresItems.ToList())
                    {

                        csv.WriteField(courseItem.COURSE_ID);

                        csv.WriteField(courseItem.PROVIDER_ID);

                        csv.WriteField(courseItem.LAD_ID);

                        csv.WriteField(courseItem.PROVIDER_COURSE_TITLE);

                        csv.WriteField(courseItem.COURSE_SUMMARY);

                        csv.WriteField(courseItem.PROVIDER_COURSE_ID);

                        csv.WriteField(courseItem.COURSE_URL);

                        csv.WriteField(courseItem.BOOKING_URL);

                        csv.WriteField(courseItem.ENTRY_REQUIREMENTS);

                        csv.WriteField(courseItem.ASSESSMENT_METHOD);

                        csv.WriteField(courseItem.EQUIPMENT_REQUIRED);

                        csv.WriteField(courseItem.QUALIFICATION_TYPE);

                        csv.WriteField(courseItem.QUALIFICATION_TITLE);

                        csv.WriteField(courseItem.QUALIFICATION_LEVEL);

                        csv.WriteField(courseItem.LDCS1);

                        csv.WriteField(courseItem.LDCS2);

                        csv.WriteField(courseItem.LDCS3);

                        csv.WriteField(courseItem.LDCS4);

                        csv.WriteField(courseItem.LDCS5);

                        csv.WriteField(courseItem.SYS_DATA_SOURCE);

                        csv.WriteField(courseItem.UCAS_TARIFF);

                        csv.WriteField(courseItem.QUAL_REF_AUTHORITY);

                        csv.WriteField(courseItem.QUAL_REFERENCE);

                        csv.WriteField(courseItem.COURSE_TYPE_ID);

                        csv.WriteField(courseItem.DATE_CREATED);

                        csv.WriteField(courseItem.DATE_UPDATED);

                        csv.WriteField(courseItem.STATUS);

                        csv.WriteField(courseItem.AWARDING_ORG_NAME);

                        csv.WriteField(courseItem.UPDATED_BY);

                        csv.WriteField(courseItem.Created_By);

                        csv.WriteField(courseItem.QUALIFICATION_TYPE_CODE);

                        csv.WriteField(courseItem.DATA_TYPE);

                        csv.WriteField(courseItem.SYS_DATA);

                        csv.WriteField(courseItem.DATE_UPDATED_COPY_OVER);

                        csv.WriteField(courseItem.DATE_CREATED_COPY_OVER);

                        csv.WriteField(courseItem.DFE_FUNDED);

                        csv.NextRecord();
                    }
                }
            }
        }
    }
}
