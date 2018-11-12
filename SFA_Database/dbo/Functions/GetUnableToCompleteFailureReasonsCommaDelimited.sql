CREATE FUNCTION [dbo].[GetUnableToCompleteFailureReasonsCommaDelimited]
(
	@ProviderUnableToCompleteId int
)
RETURNS NVARCHAR(4000)
AS
BEGIN
	DECLARE @List VARCHAR(4000)
	SELECT @List = COALESCE(@List+',' ,'') + CAST(cfr.Description AS NVARCHAR(400))
	FROM ProviderUnableToCompleteFailureReason aqac
		JOIN UnableToCompleteFailureReason cfr ON cfr.UnableToCompleteFailureReasonId = aqac.UnableToCompleteFailureReasonId
	WHERE aqac.ProviderUnableToCompleteId = @ProviderUnableToCompleteId
	ORDER BY Ordinal

	RETURN ISNULL(@List, '')
END
