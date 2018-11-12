CREATE PROCEDURE [dbo].[up_CourseInstanceA10CodesForCsvExport]
	
AS
/*
*	Name:		[up_CourseInstanceA10CodesForCsvExport]
*	System: 	Stored procedure interface module
*	Description:	List all A10 codes that are live in a format expected for the Csv Export
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Education Ltd, 2014
*			All rights reserved.
*
*	$Log:  $
*/

-- This procedure creates the O_OPP_A10.csv file

DECLARE @LiveRecordStatusId INT = (SELECT RecordStatusId FROM RecordStatus RS WHERE RS.IsPublished = 1 AND RS.IsArchived = 0 AND RS.IsDeleted = 0)

SELECT 
	CI.CourseInstanceId AS OPPORTUNITY_ID,
	CAST(CIAFC.A10FundingCode AS VARCHAR(10)) AS A10_CODE
FROM CourseInstance CI
	JOIN CourseInstanceA10FundingCode CIAFC ON CI.CourseInstanceId = CIAFC.CourseInstanceId
	JOIN Course C on C.CourseId = CI.CourseId
	JOIN Provider P on P.ProviderId = C.ProviderId
WHERE CI.RecordStatusId = @LiveRecordStatusId
	AND P.RecordStatusId = @LiveRecordStatusId
	AND P.PublishData = 1

UNION ALL

-- Output N/As, we find these by a join which have no A10 codes, we don't explicitly have an NA in this schema,
-- just having no A10 codes means NA
SELECT 
	CI.CourseInstanceId AS OPPORTUNITY_ID,
	'NA' AS A10_CODE	
FROM CourseInstance CI
	LEFT OUTER JOIN CourseInstanceA10FundingCode CIAFC ON CI.CourseInstanceId = CIAFC.CourseInstanceId
	JOIN Course C on C.CourseId = CI.CourseId
	JOIN Provider P on P.ProviderId = C.ProviderId
WHERE CIAFC.A10FundingCode IS NULL 
	AND CI.RecordStatusId = @LiveRecordStatusId
	AND P.RecordStatusId = @LiveRecordStatusId
	AND P.PublishData = 1
ORDER BY CI.CourseInstanceId

IF @@ERROR <> 0
BEGIN
	RETURN 1
END

RETURN 0

GO