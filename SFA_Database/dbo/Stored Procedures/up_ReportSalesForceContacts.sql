CREATE PROCEDURE [dbo].[up_ReportSalesForceContacts]
AS

/*
*	Name:		[up_ReportSalesForceContacts]
*	System: 	Stored procedure interface module
*	Description:	List of contacts for import into SalesForce
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
	DECLARE @Users TABLE (UserId nvarchar(128))

	INSERT INTO @Users
	SELECT UserId
	FROM AspNetUserRoles
	WHERE RoleId IN (
		SELECT RoleId
		FROM PermissionInRole pir
	 		INNER JOIN Permission p on p.PermissionId = pir.PermissionId
		WHERE p.PermissionName = 'IsPrimaryContact'
	)
	
	SELECT p.ProviderName AccountName
		, p.SFAFunded
		, p.DFE1619Funded
		, CASE
			WHEN CharIndex(' ', u.Name) = 0 THEN ''
			ELSE SubString(u.Name, 0, CharIndex(' ', u.Name))
		  END FirstName
		, CASE
			WHEN CharIndex(' ', u.Name) = 0 THEN u.Name
			ELSE SubString(u.Name, CharIndex(' ', u.Name)+1, 999)
		  END LastName
		, u.Email
		, Coalesce(a.AddressLine1 + ', ' + AddressLine2, '') MailingStreet
		, Coalesce(a.Town, '')				MailingCity
		, Coalesce(a.County, '')			MailingState
		, Coalesce(a.Postcode, '')			MailingPostalCode
		, Coalesce(u.PhoneNumber, '')		Phone
		, CASE WHEN pc.UserId IS NOT NULL THEN 'Yes' ELSE 'No' END	IsPrimaryContact
		, u.Id								UserExternalId
		, 'P' + Convert(nvarchar(11), P.ProviderId)	AS ExternalId
	FROM ProviderUser pu 
		INNER JOIN AspNetUsers u on pu.UserId = u.Id
		INNER JOIN Provider p on p.ProviderId = pu.ProviderId
		LEFT OUTER JOIN Address a on a.AddressId = u.AddressId
		LEFT OUTER JOIN @Users pc on pc.UserId = u.Id
	WHERE u.IsDeleted = 0 AND u.EmailConfirmed = 1 AND u.PasswordResetRequired = 0 AND p.RecordStatusId = 2

	UNION

	SELECT O.OrganisationName AccountName
		, CAST(CASE WHEN P.SFAFunded >= 1 THEN 1 ELSE 0 END AS BIT) AS SFAFunded
		, CAST(CASE WHEN P.DFEFunded >= 1 THEN 1 ELSE 0 END AS BIT) AS DFE1619Funded
		, CASE
			WHEN CharIndex(' ', u.Name) = 0 THEN NULL
			ELSE SubString(u.Name, 0, CharIndex(' ', u.Name))
		  END FirstName
		, CASE
			WHEN CharIndex(' ', u.Name) = 0 THEN u.Name
			ELSE SubString(u.Name, CharIndex(' ', u.Name)+1, 999)
		  END LastName
		, u.Email
		, Coalesce(a.AddressLine1 + ', ' + AddressLine2, '') MailingStreet
		, Coalesce(a.Town, '')				MailingCity
		, Coalesce(a.County, '')			MailingState
		, Coalesce(a.Postcode, '')			MailingPostalCode
		, Coalesce(u.PhoneNumber, '')		Phone
		, CASE WHEN pc.UserId IS NOT NULL THEN 'Yes' ELSE 'No' END	IsPrimaryContact
		,u.Id						UserExternalId
		, 'O' + Convert(nvarchar(11), o.OrganisationId)	AS ExternalId
	FROM OrganisationUser pu
		INNER JOIN AspNetUsers u ON pu.UserId = u.Id
		INNER JOIN Organisation O ON O.OrganisationId = pu.OrganisationId
		LEFT OUTER JOIN Address a ON a.AddressId = u.AddressId
		LEFT OUTER JOIN (SELECT OP.OrganisationId, Sum(CASE WHEN P.SFAFunded = 1 THEN 1 ELSE 0 END) AS SFAFunded, Sum(CASE WHEN P.DFE1619Funded = 1 THEN 1 ELSE 0 END) AS DFEFunded FROM OrganisationProvider OP INNER JOIN Provider P ON P.ProviderId = OP.ProviderId WHERE P.RecordStatusId = @LiveStatus GROUP BY OP.OrganisationId) P ON P.OrganisationId = O.OrganisationId
		LEFT OUTER JOIN @Users pc on pc.UserId = u.Id
	WHERE u.IsDeleted = 0 AND u.EmailConfirmed = 1 AND u.PasswordResetRequired = 0 AND o.RecordStatusId = 2

	IF (@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;
	
	RETURN 0;

END;
