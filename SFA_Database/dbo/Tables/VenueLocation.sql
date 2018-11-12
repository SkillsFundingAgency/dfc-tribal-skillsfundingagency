CREATE TABLE [dbo].[VenueLocation](
	[VenueLocationId] [int] IDENTITY(1,1) NOT NULL,
	[LocationName] [nvarchar](100) NOT NULL,
	[ParentVenueLocationId] [int] NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Geography]  AS ([geography]::STGeomFromText(((('Point('+CONVERT([varchar](32),[Longitude]))+' ')+CONVERT([varchar](32),[Latitude]))+')',(4326))), 
	[Region] [nvarchar](100) NULL,
    [Easting] INT NULL, 
    [Northing] INT NULL, 
    [EastingMin] INT NULL, 
    [EastingMax] NCHAR(10) NULL, 
    [NorthingMin] INT NULL, 
    [NorthingMax] NCHAR(10) NULL, 
    [NearestPostcode] NVARCHAR(10) NULL, 
    CONSTRAINT [PK_VenueLocation] PRIMARY KEY ([VenueLocationId]), 
    CONSTRAINT [FK_VenueLocation_ParentVenueLocationId] FOREIGN KEY ([ParentVenueLocationId]) REFERENCES [VenueLocation]([VenueLocationId]));
GO

CREATE NONCLUSTERED INDEX [IX_VenueLocation_LocationName]
	ON [dbo].[VenueLocation] ([LocationName]);
GO

