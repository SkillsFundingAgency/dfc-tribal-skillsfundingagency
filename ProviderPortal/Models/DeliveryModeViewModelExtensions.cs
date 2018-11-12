using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.Tracing;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class DeliveryModeViewModelExtensions
    {
        public static DeliveryModeViewModel Populate(this DeliveryModeViewModel model, ProviderPortalEntities db)
        {
            model.Items = db.DeliveryModes
                .Select(x => new DeliveryModeViewModelItem
                {
                    DeliveryModeId = x.DeliveryModeId,
                    DeliveryModeName = x.DeliveryModeName,
                    DeliveryModeDescription = x.DeliveryModeDescription,
                    BulkUploadRef = x.BulkUploadRef,
                    DASRef = x.DASRef,
                    MustHaveFullLocation = x.MustHaveFullLocation,
                    RecordStatusId = x.RecordStatusId,
                    RecordStatusName = x.RecordStatu.RecordStatusName
                })
                .OrderBy(x => x.DeliveryModeId)
                .ToList();
            return model;
        }

        public static DeliveryModeViewModelItem Populate(this DeliveryModeViewModelItem model,
            ProviderPortalEntities db)
        {
            return new DeliveryModeViewModelItem
            {
                DeliveryModeId = 0,
                IsNew = true,
                BulkUploadRef = String.Empty,
                RecordStatusId = (int)Constants.RecordStatus.Live,
            };
        }

        public static DeliveryModeViewModelItem Populate(this DeliveryModeViewModelItem model,
            ProviderPortalEntities db, int id)
        {
            var item = db.DeliveryModes
                .FirstOrDefault(x => x.DeliveryModeId == id);
            if (item == null) return null;
            model = new DeliveryModeViewModelItem
            {
                DeliveryModeId = item.DeliveryModeId,
                DeliveryModeName = item.DeliveryModeName,
                DeliveryModeDescription = item.DeliveryModeDescription,
                BulkUploadRef = item.BulkUploadRef,
                DASRef = item.DASRef,
                MustHaveFullLocation = item.MustHaveFullLocation,
                RecordStatusId = item.RecordStatusId,
                RecordStatusName = item.RecordStatu.RecordStatusName,
                IsNew = false
            };
            return model;
        }

        public static DeliveryMode ToEntity(this DeliveryModeViewModelItem model)
        {
            var item = new DeliveryMode
            {
                DeliveryModeId = model.DeliveryModeId,
                DeliveryModeName = model.DeliveryModeName,
                DeliveryModeDescription = model.DeliveryModeDescription,
                BulkUploadRef = model.BulkUploadRef,
                DASRef = model.DASRef,
                MustHaveFullLocation = model.MustHaveFullLocation,
                RecordStatusId = model.RecordStatusId
            };
            return item;
        }

        public static void ValidateNewEntry(this DeliveryModeViewModelItem model, ProviderPortalEntities db, ModelStateDictionary modelState)
        {
            if (db.DeliveryModes.Any(x => x.DeliveryModeId == model.DeliveryModeId))
            {
                modelState.AddModelError("DeliveryModeId", AppGlobal.Language.GetText("DeliveryMode_Create_CodeInUse", "The Delivery Mode id must be unique."));
            }
            if (db.DeliveryModes.Any(x => x.DeliveryModeName == model.DeliveryModeName))
            {
                modelState.AddModelError("DeliveryModeName", AppGlobal.Language.GetText("DeliveryMode_Create_DeliveryModeNameInUse", "The Delivery Mode Name field must be unique."));
            }
            if (db.DeliveryModes.Any(x => x.BulkUploadRef == model.BulkUploadRef))
            {
                modelState.AddModelError("BulkUploadRef", AppGlobal.Language.GetText("DeliveryMode_Create_BulkUploadRefInUse", "The Bulk Upload Ref. field must be unique."));
            }
            if (db.DeliveryModes.Any(x => x.DASRef == model.DASRef))
            {
                modelState.AddModelError("DASRef", AppGlobal.Language.GetText("DeliveryMode_Create_DASRefInUse", "The DAS Ref. field must be unique."));
            }       
        }

        public static void ValidateEditedEntry(this DeliveryModeViewModelItem model, ProviderPortalEntities db, ModelStateDictionary modelState)
        {
            if (db.DeliveryModes.Any(x => x.DeliveryModeId != model.DeliveryModeId && x.DeliveryModeName == model.DeliveryModeName))
            {
                modelState.AddModelError("DeliveryModeName", AppGlobal.Language.GetText("DeliveryMode_Create_DeliveryModeNameInUse", "The Delivery Mode Name field must be unique."));
            }
            if (db.DeliveryModes.Any(x => x.DeliveryModeId != model.DeliveryModeId && x.BulkUploadRef == model.BulkUploadRef))
            {
                modelState.AddModelError("BulkUploadRef", AppGlobal.Language.GetText("DeliveryMode_Create_BulkUploadRefInUse", "The Bulk Upload Ref. field must be unique."));
            }
            if (db.DeliveryModes.Any(x => x.DeliveryModeId != model.DeliveryModeId && x.DASRef == model.DASRef))
            {
                modelState.AddModelError("DASRef", AppGlobal.Language.GetText("DeliveryMode_Create_DASRefInUse", "The DAS Ref. field must be unique."));
            }
        }
    }
}