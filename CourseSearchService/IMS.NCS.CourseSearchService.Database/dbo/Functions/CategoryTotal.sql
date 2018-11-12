CREATE  FUNCTION [dbo].[CategoryTotal](@CategoryCode VARCHAR(10), @UCASData BIT = 0)
RETURNS INT
AS
BEGIN
 
	DECLARE @Total INT = 0
	SELECT @Total = CASE WHEN @UCASData = 1 THEN TotalUCASCourses ELSE TotalCourses END FROM [Remote].[CategoryCode] WHERE CategoryCode = @CategoryCode;

	DECLARE Children_Cursor CURSOR FOR
		SELECT CategoryCode FROM [Remote].[CategoryCode] WHERE ParentCategoryCode = @CategoryCode;

	OPEN Children_Cursor;
	FETCH NEXT FROM Children_Cursor INTO @CategoryCode;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @Total = @Total + [dbo].[CategoryTotal](@CategoryCode, @UCASData);
		FETCH NEXT FROM Children_Cursor INTO @CategoryCode;
	END;

	CLOSE Children_Cursor;
	DEALLOCATE Children_Cursor;

	RETURN @Total

END
