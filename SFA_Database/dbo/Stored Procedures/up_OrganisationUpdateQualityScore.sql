CREATE PROCEDURE [dbo].[up_OrganisationUpdateQualityScore]
		@OrganisationId int
AS


/*
*	Name:		[up_OrganisationUpdateQualityScore]
*	System: 	Stored procedure interface module
*	Description:	Update organisation quality score
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

BEGIN

	BEGIN TRANSACTION;

	DECLARE @Live int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);

	DELETE FROM OrganisationQualityScore WHERE OrganisationId = @OrganisationId;
	
	INSERT INTO OrganisationQualityScore
		(OrganisationId, EarliestModifiedDateTimeUtc, CalculatedDateTimeUtc)
	SELECT @OrganisationId,
		CASE WHEN Count(COALESCE(ModifiedDateTimeUtc, LastActivity)) = Count(*) THEN Min(CASE WHEN ModifiedDateTimeUtc > LastActivity THEN ModifiedDateTimeUtc ELSE LastActivity END) 
			 ELSE NULL END,
		GetUtcDate()
	FROM QualityScore
	WHERE ProviderId IN (
		SELECT op.ProviderId
		FROM OrganisationProvider op
			JOIN Provider p on p.ProviderId = op.ProviderId
		WHERE
			op.OrganisationId = @OrganisationId
			AND op.IsAccepted = 1
			AND op.IsRejected = 0
			AND p.RecordStatusId = @Live
	);

	COMMIT TRANSACTION;

END;
