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
    
    public partial class UCAS_Fee
    {
        public int FeeId { get; set; }
        public int CourseIndexId { get; set; }
        public int CurrencyId { get; set; }
        public int StudyPeriodId { get; set; }
        public Nullable<int> FeeYearId { get; set; }
        public decimal Fee { get; set; }
    }
}