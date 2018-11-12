CREATE PROCEDURE [remote].[CopyOverVenueLocation]
AS
BEGIN

SET IDENTITY_INSERT [search].[VenueLocation] ON

TRUNCATE TABLE [search].[VenueLocation]
INSERT INTO [search].[VenueLocation] (
	[VenueLocationId]
	, [Latitude]
	, [Longitude]
	, [Geography]
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
	, [NearestPostcode]
)
SELECT
	[VenueLocationId]
	, [Latitude]
	, [Longitude]
	, [Geography]
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
	, [NearestPostcode]
FROM [remote].[VenueLocation]

SET IDENTITY_INSERT [search].[VenueLocation] OFF
 
RETURN 0
END