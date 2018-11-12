CREATE FUNCTION [dbo].[CategorySortOrder](@CategoryCode VARCHAR(10))
RETURNS VARCHAR(4000)
AS
BEGIN
 
	DECLARE @Separator VARCHAR(1) = '\';
	DECLARE @SortOrder VARCHAR(4000) = '';
	DECLARE @Desc VARCHAR(200);
	DECLARE @Parent VARCHAR(10) = @CategoryCode;
	DECLARE @OldParent VARCHAR(10) = '';

	WHILE (@Parent IS NOT NULL AND @Parent <> '' AND @Parent <> @OldParent)
	BEGIN
		SET @OldParent = @Parent;
		SELECT @Parent = ParentCategoryCode, @Desc = [Description] FROM [Remote].[CategoryCode] WHERE CategoryCode = @Parent;		
		IF (@Parent <> @OldParent)
		BEGIN
			IF (Len(@SortOrder) = 0)
				SET @SortOrder = Replace(@Desc, @Separator, ' ');
			ELSE
				SET @SortOrder = Replace(@Desc, @Separator, ' ') + @Separator + @SortOrder;
		END;
	END;

	RETURN @SortOrder

END;