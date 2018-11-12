using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Tribal.SkillsFundingAgency.ProviderPortal.BulkUpload;
using Tribal.SkillsFundingAgency.ProviderPortal.BulkUploadWCFService;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class BulkUploadValidateExtension
    {
        public static void InitiateBulkUpload(
            this BulkUploadViewModel model,
            UserContext.UserContextInfo userContext,
            ProviderPortalEntities db)
        {
            try
            {
                // copy data to local data store
                new FileHandler(model).CopyFileToDataStore();

                // get bulk upload service client
                var bulkUploadService = new BulkUploadWCFServiceClient();

                // when validating only ...
                if (!model.OverrideException)
                {
                    // ... build contextual parameters for bulk upload serivce
                    var parameters = new EnqueueParameters
                    {
                        FileName = model.Summary.FileName,
                        FilePath = model.Summary.FilePath,
                        UserId = LoggedInUser(db),
                        UserContextType = userContext.ContextName.ToString(),
                        UserContextItemId = (int)userContext.ItemId,
                        FileSize = model.Summary.ContentLength
                    };

                    // ... and enqueue the bulk upload with the service
                    var result = bulkUploadService.Enqueue(parameters);
                }
            }
            catch (EndpointNotFoundException ex)
            {
                model.Message = AppGlobal.Language.GetText(
                    "BulkUpload_Exceptions_EndpointNotFoundException",
                    "The Bulk Upload service has encountered an error, and your upload has failed.  Please try your upload later.  If you encounter this message again, please contact the Support Team.");
                AppGlobal.Log.WriteLog(string.Concat(ex.Message, ":", ex.StackTrace));
            }
            catch (TimeoutException ex)
            {
                model.Message = AppGlobal.Language.GetText(
                    "BulkUpload_Exceptions_TimeoutException",
                    "The Bulk Upload service could not be contacted, and your upload has failed.  Please try your upload later.  If you encounter this message again, please contact the Support Team.");
                AppGlobal.Log.WriteLog(string.Concat(ex.Message, ":", ex.StackTrace));
            }
            catch (FaultException<BulkUploadProviderFault> ex)
            {
                var template = AppGlobal.Language.GetText("BulkUpload_Exceptions_BulkUploadProviderFaultException", "The Bulk Upload service has rejected your file, with the following error: {0}");
                model.Message = String.Format(template, ex.Detail.Message);
            }   
            catch (FaultException<BulkUploadFault> ex)
            {
                model.Message = AppGlobal.Language.GetText("BulkUpload_Exceptions_BulkUploadFaultException", "The Bulk Upload service has encountered an error, and your upload has failed.  Please try your upload later.  If you encounter this message again, please contact the Support Team.");
                AppGlobal.Log.WriteLog(string.Concat(ex.Detail.Message, ":", ex.StackTrace));
            }
            catch (Exception ex)
            {
                model.Message = ex.Message;
                AppGlobal.Log.WriteLog(string.Concat(ex.Message, ":", ex.StackTrace));
            }
        }

        private static string LoggedInUser(ProviderPortalEntities db)
        {
            var loggedInUserName = HttpContext.Current.User.Identity.Name;
            var user = db.AspNetUsers.FirstOrDefault(u => u.UserName.Equals(loggedInUserName));
            return user != null ? user.Id : string.Empty;
        }

        public static List<int> GetPermittedBulkUploadProviders(ProviderPortalEntities db, UserContext.UserContextInfo userContext, Constants.BulkUpload_DataType dataType)
        {
            var permittedBulkUploadProviders = new List<int>();

            var providerPermission = (dataType == Constants.BulkUpload_DataType.CourseData ? Permission.PermissionName.CanBulkUploadProviderFiles : Permission.PermissionName.CanBulkUploadProviderApprenticeshipFiles);
            var organisationPermission = (dataType == Constants.BulkUpload_DataType.CourseData ? Permission.PermissionName.CanBulkUploadOrganisationFiles : Permission.PermissionName.CanBulkUploadOrganisationApprenticeshipFiles);

            if (userContext.IsProvider() && Permission.HasPermission(false, true, providerPermission))
            {
                permittedBulkUploadProviders.Add(userContext.ItemId.Value);
            }
            else if (userContext.IsOrganisation() && Permission.HasPermission(false, true, organisationPermission))
            {
                permittedBulkUploadProviders =
                    db.OrganisationProviders.Where(
                        o =>
                            o.OrganisationId.Equals(userContext.ItemId.Value)
                            && o.CanOrganisationEditProvider
                            && o.IsAccepted && !o.IsRejected)
                        .Select(o => o.ProviderId).ToList();
            }

            return permittedBulkUploadProviders;
        }
    }
}