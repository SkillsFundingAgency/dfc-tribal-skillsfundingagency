CREATE PROCEDURE [search].[PublishProviderText]
--WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER 
AS BEGIN /* WITH (
      TRANSACTION ISOLATION LEVEL = SNAPSHOT,
      LANGUAGE = 'English') */

	DELETE FROM [dbo].[ProviderText];
	INSERT INTO [dbo].[ProviderText] (
		[ProviderTextId]
		,  [ProviderId]
		,  [OrganisationId]
		,  [SearchText]
		,  [ProviderName])
	SELECT
		[ProviderTextId]
		,  [ProviderId]
		,  [OrganisationId]
		,  [SearchText]
		,  [ProviderName]
	FROM [search].[ProviderText];

	DELETE FROM [dbo].[ProviderFreeText];
	INSERT INTO [dbo].[ProviderFreeText]
	(
		ProviderTextId,
		SearchText
	)
	SELECT ProviderTextId,
		SearchText
	FROM ProviderText;

	  
RETURN 0
END