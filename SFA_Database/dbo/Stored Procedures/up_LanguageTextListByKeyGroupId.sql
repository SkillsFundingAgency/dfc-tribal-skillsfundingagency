CREATE PROCEDURE [dbo].[up_LanguageTextListByKeyGroupId]
     @LanguageId INT,
     @LanguageKeyGroupId INT,
     @LanguageKeyChildId INT,
     @LanguageText NVARCHAR(500),
     @SortColumn INT
WITH RECOMPILE AS
/*
*    Name:       [up_LanguageFieldsListByKeyGroupId]
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

IF (@LanguageText IS NOT NULL) SET @LanguageText = '%' + @LanguageText + '%'

SELECT LanguageId, FieldNameLowered, FieldName, LanguageText, DefaultLanguageText, LanguageFieldId, KeyGroupName,
KeyChildName, LanguageFieldName
FROM
( 
SELECT @LanguageId AS LanguageId,  LOWER(LKG.KeyGroupName + '_' + LKC.KeyChildName + '_' + LF.LanguageFieldName) AS FieldNameLowered, 
LKG.KeyGroupName + '_' + LKC.KeyChildName + '_' + LF.LanguageFieldName AS FieldName, LT.LanguageText, DefaultLanguageText, LF.LanguageFieldId,
LKG.KeyGroupName, LKC.KeyChildName, LF.LanguageFieldName, lkc.LanguageKeyChildId, lkg.LanguageKeyGroupId
FROM LanguageField LF
INNER JOIN LanguageKeyChild LKC ON LF.LanguageKeyChildId = LKC.LanguageKeyChildId 
INNER JOIN LanguageKeyGroup LKG ON LKC.LanguageKeyGroupId = LKG.LanguageKeyGroupId 
LEFT OUTER JOIN LanguageText LT ON LF.LanguageFieldId = LT.LanguageFieldId AND LT.LanguageId = @LanguageId
) X
WHERE (LanguageText LIKE @LanguageText OR KeyGroupName + '_' + KeyChildName + '_' + LanguageFieldName LIKE @LanguageText OR @LanguageText IS NULL)
AND (LanguageKeyChildId = @LanguageKeyChildId OR @LanguageKeyChildId IS NULL)
AND (LanguageKeyGroupId = @LanguageKeyGroupId OR @LanguageKeyGroupId IS NULL)
ORDER BY CASE @SortColumn
	WHEN 1 THEN KeyGroupName + '_' + KeyChildName + '_' + LanguageFieldName
	WHEN 2 THEN LanguageText
	WHEN 3 THEN DefaultLanguageText
	ELSE KeyGroupName + '_' + KeyChildName + '_' + LanguageFieldName
	END ASC
 
IF @@ERROR <> 0
BEGIN
    RETURN 1
END
 
RETURN 0