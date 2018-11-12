CREATE PROCEDURE [dbo].[up_GetProviderCoursesOutOfDate]
	@ProviderId int, 
	@LongCourseMinDurationWeeks int, 
	@LongCourseMaxStartDateInPastDays int
AS

/*
*	Name:		[up_GetProviderCoursesOutofDate]
*	System: 	Stored procedure interface module
*	Description:	Get a count of course which are out of date
*
*	Get Provider Courses which are "out of date"
*	Rules
*  
*	out of date courses are those for which all the opportunities are out of date. 
*	Opportunities for “Short” SFA course.  If the opportunity’s start date is after today, then it is out of date.
*	Opportunities for longer SFA courses.  If the opportunity is more than x days in length, then it is not out of date until more 
*   than y days after the start date.   (The thinking being that learners may still join some time after the start date.)   x and y are globally configurable.)
*	DFE courses. Typically these opportunities will be made available on the portal more than a year before the start date.  
*   So there may be – for example – courses with an opportunity that starts 6 months in the future and one that starts 18 months in the future. 
*	Both of these are valid and should be displayed to searchers.  
*	As described below, in most cases, it is best for providers to duplicate the latest opportunity and adapt it, when they create one for a future date.  (If the details of the parent course are different, it should be created as a new course.)  
	*Therefore:
	*	As with short SFA courses, if there are no opportunities whose start date is today or later, the course is out of date.

*	Return Values:	0 = No problem detected
*				   -1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/
BEGIN

	IF (@ProviderId IS NULL) 
	BEGIN
		RETURN 0;
	END;

	DECLARE @LiveStatusId int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);

	DECLARE @ProviderCourses TABLE
	(
		CourseId			INT,
		CourseInstanceId	INT,
		MaxStartDate		DATETIME,
		IsLong				BIT,
		IsOutOfDate			BIT
		PRIMARY KEY (CourseId, CourseInstanceId)
	);

	INSERT INTO @ProviderCourses (CourseId, CourseInstanceId, MaxStartDate, IsLong, IsOutOfDate)
		SELECT DISTINCT C.CourseId, 
			CI.CourseInstanceId, 
			CISD.MaxStartDate, 
			CASE WHEN (
						(COALESCE(CI.DurationUnit, 0) * COALESCE(DU.WeekEquivalent, -1)) >= @LongCourseMinDurationWeeks  -- Duration Unit may be null so set default values if it is null
						OR
						(DU.DurationUnitId IS NULL AND DATEDIFF(day, MaxStartDate, COALESCE(EndDate, MaxStartDate)) >= @LongCourseMinDurationWeeks * 7 - 3) -- If the course runs from Monday to Friday then it won't be exactly 12 weeks from start date to end date so take 3 days off to compensate
					  ) THEN 1 ELSE 0 END,
			0
		FROM Course C 
			INNER JOIN CourseInstance CI on C.CourseId = CI.CourseId 
			LEFT OUTER JOIN (SELECT CourseInstanceId, Max(StartDate) As MaxStartDate FROM CourseInstanceStartDate GROUP BY CourseInstanceId) CISD ON CISD.CourseInstanceId = CI.CourseInstanceId
			LEFT OUTER JOIN DurationUnit DU on DU.DurationUnitId = CI.DurationUnitId
		WHERE ProviderId = @ProviderId
			AND C.RecordStatusId = @LiveStatusId
			AND CI.RecordStatusId = @LiveStatusId;

	-- Short Courses
	UPDATE @ProviderCourses 
	SET IsOutOfDate = 1
	WHERE IsLong = 0
		AND MaxStartDate < GetUtcDate();

	-- Long Courses
	UPDATE @ProviderCourses
	SET IsOutOfDate = 1
	WHERE IsLong = 1
		AND MaxStartDate < DATEADD(day, -@LongCourseMaxStartDateInPastDays, GetUtcDate());

	-- Remove Courses That Have Opportunities That Are Not Out Of Date
	DELETE FROM @ProviderCourses WHERE CourseId IN (SELECT DISTINCT CourseId FROM @ProviderCourses WHERE IsOutOfDate = 0);

	SELECT CourseId,
		Max(MaxStartDate) As MaxStartDate
	FROM @ProviderCourses
	GROUP BY CourseId;

	IF (@@ERROR <> 0)
	BEGIN
		RETURN -1;
	END;
	
	RETURN 0;

END;
