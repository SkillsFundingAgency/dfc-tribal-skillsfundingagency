//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMS.NCS.CourseSearchService.DatabaseContext
{
    using System;
    
    public partial class API_Course_GetById_v2_Result
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string QualificationTypeRef { get; set; }
        public string Qualification_Level { get; set; }
        public string LDCS1 { get; set; }
        public string LDCS2 { get; set; }
        public string LDCS3 { get; set; }
        public string LDCS4 { get; set; }
        public string LDCS5 { get; set; }
        public string CourseSummary { get; set; }
        public string AwardingOrganisationName { get; set; }
        public string AssessmentMethod { get; set; }
        public string BookingUrl { get; set; }
        public Nullable<int> UcasTariffPoints { get; set; }
        public string QualificationDataType { get; set; }
        public string EntryRequirements { get; set; }
        public string EquipmentRequired { get; set; }
        public string LearningAimRef { get; set; }
        public string QualificationRefAuthority { get; set; }
        public string QualificationRef { get; set; }
        public string QualificationTitle { get; set; }
        public string Url { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public string Website { get; set; }
        public int Ukprn { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public Nullable<int> UPIN { get; set; }
        public bool Loans24Plus { get; set; }
        public string AttendanceModeBulkUploadRef { get; set; }
        public string AttendancePatternBulkUploadRef { get; set; }
        public string Duration_Description { get; set; }
        public Nullable<int> DurationValue { get; set; }
        public string DurationUnit { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string PriceAsText { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string StartDateDescription { get; set; }
        public string StudyModeBulkUploadRef { get; set; }
        public string TimeTable { get; set; }
        public string RegionName { get; set; }
        public Nullable<int> VenueId { get; set; }
        public bool CanApplyAllYear { get; set; }
        public Nullable<System.DateTime> ApplyFromDate { get; set; }
        public string ApplyTo { get; set; }
        public Nullable<System.DateTime> ApplyUntilDate { get; set; }
        public string ApplyUntilText { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string EnquiryTo { get; set; }
        public string LanguageOfAssessment { get; set; }
        public string LanguageOfInstruction { get; set; }
        public Nullable<int> PlacesAvailable { get; set; }
        public string ProviderOwnCourseInstanceRef { get; set; }
        public string CourseInstanceUrl { get; set; }
        public string A10FundingCode { get; set; }
        public int OpportunityId { get; set; }
        public Nullable<bool> CourseDfEFunded { get; set; }
        public Nullable<double> FEChoices_LearnerDestination { get; set; }
        public Nullable<double> FEChoices_LearnerSatisfaction { get; set; }
        public Nullable<double> FEChoices_EmployerSatisfaction { get; set; }
    }
}
