CREATE FUNCTION [dbo].[GetCsvDateString](
	@DateTime DATETIME
	)
RETURNS VARCHAR(20)
AS 
BEGIN
	DECLARE @Output VARCHAR(20)
	IF(@DateTime IS NULL)
	BEGIN
		SET @Output = ''
	END
	ELSE
	BEGIN
		DECLARE @MonthText VARCHAR(3) = SUBSTRING(CONVERT(VARCHAR(20), @DateTime, 100), 1, 3)
		DECLARE @Year VARCHAR(2) = DATEPART(YEAR, @DateTime) - 2000
		DECLARE @Day VARCHAR(2) = RIGHT('0' + CAST(DATEPART(DAY, @DateTime) AS VARCHAR(2)), 2);				
		SET @Output = @Day + '-' + @MonthText + '-' + @Year
	END

	RETURN @Output	
END	
GO