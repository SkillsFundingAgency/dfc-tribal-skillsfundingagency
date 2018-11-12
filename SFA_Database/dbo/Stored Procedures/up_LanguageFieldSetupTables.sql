CREATE PROCEDURE [dbo].[up_LanguageFieldSetupTables]
	@RecreateAllTableKeys BIT = 0	
 
AS
/*
*	Name:		[up_TablesWithLookupLanguageFields]

*	System: 	Stored procedure interface module
*	Description:	All tables that have a DefaultText field and LanguageFieldName field will have language entries added into the
*                   the language system automatically when this script is run.  If @RecreateAllTableKeys is true then the LanguageFieldName in the table is 
*                   rebuild usign the 'Table_' + <tablename> + '_<primarykey>'
*
*	Return Values:	0 = No problem detected
*			1 = General database error.

*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:$
*/

DECLARE @TablesWithLookups TABLE (TableName NVARCHAR(128), PrimaryKey NVARCHAR(128))
DECLARE @TableLanguageLookupFields TABLE (TableName NVARCHAR(128), DefaultText NVARCHAR(2000), LanguageFieldName NVARCHAR(300), PrimaryKey INT)
DECLARE @TableRecords TABLE (PrimaryKey INT, LanguageFieldName NVARCHAR(300), DefaultText NVARCHAR(2000))

INSERT INTO @TablesWithLookups
EXEC up_LanguageFieldListTablesWithLanguageFields

-- Cursor through each table and ensure a language field name is set
DECLARE @ProcessTableName NVARCHAR(128)
DECLARE @PrimaryKeysWithNoLanguageField TABLE (PrimaryKey INT)
DECLARE db_cursor CURSOR FOR SELECT TableName FROM @TablesWithLookups
DECLARE @Sql NVARCHAR(500)
OPEN db_cursor FETCH NEXT FROM db_cursor INTO @ProcessTableName
WHILE @@FETCH_STATUS = 0
BEGIN
	-- Set primary key
	DECLARE @PrimaryKey NVARCHAR(128)= (SELECT PrimaryKey FROM @TablesWithLookups WHERE TableName = @ProcessTableName)
		
	IF(@RecreateAllTableKeys = 1)
	BEGIN
		-- If resetting LanguageFieldNames on the tables then drop all existing ones, this means keys are recreated afresh with the correct Ids etc
		SET @Sql = 'UPDATE ' + @ProcessTableName + ' SET LanguageFieldName = '''''
		EXEC (@Sql)
	END
	
	SET @Sql = 'SELECT ' + CAST(@PrimaryKey AS NVARCHAR(128)) + ' FROM ' + @ProcessTableName + ' WHERE LanguageFieldName IS NULL OR LanguageFieldName = '''''
	
	DELETE FROM @PrimaryKeysWithNoLanguageField
	INSERT INTO @PrimaryKeysWithNoLanguageField EXEC(@Sql)	

	-- Loop through all the records that have no languagefieldnames and create the name and add it
	DECLARE db_record CURSOR FOR SELECT PrimaryKey FROM @PrimaryKeysWithNoLanguageField
	DECLARE @CurrentRecord INT 
	OPEN db_record FETCH NEXT FROM db_record INTO @CurrentRecord
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Create the key and then update the record
		DECLARE @LanguageFieldName NVARCHAR(300) = 'Table_' + @ProcessTableName + '_' + CAST(@CurrentRecord AS NVARCHAR(10))
		SET @Sql = 'UPDATE ' + @ProcessTableName + ' SET LanguageFieldName =''' + @LanguageFieldName + ''' WHERE ' + @PrimaryKey + '=' + CAST(@CurrentRecord AS NVARCHAR(10))
		EXEC (@Sql)
	
		FETCH NEXT FROM db_record INTO @CurrentRecord
	END
	CLOSE db_record  
	DEALLOCATE db_record 
	
	-- Now check for every record in this table that it has a language entry, if not we add one
	SET @Sql = 'SELECT ' + @PrimaryKey + ', LanguageFieldName, DefaultText FROM ' + @ProcessTableName
	DELETE FROM @TableRecords
	INSERT INTO @TableRecords EXEC (@Sql)
	
	-- Cursor through all the records to check if a language field entry exists, if not we add
	DECLARE @RecordId INT
	DECLARE db_all_records CURSOR FOR SELECT PrimaryKey FROM @TableRecords
	OPEN db_all_records FETCH NEXT FROM db_all_records INTO @RecordId
	WHILE @@FETCH_STATUS = 0
	BEGIN	
		DECLARE @LanguageFieldNameToCheck NVARCHAR(300) = (SELECT LanguageFieldName FROM @TableRecords WHERE PrimaryKey = @RecordId)
		DECLARE @DefaultText NVARCHAR(2000) = (SELECT DefaultText FROM @TableRecords WHERE PrimaryKey = @RecordId)
		PRINT @LanguageFieldNameToCheck + ' - ' + @DefaultText
		EXEC [up_LanguageFieldAddSetDefaultByFieldName] @LanguageFieldNameToCheck, @DefaultText
		
		FETCH NEXT FROM db_all_records INTO @RecordId
	END
	CLOSE db_all_records
	DEALLOCATE db_all_records
	
	FETCH NEXT FROM db_cursor INTO @ProcessTableName	
END
CLOSE db_cursor  
DEALLOCATE db_cursor 



-- All lookup tables now have their keys set