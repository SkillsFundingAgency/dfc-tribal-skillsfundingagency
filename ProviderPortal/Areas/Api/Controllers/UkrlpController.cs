using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Models;
using Tribal.SkillsFundingAgency.ProviderPortal.uk.co.ukrlp.webservices;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Controllers
{
    public class UkrlpController : BaseController
    {
        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanAddProvider, Permission.PermissionName.CanEditProvider,
            Permission.PermissionName.CanAddOrganisation, Permission.PermissionName.CanEditOrganisation)]
        public NewtonsoftJsonResult GetUkprn(int ukprn)
        {
            return Json(GetUkrlpData(ukprn, false));
        }

        [System.Web.Http.NonAction]
        private UKRLPDataModel GetUkrlpData(int ukprn, bool activeOnly)
        {
            var ukrlpModel = new UKRLPDataModel
            {
                UKRLP = ""
            };

            try
            {
                var ukrlpWebService = new ProviderQueryServiceV3
                {
                    Url = ConfigurationManager.AppSettings["UKRLP_Url"]
                };

                var scs = new SelectionCriteriaStructure
                {
                    StakeholderId = ConfigurationManager.AppSettings["UKRLP_Stakeholder_Id"],
                    UnitedKingdomProviderReferenceNumberList = new[] {ukprn.ToString()},
                    ApprovedProvidersOnly = YesNoType.No,
                    ApprovedProvidersOnlySpecified = true,
                    ProviderStatus = "A",
                    CriteriaCondition = QueryCriteriaConditionType.OR,
                    CriteriaConditionSpecified = true
                };

                var pqs = new ProviderQueryStructure
                {
                    SelectionCriteria = scs,
                    QueryId = "1"
                };

                string[] statuses = {"A", "V", "PD1", "PD2"};
                if (activeOnly)
                {
                    statuses = new[] {"A", "V"};
                }

                foreach (var status in statuses)
                {
                    scs.ProviderStatus = status;

                    var r = ukrlpWebService.retrieveAllProviders(pqs);
                    if (r.MatchingProviderRecords != null)
                    {
                        foreach (var provider in r.MatchingProviderRecords)
                        {
                            switch (status)
                            {
                                case "A":
                                    ukrlpModel.Status = AppGlobal.Language.GetText(this, "StatusActive", "Active");
                                    break;
                                case "V":
                                    ukrlpModel.Status = AppGlobal.Language.GetText(this, "StatusVerified", "Verified");
                                    break;
                                case "PD1":
                                    ukrlpModel.Status = AppGlobal.Language.GetText(this, "StatusDeactiviationInProcess",
                                        "Provider deactivated, not verified");
                                    break;
                                case "PD2":
                                    ukrlpModel.Status = AppGlobal.Language.GetText(this, "StatusDeactivationComplete",
                                        "Provider deactivated");
                                    break;
                            }
                            ukrlpModel.UKRLP = provider.UnitedKingdomProviderReferenceNumber;
                            ukrlpModel.LegalName = provider.ProviderName;
                            ukrlpModel.TradingName = provider.ProviderAliases == null || provider.ProviderAliases.Count() == 0 || provider.ProviderAliases[0].ProviderAlias == null ? "" : provider.ProviderAliases[0].ProviderAlias.Substring(0, Math.Min(provider.ProviderAliases[0].ProviderAlias.Length, 255)) ?? "";
                            foreach (var pcs in provider.ProviderContact)
                            {
                                switch (pcs.ContactType)
                                {
                                    case "L": // Legal
                                        ukrlpModel.LegalTelephone = pcs.ContactTelephone1 ?? "";
                                        ukrlpModel.LegalFax = pcs.ContactFax ?? "";
                                        if (pcs.ContactAddress != null)
                                        {
                                            if (!string.IsNullOrEmpty(pcs.ContactAddress.PAON.Description))
                                            {
                                                ukrlpModel.LegalFullAddress += pcs.ContactAddress.PAON.Description + " ";
                                                ukrlpModel.LegalAddress1 = pcs.ContactAddress.PAON.Description + " ";
                                            }

                                            if (!string.IsNullOrEmpty(pcs.ContactAddress.SAON.Description))
                                            {
                                                ukrlpModel.LegalFullAddress += pcs.ContactAddress.SAON.Description + " ";
                                                ukrlpModel.LegalAddress1 += pcs.ContactAddress.SAON.Description + " ";
                                            }
                                            if (!string.IsNullOrEmpty(ukrlpModel.LegalFullAddress))
                                            {
                                                ukrlpModel.LegalFullAddress += "<br />";
                                            }
                                            ukrlpModel.LegalFullAddress += pcs.ContactAddress.StreetDescription +
                                                                           "<br />";
                                            ukrlpModel.LegalAddress1 += pcs.ContactAddress.StreetDescription;
                                            if (!string.IsNullOrEmpty(pcs.ContactAddress.Locality))
                                            {
                                                ukrlpModel.LegalFullAddress += pcs.ContactAddress.Locality + "<br />";
                                                ukrlpModel.LegalAddress2 = pcs.ContactAddress.Locality;
                                            }
                                            ukrlpModel.LegalFullAddress += pcs.ContactAddress.Items != null &&
                                                                           pcs.ContactAddress.Items.GetLength(0) > 0
                                                ? pcs.ContactAddress.Items[0] + "<br>"
                                                : "";
                                            ukrlpModel.LegalTown += pcs.ContactAddress.Items != null &&
                                                                    pcs.ContactAddress.Items.GetLength(0) > 0
                                                ? pcs.ContactAddress.Items[0] + "<br>"
                                                : "";
                                            ukrlpModel.LegalFullAddress += pcs.ContactAddress.PostCode + "<br>";
                                            ukrlpModel.LegalPostcode += pcs.ContactAddress.PostCode + "<br>";
                                        }
                                        break;

                                    case "P": // Primary
                                        ukrlpModel.ContactTelephone = pcs.ContactTelephone1 ?? "";
                                        ukrlpModel.ContactFax = pcs.ContactFax ?? "";
                                        ukrlpModel.ContactName =
                                            string.Format("{0} {1} {2}",
                                                pcs.ContactPersonalDetails.PersonNameTitle == null
                                                    ? ""
                                                    : pcs.ContactPersonalDetails.PersonNameTitle[0],
                                                pcs.ContactPersonalDetails.PersonGivenName == null
                                                    ? ""
                                                    : pcs.ContactPersonalDetails.PersonGivenName[0],
                                                pcs.ContactPersonalDetails.PersonFamilyName).Replace("  ", " ").Trim();
                                        if (pcs.ContactAddress != null)
                                        {
                                            if (!string.IsNullOrEmpty(pcs.ContactAddress.PAON.Description))
                                            {
                                                ukrlpModel.ContactFullAddress += pcs.ContactAddress.PAON.Description +
                                                                                 " ";
                                                ukrlpModel.ContactAddress1 = pcs.ContactAddress.PAON.Description + " ";
                                            }

                                            if (!string.IsNullOrEmpty(pcs.ContactAddress.SAON.Description))
                                            {
                                                ukrlpModel.ContactFullAddress += pcs.ContactAddress.SAON.Description +
                                                                                 " ";
                                                ukrlpModel.ContactAddress1 += pcs.ContactAddress.SAON.Description + " ";
                                            }
                                            if (!string.IsNullOrEmpty(ukrlpModel.ContactFullAddress))
                                            {
                                                ukrlpModel.ContactFullAddress += "<br />";
                                            }
                                            ukrlpModel.ContactFullAddress += pcs.ContactAddress.StreetDescription +
                                                                             "<br />";
                                            ukrlpModel.ContactAddress1 += pcs.ContactAddress.StreetDescription;
                                            if (!string.IsNullOrEmpty(pcs.ContactAddress.Locality))
                                            {
                                                ukrlpModel.ContactFullAddress += pcs.ContactAddress.Locality + "<br />";
                                                ukrlpModel.ContactAddress2 = pcs.ContactAddress.Locality;
                                            }
                                            ukrlpModel.ContactFullAddress += pcs.ContactAddress.Items != null &&
                                                                             pcs.ContactAddress.Items.GetLength(0) > 0
                                                ? pcs.ContactAddress.Items[0] + "<br>"
                                                : "";
                                            ukrlpModel.ContactTown += pcs.ContactAddress.Items != null &&
                                                                      pcs.ContactAddress.Items.GetLength(0) > 0
                                                ? pcs.ContactAddress.Items[0] + "<br>"
                                                : "";
                                            ukrlpModel.ContactFullAddress += pcs.ContactAddress.PostCode + "<br>";
                                            ukrlpModel.ContactPostcode += pcs.ContactAddress.PostCode + "<br>";
                                        }
                                        break;
                                }
                            }

                            foreach (var vds in provider.VerificationDetails)
                            {
                                switch (vds.VerificationAuthority.ToLower())
                                {
                                    case "companies house":
                                        ukrlpModel.CompanyRegistrationNumber = vds.VerificationID ?? "";
                                        break;
                                    case "charity commission":
                                        ukrlpModel.CharityRegistrationNumber = vds.VerificationID ?? "";
                                        break;
                                }
                            }

                            // Check if UKPRN is already in use
                            var providerId = 0;
                            var uc = UserContext.GetUserContext();
                            if (uc.IsProvider())
                            {
                                providerId = uc.ItemId.Value;
                            }
                            var dbProvider =
                                db.Providers.FirstOrDefault(x => x.Ukprn == ukprn && x.ProviderId != providerId);
                            if (dbProvider != null)
                            {
                                ukrlpModel.InUse = true;
                            }

                            // Check if UKPRN is in use at an organisation
                            if (!ukrlpModel.InUse)
                            {
                                var orgId = 0;
                                if (uc.IsOrganisation())
                                {
                                    orgId = uc.ItemId.Value;
                                }
                                var dbOrganisation =
                                    db.Organisations.FirstOrDefault(x => x.UKPRN == ukprn && x.OrganisationId != orgId);
                                if (dbOrganisation != null)
                                {
                                    ukrlpModel.InUse = true;
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(ukrlpModel.UKRLP))
                    {
                        break;
                    }
                }
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }

            return ukrlpModel;
        }
    }
}