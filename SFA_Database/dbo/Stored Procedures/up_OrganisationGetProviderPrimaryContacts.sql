CREATE PROCEDURE [dbo].[up_OrganisationGetProviderPrimaryContacts]
		@OrganisationId int
AS

/*
*	Name:		[up_OrganisationGetProviderPrimaryContacts]
*	System: 	Stored procedure interface module
*	Description:	List primary contacts for an organisation
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
	, u.Name
	, u.Email
	, u.PhoneNumber
FROM OrganisationProvider op
	JOIN Provider p on p.ProviderId = op.ProviderId
	JOIN ProviderUser pu on pu.ProviderId = p.ProviderId
	JOIN AspNetUsers u on pu.UserId = u.Id
	JOIN AspNetUserRoles ur on ur.UserId = u.Id
	JOIN PermissionInRole pir on pir.RoleId = ur.RoleId
	JOIN Permission pm on pm.PermissionId = pir.PermissionId
WHERE
	op.IsAccepted = 1 AND op.IsRejected = 0
	AND p.RecordStatusId = @LiveStatus
	AND op.OrganisationId = @OrganisationId
	AND u.EmailConfirmed = 1
	AND u.IsDeleted = 0
	AND pm.PermissionName = 'IsPrimaryContact'
ORDER BY u.CreatedDateTimeUtc

IF(@@ERROR <> 0)
BEGIN
	RETURN 1
END
	
RETURN 0
