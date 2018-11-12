using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Models;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using System.Web;

    public class AddEditDeliveryInformationModel
    {
        public AddEditDeliveryInformationModel(Provider provider)
        {
            ProviderId = provider.ProviderId;
            MarketingInformation = provider.MarketingInformation;
            ApprenticeshipContract = provider.ApprenticeshipContract;
            NationalApprenticeshipProvider = provider.NationalApprenticeshipProvider;
            this.PassedOverallQAChecks = provider.PassedOverallQAChecks ? "1" : "0";
            this.RoATP = provider.RoATPFFlag;
            RoATPProviderTypeName = provider.RoATPProviderType == null ? AppGlobal.Language.GetText("AddEditDeliveryInformationModel_RoATP_RoATPProviderTypeNone", "(none)") : provider.RoATPProviderType.Description;
            RoATPStartDate = provider.RoATPStartDate == null ? AppGlobal.Language.GetText("AddEditDeliveryInformationModel_RoATP_RoATPStartDateNA", "n/a") : provider.RoATPStartDate.Value.ToString(Constants.ConfigSettings.ShortDateFormat);
        }

        public AddEditDeliveryInformationModel()
        {

        }

        [LanguageDisplay("Provider Id")]
        public Int32? ProviderId { get; set; }

        public const Int32 MarketingInformationMaxLength = 900;
        [AllowHtml]
        [Display(Description = "Enter a brief introductory overview of your organisation and how it provides apprenticeships training. This must be information that employers will find useful e.g. what type of training organisation you are, how long you have been providing apprenticeship training, etc. Employers will view this information on all search results you are returned in (max 750 characters).")]
        // *************************************************************************************************
        // The following 2 strings must also be changed in ProviderController.RemoveSpellCheckHTMLFromMarketingInformation
        [LanguageDisplay("Your Generic Apprenticeship Information for Employers")]
        [LanguageStringLength(MarketingInformationMaxLength, ErrorMessage = "The maximum length of {0} is 750 characters.")]
        // *************************************************************************************************
        [LanguageRequired]
        public string MarketingInformation { get; set; }

        [Display(
            Description =
                "Submit new text for QA, to replace text currently locked."
            )]
        public bool SubmitNewTextToolTip { get; set; }

        [LanguageDisplay("Overall QA Passed")]
        [Display(Description = "Select overall QA check result")]
        public String PassedOverallQAChecks { get; set; }

        [LanguageDisplay("RoATP")]
        [Display(Description = "Has this provider applied to be on the Register of Apprenticeship Training Providers?")]
        public Boolean RoATP { get; set; }

        [LanguageDisplay("RoATP Provider Type")]
        [Display(Description = "Provider type from RoATP")]
        public String RoATPProviderTypeName { get; set; }

        [LanguageDisplay("RoATP Start Date")]
        [Display(Description = "Start date from RoATP")]
        public String RoATPStartDate { get; set; }

        [LanguageDisplay("Current contract with SFA")]
        [Display(Description = "")]
        public Boolean ApprenticeshipContract { get; set; }

        [LanguageDisplay("National")]
        [Display(Description = "Large provider with extensive range of apprenticeships, flexible delivery options, providing training to a large part of England")]
        public Boolean NationalApprenticeshipProvider { get; set; }
    }
}