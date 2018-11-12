CREATE PROCEDURE [Search].[CourseInstanceList]
AS

BEGIN

	DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM dbo.RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);
	DECLARE @OneYearAgo DATE = CAST(DATEADD(YEAR, -1, GETUTCDATE()) AS DATE);

	SELECT CI.CourseInstanceId AS OPPORTUNITY_ID,
		CI.ProviderOwnCourseInstanceRef AS PROVIDER_OPPORTUNITY_ID,
		CI.Price AS PRICE,
		CI.PriceAsText AS PRICE_DESCRIPTION,
		CI.DurationUnit AS DURATION_VALUE,
		DU.BulkUploadRef AS DURATION_UNITS,
		DU.DurationUnitName,
		CI.DurationAsText AS DURATION_DESCRIPTION,
		CI.StartDateDescription AS START_DATE_DESCRIPTION,
		CI.EndDate AS END_DATE,
		COALESCE(SM.BulkUploadRef, '') AS STUDY_MODE,
		SM.StudyModeName,
		COALESCE(AT.BulkUploadRef, '') AS ATTENDANCE_MODE,
		AT.AttendanceTypeName,
		COALESCE(AP.BulkUploadRef, '') AS ATTENDANCE_PATTERN,
		AP.AttendancePatternName,
		CI.LanguageOfInstruction AS LANGUAGE_OF_INSTRUCTION,
		CI.LanguageOfAssessment AS LANGUAGE_OF_ASSESSMENT,
		CI.PlacesAvailable AS PLACES_AVAILABLE,
		CI.EnquiryTo AS ENQUIRE_TO,
		CI.ApplyTo AS APPLY_TO,
		CI.ApplyFromDate AS APPLY_FROM,
		CI.ApplyUntilDate AS APPLY_UNTIL,
		CI.ApplyUntilText AS APPLY_UNTI_DESC,
		dbo.AppendTrackingUrl(CI.Url, p.CourseTrackingUrl) AS URL,
		CI.TimeTable AS TIMETABLE,
		CI.CourseId AS COURSE_ID,
		V.VenueId AS VENUE_ID,
		CI.CanApplyAllYear ,
		IsNull(VL.LocationName,'') AS REGION_NAME, 
		CI.CreatedDateTimeUtc AS DATE_CREATED,
		CI.ModifiedDateTimeUtc AS DATE_UPDATE,
		@LiveRecordStatusId AS STATUS,
		IsNULL('NDLPP_' + CAST(ANUCreatedBy.LegacyUserId AS VARCHAR(10)),'Import_User') AS CREATED_BY,
		'NDLPP_' + CAST(ANUModifiedBy.LegacyUserId AS VARCHAR(10)) AS UPDATED_BY,  -- As we only have NDLPP data the prefix is hard coded, see note for DATA_SOURCE
		dbo.GetCourseInstanceSummaryText
		(
			CI.DurationUnit, 
			DU.DurationUnitName, 
			CI.DurationAsText, 
			SM.StudyModeName, 
			CI.Price, 
			CI.PriceAsText, 
			CISD.MinStartDate,
			CI.StartDateDescription, 
			V.VenueName, 
			VL.LocationName, 
			VL.Region
		) AS  OPPORTUNITY_SUMMARY,
		P.ProviderRegionId AS REGION_ID,
		C.AddedByApplicationId AS SYS_DATA_SOURCE, 
		OfferedByOrg.OrganisationId AS OFFERED_BY_Organisation,
		CI.OfferedByProviderId,
		CISD.MinStartDate AS [Earliest_start_date],
		COALESCE(A10.A10_Codes, 'NA') AS [A10_CODES],
		CASE WHEN P.DFE1619Funded = 1 AND P.SFAFunded = 0 THEN 1
			 WHEN DfE.CourseInstanceId IS NOT NULL THEN 1
			 ELSE 0 END AS DfEFunded,
		VL.VenueLocationId
	FROM dbo.Course C 
		INNER JOIN dbo.CourseInstance CI ON C.CourseId = CI.CourseId
		INNER JOIN dbo.Provider P ON C.ProviderId = P.ProviderId
		LEFT OUTER JOIN (SELECT CourseInstanceId, CASE WHEN Max(StartDate) > @OneYearAgo THEN Min(CASE WHEN StartDate >= @OneYearAgo THEN StartDate ELSE CAST('31 DEC 2500' AS DATE) END) ELSE CAST('01 JAN 2000' AS DATE) END As MinStartDate FROM dbo.CourseInstanceStartDate GROUP BY CourseInstanceId) CISD ON CI.CourseInstanceId = CISD.CourseInstanceId  
		LEFT OUTER JOIN dbo.DurationUnit DU ON CI.DurationUnitId = DU.DurationUnitId
		LEFT OUTER JOIN dbo.StudyMode SM ON CI.StudyModeId = SM.StudyModeId
		LEFT OUTER JOIN dbo.AttendanceType AT ON CI.AttendanceTypeId = AT.AttendanceTypeId
		LEFT OUTER JOIN dbo.AttendancePattern AP ON CI.AttendancePatternId = AP.AttendancePatternId
		LEFT OUTER JOIN dbo.AspNetUsers ANUModifiedBy ON CI.ModifiedByUserId = ANUModifiedBy.Id
		LEFT OUTER JOIN dbo.AspNetUsers ANUCreatedBy ON CI.CreatedByUserId = ANUCreatedBy.Id
		LEFT OUTER JOIN dbo.VenueLocation VL ON CI.VenueLocationId = VL.VenueLocationId
		LEFT OUTER JOIN dbo.CourseInstanceVenue CIV ON CI.CourseInstanceId = CIV.CourseInstanceId
		LEFT OUTER JOIN dbo.Venue V ON CIV.VenueId = V.VenueId
		LEFT OUTER JOIN dbo.Organisation OfferedByOrg ON OfferedByOrg.OrganisationId = CI.OfferedByOrganisationId
		LEFT OUTER JOIN (
							SELECT CourseInstanceId,
								STUFF((SELECT ', ' + CAST(A10FundingCode AS VARCHAR(10)) [text()]
										FROM dbo.[CourseInstanceA10FundingCode]
										WHERE CourseInstanceId = CourseInstance.CourseInstanceId 
										FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS A10_Codes
							FROM dbo.CourseInstance
							WHERE CourseInstanceId IN (SELECT DISTINCT CourseInstanceId FROM dbo.CourseInstanceA10FundingCode)
						) A10 ON A10.CourseInstanceId = CI.CourseInstanceId
		LEFT OUTER JOIN (SELECT DISTINCT CourseInstanceId FROM dbo.CourseInstanceA10FundingCode WHERE A10FundingCode = '25') DfE ON DfE.CourseInstanceId = CI.CourseInstanceId
	WHERE CI.RecordStatusId = @LiveRecordStatusId
		AND C.RecordStatusId = @LiveRecordStatusId 
		AND P.RecordStatusId = @LiveRecordStatusId 
		AND P.PublishData = 1
		AND C.RecordStatusId = @LiveRecordStatusId
		AND (
				CI.CourseInstanceId NOT IN (SELECT DISTINCT CourseInstanceId FROM CourseInstanceStartDate)
				OR
				CISD.MinStartDate >= @OneYearAgo
			);

	IF @@ERROR <> 0
	BEGIN
		RETURN 1;
	END;

	RETURN 0;

END;