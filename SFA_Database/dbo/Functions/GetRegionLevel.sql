CREATE FUNCTION [dbo].[GetRegionLevel]
(
	@RegionId INT
)
RETURNS INT
AS
BEGIN

	DECLARE @Level INT = 0;
	DECLARE @ParentId INT = (SELECT ParentVenueLocationId FROM [dbo].[VenueLocation] WHERE VenueLocationId = @RegionId);

	WHILE (@ParentId IS NOT NULL)
	BEGIN
		SET @Level = @Level + 1;
		SELECT @ParentId = ParentVenueLocationId FROM [dbo].[VenueLocation] WHERE VenueLocationId = @ParentId;
	END;

	SET @Level = 7 - @Level;

	IF (@Level < 0) 
	BEGIN
		SET @Level = 0;
	END;

	RETURN @Level;

END;
