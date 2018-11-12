CREATE FUNCTION [dbo].[GetLdcsCodeList](@CourseId INT)
RETURNS VARCHAR(50)
AS 
BEGIN	

	DECLARE @List VARCHAR(50)
	SELECT @List = COALESCE(@List+' ' ,'') + CLDC.LearnDirectClassificationRef
	FROM CourseLearnDirectClassification CLDC
	WHERE CLDC.CourseId = @CourseId
	ORDER BY CLDC.ClassificationOrder ASC
	
	RETURN ISNULL(@List, '')
	
END	
GO