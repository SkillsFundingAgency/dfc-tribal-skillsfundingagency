using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tribal.SkillsFundingAgency.ProviderPortal.Controllers;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class AddEditOpportunityModel
    {
        public Int32 ProviderId { get; set; }
        public String ProviderName { get; set; }
        public Boolean IsInOrganisation { get; set; }

        public Int32? OpportunityId { get; set; }
        public Int32? DuplicatingOpportunityId { get; set; }

        public Int32 CourseId { get; set; }

        [LanguageDisplay("Status")]
        public Int32? RecordStatusId { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Provider Opportunity Id")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter any unique identifier that your provider has for this course opportunity. This may be a code/ID or even a URL unique to this course opportunity.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Provider Opportunity Id")]
        public String ProviderOwnOpportunityRef { get; set; }

        [LanguageDisplay("Offered By")]
        [Display(Description = "The Organisation that has a contract with the Skills Funding Agency to deliver this provision.")]
        public Int32? OfferedById { get; set; }

        [LanguageDisplay("Display Name")]
        [Display(Description = "This opportunity will be seen on the course search to be offered by this Organisation,and this Organisation main contact details will be displayed,in addition to any contact details you've entered for the opportunity itself or the parent course.")]
        public Int32? DisplayId { get; set; }

        [LanguageDisplay("Make Both Names Searchable")]
        [Display(Description = "If the values in Offered By and Display Name are different this should be checked")]
        public Boolean BothOfferedByDisplayBySearched { get; set; }

        [LanguageDisplay("Funding Code")]
        [Display(Description = "Please select the appropriate Funding code(s) for this course opportunity. If this is not relevant, please use 'Not Applicable'. EFA funded Providers should select funding code 25 for all courses in addition to any other funding codes that may apply.")]
        public IEnumerable<A10FundingCode> A10FundingCodes { get; set; }
        public List<Int32> SelectedA10FundingCodes { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Study Mode")]
        [Display(Description = "This field describes the focus that the learner has on the learning opportunity, including duration of attendance and private study. Please select the appropriate mode of study from the drop down menu.")]
        public Int32? StudyModeId { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Attendance Mode")]
        [Display(Description = "This field describes how the learner will access the learning opportunity. Please select the appropriate mode of attendance from the drop down menu.")]
        public Int32? AttendanceModeId { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Attendance Pattern")]
        [Display(Description = "This field describes how the learner's presence at a venue is structured. Please select the appropriate pattern of attendance from the drop down menu.")]
        public Int32? AttendancePatternId { get; set; }

        [LanguageDisplay("Duration")]
        [Display(Description = "Please enter the whole number of hours / days / weeks / months / terms / semesters / years it will take to complete this course opportunity – e.g. if it takes 2 days, please enter 2. An opportunity must include a duration element or start and end dates, or both.")]
        public Int32? Duration { get; set; }

        [LanguageDisplay("Duration Unit")]
        public Int32? DurationUnitId { get; set; }

        [LanguageDisplay("Duration Description")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(150, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "If you do not know the exact duration, please enter a description of how long the course opportunity will take to complete e.g. Different durations available. An opportunity must include a duration element or start and end dates, or both.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Duration Description")]
        public String DurationDescription { get; set; }

        [LanguageDisplay("Start Date")]
        [Display(Description = "Please click on the calendar and select a start date for this course opportunity. if you have multiple opportunities with all the same details except start dates, you can select them all here.")]
        public String StartDate { get; set; }

        [LanguageDisplay("Start Month")]
        [Display(Description = "Please click on the calendar and select a start month for this course opportunity. If you have multiple opportunities with all the same details except start month, you can select them all here.")]
        public String StartMonth { get; set; }

        [LanguageDisplay("Start Date Description")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(150, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please select an appropriate phrase that describes the start date from the dropdown menu and/or write a description. If you have a specific start date/s, please fill in the Start date field instead.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Start Date Description")]
        public String StartDateDescription { get; set; }

        [LanguageDisplay("End Date")]
        [DataType(DataType.Date)]
        [DateDisplayFormat(Format = DateFormat.ShortDate, ApplyFormatInEditMode = true)]
        [Display(Description = "Please select an end date for this course opportunity.")]
        public DateTime? EndDate { get; set; }

        [LanguageDisplay("Timetable")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(200, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please specify the day/s and start and end time/s for this course opportunity. E.g. Thursdays 09.30-16.30 ; Wednesdays and Fridays from 10.30-15.00.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Timetable")]
        public String Timetable { get; set; }

        [LanguageDisplay("Price (&pound;)")]
        [Display(Description = "You must enter a Price in pounds or a Price Description, or both. Please also use the Price Description field to provide details of any financial support you offer e.g. bursaries.")]
        public Decimal? Price { get; set; }

        [LanguageDisplay("Price Description")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(1000, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter any extra information about the price including concessions, tuition, examination fees and materials cost if appropriate. Only the standard price should be placed in the price field. Providers should provide details of any financial support they offer in terms of bursaries etc.")]
        [ProviderPortalEditCourseOpportunityTextFieldAttribute(ErrorMessage = "Please enter a valid Price Description")]
        public String PriceDescription { get; set; }

        [LanguageDisplay("Venue")]
        [Display(Description = "Please select the venue where this opportunity is held from the drop down menu. If the venue is not present in the menu, please add it by clicking on 'Add a new venue'.")]
        public Int32? VenueId { get; set; }

        [LanguageDisplay("Town/County/Region")]
        [Display(Description = "Please enter a keyword for the town, county or region where this opportunity is offered. A list of options will appear. Please select the most appropriate one. Note you cannot have both a venue and a town/county/region - you may only have one or the other.")]
        public Int32? RegionId { get; set; }
        public String RegionName { get; set; }

        [LanguageDisplay("Language of Instruction")]
        [LanguageStringLength(100, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter the language in which this opportunity will be taught.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Language of Instruction")]
        public String LanguageOfInstruction { get; set; }

        [LanguageDisplay("Language of Assessment")]
        [LanguageStringLength(100, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter the language in which this opportunity will be assessed.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Language of Assessment")]
        public String LanguageOfAssessment { get; set; }

        [LanguageDisplay("Apply From")]
        [DataType(DataType.Date)]
        [DateDisplayFormat(Format = DateFormat.ShortDate, ApplyFormatInEditMode = true)]
        [Display(Description = "Please select a date from the calendar to indicate the date from which applications will be received.")]
        public DateTime? ApplyFrom { get; set; }

        [LanguageDisplay("Apply Until")]
        [DataType(DataType.Date)]
        [DateDisplayFormat(Format = DateFormat.ShortDate, ApplyFormatInEditMode = true)]
        [Display(Description = "Please select a date from the calendar to indicate the date from which applications will no longer be accepted.")]
        public DateTime? ApplyUntil { get; set; }

        [LanguageDisplay("Apply Until Description")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(100, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter a text description of the application deadline information supplied, if needed.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Apply Until Description")]
        public String ApplyUntilDescription { get; set; }

        [LanguageDisplay("Enquire To")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter a person, email or web address.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Enquire To")]
        public String EnquireTo { get; set; }

        [LanguageDisplay("Apply To")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter a person, email or web address.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Apply To")]
        public String ApplyTo { get; set; }

        [LanguageDisplay("URL")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter a URL that may contain more information about the course opportunity.")]
        public String Url { get; set; }

        [LanguageDisplay("Application accepted throughout the year")]
        [Display(Description = "If application for this course is accepted any time throughout year, please tick this box.")]
        public Boolean AcceptedThroughoutYear { get; set; }

        [LanguageDisplay("Archive Old Opportunity")]
        public Int32? ArchiveOldOpportunity { get; set; }

        public AddEditOpportunityModel()
        {
            this.SelectedA10FundingCodes = new List<Int32>();
        }

        public AddEditOpportunityModel(CourseInstance courseInstance) : this()
        {
            this.ProviderId = courseInstance.Course.ProviderId;
            this.ProviderName = courseInstance.Course.Provider.ProviderName;
            this.IsInOrganisation = courseInstance.Course.Provider.OrganisationProviders.Count > 0;
            this.OfferedById = courseInstance.OfferedByOrganisationId;
            this.DisplayId = courseInstance.DisplayedByOrganisationId;
            this.BothOfferedByDisplayBySearched = courseInstance.BothOfferedByDisplayBySearched;

            this.OpportunityId = courseInstance.CourseInstanceId;
            this.CourseId = courseInstance.CourseId;
            this.RecordStatusId = courseInstance.RecordStatusId;

            this.ProviderOwnOpportunityRef = courseInstance.ProviderOwnCourseInstanceRef;

            // Get Selected Funding Codes
            foreach (A10FundingCode fc in courseInstance.A10FundingCode)
            {
                this.SelectedA10FundingCodes.Add(fc.A10FundingCodeId);
            }

            this.StudyModeId = courseInstance.StudyModeId;
            this.AttendanceModeId = courseInstance.AttendanceTypeId;
            this.AttendancePatternId = courseInstance.AttendancePatternId;
            this.Duration = courseInstance.DurationUnit;
            this.DurationUnitId = courseInstance.DurationUnitId;
            this.DurationDescription = courseInstance.DurationAsText;

            // Get StartDate AND/OR StartMonth
            foreach (CourseInstanceStartDate sd in courseInstance.CourseInstanceStartDates)
            {
                if (!sd.IsMonthOnlyStartDate)
                {
                    if (!String.IsNullOrEmpty(this.StartDate))
                    {
                        this.StartDate += ",";
                    }
                    this.StartDate += sd.StartDate.ToString(Constants.ConfigSettings.ShortDateFormat);
                }
                else
                {
                    if (!String.IsNullOrEmpty(this.StartMonth))
                    {
                        this.StartMonth += ",";
                    }
                    this.StartMonth += sd.StartDate.ToString(OpportunityController.StartMonthFormat);
                }
            }

            this.StartDateDescription = courseInstance.StartDateDescription;
            this.EndDate = courseInstance.EndDate;
            this.Timetable = courseInstance.TimeTable;
            this.Price = courseInstance.Price;
            this.PriceDescription = courseInstance.PriceAsText;

            // Get Venue (Model allow multiple venues but front-end does not)
            foreach (Venue venue in courseInstance.Venues)
            {
                this.VenueId = venue.VenueId;
                break;
            }

            this.RegionId = courseInstance.VenueLocationId;
            this.RegionName = courseInstance.VenueLocation != null ? courseInstance.VenueLocation.LocationName + (courseInstance.VenueLocation.ParentVenueLocation != null ? " (" + courseInstance.VenueLocation.ParentVenueLocation.LocationName + ")" : "") : "";
            this.LanguageOfInstruction = courseInstance.LanguageOfInstruction;
            this.LanguageOfAssessment = courseInstance.LanguageOfAssessment;
            this.ApplyFrom = courseInstance.ApplyFromDate;
            this.ApplyUntil = courseInstance.ApplyUntilDate;
            this.ApplyUntilDescription = courseInstance.ApplyUntilText;
            this.EnquireTo = courseInstance.EnquiryTo;
            this.ApplyTo = courseInstance.ApplyTo;
            this.Url = courseInstance.Url;
            this.AcceptedThroughoutYear = courseInstance.CanApplyAllYear;
        }
    }

    public class ViewOpportunityModel : AddEditOpportunityModel
    {
        public String RecordStatusName { get; set; }
        public String OfferedByName { get; set; }
        public String DisplayName { get; set; }
        public String StudyMode { get; set; }
        public String AttendanceMode { get; set; }
        public String AttendancePattern { get; set; }
        public String DurationUnit { get; set; }
        public String Venue { get; set; }

        public ViewOpportunityModel(CourseInstance courseInstance) : base(courseInstance)
        {
            this.RecordStatusName = courseInstance.RecordStatu.RecordStatusName;
            this.OfferedByName = courseInstance.OfferedByOrganisationId == null ? courseInstance.Course.Provider.ProviderName : courseInstance.OfferedByOrganisation.OrganisationName;
            this.DisplayName = courseInstance.DisplayedByOrganisationId == null ? courseInstance.Course.Provider.ProviderName : courseInstance.DisplayNameOrganisation.OrganisationName;
            this.StudyMode = courseInstance.StudyMode == null ? "" : courseInstance.StudyMode.StudyModeName;
            this.AttendanceMode = courseInstance.AttendanceType == null ? "" : courseInstance.AttendanceType.AttendanceTypeName;
            this.AttendancePattern = courseInstance.AttendancePattern == null ? "" : courseInstance.AttendancePattern.AttendancePatternName;
            this.DurationUnit = courseInstance.DurationUnit1 == null ? "" : courseInstance.DurationUnit1.DurationUnitName;
            foreach (Venue v in courseInstance.Venues)
            {
                this.Venue = v.VenueName;
                break;
            }
        }
    }

    public class OpportunityListModel
    {
        public Int32 OpportunityId { get; set; }

        [LanguageDisplay("Status")]
        public String Status { get; set; }

        [LanguageDisplay("Opportunity Details")]
        public String OpportunityDetails { get; set; }

        [LanguageDisplay("Last Update")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime LastUpdate { get; set; }

        public OpportunityListModel()
        {
            DateTime.SpecifyKind(this.LastUpdate, DateTimeKind.Utc);
        }

        public OpportunityListModel(CourseInstance courseInstance) : this()
        {
            this.OpportunityId = courseInstance.CourseInstanceId;
            this.Status = courseInstance.RecordStatu.RecordStatusName;
            this.OpportunityDetails = courseInstance.GetOpportunityDetails();
            this.LastUpdate = courseInstance.ModifiedDateTimeUtc ?? courseInstance.CreatedDateTimeUtc;
        }
    }


    public class FullOpportunityListModel
    {
        [LanguageDisplay("Opportunity Id")]
        public Int32 OpportunityId { get; set; }

        [LanguageDisplay("Course Id")]
        public Int32 CourseId { get; set; }

        [LanguageDisplay("Status")]
        public String Status { get; set; }

        [LanguageDisplay("Course Details")]
        public String CourseDetails { get; set; }

        [LanguageDisplay("Course Title")]
        public String CourseTitle { get; set; }


        [LanguageDisplay("Opportunity Details")]
        public String OpportunityDetails { get; set; }

        [LanguageDisplay("Last Update")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime LastUpdate { get; set; }

        [LanguageDisplay("Start Date(s)")]
        public String StartDate { get; set; }

        public Constants.OpportunityFilterDateStatus? DateStatus { get; set; }

        public Int32? RecordStatusId { get; set; }

        public bool CanAdvanceStartDate { get; set; }

        public FullOpportunityListModel()
        {
            DateTime.SpecifyKind(this.LastUpdate, DateTimeKind.Utc);
        }

        public FullOpportunityListModel(CourseInstance opportunity, DateTime? lastStartDate, Constants.OpportunityFilterDateStatus? opportunityDateStatus) : this()
        {
            this.OpportunityId = opportunity.CourseInstanceId;
            this.CourseId = opportunity.CourseId;
            this.Status = opportunity.RecordStatu.RecordStatusName;
            this.RecordStatusId = opportunity.RecordStatusId;

            this.CourseTitle = opportunity.Course.CourseTitle;
            
            this.CourseDetails = this.CourseTitle;
            if (opportunity.Course.LearningAim != null)
            {
                this.CourseDetails = String.IsNullOrWhiteSpace(opportunity.Course.LearningAim.Qualification) ?
                    String.Format("{0} | {1}", opportunity.Course.CourseTitle, opportunity.Course.LearningAim.LearningAimTitle) :
                    String.Format("{0} | {1} | {2}", opportunity.Course.CourseTitle, opportunity.Course.LearningAim.LearningAimTitle, opportunity.Course.LearningAim.Qualification);
            }


            //TODO - CHECK WITH STEVE WHETHER WE WANT LONG PRICE DISPLAYING
            //Populate opportunity details
            this.OpportunityDetails = opportunity.GetOpportunityDetails();

            this.DateStatus = opportunityDateStatus;

            this.StartDate = String.Join(", ", opportunity.CourseInstanceStartDates.Select(d => d.StartDate.ToShortDateString()));

            this.LastUpdate = opportunity.ModifiedDateTimeUtc ?? opportunity.CreatedDateTimeUtc;

            //We cannot enable automatic advance start dates functionality unless the opportunity has a single start date
            this.CanAdvanceStartDate = opportunity.CourseInstanceStartDates != null && opportunity.CourseInstanceStartDates.Count == 1;
        }
    }




    public class OpportunitySearchModel
    {
        [LanguageDisplay("Number of Pending Opportunities")]
        public Int32 NumberOfPendingOpportunities { get; set; }

        [LanguageDisplay("Provider Course Id")]
        public String ProviderCourseId { get; set; }

        [LanguageDisplay("Provider Course Title")]
        public String ProviderCourseTitle { get; set; }

        [LanguageDisplay("Learning Aim Reference")]
        public String LearningAimReference { get; set; }

        [LanguageDisplay("Last Course Updated Date")]
        [DateDisplayFormat(Format = DateFormat.ShortDate, ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? LastUpdated { get; set; }

        [LanguageDisplay("Course Status")]
        public Int32? CourseStatus { get; set; }

        [LanguageDisplay("Provider Opportunity Id")]
        public String ProviderOpportunityId { get; set; }

        [LanguageDisplay("Study Mode")]
        public Int32? StudyModeId { get; set; }

        [LanguageDisplay("Attendance Mode")]
        public Int32? AttendanceModeId { get; set; }

        [LanguageDisplay("Attendance Pattern")]
        public Int32? AttendancePatternId { get; set; }

        [LanguageDisplay("Venue")]
        public Int32? VenueId { get; set; }

        [LanguageDisplay("Start Date")]
        public Int32? StartDateId { get; set; }

        [LanguageDisplay("Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [LanguageDisplay("Start Date Description")]
        public String StartDateDescription { get; set; }

        [LanguageDisplay("Opportunity Status")]
        public Int32? OpportunityStatus { get; set; }

        public Constants.OpportunitySearchQAFilter? QualitySearchMode { get; set; }

        public List<FullOpportunityListModel> Opportunities { get; set; }

        public OpportunitySearchModel()
        {
            this.Opportunities = new List<FullOpportunityListModel>();
        }
    }

    public class OpportunityDateStatusModelItem
    {
        [LanguageDisplay("Opportunity Id")]
        public Int32 OpportunityId { get; set; }

        [LanguageDisplay("Max Start Date")]
        [DateDisplayFormat(Format = DateFormat.ShortDate, ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? MaxStartDate { get; set; }

        public Constants.OpportunityFilterDateStatus DateStatus { get; set; }
    }

    public class OpportunityDateStatusModel
    {
        public Int32 ProviderId { get; set; }
        public List<OpportunityDateStatusModelItem> Items { get; set; }
    }

    public enum AdvanceStartDateOptions
    {
        [Display(Name = "Create new opportunity")]
        Create = 0,
        [Display(Name = "Create new opportunity and archive existing")]
        CreateAndArchive = 1,
        [Display(Name = "Update existing opportunity")]
        Update = 2
    }

    public class AdvanceStartDatesNotUpdated
    {
        public Int32 OpportunityId { get; set; }

        [LanguageDisplay("Course Title")]
        public String CourseTitle { get; set; }

        [LanguageDisplay("Opportunity Details")]
        public String OpportunityDetails { get; set; }

        [LanguageDisplay("Provider Opportunity Id")]
        public String ProviderOwnOpportunityRef { get; set; }

        [LanguageDisplay("Start Date(s)")]
        public String StartDate { get; set; }

        public AdvanceStartDatesNotUpdated(CourseInstance opportunity)
        {
            this.CourseTitle = opportunity.Course.CourseTitle;
            this.OpportunityId = opportunity.CourseInstanceId;
            this.StartDate = String.Join(", ", opportunity.CourseInstanceStartDates.Select(d => d.StartDate.ToShortDateString()));
            this.OpportunityDetails = opportunity.GetOpportunityDetails(false, false);
        }

     }

    public class AdvanceStartDatesModel
    {
        [LanguageDisplay("New start date")]
        [DataType(DataType.Date)]
        [DateDisplayFormat(Format = DateFormat.ShortDate, ApplyFormatInEditMode = true)]
        [Display(Description = "Please select a new start date for the selected opportunities.")]
        public DateTime NewStartDate { get; set; }

        [LanguageDisplay("New end date")]
        [DataType(DataType.Date)]
        [DateDisplayFormat(Format = DateFormat.ShortDate, ApplyFormatInEditMode = true)]
        [Display(Description = "Please select a new end date for the selected opportunities.")]
        public DateTime NewEndDate { get; set; }

        [LanguageDisplay("Create or Update?")]
        public AdvanceStartDateOptions CreateOrUpdate { get; set; }

        public int CountUpdates { get; set; }

        public int CountErrors { get; set; }

        public int CountUpdated { get; set; }

        public string OpportunityIdsToUpdate { get; set; }

        //TODO add new list of invalid opportunities, id, code, title, duration, start, end, error message
        //Alternatively, could we flag these somehow in main view - make it easier to update - how to maintain the list though?
        public List<AdvanceStartDatesNotUpdated> OpportunitiesNotUpdated { get; set; }
    }


}