using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMS.NCS.Dashboard.Entities;
using System.Text;

namespace IMS.NCS.CourseSearchService.TestHarness
{
    public partial class DashboardSearchCriteriaCtrl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Populates the search controls with the values from the search criteria.
        /// </summary>
        /// <param name="criteria">The populated search criteria.</param>
        public void PopulatePage(JobSearchCriteria criteria)
        {
            string scriptName = "PopulateCriteria";
            Type thisType = this.GetType();

            ClientScriptManager mgr = Page.ClientScript;
            if (!mgr.IsStartupScriptRegistered(thisType, scriptName))
            {
                StringBuilder scriptText = new StringBuilder();
                scriptText.Append("<script type=text/javascript>" + Environment.NewLine);

                if (criteria.StartDate != DateTime.MinValue)
                {
                    scriptText.Append(Utilities.GetJavascriptSetValueSnippet("StartDate", criteria.StartDate.ToString("dd/MM/yyyy")));
                }
                if (criteria.EndDate != DateTime.MinValue)
                {
                    scriptText.Append(Utilities.GetJavascriptSetValueSnippet("EndDate", criteria.EndDate.ToString("dd/MM/yyyy")));
                }

                scriptText.Append(Utilities.GetJavascriptSetValueSnippet("RecordsPerPage", criteria.RecordsPerPage.ToString()));
                // only set dropdowns if the user selected a value
                if (!string.IsNullOrEmpty(criteria.SortBy))
                {
                    scriptText.Append(Utilities.GetJavascriptSetValueSnippet("SortBy", criteria.SortBy));
                }

                if (criteria.InProgressJobs)
                {
                    scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("JobStatus_InProgress", "Y"));
                }

                if (criteria.CompletedJobs)
                {
                    scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("JobStatus_Complete", "Y"));
                }

                if (criteria.FailedJobs)
                {
                    scriptText.Append(Utilities.GetJavascriptSetCheckboxValueSnippet("JobStatus_Failed", "Y"));
                }

                // end
                scriptText.Append("</script>");

                mgr.RegisterStartupScript(thisType, scriptName, scriptText.ToString());
            }
        }
    }
}