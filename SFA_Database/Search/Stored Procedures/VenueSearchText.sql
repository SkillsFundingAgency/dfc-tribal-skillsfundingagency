CREATE PROCEDURE [search].[VenueSearchText]
AS

BEGIN

	DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);

	SELECT v.VenueId,
		v.ProviderId,
		-- Create pipe separated list of distinct values ignoring case
		[dbo].[SplitAndRejoinDistinct](
			COALESCE(v.VenueName + '|' , '') 
			+ COALESCE(v.Town + '|', '') 
			+ COALESCE(v.County + '|', '') 
			+ COALESCE(v.PostTown + '|', '') 
			+ COALESCE(v.PostCode + '|', '') 
			+ COALESCE(v.DependentLocality + '|', '') 
			+ COALESCE(v.DoubleDependentLocality + '|', '') 
			+ COALESCE(vl.LocationName1 + '|', '') 
			+ COALESCE(vl.LocationName2 + '|', '') 
			+ COALESCE(vl.LocationName3 + '|', '') 
			+ COALESCE(vl.LocationName4 + '|', '') 
			+ COALESCE(vl.LocationName5 + '|', '') 
			+ COALESCE(vl.LocationName6 + '|', '') 
			+ COALESCE(vl.LocationName7 + '|', '') 
			+ COALESCE(vl.LocationName8 + '|', '') 
			+ COALESCE(v.EuropeanElectoralRegion + '|', '') 
			+ COALESCE(v.LocalAuthorityDistrict + '|', '') 
			+ COALESCE(v.CurrentElectoralWard + '|', '') 
			+ COALESCE(v.OnsCounty + '|', '') 
			+ COALESCE(ca.CountyAlias1 + '|', '')
			+ COALESCE(ca.CountyAlias2 + '|', '')
			+ COALESCE(ca.CountyAlias3 + '|', '')
			+ COALESCE(ca.CountyAlias4 + '|', '')
			+ COALESCE(ca.CountyAlias5 + '|', '')
			+ COALESCE(ca.CountyAlias6 + '|', '')
			+ COALESCE(ca.CountyAlias7 + '|', '')
			+ COALESCE(v.ParishCommunity + '|', '') 
			+ COALESCE(v.CensusBuiltUpAreaSubDivision, ''), '|') SearchText
	FROM Search.Venue v
		LEFT OUTER JOIN search.v_VenueLocation vl ON vl.VenueLocationId = v.VenueLocationId
		LEFT OUTER JOIN search.v_CountyAlias ca ON ca.CountyAliasId = v.CountyAliasId
	WHERE 
		V.RecordStatusId = @LiveRecordStatusId

	UNION ALL

	SELECT v.[VenueId], 
		v.ProviderId, 
		lh.[LocationName1]
	FROM [search].[Venue] v
			INNER JOIN [search].[v_VenueLocation] vl ON vl.[VenueLocationId] = v.[VenueLocationId]
			INNER JOIN [search].[v_VenueLocation] lh ON lh.LocationName1 = vl.[LocationName1]
												OR lh.LocationName1 = vl.[LocationName1]
												OR lh.LocationName2 = vl.[LocationName1]
												OR lh.LocationName3 = vl.[LocationName1]
												OR lh.LocationName4 = vl.[LocationName1]
												OR lh.LocationName5 = vl.[LocationName1]
												OR lh.LocationName6 = vl.[LocationName1]
												OR lh.LocationName7 = vl.[LocationName1]
												OR lh.LocationName8 = vl.[LocationName1]
												OR lh.LocationName9 = vl.[LocationName1]
	WHERE V.RecordStatusId = @LiveRecordStatusId
		-- Exclude ambiguous place names, e.g. NEWPORT & ASHFORD
		AND vl.[LocationName1] NOT IN (
			SELECT LocationName1
			FROM search.v_VenueLocation
			GROUP BY LocationName1
			HAVING Count(*) > 1
		);

	IF @@ERROR <> 0
	BEGIN
		RETURN 1;
	END

	RETURN 0;

END;