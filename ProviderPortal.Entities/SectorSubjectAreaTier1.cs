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
    
    public partial class SectorSubjectAreaTier1
    {
        public SectorSubjectAreaTier1()
        {
            this.Standards = new HashSet<Standard>();
            this.Frameworks = new HashSet<Framework>();
        }
    
        public decimal SectorSubjectAreaTier1Id { get; set; }
        public string SectorSubjectAreaTier1Desc { get; set; }
        public string SectorSubjectAreaTier1Desc2 { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public System.DateTime CreatedDateTimeUtc { get; set; }
        public Nullable<System.DateTime> ModifiedDateTimeUtc { get; set; }
    
        public virtual ICollection<Standard> Standards { get; set; }
        public virtual ICollection<Framework> Frameworks { get; set; }
    }
}
