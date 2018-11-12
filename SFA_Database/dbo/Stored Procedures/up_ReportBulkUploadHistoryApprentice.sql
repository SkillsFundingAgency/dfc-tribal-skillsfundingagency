CREATE PROCEDURE [dbo].[up_ReportBulkUploadHistoryApprentice]
		@StartDate datetime
		, @EndDate datetime
AS

/*
*	Name:		[up_ReportBulkUploadHistoryApprentice]
*	System: 	Stored procedure interface module
*	Description:	List bulk upload history
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2015
*			All rights reserved.
*
*	$Log:  $
*/

WITH CTE AS (
	SELECT e.BulkUploadId, t.BulkUploadErrorTypeId, Count(*) Items
	FROM BulkUploadExceptionItem e
		JOIN BulkUploadErrorType t on t.BulkUploadErrorTypeId = e.BulkUploadErrorTypeId
	GROUP BY e.BulkUploadId, t.BulkUploadErrorTypeId
)
SELECT
	bu.BulkUploadId
	, h.CreatedDateTimeUtc
	, Coalesce(o.Organisationname, '') OrganisationName
	, Coalesce(p.ProviderName, '') ProviderName
	, u.UserName
	, s.BulkUploadStatusText
	, bu.FileName
	, Coalesce(bu.ExistingApprenticeships, 0) ExistingApprenticeships
	, Coalesce(bu.NewApprenticeships, 0) NewApprenticeships
	, Coalesce(bu.InvalidApprenticeships, 0) InvalidApprenticeships
	, Coalesce(bu.ExistingDeliveryLocations, 0) ExistingDelivLocations
	, Coalesce(bu.NewDeliveryLocations, 0) NewDelivLocations
	, Coalesce(bu.InvalidDeliveryLocations, 0) InvalidDelivLocations
	, Coalesce((SELECT Items FROM CTE WHERE BulkUploadID = h.BulkUploadId AND BulkUploadErrorTypeId = 1), 0) Errors
	, Coalesce((SELECT Items FROM CTE WHERE BulkUploadID = h.BulkUploadId AND BulkUploadErrorTypeId = 2), 0) Warnings
	, Coalesce((SELECT Items FROM CTE WHERE BulkUploadID = h.BulkUploadId AND BulkUploadErrorTypeId = 3), 0) SystemExceptions
	, Coalesce((SELECT Items FROM CTE WHERE BulkUploadID = h.BulkUploadId AND BulkUploadErrorTypeId = 4), 0) Successes
	, Coalesce((SELECT Items FROM CTE WHERE BulkUploadID = h.BulkUploadId AND BulkUploadErrorTypeId = 5), 0) Notices
FROM BulkUpload bu
	JOIN BulkUploadStatusHistory h on bu.BulkUploadId = h.BulkUploadId
	JOIN BulkUploadStatus s on h.BulkUploadStatusId = s.BulkUploadStatusId
	LEFT JOIN BulkUploadProvider bup on bup.BulkUploadId = bu.BulkUploadId
	LEFT JOIN Provider p on p.ProviderId = bu.UserProviderId
	LEFT JOIN Organisation o on o.OrganisationId = bu.UserOrganisationId
	JOIN AspNetUsers u on u.Id = h.LoggedInUserId
WHERE
		(@StartDate IS NULL OR h.CreatedDateTimeUtc >= @StartDate)
		AND (@EndDate IS NULL OR h.CreatedDateTimeUtc <= @EndDate)
		AND (Coalesce(bu.NewApprenticeships, 0) > 0 or
			 Coalesce(bu.InvalidApprenticeships, 0) > 0 or 
			 Coalesce(bu.NewDeliveryLocations, 0) > 0 or 
			 Coalesce(bu.InvalidDeliveryLocations, 0) > 0)
ORDER BY h.CreatedDateTimeUtc DESC

IF(@@ERROR <> 0)
BEGIN
	RETURN 1
END
	
RETURN 0
