/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[Provider]
(
	[ProviderId] INT NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 16384) /* PROVIDER_ID */
	, [ProviderName]		NVARCHAR(200)	NOT NULL	/* PROVIDER_NAME */
	, [Ukprn]				INT				NOT NULL	/* UKPRN */
	/**/, [ProviderTypeId]	INT				NOT NULL	/* PROVIDER_TYPE_ID */
	, [Email]				NVARCHAR(255)	NULL		/* EMAIL */
	, [Website]				NVARCHAR(511)	NULL		/* WEBSITE */
	, [Telephone]			NVARCHAR(30)	NULL		/* PHONE */
	, [Fax]					NVARCHAR(30)	NULL		/* FAX */
	, [TradingName]			NVARCHAR(255)	NULL		/* PROV_TRADING_NAME */
	, [LegalName]			NVARCHAR(255)	NOT NULL	/* PROV_LEGAL_NAME */
	, [UPIN]				INT				NULL		/* LSC_SUPPLIER_NO */
	, [ProviderNameAlias]	NVARCHAR(100)	NULL		/* PROV_ALIAS */
	, [CreatedDateTimeUtc]	DATETIME		NOT NULL	/* DATE_CREATED */
	, [ModifiedDateTimeUtc]	DATETIME		NULL		/* DATE_UPDATED */
	, [Loans24Plus]			BIT				NOT NULL	/* TTG_FLAG */
	-- []					BIT				NOT NULL	/* TQS_FLAG */ -- Not used so outputting as null
	-- []					BIT				NOT NULL	/* IES_FLAG */ -- Not used so outputting as null
	, [RecordStatusId]		INT				NOT NULL	/* STATUS */ -- We only export live records to CSV so this not strictly necessary but here in case WHERE changes in future
	, [CreatedByUserId]     NVARCHAR(128)	NOT NULL	/* CREATED_BY */
	, [ModifiedByUserId]	NVARCHAR(128)	NULL		/* UPDATED_BY */
	, [AddressLine1]		NVARCHAR(100)	NOT NULL	/* ADDRESS_1 */
	, [AddressLine2]		NVARCHAR(100)	NULL		/* ADDRESS_2 */
	, [Town]				NVARCHAR(75)	NOT NULL	/* TOWN */
	, [County]				NVARCHAR(75)	NULL		/* COUNTY */
	, [Postcode]			NVARCHAR(30)	NOT NULL	/* POSTCODE */
	, [ApplicationId]		INT			NOT NULL	/* SYS_DATA_SOURCE */  -- This data source flag shouldn't matter for the nightly CSV outputs and currently we only export UCAS data
    , [DFE1619Funded]		BIT				NOT NULL DEFAULT 0
    , [FEChoices_LearnerDestination]	FLOAT NULL 
    , [FEChoices_LearnerSatisfaction]	FLOAT NULL 
    , [FEChoices_EmployerSatisfaction]	FLOAT NULL
	--, []					DATETIME NULL			/* DATE_UPDATED_COPY_OVER */
	--, []					DATETIME NULL			/* DATE_CREATED_COPY_OVER */	
, 
    [SFAFunded] BIT NOT NULL DEFAULT 0) WITH (MEMORY_OPTIMIZED = ON)