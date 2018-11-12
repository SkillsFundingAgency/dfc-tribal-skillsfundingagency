using System;
using System.Collections.Generic;

namespace IMS.NCS.CourseSearchService.Entities
{
    /// <summary>
    /// Opportunity Entity.
    /// </summary>
    public class Opportunity
    {
        public List<string> A10 { get; set; }
        public string ApplicationAcceptedThroughoutYear { get; set; }
        public string ApplyFromDate { get; set; }
        public string ApplyTo { get; set; }
        public string ApplyUntilDate { get; set; }
        public string ApplyUntilDescription { get; set; }
        public string AttendanceMode { get; set; }
        public string AttendancePattern { get; set; }
        public string Distance { get; set; }
        public string DurationDescription { get; set; }
        public string DurationUnit { get; set; }
        public long DurationValue { get; set; }
        public string EndDate { get; set; }
        public string EnquireTo { get; set; }
        public string LanguageOfAssessment { get; set; }
        public string LanguageOfInstruction { get; set; }
        public string OpportunityId { get; set; }
        public long PlacesAvailable { get; set; }
        public string Price { get; set; }
        public string PriceDescription { get; set; }
        public string ProviderOpportunityId { get; set; }
        public string RegionName { get; set; }
        public string StartDate { get; set; }
        public string StartDateDescription { get; set; }
        public string StudyMode { get; set; }
        public string Timetable { get; set; }
        public string Url { get; set; }
        public Venue Venue { get; set; }
        public Int32 VenueId { get; set; }
        public Boolean DfE1619Funded { get; set; }
    }
}
