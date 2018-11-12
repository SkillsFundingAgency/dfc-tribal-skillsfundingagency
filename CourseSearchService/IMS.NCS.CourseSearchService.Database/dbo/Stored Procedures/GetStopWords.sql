CREATE PROCEDURE [dbo].[GetStopWords]
AS

BEGIN

	SELECT SW.StopWord 
	FROM sys.fulltext_stopwords SW
		INNER JOIN sys.fulltext_stoplists SL ON SL.StopList_Id = SW.StopList_Id
	WHERE SL.Name = 'CourseSearch'
		AND SW.language_id = 1033; 

END;