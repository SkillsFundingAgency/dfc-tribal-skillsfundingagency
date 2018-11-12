CREATE FUNCTION [dbo].[SplitAndRejoinDistinct]
(
    @String NVARCHAR(4000),
    @Delimiter NCHAR(1)
)
RETURNS NVARCHAR(4000)
BEGIN
	DECLARE @listStr VARCHAR(4000);

	SELECT @listStr = COALESCE(@listStr+@Delimiter,'') + Items
	FROM (SELECT DISTINCT * FROM [dbo].[Split](@String, @Delimiter)) r;
	
	RETURN @listStr;
END;
