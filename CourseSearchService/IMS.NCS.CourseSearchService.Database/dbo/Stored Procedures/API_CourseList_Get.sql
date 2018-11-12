CREATE PROCEDURE [dbo].[API_CourseList_Get] 
(
	@CourseSearchText		NVARCHAR(4000) = NULL,
	@ProviderSearchText		NVARCHAR(4000) = NULL,
	@QualificationType		NVARCHAR(100) = NULL,
	@QualificationLevel		NVARCHAR(100) = NULL,
	@StudyMode				NVARCHAR(100) = NULL,
	@AttendanceMode			NVARCHAR(100) = NULL,
	@AttendancePattern		NVARCHAR(100) = NULL,
	@EarliestStartDate		DATETIME = NULL,
	@LdscCategoryCode		NVARCHAR(100)= NULL,
	@ErTtgStatus			BIT	=0,
	@A10Code				NVARCHAR(100) = NULL,
	@IndLivingFlg			BIT	=0,
	@SkillForLifeFlg		NVARCHAR(5) = NULL,
	@ErTTGFlag				NVARCHAR(50) = NULL,
	@ErAppStatus			NVARCHAR(50) = NULL,
	@AdultlrStatus			NVARCHAR(50) = NULL,
	@OtherFundingStatus		NVARCHAR(16) = NULL,
	@AppClosed				DATETIME  = NULL,
	@FlexiStartDate			BIT=0,
	@Location				NVARCHAR(1000)=NULL,
	@Distance				FLOAT=NULL,
	@PageNumber				INT=1,
	@NumberOfRecordPerPage	INT=10,
	@SortBy					NVARCHAR(1),
	@ProviderId				INT = NULL,
	@PublicAPI				INT=1
)
As
BEGIN	

	DECLARE @OneYearAgo DATE = CAST(DATEADD(yyyy, -1, GetUtcDate()) AS DATE);

	SET @CourseSearchText = [dbo].[BuildSearchTerms](@CourseSearchText);

	IF (CharIndex(' ', @ProviderSearchText) <> 0 AND CharIndex('"', @ProviderSearchText) = 0)
	BEGIN
		SET @ProviderSearchText = '"' + @ProviderSearchText + '"';
	END;

	DECLARE @splitChar  NVARCHAR(1) = '|';	
	DECLARE @FromRecord INT = (@PageNumber-1) * @NumberOfRecordPerPage + 1;
	DECLARE @ToRecord	INT = @PageNumber * @NumberOfRecordPerPage;
	DECLARE @DummyDistance INT = 9999;
	DECLARE @StartPoint geography = dbo.GetPointForPostcode(@Location);

	-- Check if postcode supplied without a space
	if (@Location IS NOT NULL AND @StartPoint IS NULL AND Len(@Location) BETWEEN 3 AND 10)
	BEGIN
		DECLARE @NewLocation VARCHAR(1000);
		SET @NewLocation = SubString(@Location, 1, Len(@Location) -3) + ' ' + SubString(@Location, Len(@Location) - 2, 3);
		SET @StartPoint = dbo.GetPointForPostcode(@NewLocation);
	END;

	-- If postcode supplied as location and distance is not supplied then default to 10 miles
	IF (@StartPoint IS NOT NULL AND @Distance IS NULL)
	BEGIN
		SET @Distance = 10;
	END;

	SET @SortBy = Upper(@SortBy);

	DECLARE @MatchingCourseResult TABLE
	(
		CourseId INT, 
		[Rank] INT,
		PRIMARY KEY(CourseId)
	);

	DECLARE @MatchingProviderResult TABLE
	(
		ProviderId INT,
		[Rank] INT,
		PRIMARY KEY (ProviderId)
	);

	IF(@CourseSearchText IS NOT NULL)
		INSERT INTO @MatchingCourseResult            
			SELECT 
				CX.[Key],
				CX.[Rank]
			FROM 
				CONTAINSTABLE(search.Course, CourseTitle, @CourseSearchText, LANGUAGE 1033) CX
				ORDER BY 
					CX.rank DESC;
	ELSE
		INSERT INTO @MatchingCourseResult  
			SELECT CourseId, 0 FROM search.Course;		

	IF(@ProviderSearchText IS NOT NULL)
		BEGIN
			INSERT INTO @MatchingProviderResult            
				SELECT P.[ProviderId],
					PX.[Rank]	
				FROM CONTAINSTABLE(search.ProviderText, SearchText, @ProviderSearchText, LANGUAGE 1033) PX
					INNER JOIN [search].ProviderText PT ON PT.ProviderTextId = PX.[Key]
					INNER JOIN [search].[Provider] P ON P.ProviderId = PT.ProviderId
				ORDER BY PX.Rank DESC;
		END;
	 ELSE
		INSERT INTO @MatchingProviderResult  
			SELECT ProviderId, 0 FROM search.Provider; 

	;WITH RawData AS 
	 (	 
		 SELECT ROW_NUMBER() OVER (ORDER BY CASE WHEN @SortBy = 'A' THEN 1000 - MC.[Rank]
											     WHEN @SortBy = 'S' THEN CASE WHEN StartDate IS NULL THEN CAST ('31 DEC 2999' AS DATE) ELSE StartDate END
												 WHEN @SortBy = 'D' THEN CASE WHEN @StartPoint IS NULL OR V.[Latitude] IS NULL OR V.Longitude IS NULL THEN @DummyDistance
																			  ELSE @StartPoint.STDistance(geography::STGeomFromText('POINT(' + CAST(V.Longitude AS VARCHAR(20)) +' ' + CAST(V.Latitude AS VARCHAR(20)) +')', 4326))/1609.344 END END,
											CASE WHEN @SortBy = 'S' THEN StartDateDescription ELSE NULL END) AS RowNumber,
			[P].[ProviderId],
			[P].[ProviderName],
			[C].CourseId,
			[C].[CourseTitle],
			[C].QualificationTypeName AS QualificationTypeRef,
			[C].QualificationLevelName AS QualificationBulkUploadRef,
			[C].[CourseSummary],
			[C].[LDCS1],
			[C].[LDCS2],
			[C].[LDCS3],
			[C].[LDCS4],
			[C].[LDCS5],
			[CI].[CourseInstanceId],
			[CI].StudyModeName AS StudyModeBulkUploadRef,
			[CI].AttendanceModeName AS AttendanceModeBulkUploadRef,
			[CI].AttendancePatternName AS [AttendancePatternBulkUploadRef],
 			[CI].[StartDate],
			[CI].[StartDateDescription],
			[CI].[EndDate],
			[CI].[RegionName],
			[CI].[DurationUnitId],
			[CI].[DurationAsText],
			[CI].[DurationUnitName] AS [DurationUnitBulkUploadRef],
			[V].[VenueName],
			[V].[AddressLine1],
			[V].[AddressLine2],
			[V].[Town],
			[V].[County],
			[V].[Postcode],
			[V].[Latitude],
			[V].[Longitude],
			--[CI].LdcsCodes,
			[P].[Loans24Plus],
			[CI].[A10Codes] AS [A10FundingCode],
			[C].[IndependentLivingSkills],
			[C].[SkillsForLife],
			[C].[ErAppStatus],
			[C].[AdultLearnerFundingStatus],
			[CI].[ApplyUntilDate],
			CASE WHEN @StartPoint IS NULL OR V.[Latitude] IS NULL OR V.Longitude IS NULL THEN @DummyDistance
				 ELSE @StartPoint.STDistance(geography::STGeomFromText('POINT(' + CAST(V.Longitude AS VARCHAR(20)) +' ' + CAST(V.Latitude AS VARCHAR(20)) +')', 4326))/1609.344 END AS Distance 
		FROM [DBO].[Course] C
			INNER JOIN @MatchingCourseResult MC ON MC.CourseId=C.CourseId
			INNER JOIN [DBO].[Provider] P ON P.ProviderId=C.ProviderId
			INNER JOIN @MatchingProviderResult MP ON MP.ProviderId=p.ProviderId
			INNER JOIN [DBO].CourseInstance CI ON CI.CourseId = C.CourseId
			LEFT OUTER JOIN [DBO].Venue V ON V.VenueId = CI.VenueId
		WHERE
			(C.QualificationTypeRef in (SELECT Data FROM [dbo].[Split](@QualificationType,@splitChar)) OR @QualificationType IS NULL)
		AND
			(C.QualificationBulkUploadRef in (SELECT Data FROM [dbo].[Split](@QualificationLevel,@splitChar)) OR @QualificationLevel IS NULL)
		AND
			((C.[LDCS1] IN (SELECT Data FROM [dbo].[Split](@LdscCategoryCode, @splitChar)) OR  
			  C.[LDCS2] IN (SELECT Data FROM [dbo].[Split](@LdscCategoryCode, @splitChar)) OR   
			  C.[LDCS3] IN (SELECT Data FROM [dbo].[Split](@LdscCategoryCode, @splitChar)) OR 
			  C.[LDCS4] IN (SELECT Data FROM [dbo].[Split](@LdscCategoryCode, @splitChar)) OR  
			  C.[LDCS5] IN (SELECT Data FROM [dbo].[Split](@LdscCategoryCode, @splitChar))) OR @LdscCategoryCode IS NULL)
		AND
			(C.[IndependentLivingSkills]=@IndLivingFlg OR @IndLivingFlg=0)
		AND
			(C.[Loans24Plus] = @ErTtgStatus OR @ErTtgStatus=0)
		AND
			(C.[OtherFundingStatus] IN (SELECT Data FROM [dbo].[Split](@OtherFundingStatus, @splitChar)) OR @OtherFundingStatus IS NULL)
	 	AND
			(C.[AdultLearnerFundingStatus] IN (SELECT Data FROM [dbo].[Split](@AdultlrStatus, @splitChar)) OR @AdultlrStatus IS NULL)
		AND
			(C.ErAppStatus=@ErAppStatus OR @ErAppStatus IS NULL)
		AND
			(C.SkillsForLife=@SkillForLifeFlg OR @SkillForLifeFlg IS NULL)
		AND
			(CI.StudyModeBulkUploadRef IN (SELECT Data FROM [dbo].[Split](@StudyMode, @splitChar)) OR @StudyMode IS NULL)
		AND
			(CI.AttendanceModeBulkUploadRef IN (SELECT Data FROM [dbo].[Split](@AttendanceMode, @splitChar)) OR @AttendanceMode IS NULL)
		AND
			(CI.AttendancePatternBulkUploadRef IN (SELECT Data FROM [dbo].[Split](@AttendancePattern, @splitChar)) OR @AttendancePattern IS NULL)
		AND
			(CI.[ApplyUntilDate] < @AppClosed OR @AppClosed IS NULL)
		AND
			(CI.CourseInstanceId IN (SELECT CourseInstanceId FROM CourseInstanceA10FundingCode WHERE A10FundingCode IN (SELECT Data FROM [dbo].[Split](@A10Code,@splitChar))) OR @A10Code IS NULL)
		AND
			((CI.StartDate>=@EarliestStartDate OR @EarliestStartDate IS NULL) Or @FlexiStartDate=1)
		AND 
			(CI.StartDate IS NULL OR CI.StartDate >= @OneYearAgo)
		AND 
			(@StartPoint IS NOT NULL OR @Location IS NULL OR (@Location = V.Town OR V.County = @Location OR @Location = CI.RegionName OR V.Postcode LIKE @Location + '%'))
		AND 
			(@Distance IS NULL OR @Distance >= CASE WHEN @StartPoint IS NULL OR V.Latitude IS NULL OR V.Longitude IS NULL THEN @DummyDistance
													ELSE @StartPoint.STDistance(geography::STGeomFromText('POINT(' + CAST(V.Longitude AS VARCHAR(20)) +' ' + CAST(V.Latitude AS VARCHAR(20)) +')', 4326))/1609.344 END)
		AND (@PublicAPI = 0 OR C.ApplicationId != 3)
		AND (P.ProviderId = @ProviderId OR @ProviderId IS NULL)
	)
	,TotalRawDataRecords AS 
	(
		SELECT Count(*) AS RecordCount 
		FROM RawData
	)

	SELECT RowNumber,	
		CASE WHEN Distance = @DummyDistance THEN NULL ELSE Convert(DECIMAL(7,3), Distance) END AS Distance,	
		ProviderId,
		ProviderName,	
		CourseId,	
		CourseTitle,	
		QualificationTypeRef,	
		QualificationBulkUploadRef,	
		CourseSummary,	
		LDCS1,	
		LDCS2,	
		LDCS3,	
		LDCS4,	
		LDCS5,	
		CourseInstanceId,	
		StudyModeBulkUploadRef,	
		AttendanceModeBulkUploadRef,	
		AttendancePatternBulkUploadRef,	
		StartDate,	
		StartDateDescription,	
		EndDate,
		RegionName,	
		DurationUnitId,	
		DurationAsText,	
		DurationUnitBulkUploadRef,	
		VenueName,	
		AddressLine1,	
		AddressLine2,	
		Town,	
		County,
		Postcode,	
		Latitude,	
		Longitude,	
		Loans24Plus,	
		A10FundingCode,	
		IndependentLivingSkills,	
		SkillsForLife,	
		ErAppStatus,	
		AdultLearnerFundingStatus,	
		ApplyUntilDate,	
		RecordCount  
	FROM RawData, 
		TotalRawDataRecords
	WHERE RowNumber BETWEEN @FromRecord AND @ToRecord
	ORDER BY RowNumber;
 
 END;