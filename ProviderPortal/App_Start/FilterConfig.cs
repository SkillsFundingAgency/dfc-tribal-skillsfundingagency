using System.Web.Mvc;

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            filters.Add(new SessionAuthorize());
        }
    }
}