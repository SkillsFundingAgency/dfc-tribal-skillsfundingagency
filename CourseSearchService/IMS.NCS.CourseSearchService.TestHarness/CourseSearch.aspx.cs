using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Ims.Schemas.Alse.CourseSearch.Contract;
using IMS.NCS.CourseSearchService.TestHarness.Models;

namespace IMS.NCS.CourseSearchService.TestHarness
{
    public partial class CourseSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SearchCriteriaStructure criteria;
            string sortBy;
            string recordsPerPage;
            string action = Utilities.GetQueryStringValue(Page.Request.QueryString, "action");

            // check for query string action so we only perform the correct function when requested
            switch (action)
            { 
                case "search":

                    criteria = CreateSearchCriteria(Page.Request.QueryString);
                    sortBy = Utilities.GetQueryStringValue(Page.Request.QueryString, Constants.QueryStrings.SortBy);
                    recordsPerPage = Utilities.GetQueryStringValue(Page.Request.QueryString, Constants.QueryStrings.RecordsPerPage);
                    
                    // always getting the first page of data so pass 1 for the page
                    PopulateData(criteria, sortBy, recordsPerPage, Constants.InitialPageNumber);

                    // once we've displayed our results we need to set the display back to the
                    // criteria values
                    SearchControl.PopulatePage(criteria, sortBy, recordsPerPage);
                    break;

                case "pagenav":

                    criteria = CreateSearchCriteria(Page.Request.QueryString);
                    sortBy = Utilities.GetQueryStringValue(Page.Request.QueryString, Constants.QueryStrings.SortBy);
                    recordsPerPage = Utilities.GetQueryStringValue(Page.Request.QueryString, Constants.QueryStrings.RecordsPerPage);
                    string nextPage = Utilities.GetQueryStringValue(Page.Request.QueryString, Constants.QueryStrings.GetPage);

                    // get the page of data as requested in the QueryString
                    PopulateData(criteria, sortBy, recordsPerPage, nextPage);

                    // once we've displayed our results we need to set the display back to the
                    // criteria values
                    SearchControl.PopulatePage(criteria, sortBy, recordsPerPage);
                    break;

                case "compare":

                    string[] selectedCourses = Utilities.GetCheckboxValues(Page.Request.QueryString, "coursechk");
                    string newQueryString = Utilities.CreateQueryString(selectedCourses, Constants.QueryStrings.CourseId);
                    Response.Redirect("CourseDetail.aspx" + newQueryString);
                    break;
            }
        }


        /// <summary>
        /// Creates the search criteria from the QueryString.
        /// </summary>
        /// <param name="requestData">The query string data.</param>
        /// <returns>A SearchCriteriaStructure with data.</returns>
        private SearchCriteriaStructure CreateSearchCriteria(NameValueCollection requestData)
        {
            SearchCriteriaStructure criteria = new SearchCriteriaStructure
            {
                //APIKey
                APIKey = Utilities.GetQueryStringValue(requestData, "APIKey"),
                // SUBJECT
                SubjectKeyword = Utilities.GetQueryStringValue(requestData, "Subject")
            };
            
            string[] tempResults = Utilities.GetQueryStringValues(requestData, "LDCSCategoryCodes");
            if (tempResults != null)
            {
                criteria.LDCS = new LDCSInputType
                {
                    CategoryCode = tempResults
                };
            }
            criteria.DFE1619Funded = Utilities.GetQueryStringValue(requestData, "DfEFundedOnly"); //Utilities.GetCheckboxValue(requestData, "DfEFundedOnly");

            // REGION
            criteria.Location = Utilities.GetQueryStringValue(requestData, "LocationPostCode");
            criteria.Distance = Utilities.GetQueryStringValueAsFloat(requestData, "MaxDistance");
            if (criteria.Distance > 0)
            {
                criteria.DistanceSpecified = true;
            }

            // PROVIDER
            criteria.ProviderID = Utilities.GetQueryStringValue(requestData, "ProviderId");
            criteria.ProviderKeyword = Utilities.GetQueryStringValue(requestData, "ProviderText");

            // QUALIFICATION
            criteria.QualificationTypes = Utilities.GetQueryStringQualificationTypes(requestData, "QualificationTypes");
            criteria.QualificationLevels = Utilities.GetQueryStringQualificationLevels(requestData, "QualificationLevels");

            // STUDY MODE AND ATTENDANCE
            criteria.EarliestStartDate = Utilities.GetQueryStringValue(requestData, "EarliestStartDate");
            criteria.StudyModes = Utilities.GetQueryStringStudyModeType(requestData, "StudyModes");
            criteria.AttendanceModes = Utilities.GetQueryStringAttendanceModeType(requestData, "AttendanceModes");
            criteria.AttendancePatterns = Utilities.GetQueryStringAttendancePatternType(requestData, "AttendancePatterns");

            // FLAGS
            criteria.FlexStartFlag = Utilities.GetCheckboxValue(requestData, "IncFlexibleStartDateFlag");
            criteria.OppsAppClosedFlag = Utilities.GetCheckboxValue(requestData, "IncIfOpportunityApplicationClosedFlag");
            criteria.TTGFlag = Utilities.GetCheckboxValue(requestData, "IncTTGFlag");
            criteria.TQSFlag = Utilities.GetCheckboxValue(requestData, "IncTQSFlag");
            criteria.IESFlag = Utilities.GetCheckboxValue(requestData, "IncIESFlag");
            criteria.A10Codes = Utilities.GetQueryStringA10Codes(requestData, "A10Flags");
            criteria.ILSFlag = Utilities.GetCheckboxValue(requestData, "IndLivingSkillsFlag");
            criteria.SFLFlag = Utilities.GetCheckboxValue(requestData, "SkillsForLifeFlag");

            // STATUS
            criteria.ERAppStatus = Utilities.GetCheckboxValues(requestData, "ERAppStatus");
            criteria.ERTtgStatus = Utilities.GetCheckboxValues(requestData, "ERTTGStatus");
            criteria.AdultLRStatus = Utilities.GetCheckboxValues(requestData, "AdultLRStatus");
            criteria.OtherFundingStatus = Utilities.GetCheckboxValues(requestData, "OtherFundingStatus");

            return criteria;
        }


        /// <summary>
        /// Gets the search results and displays on the screen.
        /// </summary>
        /// <param name="criteria">The course search criteria.</param>
        /// <param name="sortBy">The column to sort by.</param>
        /// <param name="recordsPerPage">The number of records per page.</param>
        /// <param name="pageNo">The page number</param>
        private void PopulateData(SearchCriteriaStructure criteria, string sortBy, string recordsPerPage, string pageNo)
        {
            // fire the web service to get some results
            CourseListOutput output = GetResults(criteria, sortBy, recordsPerPage, pageNo);

            if (output != null)
            {
                CourseSearchResults results = CreateCourseSearchResults(output);

                if (results.Any())
                {
                    int totalPages;
                    {
                        int.TryParse(output.CourseListResponse.ResultInfo.NoOfPages, out totalPages);
                    }

                    int currentPageNo;
                    {
                        int.TryParse(output.CourseListResponse.ResultInfo.PageNo, out currentPageNo);
                    }

                    int totalRecords;
                    {
                        int.TryParse(output.CourseListResponse.ResultInfo.NoOfRecords, out totalRecords);
                    }
                    
                    divResults.Visible = true;

                    // only need to display compare button if we have more than 1 record
                    cmdCompare.Visible = results.Count() > 1;
                    int pageSize;
                    int.TryParse(recordsPerPage, out pageSize);

                    ResultsOverviewLabel.Text = String.Format(Constants.StringFormats.ResultsOverviewStringFormat, currentPageNo, totalPages, totalRecords);

                    SetupPageNavButtons(totalPages, currentPageNo);

                    CourseResultsRepeater.DataSource = results;
                    CourseResultsRepeater.DataBind();

                    if (results.MatchingLDCSCodes != null
                        && results.MatchingLDCSCodes.Count > 1)
                    {
                        MatchingLDCSCodesRepeater.DataSource = results.MatchingLDCSCodes;
                        MatchingLDCSCodesRepeater.DataBind();
                    }
                }
                else
                {
                    DisplayNoResults();
                }
            }
            else
            {
                DisplayNoResults();
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


        /// <summary>
        /// Creates the set of results from the search response.
        /// </summary>
        /// <param name="output">The results from the search.</param>
        /// <returns>A populated CourseSearchResults object.</returns>
        private CourseSearchResults CreateCourseSearchResults(CourseListOutput output)
        {
            CourseSearchResults results = new CourseSearchResults();

            if (output != null 
                && output.CourseListResponse != null )
            {
                if (output.CourseListResponse.CourseDetails != null)
                {

                    foreach (CourseStructure course in output.CourseListResponse.CourseDetails)
                    {
                        results.Add(CreateResult(course));
                    }
                }

                if (output.CourseListResponse.MatchingLDCS != null)
                {
                    results.MatchingLDCSCodes = new List<LDCSCode>();
                    foreach (CourseListResponseStructureMatchingLDCS ldcsCode in output.CourseListResponse.MatchingLDCS)
                    { 
                        results.MatchingLDCSCodes.Add(CreateLDCS(ldcsCode));
                    }
                }
            }

            return results;
        }


        /// <summary>
        /// Creates a repeater friendly CourseSearchResult object from the response.
        /// </summary>
        /// <param name="course">The CourseStructure response from the search.</param>
        /// <returns>A populated CourseSearchResult object.</returns>
        private CourseSearchResult CreateResult(CourseStructure course)
        {
            CourseSearchResult result = new CourseSearchResult();

            if (course != null && course.Course != null)
            {
                // COURSE
                result.CourseID = HttpUtility.HtmlEncode(course.Course.CourseID);
                result.CourseName = HttpUtility.HtmlEncode(course.Course.CourseTitle);
                result.CourseSummary = HttpUtility.HtmlEncode(course.Course.CourseSummary);

                result.CourseDfEFunded = course.Opportunity.DFE1619Funded;

                result.LDCS1 = HttpUtility.HtmlEncode(course.Course.LDCS.CatCode1.LDCSCode);
                result.LDCS1Description = HttpUtility.HtmlEncode(course.Course.LDCS.CatCode1.LDCSDesc);
                result.LDCS2 = HttpUtility.HtmlEncode(course.Course.LDCS.CatCode2.LDCSCode);
                result.LDCS2Description = HttpUtility.HtmlEncode(course.Course.LDCS.CatCode2.LDCSDesc);
                result.LDCS3 = HttpUtility.HtmlEncode(course.Course.LDCS.CatCode3.LDCSCode);
                result.LDCS3Description = HttpUtility.HtmlEncode(course.Course.LDCS.CatCode3.LDCSDesc);
                result.LDCS4 = HttpUtility.HtmlEncode(course.Course.LDCS.CatCode4.LDCSCode);
                result.LDCS4Description = HttpUtility.HtmlEncode(course.Course.LDCS.CatCode4.LDCSDesc);
                result.LDCS5 = HttpUtility.HtmlEncode(course.Course.LDCS.CatCode5.LDCSCode);
                result.LDCS5Description = HttpUtility.HtmlEncode(course.Course.LDCS.CatCode5.LDCSDesc);

                result.NumberOfOpportunities = HttpUtility.HtmlEncode(course.Course.NoOfOpps);

                result.QualificationLevel = HttpUtility.HtmlEncode(course.Course.QualificationLevel);
                result.QualificationType = HttpUtility.HtmlEncode(course.Course.QualificationType);

                // OPPORTUNITY
                if (course.Opportunity != null)
                {
                    result.OpportunityID = HttpUtility.HtmlEncode(course.Opportunity.OpportunityId);
                    result.AttendanceMode = HttpUtility.HtmlEncode(course.Opportunity.AttendanceMode);
                    result.AttendancePattern = HttpUtility.HtmlEncode(course.Opportunity.AttendancePattern);

                    if (course.Opportunity.Duration != null)
                    {
                        result.DurationDescription = HttpUtility.HtmlEncode(course.Opportunity.Duration.DurationDescription);
                        result.DurationUnit = HttpUtility.HtmlEncode(course.Opportunity.Duration.DurationUnit);
                        result.DurationValue = HttpUtility.HtmlEncode(course.Opportunity.Duration.DurationValue);
                    }

                    if (course.Opportunity.Item != null)
                    {
                        // could be VenueInfo or a Region name
                        if (course.Opportunity.Item.GetType() == typeof(VenueInfo))
                        {
                            VenueInfo venueInfo = course.Opportunity.Item as VenueInfo;
                            if (venueInfo != null)
                            {
                                result.Venue = venueInfo.VenueName;
                                result.Distance = venueInfo.Distance.ToString();
                                if (venueInfo.VenueAddress != null)
                                {
                                    result.AddressLine1 = venueInfo.VenueAddress.Address_line_1;
                                    result.AddressLine2 = venueInfo.VenueAddress.Address_line_2;
                                    result.Town = venueInfo.VenueAddress.Town;
                                    result.County = venueInfo.VenueAddress.County;
                                    result.Postcode = venueInfo.VenueAddress.PostCode;
                                    result.Latitude = venueInfo.VenueAddress.Latitude;
                                    result.Longitude = venueInfo.VenueAddress.Longitude;

                                }
                            }
                        }
                        else if (course.Opportunity.Item is String)
                        {
                            result.RegionName = course.Opportunity.Item.ToString();
                        }
                    }

                    if (course.Opportunity.StartDate.ItemElementName.ToString() == "Date")
                    {
                        if (course.Opportunity.StartDate.Item != null)
                        {
                            result.StartDate = course.Opportunity.StartDate.Item;
                        }
                    }
                    else
                    {
                        result.StartDateDescription = course.Opportunity.StartDate.Item;
                    }

                    result.StudyMode = HttpUtility.HtmlEncode(course.Opportunity.StudyMode);
                }

                result.OpportunityID = HttpUtility.HtmlEncode(course.Opportunity.OpportunityId);

                // PROVIDER
                result.ProviderName = HttpUtility.HtmlEncode(course.Provider.ProviderName);
                result.TFPlusLoans = course.Provider.TFPlusLoans;
                result.ProviderDfEFunded = course.Provider.DFE1619Funded;
                result.FEChoices_LearnerDestination = course.Provider.FEChoices_LearnerDestinationSpecified ? course.Provider.FEChoices_LearnerDestination : (Double?)null;
                result.FEChoices_LearnerSatisfaction = course.Provider.FEChoices_LearnerSatisfactionSpecified ? course.Provider.FEChoices_LearnerSatisfaction : (Double?)null;
                result.FEChoices_EmployerSatisfaction = course.Provider.FEChoices_EmployerSatisfactionSpecified ? course.Provider.FEChoices_EmployerSatisfaction : (Double?)null;
            }

            return result;
        }

        
        /// <summary>
        /// Creates a repeater friendly LDCSCode object from the response.
        /// </summary>
        /// <param name="ldcsCode">The LDCSCode response from the search.</param>
        /// <returns>A populated LDCSCode object.</returns>
        private LDCSCode CreateLDCS(CourseListResponseStructureMatchingLDCS ldcsCode)
        {
            LDCSCode code = new LDCSCode();

            if (ldcsCode != null && ldcsCode.LDCS != null)
            {
                code.Code = ldcsCode.LDCS.LDCSCode;
                code.Description = ldcsCode.LDCS.LDCSDesc;

                int count;
                int.TryParse(ldcsCode.Counts, out count);
                code.CourseCount = count;
            }
            
            return code;
        }

        
        /// <summary>
        /// Gets the results from the web service.
        /// </summary>
        /// <param name="criteria">The course search criteria.</param>
        /// <param name="sortBy">The column on which to sort the results.</param>
        /// <param name="recordsPerPage">The number of records to return per page.</param>
        /// <param name="pageNo">The number of the page of data to get.</param>
        /// <returns>A list of found courses.</returns>
        private CourseListOutput GetResults(SearchCriteriaStructure criteria, string sortBy, string recordsPerPage, string pageNo)
        {
            CourseListOutput output = new CourseListOutput();
            ServiceInterface client = new ServiceInterfaceClient("CourseSearchService");

            CourseListRequestStructure listRequestStructure = new CourseListRequestStructure
            {
                CourseSearchCriteria = criteria, 
                RecordsPerPage = recordsPerPage, 
                PageNo = pageNo
            };

            if (!string.IsNullOrEmpty(sortBy))
            {
                SortType tempSortType;
                Enum.TryParse(sortBy, out tempSortType);
                listRequestStructure.SortBy = tempSortType;
                listRequestStructure.SortBySpecified = true;
            }

            CourseListInput request = new CourseListInput(listRequestStructure);
            try
            {
                output = client.CourseList(request);
            }
            catch (Exception ex)
            {
                ResultsOverviewLabel.Text = ex.Message + "\n" + ex.StackTrace;
            }

            return output;
        }
    }
}
