CREATE FUNCTION [dbo].[GetA10CommaDelimited](@CourseInstanceId INT)
RETURNS VARCHAR(50)
AS 
BEGIN	

	DECLARE @List VARCHAR(50)
	SELECT @List = COALESCE(@List+',' ,'') + CAST(CIFC.A10FundingCode AS VARCHAR(10))
	FROM CourseInstanceA10FundingCode CIFC
	WHERE CIFC.CourseInstanceId = @CourseInstanceId

	RETURN ISNULL(@List, '')
	
END	
GO
