CREATE PROCEDURE [dbo].[up_ReportProviderVenues]
		@ProviderId int
AS

/*
*	Name:		[up_ReportProviderVenues]
*	System: 	Stored procedure interface module
*	Description:	List provider courses
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

DECLARE @DeletedStatus int = (
	SELECT RecordStatusId FROM RecordStatus WHERE IsDeleted = 1
)

SELECT
	rs.RecordStatusName
	,v.ProviderOwnVenueRef
	,v.VenueName
	,a.AddressLine1
	,a.AddressLine2
	,a.Town
	,a.County
	,a.Postcode
FROM
	Venue v
	JOIN RecordStatus rs on rs.RecordStatusId = v.RecordStatusId
	LEFT JOIN Address a on a.AddressId = v.AddressId
WHERE v.ProviderId = @ProviderId
	AND v.RecordStatusId != @DeletedStatus

IF(@@ERROR <> 0)
BEGIN
	RETURN 1
END
	
RETURN 0

