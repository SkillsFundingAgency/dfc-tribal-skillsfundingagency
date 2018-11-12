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
    public class AddressViewModel
    {
        /// <summary>
        /// Gets or sets the address id.
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// Gets or sets the address line 1.
        /// </summary>
        [LanguageRequired]
        [StringLength(100, ErrorMessage = "The {0} must be less than {1} characters long.")]
        [LanguageDisplay("Address line 1")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Address Line 1")]
        [Display(Description = "Please enter a postcode and click Find Address then select your address from the drop down")]
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
        [LanguageRequired]
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

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        [LanguageDisplay("Region")]
        [Display(Description = "Please select a region.")]
        public int? RegionId { get; set; }

        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        [LanguageDisplay("Regions")]
        public IEnumerable<SelectListItem> Regions { get; set; }

        /// <summary>
        /// Gets or sets the region for display purposes.
        /// </summary>
        [LanguageDisplay("Region")]
        public string Region { get; set; }

        /// <summary>
        /// This flag controls whether the region is shown.
        /// </summary>
        public bool HideRegion { get; set; }

        public AddressViewModel()
        {
            this.RegionId = 0;
        }

        public Decimal? AddressBaseId { get; set; }
        public Double? Latitude { get; set; }
        public Double? Longitude { get; set; }

        public AddressViewModel(Address address)
        {
            this.AddressId = address.AddressId;
            this.AddressLine1 = address.AddressLine1;
            this.AddressLine2 = address.AddressLine2;
            this.Town = address.Town;
            this.County = address.County;
            this.Postcode = address.Postcode.ToUpper();
            this.RegionId = address.ProviderRegionId ?? 0;
            this.Region = address.ProviderRegionId == null ? String.Empty : address.ProviderRegion.RegionName;
            this.Latitude = address.Latitude;
            this.Longitude = address.Longitude;
        }
    }

    public class AddressBaseModel
    {
        public Decimal UPRN { get; set; }

        public String OrganisationName { get; set; }

        public String PostOfficeBox { get; set; }

        public String AddressLine1 { get; set; }

        public String AddressLine2 { get; set; }

        public String Town { get; set; }

        public String County { get; set; }

        public String Postcode { get; set; }

        public String Key
        {
            get { return String.Format("{0}~{1}~{2}~{3}~{4}~{5}", AddressLine1, AddressLine2, Town, County, Postcode, UPRN); }
        }

        public String Value
        {
            get
            {
                String strReturn = "";
                if (!String.IsNullOrEmpty(OrganisationName))
                {
                    strReturn += OrganisationName + ", ";
                }
                if (!String.IsNullOrEmpty(AddressLine1))
                {
                    strReturn += AddressLine1 + ", ";
                }
                if (!String.IsNullOrEmpty(AddressLine2))
                {
                    strReturn += AddressLine2 + ", ";
                }
                if (!String.IsNullOrEmpty(Town))
                {
                    strReturn += Town + ", ";
                }
                if (!String.IsNullOrEmpty(County))
                {
                    strReturn += County + ", ";
                }
                if (!String.IsNullOrEmpty(Postcode))
                {
                    strReturn += Postcode + ", ";
                }
                if (strReturn.Length > 0)
                {
                    strReturn = strReturn.Substring(0, strReturn.Length - 2);
                }

                return strReturn;
            }
        }

        public AddressBaseModel()
        {
        }

        public AddressBaseModel(AddressBase addressBase)
        {
            this.UPRN = addressBase.UPRN;
            this.OrganisationName = addressBase.OrganisationName;
            this.PostOfficeBox = "";
            this.AddressLine1 = "";
            this.AddressLine2 = "";

            if (!String.IsNullOrEmpty(addressBase.POBoxNumber))
            {
                this.PostOfficeBox = "PO Box " + addressBase.POBoxNumber;
                this.AddressLine1 = AppendWithSeparator(this.AddressLine1, this.PostOfficeBox);
            }

            if (!String.IsNullOrEmpty(addressBase.SubBuildingName))
            {
                this.AddressLine1 = AppendWithSeparator(this.AddressLine1, addressBase.SubBuildingName);
            }

            if (!String.IsNullOrEmpty(addressBase.BuildingName))
            {
                this.AddressLine1 = AppendWithSeparator(this.AddressLine1, addressBase.BuildingName);
                if (addressBase.BuildingNumber != null)
                {
                    // Adding a ~ to the end of the building number tells the AppendWithSepartor that the previous
                    // field added was a building number and no to put a comma next
                    this.AddressLine2 = AppendWithSeparator(this.AddressLine2, addressBase.BuildingNumber + "~");
                }
                if (!String.IsNullOrEmpty(addressBase.DependentThoroughfareName))
                {
                    this.AddressLine2 = AppendWithSeparator(this.AddressLine2, addressBase.DependentThoroughfareName);
                }
                if (!String.IsNullOrEmpty(addressBase.ThoroughfareName))
                {
                    this.AddressLine2 = AppendWithSeparator(this.AddressLine2, addressBase.ThoroughfareName);
                }
            }
            else
            {
                if (addressBase.BuildingNumber != null)
                {
                    // Adding a ~ to the end of the building number tells the AppendWithSepartor that the previous
                    // field added was a building number and no to put a comma next
                    this.AddressLine1 = AppendWithSeparator(this.AddressLine1, addressBase.BuildingNumber + "~");
                }

                if (!String.IsNullOrEmpty(addressBase.DependentThoroughfareName))
                {
                    this.AddressLine1 = AppendWithSeparator(this.AddressLine1, addressBase.DependentThoroughfareName);
                    if (!String.IsNullOrEmpty(addressBase.ThoroughfareName))
                    {
                        this.AddressLine2 = AppendWithSeparator(this.AddressLine2, addressBase.ThoroughfareName);
                    }
                }
                else if (!String.IsNullOrEmpty(addressBase.ThoroughfareName))
                {
                    this.AddressLine1 = AppendWithSeparator(this.AddressLine1, addressBase.ThoroughfareName);
                }
            }

            if (!String.IsNullOrEmpty(addressBase.DependentLocality))
            {
                this.AddressLine2 = AppendWithSeparator(this.AddressLine2, addressBase.DependentLocality);
            }

            // JUst in case we are still left with a "~", get rid of them.
            this.AddressLine1 = this.AddressLine1.Replace("~", "");
            this.AddressLine2 = this.AddressLine2.Replace("~", "");

            this.Town = addressBase.Town;
            this.County = "";
            this.Postcode = addressBase.Postcode;
        }

        private static String AppendWithSeparator(String strField, String strData)
        {
            if (strField.Length != 0)
            {
                if (strField.EndsWith("~"))
                {
                    // Last field added was a building number field so remove the "~" and just add a space (no comma)
                    strField = strField.Replace("~", "") + " ";
                }
                else
                {
                    strField += ", ";
                }
            }
            strField += strData;

            return strField;
        }
    }

    public class UploadAddressBaseModel
    {
        [LanguageDisplay("Last Uploaded By")]
        public String LastUploadedBy { get; set; }

        [LanguageDisplay("Last Updated Date/Time")]
        [DateDisplayFormat(Format = DateFormat.ShortDateTime)]
        public DateTime? LastUploadDateTimeUtc { get; set; }

        [LanguageDisplay("Last Updated Date/Time")]
        [DataType(DataType.DateTime)]
        [DateDisplayFormat(Format = DateFormat.ShortDateTime)]
        public DateTime? LastUploadDateTimeLocalTime
        {
            get
            {
                if (LastUploadDateTimeUtc.HasValue)
                {
                    return LastUploadDateTimeUtc.Value.ToLocalTime();
                }

                return null;
            }
        }
        [LanguageDisplay("Last Uploaded File Name")]
        public String LastUploadFileName { get; set; }

        [Required(ErrorMessage = "Please specify a ZIP file.")]
        public HttpPostedFileBase File { get; set; }
    }
}