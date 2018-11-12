CREATE procedure [dbo].[ProviderRequestResponesLog_Insert_v2]
	@ServiceMethod      NVARCHAR(100),
	@Request			NVARCHAR(max),
	@Response			NVARCHAR(max),
	@TimeInMillisecond	INT,
	@PublicAPI			BIT,
	@APIKey				NVARCHAR(50),
	@RecordCount		INT
as
BEGIN
	INSERT INTO [dbo].[ProviderRequestResponesLog]
			   ([ServiceMethod]
			   ,[Request]
			   ,[Response]
			   ,[TimeInMilliseconds]
			   ,[PublicAPI]
			   ,[APIKey]
			   ,[RecordCount])
		 VALUES
			   (@ServiceMethod,
			    @Request,
			    @Response,
			    @TimeInMillisecond,
				@PublicAPI,
				@APIKey,
				@RecordCount)
END