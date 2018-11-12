using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.UI;

using Ims.Schemas.Alse.CourseSearch.Contract;

namespace IMS.NCS.CourseSearchService.Monitoring
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SearchCriteriaStructure criteria = CreateSearchCriteria(Page.Request.QueryString);

                if (String.IsNullOrEmpty(criteria.SubjectKeyword))
                {
                    SendToClient("Warning: No Search Criteria");
                }
                else
                {
                    // always getting the first page of data so pass 1 for the page
                    CourseListOutput output = GetResults(criteria);
                    SendToClient(output != null && output.CourseListResponse.ResultInfo.NoOfRecords != "0" ? String.Format("Success: {0} Results", output.CourseListResponse.ResultInfo.NoOfRecords) : "Warning: 0 Results");
                }
            }
            catch (Exception ex)
            {
                SendToClient("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Creates the search criteria from the QueryString.
        /// </summary>
        /// <param name="requestData">The query string data.</param>
        /// <returns>A SearchCriteriaStructure with data.</returns>
        private static SearchCriteriaStructure CreateSearchCriteria(NameValueCollection requestData)
        {
            SearchCriteriaStructure criteria = new SearchCriteriaStructure
            {
                //APIKey
                APIKey = ConfigurationManager.AppSettings["APIKey"],
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
            criteria.DFE1619Funded = Utilities.GetCheckboxValue(requestData, "DfEFundedOnly");

            // REGION
            criteria.Location = Utilities.GetQueryStringValue(requestData, "Location");
            criteria.Distance = Utilities.GetQueryStringValueAsFloat(requestData, "Distance");
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

        private void SendToClient(String textToSend)
        {
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Clear();
            Response.Write(textToSend);
        }

        /// <summary>
        /// Gets the results from the web service.
        /// </summary>
        /// <param name="criteria">The course search criteria.</param>
        /// <returns>A list of found courses.</returns>
        private static CourseListOutput GetResults(SearchCriteriaStructure criteria)
        {
            ServiceInterface client = new ServiceInterfaceClient("CourseSearchService");

            CourseListRequestStructure listRequestStructure = new CourseListRequestStructure
            {
                CourseSearchCriteria = criteria,
                RecordsPerPage = "10",
                PageNo = "1",
                SortBy = SortType.D,
                SortBySpecified = true
            };

            CourseListInput request = new CourseListInput(listRequestStructure);
            CourseListOutput output = client.CourseList(request);

            return output;
        }
    }
}