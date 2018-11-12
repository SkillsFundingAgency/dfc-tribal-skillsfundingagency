CREATE PROCEDURE [dbo].[up_GetProviderUsers]
	@ProviderIds [dbo].[IntList] readonly
	, @IncludeNormalUsers bit
	, @IncludeSuperUsers bit
AS

/*
*	Name:		[up_GetProviderUsers]
*	System: 	Stored procedure interface module
*	Description:	Get a list of provider users
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

DECLARE @Live int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1)

SELECT
	DISTINCT  
	pu.ProviderId
	, p.ProviderName
	, u.Name
	, u.Email
	, Coalesce(ipc.IsPrimaryContact, 0) IsPrimaryContact
FROM AspNetUsers u
	JOIN ProviderUser pu on pu.UserId = u.Id
	JOIN Provider p on p.ProviderId = pu.ProviderId
	OUTER APPLY (
		SELECT
			CASE WHEN pm.PermissionId IS NULL THEN 0 ELSE 1 END IsPrimaryContact
		FROM ProviderUser pu
			JOIN AspNetUserRoles r on u.Id = r.UserId
			JOIN PermissionInRole pir on pir.Roleid = r.RoleId
			JOIN Permission pm on pm.PermissionId = pir.permissionid
		WHERE
			pu.UserId = u.Id
			AND pm.PermissionName = 'IsPrimaryContact'
	) ipc
WHERE pu.ProviderId IN (SELECT * FROM @ProviderIds)
	AND ((ipc.IsPrimaryContact = 1 AND @IncludeSuperUsers = 1)
			OR (ipc.IsPrimaryContact IS NULL AND @IncludeNormalUsers = 1))
	AND u.IsDeleted = 0
	AND p.RecordStatusId = @Live

IF(@@ERROR <> 0)
BEGIN
	RETURN 1
END
	
RETURN 0

