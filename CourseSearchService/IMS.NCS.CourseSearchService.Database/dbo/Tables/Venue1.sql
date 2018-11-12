/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[Venue1]
(
	 [VenueId]					INT NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 32768)
	, [Website]					NVARCHAR(511)	 NULL
	-- Address Base
	, [PostTown]			    NVARCHAR(30)	NULL
	, [DependentLocality]       NVARCHAR(35)	NULL
	, [DoubleDependentLocality] NVARCHAR(35)	NULL
	-- Venue Location Hierarchy
	, [VenueLocationId]			INT				NULL
	-- ONS_PostcodeDirectory
	, [EuropeanElectoralRegion]	NVARCHAR(24)	NULL
    , [LocalAuthorityDistrict]	NVARCHAR(36)	NULL
	, [CurrentElectoralWard]	NVARCHAR(56)	NULL
    , [OnsCounty]				NVARCHAR(27)	NULL
	, [CountyAliasId]			INT				NULL
    , [ParishCommunity]			NVARCHAR(70)	NULL
    , [CensusBuiltUpAreaSubDivision]	NVARCHAR(47)	NULL
) WITH (MEMORY_OPTIMIZED = ON)