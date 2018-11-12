using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class ProviderTrafficLightStatusViewModel
    {
        public readonly string Skipped = "Skipped";
        public readonly string EmailAlreadySent = "Email already sent";
        public readonly string StillRed = "Still red";
        public readonly string RedToday = "Red today";
        public readonly string RedInOneWeek = "Red in one week";
        public readonly string AmberForOneWeek = "Amber for one week";
        public readonly string AmberToday = "Amber today";

        public Dictionary<string, List<string>> Log { get; set; }
        public DateTime Today { get; set; }

        public ProviderTrafficLightStatusViewModel()
        {
            Log = new Dictionary<string, List<string>>
            {
                {AmberToday, new List<string>()},
                {AmberForOneWeek, new List<string>()},
                {RedInOneWeek, new List<string>()},
                {RedToday, new List<string>()},
                {StillRed, new List<string>()},
                {EmailAlreadySent, new List<string>()},
                {Skipped, new List<string>()},
            };
        }

        /// <summary>
        /// Provider traffic light information.
        /// </summary>
        public class ProviderTrafficLightEmail
        {
            /// <summary>
            /// Gets or sets the provider identifier.
            /// </summary>
            /// <value>
            /// The provider identifier.
            /// </value>
            public int ProviderId { get; set; }

            /// <summary>
            /// Gets or sets the name of the provider.
            /// </summary>
            /// <value>
            /// The name of the provider.
            /// </value>
            public string ProviderName { get; set; }

            /// <summary>
            /// Gets or sets the provider last modified date time UTC.
            /// </summary>
            /// <value>
            /// The provider last modified date time UTC.
            /// </value>
            public DateTime? ModifiedDateTimeUtc { get; set; }

            /// <summary>
            /// Gets or sets the last sent traffic light email date time UTC.
            /// </summary>
            /// <value>
            /// The last sent traffic light email date time UTC.
            /// </value>
            public DateTime? LastEmailDateTimeUtc { get; set; }

            /// <summary>
            /// Gets or sets this email date time UTC.
            /// </summary>
            /// <value>
            /// The email date time UTC.
            /// </value>
            public DateTime? EmailDateTimeUtc { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the provider is SFA funded.
            /// </summary>
            /// <value>
            ///   <c>true</c> if SFA funded; otherwise, <c>false</c>.
            /// </value>
            public bool SFAFunded { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether DfE 16-19 funded.
            /// </summary>
            /// <value>
            ///   <c>true</c> if DfE 16-19 funded; otherwise, <c>false</c>.
            /// </value>
            public bool DFE1619Funded { get; set; }

            /// <summary>
            /// Gets or sets the email template being sent.
            /// </summary>
            /// <value>
            /// The email template beimg sent.
            /// </value>
            public Constants.EmailTemplates? EmailTemplateId { get; set; }

            // Additional fields for reporting purposes

            /// <summary>
            /// Gets or sets the next email template the provider will receive if they do not update their provision.
            /// </summary>
            /// <value>
            /// The next email template the provider will receive.
            /// </value>
            public Constants.EmailTemplates? NextEmailTemplateId { get; set; }

            /// <summary>
            /// Gets or sets the next email date time UTC.
            /// </summary>
            /// <value>
            /// The next email date time UTC.
            /// </value>
            public DateTime? NextEmailDateTimeUtc { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance has valid recipients, i.e. there was a primary contact to send the email to.
            /// </summary>
            /// <value>
            /// <c>true</c> if this instance has valid recipients; otherwise, <c>false</c>.
            /// </value>
            public bool HasValidRecipients { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the provider has quality emails paused.
            /// </summary>
            /// <value>
            ///   <c>true</c> if quality emails are paused; otherwise, <c>false</c>.
            /// </value>
            public bool QualityEmailsPaused { get; set; }

            /// <summary>
            /// Gets or sets the provider created date time UTC.
            /// </summary>
            /// <value>
            /// The provider created date time UTC.
            /// </value>
            public DateTime ProviderCreatedDateTimeUtc { get; set; }

            /// <summary>
            /// The traffic light status at the time of recording.
            /// </summary>
            public QualityIndicator.TrafficLight TrafficLightStatusId;
        }
    }
}