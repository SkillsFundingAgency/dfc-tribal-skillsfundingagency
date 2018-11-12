using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class EmailTemplateViewModel
    {
        public int EmailTemplateId { get; set; }

        [LanguageDisplay("Name")]
        [LanguageRequired]
        public string Name { get; set; }

        [LanguageDisplay("Description")]
        [LanguageRequired]
        public string Description { get; set; }

        [LanguageDisplay("Subject")]
        [LanguageRequired]
        public string Subject { get; set; }

        [LanguageDisplay("Body")]
        [LanguageRequired]
        public string HtmlBody { get; set; }

        [LanguageDisplay("Priority")]
        [LanguageRequired]
        public int Priority { get; set; }

        [LanguageDisplay("Parameters")]
        [LanguageRequired]
        public string Params { get; set; }

        public EmailTemplateViewModel()
        {
        }

        public EmailTemplateViewModel(EmailTemplate emailTemplate)
        {
            EmailTemplateId = emailTemplate.EmailTemplateId;
            Name = emailTemplate.Name;
            Description = emailTemplate.Description;
            Subject = emailTemplate.Subject;
            HtmlBody = emailTemplate.HtmlBody;
            Priority = emailTemplate.Priority;
            Params = emailTemplate.Params;
        }
    }
}