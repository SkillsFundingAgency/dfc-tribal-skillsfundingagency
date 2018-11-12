CREATE PROCEDURE [dbo].[up_DailyReport]
	@DecimalPlaces int
	, @SFAProvider bit
	, @DFEProvider bit
AS

BEGIN

	DECLARE @LiveStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);

	SELECT P.ProviderId AS [ProviderId],
		CASE WHEN P.Ukprn = 0 THEN NULL ELSE P.Ukprn END AS Ukprn,
		P.ProviderName AS [ProviderName],
		--COALESCE(QS.Courses, 0) AS Courses,
		--(SELECT COUNT(*) FROM Course CO WHERE CO.RecordStatusId = @LiveStatus AND CO.ProviderId = P.ProviderId) as Courses,
		COALESCE(C.CourseCount, 0) AS Courses,
		COALESCE(QS.CourseInstances, 0) AS Opportunities,
		CASE WHEN COALESCE(QS.Courses, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.CourseInstances, 0) / CAST(COALESCE(QS.Courses, 0) AS FLOAT), @DecimalPlaces) END AS [OpportunitiesPerCourse],
		CASE WHEN COALESCE(QS.Courses, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.CoursesWithLongSummary, 0) / CAST(COALESCE(QS.Courses, 0) AS FLOAT) * 100, @DecimalPlaces) END AS [Summaries],
		CASE WHEN COALESCE(QS.Courses, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.CoursesWithDistinctLongSummary, 0) / CAST(COALESCE(QS.Courses, 0) AS FLOAT) * 100, @DecimalPlaces) END AS [DistinctSummaries],
		CASE WHEN COALESCE(QS.Courses, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.CoursesWithLearningAims, 0) / CAST(COALESCE(QS.Courses, 0) AS FLOAT) * 100, @DecimalPlaces) END AS [Aims],
		CASE WHEN COALESCE(QS.Courses, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.DistinctLearningAims, 0) / CAST(COALESCE(QS.Courses, 0) AS FLOAT) * 100, @DecimalPlaces) END AS [DistinctAims],
		CASE WHEN COALESCE(QS.Courses, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.CoursesWithUrls, 0) / CAST(COALESCE(QS.Courses, 0) AS FLOAT) * 100, @DecimalPlaces) END AS [Url],
		CASE WHEN COALESCE(QS.Courses, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.DistinctCourseUrls, 0) / CAST(COALESCE(QS.Courses, 0) AS FLOAT) * 100, @DecimalPlaces) END AS [DistinctUrl],
		CASE WHEN COALESCE(QS.Courses, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.CoursesWithBookingUrls, 0) / CAST(COALESCE(QS.Courses, 0) AS FLOAT) * 100, @DecimalPlaces) END AS [BookingUrl],
		CASE WHEN COALESCE(QS.Courses, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.DistinctCourseBookingUrls, 0) / CAST(COALESCE(QS.Courses, 0) AS FLOAT) * 100, @DecimalPlaces) END AS [DistinctBookingUrl],
		CASE WHEN COALESCE(QS.CourseInstances, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.CoursesWithSpecificStartDates, 0) / CAST(COALESCE(QS.CourseInstances, 0) AS FLOAT) * 100, @DecimalPlaces) END AS [SpecificStarts],
		CASE WHEN COALESCE(QS.CourseInstances, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.CoursesWithFutureStartDates, 0) / CAST(COALESCE(QS.CourseInstances, 0) AS FLOAT) * 100, @DecimalPlaces) END AS [FutureStarts],
		CASE WHEN COALESCE(QS.Courses, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.CoursesWithAnEntryRequirement, 0) / CAST(COALESCE(QS.Courses, 0) AS FLOAT) * 100, @DecimalPlaces) END AS [EntryRequirements],
		CASE WHEN COALESCE(QS.CourseInstances, 0) = 0 THEN 0 ELSE Round(COALESCE(QS.CoursesWithSpecificPrices, 0) / CAST(COALESCE(QS.CourseInstances, 0) AS FLOAT) * 100, @DecimalPlaces) END AS [Prices],
		QS.LastActivity,
		QS.ModifiedDateTimeUtc AS [LastUpdated],
		QS.AutoAggregateQualityRating AS Autoscore,
		IOs.Name AS InformationOfficer,
		RMs.Name AS RelationshipManager,
		CASE WHEN COALESCE(UC.SuperUserCount, 0) > 0 THEN 1 ELSE 0 END AS [LiveSuperuser],
		PR.RegionName AS Region,
		CASE WHEN P.DfERegionId IS NULL THEN '' ELSE PRDfE.DfERegionName END AS DfERegion,
		PT.ProviderTypeName AS [ProviderType],
		CASE WHEN P.DfEProviderTypeId IS NULL THEN '' ELSE PTDfE.DfEProviderTypeName END AS DfEProviderType,
		CASE WHEN P.DfEProviderStatusId IS NULL THEN '' ELSE DFEPS.DfEProviderStatusName END AS DfEProviderStatus,
		CASE WHEN P.DfELocalAuthorityId IS NULL THEN '' ELSE DFELA.DfELocalAuthorityName END AS DfELocalAuthority,
		CASE WHEN P.DfEEstablishmentTypeId IS NULL THEN '' ELSE DFEET.DfEEstablishmentTypeName END AS DfEEstablishmentType,
		P.RoATPFFlag AS RoATP,
		p.IsTASOnly
	FROM Provider P
		LEFT OUTER JOIN (SELECT ProviderId, Count(*) AS CourseCount FROM Course WHERE RecordStatusId = @LiveStatus GROUP BY ProviderId) C ON C.ProviderId = P.ProviderId
		LEFT OUTER JOIN QualityScore QS ON QS.ProviderId = P.ProviderId
		LEFT OUTER JOIN AspNetUsers IOs ON IOs.Id = P.InformationOfficerUserId
		LEFT OUTER JOIN AspNetUsers RMs ON RMs.Id = P.RelationshipManagerUserId
		LEFT OUTER JOIN (
							SELECT PU.ProviderId,
								Count(DISTINCT PU.UserId) As UserCount,
								SUM(CASE WHEN P.PermissionName = 'CanAddEditProviderUsers' THEN 1 ELSE 0 END) AS SuperUserCount
							FROM ProviderUser PU
								LEFT OUTER JOIN AspNetUsers U ON U.Id = PU.UserId
								LEFT OUTER JOIN AspNetUserRoles UR ON UR.UserId = PU.UserId
								LEFT OUTER JOIN PermissionInRole PIR ON PIR.RoleId = UR.RoleId
								LEFT OUTER JOIN Permission P ON P.PermissionId = PIR.PermissionId
							WHERE U.IsDeleted = 0
							GROUP BY PU.ProviderId
						) UC ON UC.ProviderId = P.ProviderId
		LEFT OUTER JOIN ProviderRegion PR ON PR.ProviderRegionId = P.ProviderRegionId
		LEFT OUTER JOIN DfERegion PRDfE ON PRDfE.DfERegionId = P.DfERegionId
		LEFT OUTER JOIN ProviderType PT ON PT.ProviderTypeId = P.ProviderTypeId
		LEFT OUTER JOIN DfEProviderType PTDfE ON PTDfE.DfEProviderTypeId = P.DfEProviderTypeId
		LEFT OUTER JOIN DfEProviderStatus DFEPS ON DFEPS.DfEProviderStatusId = P.DfEProviderStatusId
		LEFT OUTER JOIN DfELocalAuthority DFELA ON DFELA.DfELocalAuthorityId = P.DfELocalAuthorityId
		LEFT OUTER JOIN DfEEstablishmentType DFEET ON DFEET.DfEEstablishmentTypeId = P.DfEEstablishmentTypeId
	WHERE P.RecordStatusId = @LiveStatus
		AND ((@SFAProvider = 1 AND p.SFAFunded = 1) OR (@DFEProvider = 1 AND p.DFE1619Funded = 1))
	ORDER BY P.ProviderId;

	IF(@@ERROR <> 0)
	BEGIN
		RETURN -1;
	END;

	RETURN 0;

END;
