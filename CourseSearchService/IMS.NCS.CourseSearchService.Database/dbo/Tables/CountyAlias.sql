﻿CREATE TABLE [dbo].[CountyAlias] (
    [CountyAliasId]     INT NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 128),
	[CountyOnsName]		NVARCHAR(32)	NOT NULL,
	[CountyAlias1]		NVARCHAR(32)	NULL,
	[CountyAlias2]		NVARCHAR(32)	NULL,
	[CountyAlias3]		NVARCHAR(32)	NULL,
	[CountyAlias4]		NVARCHAR(32)	NULL,
	[CountyAlias5]		NVARCHAR(32)	NULL,
	[CountyAlias6]		NVARCHAR(32)	NULL,
	[CountyAlias7]		NVARCHAR(32)	NULL,
) WITH (MEMORY_OPTIMIZED = ON);