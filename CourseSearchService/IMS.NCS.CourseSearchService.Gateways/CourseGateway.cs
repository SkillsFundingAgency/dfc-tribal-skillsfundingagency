using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using IMS.NCS.CourseSearchService.Common;
using IMS.NCS.CourseSearchService.Entities;

namespace IMS.NCS.CourseSearchService.Gateways
{
    /// <summary>
    /// Implementaion of Provider gateway functions.
    /// </summary>
    public class CourseGateway : ICourseGateway
    {
        #region Public Methods

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A collection of Category entities.</returns>
        public List<Category> GetCategories()
        {
            OracleConnection connection = null;
            OracleCommand command = null;
            OracleParameter usernameIn = null;
            OracleParameter errorMessageOut = null;
            OracleParameter resultsOut = null;
            OracleDataReader dr = null;

            List<Category> categories = new List<Category>();

            try
            {
                // Create Oracle connection
                connection = new OracleConnection(Utilities.GetDatabaseConnection());
                connection.Open();

                // Create Oracle command
                command = new OracleCommand(Constants.FETCH_CATEGORIES_SP, connection);
                command.CommandType = CommandType.StoredProcedure;

                // Input parameter
                usernameIn = new OracleParameter(Constants.FetchCategoriesParameters.USERNAME, OracleDbType.Varchar2, 
                    "CourseSearchSvc", ParameterDirection.Input);
                command.Parameters.Add(usernameIn);

                // Output parameters
                errorMessageOut = new OracleParameter(
                    Constants.FetchCategoriesParameters.ERROR_MSG, OracleDbType.Varchar2, ParameterDirection.Output);
                command.Parameters.Add(errorMessageOut);
                resultsOut = new OracleParameter(
                    Constants.FetchCategoriesParameters.CATEGORIES, OracleDbType.RefCursor, ParameterDirection.Output);
                command.Parameters.Add(resultsOut);

                dr = command.ExecuteReader();

                // get error message
                string errorMessage = null;

                if (!((OracleString)(command.Parameters[Constants.FetchCategoriesParameters.ERROR_MSG].Value)).IsNull)
                {
                    errorMessage = command.Parameters[Constants.FetchCategoriesParameters.ERROR_MSG].Value.ToString();
                }

                if ((errorMessage == null || errorMessage.Length == 0) && dr.HasRows)
                {
                    while (dr.Read())
                    {
                        categories.Add(BuildCategory(dr));
                    }
                }
                else
                {
                    // throw error message ?
                }
            }
            finally
            {
                // clean up after call ...
                dr.Dispose();
                resultsOut.Dispose();
                errorMessageOut.Dispose();
                usernameIn.Dispose();
                command.Dispose();
                connection.Dispose();
            }

            return categories;
        }

        /// <summary>
        /// Retrieves course details for the list of course ids provided.
        /// </summary>
        /// <param name="courseIds">Course ids to return course details for.</param>
        /// <returns>A collection of Course entities.</returns>
        public List<Course> GetCourseDetails(List<long> courseIds)
        {
            OracleConnection connection = null;
            OracleCommand command = null;
            OracleParameter courseIdsIn = null;
            OracleParameter usernameIn = null;
            OracleParameter errorMessageOut = null;
            OracleParameter courseDetailsOut = null;
            OracleParameter providerDetailsOut = null;
            OracleParameter opportunityDetailsOut = null;
            OracleParameter venueDetailsOut = null;
            OracleDataReader dr = null;

            List<Course> courses = new List<Course>();
            List<ProviderResult> providerResults = new List<ProviderResult>();
            List<OpportunityResult> opportunityResults = new List<OpportunityResult>();
            List<VenueResult> venueResults = new List<VenueResult>();

            try
            {
                // Create Oracle connection
                connection = new OracleConnection(Utilities.GetDatabaseConnection());
                connection.Open();

                // Create Oracle command
                command = new OracleCommand(Constants.FETCH_COURSE_DETAILS_SP, connection);
                command.CommandType = CommandType.StoredProcedure;

                string ids = Utilities.ConvertToDelimitedString(courseIds, ",");

                // Input parameters
                courseIdsIn = new OracleParameter(
                    Constants.CourseDetailsParameters.COURSE_IDS, OracleDbType.Varchar2, ids, ParameterDirection.Input);
                command.Parameters.Add(courseIdsIn);
                usernameIn = new OracleParameter(Constants.CourseDetailsParameters.USERNAME, OracleDbType.Varchar2, 
                    "CourseSearchSvc", ParameterDirection.Input);
                command.Parameters.Add(usernameIn);

                // Output parameters
                errorMessageOut = new OracleParameter(
                    Constants.CourseDetailsParameters.ERROR_MSG, OracleDbType.Varchar2, ParameterDirection.Output);
                command.Parameters.Add(errorMessageOut);
                courseDetailsOut = new OracleParameter(
                    Constants.CourseDetailsParameters.COURSE_DETAILS, OracleDbType.RefCursor, ParameterDirection.Output);
                command.Parameters.Add(courseDetailsOut);
                providerDetailsOut = new OracleParameter(
                    Constants.CourseDetailsParameters.PROVIDER_DETAILS, OracleDbType.RefCursor, ParameterDirection.Output);
                command.Parameters.Add(providerDetailsOut);
                opportunityDetailsOut = new OracleParameter(
                    Constants.CourseDetailsParameters.OPPORTUNITY_DETAILS, OracleDbType.RefCursor, ParameterDirection.Output);
                command.Parameters.Add(opportunityDetailsOut);
                venueDetailsOut = new OracleParameter(
                    Constants.CourseDetailsParameters.VENUE_DETAILS, OracleDbType.RefCursor, ParameterDirection.Output);
                command.Parameters.Add(venueDetailsOut);

                dr = command.ExecuteReader();

                // get error message
                string errorMessage = null;

                if (!((OracleString)(command.Parameters[Constants.CourseDetailsParameters.ERROR_MSG].Value)).IsNull)
                {
                    errorMessage = command.Parameters[Constants.CourseDetailsParameters.ERROR_MSG].Value.ToString();
                }

                if ((errorMessage == null || errorMessage.Length == 0))
                {
                    // get Course cursor
                    while (dr.Read())
                    {
                        courses.Add(BuildCourseDetail(dr));
                    }

                    // get Provider cursor
                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            providerResults.Add(BuildProviderResult(dr));
                        }
                    }

                    // get Opportunity cursor
                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            opportunityResults.Add(BuildOpportunityResult(dr));
                        }
                    }

                    // get Venue cursor
                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            venueResults.Add(BuildVenueResult(dr));
                        }
                    }
                }
                else
                {
                    // throw error?
                }
            }
            finally
            {
                // clean up after call ...
                dr.Dispose();
                venueDetailsOut.Dispose();
                opportunityDetailsOut.Dispose();
                providerDetailsOut.Dispose();
                courseDetailsOut.Dispose();
                errorMessageOut.Dispose();
                usernameIn.Dispose();
                courseIdsIn.Dispose();
                command.Dispose();
                connection.Dispose();
            }

            // Rebuild result set and return
            return BuildCourseDetailResult(courses, providerResults, opportunityResults, venueResults);
        }

        /// <summary>
        /// Retrieves courses matching the search criteria in the request.
        /// </summary>
        /// <param name="request">Course search criteria.</param>
        /// <returns>A CourseList Response entity.</returns>
        public CourseListResponse GetCourseList(CourseListRequest request)
        {
            OracleConnection connection = null;
            OracleCommand command = null;
            OracleDataReader dr = null;

            CourseListResponse courseListResponse = new CourseListResponse();
            courseListResponse.Courses = new List<Course>();
            courseListResponse.LdcsCodes = new List<LdcsCode>();

            try
            {
                // Create Oracle connection
                connection = new OracleConnection(Utilities.GetDatabaseConnection());
                connection.Open();

                // Create Oracle command
                command = new OracleCommand(Constants.COURSE_SEARCH_SP, connection);
                command.CommandType = CommandType.StoredProcedure;

                #region Set Parameters

                // input parameters
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.COURSE_KEYWORD,
                    OracleDbType.Varchar2, request.SubjectKeyword, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.LOCATION_TEXT,
                    OracleDbType.Varchar2, request.Location, ParameterDirection.Input));
                if (request.DistanceSpecified)
                {
                    command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.MAX_DISTANCE,
                        OracleDbType.Double, request.Distance, ParameterDirection.Input));
                }
                else
                {
                    command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.MAX_DISTANCE,
                        OracleDbType.Double, ParameterDirection.Input));
                }
                if (request.ProviderId > 0)
                {
                    command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.PROVIDER_ID,
                        OracleDbType.Int32, request.ProviderId, ParameterDirection.Input));
                }
                else
                {
                    command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.PROVIDER_ID,
                        OracleDbType.Int32, ParameterDirection.Input));
                }
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.PROVIDER_KEYWORD,
                    OracleDbType.Varchar2, request.ProviderKeyword, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.QUALIFICATION_TYPES,
                    OracleDbType.Varchar2, request.QualificationTypes, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.QUALIFICATION_LEVELS,
                    OracleDbType.Varchar2, request.QualificationLevels, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.STUDY_MODES,
                    OracleDbType.Varchar2, request.StudyModes, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.ATTENDANCE_MODES,
                    OracleDbType.Varchar2, request.AttendanceModes, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.ATTENDANCE_PATTERNS,
                    OracleDbType.Varchar2, request.AttendancePatterns, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.EARLIEST_START_DATE,
                    OracleDbType.Varchar2, request.EarliestStartDate, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.FLEXIBLE_START_FLAG,
                    OracleDbType.Varchar2, request.FlexStartFlag, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.APPLICATION_CLOSED_FLAG,
                    OracleDbType.Varchar2, request.AppClosedFlag, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.LDCS_CATEGORY_CODE,
                    OracleDbType.Varchar2, request.LdcsCategoryCode, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.TTG_FLAG,
                    OracleDbType.Varchar2, request.TtgFlag, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.TQS_FLAG,
                    OracleDbType.Varchar2, request.TqsFlag, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.IES_FLAG,
                    OracleDbType.Varchar2, request.IesFlag, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.A10_CODES,
                    OracleDbType.Varchar2, request.A10Codes, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(
                    Constants.CourseListParameters.INDEPENDENT_LIVING_SKILLS_FLAG,
                    OracleDbType.Varchar2, request.IlsFlag, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.SKILLS_FOR_LIFE_FLAG,
                    OracleDbType.Varchar2, request.SflFlag, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.ER_APP_STATUS,
                    OracleDbType.Varchar2, request.ERAppStatus, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.ER_TTG_STATUS,
                    OracleDbType.Varchar2, request.ERTtgStatus, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.ADULT_LR_STATUS,
                    OracleDbType.Varchar2, request.AdultLRStatus, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.OTHER_FUNDING_STATUS,
                    OracleDbType.Varchar2, request.OtherFundingStatus, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.SORT_BY,
                    OracleDbType.Varchar2, request.SortBy, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.PAGE_NUMBER,
                    OracleDbType.Varchar2, request.PageNumber, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.RECORDS_PER_PAGE,
                    OracleDbType.Varchar2, request.RecordsPerPage, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(Constants.CourseListParameters.USERNAME,
                    OracleDbType.Varchar2, "CourseSearchSvc", ParameterDirection.Input));

                // output parameters
                command.Parameters.Add(new OracleParameter(
                    Constants.CourseListParameters.ERROR_MSG, OracleDbType.Varchar2, ParameterDirection.Output));
                command.Parameters.Add(new OracleParameter(
                    Constants.CourseListParameters.TOTAL_RESULTS_COUNT, OracleDbType.Int32, ParameterDirection.Output));
                command.Parameters.Add(new OracleParameter(
                    Constants.CourseListParameters.RESULTS_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output));
                command.Parameters.Add(new OracleParameter(
                    Constants.CourseListParameters.LDCS_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output));
                command.Parameters.Add(new OracleParameter(
                    Constants.CourseListParameters.SEARCH_ID, OracleDbType.Int32, ParameterDirection.Output));

                #endregion

                dr = command.ExecuteReader();

                // get error message
                string errorMessage = null;

                if (!((OracleString) (command.Parameters[Constants.CourseListParameters.ERROR_MSG].Value)).IsNull)
                {
                    errorMessage = command.Parameters[Constants.CourseListParameters.ERROR_MSG].Value.ToString();
                }

                if (errorMessage == null || errorMessage.Length == 0)
                {
                    // get total number of records
                    if (
                        !((OracleDecimal) (command.Parameters[Constants.CourseListParameters.TOTAL_RESULTS_COUNT].Value))
                            .IsNull)
                    {
                        courseListResponse.NumberOfRecords = Int32.Parse(
                            command.Parameters[Constants.CourseListParameters.TOTAL_RESULTS_COUNT].Value.ToString());
                    }
                    else
                    {
                        courseListResponse.NumberOfRecords = 0;
                    }

                    // get results cursor
                    while (dr.Read())
                    {
                        courseListResponse.Courses.Add(BuildCourseListResult(dr));
                    }

                    // get LDCS cursor, i.e. next cursor result
                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            courseListResponse.LdcsCodes.Add(BuildLdcsCodeResult(dr));
                        }
                    }

                    // get Search Id
                    if (!((OracleDecimal) (command.Parameters[Constants.CourseListParameters.SEARCH_ID].Value)).IsNull)
                    {
                        courseListResponse.SearchHeaderId =
                            command.Parameters[Constants.CourseListParameters.SEARCH_ID].Value.ToString();
                    }
                }
                else
                {
                    // throw error?
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                // clean up after call ...
                dr.Dispose();
                command.Dispose();
                connection.Dispose();
            }

            return courseListResponse;
        }

        /// <summary>
        /// Records time taken for search.
        /// </summary>
        /// <param name="columnFlag">Column flag.</param>
        /// <param name="searchHeaderId">Search header Id</param>
        public void RecordSearchTime(string columnFlag, string searchHeaderId)
        {
            OracleConnection connection = null;
            OracleCommand command = null;
            OracleParameter columnFlagIn = null;
            OracleParameter searchIdIn = null;

            try
            {
                // Create Oracle connection
                connection = new OracleConnection(Utilities.GetDatabaseConnection());
                connection.Open();

                // Create Oracle command
                command = new OracleCommand(Constants.UPDATE_TIMING_SP, connection);
                command.CommandType = CommandType.StoredProcedure;

                columnFlagIn = new OracleParameter(
                    Constants.UpdateTimingParameters.COLUMN_FLAG, OracleDbType.Varchar2,
                    columnFlag, ParameterDirection.Input);
                command.Parameters.Add(columnFlagIn);
                searchIdIn = new OracleParameter(
                    Constants.UpdateTimingParameters.SEARCH_ID, OracleDbType.Varchar2,
                    searchHeaderId, ParameterDirection.Input);
                command.Parameters.Add(searchIdIn);

                int returnCode = command.ExecuteNonQuery();

                if (returnCode > 0)
                {
                    // TODO: record error?
                }
            }
            finally
            {
                // clean up after call ...
                searchIdIn.Dispose();
                columnFlagIn.Dispose();
                command.Dispose();
                connection.Dispose();
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Creates Category object from data returned in data reader.
        /// </summary>
        /// <param name="dr">OracleDataReader containing category data.</param>
        /// <returns>Populated Category object.</returns>
        private static Category BuildCategory(OracleDataReader dr)
        {
            Category category = new Category();

            category.CategoryCode = dr[Constants.FetchCategoriesColumns.CATEGORY_CODE].ToString();
            category.Description = dr[Constants.FetchCategoriesColumns.DESCRIPTION].ToString();
            category.ParentCategoryCode = dr[Constants.FetchCategoriesColumns.PARENT_CATEGORY_CODE].ToString();
            category.CourseCount = Int32.Parse(dr[Constants.FetchCategoriesColumns.COURSE_COUNT].ToString());
            category.Level = Int32.Parse(dr[Constants.FetchCategoriesColumns.LEVEL].ToString());
            category.Searchable = dr[Constants.FetchCategoriesColumns.SEARCHABLE_FLAG].ToString();

            return category;
        }

        /// <summary>
        /// Creates Course object from data returned in data reader.
        /// </summary>
        /// <param name="dr">OracleDataReader containing course data.</param>
        /// <returns>Populated Course object.</returns>
        private static Course BuildCourseDetail(OracleDataReader dr)
        {
            Course course = new Course();

            course.CourseId = long.Parse(dr[Constants.CourseDetailsColumns.CourseCursor.COURSE_ID].ToString());
            course.CourseTitle = dr[Constants.CourseDetailsColumns.CourseCursor.COURSE_TITLE].ToString();
            course.QualificationType = dr[Constants.CourseDetailsColumns.CourseCursor.QUALIFICATION_TYPE].ToString();
            course.QualificationLevel = dr[Constants.CourseDetailsColumns.CourseCursor.QUALIFICATION_LEVEL].ToString();
            course.LdcsCode1 = dr[Constants.CourseDetailsColumns.CourseCursor.LDCSCODE_1].ToString();
            course.LdcsCode2 = dr[Constants.CourseDetailsColumns.CourseCursor.LDCSCODE_2].ToString();
            course.LdcsCode3 = dr[Constants.CourseDetailsColumns.CourseCursor.LDCSCODE_3].ToString();
            course.LdcsCode4 = dr[Constants.CourseDetailsColumns.CourseCursor.LDCSCODE_4].ToString();
            course.LdcsCode5 = dr[Constants.CourseDetailsColumns.CourseCursor.LDCSCODE_5].ToString();
            course.LdcsDesc1 = dr[Constants.CourseDetailsColumns.CourseCursor.LDCSDESC_1].ToString();
            course.LdcsDesc2 = dr[Constants.CourseDetailsColumns.CourseCursor.LDCSDESC_2].ToString();
            course.LdcsDesc3 = dr[Constants.CourseDetailsColumns.CourseCursor.LDCSDESC_3].ToString();
            course.LdcsDesc4 = dr[Constants.CourseDetailsColumns.CourseCursor.LDCSDESC_4].ToString();
            course.LdcsDesc5 = dr[Constants.CourseDetailsColumns.CourseCursor.LDCSDESC_5].ToString();

            if (!string.IsNullOrEmpty(dr[Constants.CourseDetailsColumns.CourseCursor.NUM_OF_OPPORTUNITIES].ToString()))
            {
                course.NumberOfOpportunities =
                    long.Parse(dr[Constants.CourseDetailsColumns.CourseCursor.NUM_OF_OPPORTUNITIES].ToString());
            }

            course.CourseSummary = dr[Constants.CourseDetailsColumns.CourseCursor.COURSE_SUMMARY].ToString();
            course.AwardingBody = dr[Constants.CourseDetailsColumns.CourseCursor.AWARDING_BODY].ToString();
            course.AssessmentMethod = dr[Constants.CourseDetailsColumns.CourseCursor.ASSESSMENT_METHOD].ToString();
            course.BookingUrl = dr[Constants.CourseDetailsColumns.CourseCursor.BOOKING_URL].ToString();
            course.AccreditationEndDate =
                dr[Constants.CourseDetailsColumns.CourseCursor.ACCREDITATION_END_DATE].ToString();
            course.AccreditationStartDate =
                dr[Constants.CourseDetailsColumns.CourseCursor.ACCREDITATION_START_DATE].ToString();
            course.CertificationEndDate =
                dr[Constants.CourseDetailsColumns.CourseCursor.CERTIFICATION_END_DATE].ToString();
            course.CreditValue = dr[Constants.CourseDetailsColumns.CourseCursor.CREDIT_VALUE].ToString();
            course.DataType = dr[Constants.CourseDetailsColumns.CourseCursor.DATA_TYPE].ToString();
            course.ERAppStatus = dr[Constants.CourseDetailsColumns.CourseCursor.ER_APP_STATUS].ToString();
            course.ERTtgStatus = dr[Constants.CourseDetailsColumns.CourseCursor.ER_TTG_STATUS].ToString();
            course.EntryRequirements = dr[Constants.CourseDetailsColumns.CourseCursor.ENTRY_REQUIREMENTS].ToString();
            course.EquipmentRequired = dr[Constants.CourseDetailsColumns.CourseCursor.EQUIPMENT_REQUIRED].ToString();
            course.IndependentLivingSkills =
                dr[Constants.CourseDetailsColumns.CourseCursor.INDEPENDENT_LIVING_SKILLS].ToString();
            course.LadId = dr[Constants.CourseDetailsColumns.CourseCursor.LAD_ID].ToString();
            course.Level2EntitlementDescription =
                dr[Constants.CourseDetailsColumns.CourseCursor.LEVEL_2_ENTITLEMENT_CATEGORY_DESCRIPTION].ToString();
            course.Level3EntitlementDescription =
                dr[Constants.CourseDetailsColumns.CourseCursor.LEVEL_3_ENTITLEMENT_CATEGORY_DESCRIPTION].ToString();
            course.OtherFundingNonFundedStatus =
                dr[Constants.CourseDetailsColumns.CourseCursor.OTHER_FUNDING_NON_FUNDED_STATUS].ToString();
            course.QcaGuidedLearningHours =
                dr[Constants.CourseDetailsColumns.CourseCursor.QCA_GUIDED_LEARNING_HOURS].ToString();
            course.QualificationReferenceAuthority =
                dr[Constants.CourseDetailsColumns.CourseCursor.QUALIFICATION_REFERENCE_AUTHORITY].ToString();
            course.QualificationReference =
                dr[Constants.CourseDetailsColumns.CourseCursor.QUALIFICATION_REFERENCE].ToString();
            course.QualificationTitle = dr[Constants.CourseDetailsColumns.CourseCursor.QUALIFICATION_TITLE].ToString();
            course.SectorLeadBodyDescription =
                dr[Constants.CourseDetailsColumns.CourseCursor.SECTOR_LEAD_BODY_DESCRIPTION].ToString();
            course.SkillsForLifeFlag = dr[Constants.CourseDetailsColumns.CourseCursor.SKILLS_FOR_LIFE_FLAG].ToString();
            course.SkillsForLifeTypeDescription =
                dr[Constants.CourseDetailsColumns.CourseCursor.SKILLS_FOR_LIFE_TYPE_DESCRIPTION].ToString();
            course.TariffRequired = dr[Constants.CourseDetailsColumns.CourseCursor.TARIFF_REQUIRED].ToString();
            course.Url = dr[Constants.CourseDetailsColumns.CourseCursor.URL].ToString();
            course.AdultLRStatus = dr[Constants.CourseDetailsColumns.CourseCursor.ADULT_LR_STATUS].ToString();

            return course;
        }

        /// <summary>
        /// Creates ProviderResult object from data returned in data reader.
        /// </summary>
        /// <param name="dr">OracleDataReader containing provider data linked to a CourseId.</param>
        /// <returns>Populated ProviderResult object.</returns>
        private static ProviderResult BuildProviderResult(OracleDataReader dr)
        {
            ProviderResult providerResult = new ProviderResult();

            providerResult.CourseId = long.Parse(dr[Constants.CourseDetailsColumns.ProviderCursor.COURSE_ID].ToString());
            providerResult.ProviderId = dr[Constants.CourseDetailsColumns.ProviderCursor.PROVIDER_ID].ToString();
            providerResult.ProviderName = dr[Constants.CourseDetailsColumns.ProviderCursor.PROVIDER_NAME].ToString();
            providerResult.AddressLine1 = dr[Constants.CourseDetailsColumns.ProviderCursor.ADDRESS_LINE1].ToString();
            providerResult.AddressLine2 = dr[Constants.CourseDetailsColumns.ProviderCursor.ADDRESS_LINE2].ToString();
            providerResult.Town = dr[Constants.CourseDetailsColumns.ProviderCursor.TOWN].ToString();
            providerResult.County = dr[Constants.CourseDetailsColumns.ProviderCursor.COUNTY].ToString();
            providerResult.Postcode = dr[Constants.CourseDetailsColumns.ProviderCursor.POSTCODE].ToString();
            providerResult.Phone = dr[Constants.CourseDetailsColumns.ProviderCursor.PHONE].ToString();
            providerResult.Website = dr[Constants.CourseDetailsColumns.ProviderCursor.WEBSITE].ToString();
            providerResult.Ukprn = dr[Constants.CourseDetailsColumns.ProviderCursor.UKPRN].ToString();
            providerResult.Fax = dr[Constants.CourseDetailsColumns.ProviderCursor.FAX].ToString();
            providerResult.Email = dr[Constants.CourseDetailsColumns.ProviderCursor.EMAIL].ToString();
            providerResult.Upin = dr[Constants.CourseDetailsColumns.ProviderCursor.UPIN].ToString();
            providerResult.TFPlusLoans = Convert.ToBoolean(dr[Constants.CourseDetailsColumns.ProviderCursor.TFPLUSLOANS].ToString());

            return providerResult;
        }

        /// <summary>
        /// Creates OpportunityResult object from data returned in data reader.
        /// </summary>
        /// <param name="dr">OracleDataReader containing opportunity data linked to a CourseId.</param>
        /// <returns>Populated OpportunityResult object.</returns>
        private static OpportunityResult BuildOpportunityResult(OracleDataReader dr)
        {
            OpportunityResult opportunityResult = new OpportunityResult();

            opportunityResult.CourseId = long.Parse(dr[Constants.CourseDetailsColumns.OpportunityCursor.COURSE_ID].ToString());
            opportunityResult.AttendanceMode = dr[Constants.CourseDetailsColumns.OpportunityCursor.ATTENDANCE_MODE].ToString();
            opportunityResult.AttendancePattern = dr[Constants.CourseDetailsColumns.OpportunityCursor.ATTENDANCE_PATTERN].ToString();
            opportunityResult.DurationDescription = dr[Constants.CourseDetailsColumns.OpportunityCursor.DURATION_DESCRIPTION].ToString();
            
            if (!string.IsNullOrEmpty(dr[Constants.CourseDetailsColumns.OpportunityCursor.DURATION_VALUE].ToString()))
            {
                opportunityResult.DurationValue = long.Parse(dr[Constants.CourseDetailsColumns.OpportunityCursor.DURATION_VALUE].ToString());
            }
            
            opportunityResult.DurationUnit = dr[Constants.CourseDetailsColumns.OpportunityCursor.DURATION_UNIT].ToString();
            opportunityResult.Price = dr[Constants.CourseDetailsColumns.OpportunityCursor.PRICE].ToString();
            opportunityResult.PriceDescription = dr[Constants.CourseDetailsColumns.OpportunityCursor.PRICE_DESCRIPTION].ToString();
            opportunityResult.StartDate = dr[Constants.CourseDetailsColumns.OpportunityCursor.START_DATE].ToString();
            opportunityResult.StartDateDescription = 
                dr[Constants.CourseDetailsColumns.OpportunityCursor.START_DATE_DESCRIPTION].ToString();
            opportunityResult.StudyMode = dr[Constants.CourseDetailsColumns.OpportunityCursor.STUDY_MODE].ToString();
            opportunityResult.Timetable = dr[Constants.CourseDetailsColumns.OpportunityCursor.TIMETABLE].ToString();
            opportunityResult.RegionName = dr[Constants.CourseDetailsColumns.OpportunityCursor.REGION_NAME].ToString();

            if (!string.IsNullOrEmpty(dr[Constants.CourseDetailsColumns.OpportunityCursor.VENUE_ID].ToString()))
            {
                opportunityResult.VenueId = long.Parse(dr[Constants.CourseDetailsColumns.OpportunityCursor.VENUE_ID].ToString());
            }

            opportunityResult.ApplicationAcceptedThroughoutYear =
                dr[Constants.CourseDetailsColumns.OpportunityCursor.APPLICATION_ACCEPTED_THROUGHOUT_YEAR].ToString();
            opportunityResult.ApplyFromDate = dr[Constants.CourseDetailsColumns.OpportunityCursor.APPLY_FROM_DATE].ToString();
            opportunityResult.ApplyTo = dr[Constants.CourseDetailsColumns.OpportunityCursor.APPLY_TO].ToString();
            opportunityResult.ApplyUntilDate = dr[Constants.CourseDetailsColumns.OpportunityCursor.APPLY_UNTIL_DATE].ToString();
            opportunityResult.ApplyUntilDescription = 
                dr[Constants.CourseDetailsColumns.OpportunityCursor.APPLY_UNTIL_DESCRIPTION].ToString();
            opportunityResult.EndDate = dr[Constants.CourseDetailsColumns.OpportunityCursor.END_DATE].ToString();
            opportunityResult.EnquireTo = dr[Constants.CourseDetailsColumns.OpportunityCursor.ENQUIRE_TO].ToString();
            opportunityResult.LanguageOfAssessment = 
                dr[Constants.CourseDetailsColumns.OpportunityCursor.LANGUAGE_OF_ASSESSMENT].ToString();
            opportunityResult.LanguageOfInstruction =
                dr[Constants.CourseDetailsColumns.OpportunityCursor.LANGUAGE_OF_INSTRUCTION].ToString();

            if (!string.IsNullOrEmpty(dr[Constants.CourseDetailsColumns.OpportunityCursor.PLACES_AVAILABLE].ToString()))
            {
                opportunityResult.PlacesAvailable =
                    long.Parse(dr[Constants.CourseDetailsColumns.OpportunityCursor.PLACES_AVAILABLE].ToString());
            }

            opportunityResult.ProviderOpportunityId =
                dr[Constants.CourseDetailsColumns.OpportunityCursor.PROVIDER_OPPORTUNITY_ID].ToString();
            opportunityResult.Url = dr[Constants.CourseDetailsColumns.OpportunityCursor.URL].ToString();
            opportunityResult.A10 = dr[Constants.CourseDetailsColumns.OpportunityCursor.A10].ToString();

            opportunityResult.OpportunityId = dr[Constants.CourseDetailsColumns.OpportunityCursor.OPPORTUNITY_ID].ToString();

            return opportunityResult;
        }

        /// <summary>
        /// Creates VenueResult object from data returned in data reader.
        /// </summary>
        /// <param name="dr">OracleDataReader containing venue data linked to a CourseId.</param>
        /// <returns>Populated VenueResult object.</returns>
        private static VenueResult BuildVenueResult(OracleDataReader dr)
        {
            VenueResult venueResult = new VenueResult();

            venueResult.CourseId = long.Parse(dr[Constants.CourseDetailsColumns.VenueCursor.COURSE_ID].ToString());
            
            if (!string.IsNullOrEmpty(dr[Constants.CourseDetailsColumns.VenueCursor.VENUE_ID].ToString()))
            {
                venueResult.VenueId = long.Parse(dr[Constants.CourseDetailsColumns.VenueCursor.VENUE_ID].ToString());
            }
            
            venueResult.VenueName = dr[Constants.CourseDetailsColumns.VenueCursor.VENUE_NAME].ToString();
            venueResult.AddressLine1 = dr[Constants.CourseDetailsColumns.VenueCursor.ADDRESS_LINE1].ToString();
            venueResult.AddressLine2 = dr[Constants.CourseDetailsColumns.VenueCursor.ADDRESS_LINE2].ToString();
            venueResult.Town = dr[Constants.CourseDetailsColumns.VenueCursor.TOWN].ToString();
            venueResult.County = dr[Constants.CourseDetailsColumns.VenueCursor.COUNTY].ToString();
            venueResult.Postcode = dr[Constants.CourseDetailsColumns.VenueCursor.POSTCODE].ToString();
            venueResult.Latitude = dr[Constants.CourseDetailsColumns.VenueCursor.LATITUDE].ToString();
            venueResult.Longitude = dr[Constants.CourseDetailsColumns.VenueCursor.LONGITUDE].ToString();
            venueResult.Phone = dr[Constants.CourseDetailsColumns.VenueCursor.PHONE].ToString();
            venueResult.Email = dr[Constants.CourseDetailsColumns.VenueCursor.EMAIL].ToString();
            venueResult.Website = dr[Constants.CourseDetailsColumns.VenueCursor.WEBSITE].ToString();
            venueResult.Fax = dr[Constants.CourseDetailsColumns.VenueCursor.FAX].ToString();
            venueResult.Facilities = dr[Constants.CourseDetailsColumns.VenueCursor.FACILITIES].ToString();

            return venueResult;
        }

        /// <summary>
        /// Combines data retrieved from Course, Provider, Opportunity and Venue results cursors into a collection 
        /// of Course objects containing the correctly associated Provider, Opportunity and Venue data.
        /// </summary>
        /// <param name="courses">Collection of Course data.</param>
        /// <param name="providerResults">
        /// Collection of all Provider data associated with Courses in course collection, associated by CourseId.
        /// </param>
        /// <param name="opportunityResults">
        /// Collection of all Opportunity data associated with Courses in course collection, associated by CourseId.
        /// </param>
        /// <param name="venueResults">
        /// Collection of all Venue data associated with Courses in course collection, associated by CourseId.
        /// </param>
        /// <returns>A collection of Course objects containing associated Provider, Opportunity and Venue data.</returns>
        private static List<Course> BuildCourseDetailResult(List<Course> courses, List<ProviderResult> providerResults, List<OpportunityResult> opportunityResults, List<VenueResult> venueResults)
        {
            List<Course> courseResults = new List<Course>();

            foreach (Course course in courses)
            {
                Course courseResult = course;
                courseResult.Provider = new Provider();
                courseResult.Opportunities = new List<Opportunity>();
                courseResult.Venues = new List<Venue>();

                // Find matching Provider - we're only expecting one.
                foreach (ProviderResult providerResult in providerResults)
                {
                    if (providerResult.CourseId == courseResult.CourseId)
                    {
                        courseResult.Provider = BuildProvider(providerResult);
                        break;
                    }
                }

                // Find matching Opportunities
                foreach (OpportunityResult opportunityResult in opportunityResults)
                {
                    if (opportunityResult.CourseId == courseResult.CourseId)
                    {
                        courseResult.Opportunities.Add(BuildOpportunity(opportunityResult));
                    }
                }

                // Find matching Venues
                foreach (VenueResult venueResult in venueResults)
                {
                    if (venueResult.CourseId == courseResult.CourseId)
                    {
                        courseResult.Venues.Add(BuildVenue(venueResult));
                    }
                }

                courseResults.Add(courseResult);
            }

            return courseResults;
        }

        /// <summary>
        /// Builds a Provider object from ProviderResult data.
        /// </summary>
        /// <param name="providerResult">ProviderResult data.</param>
        /// <returns>Populated Provider object.</returns>
        private static Provider BuildProvider(ProviderResult providerResult)
        {
            Provider provider = new Provider();

            provider.ProviderId = providerResult.ProviderId;
            provider.ProviderName = providerResult.ProviderName;
            provider.AddressLine1 = providerResult.AddressLine1;
            provider.AddressLine2 = providerResult.AddressLine2;
            provider.Town = providerResult.Town;
            provider.County = providerResult.County;
            provider.Postcode = providerResult.Postcode;
            provider.Phone = providerResult.Phone;
            provider.Website = providerResult.Website;
            provider.Ukprn = providerResult.Ukprn;
            provider.Fax = providerResult.Fax;
            provider.Email = providerResult.Email;
            provider.Upin = providerResult.Upin;
            provider.TFPlusLoans = providerResult.TFPlusLoans;

            return provider;
        }

        /// <summary>
        /// Builds an Opportunity object from OpportunityResult data.
        /// </summary>
        /// <param name="opportunityResult">OpportunityResult data.</param>
        /// <returns>Populated Opportunity object.</returns>
        private static Opportunity BuildOpportunity(OpportunityResult opportunityResult)
        {
            Opportunity opportunity = new Opportunity();

            opportunity.AttendanceMode = opportunityResult.AttendanceMode;
            opportunity.AttendancePattern = opportunityResult.AttendancePattern;
            opportunity.DurationDescription = opportunityResult.DurationDescription;
            opportunity.DurationValue = opportunityResult.DurationValue;
            opportunity.DurationUnit = opportunityResult.DurationUnit;
            opportunity.Price = opportunityResult.Price;
            opportunity.PriceDescription = opportunityResult.PriceDescription;
            opportunity.StartDate = opportunityResult.StartDate;
            opportunity.StartDateDescription = opportunityResult.StartDateDescription;
            opportunity.StudyMode = opportunityResult.StudyMode;
            opportunity.Timetable = opportunityResult.Timetable;
            opportunity.RegionName = opportunityResult.RegionName;
            opportunity.VenueId = opportunityResult.VenueId;
            opportunity.ApplicationAcceptedThroughoutYear = opportunityResult.ApplicationAcceptedThroughoutYear;
            opportunity.ApplyFromDate = opportunityResult.ApplyFromDate;
            opportunity.ApplyTo = opportunityResult.ApplyTo;
            opportunity.ApplyUntilDate = opportunityResult.ApplyUntilDate;
            opportunity.ApplyUntilDescription = opportunityResult.ApplyUntilDescription;
            opportunity.EndDate = opportunityResult.EndDate;
            opportunity.EnquireTo = opportunityResult.EnquireTo;
            opportunity.LanguageOfAssessment = opportunityResult.LanguageOfAssessment;
            opportunity.LanguageOfInstruction = opportunityResult.LanguageOfInstruction;
            opportunity.PlacesAvailable = opportunityResult.PlacesAvailable;
            opportunity.ProviderOpportunityId = opportunityResult.ProviderOpportunityId;
            opportunity.Url = opportunityResult.Url;
            opportunity.A10 = opportunityResult.A10;
            opportunity.OpportunityId = opportunityResult.OpportunityId;

            return opportunity;
        }

        /// <summary>
        /// Builds a Venue object from VenueResult data.
        /// </summary>
        /// <param name="venueResult">VenueResult data.</param>
        /// <returns>Populated Venue object.</returns>
        private static Venue BuildVenue(VenueResult venueResult)
        {
            Venue venue = new Venue();

            venue.VenueId = venueResult.VenueId;
            venue.VenueName = venueResult.VenueName;
            venue.AddressLine1 = venueResult.AddressLine1;
            venue.AddressLine2 = venueResult.AddressLine2;
            venue.Town = venueResult.Town;
            venue.County = venueResult.County;
            venue.Postcode = venueResult.Postcode;
            venue.Phone = venueResult.Phone;
            venue.Email = venueResult.Email;
            venue.Website = venueResult.Website;
            venue.Fax = venueResult.Fax;
            venue.Facilities = venueResult.Facilities;
            venue.Latitude = venueResult.Latitude;
            venue.Longitude = venueResult.Longitude;

            return venue;
        }

        /// <summary>
        /// Create a Course object from the data reader returned from a CourseList search.
        /// </summary>
        /// <param name="dr">Data reader containing returned course data.</param>
        /// <returns>Populated Course object.</returns>
        private static Course BuildCourseListResult(OracleDataReader dr)
        {
            Course course = new Course();

            course.Provider = new Provider();
            course.Provider.ProviderName = dr[Constants.CourseListColumns.CourseCursor.PROVIDER_NAME].ToString();
            course.Provider.TFPlusLoans = Convert.ToBoolean(dr[Constants.CourseListColumns.CourseCursor.TFPLUSLOANS].ToString());

            course.CourseId = long.Parse(dr[Constants.CourseListColumns.CourseCursor.COURSE_ID].ToString());
            course.CourseTitle = dr[Constants.CourseListColumns.CourseCursor.COURSE_TITLE].ToString();
            course.QualificationType = dr[Constants.CourseListColumns.CourseCursor.QUALIFICATION_TYPE].ToString();
            course.QualificationLevel = dr[Constants.CourseListColumns.CourseCursor.QUALIFICATION_LEVEL].ToString();
            course.LdcsCode1 = dr[Constants.CourseListColumns.CourseCursor.LDCSCODE_1].ToString();
            course.LdcsCode2 = dr[Constants.CourseListColumns.CourseCursor.LDCSCODE_2].ToString();
            course.LdcsCode3 = dr[Constants.CourseListColumns.CourseCursor.LDCSCODE_3].ToString();
            course.LdcsCode4 = dr[Constants.CourseListColumns.CourseCursor.LDCSCODE_4].ToString();
            course.LdcsCode5 = dr[Constants.CourseListColumns.CourseCursor.LDCSCODE_5].ToString();
            course.LdcsDesc1 = dr[Constants.CourseListColumns.CourseCursor.LDCSDESC_1].ToString();
            course.LdcsDesc2 = dr[Constants.CourseListColumns.CourseCursor.LDCSDESC_2].ToString();
            course.LdcsDesc3 = dr[Constants.CourseListColumns.CourseCursor.LDCSDESC_3].ToString();
            course.LdcsDesc4 = dr[Constants.CourseListColumns.CourseCursor.LDCSDESC_4].ToString();
            course.LdcsDesc5 = dr[Constants.CourseListColumns.CourseCursor.LDCSDESC_5].ToString();

            if (!string.IsNullOrEmpty(dr[Constants.CourseListColumns.CourseCursor.NUM_OF_OPPORTUNITIES].ToString()))
            {
                course.NumberOfOpportunities =
                    long.Parse(dr[Constants.CourseListColumns.CourseCursor.NUM_OF_OPPORTUNITIES].ToString());
            }

            course.CourseSummary = dr[Constants.CourseListColumns.CourseCursor.COURSE_SUMMARY].ToString();

            course.Opportunities = new List<Opportunity>();
            Opportunity opportunity = new Opportunity();

            opportunity.OpportunityId = dr[Constants.CourseListColumns.CourseCursor.OPPORTUNITY_ID].ToString();
            opportunity.StudyMode = dr[Constants.CourseListColumns.CourseCursor.STUDY_MODE].ToString();
            opportunity.AttendanceMode = dr[Constants.CourseListColumns.CourseCursor.ATTENDANCE_MODE].ToString();
            opportunity.AttendancePattern = dr[Constants.CourseListColumns.CourseCursor.ATTENDANCE_PATTERN].ToString();
            opportunity.StartDate = dr[Constants.CourseListColumns.CourseCursor.START_DATE].ToString();
            opportunity.StartDateDescription = dr[Constants.CourseListColumns.CourseCursor.START_DATE_DESCRIPTION].ToString();
            opportunity.EndDate = dr[Constants.CourseListColumns.CourseCursor.END_DATE].ToString();
            opportunity.RegionName = dr[Constants.CourseListColumns.CourseCursor.REGION_NAME].ToString();

            opportunity.Venue = new Venue();
            opportunity.Venue.VenueName = dr[Constants.CourseListColumns.CourseCursor.VENUE_NAME].ToString();
            opportunity.Venue.AddressLine1 = dr[Constants.CourseListColumns.CourseCursor.ADDRESS_LINE1].ToString();
            opportunity.Venue.AddressLine2 = dr[Constants.CourseListColumns.CourseCursor.ADDRESS_LINE2].ToString();
            opportunity.Venue.Town = dr[Constants.CourseListColumns.CourseCursor.TOWN].ToString();
            opportunity.Venue.County = dr[Constants.CourseListColumns.CourseCursor.COUNTY].ToString();
            opportunity.Venue.Postcode = dr[Constants.CourseListColumns.CourseCursor.POSTCODE].ToString();
            opportunity.Venue.Latitude = dr[Constants.CourseListColumns.CourseCursor.LATITUDE].ToString();
            opportunity.Venue.Longitude = dr[Constants.CourseListColumns.CourseCursor.LONGITUDE].ToString();

            opportunity.Distance = dr[Constants.CourseListColumns.CourseCursor.DISTANCE].ToString();
            opportunity.DurationUnit = dr[Constants.CourseListColumns.CourseCursor.DURATION_UNIT].ToString();
            opportunity.DurationDescription = dr[Constants.CourseListColumns.CourseCursor.DURATION_DESCRIPTION].ToString();

            if (dr[Constants.CourseListColumns.CourseCursor.DURATION_VALUE] != null &&
                (dr[Constants.CourseListColumns.CourseCursor.DURATION_VALUE].ToString() != null &&
                dr[Constants.CourseListColumns.CourseCursor.DURATION_VALUE].ToString().Length > 0))
            {
                opportunity.DurationValue = long.Parse(dr[Constants.CourseListColumns.CourseCursor.DURATION_VALUE].ToString());
            }

            course.Opportunities.Add(opportunity);

            return course;
        }

        /// <summary>
        /// Creates an LdcsCode object from data contained in the Data reader.
        /// </summary>
        /// <param name="dr">Data reader containing LdcsCode data.</param>
        /// <returns>Populated LdcsCode object.</returns>
        private static LdcsCode BuildLdcsCodeResult(OracleDataReader dr)
        {
            LdcsCode ldcsCode = new LdcsCode();

            ldcsCode.CourseCount = long.Parse(dr[Constants.CourseListColumns.LdcsCursor.LDCS_COUNTS].ToString());
            ldcsCode.LdcsCodeValue= dr[Constants.CourseListColumns.LdcsCursor.LDCS_CODE].ToString();
            ldcsCode.LdcsCodeDescription = dr[Constants.CourseListColumns.LdcsCursor.LDCS_DESCRIPTION].ToString();

            return ldcsCode;
        }

        #endregion Private Methods
    }
}
