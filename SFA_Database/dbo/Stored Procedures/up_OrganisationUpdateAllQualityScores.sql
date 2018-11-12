CREATE PROCEDURE [dbo].[up_OrganisationUpdateAllQualityScores]
AS

/*
*	Name:		[up_OrganisationUpdateAllQualityScores]
*	System: 	Stored procedure interface module
*	Description:	Update all organisation quality scores
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

SET NOCOUNT ON

DECLARE
	@OrganisationId int
	, @OrganisationName nvarchar(100) 
	, @OrganisationCursor cursor
 
SET @OrganisationCursor = CURSOR FOR
	SELECT OrganisationId, OrganisationName
	FROM Organisation
	ORDER BY OrganisationName
 
OPEN @OrganisationCursor
FETCH NEXT FROM @OrganisationCursor INTO @OrganisationId, @OrganisationName
 
WHILE @@FETCH_STATUS = 0
BEGIN
	--PRINT 'Updating: ' + @OrganisationName
	EXEC up_OrganisationUpdateQualityScore @OrganisationId
	FETCH NEXT FROM @OrganisationCursor INTO @OrganisationId, @OrganisationName
END
 
CLOSE @OrganisationCursor
DEALLOCATE @OrganisationCursor
