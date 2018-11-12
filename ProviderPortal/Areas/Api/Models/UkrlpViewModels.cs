using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Models
{
    public class UKRLPDataModel
    {
        public UKRLPDataModel()
        {
            CompanyRegistrationNumber = "";
            CharityRegistrationNumber = "";
        }

        public UKRLPDataModel(Ukrlp ukrlp)
        {
            UKRLP = ukrlp.Ukprn.ToString();
            LegalName = ukrlp.LegalName;
            TradingName = ukrlp.TradingName;
            LegalTelephone = ukrlp.LegalPhoneNumber;
            LegalFax = ukrlp.LegalFaxNumber;
            ContactName = ukrlp.PrimaryContactName;
            ContactTelephone = ukrlp.PrimaryUKPhone;
            ContactFax = ukrlp.PrimaryUKFax;

            if (ukrlp.LegalAddress != null)
            {
                LegalFullAddress = ukrlp.LegalAddress.GetMultipleLineHTMLAddress();
            }

            if (ukrlp.PrimaryAddress != null)
            {
                ContactFullAddress = ukrlp.PrimaryAddress.GetMultipleLineHTMLAddress();
            }

            if (ukrlp.RecordStatu != null)
            {
                Status = ukrlp.RecordStatu.RecordStatusName;
            }

            CompanyRegistrationNumber = ukrlp.CompanyRegistration ?? "";
            CharityRegistrationNumber = ukrlp.CharityRegistration ?? "";
        }

        public string UKRLP { get; set; }
        public bool InUse { get; set; }
        public string LegalName { get; set; }
        public string TradingName { get; set; }
        public string LegalFullAddress { get; set; }
        public string LegalAddress1 { get; set; }
        public string LegalAddress2 { get; set; }
        public string LegalTown { get; set; }
        public string LegalCounty { get; set; }
        public string LegalPostcode { get; set; }
        public string LegalTelephone { get; set; }
        public string LegalFax { get; set; }
        public string ContactName { get; set; }
        public string ContactFullAddress { get; set; }
        public string ContactAddress1 { get; set; }
        public string ContactAddress2 { get; set; }
        public string ContactTown { get; set; }
        public string ContactCounty { get; set; }
        public string ContactPostcode { get; set; }
        public string ContactTelephone { get; set; }
        public string ContactFax { get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public string CharityRegistrationNumber { get; set; }
        public string Status { get; set; }
    }
}