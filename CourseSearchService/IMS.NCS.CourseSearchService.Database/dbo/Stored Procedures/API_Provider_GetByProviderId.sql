CREATE PROCEDURE [dbo].[API_Provider_GetByProviderId]
(
	@ProviderId INT,
	@PublicAPI	INT = 1
)
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 

AS BEGIN --ATOMIC WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = 'English')
	 
SELECT 
	   [ProviderId]
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
  FROM 
	[dbo].[Provider]
  WHERE ProviderId=@ProviderId
	AND (@PublicAPI = 0 OR ApplicationId != 3)
 

END