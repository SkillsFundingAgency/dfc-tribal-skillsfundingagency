using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Tribal.SkillsFundingAgency.ProviderPortal.BulkUpload.Entities;
using System.Linq;
using System.Web.Mvc;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    #region /* Bulk Upload Home Page Classes */

    public class BulkUploadViewModel : BulkUploadStatusViewModel
    {
        

        public BulkUploadViewModel()
        {           
            Summary = new UploadSummary { SummaryItems = new List<UploadSummaryExceptionItem>() };
        }

        [Required(ErrorMessage = "Please specify a CSV file.")]
        public HttpPostedFileBase File { get; set; }

        [MustBeTrue(ErrorMessage = "You must accept the terms and conditions")]
        public bool IsTncSelected { get; set; }

        public UserContext.UserContextName UserCntxName { get; set; }

        public BulkUploadProviderViewModel ProviderViewModel { get; set; }

        public BulkUploadOrganisationViewModels OrganisationViewModel { get; set; }

        public UploadSummary Summary { get; set; }

        public bool IsTrue { get { return true; } }

        public bool IsEligibleForDbUpdate
        {
            get
            {
                return (
                          Summary.Status != Constants.BulkUploadStatus.Failed_Stage_1_of_4 &&
                          Summary.Status != Constants.BulkUploadStatus.Failed_Stage_2_of_4 &&
                          Summary.Status != Constants.BulkUploadStatus.Failed_Stage_3_of_4 &&
                          Summary.Status != Constants.BulkUploadStatus.UnknownException &&
                //          Summary.Status != Constants.BulkUploadStatus.NoValidRecords &&
                          (Summary.Status != Constants.BulkUploadStatus.Needs_Confirmation || OverrideException)  //either no exception, or user have overridden exceptions
                       );
            }
        }

        public bool OverrideException { get; set; }
    }

    public class BulkUploadProviderViewModel
    {
        public Int32 ProviderId { get; set; }

        [LanguageDisplay("Offered for")]
        public string ProviderName { get; set; }

        [LanguageDisplay("Courses")]
        [DisplayFormat(DataFormatString="{0:N0}")]
        public int Courses { get; set; }

        [LanguageDisplay("Opportunities")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Opportunities { get; set; }

        [LanguageDisplay("Apprenticeships")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Apprenticeships { get; set; }

        [LanguageDisplay("Delivery Locations")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int DeliveryLocations { get; set; }
    }

    public class BulkUploadOrganisationViewModels : List<BulkUploadOrganisationViewModel>
    {
        public bool IsAnyDownloadableProviderExists
        {
            get
            {
                return Exists(e => e.CanOrganisationEditProvider);
            }
        }
    }

    public class BulkUploadOrganisationViewModel
    {
        [LanguageDisplay("Provider")]
        public string ProviderName { get; set; }

        [LanguageDisplay("1")]
        public int CourseType1 { get; set; }

        [LanguageDisplay("2")]
        public int CourseType2 { get; set; }

        [LanguageDisplay("3")]
        public int CourseType3 { get; set; }

        [LanguageDisplay("Total")]
        public int CourseTypeSum
        {
            get
            {
                return CourseType1 + CourseType2 + CourseType3;
            }
        }

        [LanguageDisplay("In")]
        public int OpportunityScopeIn { get; set; }

        [LanguageDisplay("Out")]
        public int OpportunityScopeOut { get; set; }

        [LanguageDisplay("For Org.")]
        public int OpportunityForOrganisation { get; set; }

        [LanguageDisplay("For Prov.")]
        public int OpportunityForProvider { get; set; }

        [LanguageDisplay("Total")]
        public int OpportunityForTotal
        {
            get
            {
                return OpportunityForOrganisation + OpportunityForProvider;
            }
        }

        [LanguageDisplay("Apprenticeships")]
        public int Apprenticeships { get; set; }

        [LanguageDisplay("Delivery Locations")]
        public int DeliveryLocations { get; set; }

        public bool CanOrganisationEditProvider { get; set; }
    }

    #endregion

    #region  /*Bulk Upload History Page Classes*/

    public class BulkUploadHistoryViewModel
    {
        [LanguageDisplay("BulkUploadId")]
        public int BulkUploadId { get; set; }

        [LanguageDisplay("File Name")]
        public string FileName { get; set; }

        [LanguageDisplay("Uploaded date")]
        [DateDisplayFormat(Format = DateFormat.ShortDateTime)]
        public DateTime UploadedDateTime { get; set; }

        public int StatusId { get; set; }

        public Constants.FileContentType FileContentType { get; set; }

        [LanguageDisplay("Status")]
        public string StatusDescription { get; set; }

        [LanguageDisplay("User Name")]
        public string UserName { get; set; }

        [LanguageDisplay("Download")]
        public string DownloadUrl { get; set; }

        [LanguageDisplay("IsDownloadAvailable")]
        public bool IsDownloadAvailable { get; set; }

        [LanguageDisplay("IsUploadSuccessful")]
        public bool IsUploadSuccessful { get; set; }

        public bool IsAuthorisedToViewAndDownload { get; set; }

        public bool IsOrganisationUpload { get; set; }

        [LanguageDisplay("Provider Names")]
        public string ProviderName { get; set; }

    }

    #endregion

    #region  /*Bulk Upload History Details Page Classes*/

    public class BulkUploadStatusViewModel
    {
        public bool IsValid
        {
            get { return string.IsNullOrEmpty(Message); }
        }

        public string Message { get; set; }
    }

    public class BulkUploadHistoryDetailViewModel : BulkUploadStatusViewModel
    {
        public int BulkUploadId { get; set; }

        [LanguageDisplay("File Name")]
        public string FileName { get; set; }

        public Constants.FileContentType FileContentType { get; set; }

        [LanguageDisplay("User Name")]
        public string UserName { get; set; }

        [LanguageDisplay("Date uploaded")]
        [DateDisplayFormat(Format = DateFormat.ShortDateTime)]
        public string UploadedDateTime { get; set; }

        [LanguageDisplay("Stage")]
        public string UploadStatusText { get; set; }

        public Constants.BulkUploadStatus UploadStatus { get; set; }


        public string PartialUploadText1
        {
            get
            {
                string message = string.Empty;

                Int32 threshold = FileContentType == Constants.FileContentType.CourseData ? Constants.ConfigSettings.BulkUploadThresholdAcceptableLimit : Constants.ConfigSettings.BulkUploadApprenticeshipThresholdAcceptableLimit;
                if (ExistingCourseCount > 0)
                {
                    if (TotalCourseCount / (Double)ExistingCourseCount*100 < threshold)
                    {
                        message += string.Format(AppGlobal.Language.GetText("Confirm_PartialUpload_WarningForLowRecordCount1", "Your recent bulk uploaded file {0} contains considerably fewer valid records than you currently have on our database."), FileName);
                    }
                }

                if (message == String.Empty && ExistingOpportunityCount > 0)
                {
                    if (TotalOpportunityCount / (Double)ExistingOpportunityCount * 100 < threshold)
                    {
                        message += string.Format(AppGlobal.Language.GetText("Confirm_PartialUpload_WarningForLowRecordCount1", "Your recent bulk uploaded file {0} contains considerably fewer valid records than you currently have on our database."), FileName);
                    }
                }

                if (message == String.Empty && ExistingVenueCount > 0)
                {
                    if (TotalVenueCount / (Double)ExistingVenueCount * 100 < threshold)
                    {
                        message += string.Format(AppGlobal.Language.GetText("Confirm_PartialUpload_WarningForLowRecordCount1", "Your recent bulk uploaded file {0} contains considerably fewer valid records than you currently have on our database."), FileName);
                    }
                }

                if (ExistingApprenticeshipCount > 0)
                {
                    if (TotalApprenticeshipCount / (Double)ExistingApprenticeshipCount * 100 < threshold)
                    {
                        message += string.Format(AppGlobal.Language.GetText("Confirm_PartialUpload_WarningForLowRecordCount1", "Your recent bulk uploaded file {0} contains considerably fewer valid records than you currently have on our database."), FileName);
                    }
                }

                if (message == String.Empty && ExistingDeliveryLocationCount > 0)
                {
                    if (TotalDeliveryLocationCount / (Double)ExistingDeliveryLocationCount * 100 < threshold)
                    {
                        message += string.Format(AppGlobal.Language.GetText("Confirm_PartialUpload_WarningForLowRecordCount1", "Your recent bulk uploaded file {0} contains considerably fewer valid records than you currently have on our database."), FileName);
                    }
                }

                if (message == String.Empty && ExistingLocationCount > 0)
                {
                    if (TotalLocationCount / (Double)ExistingLocationCount * 100 < threshold)
                    {
                        message += string.Format(AppGlobal.Language.GetText("Confirm_PartialUpload_WarningForLowRecordCount1", "Your recent bulk uploaded file {0} contains considerably fewer valid records than you currently have on our database."), FileName);
                    }
                }


                return message;
            }
        }

        public string PartialUploadText2
        {
            get
            {
                string message = string.Empty;
                if (TotalCourseCount < ExistingCourseCount || TotalOpportunityCount < ExistingOpportunityCount)
                {
                    var courseWarningAndErrorCount = ErrorSummary.CourseWarningCount + ErrorSummary.CourseNoticeCount;
                    var opportunityWarningAndErrorCount = ErrorSummary.OpportunityWarningCount + ErrorSummary.OpportunityNoticeCount;
                    message += string.Format(AppGlobal.Language.GetText("Confirm_PartialUpload_WarningForLowCourseDataRecordCount2", "You currently have {0} courses and {1} opportunities on the existing database. Your newly uploaded file contains {2} courses " +
                                                                      " ({3} {4}) and {5} opportunities ({6} {7})."),
                                                                       ExistingCourseCount,
                                                                       ExistingOpportunityCount,
                                                                       TotalCourseCount,
                                                                       courseWarningAndErrorCount,
                                                                       courseWarningAndErrorCount == 1? " error or warning" : " errors or warnings",
                                                                       TotalOpportunityCount,
                                                                       opportunityWarningAndErrorCount,
                                                                       opportunityWarningAndErrorCount == 1? " error or warning" : " errors or warnings");


                }
                else if ((TotalApprenticeshipCount - InvalidApprenticeshipCount) < ExistingApprenticeshipCount || (TotalDeliveryLocationCount - InvalidDeliveryLocationCount) < ExistingDeliveryLocationCount)
                {
                    var apprenticeshipWarningAndErrorCount = ErrorSummary.ApprenticeshipWarningCount + ErrorSummary.ApprenticeshipNoticeCount;
                    var deliveryLocationWarningAndErrorCount = ErrorSummary.DeliveryLocationWarningCount + ErrorSummary.DeliveryLocationNoticeCount;                   
                    message += string.Format(AppGlobal.Language.GetText("Confirm_PartialUpload_WarningForLowApprenticeshipDataRecordCount2", "You currently have {0} apprenticeships and {1} delivery locations on the existing database. Your newly uploaded file contains {2} apprenticeships " +
                                                                      " ({3} {4}) and {5} delivery locations ({6} {7})."),
                                                                       ExistingApprenticeshipCount,
                                                                       ExistingDeliveryLocationCount,
                                                                       TotalApprenticeshipCount,
                                                                       apprenticeshipWarningAndErrorCount,
                                                                       apprenticeshipWarningAndErrorCount == 1 ? " error or warning" : " errors or warnings",
                                                                       TotalDeliveryLocationCount,
                                                                       deliveryLocationWarningAndErrorCount,
                                                                       deliveryLocationWarningAndErrorCount == 1? " error or warning" : " errors or warnings");

                }
                return message;
            }
        }

        public int ExistingVenueCount { get; set; }

        [LanguageDisplay("Number of Venues")]
        public int TotalVenueCount { get; set; }

        [LanguageDisplay("Number of Invalid Venues")]
        public int InvalidVenueCount { get; set; }

        [LanguageDisplay("Number of Valid Venues")]
        public int ValidVenueCount { get { return TotalVenueCount - InvalidVenueCount; } }

        [LanguageDisplay("Percentage of Valid Venues")]
        public string ValidVenuePercent
        {
            get
            {
                return String.Format("{0:P2}.", ((decimal)(TotalVenueCount - InvalidVenueCount) / TotalVenueCount));
            }
        }

        public int ExistingCourseCount { get; set; }

        [LanguageDisplay("Number of Courses")]
        public int TotalCourseCount { get; set; }

        [LanguageDisplay("Number of Invalid Courses")]
        public int InvalidCourseCount { get; set; }

        [LanguageDisplay("Number of Valid Courses")]
        public int ValidCourseCount { get { return TotalCourseCount - InvalidCourseCount; } }

        [LanguageDisplay("Percentage of Valid Courses")]
        public string ValidCoursePercent
        {
            get
            {
                return String.Format("{0:P2}.", ((decimal)(TotalCourseCount - InvalidCourseCount) / TotalCourseCount));
            }
        }

        public int ExistingOpportunityCount { get; set; }

        [LanguageDisplay("Number of Opportunities")]
        public int TotalOpportunityCount { get; set; }

        [LanguageDisplay("Number of Invalid Opportunities")]
        public int InvalidOpportunityCount { get; set; }

        [LanguageDisplay("Number of Valid Opportunities")]
        public int ValidOpportunityCount { get { return TotalOpportunityCount - InvalidOpportunityCount; } }

        [LanguageDisplay("Percentage of Valid Opportunities")]
        public string ValidOpportunitiesPercent
        {
            get
            {
                return String.Format("{0:P2}.", ((decimal)(TotalOpportunityCount - InvalidOpportunityCount) / TotalOpportunityCount));
            }
        }


        public int ExistingLocationCount { get; set; }

        [LanguageDisplay("Number of Locations")]
        public int TotalLocationCount { get; set; }

        [LanguageDisplay("Number of Invalid Locations")]
        public int InvalidLocationCount { get; set; }

        [LanguageDisplay("Number of Valid Locations")]
        public int ValidLocationCount { get { return TotalLocationCount - InvalidLocationCount; } }

        [LanguageDisplay("Percentage of Valid Locations")]
        public string ValidLocationPercent
        {
            get
            {
                return String.Format("{0:P2}.", ((decimal)(TotalLocationCount - InvalidLocationCount) / TotalLocationCount));
            }
        }

        public int ExistingApprenticeshipCount { get; set; }

        [LanguageDisplay("Number of Apprenticeships")]
        public int TotalApprenticeshipCount { get; set; }

        [LanguageDisplay("Number of Invalid Apprenticeships")]
        public int InvalidApprenticeshipCount { get; set; }

        [LanguageDisplay("Number of Valid Apprenticeships")]
        public int ValidApprenticeshipCount { get { return TotalApprenticeshipCount - InvalidApprenticeshipCount; } }

        [LanguageDisplay("Percentage of Valid Apprenticeships")]
        public string ValidApprenticeshipPercent
        {
            get
            {
                return String.Format("{0:P2}.", ((decimal)(TotalApprenticeshipCount - InvalidApprenticeshipCount) / TotalApprenticeshipCount));
            }
        }

        public int ExistingDeliveryLocationCount { get; set; }

        [LanguageDisplay("Number of Delivery Locations")]
        public int TotalDeliveryLocationCount { get; set; }

        [LanguageDisplay("Number of Invalid Delivery Locations")]
        public int InvalidDeliveryLocationCount { get; set; }

        [LanguageDisplay("Number of Valid Delivery Locations")]
        public int ValidDeliveryLocationCount { get { return TotalDeliveryLocationCount - InvalidDeliveryLocationCount; } }

        [LanguageDisplay("Percentage of Valid DeliveryLocations")]
        public string ValidDeliveryLocationsPercent
        {
            get
            {
                return String.Format("{0:P2}.", ((decimal)(TotalDeliveryLocationCount - InvalidDeliveryLocationCount) / TotalDeliveryLocationCount));
            }
        }




        public BulkUploadHistoryErrorSummary ErrorSummary { get; set; }

        public bool AccessDenied { get; set; }
    }

    public class BulkUploadHistoryDetailItemsViewModel
    {
        [LanguageDisplay("Line Number")]
        public int? RowId { get; set; }

        [LanguageDisplay("Column Name")]
        public string ColumnName { get; set; }

        [LanguageDisplay("Column Value")]
        public string ActualColumnValue { get; set; }

        [LanguageDisplay("Provider Id")]
        public string Provider { get; set; }

        [LanguageDisplay("Details")]
        public string Details { get; set; }

        [LanguageDisplay("Notification Type")]
        public Constants.BulkUpload_Validation_ErrorType ErrorType { get; set; }

        [LanguageDisplay("Section Name")]
        public Constants.BulkUpload_SectionName SectionName { get; set; }
    }

    public class BulkUploadHistoryErrorSummary
    {
        public int ProviderErrorCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Providers)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Error));
            }
        }

        public int ProviderWarningCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Providers)
                                                       &&
                                                       o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Warning));
            }
        }

        public int ProviderNoticeCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Providers)
                                                       &&
                                                       o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Notice));
            }
        }

        public int VenueErrorCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Venues)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Error));
            }
        }

        public int VenueWarningCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Venues)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Warning));
            }
        }

        public int VenueNoticeCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Venues)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Notice));
            }
        }

        public int CourseErrorCount
        {
            get
            {

                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Courses)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Error));
            }
        }

        public int CourseWarningCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Courses)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Warning));
            }
        }

        public int CourseNoticeCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Courses)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Notice));
            }
        }

        public int OpportunityErrorCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Opportunities)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Error));
            }
        }

        public int OpportunityWarningCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Opportunities)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Warning));
            }
        }

        public int OpportunityNoticeCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Opportunities)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Notice));
            }
        }

        public int LocationErrorCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Locations)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Error));
            }
        }

        public int LocationWarningCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Locations)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Warning));
            }
        }

        public int LocationNoticeCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Locations)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Notice));
            }
        }

        public int ApprenticeshipErrorCount
        {
            get
            {

                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Apprenticeships)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Error));
            }
        }

        public int ApprenticeshipWarningCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Apprenticeships)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Warning));
            }
        }

        public int ApprenticeshipNoticeCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.Apprenticeships)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Notice));
            }
        }

        public int DeliveryLocationErrorCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return
                    UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.DeliveryLocations)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Error));
            }
        }

        public int DeliveryLocationWarningCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.DeliveryLocations)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Warning));
            }
        }

        public int DeliveryLocationNoticeCount
        {
            get
            {
                if (UploadSummaryDetails == null)
                    return 0;
                return UploadSummaryDetails.Count(o => o.SectionName.Equals(Constants.BulkUpload_SectionName.DeliveryLocations)
                                                             &&
                                                             o.ErrorType.Equals(Constants.BulkUpload_Validation_ErrorType.Notice));
            }
        }

        public bool HasProviderErrorsOrWarnings
        {
            get
            {
                return ProviderErrorCount > 0 || ProviderWarningCount > 0 || ProviderNoticeCount > 0;
            }
        }

        public bool HasVenueErrorsOrWarnings
        {
            get
            {
                return VenueErrorCount > 0 || VenueWarningCount > 0 || VenueNoticeCount > 0;
            }
        }

        public bool HasCourseErrorsOrWarnings
        {
            get
            {
                return CourseErrorCount > 0 || CourseWarningCount > 0 || CourseNoticeCount > 0;
            }
        }

        public bool HasOpportunityErrorsOrWarnings
        {
            get
            {
                return OpportunityErrorCount > 0 || OpportunityWarningCount > 0 | OpportunityNoticeCount > 0;
            }
        }


        public bool HasLocationErrorsOrWarnings
        {
            get
            {
                return LocationErrorCount > 0 || LocationWarningCount > 0 || LocationNoticeCount > 0;
            }
        }

        public bool HasApprenticeshipErrorsOrWarnings
        {
            get
            {
                return ApprenticeshipErrorCount > 0 || ApprenticeshipWarningCount > 0 || ApprenticeshipNoticeCount > 0;
            }
        }

        public bool HasDeliveryLocationErrorsOrWarnings
        {
            get
            {
                return DeliveryLocationErrorCount > 0 || DeliveryLocationWarningCount > 0 | DeliveryLocationNoticeCount > 0;
            }
        }

        public List<BulkUploadHistoryDetailItemsViewModel> UploadSummaryDetails { get; set; }

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MustBeTrueAttribute : ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "mustbetrue"
            };
        }
    }

    #endregion
}