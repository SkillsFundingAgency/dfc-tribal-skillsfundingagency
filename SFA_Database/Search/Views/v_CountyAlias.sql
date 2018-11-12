CREATE VIEW [search].[v_CountyAlias]
	AS
	SELECT
		la.LocationAliasId CountyAliasId
		, la.LocationAliasName CountyOnsName
		, res.[1] CountyAlias1
		, res.[2] CountyAlias2
		, res.[3] CountyAlias3
		, res.[4] CountyAlias4
		, res.[5] CountyAlias5
		, res.[6] CountyAlias6
		, res.[7] CountyAlias7
	FROM LocationAlias la
		JOIN (
			SELECT *
			FROM (
				SELECT a.ParentLocationAliasId, a.LocationAliasName,
				ROW_NUMBER() OVER (PARTITION BY a.ParentLocationAliasId ORDER BY a.LocationAliasId) rownum
				FROM LocationAlias a
					JOIN LocationAliasType t on t.LocationAliasTypeId = a.LocationAliasTypeId
				WHERE t.LocationAliasTypeName = 'County' AND a.ParentLocationAliasId IS NOT NULL
			) src
			PIVOT
			(
				Max(LocationAliasName)
				for rownum in ([1], [2], [3], [4], [5], [6], [7])
			) piv
		) res ON res.ParentLocationAliasId = la.LocationAliasId
	WHERE la.ParentLocationAliasId IS NULL
