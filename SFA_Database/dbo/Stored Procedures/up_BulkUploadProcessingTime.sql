CREATE PROCEDURE [dbo].[up_BulkUploadProcessingTime]
AS
BEGIN
     SELECT [bu].[BulkUploadId], [dbo].[ConvertFileSizeToHumanReadable]([bu].[FileSize]) AS FileSize,
			CASE
				WHEN
					[h2].[CreatedDateTimeUtc] IS NOT NULL AND
					[h3].[CreatedDateTimeUtc] IS NOT NULL AND
					[h4].[CreatedDateTimeUtc] IS NOT NULL
				THEN
					[dbo].[ConvertTimeToHHMMSS](
						DATEDIFF(second, [h1].[CreatedDateTimeUtc], [h2].[CreatedDateTimeUtc])
						+
						DATEDIFF(second, [h3].[CreatedDateTimeUtc], [h4].[CreatedDateTimeUtc]),
						's'
					)
				ELSE
					CASE
						WHEN
							[h2].[CreatedDateTimeUtc] IS NULL AND
							[h3].[CreatedDateTimeUtc] IS NULL AND
							[h4].[CreatedDateTimeUtc] IS NOT NULL
						THEN
							[dbo].[ConvertTimeToHHMMSS](
								DATEDIFF(second, [h1].[CreatedDateTimeUtc], [h4].[CreatedDateTimeUtc]),
								's'
							)
						ELSE 
							'Unpublished'
						END
				END AS 'Total Processing Time',
			CASE
				WHEN
					[p].[ProviderId] IS NOT NULL
				THEN
					'Provider: ' + [p].[ProviderName]
				ELSE
					'Organisation: ' + [o].[OrganisationName]
				END AS 'Uploaded For',
			[bu].[FileSize] AS [Bytes],
	        [h1].[CreatedDateTimeUtc] AS 'Uploaded',
			[h2].[CreatedDateTimeUtc] AS 'Validation Complete',
			[h3].[CreatedDateTimeUtc] AS 'Confirmed',
			[h4].[CreatedDateTimeUtc] AS 'Published'
       FROM [dbo].[BulkUpload] [bu]
  LEFT JOIN [dbo].[BulkUploadStatusHistory] [h1]
         ON [bu].[BulkUploadId] = [h1].[BulkUploadId]
        AND [h1].[BulkUploadStatusId] = (
				SELECT BulkUploadStatusId 
				  FROM [BulkUploadStatus] 
				 WHERE [BulkUploadStatusName] = 'Unvalidated')
  LEFT JOIN [dbo].[BulkUploadStatusHistory] [h2]
         ON [bu].[BulkUploadId] = [h2].[BulkUploadId]
        AND [h2].[BulkUploadStatusId] = (
				SELECT BulkUploadStatusId
				  FROM [BulkUploadStatus]
				 WHERE [BulkUploadStatusName] = 'Needs_Confirmation')
  LEFT JOIN [dbo].[BulkUploadStatusHistory] [h3]
         ON [bu].[BulkUploadId] = [h3].[BulkUploadId]
        AND [h3].[BulkUploadStatusId] = (
				SELECT BulkUploadStatusId
				  FROM [BulkUploadStatus]
				 WHERE [BulkUploadStatusName] = 'ConfirmationReceived')
  LEFT JOIN [dbo].[BulkUploadStatusHistory] [h4]
         ON [bu].[BulkUploadId] = [h4].[BulkUploadId]
        AND [h4].[BulkUploadStatusId] = (
				SELECT BulkUploadStatusId
				  FROM [BulkUploadStatus]
				 WHERE [BulkUploadStatusName] = 'Published')
  LEFT JOIN [dbo].[Provider] [p]
         ON [bu].[UserProviderId] = [p].[ProviderId]
  LEFT JOIN [dbo].[Organisation] [o]
	     ON [bu].[UserOrganisationId] = [o].[OrganisationId]
      WHERE [bu].[FileSize] > 0
   ORDER BY [bu].[BulkUploadId] DESC

END
