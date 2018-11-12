using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder
{
    /************** NOT IN USE *******************************
     * This class is current not getting used.  As converting XLS to CSV on server will require licence for all production
     * servers.  Added file already convered to CSV to source and it will be copied to server withany any conversion.
     * Dont delete this class, which may be required in future in case changes are made in xls, this would convert to desired CSV */

    public class SLearningAIM_Old : BuilderBase
    {
        private OleDbConnection _conn;
        private OleDbCommand _cmd;
        private Action<string> _logger;

        public SLearningAIM_Old(ProviderPortalEntities db, Action<string> logger)
        {
            var a = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + a + "/Data/S_LEARNING_AIMS.xlsx;Extended Properties=\"Excel 12.0 Xml;HDR=YES;\"");
            _conn.Open();
            _cmd = new OleDbCommand { Connection = _conn, CommandText = "Select * from [S_LEARNING_AIMS$]" };
            _logger = logger;
        }

        public override void GenerateCsv()
        {
            _logger("Starting Learning AIM CSV creation.");

            using (Stream stream = File.Open(Constants.S_Learning_AIM_CsvFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    var csv = new CsvWriter(writer);

                    CreateHeader(csv);

                    var reader = _cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CreateRecord(csv, reader);
                    }

                    reader.Close();
                }
            }
            _conn.Close();
        }

        private static void CreateRecord(CsvWriter csv, IDataRecord reader)
        {
            csv.WriteField<string>(reader["LARA_RELEASE_VERSION"].ToString());

            csv.WriteField<string>(reader["LARA_DOWNLOAD_DATE"].ToString());

            csv.WriteField<string>(reader["LEARNING_AIM_REF"].ToString());

            csv.WriteField<string>(reader["LEARNING_AIM_TITLE"].ToString());

            csv.WriteField<string>(reader["LEARNING_AIM_TYPE_DESC"].ToString());

            csv.WriteField<string>(reader["AWARDING_BODY_NAME"].ToString());

            csv.WriteField<string>(reader["ENTRY_SUB_LEVEL_DESC"].ToString());

            csv.WriteField<string>(reader["NOTIONAL_LEVEL_V2_CODE"].ToString());

            csv.WriteField<string>(reader["NOTIONAL_LEVEL_V2_DESC"].ToString());

            csv.WriteField<string>(reader["CREDIT_BASED_TYPE_DESC"].ToString());

            csv.WriteField<string>(reader["QCA_GLH"].ToString());

            csv.WriteField<string>(reader["SECTOR_LEAD_BODY_DESC"].ToString());

            csv.WriteField<string>(reader["LEVEL2_ENTITLEMENT_CAT_DESC"].ToString());

            csv.WriteField<string>(reader["LEVEL3_ENTITLEMENT_CAT_DESC"].ToString());

            csv.WriteField<string>(reader["SKILLS_FOR_LIFE"].ToString());

            csv.WriteField<string>(reader["SKILLS_FOR_LIFE_TYPE_DESC"].ToString());

            csv.WriteField<string>(reader["SSA_TIER1_CODE"].ToString());

            csv.WriteField<string>(reader["SSA_TIER1_DESC"].ToString());

            csv.WriteField<string>(reader["SSA_TIER2_CODE"].ToString());

            csv.WriteField<string>(reader["SSA_TIER2_DESC"].ToString());

            csv.WriteField<string>(reader["LDCS_CODE_CODE"].ToString());

            csv.WriteField<string>(reader["ACCREDITATION_START_DATE"].ToString());

            csv.WriteField<string>(reader["ACCREDITATION_END_DATE"].ToString());

            csv.WriteField<string>(reader["CERTIFICATION_END_DATE"].ToString());

            csv.WriteField<string>(reader["FFA_CREDIT"].ToString());

            csv.WriteField<string>(reader["INDEP_LIVING_SKILLS"].ToString());

            csv.WriteField<string>(reader["ER_APP_STATUS"].ToString());

            csv.WriteField<string>(reader["ER_TTG_STATUS"].ToString());

            csv.WriteField<string>(reader["ADULTLR_STATUS"].ToString());

            csv.WriteField<string>(reader["OTHERFUNDING_NONFUNDEDSTATUS"].ToString());

            csv.WriteField<string>(reader["LEARNING_AIM_TYPE"].ToString());

            csv.WriteField<string>(reader["QUAL_REFERENCE_AUTHORITY"].ToString());

            csv.WriteField<string>(reader["QUALIFICATION_REFERENCE"].ToString());

            csv.WriteField<string>(reader["QUALIFICATION_TITLE"].ToString());

            csv.WriteField<string>(reader["QUALIFICATION_LEVEL"].ToString());

            csv.WriteField<string>(reader["QUALIFICATION_TYPE"].ToString());

            csv.WriteField<string>(reader["DATE_UPDATED"].ToString());

            csv.WriteField<string>(reader["QUALIFICATION_TYPE_CODE"].ToString());

            csv.WriteField<string>(reader["STATUS"].ToString());

            csv.WriteField<string>(reader["QUALIFICATION_LEVEL_CODE"].ToString());

            csv.WriteField<string>(reader["DATE_CREATED"].ToString());

            csv.WriteField<string>(reader["SOURCE_SYSTEM_REFERENCE"].ToString());

            csv.WriteField<string>(reader["SECTION_96_APPRVL_STATUS_CODE"].ToString());

            csv.WriteField<string>(reader["SECTION_96_APPRVL_STATUS_DESC"].ToString());

            csv.WriteField<string>(reader["SKLLS_FUNDNG_APPRV_STAT_CODE"].ToString());

            csv.WriteField<string>(reader["SKLLS_FUNDNG_APPRV_STAT_DESC"].ToString());

            csv.NextRecord();
        }

        private static void CreateHeader(CsvWriter csv)
        {

            csv.WriteField<string>("LARA_RELEASE_VERSION");

            csv.WriteField<string>("LARA_DOWNLOAD_DATE");

            csv.WriteField<string>("LEARNING_AIM_REF");

            csv.WriteField<string>("LEARNING_AIM_TITLE");

            csv.WriteField<string>("LEARNING_AIM_TYPE_DESC");

            csv.WriteField<string>("AWARDING_BODY_NAME");

            csv.WriteField<string>("ENTRY_SUB_LEVEL_DESC");

            csv.WriteField<string>("NOTIONAL_LEVEL_V2_CODE");

            csv.WriteField<string>("NOTIONAL_LEVEL_V2_DESC");

            csv.WriteField<string>("CREDIT_BASED_TYPE_DESC");

            csv.WriteField<string>("QCA_GLH");

            csv.WriteField<string>("SECTOR_LEAD_BODY_DESC");

            csv.WriteField<string>("LEVEL2_ENTITLEMENT_CAT_DESC");

            csv.WriteField<string>("LEVEL3_ENTITLEMENT_CAT_DESC");

            csv.WriteField<string>("SKILLS_FOR_LIFE");

            csv.WriteField<string>("SKILLS_FOR_LIFE_TYPE_DESC");

            csv.WriteField<string>("SSA_TIER1_CODE");

            csv.WriteField<string>("SSA_TIER1_DESC");

            csv.WriteField<string>("SSA_TIER2_CODE");

            csv.WriteField<string>("SSA_TIER2_DESC");

            csv.WriteField<string>("LDCS_CODE_CODE");

            csv.WriteField<string>("ACCREDITATION_START_DATE");

            csv.WriteField<string>("ACCREDITATION_END_DATE");

            csv.WriteField<string>("CERTIFICATION_END_DATE");

            csv.WriteField<string>("FFA_CREDIT");

            csv.WriteField<string>("INDEP_LIVING_SKILLS");

            csv.WriteField<string>("ER_APP_STATUS");

            csv.WriteField<string>("ER_TTG_STATUS");

            csv.WriteField<string>("ADULTLR_STATUS");

            csv.WriteField<string>("OTHERFUNDING_NONFUNDEDSTATUS");

            csv.WriteField<string>("LEARNING_AIM_TYPE");

            csv.WriteField<string>("QUAL_REFERENCE_AUTHORITY");

            csv.WriteField<string>("QUALIFICATION_REFERENCE");

            csv.WriteField<string>("QUALIFICATION_TITLE");

            csv.WriteField<string>("QUALIFICATION_LEVEL");

            csv.WriteField<string>("QUALIFICATION_TYPE");

            csv.WriteField<string>("DATE_UPDATED");

            csv.WriteField<string>("QUALIFICATION_TYPE_CODE");

            csv.WriteField<string>("STATUS");

            csv.WriteField<string>("QUALIFICATION_LEVEL_CODE");

            csv.WriteField<string>("DATE_CREATED");

            csv.WriteField<string>("SOURCE_SYSTEM_REFERENCE");

            csv.WriteField<string>("SECTION_96_APPRVL_STATUS_CODE");

            csv.WriteField<string>("SECTION_96_APPRVL_STATUS_DESC");

            csv.WriteField<string>("SKLLS_FUNDNG_APPRV_STAT_CODE");

            csv.WriteField<string>("SKLLS_FUNDNG_APPRV_STAT_DESC");

            csv.NextRecord();
        }

    }
}
