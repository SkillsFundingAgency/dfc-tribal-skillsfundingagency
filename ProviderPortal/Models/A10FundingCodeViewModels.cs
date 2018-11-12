using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class A10FundingCodeViewModelItem
    {
        [LanguageDisplay("A10 Funding Code")]
        [LanguageRequired]
        public int A10FundingCodeId { get; set; }
        
        [LanguageDisplay("Description")]
        [LanguageRequired]
        public string A10FundingCodeName { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Status")]
        public int RecordStatusId { get; set; }

        [LanguageDisplay("Status")]
        public string RecordStatusName { get; set; }

        public bool IsNew { get; set; }
    }

    public class A10FundingCodeViewModel
    {
        public List<A10FundingCodeViewModelItem> Items { get; set; }
    }
}