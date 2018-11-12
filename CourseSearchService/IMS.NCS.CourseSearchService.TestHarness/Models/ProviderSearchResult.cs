using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.NCS.CourseSearchService.TestHarness.Models
{
    /// <summary>
    /// Model for the Provider details.
    /// </summary>
    [Serializable]
    public class ProviderSearchResult
    {
        /// <summary>
        /// the Provider name.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// The Provider Id.
        /// </summary>
        public string ProviderID { get; set; }

        /// <summary>
        /// The address line 1 of the Provider.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// The address line 2 of the Provider.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// The town of the Provider.
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// The county of the Provider.
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// The postcode of the provider.
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// The email of the Provider.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The Provider's website.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// The Provider's phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The Provider's fax number.
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// The Provider's UKPRN.
        /// </summary>
        public string UKPRN { get; set; }

        /// <summary>
        /// The Provider's UPIN.
        /// </summary>
        public string UPIN { get; set; }

        /// <summary>
        /// The provider's 24+ loans flag
        /// </summary>
        public bool TFPlusLoans { get; set; }

        public Boolean DFE1619Funded { get; set; }

        public Double? FEChoices_LearnerDestination { get; set; }
        public Double? FEChoices_LearnerSatisfaction { get; set; }
        public Double? FEChoices_EmployerSatisfaction { get; set; }
    }
}