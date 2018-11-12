using System;
using System.Linq;
using System.Web;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Classes
{
    public static class BulkUploadIndicator
    {
        public static bool Pending()
        {
            var db = new ProviderPortalEntities();
            var context = UserContext.GetUserContext();
            return context.ItemId != null
                 && ((context.IsProvider() && db.Providers.Any(x => x.ProviderId == context.ItemId && x.BulkUploadPending))
                 || (context.IsOrganisation() && db.Organisations.Any(x => x.OrganisationId == context.ItemId && x.BulkUploadPending)));
        }
    }
}