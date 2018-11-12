using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class AddEditVenueModel
    {
        public Boolean IsInDialog { get; set; }
        
        public Int32? VenueId { get; set; }

        [LanguageDisplay("Status")]
        public Int32? RecordStatusId { get; set; }

        [LanguageDisplay("Provider Venue Id")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Provider Venue Id")]
        [Display(Description = "Please enter the unique identifier that your organisation has for this venue")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public String ProviderOwnVenueRef { get; set; }

        [LanguageRequired]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Venue Name")]
        [LanguageDisplay("Venue Name")]
        [Display(Description = "Please enter the name of the venue where training takes place")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public String VenueName { get; set; }

       // [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Address")]
        public AddressViewModel Address { get; set; }    

        [LanguageDisplay("Email Address")]
        [DataType(DataType.EmailAddress)]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter one main email address for this venue.")]
        public String Email { get; set; }

        [LanguageDisplay("Website")]
        [ProviderPortalUrl(ErrorMessage = "Please enter a valid URL")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter a website address for this venue if available.")]
        public String Website { get; set; }

        [LanguageDisplay("Telephone")]
        [LanguageStringLength(30, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter a phone number for this venue.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Phone Number")]
        public String Telephone { get; set; }

        [LanguageDisplay("Fax")]
        [LanguageStringLength(35, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter a fax number for this venue.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Fax Number")]
        public String Fax { get; set; }

        [LanguageDisplay("Facilities")]
        [DataType(DataType.MultilineText)]
        [Display(Description = "Please enter a description of the main features or facilities that may be relevant to the holding of learning instances at that venue. Examples include: overnight accommodation and meals available; full chemical and physics laboratory facilities; 5 conference rooms holding up to 50 persons each; Video conferencing available in 3 rooms.")]
        [LanguageStringLength(2000, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Facility")]
        public String Facilities { get; set; }

        public AddEditVenueModel()
        {
            this.Address = new AddressViewModel
            {
                HideRegion = true
            };
        }

        public AddEditVenueModel(Venue venue)
        {
            this.RecordStatusId = venue.RecordStatusId;
            this.VenueId = venue.VenueId;
            this.ProviderOwnVenueRef = venue.ProviderOwnVenueRef;
            this.VenueName = venue.VenueName;
            this.Email = venue.Email;
            this.Website = venue.Website;
            this.Telephone = venue.Telephone;
            this.Fax = venue.Fax;
            this.Facilities = venue.Facilities;

            this.Address = new AddressViewModel(venue.Address)
            {
                HideRegion = true
            };
        }
    }

    public class ViewVenueModel : AddEditVenueModel
    {
        public String RecordStatusName { get; set; }

        public ViewVenueModel(Venue venue) : base(venue)
        {
            this.RecordStatusName = venue.RecordStatu.RecordStatusName;
        }
    }

    public class ListVenueModel
    {
        [LanguageDisplay("Venue Id")]
        public Int32 VenueId { get; set; }

        [LanguageDisplay("Provider Venue Id")]
        public String ProviderOwnVenueRef { get; set; }

        [LanguageDisplay("Venue Name")]                                                           
        public String VenueName { get; set; }

        public AddressViewModel Address { get; set; }

        [LanguageDisplay("Email Address")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [LanguageDisplay("Website")]
        [Url]
        public String Website { get; set; }

        [LanguageDisplay("Telephone")]
        public String Telephone { get; set; }

        [LanguageDisplay("Fax")]
        public String Fax { get; set; }

        [LanguageDisplay("Facilities")]
        [DataType(DataType.MultilineText)]
        public String Facilities { get; set; }

        [LanguageDisplay("Last Update")]
        [DataType(DataType.Date)]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime LastUpdate { get; set; }

        [LanguageDisplay("Status")]
        public String Status { get; set; }

        public ListVenueModel()
        {
            this.Address = new AddressViewModel
            {
                HideRegion = true
            };

            DateTime.SpecifyKind(this.LastUpdate, DateTimeKind.Utc);
        }

        public ListVenueModel(Venue venue) : this()
        {
            this.Status = venue.RecordStatu.RecordStatusName;
            this.VenueId = venue.VenueId;
            this.ProviderOwnVenueRef = venue.ProviderOwnVenueRef;
            this.VenueName = venue.VenueName;
            this.Address = new AddressViewModel(venue.Address)
            {
                HideRegion = true
            };
            this.Email = venue.Email;
            this.Telephone = venue.Telephone;
            this.Website = venue.Website;
            this.Fax = venue.Fax;
            this.Facilities = venue.Facilities;

            this.LastUpdate = venue.ModifiedDateTimeUtc ?? venue.CreatedDateTimeUtc;
        }
    }

    public class VenueSearchModel
    {
        public List<ListVenueModel> Venues { get; set; }

        public VenueSearchModel()
        {
            this.Venues = new List<ListVenueModel>();
        }
    }

    public class VenueJsonModel
    {
        public Int32 VenueId { get; set; }
        public String VenueName { get; set; }

        public VenueJsonModel()
        {
        }

        public VenueJsonModel(Venue venue) : this()
        {
            this.VenueId = venue.VenueId;
            this.VenueName = venue.VenueName;
        }
    }
}