CREATE PROCEDURE [dbo].[up_GlobalEventUserGetID]

	@UserID GEL_ID_t OUTPUT,
	@User GEL_User_t
	
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

/*
*Get the ID if the type exists
*
*/
SELECT @UserID = UserID
FROM GlobalEventUser
WHERE [User] = @User

IF @@ERROR != 0
BEGIN
	RETURN 1
END
/*
*It doesn't exist, so add it and get the id
*
*/
IF @UserID IS NULL
BEGIN
	INSERT INTO GlobalEventUser ([User])
	VALUES (@User)

	IF @@ERROR != 0
	BEGIN
		RETURN 1
	END

	SELECT @UserID = SCOPE_IDENTITY()
END

RETURN 0