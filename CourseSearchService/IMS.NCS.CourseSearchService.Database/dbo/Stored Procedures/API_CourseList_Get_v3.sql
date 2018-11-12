CREATE PROCEDURE [dbo].[API_CourseList_Get_v3] (
	@CourseIncludeFreeText NVARCHAR(4000) = NULL
	, @CourseExcludeFreeText NVARCHAR(4000) = NULL
	, @CourseIncludeExactText NVARCHAR(4000) = NULL
	, @CourseExcludeExactText NVARCHAR(4000) = NULL
	, @ProviderSearchText NVARCHAR(4000) = NULL
	, @QualificationType NVARCHAR(100) = NULL
	, @QualificationLevel NVARCHAR(100) = NULL
	, @StudyMode NVARCHAR(100) = NULL
	, @AttendanceMode NVARCHAR(100) = NULL
	, @AttendancePattern NVARCHAR(100) = NULL
	, @EarliestStartDate DATETIME = NULL
	, @LdscCategoryCode NVARCHAR(100) = NULL
	, @ErTtgStatus BIT = 0
	, @A10Code NVARCHAR(100) = NULL
	, @IndLivingFlg BIT = 0
	, @SkillForLifeFlg NVARCHAR(5) = NULL
	, @ErTTGFlag NVARCHAR(50) = NULL
	, @ErAppStatus NVARCHAR(50) = NULL
	, @AdultlrStatus NVARCHAR(50) = NULL
	, @OtherFundingStatus NVARCHAR(16) = NULL
	, @AppClosed DATETIME = NULL
	, @FlexiStartDate BIT = 0
	, @Location NVARCHAR(1000) = NULL
	, @Distance FLOAT = NULL
	, @PageNumber INT = 1
	, @NumberOfRecordPerPage INT = 10
	, @SortBy NVARCHAR(1)
	, @ProviderId INT = NULL
	, @PublicAPI INT = 1
	, @DFEFunded BIT = NULL
	, @APIKey NVARCHAR(50) = NULL
	, @CutoffPercentage INT = NULL
	, @ProviderFreeTextMatch INT = 1
	, @CourseFreeTextMatch INT = 1
	, @SearchCourseSummary INT = 1
	, @SearchQualificationTitle INT = 1
)
WITH RECOMPILE
AS
BEGIN
	-- If this is the public API then ensure that we have a valid API Key
	IF ([dbo].[IsValidAPIKey](@PublicAPI, @APIKey) = 0)
	BEGIN
		RETURN 0;
	END;

	IF (
			CharIndex(' ', @ProviderSearchText) <> 0
			AND CharIndex('"', @ProviderSearchText) = 0
			)
	BEGIN
		SET @ProviderSearchText = '"' + @ProviderSearchText + '"';
	END;

	DECLARE @splitChar NVARCHAR(1) = '|';
	DECLARE @FromRecord INT = (@PageNumber - 1) * @NumberOfRecordPerPage + 1;
	DECLARE @ToRecord INT = @PageNumber * @NumberOfRecordPerPage;
	DECLARE @DummyDistance INT = 9999;
	DECLARE @OneYearAgo DATE = CAST(DATEADD(YEAR, -1, GETUTCDATE()) AS DATE);

	CREATE TABLE #MatchingVenueResult (
		VenueId INT NOT NULL PRIMARY KEY
	);

	IF (@Location IS NOT NULL)
	BEGIN
		CREATE TABLE #LocationSearchTerms  (
			Term NVARCHAR(4000) NOT NULL PRIMARY KEY
		);
		INSERT INTO #LocationSearchTerms (Term)
		SELECT Data FROM [dbo].[SplitAndTrim](@Location, ',');
		-- Select start point from any postcode supplied in the location search terms
		DECLARE @StartPoint GEOGRAPHY =	(SELECT TOP 1 * FROM (
											SELECT [dbo].GetPointForPostcode(Term) Coords
											FROM #LocationSearchTerms
										) r
										WHERE r.Coords IS NOT NULL);
		DECLARE @LocationSearchTermCount int = (SELECT Count(*) FROM #LocationSearchTerms);
	
		-- Check if postcode supplied without a space
		IF (
				@Location IS NOT NULL
				AND @StartPoint IS NULL
				AND Len(@Location) BETWEEN 3
					AND 10
				)
		BEGIN
			DECLARE @NewLocation VARCHAR(1000);

			SET @NewLocation = SubString(@Location, 1, Len(@Location) - 3) + ' ' + SubString(@Location, Len(@Location) - 2, 3);
			SET @StartPoint = dbo.GetPointForPostcode(@NewLocation);
		END;

		-- If postcode supplied as location and distance is not supplied then default to 10 miles
		IF (
				@StartPoint IS NOT NULL
				AND @Distance IS NULL
				)
		BEGIN
			SET @Distance = 10;
		END;

		SET @SortBy = Upper(@SortBy);

		-- Find all venues which match the location search term(s)
		IF (@LocationSearchTermCount = 1)
		BEGIN
			-- User has searched for a single location
			-- Direct comparison is faster than using Contains(...)
			INSERT INTO #MatchingVenueResult
			SELECT VenueId
			FROM [dbo].[VenueText] 
			WHERE SearchText = Upper(@Location);

			-- Add other venues matching on partial postcode
			INSERT INTO #MatchingVenueResult
			SELECT VenueId
			FROM [dbo].[Venue]
			WHERE PostCode LIKE @Location + '%'
				AND VenueId NOT IN (SELECT VenueId FROM #MatchingVenueResult)
		END;
		ELSE
		BEGIN
			-- Add venues matching on partial postcode
			INSERT INTO #MatchingVenueResult
			SELECT v.VenueId
			FROM [dbo].[Venue] v
				INNER JOIN #LocationSearchTerms lst ON v.Postcode LIKE lst.Term + '%'

			-- If we have a partial postcode match reduce the required number of results
			DECLARE @LocationsToMatch INT = @LocationSearchTermCount;
			IF EXISTS (SELECT TOP 1 * FROM #MatchingVenueResult)
				SET @LocationsToMatch = @LocationSearchTermCount - 1; 

			-- User has searched for multiple locations
			INSERT INTO #MatchingVenueResult
			SELECT VenueId
			FROM [dbo].[VenueText] vt
			WHERE vt.SearchText COLLATE SQL_Latin1_General_CP1_CI_AS IN (SELECT Upper(Term) FROM #LocationSearchTerms)
				AND VenueId NOT IN (SELECT VenueId FROM #MatchingVenueResult)
			GROUP BY VenueId
			HAVING Count(*) = @LocationsToMatch;

		END;
		DROP TABLE #LocationSearchTerms;
	END;

	-- If we can't determine the start point then distance should be null
	IF (@StartPoint IS NULL AND @Distance IS NOT NULL)
	BEGIN
		SET @Distance = NULL;
	END;

	CREATE TABLE #MatchingCourseResult (
		CourseId INT NOT NULL PRIMARY KEY,
		[Rank] INT NOT NULL
	);
	CREATE TABLE #MatchingCourseResultTemp (
		CourseId INT NOT NULL PRIMARY KEY,
		[Rank] INT NOT NULL
	);
	CREATE TABLE #MatchingProviderResult (
		ProviderId INT NOT NULL PRIMARY KEY,
		[Rank] INT NOT NULL
	);

	-- Perform Search Phrase Replacements
	DECLARE @OrigCourseIncludeFreeText	NVARCHAR(4000) = @CourseIncludeFreeText;
	DECLARE @OrigCourseIncludeExactText	NVARCHAR(4000) = @CourseIncludeExactText;
	DECLARE @SearchPhraseId				INT;
	DECLARE @SearchPhrase				VARCHAR(50);
	DECLARE @RemovePhraseFromSearch		BIT;
	DECLARE searchPhrase_Cursor			CURSOR FOR SELECT SearchPhraseId, Phrase, RemovePhraseFromSearch FROM [Remote].[SearchPhrase] WHERE RecordStatusId = 2 ORDER BY Ordinal;

	OPEN searchPhrase_Cursor;
	FETCH NEXT FROM searchPhrase_Cursor INTO @SearchPhraseId, @SearchPhrase, @RemovePhraseFromSearch;

	DECLARE @qualificationLevel_Cursor	CURSOR;
	DECLARE @studyModes_Cursor			CURSOR;
	DECLARE @attendanceTypes_Cursor		CURSOR;
	DECLARE @attendancePatterns_Cursor	CURSOR;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (CHARINDEX(@SearchPhrase, @CourseIncludeFreeText) > 0 OR CHARINDEX(@SearchPhrase, @CourseIncludeExactText) > 0)
		BEGIN
			IF (@RemovePhraseFromSearch = 1)
			BEGIN
				-- Remove the phrase from the search terms
				SET @CourseIncludeFreeText = REPLACE(@CourseIncludeFreeText, @SearchPhrase, '');
				SET @CourseIncludeExactText = REPLACE(@CourseIncludeExactText, @SearchPhrase, '');
			END;

			-- Get Qualification Levels
			DECLARE @BulkUploadRef				VARCHAR(10);
			SET @qualificationLevel_Cursor = CURSOR FOR SELECT BulkUploadRef FROM [Remote].QualificationLevel QL INNER JOIN [Remote].[SearchPhrase_QualificationLevel] SPQL ON SPQL.QualificationLevelId = QL.QualificationLevelId WHERE SPQL.SearchPhraseId = @SearchPhraseId;
			OPEN @qualificationLevel_Cursor;
			FETCH NEXT FROM @qualificationLevel_Cursor INTO @BulkUploadRef;
			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF (@QualificationLevel IS NULL OR CHARINDEX(@QualificationLevel, @BulkUploadRef) = 0)
				BEGIN
					IF @QualificationLevel IS NULL
						SET @QualificationLevel = '';
					ELSE IF Len(@QualificationLevel) > 0
						SET @QualificationLevel = @QualificationLevel + @splitChar;

					SET @QualificationLevel = @QualificationLevel + @BulkUploadRef;
				END;

				FETCH NEXT FROM @qualificationLevel_Cursor INTO @BulkUploadRef;
			END;
			CLOSE @qualificationLevel_Cursor;

			-- Get Study Modes
			SET @studyModes_Cursor = CURSOR FOR SELECT BulkUploadRef FROM [Remote].StudyMode SM INNER JOIN [Remote].[SearchPhrase_StudyMode] SPSM ON SPSM.StudyModeId = SM.StudyModeId WHERE SPSM.SearchPhraseId = @SearchPhraseId;
			OPEN @studyModes_Cursor;
			FETCH NEXT FROM @studyModes_Cursor INTO @BulkUploadRef;
			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF (@StudyMode IS NULL OR CHARINDEX(@StudyMode, @BulkUploadRef) = 0)
				BEGIN
					IF @StudyMode IS NULL
						SET @StudyMode = '';
					ELSE IF Len(@StudyMode) > 0
						SET @StudyMode = @StudyMode + @splitChar;

					SET @StudyMode = @StudyMode + @BulkUploadRef;
				END;

				FETCH NEXT FROM @studyModes_Cursor INTO @BulkUploadRef;
			END;
			CLOSE @studyModes_Cursor;

			-- Get Attendance Types
			SET @attendanceTypes_Cursor = CURSOR FOR SELECT BulkUploadRef FROM [Remote].AttendanceType AT INNER JOIN [Remote].[SearchPhrase_AttendanceType] SPAT ON SPAT.AttendanceTypeId = AT.AttendanceTypeId WHERE SPAT.SearchPhraseId = @SearchPhraseId;
			OPEN @attendanceTypes_Cursor;
			FETCH NEXT FROM @attendanceTypes_Cursor INTO @BulkUploadRef;
			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF (@AttendanceMode IS NULL OR CHARINDEX(@AttendanceMode, @BulkUploadRef) = 0)
				BEGIN
					IF @AttendanceMode IS NULL
						SET @AttendanceMode = '';
					ELSE IF Len(@AttendanceMode) > 0
						SET @AttendanceMode = @AttendanceMode + @splitChar;

					SET @AttendanceMode = @AttendanceMode + @BulkUploadRef;
				END;

				FETCH NEXT FROM @attendanceTypes_Cursor INTO @BulkUploadRef;
			END;
			CLOSE @attendanceTypes_Cursor;

			-- Get Attendance Patterns
			SET @attendancePatterns_Cursor = CURSOR FOR SELECT BulkUploadRef FROM [Remote].AttendancePattern AP INNER JOIN [Remote].[SearchPhrase_AttendancePattern] SPAP ON SPAP.AttendancePatternId = AP.AttendancePatternId WHERE SPAP.SearchPhraseId = @SearchPhraseId;
			OPEN @attendancePatterns_Cursor;
			FETCH NEXT FROM @attendancePatterns_Cursor INTO @BulkUploadRef;
			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF (@AttendancePattern IS NULL OR CHARINDEX(@AttendancePattern, @BulkUploadRef) = 0)
				BEGIN
					IF @AttendancePattern IS NULL
						SET @AttendancePattern = '';
					ELSE IF Len(@AttendancePattern) > 0
						SET @AttendancePattern = @AttendancePattern + @splitChar;

					SET @AttendancePattern = @AttendancePattern + @BulkUploadRef;
				END;

				FETCH NEXT FROM attendancePatterns_Cursor INTO @BulkUploadRef;
			END;
			CLOSE @attendancePatterns_Cursor;
		END;

		FETCH NEXT FROM searchPhrase_Cursor INTO @SearchPhraseId, @SearchPhrase, @RemovePhraseFromSearch;
	END;
	-- End of Perform Search Phrase Replacements

	-- NULLify free text search fields which are empty
	IF (@CourseIncludeFreeText = '')
		SET @CourseIncludeFreeText = NULL;
	IF (@CourseExcludeFreeText = '')
		SET @CourseExcludeFreeText = NULL;
	IF (@CourseIncludeExactText = '')
		SET @CourseIncludeExactText = NULL;
	IF (@CourseExcludeExactText = '')
		SET @CourseExcludeExactText = NULL;

	DECLARE @CourseTitleMultiplier			INT = (SELECT CAST(COALESCE([Value], [ValueDefault]) AS INT) FROM [Remote].[ConfigurationSettings] WHERE [Name] = 'SearchAPICourseTitleMultiplier');
	DECLARE @CourseSummaryMultiplier		INT = (SELECT CAST(COALESCE([Value], [ValueDefault]) AS INT) FROM [Remote].[ConfigurationSettings] WHERE [Name] = 'SearchAPICourseSummaryMultiplier');
	DECLARE @QualificationTitleMultiplier	INT = (SELECT CAST(COALESCE([Value], [ValueDefault]) AS INT) FROM [Remote].[ConfigurationSettings] WHERE [Name] = 'SearchAPIQualificationTitleMultiplier');

	-- Find matching courses
	IF (@CourseIncludeFreeText IS NOT NULL OR @CourseIncludeExactText IS NOT NULL)
	BEGIN
	
		-- Search on included terms
		IF (@CourseFreeTextMatch = 0 AND @CourseIncludeFreeText IS NOT NULL)
		BEGIN
			SET @CourseIncludeFreeText = [dbo].[BuildSearchTerms](@CourseIncludeFreeText);

			INSERT INTO #MatchingCourseResult
			SELECT R.[Key], Sum(R.[Rank]) AS [Rank]
			FROM (
				-- 1.2.2. Weighted free text search across various course fields
				SELECT CX.[Key]
					, CX.[Rank] * @CourseTitleMultiplier AS [Rank]
				FROM CONTAINSTABLE(dbo.CourseFreeText, CourseTitle, @CourseIncludeFreeText, LANGUAGE 1033) CX
				UNION ALL
				SELECT CX.[Key]
					, CX.[Rank] * @QualificationTitleMultiplier
				FROM CONTAINSTABLE(dbo.CourseFreeText, QualificationTitle, @CourseIncludeFreeText, LANGUAGE 1033) CX
				WHERE @SearchQualificationTitle = 1
				UNION ALL
				SELECT CX.[Key]
					, CX.[Rank] * @CourseSummaryMultiplier
				FROM CONTAINSTABLE(dbo.CourseFreeText, CourseSummary, @CourseIncludeFreeText, LANGUAGE 1033) CX
				WHERE @SearchCourseSummary = 1
				) R
			GROUP BY R.[Key]
			ORDER BY Sum(R.[Rank]) DESC;

			-- Re-rank the results based on original search terms
			SET @OrigCourseIncludeFreeText = [dbo].[BuildSearchTerms](@OrigCourseIncludeFreeText);
			IF (@OrigCourseIncludeFreeText <> @CourseIncludeFreeText)
			BEGIN
				MERGE #MatchingCourseResult AS target
				USING (
						SELECT R.[Key], Sum(R.[Rank]) AS [Rank]
						FROM (
								SELECT CX.[Key] AS [Key]
									, CX.[Rank] * @CourseTitleMultiplier AS [Rank]
								FROM CONTAINSTABLE(dbo.CourseFreeText, CourseTitle, @OrigCourseIncludeFreeText, LANGUAGE 1033) CX
								UNION ALL
								SELECT CX.[Key]
									, CX.[Rank] * @QualificationTitleMultiplier
								FROM CONTAINSTABLE(dbo.CourseFreeText, QualificationTitle, @OrigCourseIncludeFreeText, LANGUAGE 1033) CX
								WHERE @SearchQualificationTitle = 1
								UNION ALL
								SELECT CX.[Key]
									, CX.[Rank] * @CourseSummaryMultiplier
								FROM CONTAINSTABLE(dbo.CourseFreeText, CourseSummary, @OrigCourseIncludeFreeText, LANGUAGE 1033) CX
								WHERE @SearchCourseSummary = 1
							) R
						GROUP BY R.[Key]
					) AS source ([Key], [Rank])
				ON target.CourseId = source.[Key]
				WHEN MATCHED THEN UPDATE SET [Rank] = source.[Rank];
			END;
		END;

		IF (@CourseFreeTextMatch = 1 AND @CourseIncludeFreeText IS NOT NULL)
		BEGIN
			INSERT INTO #MatchingCourseResult
			SELECT R.[Key], Sum(R.[Rank]) AS [Rank]
			FROM (
				-- 1.2.2. Weighted free text search across various course fields
				SELECT CX.[Key]
					, CX.[Rank] * @CourseTitleMultiplier AS [Rank]
				FROM FREETEXTTABLE(dbo.CourseFreeText, CourseTitle, @CourseIncludeFreeText, LANGUAGE 1033) CX
				UNION ALL
				SELECT CX.[Key]
					, CX.[Rank] * @QualificationTitleMultiplier
				FROM FREETEXTTABLE(dbo.CourseFreeText, QualificationTitle, @CourseIncludeFreeText, LANGUAGE 1033) CX
				WHERE @SearchQualificationTitle = 1
				UNION ALL
				SELECT CX.[Key]
					, CX.[Rank] * @CourseSummaryMultiplier
				FROM FREETEXTTABLE(dbo.CourseFreeText, CourseSummary, @CourseIncludeFreeText, LANGUAGE 1033) CX
				WHERE @SearchCourseSummary = 1
				) R
			GROUP BY R.[Key]
			ORDER BY Sum(R.[Rank]) DESC;

			-- Re-rank the results based on original search terms
			IF (@OrigCourseIncludeFreeText <> @CourseIncludeFreeText)
			BEGIN
				MERGE #MatchingCourseResult AS target
				USING (
						SELECT R.[Key], Sum(R.[Rank]) AS [Rank]
						FROM (
								SELECT CX.[Key] AS [Key]
									, CX.[Rank] * @CourseTitleMultiplier AS [Rank]
								FROM FREETEXTTABLE(dbo.CourseFreeText, CourseTitle, @OrigCourseIncludeFreeText, LANGUAGE 1033) CX
								UNION ALL
								SELECT CX.[Key]
									, CX.[Rank] * @QualificationTitleMultiplier
								FROM FREETEXTTABLE(dbo.CourseFreeText, QualificationTitle, @OrigCourseIncludeFreeText, LANGUAGE 1033) CX
								WHERE @SearchQualificationTitle = 1
								UNION ALL
								SELECT CX.[Key]
									, CX.[Rank] * @CourseSummaryMultiplier
								FROM FREETEXTTABLE(dbo.CourseFreeText, CourseSummary, @OrigCourseIncludeFreeText, LANGUAGE 1033) CX
								WHERE @SearchCourseSummary = 1
							) R
						GROUP BY R.[Key]
					) AS source ([Key], [Rank])
				ON target.CourseId = source.[Key]
				WHEN MATCHED THEN UPDATE SET [Rank] = source.[Rank];
			END;
		END;

		-- Search on included exact match terms
		IF (@CourseIncludeExactText IS NOT NULL)
		BEGIN
			SET @CourseIncludeExactText = [dbo].[BuildExactSearchTerms](@CourseIncludeExactText);

			INSERT INTO #MatchingCourseResultTemp
			SELECT R.[Key], Sum(R.[Rank]) AS [Rank]
			FROM (
				-- 1.2.2. Weighted free text search across various course fields
				SELECT CX.[Key] AS [Key]
					, CX.[Rank] * @CourseTitleMultiplier AS [Rank]
				FROM CONTAINSTABLE(dbo.CourseFreeText, CourseTitle, @CourseIncludeExactText, LANGUAGE 1033) CX
				UNION ALL
				SELECT CX.[Key]
					, CX.[Rank] * @QualificationTitleMultiplier
				FROM CONTAINSTABLE(dbo.CourseFreeText, QualificationTitle, @CourseIncludeExactText, LANGUAGE 1033) CX
				WHERE @SearchQualificationTitle = 1
				UNION ALL
				SELECT CX.[Key]
					, CX.[Rank] * @CourseSummaryMultiplier
				FROM CONTAINSTABLE(dbo.CourseFreeText, CourseSummary, @CourseIncludeExactText, LANGUAGE 1033) CX
				WHERE @SearchCourseSummary = 1
				) R
			GROUP BY R.[Key]
			ORDER BY Sum(R.[Rank]) DESC;

			-- Re-rank the results based on original search terms
			SET @OrigCourseIncludeExactText = [dbo].[BuildSearchTerms](@OrigCourseIncludeExactText);
			IF (@OrigCourseIncludeExactText <> @CourseIncludeExactText)
			BEGIN
				MERGE #MatchingCourseResultTemp AS target
				USING (
						SELECT R.[Key], Sum(R.[Rank]) AS [Rank]
						FROM (
								SELECT CX.[Key]
									, CX.[Rank] * @CourseTitleMultiplier AS [Rank]
								FROM CONTAINSTABLE(dbo.CourseFreeText, CourseTitle, @OrigCourseIncludeExactText, LANGUAGE 1033) CX
								UNION ALL
								SELECT CX.[Key]
									, CX.[Rank] * @QualificationTitleMultiplier
								FROM CONTAINSTABLE(dbo.CourseFreeText, QualificationTitle, @OrigCourseIncludeExactText, LANGUAGE 1033) CX
								WHERE @SearchQualificationTitle = 1
								UNION ALL
								SELECT CX.[Key]
									, CX.[Rank] * @CourseSummaryMultiplier
								FROM CONTAINSTABLE(dbo.CourseFreeText, CourseSummary, @OrigCourseIncludeExactText, LANGUAGE 1033) CX
								WHERE @SearchCourseSummary = 1
							) R
						GROUP BY R.[Key]
					) AS source ([Key], [Rank])
				ON target.CourseId = source.[Key]
				WHEN MATCHED THEN UPDATE SET [Rank] = source.[Rank];
			END;

			UPDATE #MatchingCourseResult
				SET [Rank] = [#MatchingCourseResult].[Rank] + t.[Rank]
			FROM #MatchingCourseResultTemp t
			WHERE [#MatchingCourseResult].CourseId = t.CourseId;

			INSERT INTO #MatchingCourseResult
			SELECT * FROM #MatchingCourseResultTemp
			WHERE CourseId NOT IN (SELECT CourseId FROM #MatchingCourseResult);

		END;

	END;
	ELSE
		-- No search terms, match everything
		INSERT INTO #MatchingCourseResult
		SELECT CourseId, 0 FROM dbo.Course;

	-- Remove excluded search terms
	IF (@CourseFreeTextMatch = 0 AND @CourseExcludeFreeText IS NOT NULL)
	BEGIN
		SET @CourseExcludeFreeText = [dbo].[BuildSearchTerms](@CourseExcludeFreeText);

		DELETE FROM #MatchingCourseResult
		WHERE CourseID IN (
			SELECT CX.[Key]
			FROM CONTAINSTABLE(dbo.CourseFreeText, CourseTitle, @CourseExcludeFreeText, LANGUAGE 1033) CX
			UNION ALL
			SELECT CX.[Key]
			FROM CONTAINSTABLE(dbo.CourseFreeText, QualificationTitle, @CourseExcludeFreeText, LANGUAGE 1033) CX
			WHERE @SearchQualificationTitle = 1
			UNION ALL
			SELECT CX.[Key]
			FROM CONTAINSTABLE(dbo.CourseFreeText, CourseSummary, @CourseExcludeFreeText, LANGUAGE 1033) CX
			WHERE @SearchCourseSummary = 1
		);
	END;

	IF (@CourseFreeTextMatch = 1 AND @CourseExcludeFreeText IS NOT NULL)
	BEGIN
		DELETE FROM #MatchingCourseResult
		WHERE CourseID IN (
			SELECT CX.[Key]
			FROM FREETEXTTABLE(dbo.CourseFreeText, CourseTitle, @CourseExcludeFreeText, LANGUAGE 1033) CX
			UNION ALL
			SELECT CX.[Key]
			FROM FREETEXTTABLE(dbo.CourseFreeText, QualificationTitle, @CourseExcludeFreeText, LANGUAGE 1033) CX
			WHERE @SearchQualificationTitle = 1
			UNION ALL
			SELECT CX.[Key]
			FROM FREETEXTTABLE(dbo.CourseFreeText, CourseSummary, @CourseExcludeFreeText, LANGUAGE 1033) CX
			WHERE @SearchCourseSummary = 1
		);
	END;
	
	-- Remove exluded exatch match terms
	IF (@CourseExcludeExactText IS NOT NULL)
	BEGIN
		SET @CourseExcludeExactText = [dbo].[BuildExactSearchTerms](@CourseExcludeExactText);

		DELETE FROM #MatchingCourseResult
		WHERE CourseID IN (
			SELECT CX.[Key]
			FROM CONTAINSTABLE(dbo.CourseFreeText, CourseTitle, @CourseExcludeExactText, LANGUAGE 1033) CX
			UNION ALL
			SELECT CX.[Key]
			FROM CONTAINSTABLE(dbo.CourseFreeText, QualificationTitle, @CourseExcludeExactText, LANGUAGE 1033) CX
			WHERE @SearchQualificationTitle = 1
			UNION ALL
			SELECT CX.[Key]
			FROM CONTAINSTABLE(dbo.CourseFreeText, CourseSummary, @CourseExcludeExactText, LANGUAGE 1033) CX
			WHERE @SearchCourseSummary = 1
		);
	END;

	-- Search for a specific provider, or return all providers
	IF (@ProviderSearchText IS NOT NULL)
	BEGIN
		IF (@ProviderFreeTextMatch = 0)
		BEGIN
			INSERT INTO #MatchingProviderResult
			SELECT P.[ProviderId]
				, PX.[Rank]
			FROM CONTAINSTABLE(dbo.ProviderFreeText, SearchText, @ProviderSearchText, LANGUAGE 1033) PX
				INNER JOIN [dbo].ProviderText PT ON PT.ProviderTextId = PX.[Key]
				INNER JOIN [dbo].[Provider] P ON P.ProviderId = PT.ProviderId
			ORDER BY PX.Rank DESC;
		END;
		
		IF (@ProviderFreeTextMatch = 1)
		BEGIN
			INSERT INTO #MatchingProviderResult
			SELECT P.[ProviderId]
				, PX.[Rank]
			FROM FREETEXTTABLE(dbo.ProviderFreeText, SearchText, @ProviderSearchText, LANGUAGE 1033) PX
				INNER JOIN [dbo].ProviderText PT ON PT.ProviderTextId = PX.[Key]
				INNER JOIN [dbo].[Provider] P ON P.ProviderId = PT.ProviderId
			ORDER BY PX.Rank DESC;
		END;
	END;
	ELSE
		INSERT INTO #MatchingProviderResult
		SELECT ProviderId, 0 FROM dbo.Provider;
 	
	-- Remove any results with a rank <= @CutoffPercentage of the maximum rank
	IF (@CutOffPercentage IS NOT NULL)
	BEGIN
		DECLARE @MaxRank INT = (SELECT Max([Rank]) FROM #MatchingCourseResult);
		DECLARE @MinRank FLOAT = (CAST(@MaxRank as float) / 100) * CAST(@CutOffPercentage as float);

		DELETE FROM #MatchingCourseResult
		WHERE [Rank] <= @MinRank;
	END;

	-- Split Delimited Items into Temp Tables
	CREATE TABLE #QualificationLevels (
		Data NVARCHAR(50) NOT NULL PRIMARY KEY
	);
	IF @QualificationLevel IS NOT NULL
		INSERT INTO #QualificationLevels (Data)
		SELECT Data FROM [dbo].[SplitAndTrim](@QualificationLevel, @splitChar) WHERE Data IS NOT NULL;

	CREATE TABLE #QualificationTypes (
		Data NVARCHAR(50) NOT NULL PRIMARY KEY
	);
	IF @QualificationType IS NOT NULL
		INSERT INTO #QualificationTypes (Data)
		SELECT Data FROM [dbo].[SplitAndTrim](@QualificationType, @splitChar) WHERE Data IS NOT NULL;

	CREATE TABLE #LdcsCategoryCodes  (
		Data NVARCHAR(50) NOT NULL PRIMARY KEY
	);
	IF @LdscCategoryCode IS NOT NULL
		INSERT INTO #LdcsCategoryCodes (Data)
		SELECT Data FROM [dbo].[SplitAndTrim](@LdscCategoryCode, @splitChar) WHERE Data IS NOT NULL;

	CREATE TABLE #StudyModes  (
		Data NVARCHAR(50) NOT NULL PRIMARY KEY
	);
	IF @StudyMode IS NOT NULL
		INSERT INTO #StudyModes (Data)
		SELECT Data FROM [dbo].[SplitAndTrim](@StudyMode, @splitChar) WHERE Data IS NOT NULL;

	CREATE TABLE #AttendanceModes  (
		Data NVARCHAR(50) NOT NULL PRIMARY KEY
	);
	IF @AttendanceMode IS NOT NULL
		INSERT INTO #AttendanceModes (Data)
		SELECT Data FROM [dbo].[SplitAndTrim](@AttendanceMode, @splitChar) WHERE Data IS NOT NULL;

	CREATE TABLE #AttendancePatterns  (
		Data NVARCHAR(50) NOT NULL PRIMARY KEY
	);
	IF @AttendancePattern IS NOT NULL
		INSERT INTO #AttendancePatterns (Data)
		SELECT Data FROM [dbo].[SplitAndTrim](@AttendancePattern, @splitChar) WHERE Data IS NOT NULL;

	CREATE TABLE #A10Codes  (
		Data NVARCHAR(50) NOT NULL PRIMARY KEY
	);
	IF @A10Code IS NOT NULL
		INSERT INTO #A10Codes (Data)
		SELECT Data FROM [dbo].[SplitAndTrim](@A10Code, @splitChar) WHERE Data IS NOT NULL;

	WITH RawData
	AS (
		SELECT ROW_NUMBER() OVER (
				ORDER BY CASE 
						WHEN @SortBy = 'A'
							THEN 1000000 - ([MC].[Rank] - (COALESCE(V.RegionLevelPenalty, 0) * MC.[Rank]))
						WHEN @SortBy = 'S'
							THEN CASE 
									WHEN CISD.StartDate IS NULL
										THEN 401766-- CAST(CAST('31 DEC 2999' AS DATETIME) AS INT)
									ELSE CAST(CISD.StartDate AS INT)
									END
						WHEN @SortBy = 'D'
							THEN CASE 
									WHEN @StartPoint IS NULL
										OR V.[Latitude] IS NULL
										OR V.Longitude IS NULL
										THEN @DummyDistance
									ELSE @StartPoint.STDistance([VG].[Geography]) / 1609.344
									END
						END,
					CASE 
						WHEN @SortBy = 'S'
							THEN StartDateDescription
						ELSE NULL
						END,
					-- Incorporate rank as final sort order
					CASE
						WHEN @SortBy != 'A'
							THEN 1000000 - ([MC].[Rank] - (COALESCE(V.RegionLevelPenalty, 0) * MC.[Rank]))
						ELSE NULL
						END,
					C.CourseId  -- Finally sort by CourseId.  This ensures that if rank, start date and/or distance are the same then the results are returned in a consistent order
				) AS RowNumber,
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
			[CISD].[StartDate],
			[CI].[StartDateDescription],
			[CI].[EndDate],
			[CI].[RegionName],
			[CI].[DurationUnitId],
			[CI].[DurationAsText],
			[CI].[DurationUnitName] AS [DurationUnitBulkUploadRef],
			-- If RegionName is not empty then we have a phantom venue, simulate no join
			CASE WHEN [CI].[RegionName] = '' THEN [V].[VenueName] ELSE NULL END [VenueName],
			CASE WHEN [CI].[RegionName] = '' THEN [V].[AddressLine1] ELSE NULL END [AddressLine1],
			CASE WHEN [CI].[RegionName] = '' THEN [V].[AddressLine2] ELSE NULL END [AddressLine2],
			CASE WHEN [CI].[RegionName] = '' THEN [V].[Town] ELSE NULL END [Town],
			CASE WHEN [CI].[RegionName] = '' THEN [V].[County] ELSE NULL END [County],
			CASE WHEN [CI].[RegionName] = '' THEN [V].[Postcode] ELSE NULL END [Postcode],
			CASE WHEN [CI].[RegionName] = '' THEN [V].[Latitude] ELSE NULL END [Latitude],
			CASE WHEN [CI].[RegionName] = '' THEN [V].[Longitude] ELSE NULL END [Longitude],
			[P].[Loans24Plus],
			[CI].[A10Codes] AS [A10FundingCode],
			[C].[IndependentLivingSkills],
			[C].[SkillsForLife],
			[C].[ErAppStatus],
			[C].[AdultLearnerFundingStatus],
			[CI].[ApplyUntilDate],
			[P].DFE1619Funded AS ProviderDfEFunded,
			[CI].[DfEFunded] AS CourseDfEFunded,
			[P].FEChoices_LearnerDestination,
			[P].FEChoices_LearnerSatisfaction,
			[P].FEChoices_EmployerSatisfaction,
			[MC].[Rank] - (COALESCE([V].RegionLevelPenalty, 0) * [MC].[Rank]) AS [Rank],
			COALESCE([V].RegionLevelPenalty, 0) AS RegionLevelPenalty,
			[VG].[Geography] AS GeoPoint
		FROM #MatchingCourseResult MC
			INNER JOIN [dbo].[Course] C ON C.CourseId = MC.CourseId
			INNER JOIN [dbo].CourseInstance CI ON CI.CourseId = C.CourseId
			INNER JOIN [dbo].[Provider] P ON P.ProviderId = C.ProviderId
			INNER JOIN #MatchingProviderResult MP ON MP.ProviderId = P.ProviderId
			LEFT OUTER JOIN [dbo].Venue V ON V.VenueId = CI.VenueId
			LEFT OUTER JOIN [dbo].VenueGeography VG ON VG.VenueId = V.VenueId
			LEFT OUTER JOIN (SELECT CourseInstanceId, Min(StartDate) AS StartDate FROM [dbo].CourseInstanceStartDate WHERE StartDate >= COALESCE(@EarliestStartDate, @OneYearAgo) GROUP BY CourseInstanceId) CISD ON CISD.CourseInstanceId = CI.CourseInstanceId
		WHERE (
					@QualificationType IS NULL
					OR C.QualificationTypeRef IN (SELECT Data FROM #QualificationTypes)
			  )
			AND (
					@QualificationLevel IS NULL
					OR C.QualificationBulkUploadRef IN (SELECT Data FROM #QualificationLevels)
				)
			AND (
					@LdscCategoryCode IS NULL
					OR C.[LDCS1] IN (SELECT Data FROM #LdcsCategoryCodes)
					OR C.[LDCS2] IN (SELECT Data FROM #LdcsCategoryCodes)
					OR C.[LDCS3] IN (SELECT Data FROM #LdcsCategoryCodes)
					OR C.[LDCS4] IN (SELECT Data FROM #LdcsCategoryCodes)
					OR C.[LDCS5] IN (SELECT Data FROM #LdcsCategoryCodes)
				)
			AND (
					@StudyMode IS NULL
					OR CI.StudyModeBulkUploadRef IN (SELECT Data FROM #StudyModes)
				)
			AND (
					@AttendanceMode IS NULL
					OR CI.AttendanceModeBulkUploadRef IN (SELECT Data FROM #AttendanceModes)
				)
			AND (
					@AttendancePattern IS NULL
					OR CI.AttendancePatternBulkUploadRef IN (SELECT Data FROM #AttendancePatterns)
				)
			AND (
					@AppClosed IS NULL
					OR CI.[ApplyUntilDate] < @AppClosed
				)
			AND (
					@A10Code IS NULL
					OR CI.CourseInstanceId IN (
						SELECT DISTINCT CourseInstanceId
						FROM CourseInstanceA10FundingCode
						WHERE A10FundingCode IN (SELECT Data FROM #A10Codes)
					)
				)
			AND (
					@EarliestStartDate IS NULL
					OR @FlexiStartDate = 1
					OR CISD.StartDate >= @EarliestStartDate
				)
			AND (
					@StartPoint IS NOT NULL
					OR @Location IS NULL
					-- This is 2x faster than joining on @MatchingVenueResults
					OR [V].[VenueId] IN (SELECT VenueId FROM #MatchingVenueResult)
				)
			AND (
					@Distance IS NULL
					OR @Distance >= CASE 
						WHEN @StartPoint IS NULL
							OR V.Latitude IS NULL
							OR V.Longitude IS NULL
							THEN @DummyDistance
						ELSE @StartPoint.STDistance(VG.[Geography]) / 1609.344
						END
				)
			AND (
					@PublicAPI = 0
					OR C.ApplicationId != 3
				)
			AND (
					@ProviderId IS NULL
					OR P.ProviderId = @ProviderId
				)
			AND (
					@DFEFunded IS NULL
					OR CI.DfEFunded = @DFEFunded
				)
	)
	, TotalRawDataRecords
	AS (
			SELECT Count(*) AS RecordCount
			FROM RawData
		)
	SELECT RowNumber,
		[Rank],
		[RegionLevelPenalty],
		CASE WHEN @StartPoint IS NULL
				OR Latitude IS NULL
				OR Longitude IS NULL
				THEN NULL
			 ELSE Convert(DECIMAL(7, 3), @StartPoint.STDistance(GeoPoint) / 1609.344)
			 END AS Distance,
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
		ProviderDfEFunded,
		FEChoices_LearnerDestination,
		FEChoices_LearnerSatisfaction,
		FEChoices_EmployerSatisfaction,
		CourseDfEFunded,
		RecordCount
	FROM RawData,
		TotalRawDataRecords
	WHERE RowNumber BETWEEN @FromRecord AND @ToRecord
	ORDER BY RowNumber;

	-- Not Strictly Necessary But Tidy Up Anyway
	DROP TABLE #MatchingCourseResult;
	DROP TABLE #MatchingCourseResultTemp;
	DROP TABLE #MatchingProviderResult;
	DROP TABLE #MatchingVenueResult;

	DROP TABLE #A10Codes;
	DROP TABLE #AttendanceModes;
	DROP TABLE #AttendancePatterns;
	DROP TABLE #LdcsCategoryCodes;
	DROP TABLE #QualificationLevels;
	DROP TABLE #QualificationTypes;
	DROP TABLE #StudyModes;

END;