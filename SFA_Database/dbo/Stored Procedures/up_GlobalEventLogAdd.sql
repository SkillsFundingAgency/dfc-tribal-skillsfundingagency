CREATE PROCEDURE [dbo].[up_GlobalEventLogAdd]

	@EventID GEL_ID_t OUTPUT,
	@DateTimeUtc GEL_DateTime_t,
	@DateTime GEL_DateTime_t,
	@Type GEL_Type_t,
	@Source GEL_Source_t,
	@Computer GEL_Computer_t,
	@User GEL_User_t,
	@Event GEL_Event_t
	
AS
/*
*	Name:		up_GlobalEventLogAdd
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
 * 1     1/07/09 11:28 Robert.knapton
 * First pass code
*/
DECLARE @TypeID GEL_ID_t
DECLARE @ComputerID GEL_ID_t
DECLARE @UserID GEL_ID_t
DECLARE @SourceID GEL_ID_t
DECLARE @RC INT
DECLARE @Error INT
DECLARE @RowCount INT
/*
*Get/add id's for strings supplied
*
*/
EXEC @RC = up_GlobalEventUserGetID	@UserID = @UserID OUTPUT,
									@User = @User

IF @@ERROR != 0 OR @RC != 0
BEGIN
	RETURN 1
END

EXEC @RC = up_GlobalEventComputerGetID	@ComputerID = @ComputerID OUTPUT,
										@Computer = @Computer

IF @@ERROR != 0 OR @RC != 0
BEGIN
	RETURN 1
END

EXEC @RC = up_GlobalEventSourceGetID	@SourceID = @SourceID OUTPUT,
										@Source = @Source

IF @@ERROR != 0 OR @RC != 0
BEGIN
	RETURN 1
END

EXEC @RC = up_GlobalEventTypeGetID	@TypeID = @TypeID OUTPUT,
									@Type = @Type

IF @@ERROR != 0 OR @RC != 0
BEGIN
	RETURN 1
END
/*
*add the record
*
*/
INSERT INTO GlobalEventLog (DateTimeUtc, [DateTime], TypeID, SourceID, ComputerID, UserID, [Event])
VALUES (@DateTimeUtc, @DateTime, @TypeID, @SourceID, @ComputerID, @UserID, @Event)

IF @@ERROR != 0
BEGIN
	RETURN 1
END

SELECT @EventID = SCOPE_IDENTITY()

RETURN 0