CREATE PROCEDURE [dbo].[AddStopWord]
	@StopWord NVARCHAR(64)
AS

BEGIN

	IF NOT EXISTS (SELECT StopWord FROM sys.fulltext_stopwords SW INNER JOIN sys.fulltext_stoplists SL ON SL.StopList_Id = SW.StopList_Id WHERE SL.Name = 'CourseSearch' AND SW.language_id = 1033 AND StopWord = @StopWord)
	BEGIN
		DECLARE @Query NVARCHAR(Max) = 'ALTER FULLTEXT STOPLIST CourseSearch ADD ''' + @StopWord + ''' LANGUAGE 1033;';	
		EXECUTE(@Query);
	END;

	IF (@@ERROR <> 0)
	BEGIN
		RETURN -1;
	END;

	RETURN 0;

END;
