using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Tribal.SkillsFundingAgency.ProviderPortal.BulkUpload.Convertors;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using WebGrease.Css.Ast.Selectors;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using BulkUpload = Tribal.SkillsFundingAgency.ProviderPortal.Entities.BulkUpload;


    public static class BulkUploadModelExtensions
    {
        public static void Populate(this BulkUploadViewModel model, UserContext.UserContextInfo userContext, ProviderPortalEntities db, Constants.BulkUpload_DataType dataType)
        {
            var providers = LoadProviders(userContext, db, true, userContext.IsOrganisation());

            model.UserCntxName = userContext.ContextName;

            if (userContext.IsProvider())
            {
                model.ProviderViewModel = LoadProviderInformation(providers.FirstOrDefault(), dataType);
            }
            else if (userContext.IsOrganisation())
            {
                model.OrganisationViewModel = LoadOrganisationProviderInfo(providers, dataType);
            }
        }

        public static void Populate(this List<BulkUploadHistoryViewModel> model, UserContext.UserContextInfo userContext, ProviderPortalEntities db, Constants.BulkUpload_DataType dataType)
        {
     
            try
            {
                //IQueryable<BulkUploadHistory> historyRecords;
                IQueryable<BulkUpload> historyRecords;

                if (userContext.IsOrganisation())
                {
                    //all providerswhich belongs to the organsiation
                    var providersToSearch = BulkUploadValidateExtension.GetPermittedBulkUploadProviders(db, userContext, dataType);

                    //get history of all the provider which belongs to organisation + organisation's respective upload
                    /*
                    historyRecords =
                        db.BulkUploadHistories.Include("BulkUploadHistoryProviders.Provider")
                            .Where(
                                x =>
                                providersToSearch.Contains(x.UserProviderId.Value)
                                || x.UserOrganisationId == userContext.ItemId);
                     */

                    historyRecords =
                        db.BulkUploads
                            .Include("BulkUploadProviders.Provider")
                            .Where(x =>
                                (providersToSearch.Contains(x.UserProviderId.Value) || x.UserOrganisationId == userContext.ItemId)
                                 &&
                                (x.FileContentType == null || x.FileContentType == (int)dataType));
                }
                else
                { 
                    /*
                    historyRecords =
                        db.BulkUploadHistories.Include("BulkUploadHistoryProviders.Provider")
                            .Where(
                                x =>
                                x.UserProviderId.HasValue && x.UserProviderId == userContext.ItemId
                                || x.BulkUploadHistoryProviders.Any(p => p.ProviderId == userContext.ItemId));
                     */

                    historyRecords =
                        db.BulkUploads
                            .Include("BulkUploadProviders.Provider")
                            .Where(x =>
                                (x.UserProviderId.HasValue && x.UserProviderId == userContext.ItemId || x.BulkUploadProviders.Any(p => p.ProviderId == userContext.ItemId))
                                 &&
                                 (x.FileContentType == null || x.FileContentType == (int)dataType));

                }

                foreach (var historyRecord in historyRecords.OrderByDescending(x => x.BulkUploadId))
                {
                    var currentStatus =
                        historyRecord.BulkUploadStatusHistories.OrderByDescending(x => x.CreatedDateTimeUtc)
                            .FirstOrDefault();
                    if (currentStatus == null) continue;
                    var item = new BulkUploadHistoryViewModel
                    {
                        BulkUploadId = historyRecord.BulkUploadId,
                        FileName = historyRecord.FileName,
                        DownloadUrl = historyRecord.FilePath,
                        IsOrganisationUpload = userContext.IsOrganisation(),
                        ProviderName = LoadProviderName(historyRecord),
                        IsAuthorisedToViewAndDownload = userContext.IsOrganisation()
                            ? historyRecord.UserOrganisationId == userContext.ItemId
                            : historyRecord.UserProviderId == userContext.ItemId,
                        UserName = currentStatus.AspNetUser.Name,
                        UploadedDateTime = currentStatus.CreatedDateTimeUtc.ToLocalTime(),
                        StatusDescription = currentStatus.BulkUploadStatu.BulkUploadStatusText,
                        IsDownloadAvailable =
                            !string.IsNullOrEmpty(historyRecord.FilePath) && File.Exists(historyRecord.FilePath),
                        IsUploadSuccessful =
                            currentStatus.BulkUploadStatusId.Equals((int) Constants.BulkUploadStatus.Published),
                    };

                    if (historyRecord.FileContentType != null)
                    {
                        item.FileContentType = (Constants.FileContentType)Enum.Parse(typeof(Constants.FileContentType), historyRecord.FileContentType.ToString());
                    }

                    model.Add(item);
                }
            }
            catch (Exception ex)
            {
                AppGlobal.Log.WriteLog(string.Concat(ex.Message, "-", ex.StackTrace));
            }
        }

        public static void Populate(this BulkUploadHistoryDetailViewModel model, UserContext.UserContextInfo userContext, ProviderPortalEntities db, int bulkUploadId)
        {

            var history = (from bulk in db.BulkUploads
                           where bulk.BulkUploadId == bulkUploadId
                           select bulk).FirstOrDefault();

            if (history == null)
            {
                model.AccessDenied = true;
                return;
            }
            if ((userContext.IsOrganisation() && history.UserOrganisationId != userContext.ItemId.Value)
                || (userContext.IsProvider() && history.UserProviderId != userContext.ItemId.Value))
            {
                model.AccessDenied = true;
                return;
            }

            var currentStatus = history.BulkUploadStatusHistories.OrderByDescending(x => x.CreatedDateTimeUtc).FirstOrDefault();
            if (currentStatus == null)
            {
                model.AccessDenied = true;
                return;
            }

            model.BulkUploadId = bulkUploadId;

            model.FileName = history.FileName;

            if (history.FileContentType != null)
            {
                model.FileContentType = (Constants.FileContentType)Enum.Parse(typeof(Constants.FileContentType), history.FileContentType.ToString());
            }

            model.UserName = db.AspNetUsers.FirstOrDefault(x => x.Id.ToString() == currentStatus.LoggedInUserId).Name;

            model.UploadedDateTime = currentStatus.CreatedDateTimeUtc.ToLocalTime().ToString(Constants.ConfigSettings.ShortDateTimeFormat);

            model.UploadStatusText = currentStatus.BulkUploadStatu.BulkUploadStatusText;

            model.ExistingCourseCount = history.ExistingCourses ?? 0;

            model.TotalCourseCount = history.NewCourses ?? 0;

            model.InvalidCourseCount = history.InvalidCourses ?? 0;

            model.ExistingVenueCount = history.ExistingVenues ?? 0;

            model.TotalVenueCount = history.NewVenues ?? 0;

            model.InvalidVenueCount = history.InvalidVenues ?? 0;

            model.ExistingOpportunityCount = history.ExistingOpportunities ?? 0;

            model.TotalOpportunityCount = history.NewOpportunities ?? 0;

            model.InvalidOpportunityCount = history.InvalidOpportunities ?? 0;

            model.ExistingApprenticeshipCount = history.ExistingApprenticeships ?? 0;

            model.TotalApprenticeshipCount = history.NewApprenticeships ?? 0;

            model.InvalidApprenticeshipCount = history.InvalidApprenticeships ?? 0;

            model.ExistingLocationCount = history.ExistingLocations ?? 0;

            model.TotalLocationCount = history.NewLocations ?? 0;

            model.InvalidLocationCount = history.InvalidLocations ?? 0;

            model.ExistingDeliveryLocationCount = history.ExistingDeliveryLocations ?? 0;

            model.TotalDeliveryLocationCount = history.NewDeliveryLocations ?? 0;

            model.InvalidDeliveryLocationCount = history.InvalidDeliveryLocations ?? 0;


            model.UploadStatus = (Constants.BulkUploadStatus)Enum.Parse(typeof(Constants.BulkUploadStatus), currentStatus.BulkUploadStatu.BulkUploadStatusName);

            model.ErrorSummary = new BulkUploadHistoryErrorSummary
            {
                UploadSummaryDetails = new List<BulkUploadHistoryDetailItemsViewModel>()
            };

            if (history.FileContentType != null)
            {
                model.FileContentType = (Constants.FileContentType)Enum.Parse(typeof(Constants.FileContentType), history.FileContentType.ToString());
            }

            foreach (var item in db.BulkUploadExceptionItems.Where(x => x.BulkUploadId == bulkUploadId))
            {
                var bulkUploadHistoryDetailItemsViewModel = new BulkUploadHistoryDetailItemsViewModel
                {
                    ColumnName = item.ColumnName,
                    RowId = item.LineNumber,
                    ActualColumnValue = item.ColumnValue,
                    Provider = item.ProviderId.HasValue ? item.ProviderId.ToString() : string.Empty,
                    Details = item.Details,
                    ErrorType = (Constants.BulkUpload_Validation_ErrorType)Enum.Parse(typeof(Constants.BulkUpload_Validation_ErrorType), item.BulkUploadErrorType.BulkUploadErrorTypeName),
                    SectionName = (Constants.BulkUpload_SectionName)item.BulkUploadSectionId
                };
                model.ErrorSummary.UploadSummaryDetails.Add(bulkUploadHistoryDetailItemsViewModel);
            }
        }

        public static BulkUploadHistoryViewModel GetHistoryFileUrl(this List<BulkUploadHistoryViewModel> model, UserContext.UserContextInfo userContext, ProviderPortalEntities db, int bulkUploadId)
        {
            var url = (from bulk in db.BulkUploads
                       where bulk.BulkUploadId == bulkUploadId
                       select bulk).FirstOrDefault();


            if ((userContext.IsOrganisation() && url.UserOrganisationId != userContext.ItemId.Value)
                || (userContext.IsProvider() && url.UserProviderId != userContext.ItemId.Value))
            {
                return null;
            }

            BulkUploadHistoryViewModel filteredModel = null;
            if (url != null)
            {
                filteredModel = new BulkUploadHistoryViewModel
                {
                    FileName = url.FileName,
                    DownloadUrl = url.FilePath
                };
            }

            return filteredModel;
        }

        public static byte[] TransformToCsv(this BulkUploadViewModel model, UserContext.UserContextInfo userContext, ProviderPortalEntities db, Constants.BulkUpload_DataType dataType)
        {
            var providers = LoadProviders(userContext, db, true, true);

            return new EntityToCsvConvertor(providers, userContext, dataType).ToCsv();
        }

        public static string GetCsvFileName(this BulkUploadViewModel model, UserContext.UserContextInfo userContext, ProviderPortalEntities db, Constants.BulkUpload_DataType dataType)
        {
            var providerOrOrganisationName = userContext.IsOrganisation()
                           ? db.Organisations.Where(o => o.OrganisationId.Equals(userContext.ItemId.Value)).Select(o => o.OrganisationName).FirstOrDefault()
                           : db.Providers.Where(p => p.ProviderId.Equals(userContext.ItemId.Value)).Select(p => p.ProviderName).FirstOrDefault();

            var dataTypeName = (dataType == Constants.BulkUpload_DataType.CourseData ? "Courses" : "Apprenticeships");

            return string.Format("{0}_{1}.{2}", providerOrOrganisationName, dataTypeName, Constants.BulkDownloadFileExtension);
        }

        private static BulkUploadOrganisationViewModels LoadOrganisationProviderInfo(IEnumerable<Provider> providers, Constants.BulkUpload_DataType dataType)
        {
            var orgViewModels = new BulkUploadOrganisationViewModels();

            foreach (var provider in providers)
            {
                var orgViewModel = new BulkUploadOrganisationViewModel
                {
                    ProviderName = provider.ProviderName
                };

                if (dataType == Constants.BulkUpload_DataType.CourseData)
                {
                    orgViewModel.CourseType1 = LoadCourseType1(provider);

                    orgViewModel.CourseType2 = LoadCourseType2(provider);

                    orgViewModel.CourseType3 = LoadCourseType3(provider);

                    orgViewModel.OpportunityScopeIn = LoadScopeInCount(provider);

                    orgViewModel.OpportunityScopeOut = LoadScopeOutCount(provider);

                    orgViewModel.OpportunityForOrganisation = LoadOpportunityForOrganisation(provider);

                    orgViewModel.OpportunityForProvider = LoadOpportunityForProvider(provider);
                }
                else //its Apprenticeship data
                {
                    orgViewModel.Apprenticeships = GetApprenticeshipCount(provider.ProviderId);
                    orgViewModel.DeliveryLocations = GetDeliveryLocationCount(provider.ProviderId);
                }

                orgViewModel.CanOrganisationEditProvider = provider.OrganisationProviders.FirstOrDefault(p => p.CanOrganisationEditProvider && p.IsAccepted && !p.IsRejected) != null;

                orgViewModels.Add(orgViewModel);
            }
            return orgViewModels;
        }

        private static BulkUploadProviderViewModel LoadProviderInformation(Provider provider, Constants.BulkUpload_DataType dataType)
        {
            if (provider != null)
            {
                BulkUploadProviderViewModel model = new BulkUploadProviderViewModel
                {
                    ProviderId = provider.ProviderId,
                    ProviderName = provider.ProviderName                   
                };
                
                if (dataType == Constants.BulkUpload_DataType.CourseData)
                {
                    model.Courses = provider.Courses.Count(c => c.RecordStatusId.Equals((int)Constants.RecordStatus.Live));
                    if (model.Courses < 1000)
                    {
                        model.Opportunities = GetOpportunityCount(provider.ProviderId);
                    }
                    else
                    {
                    model.Opportunities = -1; // GetOpportunityCount(provider.Courses.Where(c => c.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))                     
                    }
                }
                else //Apprenticeship Data
                {
                    model.Apprenticeships = GetApprenticeshipCount(provider.ProviderId);
                    //if (model.Apprenticeships < 1000)
                    //{
                        model.DeliveryLocations = GetDeliveryLocationCount(provider.ProviderId);
                    //}
                    //else
                    //{
                    //    model.DeliveryLocations = -1;
                    //}                  
                }
                return model;
            }
            return null;
        }

        private static int LoadCourseType1(Provider provider)
        {
            return provider.Courses.Count(c => c.LearningAim != null &&
                                               c.LearningAim.Qualification.Equals(string.Empty) &&
                                               (c.RecordStatusId.Equals((int)Constants.RecordStatus.Live)));
        }

        private static int LoadCourseType2(Provider provider)
        {
            return provider.Courses.Count(c => c.LearningAim != null &&
                                               !c.LearningAim.Qualification.Equals(string.Empty) &&
                                               (c.RecordStatusId.Equals((int)Constants.RecordStatus.Live)));
        }

        private static int LoadCourseType3(Provider provider)
        {
            return provider.Courses.Count(c => c.LearningAim == null &&
                                               (c.RecordStatusId.Equals((int)Constants.RecordStatus.Live)));
        }

        private static int LoadScopeInCount(Provider provider)
        {
            var scopeInCodes = Constants.ScopeInCodes;

            var result = 0;

            foreach (var course in provider.Courses.Where(c => c.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))
            {
                foreach (var instance in course.CourseInstances.Where(c => c.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))
                {
                    if (instance.A10FundingCode.Any(a10 => scopeInCodes.Contains(a10.A10FundingCodeId)))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        private static int LoadScopeOutCount(Provider provider)
        {
            var scopeOutCodes = Constants.scopeOutCodes;

            var result = 0;

            foreach (var course in provider.Courses.Where(c => c.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))
            {
                foreach (var instance in course.CourseInstances.Where(c => c.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))
                {
                    if (instance.A10FundingCode.Count.Equals(0) || instance.A10FundingCode.Any(a10 => scopeOutCodes.Contains(a10.A10FundingCodeId)))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public static int GetOpportunityCount(Int32 providerId)
        {
            return new ProviderPortalEntities().CourseInstances.Count(ci => ci.Course.RecordStatusId.Equals((int)Constants.RecordStatus.Live) && ci.Course.ProviderId == providerId && ci.RecordStatusId.Equals((int)Constants.RecordStatus.Live));
        }

        public static int GetDeliveryLocationCount(Int32 providerId)
        {
            return new ProviderPortalEntities().ApprenticeshipLocations.Count(al => al.Apprenticeship.RecordStatusId.Equals((int)Constants.RecordStatus.Live) && al.Apprenticeship.ProviderId == providerId && al.RecordStatusId.Equals((int)Constants.RecordStatus.Live));
         //   return new ProviderPortalEntities().Locations.Count(l => l.RecordStatusId.Equals((int)Constants.RecordStatus.Live) && l.ProviderId == providerId && l.ApprenticeshipLocations..Equals((int)Constants.RecordStatus.Live));
      
        }

        public static int GetApprenticeshipCount(Int32 providerId)
        {
            return new ProviderPortalEntities().Apprenticeships.Count(a => a.RecordStatusId.Equals((int)Constants.RecordStatus.Live) && a.ProviderId == providerId);
        }

        private static int LoadOpportunityForOrganisation(Provider provider)
        {
            var count = 0;
            var courses = provider.Courses.Where(c => c.RecordStatusId.Equals((int)Constants.RecordStatus.Live));
            foreach (var course in courses)
            {
                count += course.CourseInstances.Count(i => i.OfferedByOrganisationId.HasValue);
            }
            return count;
        }

        private static int LoadOpportunityForProvider(Provider provider)
        {
            var count = 0;
            var courses = provider.Courses.Where(c => c.RecordStatusId.Equals((int)Constants.RecordStatus.Live));
            foreach (var course in courses)
            {
                count += course.CourseInstances.Count(i => !i.OfferedByOrganisationId.HasValue);
            }
            return count;
        }

        private static List<Provider> LoadProviders(UserContext.UserContextInfo userContext, ProviderPortalEntities db,
            bool downloadableProvidersOnly = false, bool eagerLoading = false)
        {
            var providers = new List<Provider>();

            switch (userContext.ContextName)
            {
                case UserContext.UserContextName.Provider:
                {
                    var provider = eagerLoading
                        ? db.Providers
                            .Include("Venues.Address")
                            .Include("Courses.CourseInstances.CourseInstanceStartDates")
                            .First(x => x.ProviderId == userContext.ItemId)
                        : db.Providers.Find(userContext.ItemId);
                    if (provider != null)
                    {
                        providers.Add(provider);
                    }
                    break;
                }
                case UserContext.UserContextName.Organisation:
                {
                    var organisation = db.Organisations.FirstOrDefault(x => x.OrganisationId == userContext.ItemId);

                    if (organisation != null)
                    {
                        var providerIds = organisation.OrganisationProviders.Where(
                            o =>
                                o.IsAccepted && !o.IsRejected &&
                                o.Provider.RecordStatusId == (int) Constants.RecordStatus.Live
                                && (o.CanOrganisationEditProvider || !downloadableProvidersOnly))
                            .Select(x => x.ProviderId).ToList();

                        //foreach (var item in providerIds)
                        //{
                        //    providers.Add(
                        //        eagerLoading
                        //            ? db.Providers
                        //                .Include("Venues.Address")
                        //                .Include("Courses.CourseInstances.CourseInstanceStartDates")
                        //                .First(x => x.ProviderId == item)
                        //            : db.Providers.Find(item)
                        //        );
                        //}

                        providers.AddRange(
                            providerIds.Select(
                                item =>
                                    (Provider)
                                        (eagerLoading
                                            ? db.Providers.Include("Venues.Address")
                                                .Include("Courses.CourseInstances.CourseInstanceStartDates")
                                                .First(x => x.ProviderId == item)
                                            : db.Providers.Find(item))));
                    }
                    break;
                }
            }
            return providers;
        }

        private static string LoadProviderName(BulkUpload historyRecord)
        {
            var providerName = string.Empty;

            foreach (var provider in historyRecord.BulkUploadProviders)
            {
                if (provider.Provider != null)
                {
                    providerName += string.Concat(provider.Provider.ProviderName, Environment.NewLine);
                }
            }

            return providerName;
        }
    }
}