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
    
    public partial class ImportBatch
    {
        public ImportBatch()
        {
            this.ImportBatchProviders = new HashSet<ImportBatchProvider>();
            this.ProviderTASRefreshes = new HashSet<ProviderTASRefresh>();
        }
    
        public int ImportBatchId { get; set; }
        public string ImportBatchName { get; set; }
        public bool Current { get; set; }
    
        public virtual ICollection<ImportBatchProvider> ImportBatchProviders { get; set; }
        public virtual ICollection<ProviderTASRefresh> ProviderTASRefreshes { get; set; }
    }
}