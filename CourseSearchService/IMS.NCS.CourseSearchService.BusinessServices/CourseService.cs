using System;
using System.Collections.Generic;
using System.Linq;

using IMS.NCS.CourseSearchService.Common;
using IMS.NCS.CourseSearchService.Entities;
using IMS.NCS.CourseSearchService.Queries;
using Ims.Schemas.Alse.CourseSearch.Contract;

namespace IMS.NCS.CourseSearchService.BusinessServices
{
    /// <summary>
    /// Service Implementation providing all course functions.
    /// </summary>
    public class CourseService : ICourseService
    {
        #region Variables

        private readonly ICourseQuery _courseQuery;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Unity Constructor.
        /// </summary>
        /// <param name="courseQuery">ICourseQuery object.</param>
        public CourseService(ICourseQuery courseQuery)
        {
            _courseQuery = courseQuery;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Gets a list of courses matching the search criteria in CourseListRequestStructure
        /// and logs the duration of the search.
        /// </summary>
        /// <param name="courseListInput">Search criteria.</param>
        /// <returns>Populated CourseLisOutput containing matching courses.</returns>
        public CourseListOutput GetCourseList(CourseListInput courseListInput)
        {
            CourseListOutput courseListOutput = new CourseListOutput(new CourseListResponseStructure());

            CourseListRequest request = BuildCourseListRequest(courseListInput.CourseListRequest);

            CourseListResponse response = _courseQuery.GetCourseList(request);

            courseListOutput.CourseListResponse =
                BuildCourseListResponseStructure(response, courseListInput.CourseListRequest);

            // Record search time
            _courseQuery.RecordSearchTime(Constants.SEARCH_TIME_COLUMN_FLAG, response.SearchHeaderId);

            return courseListOutput;
        }

        /// <summary>
        /// Gets course details for the course ids in CourseDetailInput.
        /// </summary>
        /// <param name="courseDetailInput">CourseDetailInput containing course ids to return details for.</param>
        /// <returns>Populated CourseDetailsOutput.</returns>
        public CourseDetailOutput GetCourseDetails(CourseDetailInput courseDetailInput)
        {
            List<long> courseIds = BuildCourseIdList(courseDetailInput.CourseID);

            List<Course> courses = _courseQuery.GetCourseDetails(courseIds, courseDetailInput.APIKey);

            return BuildCourseDetailOutput(courses, courseDetailInput);
        }

        /// <summary>
        /// Gets a list of categories matching the search crietria.
        /// </summary>
        /// <param name="subjectBrowseInput">Search criteria.</param>
        /// <returns>Populated SubjectBrowseOutput containing matching categories.</returns>
        public SubjectBrowseOutput GetCategories(SubjectBrowseInput subjectBrowseInput)
        {
            List<Category> categories = _courseQuery.GetCategories(subjectBrowseInput.APIKey);

            return BuildSubjectBrowseOutput(categories);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Build CourseListRequest object from CourseListRequstStructure.
        /// </summary>
        /// <param name="courseRequest">CourseListRequestStructure data.</param>
        /// <returns>Populated CourseListRequest object.</returns>
        private static CourseListRequest BuildCourseListRequest(CourseListRequestStructure courseRequest)
        {
            SearchCriteriaStructure searchCriteria = courseRequest.CourseSearchCriteria;
            CourseListRequest request = new CourseListRequest();

            // if seachCriteria not null, populate request.
            if (searchCriteria != null)
            {
                request.APIKey = searchCriteria.APIKey;
                request.SubjectKeyword = searchCriteria.SubjectKeyword;
                request.DFE1619Funded = searchCriteria.DFE1619Funded;
                request.AppClosedFlag = searchCriteria.OppsAppClosedFlag;
                request.Distance = searchCriteria.Distance;
                request.DistanceSpecified = searchCriteria.DistanceSpecified;
                request.EarliestStartDate = searchCriteria.EarliestStartDate;
                request.FlexStartFlag = searchCriteria.FlexStartFlag;
                request.IesFlag = searchCriteria.IESFlag;
                request.IlsFlag = searchCriteria.ILSFlag;

                if (!string.IsNullOrEmpty(searchCriteria.ProviderID))
                {
                    request.ProviderId = Int32.Parse(searchCriteria.ProviderID);
                }

                request.ProviderKeyword = searchCriteria.ProviderKeyword;
                request.SflFlag = searchCriteria.SFLFlag;
                request.Location = searchCriteria.Location;
                request.TqsFlag = searchCriteria.TQSFlag;
                request.TtgFlag = searchCriteria.TTGFlag;

                // If A10 codes exist, add them to the request.
                if ((searchCriteria.A10Codes != null) &&
                    (searchCriteria.A10Codes.A10Code != null && searchCriteria.A10Codes.A10Code.Length > 0))
                {
                    request.A10Codes = Utilities.ConvertToDelimitedString(searchCriteria.A10Codes.A10Code, "|");
                }

                // If Adult LR statuses exist, add them to the request.
                if (searchCriteria.AdultLRStatus != null && searchCriteria.AdultLRStatus.Length > 0)
                {
                    request.AdultLRStatus = Utilities.ConvertToDelimitedString(searchCriteria.AdultLRStatus, "|");
                }

                // If Attendance modes exist, add them to the request.
                if ((searchCriteria.AttendanceModes != null) &&
                    (searchCriteria.AttendanceModes.AttendanceMode != null &&
                    searchCriteria.AttendanceModes.AttendanceMode.Length > 0))
                {
                    request.AttendanceModes =
                        Utilities.ConvertToDelimitedString(searchCriteria.AttendanceModes.AttendanceMode, "|");
                }

                // If Attendance patterns exist, add them to the request.
                if ((searchCriteria.AttendancePatterns != null) &&
                    (searchCriteria.AttendancePatterns.AttendancePattern != null &&
                    searchCriteria.AttendancePatterns.AttendancePattern.Length > 0))
                {
                    request.AttendancePatterns =
                        Utilities.ConvertToDelimitedString(searchCriteria.AttendancePatterns.AttendancePattern, "|");
                }

                // If ER App statuses exist, add them to the request.
                if (searchCriteria.ERAppStatus != null && searchCriteria.ERAppStatus.Length > 0)
                {
                    request.ERAppStatus = Utilities.ConvertToDelimitedString(searchCriteria.ERAppStatus, "|");
                }

                // If ER TTG statuses exist, add them to the request.
                if (searchCriteria.ERTtgStatus != null && searchCriteria.ERTtgStatus.Length > 0)
                {
                    request.ERTtgStatus = Utilities.ConvertToDelimitedString(searchCriteria.ERTtgStatus, "|");
                }

                // If LDCS category codes exist, add them to the request.
                if ((searchCriteria.LDCS != null) &&
                    (searchCriteria.LDCS.CategoryCode != null && searchCriteria.LDCS.CategoryCode.Length > 0))
                {
                    request.LdcsCategoryCode =
                        Utilities.ConvertToDelimitedString(searchCriteria.LDCS.CategoryCode, "|");
                }

                // If Other funding statuses exist, add them to the request.
                if (searchCriteria.OtherFundingStatus != null && searchCriteria.OtherFundingStatus.Length > 0)
                {
                    request.OtherFundingStatus =
                        Utilities.ConvertToDelimitedString(searchCriteria.OtherFundingStatus, "|");
                }

                // If Qualification levels exist, add them to the request.
                if ((searchCriteria.QualificationLevels != null) &&
                    (searchCriteria.QualificationLevels.QualificationLevel != null &&
                    searchCriteria.QualificationLevels.QualificationLevel.Length > 0))
                {
                    request.QualificationLevels =
                        Utilities.ConvertToDelimitedString(searchCriteria.QualificationLevels.QualificationLevel, "|");
                }

                // If Qualification types exist, add them to the request.
                if ((searchCriteria.QualificationTypes != null) &&
                    (searchCriteria.QualificationTypes.QualificationType != null &&
                    searchCriteria.QualificationTypes.QualificationType.Length > 0))
                {
                    request.QualificationTypes =
                        Utilities.ConvertToDelimitedString(searchCriteria.QualificationTypes.QualificationType, "|");
                }

                // If Study modes exist, add them to the request.
                if ((searchCriteria.StudyModes != null) &&
                    (searchCriteria.StudyModes.StudyMode != null && searchCriteria.StudyModes.StudyMode.Length > 0))
                {
                    request.StudyModes = Utilities.ConvertToDelimitedString(searchCriteria.StudyModes.StudyMode, "|");
                }

                // Add number of records per page to request, default to 20 if does not exist.
                int recordsPerPage = 0;

                if (!string.IsNullOrEmpty(courseRequest.RecordsPerPage))
                {
                    recordsPerPage = Int32.Parse(courseRequest.RecordsPerPage);
                }

                request.RecordsPerPage = (recordsPerPage > 0) ? recordsPerPage : 20;

                // Add sort by to request.
                request.SortBy = courseRequest.SortBy.ToString();

                // Add page number to request, default to 1 if less than 1.
                if (!string.IsNullOrEmpty(courseRequest.PageNo))
                {
                    long pageNumber = Int64.Parse(courseRequest.PageNo);
                    request.PageNumber = (pageNumber > 0) ? pageNumber : 1;
                }
                else
                {
                    request.PageNumber = 1;
                }
            }

            return request;
        }

        /// <summary>
        /// Build CourseListResponseStructure object from CourseListResponse data and original CourseListRequestStructure.
        /// </summary>
        /// <param name="response">CourseListResponse data.</param>
        /// <param name="request">Original CourseListRequestStructure.</param>
        /// <returns>Populated CourseListResponseStructure object.</returns>
        private static CourseListResponseStructure BuildCourseListResponseStructure(CourseListResponse response, CourseListRequestStructure request)
        {
            CourseListResponseStructure courseListResponse = new CourseListResponseStructure();

            // Create matching Ldcs collection
            List<CourseListResponseStructureMatchingLDCS> matchingLdcss =
                new List<CourseListResponseStructureMatchingLDCS>();

            foreach (LdcsCode ldcsCode in response.LdcsCodes)
            {
                CourseListResponseStructureMatchingLDCS matchingLdcs = new CourseListResponseStructureMatchingLDCS();
                matchingLdcs.LDCS = BuildLdcsInfoType(ldcsCode.LdcsCodeValue, ldcsCode.LdcsCodeDescription);
                matchingLdcs.Counts = ldcsCode.CourseCount.ToString();

                matchingLdcss.Add(matchingLdcs);
            }

            courseListResponse.MatchingLDCS = matchingLdcss.ToArray();

            // Create CourseStructure collection
            List<CourseStructure> courseStructures = new List<CourseStructure>();

            foreach (Course course in response.Courses)
            {
                // Get Course information
                CourseStructure courseStructure = new CourseStructure();

                courseStructure.Course = new CourseInfo();
                courseStructure.Course.CourseID = course.CourseId.ToString();
                courseStructure.Course.CourseSummary = course.CourseSummary;
                courseStructure.Course.CourseTitle = course.CourseTitle;

                courseStructure.Course.LDCS = new LDCSOutputType();
                courseStructure.Course.LDCS.CatCode1 = BuildLdcsInfoType(course.LdcsCode1, course.LdcsDesc1);
                courseStructure.Course.LDCS.CatCode2 = BuildLdcsInfoType(course.LdcsCode2, course.LdcsDesc2);
                courseStructure.Course.LDCS.CatCode3 = BuildLdcsInfoType(course.LdcsCode3, course.LdcsDesc3);
                courseStructure.Course.LDCS.CatCode4 = BuildLdcsInfoType(course.LdcsCode4, course.LdcsDesc4);
                courseStructure.Course.LDCS.CatCode5 = BuildLdcsInfoType(course.LdcsCode5, course.LdcsDesc5);
                courseStructure.Course.NoOfOpps = course.NumberOfOpportunities.ToString();
                courseStructure.Course.QualificationLevel = course.QualificationLevel;
                courseStructure.Course.QualificationType = course.QualificationType;

                // Get Opportunity information
                List<OpportunityInfo> opportunityInfos = new List<OpportunityInfo>();

                foreach (Opportunity opportunity in course.Opportunities)
                {
                    OpportunityInfo opportunityInfo = new OpportunityInfo();

                    opportunityInfo.AttendanceMode = opportunity.AttendanceMode;
                    opportunityInfo.AttendancePattern = opportunity.AttendancePattern;
                    opportunityInfo.OpportunityId = opportunity.OpportunityId;

                    StartDateType startDateType = BuildStartDateType(opportunity);
                    
                    //StartDateType startDateType = new StartDateType();
                    //startDateType.ItemElementName = ItemChoiceType.Date;
                    //startDateType.Item = opportunity.StartDate;

                    // TODO: how do we add these in?  I suspect we need to change the contract.
                    //StartDateType startDateDescType = new StartDateType();
                    //startDateDescType.ItemElementName = ItemChoiceType.DateDesc;
                    //startDateDescType.Item = opportunity.StartDateDescription;

                    opportunityInfo.StartDate = startDateType;
                    opportunityInfo.Duration = new DurationType();
                    opportunityInfo.Duration.DurationDescription = opportunity.DurationDescription;
                    opportunityInfo.Duration.DurationUnit = opportunity.DurationUnit;

                    if (opportunity.DurationValue != 0)
                    {
                        opportunityInfo.Duration.DurationValue = opportunity.DurationValue.ToString();
                    }

                    opportunityInfo.EndDate = opportunity.EndDate;
                    opportunityInfo.StudyMode = opportunity.StudyMode;
                    opportunityInfo.DFE1619Funded = opportunity.DfE1619Funded;
                    opportunityInfo.DFE1619FundedSpecified = true;

                    if (opportunity.Venue != null && !string.IsNullOrEmpty(opportunity.Venue.VenueName))
                    {
                        VenueInfo venueInfo = new VenueInfo();
                        if (!string.IsNullOrEmpty(opportunity.Distance))
                        {
                            venueInfo.DistanceSpecified = true;
                            venueInfo.Distance = float.Parse(opportunity.Distance);
                        }
                        venueInfo.VenueName = opportunity.Venue.VenueName;
                        venueInfo.VenueAddress = 
                            BuildAddressType(opportunity.Venue.AddressLine1, opportunity.Venue.AddressLine2, 
                            opportunity.Venue.Town, opportunity.Venue.County, opportunity.Venue.Postcode, opportunity.Venue.Latitude, opportunity.Venue.Longitude);

                        opportunityInfo.Item = venueInfo;
                    }
                    else
                    {
                        opportunityInfo.Item = opportunity.RegionName;
                    }

                    opportunityInfos.Add(opportunityInfo);
                }

                // pick out the first opportunity (expecting there to be only one looking at the java web service's design).
                // if there is more than one here - then current site cannot deal with it.
                courseStructure.Opportunity = opportunityInfos.ElementAt(0);

                courseStructure.Provider = new ProviderInfo();
                courseStructure.Provider.ProviderName = course.Provider.ProviderName;
                courseStructure.Provider.TFPlusLoans = course.Provider.TFPlusLoans;
                courseStructure.Provider.TFPlusLoansSpecified = true;
                courseStructure.Provider.DFE1619Funded = course.Provider.DFE1619Funded;
                courseStructure.Provider.DFE1619FundedSpecified = true;

                if (course.Provider.FEChoices_EmployerSatisfaction.HasValue)
                {
                    courseStructure.Provider.FEChoices_EmployerSatisfaction = course.Provider.FEChoices_EmployerSatisfaction.Value;
                }
                courseStructure.Provider.FEChoices_EmployerSatisfactionSpecified = course.Provider.FEChoices_EmployerSatisfaction.HasValue;
                if (course.Provider.FEChoices_LearnerSatisfaction.HasValue)
                {
                    courseStructure.Provider.FEChoices_LearnerSatisfaction = course.Provider.FEChoices_LearnerSatisfaction.Value;
                }
                courseStructure.Provider.FEChoices_LearnerSatisfactionSpecified = course.Provider.FEChoices_LearnerSatisfaction.HasValue;
                if (course.Provider.FEChoices_LearnerDestination.HasValue)
                {
                    courseStructure.Provider.FEChoices_LearnerDestination = course.Provider.FEChoices_LearnerDestination.Value;
                }
                courseStructure.Provider.FEChoices_LearnerDestinationSpecified = course.Provider.FEChoices_LearnerDestination.HasValue;


                courseStructures.Add(courseStructure);
            }

            courseListResponse.CourseDetails = courseStructures.ToArray();

            // Get Result information, i.e. page number etc.
            courseListResponse.ResultInfo = new ResultInfoType();

            int requestRecordsPerPage = 0;
            if (!string.IsNullOrEmpty(request.RecordsPerPage))
            {
                requestRecordsPerPage = Int32.Parse(request.RecordsPerPage);
            }

            int responseRecordsPerPage = (requestRecordsPerPage > 0) ? requestRecordsPerPage : 50;
            int totalRecords = response.NumberOfRecords;
            int numberOfPages =
                (totalRecords / responseRecordsPerPage) + ((totalRecords % responseRecordsPerPage == 0) ? 0 : 1);

            int currentPage = 1;
            if (!string.IsNullOrEmpty(request.PageNo))
            {
                currentPage = Int32.Parse(request.PageNo);
            }

            courseListResponse.ResultInfo.NoOfRecords = response.NumberOfRecords.ToString();
            courseListResponse.ResultInfo.PageNo = (currentPage > 0) ? currentPage.ToString() : "1";
            courseListResponse.ResultInfo.NoOfPages = numberOfPages.ToString();

            // Get original Request details
            courseListResponse.RequestDetails = request;

            return courseListResponse;
        }

        /// <summary>
        /// Builds a List of course ids from an Array of course ids.
        /// </summary>
        /// <param name="courseIds">Source Array of course ids.</param>
        /// <returns>A List of course ids.</returns>
        private static List<long> BuildCourseIdList(string[] courseIds)
        {
            List<long> courseIdList = new List<long>();

            foreach (string courseId in courseIds)
            {
                if (!string.IsNullOrEmpty(courseId))
                {
                    long id = long.Parse(courseId);
                    courseIdList.Add(id);
                }
            }

            return courseIdList;
        }

        /// <summary>
        /// Build CourseDetailOutput object from collection of Courses and CourseDetailInput.
        /// </summary>
        /// <param name="courses">Courses collection.</param>
        /// <param name="courseDetailInput">Original CourseDetailInput.</param>
        /// <returns>Populated CourseDetailOutput object.</returns>
        private static CourseDetailOutput BuildCourseDetailOutput(List<Course> courses, CourseDetailInput courseDetailInput)
        {
            CourseDetailOutput courseDetailOutput = new CourseDetailOutput();
            List<CourseDetailStructure> courseDetailStructures = new List<CourseDetailStructure>();

            foreach (Course course in courses)
            {
                // Get Course detail
                CourseDetailStructure courseDetailStructure = new CourseDetailStructure();

                courseDetailStructure.Course = new CourseDetail();

                courseDetailStructure.Course.CourseID = course.CourseId.ToString();
                courseDetailStructure.Course.CourseTitle = course.CourseTitle;
                courseDetailStructure.Course.CourseSummary = course.CourseSummary;
                courseDetailStructure.Course.NoOfOpps = course.NumberOfOpportunities.ToString();
                courseDetailStructure.Course.QualificationLevel = course.QualificationLevel;
                courseDetailStructure.Course.QualificationType = course.QualificationType;
                courseDetailStructure.Course.AccreditationEndDate = course.AccreditationEndDate;
                courseDetailStructure.Course.AccreditationStartDate = course.AccreditationStartDate;
                courseDetailStructure.Course.AdultLRStatus = GetAdultLRStatus(course.AdultLRStatus);
                courseDetailStructure.Course.AssessmentMethod = course.AssessmentMethod;
                courseDetailStructure.Course.AwardingBody = course.AwardingBody;
                courseDetailStructure.Course.BookingURL = course.BookingUrl;
                courseDetailStructure.Course.CertificationEndDate = course.CertificationEndDate;
                courseDetailStructure.Course.CreditValue = course.CreditValue;
                courseDetailStructure.Course.DataType = GetDataType(course.DataType);
                courseDetailStructure.Course.ERAppStatus = GetERAppStatus(course.ERAppStatus);
                courseDetailStructure.Course.ERTTGStatus = GetERTtgStatus(course.ERTtgStatus);
                courseDetailStructure.Course.EntryRequirements = course.EntryRequirements;
                courseDetailStructure.Course.EquipmentRequired = course.EquipmentRequired;
                courseDetailStructure.Course.IndependentLivingSkills = 
                    GetIndependentLivingSkills(course.IndependentLivingSkills);
                courseDetailStructure.Course.LADID = course.LadId;
                courseDetailStructure.Course.Level2EntitlementCategoryDesc = course.Level2EntitlementDescription;
                courseDetailStructure.Course.Level3EntitlementCategoryDesc = course.Level3EntitlementDescription;
                courseDetailStructure.Course.OtherFundingNonFundedStatus = 
                    GetOtherFundingNonFundedStatus(course.OtherFundingNonFundedStatus);
                courseDetailStructure.Course.QCAGuidedLearningHours = course.QcaGuidedLearningHours;
                courseDetailStructure.Course.QualificationReference = course.QualificationReference;
                courseDetailStructure.Course.QualificationReferenceAuthority = course.QualificationReferenceAuthority;
                courseDetailStructure.Course.QualificationTitle = course.QualificationTitle;
                courseDetailStructure.Course.SkillsForLifeFlag = GetSkillsForLifeFlag(course.SkillsForLifeFlag);
                courseDetailStructure.Course.SkillsForLifeTypeDesc = course.SkillsForLifeTypeDescription;
                courseDetailStructure.Course.TariffRequired = course.TariffRequired;
                courseDetailStructure.Course.URL = course.Url;

                courseDetailStructure.Course.LDCS = new LDCSOutputType();
                courseDetailStructure.Course.LDCS.CatCode1 = BuildLdcsInfoType(course.LdcsCode1, course.LdcsDesc1);
                courseDetailStructure.Course.LDCS.CatCode2 = BuildLdcsInfoType(course.LdcsCode2, course.LdcsDesc2);
                courseDetailStructure.Course.LDCS.CatCode3 = BuildLdcsInfoType(course.LdcsCode3, course.LdcsDesc3);
                courseDetailStructure.Course.LDCS.CatCode4 = BuildLdcsInfoType(course.LdcsCode4, course.LdcsDesc4);
                courseDetailStructure.Course.LDCS.CatCode5 = BuildLdcsInfoType(course.LdcsCode5, course.LdcsDesc5);

                // Get Povider detail
                courseDetailStructure.Provider = new ProviderDetail();
                if (course.Provider != null)
                {
                    courseDetailStructure.Provider.ProviderID = course.Provider.ProviderId;
                    courseDetailStructure.Provider.ProviderName = course.Provider.ProviderName;
                    courseDetailStructure.Provider.Phone = course.Provider.Phone;
                    courseDetailStructure.Provider.Fax = course.Provider.Fax;
                    courseDetailStructure.Provider.Email = course.Provider.Email;
                    courseDetailStructure.Provider.Website = course.Provider.Website;
                    courseDetailStructure.Provider.UKPRN = course.Provider.Ukprn;
                    courseDetailStructure.Provider.UPIN = course.Provider.Upin;
                    courseDetailStructure.Provider.TFPlusLoans = course.Provider.TFPlusLoans;
                    courseDetailStructure.Provider.TFPlusLoansSpecified = true;

                    if (course.Provider.FEChoices_EmployerSatisfaction.HasValue)
                    {
                        courseDetailStructure.Provider.FEChoices_EmployerSatisfaction = course.Provider.FEChoices_EmployerSatisfaction.Value;
                    }
                    courseDetailStructure.Provider.FEChoices_EmployerSatisfactionSpecified = course.Provider.FEChoices_EmployerSatisfaction.HasValue;
                    if (course.Provider.FEChoices_LearnerSatisfaction.HasValue)
                    {
                        courseDetailStructure.Provider.FEChoices_LearnerSatisfaction = course.Provider.FEChoices_LearnerSatisfaction.Value;
                    }
                    courseDetailStructure.Provider.FEChoices_LearnerSatisfactionSpecified = course.Provider.FEChoices_LearnerSatisfaction.HasValue;
                    if (course.Provider.FEChoices_LearnerDestination.HasValue)
                    {
                        courseDetailStructure.Provider.FEChoices_LearnerDestination = course.Provider.FEChoices_LearnerDestination.Value;
                    }
                    courseDetailStructure.Provider.FEChoices_LearnerDestinationSpecified = course.Provider.FEChoices_LearnerDestination.HasValue;

                    courseDetailStructure.Provider.ProviderAddress = BuildAddressType(course.Provider.AddressLine1, 
                        course.Provider.AddressLine2, course.Provider.Town, course.Provider.County, course.Provider.Postcode);
                }

                // Get Opportunity detail
                List<OpportunityDetail> opportunityDetails = new List<OpportunityDetail>();
                foreach (Opportunity opportunity in course.Opportunities)
                {
                    OpportunityDetail opportunityDetail = new OpportunityDetail();

                    opportunityDetail.AttendanceMode = opportunity.AttendanceMode;
                    opportunityDetail.AttendancePattern = opportunity.AttendancePattern;

                    opportunityDetail.Duration = new DurationType();
                    opportunityDetail.Duration.DurationDescription = opportunity.DurationDescription;
                    opportunityDetail.Duration.DurationUnit = opportunity.DurationUnit;

                    if (opportunity.DurationValue != 0)
                    {
                        opportunityDetail.Duration.DurationValue = opportunity.DurationValue.ToString();
                    }

                    opportunityDetail.Price = opportunity.Price;
                    opportunityDetail.PriceDesc = opportunity.PriceDescription;
                    opportunityDetail.StudyMode = opportunity.StudyMode;
                    opportunityDetail.Timetable = opportunity.Timetable;

                    opportunityDetail.StartDate = BuildStartDateType(opportunity);
                    //opportunityDetail.StartDate = new StartDateType();
                    //opportunityDetail.StartDate.ItemElementName = ItemChoiceType.Date;
                    //opportunityDetail.StartDate.Item = opportunity.StartDate;

                    //opportunityDetail.StartDate = new StartDateType();
                    //opportunityDetail.StartDate.ItemElementName = ItemChoiceType.DateDesc;
                    //opportunityDetail.StartDate.Item = opportunity.StartDateDescription;

                    opportunityDetail.ApplicationAcceptedThroughoutYear =
                        GetApplicationAcceptedThroughoutYear(opportunity.ApplicationAcceptedThroughoutYear);
                    opportunityDetail.ApplyFromDate = opportunity.ApplyFromDate;
                    opportunityDetail.ApplyTo = opportunity.ApplyTo;
                    opportunityDetail.ApplyUntilDate = opportunity.ApplyUntilDate;
                    opportunityDetail.ApplyUntilDesc = opportunity.ApplyUntilDescription;
                    opportunityDetail.EndDate = opportunity.EndDate;
                    opportunityDetail.EnquireTo = opportunity.EnquireTo;
                    opportunityDetail.LanguageOfAssessment = opportunity.LanguageOfAssessment;
                    opportunityDetail.LanguageOfInstruction = opportunity.LanguageOfInstruction;
                    opportunityDetail.PlacesAvailable = opportunity.PlacesAvailable.ToString();
                    opportunityDetail.ProviderOpportunityId = opportunity.ProviderOpportunityId;
                    opportunityDetail.URL = opportunity.Url;
                    opportunityDetail.A10 = opportunity.A10.ToArray();

                    List<String> items = new List<String>();
                    List<ItemsChoiceType> itemNames = new List<ItemsChoiceType>();

                    if (opportunity.VenueId != 0 && String.IsNullOrWhiteSpace(opportunity.RegionName))
                    {
                        items.Add(opportunity.VenueId.ToString());
                        itemNames.Add(ItemsChoiceType.VenueID);
                    }
                    if (!String.IsNullOrWhiteSpace(opportunity.RegionName))
                    {
                        items.Add(opportunity.RegionName);
                        itemNames.Add(ItemsChoiceType.RegionName);
                    }

                    opportunityDetail.Items = items.ToArray();
                    opportunityDetail.ItemsElementName = itemNames.ToArray();

                    opportunityDetail.OpportunityId = opportunity.OpportunityId;

                    opportunityDetails.Add(opportunityDetail);
                }

                courseDetailStructure.Opportunity = opportunityDetails.ToArray();

                // Get Venue details
                List<VenueDetail> venueDetails = new List<VenueDetail>();

                foreach (Venue venue in course.Venues)
                {
                    VenueDetail venueDetail = new VenueDetail();

                    venueDetail.VenueID = venue.VenueId.ToString();
                    venueDetail.VenueName = venue.VenueName;
                    venueDetail.Phone = venue.Phone;
                    venueDetail.Email = venue.Email;
                    venueDetail.Facilities = venue.Facilities;
                    venueDetail.Fax = venue.Fax;
                    venueDetail.Website = venue.Website;
                    venueDetail.VenueAddress = 
                        BuildAddressType(venue.AddressLine1, venue.AddressLine2, venue.Town, venue.County, venue.Postcode, venue.Latitude, venue.Longitude);

                    venueDetails.Add(venueDetail);
                }

                courseDetailStructure.Venue = venueDetails.ToArray();

                courseDetailStructures.Add(courseDetailStructure);
            }

            courseDetailOutput.CourseDetails = courseDetailStructures.ToArray();

            // Get original Request details
            courseDetailOutput.RequestDetails = new CourseDetailRequestStructure
            {
                CourseID = courseDetailInput.CourseID, 
                APIKey = courseDetailInput.APIKey
            };

            return courseDetailOutput;
        }

        /// <summary>
        /// Build SubjectBrowseOutput object from collection of Categories.
        /// </summary>
        /// <remarks>
        /// The FetchCategories stored procedure returns Category results in a specific order, e.g.
        /// Level 1:    .A - Art
        /// Level 2:        .A.1 - Animation / Multimedia Software
        /// Level 3:            CC.31 - Animation Software (Use)
        /// Level 3:            CC.3111 - 3d Studio (Animation Software)
        /// Level 3:            CC.3122 - Flash (Animation Software)
        /// Level 2:        .A.10 - Graphics Software
        /// Level 3:            CC.34 - Drawing / Painting Software (Use)
        /// Level 3:            CC.34 - Graphics Software (Use)
        /// Level 1:    .ACE - Adult and Continuing Education
        /// Level 2:        .ACE.2 - Job Preparation
        /// Level 3:            HC.1 - Career Planning
        /// ... etc.
        /// This code expects the results to be in this sequence.
        /// </remarks>
        /// <param name="categories">Categories collection.</param>
        /// <returns>Populated SubjectBrowseOutput object.</returns>
        private static SubjectBrowseOutput BuildSubjectBrowseOutput(List<Category> categories)
        {
            SubjectBrowseOutput subjectBrowseOutput = new SubjectBrowseOutput();
            List<Level1> level1s = new List<Level1>();
            List<Level2> level2s = new List<Level2>();
            List<CategoryInfo> level3s = new List<CategoryInfo>();

            Level1 currentLevel1 = null;
            Level2 currentLevel2 = null;
            CategoryInfo currentLevel3;

            foreach (Category category in categories)
            {
                switch (category.Level)
                {
                    case 1:
                        currentLevel1 = BuildLevel1(category);
                        level1s.Add(currentLevel1);

                        // clear the level 2's so we don't have the
                        // previous level 1 level 2's in there
                        level2s.Clear();
                        break;
                    case 2:
                        currentLevel2 = BuildLevel2(category);
                        if (currentLevel1.Level2 != null)
                        {
                            level2s = currentLevel1.Level2.ToList();
                        }
                        level2s.Add(currentLevel2);
                        currentLevel1.Level2 = level2s.ToArray();

                        // clear the level 3's so we don't have the
                        // previous level 2 level 3's in there
                        level3s.Clear();
                        break;
                    case 3:
                        currentLevel3 = BuildCategoryInfo(category);
                        if (currentLevel2.Level3 != null)
                        {
                            level3s = currentLevel2.Level3.ToList();
                        }
                        level3s.Add(currentLevel3);
                        currentLevel2.Level3 = level3s.ToArray();
                        break;
                    default:
                        break;
                }
            }

            subjectBrowseOutput.Level1 = level1s.ToArray();

            return subjectBrowseOutput;
        }

        /// <summary>
        /// Build Level1 object from Category data.
        /// </summary>
        /// <param name="category">Category data to use.</param>
        /// <returns>Populated Level1 object.</returns>
        private static Level1 BuildLevel1(Category category)
        {
            Level1 level1 = new Level1();

            level1.LDCS = BuildLdcsInfoType(category);
            level1.CourseCounts = category.CourseCount.ToString();

            return level1;
        }

        /// <summary>
        /// Build Level2 object from Category data.
        /// </summary>
        /// <param name="category">Category data to use.</param>
        /// <returns>Populated Level2 object.</returns>
        private static Level2 BuildLevel2(Category category)
        {
            Level2 level2 = new Level2();

            level2.LDCS = BuildLdcsInfoType(category);
            level2.CourseCounts = category.CourseCount.ToString();

            return level2;
        }

        /// <summary>
        /// Build CategoryInfo object from Category data.
        /// </summary>
        /// <param name="category">Category data to use.</param>
        /// <returns>Populated CategoryInfo object.</returns>
        private static CategoryInfo BuildCategoryInfo(Category category)
        {
            CategoryInfo categoryInfo = new CategoryInfo();

            categoryInfo.LDCS = BuildLdcsInfoType(category);
            categoryInfo.CourseCounts = category.CourseCount.ToString();

            return categoryInfo;
        }

        /// <summary>
        /// Build LDCSInfoType object from Category data.
        /// </summary>
        /// <param name="category">Category data to use.</param>
        /// <returns>LDCSInfoType object created.</returns>
        private static LDCSInfoType BuildLdcsInfoType(Category category)
        {
            LDCSInfoType ldcsInfoType = BuildLdcsInfoType(category.CategoryCode, category.Description);

            ldcsInfoType.Searchable = (category.Searchable == "Y") ? YesNoType.Yes : YesNoType.No;

            return ldcsInfoType;
        }

        /// <summary>
        /// Build LDCSInfoType object from code and description data.
        /// </summary>
        /// <param name="code">Category code.</param>
        /// <param name="description">Category description.</param>
        /// <returns>LDCSInfoType object created.</returns>
        private static LDCSInfoType BuildLdcsInfoType(string code, string description)
        {
            LDCSInfoType ldcsInfoType = new LDCSInfoType();

            ldcsInfoType.LDCSCode = code;
            ldcsInfoType.LDCSDesc = description;

            return ldcsInfoType;
        }

        /// <summary>
        /// Build AddressType object.
        /// </summary>
        /// <param name="addressLine1">Address Line 1.</param>
        /// <param name="addressLine2">Address Line 2.</param>
        /// <param name="town">Town.</param>
        /// <param name="county">County.</param>
        /// <param name="postcode">Postcode.</param>
        /// <returns>AddressType object created.</returns>
        private static AddressType BuildAddressType(string addressLine1, string addressLine2, string town, string county, string postcode)
        {
            AddressType addressType = new AddressType();

            addressType.Address_line_1 = addressLine1;
            addressType.Address_line_2 = addressLine2;
            addressType.Town = town;
            addressType.County = county;
            addressType.PostCode = postcode;

            return addressType;
        }

        /// <summary>
        /// Build AddressType object.
        /// </summary>
        /// <param name="addressLine1">Address Line 1.</param>
        /// <param name="addressLine2">Address Line 2.</param>
        /// <param name="town">Town.</param>
        /// <param name="county">County.</param>
        /// <param name="postcode">Postcode.</param>
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude</param>
        /// <returns>AddressType object created.</returns>
        private static AddressType BuildAddressType(string addressLine1, string addressLine2, string town, string county, string postcode, string latitude, string longitude)
        {
            AddressType addressType = new AddressType();

            addressType.Address_line_1 = addressLine1;
            addressType.Address_line_2 = addressLine2;
            addressType.Town = town;
            addressType.County = county;
            addressType.PostCode = postcode;
            addressType.Latitude = latitude;
            addressType.Longitude = longitude;

            return addressType;
        }

        /// <summary>
        /// Build StartDateType object
        /// </summary>
        /// <param name="opportunity">Opportunity</param>
        /// <returns>StartDateType object created.</returns>
        private static StartDateType BuildStartDateType(Opportunity opportunity)
        {
            StartDateType startDateType = new StartDateType();

            if (opportunity.StartDate != null && !string.IsNullOrEmpty(opportunity.StartDate))
            {
                startDateType.ItemElementName = ItemChoiceType.Date;
                startDateType.Item = opportunity.StartDate;
            }
            else
            {
                startDateType.ItemElementName = ItemChoiceType.DateDesc;
                startDateType.Item = opportunity.StartDateDescription;
            }

            return startDateType;
        }

        /// <summary>
        /// <summary>
        /// Retrieves CourseDetailAdultLRStatus from supplied string value.
        /// </summary>
        /// <param name="status">status as a string.</param>
        /// <returns>CourseDetailAdultLRStatus represented by status string.</returns>
        private static CourseDetailAdultLRStatus GetAdultLRStatus(string status)
        {
            CourseDetailAdultLRStatus adultLRStatus = new CourseDetailAdultLRStatus();

            switch (status)
            {
                case "Invalid":
                    adultLRStatus = CourseDetailAdultLRStatus.Invalid;
                    break;
                case "NotNewStarts":
                    adultLRStatus = CourseDetailAdultLRStatus.NotNewStarts;
                    break;
                case "Valid":
                    adultLRStatus = CourseDetailAdultLRStatus.Valid;
                    break;
                default:
                    break;
            }

            return adultLRStatus;
        }

        /// <summary>
        /// Retrieves CourseDetailDataType from supplied string value.
        /// </summary>
        /// <param name="type">type as a string.</param>
        /// <returns>CourseDetailDataType represented by type string.</returns>
        private static CourseDetailDataType GetDataType(string type)
        {
            CourseDetailDataType dataType = new CourseDetailDataType();

            switch (type)
            {
                case "LADType1":
                    dataType = CourseDetailDataType.LADType1;
                    break;
                case "LADType2":
                    dataType = CourseDetailDataType.LADType2;
                    break;
                case "NoLADType3":
                    dataType = CourseDetailDataType.NoLADType3;
                    break;
                case "UCAS":
                    dataType = CourseDetailDataType.UCAS;
                    break;
                default:
                    break;
            }

            return dataType;
        }

        /// <summary>
        /// Retrieves CourseDetailERAppStatus from supplied string value.
        /// </summary>
        /// <param name="status">status as a string.</param>
        /// <returns>CourseDetailERAppStatus represented by status string.</returns>
        private static CourseDetailERAppStatus GetERAppStatus(string status)
        {
            CourseDetailERAppStatus erAppStatus = new CourseDetailERAppStatus();

            switch (status)
            {
                case "Invalid":
                    erAppStatus = CourseDetailERAppStatus.Invalid;
                    break;
                case "NotNewStarts":
                    erAppStatus = CourseDetailERAppStatus.NotNewStarts;
                    break;
                case "Valid":
                    erAppStatus = CourseDetailERAppStatus.Valid;
                    break;
                default:
                    break;
            }

            return erAppStatus;
        }

        /// <summary>
        /// Retrieves CourseDetailERTTGStatus from supplied string value.
        /// </summary>
        /// <param name="status">status as a string.</param>
        /// <returns>CourseDetailERTTGStatus represented by status string.</returns>
        private static CourseDetailERTTGStatus GetERTtgStatus(string status)
        {
            CourseDetailERTTGStatus erTtgStatus = new CourseDetailERTTGStatus();

            switch (status)
            {
                case "Invalid":
                    erTtgStatus = CourseDetailERTTGStatus.Invalid;
                    break;
                case "NotNewStarts":
                    erTtgStatus = CourseDetailERTTGStatus.NotNewStarts;
                    break;
                case "Valid":
                    erTtgStatus = CourseDetailERTTGStatus.Valid;
                    break;
                default:
                    break;
            }

            return erTtgStatus;
        }

        /// <summary>
        /// Retrieves CourseDetailIndependentLivingSkills from supplied string value.
        /// </summary>
        /// <param name="skills">skills as a string.</param>
        /// <returns>CourseDetailIndependentLivingSkills represented by skills string.</returns>
        private static CourseDetailIndependentLivingSkills GetIndependentLivingSkills(string skills)
        {
            return (skills == "Y") ? CourseDetailIndependentLivingSkills.Y : CourseDetailIndependentLivingSkills.N;
        }

        /// <summary>
        /// Retrieves CourseDetailOtherFundingNonFundedStatus from supplied string value.
        /// </summary>
        /// <param name="status">status as a string.</param>
        /// <returns>CourseDetailOtherFundingNonFundedStatus represented by status string.</returns>
        private static CourseDetailOtherFundingNonFundedStatus GetOtherFundingNonFundedStatus(string status)
        {
            return (status == "Valid") ? 
                CourseDetailOtherFundingNonFundedStatus.Valid : CourseDetailOtherFundingNonFundedStatus.NotNewStarts;
        }

        /// <summary>
        /// Retrieves CourseDetailSkillsForLifeFlag from supplied string value.
        /// </summary>
        /// <param name="flag">flag as a string.</param>
        /// <returns>CourseDetailSkillsForLifeFlag represented by flag string.</returns>
        private static CourseDetailSkillsForLifeFlag GetSkillsForLifeFlag(string flag)
        {
            return (flag == "Y") ? CourseDetailSkillsForLifeFlag.Y : CourseDetailSkillsForLifeFlag.N;
        }

        /// <summary>
        /// Retrieves OpportunityDetailApplicationAcceptedThroughoutYear from supplied string value.
        /// </summary>
        /// <param name="flag">flag as a string.</param>
        /// <returns>OpportunityDetailApplicationAcceptedThroughoutYear represented by flag string.</returns>
        private static OpportunityDetailApplicationAcceptedThroughoutYear GetApplicationAcceptedThroughoutYear(string flag)
        {
            return (flag == "Y") ? 
                OpportunityDetailApplicationAcceptedThroughoutYear.Y : OpportunityDetailApplicationAcceptedThroughoutYear.N;
        }

        #endregion Private Methods

    }
}
