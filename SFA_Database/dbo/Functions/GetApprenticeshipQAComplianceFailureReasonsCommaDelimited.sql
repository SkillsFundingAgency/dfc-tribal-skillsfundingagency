CREATE FUNCTION [dbo].[GetApprenticeshipQAComplianceFailureReasonsCommaDelimited]
(
	@ApprenticeshipQAComplianceId int
)
RETURNS NVARCHAR(4000)
AS
BEGIN
	DECLARE @List VARCHAR(4000)
	SELECT @List = COALESCE(@List+',' ,'') + CAST(cfr.Description AS NVARCHAR(100))
	FROM ApprenticeshipQAComplianceFailureReason aqac
		JOIN QAComplianceFailureReason cfr ON cfr.QAComplianceFailureReasonId = aqac.QAComplianceFailureReasonId
	WHERE aqac.ApprenticeshipQAComplianceId = @ApprenticeshipQAComplianceId
	ORDER BY Ordinal

	RETURN ISNULL(@List, '')
END
