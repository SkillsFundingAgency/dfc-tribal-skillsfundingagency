CREATE PROCEDURE [dbo].[up_BulkUploadConfirmedQueueNext]
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @BulkUploadId		INT					= 0
	DECLARE @RowCount			INT					= 0
	DECLARE @ProcessingToken	UNIQUEIDENTIFIER	= NEWID()
	DECLARE @UserId				VARCHAR(100)		= NULL
		
	UPDATE t
		SET t.ProcessingToken = @ProcessingToken
	FROM (
  		SELECT TOP 1
			[bucs].[ProcessingToken]
		FROM	[dbo].[v_BulkUploadCurrentStatus] [bucs]
			JOIN	[dbo].[BulkUploadStatus] [bus]
				ON	[bucs].[BulkUploadStatusId] = [bus].[BulkUploadStatusId]
				AND	[bus].[BulkUploadStatusName] = 'ConfirmationReceived'
		 WHERE	[bucs].[ProcessingToken] IS NULL
		 ORDER BY	[bucs].[CreatedDateTimeUtc] ASC
		) t

	SET @RowCount = @@ROWCOUNT

	IF @RowCount = 1
	BEGIN
		SELECT @UserId = LoggedInUserId, @BulkUploadId = BulkUploadId, @ProcessingToken = ProcessingToken
		FROM v_BulkUploadCurrentStatus
		WHERE ProcessingToken = @ProcessingToken
	END
	ELSE
	BEGIN
		SET @ProcessingToken = NULL
	END

	SELECT @BulkUploadId AS BulkUploadId, @ProcessingToken AS ProcessingToken, @UserId AS UserId
	
	IF (@@ERROR <> 0)
	BEGIN
		RETURN 1
	END
	RETURN 0

END