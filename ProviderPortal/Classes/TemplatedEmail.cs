// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Defines the TemplatedEmail factory class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Text;

// ReSharper disable once CheckNamespace


namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Entities;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using TribalTechnology.InformationManagement.Net.Mail;

    /// <summary>
    /// Factory class for creating templated EmailMessages.
    /// </summary>
    public static class TemplatedEmail
    {
        private const string cacheKeyBase = "EmailTemplate:";

        static TemplatedEmail()
        {
            LoadCache();
        }

        /// <summary>
        /// Invalidate the email template cache.
        /// </summary>
        public static void InvalidateCache()
        {
            CacheManagement.CacheHandler.Invalidate(cacheKeyBase);
        }

        /// <summary>
        /// Reload the email template cache.
        /// </summary>
        public static void LoadCache()
        {
            InvalidateCache();
            var db = new ProviderPortalEntities();
            foreach (var template in db.EmailTemplates)
            {
                var key = CacheKey(template.EmailTemplateId);
                CacheManagement.CacheHandler.Add(key, template);
            }
        }

        /// <summary>
        /// Create an EmailMessage based on a template.
        /// </summary>
        /// <param name="userId">The UserID to sent the email to.</param>
        /// <param name="emailTemplate">The <see cref="Constants.EmailTemplates"/> to send.</param>
        /// <param name="parameters">A list of <see cref="TribalTechnology.InformationManagement.Net.Mail.EmailParameter"/>s, or null for none.</param>
        /// <param name="overrideRecipientText">n option text to add when the override recipient option is in use.  Leave blank tp get text from language sub-system</param>
        /// <returns>A <see cref="TribalTechnology.InformationManagement.Net.Mail.EmailMessage"/>.</returns>
        public static EmailMessage EmailMessage(string userId, Constants.EmailTemplates emailTemplate,
            List<EmailParameter> parameters = null, String overrideRecipientText = "")
        {
            return EmailMessage(userId, null, emailTemplate, parameters, overrideRecipientText);
        }

        /// <summary>
        /// Create an EmailMessage based on a template.
        /// </summary>
        /// <param name="userId">The UserID to sent the email to.</param>
        /// <param name="from">The MailAddress to sent the email from, or NULL to use the site defaults.</param>
        /// <param name="emailTemplate">The <see cref="Constants.EmailTemplates"/> to send.</param>
        /// <param name="parameters">A list of <see cref="TribalTechnology.InformationManagement.Net.Mail.EmailParameter"/>s, or null for none.</param>
        /// <param name="overrideRecipientText">n option text to add when the override recipient option is in use.  Leave blank tp get text from language sub-system</param>
        /// <returns>A <see cref="TribalTechnology.InformationManagement.Net.Mail.EmailMessage"/>.</returns>
        public static EmailMessage EmailMessage(string userId, MailAddress from, Constants.EmailTemplates emailTemplate,
            List<EmailParameter> parameters = null, String overrideRecipientText = "")
        {
            var db = new ProviderPortalEntities();
            var user = db.AspNetUsers.First(x => x.Id == userId);
            var to = new MailAddress(user.Email, user.Name);
            return EmailMessage(to, null, null, emailTemplate, parameters, overrideRecipientText);
        }

        /// <summary>
        /// Create an EmailMessage based on a template.
        /// </summary>
        /// <param name="to">The MailAddress to sent the email to.</param>
        /// <param name="cc">A collection of email addresses to carbon copy the message to.</param>
        /// <param name="from">The MailAddress to sent the email from, or NULL to use the site defaults.</param>
        /// <param name="emailTemplate">The <see cref="Constants.EmailTemplates"/> to send.</param>
        /// <param name="parameters">A list of <see cref="TribalTechnology.InformationManagement.Net.Mail.EmailParameter"/>s, or null for none.</param>
        /// <param name="overrideRecipientText">n option text to add when the override recipient option is in use.  Leave blank tp get text from language sub-system</param>
        /// <returns>A <see cref="TribalTechnology.InformationManagement.Net.Mail.EmailMessage"/>.</returns>
        public static EmailMessage EmailMessage(MailAddress to, MailAddressCollection cc, MailAddress @from, Constants.EmailTemplates emailTemplate, List<EmailParameter> parameters, String overrideRecipientText = "")
        {
            if (String.IsNullOrWhiteSpace(overrideRecipientText))
            {
                overrideRecipientText = AppGlobal.Language.GetText("TemplatedEmail_EmailOverride_FormatString", "<p>This email was originally sent to {0}:<p>{1}");
            }

            var overrideRecipient = OverrideRecipient(to);
            var settings = new ConfigurationSettings();
            if (@from == null)
                @from = new MailAddress(settings.AutomatedFromEmailAddress,
                    settings.AutomatedFromEmailName);

            var @base = GetBaseTemplate();

            EmailTemplate template = (EmailTemplate) CacheManagement.CacheHandler.Get(CacheKey((int) emailTemplate));

            // If items have been removed from cache then read them from the database
            if (@base == null || template == null)
            {
                ProviderPortalEntities db = new ProviderPortalEntities();
                @base = db.EmailTemplates.Find((Int32)Constants.EmailTemplates.Base);
                template = db.EmailTemplates.Find((Int32) emailTemplate);
            }

            EmailMessage email;
            StringBuilder sbOriginalRecipients = new StringBuilder();
            if (overrideRecipient)
            {
                var recipients = Constants.ConfigSettings.EmailOverrideRecipients;
                var newTo = new MailAddress(recipients.First());
                email = new EmailMessage(newTo, @from);
                if (recipients.Count() > 1)
                    email.CC.Add(String.Join(",", recipients.Skip(1)));
                sbOriginalRecipients.AppendFormat("{0} ({1})", to.DisplayName, to.Address);
                if (cc != null)
                {
                    foreach (var ma in cc)
                    {
                        sbOriginalRecipients.AppendFormat(", {0} ({1})", ma.DisplayName, ma.Address);
                    }
                }
            }
            else
            {
                email = new EmailMessage(to, @from);
            }
            email.Subject = @base.Subject;
            email.Body = overrideRecipient
                ? String.Format(
                    overrideRecipientText,
                    sbOriginalRecipients, @base.HtmlBody)
                : @base.HtmlBody;
            email.Priority =
                template.Priority == 1
                    ? MailPriority.High
                    : template.Priority == 2
                        ? MailPriority.Normal
                        : MailPriority.Low;
            email.IsBodyHtml = true;
            if (cc != null && !overrideRecipient)
            {
                foreach (var ma in cc)
                {
                    email.CC.Add(ma);
                }
            }

            parameters = parameters ?? new List<EmailParameter>();
            if (parameters.All(k => k.Key != "%SUBJECT%"))
                parameters.Add(new EmailParameter("%SUBJECT%", template.Subject));
            if (parameters.All(k => k.Key != "%NAME%"))
                parameters.Add(new EmailParameter("%NAME%", to.DisplayName));
            if (parameters.All(k => k.Key != "%HTMLBODY%"))
                parameters.Add(new EmailParameter("%HTMLBODY%", template.HtmlBody));
            if (parameters.All(k => k.Key != "%TEXTBODY%"))
                parameters.Add(new EmailParameter("%TEXTBODY%", template.TextBody));

            foreach (var parameter in parameters)
                email.AddEmailParameter(parameter.Key, parameter.Value);

            // De-reference master template parameters
            email.Subject = email.SubjectWithKeysToValue;
            email.Body = email.BodyWithKeysToValue;

            return email;
        }

        private static EmailTemplate GetBaseTemplate()
        {
            EmailTemplate @base =
                (EmailTemplate) CacheManagement.CacheHandler.Get(CacheKey((int) Constants.EmailTemplates.Base));

            if (@base == null)
            {
                LoadCache();
                @base = (EmailTemplate) CacheManagement.CacheHandler.Get(CacheKey((int) Constants.EmailTemplates.Base));
            }
            
            return @base;
        }

        /// <summary>
        /// Get the cache key for the specified EmailTemplate.
        /// </summary>
        /// <param name="emailTemplate"></param>
        /// <returns></returns>
        private static string CacheKey(int emailTemplate)
        {
            return cacheKeyBase + emailTemplate;
        }

        /// <summary>
        /// Get whether the message should be sent to the specified recipient or the currently configured email override address.
        /// </summary>
        /// <param name="to">The original recipient details.</param>
        /// <returns>Whether to send the email to the override address.</returns>
        private static bool OverrideRecipient(MailAddress to)
        {
            return Constants.ConfigSettings.EmailOverrideEnabled
                   &&
                   !to.Address.EndsWith("@"+Constants.ConfigSettings.EmailOverrideSafeDomain, true, CultureInfo.CurrentCulture);
        }
    }
}