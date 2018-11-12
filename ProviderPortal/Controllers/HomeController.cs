using System.Web.Mvc;
using System.Web.Routing;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            switch (userContext.ContextName)
            {
                case UserContext.UserContextName.Unauthenticated:
                case UserContext.UserContextName.AuthenticatedNoAccess:
                    return View();
                case UserContext.UserContextName.Administration:
                    return RedirectToAction("Dashboard", "Admin");
                case UserContext.UserContextName.Organisation:
                    return RedirectToAction("Dashboard", "Organisation");
                case UserContext.UserContextName.DeletedOrganisation:
                    return Permission.HasPermission(false, true, Permission.PermissionName.CanEditOrganisation)
                        ? RedirectToAction("Edit", "Organisation")
                        : RedirectToAction("Details", "Organisation");
                case UserContext.UserContextName.Provider:
                    return RedirectToAction("Dashboard", "Report"); //2.1.9.0 Change, provider now opens their report dashboard
                case UserContext.UserContextName.DeletedProvider:
                    return Permission.HasPermission(false, true, Permission.PermissionName.CanEditProvider)
                        ? RedirectToAction("Edit", "Provider")
                        : RedirectToAction("Details", "Provider");
                default:
                    return new HttpUnauthorizedResult();
            }
        }

        [PermissionAuthorize(Permission.PermissionName.CanViewAdministratorHomePage,
            Permission.PermissionName.CanViewOrganisationHomePage)]
        [HttpPost]
        public ActionResult BackToSearch()
        {
            UserContext.InstantiateSession();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult BounceToIndex()
        {
            return RedirectToActionPermanent("Index", "Home");
        }
    }
}