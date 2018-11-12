CREATE PROCEDURE [dbo].[up_ReportProviderCourses]
		@ProviderId int
AS

/*
*	Name:		[up_ReportProviderCourses]
*	System: 	Stored procedure interface module
*	Description:	List provider courses
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

BEGIN

	DECLARE @DeletedStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsDeleted = 1);

	SELECT c.CourseId,
		rs.RecordStatusName,
		c.ProviderOwnCourseRef,
		c.CourseTitle,
		COALESCE(qt.QualificationTypeName, la.Qualification) QualificationTypeName,
		COALESCE(la.LearningAimTitle, c.WhenNoLarQualificationTitle) QualificationTitle,
		COALESCE(ao.AwardOrgName, c.AwardingOrganisationName) AwardOrgName
	FROM Course c
		INNER JOIN RecordStatus rs ON rs.RecordStatusId = c.RecordStatusId 
		LEFT OUTER JOIN LearningAim la ON la.LearningAimRefId = c.LearningAimRefId
		LEFT OUTER JOIN QualificationType qt ON qt.QualificationTypeId = COALESCE(la.QualificationTypeId, c.WhenNoLarQualificationTypeId)
		LEFT OUTER JOIN LearningAimAwardOrg ao ON ao.LearningAimAwardOrgCode = COALESCE(la.LearningAimAwardOrgCode, c.AwardingOrganisationName)
	WHERE c.ProviderId = @ProviderId
		AND c.RecordStatusId != @DeletedStatus;

	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;
	
	RETURN 0;

END;