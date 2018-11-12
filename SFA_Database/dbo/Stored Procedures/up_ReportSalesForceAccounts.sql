CREATE PROCEDURE [dbo].[up_ReportSalesForceAccounts]
AS

/*
*	Name:		[up_ReportSalesForceAccounts]
*	System: 	Stored procedure interface module
*	Description:	List of providers and organisations for import into SalesForce
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/
BEGIN

	DECLARE @DefaultDate DATE = CAST('01 JAN 2000' AS DATE);

	SELECT
		p.ProviderName					Name
		, p.DFE1619Funded				DFE1619Funded
		, p.SFAFunded					SFAFunded
		, pt.ProviderTypeName			Type
		, Coalesce(a.AddressLine1 + Coalesce(', ' + AddressLine2, ''), '') BillingStreet
		, Coalesce(a.Town, '')			BillingCity
		, Coalesce(a.County, '')		BillingState
		, Coalesce(a.Postcode, '')		BillingPostalCode
		, Coalesce(p.Telephone, '')		Phone
		, Coalesce(p.Fax, '')			Fax
		, Coalesce(p.Website, '')		Website
		, p.Ukprn						UKPRN
		, Coalesce(pr.RegionName, '')	Region
		, 'P' + Convert(nvarchar(11), p.ProviderId)	ExternalId
		, CASE 
			WHEN qs.AutoAggregateQualityRating >= 91 THEN 'Very Good'
			WHEN qs.AutoAggregateQualityRating >= 71 THEN 'Good'
			WHEN qs.AutoAggregateQualityRating >= 51 THEN 'Average'
			ELSE 'Poor'
		  END							QualityStatus
		, Coalesce(u.Name, '')			PrimaryContact
		, Coalesce(u.Email, '')			PrimaryContactEmail
		, qs.ModifiedDateTimeUtc		DatePortalLastUpdated
		, Coalesce(pti.DfEProviderTypeName, '') DfEProviderType
		, Coalesce(pts.DfEProviderStatusName, '') DfEProviderStatus
		, Coalesce(la.DfELocalAuthorityName, '') DfELocalAuthority
		, Coalesce(re.DfeRegionName, '') DfeRegion
		, Coalesce(est.DfEEstablishmentTypeName, '') DfEEstablishmentType
		, qel.LastQualityEmailName		LastQualityEmailName
		, qel.LastQualityEmailDate		LastQualityEmailDate
		, qel.NextQualityEmailName		NextQualityEmailName
		, qel.NextQualityEmailDate		NextQualityEmailDate
		, qel.QualityEmailSent			QualityEmailSent
		, qel.QualityEmailsPaused		QualityEmailsPaused
		, qs.AutoAggregateQualityRating	AutoAggregateQualityRating
		, CASE dbo.GetTrafficStatus(CASE WHEN qs.ModifiedDateTimeUtc > COALESCE(QS.LastActivity, @DefaultDate) THEN QS.ModifiedDateTimeUtc ELSE QS.LastActivity END, p.SFAFunded, p.DFE1619Funded)
			WHEN 1 THEN 'Red'
			WHEN 2 THEN 'Amber'
			WHEN 3 THEN 'Green'
			ELSE ''
		  END							TrafficLightStatus
	FROM
		Provider p
		JOIN ProviderType pt on pt.ProviderTypeId = p.ProviderTypeId
		JOIN Address a on p.AddressId = a.AddressId
		JOIN RecordStatus rs on rs.RecordStatusId = p.RecordStatusId
		LEFT JOIN DfEProviderType pti on pti.DfEProviderTypeId = p.DfeProviderTypeId
		LEFT JOIN DfEProviderStatus pts on pts.DfEProviderStatusId = p.DfEProviderStatusId
		LEFT JOIN DfELocalAuthority la on la.DfELocalAuthorityId = p.DfELocalAuthorityId
		LEFT JOIN DfERegion re on re.DfeRegionId = p.DfeRegionId
		LEFT JOIN DfEEstablishmentType est on est.DfEEstablishmentTypeId = p.DfeEstablishmentTypeId
		LEFT JOIN ProviderRegion pr on pr.ProviderRegionId = p.ProviderRegionId
		LEFT JOIN QualityScore qs on qs.ProviderId = p.ProviderId
		OUTER APPLY (
			SELECT
				TOP 1  
				pu.ProviderId
				, u.Name
				, u.Email
			FROM AspNetUsers u
				JOIN ProviderUser pu on pu.UserId = u.Id
				JOIN AspNetUserRoles r on u.Id = r.UserId
				JOIN PermissionInRole pir on pir.Roleid = r.RoleId
				JOIN Permission pm on pm.PermissionId = pir.permissionid
				LEFT JOIN Address a on a.AddressId = u.AddressId
			WHERE
				pm.PermissionName = 'IsPrimaryContact'
				AND pu.ProviderId = p.ProviderId
				AND U.IsDeleted <> 1
		) u
		OUTER APPLY (
			SELECT TOP 1
				qet1.Name					LastQualityEmailName
				, _qel.EmailDateTimeUtc		LastQualityEmailDate
				, qet2.Name					NextQualityEmailName
				, _qel.NextEmailDateTimeUtc NextQualityEmailDate
				, _qel.HasValidRecipients	QualityEmailSent
				, _qel.QualityEmailsPaused	QualityEmailsPaused
			FROM QualityEmailLog _qel
				LEFT JOIN EmailTemplate qet1 on qet1.EmailTemplateId = _qel.EmailTemplateId
				LEFT JOIN EmailTemplate qet2 on qet2.EmailTemplateId = _qel.NextEmailTemplateId
			WHERE _qel.ProviderId = p.ProviderId
			ORDER BY _qel.EmailDateTimeUtc DESC
		) qel
	WHERE rs.IsPublished = 1
	UNION
	SELECT
		o.OrganisationName			Name
		, 0							DFE1619Funded
		, 0							SFAFunded
		, ot.OrganisationTypeName	Type
		, Coalesce(a.AddressLine1 + ', ' + AddressLine2, '') BillingStreet
		, Coalesce(a.Town, '')		BillingCity
		, Coalesce(a.County, '')	BillingState
		, Coalesce(a.Postcode, '')	BillingPostalCode
		, Coalesce(o.Phone, '')		Phone
		, Coalesce(o.Fax, '')		Fax
		, Coalesce(o.Website, '')	Website
		, o.Ukprn					UKPRN
		, ''						Region
		, 'O' + Convert(nvarchar(11), o.OrganisationId)	ExternalId
		, ''						QualityStatus
		--,	Funding_value
		, Coalesce(u.Name, '')		PrimaryContact
		, Coalesce(u.Email, '')		PrimaryContactEmail
		, NULL						DatePortalLastUpdated
		, ''						DfEProviderType
		, ''						DfEProviderStatus
		, ''						DfELocalAuthority
		, ''						DfeRegion
		, ''						DfEEstablishmentType
		, ''						LastQualityEmailName
		, NULL						LastQualityEmailDate
		, ''						NextQualityEmailName
		, NULL						NextQualityEmailDate
		, NULL						QualityEmailSent
		, NULL						QualityEmailsPaused
		, NULL						AutoAggregateQualityRating
		, NULL						TrafficLightStatusId
	FROM
		Organisation o
		JOIN OrganisationType ot on ot.OrganisationTypeId = o.OrganisationTypeId
		JOIN Address a on o.AddressId = a.AddressId
		JOIN RecordStatus rs on rs.RecordStatusId = o.RecordStatusId
		JOIN OrganisationQualityScore qs on o.OrganisationId = qs.OrganisationId
		OUTER APPLY (
			SELECT
				TOP 1  
				o.OrganisationId
				, u.Name
				, u.Email
			FROM AspNetUsers u
				JOIN OrganisationUser pu on pu.UserId = u.Id
				JOIN AspNetUserRoles r on u.Id = r.UserId
				JOIN PermissionInRole pir on pir.Roleid = r.RoleId
				JOIN Permission pm on pm.PermissionId = pir.permissionid
				LEFT JOIN Address a on a.AddressId = u.AddressId
			WHERE
				pm.PermissionName = 'IsPrimaryContact'
				AND pu.OrganisationId = o.OrganisationId
				AND U.IsDeleted <> 1
		) u
	WHERE rs.IsPublished = 1
	ORDER BY Name;

	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;
	
	RETURN 0;

END;
