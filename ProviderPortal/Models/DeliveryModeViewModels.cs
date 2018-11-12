using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class DeliveryModeViewModelItem
    {
        [LanguageDisplay("Delivery Mode Id")]
        [LanguageRequired]
        public int DeliveryModeId { get; set; }

        [LanguageDisplay("Name")]
        [LanguageRequired]
        public string DeliveryModeName { get; set; }

        [LanguageDisplay("Description")]
        [LanguageRequired]
        public string DeliveryModeDescription { get; set; }

        [LanguageDisplay("Bulk Upload Ref.")]
        [LanguageRequired]
        public string BulkUploadRef { get; set; }

        [LanguageDisplay("DAS Ref.")]
        [LanguageRequired]
        public string DASRef { get; set; }

        [LanguageDisplay("Must Have Full Location")]
        [LanguageRequired]
        public Boolean MustHaveFullLocation { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Status")]
        public int RecordStatusId { get; set; }

        [LanguageDisplay("Status")]
        public string RecordStatusName { get; set; }

        public bool IsNew { get; set; }
    }

    public class DeliveryModeViewModel
    {
        public List<DeliveryModeViewModelItem> Items { get; set; }
    }
}