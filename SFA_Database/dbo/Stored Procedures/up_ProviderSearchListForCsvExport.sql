CREATE PROCEDURE [dbo].[up_ProviderSearchListForCsvExport]
	
AS
/*
*	Name:		[up_ProviderSearchListForCsvExport]
*	System: 	Stored procedure interface module
*	Description:	List all providers and organisations for the CSV export
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Education Ltd, 2014
*			All rights reserved.
*
*	$Log:  $
*/

-- This procedure creates the W_PROVIDER_TEXT.csv file

DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0)

SELECT 
	ProviderName
	+ ' ' + ISNULL(ProviderNameAlias, '') 
	+ ' ' + 
	+ Case When P.Ukprn=0 then '' else ISNULL(U.LegalName, '') END
	+ ' ' + ISNULL(U.TradingName, '') AS PROVIDER_SEARCH_TEXT,
	P.ProviderId AS PROVIDER_ID,
	P.ProviderName AS PROVIDERNAME
FROM Provider P
	LEFT OUTER JOIN Ukrlp U ON P.Ukprn = U.Ukprn
WHERE P.RecordStatusId = @LiveRecordStatusId
	AND P.PublishData = 1

UNION

-- LC: Add any organisations where display by or offered by is set by the provider as these should be included
-- to allow searching by org name, not sure if this happens currently but makes sense to
SELECT 
	OrganisationName
	+ ' ' + ISNULL(U.LegalName, '') 
	+ ' ' + ISNULL(U.TradingName, '') AS PROVIDER_SEARCH_TEXT,
	O.OrganisationId AS PROVIDER_ID,
	O.OrganisationName AS PROVIDERNAME
FROM Organisation O
	INNER JOIN CourseInstance CI ON O.OrganisationId = CI.DisplayedByOrganisationId
	INNER JOIN Course C ON CI.CourseId = C.CourseId
	LEFT OUTER JOIN Ukrlp U ON O.UKPRN = U.Ukprn
	JOIN Provider P on P.ProviderId = C.ProviderId
WHERE
	CI.RecordStatusId = @LiveRecordStatusId
	AND C.RecordStatusId = @LiveRecordStatusId
	AND P.RecordStatusId = @LiveRecordStatusId
	AND P.PublishData = 1
	AND O.RecordStatusId = @LiveRecordStatusId

UNION

SELECT 
	OrganisationName
	+ ' ' + ISNULL(U.LegalName, '') 
	+ ' ' + ISNULL(U.TradingName, '') AS PROVIDER_SEARCH_TEXT,
	O.OrganisationId AS PROVIDER_ID,
	O.OrganisationName AS PROVIDERNAME
FROM Organisation O
	INNER JOIN CourseInstance CI ON O.OrganisationId = CI.OfferedByOrganisationId
	INNER JOIN Course C ON CI.CourseId = C.CourseId
	LEFT OUTER JOIN Ukrlp U ON O.UKPRN = U.Ukprn
	JOIN Provider P on P.ProviderId = C.ProviderId
WHERE
	CI.RecordStatusId = @LiveRecordStatusId
	AND C.RecordStatusId = @LiveRecordStatusId
	AND P.RecordStatusId = @LiveRecordStatusId
	AND P.PublishData = 1
	AND O.RecordStatusId = @LiveRecordStatusId
ORDER BY ProviderName

IF @@ERROR <> 0
BEGIN
	RETURN 1
END

RETURN 0

GO