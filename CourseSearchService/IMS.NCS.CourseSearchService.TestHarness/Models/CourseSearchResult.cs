using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.NCS.CourseSearchService.TestHarness.Models
{
    [Serializable]
    public class CourseSearchResult
    {
        // Course
        public string CourseName { get; set; }
        public string CourseID { get; set; }
        public string ProviderName { get; set; }
        public bool TFPlusLoans { get; set; }
        public string QualificationType { get; set; }
        public string QualificationLevel { get; set; }
        public string NumberOfOpportunities { get; set; }
        public string CourseSummary { get; set; }
        public string LDCS1 { get; set; }
        public string LDCS1Description { get; set; }
        public string LDCS2 { get; set; }
        public string LDCS2Description { get; set; }
        public string LDCS3 { get; set; }
        public string LDCS3Description { get; set; }
        public string LDCS4 { get; set; }
        public string LDCS4Description { get; set; }
        public string LDCS5 { get; set; }
        public string LDCS5Description { get; set; }

        // Opportunity
        public string OpportunityID { get; set; }
        public string StudyMode { get; set; }
        public string AttendanceMode { get; set; }
        public string AttendancePattern { get; set; }
        public string StartDate { get; set; }
        public string DurationValue { get; set; }
        public string DurationUnit { get; set; }
        public string DurationDescription { get; set; }
        public string StartDateDescription { get; set; }
        public string RegionName { get; set; }
        public string Venue { get; set; }
        public string Distance { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Boolean ProviderDfEFunded { get; set; }
        public Boolean CourseDfEFunded { get; set; }

        public Double? FEChoices_LearnerDestination { get; set; }
        public Double? FEChoices_LearnerSatisfaction { get; set; }
        public Double? FEChoices_EmployerSatisfaction { get; set; }
    }
}