CREATE PROCEDURE [dbo].[up_GetProviderCoursesDateStatus]
		@ProviderId int
AS

/*
*	Name:		[up_GetProviderCoursesDateStatus]
*	System: 	Stored procedure interface module
*	Description:	Get provider course date statuses (up to date, due for update, out of date)
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

BEGIN

	DECLARE	@today datetime = GetUtcDate()
	DECLARE @liveStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);

	DECLARE @LongCourseMinDurationWeeks int = Convert(int,isnull((select Value from ConfigurationSettings where Name = 'LongCourseMinDurationWeeks'),12))
	DECLARE @LongCourseMaxStartDateInPastDays int = Convert(int,isnull((select Value from ConfigurationSettings where Name = 'LongCourseMaxStartDateInPastDays'),28))
	DECLARE @ShortCourseExpiringDays int = Convert(int,isnull((select Value from ConfigurationSettings where Name = 'ShortCourseExpiringDays'),14))
	DECLARE @LongCourseExpiringDays int = Convert(int,isnull((select Value from ConfigurationSettings where Name = 'LongCourseExpiringDays'),28))

	-- CourseInstance information
	DECLARE  @CourseInstanceInfo TABLE (
		  CourseId				int				not null
		, CourseInstanceId		int				not null
		, MaxStartDate			date			null
		, StartDateDescription	nvarchar(150)	null
		, IsLong				bit				not null default 0
		, IsOutOfDate			bit				not null default 0
		, IsNeedingUpdate		bit				not null default 0
	)

	INSERT INTO @CourseInstanceInfo
		(CourseId, CourseInstanceId, MaxStartDate, StartDateDescription, IsLong)
	SELECT c.CourseId, ci.CourseInstanceId, cisd.MaxStartDate,	ci.StartDateDescription,
			CASE WHEN (
						(COALESCE(CI.DurationUnit, 0) * COALESCE(DU.WeekEquivalent, -1)) >= @LongCourseMinDurationWeeks  -- Duration Unit may be null so set default values if it is null
						OR
						(DU.DurationUnitId IS NULL AND DATEDIFF(day, MaxStartDate, COALESCE(EndDate, MaxStartDate)) >= @LongCourseMinDurationWeeks * 7 - 3) -- If the course runs from Monday to Friday then it won't be exactly 12 weeks from start date to end date so take 3 days off to compensate
					  ) THEN 1 ELSE 0 END as IsLong

	FROM Course c
		JOIN CourseInstance ci on ci.CourseId = c.CourseId
		LEFT OUTER JOIN (SELECT CourseInstanceId, Max(StartDate) As MaxStartDate FROM CourseInstanceStartDate GROUP BY CourseInstanceId) CISD ON CISD.CourseInstanceId = CI.CourseInstanceId
		LEFT OUTER JOIN DurationUnit DU on DU.DurationUnitId = CI.DurationUnitId
	WHERE 
		c.ProviderId = @ProviderId AND c.RecordStatusId = @LiveStatus
		AND ci.RecordStatusId = @LiveStatus


	-- Short Courses
	UPDATE @CourseInstanceInfo SET IsOutOfDate = 1
		WHERE IsLong = 0 AND MaxStartDate < GetUtcDate();

	UPDATE @CourseInstanceInfo SET IsNeedingUpdate = 1
		WHERE IsLong = 0 AND IsOutOfDate = 0
		AND MaxStartDate < DATEADD(day, @ShortCourseExpiringDays, GetUtcDate());

	-- Long Courses
	UPDATE @CourseInstanceInfo	SET IsOutOfDate = 1
		WHERE IsLong = 1
		AND MaxStartDate < DATEADD(day, -@LongCourseMaxStartDateInPastDays, GetUtcDate());

	UPDATE @CourseInstanceInfo SET IsNeedingUpdate = 1
	WHERE IsLong = 1 and IsOutOfDate = 0
		AND MaxStartDate < DATEADD(day, @LongCourseExpiringDays, GetUtcDate());


	UPDATE @CourseInstanceInfo SET IsNeedingUpdate = 1 WHERE IsOutOfDate = 1;


	--Calculate the results for courses
	DECLARE  @CourseInfo TABLE (
		  CourseId				int				not null
		, MaxStartDate			date			null
		, IsOutOfDate			bit				not null default 0
		, IsNeedingUpdate		bit				not null default 0
		, DateStatus			int				null
	)

	--For courses, we need to group by CourseId and choose the best result from the instances
	insert into @CourseInfo (CourseId, MaxStartDate, IsOutOfDate, IsNeedingUpdate)
	select CourseId, Max(MaxStartDate), cast(min(convert(int,IsOutOfdate)) as bit), cast(min(convert(int,IsNeedingUpdate)) as bit)
	from @CourseInstanceInfo
	Group by CourseId

	--Now we can calculate the final date status:
		--Up to Date = 1
		--Neeing Update = 2
		--Out of Date = 3
	Update @CourseInfo set DateStatus =
		Case when IsNeedingUpdate = 0 AND IsOutOfDate = 0 Then 1
			 when IsNeedingUpdate = 1 AND IsOutOfDate = 0 Then 2
			 when IsOutOfDate = 1 Then 3
			 end

	Select CourseId, MaxStartDate, DateStatus from @CourseInfo

	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;
	
	RETURN 0;

END;