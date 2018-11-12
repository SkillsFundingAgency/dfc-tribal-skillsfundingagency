CREATE PROCEDURE [dbo].[up_QualityEmailLogDeleteDuplicateRows]
AS

/*
*	Name:		[up_QualityEmailLogDeleteDuplicateRows]
*	System: 	Stored procedure interface module
*	Description:	Delete duplicate entries from the QualityEmailLog table
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

WITH CTE AS (
   SELECT QualityEmailLogId,
       RN = ROW_NUMBER()OVER(PARTITION BY ProviderId, ModifiedDateTimeUtc, TrafficLightStatusId, SFAFunded, DFE1619Funded, QualityEmailsPaused, HasValidRecipients, EmailTemplateId, EmailDateTimeUtc, NextEmailTemplateId, NextEmailDateTimeUtc ORDER BY ProviderId)
   FROM QualityEmailLog
)
DELETE FROM CTE WHERE RN > 1

IF(@@ERROR <> 0)
BEGIN
	RETURN 1
END
	
RETURN 0
