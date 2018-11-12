CREATE PROCEDURE [dbo].[up_BulkUploadPendingOrganisationUploadForProvider]
(
	@ProviderId INT
)
AS
BEGIN

	SELECT [bup].[BulkUploadId]
	  FROM [dbo].[BulkUploadProvider] [bup]
	  JOIN [dbo].[v_BulkUploadCurrentStatus] [bucs]
		ON [bup].[BulkUploadId] = [bucs].[BulkUploadId]
	  JOIN [dbo].[BulkUploadStatus] [bus]
		ON [bucs].[BulkUploadStatusId] = [bus].[BulkUploadStatusId]
	 WHERE [bus].[BulkUploadStatusName] = 'Needs_Confirmation'
	   AND [bup].[ProviderId] = @ProviderId
	   AND [bucs].[UserOrganisationId] IS NOT NULL

END