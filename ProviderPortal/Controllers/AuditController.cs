using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public class AuditController : BaseController
    {
        //
        // GET: /Audit/
        [ContextAuthorize(UserContext.UserContextName.AdministrationProvider)]
        [PermissionAuthorize(new [] {Permission.PermissionName.CanManuallyAuditCourses, Permission.PermissionName.CanManuallyAuditProviders})]
        public ActionResult Index()
        {
            switch (userContext.ContextName)
            {
                case UserContext.UserContextName.Provider:
                    if (Permission.HasPermission(true, true, Permission.PermissionName.CanManuallyAuditCourses))
                    {
                        return RedirectToAction("Courses");
                    }
                    break;

                    case UserContext.UserContextName.Administration:
                                        if (Permission.HasPermission(true, true, Permission.PermissionName.CanManuallyAuditProviders))
                    {
                        return RedirectToAction("Providers");
                    }
                    break;
            }

            return HttpNotFound();
        }

        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanManuallyAuditProviders)]
        public ActionResult Providers()
        {
            return View();
        }

        [ContextAuthorize(UserContext.UserContextName.Provider)]
        [PermissionAuthorize(Permission.PermissionName.CanManuallyAuditCourses)]
        public ActionResult Courses()
        {
            return View();
        }
    }
}