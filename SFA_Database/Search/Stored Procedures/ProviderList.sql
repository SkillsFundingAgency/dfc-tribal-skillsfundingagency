CREATE PROCEDURE [Search].[ProviderList]
AS

DECLARE 
	@LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0)

SELECT 
	P.ProviderId	AS PROVIDER_ID, 
	P.ProviderName	AS PROVIDER_NAME,
	P.Ukprn			AS UKPRN,
	P.ProviderTypeId AS PROVIDER_TYPE_ID,
	P.Email			AS EMAIL,
	dbo.AppendTrackingUrl(P.Website, P.ProviderTrackingUrl)	AS WEBSITE,
	P.Telephone		AS PHONE,
	P.Fax			AS FAX,
	U.TradingName	AS PROV_TRADING_NAME,
	U.LegalName		AS PROV_LEGAL_NAME,
	P.UPIN			AS LSC_SUPPLIER_NO,
	P.ProviderNameAlias AS PROV_ALIAS,
	dbo.GetCsvDateTimeString(P.CreatedDateTimeUtc) AS DATE_CREATED,
	dbo.GetCsvDateTimeString(P.ModifiedDateTimeUtc) AS DATE_UPDATED,
	P.Loans24Plus,
	P.RecordStatusId AS STATUS, -- We only export live records to CSV so this not strictly necessary but here in case WHERE changes in future
	'NDLPP_' + CAST(ANUCreatedBy.LegacyUserId AS VARCHAR(10)) AS CREATED_BY,
	'NDLPP_' + CAST(ANUModifiedBy.LegacyUserId AS VARCHAR(10)) AS UPDATED_BY,
	A.AddressLine1 AS ADDRESS_1,
	A.AddressLine2 AS ADDRESS_2,
	A.Town AS TOWN,
	A.County AS COUNTY,
	A.Postcode AS POSTCODE,
	1 AS SYS_DATA_SOURCE,   -- This data source flag shouldn't matter for the nightly CSV outputs and currently we only export UCAS data
	P.DFE1619Funded,
	P.SFAFunded,
	FE.LearnerDestination,
	FE.LearnerSatisfaction,
	FE.EmployerSatisfaction
FROM 
	dbo.Provider P
		LEFT OUTER JOIN dbo.Ukrlp U ON P.Ukprn = U.Ukprn
		LEFT OUTER JOIN dbo.AspNetUsers ANUModifiedBy ON P.ModifiedByUserId = ANUModifiedBy.Id
		LEFT OUTER JOIN dbo.AspNetUsers ANUCreatedBy ON P.CreatedByUserId = ANUCreatedBy.Id
		LEFT OUTER JOIN dbo.Address A ON P.AddressId = A.AddressId
		LEFT OUTER JOIN dbo.FEChoices FE ON FE.UPIN = P.UPIN
WHERE 
	P.RecordStatusId = @LiveRecordStatusId
	AND P.PublishData = 1
ORDER BY 
	PROVIDER_NAME

IF @@ERROR <> 0
BEGIN
	RETURN 1
END

RETURN 0