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
    
    public partial class LanguageField
    {
        public LanguageField()
        {
            this.LanguageTexts = new HashSet<LanguageText>();
        }
    
        public int LanguageFieldId { get; set; }
        public string LanguageFieldName { get; set; }
        public int LanguageKeyChildId { get; set; }
        public string DefaultLanguageText { get; set; }
    
        public virtual LanguageKeyChild LanguageKeyChild { get; set; }
        public virtual ICollection<LanguageText> LanguageTexts { get; set; }
    }
}
