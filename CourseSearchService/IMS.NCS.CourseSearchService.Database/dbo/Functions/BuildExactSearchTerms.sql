CREATE FUNCTION [dbo].[BuildExactSearchTerms] (@SearchText VARCHAR(1000))
RETURNS VARCHAR(MAX)
AS
BEGIN

	SET @SearchText = LTRIM(RTRIM(@SearchText));
	IF (@SearchText = '')
		RETURN @SearchText;

	-- If the string passed in contains " then pass it straight back out
	IF (CharIndex('"', @SearchText) <> 0)
		RETURN @SearchText;

	-- Remove any double pipes
	WHILE (CharIndex('||', @SearchText) <> 0)
	BEGIN
		SET @SearchText = REPLACE(@SearchText, '||', '|');
	END;

	DECLARE @returnValue VARCHAR(MAX);

	DECLARE @pipePos INT = CharIndex('|', @SearchText);
	IF (@pipePos = 0)
		RETURN '"' + @SearchText + '"';
	ELSE
		BEGIN	
			SET @returnValue = '"' + SubString(@SearchText, 1, @pipePos - 1) + '"';
			WHILE (@pipePos <> 0)
			BEGIN
				DECLARE @newPipePos INT = CharIndex('|', @SearchText, @pipePos + 1);
				SET @returnValue = @returnValue + ' OR ';

				IF (@newPipePos = 0)
					SET @returnValue = @returnValue + '"' + SubString(@SearchText, @pipePos + 1, Len(@SearchText) - @pipePos) + '"';
				ELSE
					SET @returnValue = @returnValue + '"' + SubString(@SearchText, @pipePos + 1, @newPipePos - @pipePos - 1) + '"';

				SET @pipePos = CharIndex(' ', @SearchText, @pipePos + 1);
			END;
		END;	

	-- Return the result of the function
	RETURN @returnValue;

END;
