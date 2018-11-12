CREATE TABLE [dbo].[GeoLocation]
(
	[Postcode] NVARCHAR(8) NOT NULL PRIMARY KEY, 
    [Lat] FLOAT NOT NULL, 
    [Lng] FLOAT NOT NULL, 
    [Northing] FLOAT NOT NULL, 
    [Easting] FLOAT NOT NULL 
)
