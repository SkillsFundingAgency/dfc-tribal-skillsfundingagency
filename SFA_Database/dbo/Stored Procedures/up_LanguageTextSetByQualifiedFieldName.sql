CREATE PROCEDURE [dbo].[up_LanguageTextSetByQualifiedFieldName]
	 @LanguageId INT,
	 @FieldName NVARCHAR(300),
	 @DefaultLanguageText NVARCHAR(2000),	 
	 @LanguageText NVARCHAR(2000)
	 
AS
/*
*	Name:		[up_LanguageTextSet]
*	System: 	Stored procedure interface module
*	Description:	Sets a language field
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*			2 = Qualifed field name does not contain 3 sections
*			3 = LanguageKeyGroup doesn't exist
*			4 = LanguageKeyChild doesn't exist
*			5 = LanguageField doesn't exist
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

DECLARE @LanguageKeyGroupId INT
DECLARE @LanguageKeyChildId INT
DECLARE @LanguageFieldId INT

-- Need to parse the FieldName to find the group/child and language field Id
DECLARE @Names TABLE (Id INT NOT NULL IDENTITY(1,1), Name NVARCHAR(100))
-- From the field name extract the three individual field names for group/child/field
INSERT INTO @Names SELECT * FROM dbo.Split(@FieldName, '_')
-- Must have three and not nulls
IF ((SELECT COUNT(-1) FROM @Names WHERE Name IS NOT NULL AND Name <> '') <> 3)
BEGIN		
		RETURN 2
END

/*******************************************************************
** Check the KeyGroup exists for the group name if not can't set the field
********************************************************************/
SELECT @LanguageKeyGroupId = LKG.LanguageKeyGroupId FROM LanguageKeyGroup LKG WHERE LOWER(LKG.KeyGroupName) = (SELECT LOWER(Name) FROM @Names WHERE Id = 1)
-- Check the group exists if not we add it
IF (@LanguageKeyGroupId IS NULL) 
BEGIN
	RETURN 3
END

/*******************************************************************
** Check the KeyChild exists for the keygroup name if not then add it
********************************************************************/
-- Now check the child exists
SELECT @LanguageKeyChildId = LKC.LanguageKeyChildId FROM LanguageKeyChild LKC 
WHERE LOWER(LKC.KeyChildName) = (SELECT LOWER(Name) FROM @Names WHERE Id = 2) AND LKC.LanguageKeyGroupId = @LanguageKeyGroupId
-- Check exists if not can't set the field
IF (@LanguageKeyChildId IS NULL)
BEGIN
	RETURN 4
END

/*******************************************************************
** Check the Language field exists, if not add it
********************************************************************/
SELECT  @LanguageFieldId = LanguageFieldId FROM LanguageField LF WHERE LF.LanguageKeyChildId = @LanguageKeyChildId AND LOWER(LF.LanguageFieldName) =  (SELECT LOWER(Name) FROM @Names WHERE Id = 3)
IF (@LanguageFieldId IS NULL)
BEGIN
	-- No language field entry so add it
	RETURN 5
END

DECLARE @RowCount INT
DECLARE @Error INT
-- Now have Ids, set the LanguageText, or do insert
UPDATE LanguageText SET 
LanguageText = @LanguageText,
ModifiedDateTimeUtc = GETUTCDATE()
WHERE LanguageFieldId = @LanguageFieldId AND LanguageId = @LanguageId

SELECT @RowCount = @@ROWCOUNT, @Error = @@ERROR
IF (@Error <> 0)
BEGIN
	RETURN 1
END

-- If nothing updated need to insert a record
IF (@RowCount = 0)
BEGIN
	INSERT INTO LanguageText (LanguageFieldId, LanguageText, ModifiedDateTimeUtc, LanguageId)
	VALUES (@LanguageFieldId, @LanguageText, GETUTCDATE(), @LanguageId)
END
IF (@Error <> 0)
BEGIN
	RETURN 1
END

-- Do we need to set the default text
IF (@DefaultLanguageText IS NOT NULL)
BEGIN
	UPDATE LanguageField SET DefaultLanguageText = @DefaultLanguageText 
	WHERE LanguageFieldId = @LanguageFieldId
END
IF (@Error <> 0)
BEGIN
	RETURN 1
END
RETURN 0