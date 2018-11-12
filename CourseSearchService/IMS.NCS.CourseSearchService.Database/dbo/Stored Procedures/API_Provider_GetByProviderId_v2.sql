CREATE PROCEDURE [dbo].[API_Provider_GetByProviderId_v2]
(
	@ProviderId		INT,
	@PublicAPI		INT = 1,
	@APIKey			NVARCHAR(50) = NULL
)
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 

AS BEGIN --ATOMIC WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = 'English')
	 
	-- If this is the public API then ensure that we have a valid API Key
	IF ([dbo].[IsValidAPIKey](@PublicAPI, @APIKey) = 0)
	BEGIN
		RETURN 0;
	END;

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
  WHERE ProviderId=@ProviderId
	AND (@PublicAPI = 0 OR ApplicationId != 3);

END;