CREATE PROCEDURE [dbo].[up_GetOrganisationUsers]
	@OrganisationIds [dbo].[IntList] readonly
	, @IncludeNormalUsers bit
	, @IncludeSuperUsers bit
AS

/*
*	Name:		[up_GetOrganisationUsers]
*	System: 	Stored procedure interface module
*	Description:	Get a list of organisation users
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
	ou.OrganisationId
	, o.OrganisationName
	, u.Name
	, u.Email
	, Coalesce(ipc.IsPrimaryContact, 0) IsPrimaryContact
FROM AspNetUsers u
	JOIN OrganisationUser ou on ou.UserId = u.Id
	JOIN Organisation o on o.OrganisationId = ou.OrganisationId
	OUTER APPLY (
		SELECT
			CASE WHEN pm.PermissionId IS NULL THEN 0 ELSE 1 END IsPrimaryContact
		FROM OrganisationUser ou
			JOIN AspNetUserRoles r on u.Id = r.UserId
			JOIN PermissionInRole pir on pir.Roleid = r.RoleId
			JOIN Permission pm on pm.PermissionId = pir.permissionid
		WHERE
			ou.UserId = u.Id
			AND pm.PermissionName = 'IsPrimaryContact'
	) ipc
WHERE ou.OrganisationId IN (SELECT * FROM @OrganisationIds)
	AND ((ipc.IsPrimaryContact = 1 AND @IncludeSuperUsers = 1)
			OR (ipc.IsPrimaryContact IS NULL AND @IncludeNormalUsers = 1))
	AND u.IsDeleted = 0
	AND o.RecordStatusId = @Live

IF(@@ERROR <> 0)
BEGIN
	RETURN 1
END
	
RETURN 0

