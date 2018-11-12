CREATE FUNCTION [dbo].[GetPostcodeOutcode](@Postcode NVARCHAR(20))
RETURNS NVARCHAR(4)
AS 
BEGIN	
	DECLARE @OutCode NVARCHAR(4)
	DECLARE @Space INT = CHARINDEX(' ', @Postcode, 1)
	IF(@Space > 0)
	BEGIN
		SET @OutCode = SUBSTRING(@Postcode, 1, 4)
	END
	ELSE
	BEGIN
		SET @OutCode = NULL
	END
	
	RETURN @OutCode	
END	
