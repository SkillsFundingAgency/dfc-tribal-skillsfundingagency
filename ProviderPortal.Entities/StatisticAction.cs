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
    
    public partial class StatisticAction
    {
        public StatisticAction()
        {
            this.ProviderPortalUsageStatistics = new HashSet<ProviderPortalUsageStatistic>();
        }
    
        public int StatisticActionId { get; set; }
        public string StatisticActionName { get; set; }
    
        public virtual ICollection<ProviderPortalUsageStatistic> ProviderPortalUsageStatistics { get; set; }
    }
}
