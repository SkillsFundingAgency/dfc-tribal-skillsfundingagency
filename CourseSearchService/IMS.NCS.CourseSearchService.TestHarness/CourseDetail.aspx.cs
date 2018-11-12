using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using IMS.NCS.CourseSearchService.TestHarness.Models;
using Ims.Schemas.Alse.CourseSearch.Contract;
using System.Collections.Specialized;

namespace IMS.NCS.CourseSearchService.TestHarness
{
    public partial class CourseDetail : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // check for submit from search control
                string action = Utilities.GetQueryStringValue(Page.Request.QueryString, "action");

                // check for query string
                string[] courseIds = Utilities.GetQueryStringValues(Page.Request.QueryString, Constants.QueryStrings.CourseId);
                String APIKey = Page.Request.QueryString["APIKey"];
                if (courseIds != null 
                    && courseIds.Count() > 0)
                {
                    PopulateData(courseIds, APIKey);
                }
            }
            else
            {
                // need to remove the viewstate for cross page redirects, otherwise we'll get a MAC validation error
                NameValueCollection newQueryString = Utilities.RemoveViewState(Page.Request.QueryString);
                Response.Redirect("CourseSearch.aspx" + Utilities.CreateQueryString(newQueryString));
            }
        }


        /// <summary>
        /// Gets the data from the web service and populates the controls.
        /// </summary>
        /// <param name="courseIds">The id's of the courses to display.</param>
        /// <param name="APIKey"></param>
        private void PopulateData(string[] courseIds, String APIKey)
        {
            ServiceInterface client = new ServiceInterfaceClient("CourseSearchService");
            CourseDetailInput courseListInput = new CourseDetailInput(courseIds, APIKey);

            try
            {   
                CourseDetailOutput courseDetailOutput = client.CourseDetail(courseListInput);

                List<CourseInformation> courseDetails = CreateCourseDetails(courseDetailOutput);

                if (courseDetails.Count() > 0)
                {
                    NumberOfCourses.Text = string.Format(Constants.StringFormats.CourseDetailNoOfCourses, courseDetails.Count());
                    divResults.Visible = true;
                    
                    CourseRepeater.DataSource = courseDetails;
                    CourseRepeater.DataBind();
                }
                else
                {
                    divResults.Visible = false;
                    ResultsOverviewLabel.Text = "There are no results to display.";
                }
            }
            catch(Exception ex)
            {
                ResultsOverviewLabel.Text = ex.Message + "\n" + ex.StackTrace;
            }
        }


        /// <summary>
        /// Maps the response from the web service search to a ui friendly class.
        /// </summary>
        /// <param name="output">The output from the web service.</param>
        /// <returns>a liost of CourseDetail objects.</returns>
        private List<CourseInformation> CreateCourseDetails(CourseDetailOutput output)
        {
            List<CourseInformation> courseDetails = new List<CourseInformation>();

            foreach (CourseDetailStructure courseDetailStructure in output.CourseDetails)
            {
                CourseInformation courseInfo = new CourseInformation();
                
                if (courseDetailStructure.Course != null)
                {
                    MapCourseData(courseInfo, courseDetailStructure);
                }

                if (courseDetailStructure.Provider != null)
                {
                    MapProviderData(courseInfo, courseDetailStructure);
                }

                if (courseDetailStructure.Opportunity != null)
                {
                    MapOpportunityData(courseInfo, courseDetailStructure);
                }

                if (courseDetailStructure.Venue != null)
                {
                    MapVenueData(courseInfo, courseDetailStructure);
                }

                courseDetails.Add(courseInfo);
            }

            return courseDetails;
        }


        /// <summary>
        /// Maps the course response data to our ui friendly structure.
        /// </summary>
        /// <param name="courseInfo">The class to populate, must not be null.</param>
        /// <param name="courseDetailStructure">The response from the web service</param>
        private void MapCourseData(CourseInformation courseInfo, CourseDetailStructure courseDetailStructure)
        {
            courseInfo.CourseId = courseDetailStructure.Course.CourseID;
            courseInfo.ProviderCourseTitle = courseDetailStructure.Course.CourseTitle;
            courseInfo.Summary = courseDetailStructure.Course.CourseSummary;
            courseInfo.URL = courseDetailStructure.Course.URL;
            courseInfo.BookingURL = courseDetailStructure.Course.BookingURL;
            courseInfo.EntryRequirements = courseDetailStructure.Course.EntryRequirements;
            courseInfo.AssessmentMethod = courseDetailStructure.Course.AssessmentMethod;
            courseInfo.EquipmentRequired = courseDetailStructure.Course.EquipmentRequired;
            courseInfo.TariffRequired = courseDetailStructure.Course.TariffRequired;
            courseInfo.LearningAimRef = courseDetailStructure.Course.QualificationReference;

            // referred LAD data
            courseInfo.AwardingOrganisationName = courseDetailStructure.Course.AwardingBody;
            courseInfo.Level2EntitlementCategoryDescription = courseDetailStructure.Course.Level2EntitlementCategoryDesc;
            courseInfo.Level3EntitlementCategoryDescription = courseDetailStructure.Course.Level3EntitlementCategoryDesc;
            courseInfo.SectorLeadBodyDescription = courseDetailStructure.Course.SectorLeadBodyDesc;
            courseInfo.AccreditationStartDate = courseDetailStructure.Course.AccreditationStartDate;
            courseInfo.AccreditationEndDate = courseDetailStructure.Course.AccreditationEndDate;
            courseInfo.CertificationEndDate = courseDetailStructure.Course.CertificationEndDate;
            courseInfo.CreditValue = courseDetailStructure.Course.CreditValue;
            courseInfo.QCAGuidedLearningHours = courseDetailStructure.Course.QCAGuidedLearningHours;
            courseInfo.IndependentLivingSkills = courseDetailStructure.Course.IndependentLivingSkills.ToString();
            courseInfo.SkillsforLifeFlag = courseDetailStructure.Course.SkillsForLifeFlag.ToString();
            courseInfo.SkillsForLifeTypeDescription = courseDetailStructure.Course.SkillsForLifeTypeDesc;
            courseInfo.ERAppStatus = courseDetailStructure.Course.ERAppStatus.ToString();
            courseInfo.ERTTGStatus = courseDetailStructure.Course.ERTTGStatus.ToString();
            courseInfo.AdultLRStatus = courseDetailStructure.Course.AdultLRStatus.ToString();
            courseInfo.OtherFundingNonFundingStatus = courseDetailStructure.Course.OtherFundingNonFundedStatus.ToString();

            // conditionally derived data
            courseInfo.DataType = courseDetailStructure.Course.DataType.ToString();
            courseInfo.QualificationReferenceAuthority = courseDetailStructure.Course.QualificationReferenceAuthority;
            courseInfo.QualificationReference = courseDetailStructure.Course.QualificationReference;
            courseInfo.QualificationTitle = courseDetailStructure.Course.QualificationTitle;
            courseInfo.QualificationType = courseDetailStructure.Course.QualificationType;
            courseInfo.QualificationLevel = courseDetailStructure.Course.QualificationLevel;
            courseInfo.LDCSCategoryCodeApplicability1 = courseDetailStructure.Course.LDCS.CatCode1.LDCSDesc;
            courseInfo.LDCSCategoryCodeApplicability2 = courseDetailStructure.Course.LDCS.CatCode2.LDCSDesc;
        }


        /// <summary>
        /// Maps the Opportunity response data to our ui friendly structure.
        /// </summary>
        /// <param name="courseInfo">The class to populate, must not be null.</param>
        /// <param name="courseDetailStructure">The response from the web service</param>
        private void MapOpportunityData(CourseInformation courseInfo, CourseDetailStructure courseDetailStructure)
        {
            if (courseDetailStructure.Opportunity != null 
                && courseDetailStructure.Opportunity.Count() > 0)
            {
                courseInfo.Opportunities = new List<Opportunity>();

                foreach(OpportunityDetail opportunity in courseDetailStructure.Opportunity)
                {
                    Opportunity opp = new Opportunity();

                    if (opportunity.A10 != null)
                    {
                        opp.A10Field = string.Join(",", opportunity.A10);
                    }
                    opp.ApplicationAcceptedThroughoutYear = opportunity.ApplicationAcceptedThroughoutYear.ToString();
                    opp.ApplyFromDate = opportunity.ApplyFromDate;
                    opp.ApplyTo = opportunity.ApplyTo;
                    opp.ApplyUntilDate = opportunity.ApplyUntilDate;
                    opp.ApplyUntilDescription = opportunity.ApplyUntilDesc;
                    opp.AttendanceMode = opportunity.AttendanceMode;
                    opp.AttendancePattern = opportunity.AttendancePattern;

                    if (opportunity.Duration != null)
                    {
                        opp.DurationDescription = opportunity.Duration.DurationDescription;
                        opp.DurationUnit = opportunity.Duration.DurationUnit;
                        opp.DurationValue = opportunity.Duration.DurationValue;
                    }

                    opp.EndDate = opportunity.EndDate;
                    opp.EnquireTo = opportunity.EnquireTo;
                    opp.LanguageOfAssessment = opportunity.LanguageOfAssessment;
                    opp.LanguageOfInstruction = opportunity.LanguageOfInstruction;

                    // region name and venue id are in the Items collection
                    // need to check for type to work out which field we populate
                    if (opportunity.Items.Count() > 0)
                    {
                        for (Int32 i = 0; i < opportunity.Items.Length; i++)
                        {
                            switch (opportunity.ItemsElementName[i])
                            {
                                case ItemsChoiceType.VenueID:
                                    opp.VenueId = opportunity.Items[i];
                                    break;
                                case ItemsChoiceType.RegionName:
                                    opp.RegionName = opportunity.Items[i];
                                    break;
                            }
                        }
                    }

                    opp.OpportunityId = opportunity.OpportunityId;
                    opp.PlacesAvailable = opportunity.PlacesAvailable;
                    opp.Price = opportunity.Price;
                    opp.PriceDescription = opportunity.PriceDesc;
                    opp.ProviderOpportunityId = opportunity.ProviderOpportunityId;

                    if (opportunity.StartDate.ItemElementName.ToString() == "Date")
                    {
                        if (opportunity.StartDate.Item != null)
                        {
                            opp.StartDate = opportunity.StartDate.Item.ToString();
                        }
                    }
                    else
                    {
                        if (opportunity.StartDate.Item != null)
                        {
                            opp.StartDateDescription = opportunity.StartDate.Item.ToString();
                        }
                    }

                    opp.StudyMode = opportunity.StudyMode;
                    opp.Timetable = opportunity.Timetable;
                    opp.Url = opportunity.URL;


                    courseInfo.Opportunities.Add(opp);
                }
            }
        }


        /// <summary>
        /// Maps the Provider response data to our ui friendly structure.
        /// </summary>
        /// <param name="courseInfo">The class to populate, must not be null.</param>
        /// <param name="courseDetailStructure">The response from the web service</param>
        private void MapProviderData(CourseInformation courseInfo, CourseDetailStructure courseDetailStructure)
        {
            if (courseDetailStructure.Provider != null)
            {
                ProviderSearchResult provider = new ProviderSearchResult();

                provider.ProviderID = courseDetailStructure.Provider.ProviderID;
                provider.ProviderName = courseDetailStructure.Provider.ProviderName;
                provider.AddressLine1 = courseDetailStructure.Provider.ProviderAddress.Address_line_1;
                provider.AddressLine2 = courseDetailStructure.Provider.ProviderAddress.Address_line_2;
                provider.Town = courseDetailStructure.Provider.ProviderAddress.Town;
                provider.County = courseDetailStructure.Provider.ProviderAddress.County;
                provider.Postcode = courseDetailStructure.Provider.ProviderAddress.PostCode;
                provider.Email = courseDetailStructure.Provider.Email;
                provider.Website = courseDetailStructure.Provider.Website;
                provider.Phone = courseDetailStructure.Provider.Phone;
                provider.Fax = courseDetailStructure.Provider.Fax;
                provider.UKPRN = courseDetailStructure.Provider.UKPRN;
                provider.UPIN = courseDetailStructure.Provider.UPIN;
                provider.TFPlusLoans = courseDetailStructure.Provider.TFPlusLoans;
                provider.DFE1619Funded = courseDetailStructure.Provider.DFE1619Funded;
                provider.FEChoices_LearnerDestination = courseDetailStructure.Provider.FEChoices_LearnerDestinationSpecified ? courseDetailStructure.Provider.FEChoices_LearnerDestination : (Double?)null;
                provider.FEChoices_LearnerSatisfaction = courseDetailStructure.Provider.FEChoices_LearnerSatisfactionSpecified ? courseDetailStructure.Provider.FEChoices_LearnerSatisfaction : (Double?)null;
                provider.FEChoices_EmployerSatisfaction = courseDetailStructure.Provider.FEChoices_EmployerSatisfactionSpecified ? courseDetailStructure.Provider.FEChoices_EmployerSatisfaction : (Double?)null;

                courseInfo.Provider = provider;
            }
        }


        /// <summary>
        /// Maps the venue response data to our ui friendly structure.
        /// </summary>
        /// <param name="courseInfo">The class to populate, must not be null.</param>
        /// <param name="courseDetailStructure">The response from the web service</param>
        private void MapVenueData(CourseInformation courseInfo, CourseDetailStructure courseDetailStructure)
        {
            if (courseDetailStructure.Venue != null
                && courseDetailStructure.Venue.Count() > 0)
            {
                courseInfo.Venues = new List<Venue>();
                
                foreach (VenueDetail venueDetail in courseDetailStructure.Venue)
                {
                    Venue venue = new Venue();

                    venue.VenueId = venueDetail.VenueID;
                    venue.VenueName = venueDetail.VenueName;
                    venue.AddressLine1 = venueDetail.VenueAddress.Address_line_1;
                    venue.AddressLine2 = venueDetail.VenueAddress.Address_line_2;
                    venue.Town = venueDetail.VenueAddress.Town;
                    venue.County = venueDetail.VenueAddress.County;
                    venue.Postcode = venueDetail.VenueAddress.PostCode;
                    venue.Email = venueDetail.Email;
                    venue.Facilities = venueDetail.Facilities;
                    venue.Fax = venueDetail.Fax;
                    venue.Phone = venueDetail.Phone;
                    venue.Website = venueDetail.Website;
                    venue.Latitude = venueDetail.VenueAddress.Latitude;
                    venue.Longitude = venueDetail.VenueAddress.Longitude;

                    courseInfo.Venues.Add(venue);
                }
            }
        }


        /// <summary>
        /// Applies datasources to nested repeaters.
        /// </summary>
        /// <param name="sender">the parent CourseRepeater.</param>
        /// <param name="e">The event arguments.</param>
        protected void CourseRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // set the data source of the other repeaters here
            
            // only one provider so need to put it into a list
            List<ProviderSearchResult> providers = new List<ProviderSearchResult>();
            providers.Add(((CourseInformation)e.Item.DataItem).Provider);
            Repeater provRepeater = e.Item.FindControl("ProviderRepeater") as Repeater;
            provRepeater.DataSource = providers;

            // as the Opportunity repeater is nested we need to find it via the parent repeater
            Repeater opportunityRepeater = e.Item.FindControl("OpportunityRepeater") as Repeater;
            opportunityRepeater.DataSource = ((CourseInformation)e.Item.DataItem).Opportunities;

            Repeater venueRepeater = e.Item.FindControl("VenueRepeater") as Repeater;
            venueRepeater.DataSource = ((CourseInformation)e.Item.DataItem).Venues;

            // bind all the data
            provRepeater.DataBind();
            opportunityRepeater.DataBind();
            venueRepeater.DataBind();
        }
    }
}
