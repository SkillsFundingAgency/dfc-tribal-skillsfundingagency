CREATE PROCEDURE [dbo].[up_ReportProviderDashboard]
		@ProviderId int
AS

/*
*	Name:		[up_ReportProviderDashboard]
*	System: 	Stored procedure interface module
*	Description:	Get provider dashboard data
*	Return Values:	0 = No problem detected
*			1 = General database error.
*	Copyright:	(c) Tribal Data Solutions Ltd, 2012
*			All rights reserved.
*
*	$Log:  $
*/

BEGIN

	DECLARE @DeletedStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsDeleted = 1);
	DECLARE @liveStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 1);
	DECLARE @pendingStatus int = (SELECT RecordStatusId FROM RecordStatus WHERE IsPublished = 0 and IsArchived=0 and IsDeleted=0);

	DECLARE @DefaultDate DATE = CAST('01 JAN 2000' AS DATE);
	DECLARE	@today datetime = GetUtcDate()

	-- Create the quality scores if they don't exist
	IF NOT EXISTS (SELECT 1 FROM QualityScore WHERE ProviderId = @ProviderId)
	BEGIN
		exec dbo.up_ProviderUpdateQualityScore @ProviderId;
	END;

	-- Most recent provider user
	CREATE TABLE #ProviderUserLogin (
		UserId					nvarchar(128) NOT NULL,
		DisplayName				nvarchar(255) NULL,
		LastLoginDateTimeUtc	datetime NULL
	);

	INSERT INTO #ProviderUserLogin
		(UserId, DisplayName, LastLoginDateTimeUtc)
	SELECT u.Id, u.Name, u.LastLoginDateTimeUtc 
	FROM ProviderUser pu
		JOIN AspNetUsers u on pu.UserId = u.Id
	WHERE pu.ProviderId = @ProviderId;

	DECLARE @PendingCourseCount int
	Select @PendingCourseCount = count(courseId) from Course where ProviderId=@ProviderId and RecordStatusId=@pendingStatus;

	--Main dashboard stats
	SELECT
		p.ProviderId
		, p.ProviderName
		, p.CreatedDateTimeUtc ProviderCreatedDateTimeUtc
		, io.Name InformationOfficerDisplayName
		, rm.Name RelationshipManagerDisplayName
		, rm.Email RelationShipManagerEmail
		, rm.PhoneNumber RelationshipManagerPhone
		, pul.LastLoginDateTimeUtc LastProviderLoginDateTimeUtc
		, pul.DisplayName LastProviderLoginUserDisplayName
		, qs.ModifiedDateTimeUtc LastUpdatingDateTimeUtc
		, p.LastAllDataUpToDateTimeUtc LastAllDataUpToDateTimeUtc
		, QS.LastActivity
		, luu.Name LastUpdatingUserDisplayName
		, lua.ApplicationName LastUpdatingUserUpdateMethod
		-- Provision
		, qs.Courses
		, qs.CourseInstances
		, qs.CoursesWithLongSummary
		, qs.CoursesWithDistinctLongSummary
		, qs.CoursesWithFutureStartDates
		, qs.CoursesWithLearningAims
		, qs.AutoAggregateQualityRating
		, @PendingCourseCount as NumberOfPendingCourses
		, p.IsTASOnly
	FROM Provider p
		LEFT JOIN AspNetUsers io on io.Id = p.InformationOfficerUserId AND io.IsDeleted = 0
		LEFT JOIN AspNetUsers rm on rm.Id = p.RelationshipManagerUserId AND rm.IsDeleted = 0
		LEFT JOIN QualityScore qs on qs.ProviderId = @ProviderId
		LEFT JOIN AspNetUsers luu on luu.Id = qs.ModifiedByUserId
		LEFT JOIN Application lua on lua.ApplicationId = qs.ModifiedByApplicationId
		LEFT JOIN #ProviderUserLogin pul on pul.LastLoginDateTimeUtc = (SELECT MAX(LastLoginDateTimeUtc) FROM #ProviderUserLogin)
	WHERE 
		p.ProviderId = @ProviderId;

	/* 
	* Return a list of organisations for this provider
	*/
	select o.OrganisationId, OrganisationName from Organisation o
		join OrganisationProvider op on o.OrganisationId = op.OrganisationId
		where op.ProviderId = @ProviderId

	/*
	* Collect Course instance information for creating pie charts
	* We use the same definitions for out of date courses as the view course list, these are different to quality scoring
	*/

	DECLARE @LongCourseMinDurationWeeks int = Convert(int,isnull((select Value from ConfigurationSettings where Name = 'LongCourseMinDurationWeeks'),12))
	DECLARE @LongCourseMaxStartDateInPastDays int = Convert(int,isnull((select Value from ConfigurationSettings where Name = 'LongCourseMaxStartDateInPastDays'),28))
	DECLARE @ShortCourseExpiringDays int = Convert(int,isnull((select Value from ConfigurationSettings where Name = 'ShortCourseExpiringDays'),14))
	DECLARE @LongCourseExpiringDays int = Convert(int,isnull((select Value from ConfigurationSettings where Name = 'LongCourseExpiringDays'),28))


	DECLARE @LiveExpiredCourseCount int
	DECLARE @LiveExpiringCourseCount int
	DECLARE @LiveFutureCourseCount int

	DECLARE @PendingCourseInstanceCount int
	DECLARE @LiveExpiredCourseInstanceCount int
	DECLARE @LiveExpiringCourseInstanceCount int
	DECLARE @LiveFutureCourseInstanceCount int

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

	Select @PendingCourseInstanceCount = count(courseInstanceId) from CourseInstance ci
		join Course c on ci.CourseId = c.CourseId
		where c.ProviderId=@ProviderId and ci.RecordStatusId=@pendingStatus;

	SELECT @LiveFutureCourseInstanceCount = Count(courseInstanceId) FROM @CourseInstanceInfo 
		WHERE IsOutOfDate=0 and IsNeedingUpdate = 0
		
	SELECT @LiveExpiringCourseInstanceCount = Count(distinct courseInstanceId) FROM @CourseInstanceInfo 
		WHERE IsOutOfDate=0 and IsNeedingUpdate = 1

	SELECT @LiveExpiredCourseInstanceCount = Count(distinct courseInstanceId) FROM @CourseInstanceInfo 
		WHERE IsOutOfDate=1

	--Return course instance statuses
	--Spring 2018 Update, Opportunities cannot be pending
	select CourseStatusName, CourseCount, BarColour, Link from 
	(
	Select 'Up to date' as CourseStatusName, @LiveFutureCourseInstanceCount as CourseCount, '#2bf709' as BarColour, 'OpportunitiesUpToDate' as Link, 1 as DisplayOrder 
	union Select 'Due for update' as CourseStatusName, @LiveExpiringCourseInstanceCount as CourseCount, '#ffc000' as BarColour, 'OpportunitiesExpiring' as Link, 2 as DisplayOrder
	union Select 'Out of date' as CourseStatusName, @LiveExpiredCourseInstanceCount as CourseCount, '#ff0000' as BarColour, 'OpportunitiesOutOfDate' as Link, 3 as DisplayOrder
	--union Select 'Pending' as CourseStatusName, @PendingCourseInstanceCount as CourseCount, '#0000ff' as BarColour, 'none' as Link, 4 as DisplayOrder
	) AllStatistics 
	Order by DisplayOrder

	--Calculate the results for courses
	DECLARE  @CourseInfo TABLE (
		CourseId				int				not null
	, IsOutOfDate			bit				not null default 0
	, IsNeedingUpdate		bit				not null default 0
	)

	--For courses, we need to group by CourseId and choose the best result from the instances
	insert into @CourseInfo (CourseId, IsOutOfDate, IsNeedingUpdate)
	select CourseId, cast(min(convert(int,IsOutOfdate)) as bit), cast(min(convert(int,IsNeedingUpdate)) as bit)
	from @CourseInstanceInfo
	Group by CourseId


	SELECT @LiveFutureCourseCount = Count(courseId) FROM @CourseInfo 
		WHERE IsOutOfDate=0 and IsNeedingUpdate = 0
		
	SELECT @LiveExpiringCourseCount = Count(courseId) FROM @CourseInfo 
		WHERE IsOutOfDate=0 and IsNeedingUpdate = 1

	SELECT @LiveExpiredCourseCount = Count(courseId) FROM @CourseInfo 
		WHERE IsOutOfDate=1

	--Return course instance statuses
	select CourseStatusName, CourseCount, BarColour, Link from 
	(
	Select 'Up to date' as CourseStatusName, @LiveFutureCourseCount as CourseCount, '#2bf709' as BarColour, 'CoursesUpToDate' as Link, 1 as DisplayOrder 
	union Select 'Due for update' as CourseStatusName, @LiveExpiringCourseCount as CourseCount, '#ffc000' as BarColour, 'CoursesExpiring' as Link, 2 as DisplayOrder
	union Select 'Out of date' as CourseStatusName, @LiveExpiredCourseCount as CourseCount, '#ff0000' as BarColour, 'CoursesOutOfDate' as Link, 3 as DisplayOrder
	union Select 'Pending' as CourseStatusName, @PendingCourseCount as CourseCount, '#0000ff' as BarColour, 'CoursesPending' as Link, 4 as DisplayOrder
	) AllStatistics 
	Order by DisplayOrder

	DROP TABLE #ProviderUserLogin;

	IF(@@ERROR <> 0)
	BEGIN
		RETURN 1;
	END;
	
	RETURN 0;

END;
