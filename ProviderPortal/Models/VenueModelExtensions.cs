using System;
using System.Collections.Generic;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class VenueModelExtensions
    {
        /// <summary>
        /// Convert an <see cref="AddEditVenueModel"/> to an <see cref="Venue"/>.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <returns>
        /// The <see cref="Venue"/>.
        /// </returns>        
        public static Venue ToEntity(this AddEditVenueModel model, ProviderPortalEntities db)
        {
            Venue venue;

            if (model.VenueId == null)
            {
                venue = new Venue
                {
                    CreatedByUserId = Permission.GetCurrentUserId(),
                    CreatedDateTimeUtc = DateTime.UtcNow
                };
            }
            else
            {
                venue = db.Venues.Find(model.VenueId);
                if (venue == null)
                {
                    return null;
                }
            }

            venue.ProviderOwnVenueRef = model.ProviderOwnVenueRef;
            venue.VenueName = model.VenueName;
            venue.Email = model.Email;
            venue.Website = UrlHelper.GetFullUrl(model.Website);
            venue.Telephone = model.Telephone;
            venue.Fax = model.Fax;
            venue.Facilities = model.Facilities;

            return venue;
        }

        public static List<String> GetWarningMessages(this AddEditVenueModel model)
        {
            List<String> messages = new List<String>();

            if (!String.IsNullOrWhiteSpace(model.Website) && !UrlHelper.UrlIsReachable(model.Website))
            {
                messages.Add(String.Format(AppGlobal.Language.GetText("AddEditVenueModel_Edit_UrlNotReachable", "The web address for {0} returns a response that suggests this page may not exist. Please check that the web address entered is correct."), AppGlobal.Language.GetText("AddEditVenueModel_DisplayName_Website", "Website")));
            }

            return messages;
        }
    }
}