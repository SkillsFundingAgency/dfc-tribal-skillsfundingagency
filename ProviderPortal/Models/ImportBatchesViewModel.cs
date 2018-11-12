using System.Collections.Generic;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class ImportBatchesViewModelItem
    {
        [LanguageDisplay("Import Batch")]
        [LanguageRequired]
        public int ImportBatchId { get; set; }
        
        [LanguageDisplay("Batch Name")]
        [LanguageRequired]
        [LanguageStringLength(50, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        public string ImportBatchName { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Current")]
        public bool Current { get; set; }

    }

    public class ImportBatchesViewModel
    {
        public List<ImportBatchesViewModelItem> Items { get; set; }
    }
}