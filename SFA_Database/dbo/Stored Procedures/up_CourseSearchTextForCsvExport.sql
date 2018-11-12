

CREATE PROCEDURE [dbo].[up_CourseSearchTextForCsvExport]
	
AS
/*
*	Name:		[up_CourseSearchTextForCsvExport]
*	System: 	Stored procedure interface module
*	Description:	Lists all live courses in the format for the W_SEARCH_TEXT.csv output
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Education Ltd, 2014
*			All rights reserved.
*
*	$Log:  $
*/

-- This procedure creates the W_SEARCH_TEXT.csv file

DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0)

SELECT 
	C.CourseId AS COURSE_ID,
	CI.CourseInstanceId AS OPPORTUNITY_ID,
	SM.BulkUploadRef AS STUDY_MODE,
	AT.BulkUploadRef AS ATTENDANCE_MODE,
	AP.BulkUploadRef AS ATTENDANCE_PATTERN,
	ISNULL(LAQT.BulkUploadRef, QT.BulkUploadRef) AS QUAL_TYPE_CODE,
	VL.EastingMin AS XMIN, 
	VL.EastingMax AS XMAX,
	VL.NorthingMin AS YMIN,
	VL.NorthingMax AS YMAX,
	ISNULL(VL.Easting, GL.Easting) AS X_COORD,
	ISNULL(VL.Northing, GL.Northing) AS Y_COORD,
	C.CourseTitle 
	+ '<C>' + dbo.GetLdcsCodeList(C.CourseId) + '</C>'		 
	+ '<S>' + ISNULL(SM.BulkUploadRef,'') + '</S>'
	+ '<M>' + ISNULL(AT.BulkUploadRef, '') + '</M>'
	+ '<P>' + ISNULL(AP.BulkUploadRef, '') + '</P>'
	+ '<Q>' + ISNULL(ISNULL(LAQT.BulkUploadRef, QT.BulkUploadRef), '') + '</Q>'
	+ '<PI>' + CAST(C.ProviderId AS VARCHAR(10)) + '</PI>'
	+ '<PN>' + P.ProviderName + '<PN>'
	+ '<ON>' + ISNULL(O.OrganisationName, '') + '</ON>'
	+ '<QL>' + ISNULL(QL.BulkUploadRef, '') + '</QL>'
	+ '<A>' + dbo.GetA10CommaDelimited(CI.CourseInstanceId) + '</A>'
	AS SEARCH_TEXT,
	(SELECT COUNT(1) FROM CourseInstance WHERE CourseId = C.CourseId AND RecordStatusId = @LiveRecordStatusId) AS NO_OF_OPPS,
	dbo.GetCsvDateTimeString(ISNULL(CI.ModifiedDateTimeUtc, CI.CreatedDateTimeUtc)) AS LAST_UPDATE_DATE,
	P.ProviderId AS PROVIDER_ID,
	dbo.GetLdcsCodeList(C.CourseId) AS SEARCH_LDCS,
	P.ProviderName AS PROVIDER_NAME,
	NULL AS IES_FLAG, -- TODO Not sure what this is, update, flag no longer used so outputting nothing should be okay
	CASE WHEN P.Loans24Plus = 1 THEN 'Y' ELSE 'N' END AS TTG_FLAG,
	NULL AS TQS_FLAG,  -- TODO Not sure what this is, update flag no longer used so outputting nothing should be okay
	NULL AS SFL_FLAG,  -- TODO Not sure what this is, update flag no longer used so outputting nothing should be okay
	--CI.ApplyUntilDate AS APP_DEADLINE,  -- Always seems to be null but assume it should be apply until date
	dbo.GetCsvDateTimeString(CI.ApplyUntilDate) AS APP_DEADLINE, -- changed this to match last_updated
	CASE 
		WHEN LAV_App.LearningAimRefId IS NULL THEN NULL
		WHEN LAV_App.EndDate < CAST(GETDATE() AS DATE) THEN 'Invalid'
		WHEN LAV_App.LastNewStartDate < CAST(GETDATE() AS DATE) THEN 'NotNewStarts'
		ELSE 'Valid' END AS APP_STATUS,
	CASE 
		WHEN LAV_Any.LearningAimRefId IS NULL THEN NULL
		WHEN LAV_Any.EndDate < CAST(GETDATE() AS DATE) THEN 'Invalid'
		WHEN LAV_Any.LastNewStartDate < CAST(GETDATE() AS DATE) THEN 'NotNewStarts'
		ELSE 'Valid' END AS TTG_STATUS,-- TODO Needs checking to see if this should come from the Any category	
	CASE 
		WHEN LAV_Adult.LearningAimRefId IS NULL THEN NULL
		WHEN LAV_Adult.EndDate < CAST(GETDATE() AS DATE) THEN 'Invalid'
		WHEN LAV_Adult.LastNewStartDate < CAST(GETDATE() AS DATE) THEN 'NotNewStarts'
		ELSE 'Valid' END AS ADULT_LEARNER,
	CASE 
		WHEN LAV_Other.LearningAimRefId IS NULL THEN NULL
		WHEN LAV_Other.EndDate < CAST(GETDATE() AS DATE) THEN 'Invalid'
		WHEN LAV_Other.LastNewStartDate < CAST(GETDATE() AS DATE) THEN 'NotNewStarts'
		ELSE 'Valid' END AS OTHER_FUNDING,	
	CASE WHEN LA.IndependentLivingSkills IS NULL THEN NULL
		 WHEN LA.IndependentLivingSkills = 1 THEN 'Y'
		 ELSE 'N' END AS ILS_FLAG,
	CASE WHEN CI.CanApplyAllYear = 1 THEN 'Y' ELSE 'N' END AS FLEXIBLE_START_FLAG,
	ISNULL(VL.Region, dbo.GetPostcodeOutcode(A.PostCode)) AS SEARCH_REGION,
	ISNULL(VL.Region, A.Town) AS SEARCH_TOWN,
	A.Postcode AS SEARCH_POSTCODE	
FROM Provider P
	INNER JOIN Course C ON C.ProviderId = P.ProviderId
	INNER JOIN CourseInstance CI ON C.CourseId = CI.CourseId
	LEFT OUTER JOIN OrganisationProvider OP ON OP.ProviderId = P.ProviderId
	LEFT OUTER JOIN Organisation O ON O.OrganisationId = OP.OrganisationId
	LEFT OUTER JOIN StudyMode SM ON CI.StudyModeId = SM.StudyModeId
	LEFT OUTER JOIN AttendanceType AT ON CI.AttendanceTypeId = AT.AttendanceTypeId
	LEFT OUTER JOIN AttendancePattern AP ON CI.AttendancePatternId = AP.AttendancePatternId
	LEFT OUTER JOIN LearningAim LA ON C.LearningAimRefId = LA.LearningAimRefId
	LEFT OUTER JOIN LearningAimValidity LAV_App ON C.LearningAimRefId = LAV_App.LearningAimRefId AND LAV_App.ValidityCategory = 'ER_APP'
	LEFT OUTER JOIN LearningAimValidity LAV_Adult ON C.LearningAimRefId = LAV_Adult.LearningAimRefId AND LAV_Adult.ValidityCategory = 'ADULT_LR'
	LEFT OUTER JOIN LearningAimValidity LAV_Other ON C.LearningAimRefId = LAV_Other.LearningAimRefId AND LAV_Other.ValidityCategory = 'ER_OTHER'
	LEFT OUTER JOIN LearningAimValidity LAV_Any ON C.LearningAimRefId = LAV_Any.LearningAimRefId AND LAV_Any.ValidityCategory = 'ANY'
	LEFT OUTER JOIN QualificationType LAQT ON LA.QualificationTypeId = LAQT.QualificationTypeId
	LEFT OUTER JOIN QualificationType QT ON C.WhenNoLarQualificationTypeId = QT.QualificationTypeId
	LEFT OUTER JOIN QualificationLevel QL ON C.QualificationLevelId = QL.QualificationLevelId
	LEFT OUTER JOIN CourseInstanceVenue CIV ON CI.CourseInstanceId = CIV.CourseInstanceId
	LEFT OUTER JOIN Venue V ON CIV.VenueId = V.VenueId
	LEFT OUTER JOIN [Address] A ON V.AddressId = A.AddressId
	LEFT OUTER JOIN GeoLocation GL ON GL.Postcode = A.Postcode
	LEFT OUTER JOIN VenueLocation VL ON CI.VenueLocationId = VL.VenueLocationId
	LEFT OUTER JOIN CourseLearnDirectClassification CLDC1 ON CLDC1.CourseId = C.CourseId AND CLDC1.ClassificationOrder = 1
	LEFT OUTER JOIN CourseLearnDirectClassification CLDC2 ON CLDC2.CourseId = C.CourseId AND CLDC2.ClassificationOrder = 2
	LEFT OUTER JOIN CourseLearnDirectClassification CLDC3 ON CLDC3.CourseId = C.CourseId AND CLDC3.ClassificationOrder = 3
	LEFT OUTER JOIN CourseLearnDirectClassification CLDC4 ON CLDC4.CourseId = C.CourseId AND CLDC4.ClassificationOrder = 4
	LEFT OUTER JOIN CourseLearnDirectClassification CLDC5 ON CLDC5.CourseId = C.CourseId AND CLDC5.ClassificationOrder = 5
WHERE
	C.RecordStatusId = @LiveRecordStatusId
	AND CI.RecordStatusId = @LiveRecordStatusId
	AND P.RecordStatusId = @LiveRecordStatusId 
	AND P.PublishData = 1
	AND A.Postcode NOT IN (SELECT PrisonPostcode FROM Prison) -- LC: Prison check only seems to take place for this CSV output
ORDER BY C.CourseId

IF @@ERROR <> 0
BEGIN
	RETURN 1
END

RETURN 0

GO