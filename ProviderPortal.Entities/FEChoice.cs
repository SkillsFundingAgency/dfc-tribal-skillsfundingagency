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
    
    public partial class FEChoice
    {
        public FEChoice()
        {
            this.Providers = new HashSet<Provider>();
        }
    
        public int UPIN { get; set; }
        public Nullable<double> LearnerDestination { get; set; }
        public Nullable<double> LearnerSatisfaction { get; set; }
        public Nullable<double> EmployerSatisfaction { get; set; }
        public System.DateTime CreatedDateTimeUtc { get; set; }
    
        public virtual ICollection<Provider> Providers { get; set; }
    }
}
