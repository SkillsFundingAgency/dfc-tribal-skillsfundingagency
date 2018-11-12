using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.NCS.CourseSearchService.TestHarness.Models
{
    /// <summary>
    /// Represents an opportunity as part of the course details
    /// </summary>
    public class Opportunity
    {
        // opportunity information
        public string OpportunityId { get; set; }
        public string ProviderOpportunityId { get; set; }
        public string Price { get; set; }
        public string PriceDescription { get; set; }
        public string DurationValue { get; set; }
        public string DurationUnit { get; set; }
        public string DurationDescription { get; set; }
        public string StartDateDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StudyMode { get; set; }
        public string AttendanceMode { get; set; }
        public string AttendancePattern { get; set; }
        public string LanguageOfInstruction { get; set; }
        public string LanguageOfAssessment { get; set; }
        public string PlacesAvailable { get; set; }
        public string EnquireTo { get; set; }
        public string ApplyTo { get; set; }
        public string ApplyFromDate { get; set; }
        public string ApplyUntilDate { get; set; }
        public string ApplyUntilDescription { get; set; }
        public string Url { get; set; }
        public string Timetable { get; set; }
        public string A10Field { get; set; }
        public string ApplicationAcceptedThroughoutYear { get; set; }
        public string VenueId { get; set; }
        public string RegionName { get; set; }
    }
}