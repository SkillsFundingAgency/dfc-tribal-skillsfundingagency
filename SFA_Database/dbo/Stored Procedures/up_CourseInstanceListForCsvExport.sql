CREATE PROCEDURE [dbo].[up_CourseInstanceListForCsvExport]
	
AS
/*
*	Name:		[up_CourseInstanceListForCsvExport]
*	System: 	Stored procedure interface module
*	Description:	List all CourseInstances (aka Opportunities) that are live in a format expected for the Csv Export
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Education Ltd, 2014
*			All rights reserved.
*
*	$Log:  $
*/

-- This procedure creates the O_OPPORTUNITIES.csv file

BEGIN

	DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0)

	SELECT CI.CourseInstanceId AS OPPORTUNITY_ID,
		CI.ProviderOwnCourseInstanceRef AS PROVIDER_OPPORTUNITY_ID,
		CI.Price AS PRICE,
		CI.PriceAsText AS PRICE_DESCRIPTION,
		CI.DurationUnit AS DURATION_VALUE,
		DU.BulkUploadRef AS DURATION_UNITS,
		CI.DurationAsText AS DURATION_DESCRIPTION,
		CI.StartDateDescription AS START_DATE_DESCRIPTION,
		dbo.GetCsvDateString(CI.EndDate) AS END_DATE,
		SM.BulkUploadRef AS STUDY_MODE,
		AT.BulkUploadRef AS ATTENDANCE_MODE,
		AP.BulkUploadRef AS ATTENDANCE_PATTERN,
		CI.LanguageOfAssessment AS LANGUAGE_OF_INSTRUCTION,
		CI.LanguageOfAssessment AS LANGUAGE_OF_ASSESSMENT,
		CI.PlacesAvailable AS PLACES_AVAILABLE,
		CI.EnquiryTo AS ENQUIRE_TO,
		CI.ApplyTo AS APPLY_TO,
		dbo.GetCsvDateString(CASE WHEN CI.ApplyFromDate < CAST('01 Jan 1753' AS DATE) THEN Null ELSE CI.ApplyFromDate END) AS APPLY_FROM,
		dbo.GetCsvDateString(CASE WHEN CI.ApplyUntilDate < CAST('01 Jan 1753' AS DATE) THEN Null ELSE CI.ApplyUntilDate END) AS APPLY_UNTIL,
		CI.ApplyUntilText AS APPLY_UNTI_DESC,
		CI.Url AS URL,
		CI.TimeTable AS TIMETABLE,
		CI.CourseId AS COURSE_ID,
		V.VenueId AS VENUE_ID,
		CASE WHEN CI.CanApplyAllYear = 1 THEN 'Y' ELSE '' END AS APPLY_THROUGHOUT_YEAR,
		NULL AS EIS_FLAG, -- Appears unused, we don't have this in our schema anyway
		IsNull(VL.LocationName,'') AS REGION_NAME, 
		dbo.GetCsvDateTimeString(CI.CreatedDateTimeUtc) AS DATE_CREATED,
		dbo.GetCsvDateTimeString(CI.ModifiedDateTimeUtc) AS DATE_UPDATE,
		CASE WHEN CI.RecordStatusId = @LiveRecordStatusId THEN 'LIVE' ELSE 'DELETED' END AS STATUS, -- We only export live records to CSV so this not strictly necessary to have a CASE statement here, but just in case
		'NDLPP_' + CAST(ANUModifiedBy.LegacyUserId AS VARCHAR(10)) AS UPDATED_BY,  -- As we only have NDLPP data the prefix is hard coded, see note for DATA_SOURCE
		'NDLPP_' + CAST(ANUCreatedBy.LegacyUserId AS VARCHAR(10)) AS CREATED_BY,
		dbo.GetCourseInstanceSummaryText
		(
			CI.DurationUnit, 
			DU.DurationUnitName, 
			CI.DurationAsText, 
			SM.StudyModeName, 
			CI.Price, 
			CI.PriceAsText, 
			(SELECT TOP 1 CISD.StartDate FROM CourseInstanceStartDate CISD WHERE CISD.CourseInstanceId = CI.CourseInstanceId ORDER BY CISD.StartDate ASC),
			CI.StartDateDescription, V.VenueName, VL.LocationName, VL.Region
		) 
		AS  OPPORTUNITY_SUMMARY,
		P.ProviderRegionId AS REGION_ID,
		'NDLPP' AS SYS_DATA_SOURCE,  -- TODO Need to sort out how Ucas data is imported to be exported, currently all data is NDLPP, it may be the case UCAS happens after the switch to new APIs so this will not matter.
		CASE WHEN CI.ModifiedDateTimeUtc IS NOT NULL AND CI.ModifiedDateTimeUtc <> CI.CreatedDateTimeUtc THEN dbo.GetCsvDateTimeString(CI.ModifiedDateTimeUtc) ELSE NULL END AS DATE_UPDATED_COPY_OVER,   -- Note we don't have a copy over dates, as we don't copy a denormalised version to another database, 
		CASE WHEN CI.ModifiedDateTimeUtc IS NULL OR CI.ModifiedDateTimeUtc = CI.CreatedDateTimeUtc THEN dbo.GetCsvDateTimeString(CI.CreatedDateTimeUtc) ELSE NULL END AS DATE_CREATED_COPY_OVER, -- so these dates default to the modified and created dates, i.e. changes are visible immediately
		CASE WHEN CI.OfferedByOrganisationId IS NOT NULL THEN (SELECT O.OrganisationId FROM Organisation O WHERE O.OrganisationId = CI.OfferedByOrganisationId AND O.RecordStatusId = @LiveRecordStatusId)
			 WHEN CI.OfferedByProviderId = C.ProviderId THEN NULL 
			 ELSE CI.OfferedByProviderId END AS OFFERED_BY,
		CASE WHEN P.DFE1619Funded = 1 AND P.SFAFunded = 0 THEN 'Y'
			 WHEN DfE.CourseInstanceId IS NOT NULL THEN 'Y'
			 ELSE 'N' END AS DFE_FUNDED
	FROM Course C 
		INNER JOIN CourseInstance CI ON C.CourseId = CI.CourseId
		INNER JOIN Provider P ON C.ProviderId = P.ProviderId
		LEFT OUTER JOIN DurationUnit DU ON CI.DurationUnitId = DU.DurationUnitId
		LEFT OUTER JOIN StudyMode SM ON CI.StudyModeId = SM.StudyModeId
		LEFT OUTER JOIN AttendanceType AT ON CI.AttendanceTypeId = AT.AttendanceTypeId
		LEFT OUTER JOIN AttendancePattern AP ON CI.AttendancePatternId = AP.AttendancePatternId
		LEFT OUTER JOIN AspNetUsers ANUModifiedBy ON CI.ModifiedByUserId = ANUModifiedBy.Id
		LEFT OUTER JOIN AspNetUsers ANUCreatedBy ON CI.CreatedByUserId = ANUCreatedBy.Id
		LEFT OUTER JOIN VenueLocation VL ON CI.VenueLocationId = VL.VenueLocationId
		LEFT OUTER JOIN CourseInstanceVenue CIV ON CI.CourseInstanceId = CIV.CourseInstanceId
		LEFT OUTER JOIN Venue V ON CIV.VenueId = V.VenueId
		LEFT OUTER JOIN (SELECT DISTINCT CourseInstanceId FROM CourseInstanceA10FundingCode WHERE A10FundingCode = '25') DfE ON DfE.CourseInstanceId = CI.CourseInstanceId
	WHERE CI.RecordStatusId = @LiveRecordStatusId
		AND C.RecordStatusId = @LiveRecordStatusId 
		AND P.RecordStatusId = @LiveRecordStatusId
		AND P.PublishData = 1
		AND C.RecordStatusId = @LiveRecordStatusId
	ORDER BY CI.CourseInstanceId;

	IF @@ERROR <> 0
	BEGIN
		RETURN 1;
	END;

	RETURN 0;

END;