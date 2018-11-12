CREATE PROCEDURE [dbo].[up_LanguageFieldAddSetDefaultByFieldName]
	 @FieldName NVARCHAR(300),
	 @DefaultText  NVARCHAR(2000),
	 @AlwaysUpdateDefaultText BIT = 0 -- Always updates the default text even if an entry already exists, for use with up_LanguageFieldSetupTables that calls this SP
AS
/*
*	Name:		up_LanguageFieldAddSetDefaultByFieldName]
*	System: 	Stored procedure interface module
*	Description:	List all languages fields
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

BEGIN TRY
	BEGIN TRAN 	
		DECLARE @DefaultLanguageId INT
		DECLARE @Names TABLE (Id INT NOT NULL IDENTITY(1,1), Name NVARCHAR(100))
		-- From the field name extract the three individual field names for group/child/field
		INSERT INTO @Names SELECT * FROM dbo.Split(@FieldName, '_')
		-- Must have three and not nulls
		IF ((SELECT COUNT(-1) FROM @Names WHERE Name IS NOT NULL AND Name <> '') <> 3)
		BEGIN
			ROLLBACK TRAN
			RETURN 2
		END

		-- Fetch the default languageId
		IF((SELECT COUNT(-1) FROM [Language] L WHERE L.IsDefaultLanguage = 1) <> 1)
		BEGIN
			ROLLBACK
			RETURN 3 -- Zero or more than 1 default language set
		END
		SELECT @DefaultLanguageId = LanguageID FROM [Language] L WHERE L.IsDefaultLanguage = 1

		DECLARE @LanguageKeyGroupId INT
		DECLARE @LanguageKeyChildId INT
		DECLARE @LanguageFieldId INT

		/*******************************************************************
		** Check the KeyGroup exists for the group name if not then add it
		********************************************************************/
		SELECT @LanguageKeyGroupId = LKG.LanguageKeyGroupId FROM LanguageKeyGroup LKG WHERE LOWER(LKG.KeyGroupName) = (SELECT LOWER(Name) FROM @Names WHERE Id = 1)
		-- Check the group exists if not we add it
		IF (@LanguageKeyGroupId IS NULL) 
		BEGIN
			INSERT INTO LanguageKeyGroup (KeyGroupName) SELECT Name FROM @Names WHERE Id = 1
			SET @LanguageKeyGroupId = SCOPE_IDENTITY()
		END

		/*******************************************************************
		** Check the KeyChild exists for the keygroup name if not then add it
		********************************************************************/
		-- Now check the child exists
		SELECT @LanguageKeyChildId = LKC.LanguageKeyChildId FROM LanguageKeyChild LKC 
		WHERE LOWER(LKC.KeyChildName) = (SELECT LOWER(Name) FROM @Names WHERE Id = 2) AND LKC.LanguageKeyGroupId = @LanguageKeyGroupId
		-- Check exists if not add it
		IF (@LanguageKeyChildId IS NULL)
		BEGIN
			INSERT INTO LanguageKeyChild (LanguageKeyGroupId, KeyChildName) SELECT @LanguageKeyGroupId, Name FROM @Names WHERE Id = 2
			SET @LanguageKeyChildId = SCOPE_IDENTITY()
		END

		/*******************************************************************
		** Check the Language field exists, if not add it
		********************************************************************/
		SELECT  @LanguageFieldId = LanguageFieldId FROM LanguageField LF WHERE LF.LanguageKeyChildId = @LanguageKeyChildId AND LOWER(LF.LanguageFieldName) =  (SELECT LOWER(Name) FROM @Names WHERE Id = 3)
		IF (@LanguageFieldId IS NULL)
		BEGIN
			-- No language field entry so add it
			INSERT INTO LanguageField (LanguageFieldName, LanguageKeyChildId, DefaultLanguageText)
			VALUES ((SELECT Name FROM @Names WHERE Id = 3), @LanguageKeyChildId, @DefaultText)
			SET @LanguageFieldId = SCOPE_IDENTITY()
		END

		-- If there is no language text entry for the default language add it
		IF NOT EXISTS(SELECT LanguageFieldId FROM LanguageText LT WHERE LT.LanguageId = @DefaultLanguageId AND LT.LanguageFieldId = @LanguageFieldId)
		BEGIN
			INSERT INTO LanguageText (LanguageFieldId, LanguageId, LanguageText, ModifiedDateTimeUtc)
			VALUES (@LanguageFieldId, @DefaultLanguageId, @DefaultText, GETUTCDATE())
		END		
		ELSE IF (@AlwaysUpdateDefaultText = 1)
		BEGIN
			UPDATE LanguageField SET LanguageFieldName = (SELECT Name FROM @Names WHERE Id = 3), 
			DefaultLanguageText = @DefaultText
			WHERE LanguageFieldId = @LanguageFieldId
			
			-- Update the default text even though it already exists
			UPDATE LanguageText SET LanguageText = @DefaultText, ModifiedDateTimeUtc = GETUTCDATE()
			WHERE LanguageFieldId = @LanguageFieldId
		END
	COMMIT TRAN
	RETURN 0
END TRY
BEGIN CATCH
	ROLLBACK TRAN
END CATCH

RETURN 1