CREATE PROCEDURE [search].[PublishCountyAlias]
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 
AS BEGIN /* WITH (
      TRANSACTION ISOLATION LEVEL = SNAPSHOT,
      LANGUAGE = 'English') */

DELETE FROM [dbo].[CountyAlias]
INSERT INTO [dbo].[CountyAlias] (
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
FROM [search].[CountyAlias];
	  
RETURN 0
END