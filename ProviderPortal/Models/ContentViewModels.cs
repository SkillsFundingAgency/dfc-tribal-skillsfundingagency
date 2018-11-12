using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes.Content;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    /// <summary>
    /// View model item for the content list page.
    /// </summary>
    public class ContentListViewModelItem
    {
        [LanguageDisplay("Content ID")]
        public int ContentId { get; set; }

        [LanguageDisplay("Path")]
        public string Path { get; set; }

        [LanguageDisplay("Title")]
        public string Title { get; set; }

        [LanguageDisplay("User Contexts")]
        public UserContext.UserContextName UserContext { get; set; }

        [LanguageDisplay("Status")]
        public Constants.RecordStatus RecordStatus { get; set; }

        [LanguageDisplay("Embedded")]
        public bool Embed { get; set; }

        [LanguageDisplay("Last Modified")]
        public DateTime LastModifiedDateTimeUtc { get; set; }

        [LanguageDisplay("Modified By")]
        public string LastModifiedBy { get; set; }

        [LanguageDisplay("Summary")]
        public string Summary { get; set; }

        [LanguageDisplay("Language")]
        public string Language { get; set; }

        [LanguageDisplay("Version")]
        public int Version { get; set; }

        public DateTime LastModifiedDateTimeLocal {
            get { return LastModifiedDateTimeUtc.ToLocalTime(); }
        }
    }

    /// <summary>
    /// View model for the content list page.
    /// </summary>
    public class ContentListViewModel
    {
        public List<ContentListViewModelItem> Items { get; set; }
        public ContentListDisplayMode DisplayMode { get; set; }
    }

    /// <summary>
    /// View model for displaying content.
    /// </summary>
    public class ContentViewModel
    {
        /// <summary>
        /// The content itself.
        /// </summary>
        public AddEditContentViewModel Content { get; set; }
        /// <summary>
        /// The status of this content.
        /// </summary>
        public ContentStatus Status { get; set; }

        /// <summary>
        /// Where embedded content is part of the page chrome. If that content is
        /// unavailable it causes the content system to infinitely nest the add
        /// content page. For these cases this value must be set to true.
        /// </summary>
        public bool SafeEmbed { get; set; }
    }

    /// <summary>
    /// View model for adding and editing content.
    /// </summary>
    public class AddEditContentViewModel
    {
        [LanguageDisplay("Content Id")]
        public int ContentId { get; set; }

        [LanguageDisplay("Version")]
        public int Version { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Path")]
        [StringLength(1000, ErrorMessage = "The {0} must be less than {1} characters long.")]
        [Display(Description = @"Enter the URL of the page omitting any leading or trailing forward slashes. Paths must not contain any of the following characters ? & # "" & * | \ : < > . + %")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid path.")]
        [ContentPath]
        public string Path { get; set; }

        [LanguageDisplay("Title")]
        [StringLength(1000, ErrorMessage = "The {0} must be less than {1} characters long.")]
        [Display(Description = "Enter the content title as it will appear on the page. Leave this blank to hide the title.")]
        public string Title { get; set; }

        [LanguageDisplay("Body")]
        [Display(Description = "Enter the content that will appear on the page.")]
        public string Body { get; set; }

        [LanguageDisplay("Javascript")]
        [Display(Description = "Enter any Javascript that will be used on the page. There is no need to specify the <script/> tags.")]
        public string Scripts { get; set; }

        [LanguageDisplay("Stylesheet")]
        [Display(Description = "Enter any CSS styles that will be used on the page. There is no need to specify the <style/> tags.")]
        public string Styles { get; set; }

        [LanguageDisplay("Summary of Changes")]
        [StringLength(1000, ErrorMessage = "The {0} must be less than {1} characters long.")]
        [Display(Description = "Enter a summary of the changes you have made.")]
        public string Summary { get; set; }

        [LanguageDisplay("Provider")]
        [Display(Description = "This content is available when viewing the site as a provider.")]
        public bool VisibleToProvider { get; set; }

        [LanguageDisplay("Organisation")]
        [Display(Description = "This content is available when viewing the site as an origanisation.")]
        public bool VisibleToOrganisation { get; set; }

        [LanguageDisplay("Administration")]
        [Display(Description = "This content is available when viewing the site as an administrator.")]
        public bool VisibleToAdministration { get; set; }

        [LanguageDisplay("Invalid Account")]
        [Display(Description = "The content is available when viewing the site as a user whose account is not set up correctly.")]
        public bool VisibleToAuthenticatedNoAccess { get; set; }

        [LanguageDisplay("Unauthenticated")]
        [Display(Description = "The content will be available to visitors who are not logged in.")]
        public bool VisibleToUnauthenticated { get; set; }

        [LanguageDisplay("Deleted Provider")]
        [Display(Description = "The content will be available when viewing the site as a deleted provider.")]
        public bool VisibleToDeletedProvider { get; set; }

        [LanguageDisplay("Deleted Organisation")]
        [Display(Description = "The content will be available when viewing the site as a deleted provider.")]
        public bool VisibleToDeletedOrganisation { get; set; }

        [LanguageDisplay("Availablity")]
        public UserContext.UserContextName UserContext
        {
            get
            {
                return ((VisibleToProvider ? ProviderPortal.UserContext.UserContextName.Provider : 0)
                          | (VisibleToOrganisation ? ProviderPortal.UserContext.UserContextName.Organisation : 0)
                          | (VisibleToAdministration ? ProviderPortal.UserContext.UserContextName.Administration : 0)
                          | (VisibleToAuthenticatedNoAccess ? ProviderPortal.UserContext.UserContextName.AuthenticatedNoAccess : 0)
                          | (VisibleToUnauthenticated ? ProviderPortal.UserContext.UserContextName.Unauthenticated : 0)
                          | (VisibleToDeletedProvider ? ProviderPortal.UserContext.UserContextName.DeletedProvider : 0)
                          | (VisibleToDeletedOrganisation ? ProviderPortal.UserContext.UserContextName.DeletedOrganisation : 0));
            }
            set
            {
                VisibleToProvider = (value & ProviderPortal.UserContext.UserContextName.Provider) != 0;
                VisibleToOrganisation = (value & ProviderPortal.UserContext.UserContextName.Organisation) != 0;
                VisibleToAdministration = (value & ProviderPortal.UserContext.UserContextName.Administration) != 0;
                VisibleToAuthenticatedNoAccess = (value & ProviderPortal.UserContext.UserContextName.AuthenticatedNoAccess) != 0;
                VisibleToUnauthenticated = (value & ProviderPortal.UserContext.UserContextName.Unauthenticated) != 0;
                VisibleToDeletedProvider = (value & ProviderPortal.UserContext.UserContextName.DeletedProvider) != 0;
                VisibleToDeletedOrganisation = (value & ProviderPortal.UserContext.UserContextName.DeletedOrganisation) != 0;
            }
        }

        [LanguageDisplay("Embedded Content")]
        public bool Embed { get; set; }

        [LanguageDisplay("Status")]
        public int RecordStatusId { get; set; }

        [LanguageDisplay("Language")]
        public int LanguageId { get; set; }

        [LanguageDisplay("Contexts In Use")]
        public UserContext.UserContextName ContextsInUse { get; set; }

        /// <summary>
        /// The form submit action requested.
        /// </summary>
        public string SubmitAction { get; set; }
    }

    /// <summary>
    /// Current status of a content item for viewing purposes.
    /// </summary>
    public enum ContentStatus
    {
        NewPage,
        ExistingPage,
        NotFound,
        AuthenticationRequired
    }

    /// <summary>
    /// Current DisplayMode for a content list.
    /// </summary>
    public enum ContentListDisplayMode
    {
        Index,
        History,
        Archived
    }
}