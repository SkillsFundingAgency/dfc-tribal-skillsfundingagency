CREATE FUNCTION [dbo].[GetProviderQAComplianceFailureReasonsCommaDelimited]
(
	@ProviderQAComplianceId int
)
RETURNS NVARCHAR(4000)
AS
BEGIN
	DECLARE @List VARCHAR(4000)
	SELECT @List = COALESCE(@List+',' ,'') + CAST(cfr.Description AS NVARCHAR(100))
	FROM ProviderQAComplianceFailureReason aqac
		JOIN QAComplianceFailureReason cfr ON cfr.QAComplianceFailureReasonId = aqac.QAComplianceFailureReasonId
	WHERE aqac.ProviderQAComplianceId = @ProviderQAComplianceId
	ORDER BY Ordinal

	RETURN ISNULL(@List, '')
END
