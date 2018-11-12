using System;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class PublicAPIUserModelExtensions
    {
        /// <summary>
        /// Convert an <see cref="AddEditPublicAPIUserModel"/> to an <see cref="PublicAPIUser"/>.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <returns>
        /// The <see cref="PublicAPIUser"/>.
        /// </returns>        
        public static PublicAPIUser ToEntity(this AddEditPublicAPIUserModel model, ProviderPortalEntities db)
        {
            PublicAPIUser pau;

            if (model.APIKey == null)
            {
                pau = new PublicAPIUser
                {
                    RecordStatusId = (Int32)Constants.RecordStatus.Live,
                    CreatedByUserId = Permission.GetCurrentUserId(),
                    CreatedDateTimeUtc = DateTime.UtcNow
                };
            }
            else
            {
                pau = db.PublicAPIUsers.Find(Guid.Parse(model.APIKey));
                if (pau == null)
                {
                    pau = new PublicAPIUser
                    {
                        RecordStatusId = (Int32) Constants.RecordStatus.Live,
                        CreatedByUserId = Permission.GetCurrentUserId(),
                        CreatedDateTimeUtc = DateTime.UtcNow
                    };
                }
                else
                {
                    if (model.RecordStatusId.HasValue)
                    {
                        pau.RecordStatusId = model.RecordStatusId.Value;
                    }
                }
            }

            pau.CompanyName = model.CompanyName;
            pau.Email = model.Email;
            pau.Telephone = model.Telephone;
            pau.ContactFirstName = model.ContactFirstName;
            pau.ContactLastName = model.ContactLastName;

            return pau;
        }

    }
}