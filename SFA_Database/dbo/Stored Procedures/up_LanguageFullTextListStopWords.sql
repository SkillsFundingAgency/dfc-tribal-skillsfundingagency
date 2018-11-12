CREATE PROCEDURE [dbo].[up_LanguageFullTextListStopWords]

	@LanguageId INT	
		 
AS
/*
*	Name:		[up_LanguageFullTextListStopWords]
*	System: 	Stored procedure interface module
*	Description:	Lists all the stops for the language Id, returns no rows if FullTextindexing not enabled
*
*	Return Values:	0 = No problem detected
*			1 = General database error.

*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:$
*/

IF ((select compatibility_level from sys.databases where name=db_name()) > 90
	AND (SELECT FullTextServiceProperty('IsFullTextInstalled'))= 1)
BEGIN
	SELECT Stopword FROM sys.fulltext_system_stopwords FSS
	INNER JOIN [Language] L ON FSS.language_id = L.SqlLanguageId
	WHERE L.LanguageID = @LanguageId	
END
ELSE
BEGIN
	-- Return no rows in the table
	SELECT NULL AS Stopword WHERE NULL IS NOT NULL
END

IF (@@ERROR <> 0)
	RETURN 1
ELSE 
	RETURN 0