CREATE PROCEDURE [dbo].[up_ProviderUpdateAllQualityScores]
	@Force bit = 0
AS

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

SET NOCOUNT ON

IF @Force = 1
BEGIN
	DELETE FROM QualityScore
END

DECLARE
	@ProviderId int
	, @ProviderName nvarchar(256) 
	, @ProviderCursor cursor
 
SET @ProviderCursor = CURSOR FOR
	SELECT ProviderId, ProviderName
	FROM Provider
	ORDER BY ProviderName
 
OPEN @ProviderCursor
FETCH NEXT FROM @ProviderCursor INTO @ProviderId, @ProviderName
 
WHILE @@FETCH_STATUS = 0
BEGIN
	-- PRINT 'Updating: ' + @ProviderName
	EXEC up_ProviderUpdateQualityScore @ProviderId
	FETCH NEXT FROM @ProviderCursor INTO @ProviderId, @ProviderName
END
 
CLOSE @ProviderCursor
DEALLOCATE @ProviderCursor
