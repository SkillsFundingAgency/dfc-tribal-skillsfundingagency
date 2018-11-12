using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using TribalTechnology.InformationManagement.Net.Mail;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Classes
{
    public static class SfaSendGridClient
    {
        /// <summary>
        /// Sends an EmailMessage using SendGrid client. 
        /// </summary>
        /// <param name="emailMessage">The <see cref="TribalTechnology.InformationManagement.Net.Mail.EmailMessage"/> to send.</param>
        /// <param name="userId">The UserID to sent the email to.</param>
        /// <param name="toMiltipleRecipients">Indicates Miltiple Recipients (optional)</param>
        /// <returns>A <see cref="SendGrid.Response"/>.</returns>
        public static Task<Response> SendGridEmailMessage(EmailMessage emailMessage, string userId, bool toMiltipleRecipients = false)
        {
            // Used to enable/disable Email Services in DEV and TEST environments.
            if (!Constants.ConfigSettings.EmailServiceEnabled)
                return null;

            var toEmail = new EmailAddress();
            if (!string.IsNullOrEmpty(userId))
            {
                var db = new ProviderPortalEntities();
                var user = db.AspNetUsers.First(x => x.Id == userId);
                toEmail = new EmailAddress(user.Email, user.Name);
            }
            else
            {
                toEmail = new EmailAddress(emailMessage.To[0].Address, emailMessage.To[0].DisplayName);
            }

            if(toEmail == null)
            {
                return null;
            }
            var clientSendGrid = new SendGridClient(Constants.ConfigSettings.SmtpPassword);
            var fromEmail = new EmailAddress(Constants.ConfigSettings.EmailSenderEmailAddress, Constants.ConfigSettings.EmailSenderUsername);
            var subject = emailMessage.SubjectWithKeysToValue;           
            var plainTextContent = emailMessage.BodyWithKeysToValue; // "plainTextContent - and easy to do anywhere, even with C#";
            var htmlContent = emailMessage.BodyWithKeysToValue; //"htmlContent - <strong>and easy to do anywhere, even with C#</strong>";

            var msg = new SendGridMessage();
            if (toMiltipleRecipients)
            {
                var toEmailList = new List<EmailAddress>();
                toEmailList.Add(toEmail);
                foreach(var emailAddress in emailMessage.CC)
                {
                    toEmailList.Add(new EmailAddress(emailAddress.Address, emailAddress.DisplayName));
                }

                msg = MailHelper.CreateSingleEmailToMultipleRecipients(fromEmail, toEmailList, subject, plainTextContent, htmlContent);
            }
            else
            {
                msg = MailHelper.CreateSingleEmail(fromEmail, toEmail, subject, plainTextContent, htmlContent);
            }

            var response = clientSendGrid.SendEmailAsync(msg);

            return response;
        }
    }
}