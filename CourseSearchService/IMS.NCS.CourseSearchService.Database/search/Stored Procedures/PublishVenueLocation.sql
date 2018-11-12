CREATE PROCEDURE [search].[PublishVenueLocation]
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 
AS BEGIN /* WITH (
      TRANSACTION ISOLATION LEVEL = SNAPSHOT,
      LANGUAGE = 'English') */

DELETE FROM [dbo].[VenueLocation]
INSERT INTO [dbo].[VenueLocation] (
	[VenueLocationId]
	, [Latitude]
	, [Longitude]
	, [Easting]
	, [Northing]
	, [LocationName1]
	, [LocationName2]
	, [LocationName3]
	, [LocationName4]
	, [LocationName5]
	, [LocationName6]
	, [LocationName7]
	, [LocationName8]
	, [LocationName9]
)
SELECT
	[VenueLocationId]
	, [Latitude]
	, [Longitude]
	, [Easting]
	, [Northing]
	, [LocationName1]
	, [LocationName2]
	, [LocationName3]
	, [LocationName4]
	, [LocationName5]
	, [LocationName6]
	, [LocationName7]
	, [LocationName8]
	, [LocationName9]
FROM [search].[VenueLocation]
	  
RETURN 0
END