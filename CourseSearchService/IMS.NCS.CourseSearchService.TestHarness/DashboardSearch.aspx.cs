using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMS.NCS.Dashboard.Entities;
using System.Collections.Specialized;
using IMS.NCS.Dashboard.BusinessServices;
using System.Configuration;
using System.IO;
using System.Security.Principal;

namespace IMS.NCS.CourseSearchService.TestHarness
{
    public partial class DashboardSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JobSearchCriteria criteria;
            string action = Utilities.GetQueryStringValue(Page.Request.QueryString, "action");

            // check for query string action so we only perform the correct function when requested
            switch (action)
            {
                case "search":

                    criteria = CreateSearchCriteria(Page.Request.QueryString);

                    // always getting the first page of data so pass 1 for the page
                    PopulateData(criteria);

                    // once we've displayed our results we need to set the display back to the
                    // criteria values
                    SearchControl.PopulatePage(criteria);
                    break;

                case "pagenav":

                    criteria = CreateSearchCriteria(Page.Request.QueryString);

                    // get the page of data as requested in the QueryString
                    PopulateData(criteria);

                    // once we've displayed our results we need to set the display back to the
                    // criteria values
                    SearchControl.PopulatePage(criteria);
                    break;

                case "remove":
                    RemoveOldFiles();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Creates the search criteria from the QueryString.
        /// </summary>
        /// <param name="requestData">The query string data.</param>
        /// <returns>A JobSearchCriteria with data.</returns>
        private JobSearchCriteria CreateSearchCriteria(NameValueCollection requestData)
        {
            bool result;
            JobSearchCriteria criteria = new JobSearchCriteria();
            DateTime tempDateTime;

            result = DateTime.TryParse(Utilities.GetQueryStringValue(requestData, "EndDate"), out tempDateTime);
            if (result)
            {
                criteria.EndDate = tempDateTime.Date;
            }

            result = DateTime.TryParse(Utilities.GetQueryStringValue(requestData, "StartDate"), out tempDateTime);
            if (result)
            {
                criteria.StartDate = tempDateTime.Date;
            }

            string[] jobStatuses = Utilities.GetQueryStringValues(Page.Request.QueryString, "JobStatus");
            criteria.CompletedJobs = jobStatuses != null && jobStatuses.Contains("Completed");
            criteria.InProgressJobs = jobStatuses != null && jobStatuses.Contains("InProgress");
            criteria.FailedJobs = jobStatuses != null && jobStatuses.Contains("Failed");

            // default SortBy if the user hasn't entered one
            criteria.SortBy = Utilities.GetQueryStringValue(Page.Request.QueryString, Constants.QueryStrings.SortBy);
            criteria.SortBy = criteria.SortBy == string.Empty ? "StartDate" : criteria.SortBy;

            criteria.RecordsPerPage = Int32.Parse(Utilities.GetQueryStringValue(Page.Request.QueryString, Constants.QueryStrings.RecordsPerPage));
            criteria.RecordsPerPage = criteria.RecordsPerPage == 0 ? 10 : criteria.RecordsPerPage;

            // default to first page if next page not set
            string nextPage = Utilities.GetQueryStringValue(Page.Request.QueryString, Constants.QueryStrings.GetPage);
            criteria.NextPage = string.IsNullOrEmpty(nextPage) ? 1 : Int32.Parse(nextPage);

            return criteria;
        }

        /// Gets the search results and displays on the screen.
        /// </summary>
        /// <param name="criteria">The job search criteria.</param>
        private void PopulateData(JobSearchCriteria criteria)
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
                    DashboardJob dashboardJob = service.GetJobList(criteria);

                    if (dashboardJob != null && dashboardJob.Jobs.Count() > 0)
                    {
                        divSearchResults.Visible = true;

                        ResultsOverviewLabel.Text = String.Format(Constants.StringFormats.ResultsOverviewStringFormat, dashboardJob.CurrentPageNo, dashboardJob.TotalPages, dashboardJob.TotalRecords);

                        SetupPageNavButtons(dashboardJob.TotalPages, dashboardJob.CurrentPageNo);

                        gridJobs.DataSource = dashboardJob.Jobs;
                        gridJobs.DataBind();
                    }
                    else
                    {
                        DisplayNoResults();
                    }
                }
                else
                {
                    ResultsOverviewLabel.Text = "The service account cannot be impersonated.";
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
        /// Sets up the First, Prev, Next and Last buttons based on
        /// the current page and total number of pages.
        /// </summary>
        /// <param name="totalPages">The total number of pages we have to display.</param>
        /// <param name="currentPageNo">The current page number.</param>
        private void SetupPageNavButtons(int totalPages, int currentPageNo)
        {
            // we want to see all the buttons, we'll enable / disable them after
            cmdFirst.Visible = true;
            cmdPrevious.Visible = true;
            cmdNext.Visible = true;
            cmdLast.Visible = true;

            // first enabled if we're not on the first page
            cmdFirst.Enabled = currentPageNo > 1;

            // last enabled if we're not on the last page
            cmdLast.Enabled = currentPageNo < totalPages;

            // previous enabled if we have a previous page to go back to
            cmdPrevious.Enabled = currentPageNo > 1;

            // next enabled if we have a page to go forward to.
            cmdNext.Enabled = currentPageNo < totalPages;

            // if any of the buttons are enabled, we need to set the correct pages to navigate to
            if (cmdFirst.Enabled)
            {
                cmdFirst.OnClientClick = String.Format("javascript:navigatePage(1);");
            }

            if (cmdPrevious.Enabled)
            {
                cmdPrevious.OnClientClick = String.Format(Constants.StringFormats.JavascriptNavigatePage, (currentPageNo - 1).ToString());
            }

            if (cmdNext.Enabled)
            {
                cmdNext.OnClientClick = String.Format(Constants.StringFormats.JavascriptNavigatePage, (currentPageNo + 1).ToString());
            }

            if (cmdLast.Enabled)
            {
                cmdLast.OnClientClick = String.Format(Constants.StringFormats.JavascriptNavigatePage, totalPages);
            }
        }


        /// <summary>
        /// Sets up page based on not having any results.
        /// </summary>
        private void DisplayNoResults()
        {
            cmdPrevious.Visible = false;
            cmdNext.Visible = false;
            ResultsOverviewLabel.Text = "There are no results to display.";
        }

        private void RemoveOldFiles()
        {
            // need to impersonate as the service account so we can access the share folder
            string serviceAccount = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.ServiceAccount].ToString();
            string domain = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.ServiceAccountDomain].ToString();
            string password = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.ServiceAccountPassword].ToString();

            WindowsImpersonationContext context = null;

            try
            {
                context = IMS.NCS.CourseSearchService.Common.Utilities.impersonateValidUser(serviceAccount, domain, password);
                if (context != null)
                {
                    string fileSharePath = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.FILESRVLocation].ToString();
                    if (Directory.Exists(fileSharePath))
                    {
                        // get a list of files that are older than 7 days
                        // get our date in the past
                        string tempdaysAge = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.ZipFileDeletionAge].ToString();
                        int daysAge;
                        bool result = Int32.TryParse(tempdaysAge, out daysAge);

                        if (result)
                        {
                            // need to use a negative value to return the date in the past
                            DateTime ageDaysDate = DateTime.Now.AddDays(daysAge * -1);

                            DirectoryInfo fileShareDirectory = new DirectoryInfo(fileSharePath);
                            FileInfo[] files = fileShareDirectory.GetFiles("*.zip");
                            int fileDeleteCount = 0;

                            // iterate through each file and delete any that are older than our 
                            foreach (FileInfo file in files)
                            {
                                if (file.LastWriteTime.Date < ageDaysDate.Date)
                                {
                                    file.Delete();
                                    fileDeleteCount++;
                                }
                            }

                            string script = string.Format("{0} file(s) have been deleted.", fileDeleteCount.ToString());
                            RegisterAlert(script);
                        }
                        else
                        {
                            RegisterAlert(string.Format("Invalid configuarion value {0} for ZipFileDeletionAge", tempdaysAge));
                        }
                    }
                    else
                    {
                        RegisterAlert(string.Format("The File Share path {0} does not exist", fileSharePath));
                    }
                }
                else
                {
                    ResultsOverviewLabel.Text = "The service account cannot be impersonated.";
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