CREATE PROCEDURE [dbo].[up_ProviderListForCsvExport]
	
AS
/*
*	Name:		[up_ProviderListForCsvExport]
*	System: 	Stored procedure interface module
*	Description:	List all providers that are live in a format expected for the Csv Export
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Education Ltd, 2014
*			All rights reserved.
*
*	$Log:  $
*/

-- This procedure creates the C_PROVIDERS.csv file

BEGIN

	DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);

	SELECT P.ProviderId AS PROVIDER_ID, 
		P.ProviderName AS PROVIDER_NAME,
		P.Ukprn AS UKPRN,
		P.ProviderTypeId AS PROVIDER_TYPE_ID,
		PT.ProviderTypeName AS PROVIDER_TYPE_DESCRIPTION,
		P.Email AS EMAIL,
		P.Website AS WEBSITE,
		P.Telephone AS PHONE,
		P.Fax AS FAX,
		U.TradingName AS PROV_TRADING_NAME,
		U.LegalName AS PROV_LEGAL_NAME,
		P.UPIN AS LSC_SUPPLIER_NO,
		P.ProviderNameAlias AS PROV_ALIAS,
		dbo.GetCsvDateTimeString(P.CreatedDateTimeUtc) AS DATE_CREATED,
		dbo.GetCsvDateTimeString(P.ModifiedDateTimeUtc) AS DATE_UPDATED,
		CASE WHEN P.Loans24Plus = 1 THEN 'Y' ELSE 'N' END AS TTG_FLAG,
		'' AS TQS_FLAG, -- Not used so outputting as null
		'' AS IES_FLAG, -- Not used so outputting as null
		CASE WHEN P.RecordStatusId = @LiveRecordStatusId THEN 'LIVE' ELSE 'DELETED' END AS STATUS, -- We only export live records to CSV so this not strictly necessary but here in case WHERE changes in future
		'NDLPP_' + CAST(ANUModifiedBy.LegacyUserId AS VARCHAR(10)) AS UPDATED_BY,
		'NDLPP_' + CAST(ANUCreatedBy.LegacyUserId AS VARCHAR(10)) AS CREATED_BY,
		A.AddressLine1 AS ADDRESS_1,
		A.AddressLine2 AS ADDRESS_2,
		A.Town AS TOWN,
		A.County AS COUNTY,
		A.Postcode AS POSTCODE,
		'NDLPP' AS SYS_DATA_SOURCE,  -- This data source flag shouldn't matter for the nightly CSV outputs and currently we only export UCAS data
		CASE WHEN P.ModifiedDateTimeUtc IS NOT NULL AND P.ModifiedDateTimeUtc <> P.CreatedDateTimeUtc THEN dbo.GetCsvDateTimeString(P.ModifiedDateTimeUtc) ELSE NULL END AS DATE_UPDATED_COPY_OVER,   -- Note we don't have a copy over dates, as we don't copy a denormalised version to another database, 
		CASE WHEN P.ModifiedDateTimeUtc IS NULL OR P.ModifiedDateTimeUtc = P.CreatedDateTimeUtc THEN dbo.GetCsvDateTimeString(P.CreatedDateTimeUtc) ELSE NULL END AS DATE_CREATED_COPY_OVER, -- so these dates default to the modified and created dates, i.e. changes are visible immediately
		DFE_PT.DfEProviderTypeCode AS DFE_PROVIDER_TYPE_ID,
		DFE_PT.DfEProviderTypeName AS DFE_PROVIDER_TYPE_DESCRIPTION,
		DFE_LA.DfELocalAuthorityCode AS DFE_LOCAL_AUTHORITY_CODE,
		DfE_LA.DfELocalAuthorityName AS DFE_LOCAL_AUTHORITY_DESCRIPTION,
		DfE_Region.DfERegionCode AS DFE_REGION_CODE,
		DfE_Region.DfERegionName AS DFE_REGION_DESCRIPTION,
		DfE_ET.DfEEstablishmentTypeCode AS DFE_ESTABLISHMENT_TYPE_CODE,
		DfE_ET.DfEEstablishmentTypeName AS DFE_ESTABLISHMENT_TYPE_DESCRIPTION
	FROM Provider P
		LEFT OUTER JOIN Ukrlp U ON P.Ukprn = U.Ukprn
		LEFT OUTER JOIN AspNetUsers ANUModifiedBy ON P.ModifiedByUserId = ANUModifiedBy.Id
		LEFT OUTER JOIN AspNetUsers ANUCreatedBy ON P.CreatedByUserId = ANUCreatedBy.Id
		LEFT OUTER JOIN [Address] A ON P.AddressId = A.AddressId
		LEFT OUTER JOIN ProviderType PT ON PT.ProviderTypeId = P.ProviderTypeId
		LEFT OUTER JOIN DfEProviderType DfE_PT ON DfE_PT.DfEProviderTypeId = P.DfEProviderTypeId
		LEFT OUTER JOIN DfELocalAuthority DfE_LA ON DfE_LA.DfELocalAuthorityId = P.DfELocalAuthorityId
		LEFT OUTER JOIN DfERegion DfE_Region ON DfE_Region.DfERegionId = P.DfERegionId
		LEFT OUTER JOIN DfEEstablishmentType DfE_ET ON DfE_ET.DfEEstablishmentTypeId = P.DfEEstablishmentTypeId
	WHERE P.RecordStatusId = @LiveRecordStatusId
		AND P.PublishData = 1
	ORDER BY PROVIDER_NAME;

	IF @@ERROR <> 0
	BEGIN
		RETURN 1;
	END;

	RETURN 0;

END;
GO