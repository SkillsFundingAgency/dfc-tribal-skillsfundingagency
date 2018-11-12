CREATE PROCEDURE [dbo].[up_ReportAdminReport]
	@IncludeProviders bit
	, @IncludeOrganisations bit
	, @ContractingBodiesOnly bit
	, @SFAFunded bit
	, @DFEFunded bit
AS

/*
*	Name:		[up_ReportAdminReport]
*	System: 	Stored procedure interface module
*	Description:	Admin reports master
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

BEGIN

	DECLARE @LiveStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);
	DECLARE @DeletedStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsDeleted = 1);
	DECLARE @DefaultDate DATE = CAST('01 JAN 2000' AS DATE);

	SELECT
		p.ProviderId
		, p.Ukprn
		, p.IsContractingBody
		, convert(bit, 1) IsProvider
		, pt.ProviderTypeName
		, p.ProviderName
		, p.ProviderNameAlias
		, rlp.LegalName
		, qs.LastActivity
		, qs.ModifiedDateTimeUtc LastProvisionUpdate
		, COALESCE(Confirmations.NumberOfConfirmations, 0) AS UpToDateConfirmations
		, a.ApplicationName
		, COALESCE(ExpiredLARS.NumberOfExpiredLARS, 0) AS ExpiredLARS
		, io.Name InformationOfficerDisplayName
		, rm.Name RelationshipManagerDisplayName
		, qs.AutoAggregateQualityRating
		, p.SFAFunded
		, p.DFE1619Funded
		, p.PublishData
		, p.IsTASOnly
	FROM Provider p
		INNER JOIN ProviderType pt on pt.ProviderTypeId = p.ProviderTypeId
		LEFT OUTER JOIN Ukrlp rlp on rlp.Ukprn = p.Ukprn
		LEFT OUTER JOIN QualityScore qs on qs.ProviderId = p.ProviderId
		LEFT OUTER JOIN Application a on a.ApplicationId = qs.ModifiedByApplicationId
		LEFT OUTER JOIN AspNetUsers io on io.Id = p.InformationOfficerUserId
		LEFT OUTER JOIN AspNetUsers rm on rm.Id = p.RelationshipManagerUserId	
		LEFT OUTER JOIN (SELECT ProviderId, Count(*) As NumberOfConfirmations FROM Provider_AllCoursesOKConfirmations C WHERE DateTimeUtc > (SELECT COALESCE(ModifiedDateTimeUtc, @DefaultDate) FROM QualityScore WHERE ProviderId = C.ProviderId) GROUP BY ProviderId) Confirmations ON Confirmations.ProviderId = P.ProviderId
		LEFT OUTER JOIN (
							SELECT ProviderId,
								Count(*) AS NumberOfExpiredLARS
							FROM Course C
								INNER JOIN LearningAim LA ON LA.LearningAimRefId = C.LearningAimRefId
							WHERE LA.RecordStatusId = @DeletedStatus
								AND C.RecordStatusId = @LiveStatus
							GROUP BY ProviderId
						) ExpiredLARS ON ExpiredLARS.ProviderId = P.ProviderId
	WHERE @IncludeProviders = 1
		AND p.RecordStatusId = @LiveStatus
		AND (@ContractingBodiesOnly = 0 OR p.IsContractingBody = 1)
		AND ((@SFAFunded = 1 AND p.SFAFunded = 1) OR (@DFEFunded = 1 AND p.DFE1619Funded = 1))	

	UNION

	SELECT
		o.OrganisationId
		, o.Ukprn
		, o.IsContractingBody
		, convert(bit, 0) IsProvider
		, ot.OrganisationTypeName
		, o.OrganisationName
		, o.OrganisationAlias
		, rlp.LegalName
		, NULL LastActivity
		, qs.EarliestModifiedDateTimeUtc ModifiedDateTimeUtc
		, NULL UpToDateConfirmations
		, null ApplicationName
		, null ExpiredLARS
		, null InformationOfficerDisplayName
		, null RelationshipManagerDisplayName
		, null AutoAggregateQualityRating
		, CAST(CASE WHEN P.SFAFunded >= 1 THEN 1 ELSE 0 END AS BIT) AS SFAFunded
		, CAST(CASE WHEN P.DFEFunded >= 1 THEN 1 ELSE 0 END AS BIT) AS DFE1619Funded
		, null PublishData
		, null IsTASOnly
	FROM Organisation o
		INNER JOIN OrganisationType ot on ot.OrganisationTypeId = o.OrganisationTypeId
		LEFT OUTER JOIN Ukrlp rlp on rlp.Ukprn = o.Ukprn
		LEFT OUTER JOIN OrganisationQualityScore qs on qs.OrganisationId = o.OrganisationId
		LEFT OUTER JOIN (SELECT OP.OrganisationId, Sum(CASE WHEN P.SFAFunded = 1 THEN 1 ELSE 0 END) AS SFAFunded, Sum(CASE WHEN P.DFE1619Funded = 1 THEN 1 ELSE 0 END) AS DFEFunded FROM OrganisationProvider OP INNER JOIN Provider P ON P.ProviderId = OP.ProviderId WHERE P.RecordStatusId = @LiveStatus GROUP BY OP.OrganisationId) P ON P.OrganisationId = O.OrganisationId
	WHERE @IncludeOrganisations = 1
		AND o.RecordStatusId = @LiveStatus
		AND (@ContractingBodiesOnly = 0 OR o.IsContractingBody = 1)	
		AND (
				(@SFAFunded = 1 AND o.OrganisationId IN (SELECT DISTINCT OrganisationId FROM OrganisationProvider OP INNER JOIN Provider P ON P.ProviderId = OP.ProviderId WHERE P.RecordStatusId = @LiveStatus AND P.SFAFunded = 1 AND Op.IsAccepted = 1 AND OP.IsRejected = 0))
				OR
				(@DFEFunded = 1 AND o.OrganisationId IN (SELECT DISTINCT OrganisationId FROM OrganisationProvider OP INNER JOIN Provider P ON P.ProviderId = OP.ProviderId WHERE P.RecordStatusId = @LiveStatus AND P.DFE1619Funded = 1 AND Op.IsAccepted = 1 AND OP.IsRejected = 0))
			);

	SELECT 
		p.ProviderId
		, null OrganisationId
		, u.Name
		, u.Email
		, u.PhoneNumber
	FROM Provider p
		INNER JOIN ProviderUser pu on p.ProviderId = pu.ProviderId
		INNER JOIN AspNetUsers u on u.Id = pu.UserId	
	WHERE @IncludeProviders = 1
		AND (@ContractingBodiesOnly = 0 OR p.IsContractingBody = 1)
		AND p.RecordStatusId = @LiveStatus
		AND u.IsDeleted = 0 AND u.EmailConfirmed = 1	
		AND ((@SFAFunded = 1 AND p.SFAFunded = 1) OR (@DFEFunded = 1 AND p.DFE1619Funded = 1))

	UNION

	SELECT 
		null ProviderId
		, o.OrganisationId
		, u.Name
		, u.Email
		, u.PhoneNumber
	FROM Organisation o
		INNER JOIN OrganisationUser ou on o.OrganisationId = ou.OrganisationId
		INNER JOIN AspNetUsers u on u.Id = ou.UserId
	WHERE @IncludeOrganisations = 1
		AND (@ContractingBodiesOnly = 0 OR o.IsContractingBody = 1)
		AND o.RecordStatusId = @LiveStatus
		AND u.IsDeleted = 0 AND u.EmailConfirmed = 1	
		AND (
				(@SFAFunded = 1 AND o.OrganisationId IN (SELECT DISTINCT OrganisationId FROM OrganisationProvider OP INNER JOIN Provider P ON P.ProviderId = OP.ProviderId WHERE P.RecordStatusId = @LiveStatus AND P.SFAFunded = 1 AND Op.IsAccepted = 1 AND OP.IsRejected = 0))
				OR
				(@DFEFunded = 1 AND o.OrganisationId IN (SELECT DISTINCT OrganisationId FROM OrganisationProvider OP INNER JOIN Provider P ON P.ProviderId = OP.ProviderId WHERE P.RecordStatusId = @LiveStatus AND P.DFE1619Funded = 1 AND Op.IsAccepted = 1 AND OP.IsRejected = 0))
			);

	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;
	
	RETURN 0;

END;
