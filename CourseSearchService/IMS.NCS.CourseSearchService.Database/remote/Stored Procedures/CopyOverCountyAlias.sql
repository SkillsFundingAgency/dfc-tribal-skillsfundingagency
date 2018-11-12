CREATE PROCEDURE [remote].[CopyOverCountyAlias]
AS
BEGIN

SET IDENTITY_INSERT [search].[CountyAlias] ON

TRUNCATE TABLE [search].[CountyAlias]
INSERT INTO [search].[CountyAlias] (
	[CountyAliasId]
	, [CountyOnsName]
	, [CountyAlias1]
	, [CountyAlias2]
	, [CountyAlias3]
	, [CountyAlias4]
	, [CountyAlias5]
	, [CountyAlias6]
	, [CountyAlias7]
)
SELECT
	[CountyAliasId]
	, [CountyOnsName]
	, [CountyAlias1]
	, [CountyAlias2]
	, [CountyAlias3]
	, [CountyAlias4]
	, [CountyAlias5]
	, [CountyAlias6]
	, [CountyAlias7]
FROM [remote].[CountyAlias];

SET IDENTITY_INSERT [search].[CountyAlias] OFF
 
RETURN 0
END