CREATE PROCEDURE [search].[CourseInstanceA10Codes]
	
AS

BEGIN

	DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0);

	SELECT 
		CI.CourseInstanceId AS OPPORTUNITY_ID,
		CAST(CIAFC.A10FundingCode AS VARCHAR(10)) AS A10_CODE
	FROM dbo.CourseInstance CI
		JOIN dbo.CourseInstanceA10FundingCode CIAFC ON CI.CourseInstanceId = CIAFC.CourseInstanceId
		JOIN dbo.Course C on C.CourseId = CI.CourseId
		JOIN dbo.Provider P on P.ProviderId = C.ProviderId
	WHERE CI.RecordStatusId = @LiveRecordStatusId
		AND P.RecordStatusId = @LiveRecordStatusId
		AND P.PublishData = 1

	UNION ALL

	-- Output N/As, we find these by a join which have no A10 codes, we don't explicitly have an NA in this schema,
	-- just having no A10 codes means NA
	SELECT 
		CI.CourseInstanceId AS OPPORTUNITY_ID,
		'NA' AS A10_CODE	
	FROM dbo.CourseInstance CI
		LEFT OUTER JOIN dbo.CourseInstanceA10FundingCode CIAFC ON CI.CourseInstanceId = CIAFC.CourseInstanceId
		JOIN dbo.Course C on C.CourseId = CI.CourseId
		JOIN dbo.Provider P on P.ProviderId = C.ProviderId
	WHERE CIAFC.A10FundingCode IS NULL 
		AND CI.RecordStatusId = @LiveRecordStatusId
		AND P.RecordStatusId = @LiveRecordStatusId
		AND P.PublishData = 1

	IF @@ERROR <> 0
	BEGIN
		RETURN 1;
	END;

	RETURN 0;

END;