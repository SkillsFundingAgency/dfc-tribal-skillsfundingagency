CREATE TABLE [dbo].[VenueLocation] (
    [VenueLocationId]	INT NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 4096),
	[Latitude]			FLOAT			NULL,
	[Longitude]			FLOAT			NULL,
	[Easting]			INT				NULL,
	[Northing]			INT				NULL,
	[LocationName1]		NVARCHAR(100)	NOT NULL,
	[LocationName2]		NVARCHAR(100)	NULL,
	[LocationName3]		NVARCHAR(100)	NULL,
	[LocationName4]		NVARCHAR(100)	NULL,
	[LocationName5]		NVARCHAR(100)	NULL,
	[LocationName6]		NVARCHAR(100)	NULL,
	[LocationName7]		NVARCHAR(100)	NULL,
	[LocationName8]		NVARCHAR(100)	NULL,
	[LocationName9]		NVARCHAR(100)	NULL, 
    [NearestPostcode]	NVARCHAR(10)	NULL
) WITH (MEMORY_OPTIMIZED = ON);
