// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Tribal" file="WebEmailSendQueue.cs">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   The email queue
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    using System.Web.Hosting;

    using TribalTechnology.InformationManagement.Interfaces;

    /// <summary>
    /// Inherits the EmailSendQueue and implements the IRegisteredObject to ensure the task is closed down cleanly by the hosting environment
    /// </summary>
    public class WebEmailSendQueue: TribalTechnology.InformationManagement.Net.Mail.EmailSendQueue, IRegisteredObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEmailSendQueue"/> class.
        /// </summary>
        /// <param name="smtpHost">
        /// The SMTP host.
        /// </param>
        /// <param name="smtpPort">
        /// The SMTP port.
        /// </param>
        /// <param name="isSsl">
        /// Indicates if connection is SSL.
        /// </param>
        /// <param name="smtpUserName">
        /// The SMTP user name.
        /// </param>
        /// <param name="smtpPassword">
        /// The SMTP password.
        /// </param>
        /// <param name="defaultFromEmailAddress">
        /// The default from email address.
        /// </param>
        /// <param name="eventLog">
        /// The event log.
        /// </param>
        /// <param name="isRetryPolicyEnabled">
        /// The is retry policy enabled.
        /// </param>
        public WebEmailSendQueue(string smtpHost, int smtpPort, bool isSsl, string smtpUserName, string smtpPassword, string defaultFromEmailAddress, ILog eventLog, bool isRetryPolicyEnabled)
            : base(
                smtpHost,
                smtpPort,
                isSsl,
                smtpUserName,
                smtpPassword,
                defaultFromEmailAddress,
                eventLog,
                isRetryPolicyEnabled)
        {
        }

        /// <summary>
        /// Used by the hosting service to dispose the email queue
        /// </summary>
        /// <param name="immediate">When true indicates an immediate exit of the application</param>
        public void Stop(bool immediate)
        {           
            this.Dispose();  // Dispose blocks until the object has shut down cleaning
            HostingEnvironment.UnregisterObject(this); 
        }
    }
}