CREATE PROCEDURE [dbo].[up_LanguageList]
	 @LanguageId INT,
	 @LookupLanguageId INT
AS
/*
*	Name:		[up_LanguageList]
*	System: 	Stored procedure interface module
*	Description:	List all languages
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2008
*			All rights reserved.
*
*	$Log:  $
*/

SELECT L.LanguageID, DefaultText, IETF, dbo.GetLanguageTextForQualifiedFieldName(L.LanguageFieldName, @LookupLanguageId, L.DefaultText)  + ' (' + IETF + ')' AS DisplayName,
dbo.GetLanguageTextForQualifiedFieldName(L.LanguageFieldName, @LookupLanguageId, L.DefaultText) AS LanguageText
FROM [Language] L
WHERE L.LanguageId = @LanguageId OR @LanguageId IS NULL
ORDER BY LanguageText

IF @@ERROR <> 0
BEGIN

	RETURN 1

END

RETURN 0