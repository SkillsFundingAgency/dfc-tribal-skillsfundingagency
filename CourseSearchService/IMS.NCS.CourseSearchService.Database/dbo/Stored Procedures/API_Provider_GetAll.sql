CREATE PROCEDURE [dbo].[API_Provider_GetAll]
(
	@PublicAPI	INT = 1
)
	 
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 

AS 

BEGIN --ATOMIC WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = 'English')

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
	FROM [dbo].[Provider]
	WHERE (@PublicAPI = 0 OR ApplicationId != 3)
		AND ProviderId IN (
							SELECT C.ProviderId
							FROM [dbo].[Course] C
								INNER JOIN [dbo].[CourseInstance] CI ON CI.CourseId = C.CourseId
							WHERE (CI.StartDate IS NULL OR CI.StartDate >= @OneYearAgo)
						  );

END;