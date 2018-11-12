CREATE PROCEDURE [dbo].[ProviderRequestResponesLog_Insert]
	@ServiceMethod      NVARCHAR(100),
	@Request			NVARCHAR(max),
	@Response			NVARCHAR(max),
	@TimeInMillisecond	INT
as
BEGIN
	INSERT INTO [dbo].[ProviderRequestResponesLog]
			   ([ServiceMethod]
			   ,[Request]
			   ,[Response]
			   ,[TimeInMilliseconds]
			   ,[PublicAPI]
			   ,[APIKey])
		 VALUES
			   (@ServiceMethod,
			    @Request,
			    @Response,
			    @TimeInMillisecond,
				0,
				null);
END