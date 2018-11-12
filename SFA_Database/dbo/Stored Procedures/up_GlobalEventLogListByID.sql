CREATE PROCEDURE [dbo].[up_GlobalEventLogListByID]

	@EventID GEL_ID_t
	
	
AS
/*
*	Name:		up_GlobalEventLogListByID
*	System: 	Stored procedure interface module
*	Description:	List all events filtered
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*
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
 * 1     23/05/08 10:57 Leigh.carpenter
*/

SELECT	GEL.EventID, GEL.DateTimeUtc, GEL.[DateTime], GETy.[Type], GES.Source, 
		GEC.Computer, GEU.[User], GEL.[Event]
FROM GlobalEventLog GEL
INNER JOIN GlobalEventComputer GEC ON GEL.ComputerID = GEC.ComputerID
INNER JOIN GlobalEventSource GES ON GEL.SourceID = GES.SourceID
INNER JOIN GlobalEventType GETy ON GEL.TypeID = GETy.TypeID
INNER JOIN GlobalEventUser GEU ON GEL.UserID = GEU.UserID
WHERE GEL.EventID = @EventID

IF @@ERROR <> 0
BEGIN
	RETURN 1
END

RETURN 0