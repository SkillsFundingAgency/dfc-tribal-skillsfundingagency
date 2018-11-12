CREATE PROCEDURE [dbo].[up_LanguageKeyChildList]
	 @LanguageKeyGroupId INT
AS
/*
*	Name:		[up_LanguageKeyChildList]
*	System: 	Stored procedure interface module
*	Description:	List all language child roots
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

-- Gets the top level roots for languagefields
SELECT LKC.KeyChildName, LKC.LanguageKeyChildId, LKC.LanguageKeyGroupId
FROM LanguageKeyChild LKC
WHERE (LKC.LanguageKeyGroupId = @LanguageKeyGroupId OR @LanguageKeyGroupId IS NULL)
ORDER BY LKC.KeyChildName

IF @@ERROR <> 0
BEGIN
	RETURN 1
END

RETURN 0