using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class ManageUsersViewModelExtensions
    {
        public static ManageUsersViewModel Populate(this ManageUsersViewModel model, ProviderPortalEntities db,
            UserContext.UserContextInfo context, bool deferredLoad)
        {
            model.CanAdd = false;
            model.CanEditSecureAccessUsers = Permission.HasPermission(false, true,
                Permission.PermissionName.CanEditSecureAccessUsers);
            model.DeferredLoad = deferredLoad;

            var noUkprn = AppGlobal.Language.GetText("Manage_Users_NoUKPRN", "N/A");
            var noProviderOrg = AppGlobal.Language.GetText("Manage_Users_NoProviderOrOrganisation", "N/A");
            var noRole = AppGlobal.Language.GetText("Manage_Users_NoRole", "N/A");

            IQueryable<AspNetUser> aspNetUsers = null;

            switch (context.ContextName)
            {
                case UserContext.UserContextName.Provider:

                    model.CanAdd = Permission.HasPermission(false, true,
                        Permission.PermissionName.CanAddEditProviderUsers);
                    if (!deferredLoad)
                    {
                        aspNetUsers = db.Providers
                            .Where(x => x.ProviderId == (int) context.ItemId)
                            .SelectMany(x => x.AspNetUsers);
                    }
                    break;

                case UserContext.UserContextName.Organisation:

                    model.CanAdd = Permission.HasPermission(false, true,
                        Permission.PermissionName.CanAddEditOrganisationUsers);
                    if (!deferredLoad)
                    {
                        aspNetUsers = db.Organisations
                            .Where(x => x.OrganisationId == (int) context.ItemId)
                            .SelectMany(x => x.AspNetUsers);
                    }
                    break;

                case UserContext.UserContextName.Administration:

                    model.CanAdd = Permission.HasPermission(false, true,
                        Permission.PermissionName.CanAddEditAdminUsers);
                    if (!deferredLoad)
                    {
                        aspNetUsers = db.AspNetUsers;
                    }
                    break;

                default:

                    model.Users = new List<ManageUsersViewModelItem>();
                    model.CanAdd = false;
                    break;
            }

            if (aspNetUsers != null && model.Category != UserCategory.All)
            {
                switch (model.Category)
                {
                    case UserCategory.Active:
                        // Live
                        // DfE Secure Access
                        aspNetUsers = aspNetUsers
                            .Where(x =>
                                !x.IsDeleted && (
                                    x.IsSecureAccessUser
                                    ||
                                    (x.EmailConfirmed && !x.PasswordResetRequired &&
                                    (!x.LockoutEnabled || (x.LockoutEnabled && x.LockoutEndDateUtc == null)))
                                    ));
                        break;
                 
                    case UserCategory.Deleted:
                        // Deleted
                        aspNetUsers = aspNetUsers.Where(x => x.IsDeleted);
                        break;
                    
                    case UserCategory.Pending:
                        // Email Confirmation Required
                        // Pasword Reset Required
                        // Account Locked
                        aspNetUsers = aspNetUsers
                            .Where(x => !x.IsDeleted && !x.IsSecureAccessUser
                                        &&
                                        (!x.EmailConfirmed || x.PasswordResetRequired ||
                                         x.LockoutEnabled && x.LockoutEndDateUtc > DateTime.UtcNow)
                            );
                        break;
                }
            }

            model.Users = aspNetUsers == null
                ? new List<ManageUsersViewModelItem>()
                : aspNetUsers
                .Select(x => new ManageUsersViewModelItem
                {
                    UserId = x.Id,
                    Email = x.Email,
                    Status = x.IsDeleted
                        ? "Deleted"
                        : x.IsSecureAccessUser
                            ? "DfE Secure Access"
                            : !x.EmailConfirmed
                                ? "Email Confirmation Required"
                                : x.PasswordResetRequired
                                    ? "Password Reset Required"
                                    : x.LockoutEnabled && x.LockoutEndDateUtc > DateTime.UtcNow
                                        ? "Account Locked"
                                        : "Live",
                    Ukprn = x.Providers2.Any()
                        ? "" + x.Providers2.FirstOrDefault().Ukprn
                        : x.Organisations2.Any() && x.Organisations2.FirstOrDefault().UKPRN != null
                            ? "" + x.Organisations2.FirstOrDefault().UKPRN
                            : noUkprn,
                    ProviderName = x.Providers2.Any()
                        ? x.Providers2.FirstOrDefault().ProviderName
                        : x.Organisations2.Any()
                            ? x.Organisations2.FirstOrDefault().OrganisationName
                            : noProviderOrg,
                    DisplayName = x.Name,
                    Role = x.AspNetRoles.Any() ? x.AspNetRoles.FirstOrDefault().Name : noRole,
                    LastLogInDate = x.LastLoginDateTimeUtc,
                    IsSecureAccessUser = x.IsSecureAccessUser
                })
                    .OrderByDescending(x => x.LastLogInDate)
                    .ToList();

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this ManageUsersViewModel model)
        {
            var edit = AppGlobal.Language.GetText("Manage_Users_EditAccount", "Edit");
            var view = AppGlobal.Language.GetText("Manage_Users_ViewAccount", "View");
            var action = model.CanAdd ? "Edit" : "Details";
            var verb = model.CanAdd ? edit : view;

            var data = model.Users.Select(x =>
                new[]
                {
                    x.Status,
                    x.Ukprn,
                    x.ProviderName,
                    x.DisplayName,
                    x.Email.Replace("@", "@<wbr/>").Replace(".", ".<wbr/>").Replace("-", "-<wbr/>"),
                    x.Role,
                    x.LastLogInDate.HasValue
                        ? x.LastLogInDate.Value.ToLocalTime().ToString(Constants.ConfigSettings.ShortDateTimeFormat)
                        : null,
                    string.Format(@"<a href=""/Account/{0}/{1}"">{2}</a>",
                        x.IsSecureAccessUser && !model.CanEditSecureAccessUsers ? "Details" : action,
                        x.Email.Replace("@", "(at)").Replace(".", "(dot)"),
                        x.IsSecureAccessUser && !model.CanEditSecureAccessUsers ? view : verb)
                }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data
                }
            };
        }
    }
}