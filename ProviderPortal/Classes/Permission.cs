// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Permission.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Checks the permission and allows access to the controller where the user has at least one permission in the supplied list
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data.Entity;
using System.Web.UI;
// ReSharper disable once CheckNamespace


namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

    /// <summary>
    /// Manages and checks permission for the current logged in user
    /// </summary>
    public class Permission
    {
        #region Public Enum - PermissionName
        /// <summary>
        /// A permission to check
        /// </summary>
        public enum PermissionName
        {
            /// <summary>
            /// Not used as a permission, required to ensure any failed parse results in a permission that doesn't apply any rights
            /// </summary>
            None = 0,
            
            /// <summary>
            /// With this permission a user can view the standard home page
            /// </summary>
            CanViewHomePage = 1,

            /// <summary>
            /// With this permission a user can view the administrator home page
            /// </summary>
            CanViewAdministratorHomePage = 2,

            /// <summary>
            /// With this permission a user can view the provider home page
            /// </summary>
            CanViewProviderHomePage = 3,

            /// <summary>
            /// With this permission a user can view the organisation home page
            /// </summary>
            CanViewOrganisationHomePage = 4,

            /// <summary>
            /// With this permission a user may add or edit roles and assign or remove permissions from roles
            /// </summary>
            CanAddEditRoles = 5,

            /// <summary>
            /// With this permission a user may add a new provider
            /// </summary>
            CanAddProvider = 6,

            /// <summary>
            /// With this permission a user may edit a provider
            /// </summary>
            CanEditProvider = 7,

            /// <summary>
            /// With this permission a user can add and edit user accounts at the system level
            /// </summary>
            CanAddEditAdminUsers = 8,

            /// <summary>
            /// With this permission a user can view user accounts at the system level
            /// </summary>
            CanViewAdminUsers  = 9,

            /// <summary>
            /// With this permission a user can add and edit user accounts at the provider level
            /// </summary>
            CanAddEditProviderUsers = 10,

            /// <summary>
            /// With this permission a user can view user accounts at the provider level
            /// </summary>
            CanViewProviderUsers = 11,

            /// <summary>
            /// With this permission a user can add and edit user accounts at the organisation level
            /// </summary>
            CanAddEditOrganisationUsers = 12,

            /// <summary>
            /// With this permission a user can view user accounts at the organisation level
            /// </summary>
            CanViewOrganisationUsers = 13,

            /// <summary>
            /// With this permission a user can edit email templates
            /// </summary>
            CanEditEmailTemplates = 14,

            /// <summary>
            /// With this permission a user may add a new organisation
            /// </summary>
            CanAddOrganisation = 15,

            /// <summary>
            /// With this permission a user may edit an organisation
            /// </summary>
            CanEditOrganisation = 16,
           
            /// <summary>
            /// With this permission a user may view all admin reports
            /// </summary>
            CanViewAdminReports = 17,

            /// <summary>
            /// With this permission a user may manually audit providers
            /// </summary>
            CanManuallyAuditProviders = 18,

            /// <summary>
            /// With this permission a user may view organisation reports
            /// </summary>
            CanViewOrganisationReports = 19,

            /// <summary>
            /// With this permission a user may view provider reports
            /// </summary>
            CanViewProviderReports = 20,

            /// <summary>
            /// With this permission a user may access the bulk upload screens at the organisation level
            /// </summary>
            CanBulkUploadOrganisationFiles = 21,

            /// <summary>
            /// With this permission a user may access the bulk upload screens at the provider level
            /// </summary>
            CanBulkUploadProviderFiles = 22,

            /// <summary>
            /// With this permission a user may manually audit courses
            /// </summary>
            CanManuallyAuditCourses = 23,

            /// <summary>
            /// With this permission a user may add a new venue
            /// </summary>
            CanAddProviderVenue = 24,

            /// <summary>
            /// With this permission a user may edit a venue
            /// </summary>
            CanEditProviderVenue = 25,

            /// <summary>
            /// With this permission a user may view a venue
            /// </summary>
            CanViewProviderVenue = 26,

            /// <summary>
            /// With this permission a user may add a new course
            /// </summary>
            CanAddProviderCourse = 27,

            /// <summary>
            /// With this permission a user may edit a course
            /// </summary>
            CanEditProviderCourse = 28,

            /// <summary>
            /// With this permission a user may view a course
            /// </summary>
            CanViewProviderCourse = 29,

            /// <summary>
            /// With this permission a user may manage the application cache
            /// </summary>
            CanManageCache = 30,

            /// <summary>
            /// With this permission a user may manage the application configuration
            /// </summary>
            CanManageConfiguration = 31,

            /// <summary>
            /// With this permission a user may add a new opportunity
            /// </summary>
            CanAddProviderOpportunity = 32,

            /// <summary>
            /// With this permission a user may edit an opportunity
            /// </summary>
            CanEditProviderOpportunity = 33,

            /// <summary>
            /// With this permission a user may view an opportunity
            /// </summary>
            CanViewProviderOpportunity = 34,

            /// <summary>
            /// With this permission a user can manage provider memberships for an organisation
            /// </summary>
            CanManageOrganisationProviderMembership = 35,
            
            /// <summary>
            /// With this permission a user may view provider details
            /// </summary>
            CanViewProvider = 36,

            /// <summary>
            /// With this permission a user may view organisation details
            /// </summary>
            CanViewOrganisation = 37,

            /// <summary>
            /// With this permission a user is listed as a primary contact
            /// </summary>
            // This replaces the idea that a superuser is the primary contact as
            // we have no way of knowing which roles are considered superusers
            // without having a disembodied 'IsSuperUser' permission which seems
            // dangerous and wrong.
            IsPrimaryContact = 38,

            /// <summary>
            /// With this permission a user can view and manage organisation memberships for a provider
            /// </summary>
            CanManageProviderOrganisationMembership = 39,

            /// <summary>
            /// With this permission a user can delete opportunities for a provider
            /// </summary>
            CanDeleteProviderOpportunity = 40,

            /// <summary>
            /// With this permission a user can delete courses for a provider
            /// </summary>
            CanDeleteProviderCourse = 41,

            /// <summary>
            /// With this permission a user can delete venues for a provider
            /// </summary>
            CanDeleteProviderVenue = 42,

            /// <summary>
            /// With this permission a user can edit special fields against a provider such as UKPRN and Contracting Body
            /// </summary>
            CanEditProviderSpecialFields = 43,

            /// <summary>
            /// With this permission a user can edit special fields against an organisation such as UKPRN and Contracting Body
            /// </summary>
            CanEditOrganisationSpecialFields = 44,

            /// <summary>
            /// With this permission a user may delete an organisation
            /// </summary>
            CanDeleteOrganisation = 45,

            /// <summary>
            /// With this permission a user may recalculate provider or organisation quality scores
            /// </summary>
            CanRecalculateQualityScores = 46,

            /// <summary>
            /// With this permission a user may edit Secure Access users
            /// </summary>
            CanEditSecureAccessUsers = 47,

            /// <summary>
            /// With this permission a user may upload and administer Course Search Usage Statistics files
            /// </summary>
            CanEditCourseSearchStats = 48,

            /// <summary>
            /// With this permission a user may manage language resources
            /// </summary>
            CanManageLanguages = 49,

            /// <summary>
            /// With this permission a user may view a list of public API users
            /// </summary>
            CanViewPublicAPIUsers = 50,

            /// <summary>
            /// With this permission a user may view add or edit public API users
            /// </summary>
            CanAddEditPublicAPIUsers = 51,

            /// <summary>
            /// With this permission a user may view upload FE Choices data
            /// </summary>
            CanUploadFEChoicesData = 52,

            /// <summary>
            /// With this permission a user may view upload LARS data
            /// </summary>
            CanUploadLARSData = 53,

            /// <summary>
            /// With this permission a user may view upload Address Base data
            /// </summary>
            CanUploadAddressBaseData = 54,

            /// <summary>
            /// With this permission a user may manage site content
            /// </summary>
            CanManageContent = 55,

            /// <summary>
            /// With this permission a user may manage content files
            /// </summary>
            CanManageFiles = 56,

            /// <summary>
            /// With this permission a user may manage A10 codes
            /// </summary>
            CanManageA10Codes = 57,

            /// <summary>
            /// With this permission a user may upload UCAS data
            /// </summary>
            CanUploadUCASData = 58,

            /// <summary>
            /// With this permission a user may add locations.
            /// </summary>
            CanAddProviderLocation = 59,

            /// <summary>
            /// With this permission a user may edit locations.
            /// </summary>
            CanEditProviderLocation = 60,

            /// <summary>
            /// With this permission a user may view locations.
            /// </summary>
            CanViewProviderLocation = 61,

            /// <summary>
            /// With this permission a user may delete locations.
            /// </summary>
            CanDeleteProviderLocation = 62,

            /// <summary>
            /// With this permission a user may add apprenticeships.
            /// </summary>
            CanAddProviderApprenticeship = 63,

            /// <summary>
            /// With this permission a user may edit apprenticeships.
            /// </summary>
            CanEditProviderApprenticeship = 64,

            /// <summary>
            /// With this permission a user may view apprenticeships.
            /// </summary>
            CanViewProviderApprenticeship = 65,

            /// <summary>
            /// With this permission a user may delete apprenticeships.
            /// </summary>
            CanDeleteProviderApprenticeship = 66,

            /// <summary>
            /// With this permission a user may access the apprenticeship bulk upload screens at the organisation level
            /// </summary>
            CanBulkUploadOrganisationApprenticeshipFiles = 67,

            /// <summary>
            /// With this permission a user may access the apprenticeship bulk upload screens at the provider level
            /// </summary>
            CanBulkUploadProviderApprenticeshipFiles = 68,

            /// <summary>
            /// With this permission a user may manage the list of apprenticeship location delivery modes.
            /// </summary>
            CanManageDeliveryModes = 69,

            /// <summary>
            /// With this permission a user may import a noew code point (GeoLocation) file.
            /// </summary>
            CanUploadCodePointData = 70,

            /// <summary>
            /// With this permission a user may upload Provider data
            /// </summary>
            CanUploadProviderData = 71,

            /// <summary>
            /// With this permission a user may QA apprenticeships
            /// </summary>
            CanQAApprenticeships = 72,

            /// <summary>
            /// With this permission a user may view apprenticeship QA status
            /// </summary>
            CanViewApprenticeshipQA = 73,

            /// <summary>
            /// With this permission a user may QA providers
            /// </summary>
            CanQAProviders = 74,

            /// <summary>
            /// With this permission a user may view provider QA status
            /// </summary>
            CanViewProviderQA = 75,

            /// <summary>
            /// With this permission a user can view the apprenticeship reports
            /// </summary>
            CanViewApprenticeshipReports = 76,

            /// <summary>
            /// With this permission a user can click the "All Courses up to Date" button
            /// </summary>
            CanSetAllCoursesUpToDate = 77,

            /// <summary>
            /// With this permission a user may manage import batch names
            /// </summary>
            CanManageImportBatches = 78,

            /// <summary>
            /// With this permission a user may view, set and clear Unable to Complete status for a provider
            /// </summary>
            CanManageUnableToComplete = 79,
            /// <summary>
            /// With this permission a user may manually assign an import batch to a provider
            /// </summary>
            CanManuallyAssignImportBatches = 80,
            /// <summary>
            /// With this permission a user can override the maximum number of locations for a specific provider
            /// </summary>
            CanOverrideMaxLocations = 81,
            /// <summary>
            /// With this permission a user can edit the course search API search phrases
            /// </summary>
            CanEditAPISearchPhrases = 82,
            /// <summary>
            /// With this permission a user can edit the course search API stop words
            /// </summary>
            CanEditStopWords = 83
        }
        #endregion

        #region Public Method - HasPermission
        /// <summary>
        /// Checks if the current authenticated user has the permissions requested
        /// </summary>
        /// <param name="throwNoPermissionException">When true if no permission than an exception is thrown</param>
        /// <param name="requiresAll">When true the user must have all the permissions requested for the permission check to return true.
        /// When false the user only needs to have one of the permissions requested for true to be returned</param>
        /// <param name="requiredPermission">The permission or permissions to check the user has</param>
        /// <returns>Returns true if has permission, false if no permissions</returns>
        /// <exception cref="System.Security.SecurityException">Thrown if throwNoPermissionException is true and permission check is false</exception>
        public static bool HasPermission(bool throwNoPermissionException, bool requiresAll, params Permission.PermissionName[] requiredPermission)
        {
            System.Collections.Generic.List<int> usersPermissions = GetPermissionsForCurrentUser();
            int matchCount = 0;

            // No permissions fetched or not authenticated so return false
            if (usersPermissions == null)
            {
                return false;
            }

            foreach (PermissionName permission in requiredPermission)
            {
                if (usersPermissions.Contains((int)permission))
                {
                    matchCount++;
                    if (!requiresAll || matchCount == requiredPermission.Length)
                    {
                        return true;
                    }
                }
            }

            // If reaching here no permission match
            if (throwNoPermissionException)
            {
                try
                {
                    string listOfRequestPermissions = string.Join(",", requiredPermission.Select(x => x.ToString()).ToArray());
                    string url = System.Web.HttpContext.Current.Request.Url.ToString();
                    AppGlobal.WriteAudit(string.Format("A no permission exception of type System.Security.SecurityException was thrown for user '{0}' who requested an action requiring permissions '{1}' at page {2}", System.Web.HttpContext.Current.User.Identity.Name, listOfRequestPermissions, url), false);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                { // Failsafe, if the audit code throws an exception this catch ensures we always throw the correct no permission exception from here 
                }

                throw new System.Security.SecurityException("No permissions for this action");
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Public Method - GetCurrentUserGuid
        /// <summary>
        /// Returns the user Id for the current authenticated user, or GUID.Empty if not authenticate or no user Id found
        /// </summary>
        /// <returns>The user Id or GUID.Empty if no UserId found or not authenticated</returns>
        public static Guid GetCurrentUserGuid()
        {
            // Gets a permission list, if permission list is null then the user Id is not valid so we send back an empty GUID
            Guid userId;
            if (GetPermissionsForCurrentUser() != null
                && Guid.TryParse(
                    (string)System.Web.HttpContext.Current.Session[Constants.SessionFieldNames.UserId],
                    out userId))
            {
                return userId;
            }
            else
            {
                return Guid.Empty;
            }
        }
        #endregion

        #region Public Method - GetCurrentUserId
        /// <summary>
        /// Returns the user Id as a string for the current authenticated user, or null if not authenticated or no user Id found. The string is formatted from a GUID
        /// using the format internal to OWIN.  For an explicit Guid use GetCurrentUserGuid      
        /// </summary>
        /// <returns>The user Id as a string or null if not authenticated</returns>
        public static string GetCurrentUserId()
        {
            // Gets a permission list, if permission list is null then the user Id is not valid so we send back an empty GUID
            string userId = null;
            if (GetPermissionsForCurrentUser() != null)
            {
                userId = (string)System.Web.HttpContext.Current.Session[Constants.SessionFieldNames.UserId];
            }
            return userId;
        }
        #endregion

        #region Public Method - AddMissingPermissions
        /// <summary>
        /// Finds all permissions from the constants in the Permissions Class, and adds the missing ones to the DB.
        /// </summary>
        public static void AddMissingPermissions()
        {
            // Get Database Permissions
            using (ProviderPortalEntities databaseContext = new ProviderPortalEntities())
            {
                var permissions = from perm in databaseContext.Permissions
                                  select new { PermissionId = perm.PermissionId };

                System.Array enumValues = System.Enum.GetValues(typeof(Permission.PermissionName));
                for (int index = 1; index < enumValues.Length; index++)
                {
                    int permissionId = index;
                    if (!permissions.Any(p => p.PermissionId == permissionId))
                    {
                        // Add permission
                        string permissionName = ((Permission.PermissionName)(int)enumValues.GetValue(index)).ToString();

                        Entities.Permission newPermission = new Entities.Permission
                                                                {
                                                                    PermissionName = permissionName,
                                                                    PermissionId = permissionId,
                                                                    PermissionDescription =
                                                                        "Automatically added by the site code, description required"
                                                                };
                        databaseContext.Permissions.Add(newPermission);
                    }
                }

                databaseContext.SaveChanges();
            }
        }
        #endregion

        #region Private Method - GetPermissionsForCurrentUser
        /// <summary>
        /// The get permissions for current user.
        /// </summary>
        /// <returns>
        /// A list of permissions.
        /// </returns>
        private static List<int> GetPermissionsForCurrentUser()
        {
            System.Collections.Generic.List<int> permissionList = null;

            // Do we have cached permission list
            System.Web.SessionState.HttpSessionState session = System.Web.HttpContext.Current.Session;
            bool isAuthenticated = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                string thisUser = System.Web.HttpContext.Current.User.Identity.Name;
                if (session[Constants.SessionFieldNames.Permissions] != null && session[Constants.SessionFieldNames.UserName] != null)
                {
                    // Have cached permissions, make sure session is for this user, if not invalidate and reload the permissions
                    var sessionForUser = (string)session[Constants.SessionFieldNames.UserName];
                    if (thisUser.Equals(sessionForUser, StringComparison.InvariantCultureIgnoreCase))
                    {
                        permissionList = (System.Collections.Generic.List<int>)session[Constants.SessionFieldNames.Permissions];
                    }
                    else
                    {
                        session[Constants.SessionFieldNames.UserName] = thisUser;
                        session[Constants.SessionFieldNames.UserRealName] = null;
                        session[Constants.SessionFieldNames.Permissions] = null;
                        permissionList = null;
                        session[Constants.SessionFieldNames.UserRole] = null;
                        AppGlobal.Log.WriteWarning(string.Format("The user {0} has visited the site with the session details for user {1}, the session details have been invalidated and permissions reloaded for the correct user.", thisUser, sessionForUser));
                    }
                }

                if (permissionList == null)
                {
                    // No cached copy so load                    
                    using (var databaseContext = new ProviderPortalEntities())
                    {
                        var user = databaseContext.AspNetUsers.FirstOrDefault(u => u.UserName.Equals(thisUser));
                        if (user == null)
                        {
                            // Not authenticated, should not have got this far, return nothing
                            return null;
                        }
                        permissionList =
                            user.AspNetRoles.SelectMany(x => x.Permissions)
                                .Select(x => x.PermissionId)
                                .Distinct()
                                .ToList();

                        // Get user roles
                        var roles = String.Join(", ", user.AspNetRoles.Select(x => x.Name).ToArray());

                        // Cache in the session for subsequent visits
                        session.Add(Constants.SessionFieldNames.Permissions, permissionList);
                        session.Add(Constants.SessionFieldNames.UserName, thisUser);
                        session.Add(Constants.SessionFieldNames.UserId, user.Id);
                        session.Add(Constants.SessionFieldNames.UserRole, roles);
                        session.Add(Constants.SessionFieldNames.UserRealName, user.Name);
                    }
                }

                // Have permissions so return
                return permissionList;
            }

            // Not authenticated, should not have got this far, return nothing
            return null;
        }
        #endregion
    }
}