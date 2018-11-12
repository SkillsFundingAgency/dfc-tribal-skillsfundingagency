using System;
using System.Linq;
using System.Web;
using IMS.NCS.CourseSearchService.TestHarness.Models;
using Ims.Schemas.Alse.CourseSearch.Contract;
using System.Collections.Specialized;

namespace IMS.NCS.CourseSearchService.TestHarness
{
    public partial class ProviderSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // check for query string action so we only perform the search function when requested
            string action = Utilities.GetQueryStringValue(Page.Request.QueryString, "action");
            if (action == "search")
            {
                // get the search criteria so we know what we're trying to find
                SearchCriteriaStructure criteria = CreateSearchCriteria(Page.Request.QueryString);
                
                // call the search and populate our results table
                PopulateData(criteria);

                // once we've displayed our results we need to set the display back to the
                // criteria values
                SearchControl.PopulatePage(criteria);
            }
        }


        /// <summary>
        /// Creates the search criteria from the QueryString.
        /// </summary>
        /// <param name="requestData">The query string data.</param>
        /// <returns>A SearchCriteriaStructure with data.</returns>
        private SearchCriteriaStructure CreateSearchCriteria(NameValueCollection requestData)
        {
            SearchCriteriaStructure criteria = new SearchCriteriaStructure();

            // API Key
            criteria.APIKey = Utilities.GetQueryStringValue(requestData, "APIKey");

            // PROVIDER
            criteria.ProviderKeyword = Utilities.GetQueryStringValue(requestData, "Provider");

            return criteria;
        }

        
        /// <summary>
        /// Creates the ui friendly class from the results.
        /// </summary>
        /// <param name="output">The results of the search.</param>
        /// <returns>A class containing the results</returns>
        private ProviderSearchResults CreateProviderSearchResults(ProviderSearchOutput output)
        {
            ProviderSearchResults results = new ProviderSearchResults();

            if (output != null 
                && output.ProviderSearchResponse != null 
                && output.ProviderSearchResponse.ProviderDetails != null)
            {
                foreach (ProviderStructure course in output.ProviderSearchResponse.ProviderDetails)
                {
                    results.Add(CreateResult(course));
                }
            }

            return results;
        }


        /// <summary>
        /// Gets the data and displays on screen if we have any.
        /// </summary>
        /// <param name="criteria"></param>
        private void PopulateData(SearchCriteriaStructure criteria)
        {
            try
            {
                // fire the web service to get some results
                ProviderSearchOutput output = GetResults(criteria);
                ProviderSearchResults results = CreateProviderSearchResults(output);

                if (results.Count() > 0)
                {
                    ResultsOverviewLabel.Text = "No of records: " + results.Count;
                    RepeaterContainer.DataSource = results; // pagedDS;
                    RepeaterContainer.DataBind();
                }
                else
                {
                    ResultsOverviewLabel.Text = "There are no results to display.";
                }
            }
            catch (Exception ex)
            {
                ResultsOverviewLabel.Text = ex.Message + "/n/n" + ex.StackTrace;
            }
        }


        /// <summary>
        /// Maps the provider data from the search into a class that we can
        /// more easily use as a datasource.
        /// </summary>
        /// <param name="course">A single set of provider data from the search.</param>
        /// <returns>A ProviderSearchResult of data from the input.</returns>
        private ProviderSearchResult CreateResult(ProviderStructure course)
        {
            ProviderSearchResult result = new ProviderSearchResult();

            if (course.Provider != null)
            {
                result.ProviderID = HttpUtility.HtmlEncode(course.Provider.ProviderID);
                result.ProviderName = HttpUtility.HtmlEncode(course.Provider.ProviderName);
                result.Email = HttpUtility.HtmlEncode(course.Provider.Email);
                result.Fax = HttpUtility.HtmlEncode(course.Provider.Fax);
                result.Phone = HttpUtility.HtmlEncode(course.Provider.Phone);
                result.Website = HttpUtility.HtmlEncode(course.Provider.Website);
                result.TFPlusLoans = course.Provider.TFPlusLoans;
                result.DFE1619Funded = course.Provider.DFE1619Funded;

                if (course.Provider.ProviderAddress != null)
                {
                    result.AddressLine1 = HttpUtility.HtmlEncode(course.Provider.ProviderAddress.Address_line_1);
                    result.AddressLine2 = HttpUtility.HtmlEncode(course.Provider.ProviderAddress.Address_line_2);
                    result.Town = HttpUtility.HtmlEncode(course.Provider.ProviderAddress.Town);
                    result.County = HttpUtility.HtmlEncode(course.Provider.ProviderAddress.County);
                    result.Postcode = HttpUtility.HtmlEncode(course.Provider.ProviderAddress.PostCode);
                }
            }
            
            return result;
        }


        /// <summary>
        /// Gets the results from the web service.
        /// </summary>
        /// <param name="criteria">The course search criteria.</param>
        /// <returns>A list of found courses.</returns>
        private ProviderSearchOutput GetResults(SearchCriteriaStructure criteria)
        {
            ProviderSearchOutput output = new ProviderSearchOutput();
            ServiceInterface client = new ServiceInterfaceClient("CourseSearchService");

            ProviderSearchRequestStructure listRequestStructure = new ProviderSearchRequestStructure
            {
                APIKey = criteria.APIKey, 
                ProviderKeyword = criteria.ProviderKeyword
            };

            ProviderSearchInput request = new ProviderSearchInput(listRequestStructure);
            try
            {
                output = client.ProviderSearch(request);
            }
            catch (Exception ex)
            {
                ResultsOverviewLabel.Text = ex.Message + "\n" + ex.StackTrace;
            }

            return output;
        }
    }
}
