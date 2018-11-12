CREATE PROCEDURE [dbo].[up_LanguageFieldListTablesWithLanguageFields]
	
AS
/*
*	Name:		[up_LanguageFieldListTablesWithLanguageFields]
*	System: 	Stored procedure interface module
*	Description:	Returns all tables using DefaultText and LanguageFieldNames to retrieve text from the language system
*
*	Return Values:	0 = No problem detected
*			1 = General database error.

*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:$
*/

SELECT t.name AS TableName, dbo.GetPrimaryKeyForTable(t.name) AS PrimaryKey
--SCHEMA_NAME(schema_id) AS schema_name,
--c.name AS column_name
FROM sys.tables AS t
INNER JOIN sys.columns c ON t.OBJECT_ID = c.OBJECT_ID
INNER JOIN sys.columns c1 ON t.object_id = c1.object_id
WHERE c.name = 'DefaultText' AND c1.name = 'LanguageFieldName'
ORDER BY t.name

IF (@@ERROR<>0) RETURN 1 ELSE RETURN 0