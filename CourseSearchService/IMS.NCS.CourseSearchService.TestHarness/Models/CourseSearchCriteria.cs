using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.NCS.CourseSearchService.TestHarness.Models
{
    [Serializable]
    public class CourseSearchCriteria
    {
        // Helper properties - not partof search criteria
        public int Page { get; set; }
        
        // API Key
        public String APIKey { get; set; }

        // Subject
        public string Subject { get; set; }        
        public string LDCSCategoryCodes { get; set; }
        public String DFE1619Funded { get; set; }

        // Region
        public string LocationPostCode { get; set; }
        public string MaxDistance { get; set; }
        public string SortBy { get; set; }
        public string RecordsPerPage { get; set; }

        // Provider
        public string ProviderID { get; set; }
        public string ProviderText { get; set; }

        // Qualification
        public string[] QualificationTypes { get; set; }
        public string[] QualificationLevels { get; set; }

        // Study Mode
        public string EarliestStartDate { get; set; }
        public string[] StudyModes { get; set; }
        public string[] AttendanceModes { get; set; }
        public string[] AttendancePatterns { get; set; }

        // Flags
        public string IncFlexibleStartDateFlag { get; set; }
        public string IncIfOpportunityApplicationClosedFlag { get; set; }
        public string IncTTGFlag { get; set; }
        public string IncTQSFlag { get; set; }
        public string IncIESFlag { get; set; }
        public string[] A10Flag { get; set; }
        public string IndLivingSkillsFlag { get; set; }
        public string SkillsForLifeFlag { get; set; }

        // Status
        public string[] ERAppStatus { get; set; }
        public string[] ERTTGStatus { get; set; }
        public string[] AdultLRStatus { get; set; }
        public string[] OtherFundingStatus { get; set; }
    }
}