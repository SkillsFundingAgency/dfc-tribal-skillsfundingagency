CREATE PROCEDURE [dbo].[up_GetProviderCoursesWithExpiredLAR]
	@ProviderId int
AS

/*
*	Name:		[up_GetCountProviderCoursesWithExpiredLAR]
*	System: 	Stored procedure interface module
*	Description:	Get a count of course which have an expired LAR (learning aim)
*
*	Return Values:	0 = No problem detected
*			-1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

BEGIN

	IF (@ProviderId IS NULL) 
	BEGIN
		RETURN 0;
	END;

	DECLARE @LiveStatusId int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);

	SELECT C.CourseId 
	FROM Course C
		INNER JOIN LearningAim LA ON LA.LearningAimRefId = C.LearningAimRefId
	WHERE C.ProviderId = @ProviderId
		AND C.RecordStatusId = @LiveStatusId
		AND LA.RecordStatusId <> @LiveStatusId;

	IF (@@ERROR <> 0)
	BEGIN
		RETURN -1;
	END;

	RETURN 0;

END;