using System.Web.Mvc;
using System.Web.Routing;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //ignore the CacheManagement handler
            routes.IgnoreRoute("{*cache}", new {cache = @"clear.cache"});

            // Map the Secure Access endpoint
            routes.MapRoute(
                "SAEndpoint",
                "SA/SAML/Accept",
                new {controller = "SA", action = "AssertionConsumerService"}
                );

            // To support site content we must not use the default catch all route
            // Map the home page directly
            routes.MapRoute(
                "HomePage",
                "",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );

            // Map routes for all known controllers
            ContentManager.MapRoutes(routes);

            // Map site content actions and catchall, do not map any routes below this point
            ContentManager.MapCatchAll(routes);

            //routes.MapMvcAttributeRoutes();
        }
    }
}