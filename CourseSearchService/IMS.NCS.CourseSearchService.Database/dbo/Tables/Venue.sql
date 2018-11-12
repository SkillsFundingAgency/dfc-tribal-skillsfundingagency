/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[Venue]
(
	 [VenueId]			    INT NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 32768)
	, [ProviderId]			INT NOT NULL			/* VENUE_ID */
	, [VenueName]			NVARCHAR(255) NOT NULL	/* VENUE_NAME */
	, [ProviderOwnVenueRef] NVARCHAR(255) NULL		/* PROV_VENUE_ID */
	, [Telephone]			NVARCHAR(30) NULL		/* PHONE */
	, [AddressLine1]		NVARCHAR(100) NOT NULL	/* ADDRESS_1 */
	, [AddressLine2]		NVARCHAR(100) NULL		/* ADDRESS_2 */
	, [Town]				NVARCHAR(75) NOT NULL	/* TOWN */
	, [County]				NVARCHAR(75) NULL		/* COUNTY */
	, [Postcode]			NVARCHAR(30) NOT NULL	/* POSTCODE */
	, [Email]				NVARCHAR(255) NULL		/* EMAIL */
	-- Next Table --, [Website]				NVARCHAR(511) NULL		/* WEBSITE */
	, [Fax]					NVARCHAR(35) NULL		/* FAX */
	, [Facilities]			NVARCHAR(2000) NULL		/* FACILITIES */
    , [CreatedDateTimeUtc]	DATETIME NOT NULL		/* DATE_CREATED */
	, [ModifiedDateTimeUtc] DATETIME NULL			/* DATE_UPDATE */
	, [RecordStatusId]		INT	NOT NULL			/* STATUS */ -- We only export live records to CSV so this not strictly necessary but here in case WHERE changes in future
	, [CreatedByUserId]     NVARCHAR(128) NOT NULL	/* CREATED_BY */
	, [ModifiedByUserId]	NVARCHAR(128) NULL		/* UPDATED_BY */
	--, [XMin]				??? NULL				/* XMIN */
	--, [XMax]				??? NULL				/* XMAX */
	--, [YMin]				???	NULL				/* YMIN */
	--, [YMax]				???	NULL				/* YMAX  */
	, [Easting]				FLOAT NULL				/* X_COORD */
	, [Northing]			FLOAT NULL				/* Y_COORD */
	, [SearchRegion]		NVARCHAR(30) NOT NULL	/* SEARCH_REGION */
	, [ApplicationId]		INT	NOT NULL			/* SYS_DATA_SOURCE */
	--, []					DATETIME NULL			/* DATE_UPDATED_COPY_OVER */
	--, []					DATETIME NULL			/* DATE_CREATED_COPY_OVER */
, 
    [Latitude] FLOAT NULL, 
    [Longitude] FLOAT NULL,
    [RegionLevelPenalty] FLOAT NOT NULL DEFAULT 0
) WITH (MEMORY_OPTIMIZED = ON)