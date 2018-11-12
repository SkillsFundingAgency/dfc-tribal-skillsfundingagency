using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Tribal.SkillsFundingAgency.ProviderPortal.SingleSignOn
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Helper class to make handling authentication response claims simpler.
    /// </summary>
    public class SAAuthenticationResponse
    {
        /// <summary>
        /// Map of property names in the SA authentication response to sensible human readable names.
        /// </summary>
        private readonly Dictionary<string, string> _propertyMap = new Dictionary<string, string>
        {
            {"urn:oid:2.5.4.45", "userId"},
            {"urn:oid:0.9.2342.19200300.100.1.1", "userName"},
            {"urn:oid:2.5.4.42", "firstName"},
            {"urn:oid:2.5.4.4", "lastName"},
            {"urn:oid:1.2.840.113549.1.9.1", "email"},
            {"urn:oid:2.5.4.15", "organisationPosition"},
            {"https://sa.education.gov.uk/idp/user/userStatusCode", "userStatusCode"},
            {"https://sa.education.gov.uk/idp/user/userStatusName", "userStatusName"},
            {"https://sa.education.gov.uk/idp/user/groups", "groups"},
            {"https://sa.education.gov.uk/idp/org/organisationId", "organisationId"},
            {"urn:oid:2.5.4.10", "organisationName"},
            {"https://sa.education.gov.uk/idp/org/typeCode", "typeCode"},
            {"https://sa.education.gov.uk/idp/org/typeName", "typeName"},
            {"https://sa.education.gov.uk/idp/org/orgStatusCode", "orgStatusCode"},
            {"https://sa.education.gov.uk/idp/org/orgStatusName", "orgStatusName"},
            {"urn:oid:2.5.4.20", "telephoneNumber"},
            {"https://sa.education.gov.uk/idp/org/regionCode", "regionCode"},
            {"https://sa.education.gov.uk/idp/org/regionName", "regionName"},
            {"https://sa.education.gov.uk/idp/org/localAuthority", "localAuthority"},
            {"https://sa.education.gov.uk/idp/org/localAuthorityName", "localAuthorityName"},
            {"https://sa.education.gov.uk/idp/org/establishment/dfeNumber", "dfeNumber"},
            {"https://sa.education.gov.uk/idp/org/establishment/uRN", "uRN"},
            {"https://sa.education.gov.uk/idp/org/establishment/ascLowestAge", "ascLowestAge"},
            {"https://sa.education.gov.uk/idp/org/establishment/ascHighestAge", "ascHighestAge"},
            {"https://sa.education.gov.uk/idp/org/establishment/ageRange", "ageRange"},
            {"https://sa.education.gov.uk/idp/org/establishment/number", "establishmentNumber"},
            {"https://sa.education.gov.uk/idp/org/establishment/statutoryHighestAge", "statutoryHighestAge"},
            {"https://sa.education.gov.uk/idp/org/establishment/statutoryLowestAge", "statutoryLowestAge"},
            {"https://sa.education.gov.uk/idp/org/uKPRN", "uKPRN"},
            {"https://sa.education.gov.uk/idp/org/companyRegistrationNumber", "companyRegistrationNumber"},
            {"https://sa.education.gov.uk/idp/org/uID", "uID"},
            {"http://schemas.itfoxtec.com/ws/2014/02/identity/claims/saml2nameid", "NameId"},
            {"http://schemas.itfoxtec.com/ws/2014/02/identity/claims/saml2nameidformat", "NameIdFormat"},
            {"http://schemas.itfoxtec.com/ws/2014/02/identity/claims/saml2sessionindex", "SessionIndex"},
        };

        /// <summary>
        /// User properties from the response.
        /// </summary>
        public Dictionary<string, string> Items;

        /// <summary>
        /// Establishment age range.
        /// </summary>
        public string AgeRange
        {
            get
            {
                return Items.ContainsKey("ageRange")
                    ? Items["ageRange"]
                    : null;
            }
        }

        /// <summary>
        /// Establishment annual school census highest age.
        /// </summary>
        public int? AscHighestAge
        {
            get
            {
                return Items.ContainsKey("ascHighestAge")
                    ? Convert.ToInt32(Items["ascHighestAge"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// Establishment annual school census lowest age.
        /// </summary>
        public int? AscLowestAge
        {
            get
            {
                return Items.ContainsKey("ascLowestAge")
                    ? Convert.ToInt32(Items["ascLowestAge"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// The company registration number for regsiteration at Companies House.
        /// </summary>
        public int? CompanyRegistrationNumber
        {
            get
            {
                return Items.ContainsKey("companyRegistrationNumber")
                    ? Convert.ToInt32(Items["companyRegistrationNumber"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// Establishment DfE number.
        /// </summary>
        public int? DfENumber
        {
            get
            {
                return Items.ContainsKey("dfeNumber")
                    ? Convert.ToInt32(Items["dfeNumber"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// User email address.
        /// </summary>
        public string Email
        {
            get
            {
                return Items.ContainsKey("email")
                    ? Items["email"]
                    : null;
            }
        }

        /// <summary>
        /// Establishment number.
        /// </summary>
        public int? EstablishmentNumber
        {
            get
            {
                return Items.ContainsKey("establishmentNumber")
                    ? Convert.ToInt32(Items["establishmentNumber"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// User first name.
        /// </summary>
        public string FirstName
        {
            get
            {
                return Items.ContainsKey("firstName")
                    ? Items["firstName"]
                    : null;
            }
        }

        /// <summary>
        /// Codes of groups to which the user belongs. Note, this only includes groups associated with this application.
        /// </summary>
        public string Groups
        {
            get
            {
                return Items.ContainsKey("groups")
                    ? Items["groups"]
                    : null;
            }
        }

        /// <summary>
        /// User last name.
        /// </summary>
        public string LastName
        {
            get
            {
                return Items.ContainsKey("lastName")
                    ? Items["lastName"]
                    : null;
            }
        }

        /// <summary>
        /// Establishment local authority code.
        /// </summary>
        public int? LocalAuthority
        {
            get
            {
                return Items.ContainsKey("localAuthority")
                    ? Convert.ToInt32(Items["localAuthority"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// Establishment local authority name.
        /// </summary>
        public string LocalAuthorityName
        {
            get
            {
                return Items.ContainsKey("localAuthorityName")
                    ? Items["localAuthorityName"]
                    : null;
            }
        }

        /// <summary>
        /// SAML2 claim.
        /// </summary>
        public string NameId
        {
            get
            {
                return Items.ContainsKey("NameId")
                    ? Items["NameId"]
                    : null;
            }
        }

        /// <summary>
        /// SAML2 claim.
        /// </summary>
        public string NameIdFormat
        {
            get
            {
                return Items.ContainsKey("NameIdFormat")
                    ? Items["NameIdFormat"]
                    : null;
            }
        }

        /// <summary>
        /// Secure Access origanisation Id.
        /// </summary>
        public int? OrganisationId
        {
            get
            {
                return Items.ContainsKey("organisationId")
                    ? Convert.ToInt32(Items["organisationId"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// Organisation name.
        /// </summary>
        public string OrganisationName
        {
            get
            {
                return Items.ContainsKey("organisationName")
                    ? Items["organisationName"]
                    : null;
            }
        }

        /// <summary>
        /// User organisation position.
        /// </summary>
        public string OrganisationPosition
        {
            get
            {
                return Items.ContainsKey("organisationPosition")
                    ? Items["organisationPosition"]
                    : null;
            }
        }

        /// <summary>
        /// Organisation status code.
        /// </summary>
        public int? OrgStatusCode
        {
            get
            {
                return Items.ContainsKey("orgStatusCode")
                    ? Convert.ToInt32(Items["orgStatusCode"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// Organisation status name.
        /// </summary>
        public string OrgStatusName
        {
            get
            {
                return Items.ContainsKey("orgStatusName")
                    ? Items["orgStatusName"]
                    : null;
            }
        }

        /// <summary>
        /// Organisation region code.
        /// </summary>
        public string RegionCode
        {
            get
            {
                return Items.ContainsKey("regionCode")
                    ? Items["regionCode"]
                    : null;
            }
        }

        /// <summary>
        /// Organisation region name.
        /// </summary>
        public string RegionName
        {
            get
            {
                return Items.ContainsKey("regionName")
                    ? Items["regionName"]
                    : null;
            }
        }

        /// <summary>
        /// SAML2 claim.
        /// </summary>
        public string SessionIndex
        {
            get
            {
                return Items.ContainsKey("SessionIndex")
                    ? Items["SessionIndex"]
                    : null;
            }
        }

        /// <summary>
        /// Establishment statutory highest age.
        /// </summary>
        public int? StatutoryHighestAge
        {
            get
            {
                return Items.ContainsKey("statutoryHighestAge")
                    ? Convert.ToInt32(Items["statutoryHighestAge"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// Establishment statutory lowest age.
        /// </summary>
        public int? StatutoryLowestAge
        {
            get
            {
                return Items.ContainsKey("statutoryLowestAge")
                    ? Convert.ToInt32(Items["statutoryLowestAge"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// Organisation telephone number.
        /// </summary>
        public string TelephoneNumber
        {
            get
            {
                return Items.ContainsKey("telephoneNumber")
                    ? Items["telephoneNumber"]
                    : null;
            }
        }

        /// <summary>
        /// Organisation type code.
        /// </summary>
        public string TypeCode
        {
            get
            {
                return Items.ContainsKey("typeCode")
                    ? Items["typeCode"]
                    : null;
            }
        }

        /// <summary>
        /// Organisation type name.
        /// </summary>
        public string TypeName
        {
            get
            {
                return Items.ContainsKey("typeName")
                    ? Items["typeName"]
                    : null;
            }
        }
        
        /// <summary>
        /// Organisation ???.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int? UID
        {
            get
            {
                return Items.ContainsKey("uID")
                    ? Convert.ToInt32(Items["uID"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// Organisation UKPRN as listed by the UKRLP.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int? UKPRN
        {
            get
            {
                return Items.ContainsKey("uKPRN")
                    ? Convert.ToInt32(Items["uKPRN"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// Establishment URN.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int? URN
        {
            get
            {
                return Items.ContainsKey("uRN")
                    ? Convert.ToInt32(Items["uRN"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// Secure Access user Id.
        /// </summary>
        public int? UserId
        {
            get
            {
                return Items.ContainsKey("userId")
                    ? Convert.ToInt32(Items["userId"])
                    : (int?)null;
            }
        }

        /// <summary>
        /// User name.
        /// </summary>
        public string UserName
        {
            get
            {
                return Items.ContainsKey("userName")
                    ? Items["userName"]
                    : null;
            }
        }

        /// <summary>
        /// User status code.
        /// </summary>
        public string UserStatusCode
        {
            get
            {
                return Items.ContainsKey("userStatusCode")
                    ? Items["userStatusCode"]
                    : null;
            }
        }

        /// <summary>
        /// User status name.
        /// </summary>
        public string UserStatusName
        {
            get
            {
                return Items.ContainsKey("userStatusName")
                    ? Items["userStatusName"]
                    : null;
            }
        }

        /// <summary>
        /// Create a new instance of the SAAuthenticationResponse class.
        /// </summary>
        /// <param name="claims">The claims.</param>
        public SAAuthenticationResponse(IEnumerable<Claim> claims)
        {
            Items = new Dictionary<string, string>();
            foreach (var item in claims)
            {
                if (_propertyMap.ContainsKey(item.Type))
                {
                    var key = _propertyMap[item.Type];
                    var value = item.Value;
                    Items[key] = value;
                }
            }
        }
    }
}