CREATE FUNCTION [dbo].[GetLanguageTextForQualifiedFieldName](@QualifiedFieldName NVARCHAR(300), @LanguageId INT, @DefaultText NVARCHAR(2000))
RETURNS NVARCHAR(2000)
AS 
BEGIN
	
	DECLARE @LanguageKeyGroupId INT
	DECLARE @LanguageKeyChildId INT
	DECLARE @LanguageFieldId INT
	
	DECLARE @Names TABLE (Id INT NOT NULL IDENTITY(1,1), Name NVARCHAR(100))
	-- From the field name extract the three individual field names for group/child/field
	INSERT INTO @Names SELECT * FROM dbo.Split(@QualifiedFieldName, '_')
	-- Must have three and not nulls
	IF ((SELECT COUNT(-1) FROM @Names WHERE Name IS NOT NULL AND Name <> '') <> 3)
	BEGIN		
		RETURN @DefaultText
	END
	
	/*******************************************************************
	** Check the KeyGroup exists for the group name 
	********************************************************************/
	SELECT @LanguageKeyGroupId = LKG.LanguageKeyGroupId FROM LanguageKeyGroup LKG WHERE LOWER(LKG.KeyGroupName) = (SELECT LOWER(Name) FROM @Names WHERE Id = 1)
	-- If group doesn't exist return null
	IF (@LanguageKeyGroupId IS NULL) 
	BEGIN
		RETURN @DefaultText
	END
	
	/*******************************************************************
	** Check the KeyChild exists for the keygroup 
	********************************************************************/
	-- Now check the child exists
	SELECT @LanguageKeyChildId = LKC.LanguageKeyChildId FROM LanguageKeyChild LKC 
	WHERE LOWER(LKC.KeyChildName) = (SELECT LOWER(Name) FROM @Names WHERE Id = 2) AND LKC.LanguageKeyGroupId = @LanguageKeyGroupId
	-- Check exists if not add it
	IF (@LanguageKeyChildId IS NULL)
	BEGIN
		RETURN @DefaultText
	END
	
	/*******************************************************************
	** Check the Language field exists, if it does we can then fetch the language text
	********************************************************************/
	SELECT  @LanguageFieldId = LanguageFieldId FROM LanguageField LF WHERE LF.LanguageKeyChildId = @LanguageKeyChildId AND LOWER(LF.LanguageFieldName) =  (SELECT LOWER(Name) FROM @Names WHERE Id = 3)
	IF (@LanguageFieldId IS NULL)
	BEGIN
		-- Does't exist so return null
		RETURN @DefaultText
	END
	
	-- Now we have the @LanguageFieldId try and fetch the text
	DECLARE @LanguageText NVARCHAR(2000)
	SELECT @LanguageText = LT.LanguageText FROM LanguageText LT 
	WHERE LT.LanguageFieldId = @LanguageFieldId AND LT.LanguageId = @LanguageId
	
	RETURN COALESCE(@LanguageText, @DefaultText)
	
	
END

