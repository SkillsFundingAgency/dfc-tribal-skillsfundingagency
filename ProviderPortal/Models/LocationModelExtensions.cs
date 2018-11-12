using System;
using System.Collections.Generic;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class LocationModelExtensions
    {
        /// <summary>
        /// Convert an <see cref="AddEditLocationModel"/> to an <see cref="Location"/>.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <returns>
        /// The <see cref="Location"/>.
        /// </returns>        
        public static Location ToEntity(this AddEditLocationModel model, ProviderPortalEntities db)
        {
            Location location;

            if (model.LocationId == null)
            {
                location = new Location
                {
                    CreatedByUserId = Permission.GetCurrentUserId(),
                    CreatedDateTimeUtc = DateTime.UtcNow
                };
            }
            else
            {
                location = db.Locations.Find(model.LocationId);
                if (location == null)
                {
                    return null;
                }
            }

            location.ProviderOwnLocationRef = model.ProviderOwnLocationRef;
            location.LocationName = model.LocationName;
            location.Email = model.Email;
            location.Website = UrlHelper.GetFullUrl(model.Website);
            location.Telephone = model.Telephone;

            return location;
        }

        public static List<String> GetWarningMessages(this AddEditLocationModel model)
        {
            List<String> messages = new List<String>();

            if (!String.IsNullOrWhiteSpace(model.Website) && !UrlHelper.UrlIsReachable(model.Website))
            {
                messages.Add(String.Format(AppGlobal.Language.GetText("AddEditLocationModel_Edit_UrlNotReachable", "The web address for {0} returns a response that suggests this page may not exist. Please check that the web address entered is correct."), AppGlobal.Language.GetText("AddEditLocationModel_DisplayName_Website", "Website")));
            }

            return messages;
        }
    }
}