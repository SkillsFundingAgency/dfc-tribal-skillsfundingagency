CREATE PROCEDURE [dbo].[up_ReportOrganisationTrafficLight]
	@OrganisationId int
AS

/*
*	Name:		[up_ReportOrganisationTrafficLight]
*	System: 	Stored procedure interface module
*	Description:	Get traffic light report data for an organisation
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

DECLARE @LiveStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1)

SELECT
	p.ProviderId
	, p.Ukprn
	, pt.ProviderTypeName
	, p.ProviderName
	, p.ProviderNameAlias
	, rlp.LegalName UkrlpName
	, qs.ModifiedDateTimeUtc
	, a.ApplicationName
	, p.SFAFunded
	, p.DFE1619Funded
FROM OrganisationProvider op
	JOIN Provider p on p.ProviderId = op.ProviderId
	JOIN ProviderType pt on pt.ProviderTypeId = p.ProviderTypeId
	LEFT JOIN Ukrlp rlp on rlp.Ukprn = p.Ukprn
	LEFT JOIN QualityScore qs on qs.ProviderId = p.ProviderId
	LEFT JOIN Application a on a.ApplicationId = qs.ModifiedByApplicationId
WHERE
	op.IsAccepted = 1 AND op.IsRejected = 0
	AND p.RecordStatusId = @LiveStatus
	AND op.OrganisationId = @OrganisationId

exec [dbo].[up_OrganisationGetProviderPrimaryContacts] @OrganisationId

IF(@@ERROR <> 0)
BEGIN
	RETURN 1
END
	
RETURN 0
