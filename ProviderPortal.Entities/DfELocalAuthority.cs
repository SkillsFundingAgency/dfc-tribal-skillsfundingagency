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
    
    public partial class DfELocalAuthority
    {
        public DfELocalAuthority()
        {
            this.Providers = new HashSet<Provider>();
        }
    
        public int DfELocalAuthorityId { get; set; }
        public string DfELocalAuthorityCode { get; set; }
        public string DfELocalAuthorityName { get; set; }
    
        public virtual ICollection<Provider> Providers { get; set; }
    }
}
