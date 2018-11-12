using System;
using System.Web;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using Entities;

    /// <summary>
    /// The address view model.
    /// </summary>
    public class LocationAddressViewModel
    {
        /// <summary>
        /// Gets or sets the address id.
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// Gets or sets the address line 1.
        /// </summary>
        [StringLength(100, ErrorMessage = "The {0} must be less than {1} characters long.")]
        [LanguageDisplay("Address line 1")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Address Line 1")]
        [Display(Description = "Enter a postcode and click Find Address, then select your address from the drop down. You can also just enter a postcode and leave the rest blank.")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line 2.
        /// </summary>
        [StringLength(100, ErrorMessage = "The {0} must be less than {1} characters long.")]
        [LanguageDisplay("Address line 2")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Address Line 2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the town.
        /// </summary>
        [StringLength(75, ErrorMessage = "The {0} must be less than {1} characters long.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Town")]
        [LanguageDisplay("Town")]
        public string Town { get; set; }

        /// <summary>
        /// Gets or sets the county.
        /// </summary>
        [StringLength(75, ErrorMessage = "The {0} must be less than {1} characters long.")]
        [LanguageDisplay("County")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid County")]
        public string County { get; set; }

        /// <summary>
        /// Gets or sets the postcode.
        /// </summary>
        [LanguageRequired]
        [StringLength(8, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 6)]
        [LanguageDisplay("Post code")]
        [ProviderPortalPostcode(ErrorMessage = "Invalid {0} format")]
        public string Postcode { get; set; }

        public Decimal? AddressBaseId { get; set; }
        public Double? Latitude { get; set; }
        public Double? Longitude { get; set; }

        public LocationAddressViewModel()
        {

        }

        public LocationAddressViewModel(Address address)
        {
            this.AddressId = address.AddressId;
            this.AddressLine1 = address.AddressLine1;
            this.AddressLine2 = address.AddressLine2;
            this.Town = address.Town;
            this.County = address.County;
            this.Postcode = address.Postcode.ToUpper();
            this.Latitude = address.Latitude;
            this.Longitude = address.Longitude;
        }
    }
}