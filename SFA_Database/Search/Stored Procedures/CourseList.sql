CREATE PROCEDURE [Search].[CourseList]
AS

DECLARE
	@LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0)
	, @DeletedRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 0 AND RS.IsArchived = 0 AND RS.IsDeleted = 1)

SELECT 
	C.CourseId AS COURSE_ID,
	C.ProviderId AS PROVIDER_ID,
	C.LearningAimRefId AS LAD_ID,
	C.CourseTitle AS PROVIDER_COURSE_TITLE,
	C.CourseSummary AS COURSE_SUMMARY,
	C.ProviderOwnCourseRef AS PROVIDER_COURSE_ID,
	dbo.AppendTrackingUrl(C.Url, P.CourseTrackingUrl)  AS COURSE_URL,
	dbo.AppendTrackingUrl(C.BookingUrl, P.BookingTrackingUrl) AS BOOKING_URL,
	C.EntryRequirements AS ENTRY_REQUIREMENTS,
	C.AssessmentMethod AS ASSESSMENT_METHOD,
	C.EquipmentRequired AS EQUIPMENT_REQUIRED,
	ISNULL(LA.LearningAimTitle, C.WhenNoLarQualificationTitle) AS QUALIFICATION_TITLE,	
	QL.BulkUploadRef AS QUALIFICATION_LEVEL,
	QL.QualificationLevelName,
	CASE WHEN LA.LearningAimRefId IS NOT NULL THEN LA.LearnDirectClassSystemCode1 ELSE LDCS1.LearnDirectClassificationRef END AS LDCS1,
	CASE WHEN LA.LearningAimRefId IS NOT NULL THEN LA.LearnDirectClassSystemCode2 ELSE LDCS2.LearnDirectClassificationRef END AS LDCS2,
	CASE WHEN LA.LearningAimRefId IS NOT NULL THEN LA.LearnDirectClassSystemCode3 ELSE LDCS3.LearnDirectClassificationRef END AS LDCS3,
	CASE WHEN LA.LearningAimRefId IS NOT NULL THEN NULL ELSE LDCS4.LearnDirectClassificationRef END AS LDCS4,
	CASE WHEN LA.LearningAimRefId IS NOT NULL THEN NULL ELSE LDCS5.LearnDirectClassificationRef END AS LDCS5,
	C.AddedByApplicationId , 
	C.UcasTariffPoints AS UCAS_TARIFF,
	CASE WHEN C.LearningAimRefId IS NULL THEN NULL ELSE 'Ofqual' END AS QUAL_REF_AUTHORITY, -- Appears to just default to Ofqual when this has a learning aim ref
	C.LearningAimRefId AS QUAL_REFERENCE,
	CASE WHEN C.LearningAimRefId IS NULL THEN 3 ELSE 1 END AS COURSE_TYPE_ID, -- TODO Check course type is correct, we have a few 2's and higher numbers in the example CSV output, update Graeme has suggested these other numbers are UCAS related, so only outputting 1 and 3 here currently
	dbo.GetCsvDateTimeString(C.CreatedDateTimeUtc) AS DATE_CREATED,
	dbo.GetCsvDateTimeString(C.ModifiedDateTimeUtc) AS DATE_UPDATED,
	CASE WHEN C.RecordStatusId = @LiveRecordStatusId THEN @LiveRecordStatusId ELSE @DeletedRecordStatusId END AS STATUS, -- We only export live records to CSV so this not strictly necessary but here in case WHERE changes in future
	CASE WHEN C.LearningAimRefId IS NULL THEN C.AwardingOrganisationName ELSE LAAO.AwardOrgName END AS AWARDING_ORG_NAME,
	IsNULL(ANUModifiedBy.CreatedByUserId, 'Import_user') Created_By,
	ANUModifiedBy.LegacyUserId AS UPDATED_BY,
	CASE WHEN C.LearningAimRefId IS NULL THEN QT.BulkUploadRef ELSE QTLAR.BulkUploadRef END AS QUALIFICATION_TYPE_CODE,
	CASE WHEN C.LearningAimRefId IS NULL THEN QT.QualificationTypeName ELSE QTLAR.QualificationTypeName END AS QUALIFICATION_TYPE_NAME,
	CASE WHEN C.AddedByApplicationId = 3 THEN 'UCAS'
		 WHEN C.LearningAimRefId IS NOT NULL AND LA.QualificationTypeId IS NOT NULL THEN 'LADType1'
		 WHEN C.LearningAimRefId IS NOT NULL AND LA.QualificationTypeId IS NULL THEN 'LADType2'
		 ELSE 'NoLADType3' END AS DATA_TYPE,
	C.AddedByApplicationId,
	LA.ErAppStatus,
	LA.SkillsForLife,
	P.Loans24Plus,
	CASE 
		WHEN LAV_Other.LearningAimRefId IS NULL THEN NULL
		WHEN LAV_Other.EndDate < CAST(GETDATE() AS DATE) THEN 'Invalid'
		WHEN LAV_Other.LastNewStartDate < CAST(GETDATE() AS DATE) THEN 'NotNewStarts'
		ELSE 'Valid' 
	END AS OTHER_FUNDING,
		CASE 
		WHEN LAV_Adult.LearningAimRefId IS NULL THEN NULL
		WHEN LAV_Adult.EndDate < CAST(GETDATE() AS DATE) THEN 'Invalid'
		WHEN LAV_Adult.LastNewStartDate < CAST(GETDATE() AS DATE) THEN 'NotNewStarts'
		ELSE 'Valid' END AS ADULT_LEARNER,
		LA.IndependentLivingSkills 
FROM 
	dbo.Course C
		INNER JOIN dbo.Provider P on P.ProviderId=c.ProviderId
		LEFT OUTER JOIN dbo.LearningAim LA ON C.LearningAimRefId = LA.LearningAimRefId
		LEFT OUTER JOIN dbo.LearningAimValidity LAV_Other ON C.LearningAimRefId = LAV_Other.LearningAimRefId AND LAV_Other.ValidityCategory = 'ER_OTHER'
		LEFT OUTER JOIN dbo.LearningAimValidity LAV_Adult ON C.LearningAimRefId = LAV_Adult.LearningAimRefId AND LAV_Adult.ValidityCategory = 'ADULT_LR'
		LEFT OUTER JOIN dbo.LearningAimAwardOrg LAAO ON LA.LearningAimAwardOrgCode = LAAO.LearningAimAwardOrgCode
		LEFT OUTER JOIN dbo.QualificationLevel QL ON C.QualificationLevelId =QL.QualificationLevelId
		LEFT OUTER JOIN dbo.AspNetUsers ANUModifiedBy ON C.ModifiedByUserId = ANUModifiedBy.Id
		LEFT OUTER JOIN dbo.AspNetUsers ANUCreatedBy ON C.CreatedByUserId = ANUCreatedBy.Id
		LEFT OUTER JOIN dbo.QualificationType QT ON C.WhenNoLarQualificationTypeId = QT.QualificationTypeId
		LEFT OUTER JOIN dbo.QualificationType QTLAR ON LA.QualificationTypeId = QTLAR.QualificationTypeId	
		LEFT OUTER JOIN (SELECT CourseId, LearnDirectClassificationRef FROM dbo.CourseLearnDirectClassification WHERE ClassificationOrder = 1) LDCS1 ON LDCS1.CourseId = C.CourseId
		LEFT OUTER JOIN (SELECT CourseId, LearnDirectClassificationRef FROM dbo.CourseLearnDirectClassification WHERE ClassificationOrder = 2) LDCS2 ON LDCS2.CourseId = C.CourseId
		LEFT OUTER JOIN (SELECT CourseId, LearnDirectClassificationRef FROM dbo.CourseLearnDirectClassification WHERE ClassificationOrder = 3) LDCS3 ON LDCS3.CourseId = C.CourseId
		LEFT OUTER JOIN (SELECT CourseId, LearnDirectClassificationRef FROM dbo.CourseLearnDirectClassification WHERE ClassificationOrder = 4) LDCS4 ON LDCS4.CourseId = C.CourseId
		LEFT OUTER JOIN (SELECT CourseId, LearnDirectClassificationRef FROM dbo.CourseLearnDirectClassification WHERE ClassificationOrder = 5) LDCS5 ON LDCS5.CourseId = C.CourseId
	WHERE 
		C.RecordStatusId = @LiveRecordStatusId
	AND
		P.RecordStatusId = @LiveRecordStatusId
	AND
		P.PublishData = 1
	ORDER BY 
		C.CourseId

IF @@ERROR <> 0
BEGIN
	RETURN 1
END

RETURN 0
