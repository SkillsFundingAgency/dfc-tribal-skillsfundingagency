CREATE PROCEDURE [search].[PublishVenue]
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 
AS BEGIN /* WITH (
      TRANSACTION ISOLATION LEVEL = SNAPSHOT,
      LANGUAGE = 'English') */

	DELETE FROM [dbo].[Venue];
	INSERT INTO [dbo].[Venue] (
		[ProviderId]
		, [VenueId]
		, [VenueName]
		, [ProviderOwnVenueRef]
		, [Telephone]
		, [AddressLine1]
		, [AddressLine2]
		, [Town]
		, [County]
		, [Postcode]
		, [Email]
		, [Fax]
		, [Facilities]
		, [CreatedDateTimeUtc]
		, [ModifiedDateTimeUtc]
		, [RecordStatusId]
		, [CreatedByUserId]
		, [ModifiedByUserId]
		, [Easting]
		, [Northing]
		, [SearchRegion]
		, [ApplicationId]
		, [Latitude]
		, [Longitude]
		, [RegionLevelPenalty]
	)
	SELECT
		[ProviderId]
		, [VenueId]
		, [VenueName]
		, [ProviderOwnVenueRef]
		, [Telephone]
		, [AddressLine1]
		, [AddressLine2]
		, [Town]
		, [County]
		, [Postcode]
		, [Email]
		, [Fax]
		, [Facilities]
		, [CreatedDateTimeUtc]
		, [ModifiedDateTimeUtc]
		, [RecordStatusId]
		, [CreatedByUserId]
		, [ModifiedByUserId]
		, [Easting]
		, [Northing]
		, [SearchRegion]
		, [ApplicationId]
		, [Latitude]
		, [Longitude]
		, [RegionLevelPenalty]
	FROM [search].[Venue];

	DELETE FROM [dbo].[Venue1]
	INSERT INTO [dbo].[Venue1] (
		[VenueId]
		, [Website]
		, [PostTown]
		, [DependentLocality]
		, [DoubleDependentLocality]
		, [VenueLocationId]
		, [EuropeanElectoralRegion]
		, [LocalAuthorityDistrict]
		, [CurrentElectoralWard]
		, [OnsCounty]
		, [CountyAliasId]
		, [ParishCommunity]
		, [CensusBuiltUpAreaSubDivision]
	)
	SELECT
		[VenueId]
		, [Website]
		, [PostTown]
		, [DependentLocality]
		, [DoubleDependentLocality]
		, [VenueLocationId]
		, [EuropeanElectoralRegion]
		, [LocalAuthorityDistrict]
		, [CurrentElectoralWard]
		, [OnsCounty]
		, [CountyAliasId]
		, [ParishCommunity]
		, [CensusBuiltUpAreaSubDivision]
	FROM [search].[Venue];


	TRUNCATE TABLE [dbo].[VenueGeography];
	INSERT INTO [dbo].[VenueGeography] (
		VenueId,
		[Geography]
	)
	SELECT VenueId,
		[Geography]
	FROM [Search].[Venue]
	WHERE [Geography] IS NOT NULL;


	RETURN 0;

END;