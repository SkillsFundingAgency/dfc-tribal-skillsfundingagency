using System.Linq;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class ImportBatchesViewModelExtensions
    {
        public static ImportBatchesViewModel Populate(this ImportBatchesViewModel model, ProviderPortalEntities db)
        {
            model.Items = db.ImportBatches
                .Select(x => new ImportBatchesViewModelItem
                {
                    ImportBatchId = x.ImportBatchId,
                    ImportBatchName = x.ImportBatchName,
                    Current = x.Current
                })
                .OrderByDescending(x => x.Current)
                .ThenByDescending(x => x.ImportBatchName)
                .ToList();
            return model;
        }

        public static ImportBatchesViewModelItem Populate(this ImportBatchesViewModelItem model, ProviderPortalEntities db)
        {
            return new ImportBatchesViewModelItem
            {
                Current = false
            };
        }

        public static ImportBatchesViewModelItem Populate(this ImportBatchesViewModelItem model, ProviderPortalEntities db, int id)
        {
            var item = db.ImportBatches.FirstOrDefault(x => x.ImportBatchId == id);
            if (item == null) return null;
            model = new ImportBatchesViewModelItem
            {
                ImportBatchId = item.ImportBatchId,
                ImportBatchName = item.ImportBatchName,
                Current = item.Current
            };
            return model;
        }

        public static ImportBatch ToEntity(this ImportBatchesViewModelItem model)
        {
            var item = new ImportBatch
            {
                ImportBatchId = model.ImportBatchId,
                ImportBatchName = model.ImportBatchName,
                Current = model.Current
            };
            return item;
        }

        public static void ValidateEntry(this ImportBatchesViewModelItem model, ProviderPortalEntities db, ModelStateDictionary modelState)
        {
            if (db.ImportBatches.Any(x => x.ImportBatchName == model.ImportBatchName && x.ImportBatchId != model.ImportBatchId))
            {
                modelState.AddModelError("ImportBatchName", AppGlobal.Language.GetText("ImportBatches_Create_NameInUse", "The batch name must be unique."));
            }
        }
    }
}