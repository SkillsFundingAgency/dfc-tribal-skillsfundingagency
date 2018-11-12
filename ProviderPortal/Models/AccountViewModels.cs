using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

using Tribal.SkillsFundingAgency.ProviderPortal;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using System;
    using System.Web.Mvc;

    public class ExternalLoginConfirmationViewModel
    {
        [LanguageRequired]
        [LanguageDisplay("Email Address")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [LanguageRequired]
        public string Provider { get; set; }

        [LanguageRequired]
        [LanguageDisplay]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [LanguageDisplay("Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [LanguageRequired]
        [LanguageDisplay]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [LanguageRequired]
        [LanguageDisplay]
        [LanguageEmailAddress]
        public string Email { get; set; }

        [LanguageRequired]
        [DataType(DataType.Password)]
        [LanguageDisplay]
        public string Password { get; set; }

        [LanguageDisplay("Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class AddEditAccountViewModel
    {
        public string UserId { get; set; }

        [LanguageRequired]
        [LanguageEmailAddress]
        [LanguageDisplay]
        public string Email { get; set; }

        [LanguageRequired]
        [LanguageDisplay]
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        [LanguageDisplay("Phone number")]
        public string PhoneNumber { get; set; }

        public AddressViewModel Address { get; set; }

        [LanguageDisplay("Role")]
        public string RoleId { get; set; }

        public List<SelectListItem> Roles { get; set; }

        [LanguageDisplay("User type")]
        public int UserTypeId { get; set; }

        [LanguageDisplay]
        public List<SelectListItem> UserTypes { get; set; }

        /// <summary>
        /// User type for display purposes.
        /// </summary>
        public string UserType { get; set; }

        public List<AspNetRole> AspNetRoles { get; set; }
        
        [LanguageDisplay("Provider")]
        public string Provider { get; set; }

        [LanguageDisplay("Organisation")]
        public string Organisation { get; set; }
        
        public string ProviderId { get; set; }

        public bool CanEditRole { get; set; }

        public bool CanEditUserType { get; set; }

        public bool CanEditProviderOrganisation { get; set; }
        
        public bool EditingSelf { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSecureAccessUser { get; set; }
    }

    public class RegisterViewModel
    {
        [LanguageRequired]        
        [LanguageEmailAddress]
        [LanguageDisplay]
        public string Email { get; set; }

        [LanguageRequired]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        [LanguageDisplay]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [LanguageDisplay("Confirm password")]
        [LanguageCompare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [LanguageRequired]
        [LanguageDisplay]
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        [LanguageDisplay("Phone number")]
        public string PhoneNumber { get; set; }

        public AddressViewModel Address { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [LanguageRequired]
        [LanguageEmailAddress]
        [LanguageDisplay]
        public string Email { get; set; }

        [LanguageRequired]
        [LanguageStringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        [LanguageDisplay]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [LanguageDisplay("Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
        public bool EmailConfirmation { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [LanguageRequired]
        [LanguageEmailAddress]
        [LanguageDisplay]
        public string Email { get; set; }
    }

    public class AccountSearchViewModel
    {
        [LanguageRequired]
        public string UserId { get; set; }

        [LanguageRequired]
        [LanguageDisplay("User name or email address")]
        public string Username { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEditSecureAccessUsers { get; set; }
    }
}
