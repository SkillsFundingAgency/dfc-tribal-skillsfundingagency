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
    
    public partial class UcasStudyModeMapping
    {
        public int UcasStudyModeId { get; set; }
        public string UcasStudyMode { get; set; }
        public Nullable<int> MapsToStudyModeId { get; set; }
        public Nullable<int> MapsToAttendanceTypeId { get; set; }
        public Nullable<int> MapsToAttendancePattern { get; set; }
    
        public virtual StudyMode StudyMode { get; set; }
        public virtual AttendancePattern AttendancePattern { get; set; }
        public virtual AttendanceType AttendanceType { get; set; }
    }
}
