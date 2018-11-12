CREATE PROCEDURE [dbo].[up_LanguageFieldDelete]

	@LanguageFieldId INT
	 
AS
/*
*	Name:		[up_LanguageFieldDelete]
*	System: 	Stored procedure interface module
*	Description:	Deletes a language field
*
*	Return Values:	0 = No problem detected
*			1 = General database error.

*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:$
*/

DELETE FROM LanguageText WHERE LanguageFieldId = @LanguageFieldId
DELETE FROM LanguageField WHERe LanguageFieldId = @LanguageFieldId

IF (@@ERROR <> 0)
	RETURN 1
ELSE 
	RETURN 0