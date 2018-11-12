using System;
using System.ComponentModel.DataAnnotations;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class ListPublicAPIUserModel
    {
        [LanguageDisplay("API Key")]
        public String APIKey { get; set; }

        [LanguageDisplay("Company Name")]
        public String CompanyName { get; set; }

        [LanguageDisplay("Contact First Name")]                                                           
        public String ContactFirstName { get; set; }

        [LanguageDisplay("Contact Last Name")]
        public String ContactLastName { get; set; }

        [LanguageDisplay("Email Address")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [LanguageDisplay("Telephone")]
        public String Telephone { get; set; }

        [LanguageDisplay("Last Update")]
        [DataType(DataType.Date)]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime LastUpdate { get; set; }

        [LanguageDisplay("Status")]
        public String Status { get; set; }

        public ListPublicAPIUserModel()
        {
            DateTime.SpecifyKind(this.LastUpdate, DateTimeKind.Utc);
        }

        public ListPublicAPIUserModel(PublicAPIUser publicAPIUser)
            : this()
        {
            this.Status = publicAPIUser.RecordStatu.RecordStatusName;
            this.APIKey = publicAPIUser.PublicAPIUserId.ToString();
            this.CompanyName = publicAPIUser.CompanyName;
            this.ContactFirstName = publicAPIUser.ContactFirstName;
            this.ContactLastName = publicAPIUser.ContactLastName;
            this.Email = publicAPIUser.Email;
            this.Telephone = publicAPIUser.Telephone;

            this.LastUpdate = publicAPIUser.ModifiedDateTimeUtc ?? publicAPIUser.CreatedDateTimeUtc;
        }
    }

    public class AddEditPublicAPIUserModel
    {
        [LanguageDisplay("API Key")]
        public String APIKey { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Company Name")]
        public String CompanyName { get; set; }

        [LanguageDisplay("Contact First Name")]
        public String ContactFirstName { get; set; }

        [LanguageDisplay("Contact Last Name")]
        public String ContactLastName { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Email Address")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Telephone")]
        public String Telephone { get; set; }

        [LanguageDisplay("Status")]
        public Int32? RecordStatusId { get; set; }

        public AddEditPublicAPIUserModel()
        {
        }

        public AddEditPublicAPIUserModel(PublicAPIUser pau)
        {
            this.APIKey = pau.PublicAPIUserId.ToString();
            this.CompanyName = pau.CompanyName;
            this.Email = pau.Email;
            this.Telephone = pau.Telephone;
            this.ContactFirstName = pau.ContactFirstName;
            this.ContactLastName = pau.ContactLastName;
            this.RecordStatusId = pau.RecordStatusId;
        }
    }
}