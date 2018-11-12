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
    
    public partial class Standard
    {
        public Standard()
        {
            this.Apprenticeships = new HashSet<Apprenticeship>();
        }
    
        public int StandardCode { get; set; }
        public int Version { get; set; }
        public string StandardName { get; set; }
        public string StandardSectorCode { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public string URLLink { get; set; }
        public Nullable<decimal> SectorSubjectAreaTier1 { get; set; }
        public Nullable<decimal> SectorSubjectAreaTier2 { get; set; }
        public System.DateTime CreatedDateTimeUtc { get; set; }
        public Nullable<System.DateTime> ModifiedDateTimeUtc { get; set; }
        public int RecordStatusId { get; set; }
        public string NotionalEndLevel { get; set; }
        public string OtherBodyApprovalRequired { get; set; }
    
        public virtual SectorSubjectAreaTier1 SectorSubjectAreaTier11 { get; set; }
        public virtual SectorSubjectAreaTier2 SectorSubjectAreaTier21 { get; set; }
        public virtual StandardSectorCode StandardSectorCode1 { get; set; }
        public virtual ICollection<Apprenticeship> Apprenticeships { get; set; }
        public virtual RecordStatu RecordStatu { get; set; }
    }
}