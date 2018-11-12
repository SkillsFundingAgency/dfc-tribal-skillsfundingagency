/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[ProviderText]
(
	[ProviderTextId]	INT				NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 16384)
	, [ProviderId]		INT				NULL		/* PROVIDER_ID */
	, [OrganisationId]	INT				NULL		/* PROVIDER_ID */
	-- 100 + 1 + 100 + 1 + 255 + 1 + 255 + 1 
	-- ProviderName + ' ' + ProviderNameAlias + ' ' + LegalName + ' ' + TradingName
	, [SearchText]		NVARCHAR(714)	NOT NULL	/* PROVIDER_SEARCH_TEXT */
	, [ProviderName]	NVARCHAR(200)	NOT NULL	/* PROVIDERNAME */
) WITH (MEMORY_OPTIMIZED = ON)