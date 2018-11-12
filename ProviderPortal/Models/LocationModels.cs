using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class AddEditLocationModel
    {
        public AddEditLocationModel()
        {
            Address = new LocationAddressViewModel();
        }

        public AddEditLocationModel(Location location)
        {
            ProviderId = location.ProviderId;
            RecordStatusId = location.RecordStatusId;
            LocationId = location.LocationId;
            ProviderOwnLocationRef = location.ProviderOwnLocationRef;
            LocationName = location.LocationName;
            Email = location.Email;
            Website = location.Website;
            Telephone = location.Telephone;

            Address = new LocationAddressViewModel(location.Address);
        }

        public Boolean IsInDialog { get; set; }

        public Int32 ProviderId { get; set; }
        public Int32? LocationId { get; set; }

        [LanguageDisplay("Status")]
        public Int32? RecordStatusId { get; set; }

        [LanguageDisplay("Provider Location Id")]
        [ProviderPortalTextField(ErrorMessage = "Please enter a valid Provider Location Id")]
        [Display(Description = "Please enter the unique identifier that your organisation has for this location")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public String ProviderOwnLocationRef { get; set; }

        [LanguageRequired]
        [ProviderPortalTextField(ErrorMessage = "Please enter a valid Location Name")]
        [LanguageDisplay("Location Name")]
        [Display(Description = "Enter a unique name of the location where training takes place.")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public String LocationName { get; set; }

        // [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Address")]
        public LocationAddressViewModel Address { get; set; }

        [LanguageDisplay("Email")]
        [DataType(DataType.EmailAddress)]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Enter one main email address for enquiries about this location.")]
        [LanguageEmailAddress]
        public String Email { get; set; }

        [LanguageDisplay("Website")]
        //[DataType(DataType.Url)]
        [ProviderPortalUrl(ErrorMessage = "Please include the full URL including http://www")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Enter a link for general enquiries about this location if available.")]
        public String Website { get; set; }

        [LanguageDisplay("Telephone")]
        [LanguageStringLength(30, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Enter a phone number for general enquiries about this location.")]
        [ProviderPortalTextField(ErrorMessage = "Please enter a valid Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public String Telephone { get; set; }

        //[LanguageRequired]
        //[LanguageDisplay("Catchment Radius")]
        //[Display(Description = "Please enter a catchment radius number for this location.")]
        //// e.g. 874 miles, i.e. a provider in Lands End who accepts applicants from John o' Groats
        //[ProviderPortalRange(0, 874, ErrorMessage = "Please enter a number of miles between {1} and {2}")]
        //public Int32? Radius { get; set; }
    }

    public class ViewLocationModel : AddEditLocationModel
    {
        public ViewLocationModel(Location location) : base(location)
        {
            RecordStatusName = location.RecordStatu.RecordStatusName;
        }

        public String RecordStatusName { get; set; }
    }

    public class ListLocationModel
    {
        public ListLocationModel()
        {
            Address = new LocationAddressViewModel();


            DateTime.SpecifyKind(LastUpdate, DateTimeKind.Utc);
        }

        public ListLocationModel(Location location) : this()
        {
            Status = location.RecordStatu.RecordStatusName;
            LocationId = location.LocationId;
            ProviderOwnLocationRef = location.ProviderOwnLocationRef;
            LocationName = location.LocationName;
            Address = new LocationAddressViewModel(location.Address);

            Email = location.Email;
            Telephone = location.Telephone;
            Website = location.Website;

            LastUpdate = location.ModifiedDateTimeUtc ?? location.CreatedDateTimeUtc;
        }

        [LanguageDisplay("Location Id")]
        public Int32 LocationId { get; set; }

        [LanguageDisplay("Location Id")]
        public String ProviderOwnLocationRef { get; set; }

        [LanguageDisplay("Location Name")]
        public String LocationName { get; set; }

        public LocationAddressViewModel Address { get; set; }

        [LanguageDisplay("Email Address")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [LanguageDisplay("Website")]
        [ProviderPortalUrl(ErrorMessage = "Please enter a valid URL")]
        public String Website { get; set; }

        [LanguageDisplay("Telephone")]
        public String Telephone { get; set; }

        [LanguageDisplay("Catchment Radius")]
        public Int32? Radius { get; set; }

        [LanguageDisplay("Last Update")]
        [DataType(DataType.Date)]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime LastUpdate { get; set; }

        [LanguageDisplay("Status")]
        public String Status { get; set; }
    }

    public class LocationSearchModel
    {
        public LocationSearchModel()
        {
            Locations = new List<ListLocationModel>();
        }

        public List<ListLocationModel> Locations { get; set; }
    }

    public class LocationJsonModel
    {
        public LocationJsonModel()
        {
        }

        public LocationJsonModel(Location location) : this()
        {
            ProviderId = location.ProviderId;
            LocationId = location.LocationId;
            LocationName = location.LocationName;
        }

        public Int32 ProviderId { get; set; }
        public Int32 LocationId { get; set; }
        public String LocationName { get; set; }
    }
}