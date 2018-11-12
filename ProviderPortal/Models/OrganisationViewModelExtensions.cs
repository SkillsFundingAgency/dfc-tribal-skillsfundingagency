using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using Entities;
    using Permission = Tribal.SkillsFundingAgency.ProviderPortal.Permission;

    public static class OrganisationViewModelExtensions
    {
        /// <summary>
        /// Convert an <see cref="AddEditOrganisationModel"/> to an <see cref="Provider"/>.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <returns>
        /// The <see cref="Organisation"/>.
        /// </returns>        
        public static Organisation ToEntity(this AddEditOrganisationModel model, ProviderPortalEntities db)
        {
            Organisation organisation;

            if (model.OrganisationId == null)
            {
                organisation = new Organisation
                {
                    UKPRN = model.UKPRN.HasValue ? model.UKPRN.Value : 0,
                    CreatedByUserId = Permission.GetCurrentUserId(), 
                    CreatedDateTimeUtc = DateTime.UtcNow
                };
            }
            else
            {
                organisation = db.Organisations.Find(model.OrganisationId);
                if (organisation == null)
                {
                    return null;
                }
            }

            organisation.UPIN = model.UPIN;
            if (Permission.HasPermission(false, true, Permission.PermissionName.CanEditOrganisationSpecialFields))
            {
                organisation.IsContractingBody = model.IsContractingBody;
            }
            organisation.OrganisationTypeId = model.OrganisationTypeId;
            organisation.OrganisationName = model.OrganisationName;
            organisation.OrganisationAlias = model.OrganisationAlias;
            organisation.Loans24Plus = model.Loans24Plus;
            organisation.Email = model.Email;
            organisation.Website = UrlHelper.GetFullUrl(model.Website);
            organisation.Phone = model.Telephone;
            organisation.Fax = model.Fax;
            organisation.BulkUploadPending = model.BulkUploadPending;

            return organisation;
        }
    }

    public static class AddEditOrganisationModelExtensions
    {
        public static AddEditOrganisationModel Populate(this AddEditOrganisationModel model, ProviderPortalEntities db)
        {
            model.Address = model.Address ?? new AddressViewModel();
            model.Address.Populate(db);
            model.Address.HideRegion = true;
            return model;
        }
    }

    public static class DeleteOrganisationViewModelExtensions
    {
        /// <summary>
        /// Populates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="db">The database.</param>
        /// <returns></returns>
        public static DeleteOrganisationViewModel Populate(this DeleteOrganisationViewModel model, ProviderPortalEntities db)
        {
            model.HasActiveMembers = db.OrganisationProviders
                .Any(x => x.OrganisationId == model.OrganisationId && x.IsAccepted && !x.IsRejected);
            return model;
        }
    }
}
