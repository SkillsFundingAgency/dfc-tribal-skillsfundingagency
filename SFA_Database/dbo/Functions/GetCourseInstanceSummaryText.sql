CREATE FUNCTION [dbo].[GetCourseInstanceSummaryText]
(
	@DurationUnit INT,
	@DurationUnitName NVARCHAR(50),
	@DurationAsText NVARCHAR(150),
	@StudyModeName NVARCHAR(50),
	@Price DECIMAL(10,2),
	@PriceAsText NVARCHAR(150),
	@StartDate DATE,
	@StartDateDesc NVARCHAR(150),
	@VenueName NVARCHAR(255),
	@VenueLocationName NVARCHAR(100),
	@Region NVARCHAR(100)

)
RETURNS NVARCHAR(255)
AS
BEGIN

/*
Providers a summary output for the CSV export, although doubtful this information used for searching
*/
DECLARE @Output VARCHAR(255)

IF(@DurationUnit IS NOT NULL)
BEGIN
	-- Have duration output so add 
	SET @Output = CAST(@DurationUnit AS VARCHAR(10)) + ' ' + ISNULL(@DurationUnitName, '')
END
ELSE
BEGIN
	-- No duration so output the text
	SET @Output = SUBSTRING(ISNULL(@DurationAsText, ''), 1, 45)
	IF(LEN(@DurationAsText) > 45) SET @Output = @Output + '...'
END

-- Add the study mode
SET @Output = @Output + ' | ' + ISNULL(@StudyModeName, '') + ' | '

IF(@Price IS NOT NULL)
BEGIN
	-- Add the price 
	SET @Output = @Output + '£' + CAST(@Price AS VARCHAR(10))
END
ELSE
BEGIN
	-- No price, add the text if we have some
	SET @Output = @Output + SUBSTRING(@PriceAsText, 1, 45)
	IF(LEN(@PriceAsText) > 45) SET @Output = @Output + '...' 
END

IF(@StartDate IS NOT NULL)
BEGIN
	-- Have start date, format in using the format expected
	SET @Output = @Output + ' | ' + dbo.GetCsvDateString(@StartDate)
END
ELSE
BEGIN
	-- No date, add the description
	SET @Output = @Output + ' | ' + SUBSTRING(ISNULL(@StartDateDesc, 'Unavailable'), 1, 45)
	IF(LEN(@StartDateDesc) > 45) SET @Output = @Output + '...'
END

IF(@VenueLocationName IS NOT NULL)
BEGIN
	-- Have venue location
	SET @Output = @Output + ' | in ' + @VenueLocationName + '(' + @Region + ')'
END
ELSE
BEGIN
	-- Add venue name
	SET @Output = @Output + ' | at ' + ISNULL(@VenueName, '')
END

RETURN @Output

END