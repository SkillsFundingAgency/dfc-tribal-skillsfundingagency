using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.NCS.CourseSearchService.TestHarness.Models
{
    /// <summary>
    /// Represents the full course information
    /// </summary>
    public class CourseInformation
    {
        // course information
        public string CourseId { get; set; }
        public string ProviderCourseTitle { get; set; }
        public string Summary { get; set; }
        public string URL { get; set; }
        public string BookingURL { get; set; }
        public string EntryRequirements { get; set; }
        public string AssessmentMethod { get; set; }
        public string EquipmentRequired { get; set; }
        public string TariffRequired { get; set; }
        public string LearningAimRef { get; set; }

        // referred LAD data
        public string AwardingOrganisationName { get; set; }
        public string Level2EntitlementCategoryDescription { get; set; }
        public string Level3EntitlementCategoryDescription { get; set; }
        public string SectorLeadBodyDescription { get; set; }
        public string AccreditationStartDate { get; set; }
        public string AccreditationEndDate { get; set; }
        public string CertificationEndDate { get; set; }
        public string CreditValue { get; set; }
        public string QCAGuidedLearningHours { get; set; }
        public string IndependentLivingSkills { get; set; }
        public string SkillsforLifeFlag { get; set; }
        public string SkillsForLifeTypeDescription { get; set; }
        public string ERAppStatus { get; set; }
        public string ERTTGStatus { get; set; }
        public string AdultLRStatus { get; set; }
        public string OtherFundingNonFundingStatus { get; set; }

        // conditionally derived data
        public string DataType { get; set; }
        public string QualificationReferenceAuthority { get; set; }
        public string QualificationReference { get; set; }
        public string QualificationTitle { get; set; }
        public string QualificationType { get; set; }
        public string QualificationLevel { get; set; }
        public string LDCSCategoryCodeApplicability1 { get; set; }
        public string LDCSCategoryCodeApplicability2 { get; set; }

        // opportunity information
        public List<Opportunity> Opportunities { get; set; }

        // provider information
        public ProviderSearchResult Provider { get; set; }

        // venue information
        public List<Venue> Venues { get; set; }  
    }
}