using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using IMS.NCS.Dashboard.BusinessServices;
using IMS.NCS.Dashboard.Entities;
using System.Configuration;
using System.Security.Principal;

namespace IMS.NCS.CourseSearchService.TestHarness
{
    public partial class DashboardDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // check for submit from search control
                string action = Utilities.GetQueryStringValue(Page.Request.QueryString, "action");

                // check for query string
                string tempJobId = Utilities.GetQueryStringValue(Page.Request.QueryString, Constants.QueryStrings.JobId);
                if (!string.IsNullOrEmpty(tempJobId))
                {
                    int jobId;
                    bool result = Int32.TryParse(tempJobId, out jobId);
                    if (result)
                    {
                        PopulateData(jobId);
                    }
                    else
                    {
                        RegisterAlert(string.Format("The JobId query string {0} cannot be parsed to a integer", tempJobId));
                    }
                }
            }
            else
            {
                // need to remove the viewstate for cross page redirects, otherwise we'll get a MAC validation error
                NameValueCollection newQueryString = Utilities.RemoveViewState(Page.Request.QueryString);
                Response.Redirect("DashboardSearch.aspx" + Utilities.CreateQueryString(newQueryString));
            }
        }


        /// <summary>
        /// Retrieves the Job data for the Job Id passed in and displays the data
        /// </summary>
        /// <param name="jobId">The id of the Job to view.</param>
        private void PopulateData(int jobId)
        {
            // get some results
            // need to impersonate as the service account so we can access the Import Control database.
            string serviceAccount = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.ServiceAccount].ToString();
            string domain = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.ServiceAccountDomain].ToString();
            string password = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.ServiceAccountPassword].ToString();

            WindowsImpersonationContext context = null;

            try
            {
                context = IMS.NCS.CourseSearchService.Common.Utilities.impersonateValidUser(serviceAccount, domain, password);
                if (context != null)
                {
                    IDashboardService service = new DashboardService();
                    DashboardDetailJob jobDetails = service.GetJobDetails(jobId);

                    if (jobDetails != null && jobDetails.DetailJob != null)
                    {
                        JobTitle.Text = "JOB - " + jobDetails.DetailJob.JobName;
                        JobId.Text = jobDetails.DetailJob.JobId.ToString();
                        TimeElapsed.Text = jobDetails.DetailJob.ElapsedTime.ToString();

                        if (jobDetails.DetailJob.ProcessStart != null)
                        {
                            StartDate.Text = jobDetails.DetailJob.ProcessStart.ToString("dd/MM/yyyy HH:mm:ss");
                        }
                        else
                        {
                            StartDate.Text = "N/A";
                        }

                        if (jobDetails.DetailJob.ProcessEnd != null)
                        {
                            EndDate.Text = jobDetails.DetailJob.ProcessEnd.ToString("dd/MM/yyyy HH:mm:ss");
                        }
                        else
                        {
                            EndDate.Text = "N/A";
                        }

                        divResults.Visible = true;

                        gridSteps.DataSource = jobDetails.Steps;
                        gridSteps.DataBind();
                    }
                    else
                    {
                        divResults.Visible = false;
                        ResultsOverviewLabel.Text = "There are no results to display.";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultsOverviewLabel.Text = ex.Message + "\n" + ex.StackTrace;
            }
            finally
            {
                if (context != null)
                {
                    IMS.NCS.CourseSearchService.Common.Utilities.UndoImpersonation(context);
                }
            }
        }


        /// <summary>
        /// Registers an Alert message from the text passed in.
        /// </summary>
        /// <param name="alertText">the text to display in the alert.</param>
        private void RegisterAlert(string alertText)
        {
            string script = "alert('" + alertText + "');\n";
            Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", script, true);
        }
    }
}