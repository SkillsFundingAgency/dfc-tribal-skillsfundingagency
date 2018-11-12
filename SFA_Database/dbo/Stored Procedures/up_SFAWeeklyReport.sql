CREATE PROCEDURE [dbo].[up_SFAWeeklyReport]
	@SFAProvider BIT, 
	@DFEProvider BIT
AS

BEGIN

	DECLARE @LiveStatusId INT = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);
	DECLARE @DeletedStatusId INT = (SELECT RecordStatusId FROM RecordStatus WHERE IsDeleted = 1);
	DECLARE @PortalApplicationId INT = 1;
	DECLARE @BulkUploadApplicationId INT = 2;
	DECLARE @MayThisYear DATE;
	DECLARE @MayLastYear DATE;
	DECLARE @Year INT = DATEPART(Year, GetUtcDate());
	DECLARE @DefaultDate DATE = CAST('01 JAN 2000' AS DATE);

	IF (DATEPART(Month, GetUtcDate()) < 5)
		SET @Year = @Year - 1;

	SET @MayThisYear = CAST('01/05/' + CAST(@Year AS VARCHAR) AS DATE);
	SET @MayLastYear = DATEADD(Year, -1, @MayThisYear);

	SELECT SU.SuperUserCount AS NumberOfProvidersWithSuperusers,
		Providers.UpdatedAtLeast1Opportunity,
		CourseInstance.BulkUploadOpportunities,
		CourseInstance.BulkUploadOpportunitiesLast7Days,
		CourseInstance.ManualOpportunities,
		CourseInstance.ManualOpportunitiesLast7Days,
		Providers.NotUpdatedInLastYear,
		Providers.UpdatedInLast7Days,
		Providers.UpdatedBetween8and30Days,
		Providers.UpdatedBetween31and60Days,
		Providers.UpdatedBetween61and90Days,
		Providers.UpdatedMoreThan90Days,
		Providers.PublishedProviders,
		Providers.UnpublishedProviders,
		Courses.NumberOfCourses,
		Courses.NumberOfLiveCourses,
		CourseInstance.NumberOfLiveOpportunities,
		Courses.NumberOfInScopeCourses,
		Courses.NumberOfOutOfScopeCourses,
		CourseInstance.NumberOfInScopeOpportunities,
		CourseInstance.NumberOfOutOfScopeOpportunities,
		Providers.NumberOfProvidersWithNoCourses,
		Courses.NumberOfType1Courses,
		Courses.NumberOfType2Courses,
		Courses.NumberOfType3Courses,
		A10Codes.A1010,
		A10Codes.A1021,
		A10Codes.A1022,
		A10Codes.A1025,
		A10Codes.A1035,
		A10Codes.A1045,
		A10Codes.A1046,
		A10Codes.A1070,
		A10Codes.A1080,
		A10Codes.A1081,
		A10Codes.A1082,
		A10Codes.A1099,
		A10Codes.A10NA,
		CourseInstance.StudyModeFlexible,
		CourseInstance.StudyModeFullTime,
		CourseInstance.StudyModeNotKnown,
		CourseInstance.StudyModePartOfFullTimeProgramme,
		CourseInstance.StudyModePartTime,
		CourseInstance.AttendanceModeDistanceWithAttendance,
		CourseInstance.AttendanceModeDistanceWithoutAttendance,
		CourseInstance.AttendanceModeFaceToFace,
		CourseInstance.AttendanceModeLocation,
		CourseInstance.AttendanceModeMixed,
		CourseInstance.AttendanceModeNotKnown,
		CourseInstance.AttendanceModeOnlineWithAttendance,
		CourseInstance.AttendanceModeOnlineWithoutAttendance,
		CourseInstance.AttendanceModeWorkBased,
		CourseInstance.AttendancePatternCustomised,
		CourseInstance.AttendancePatternDayRelease,
		CourseInstance.AttendancePatternDaytime,
		CourseInstance.AttendancePatternEvening,
		CourseInstance.AttendancePatternNotApplicable,
		CourseInstance.AttendancePatternNotKnown,
		CourseInstance.AttendancePatternTwilight,
		CourseInstance.AttendancePatternWeekend,
		CourseInstance.Duration1Week,
		CourseInstance.Duration1To4Weeks,
		CourseInstance.Duration1To3Months,
		CourseInstance.Duration3To6Months,
		CourseInstance.Duration6To12Months,
		CourseInstance.Duration1To2Years,
		CourseInstance.DurationNotKnown,
		Courses.QualType14To19Diploma,
		Courses.QualTypeAccessToHigher,
		Courses.QualTypeApprenticeship,
		Courses.QualTypeBasicSkill,
		Courses.QualTypeCertificateOfAttendance,
		Courses.QualTypeCourseProviderCertificate,
		Courses.QualTypeExternalAward,
		Courses.QualTypeFoundationDegree,
		Courses.QualTypeFunctionalSkills,
		Courses.QualTypeGCE,
		Courses.QualTypeGSCE,
		Courses.QualTypeHND,
		Courses.QualTypeBaccalaureate,
		Courses.QualTypeNoQualification,
		Courses.QualTypeNVQ,
		Courses.QualTypeOtherAccredited,
		Courses.QualTypePostgraduate,
		Courses.QualTypeProfessionalOrIndustrySpecific,
		Courses.QualTypeUndergraduate,
		Courses.QualLevelEntry,
		Courses.QualLevelHigher,
		Courses.QualLevel1,
		Courses.QualLevel2,
		Courses.QualLevel3,
		Courses.QualLevel4,
		Courses.QualLevel5,
		Courses.QualLevel6,
		Courses.QualLevel7,
		Courses.QualLevel8,
		Courses.QualLevelUnknown,
		SU.ProvidersWithSuperusersUpdatedInLastMonth,
		SU.ProvidersWithSuperusersUpdatedBetween2and3Months,
		SU.ProvidersWithSuperusersUpdatedMoreThan3Months,
		SU.ProvidersWithLoggedInUsersNotUpdatedSince01Aug2010,
		TrafficLight.TrafficLightGreen,
		TrafficLight.TrafficLightAmber,
		TrafficLight.TrafficLightRed,
		QualityScores.QualityVeryGood,
		QualityScores.QualityGood,
		QualityScores.QualityAverage,
		QualityScores.QualityPoor,
		QualityScores.QualityPoorAndAverage,
		QualityScores.QualityGoodAndVeryGood,
		QualityScores.AverageScore,
		CourseInstance.NumberOfOpportunities,
		UserSessions.TotalUserSessions,
		UserSessions.ProviderPortalUserSessions,
		UserSessions.SecureAccessUserSessions
	FROM	
	(	
		SELECT Sum(CASE WHEN A.SuperUserCount > 0 THEN 1 ELSE 0 END) As SuperUserCount,
			Sum(CASE WHEN A.SuperUserCount > 0 AND A.LastUpdatedOpportunity >= DateAdd(m, -1, GetUtcDate()) THEN 1 ELSE 0 END) AS ProvidersWithSuperusersUpdatedInLastMonth,
			Sum(CASE WHEN A.SuperUserCount > 0 AND A.LastUpdatedOpportunity >= DateAdd(m, -2, GetUtcDate()) AND A.LastUpdatedOpportunity < DateAdd(m, -2, GetUtcDate()) THEN 1 ELSE 0 END) AS ProvidersWithSuperusersUpdatedBetween2and3Months,
			Sum(CASE WHEN A.SuperUserCount > 0 AND A.LastUpdatedOpportunity < DateAdd(m, -3, GetUtcDate()) THEN 1 ELSE 0 END) AS ProvidersWithSuperusersUpdatedMoreThan3Months,
			Sum(CASE WHEN A.LastLoginDateTime IS NOT NULL AND COALESCE(A.LastUpdatedOpportunity, CAST('01 Aug 2010' AS DATE)) <= CAST('01 Aug 2010' AS DATE) THEN 1 ELSE 0 END) AS ProvidersWithLoggedInUsersNotUpdatedSince01Aug2010
		FROM (
				SELECT PU.ProviderId,
					SUM(CASE WHEN P.PermissionName = 'CanAddEditProviderUsers' THEN 1 ELSE 0 END) AS SuperUserCount,
					Max(CI.LastUpdated) AS LastUpdatedOpportunity,
					Max(U.LastLoginDateTimeUtc) AS LastLoginDateTime
				FROM Provider PR
					INNER JOIN ProviderUser PU ON PU.ProviderId = PR.ProviderId
					LEFT OUTER JOIN AspNetUsers U ON U.Id = PU.UserId
					LEFT OUTER JOIN AspNetUserRoles UR ON UR.UserId = PU.UserId
					LEFT OUTER JOIN PermissionInRole PIR ON PIR.RoleId = UR.RoleId
					LEFT OUTER JOIN Permission P ON P.PermissionId = PIR.PermissionId
					LEFT OUTER JOIN (
										SELECT C.ProviderId,
											Max(COALESCE(CI.ModifiedDateTimeUtc, CI.CreatedDateTimeUtc)) As LastUpdated
										FROM Course C
											INNER JOIN CourseInstance CI ON CI.CourseId = C.CourseId
										GROUP BY C.ProviderId
									) CI ON CI.ProviderId = PU.ProviderId
				WHERE U.IsDeleted = 0
					AND ((@SFAProvider = 1 AND PR.SFAFunded = 1) OR (@DFEProvider = 1 AND PR.DFE1619Funded = 1))
					AND PR.RecordStatusId = @LiveStatusId
				GROUP BY PU.ProviderId
			) A
	) SU,
	(
		SELECT Sum(CASE WHEN CourseInstance.AddedByApplicationId = @BulkUploadApplicationId AND CourseInstance.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS BulkUploadOpportunities,
			Sum(CASE WHEN CourseInstance.AddedByApplicationId = @BulkUploadApplicationId AND CourseInstance.RecordStatusId = @LiveStatusId AND COALESCE(CourseInstance.ModifiedDateTimeUtc, CourseInstance.CreatedDateTimeUtc) >= DateAdd(dd, -7, GetUtcDate()) THEN 1 ELSE 0 END) AS BulkUploadOpportunitiesLast7Days,
			Sum(CASE WHEN CourseInstance.AddedByApplicationId = @PortalApplicationId AND CourseInstance.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS ManualOpportunities,
			Sum(CASE WHEN CourseInstance.AddedByApplicationId = @PortalApplicationId AND CourseInstance.RecordStatusId = @LiveStatusId AND COALESCE(CourseInstance.ModifiedDateTimeUtc, CourseInstance.CreatedDateTimeUtc) >= DateAdd(dd, -7, GetUtcDate()) THEN 1 ELSE 0 END) AS ManualOpportunitiesLast7Days,
			Sum(CASE WHEN CourseInstance.RecordStatusId = @DeletedStatusId THEN 0 ELSE 1 END) AS NumberOfOpportunities,
			Sum(CASE WHEN CourseInstance.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumberOfLiveOpportunities,
			Sum(CASE WHEN InScopeOpportunities.CourseInstanceId IS NOT NULL AND CourseInstance.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumberOfInScopeOpportunities,
			Sum(CASE WHEN OutOfScopeOpportunities.CourseInstanceId IS NOT NULL AND CourseInstance.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumberOfOutOfScopeOpportunities,
			Sum(CASE WHEN CourseInstance.StudyModeId = 4 THEN 1 ELSE 0 END) AS StudyModeFlexible,
			Sum(CASE WHEN CourseInstance.StudyModeId = 1 THEN 1 ELSE 0 END) AS StudyModeFullTime,
			Sum(CASE WHEN CourseInstance.StudyModeId = 5 THEN 1 ELSE 0 END) AS StudyModeNotKnown,
			Sum(CASE WHEN CourseInstance.StudyModeId = 3 THEN 1 ELSE 0 END) AS StudyModePartOfFullTimeProgramme,
			Sum(CASE WHEN CourseInstance.StudyModeId = 2 THEN 1 ELSE 0 END) AS StudyModePartTime,
			Sum(CASE WHEN CourseInstance.AttendanceTypeId = 5 THEN 1 ELSE 0 END) AS AttendanceModeDistanceWithAttendance,
			Sum(CASE WHEN CourseInstance.AttendanceTypeId = 6 THEN 1 ELSE 0 END) AS AttendanceModeDistanceWithoutAttendance,
			Sum(CASE WHEN CourseInstance.AttendanceTypeId = 2 THEN 1 ELSE 0 END) AS AttendanceModeFaceToFace,
			Sum(CASE WHEN CourseInstance.AttendanceTypeId = 1 THEN 1 ELSE 0 END) AS AttendanceModeLocation,
			Sum(CASE WHEN CourseInstance.AttendanceTypeId = 4 THEN 1 ELSE 0 END) AS AttendanceModeMixed,
			Sum(CASE WHEN CourseInstance.AttendanceTypeId = 9 THEN 1 ELSE 0 END) AS AttendanceModeNotKnown,
			Sum(CASE WHEN CourseInstance.AttendanceTypeId = 8 THEN 1 ELSE 0 END) AS AttendanceModeOnlineWithAttendance,
			Sum(CASE WHEN CourseInstance.AttendanceTypeId = 7 THEN 1 ELSE 0 END) AS AttendanceModeOnlineWithoutAttendance,
			Sum(CASE WHEN CourseInstance.AttendanceTypeId = 3 THEN 1 ELSE 0 END) AS AttendanceModeWorkBased,
			Sum(CASE WHEN CourseInstance.AttendancePatternId = 6 THEN 1 ELSE 0 END) AS AttendancePatternCustomised,
			Sum(CASE WHEN CourseInstance.AttendancePatternId = 2 THEN 1 ELSE 0 END) AS AttendancePatternDayRelease,
			Sum(CASE WHEN CourseInstance.AttendancePatternId = 1 THEN 1 ELSE 0 END) AS AttendancePatternDaytime,
			Sum(CASE WHEN CourseInstance.AttendancePatternId = 3 THEN 1 ELSE 0 END) AS AttendancePatternEvening,
			Sum(CASE WHEN CourseInstance.AttendancePatternId = 8 THEN 1 ELSE 0 END) AS AttendancePatternNotApplicable,
			Sum(CASE WHEN CourseInstance.AttendancePatternId = 7 THEN 1 ELSE 0 END) AS AttendancePatternNotKnown,
			Sum(CASE WHEN CourseInstance.AttendancePatternId = 4 THEN 1 ELSE 0 END) AS AttendancePatternTwilight,
			Sum(CASE WHEN CourseInstance.AttendancePatternId = 5 THEN 1 ELSE 0 END) AS AttendancePatternWeekend,
			Sum(CASE WHEN CourseInstance.DurationUnit IS NOT NULL AND CourseInstance.DurationUnit * DU.WeekEquivalent <= 1 THEN 1 ELSE 0 END) AS Duration1Week,
			Sum(CASE WHEN CourseInstance.DurationUnit IS NOT NULL AND CourseInstance.DurationUnit * DU.WeekEquivalent > 1 AND CourseInstance.DurationUnit * DU.WeekEquivalent <= 4 THEN 1 ELSE 0 END) AS Duration1To4Weeks,
			Sum(CASE WHEN CourseInstance.DurationUnit IS NOT NULL AND CourseInstance.DurationUnit * DU.WeekEquivalent > 4 AND CourseInstance.DurationUnit * DU.WeekEquivalent <= 13 THEN 1 ELSE 0 END) AS Duration1To3Months,
			Sum(CASE WHEN CourseInstance.DurationUnit IS NOT NULL AND CourseInstance.DurationUnit * DU.WeekEquivalent > 13 AND CourseInstance.DurationUnit * DU.WeekEquivalent <= 26 THEN 1 ELSE 0 END) AS Duration3To6Months,
			Sum(CASE WHEN CourseInstance.DurationUnit IS NOT NULL AND CourseInstance.DurationUnit * DU.WeekEquivalent > 26 AND CourseInstance.DurationUnit * DU.WeekEquivalent <= 52 THEN 1 ELSE 0 END) AS Duration6To12Months,
			Sum(CASE WHEN CourseInstance.DurationUnit IS NOT NULL AND CourseInstance.DurationUnit * DU.WeekEquivalent > 52 AND CourseInstance.DurationUnit * DU.WeekEquivalent <= 104 THEN 1 ELSE 0 END) AS Duration1To2Years,
			Sum(CASE WHEN CourseInstance.DurationUnit IS NULL THEN 1 ELSE 0 END) AS DurationNotKnown
		FROM CourseInstance
			LEFT OUTER JOIN (SELECT DISTINCT CourseInstanceId FROM CourseInstanceA10FundingCode WHERE A10FundingCode IN ('10', '22', '45', '46', '70', '80', '81')) InScopeOpportunities ON InScopeOpportunities.CourseInstanceId = CourseInstance.CourseInstanceId
			LEFT OUTER JOIN (SELECT DISTINCT CourseInstanceId FROM CourseInstanceA10FundingCode WHERE A10FundingCode IN ('21', '82', '99') UNION SELECT DISTINCT CourseInstanceId FROM CourseInstance WHERE CourseInstanceId NOT IN (SELECT DISTINCT CourseInstanceId FROM CourseInstanceA10FundingCode)) OutOfScopeOpportunities ON OutOfScopeOpportunities.CourseInstanceId = CourseInstance.CourseInstanceId
			LEFT OUTER JOIN DurationUnit DU ON DU.DurationUnitId = CourseInstance.DurationUnitId
			INNER JOIN Course C ON C.CourseId = CourseInstance.CourseId
			INNER JOIN Provider P ON P.ProviderId = C.ProviderId
		WHERE ((@SFAProvider = 1 AND P.SFAFunded = 1) OR (@DFEProvider = 1 AND P.DFE1619Funded = 1))
			AND P.RecordStatusId = @LiveStatusId
	) CourseInstance,
	(
		SELECT Sum(CASE WHEN LastUpdated IS NOT NULL THEN 1 ELSE 0 END) AS UpdatedAtLeast1Opportunity,
			Sum(CASE WHEN LastUpdated IS NULL OR LastUpdated < DateAdd(yyyy, -1, GetUtcDate()) THEN 1 ELSE 0 END) AS NotUpdatedInLastYear,
			Sum(CASE WHEN LastUpdated IS NOT NULL AND DATEDIFF(dd, LastUpdated, GetUtcDate()) <= 7 THEN 1 ELSE 0 END) AS UpdatedInLast7Days,
			Sum(CASE WHEN LastUpdated IS NOT NULL AND DATEDIFF(dd, LastUpdated, GetUtcDate()) BETWEEN 8 AND 30 THEN 1 ELSE 0 END) AS UpdatedBetween8and30Days,
			Sum(CASE WHEN LastUpdated IS NOT NULL AND DATEDIFF(dd, LastUpdated, GetUtcDate()) BETWEEN 31 AND 60 THEN 1 ELSE 0 END) AS UpdatedBetween31and60Days,
			Sum(CASE WHEN LastUpdated IS NOT NULL AND DATEDIFF(dd, LastUpdated, GetUtcDate()) BETWEEN 61 AND 90 THEN 1 ELSE 0 END) AS UpdatedBetween61and90Days,
			Sum(CASE WHEN LastUpdated IS NOT NULL AND DATEDIFF(dd, LastUpdated, GetUtcDate()) > 90 THEN 1 ELSE 0 END) AS UpdatedMoreThan90Days,
			Sum(CASE WHEN CourseProviderId IS NULL THEN 1 ELSE 0 END) AS NumberOfProvidersWithNoCourses,
			Sum(CASE WHEN PublishData = 1 THEN 1 ELSE 0 END) PublishedProviders,
			Sum(CASE WHEN PublishData = 1 THEN 0 ELSE 1 END) UnpublishedProviders
		FROM (
				SELECT P.ProviderId,
					MAX(COALESCE(CI.ModifiedDateTimeUtc, CI.CreatedDateTimeUtc)) AS LastUpdated,
					C.ProviderId As CourseProviderId, P.PublishData					
				FROM Provider P
					LEFT OUTER JOIN (SELECT * FROM Course WHERE RecordStatusId = @LiveStatusId) C ON C.ProviderId = P.ProviderId
					LEFT OUTER JOIN (SELECT * FROM CourseInstance WHERE RecordStatusId = @LiveStatusId) CI ON CI.CourseId = C.CourseId
				WHERE ((@SFAProvider = 1 AND p.SFAFunded = 1) OR (@DFEProvider = 1 AND p.DFE1619Funded = 1))
					AND P.RecordStatusId = @LiveStatusId
				GROUP BY P.ProviderId,
					C.ProviderId, P.PublishData
			) A
	) Providers,	
	(
			SELECT Count(*) AS NumberOfCourses,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumberOfLiveCourses,
			Sum(CASE WHEN InScopeCourses.CourseId IS NOT NULL AND Course.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumberOfInScopeCourses,
			Sum(CASE WHEN OutOfScopeCourses.CourseId IS NOT NULL AND Course.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumberOfOutOfScopeCourses,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.LearningAimRefId IS NOT NULL AND LAR.QualificationTypeId IS NOT NULL THEN 1 ELSE 0 END) AS NumberOfType1Courses,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.LearningAimRefId IS NOT NULL AND LAR.QualificationTypeId IS NULL THEN 1 ELSE 0 END) AS NumberOfType2Courses,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.LearningAimRefId IS NULL THEN 1 ELSE 0 END) AS NumberOfType3Courses,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 9 THEN 1 ELSE 0 END) AS QualType14To19Diploma,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 14 THEN 1 ELSE 0 END) AS QualTypeAccessToHigher,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 10 THEN 1 ELSE 0 END) AS QualTypeApprenticeship,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 4 THEN 1 ELSE 0 END) AS QualTypeBasicSkill,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 2 THEN 1 ELSE 0 END) AS QualTypeCertificateOfAttendance,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 5 THEN 1 ELSE 0 END) AS QualTypeCourseProviderCertificate,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 6 THEN 1 ELSE 0 END) AS QualTypeExternalAward,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 16 THEN 1 ELSE 0 END) AS QualTypeFoundationDegree,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 3 THEN 1 ELSE 0 END) AS QualTypeFunctionalSkills,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 13 THEN 1 ELSE 0 END) AS QualTypeGCE,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 8 THEN 1 ELSE 0 END) AS QualTypeGSCE,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 15 THEN 1 ELSE 0 END) AS QualTypeHND,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 12 THEN 1 ELSE 0 END) AS QualTypeBaccalaureate,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 1 THEN 1 ELSE 0 END) AS QualTypeNoQualification,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 11 THEN 1 ELSE 0 END) AS QualTypeNVQ,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 7 THEN 1 ELSE 0 END) AS QualTypeOtherAccredited,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 18 THEN 1 ELSE 0 END) AS QualTypePostgraduate,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 19 THEN 1 ELSE 0 END) AS QualTypeProfessionalOrIndustrySpecific,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND COALESCE(Course.WhenNoLarQualificationTypeId, LAR.QualificationTypeId) = 17 THEN 1 ELSE 0 END) AS QualTypeUndergraduate,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.QualificationLevelId = 10 THEN 1 ELSE 0 END) AS QualLevelEntry,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.QualificationLevelId = 9 THEN 1 ELSE 0 END) AS QualLevelHigher,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.QualificationLevelId = 1 THEN 1 ELSE 0 END) AS QualLevel1,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.QualificationLevelId = 2 THEN 1 ELSE 0 END) AS QualLevel2,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.QualificationLevelId = 3 THEN 1 ELSE 0 END) AS QualLevel3,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.QualificationLevelId = 4 THEN 1 ELSE 0 END) AS QualLevel4,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.QualificationLevelId = 5 THEN 1 ELSE 0 END) AS QualLevel5,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.QualificationLevelId = 6 THEN 1 ELSE 0 END) AS QualLevel6,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.QualificationLevelId = 7 THEN 1 ELSE 0 END) AS QualLevel7,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.QualificationLevelId = 8 THEN 1 ELSE 0 END) AS QualLevel8,
			Sum(CASE WHEN Course.RecordStatusId = @LiveStatusId AND Course.QualificationLevelId = 11 THEN 1 ELSE 0 END) AS QualLevelUnknown
		FROM Course
			LEFT OUTER JOIN LearningAim LAR ON LAR.LearningAimRefId = Course.LearningAimRefId
			LEFT OUTER JOIN (SELECT DISTINCT CI.CourseId FROM CourseInstance CI INNER JOIN CourseInstanceA10FundingCode CIFC ON CIFC.CourseInstanceId = CI.CourseInstanceId WHERE CIFC.A10FundingCode IN ('10', '22', '45', '46', '70', '80', '81')) InScopeCourses ON InScopeCourses.CourseId = Course.CourseId
			LEFT OUTER JOIN (SELECT DISTINCT CI.CourseId FROM CourseInstance CI INNER JOIN CourseInstanceA10FundingCode CIFC ON CIFC.CourseInstanceId = CI.CourseInstanceId WHERE CIFC.A10FundingCode IN ('21', '82', '99') UNION SELECT DISTINCT CourseId FROM CourseInstance WHERE CourseInstanceId NOT IN (SELECT DISTINCT CourseInstanceId FROM CourseInstanceA10FundingCode)) OutOfScopeCourses ON OutOfScopeCourses.CourseId = Course.CourseId
			INNER JOIN Provider P ON P.ProviderId = Course.ProviderId
		WHERE ((@SFAProvider = 1 AND P.SFAFunded = 1) OR (@DFEProvider = 1 AND P.DFE1619Funded = 1))
			AND P.RecordStatusId = @LiveStatusId
	) Courses,
	(	
		SELECT Sum(CASE WHEN FundingCode = '10' THEN Count ELSE 0 END) AS A1010,
			Sum(CASE WHEN FundingCode = '21' THEN Count ELSE 0 END) AS A1021,
			Sum(CASE WHEN FundingCode = '22' THEN Count ELSE 0 END) AS A1022,
			Sum(CASE WHEN FundingCode = '25' THEN Count ELSE 0 END) AS A1025,
			Sum(CASE WHEN FundingCode = '35' THEN Count ELSE 0 END) AS A1035,
			Sum(CASE WHEN FundingCode = '45' THEN Count ELSE 0 END) AS A1045,
			Sum(CASE WHEN FundingCode = '46' THEN Count ELSE 0 END) AS A1046,
			Sum(CASE WHEN FundingCode = '70' THEN Count ELSE 0 END) AS A1070,
			Sum(CASE WHEN FundingCode = '80' THEN Count ELSE 0 END) AS A1080,
			Sum(CASE WHEN FundingCode = '81' THEN Count ELSE 0 END) AS A1081,
			Sum(CASE WHEN FundingCode = '82' THEN Count ELSE 0 END) AS A1082,
			Sum(CASE WHEN FundingCode = '99' THEN Count ELSE 0 END) AS A1099,
			Sum(CASE WHEN FundingCode = 'NA' THEN Count ELSE 0 END) AS A10NA
		FROM
		(
			SELECT CAST(A10FundingCode AS VARCHAR) AS FundingCode,
				Count(DISTINCT CI.CourseId) As Count
			FROM A10FundingCode A10
				LEFT OUTER JOIN CourseInstanceA10FundingCode CIA10 ON CIA10.A10FundingCode = A10.A10FundingCodeId
				LEFT OUTER JOIN (SELECT * FROM CourseInstance WHERE RecordStatusId = @LiveStatusId) CI ON CI.CourseInstanceId = CIA10.CourseInstanceId
				INNER JOIN Course C ON C.CourseId = CI.CourseId
				INNER JOIN Provider P ON P.ProviderId = C.ProviderId
			WHERE ((@SFAProvider = 1 AND P.SFAFunded = 1) OR (@DFEProvider = 1 AND P.DFE1619Funded = 1))
				AND P.RecordStatusId = @LiveStatusId
			GROUP BY A10FundingCode

			UNION 

			SELECT 'NA',
				Count(DISTINCT CI.CourseId) As Count
			FROM CourseInstance CI
				INNER JOIN Course C ON C.CourseId = CI.CourseId
				INNER JOIN Provider P ON P.ProviderId = C.ProviderId
				LEFT OUTER JOIN CourseInstanceA10FundingCode CIFC ON CIFC.CourseInstanceId = CI.CourseInstanceId
			WHERE ((@SFAProvider = 1 AND P.SFAFunded = 1) OR (@DFEProvider = 1 AND P.DFE1619Funded = 1))
				AND P.RecordStatusId = @LiveStatusId
				AND CI.RecordStatusId = @LiveStatusId
				AND CIFC.CourseInstanceId IS NULL
		) A
	) A10Codes,	
	(
		SELECT 
			Sum(CASE WHEN COALESCE(AutoAggregateQualityRating, 0) >= 91 THEN 1 ELSE 0 END) AS QualityVeryGood,
			Sum(CASE WHEN COALESCE(AutoAggregateQualityRating, 0) >= 71 AND AutoAggregateQualityRating < 91 THEN 1 ELSE 0 END) AS QualityGood,
			Sum(CASE WHEN COALESCE(AutoAggregateQualityRating, 0) >= 51 AND AutoAggregateQualityRating < 71 THEN 1 ELSE 0 END) AS QualityAverage,
			Sum(CASE WHEN COALESCE(AutoAggregateQualityRating, 0) < 51 THEN 1 ELSE 0 END) AS QualityPoor,
			Sum(CASE WHEN COALESCE(AutoAggregateQualityRating, 0) < 71 THEN 1 ELSE 0 END) As QualityPoorAndAverage,
			Sum(CASE WHEN COALESCE(AutoAggregateQualityRating, 0) >= 71 THEN 1 ELSE 0 END) As QualityGoodAndVeryGood,
			Avg(AutoAggregateQualityRating) As AverageScore,
			SUM(COALESCE(QualityScore.CourseInstances, 0)) as NumberOfOpportunities		
		FROM Provider P
			LEFT OUTER JOIN QualityScore ON QualityScore.ProviderId = P.ProviderId
		WHERE ((@SFAProvider = 1 AND P.SFAFunded = 1) OR (@DFEProvider = 1 AND P.DFE1619Funded = 1))
			AND P.RecordStatusId = @LiveStatusId
	) QualityScores,
	(
		SELECT
			Count(*) TotalUserSessions
			, SUM(CASE WHEN r.IsSecureAccessUser = 0 THEN 1 ELSE 0 END) ProviderPortalUserSessions 
			, SUM(Convert(int, r.IsSecureAccessUser)) SecureAccessUserSessions
		FROM (
			SELECT DISTINCT
				Id, IsSecureAccessUser, LastLoginDateTimeUtc
			FROM Audit_AspNetUsers u
			WHERE u.LastLoginDateTimeUtc >= Convert(date,GetUtcDate() - 7)
		) r
	) UserSessions,
	(
		SELECT
			Sum(CASE WHEN Traffic.TrafficLightStatus = 1 THEN 1 ELSE 0 END) TrafficLightRed
			, Sum(CASE WHEN Traffic.TrafficLightStatus = 2 THEN 1 ELSE 0 END) TrafficLightAmber
			, Sum(CASE WHEN Traffic.TrafficLightStatus = 3 THEN 1 ELSE 0 END) TrafficLightGreen
		FROM (
			SELECT
				p.ProviderId
				, dbo.GetTrafficStatus(CASE WHEN qs.ModifiedDateTimeUtc > COALESCE(QS.LastActivity, @DefaultDate) THEN QS.ModifiedDateTimeUtc ELSE QS.LastActivity END, p.SFAFunded, p.DFE1619Funded) TrafficLightStatus
			FROM Provider p
				LEFT JOIN QualityScore qs ON p.ProviderId = qs.ProviderId
			WHERE ((@SFAProvider = 1 AND P.SFAFunded = 1) OR (@DFEProvider = 1 AND P.DFE1619Funded = 1))
				AND P.RecordStatusId = @LiveStatusId
		) Traffic
	) TrafficLight;

	IF(@@ERROR <> 0)
	BEGIN
		RETURN -1;
	END;

	RETURN 0;

END;


