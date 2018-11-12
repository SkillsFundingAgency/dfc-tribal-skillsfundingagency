CREATE PROCEDURE [search].[DAS_DeliveryModeList]

AS

BEGIN

	DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);

	SELECT DeliveryModeId,
		DASRef
	FROM dbo.DeliveryMode
	WHERE RecordStatusId = @LiveRecordStatusId;

END;