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
    
    public partial class DfEWsProviderStatu
    {
        public DfEWsProviderStatu()
        {
            this.DfEProviderStatus = new HashSet<DfEProviderStatu>();
        }
    
        public int DfEWsProviderStatusId { get; set; }
        public string DfEWsProviderStatusCode { get; set; }
        public string DfEWsProviderStatusName { get; set; }
    
        public virtual ICollection<DfEProviderStatu> DfEProviderStatus { get; set; }
    }
}
