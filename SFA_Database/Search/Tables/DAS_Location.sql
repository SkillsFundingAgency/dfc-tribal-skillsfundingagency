
CREATE TABLE [Search].[DAS_Location](
	[LocationId] [int] NOT NULL,
	[ProviderId] [int] NOT NULL,
	[LocationName] [nvarchar](255) NULL,
	[AddressLine1] [nvarchar](100) NULL,
	[AddressLine2] [nvarchar](100) NULL,
	[Town] [nvarchar](75) NULL,
	[County] [nvarchar](75) NULL,
	[Postcode] [nvarchar](30) NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Telephone] [nvarchar](30) NULL,
	[Email] [nvarchar](255) NULL,
	[Website] [nvarchar](255) NULL,
    PRIMARY KEY NONCLUSTERED ([LocationId] ASC)
);
