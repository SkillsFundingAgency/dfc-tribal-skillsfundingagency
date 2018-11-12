-- ==========================================================================
-- Author:		Steve Fulleylove
-- Create date: 7th September 2015
-- Description:	Used by the LARS importer to create the heirarchy for the API
-- ==========================================================================
CREATE PROCEDURE [dbo].[up_UpdateLDCSHierarchy]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Declare Variables
	DECLARE @Code NVARCHAR(10);
	DECLARE @Desc NVARCHAR(200);
	DECLARE @Cursor CURSOR

	-- Setup Cursor
	SET @Cursor = CURSOR FOR SELECT LearnDirectClassificationRef, LearnDirectClassSystemCodeDesc FROM [dbo].[LearnDirectClassification];
	OPEN @Cursor;
	FETCH NEXT FROM @Cursor INTO @Code, @Desc;

	-- Loop through rows
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		DECLARE @ParentCode NVARCHAR(10) = NULL;

		-- Determine Parent Code
		SET @ParentCode = @Code;

		-- If Only 3 Levels of Hierarchy
		IF (CHARINDEX('.', @ParentCode) BETWEEN 1 AND LEN(@ParentCode) - 1)
			SET @ParentCode = LEFT(@ParentCode, CHARINDEX('.', @ParentCode) + 1);

		-- Calculate Parent Code
		WHILE (LEN(@ParentCode) > 0)
		BEGIN
			SET @ParentCode = LEFT(@ParentCode, LEN(@ParentCode) - 1);
			IF EXISTS (SELECT * FROM [dbo].[LegacyCourseSubjectBrowseCategories] WHERE CategoryCodeId = @ParentCode)
				BREAK;
		END;

		-- Update or Insert Hierarchy
		IF EXISTS (SELECT * FROM [dbo].[LegacyCourseSubjectBrowseCategories] WHERE CategoryCodeId = @Code)
			UPDATE [dbo].[LegacyCourseSubjectBrowseCategories] 
			SET ParentCategoryCodeId = @ParentCode,
				[Description] = @Desc,
				IsSearchable = 1
			WHERE CategoryCodeId = @Code
				AND ParentCategoryCodeId IS NULL;  -- If item is already in a category then leave it. Remove this if we want to fix a category that's wrong and stop using the old Hot Courses categories
		ELSE
			INSERT INTO [dbo].[LegacyCourseSubjectBrowseCategories] (CategoryCodeId, ParentCategoryCodeId, [Description], IsSearchable) VALUES (@Code, @ParentCode, @Desc, 1);

		-- Get Next Row
		FETCH NEXT FROM @Cursor INTO @Code, @Desc;
	END;

	-- Close & Clean Up Cursor
	CLOSE @Cursor;
	DEALLOCATE @Cursor;

	-- Clear IsSearchable for all Codes that are parents
	UPDATE [dbo].[LegacyCourseSubjectBrowseCategories] SET IsSearchable = 1;

	UPDATE [dbo].[LegacyCourseSubjectBrowseCategories]
	SET IsSearchable = 0
	WHERE CategoryCodeId IN (SELECT DISTINCT ParentCategoryCodeId FROM [dbo].[LegacyCourseSubjectBrowseCategories]);

END;
