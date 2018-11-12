CREATE PROCEDURE [dbo].[up_GlobalEventLogListTypes]

	
AS
/*
*	Name:		up_GlobalEventLogListTypes
*	System: 	Stored procedure interface module
*	Description:	List all types available for filtering
*
*	Return Values:	0 = No problem detected
*			1 = General database error.

*	Copyright:	(c) Tribal Data Solutions Ltd, 2006
*			All rights reserved.
*
*	$Log: /src/Tribal/TribalTechnology/InformationManagement/Net Version 4.0 Common/DatabaseEventLogger/DatabaseEventLogger.root/DatabaseEventLogger/DatabaseEventLogger/Create GlobalEventLog script C4_0_2.sql $
 * 
 * 2     24/01/12 15:04 Leigh.carpenter
 * 
 * 1     23/01/12 7:55 Leigh.carpenter
 * 
 * 1     1/07/09 14:04 Robert.knapton
 * C1_0_1 Release
 * 
 * 2     1/07/09 11:27 Robert.knapton
 * Changes to abstract properites to linked server
 * 
 * 1     1/07/09 8:49 Robert.knapton
 * Checked in to common code area
 * 
 * 1     15/12/08 9:32 Robert.knapton
 * First pass code
 * 
 * 1     13/05/08 15:22 Leigh.carpenter
*/

SELECT [Type] 
FROM GlobalEventType
ORDER BY [Type] ASC

IF @@ERROR <> 0
BEGIN
	RETURN 1
END

RETURN 0