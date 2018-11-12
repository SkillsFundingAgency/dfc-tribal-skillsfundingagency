CREATE PROCEDURE [dbo].[up_GlobalEventLogDelete]

	@UserName GEL_User_t = NULL,
	@Type GEL_Type_t = NULL,
	@Computer GEL_Computer_t = NULL,
	@FromDate GEL_DateTime_t = NULL,
	@ToDate GEL_DateTime_t = NULL
	
AS
/*
*	Name:		up_GlobalEventLogDelete
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
 * 3     1/07/09 12:04 Robert.knapton
 * fixed error referencing columns
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
 * 1     10/10/08 14:20 Leigh.carpenter
 * First pass
*/
DECLARE @UserID GEL_ID_t
DECLARE @ComputerID GEL_ID_t
DECLARE @TypeID GEL_ID_t
DECLARE @RC INT
/*
*Get the id's from strings supplied
*(NB: We will add a value in these strings if the string supplied doesn't already exist
*but we clear up later so we aren't concerned)
*
*/
IF @UserName IS NOT NULL
BEGIN
	EXEC @RC = up_GlobalEventUserGetID	@UserID = @UserID OUTPUT,
										@User = @UserName

	IF @@ERROR <> 0 OR @RC != 0
	BEGIN
		RETURN 1
	END
END

IF @Type IS NOT NULL
BEGIN
	EXEC @RC = up_GlobalEventTypeGetID	@TypeID = @TypeID OUTPUT,
										@Type = @Type

	IF @@ERROR <> 0 OR @RC != 0
	BEGIN
		RETURN 1
	END
END

IF @Computer IS NOT NULL
BEGIN
	EXEC @RC = up_GlobalEventComputerGetID	@ComputerID = @ComputerID OUTPUT,
											@Computer = @Computer

	IF @@ERROR <> 0 OR @RC != 0
	BEGIN
		RETURN 1
	END
END
/*
*Delete from the GlobalEventLog
*
*/
DELETE FROM GlobalEventLog
WHERE (UserID = @UserID OR @UserID IS NULL)
AND (TypeID = @TypeID OR @TypeID IS NULL)
AND (ComputerID = @ComputerID OR @ComputerID IS NULL)
AND ([DateTime] >= @FromDate OR @FromDate IS NULL)
AND ([DateTime] <= @ToDate OR @ToDate IS NULL)

IF @@ERROR <> 0
BEGIN
	RETURN 1
END
/*
*Delete the records from the related tables where they are no longer referenced
*
*/
EXEC @RC = up_GlobalEventLogRelationshipPurge

IF @@ERROR != 0 OR @RC != 0
BEGIN
	RETURN 1
END

RETURN 0