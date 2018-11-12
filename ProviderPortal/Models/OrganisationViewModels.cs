using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Models;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class OrganisationDashboardViewModel
    {
        public OrganisationViewModel Organisation { get; set; }
        public List<OrganisationProviderViewModel> Providers { get; set; }

        [LanguageDisplay("Provider name")]
        public string InvitedProvider { get; set; }

        public string InvitedProviderId { get; set; }

        public OrganisationDashboardViewModel()
        {
            this.Organisation = new OrganisationViewModel();
            this.Providers = new List<OrganisationProviderViewModel>();
        }

        public OrganisationDashboardViewModel(Organisation organisation)
        {
            var statusLeft = AppGlobal.Language.GetText("OrganisationDashboardViewModel_Status_Left", "Left");
            var statusAccepted = AppGlobal.Language.GetText("OrganisationDashboardViewModel_Status_Accepted", "Accepted");
            var statusRejected = AppGlobal.Language.GetText("OrganisationDashboardViewModel_Status_Rejected", "Rejected");
            var statusPending = AppGlobal.Language.GetText("OrganisationDashboardViewModel_Status_Pending", "Pending");
            var statusDeleted = AppGlobal.Language.GetText("OrganisationDashboardViewModel_Status_Deleted", "Deleted");

            this.Organisation = new OrganisationViewModel(organisation);
            this.Providers = organisation.OrganisationProviders
                .Select(x =>
                    new OrganisationProviderViewModel
                    {
                        ProviderId = x.ProviderId,
                        UKPRN = x.Provider.Ukprn,
                        ProviderName = x.Provider.ProviderName,
                        IsRejected = x.IsRejected,
                        IsAccepted = x.IsAccepted,
                        IsProviderDeleted = x.Provider.RecordStatusId == (int) Constants.RecordStatus.Deleted,
                        CanOrganisationEditProvider = x.CanOrganisationEditProvider,
                        Reason = x.Reason,
                        Status = x.Provider.RecordStatusId == (int) Constants.RecordStatus.Deleted
                            ? statusDeleted
                            : x.IsAccepted && x.IsRejected
                                ? statusLeft
                                : x.IsAccepted
                                    ? statusAccepted
                                    : x.IsRejected
                                        ? statusRejected
                                        : statusPending,
                        PrimaryContacts = x.Provider.AspNetUsers
                            .Where(
                                y =>
                                    y.AspNetRoles.Any(
                                        z =>
                                            z.Permissions.Any(
                                                a => a.PermissionId == (int) Permission.PermissionName.IsPrimaryContact)))
                            .OrderBy(y => y.Name)
                            .Select(y => new MailAddress(y.Email, y.Name))
                            .ToList()
                    })
                .OrderBy(x => x.ProviderName)
                .ToList();
        }
    }

    public class OrganisationProviderViewModel
    {
        public Int32? ProviderId { get; set; }

        [LanguageDisplay("UKPRN")]
        public Int32? UKPRN { get; set; }

        [LanguageDisplay("Provider Name")]
        public String ProviderName { get; set; }

        [LanguageDisplay("Primary Contacts")]
        public List<MailAddress> PrimaryContacts { get; set; }

        [LanguageDisplay("Status")]
        public String Status { get; set; }

        public Boolean IsRejected { get; set; }
        public Boolean IsAccepted { get; set; }
        public Boolean IsProviderDeleted { get; set; }
        public Boolean CanOrganisationEditProvider { get; set; }

        [LanguageDisplay("Reason")]
        public String Reason { get; set; }
    }

    public class OrganisationViewModel
    {
        public Int32? OrganisationId { get; set; }
        public Int32? RecordStatusId { get; set; }
        public Int32? UKPRN { get; set; }
        public Int32? UPIN { get; set; }

        [Display(
            Description =
                "This field should be checked if this Organisation has a contract with the Skills Funding Agency to deliver provision."
            )]
        public Boolean IsContractingBody { get; set; }

        public String OrganisationName { get; set; }
        public String OrganisationAlias { get; set; }
        public AddressViewModel Address { get; set; }
        public String Email { get; set; }
        public String Website { get; set; }
        public String Telephone { get; set; }
        public String Fax { get; set; }
        public List<MailAddress> PrimaryContacts { get; set; }

        public OrganisationViewModel()
        {
            this.Address = new AddressViewModel();
        }

        public OrganisationViewModel(Organisation organisation)
        {
            this.OrganisationId = organisation.OrganisationId;
            this.UKPRN = organisation.UKPRN;
            this.UPIN = organisation.UPIN;
            this.RecordStatusId = organisation.RecordStatusId;
            //this.RecordStatus = organisation.RecordStatu.RecordStatusName;
            //this.OrganisationType = organisation.OrganisationType.OrganisationTypeName;
            this.IsContractingBody = organisation.IsContractingBody;
            this.OrganisationName = organisation.OrganisationName;
            this.OrganisationAlias = organisation.OrganisationAlias;

            this.Email = organisation.Email;
            this.Website = organisation.Website;
            this.Telephone = organisation.Phone;
            this.Fax = organisation.Fax;

            this.PrimaryContacts = organisation.AspNetUsers
                .Where(
                    y =>
                        y.AspNetRoles.Any(
                            z =>
                                z.Permissions.Any(
                                    a => a.PermissionId == (int) Permission.PermissionName.IsPrimaryContact)))
                .OrderBy(y => y.Name)
                .Select(y => new MailAddress(y.Email, y.Name))
                .ToList();

            this.Address = new AddressViewModel(organisation.Address);
        }
    }

    /// <summary>
    /// A view model representing an action that can be performed on a provider organisation membership.
    /// </summary>
    public class OrganisationMembershipActionViewModel
    {
        /// <summary>
        /// An Id related to the action.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The provider name related to the action.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The action being performed.
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// A reason associated with the action.
        /// </summary>
        [LanguageStringLength(200, ErrorMessage = "The reason must be at most 200 characters long.")]
        public string Reason { get; set; }
        /// <summary>
        /// A flag associated with the action.
        /// </summary>
        public bool? Flag { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProviderOrganisationViewModelItem
    {
        public Int32 OrganisationId { get; set; }
        [LanguageDisplay("UKPRN")]
        public Int32? Ukprn { get; set; }
        [LanguageDisplay("Name")]
        public String OrganisationName { get; set; }
        [LanguageDisplay("Primary Contacts")]
        public List<MailAddress> PrimaryContacts { get; set; }
        public Boolean IsRejected { get; set; }
        public Boolean IsAccepted { get; set; }
        [LanguageDisplay("Organisation Can View and Change Data?")]
        public Boolean CanOrganisationEditProvider { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProviderOrganisationsViewModel
    {
        public Provider Provider { get; set; }
        public List<ProviderOrganisationViewModelItem> Organisations { get; set; }
        public List<ProviderOrganisationViewModelItem> Invitations { get; set; }

        public ProviderOrganisationsViewModel(Provider provider)
        {
            this.Provider = provider;
            var temp = provider.OrganisationProviders
                .Where(x => x.IsRejected == false
                    && x.Provider.RecordStatusId == (int)Constants.RecordStatus.Live
                    && x.Organisation.RecordStatusId == (int)Constants.RecordStatus.Live)
                .Select(x =>
                new ProviderOrganisationViewModelItem
                {
                    OrganisationId = x.OrganisationId,
                    OrganisationName = x.Organisation.OrganisationName,
                    Ukprn = x.Organisation.UKPRN,
                    IsAccepted = x.IsAccepted,
                    IsRejected = x.IsRejected,
                    CanOrganisationEditProvider = x.CanOrganisationEditProvider,
                    PrimaryContacts = x.Organisation.AspNetUsers
                        .Where(
                            y =>
                                y.AspNetRoles.Any(
                                    z =>
                                        z.Permissions.Any(
                                            a => a.PermissionId == (int) Permission.PermissionName.IsPrimaryContact)))
                        .OrderBy(y => y.Name)
                        .Select(y => new MailAddress(y.Email, y.Name))
                        .ToList()
                })
                .ToList();

            this.Organisations = temp.Where(x => x.IsAccepted).ToList();
            this.Invitations = temp.Where(x => !x.IsAccepted).ToList();
        }
    }

    public class AddEditOrganisationModel
    {
        public Int32? OrganisationId { get; set; }

        [LanguageDisplay("Status")]
        public Int32? RecordStatusId { get; set; }
        // Just used for displaying the organisation details
        public String RecordStatusName { get; set; }

        [LanguageRequired]
        [DataType(DataType.Text)]
        [LanguageDisplay("UKPRN")]
        [Display(
            Description =
                "United Kingdom Provider Reference Number (UKPRN). This number is the unique identifier for this organisation from the United Kingdom Register of Learning Providers (UKRLP). For more information see http://www.ukrlp.co.uk/."
            )]
        public Int32? UKPRN { get; set; }

        [LanguageDisplay("Contracting Body")]
        [Display(
            Description =
                "This field should be checked if this Organisation has a contract with the Skills Funding Agency to deliver provision."
            )]
        public Boolean IsContractingBody { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Organisation Type")]
        [Display(Description = "Please select the most appropriate type from the list.")]
        public Int32 OrganisationTypeId { get; set; }
        // Just used for displaying the organisation details
        public String OrganisationTypeName { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Organisation Name")]
        [LanguageStringLength(100, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter the name of your organisation.")]
        public String OrganisationName { get; set; }

        [LanguageDisplay("Organisation Alias")]
        [LanguageStringLength(100, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(
            Description =
                "Please enter alternative name your organisation is known by. Learners will be able to search the Course Directory using this name and find you."
            )]
        public String OrganisationAlias { get; set; }

        [LanguageDisplay("UPIN")]
        [Display(
            Description =
                "The Unique Provider Identification Number (UPIN) is a reference number assigned by the Provider Information Management System (PIMS) to each provider contracted by the Skills Funding Agency."
            )]
        [DataType(DataType.Text)]
        public Int32? UPIN { get; set; }

        [LanguageDisplay("24+ Loans")]
        [Display(
            Description =
                "This field should be checked if the Organisation offers loans under the 24+ Learning Loans scheme.")]
        public Boolean Loans24Plus { get; set; }

        public AddressViewModel Address { get; set; }

        [LanguageDisplay("Email Address")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter the main email address for your organisation.")]
        public String Email { get; set; }

        [LanguageDisplay("Website")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter your organisation's website address.")]
        public String Website { get; set; }

        [LanguageDisplay("Telephone")]
        [LanguageStringLength(30, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter your organisation's admissions telephone number.")]
        public String Telephone { get; set; }

        [LanguageDisplay("Fax")]
        [LanguageStringLength(30, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter your organisation's fax number.")]
        public String Fax { get; set; }

        [LanguageDisplay("Pause automated quality emails")]
        public Boolean QualityEmailsPaused { get; set; }

        [LanguageDisplay("Automated quality email status")]
        public Int32? QualityEmailStatusId { get; set; }

        public UKRLPDataModel UKRLPData { get; set; }

        [LanguageDisplay("Bulk Upload Pending")]
        [Display(Description = "A bulk upload is pending.")]
        public bool BulkUploadPending { get; set; }

        public AddEditOrganisationModel()
        {
            this.Address = new AddressViewModel
            {
                HideRegion = true,
                RegionId = 0
            };
            this.UKRLPData = new UKRLPDataModel();
        }

        public AddEditOrganisationModel(Organisation organisation, Boolean includeDisplayFields = false)
        {
            this.OrganisationId = organisation.OrganisationId;
            this.UKPRN = organisation.UKPRN;
            this.UPIN = organisation.UPIN;
            this.RecordStatusId = organisation.RecordStatusId;
            this.IsContractingBody = organisation.IsContractingBody;
            this.OrganisationTypeId = organisation.OrganisationTypeId;
            this.OrganisationName = organisation.OrganisationName;
            this.OrganisationAlias = organisation.OrganisationAlias;
            this.Loans24Plus = organisation.Loans24Plus;
            this.Email = organisation.Email;
            this.Website = organisation.Website;
            this.Telephone = organisation.Phone;
            this.Fax = organisation.Fax;
            
            this.Address = new AddressViewModel(organisation.Address)
            {
                HideRegion = true
            };
            this.UKRLPData = new UKRLPDataModel();

            this.QualityEmailsPaused = organisation.QualityEmailsPaused;
            this.QualityEmailStatusId = organisation.QualityEmailStatusId;

            this.BulkUploadPending = organisation.BulkUploadPending;

            if (includeDisplayFields)
            {
                this.RecordStatusName = organisation.RecordStatu.RecordStatusName;
                this.OrganisationTypeName = organisation.OrganisationType.OrganisationTypeName;
            }
        }
    }

    public class DeleteOrganisationViewModel
    {        
        /// <summary>
        /// Gets or sets a value indicating whether this organisation has active members.
        /// </summary>
        /// <value>
        /// <c>true</c> if this organisaion has active members; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasActiveMembers { get; set; }

        /// <summary>
        /// Gets or sets the organisation identifier.
        /// </summary>
        /// <value>
        /// The organisation identifier.
        /// </value>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteOrganisationViewModel"/> class.
        /// </summary>
        public DeleteOrganisationViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteOrganisationViewModel"/> class.
        /// </summary>
        /// <param name="organisationId">The organisation identifier.</param>
        public DeleteOrganisationViewModel(int organisationId)
        {
            OrganisationId = organisationId;
        }
    }
}