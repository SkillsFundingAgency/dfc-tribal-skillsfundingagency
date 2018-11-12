CREATE PROCEDURE [dbo].[up_GlobalEventLogRelationshipPurge]
	
AS
/*
*	Name:		up_GlobalEventLogRelationshipPurge
*	System: 	Stored procedure interface module
*	Description:	Delete events from the log
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
 * 1     1/07/09 11:28 Robert.knapton
 * First pass code
*/
DELETE GlobalEventComputer
FROM GlobalEventComputer GEC
LEFT OUTER JOIN GlobalEventLog GEL ON GEC.ComputerID = GEL.ComputerID
WHERE GEL.EventID IS NULL

IF @@ERROR != 0
BEGIN
	RETURN 1
END

DELETE GlobalEventUser
FROM GlobalEventUser GEU
LEFT OUTER JOIN GlobalEventLog GEL ON GEU.UserID = GEL.UserID
WHERE GEL.EventID IS NULL

IF @@ERROR != 0
BEGIN
	RETURN 1
END

DELETE GlobalEventSource
FROM GlobalEventSource GES
LEFT OUTER JOIN GlobalEventLog GEL ON GES.SourceID = GEL.SourceID
WHERE GEL.EventID IS NULL

IF @@ERROR != 0
BEGIN
	RETURN 1
END

DELETE GlobalEventType
FROM GlobalEventType GETy
LEFT OUTER JOIN GlobalEventLog GEL ON GETy.TypeID = GEL.TypeID
WHERE GEL.EventID IS NULL

IF @@ERROR != 0
BEGIN
	RETURN 1
END

RETURN 0