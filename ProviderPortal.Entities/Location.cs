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
    
    public partial class Location
    {
        public Location()
        {
            this.ApprenticeshipLocations = new HashSet<ApprenticeshipLocation>();
        }
    
        public int LocationId { get; set; }
        public int ProviderId { get; set; }
        public string ProviderOwnLocationRef { get; set; }
        public string LocationName { get; set; }
        public int AddressId { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public int RecordStatusId { get; set; }
        public string CreatedByUserId { get; set; }
        public System.DateTime CreatedDateTimeUtc { get; set; }
        public string ModifiedByUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDateTimeUtc { get; set; }
        public string BulkUploadLocationId { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual RecordStatu RecordStatu { get; set; }
        public virtual ICollection<ApprenticeshipLocation> ApprenticeshipLocations { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
    }
}
