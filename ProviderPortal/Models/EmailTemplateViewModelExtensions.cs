using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class EmailTemplateViewModelExtensions
    {
        public static EmailTemplateViewModel Populate(this EmailTemplateViewModel model, ProviderPortalEntities db)
        {
            return model;
        }

        public static EmailTemplate ToEntity(this EmailTemplateViewModel model, ProviderPortalEntities db)
        {
            var emailTemplate = model.EmailTemplateId == 0 ? new EmailTemplate() : db.EmailTemplates.Find(model.EmailTemplateId);
            emailTemplate.Name = model.Name;
            emailTemplate.Description = model.Description;
            emailTemplate.Subject = model.Subject;
            emailTemplate.HtmlBody = model.HtmlBody;
            emailTemplate.Priority = model.Priority;
            emailTemplate.Params = model.Params;
            return emailTemplate;
        }
    }
}