CREATE FUNCTION [dbo].[CategoryLevel](@CategoryCode VARCHAR(10))
RETURNS INT
AS
BEGIN
 
	DECLARE @Level INT = 0;
	DECLARE @Parent VARCHAR(10) = @CategoryCode;
	DECLARE @OldParent VARCHAR(10) = '';

	WHILE (@Parent IS NOT NULL AND @Parent <> '' AND @Parent <> @OldParent AND @Level < 10)
	BEGIN
		SET @OldParent = @Parent;
		SELECT @Parent = ParentCategoryCode FROM [Remote].[CategoryCode] WHERE CategoryCode = @Parent;	
		SET @Level = @Level + 1;

		-- If Parent is same as old parent then parent row doesn't exist so set the level to -1 
		-- This will be ignored by the API
		IF @Parent = @OldParent
			SET @Level = -1;
	END

	RETURN @Level;

END