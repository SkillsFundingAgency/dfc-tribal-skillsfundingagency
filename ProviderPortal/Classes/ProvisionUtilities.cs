using System;       
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.Win32.SafeHandles;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;
using TribalTechnology.InformationManagement.Net.Mail;

// ReSharper disable once CheckNamespace

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    /// <summary>
    /// Provider user contact details.
    /// </summary>
    public class ProviderUserContactDetails
    {
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int IsPrimaryContact { get; set; }
    }

    /// <summary>
    /// Organisation user contact details.
    /// </summary>
    public class OrganisationUserContactDetails
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int IsPrimaryContact { get; set; }
    }

    /// <summary>
    /// General utilies for provisions.
    /// </summary>
    public static class ProvisionUtilities
    {
        #region Provision Name in Use

        /// <summary>
        /// Determines whether a provision name is in use when <cref>onstants.ConfigSettings.RequireUniqueProvisionNames</cref> is set to <c>true</c>; otherwise returns <c>false</c>.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="name">Provision name to check</param>
        /// <param name="providerId">Provider Id to ignore or null for none.</param>
        /// <param name="organisationId">Organisation Id to ignore or null for none.</param>
        /// <returns></returns>
        public static bool IsNameInUse(ProviderPortalEntities db, string name, int? providerId, int? organisationId)
        {
            if (!Constants.ConfigSettings.RequireUniqueProvisionNames)
                return false;

            name = name.Trim();
            var orgs = db.Organisations.AsQueryable();
            if (organisationId != null)
            {
                orgs = orgs.Where(x => x.OrganisationId != organisationId);
            }

            var providers = db.Providers.AsQueryable();
            if (providerId != null)
            {
                providers = providers.Where(x => x.ProviderId != providerId);
            }

            return orgs.Any(x => x.OrganisationName.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                   || providers.Any(x => x.ProviderName.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        #endregion

        #region New Provider/Organisation Email

        /// <summary>
        /// Sends the new provider email.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public static void SendNewProviderEmail(Provider provider)
        {
            SendNewProvisionEmail(Constants.EmailTemplates.NewProviderNotification,
                new List<EmailParameter>
                {
                    new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                    new EmailParameter("%ADDRESS%", provider.Address.GetSingleLineHTMLAddress())
                });
        }

        /// <summary>
        /// Sends the new organisation email.
        /// </summary>
        /// <param name="organisation">The organisation.</param>
        public static void SendNewOrganisationEmail(Organisation organisation)
        {
            SendNewProvisionEmail(Constants.EmailTemplates.NewOrganisationNotification,
                new List<EmailParameter>
                {
                    new EmailParameter("%ORGANISATIONNAME%", organisation.OrganisationName),
                    new EmailParameter("%ADDRESS%", organisation.Address.GetSingleLineHTMLAddress())
                });
        }

        /// <summary>
        /// Sends the new provision email.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">template</exception>
        private static void SendNewProvisionEmail(Constants.EmailTemplates template, List<EmailParameter> parameters)
        {
            if (!(template == Constants.EmailTemplates.NewOrganisationNotification
                  || template == Constants.EmailTemplates.NewProviderNotification))
            {
                throw new ArgumentOutOfRangeException("template");
            }

            if (parameters == null)
            {
                parameters = new List<EmailParameter>();
            }
            parameters.Add(new EmailParameter("%NAME%", Constants.ConfigSettings.SupportTeamEmailName));

            var recipients = Constants.ConfigSettings.SupportTeamEmailRecipients;
            var to = new MailAddress(recipients.First());
            var cc = new MailAddressCollection();
            foreach (var recipient in recipients.Skip(1))
            {
                cc.Add(new MailAddress(recipient));
            }

            //AppGlobal.EmailQueue.AddToSendQueue(
            //    TemplatedEmail.EmailMessage(
            //        to,
            //        cc,
            //        null,
            //        template,
            //        parameters));

            var emailMessage = TemplatedEmail.EmailMessage(
                    to,
                    cc,
                    null,
                    template,
                    parameters);

            var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null, true);
        }

        #endregion

        #region Provider Traffic Light Emails

        #endregion

        #region Get provider / organisation user contact details

        public static List<ProviderUserContactDetails> GetProviderUsers(ProviderPortalEntities db,
            int providerId, bool includeNormalUsers, bool includePrimaryContacts)
        {
            return GetProviderUsers(db, new List<int> {providerId}, includeNormalUsers, includePrimaryContacts);
        }

        public static List<ProviderUserContactDetails> GetProviderUsers(ProviderPortalEntities db,
            IEnumerable<int> providerIds, bool includeNormalUsers, bool includePrimaryContacts)
        {
            var providerDt = new DataTable();
            providerDt.Columns.Add("Id", typeof (int));
            foreach (var providerId in providerIds)
            {
                providerDt.Rows.Add(providerId);
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec [dbo].[up_GetProviderUsers]  @providers, @includeUsers, @includeSuperUsers";
            cmd.Parameters.Add(new SqlParameter("@providers", SqlDbType.Structured)
            {
                Value = providerDt,
                TypeName = "dbo.IntList"
            });
            cmd.Parameters.Add(new SqlParameter("@includeUsers", includeNormalUsers));
            cmd.Parameters.Add(new SqlParameter("@includeSuperUsers", includePrimaryContacts));

            List<ProviderUserContactDetails> users;
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                users =
                    ((IObjectContextAdapter) db).ObjectContext.Translate<ProviderUserContactDetails>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }
            return users.ToList();
        }

        public static List<OrganisationUserContactDetails> GetOrganisationUsers(ProviderPortalEntities db,
            int organisationId, bool includeNormalUsers, bool includePrimaryContacts)
        {
            return GetOrganisationUsers(db, new List<int> {organisationId}, includeNormalUsers, includePrimaryContacts);
        }

        public static List<OrganisationUserContactDetails> GetOrganisationUsers(ProviderPortalEntities db,
            IEnumerable<int> organisationIds, bool includeNormalUsers, bool includePrimaryContacts)
        {
            var organisationDt = new DataTable();
            organisationDt.Columns.Add("Id", typeof (int));
            foreach (var providerId in organisationIds)
            {
                organisationDt.Rows.Add(providerId);
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec [dbo].[up_GetOrganisationUsers]  @organisations, @includeUsers, @includeSuperUsers";
            cmd.Parameters.Add(new SqlParameter("@organisations", SqlDbType.Structured)
            {
                Value = organisationDt,
                TypeName = "dbo.IntList"
            });
            cmd.Parameters.Add(new SqlParameter("@includeUsers", includeNormalUsers));
            cmd.Parameters.Add(new SqlParameter("@includeSuperUsers", includePrimaryContacts));

            List<OrganisationUserContactDetails> users;
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                users =
                    ((IObjectContextAdapter) db).ObjectContext.Translate<OrganisationUserContactDetails>(reader)
                        .ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }
            return users.ToList();
        }

        #endregion

        /// <summary>
        /// Send an organisation membership email to all provider superusers.
        /// </summary>
        /// <param name="emailTemplate">The email template.</param>
        /// <param name="providerId">The provider identifier.</param>
        /// <param name="organisationId">The organisation identifier.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void SendProviderMembershipEmail(ProviderPortalEntities db, Constants.EmailTemplates emailTemplate,
            int providerId, int organisationId, List<EmailParameter> parameters)
        {
            var providerUsers = GetProviderUsers(db, providerId, false, true).ToList();
            var organisation = db.Organisations.FirstOrDefault(x => x.OrganisationId == organisationId);
            if (organisation == null || !providerUsers.Any()) return;

            var to = new MailAddress(providerUsers.First().Email, providerUsers.First().Name);
            var cc = new MailAddressCollection();
            foreach (var user in providerUsers.Skip(1))
            {
                cc.Add(new MailAddress(user.Email, user.Name));
            }

            if (parameters == null)
            {
                parameters = new List<EmailParameter>();
            }
            parameters.AddRange(new List<EmailParameter>
            {
                new EmailParameter("%PROVIDERNAME%", providerUsers.First().ProviderName),
                new EmailParameter("%ORGANISATIONNAME%", organisation.OrganisationName)
            });
            parameters.AddRange(parameters);

            //AppGlobal.EmailQueue.AddToSendQueue(
            //    TemplatedEmail.EmailMessage(
            //        to,
            //        cc,
            //        null,
            //        emailTemplate,
            //        parameters));

            var emailMessage = TemplatedEmail.EmailMessage(
                    to,
                    cc,
                    null,
                    emailTemplate,
                    parameters);

            var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null, true);

        }

        /// <summary>
        /// Send an provider membership email to all organisation superusers.
        /// </summary>
        /// <param name="emailTemplate">The email template.</param>
        /// <param name="providerId">The provider identifier.</param>
        /// <param name="organisationId">The organisation identifier.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void SendOrganisationMembershipEmail(ProviderPortalEntities db,
            Constants.EmailTemplates emailTemplate, int providerId, int organisationId, List<EmailParameter> parameters)
        {
            var organisationUsers = GetOrganisationUsers(db, organisationId, false, true).ToList();
            var provider = db.Providers.FirstOrDefault(x => x.ProviderId == providerId);
            if (provider == null || !organisationUsers.Any()) return;

            var to = new MailAddress(organisationUsers.First().Email, organisationUsers.First().Name);
            var cc = new MailAddressCollection();
            foreach (var user in organisationUsers.Skip(1))
            {
                cc.Add(new MailAddress(user.Email, user.Name));
            }

            if (parameters == null)
            {
                parameters = new List<EmailParameter>();
            }
            parameters.AddRange(new List<EmailParameter>
            {
                new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                new EmailParameter("%ORGANISATIONNAME%", organisationUsers.First().OrganisationName)
            });
            parameters.AddRange(parameters);

            //AppGlobal.EmailQueue.AddToSendQueue(
            //    TemplatedEmail.EmailMessage(
            //        to,
            //        cc,
            //        null,
            //        emailTemplate,
            //        parameters));

            var emailMessage = TemplatedEmail.EmailMessage(
                    to,
                    cc,
                    null,
                    emailTemplate,
                    parameters);

            var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null, true);
        }
    }
}