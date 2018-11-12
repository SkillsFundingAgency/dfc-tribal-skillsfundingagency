CREATE PROCEDURE [dbo].[up_WeeklyReport]
	@SFAProvider bit
	, @DFEProvider bit
AS

BEGIN

	DECLARE @LiveStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);
	DECLARE @BulkUploadAppId int = 2;
	DECLARE @PortalAppId int = 1;

	SELECT P.ProviderId,
		P.Ukprn,
		P.ProviderName,	
		P.SFAFunded,
		P.DFE1619Funded,
		QS.LastActivity,
		QS.ModifiedDateTimeUtc As LastUpdate,
		CI.LastUpdated AS LastOpportunityUpdate,
		QS.AutoAggregateQualityRating AS Autoscore,
		APP.ApplicationName as LastUpdateMethod,		
		COALESCE(BH.BulkUploadSuccess, 0) AS BulkUploadSuccess,
		COALESCE(BH.BulkUploads, 0) AS BulkUploads,
		COALESCE(UC.SuperUserCount, 0) AS NumberOfSuperUsers,
		COALESCE(UC.UserCount, 0) AS NumberOfUsers,
		COALESCE(C.NumCourses, 0) AS Courses,
		COALESCE(C.NumBUCourses, 0) AS BulkUploadCourses,
		COALESCE(C.NumPPCourses, 0) AS ProviderPortalCourses,
		0 AS PlanBCourses,
		COALESCE(C.NumCoursesWithLearningAims, 0) AS CoursesWithLearningAims,
		COALESCE(C.NumCoursesWithoutLearningAims, 0) AS CoursesWithoutLearningAims,
		COALESCE(CI.NumberOfOpportunities, 0) AS Opportunities,
		COALESCE(ISO.InScopeOpportunities, 0) AS InScopeOpportunities,
		COALESCE(OSO.OutOfScopeOpportunities, 0) AS OutOfScopeOpportunities,
		COALESCE(A10.A10_10, 0) AS A1010,
		COALESCE(A10.A10_22, 0) AS A1022,
		COALESCE(A10.A10_25, 0) AS A1025,
		COALESCE(A10.A10_35, 0) AS A1035,
		COALESCE(A10.A10_45, 0) AS A1045,
		COALESCE(A10.A10_46, 0) AS A1046,
		COALESCE(A10.A10_70, 0) AS A1070,
		COALESCE(A10.A10_80, 0) AS A1080,
		COALESCE(A10.A10_81, 0) AS A1081,
		COALESCE(A10.A10_21, 0) AS A1021,
		COALESCE(A10.A10_82, 0) AS A1082,
		COALESCE(A10.A10_99, 0) AS A1099,
		COALESCE(A10.A10_NA, 0) AS A10NA,
		P.RoATPFFlag AS RoATP,
		p.IsTASOnly
	FROM Provider P
		LEFT OUTER JOIN (
							SELECT ProviderId, 
								MAX(COALESCE(ModifiedDateTimeUtc, CreatedDateTimeUtc)) AS LastUpdated, 
								Count(*) AS NumCourses, 
								Sum(CASE WHEN AddedByApplicationId = @BulkUploadAppId THEN 1 ELSE 0 END) AS NumBUCourses, 
								Sum(CASE WHEN AddedByApplicationId = @PortalAppId THEN 1 ELSE 0 END) AS NumPPCourses,
								Sum(CASE WHEN LearningAimRefId IS NOT NULL THEN 1 ELSE 0 END) As NumCoursesWithLearningAims,
								Sum(CASE WHEN LearningAimRefId IS NULL THEN 1 ELSE 0 END) As NumCoursesWithoutLearningAims
							FROM Course 
							WHERE RecordStatusId = @LiveStatus 
							GROUP BY ProviderId
						) C ON C.ProviderId = P.ProviderId
		LEFT OUTER JOIN (SELECT C.ProviderId, MAX(COALESCE(CI.ModifiedDateTimeUtc, CI.CreatedDateTimeUtc)) AS LastUpdated, Count(*) As NumberOfOpportunities FROM CourseInstance CI INNER JOIN Course C ON C.CourseId = CI.CourseId WHERE CI.RecordStatusId = @LiveStatus AND C.RecordStatusId = @LiveStatus GROUP BY C.ProviderId) CI ON CI.ProviderId = P.ProviderId
		LEFT OUTER JOIN (SELECT ProviderId, MAX(COALESCE(ModifiedDateTimeUtc, CreatedDateTimeUtc)) AS LastUpdated FROM Venue WHERE RecordStatusId = @LiveStatus GROUP BY ProviderId) V ON V.ProviderId = P.ProviderId
		LEFT OUTER JOIN QualityScore QS ON QS.ProviderId = P.ProviderId
		LEFT OUTER JOIN Application APP ON APP.ApplicationId = QS.ModifiedByApplicationId
		LEFT OUTER JOIN (
							SELECT UserProviderId AS ProviderId,
								Count(*) AS BulkUploads,
								Sum(CASE WHEN BulkUploadStatusId = 7 THEN 1 ELSE 0 END) AS BulkUploadSuccess
							FROM BulkUploadHistory 
							GROUP BY UserProviderId
						) BH ON BH.ProviderId = P.ProviderId
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
		LEFT OUTER JOIN (
							SELECT C.ProviderId,
								Sum(CASE WHEN CIFC.A10FundingCode = 10 THEN 1 ELSE 0 END) AS A10_10,
								Sum(CASE WHEN CIFC.A10FundingCode = 22 THEN 1 ELSE 0 END) AS A10_22,
								Sum(CASE WHEN CIFC.A10FundingCode = 25 THEN 1 ELSE 0 END) AS A10_25,
								Sum(CASE WHEN CIFC.A10FundingCode = 35 THEN 1 ELSE 0 END) AS A10_35,
								Sum(CASE WHEN CIFC.A10FundingCode = 45 THEN 1 ELSE 0 END) AS A10_45,
								Sum(CASE WHEN CIFC.A10FundingCode = 46 THEN 1 ELSE 0 END) AS A10_46,
								Sum(CASE WHEN CIFC.A10FundingCode = 70 THEN 1 ELSE 0 END) AS A10_70,
								Sum(CASE WHEN CIFC.A10FundingCode = 80 THEN 1 ELSE 0 END) AS A10_80,
								Sum(CASE WHEN CIFC.A10FundingCode = 81 THEN 1 ELSE 0 END) AS A10_81,
								Sum(CASE WHEN CIFC.A10FundingCode = 21 THEN 1 ELSE 0 END) AS A10_21,
								Sum(CASE WHEN CIFC.A10FundingCode = 82 THEN 1 ELSE 0 END) AS A10_82,
								Sum(CASE WHEN CIFC.A10FundingCode = 99 THEN 1 ELSE 0 END) AS A10_99,
								Sum(CASE WHEN CIFC.A10FundingCode IS NULL THEN 1 ELSE 0 END) AS A10_NA
							FROM Course C
								LEFT OUTER JOIN CourseInstance CI ON CI.CourseId = C.CourseId
								LEFT OUTER JOIN CourseInstanceA10FundingCode CIFC ON CIFC.CourseInstanceId = CI.CourseInstanceId
							GROUP BY C.ProviderId
						) A10 ON A10.ProviderId = P.ProviderId
		LEFT OUTER JOIN (
							SELECT C.ProviderId,
								Count(DISTINCT CI.CourseInstanceId) As InScopeOpportunities
							FROM Course C
								LEFT OUTER JOIN CourseInstance CI ON CI.CourseId = C.CourseId
								LEFT OUTER JOIN CourseInstanceA10FundingCode CIFC ON CIFC.CourseInstanceId = CI.CourseInstanceId
							WHERE CIFC.A10FundingCode IN (10, 22, 25, 25, 45, 46, 70, 80, 81)
							GROUP BY C.ProviderId
						) ISO ON ISO.ProviderId = P.ProviderId
		LEFT OUTER JOIN (
							SELECT C.ProviderId,
								Count(DISTINCT CI.CourseInstanceId) As OutOfScopeOpportunities
							FROM Course C
								LEFT OUTER JOIN CourseInstance CI ON CI.CourseId = C.CourseId
								LEFT OUTER JOIN CourseInstanceA10FundingCode CIFC ON CIFC.CourseInstanceId = CI.CourseInstanceId
							WHERE CIFC.A10FundingCode NOT IN (10, 22, 25, 35, 45, 46, 70, 80, 81) OR CIFC.A10FundingCode IS NULL						
							GROUP BY C.ProviderId
						) OSO ON OSO.ProviderId = P.ProviderId
	WHERE P.RecordStatusId = @LiveStatus
	AND ((@SFAProvider = 1 AND p.SFAFunded = 1) OR (@DFEProvider = 1 AND p.DFE1619Funded = 1))

	IF(@@ERROR <> 0)
	BEGIN
		RETURN -1;
	END;

	RETURN 0;

END;
GO


