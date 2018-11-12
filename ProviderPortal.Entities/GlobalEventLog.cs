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
    
    public partial class GlobalEventLog
    {
        public int EventID { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<int> TypeID { get; set; }
        public Nullable<int> SourceID { get; set; }
        public Nullable<int> ComputerID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Event { get; set; }
        public Nullable<System.DateTime> DateTimeUtc { get; set; }
    
        public virtual GlobalEventComputer GlobalEventComputer { get; set; }
        public virtual GlobalEventSource GlobalEventSource { get; set; }
        public virtual GlobalEventType GlobalEventType { get; set; }
        public virtual GlobalEventUser GlobalEventUser { get; set; }
    }
}