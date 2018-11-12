CREATE PROCEDURE [remote].[CopyOverProviderText]
AS
BEGIN

SET IDENTITY_INSERT [search].[ProviderText] ON

TRUNCATE TABLE [search].[ProviderText]
INSERT INTO [search].[ProviderText] (
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
FROM [remote].[ProviderText]

SET IDENTITY_INSERT [search].[ProviderText] OFF

RETURN 0
END