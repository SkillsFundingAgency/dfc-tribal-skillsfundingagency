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
    
    public partial class DeliveryMode
    {
        public DeliveryMode()
        {
            this.ApprenticeshipLocations = new HashSet<ApprenticeshipLocation>();
        }
    
        public int DeliveryModeId { get; set; }
        public string DeliveryModeName { get; set; }
        public string BulkUploadRef { get; set; }
        public int RecordStatusId { get; set; }
        public string DeliveryModeDescription { get; set; }
        public string DASRef { get; set; }
        public bool MustHaveFullLocation { get; set; }
    
        public virtual RecordStatu RecordStatu { get; set; }
        public virtual ICollection<ApprenticeshipLocation> ApprenticeshipLocations { get; set; }
    }
}
