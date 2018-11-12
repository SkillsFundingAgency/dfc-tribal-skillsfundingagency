CREATE PROCEDURE [dbo].[up_BulkUploadMigration]
AS
BEGIN

	DECLARE @FilePath VARCHAR(500)
	DECLARE @BulkUploadId INT

	DECLARE bu_cursor CURSOR FOR
		SELECT [buh].[FilePath]
		  FROM [dbo].[BulkUploadHistory] [buh]
	 LEFT JOIN [dbo].[BulkUpload] [bu]
			ON [bu].[FilePath] = [buh].[FilePath]
		 WHERE [bu].[BulkUploadId] IS NULL
	  GROUP BY [buh].[FilePath]

	OPEN bu_cursor
	FETCH NEXT FROM bu_cursor INTO @FilePath

	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		INSERT INTO [dbo].[BulkUpload] 
			([UserProviderId], [UserOrganisationId], [FileName], 
			 [FilePath], [ExistingCourses], [NewCourses], [InvalidCourses],
			 [ExistingVenues], [NewVenues], [InvalidVenues], [ExistingOpportunities], 
			 [NewOpportunities], [InvalidOpportunities])
		SELECT TOP 1 [UserProviderId], [UserOrganisationId], [FileName], [FilePath], 
		             [ExistingCourses], [NewCourses], [InvalidCourses], 
					 [ExistingVenues], [NewVenues], [InvalidVenues], 
					 [ExistingOpportunities], [NewOpportunities], [InvalidOpportunities]
		  FROM [dbo].[BulkUploadHistory]
		 WHERE [FilePath] = @FilePath

		SET @BulkUploadId = SCOPE_IDENTITY()

		INSERT INTO [dbo].[BulkUploadStatusHistory]
			([BulkUploadId], [BulkUploadStatusId], [CreatedDateTimeUtc], [LoggedInUserId])
		SELECT @BulkUploadId, [BulkUploadStatusId], [CreatedDateTimeUtc], [LogedInUserId]
		  FROM [dbo].[BulkUploadHistory]
		 WHERE [FilePath] = @FilePath

		INSERT INTO [dbo].[BulkUploadProvider]
			([BulkUploadId], [ProviderId], [IsAuthorisedUpload])
		SELECT @BulkUploadId, [buhp].[ProviderId], [buhp].[IsAuthorisedUpload]
		  FROM [dbo].[BulkUploadHistoryProvider] [buhp]
		 WHERE [BulkUploadHistoryId] = (
			SELECT TOP 1 [BulkUploadHistoryId] 
			  FROM [dbo].[BulkUploadHistory] 
			 WHERE [FilePath] = @FilePath)

		INSERT INTO [dbo].[BulkUploadExceptionItem]
			([BulkUploadId], [BulkUploadSectionId], [ProviderId], [BulkUploadErrorTypeId], [LineNumber],
			 [ColumnName], [ColumnValue], [Details], [CreatedDateTimeUtc])
		SELECT @BulkUploadId, [bue].[BulkUploadSectionId], [bue].[ProviderId],
		       [bue].[BulkUploadErrorTypeId], [bue].[LineNumber], [bue].[ColumnName],
			   [bue].[ColumnValue], [bue].[Details], [bue].[CreatedDateTimeUtc]
		  FROM [dbo].[BulkUploadException] [bue]
		 WHERE [BulkUploadHistoryId] = (
			SELECT TOP 1 [BulkUploadHistoryId] 
			  FROM [dbo].[BulkUploadHistory] 
			 WHERE [FilePath] = @FilePath)
		 
		FETCH NEXT FROM bu_cursor INTO @FilePath
	END

	CLOSE bu_cursor
	DEALLOCATE bu_cursor

END