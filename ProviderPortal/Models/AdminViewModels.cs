using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using System.ComponentModel.DataAnnotations;

    public class AdminDashboardViewModel
    {
        [LanguageRequired]
        [LanguageDisplay("Provider or organisation name")]
        public string Provider { get; set; }

        /// <summary>
        /// The selected provider or organisation ID.
        /// </summary>
        [LanguageRequired]
        public string ProviderId { get; set; }

        /// <summary>
        /// The Action to redirect to after switching, or Index if unset.
        /// </summary>
        public string SuccessAction { get; set; }

        /// <summary>
        /// The Controller to redirect to after switching, or Home if unset.
        /// </summary>
        public string SuccessController { get; set; }

        /// <summary>
        /// The Action to redirect to if switching fails, or Index if unset.
        /// </summary>
        public string FailureAction { get; set; }

        /// <summary>
        /// The Controller to redirect to after switching, or Home if unset.
        /// </summary>
        public string FailureController { get; set; }

        /// <summary>
        /// A list of providers that the user is associated with.
        /// </summary>
        [LanguageDisplay("Your providers")]
        public IEnumerable<SelectListItem> UserProviders { get; set; }

        /// <summary>
        /// A list of providers that the user is associated with.
        /// </summary>
        [LanguageDisplay("Your organisations")]
        public IEnumerable<SelectListItem> UserOrganisations { get; set; }


        /// <summary>
        /// A list of providers that the user has viewed recently.
        /// </summary>
        [LanguageDisplay("Recent providers")]
        public IEnumerable<SelectListItem> RecentProviders { get; set; }

        /// <summary>
        /// A list of providers that the user has viewed recently.
        /// </summary>
        [LanguageDisplay("Recent organisations")]
        public IEnumerable<SelectListItem> RecentOrganisations { get; set; }
    }
}