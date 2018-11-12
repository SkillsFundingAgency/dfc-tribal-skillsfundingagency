//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tribal.SkillsFundingAgency.ProviderPortal.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class CourseInstance
    {
        public CourseInstance()
        {
            this.CourseInstanceStartDates = new HashSet<CourseInstanceStartDate>();
            this.A10FundingCode = new HashSet<A10FundingCode>();
            this.Venues = new HashSet<Venue>();
        }
    
        public int CourseInstanceId { get; set; }
        public int CourseId { get; set; }
        public int RecordStatusId { get; set; }
        public string ProviderOwnCourseInstanceRef { get; set; }
        public Nullable<int> OfferedByProviderId { get; set; }
        public Nullable<int> DisplayProviderId { get; set; }
        public Nullable<int> StudyModeId { get; set; }
        public Nullable<int> AttendanceTypeId { get; set; }
        public Nullable<int> AttendancePatternId { get; set; }
        public Nullable<int> DurationUnit { get; set; }
        public Nullable<int> DurationUnitId { get; set; }
        public string DurationAsText { get; set; }
        public string StartDateDescription { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string TimeTable { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string PriceAsText { get; set; }
        public int AddedByApplicationId { get; set; }
        public string LanguageOfInstruction { get; set; }
        public string LanguageOfAssessment { get; set; }
        public Nullable<System.DateTime> ApplyFromDate { get; set; }
        public Nullable<System.DateTime> ApplyUntilDate { get; set; }
        public string ApplyUntilText { get; set; }
        public string EnquiryTo { get; set; }
        public string ApplyTo { get; set; }
        public string Url { get; set; }
        public bool CanApplyAllYear { get; set; }
        public string CreatedByUserId { get; set; }
        public System.DateTime CreatedDateTimeUtc { get; set; }
        public string ModifiedByUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDateTimeUtc { get; set; }
        public Nullable<int> PlacesAvailable { get; set; }
        public bool BothOfferedByDisplayBySearched { get; set; }
        public Nullable<int> VenueLocationId { get; set; }
        public Nullable<int> OfferedByOrganisationId { get; set; }
        public Nullable<int> DisplayedByOrganisationId { get; set; }
    
        public virtual Application Application { get; set; }
        public virtual AttendancePattern AttendancePattern { get; set; }
        public virtual AttendanceType AttendanceType { get; set; }
        public virtual Course Course { get; set; }
        public virtual Organisation DisplayNameOrganisation { get; set; }
        public virtual DurationUnit DurationUnit1 { get; set; }
        public virtual Organisation OfferedByOrganisation { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual Provider Provider1 { get; set; }
        public virtual RecordStatu RecordStatu { get; set; }
        public virtual StudyMode StudyMode { get; set; }
        public virtual VenueLocation VenueLocation { get; set; }
        public virtual ICollection<CourseInstanceStartDate> CourseInstanceStartDates { get; set; }
        public virtual ICollection<A10FundingCode> A10FundingCode { get; set; }
        public virtual ICollection<Venue> Venues { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
    }
}
