CREATE FUNCTION [dbo].[BuildSearchTerms] (@SearchText VARCHAR(1000))
RETURNS VARCHAR(MAX)
AS
BEGIN

	SET @SearchText = LTRIM(RTRIM(@SearchText));
	IF (@SearchText = '')
		RETURN @SearchText;

	-- If the string passed in contains " then pass it straight back out
	IF (CharIndex('"', @SearchText) <> 0)
		RETURN @SearchText;

	-- Remove any double spaces
	WHILE (CharIndex('  ', @SearchText) <> 0)
	BEGIN
		SET @SearchText = REPLACE(@SearchText, '  ', ' ');
	END;

	DECLARE @returnValue VARCHAR(MAX);

	DECLARE @spacePos INT = CharIndex(' ', @SearchText);
	IF (@spacePos = 0)
		RETURN 'FORMSOF (THESAURUS, "' + @SearchText + '") OR FORMSOF (INFLECTIONAL, "' + @SearchText + '")';
	ELSE
		BEGIN	
			SET @returnValue = '(FORMSOF (THESAURUS, "' + SubString(@SearchText, 1, @spacePos - 1) + '") OR FORMSOF (INFLECTIONAL, "' + SubString(@SearchText, 1, @spacePos - 1) + '"))';
			WHILE (@spacePos <> 0)
			BEGIN
				DECLARE @newSpacePos INT = CharIndex(' ', @SearchText, @spacePos + 1);
				SET @returnValue = @returnValue + ' AND ';

				IF (@newSpacePos = 0)
					SET @returnValue = @returnValue + '(FORMSOF (THESAURUS, "' + SubString(@SearchText, @spacePos + 1, Len(@SearchText) - @spacePos) + '") OR FORMSOF (INFLECTIONAL, "' + SubString(@SearchText, @spacePos + 1, Len(@SearchText) - @spacePos) + '"))';
				ELSE
					SET @returnValue = @returnValue + '(FORMSOF (THESAURUS, "' + SubString(@SearchText, @spacePos + 1, @newSpacePos - @spacePos - 1) + '") OR FORMSOF (INFLECTIONAL, "' + SubString(@SearchText, @spacePos + 1, @newSpacePos - @spacePos - 1) + '"))';

				SET @spacePos = CharIndex(' ', @SearchText, @spacePos + 1);
			END;
		END;

	--SET @returnValue =  @returnValue + ' OR FORMSOF (THESAURUS, "' + @SearchText + '")';

	-- Return the result of the function
	RETURN @returnValue;

END;
