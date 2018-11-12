CREATE FUNCTION [dbo].[GetProviderQAStyleFailureReasonsCommaDelimited]
(
	@ProviderQAStyleId int
)
RETURNS NVARCHAR(4000)
AS
BEGIN
	DECLARE @List VARCHAR(4000)
	SELECT @List = COALESCE(@List+',' ,'') + CAST(sfr.Description AS NVARCHAR(100))
	FROM ProviderQAStyleFailureReason aqas
		JOIN QAStyleFailureReason sfr ON sfr.QAStyleFailureReasonId = aqas.QAStyleFailureReasonId
	WHERE aqas.ProviderQAStyleId = @ProviderQAStyleId
	ORDER BY Ordinal

	RETURN ISNULL(@List, '')
END
