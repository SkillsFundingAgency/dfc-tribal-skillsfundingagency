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
    
    public partial class EmailTemplate
    {
        public EmailTemplate()
        {
            this.QualityEmailLogs = new HashSet<QualityEmailLog>();
            this.QualityEmailLogs1 = new HashSet<QualityEmailLog>();
        }
    
        public int EmailTemplateId { get; set; }
        public int EmailTemplateGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Params { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public int Priority { get; set; }
        public string UserDescription { get; set; }
    
        public virtual EmailTemplateGroup EmailTemplateGroup { get; set; }
        public virtual ICollection<QualityEmailLog> QualityEmailLogs { get; set; }
        public virtual ICollection<QualityEmailLog> QualityEmailLogs1 { get; set; }
    }
}
