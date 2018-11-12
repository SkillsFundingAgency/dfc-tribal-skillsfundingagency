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
    public static class A10FundingCodeViewModelExtensions
    {
        public static A10FundingCodeViewModel Populate(this A10FundingCodeViewModel model, ProviderPortalEntities db)
        {
            model.Items = db.A10FundingCode
                .Select(x => new A10FundingCodeViewModelItem
                {
                    A10FundingCodeId = x.A10FundingCodeId,
                    A10FundingCodeName = x.A10FundingCodeName,
                    RecordStatusId = x.RecordStatusId,
                    RecordStatusName = x.RecordStatu.RecordStatusName
                })
                .OrderBy(x => x.A10FundingCodeId)
                .ToList();
            return model;
        }

        public static A10FundingCodeViewModelItem Populate(this A10FundingCodeViewModelItem model,
            ProviderPortalEntities db)
        {
            return new A10FundingCodeViewModelItem
            {
                A10FundingCodeId = 0,
                IsNew = true,
                RecordStatusId = (int) Constants.RecordStatus.Live,
            };
        }

        public static A10FundingCodeViewModelItem Populate(this A10FundingCodeViewModelItem model,
            ProviderPortalEntities db, int id)
        {
            var item = db.A10FundingCode
                .FirstOrDefault(x => x.A10FundingCodeId == id);
            if (item == null) return null;
            model = new A10FundingCodeViewModelItem
            {
                A10FundingCodeId = item.A10FundingCodeId,
                A10FundingCodeName = item.A10FundingCodeName,
                RecordStatusId = item.RecordStatusId,
                RecordStatusName = item.RecordStatu.RecordStatusName,
                IsNew = false
            };
            return model;
        }

        public static A10FundingCode ToEntity(this A10FundingCodeViewModelItem model)
        {
            var item = new A10FundingCode
            {
                A10FundingCodeId = model.A10FundingCodeId,
                A10FundingCodeName = model.A10FundingCodeName,
                RecordStatusId = model.RecordStatusId
            };
            return item;
        }

        public static void ValidateNewEntry(this A10FundingCodeViewModelItem model, ProviderPortalEntities db, ModelStateDictionary modelState)
        {
            if (db.A10FundingCode.Any(x => x.A10FundingCodeId == model.A10FundingCodeId))
            {
                modelState.AddModelError("A10FundingCodeId", AppGlobal.Language.GetText("A10FundingCode_Create_CodeInUse", "The A10 Funding Code field must be unique."));
            }
        }
    }
}