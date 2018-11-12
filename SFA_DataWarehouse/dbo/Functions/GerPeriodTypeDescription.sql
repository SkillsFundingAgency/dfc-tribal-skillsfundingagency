CREATE FUNCTION [dbo].[GetPeriodTypeDescription]
(
	@PeriodType		NVARCHAR(1)
)
RETURNS NVARCHAR(15)
AS
BEGIN

	IF (@PeriodType = 'W')
		RETURN 'Week';

	IF (@PeriodType = 'M')
		RETURN 'Month';

	RETURN '';

END;
