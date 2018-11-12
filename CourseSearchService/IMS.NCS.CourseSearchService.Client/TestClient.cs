using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;

using Ims.Schemas.Alse.CourseSearch.Contract;

namespace IMS.NCS.CourseSearchService.Client
{
    /// <summary>
    /// TestClient code behind implementing bespoke Form functionality.
    /// </summary>
    public partial class TestClient : Form
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public TestClient()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On clicking CourseList button, calls CourseSearchService ClientList() method wtih test data.
        /// </summary>
        private void btnCourseList_Click(object sender, EventArgs e)
        {
            txtResult.Text = "Processing ...";

            ServiceInterface client = new ServiceInterfaceClient("CourseSearch");

            CourseListRequestStructure listRequestStructure = new CourseListRequestStructure();
            listRequestStructure.CourseSearchCriteria = new SearchCriteriaStructure();
            listRequestStructure.CourseSearchCriteria.SubjectKeyword = "chemistry";
//            listRequestStructure.CourseSearchCriteria.ProviderID = "4517";  // 4517 University of Bristol
            listRequestStructure.CourseSearchCriteria.Location = "grantham";
//            listRequestStructure.CourseSearchCriteria.Distance = 30.0f;
//            listRequestStructure.CourseSearchCriteria.DistanceSpecified = true;

            CourseListInput request = new CourseListInput(listRequestStructure);
            try
            {
                CourseListOutput response = client.CourseList(request);

                StringBuilder sb = new StringBuilder();
                sb.Append("Request details:");
                sb.Append("\nCourse Search Criteria:");
                sb.Append("\n A10 codes: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.A10Codes);
                sb.Append("\n Adult LR status: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.AdultLRStatus);
                sb.Append("\n Attendance Modes: " +
                    ((response.CourseListResponse.RequestDetails.CourseSearchCriteria.AttendanceModes != null) ?
                    response.CourseListResponse.RequestDetails.CourseSearchCriteria.AttendanceModes.ToString() : "null"));
                sb.Append("\n Attendance Patterns: " +
                    ((response.CourseListResponse.RequestDetails.CourseSearchCriteria.AttendancePatterns != null) ?
                    response.CourseListResponse.RequestDetails.CourseSearchCriteria.AttendancePatterns.ToString() : "null"));
                sb.Append("\n Distance: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.Distance.ToString());
                sb.Append("\n Distance specified: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.DistanceSpecified.ToString());
                sb.Append("\n Earliest Start date: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.EarliestStartDate);
                sb.Append("\n ER app status: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.ERAppStatus);
                sb.Append("\n ER TTG status: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.ERTtgStatus);
                sb.Append("\n Flex start date: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.FlexStartFlag);
                sb.Append("\n IES flag: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.IESFlag);
                sb.Append("\n ILS flag: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.ILSFlag);
                sb.Append("\n LDCS Category code: " +
                    ((response.CourseListResponse.RequestDetails.CourseSearchCriteria.LDCS != null) ?
                    response.CourseListResponse.RequestDetails.CourseSearchCriteria.LDCS.CategoryCode.ToString() : ""));
                sb.Append("\n Location: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.Location);
                sb.Append("\n Opps App closed flag: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.OppsAppClosedFlag);
                sb.Append("\n Other funding status: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.OtherFundingStatus);
                sb.Append("\n Provider ID: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.ProviderID);
                sb.Append("\n Provider Keyword: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.ProviderKeyword);
                sb.Append("\n Qualification levels: " +
                    ((response.CourseListResponse.RequestDetails.CourseSearchCriteria.QualificationLevels != null) ?
                    response.CourseListResponse.RequestDetails.CourseSearchCriteria.QualificationLevels.ToString() : "null"));
                sb.Append("\n Qualification types: " +
                    ((response.CourseListResponse.RequestDetails.CourseSearchCriteria.QualificationTypes != null) ?
                    response.CourseListResponse.RequestDetails.CourseSearchCriteria.QualificationTypes.ToString() : "null"));
                sb.Append("\n SFL flag: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.SFLFlag);
                sb.Append("\n Study modes: " +
                    ((response.CourseListResponse.RequestDetails.CourseSearchCriteria.StudyModes != null) ?
                    response.CourseListResponse.RequestDetails.CourseSearchCriteria.StudyModes.ToString() : "null"));
                sb.Append("\n Subject keyword: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.SubjectKeyword);
                sb.Append("\n TQS flag: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.TQSFlag);
                sb.Append("\n TTG flag: " + response.CourseListResponse.RequestDetails.CourseSearchCriteria.TTGFlag);
                sb.Append("\n\n");

                sb.Append("Course Details:");

                if (response.CourseListResponse.CourseDetails != null)
                {
                    foreach (CourseStructure courseStructure in response.CourseListResponse.CourseDetails)
                    {
                        sb.Append("\n" + courseStructure.Course.CourseID);
                        sb.Append("\n" + courseStructure.Provider);
                        sb.Append("\n" + courseStructure.Course.CourseTitle);
                        sb.Append("\n" + courseStructure.Course.QualificationType);
                        sb.Append("\n" + courseStructure.Course.QualificationLevel);
                        sb.Append("\n" + courseStructure.Course.CourseSummary);
                        sb.Append("\n" + courseStructure.Course.NoOfOpps);

                        sb.Append("\n" + courseStructure.Opportunity.OpportunityId);
                        sb.Append("\n" + courseStructure.Opportunity.StudyMode);
                        sb.Append("\n" + courseStructure.Opportunity.AttendanceMode);
                        sb.Append("\n" + courseStructure.Opportunity.AttendancePattern);
                        sb.Append("\n" + courseStructure.Opportunity.StartDate.Item);
                        sb.Append("\n" + courseStructure.Opportunity.Duration.DurationValue);
                        sb.Append("\n" + courseStructure.Opportunity.Duration.DurationUnit);
                        sb.Append("\n" + courseStructure.Opportunity.Duration.DurationDescription);

                        if (courseStructure.Opportunity.Item.GetType() == typeof(VenueInfo))
                        {
                            VenueInfo venue = (VenueInfo) courseStructure.Opportunity.Item;
                            sb.Append("\n" + venue.VenueName);
                            sb.Append("\n" + venue.Distance);
                            sb.Append("\n" + venue.VenueAddress.Address_line_1);
                            sb.Append("\n" + venue.VenueAddress.Address_line_2);
                            sb.Append("\n" + venue.VenueAddress.Town);
                            sb.Append("\n" + venue.VenueAddress.County);
                            sb.Append("\n" + venue.VenueAddress.PostCode);
                            sb.Append("\n" + venue.VenueAddress.Latitude);
                            sb.Append("\n" + venue.VenueAddress.Longitude);
                        }
                        else
                        {
                            sb.Append("\n" + (string) courseStructure.Opportunity.Item);
                        }

                        sb.Append("\n");
                    }
                }
                sb.Append("\n\n");

                sb.Append("Matching LDCS Details:");

                if (response.CourseListResponse.MatchingLDCS != null)
                {
                    foreach (CourseListResponseStructureMatchingLDCS mathcingLDCS in response.CourseListResponse.MatchingLDCS)
                    {
                        sb.Append("\n" + mathcingLDCS.LDCS.LDCSCode);
                        sb.Append("\n" + mathcingLDCS.LDCS.LDCSDesc);
                        sb.Append("\n" + mathcingLDCS.Counts);
                    }
                }
                sb.Append("\n\n");

                txtResult.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
        }

        /// <summary>
        /// On clicking CourseDetail button, calls CourseSearchService ClientDetail() method with test data.
        /// </summary>
        private void btnCourseDetail_Click(object sender, EventArgs e)
        {
            txtResult.Text = "Processing ...";

            ServiceInterface client = new ServiceInterfaceClient("CourseSearch");

            string[] courseIds = { "53527655"};
            CourseDetailInput request = new CourseDetailInput(courseIds, "");

            try
            {
                CourseDetailOutput response = client.CourseDetail(request);

                StringBuilder sb = new StringBuilder();
                sb.Append("Request details:");
                foreach (string s in response.RequestDetails.CourseID)
                {
                    sb.Append("\n" + s);
                }
                sb.Append("\n\n");

                sb.Append("Course details:");
                foreach (CourseDetailStructure courseDetail in response.CourseDetails)
                {
                    sb.Append("\n" + courseDetail.Course.CourseID);
                    sb.Append("\n" + courseDetail.Course.CourseTitle);
                    sb.Append("\n" + courseDetail.Course.CourseSummary);
                    sb.Append("\n" + courseDetail.Provider.ProviderID);
                    sb.Append("\n" + courseDetail.Provider.ProviderName);
                    sb.Append("\n" + courseDetail.Provider.UPIN);
                    sb.Append("\n" + courseDetail.Provider.Email);
                    sb.Append("\n" + courseDetail.Provider.Website);
                    sb.Append("\n");

                    sb.Append("Opportunity details:");
                    foreach (OpportunityDetail opportunityDetail in courseDetail.Opportunity)
                    {
                        sb.Append("\n" + opportunityDetail.ProviderOpportunityId);
                        sb.Append("\n" + opportunityDetail.URL);
                        sb.Append("\n" + opportunityDetail.A10);
                        sb.Append("\n" + opportunityDetail.ApplyFromDate);
                        sb.Append("\n" + opportunityDetail.ApplyUntilDate);
                        sb.Append("\n" + opportunityDetail.ApplyTo);
                        sb.Append("\n" + opportunityDetail.AttendanceMode);
                        sb.Append("\n" + opportunityDetail.AttendancePattern);
                        sb.Append("\n" + opportunityDetail.EnquireTo);
                        sb.Append("\n" + opportunityDetail.PlacesAvailable);
                        sb.Append("\n" + opportunityDetail.StudyMode);
                        sb.Append("\n" + opportunityDetail.Price);
                        sb.Append("\n");
                    }

                    sb.Append("Venue details:");
                    foreach (VenueDetail venueDetail in courseDetail.Venue)
                    {
                        sb.Append("\n" + venueDetail.VenueID);
                        sb.Append("\n" + venueDetail.VenueName);
                        sb.Append("\n" + venueDetail.Email);
                        sb.Append("\n" + venueDetail.Facilities);
                        sb.Append("\n" + venueDetail.Website);
                        sb.Append("\n");
                    }

                    sb.Append("\n");
                }

                txtResult.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
        }

        /// <summary>
        /// On clicking ProviderSearch button, calls CourseSearchService ProviderSearch() method wtih test data.
        /// </summary>
        private void btnProviderSearch_Click(object sender, EventArgs e)
        {
            txtResult.Text = "Processing ...";

            ServiceInterface client = new ServiceInterfaceClient("CourseSearch");

            ProviderSearchInput request = new ProviderSearchInput(new ProviderSearchRequestStructure());
            request.ProviderSearchRequest.ProviderKeyword = "Cardiff";

            try
            {
                ProviderSearchOutput response = client.ProviderSearch(request);
                StringBuilder sb = new StringBuilder();
                sb.Append("Request details:");
                sb.Append("\nProvider Keyword = " +
                    response.ProviderSearchResponse.RequestDetails.ProviderSearch.ProviderKeyword);

                sb.Append("\n\n");

                if (response.ProviderSearchResponse != null && response.ProviderSearchResponse.ProviderDetails != null)
                {
                    sb.Append("Provider details:");
                    foreach (ProviderStructure providerDetail in response.ProviderSearchResponse.ProviderDetails)
                    {
                        sb.Append("\n" + providerDetail.Provider.ProviderID);
                        sb.Append("\n" + providerDetail.Provider.ProviderName);
                        sb.Append("\n");
                    }
                }

                txtResult.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
        }

        /// <summary>
        /// On clicking ProviderDetails button, calls CourseSearchService ProviderDetails() method wtih test data.
        /// </summary>
        private void btnProviderDetails_Click(object sender, EventArgs e)
        {
            txtResult.Text = "Processing ...";

            ServiceInterface client = new ServiceInterfaceClient("CourseSearch");

            ProviderDetailsInput request = new ProviderDetailsInput("300017", "");  // 3958 = Cardiff University

            try
            {
                ProviderDetailsOutput response = client.ProviderDetails(request);
                StringBuilder sb = new StringBuilder();
                sb.Append("Request details:");
                sb.Append("\nProvider ID = " +
                    response.RequestDetails.ProviderID);

                sb.Append("\n\n");

                if (response.ProviderDetails != null)
                {
                    sb.Append("Provider details:");
                    sb.Append("\nID: " + response.ProviderDetails.ProviderID);
                    sb.Append("\nName: " + response.ProviderDetails.ProviderName);
                    sb.Append("\nUKPRN: " + response.ProviderDetails.UKPRN);
                    sb.Append("\nUPIN:" + response.ProviderDetails.UPIN);
                    sb.Append("\nAddress: " + response.ProviderDetails.ProviderAddress.Address_line_1);
                    sb.Append(", " + response.ProviderDetails.ProviderAddress.Address_line_2);
                    sb.Append(", " + response.ProviderDetails.ProviderAddress.Town);
                    sb.Append(", " + response.ProviderDetails.ProviderAddress.County);
                    sb.Append(", " + response.ProviderDetails.ProviderAddress.PostCode);
                    sb.Append("\nPhone: " + response.ProviderDetails.Phone);
                    sb.Append("\nFax: " + response.ProviderDetails.Fax);
                    sb.Append("\nEmail: " + response.ProviderDetails.Email);
                    sb.Append("\nWebsite: " + response.ProviderDetails.Website);
                    sb.Append("\n");
                }

                txtResult.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }

        }

        /// <summary>
        /// On clicking GetCategories button, calls CourseSearchService GetCategories() method wtih test data.
        /// </summary>
        private void btnGetCategories_Click(object sender, EventArgs e)
        {
            txtResult.Text = "Processing ...";

            ServiceInterface client = new ServiceInterfaceClient("CourseSearch");

            SubjectBrowseInput request = new SubjectBrowseInput();
            StringBuilder sb = new StringBuilder();
            try
            {
                SubjectBrowseOutput response = client.GetCategories(request);

                foreach (Level1 level1 in response.Level1)
                {
                    sb.Append("Level 1:" + level1.LDCS.LDCSCode + "; " + level1.LDCS.LDCSDesc + "; " + 
                        level1.LDCS.Searchable.ToString() + "; " + level1.CourseCounts + "\n");

                    if (level1.Level2 != null)
                    {
                        foreach (Level2 level2 in level1.Level2)
                        {
                            sb.Append("Level 2:" + level2.LDCS.LDCSCode + "; " + level2.LDCS.LDCSDesc + "; " + 
                                level2.LDCS.Searchable.ToString() + "; " + level2.CourseCounts + "\n");

                            if (level2.Level3 != null)
                            {
                                foreach (CategoryInfo level3 in level2.Level3)
                                {
                                    sb.Append("Level 3:" + level3.LDCS.LDCSCode + "; " + level3.LDCS.LDCSDesc + "; " + 
                                        level3.LDCS.Searchable.ToString() + "; " + level3.CourseCounts + "\n");
                                }
                            }
                        }
                    }

                    sb.Append("\n");
                }

                txtResult.Text = sb.ToString();

            }
            catch (Exception ex)
            {
                txtResult.Text = sb.ToString() + "\n\n" + ex.Message;
            }
        }
    }
}
