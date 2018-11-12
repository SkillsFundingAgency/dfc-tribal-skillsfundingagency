CREATE PROCEDURE [dbo].[up_LanguageKeyGroupList]
	 
AS
/*
*	Name:		[up_LanguageKeyGroupList]
*	System: 	Stored procedure interface module
*	Description:	List all language top level roots
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

-- Gets the top level roots for languagefields
SELECT LKG.KeyGroupName, LKG.LanguageKeyGroupId FROM LanguageKeyGroup LKG
ORDER BY LKG.KeyGroupName

IF @@ERROR <> 0
BEGIN
	RETURN 1
END

RETURN 0