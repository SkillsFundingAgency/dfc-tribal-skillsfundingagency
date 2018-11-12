using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ims.Schemas.Alse.CourseSearch.Contract;
using System.Text;

namespace IMS.NCS.CourseSearchService.TestHarness
{
    public partial class CourseSearchCriteriaCtrl : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            phAPIKey.Visible = !ConfigurationManager.AppSettings[Constants.ConfigurationKeys.Environment].Contains("NCS Search");
        }

        /// <summary>
        /// Populates the search controls with the values from the search criteria.
        /// </summary>
        /// <param name="criteria">The populated search criteria.</param>
        /// <param name="sortBy">The sort by value.</param>
        /// <param name="recordsPerPage">The records per page value.</param>
        public void PopulatePage(SearchCriteriaStructure criteria, string sortBy, string recordsPerPage)
        {
            string scriptName = "PopulateCriteria";
            Type thisType = this.GetType();

            ClientScriptManager mgr = Page.ClientScript;
            if (!mgr.IsStartupScriptRegistered(thisType, scriptName))
            {
                StringBuilder scriptText = new StringBuilder();
                scriptText.Append("<script type=text/javascript>" + Environment.NewLine);

                //API Key
                if (phAPIKey.Visible)
                {
                    scriptText.Append(Utilities.GetJavascriptSetValueSnippet("APIKey", criteria.APIKey));
                }

                // SUBJECT
                scriptText.Append(Utilities.GetJavascriptSetValueSnippet("Subject", criteria.SubjectKeyword));
                if (criteria.LDCS != null)
                {
                    scriptText.Append(Utilities.GetJavascriptSetValueSnippet("LDCSCategoryCodes", criteria.LDCS.CategoryCode));
                }
                scriptText.Append(Utilities.GetJavascriptSetValueSnippet("DfEFundedOnly", criteria.DFE1619Funded));

                // REGION
                scriptText.Append(Utilities.GetJavascriptSetValueSnippet("LocationPostCode", criteria.Location));
                scriptText.Append(Utilities.GetJavascriptSetValueSnippet("RecordsPerPage", recordsPerPage));
                // only set dropdowns if the user selected a value
                if (criteria.Distance > 0)
                {
                    scriptText.Append(Utilities.GetJavascriptSetValueSnippet("MaxDistance", criteria.Distance.ToString()));
                }

                if (!string.IsNullOrEmpty(sortBy))
                {
                    scriptText.Append(Utilities.GetJavascriptSetValueSnippet("SortBy", sortBy));
                }


                // PROVIDER
                scriptText.Append(Utilities.GetJavascriptSetValueSnippet("ProviderID", criteria.ProviderID));
                scriptText.Append(Utilities.GetJavascriptSetValueSnippet("ProviderText", criteria.ProviderKeyword));

                // QUALIFICATION
                if (criteria.QualificationTypes != null)
                {
                    scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("QualificationTypes", criteria.QualificationTypes.QualificationType));
                }

                if (criteria.QualificationLevels != null)
                {
                    scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("QualificationLevels", criteria.QualificationLevels.QualificationLevel));
                }


                // STUDY MODE AND ATTENDANCE
                scriptText.Append(Utilities.GetJavascriptSetValueSnippet("EarliestStartDate", criteria.EarliestStartDate));

                if (criteria.StudyModes != null)
                {
                    scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("StudyModes", criteria.StudyModes.StudyMode));
                }

                if (criteria.AttendanceModes != null)
                {
                    scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("AttendanceModes", criteria.AttendanceModes.AttendanceMode));
                }

                if (criteria.AttendancePatterns != null)
                {
                    scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("AttendancePatterns", criteria.AttendancePatterns.AttendancePattern));
                }


                // FLAGS
                scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("IncFlexibleStartDateFlag", criteria.FlexStartFlag));
                scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("IncIfOpportunityApplicationClosedFlag", criteria.OppsAppClosedFlag));
                scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("IncTTGFlag", criteria.TTGFlag));
                scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("IncTQSFlag", criteria.TQSFlag));
                scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("IncIESFlag", criteria.IESFlag));

                if (criteria.A10Codes != null)
                {
                    scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("A10Flags", criteria.A10Codes.A10Code));
                }

                scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("IndLivingSkillsFlag", criteria.ILSFlag));
                scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("SkillsForLifeFlag", criteria.SFLFlag));

                // STATUS
                scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("ERAppStatus", criteria.ERAppStatus));
                scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("ERTTGStatus", criteria.ERTtgStatus));
                scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("AdultLRStatus", criteria.AdultLRStatus));
                scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("OtherFundingStatus", criteria.OtherFundingStatus));

                // end
                scriptText.Append("</script>");

                mgr.RegisterStartupScript(thisType, scriptName, scriptText.ToString());
            }
        }
    }
}