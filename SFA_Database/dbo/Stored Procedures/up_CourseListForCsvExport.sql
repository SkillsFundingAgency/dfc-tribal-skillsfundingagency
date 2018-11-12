CREATE PROCEDURE [dbo].[up_CourseListForCsvExport]
AS
/*
*	Name:		[up_CourseListForCsvExport]
*	System: 	Stored procedure interface module
*	Description:	List all courses that are live in a format expected for the Csv Export
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Education Ltd, 2014
*			All rights reserved.
*
*	$Log:  $
*/

-- This procedure creates the O_COURSES.csv file
BEGIN

	DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);

	SELECT C.CourseId AS COURSE_ID,
		C.ProviderId AS PROVIDER_ID,
		C.LearningAimRefId AS LAD_ID,
		C.CourseTitle AS PROVIDER_COURSE_TITLE,
		C.CourseSummary AS COURSE_SUMMARY,
		C.ProviderOwnCourseRef AS PROVIDER_COURSE_ID,
		C.Url AS COURSE_URL,
		C.BookingUrl AS BOOKING_URL,
		C.EntryRequirements AS ENTRY_REQUIREMENTS,
		C.AssessmentMethod AS ASSESSMENT_METHOD,
		C.EquipmentRequired AS EQUIPMENT_REQUIRED,
		QT.QualificationTypeName AS QUALIFICATION_TYPE, -- Appears to never be populated
		ISNULL(LA.LearningAimTitle, C.WhenNoLarQualificationTitle) AS QUALIFICATION_TITLE,	
		QL.BulkUploadRef AS QUALIFICATION_LEVEL,
		CASE WHEN LA.LearningAimRefId IS NOT NULL THEN LA.LearnDirectClassSystemCode1 ELSE LDCS1.LearnDirectClassificationRef END AS LDCS1,
		CASE WHEN LA.LearningAimRefId IS NOT NULL THEN LA.LearnDirectClassSystemCode2 ELSE LDCS2.LearnDirectClassificationRef END AS LDCS2,
		CASE WHEN LA.LearningAimRefId IS NOT NULL THEN LA.LearnDirectClassSystemCode3 ELSE LDCS3.LearnDirectClassificationRef END AS LDCS3,
		CASE WHEN LA.LearningAimRefId IS NOT NULL THEN NULL ELSE LDCS4.LearnDirectClassificationRef END AS LDCS4,
		CASE WHEN LA.LearningAimRefId IS NOT NULL THEN NULL ELSE LDCS5.LearnDirectClassificationRef END AS LDCS5,
		CASE C.AddedByApplicationId
			WHEN 1 THEN 'NDLPP'
			WHEN 2 THEN 'BU'
			WHEN 3 THEN 'UCAS'
			ELSE 'UNKNOWN' END AS SYS_DATA_SOURCE, 
		C.UcasTariffPoints AS UCAS_TARIFF,
		CASE WHEN C.LearningAimRefId IS NULL THEN NULL ELSE 'Ofqual' END AS QUAL_REF_AUTHORITY, -- Appears to just default to Ofqual when this has a learning aim ref
		C.LearningAimRefId AS QUAL_REFERENCE,
		CASE WHEN C.LearningAimRefId IS NULL THEN 3 ELSE 1 END AS COURSE_TYPE_ID, -- TODO Check course type is correct, we have a few 2's and higher numbers in the example CSV output, update Graeme has suggested these other numbers are UCAS related, so only outputting 1 and 3 here currently
		dbo.GetCsvDateTimeString(C.CreatedDateTimeUtc) AS DATE_CREATED,
		dbo.GetCsvDateTimeString(C.ModifiedDateTimeUtc) AS DATE_UPDATED,
		CASE WHEN C.RecordStatusId = @LiveRecordStatusId THEN 'LIVE' ELSE 'DELETED' END AS STATUS, -- We only export live records to CSV so this not strictly necessary but here in case WHERE changes in future
		CASE WHEN C.LearningAimRefId IS NULL THEN C.AwardingOrganisationName ELSE LAAO.AwardOrgName END AS AWARDING_ORG_NAME,
		ANUModifiedBy.LegacyUserId AS UPDATED_BY,
		ANUModifiedBy.CreatedByUserId Created_By,
		CASE WHEN C.LearningAimRefId IS NULL THEN QT.BulkUploadRef ELSE QTLAR.BulkUploadRef END AS QUALIFICATION_TYPE_CODE,
		CASE WHEN C.LearningAimRefId IS NULL THEN 'No LAD - Type3' ELSE 'LAD - Type1' END AS DATA_TYPE,
		CASE C.AddedByApplicationId
			WHEN 1 THEN 'NDLPP'
			WHEN 2 THEN 'NDLPP'  -- Bulk upload here appears to be considered NDLPP data, i.e. it hasn't come from Ucas, this differs to the SYS_DATA_SOURCE output which shows BU when bulk upload
			WHEN 3 THEN 'UCAS'
			ELSE 'UNKNOWN' END AS SYS_DATA,
		CASE WHEN C.ModifiedDateTimeUtc IS NOT NULL AND C.ModifiedDateTimeUtc <> C.CreatedDateTimeUtc THEN dbo.GetCsvDateTimeString(C.ModifiedDateTimeUtc) ELSE NULL END AS DATE_UPDATED_COPY_OVER,   -- Note we don't have a copy over dates, as we don't copy a denormalised version to another database, 
		CASE WHEN C.ModifiedDateTimeUtc IS NULL OR C.ModifiedDateTimeUtc = C.CreatedDateTimeUtc THEN dbo.GetCsvDateTimeString(C.CreatedDateTimeUtc) ELSE NULL END AS DATE_CREATED_COPY_OVER, -- so these dates default to the modified and created dates, i.e. changes are visible immediately
		CASE WHEN P.DFE1619Funded = 1 AND P.SFAFunded = 0 THEN 'Y'
			 WHEN DfE.CourseId IS NOT NULL THEN 'Y'
			 ELSE 'N' END AS DFE_FUNDED
	FROM Course C
		INNER JOIN Provider P on P.ProviderId=c.ProviderId
		LEFT OUTER JOIN LearningAim LA ON C.LearningAimRefId = LA.LearningAimRefId
		LEFT OUTER JOIN LearningAimAwardOrg LAAO ON LA.LearningAimAwardOrgCode = LAAO.LearningAimAwardOrgCode
		LEFT OUTER JOIN QualificationLevel QL ON C.QualificationLevelId =QL.QualificationLevelId
		LEFT OUTER JOIN AspNetUsers ANUModifiedBy ON C.ModifiedByUserId = ANUModifiedBy.Id
		LEFT OUTER JOIN AspNetUsers ANUCreatedBy ON C.CreatedByUserId = ANUCreatedBy.Id
		LEFT OUTER JOIN QualificationType QT ON C.WhenNoLarQualificationTypeId = QT.QualificationTypeId
		LEFT OUTER JOIN QualificationType QTLAR ON LA.QualificationTypeId = QTLAR.QualificationTypeId	
		LEFT OUTER JOIN (SELECT CourseId, LearnDirectClassificationRef FROM CourseLearnDirectClassification WHERE ClassificationOrder = 1) LDCS1 ON LDCS1.CourseId = C.CourseId
		LEFT OUTER JOIN (SELECT CourseId, LearnDirectClassificationRef FROM CourseLearnDirectClassification WHERE ClassificationOrder = 2) LDCS2 ON LDCS2.CourseId = C.CourseId
		LEFT OUTER JOIN (SELECT CourseId, LearnDirectClassificationRef FROM CourseLearnDirectClassification WHERE ClassificationOrder = 3) LDCS3 ON LDCS3.CourseId = C.CourseId
		LEFT OUTER JOIN (SELECT CourseId, LearnDirectClassificationRef FROM CourseLearnDirectClassification WHERE ClassificationOrder = 4) LDCS4 ON LDCS4.CourseId = C.CourseId
		LEFT OUTER JOIN (SELECT CourseId, LearnDirectClassificationRef FROM CourseLearnDirectClassification WHERE ClassificationOrder = 5) LDCS5 ON LDCS5.CourseId = C.CourseId
		LEFT OUTER JOIN (SELECT DISTINCT CourseId FROM CourseInstance CI INNER JOIN CourseInstanceA10FundingCode CIFC ON CIFC.CourseInstanceId = CI.CourseInstanceId WHERE CIFC.A10FundingCode = '25') DfE ON DfE.CourseId = C.CourseId
	WHERE C.RecordStatusId = @LiveRecordStatusId  
		AND P.RecordStatusId = @LiveRecordStatusId
		AND P.PublishData = 1
	ORDER BY C.CourseId;

	IF @@ERROR <> 0
	BEGIN
		RETURN 1;
	END;

	RETURN 0;

END;
