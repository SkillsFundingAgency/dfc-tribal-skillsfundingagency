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
    
    public partial class Language
    {
        public Language()
        {
            this.LanguageTexts = new HashSet<LanguageText>();
            this.Contents = new HashSet<Content>();
        }
    
        public int LanguageID { get; set; }
        public string IETF { get; set; }
        public string DefaultText { get; set; }
        public string LanguageFieldName { get; set; }
        public Nullable<int> SqlLanguageId { get; set; }
        public bool IsDefaultLanguage { get; set; }
    
        public virtual ICollection<LanguageText> LanguageTexts { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
    }
}
