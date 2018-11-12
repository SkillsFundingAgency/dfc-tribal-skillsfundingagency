CREATE PROCEDURE [search].[ProviderTextSearchList]
	
AS

DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0)
DECLARE @IncludeUCASData INT = (SELECT IncludeUCASData FROM [Search].DataExportConfiguration);

SELECT 
	P.ProviderId AS PROVIDER_ID,
	NULL,
	ProviderName
	+ ' ' + ISNULL(ProviderNameAlias, '') 
	+ ' ' + 
	+ Case When P.Ukprn=0 then '' else ISNULL(U.LegalName, '') END
	+ ' ' + ISNULL(U.TradingName, '') AS PROVIDER_SEARCH_TEXT,
	P.ProviderName AS PROVIDERNAME,
	0 AS IsUCASData
FROM dbo.Provider P
	LEFT OUTER JOIN dbo.Ukrlp U ON P.Ukprn = U.Ukprn
WHERE P.RecordStatusId = @LiveRecordStatusId
	AND P.PublishData = 1

UNION

-- LC: Add any organisations where display by or offered by is set by the provider as these should be included
-- to allow searching by org name, not sure if this happens currently but makes sense to
SELECT 
	NULL,
	O.OrganisationId AS PROVIDER_ID,
	OrganisationName
	+ ' ' + ISNULL(U.LegalName, '') 
	+ ' ' + ISNULL(U.TradingName, '') AS PROVIDER_SEARCH_TEXT,
	O.OrganisationName AS PROVIDERNAME,
	0
FROM dbo.Organisation O
	JOIN dbo.CourseInstance CI ON O.OrganisationId = CI.DisplayedByOrganisationId
	JOIN dbo.Course C ON CI.CourseId = C.CourseId
	LEFT OUTER JOIN dbo.Ukrlp U ON O.UKPRN = U.Ukprn
	JOIN dbo.Provider P on P.ProviderId = C.ProviderId
WHERE
	CI.RecordStatusId = @LiveRecordStatusId
	AND C.RecordStatusId = @LiveRecordStatusId
	AND O.RecordStatusId = @LiveRecordStatusId
	AND P.RecordStatusId = @LiveRecordStatusId
	AND P.PublishData = 1

UNION

SELECT 
	NULL,
	O.OrganisationId AS PROVIDER_ID,
	OrganisationName
	+ ' ' + ISNULL(U.LegalName, '') 
	+ ' ' + ISNULL(U.TradingName, '') AS PROVIDER_SEARCH_TEXT,
	O.OrganisationName AS PROVIDERNAME,
	0
FROM dbo.Organisation O
	JOIN dbo.CourseInstance CI ON O.OrganisationId = CI.OfferedByOrganisationId
	JOIN dbo.Course C ON CI.CourseId = C.CourseId
	LEFT OUTER JOIN dbo.Ukrlp U ON O.UKPRN = U.Ukprn
	JOIN dbo.Provider P on P.ProviderId = C.ProviderId
WHERE
	CI.RecordStatusId = @LiveRecordStatusId
	AND C.RecordStatusId = @LiveRecordStatusId
	AND O.RecordStatusId = @LiveRecordStatusId
	AND P.RecordStatusId = @LiveRecordStatusId
	AND P.PublishData = 1

UNION 

SELECT O.OrgId * (-1) AS Provider_Id,	
	NULL,
	OrgName
	+ ' ' 
	+ ' '  
	+ CASE WHEN O.Ukprn = 0 THEN '' ELSE COALESCE(U.LegalName, '') END
	+ ' ' + COALESCE(U.TradingName, '') AS PROVIDER_SEARCH_TEXT,
	O.OrgName AS ProviderName,
	1
FROM [UCAS].Orgs O
	LEFT OUTER JOIN dbo.Ukrlp U ON O.Ukprn = U.Ukprn
WHERE @IncludeUCASData = 1
UNION

SELECT P.ProviderId AS Provider_Id,	
	NULL,
	P.ProviderName,
	P.ProviderName,
	1
FROM [UCAS_PG].Provider P
WHERE @IncludeUCASData = 1

ORDER BY ProviderName

IF @@ERROR <> 0
BEGIN
	RETURN 1
END

RETURN 0