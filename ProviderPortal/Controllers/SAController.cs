using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ITfoxtec.Saml2;
using ITfoxtec.Saml2.Bindings;
using ITfoxtec.Saml2.Mvc;
using ITfoxtec.Saml2.Schemas;
using ITfoxtec.Saml2.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;
using Tribal.SkillsFundingAgency.ProviderPortal.SingleSignOn;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    [AllowAnonymous]
    // ReSharper disable once InconsistentNaming
    public class SAController : BaseController
    {
        private const string RelayStateReturnUrl = "ReturnUrl";

        public ApplicationSignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
        }

        public ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public ActionResult Index()
        {
            // TODO
            // No SA session: redirect to SA login page, user logs in, is directed to P16
            // Timed out SA session: redirect to SA timeout page, user clicks through to SA login page, user logs in, is directed to P16
            // Active session: directed to P16

            // Note: Some of this will probably be handled by the SessionAuthorize attribute

            return RedirectToAction("Login");
        }

        public ActionResult Login(string returnUrl)
        {
            var binding = new Saml2PostBinding();
            binding.SetRelayStateQuery(new Dictionary<string, string> { { RelayStateReturnUrl, returnUrl } });

            return binding.Bind(new Saml2AuthnRequest
            {
                ForceAuthn = false,
                IsPassive = false,
                NameIdPolicy =
                    new NameIdPolicy
                    {
                        AllowCreate = true,
                        Format = "urn:oasis:names:tc:SAML:2.0:nameid-format:transient"
                    },
                RequestedAuthnContext = new RequestedAuthnContext
                {
                    Comparison = AuthnContextComparisonTypes.Exact,
                    AuthnContextClassRef =
                        new[] { AuthnContextClassTypes.PasswordProtectedTransport.OriginalString }
                },
                Issuer = new EndpointReference(Constants.ConfigSettings.SAServiceProviderEntityId),
                Destination = new EndpointAddress(Constants.ConfigSettings.SADestination),
                AssertionConsumerServiceUrl =
                    new EndpointAddress(Constants.ConfigSettings.SAAssertionConsumerServiceUrl),
            }).ToActionResult();
        }

        [HttpPost]
        public async Task<ActionResult> AssertionConsumerService()
        {
            var binding = new Saml2PostBinding();
            var saml2AuthnResponse = new Saml2AuthnResponse();
            binding.Unbind(Request, saml2AuthnResponse, CertificateUtil.Load(Constants.ConfigSettings.SAX509Certificate));

            SAAuthenticationResponse claims;
            try
            {
                claims = DecodeAuthnResponse(saml2AuthnResponse);
            }
            catch (Exception e)
            {
                AppGlobal.Log.WriteLog(String.Format("Secure Access - Decoding AuthnResponse failed due to {0}.",
                    e.InnerException));
                ViewBag.MessageHtml = AppGlobal.Language.GetText(this, "SSOLogInFailed",
                    "Log in failed for DfE Secure Access. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href='tel:08448115028'>0844 8115 028</a> or <a href='mailto:dfe.support@coursedirectoryproviderportal.org.uk'>dfe.support@coursedirectoryproviderportal.org.uk</a>.");
                ViewBag.ButtonText = AppGlobal.Language.GetText(this, "BackToSecureAccessButton",
                    "Back to Secure Access");
                ViewBag.ButtonUrl = Constants.ConfigSettings.SAHomePage;
                return View("Info");
            }

            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                AuthenticationManager.SignOut();
                SessionManager.End();
            }

            UserResponse userResult = await GetUserAsync(claims);
            if (!String.IsNullOrEmpty(userResult.Message))
            {
                ViewBag.MessageHtml = userResult.Message;
                ViewBag.MessageHtml = userResult.Message;
                ViewBag.ButtonText = AppGlobal.Language.GetText(this, "BackToSecureAccessButton",
                    "Back to Secure Access");
                ViewBag.ButtonUrl = Constants.ConfigSettings.SAHomePage;
                return View("Info");
            }

            ProviderResponse providerResult = await GetValidatedProviderAsync(claims, userResult.User.Id);
            if (!String.IsNullOrEmpty(providerResult.Message))
            {
                ViewBag.MessageHtml = providerResult.Message;
                ViewBag.ButtonText = AppGlobal.Language.GetText(this, "BackToSecureAccessButton",
                    "Back to Secure Access");
                ViewBag.ButtonUrl = Constants.ConfigSettings.SAHomePage;
                return View("Info");
            }

            // Associate user with the provider
            if (!userResult.User.Providers2.Any() ||
                userResult.User.Providers2.All(x => x.ProviderId != providerResult.Provider.ProviderId))
            {
                userResult.User.Providers2.Clear();
                userResult.User.Providers2.Add(providerResult.Provider);
            }
            userResult.User.LastLoginDateTimeUtc = DateTime.UtcNow;
            await db.SaveChangesAsync();

            // Actually log them in
            ApplicationUser user = await UserManager.FindByIdAsync(userResult.User.Id);
            await SignInManager.SignInAsync(user, true, false);

            // If we are doing a SAML2 log out we need to store this information for later use.
            // Set some extra properties so we can log out later
            //CacheManagement.CacheHandler.Add("SAML2Claims:" + aspNetUser.Id, new List<Claim>
            //{
            //    new Claim(Saml2ClaimTypes.NameId, claims.NameId),
            //    new Claim(Saml2ClaimTypes.NameIdFormat, claims.NameIdFormat),
            //    new Claim(Saml2ClaimTypes.SessionIndex, claims.SessionIndex),
            //});

            SessionManager.Start();

            // Bounce them via the following page so that their session is instantiated correctly
            string returnUrl = binding.GetRelayStateQuery()[RelayStateReturnUrl];
            return RedirectToAction("LogInComplete", new { returnUrl });
        }

        public ActionResult LogInComplete(string returnUrl)
        {
            return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Index", "Home") : returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            bool isSecureAccessUser = Session[Constants.SessionFieldNames.IsSecureAccessUser] != null &&
                                      (bool)Session[Constants.SessionFieldNames.IsSecureAccessUser];
            if (!User.Identity.IsAuthenticated || !isSecureAccessUser)
            {
                return Redirect(Url.Content("~/"));
            }

            AuthenticationManager.SignOut();
            SessionManager.End();

            return Redirect(Constants.ConfigSettings.SALoggedOutNotification);
        }

        #region Private methods

        private SAAuthenticationResponse DecodeAuthnResponse(Saml2AuthnResponse saml2AuthnResponse)
        {
            if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
                throw new InvalidOperationException(
                    string.Format("The SAML2 Response Status is not Success, the Response Status is: {0}.",
                        saml2AuthnResponse.Status));
            var incomingPrincipal = new ClaimsPrincipal(saml2AuthnResponse.ClaimsIdentity);
            if (incomingPrincipal.Identity == null || !incomingPrincipal.Identity.IsAuthenticated)
                throw new InvalidOperationException("No Claims Identity created from SAML2 Response.");
            ClaimsPrincipal claimsPrincipal =
                FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager
                    .Authenticate(null, incomingPrincipal);
            return new SAAuthenticationResponse(claimsPrincipal.Claims);
        }

        private async Task<UserResponse> GetUserAsync(SAAuthenticationResponse claims)
        {
            AspNetUser aspNetUser =
                db.AspNetUsers.FirstOrDefault(x => x.SecureAccessUserId == claims.UserId && x.IsSecureAccessUser);
            if (aspNetUser != null)
            {
                aspNetUser.Name = String.Format("{0} {1}", claims.FirstName, claims.LastName).Trim();
                return new UserResponse(aspNetUser);
            }

            List<AspNetUser> matches =
                db.AspNetUsers.Where(x => x.UserName == claims.UserName || x.Email == claims.Email).ToList();
            if (matches.Any(x => x.UserName == claims.UserName))
            {
                return UserResponseError(Constants.ErrorCodes.SecureAccessUserNameInUse);
            }

            if (matches.Any(x => x.Email == claims.Email))
            {
                return UserResponseError(Constants.ErrorCodes.SecureAccessEmailInUse);
            }

            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                bool deleteUser = false;
                try
                {
                    var user = new ApplicationUser { UserName = claims.UserName, Email = claims.Email };
                    IdentityResult result = await UserManager.CreateAsync(user, "Aa1!" + Guid.NewGuid());
                    if (result.Succeeded)
                    {
                        // Convert to a AspNetUser and set the address
                        aspNetUser = db.AspNetUsers.First(x => x.Email == claims.Email && x.UserName == claims.UserName);
                        aspNetUser.Address = null;
                        aspNetUser.Name = String.Format("{0} {1}", claims.FirstName, claims.LastName).Trim();
                        aspNetUser.PhoneNumber = null;
                        aspNetUser.PasswordResetRequired = false;
                        aspNetUser.CreatedDateTimeUtc = DateTime.UtcNow;
                        aspNetUser.CreatedByUserId = Permission.GetCurrentUserId();
                        aspNetUser.IsSecureAccessUser = true;
                        aspNetUser.SecureAccessUserId = claims.UserId;
                        bool hasOtherUsers = await db.Providers.Where(x => x.SecureAccessId == claims.OrganisationId)
                            .SelectMany(x => x.AspNetUsers)
                            .AnyAsync();
                        UserManager.AddToRole(aspNetUser.Id,
                            hasOtherUsers
                                ? Constants.ConfigSettings.SAUserRoleSecondary
                                : Constants.ConfigSettings.SAUserRolePrimary);
                        aspNetUser.ProviderUserTypeId = (int)Constants.ProviderUserTypes.NormalUser;
                        await db.SaveChangesAsync();
                        trans.Commit();
                        return new UserResponse(aspNetUser);
                    }

                    // There were non-fatal errors
                    return UserResponseError(Constants.ErrorCodes.SecureAccessCreateFailed);
                }
                catch (Exception e)
                {
                    // User create failed transaction automatically gets rolled back
                    // but we still have to delete the user manually
                    if (aspNetUser != null)
                    {
                        deleteUser = true;
                    }

                    AppGlobal.Log.WriteLog(
                        String.Format("Secure Access - Failed to create user account for {0} due to {1}.",
                            claims.UserName, e.InnerException));
                }
                if (deleteUser)
                {
                    ApplicationUser user = UserManager.Users.SingleOrDefault(u => u.Id == aspNetUser.Id);
                    // ReSharper disable once CSharpWarnings::CS4014
                    await UserManager.DeleteAsync(user);
                }
            }
            return UserResponseError(Constants.ErrorCodes.SecureAccessCreateError);
        }

        private UserResponse UserResponseError(Constants.ErrorCodes error)
        {
            string errorMessage = String.Format(
                AppGlobal.Language.GetText(this, error.ToString(),
                    "Log in failed for DfE Secure Access. Invalid log in - error code {0}. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href='tel:08448115028'>0844 8115 028</a> or <a href='mailto:dfe.support@coursedirectoryproviderportal.org.uk'>dfe.support@coursedirectoryproviderportal.org.uk</a>."),
                (int)error);
            return new UserResponse(null, errorMessage);
        }

        public async Task<ProviderResponse> GetValidatedProviderAsync(SAAuthenticationResponse claims, string userId)
        {
            var response = await GetProviderAsync(claims, userId);
            if (!String.IsNullOrEmpty(response.Message)) return response;
            if (claims.OrgStatusCode == 1 && response.Provider.RecordStatusId != (int)Constants.RecordStatus.Live)
            {
                response.Provider.RecordStatusId = (int)Constants.RecordStatus.Live;
                await db.SaveChangesAsync();
            }
            if (claims.OrgStatusCode == 2 && response.Provider.RecordStatusId != (int)Constants.RecordStatus.Deleted)
            {
                response.Provider.RecordStatusId = (int)Constants.RecordStatus.Deleted;
                await db.SaveChangesAsync();
            }
            if (response.Provider.RecordStatusId != (int)Constants.RecordStatus.Live)
            {
                AppGlobal.Log.WriteLog(String.Format(
                    "Secure Access - Failed login for '{0}' SAOrgId {1} as the Secure Access organisation is closed.",
                    claims.OrganisationName, claims.OrganisationId));
                var errorMessage = AppGlobal.Language.GetText(this, "ProviderNotAvailable",
                    "Log in failed for DfE Secure Access. Your organisation is no longer available. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href='tel:08448115028'>0844 811 5028</a> or <a href='mailto:dfe.support@coursedirectoryproviderportal.org.uk'>dfe.support@coursedirectoryproviderportal.org.uk</a>.");
                return new ProviderResponse(null, errorMessage);
            }
            return response;
        }

        private async Task<ProviderResponse> GetProviderAsync(SAAuthenticationResponse claims, string userId)
        {
            string errorMessage;
            IQueryable<Provider> providers =
                db.Providers.Where(x => x.SecureAccessId == claims.OrganisationId);
            if (providers.Count() == 1)
            {
                return new ProviderResponse(providers.First());
            }
            if (providers.Count() > 1)
            {
                errorMessage = AppGlobal.Language.GetText(this, "SecureAccessIdNotUnique",
                    "Log in failed for DfE Secure Access. We are unable to uniquely identify your organisation and cannot log you in. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href='tel:08448115028'>0844 8115 028</a> or <a href='mailto:dfe.support@coursedirectoryproviderportal.org.uk'>dfe.support@coursedirectoryproviderportal.org.uk</a>.");
                return new ProviderResponse(null, errorMessage);
            }

            bool isNewProvider = false;
            Provider provider = null;
            ProviderType providerType = db.ProviderTypes
                .FirstOrDefault(x => x.ProviderTypeName == "DfE 16-19");
            DfEProviderType dfeProviderType = db.DfEProviderTypes
                .FirstOrDefault(pt => pt.DfEProviderTypeCode == claims.TypeCode);
            DfEProviderStatu dfeProviderStatus = db.DfEProviderStatus
                .FirstOrDefault(ps => ps.DfEProviderStatusCode == claims.OrgStatusCode.ToString());
            DfELocalAuthority dfeLocalAuthority = db.DfELocalAuthorities
                .FirstOrDefault(la => la.DfELocalAuthorityCode == claims.LocalAuthority.ToString());
            DfERegion dfeRegion = db.DfERegions
                .FirstOrDefault(r => r.DfERegionCode == claims.RegionCode);
            DfEEstablishmentType dfeEstablishmentType = db.DfEEstablishmentTypes
                .FirstOrDefault(et => et.DfEEstablishmentTypeCode == claims.TypeCode);
            Ukrlp ukrlp = db.Ukrlps.FirstOrDefault(x => x.Ukprn == claims.UKPRN);

            if (claims.UKPRN == null)
            {
                errorMessage = AppGlobal.Language.GetText(this, "NoUkprn",
                    "Log in failed for DfE Secure Access. Your organisation does not have a UKPRN record which is required to access the Post 16 Provider Portal. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href='tel:08448115028'>0844 8115 028</a> or <a href='mailto:dfe.support@coursedirectoryproviderportal.org.uk'>dfe.support@coursedirectoryproviderportal.org.uk</a>.");
                AppGlobal.Log.WriteLog(String.Format(
                    "Secure Access - Failed to create new provider for '{0}' SAOrgId {1} as the provider does not have a UKPRN.",
                    claims.OrganisationName, claims.OrganisationId));
                return new ProviderResponse(null, errorMessage);
            }

            // Is this an existing LIVE provider that needs to be upgraded?
            IQueryable<Provider> candidates =
                db.Providers.Where(
                    x => x.Ukprn == claims.UKPRN && x.RecordStatusId == (int)Constants.RecordStatus.Live);
            // If there are more than one try and filter by name
            if (candidates.Count() > 1)
            {
                candidates =
                    candidates.Where(
                        x =>
                            x.ProviderName.Equals(claims.OrganisationName, StringComparison.CurrentCultureIgnoreCase)
                            ||
                            x.Ukrlp.LegalName.Equals(claims.OrganisationName, StringComparison.CurrentCultureIgnoreCase)
                            ||
                            x.Ukrlp.TradingName.Equals(claims.OrganisationName,
                                StringComparison.CurrentCultureIgnoreCase));
                if (candidates.Count() > 1)
                {
                    errorMessage = AppGlobal.Language.GetText(this, "CannotDetermineProvider",
                        "Log in failed for DfE Secure Access. We are unable to uniquely identify your organisation and cannot log you in. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href='tel:08448115028'>0844 8115 028</a> or <a href='mailto:dfe.support@coursedirectoryproviderportal.org.uk'>dfe.support@coursedirectoryproviderportal.org.uk</a>.");
                    AppGlobal.Log.WriteLog(String.Format(
                        "Secure Access - Failed to create new provider for '{0}' SAOrgId {1} UKPRN {2} as there is more than one candidate provider in the system.",
                        claims.OrganisationName, claims.OrganisationId, claims.UKPRN));
                    return new ProviderResponse(null, errorMessage);
                }
            }
            // If we found one use it
            if (candidates.Count() == 1)
            {
                provider = candidates.First();
            }
            // If not create one
            if (provider == null)
            {
                isNewProvider = true;
                provider = new Provider
                {
                    Ukprn = claims.UKPRN.HasValue ? claims.UKPRN.Value : 0,
                    RecordStatusId =
                        (int)(claims.OrgStatusCode == 1 ? Constants.RecordStatus.Live : Constants.RecordStatus.Deleted),
                    IsContractingBody = false,
                    ProviderTypeId = providerType.ProviderTypeId,
                    ProviderName = claims.OrganisationName,
                    ProviderNameAlias = null,
                    Loans24Plus = false,
                    InformationOfficerUserId = null,
                    RelationshipManagerUserId = null,
                    Telephone = claims.TelephoneNumber,
                    ProviderTrackingUrl = null,
                    VenueTrackingUrl = null,
                    CourseTrackingUrl = null,
                    BookingTrackingUrl = null,
                    SFAFunded = false,
                    QualityEmailsPaused = false,
                    QualityEmailStatusId = null,
                    Address = ukrlp == null || ukrlp.LegalAddress == null
                        ? null
                        : ukrlp.LegalAddress.Clone(),
                    CreatedDateTimeUtc = DateTime.UtcNow,
                    CreatedByUserId = userId,
                    ModifiedDateTimeUtc = DateTime.UtcNow,
                    ModifiedByUserId = userId,
                    // UPIN = ,
                    // ProviderRegionId = ,
                    // Email = ,
                    // Website = ,
                    // Fax = ,
                };
                db.Providers.Add(provider);
            }
            // Update it to be a DfE provider
            provider.DfENumber = claims.DfENumber;
            provider.DfEUrn = claims.URN;
            provider.DfEProviderTypeId = dfeProviderType == null ? (int?)null : dfeProviderType.DfEProviderTypeId;
            provider.DfEProviderStatusId =
                dfeProviderStatus == null ? (int?)null : dfeProviderStatus.DfEProviderStatusId;
            provider.DfELocalAuthorityId =
                dfeLocalAuthority == null ? (int?)null : dfeLocalAuthority.DfELocalAuthorityId;
            provider.DfERegionId = dfeRegion == null ? (int?)null : dfeRegion.DfERegionId;
            provider.DfEEstablishmentTypeId =
                dfeEstablishmentType == null ? (int?)null : dfeEstablishmentType.DfEEstablishmentTypeId;
            provider.DfEEstablishmentNumber = claims.EstablishmentNumber;
            provider.StatutoryLowestAge = claims.StatutoryLowestAge;
            provider.StatutoryHighestAge = claims.StatutoryHighestAge;
            provider.AgeRange = claims.AgeRange;
            provider.AnnualSchoolCensusLowestAge = claims.AscLowestAge;
            provider.AnnualSchoolCensusHighestAge = claims.AscHighestAge;
            provider.CompanyRegistrationNumber = claims.CompanyRegistrationNumber;
            provider.Uid = claims.UID;
            provider.SecureAccessId = claims.OrganisationId;
            provider.DFE1619Funded = true;
            provider.RecordStatusId =
                (int)(claims.OrgStatusCode == 1 ? Constants.RecordStatus.Live : Constants.RecordStatus.Deleted);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                var sb = new StringBuilder();
                foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                    {
                        sb.AppendFormat("Property: {0} Error: {1}", validationError.PropertyName,
                            validationError.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                AppGlobal.Log.WriteLog(String.Format(
                    "Secure Access - Failed to {0} provider for '{1}' SAOrgId {2} UKPRN {3} due to {4}",
                    isNewProvider ? "create a new" : "update an existing", claims.OrganisationName,
                    claims.OrganisationId, claims.UKPRN, sb));
                errorMessage = AppGlobal.Language.GetText(this, "ErrorSavingProvider",
                    "Log in failed for DfE Secure Access. An error occurred setting up your organisation as a new DFE 16-19 provider. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href='tel:08448115028'>0844 8115 028</a> or <a href='mailto:dfe.support@coursedirectoryproviderportal.org.uk'>dfe.support@coursedirectoryproviderportal.org.uk</a>.");
                return new ProviderResponse(null, errorMessage);
            }

            return new ProviderResponse(provider);
        }

        public class ProviderResponse
        {
            public ProviderResponse(Provider provider, string message = null)
            {
                Provider = provider;
                Message = message;
            }

            public Provider Provider { get; set; }
            public string Message { get; set; }
        }

        public class UserResponse
        {
            public UserResponse(AspNetUser user, string message = null)
            {
                User = user;
                Message = message;
            }

            public AspNetUser User { get; set; }
            public string Message { get; set; }
        }

        #endregion
    }
}