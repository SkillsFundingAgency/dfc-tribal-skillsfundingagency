using System;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using System.Linq;

    using Entities;
    using Permission = Tribal.SkillsFundingAgency.ProviderPortal.Permission;

    public static class DeliveryInformationModelExtensions
    {
        /// <summary>
        /// Convert an <see cref="AddEditDeliveryInformationModel"/> to an <see cref="Provider"/>.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <returns>
        /// The <see cref="Provider"/>.
        /// </returns>        
        public static Provider ToEntity(this AddEditDeliveryInformationModel model, ProviderPortalEntities db)
        {
            Provider provider;

            if (model.ProviderId == null)
            {
                return null;
            }
            else
            {
                provider = db.Providers.Find(model.ProviderId);
                if (provider == null)
                {
                    return null;
                }
            }

            var canEditSpecialFields = Permission.HasPermission(false, true,
               Permission.PermissionName.CanEditProviderSpecialFields);
            if (canEditSpecialFields && model.ProviderId != null)
            {
                provider.RoATPFFlag = model.RoATP;
            }

            provider.ApprenticeshipContract = model.ApprenticeshipContract;
            provider.NationalApprenticeshipProvider = model.NationalApprenticeshipProvider;

            if (!provider.PassedOverallQAChecks || Permission.HasPermission(false, false, Permission.PermissionName.CanQAProviders))
            {
                provider.MarketingInformation = Markdown.Sanitize(model.MarketingInformation);
            }

            return provider;
        }
    }
}
