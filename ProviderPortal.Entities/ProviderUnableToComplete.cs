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
    
    public partial class ProviderUnableToComplete
    {
        public ProviderUnableToComplete()
        {
            this.UnableToCompleteFailureReasons = new HashSet<UnableToCompleteFailureReason>();
        }
    
        public int ProviderUnableToCompleteId { get; set; }
        public int ProviderId { get; set; }
        public string CreatedByUserId { get; set; }
        public System.DateTime CreatedDateTimeUtc { get; set; }
        public string TextUnableToComplete { get; set; }
        public bool Active { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual ICollection<UnableToCompleteFailureReason> UnableToCompleteFailureReasons { get; set; }
    }
}
