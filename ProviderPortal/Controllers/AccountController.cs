using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Controllers;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;
using TribalTechnology.InformationManagement.Net.Mail;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    using System.Data.Entity;
    using System.Web.Security;

    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // Just ignore returnUrl if required to avoid a round trip to the server
            ViewBag.ReturnUrl = Constants.ConfigSettings.LoginRedirectsToHomePage ? null : returnUrl;
            return View();
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Get both the application user and the base row
            var user = await db.AspNetUsers.FirstOrDefaultAsync(x => x.Email == model.Email);

            // Fail early for invalid users, deleted users and DfE 19-16 users
            if (user == null || user.IsDeleted || user.IsSecureAccessUser)
            {
                return InvalidLoginAttempt(model);
            }

            // ---These checks don't ncecessarily require valid creds---

            // Require the user to have a confirmed email before they can log on.
            if (!user.EmailConfirmed)
            {
                string callbackUrl =
                    await SendEmailConfirmationTokenAsync(user.Id, EmailConfirmationType.UnconfirmedUserLogin);
                return InvalidLoginAttempt(model);
            }

            // Require added users to reset their password manually
            if (user.PasswordResetRequired)
            {
                string callbackUrl =
                    await this.SendPasswordResetTokenAsync(user.Id, PasswordResetType.PasswordResetRequired);
                return InvalidLoginAttempt(model);
            }

            // ---These checks do require valid creds---

            var appUser = await UserManager.FindAsync(model.Email, model.Password);
            // Fail early for invalid credentials and deleted users
            if (appUser == null || user.IsDeleted)
            {

                return InvalidLoginAttempt(model);
            }

            // Prevent log in where the user's provider is not live
            if (user.Providers2.Any(x => x.RecordStatusId != (int) Constants.RecordStatus.Live))
            {
                ModelState.AddModelError("",
                    AppGlobal.Language.GetText(this, "ProviderNotAvailable",
                        "Unable to login. Your provider is no longer available."));

                return View(model);
            }

            // Prevent log in where the user's organisation is not live
            if (user.Organisations2.Any(x => x.RecordStatusId != (int) Constants.RecordStatus.Live))
            {
                ModelState.AddModelError("",
                    AppGlobal.Language.GetText(this, "OrganisationNotAvailable",
                        "Unable to login. Your organisation is no longer available."));

                return View(model);
            }

            var rememberMe = Constants.ConfigSettings.AutoSiteLoginAllow && model.RememberMe;

            // Force a new session
            Session.Abandon();

            // Prevent redirect if not allowed
            if (Constants.ConfigSettings.LoginRedirectsToHomePage)
            {
                returnUrl = null;
            }

            // This counts login failures towards account lockout
            var result =
                await SignInManager.PasswordSignInAsync(model.Email, model.Password, rememberMe, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    user.LastLoginDateTimeUtc = DateTime.UtcNow;
                    await db.SaveChangesAsync();
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, RememberMe = model.RememberMe});
                case SignInStatus.Failure:
                default:
                    return InvalidLoginAttempt(model);
            }
        }

        private ActionResult InvalidLoginAttempt(LoginViewModel model)
        {
            ModelState.AddModelError("",
                AppGlobal.Language.GetText(this, "InvalidUserLoginAttempt", "Invalid login attempt."));
            return View(model);
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                var code = await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel {Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result =
                await
                    SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe,
                        rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", AppGlobal.Language.GetText(this, "InvalidCode", "Invalid code."));
                    return View(model);
            }
        }

        // GET: /Account/Create
        [PermissionAuthorize(
            new[]
            {
                Permission.PermissionName.CanAddEditAdminUsers, Permission.PermissionName.CanAddEditProviderUsers,
                Permission.PermissionName.CanAddEditOrganisationUsers
            })]
        [AllowAnonymous]
        public ActionResult Create(Permission.PermissionName mode = Permission.PermissionName.CanAddEditAdminUsers)
        {
            var model = new AddEditAccountViewModel();
            model.Populate(db);
            return View(model);
        }

        // POST: /Account/Create
        [ContextAuthorize(UserContext.UserContextName.AdministrationProviderOrganisation)]
        [PermissionAuthorize(
            new[]
            {
                Permission.PermissionName.CanAddEditAdminUsers, Permission.PermissionName.CanAddEditProviderUsers,
                Permission.PermissionName.CanAddEditOrganisationUsers
            })]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Create(AddEditAccountViewModel model)
        {
            model.Validate(db, ModelState);
            if (!ModelState.IsValid)
            {
                model.Populate(db);
                return View(model);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                bool deleteUser = false;
                AspNetUser aspNetUser = null;
                try
                {
                    var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                    var result = await UserManager.CreateAsync(user, "Aa1!" + Guid.NewGuid());

                    if (result.Succeeded)
                    {
                        // Convert to a AspNetUser and set the address
                        aspNetUser = db.AspNetUsers.First(x => x.Email == model.Email);
                        aspNetUser.Address = model.Address.ToEntity(db);
                        aspNetUser.Name = model.Name;
                        aspNetUser.PhoneNumber = model.PhoneNumber;
                        aspNetUser.PasswordResetRequired = true;
                        aspNetUser.CreatedDateTimeUtc = DateTime.UtcNow;
                        aspNetUser.CreatedByUserId = Permission.GetCurrentUserId();
                        aspNetUser.IsSecureAccessUser = false;

                        UserManager.AddToRole(aspNetUser.Id, model.RoleId);
                        SetUserTypeAndAffiliation(model, aspNetUser);

                        await db.SaveChangesAsync();

                        trans.Commit();

                        // Send email confirmation email with this link
                        string callbackUrl =
                            await SendEmailConfirmationTokenAsync(user.Id, EmailConfirmationType.AddUser);

                        // Uncomment to debug locally  
                        // ViewBag.Link = callbackUrl;                   
                        ViewBag.Message = AppGlobal.Language.GetText(this, "EmailConfirmationInitial",
                            "The new user will need to check their email and confirm their account. Once their account is confirmed they can set a password and log in to the portal.");

                        ShowGenericSavedMessage();
                        return View("Info");
                    }
                    AddErrors(result);
                }
                catch(Exception ex)
                {
                    string errorMessage = string.Empty;
                    // In development uncomment line bellow
                    //errorMessage = ex.Message;

                    // User create failed transaction automatically gets rolled back
                    // but we still have to delete the user manually
                    if (aspNetUser != null)
                    {
                        deleteUser = true;
                    }
                    ModelState.AddModelError("",
                        "There was a problem creating the user, please try again and contact support if the problem persists. " + errorMessage);
                }
                if (deleteUser)
                {
                    var user = UserManager.Users.SingleOrDefault(u => u.Id == aspNetUser.Id);
                    // ReSharper disable once CSharpWarnings::CS4014
                    await UserManager.DeleteAsync(user);
                }
            }

            // If we got this far, something failed, redisplay form
            model.Populate(db);
            return View(model);
        }

        private void SetUserTypeAndAffiliation(AddEditAccountViewModel model, AspNetUser aspNetUser)
        {
            var affiliation = (UserContext.UserContextInfo) null;
            var wasInfoOfficer = aspNetUser.ProviderUserTypeId == (int) Constants.ProviderUserTypes.InformationOfficer;
            var wasRelManager = aspNetUser.ProviderUserTypeId == (int) Constants.ProviderUserTypes.RelationshipManager;

            switch (userContext.ContextName)
            {
                case UserContext.UserContextName.Administration:

                    aspNetUser.ProviderUserTypeId = model.UserTypeId;
                    // ReSharper disable once PossibleInvalidOperationException
                    var roleContext =
                        (UserContext.UserContextName) db.AspNetRoles.First(x => x.Name == model.RoleId).UserContextId;
                    affiliation = roleContext == UserContext.UserContextName.Administration
                        ? new UserContext.UserContextInfo(roleContext, null)
                        : TypeaheadController.DecodeProviderId(model.ProviderId);
                    break;

                case UserContext.UserContextName.Provider:

                    aspNetUser.ProviderUserTypeId = (int) Constants.ProviderUserTypes.NormalUser;
                    affiliation = userContext;
                    break;

                case UserContext.UserContextName.Organisation:

                    aspNetUser.ProviderUserTypeId = (int) Constants.ProviderUserTypes.NormalUser;
                    affiliation = userContext;
                    break;
            }

            if (affiliation != null)
            {
                aspNetUser.Providers2.Clear();
                aspNetUser.Organisations2.Clear();
                if (affiliation.ContextName == UserContext.UserContextName.Provider)
                {
                    aspNetUser.Providers2.Add(db.Providers.First(x => x.ProviderId == (int) affiliation.ItemId));
                }
                else if (affiliation.ContextName == UserContext.UserContextName.Organisation)
                {
                    aspNetUser.Organisations2.Add(
                        db.Organisations.First(x => x.OrganisationId == (int) affiliation.ItemId));
                }
            }

            if (wasInfoOfficer && aspNetUser.ProviderUserTypeId != (int) Constants.ProviderUserTypes.InformationOfficer)
            {
                RemoveInformationOfficerLink(aspNetUser.Id);
            }
            if (wasRelManager && aspNetUser.ProviderUserTypeId != (int) Constants.ProviderUserTypes.RelationshipManager)
            {
                RemoveRelationshipManagerLink(aspNetUser.Id);
            }
        }

        /// <summary>
        /// Remove relationship manager link from providers and organisations.
        /// </summary>
        /// <param name="userId">The user to remove.</param>
        private void RemoveRelationshipManagerLink(string userId)
        {
            var providerParams = new SqlParameter("userId", userId);
            db.Database.ExecuteSqlCommand(
                "UPDATE Provider SET RelationshipManagerUserId = null where RelationshipManagerUserId = @userId",
                providerParams);
            var orgParams = new SqlParameter("userId", userId);
            db.Database.ExecuteSqlCommand(
                "UPDATE Organisation SET RelationshipManagerUserId = null where RelationshipManagerUserId = @userId",
                orgParams);
        }

        /// <summary>
        /// Remove information officer link from providers and organisations.
        /// </summary>
        /// <param name="userId">The user to remove.</param>
        private void RemoveInformationOfficerLink(string userId)
        {
            var providerParams = new SqlParameter("userId", userId);
            db.Database.ExecuteSqlCommand(
                "UPDATE Provider SET InformationOfficerUserId = null where InformationOfficerUserId = @userId",
                providerParams);
            var orgParams = new SqlParameter("userId", userId);
            db.Database.ExecuteSqlCommand(
                "UPDATE Organisation SET InformationOfficerUserId = null where InformationOfficerUserId = @userId",
                orgParams);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (!Constants.ConfigSettings.AllowSelfRegistration)
                return View("RegistrationDisabled");
            var model = new RegisterViewModel();
            model.Populate(db);
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!Constants.ConfigSettings.AllowSelfRegistration)
                return View("RegistrationDisabled");

            model.Address.Validate(db, ModelState);
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Convert to a AspNetUser and set the address
                    var aspNetUser = db.AspNetUsers.First(x => x.Email == model.Email);
                    aspNetUser.Address = model.Address.ToEntity(db);
                    aspNetUser.Name = model.Name;
                    await db.SaveChangesAsync();

                    //  Comment the following line to prevent log in until the user is confirmed.
                    //  await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // Send email confirmation email with this link
                    string callbackUrl =
                        await SendEmailConfirmationTokenAsync(user.Id, EmailConfirmationType.SelfRegister);

                    // Uncomment to debug locally  
                    // ViewBag.Link = callbackUrl;                   
                    ViewBag.Message = AppGlobal.Language.GetText(this, "EmailConfirmationInitial",
                        "Check your email and confirm your account, you must be confirmed "
                        + "before you can log in.");

                    ShowGenericSavedMessage();
                    return View("Info");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            model.Populate(db);
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (!String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(code))
            {
                var result = await UserManager.ConfirmEmailAsync(userId, code);
                if (result.Succeeded)
                {
                    var aspNetUser = await db.AspNetUsers.FindAsync(userId);
                    if (aspNetUser == null || aspNetUser.IsDeleted) return View("Error");
                    if (aspNetUser.PasswordResetRequired)
                    {
                        string passwordResetCode = await UserManager.GeneratePasswordResetTokenAsync(userId);
                        return RedirectToAction("ResetPassword",
                            new { userId = userId, code = passwordResetCode, confirm = true });
                    }
                    return View("ConfirmEmail");
                }
                ViewBag.Message = AppGlobal.Language.GetText(this, "ExpiredCode",
                    "Your account confirmation code has expired or was not recognised.");
            }
            else
            {
                ViewBag.Message = AppGlobal.Language.GetText(this, "InvalidCode",
                    "Your account confirmation code was not recognised.");
            }

            if (!String.IsNullOrEmpty(userId))
            {
                var aspNetUser = await db.AspNetUsers.FindAsync(userId);
                if (aspNetUser == null || aspNetUser.IsDeleted) return View("Info");
                if (aspNetUser.EmailConfirmed && ViewBag.errorMessage == null)
                {
                    if (aspNetUser.PasswordResetRequired)
                    {
                        await db.SaveChangesAsync();
                        string passwordResetCode = await UserManager.GeneratePasswordResetTokenAsync(userId);
                        return RedirectToAction("ResetPassword",
                            new {userId = userId, code = passwordResetCode, confirm = true});
                    }
                    return ViewBag.Message == null ? View("ConfirmEmail") : View("Info");
                }
                // Send email confirmation email with this link
                string callbackUrl =
                    await SendEmailConfirmationTokenAsync(userId, EmailConfirmationType.UnconfirmedUserLogin);
                ViewBag.Message += " " + AppGlobal.Language.GetText(this, "ResentCode",
                    "A new account confirmation code has been sent to you. Check your email and confirm your account.");
                return View("Info");
            }

            return View("Info");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // This will deliberately fail on SecureAccess users as their username is not an email address
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
                var aspNetUser = await db.AspNetUsers.FindAsync(user.Id);
                if (aspNetUser.IsDeleted || aspNetUser.IsSecureAccessUser)
                {
                    // Don't reveal that the user is deleted or a secure access user
                    return View("ForgotPasswordConfirmation");
                }

                var callbackUrl = await SendPasswordResetTokenAsync(user.Id, PasswordResetType.ForgottenPassword);

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string userId, string code, bool confirm = false)
        {
            if (!String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(code))
            {
                var result = await UserManager.VerifyUserTokenAsync(userId, "ResetPassword", code);
                if (result)
                {
                    var model = new ResetPasswordViewModel
                    {
                        EmailConfirmation = confirm,
                        Code = code
                    };
                    return View("ResetPassword", model);
                }
                ViewBag.Message = AppGlobal.Language.GetText(this, "ExpiredCode",
                    "Your password reset request has expired or was not recognised.");
            }
            else
            {
                ViewBag.Message = AppGlobal.Language.GetText(this, "InvalidCode",
                    "Your password reset request was not recognised.");
            }

            if (!String.IsNullOrEmpty(userId))
            {
                var aspNetUser = await db.AspNetUsers.FindAsync(userId);
                if (aspNetUser == null || aspNetUser.IsDeleted) return View("Info");
                // Send password reset email with this link
                string callbackUrl =
                    await SendPasswordResetTokenAsync(userId, PasswordResetType.ForgottenPassword);
                ViewBag.Message += " " + AppGlobal.Language.GetText(this, "ResentCode",
                    "A new password reset request has been sent to you. Check your email and try again, if you have remembered your password you can ignore this email and log in as normal.");
            }

            return View("Info");
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                var aspNetUser = db.AspNetUsers.First(x => x.Email == model.Email);
                aspNetUser.PasswordResetRequired = false;
                db.SaveChanges();
                if (model.EmailConfirmation)
                    return View("ConfirmEmail");
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions =
                userFactors.Select(purpose => new SelectListItem {Text = purpose, Value = purpose}).ToList();
            return
                View(new SendCodeViewModel {Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode",
                new {Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe});
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, RememberMe = false});
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation",
                        new ExternalLoginConfirmationViewModel {Email = loginInfo.Email});
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            SessionManager.End();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        //
        // GET: /Account/Find
        [ContextAuthorize(UserContext.UserContextName.AdministrationProviderOrganisation)]
        [PermissionAuthorize(
            new[]
            {
                Permission.PermissionName.CanViewAdminUsers, Permission.PermissionName.CanViewOrganisationUsers,
                Permission.PermissionName.CanViewProviderUsers
            })]
        public ActionResult Find()
        {
            var model = new AccountSearchViewModel();
            model.Populate(db);
            return View(model);
        }

        //
        // POST: /Account/Find
        [ContextAuthorize(UserContext.UserContextName.AdministrationProviderOrganisation)]
        [PermissionAuthorize(
            new[]
            {
                Permission.PermissionName.CanViewAdminUsers, Permission.PermissionName.CanViewOrganisationUsers,
                Permission.PermissionName.CanViewProviderUsers
            })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Find(AccountSearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Call populate to reset the CanAdd property
                model.Populate(db);
                var user = db.AspNetUsers.FirstOrDefault(x => x.UserName == model.UserId);
                if (user != null)
                {
                    var canEdit = model.CanAdd && (!user.IsSecureAccessUser || model.CanEditSecureAccessUsers);
                    return RedirectToAction(canEdit ? "Edit" : "Details",
                        new {id = model.UserId.Replace("@", "(at)").Replace(".", "(dot)")});
                }
            }

            // Something happened so return the model
            ModelState.AddModelError("Username",
                AppGlobal.Language.GetText(this, "UnableToSwitch",
                    "Unable to select the specified user account."));
            model.Populate(db);
            return View(model);
        }

        //
        // GET: /Account/Details/userId
        public ActionResult Details(string id)
        {
            id = id == null ? User.Identity.Name : id.Replace("(at)", "@").Replace("(dot)", ".");
            var model = new AddEditAccountViewModel();

            try
            {
                model.Populate(db, id);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return HttpNotFound();
            }

            if (!UserCanVerbUser(UserAction.View, model.UserId))
            {
                throw new SecurityException("Operation not permitted.");
            }

            return View(model);

        }

        //
        // GET: /Account/Edit/userId
        public ActionResult Edit(string id)
        {
            id = id == null ? User.Identity.Name : id.Replace("(at)", "@").Replace("(dot)", ".");

            var model = new AddEditAccountViewModel();
            try
            {
                model.Populate(db, id);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return HttpNotFound();
            }

            if (!UserCanVerbUser(UserAction.Edit, model.UserId))
            {
                throw new SecurityException("Operation not permitted.");
            }

            return View(model);
        }

        //
        // POST: /Account/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, AddEditAccountViewModel model)
        {
            if (!UserCanVerbUser(UserAction.Edit, model.UserId))
            {
                throw new SecurityException("Operation not permitted.");
            }

            model.Validate(db, ModelState);
            if (ModelState.IsValid)
            {
                var user = db.AspNetUsers.First(x => x.Id == model.UserId);
                var emailAddressChanged = !user.Email.Equals(model.Email, StringComparison.CurrentCultureIgnoreCase);
                var phoneNumberChanged = user.PhoneNumber != null &&
                                         !user.PhoneNumber.Equals(model.PhoneNumber,
                                             StringComparison.CurrentCultureIgnoreCase);

                var oldEmail = new MailAddress(user.Email, user.Name);
                var newEmail = new MailAddress(model.Email, model.Name);

                user.Email = model.Email;
                user.UserName = model.Email;
                user.Name = model.Name;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = user.IsSecureAccessUser ? null : model.Address.ToEntity(db);
                user.ModifiedDateTimeUtc = DateTime.UtcNow;
                user.ModifiedByUserId = Permission.GetCurrentUserId();

                var trustedModel = new AddEditAccountViewModel();
                trustedModel.Populate(db, model.UserId);

                if (trustedModel.CanEditRole && !trustedModel.EditingSelf)
                {
                    if (user.AspNetRoles.All(x => x.Name != model.RoleId))
                    {
                        UserManager.RemoveFromRole(user.Id, user.AspNetRoles.First().Name);
                        UserManager.AddToRole(user.Id, model.RoleId);
                    }
                }

                if (!trustedModel.EditingSelf && userContext.ContextName == UserContext.UserContextName.Administration)
                {
                    SetUserTypeAndAffiliation(model, user);
                }

                // Users must have a verified email address to access the site so lock them out
                if (emailAddressChanged)
                {
                    user.SecurityStamp = Guid.NewGuid().ToString();
                    user.EmailConfirmed = false;
                    if (trustedModel.EditingSelf)
                    {
                        AuthenticationManager.SignOut();
                        SessionManager.End();
                    }
                }

                if (phoneNumberChanged)
                {
                    user.PhoneNumberConfirmed = false;
                }

                await db.SaveChangesAsync();

                if (emailAddressChanged)
                {
                    await
                        SendEmailConfirmationTokenAsync(user.Id, EmailConfirmationType.LoginDetailsChanged, oldEmail,
                            newEmail);
                }

                ShowGenericSavedMessage();
                return trustedModel.EditingSelf ? RedirectToAction("Index", "Home") : RedirectToAction("Users", "Manage");
            }

            model.Populate(db);
            return View(model);
        }

        //
        // POST: /Account/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            if (UserCanVerbUser(UserAction.Edit, id) && id != Permission.GetCurrentUserId())
            {
                var user = db.AspNetUsers.FirstOrDefault(x => x.Id == id);
                if (user == null)
                    return new HttpNotFoundResult();
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.IsDeleted = true;

                if (user.ProviderUserTypeId == (int) Constants.ProviderUserTypes.RelationshipManager)
                {
                    RemoveRelationshipManagerLink(user.Id);
                }
                if (user.ProviderUserTypeId == (int) Constants.ProviderUserTypes.InformationOfficer)
                {
                    RemoveInformationOfficerLink(user.Id);
                }
                await db.SaveChangesAsync();

                ViewBag.Message = AppGlobal.Language.GetText(this, "DeleteConfirm",
                    "The user account has been deleted.");

                ShowGenericSavedMessage();
                return View("Info");
            }
            ViewBag.Message = AppGlobal.Language.GetText(this, "DeleteNotPermitted",
                "You are not permitted to perform this action.");
            return View("Info");
        }

        //
        // POST: /Account/Restore
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Restore(string id)
        {
            if (UserCanVerbUser(UserAction.Edit, id) && id != Permission.GetCurrentUserId())
            {
                var user = db.AspNetUsers.FirstOrDefault(x => x.Id == id);
                if (user == null)
                    return new HttpNotFoundResult();
                user.IsDeleted = false;
                await db.SaveChangesAsync();

                ViewBag.Message = AppGlobal.Language.GetText(this, "RestoreConfirm",
                    "The user account has been restored.");
                ShowGenericSavedMessage();
                return View("Info");
            }
            ViewBag.Message = AppGlobal.Language.GetText(this, "RestoreNotPermitted",
                "You are not permitted to perform this action.");
            return View("Info");

        }

        /// <summary>
        /// Actions that one user might do to another.
        /// </summary>
        private enum UserAction
        {
            /// <summary>
            /// Edit, create or delete the user.
            /// </summary>
            Edit,

            /// <summary>
            /// View the user details.
            /// </summary>
            View,
        }

        /// <summary>
        /// Can the current user <c ref="UserAction"/> another user in the current context.
        /// </summary>
        /// <param name="verb">The action.</param>
        /// <param name="otherUserId">The user ID to be operated on.</param>
        /// <returns>True if the operation is permitted.</returns>
        private bool UserCanVerbUser(UserAction verb, string otherUserId)
        {
            if (otherUserId == User.Identity.GetUserId())
            {
                return true;
            }

            var canEditSecureAccessUsers = Permission.HasPermission(false, true,
                Permission.PermissionName.CanEditSecureAccessUsers);
            var user = db.AspNetUsers.FirstOrDefault(x => x.Id == otherUserId);
            if (user == null) return false;

            switch (userContext.ContextName)
            {
                case UserContext.UserContextName.Provider:
                {
                    return userContext.ItemId != null
                           && (user.Providers2.Any(x => x.ProviderId == userContext.ItemId)
                               && ((verb == UserAction.Edit &&
                                    Permission.HasPermission(false, true,
                                        Permission.PermissionName.CanAddEditProviderUsers)
                                    && (!user.IsSecureAccessUser || canEditSecureAccessUsers))
                                   ||
                                   (verb == UserAction.View &&
                                    Permission.HasPermission(false, true, Permission.PermissionName.CanViewProviderUsers))));
                }

                case UserContext.UserContextName.Organisation:
                {
                    return userContext.ItemId != null
                           && (user.Organisations2.Any(x => x.OrganisationId == userContext.ItemId)
                               && ((verb == UserAction.Edit &&
                                    Permission.HasPermission(false, true,
                                        Permission.PermissionName.CanAddEditOrganisationUsers)
                                    && (!user.IsSecureAccessUser || canEditSecureAccessUsers))
                                   ||
                                   (verb == UserAction.View &&
                                    Permission.HasPermission(false, true,
                                        Permission.PermissionName.CanViewOrganisationUsers))));
                }

                case UserContext.UserContextName.Administration:
                {
                    return
                        (verb == UserAction.Edit &&
                         Permission.HasPermission(false, true, Permission.PermissionName.CanAddEditAdminUsers)
                         && (!user.IsSecureAccessUser || canEditSecureAccessUsers))
                        ||
                        (verb == UserAction.View &&
                         Permission.HasPermission(false, true, Permission.PermissionName.CanViewAdminUsers));
                }

                default:
                    return false;
            }
        }


        [HttpPost]
        public void SaveUserWizardPreference(bool showAgain)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string thisUser = System.Web.HttpContext.Current.User.Identity.Name;
                var user = db.AspNetUsers.FirstOrDefault(u => u.UserName.Equals(thisUser));
                if (user != null)
                {
                    user.ShowUserWizard = showAgain;
                    db.SaveChanges();
                }
            }
        }



        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        /// <summary>
        /// Specifies the reason why an account confirmation email is being sent.
        /// </summary>
        public enum EmailConfirmationType
        {
            /// <summary>
            /// Email is the result of a user self registering.
            /// </summary>
            SelfRegister,

            /// <summary>
            /// Email is the result of a new user being added.
            /// </summary>
            AddUser,

            /// <summary>
            /// Email is the result of an unconfirmed user attempting to log in.
            /// </summary>
            UnconfirmedUserLogin,

            /// <summary>
            /// Email is the result of an existing user's email address being changed.
            /// </summary>
            LoginDetailsChanged,
        }

        /// <summary>
        /// Send the account confirmation email.
        /// </summary>
        /// <param name="userId">User ID to send the email to.</param>
        /// <param name="emailType">Specifies the reason type why the email is being sent.</param>
        /// <param name="oldEmail">For EmailConfirmationType.LoginDetailsChanged this is the user's old email address</param>
        /// <param name="newEmail">For EmailConfirmationType.LoginDetailsChanged this is the user's new email address</param>
        /// <returns>The callback url</returns>
        private async Task<string> SendEmailConfirmationTokenAsync(string userId, EmailConfirmationType emailType,
            MailAddress oldEmail = null, MailAddress newEmail = null)
        {
            if (userId == null) return null;

            String CallbackUrl = Url.Action("ConfirmEmail", "Account", new { userId, code = "CodeGoesHere" }, Request.Url == null ? "https" : Request.Url.Scheme);
            String code = HttpUtility.UrlEncode(UserManager.GenerateEmailConfirmationTokenAsync(userId).Result);
            String callbackUrl = CallbackUrl.Replace("CodeGoesHere", code);

//            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
//            var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = userId, code = code}, protocol: Request.Url.Scheme);

            if (emailType == EmailConfirmationType.LoginDetailsChanged)
            {
                if (oldEmail == null || newEmail == null)
                {
                    throw new ArgumentNullException("The oldEmail and newEmail paramaters must be specified.");
                }

                //AppGlobal.EmailQueue.AddToSendQueue(
                //    TemplatedEmail.EmailMessage(
                //        oldEmail,
                //        new MailAddressCollection {newEmail}, null, Constants.EmailTemplates.LoginDetailsChanged,
                //        new List<EmailParameter>
                //        {
                //            new EmailParameter("%OLDEMAIL%", oldEmail.Address),
                //            new EmailParameter("%NEWEMAIL%", newEmail.Address)
                //        }));
                // also sends Constants.EmailTemplates.EmailConfirmation without resent flag

                var emailMessageLoginChanged = TemplatedEmail.EmailMessage(
                        oldEmail,
                        new MailAddressCollection { newEmail }, null, Constants.EmailTemplates.LoginDetailsChanged,
                        new List<EmailParameter>
                        {
                            new EmailParameter("%OLDEMAIL%", oldEmail.Address),
                            new EmailParameter("%NEWEMAIL%", newEmail.Address)
                        });

               var responseLoginChanged = SfaSendGridClient.SendGridEmailMessage(emailMessageLoginChanged, userId);
            }

            //AppGlobal.EmailQueue.AddToSendQueue(
            //    TemplatedEmail.EmailMessage(
            //        userId,
            //        emailType == EmailConfirmationType.AddUser
            //            ? Constants.EmailTemplates.NewUserWelcome
            //            : Constants.EmailTemplates.EmailConfirmation /* LoginDetailsChanged also hits this*/,
            //        new List<EmailParameter>
            //        {
            //            new EmailParameter("%URL%", callbackUrl),
            //            new EmailParameter("%RESENT%",
            //                emailType == EmailConfirmationType.UnconfirmedUserLogin ? "(Resent)" : "")
            //        }));

            var emailMessage = TemplatedEmail.EmailMessage(
                    userId,
                    emailType == EmailConfirmationType.AddUser
                        ? Constants.EmailTemplates.NewUserWelcome
                        : Constants.EmailTemplates.EmailConfirmation /* LoginDetailsChanged also hits this*/,
                    new List<EmailParameter>
                    {
                        new EmailParameter("%URL%", callbackUrl),
                        new EmailParameter("%RESENT%",
                            emailType == EmailConfirmationType.UnconfirmedUserLogin ? "(Resent)" : "")
                    });

            var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, userId);

            return callbackUrl;
        }

        /// <summary>
        /// Specifies the reason why a password reset email is being sent.
        /// </summary>
        public enum PasswordResetType
        {
            /// <summary>
            /// The user has requested a password reset.
            /// </summary>
            ForgottenPassword,

            /// <summary>
            /// The password must be reset before the user can log in.
            /// </summary>
            PasswordResetRequired
        }

        /// <summary>
        /// Send the password reset email.
        /// </summary>
        /// <param name="userId">User ID to send the email to.</param>
        /// <returns>The callback url.</returns>
        private async Task<string> SendPasswordResetTokenAsync(string userId, PasswordResetType emailType)
        {
            // Send an email with this link
            String CallbackUrl = Url.Action("ResetPassword", "Account", new { userId, code = "CodeGoesHere" }, Request.Url == null ? "https" : Request.Url.Scheme);
            String code = HttpUtility.UrlEncode(UserManager.GeneratePasswordResetTokenAsync(userId).Result);
            String callbackUrl = CallbackUrl.Replace("CodeGoesHere", code);

            //string code = await UserManager.GeneratePasswordResetTokenAsync(userId);
            //var callbackUrl = Url.Action("ResetPassword", "Account", new {userId = userId, code = code}, protocol: Request.Url.Scheme);

            //AppGlobal.EmailQueue.AddToSendQueue(
            //    TemplatedEmail.EmailMessage(
            //        userId,
            //        emailType == PasswordResetType.ForgottenPassword
            //            ? Constants.EmailTemplates.PasswordReset
            //            : Constants.EmailTemplates.PasswordCreate,
            //        new List<EmailParameter>
            //        {
            //            new EmailParameter("%URL%", callbackUrl)
            //        }));

            var emailMessage = TemplatedEmail.EmailMessage(
                    userId,
                    emailType == PasswordResetType.ForgottenPassword
                        ? Constants.EmailTemplates.PasswordReset
                        : Constants.EmailTemplates.PasswordCreate,
                    new List<EmailParameter>
                    {
                        new EmailParameter("%URL%", callbackUrl)
                    });

           var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, userId);

            return callbackUrl;
        }

        #endregion
    }
}