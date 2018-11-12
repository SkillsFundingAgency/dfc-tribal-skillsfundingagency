using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class AdminViewModelExtensions
    {
        public static AdminDashboardViewModel Populate(this AdminDashboardViewModel model, ProviderPortalEntities db)
        {
            model.UserProviders = new List<SelectListItem>();
            model.UserOrganisations = new List<SelectListItem>();

            var userId = Permission.GetCurrentUserId();

            model.UserProviders = db.Providers.Where(
                x => x.RelationshipManagerUserId == userId || x.InformationOfficerUserId == userId)
                .OrderBy(x => x.ProviderName)
                .Select(x => new SelectListItem
                {
                    Value = "P" + x.ProviderId,
                    Text = x.ProviderName
                });

            model.UserOrganisations = db.Organisations.Where(
                x => x.RelationshipManagerUserId == userId || x.InformationOfficerUserId == userId)
                .OrderBy(x => x.OrganisationName)
                .Select(x => new SelectListItem
                {
                    Value = "O" + x.OrganisationId,
                    Text = x.OrganisationName
                });

            var recentProvisions = new RecentProvisions(userId);
            // Probably not a real issue but break the cache anyways (TFS 133464)
            recentProvisions.Load(true);
            model.RecentProviders = recentProvisions.GetProviders();
            model.RecentOrganisations = recentProvisions.GetOrganisations();

            return model;
        }
    }
}