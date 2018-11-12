CREATE TABLE [Search].[CountyAlias] (
    [CountyAliasId]     INT				IDENTITY (1, 1) NOT NULL,
	[CountyOnsName]		NVARCHAR(32)	NOT NULL,
	[CountyAlias1]		NVARCHAR(32)	NULL,
	[CountyAlias2]		NVARCHAR(32)	NULL,
	[CountyAlias3]		NVARCHAR(32)	NULL,
	[CountyAlias4]		NVARCHAR(32)	NULL,
	[CountyAlias5]		NVARCHAR(32)	NULL,
	[CountyAlias6]		NVARCHAR(32)	NULL,
	[CountyAlias7]		NVARCHAR(32)	NULL,
    CONSTRAINT [PK__CountyAlias__BD596394DB986AF5] PRIMARY KEY NONCLUSTERED ([CountyAliasId] ASC)
);
