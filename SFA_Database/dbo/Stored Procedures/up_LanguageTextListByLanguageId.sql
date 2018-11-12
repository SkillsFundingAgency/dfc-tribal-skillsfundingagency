CREATE PROCEDURE [dbo].[up_LanguageTextListByLanguageId]
     @LanguageId INT,
     @LanguageFieldId INT     
AS
/*
*    Name:        [up_LanguageTextListByLanguageId]
*    System:     Stored procedure interface module
*    Description:    List all languages fields
*
*    Return Values:    0 = No problem detected
*            1 = General database error.
*    Copyright:    (c) Tribal Data Solutions Ltd, 2012
*            All rights reserved.
*
*    $Log:  $
*/

SELECT @LanguageId AS LanguageId,  LOWER(LKG.KeyGroupName + '_' + LKC.KeyChildName + '_' + LF.LanguageFieldName) AS FieldNameLowered, 
LKG.KeyGroupName + '_' + LKC.KeyChildName + '_' + LF.LanguageFieldName AS FieldName, LT.LanguageText, DefaultLanguageText, L.DefaultText AS [Language]
FROM LanguageField LF
INNER JOIN LanguageKeyChild LKC ON LF.LanguageKeyChildId = LKC.LanguageKeyChildId 
INNER JOIN LanguageKeyGroup LKG ON LKC.LanguageKeyGroupId = LKG.LanguageKeyGroupId
LEFT OUTER JOIN LanguageText LT ON LF.LanguageFieldId = LT.LanguageFieldId AND LT.LanguageId = @LanguageId
INNER JOIN [Language] L ON @LanguageId = L.LanguageID
WHERE (LF.LanguageFieldId = @LanguageFieldId OR @LanguageFieldId IS NULL)
ORDER BY LKG.KeyGroupName, LKC.KeyChildName, LF.LanguageFieldName

IF @@ERROR <> 0
BEGIN
    RETURN 1
END
 
RETURN 0