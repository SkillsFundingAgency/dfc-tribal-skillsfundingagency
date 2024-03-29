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
    
    public partial class ProviderQACompliance
    {
        public ProviderQACompliance()
        {
            this.QAComplianceFailureReasons = new HashSet<QAComplianceFailureReason>();
        }
    
        public int ProviderQAComplianceId { get; set; }
        public int ProviderId { get; set; }
        public string CreatedByUserId { get; set; }
        public System.DateTime CreatedDateTimeUtc { get; set; }
        public string TextQAd { get; set; }
        public string DetailsOfUnverifiableClaim { get; set; }
        public bool Passed { get; set; }
        public string DetailsOfComplianceFailure { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual ICollection<QAComplianceFailureReason> QAComplianceFailureReasons { get; set; }
    }
}
