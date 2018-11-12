CREATE PROCEDURE [Search].[DAS_LocationList]

AS

BEGIN

	DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);

	SELECT L.LocationId AS LOCATION_ID,
		L.ProviderId AS PROVIDER_ID,
		L.LocationName AS LOCATION_NAME,
		REPLACE(REPLACE(REPLACE(A.AddressLine1, Char(13) + Char(10), ' '), Char(10), ' '), Char(13), ' ') AS ADDRESS_1,
		REPLACE(REPLACE(REPLACE(A.AddressLine2, Char(13) + Char(10), ' '), Char(10), ' '), Char(13), ' ') AS ADDRESS_2,
		REPLACE(REPLACE(REPLACE(A.Town, Char(13) + Char(10), ' '), Char(10), ' '), Char(13), ' ') AS TOWN,
		REPLACE(REPLACE(REPLACE(A.County, Char(13) + Char(10), ' '), Char(10), ' '), Char(13), ' ') AS COUNTY,
		REPLACE(REPLACE(REPLACE(A.Postcode, Char(13) + Char(10), ' '), Char(10), ' '), Char(13), ' ') AS POSTCODE,
		A.Latitude AS LATITUDE,
		A.Longitude AS LONGITUDE,
		L.Telephone AS TELEPHONE,
		L.Email AS EMAIL,
		L.Website AS WEBSITE
	FROM dbo.Location L
		INNER JOIN dbo.Provider P on P.ProviderId=L.ProviderId
		LEFT OUTER JOIN dbo.Address A ON L.AddressId = A.AddressId
	WHERE L.RecordStatusId = @LiveRecordStatusId
		AND P.RecordStatusId = @LiveRecordStatusId
		AND	p.PublishData = 1
		AND P.ApprenticeshipContract = 1
		AND P.PassedOverallQAChecks = 1
		AND COALESCE(P.MarketingInformation, '') <> ''
		AND EXISTS (
						select al.apprenticeshipId 
						from dbo.ApprenticeshipLocation al 
							inner join dbo.Apprenticeship a on a.ApprenticeshipId = al.ApprenticeshipId
						where al.LocationId = l.LocationId
							and a.RecordStatusId = @LiveRecordStatusId
				   )
	ORDER BY L.ProviderId;

END;

