CREATE PROCEDURE [dbo].[API_Provider_GetAll_v2]
(
	@PublicAPI	INT = 1,
	@APIKey		NVARCHAR(50) = NULL
)
	 
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 

AS 

BEGIN --ATOMIC WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = 'English')
	 
	-- If this is the public API then ensure that we have a valid API Key
	IF ([dbo].[IsValidAPIKey](@PublicAPI, @APIKey) = 0)
	BEGIN
		RETURN 0;
	END;

	DECLARE @OneYearAgo DATE = CAST(DATEADD(yyyy, -1, GetUtcDate()) AS DATE);

	SELECT [ProviderId]
		  ,[ProviderName]
		  ,[Ukprn]
		  ,[ProviderTypeId]
		  ,[Email]
		  ,[Website]
		  ,[Telephone]
		  ,[Fax]
		  ,[TradingName]
		  ,[LegalName]
		  ,[UPIN]
		  ,[ProviderNameAlias]
		  ,[CreatedDateTimeUtc]
		  ,[ModifiedDateTimeUtc]
		  ,[Loans24Plus]
		  ,[RecordStatusId]
		  ,[CreatedByUserId]
		  ,[ModifiedByUserId]
		  ,[AddressLine1]
		  ,[AddressLine2]
		  ,[Town]
		  ,[County]
		  ,[Postcode]
		  ,[ApplicationId]
		  ,[DFE1619Funded]
	FROM [dbo].[Provider]
	WHERE (@PublicAPI = 0 OR ApplicationId != 3)
		AND ProviderId IN (
							SELECT C.ProviderId
							FROM [dbo].[Course] C
								INNER JOIN [dbo].[CourseInstance] CI ON CI.CourseId = C.CourseId
							WHERE (CI.StartDate IS NULL OR CI.StartDate >= @OneYearAgo)
						  );

END;