using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.CacheManagement;
using Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Models;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Controllers
{
    public class TypeaheadController : BaseController
    {
        /// <summary>
        ///     Get a list users for the current context.
        /// </summary>
        /// <returns>Json formatted results.</returns>
        // GET Api/Typeahead/Users/id
        public NewtonsoftJsonResult Users()
        {
            IQueryable<AspNetUser> aspNetUsers = new List<AspNetUser>().AsQueryable();
            switch (userContext.ContextName)
            {
                case UserContext.UserContextName.Provider:

                    if (Permission.HasPermission(false, true,
                        Permission.PermissionName.CanViewProviderUsers))
                    {
                        aspNetUsers = db.Providers
                            .Where(x => x.ProviderId == (int) userContext.ItemId)
                            .SelectMany(x => x.AspNetUsers);
                    }
                    break;

                case UserContext.UserContextName.Organisation:

                    if (Permission.HasPermission(false, true,
                        Permission.PermissionName.CanViewOrganisationUsers))
                    {
                        aspNetUsers = db.Organisations
                            .Where(x => x.OrganisationId == (int) userContext.ItemId)
                            .SelectMany(x => x.AspNetUsers);
                    }
                    break;

                case UserContext.UserContextName.Administration:

                    if (Permission.HasPermission(false, true,
                        Permission.PermissionName.CanViewAdminUsers))
                    {
                        aspNetUsers = db.AspNetUsers;
                    }
                    break;
            }

            var result = new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = aspNetUsers.Select(x => new TypeaheadUserResult
                {
                    //id = x.Id,
                    email = x.UserName ?? string.Empty,
                    name = x.Name ?? string.Empty,
                    phone = x.PhoneNumber ?? string.Empty
                }).OrderBy(x => x.name).ToArray()
            };

            return result;
        }

        /// <summary>
        ///     Get a list of <c ref="CourseLanguages" />.
        /// </summary>
        /// <returns>Json formatted results.</returns>
        // GET Api/Typeahead/CourseLanguages
        public NewtonsoftJsonResult CourseLanguages()
        {
            const string cacheKey = "Typeahead:CourseLanguages";
            var result = (NewtonsoftJsonResult) CacheHandler.Get(cacheKey);
            if (result == null)
            {
                result = new NewtonsoftJsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = db.CourseLanguages.OrderBy(x => x.DisplayOrder).Select(x =>
                        new TypeAheadCourseLanguageResult
                        {
                            Language = x.Language
                        }).ToArray()
                };
                CacheHandler.Add(cacheKey, result, new TimeSpan(0, 5, 0));
            }
            return result;
        }

        /// <summary>
        ///     Get a list of <c ref="QualficationTitles" />.
        /// </summary>
        /// <returns>Json formatted results.</returns>
        // GET Api/Typeahead/QualificationTitles
        public NewtonsoftJsonResult QualificationTitles()
        {
            const string cacheKey = "Typeahead:QualificationTitles";
            var result = (NewtonsoftJsonResult) CacheHandler.Get(cacheKey);
            if (result == null)
            {
                result = new NewtonsoftJsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = db.QualificationTitles.Select(x =>
                        new TypeAheadQualificationTitleResult
                        {
                            QualificationTitle = x.QualficationTitle
                        }).ToArray()
                };
                CacheHandler.Add(cacheKey, result, new TimeSpan(0, 5, 0));
            }
            return result;
        }

        /// <summary>
        ///     Get a list of <c ref="LearningAims" />.
        /// </summary>
        /// <returns>Json formatted results.</returns>
        // GET Api/Typeahead/LearningAims
        public NewtonsoftJsonResult LearningAims(string query)
        {
            var result = new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data =
                    db.LearningAims.Where(x => x.RecordStatusId == (int) Constants.RecordStatus.Live)
                        .Where(
                            x =>
                                x.LearningAimRefId.StartsWith(query) || x.LearningAimTitle.Contains(query) ||
                                x.Qualification.Contains(query)).Select(x =>
                                    new TypeAheadLearningAimResult
                                    {
                                        LearningAimRefId = x.LearningAimRefId,
                                        LearningAimTitle = x.LearningAimTitle,
                                        Qualification = x.Qualification
                                    }).ToArray()
            };
            return result;
        }

        /// <summary>
        ///     Get a list of <c ref="LearningAimAwardOrgs" />.
        /// </summary>
        /// <returns>Json formatted results.</returns>
        // GET Api/Typeahead/AwardingOrganisations
        public NewtonsoftJsonResult AwardingOrganisations()
        {
            const string cacheKey = "Typeahead:AwardingOrgnisations";
            var result = (NewtonsoftJsonResult) CacheHandler.Get(cacheKey);
            if (result == null)
            {
                result = new NewtonsoftJsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = db.LearningAimAwardOrgs.Select(x =>
                        new TypeAheadAwardingOrganisationResult
                        {
                            LearningAimAwardOrgCode = x.LearningAimAwardOrgCode,
                            AwardOrgName = x.AwardOrgName
                        }).ToArray()
                };
                CacheHandler.Add(cacheKey, result, new TimeSpan(0, 5, 0));
            }
            return result;
        }

        /// <summary>
        ///     Get a list of <c ref="VenueLocations" />.
        /// </summary>
        /// <returns>Json formatted results.</returns>
        // GET Api/Typeahead/VenueLocations
        public NewtonsoftJsonResult VenueLocations()
        {
            const string cacheKey = "Typeahead:VenueLocations";
            var result = (NewtonsoftJsonResult) CacheHandler.Get(cacheKey);
            if (result == null)
            {
                result = new NewtonsoftJsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = db.VenueLocations.Select(x =>
                        new TypeAheadVenueLocationResult
                        {
                            VenueLocationId = x.VenueLocationId,
                            LocationName = x.LocationName,
                            ParentVenueLocation = x.ParentVenueLocation.LocationName
                        })
                        .OrderBy(
                            x =>
                                !x.ParentVenueLocation.Equals("UNITED KINGDOM",
                                    StringComparison.CurrentCultureIgnoreCase))
                        .ThenBy(x => x.LocationName)
                        .ThenBy(x => x.ParentVenueLocation)
                        .ToArray()
                };
                CacheHandler.Add(cacheKey, result, new TimeSpan(0, 5, 0));
            }
            return result;
        }

        /// <summary>
        ///     Get a list of <c ref="LearnDirectClassifications" />.
        /// </summary>
        /// <returns>Json formatted results.</returns>
        // GET Api/Typeahead/LearnDirectClassifications
        public NewtonsoftJsonResult LearnDirectClassifications()
        {
            const string cacheKey = "Typeahead:LearnDirectClassifications";
            var result = (NewtonsoftJsonResult) CacheHandler.Get(cacheKey);
            if (result == null)
            {
                result = new NewtonsoftJsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = db.LearnDirectClassifications.Select(x =>
                        new TypeAheadLearnDirectClassificationResult
                        {
                            LearnDirectClassificationRef = x.LearnDirectClassificationRef,
                            LearnDirectClassSystemCodeDesc =
                                x.LearnDirectClassSystemCodeDesc + " (" + x.LearnDirectClassificationRef + ")"
                        }).ToArray()
                };
                CacheHandler.Add(cacheKey, result, new TimeSpan(0, 5, 0));
            }
            return result;
        }

        /// <summary>
        ///     Get a list of <c ref="StandardsAndFrameworks" />.
        /// </summary>
        /// <returns>Json formatted results.</returns>
        // GET Api/Typeahead/StandardsAndFrameworks
        public NewtonsoftJsonResult StandardsAndFrameworks()
        {
            const string cacheKey = "Typeahead:StandardsAndFrameworks";
            var result = (NewtonsoftJsonResult) CacheHandler.Get(cacheKey);
            if (result == null)
            {
                var frameworks =
                    db.Frameworks.Where(x => x.RecordStatusId == (int) Constants.RecordStatus.Live)
                        .ToList()
                        .Select(x => new
                        {
                            Id = "F" + x.FrameworkCode + "-" + x.ProgType + "-" + x.PathwayCode,
                            Name = x.Details(),
                            Url = (string) null
                        }).ToList();
                var standards =
                    db.Standards.Where(x => x.RecordStatusId == (int) Constants.RecordStatus.Live)
                        .ToList()
                        .Select(x => new
                        {
                            Id = "S" + x.StandardCode + "-" + x.Version,
                            Name = x.Details(),
                            Url = x.URLLink
                        });

                result = new NewtonsoftJsonResult
                {
                    // [FrameworkCode], [ProgType], [PathwayCode]
                    // [StandardCode], [Version]
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = frameworks.Union(standards).OrderBy(x => x.Name)
                };
                CacheHandler.Add(cacheKey, result, new TimeSpan(0, 5, 0));
            }
            return result;
        }

        #region Providers and organisations

        /// <summary>
        ///     Regular expression to match and decode provider and organisation IDs passed back from typeahead.
        /// </summary>
        private static readonly Regex DecodeProviderRegex = new Regex("(?<Type>P|O)(?<Id>[0-9]+)", RegexOptions.Compiled);

        /// <summary>
        ///     Get a list of <c ref="Providers" />.
        /// </summary>
        /// <returns>Json formatted results.</returns>
        // GET Api/Typeahead/Providers
        public NewtonsoftJsonResult Providers()
        {
            var showDeleted = UserContext.GetUserContext().IsAdministration();
            var result = new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = db.Providers
                    .Where(x => x.RecordStatu.IsPublished || showDeleted)
                    .Select(x =>
                        new TypeAheadProviderResult
                        {
                            id = "P" + x.ProviderId,
                            ukprn = x.Ukprn,
                            name = x.ProviderName ?? string.Empty,
                            alias = x.ProviderNameAlias ?? string.Empty,
                            town = x.Address == null ? string.Empty : x.Address.Town ?? string.Empty,
                            county = x.Address == null ? string.Empty : x.Address.County ?? string.Empty,
                            postcode = x.Address == null ? string.Empty : x.Address.Postcode ?? string.Empty,
                            deleted = x.RecordStatu.IsDeleted
                        }).OrderBy(x => x.name).ToArray()
            };

            return result;
        }

        /// <summary>
        ///     Get a list of <c ref="Providers" /> and <c ref="Organisations" />.
        /// </summary>
        /// <returns>Json formatted results.</returns>
        // GET Api/Typeahead/Providers
        public NewtonsoftJsonResult ProvidersAndOrganisations()
        {
            var showDeleted = UserContext.GetUserContext().IsAdministration();
            var result = new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = db.Providers
                    .Where(x => x.RecordStatu.IsPublished || showDeleted)
                    .Select(x =>
                        new TypeAheadProviderResult
                        {
                            id = "P" + x.ProviderId,
                            ukprn = x.Ukprn,
                            name = x.ProviderName ?? string.Empty,
                            alias = x.ProviderNameAlias ?? string.Empty,
                            town = x.Address == null ? string.Empty : x.Address.Town ?? string.Empty,
                            county = x.Address == null ? string.Empty : x.Address.County ?? string.Empty,
                            postcode = x.Address == null ? string.Empty : x.Address.Postcode ?? string.Empty,
                            deleted = x.RecordStatu.IsDeleted
                        })
                    .Union(db.Organisations
                        .Where(x => x.RecordStatu.IsPublished || showDeleted)
                        .Select(x =>
                            new TypeAheadProviderResult
                            {
                                id = "O" + x.OrganisationId,
                                ukprn = (int) x.UKPRN,
                                name = x.OrganisationName ?? string.Empty,
                                alias = x.OrganisationAlias ?? string.Empty,
                                town = x.Address == null ? string.Empty : x.Address.Town ?? string.Empty,
                                county = x.Address == null ? string.Empty : x.Address.County ?? string.Empty,
                                postcode = x.Address == null ? string.Empty : x.Address.Postcode ?? string.Empty,
                                deleted = x.RecordStatu.IsDeleted
                            })).OrderBy(x => x.name).ToArray()
            };

            return result;
        }

        /// <summary>
        ///     Get a list of <c ref="Organisations" />.
        /// </summary>
        /// <returns>Json formatted results.</returns>
        // GET Api/Typeahead/Organisations
        public NewtonsoftJsonResult Organisations()
        {
            var showDeleted = UserContext.GetUserContext().IsAdministration();
            var result = new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = db.Organisations
                    .Where(x => x.RecordStatu.IsPublished || showDeleted)
                    .Select(x =>
                        new TypeAheadProviderResult
                        {
                            id = "O" + x.OrganisationId,
                            ukprn = (int) x.UKPRN,
                            name = x.OrganisationName ?? string.Empty,
                            alias = x.OrganisationAlias ?? string.Empty,
                            town = x.Address == null ? string.Empty : x.Address.Town ?? string.Empty,
                            county = x.Address == null ? string.Empty : x.Address.County ?? string.Empty,
                            postcode = x.Address == null ? string.Empty : x.Address.Postcode ?? string.Empty,
                            deleted = x.RecordStatu.IsDeleted
                        }).OrderBy(x => x.name).ToArray()
            };

            return result;
        }

        [NonAction]
        public static UserContext.UserContextInfo DecodeProviderId(string providerId)
        {
            var m = DecodeProviderRegex.Match(providerId);
            return !m.Success
                ? null
                : new UserContext.UserContextInfo(
                    m.Groups["Type"].Value == "P"
                        ? UserContext.UserContextName.Provider
                        : UserContext.UserContextName.Organisation,
                    int.Parse(m.Groups["Id"].Value));
        }

        [NonAction]
        public bool FindProviderOrOrganisationByName(string name, out string value)
        {
            var provider = db.Providers.FirstOrDefault(x => x.ProviderName == name);
            if (provider != null)
            {
                value = "P" + provider.ProviderId;
                return true;
            }
            var organisation = db.Organisations.FirstOrDefault(x => x.OrganisationName == name);
            if (organisation != null)
            {
                value = "O" + organisation.OrganisationId;
                return true;
            }
            value = null;
            return false;
        }

        #endregion
    }
}