CREATE PROCEDURE [dbo].[up_GlobalEventLogList]

	@UserName GEL_User_t = NULL,
	@Type GEL_Type_t = NULL,
	@Computer GEL_Computer_t = NULL,
	@FromDate GEL_DateTime_t = NULL,
	@ToDate GEL_DateTime_t = NULL,
	@SortCol INT
	
AS
/*
*	Name:		up_GlobalEventLogList
*	System: 	Stored procedure interface module
*	Description:	List all events filtered
*
*	Return Values:	0 = No problem detected
*			1 = General database error.
*
*	Copyright:	(c) Tribal Data Solutions Ltd, 2006
*			All rights reserved.
*
*	$Log: /doc/Tribal Technology (Leeds)/Global Event Log/C1_0_1.sql $
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
 * 2     20/08/08 13:49 Leigh.carpenter
 * 
 * 1     19/08/08 1:43p Chris.greenhow
 * 
 * 1     13/05/08 15:22 Leigh.carpenter
*/



SELECT	GEL.EventID, GEL.[DateTime], GEL.[DateTimeUtc], GETy.[Type], GES.Source, 
		GEC.Computer, GEU.[User], GEL.[Event]
FROM GlobalEventLog GEL
INNER JOIN GlobalEventComputer GEC ON GEL.ComputerID = GEC.ComputerID
INNER JOIN GlobalEventSource GES ON GEL.SourceID = GES.SourceID
INNER JOIN GlobalEventType GETy ON GEL.TypeID = GETy.TypeID
INNER JOIN GlobalEventUser GEU ON GEL.UserID = GEU.UserID
WHERE (GEU.[User] = @UserName OR @UserName IS NULL)
AND (GETy.[Type] = @Type OR @Type IS NULL)
AND (GEC.Computer = @Computer OR @Computer IS NULL)
AND (GEL.[DateTime] >= @FromDate OR @FromDate IS NULL)
AND (GEL.[DateTime] <= @ToDate OR @ToDate IS NULL)
ORDER BY CASE @SortCol 
			WHEN 1 THEN RIGHT('0000000000' + CAST(GEL.EventID AS VARCHAR(10)), 10)
			WHEN 2 THEN 
				CASE WHEN GEL.[DateTimeUtc] IS NULL THEN '219912312359'
				ELSE DATENAME(YYYY,(GEL.[DateTime])) + 
				RIGHT('00' + CAST(DATEPART(MM,GEL.[DateTimeUtc]) AS VARCHAR),2) + 
				RIGHT('00' + CAST(DATEPART(DD,GEL.[DateTimeUtc]) AS VARCHAR),2) + 
				RIGHT('00' + CAST(DATEPART(HH,GEL.[DateTimeUtc]) AS VARCHAR),2) +
				RIGHT('00' + CAST(DATEPART(MI,GEL.[DateTimeUtc]) AS VARCHAR),2) + 
				RIGHT('00' + CAST(DATEPART(SS,GEL.[DateTimeUtc]) AS VARCHAR),2) END 
			WHEN 3 THEN GETy.[Type]
			WHEN 4 THEN GES.Source
			WHEN 5 THEN GEC.Computer
			WHEN 6 THEN GEU.[User]
			WHEN 7 THEN GEL.[Event]
			ELSE RIGHT('0000000000' + CAST(1000000000 - GEL.EventID AS VARCHAR(10)), 10)
			END

IF @@ERROR <> 0
BEGIN
	RETURN 1
END

RETURN 0