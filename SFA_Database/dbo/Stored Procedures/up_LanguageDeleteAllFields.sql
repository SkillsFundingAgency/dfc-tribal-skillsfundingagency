CREATE PROCEDURE [dbo].[up_LanguageDeleteAllFields]
		@DeleteTableLanguageFieldNames BIT = 0,
		@DeleteNormalLanguageFieldNames BIT = 0
AS

/*
*	Name:		[up_LanguageDeleteAllFields]
*	System: 	Stored procedure interface module
*	Description:	For development, clears all the languages fields, default values will then be added back as they are seen
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/



DELETE LanguageText FROM LanguageText LT
INNER JOIN LanguageField LF ON LT.LanguageFieldId = LF.LanguageFieldId
INNER JOIN LanguageKeyChild LKC ON LF.LanguageKeyChildId = LKC.LanguageKeyChildId
INNER JOIN LanguageKeyGroup LKG ON LKC.LanguageKeyGroupId = LKG.LanguageKeyGroupId
WHERE (LKG.KeyGroupName = 'Table' AND @DeleteTableLanguageFieldNames = 1)
OR (LKG.KeyGroupName <> 'Table' AND @DeleteNormalLanguageFieldNames = 1)



DELETE LanguageField FROM LanguageField LF
INNER JOIN LanguageKeyChild LKC ON LF.LanguageKeyChildId = LKC.LanguageKeyChildId
INNER JOIN LanguageKeyGroup LKG ON LKC.LanguageKeyGroupId = LKG.LanguageKeyGroupId
WHERE (LKG.KeyGroupName = 'Table' AND @DeleteTableLanguageFieldNames = 1)
OR (LKG.KeyGroupName <> 'Table' AND @DeleteNormalLanguageFieldNames = 1)


DELETE LanguageKeyChild FROM LanguageKeyChild LKC
INNER JOIN LanguageKeyGroup LKG ON LKC.LanguageKeyGroupId = LKG.LanguageKeyGroupId
WHERE (LKG.KeyGroupName = 'Table' AND @DeleteTableLanguageFieldNames = 1)
OR (LKG.KeyGroupName <> 'Table' AND @DeleteNormalLanguageFieldNames = 1)

DELETE FROM LanguageKeyGroup 
WHERE (KeyGroupName = 'Table' AND @DeleteTableLanguageFieldNames = 1)
OR (KeyGroupName <> 'Table' AND @DeleteNormalLanguageFieldNames = 1)

	
IF(@@ERROR <> 0)
BEGIN
	RETURN 1
END
	
RETURN 0