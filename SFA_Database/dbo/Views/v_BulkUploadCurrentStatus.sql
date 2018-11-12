CREATE VIEW [dbo].[v_BulkUploadCurrentStatus]
AS

	SELECT	[bu].[BulkUploadId], [bu].[ProcessingToken], [bu].[FileName],
	        [bu].[FilePath], [bu].[FileSize], [bu].[UserProviderId], [bu].[UserOrganisationId],
			[bush].[BulkUploadStatusHistoryId], [bush].[BulkUploadStatusId],
			[bush].[CreatedDateTimeUtc], [bush].[LoggedInUserId],
			[ExceptionItemCount].[NumberOfExceptions],
			[ProviderCount].[NumberOfProviders]
	  FROM	[dbo].[BulkUpload] [bu]
	  JOIN	(	
				SELECT		[BulkUploadId], MAX(CreatedDateTimeUtc) MaxDate
				FROM		[dbo].[BulkUploadStatusHistory]
				GROUP BY	[BulkUploadId]
			) AS [MostRecent]
	    ON	[bu].BulkUploadId = [MostRecent].[BulkUploadId]
	  JOIN	[dbo].[BulkUploadStatusHistory] [bush]
	    ON	[MostRecent].[BulkUploadId] = [bush].[BulkUploadId]
	   AND	[MostRecent].MaxDate = [bush].[CreatedDateTimeUtc]
 LEFT JOIN	(
				SELECT		[BulkUploadId], COUNT([BulkUploadExceptionItemID]) 'NumberOfExceptions'
				FROM		[dbo].[BulkUploadExceptionItem]
			    GROUP BY	[BulkUploadId]
			) [ExceptionItemCount]
	    ON	[ExceptionItemCount].[BulkUploadId] = [bu].[BulkUploadId]
 LEFT JOIN	(
				SELECT		[BulkUploadId], COUNT(ProviderId) AS 'NumberOfProviders'
				FROM		[BulkUploadProvider]
				GROUP BY	[BulkUploadId]
			) AS [ProviderCount]
		ON	[ProviderCount].[BulkUploadId] = [bu].[BulkUploadId]