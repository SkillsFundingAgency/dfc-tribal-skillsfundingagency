using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using IMS.NCS.CourseSearchService.Entities;
using IMS.NCS.CourseSearchService.DatabaseContext;
using System.Diagnostics;

namespace IMS.NCS.CourseSearchService.Sql.Gateways
{
    public class CourseGateway : ICourseGateway
    {
        #region Public methods

        /// <summary>
        /// Get all categories from DB
        /// </summary>
        /// <returns></returns>
        public List<Category> GetCategories(String APIKey)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                Boolean isPublicAPI = ConfigurationManager.AppSettings["IncludeUCASData"].ToLower() != "true";
                if (isPublicAPI && String.IsNullOrEmpty(APIKey.Trim()))
                {
                    return new List<Category>();
                }

                List<Category> categoryCode = CacheHelper.GetCategories(APIKey);

                if (categoryCode == null)
                {
                    using (SFA_SearchAPIEntities db = new SFA_SearchAPIEntities())
                    {
                        categoryCode = db.API_CategoryCode_GetAll_v2(isPublicAPI ? 1 : 0, APIKey).Select(x => new Category
                        {
                            CategoryCode = x.CategoryCodeName,
                            Description = x.Description,
                            ParentCategoryCode = x.ParentCategoryCode,
                            CourseCount = x.TotalCourses,
                            Searchable = x.IsSearchable ? "Y" : "N",
                            Level = x.Level.HasValue ? x.Level.Value : 1
                        }).ToList();
                    }

                    CacheHelper.SaveCategories(APIKey, categoryCode);
                }

                new DBHelper().LogProviderRequestResponseLog(DBHelper.ServiceMethodName.GetCategories, stopwatch.ElapsedMilliseconds, string.Empty, DBHelper.Serialize(categoryCode), isPublicAPI, APIKey, categoryCode.Count);

                return categoryCode;
            }
            catch (Exception ex)
            {
                LogException(ex, "GetCategories");
                throw;
            }
        }

        /// <summary>
        /// Load course details 
        /// </summary>
        /// <param name="courseIds">Course Ids to search</param>
        /// <param name="APIKey"></param>
        /// <returns>List of courses matching the course Ids</returns>
        public List<Course> GetCourseDetails(List<long> courseIds, String APIKey)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                List<Course> courses = new List<Course>();
                Boolean isPublicAPI = ConfigurationManager.AppSettings["IncludeUCASData"].ToLower() != "true";
                if (isPublicAPI && String.IsNullOrEmpty(APIKey.Trim()))
                {
                    return new List<Course>();
                }

                foreach (int courseId in courseIds)
                {
                    var courseResult = CacheHelper.GetCourseDetailsByCourseId(courseId, isPublicAPI ? 1 : 0, APIKey);

                    if (courseResult == null)
                    {
                        using (SFA_SearchAPIEntities db = new SFA_SearchAPIEntities())
                        {
                            var entity = db.API_Course_GetById_v2(courseId, isPublicAPI ? 1 : 0, APIKey).ToList();

                            if (entity.Count > 0)
                            {
                                var entityCourse = entity[0];

                                Dictionary<string, string> categoryCodeList = CacheHelper.CategoryCodesList(isPublicAPI ? 1 : 0, APIKey);

                                courseResult = new Course
                                {
                                    Venues = new List<Venue>(),
                                    Opportunities = new List<Opportunity>()
                                };

                                PopulateCourseDetails(categoryCodeList, entityCourse, courseResult);

                                PopulateProviderDetails(entityCourse, courseResult);

                                PopulateOpportunityDetails(entity, courseResult);

                                PopulateVenueDetails(courseResult);

                                CacheHelper.SaveCourseDetailsByCourseId(courseId, isPublicAPI ? 1 : 0, APIKey, courseResult);
                            }
                        }
                    }
                    courses.Add(courseResult);
                }

                new DBHelper().LogProviderRequestResponseLog(DBHelper.ServiceMethodName.GetCourseDetails, stopwatch.ElapsedMilliseconds, DBHelper.Serialize(courseIds), DBHelper.Serialize(courses), isPublicAPI, APIKey, courses.Count);

                return courses;
            }
            catch (Exception ex)
            {
                LogException(ex, "GetCourseDetails");
                throw;
            }
        }

        /// <summary>
        /// Get matching courses based on request
        /// </summary>
        /// <param name="request">search parameters</param>
        /// <returns>Matching course reponse object</returns>
        public CourseListResponse GetCourseList(CourseListRequest request)
        {
            try
            {
                Boolean isPublicAPI = ConfigurationManager.AppSettings["IncludeUCASData"].ToLower() != "true";
                Boolean v3SearchEnhancements = ConfigurationManager.AppSettings["UseV3SearchEnhancements"].ToLower() == "true";
                Int32 cutoffPercentage;
                Int32.TryParse(ConfigurationManager.AppSettings["RemoveLowestRankedResultPercentage"], out cutoffPercentage);
                Boolean providerFreeTextMatch = ConfigurationManager.AppSettings["ProviderFreeTextMatch"].ToLower() == "true";
                Boolean courseFreeTextMatch = ConfigurationManager.AppSettings["CourseFreeTextMatch"].ToLower() == "true";
                Boolean searchCourseSummary = ConfigurationManager.AppSettings["SearchCourseSummary"].ToLower() == "true";
                Boolean searchQualificationTitle = ConfigurationManager.AppSettings["SearchQualificationTitle"].ToLower() == "true";

                if (isPublicAPI && String.IsNullOrEmpty(request.APIKey.Trim()))
                {
                    return new CourseListResponse();
                }

                Stopwatch stopwatch = Stopwatch.StartNew();
                string courseCacheKey = CreateCacheKey(request, v3SearchEnhancements, providerFreeTextMatch, courseFreeTextMatch, searchCourseSummary, searchQualificationTitle);

                DateTime earlierStartDate;
                DateTime.TryParse(request.EarliestStartDate, out earlierStartDate);

                DateTime applyUntil;
                DateTime.TryParse(request.AppClosedFlag, out applyUntil);

                CourseListResponse courseResult = CacheHelper.GetCourseListResponse(courseCacheKey);
                if (courseResult == null)
                {
                    courseResult = LoadGetCourseList(request, earlierStartDate, applyUntil, isPublicAPI, v3SearchEnhancements, cutoffPercentage, providerFreeTextMatch, courseFreeTextMatch, searchCourseSummary, searchQualificationTitle);

                    CacheHelper.SaveCourseListResponse(courseCacheKey, courseResult);
                }

                new DBHelper().LogProviderRequestResponseLog(DBHelper.ServiceMethodName.GetCourseList, stopwatch.ElapsedMilliseconds, DBHelper.Serialize(request), DBHelper.Serialize(courseResult), isPublicAPI, request.APIKey, courseResult.NumberOfRecords);

                return courseResult;
            }
            catch (Exception ex)
            {
                LogException(ex, "GetCourseList");
                throw;
            }
        }

        #endregion

        #region private methods

        private static void LogException(Exception ex, String method)
        {
            Log log = new Log
            {
                DateTimeUtc = DateTime.UtcNow,
                MachineName = Environment.MachineName,
                Method = method,
                Details = String.Format("{0}\r\rStack Trace: {1}", ex.Message, ex.StackTrace ?? String.Empty)
            };

            if (ex.InnerException != null)
            {
                log.Details += String.Format("\r\rInner Exception: {0}\r\rInner Stack Trace: {1}", ex.InnerException.Message, ex.InnerException.StackTrace ?? String.Empty);
            }

            using (SFA_SearchAPIEntities db = new SFA_SearchAPIEntities())
            {
                db.Logs.Add(log);
                db.SaveChanges();
            }
        }

        private void PopulateCourseDetails(Dictionary<string, string> categoryCodeList, API_Course_GetById_v2_Result entityCourse, Course course)
        {
            course.CourseId = entityCourse.CourseId;
            course.CourseTitle = entityCourse.CourseTitle;
            course.QualificationType = entityCourse.QualificationTypeRef;
            course.QualificationLevel = entityCourse.Qualification_Level;
            course.LdcsCode1 = entityCourse.LDCS1;
            course.LdcsCode2 = entityCourse.LDCS2;
            course.LdcsCode3 = entityCourse.LDCS3;
            course.LdcsCode4 = entityCourse.LDCS4;
            course.LdcsCode5 = entityCourse.LDCS5;
            course.LdcsDesc1 = GetLdscDescription(entityCourse.LDCS1, categoryCodeList);
            course.LdcsDesc2 = GetLdscDescription(entityCourse.LDCS2, categoryCodeList);
            course.LdcsDesc3 = GetLdscDescription(entityCourse.LDCS3, categoryCodeList);
            course.LdcsDesc4 = GetLdscDescription(entityCourse.LDCS4, categoryCodeList);
            course.LdcsDesc5 = GetLdscDescription(entityCourse.LDCS5, categoryCodeList);
            course.TariffRequired = entityCourse.UcasTariffPoints.HasValue ? entityCourse.UcasTariffPoints.Value.ToString() : "";

            course.CourseSummary = entityCourse.CourseSummary;
            course.AwardingBody = entityCourse.AwardingOrganisationName;
            course.AssessmentMethod = entityCourse.AssessmentMethod;
            course.BookingUrl = entityCourse.BookingUrl;
            course.CreditValue = entityCourse.UcasTariffPoints.HasValue ? entityCourse.UcasTariffPoints.Value.ToString() : string.Empty;
            course.DataType = entityCourse.QualificationDataType;
            course.EntryRequirements = entityCourse.EntryRequirements;
            course.EquipmentRequired = entityCourse.EquipmentRequired;
            course.LadId = entityCourse.LearningAimRef;
            course.QualificationReferenceAuthority = entityCourse.QualificationRefAuthority;
            course.QualificationReference = entityCourse.QualificationRef;
            course.QualificationTitle = entityCourse.QualificationTitle;
            course.Url = entityCourse.Url;
            course.DataType = entityCourse.QualificationDataType;

            if (!string.IsNullOrEmpty(entityCourse.LearningAimRef))
            {
                using (SFA_SearchAPIEntities db = new SFA_SearchAPIEntities())
                {
                    var larEntity = db.API_LearningAim_GetById(entityCourse.LearningAimRef).ToList().FirstOrDefault();

                    if (larEntity != null)
                    {
                        course.AccreditationEndDate = larEntity.AccreditationEndDate.HasValue ? larEntity.AccreditationEndDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                        course.ERAppStatus = larEntity.ErAppStatus;
                        course.ERTtgStatus = larEntity.ErTtgStatus;
                        course.IndependentLivingSkills = larEntity.IndepLivingSkills;
                        course.Level2EntitlementDescription = larEntity.Level2EntitlementCatDesc;
                        course.Level3EntitlementDescription = larEntity.Level3EntitlementCatDesc;
                        course.OtherFundingNonFundedStatus = larEntity.OtherfundingNonFundedStatus;
                        course.QcaGuidedLearningHours = larEntity.QcaGlh;
                        course.SectorLeadBodyDescription = larEntity.SectorLeadBodyDesc;
                        course.SkillsForLifeFlag = larEntity.SkillsForLife;
                        course.SkillsForLifeTypeDescription = larEntity.SkillsForLife;
                        course.TariffRequired = entityCourse.UcasTariffPoints.HasValue ? entityCourse.UcasTariffPoints.ToString() : "";
                        course.AdultLRStatus = larEntity.AdultlrStatus;
                    }
                }
            }
        }

        private void PopulateProviderDetails(API_Course_GetById_v2_Result entityCourse, Course course)
        {
            var provider = new Provider
            {
                ProviderId = entityCourse.ProviderId.ToString(),
                ProviderName = entityCourse.ProviderName,
                AddressLine1 = entityCourse.AddressLine1,
                AddressLine2 = entityCourse.AddressLine2,
                Town = entityCourse.Town,
                County = entityCourse.County,
                Postcode = entityCourse.Postcode,
                Phone = entityCourse.Telephone,
                Website = entityCourse.Website,
                Ukprn = entityCourse.Ukprn.ToString(),
                Fax = entityCourse.Fax,
                Email = entityCourse.Email,
                Upin = entityCourse.UPIN.HasValue ? entityCourse.UPIN.Value.ToString() : string.Empty,
                TFPlusLoans = entityCourse.Loans24Plus,
                DFE1619Funded = entityCourse.CourseDfEFunded ?? false,
                FEChoices_LearnerDestination = entityCourse.FEChoices_LearnerDestination,
                FEChoices_LearnerSatisfaction = entityCourse.FEChoices_LearnerSatisfaction,
                FEChoices_EmployerSatisfaction = entityCourse.FEChoices_EmployerSatisfaction

            };
            course.Provider = provider;
        }

        private void PopulateOpportunityDetails(IEnumerable<API_Course_GetById_v2_Result> entity, Course course)
        {
            //Populate Opportunity Details//
            foreach (var entityCourse in entity)
            {
                Boolean opportunityExists = false;
                foreach (OpportunityResult opportunity in course.Opportunities)
                {
                    if (opportunity.OpportunityId == entityCourse.OpportunityId.ToString())
                    {
                        // Opportunity already exists in results so update A10 code
                        if (!String.IsNullOrWhiteSpace(entityCourse.A10FundingCode) && !opportunity.A10.Any(x => x == entityCourse.A10FundingCode))
                        {
                            opportunity.A10.Add(entityCourse.A10FundingCode);
                        }
                        opportunityExists = true;
                        break;
                    }
                }

                if (!opportunityExists)
                {
                    var opportunityResult = new OpportunityResult
                    {
                        CourseId = entityCourse.CourseId,
                        AttendanceMode = entityCourse.AttendanceModeBulkUploadRef,
                        AttendancePattern = entityCourse.AttendancePatternBulkUploadRef,
                        DurationDescription = entityCourse.Duration_Description,
                        DurationValue = entityCourse.DurationValue.HasValue ? entityCourse.DurationValue.Value : 0,
                        DurationUnit = entityCourse.DurationUnit,
                        Price = entityCourse.Price.HasValue ? entityCourse.Price.Value.ToString("c") : string.Empty,
                        PriceDescription = entityCourse.PriceAsText,
                        StartDate = entityCourse.StartDate.HasValue ? entityCourse.StartDate.Value.ToString("dd MMMM yyyy") : String.Empty,
                        StartDateDescription = entityCourse.StartDateDescription,
                        StudyMode = entityCourse.StudyModeBulkUploadRef,
                        Timetable = entityCourse.TimeTable,
                        RegionName = entityCourse.RegionName,
                        VenueId = entityCourse.VenueId.HasValue && String.IsNullOrWhiteSpace(entityCourse.RegionName) ? entityCourse.VenueId.Value : 0,
                        ApplicationAcceptedThroughoutYear = entityCourse.CanApplyAllYear ? "Y" : string.Empty,
                        ApplyFromDate =
                        entityCourse.ApplyFromDate.HasValue
                            ? entityCourse.ApplyFromDate.Value.ToString("dd-MMM-yy")
                            : string.Empty,
                        ApplyTo = entityCourse.ApplyTo,
                        ApplyUntilDate =
                        entityCourse.ApplyUntilDate.HasValue
                            ? entityCourse.ApplyUntilDate.Value.ToString("yyyyMMdd HH:mm:ss")
                            : string.Empty,
                        ApplyUntilDescription = entityCourse.ApplyUntilText,
                        EndDate =
                        entityCourse.EndDate.HasValue ? entityCourse.EndDate.Value.ToString("dd MMMM yyyy") : string.Empty,
                        EnquireTo = entityCourse.EnquiryTo,
                        LanguageOfAssessment = entityCourse.LanguageOfAssessment,
                        LanguageOfInstruction = entityCourse.LanguageOfInstruction,
                        PlacesAvailable = entityCourse.PlacesAvailable.HasValue ? entityCourse.PlacesAvailable.Value : 0,
                        ProviderOpportunityId = entityCourse.ProviderOwnCourseInstanceRef,
                        Url = entityCourse.CourseInstanceUrl,
                        OpportunityId = entityCourse.OpportunityId.ToString(),
                        DfE1619Funded = entityCourse.CourseDfEFunded ?? false,
                        A10 = new List<String>()
                    };
                    if (!String.IsNullOrWhiteSpace(entityCourse.A10FundingCode))
                    {
                        opportunityResult.A10.Add(entityCourse.A10FundingCode);
                    }
                    course.Opportunities.Add(opportunityResult);
                }
            }
            course.NumberOfOpportunities = course.Opportunities.Count();
        }

        private void PopulateVenueDetails(Course course)
        {
            Boolean isPublicAPI = ConfigurationManager.AppSettings["IncludeUCASData"].ToLower() != "true";

            //Populate Venue details//
            foreach (var opportunity in course.Opportunities)
            {
                using (SFA_SearchAPIEntities db = new SFA_SearchAPIEntities())
                {
                    var venueEntity = db.API_Venue_GetById((int) opportunity.VenueId, isPublicAPI ? 1 : 0).FirstOrDefault();

                    if (venueEntity != null)
                    {
                        if (!course.Venues.Any(c => c.VenueId.Equals(opportunity.VenueId)))
                        {
                            var venue = new Venue
                            {
                                VenueName = venueEntity.VenueName,
                                AddressLine1 = venueEntity.AddressLine1,
                                AddressLine2 = venueEntity.AddressLine2,
                                Town = venueEntity.Town,
                                County = venueEntity.County,
                                Postcode = venueEntity.Postcode,
                                Latitude = venueEntity.Latitude.ToString(),
                                Longitude = venueEntity.Longitude.ToString(),
                                Phone = venueEntity.Telephone,
                                Email = venueEntity.Email,
                                Website = venueEntity.Website,
                                Fax = venueEntity.Fax,
                                Facilities = venueEntity.Facilities,
                                VenueId = opportunity.VenueId
                            };
                            course.Venues.Add(venue);
                        }
                    }
                }
            }
        }

        private CourseListResponse LoadGetCourseList(CourseListRequest request, DateTime earlierStartDate, DateTime applyUntil, bool isPublicAPI, bool v3SearchEnhancements, int? cutoffPercentage, bool providerFreeTextMatch, bool courseFreeTextMatch, bool searchCourseSummary, bool searchQualificationTitle)
        {
            using (SFA_SearchAPIEntities db = new SFA_SearchAPIEntities())
            {
                List<API_CourseList_Get_v2_Result> courseListItems;

                if (!v3SearchEnhancements)
                {
                    courseListItems =
                        db.API_CourseList_Get_v2(GetNullableString(request.SubjectKeyword),
                            GetNullableString(request.ProviderKeyword),
                            GetNullableString(request.QualificationTypes),
                            GetNullableString(request.QualificationLevels),
                            GetNullableString(request.StudyModes),
                            GetNullableString(request.AttendanceModes),
                            GetNullableString(request.AttendancePatterns),
                            !earlierStartDate.Equals(DateTime.MinValue) ? earlierStartDate : (DateTime?) null,
                            GetNullableString(request.LdcsCategoryCode),
                            !string.IsNullOrEmpty(request.ERTtgStatus) &&
                            request.ERTtgStatus.Equals("Y", StringComparison.CurrentCultureIgnoreCase),
                            GetNullableString(request.A10Codes),
                            !string.IsNullOrEmpty(request.IlsFlag) &&
                            request.IlsFlag.Equals("Y", StringComparison.CurrentCultureIgnoreCase),
                            GetNullableString(request.SflFlag),
                            GetNullableString(request.ERTtgStatus),
                            GetNullableString(request.ERAppStatus),
                            GetNullableString(request.AdultLRStatus),
                            GetNullableString(request.OtherFundingStatus),
                            !applyUntil.Equals(DateTime.MinValue) ? applyUntil : (DateTime?) null,
                            !string.IsNullOrEmpty(request.FlexStartFlag) &&
                            request.FlexStartFlag.Equals("Y", StringComparison.CurrentCultureIgnoreCase),
                            GetNullableString(request.Location),
                            request.Distance > 0 ? request.Distance : (float?) null,
                            request.PageNumber > 0 ? (int) request.PageNumber : 0,
                            request.RecordsPerPage > 0 ? request.RecordsPerPage : (int?) null,
                            request.SortBy,
                            request.ProviderId > 0 ? request.ProviderId : (Int32?) null,
                            isPublicAPI ? 1 : 0,
                            String.IsNullOrEmpty(request.DFE1619Funded)
                                ? (Boolean?) null
                                : request.DFE1619Funded.Equals("Y", StringComparison.CurrentCultureIgnoreCase),
                            request.APIKey)
                            .ToList();
                }
                else
                {
                    SearchQuery query = new SearchQuery(GetNullableString(request.SubjectKeyword));

                    courseListItems =
                        db.API_CourseList_Get_v3(String.Join(" ", query.Include),
                            String.Join(" ", query.Exclude),
                            String.Join("|", query.IncludeExact),
                            String.Join("|", query.ExcludeExact),
                            GetNullableString(request.ProviderKeyword),
                            GetNullableString(request.QualificationTypes),
                            GetNullableString(request.QualificationLevels),
                            GetNullableString(request.StudyModes),
                            GetNullableString(request.AttendanceModes),
                            GetNullableString(request.AttendancePatterns),
                            !earlierStartDate.Equals(DateTime.MinValue) ? earlierStartDate : (DateTime?) null,
                            GetNullableString(request.LdcsCategoryCode),
                            !string.IsNullOrEmpty(request.ERTtgStatus) &&
                            request.ERTtgStatus.Equals("Y", StringComparison.CurrentCultureIgnoreCase),
                            GetNullableString(request.A10Codes),
                            !string.IsNullOrEmpty(request.IlsFlag) &&
                            request.IlsFlag.Equals("Y", StringComparison.CurrentCultureIgnoreCase),
                            GetNullableString(request.SflFlag),
                            GetNullableString(request.ERTtgStatus),
                            GetNullableString(request.ERAppStatus),
                            GetNullableString(request.AdultLRStatus),
                            GetNullableString(request.OtherFundingStatus),
                            !applyUntil.Equals(DateTime.MinValue) ? applyUntil : (DateTime?) null,
                            !string.IsNullOrEmpty(request.FlexStartFlag) &&
                            request.FlexStartFlag.Equals("Y", StringComparison.CurrentCultureIgnoreCase),
                            GetNullableString(request.Location),
                            request.Distance > 0 ? request.Distance : (float?) null,
                            request.PageNumber > 0 ? (int) request.PageNumber : 0,
                            request.RecordsPerPage > 0 ? request.RecordsPerPage : (int?) null,
                            request.SortBy,
                            request.ProviderId > 0 ? request.ProviderId : (Int32?) null,
                            isPublicAPI ? 1 : 0,
                            String.IsNullOrEmpty(request.DFE1619Funded)
                                ? (Boolean?) null
                                : request.DFE1619Funded.Equals("Y", StringComparison.CurrentCultureIgnoreCase),
                            request.APIKey,
                            cutoffPercentage == 0 ? null : cutoffPercentage,
                            providerFreeTextMatch ? 1 : 0,
                            courseFreeTextMatch ? 1 : 0,
                            searchCourseSummary ? 1 : 0,
                            searchQualificationTitle ? 1 : 0)
                            .ToList();
                }

                var response = new CourseListResponse
                {
                    NumberOfRecords = courseListItems.Count > 0 ? courseListItems.FirstOrDefault().RecordCount.Value : 0,
                    Courses = new List<Course>()
                };

                var categoryCodeList = new DBHelper().LoadCategoryCodes(isPublicAPI ? 1 : 0, request.APIKey);

                foreach (var courseListItem in courseListItems)
                {
                    var course = new Course
                    {
                        Provider = new Provider
                        {
                            ProviderName = courseListItem.ProviderName,
                            TFPlusLoans = courseListItem.Loans24Plus,
                            DFE1619Funded = courseListItem.ProviderDfEFunded,
                            FEChoices_LearnerDestination = courseListItem.FEChoices_LearnerDestination,
                            FEChoices_LearnerSatisfaction = courseListItem.FEChoices_LearnerSatisfaction,
                            FEChoices_EmployerSatisfaction = courseListItem.FEChoices_EmployerSatisfaction
                        },
                        CourseId = courseListItem.CourseId,
                        CourseTitle = courseListItem.CourseTitle,
                        QualificationType = courseListItem.QualificationTypeRef,
                        QualificationLevel = courseListItem.QualificationBulkUploadRef,
                        LdcsCode1 = courseListItem.LDCS1,
                        LdcsCode2 = courseListItem.LDCS2,
                        LdcsCode3 = courseListItem.LDCS3,
                        LdcsCode4 = courseListItem.LDCS4,
                        LdcsCode5 = courseListItem.LDCS5,
                        LdcsDesc1 = GetLdscDescription(courseListItem.LDCS1, categoryCodeList),
                        LdcsDesc2 = GetLdscDescription(courseListItem.LDCS2, categoryCodeList),
                        LdcsDesc3 = GetLdscDescription(courseListItem.LDCS3, categoryCodeList),
                        LdcsDesc4 = GetLdscDescription(courseListItem.LDCS4, categoryCodeList),
                        LdcsDesc5 = GetLdscDescription(courseListItem.LDCS5, categoryCodeList),
                        NumberOfOpportunities = 1,
                        CourseSummary = courseListItem.CourseSummary,
                        Opportunities = new List<Opportunity>()
                    };

                    Opportunity opportunity = new Opportunity
                    {
                        OpportunityId = courseListItem.CourseInstanceId.ToString(),
                        StudyMode = courseListItem.StudyModeBulkUploadRef,
                        AttendanceMode = courseListItem.AttendanceModeBulkUploadRef,
                        AttendancePattern = courseListItem.AttendancePatternBulkUploadRef,
                        StartDate = courseListItem.StartDate.HasValue ? courseListItem.StartDate.Value.ToString("dd MMMM yyyy") : String.Empty,
                        StartDateDescription = courseListItem.StartDateDescription,
                        EndDate = courseListItem.EndDate.HasValue ? courseListItem.EndDate.Value.ToString("dd MMMM yyyy") : string.Empty,
                        RegionName = courseListItem.RegionName,
                        Distance = courseListItem.Distance.ToString(),
                        DurationUnit = courseListItem.DurationUnitBulkUploadRef,
                        DurationDescription = courseListItem.DurationAsText,
                        DurationValue = courseListItem.DurationUnitId.HasValue ? courseListItem.DurationUnitId.Value : 0,
                        DfE1619Funded = courseListItem.CourseDfEFunded ?? false,
                        Venue = new Venue
                        {
                            VenueName = courseListItem.VenueName,
                            AddressLine1 = courseListItem.AddressLine1,
                            AddressLine2 = courseListItem.AddressLine2,
                            Town = courseListItem.Town,
                            County = courseListItem.County,
                            Postcode = courseListItem.Postcode,
                            Latitude = courseListItem.Latitude.HasValue ? courseListItem.Latitude.Value.ToString() : string.Empty,
                            Longitude = courseListItem.Longitude.HasValue ? courseListItem.Longitude.Value.ToString() : string.Empty
                        },
                        A10 = new List<String>()
                    };
                    opportunity.A10.Add(courseListItem.A10FundingCode);
                    course.Opportunities.Add(opportunity);

                    response.Courses.Add(course);
                }

                return response;
            }
        }

        private static string GetNullableString(string stringValue)
        {
            return string.IsNullOrEmpty(stringValue) ? null : stringValue.Trim();
        }

        private static string GetLdscDescription(string ldscCode, Dictionary<string, string> categoryCodeList)
        {
            return !string.IsNullOrEmpty(ldscCode) && categoryCodeList.ContainsKey(ldscCode) ? categoryCodeList[ldscCode] : string.Empty;
        }

        private static string CreateCacheKey(CourseListRequest request, bool v3SearchEnhancements, bool providerFreeTextMatch, bool courseFreeTextMatch, bool searchCourseSummary, bool searchQualificationTitle)
        {
            string key = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}_{13}_{14}_{15}_{16}_{17}_{18}_{19}_{20}_{21}_{22}_{23}_{24}_{25}_{26}_{27}_{28}_{29}_{30}_{31}_{32}_{33}_{34}_{35}",
                request.A10Codes,
                request.AdultLRStatus,
                request.AppClosedFlag,
                request.AttendanceModes,
                request.AttendancePatterns,
                request.Distance,
                request.DistanceSpecified,
                request.EarliestStartDate,
                request.ERAppStatus,
                request.ERTtgStatus,
                request.FlexStartFlag,
                request.IesFlag,
                request.IlsFlag,
                request.LdcsCategoryCode,
                request.Location,
                request.OtherFundingStatus,
                request.PageNumber,
                request.ProviderId,
                request.ProviderKeyword,
                request.QualificationLevels,
                request.QualificationTypes,
                request.RecordsPerPage,
                request.SflFlag,
                request.StudyModes,
                request.SubjectKeyword,
                request.TqsFlag,
                request.TtgFlag,
                request.ProviderId,
                request.SortBy,
                request.DFE1619Funded,
                request.APIKey,
                v3SearchEnhancements ? 1 : 0,
                providerFreeTextMatch ? 1 : 0,
                courseFreeTextMatch ? 1 : 0,
                searchCourseSummary ? 1 : 0,
                searchQualificationTitle ? 1 : 0);

            return key;
        }

        #endregion

        #region methods not in use

        public void RecordSearchTime(string columnFlag, string searchHeaderId)
        {
            //May be used later when loggin will be done!
        }

        #endregion
    }
}