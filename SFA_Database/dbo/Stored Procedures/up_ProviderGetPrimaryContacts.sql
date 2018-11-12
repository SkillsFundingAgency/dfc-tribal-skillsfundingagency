CREATE PROCEDURE [dbo].[up_ProviderGetPrimaryContacts]
		@ProviderId int
AS

/*
*	Name:		[up_ProviderGetPrimaryContacts]
*	System: 	Stored procedure interface module
*	Description:	List primary contacts for a provider
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
	u.Name
	, u.Email
	, u.PhoneNumber
FROM Provider p
	JOIN ProviderUser pu on pu.ProviderId = p.ProviderId
	JOIN AspNetUsers u on pu.UserId = u.Id
	JOIN AspNetUserRoles ur on ur.UserId = u.Id
	JOIN PermissionInRole pir on pir.RoleId = ur.RoleId
	JOIN Permission pm on pm.PermissionId = pir.PermissionId
WHERE
	p.ProviderId = @ProviderId
	AND p.RecordStatusId = @LiveStatus
	AND u.EmailConfirmed = 1
	AND u.IsDeleted = 0
	AND pm.PermissionName = 'IsPrimaryContact'
ORDER BY u.CreatedDateTimeUtc

IF(@@ERROR <> 0)
BEGIN
	RETURN 1
END
	
RETURN 0
