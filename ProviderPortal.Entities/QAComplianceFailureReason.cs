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
    
    public partial class QAComplianceFailureReason
    {
        public QAComplianceFailureReason()
        {
            this.ApprenticeshipQACompliances = new HashSet<ApprenticeshipQACompliance>();
            this.ProviderQACompliances = new HashSet<ProviderQACompliance>();
        }
    
        public int QAComplianceFailureReasonId { get; set; }
        public string Description { get; set; }
        public int Ordinal { get; set; }
        public int RecordStatusId { get; set; }
        public string FullDescription { get; set; }
    
        public virtual RecordStatu RecordStatu { get; set; }
        public virtual ICollection<ApprenticeshipQACompliance> ApprenticeshipQACompliances { get; set; }
        public virtual ICollection<ProviderQACompliance> ProviderQACompliances { get; set; }
    }
}
