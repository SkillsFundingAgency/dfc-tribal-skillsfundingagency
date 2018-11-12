CREATE PROCEDURE [Search].[VenueList]

AS

BEGIN

	DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);
	DECLARE @DeletedStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 0 AND RS.IsArchived = 0 AND RS.IsDeleted = 1);
	DECLARE @WorldVenueLocationId INT = (SELECT VenueLocationId FROM VenueLocation WHERE LocationName = 'WORLD');

	SELECT V.VenueId AS VENUE_ID,
		V.ProviderId AS PROVIDER_ID,
		V.VenueName AS VENUE_NAME,
		V.ProviderOwnVenueRef AS PROV_VENUE_ID,
		V.Telephone AS PHONE,
		A.AddressLine1 AS ADDRESS_1,
		A.AddressLine2 AS ADDRESS_2,
		A.Town AS TOWN,
		A.County AS COUNTY,
		A.Postcode AS POSTCODE,
		V.Email AS EMAIL,
		dbo.AppendTrackingUrl(V.Website, p.VenueTrackingUrl) AS WEBSITE,
		V.Fax AS FAX,
		V.Facilities AS FACILITIES,
		dbo.GetCsvDateTimeString(V.CreatedDateTimeUtc) AS DATE_CREATED,
		dbo.GetCsvDateTimeString(V.ModifiedDateTimeUtc) AS DATE_UPDATE,
		CASE WHEN V.RecordStatusId = @LiveRecordStatusId THEN @LiveRecordStatusId ELSE @DeletedStatusId END AS STATUS, -- We only export live records to CSV so this not strictly necessary but here in case WHERE changes in future
		'NDLPP_' + CAST(ANUCreatedBy.LegacyUserId AS VARCHAR(10)) AS CREATED_BY,
		'NDLPP_' + CAST(ANUModifiedBy.LegacyUserId AS VARCHAR(10)) AS UPDATED_BY,
		G.Easting,
		G.Northing,
		SUBSTRING(A.Postcode, 0, CHARINDEX(' ', A.Postcode, 1)) AS SEARCH_REGION,
		1 AS SYS_DATA_SOURCE,   
		CASE WHEN A.Latitude IS NOT NULL THEN A.Latitude ELSE G.Lat END AS Latitude,
		CASE WHEN A.Longitude IS NOT NULL THEN A.Longitude ELSE G.Lng END AS Longitude,
		-- Address Base
		ab.Town AS PostTown,
		ab.DependentLocality,
		ab.DoubleDependentLocality,
		-- Venue Location Hierarchy (excl. World)
		vl.VenueLocationId,
		-- ONS_PostcodeDirectory
		ons.EuropeanElectoralRegion,
		ons.LocalAuthorityDistrict,
		ons.CurrentElectoralWard,
		ons.County AS OnsCounty,
		ca.CountyAliasId,
		ons.ParishCommunity,
		ons.CensusBuiltUpAreaSubDivision,
		0
	FROM dbo.Venue V
		INNER JOIN dbo.Provider P ON P.ProviderId = V.ProviderId
		LEFT OUTER JOIN dbo.Address A ON V.AddressId = A.AddressId
		LEFT OUTER JOIN dbo.AspNetUsers ANUModifiedBy ON V.ModifiedByUserId = ANUModifiedBy.Id
		LEFT OUTER JOIN dbo.AspNetUsers ANUCreatedBy ON V.CreatedByUserId = ANUCreatedBy.Id
		LEFT OUTER JOIN dbo.GeoLocation G ON A.Postcode = G.Postcode
		CROSS APPLY (
			SELECT TOP 1 *
			FROM dbo.AddressBase abi
			WHERE abi.PostCode = A.Postcode
		) AS ab
		LEFT OUTER JOIN onspd.VenueEnhancement ons ON ons.Postcode = A.Postcode
		LEFT OUTER JOIN (
			SELECT vli.LocationName, 
				Count(*) LocationCount
			FROM VenueLocation vli
			GROUP BY LocationName
		) vlc ON ab.Town = vlc.LocationName
		LEFT OUTER JOIN dbo.venuelocation vl ON ab.town = vl.locationname AND vlc.LocationCount = 1
		LEFT OUTER JOIN search.v_CountyAlias ca ON ca.CountyOnsName = ons.County OR (ons.County IS NULL AND ca.CountyOnsName = a.County)
	WHERE V.RecordStatusId = @LiveRecordStatusId
		AND	P.RecordStatusId = @LiveRecordStatusId
		AND	p.PublishData = 1
	ORDER BY V.ProviderId;

	IF @@ERROR <> 0
	BEGIN
		RETURN 1;
	END

	RETURN 0;

END;