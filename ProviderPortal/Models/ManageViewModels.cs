using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using TribalTechnology.InformationManagement.Web.UI.Text;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [LanguageRequired]
        [LanguageStringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        [LanguageDisplay("New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [LanguageDisplay("Confirm new password")]
        [LanguageCompare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [LanguageRequired]
        [DataType(DataType.Password)]
        [LanguageDisplay("Old Password")]
        public string OldPassword { get; set; }

        [LanguageRequired]
        [LanguageStringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        [LanguageDisplay("New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [LanguageDisplay("Confirm new password")]
        [LanguageCompare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [LanguageRequired]
        [LanguagePhone]
        [LanguageDisplay("Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [LanguageRequired]
        [LanguageDisplay]
        public string Code { get; set; }

        [LanguageRequired]
        [LanguagePhone]
        [LanguageDisplay("Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    public class ManageUsersViewModelItem
    {
        /// <summary>
        /// The user's ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The user's email address
        /// </summary>
        [LanguageDisplay("Email")]
        public string Email { get; set; }

        /// <summary>
        /// The user's current status.
        /// </summary>
        [LanguageDisplay("Status")]
        public string Status { get; set; }

        /// <summary>
        /// The UKPRN associated with the user.
        /// </summary>
        [LanguageDisplay("UKPRN")]
        public string Ukprn { get; set; }

        /// <summary>
        /// The provider or organisation name.
        /// </summary>
        [LanguageDisplay("Provider/Organisation Name")]
        public string ProviderName { get; set; }

        /// <summary>
        /// The user's display name.
        /// </summary>
        [LanguageDisplay("User Name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// The user's role.
        /// </summary>
        [LanguageDisplay("Role")]
        public string Role { get; set; }

        /// <summary>
        /// The last log in date.
        /// </summary>
        [LanguageDisplay("Last Log In")]
        [DataType(DataType.DateTime)]
        [DateDisplayFormat(Format = DateFormat.ShortDateTime)]
        public DateTime? LastLogInDate { get; set; }

        [LanguageDisplay("Last Log In")]
        [DataType(DataType.DateTime)]
        [DateDisplayFormat(Format = DateFormat.ShortDateTime)]
        public DateTime? LastLogInDateLocalTime
        {
            get
            {
                if (LastLogInDate.HasValue)
                {
                    return LastLogInDate.Value.ToLocalTime();
                }

                return null;
            }
        }

        /// <summary>
        /// Whether the user accesses the portal from DfE Secure Access.
        /// </summary>
        [LanguageDisplay("DfE Secure Access User")]
        public bool IsSecureAccessUser { get; set; }
    }

    public enum UserCategory
    {
        All = 1,
        Active = 2,
        Pending = 3,
        Deleted = 4
    }

    public class ManageUsersViewModel
    {
        public bool CanAdd { get; set; }
        public bool CanEditSecureAccessUsers { get; set; }
        public bool DeferredLoad { get; set; }
        public UserCategory Category { get; set; }
        public IEnumerable<ManageUsersViewModelItem> Users { get; set; }

        public ManageUsersViewModel(string category)
        {
            category = category == null ? "" : category.ToLower(CultureInfo.CurrentCulture);
            switch (category)
            {
                case "all":
                    Category = UserCategory.All;
                    break;
                case "":
                case "active":
                    Category = UserCategory.Active;
                    break;
                case "pending":
                    Category = UserCategory.Pending;
                    break;
                case "deleted":
                    Category = UserCategory.Deleted;
                    break;
                default:
                    Category = UserCategory.Active;
                    break;
            }
        }
    }
}