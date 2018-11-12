CREATE TABLE [Search].[VenueLocation] (
    [VenueLocationId]   INT				IDENTITY (1, 1) NOT NULL,
	[Latitude]			FLOAT			NULL,
	[Longitude]			FLOAT			NULL,
	[Geography]			Geography		NULL,
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
	[NearestPostcode]	NVARCHAR(10)	NULL,
    CONSTRAINT [PK__VenueLocation__FC873642BA3546ED] PRIMARY KEY NONCLUSTERED ([VenueLocationId])
);
