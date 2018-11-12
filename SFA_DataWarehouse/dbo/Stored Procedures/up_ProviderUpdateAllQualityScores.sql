CREATE PROCEDURE [dbo].[up_ProviderUpdateAllQualityScores]
	@PeriodToRun	varchar(7)
AS

BEGIN
/*
*	Name:		[up_ProviderUpdateAllQualityScores]
*	System: 	Stored procedure interface module
*	Description:	Update all provider quality scores
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

	SET NOCOUNT ON;

	DELETE FROM Snapshot_QualityScore WHERE [Period] = @PeriodToRun;

	DECLARE
		@ProviderId int
		, @ProviderName nvarchar(256) 
		, @ProviderCursor cursor;
 
	SET @ProviderCursor = CURSOR FOR
		SELECT ProviderId, ProviderName
		FROM Snapshot_Provider
		WHERE [Period] = @PeriodToRun
		ORDER BY ProviderName;
 
	OPEN @ProviderCursor;
	FETCH NEXT FROM @ProviderCursor INTO @ProviderId, @ProviderName;
 
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- PRINT 'Updating: ' + @ProviderName
		EXEC up_ProviderUpdateQualityScore @PeriodToRun = @PeriodToRun, @ProviderId = @ProviderId;
		FETCH NEXT FROM @ProviderCursor INTO @ProviderId, @ProviderName;
	END;
 
	CLOSE @ProviderCursor;
	DEALLOCATE @ProviderCursor;

END;
