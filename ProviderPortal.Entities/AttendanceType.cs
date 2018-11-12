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
    
    public partial class AttendanceType
    {
        public AttendanceType()
        {
            this.CourseInstances = new HashSet<CourseInstance>();
            this.UcasStudyModeMappings = new HashSet<UcasStudyModeMapping>();
            this.SearchPhrases = new HashSet<SearchPhrase>();
        }
    
        public int AttendanceTypeId { get; set; }
        public string AttendanceTypeName { get; set; }
        public string BulkUploadRef { get; set; }
        public int DisplayOrder { get; set; }
    
        public virtual ICollection<CourseInstance> CourseInstances { get; set; }
        public virtual ICollection<UcasStudyModeMapping> UcasStudyModeMappings { get; set; }
        public virtual ICollection<SearchPhrase> SearchPhrases { get; set; }
    }
}