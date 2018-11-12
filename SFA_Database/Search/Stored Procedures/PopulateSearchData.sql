CREATE PROCEDURE [Search].[PopulateSearchData]
AS
BEGIN

	DECLARE @LogId int;

	DECLARE @NCSExportEnabled BIT = 0;
	SELECT @NCSExportEnabled = IsNull(Value, ValueDefault) FROM [dbo].ConfigurationSettings WHERE Name = 'NCSExportEnabled';
	IF (@NCSExportEnabled = 0)
	BEGIN
		INSERT INTO [Search].[DataExportLog] ([ExportType], [ExecutedOn], [IsSuccessful], [IsValidDataImport]) VALUES ('Courses', GetUtcDate(), 0, 0);
		SET @LogId = SCOPE_IDENTITY();

		INSERT INTO [Search].[DataExportException] ([DataExportLogId],[ExceptionDetails]) VALUES (@LogId, 'NCS Data Export is Disabled');

		RETURN 0;
	END;

	DECLARE @IncludeUCASData BIT = 0;
	SELECT @IncludeUCASData = IsNull(Value, ValueDefault) FROM [dbo].ConfigurationSettings WHERE Name = 'NCSExportIncludeUCASData';
	
	DECLARE @IncludeUCAS_PG_Data BIT = 0;
	SELECT @IncludeUCAS_PG_Data = IsNull(Value, ValueDefault) FROM [dbo].ConfigurationSettings WHERE Name = 'NCSExportIncludeUCAS-PG-Data';

	DECLARE @OldProviderCount FLOAT = 0;
	DECLARE @NewProviderCount FLOAT = 0;

	DECLARE @OldProviderTextCount FLOAT = 0;
	DECLARE @NewProviderTextCount FLOAT = 0;

	DECLARE @OldVenueCount FLOAT = 0;
	DECLARE @NewVenueCount FLOAT = 0;

	DECLARE @OldCourseCount FLOAT = 0;
	DECLARE @NewCourseCount FLOAT = 0;

	DECLARE @OldCourseTextCount FLOAT = 0;
	DECLARE @NewCourseTextCount FLOAT = 0;

	DECLARE @OldCourseInstanceCount FLOAT = 0;
	DECLARE @NewCourseInstanceCount FLOAT = 0;

	DECLARE @OldCourseInstanceA10Count FLOAT = 0;
	DECLARE @NewCourseInstanceA10Count FLOAT = 0;

	DECLARE @OldCourseInstanceStartDateCount FLOAT = 0;
	DECLARE @NewCourseInstanceStartDateCount FLOAT = 0;

	DECLARE @OldCategoryCodeCount FLOAT = 0;
	DECLARE @NewCategoryCodeCount FLOAT	= 0;
	 
	DECLARE @IsValidInput BIT = 0;

	DECLARE @WorldId int = (SELECT VenueLocationId FROM VenueLocation WHERE LocationName = 'WORLD');
	DECLARE @RegionLevelPenalty FLOAT = (SELECT CASE WHEN [Value] IS NULL THEN [ValueDefault] ELSE [Value] END FROM ConfigurationSettings WHERE [Name] = 'SearchAPIRegionLevelPenalty');
	DECLARE @MaxRegionLevelPenalty FLOAT = (SELECT CASE WHEN [Value] IS NULL THEN [ValueDefault] ELSE [Value] END FROM ConfigurationSettings WHERE [Name] = 'SearchAPIMaxRegionLevelPenalty');
 	DECLARE @OneYearAgo DATE = CAST(DATEADD(YEAR, -1, GETUTCDATE()) AS DATE);

	BEGIN TRY
 	 
		BEGIN TRANSACTION;

		/** Provider List **/
		SELECT @OldProviderCount = Count(*) FROM [Search].[Provider] WHERE (@IncludeUCASData = 1 AND ProviderId < 0 AND ApplicationId = 3) OR (@IncludeUCAS_PG_Data = 1 AND ProviderId > 0 AND ApplicationId = 3) OR ApplicationId <> 3;
		TRUNCATE TABLE [Search].[Provider];
		INSERT INTO [Search].[Provider]
			   ([ProviderId]
			   ,[ProviderName]
			   ,[Ukprn]
			   ,[ProviderTypeId]
			   ,[Email]
			   ,[Website]
			   ,[Telephone]
			   ,[Fax]
			   ,[TradingName]
			   ,[LegalName]
			   ,[UPIN]
			   ,[ProviderNameAlias]
			   ,[CreatedDateTimeUtc]
			   ,[ModifiedDateTimeUtc]
			   ,[Loans24Plus]
			   ,[RecordStatusId]
			   ,[CreatedByUserId]
			   ,[ModifiedByUserId]
			   ,[AddressLine1]
			   ,[AddressLine2]
			   ,[Town]
			   ,[County]
			   ,[Postcode]
			   ,[ApplicationId]
			   ,[DFE1619Funded]
			   ,[SFAFunded]
			   ,[FEChoices_LearnerDestination]
			   ,[FEChoices_LearnerSatisfaction]
			   ,[FEChoices_EmployerSatisfaction])
			EXEC [search].[ProviderList];

		-- Insert UCAS Data
		IF (@IncludeUCASData = 1)
		BEGIN
			INSERT INTO [Search].[Provider]
				   ([ProviderId]
				   ,[ProviderName]
				   ,[Ukprn]
				   ,[ProviderTypeId]
				   ,[Email]
				   ,[Website]
				   ,[Telephone]
				   ,[Fax]
				   ,[TradingName]
				   ,[LegalName]
				   ,[UPIN]
				   ,[ProviderNameAlias]
				   ,[CreatedDateTimeUtc]
				   ,[ModifiedDateTimeUtc]
				   ,[Loans24Plus]
				   ,[RecordStatusId]
				   ,[CreatedByUserId]
				   ,[ModifiedByUserId]
				   ,[AddressLine1]
				   ,[AddressLine2]
				   ,[Town]
				   ,[County]
				   ,[Postcode]
				   ,[ApplicationId]
				   ,[DFE1619Funded]
				   ,[SFAFunded]
				   ,[FEChoices_LearnerDestination]
				   ,[FEChoices_LearnerSatisfaction]
				   ,[FEChoices_EmployerSatisfaction])
				SELECT OrgId * (-1),
					OrgName,
					COALESCE(UKPRN, 0),
					0,
					Email,
					Web,
					Phone,
					Fax,
					Null,
					OrgName,
					Null,
					Null,
					CreatedDateTimeUtc,
					Null,
					0,
					2,
					CreatedByUserId,
					Null,
					Address1,
					Address2,
					T.Town,
					Null,
					Postcode,
					3,
					0,
					0,
					Null,
					Null,
					Null
				FROM [UCAS].[Orgs] O
					INNER JOIN [UCAS].[Towns] T ON T.TownId = O.TownId;
		END;
		
		-- Insert UCAS PG Data
		IF (@IncludeUCAS_PG_Data = 1)
		BEGIN
			INSERT INTO [Search].[Provider]
				   ([ProviderId]
				   ,[ProviderName]
				   ,[Ukprn]
				   ,[ProviderTypeId]
				   ,[Email]
				   ,[Website]
				   ,[Telephone]
				   ,[Fax]
				   ,[TradingName]
				   ,[LegalName]
				   ,[UPIN]
				   ,[ProviderNameAlias]
				   ,[CreatedDateTimeUtc]
				   ,[ModifiedDateTimeUtc]
				   ,[Loans24Plus]
				   ,[RecordStatusId]
				   ,[CreatedByUserId]
				   ,[ModifiedByUserId]
				   ,[AddressLine1]
				   ,[AddressLine2]
				   ,[Town]
				   ,[County]
				   ,[Postcode]
				   ,[ApplicationId]
				   ,[DFE1619Funded]
				   ,[SFAFunded]
				   ,[FEChoices_LearnerDestination]
				   ,[FEChoices_LearnerSatisfaction]
				   ,[FEChoices_EmployerSatisfaction])
				SELECT ProviderId,
					ProviderName,
					0, --COALESCE(UKPRN, 0),  -- TODO: Get UKPRN
					0,
					ContactEmail,
					Website,
					ContactPhone,
					ContactFax,
					Null,
					ProviderName,
					Null,
					Null,
					CreatedDateTimeUtc,
					Null,
					0,
					2,
					CreatedByUserId,
					Null,
					Address1,
					Address2,
					Address3,
					Address4,
					Postcode,
					3,
					0,
					0,
					Null,
					Null,
					Null
				FROM [UCAS_PG].[Provider];
		END;
		SELECT @NewProviderCount = Count(*) FROM [Search].[Provider];


		/**Provider Text search**/
		SELECT @OldProviderTextCount = Count(*) FROM [Search].[ProviderText] WHERE (@IncludeUCASData = 1 AND ProviderId < 0 AND IsUCASData = 3) OR (@IncludeUCAS_PG_Data = 1 AND ProviderId > 0 AND IsUCASData = 3) OR IsUCASData = 0;
		TRUNCATE TABLE [Search].[ProviderText];
		INSERT INTO [Search].[ProviderText]
			   ([ProviderId]
			   ,[OrganisationId]
			   ,[SearchText]
			   ,[ProviderName]
			   ,[IsUCASData])
			EXEC  [search].[ProviderTextSearchList];

		SELECT @NewProviderTextCount = Count(*) FROM [Search].[ProviderText];

		/** Course Instance A10 Codes **/
		SELECT @OldCourseInstanceA10Count = Count(*) FROM [Search].[CourseInstanceA10FundingCode];
		TRUNCATE TABLE [Search].[CourseInstanceA10FundingCode];
		INSERT INTO [Search].[CourseInstanceA10FundingCode]
			   ([CourseInstanceId]
			   ,[A10FundingCode])
			EXEC [Search].[CourseInstanceA10Codes];
	
		SELECT @NewCourseInstanceA10Count = Count(*) FROM [Search].[CourseInstanceA10FundingCode];

		/**Course Intance start date**/
		SELECT @OldCourseInstanceStartDateCount = Count(*) FROM [Search].[CourseInstanceStartDate] CISD INNER JOIN [Search].[CourseInstance] CI ON CI.CourseInstanceId = CISD.CourseInstanceId WHERE (@IncludeUCASData = 1 AND CI.CourseInstanceId < 0 AND ApplicationId = 3) OR (@IncludeUCAS_PG_Data = 1 AND CI.CourseInstanceId > 0 AND ApplicationId = 3) OR ApplicationId <> 3;
		TRUNCATE TABLE [Search].[CourseInstanceStartDate];
		INSERT INTO [Search].[CourseInstanceStartDate]
			   ([CourseInstanceId]
			   ,[StartDate]
			   ,[PlacesAvailable]
			   ,[DateFormat])
			EXEC [Search].[CourseInstanceStartDatesList];

		-- Include UCAS Data
		IF (@IncludeUCASData = 1)
		BEGIN
			INSERT INTO [Search].[CourseInstanceStartDate]
				   ([CourseInstanceId]
				   ,[StartDate]
				   ,[PlacesAvailable]
				   ,[DateFormat])
			-- Need to generate a key from 2 ints that
			-- 1) Is repeatable
			-- 2) Still fits in an int
			-- 3) Is unique
			SELECT (SI.StartIndexId + CI.CourseIndexId + ROW_NUMBER() OVER (PARTITION BY SI.StartIndexId + CI.CourseIndexId ORDER BY SI.StartIndexId + CI.CourseIndexId) * 1000000) * (-1) AS CourseInstanceId,
				S.StartDate,
				C.NoOfPlaces,
				'DD-Mon-RR' AS DateFormat
			FROM [UCAS].Starts S
				INNER JOIN [UCAS].StartsIndex SI ON SI.StartId = S.StartId
				INNER JOIN [UCAS].[CoursesIndex] CI ON CI.CourseId = S.CourseId
				LEFT OUTER JOIN [UCAS].Courses C ON C.CourseId = CI.CourseId
			WHERE S.StartDate IS NOT NULL
		END;

		-- Include UCAS PG Data
		IF (@IncludeUCAS_PG_Data = 1)
		BEGIN
			INSERT INTO [Search].[CourseInstanceStartDate]
				   ([CourseInstanceId]
				   ,[StartDate]
				   ,[PlacesAvailable]
				   ,[DateFormat])
			SELECT CO.CourseOptionId,
				CO.StartDate,
				Null, -- C.NoOfPlaces,  -- TODO: Get Number of places
				'DD-Mon-RR' AS DateFormat
			FROM [UCAS_PG].[CourseOption] CO
				LEFT OUTER JOIN [UCAS_PG].Course C ON C.CourseId = CO.CourseId
			WHERE CO.StartDate IS NOT NULL
		END;

		SELECT @NewCourseInstanceStartDateCount = Count(*) FROM [Search].[CourseInstanceStartDate];

			/**Course List**/
		SELECT @OldCourseCount = Count(*) FROM [Search].[Course] WHERE (@IncludeUCASData = 1 AND CourseId < 0 AND ApplicationId = 3) OR (@IncludeUCAS_PG_Data = 1 AND CourseId > 0 AND ApplicationId = 3) OR ApplicationId <> 3;
		TRUNCATE TABLE [Search].[Course];
		INSERT INTO [Search].[Course]
			   ([CourseId]
			   ,[ProviderId]
			   ,[LearningAimRef]
			   ,[CourseTitle]
			   ,[CourseSummary]
			   ,[ProviderOwnCourseRef]
			   ,[Url]
			   ,[BookingUrl]
			   ,[EntryRequirements]
			   ,[AssessmentMethod]
			   ,[EquipmentRequired]
			   ,[QualificationTitle]
			   ,[QualificationBulkUploadRef]
			   ,[QualificationLevelName]
			   ,[LDCS1]
			   ,[LDCS2]
			   ,[LDCS3]
			   ,[LDCS4]
			   ,[LDCS5]
			   ,[ApplicationId]
			   ,[UcasTariffPoints]
			   ,[QualificationRefAuthority]
			   ,[QualificationRef]
			   ,[CourseTypeId]
			   ,[CreatedDateTimeUtc]
			   ,[ModifiedDateTimeUtc]
			   ,[RecordStatusId]
			   ,[AwardingOrganisationName]
			   ,[CreatedByUserId]
			   ,[ModifiedByUserId]
			   ,[QualificationTypeRef]
			   ,[QualificationTypeName]
			   ,[QualificationDataType]
			   ,[PrimaryApplicationId]
				,[ErAppStatus]              
				,[SkillsForLife]            
				,[Loans24Plus]              
				,[AdultLearnerFundingStatus]
				,[OtherFundingStatus]       
				,[IndependentLivingSkills]  
			   )
			EXEC [search].[CourseList];

		-- Include UCAS Data
		IF (@IncludeUCASData = 1)
		BEGIN
			
			INSERT INTO [Search].[Course]
				   ([CourseId]
				   ,[ProviderId]
				   ,[LearningAimRef]
				   ,[CourseTitle]
				   ,[CourseSummary]
				   ,[ProviderOwnCourseRef]
				   ,[Url]
				   ,[BookingUrl]
				   ,[EntryRequirements]
				   ,[AssessmentMethod]
				   ,[EquipmentRequired]
				   ,[QualificationTitle]
				   ,[QualificationBulkUploadRef]
				   ,[QualificationLevelName]
				   ,[LDCS1]
				   ,[LDCS2]
				   ,[LDCS3]
				   ,[LDCS4]
				   ,[LDCS5]
				   ,[ApplicationId]
				   ,[UcasTariffPoints]
				   ,[QualificationRefAuthority]
				   ,[QualificationRef]
				   ,[CourseTypeId]
				   ,[CreatedDateTimeUtc]
				   ,[ModifiedDateTimeUtc]
				   ,[RecordStatusId]
				   ,[AwardingOrganisationName]
				   ,[CreatedByUserId]
				   ,[ModifiedByUserId]
				   ,[QualificationTypeRef]
				   ,[QualificationTypeName]
				   ,[QualificationDataType]
				   ,[PrimaryApplicationId]
					,[ErAppStatus]              
					,[SkillsForLife]            
					,[Loans24Plus]              
					,[AdultLearnerFundingStatus]
					,[OtherFundingStatus]       
					,[IndependentLivingSkills]  
				   )
			SELECT CourseIndexId * (-1),
				OrgId * (-1),
				Null,
				C.CourseTitle,
				LEFT(C.Summary, 2000),
				Null,
				Null,
				null,
				LEFT(AdditionalEntry, 4000),
				LEFT(AssessmentMethods, 4000),
				Null,
				Q.Qualification,
				Null,
				Null,
				LDCS1.LearnDirectClassificationRef,
				LDCS2.LearnDirectClassificationRef,
				LDCS3.LearnDirectClassificationRef,
				Null,
				Null,
				3,
				Null,
				Null,
				Null,
				Null,
				C.CreatedDateTimeUtc,
				null,
				2,
				Null,
				C.CreatedByUserId,
				Null,
				Null,
				Null,
				'',
				3,
				Null,
				Null,
				Null,
				Null,
				Null,
				Null
			FROM [UCAS].[Courses] C
				INNER JOIN (SELECT CourseId, QualificationId, Max(CourseIndexid) AS CourseIndexId FROM [UCAS].[CoursesIndex] GROUP BY CourseId, QualificationId) CI ON CI.CourseId = C.CourseId
				LEFT OUTER JOIN [UCAS].[Qualifications] Q ON Q.QualificationId = CI.QualificationId
				LEFT OUTER JOIN [dbo].[LearnDirectClassificationToJACS3Mapping] LDCS1 ON LDCS1.JACS3 = C.HESA1
				LEFT OUTER JOIN [dbo].[LearnDirectClassificationToJACS3Mapping] LDCS2 ON LDCS2.JACS3 = C.HESA2
				LEFT OUTER JOIN [dbo].[LearnDirectClassificationToJACS3Mapping] LDCS3 ON LDCS3.JACS3 = C.HESA3

		END;

		-- Include UCAS PG Data
		IF (@IncludeUCAS_PG_Data = 1)
		BEGIN
			INSERT INTO [Search].[Course]
				   ([CourseId]
				   ,[ProviderId]
				   ,[LearningAimRef]
				   ,[CourseTitle]
				   ,[CourseSummary]
				   ,[ProviderOwnCourseRef]
				   ,[Url]
				   ,[BookingUrl]
				   ,[EntryRequirements]
				   ,[AssessmentMethod]
				   ,[EquipmentRequired]
				   ,[QualificationTitle]
				   ,[QualificationBulkUploadRef]
				   ,[QualificationLevelName]
				   ,[LDCS1]
				   ,[LDCS2]
				   ,[LDCS3]
				   ,[LDCS4]
				   ,[LDCS5]
				   ,[ApplicationId]
				   ,[UcasTariffPoints]
				   ,[QualificationRefAuthority]
				   ,[QualificationRef]
				   ,[CourseTypeId]
				   ,[CreatedDateTimeUtc]
				   ,[ModifiedDateTimeUtc]
				   ,[RecordStatusId]
				   ,[AwardingOrganisationName]
				   ,[CreatedByUserId]
				   ,[ModifiedByUserId]
				   ,[QualificationTypeRef]
				   ,[QualificationTypeName]
				   ,[QualificationDataType]
				   ,[PrimaryApplicationId]
					,[ErAppStatus]              
					,[SkillsForLife]            
					,[Loans24Plus]              
					,[AdultLearnerFundingStatus]
					,[OtherFundingStatus]       
					,[IndependentLivingSkills]  
				   )
			SELECT Max(CO.CourseOptionId) AS CourseId,
				C.ProviderId,
				Null AS LearningAimRef,
				C.CourseTitle As CourseTitle,
				LEFT(C.CourseSummary, 2000) As CourseSummary,
				Null AS ProviderOwnCourseRef,
				Null AS Url,
				Null AS BookingUrl,
				LEFT(CO.EntryRequirements, 4000) AS EntryRequirements,
				LEFT(CO.AssessmentMethods, 4000) AS AssessmentMethod,
				Null AS EquipmentRequired,
				CO.Qualification AS QualificationTitle,
				Null AS QualificationBulkUploadRef,
				CO.QualificationLevel AS QualificationLevelName,
				Null AS LDCS1, -- LDCS1.LearnDirectClassificationRef,	-- TODO: Get LDCS Codes
				Null AS LDCS2, -- LDCS2.LearnDirectClassificationRef,
				Null AS LDCS3, -- LDCS3.LearnDirectClassificationRef,
				Null AS LDCS4,
				Null AS LDCS5,
				3 AS ApplicationId,
				Null AS UCASTariffPoints,
				Null AS QualificationRefAuthority,
				Null AS QualificationRef,
				Null AS CourseTypeId,
				Min(C.CreatedDateTimeUtc) AS CreatedDateTimeUtc,
				Null AS ModifiedDateTimeUtc,
				2 AS RecordStatusId,
				Null AS AwardingOrganisationName,
				Min(C.CreatedByUserId) AS CreatedByUserId,
				Null AS ModifiedByUserId,
				Null AS QualificationTypeRef,
				Null AS QualificationDataName,
				'' AS QualificationDataType,
				3 AS PrimaryApplicationId,
				Null AS ErAppStatus,
				Null AS SkillsForLife,
				Null AS Loans24Plus,
				Null AS AdultLearnerFundingStatus,
				Null AS OtherFundingStatus,
				Null AS IndependentLivingSkills
			FROM [UCAS_PG].[Course] C
				INNER JOIN [UCAS_PG].[CourseOption] CO ON CO.CourseId = C.CourseId
			GROUP BY C.ProviderId,
				C.CourseTitle,
				C.CourseSummary,
				CO.EntryRequirements,
				CO.AssessmentMethods,
				CO.Qualification,
				CO.QualificationLevel
			ORDER BY Min(CO.CourseOptionId);

		END;

		SELECT @NewCourseCount = Count(*) FROM [Search].[Course];

		/*CourseInstance*/
		SELECT @OldCourseInstanceCount = Count(*) FROM [Search].[CourseInstance] WHERE (@IncludeUCASData = 1 AND CourseInstanceId < 0 AND ApplicationId = 3) OR (@IncludeUCAS_PG_Data = 1 AND CourseInstanceId > 0 AND ApplicationId = 3) OR ApplicationId <> 3;

		TRUNCATE TABLE [Search].[CourseInstance];

		INSERT INTO [Search].[CourseInstance]
			   ([CourseInstanceId]
			   ,[ProviderOwnCourseInstanceRef]
			   ,[Price]
			   ,[PriceAsText]
			   ,[DurationUnitId]
			   ,[DurationUnitBulkUploadRef]
			   ,[DurationUnitName]
			   ,[DurationAsText]
			   ,[StartDateDescription]
			   ,[EndDate]
			   ,[StudyModeBulkUploadRef]
			   ,[StudyModeName]
			   ,[AttendanceModeBulkUploadRef]
			   ,[AttendanceModeName]
			   ,[AttendancePatternBulkUploadRef]
			   ,[AttendancePatternName]
			   ,[LanguageOfInstruction]
			   ,[LanguageOfAssessment]
			   ,[PlacesAvailable]
			   ,[EnquiryTo]
			   ,[ApplyTo]
			   ,[ApplyFromDate]
			   ,[ApplyUntilDate]
			   ,[ApplyUntilText]
			   ,[Url]
			   ,[TimeTable]
			   ,[CourseId]
			   ,[VenueId]
			   ,[CanApplyAllYear]
			   ,[RegionName]
			   ,[CreatedDateTimeUtc]
			   ,[ModifiedDateTimeUtc]
			   ,[RecordStatusId]
			   ,[CreatedByUserId]
			   ,[ModifiedByUserId]
			   ,[CourseInstanceSummary]
			   ,[ProviderRegionId]
			   ,[ApplicationId]
			   ,[OfferedByOrganisationId]
			   ,[OfferedByProviderId] 
			   ,[StartDate]
			   ,[A10Codes]
			   ,[DfEFunded]
			   ,[VenueLocationId])
				EXEC [Search].[CourseInstanceList];

		-- Include UCAS Data
		IF (@IncludeUCASData = 1)
		BEGIN
			INSERT INTO [Search].[CourseInstance]
				   ([CourseInstanceId]
				   ,[ProviderOwnCourseInstanceRef]
				   ,[Price]
				   ,[PriceAsText]
				   ,[DurationUnitId]
				   ,[DurationUnitBulkUploadRef]
				   ,[DurationUnitName]
				   ,[DurationAsText]
				   ,[StartDateDescription]
				   ,[EndDate]
				   ,[StudyModeBulkUploadRef]
				   ,[StudyModeName]
				   ,[AttendanceModeBulkUploadRef]
				   ,[AttendanceModeName]
				   ,[AttendancePatternBulkUploadRef]
				   ,[AttendancePatternName]
				   ,[LanguageOfInstruction]
				   ,[LanguageOfAssessment]
				   ,[PlacesAvailable]
				   ,[EnquiryTo]
				   ,[ApplyTo]
				   ,[ApplyFromDate]
				   ,[ApplyUntilDate]
				   ,[ApplyUntilText]
				   ,[Url]
				   ,[TimeTable]
				   ,[CourseId]
				   ,[VenueId]
				   ,[CanApplyAllYear]
				   ,[RegionName]
				   ,[CreatedDateTimeUtc]
				   ,[ModifiedDateTimeUtc]
				   ,[RecordStatusId]
				   ,[CreatedByUserId]
				   ,[ModifiedByUserId]
				   ,[CourseInstanceSummary]
				   ,[ProviderRegionId]
				   ,[ApplicationId]
				   ,[OfferedByOrganisationId]
				   ,[OfferedByProviderId] 
				   ,[StartDate]
				   ,[A10Codes]
				   ,[DfEFunded])
			-- Need to generate a key from 2 ints that
			-- 1) Is repeatable
			-- 2) Still fits in an int
			-- 3) Is unique
			SELECT (SI.StartIndexId + CI.CourseIndexId + ROW_NUMBER() OVER (PARTITION BY SI.StartIndexId + CI.CourseIndexId ORDER BY SI.StartIndexId + CI.CourseIndexId) * 1000000) * (-1) AS CourseInstanceId,
				Null AS ProviderOwnCourseInstanceRef,
				F.Price,
				F.PriceDescription AS PriceAsText,
				CI.MinDuration, --CI.DurationUnitId,
				DU.BulkUploadRef AS DurationBulkUploadRef,
				DU.DurationUnitName,
				Null AS DurationAsText,
				S.StartDescription,
				Null AS EndDate,
				COALESCE(SM.BulkUploadRef, '') AS StudyModeBulkUploadRef,
				SM.StudyModeName,
				COALESCE(AT.BulkUploadRef, '') AS AttendanceModeBulkUploadRef,
				AT.AttendanceTypeName AS AttendanceModeName,
				COALESCE(AP.BulkUploadRef, '') AS AttendancePatternBulkUploadRef,
				AP.AttendancePatternName,
				Null AS LanguageOfInstruction,
				Null AS LanguageOfAssessment,
				C.NoOfPlaces AS PlacesAvailable,
				Null AS EnquiryTo,
				Null AS ApplyTo,
				Null AS ApplyFromDate,
				Null AS ApplyUntilDate,
				Null AS ApplyUntilText,
				Null AS Url,
				Null AS TimeTable,
				CQ.CourseIndexId * (-1) AS CourseId,
				SI.PlaceOfStudyId * (-1) AS VenueId,
				0 AS CanApplyAllYear,
				'' AS RegionName,
				C.CreatedDateTimeUtc,
				Null AS ModifiedDateTimeUtc,
				2 AS RecordStatusId,
				C.CreatedByUserId,
				Null AS ModifiedByUserId,
				dbo.GetCourseInstanceSummaryText
					(
						DU.DurationUnitId, 
						DU.DurationUnitName, 
						'', 
						SM.StudyModeName, 
						F.Price, 
						F.PriceDescription, 
						S.StartDate,
						S.StartDescription, 
						POS.PlaceOfStudy, 
						'', 
						''
					) AS CourseInstanceSummary,
				Null AS ProviderRegionId,
				3 AS ApplicationId,
				Null AS OfferedByOrganisationId,
				Null AS OfferedByProviderId,
				S.StartDate,
				Null AS A10Codes,
				0 AS DFEFunded
			FROM [UCAS].Starts S
				INNER JOIN [UCAS].StartsIndex SI ON SI.StartId = S.StartId
				INNER JOIN (SELECT CourseId, QualificationId, Max(CourseIndexId) AS CourseIndexId FROM [UCAS].[CoursesIndex] GROUP BY CourseId, QualificationId) CQ ON CQ.CourseId = S.CourseId
				INNER JOIN [UCAS].[CoursesIndex] CI ON CI.CourseId = S.CourseId
				INNER JOIN [UCAS].Courses C ON C.CourseId = CI.CourseId
				LEFT OUTER JOIN [UCAS].[PlacesOfStudy] POS ON POS.PlaceOfStudyId = SI.PlaceOfStudyId
				LEFT OUTER JOIN (
									SELECT DISTINCT F.CourseIndexId,
										CASE WHEN Count(*) = 1 THEN Min(Fee) ELSE Null END AS Price,
										REPLACE(Min(FeeText), '~', Char(13) + Char(10)) AS PriceDescription
									FROM [UCAS].Fees F
										LEFT OUTER JOIN (
															SELECT CourseIndexId,
																(
																	SELECT FeeYear + ' ' + Currency + CAST(Fee AS VARCHAR(10)) + '~' AS [text()] 
																	FROM [UCAS].Fees F1
																		LEFT OUTER JOIN [UCAS].Currencies C1 ON C1.CurrencyId = F1.CurrencyId
																		LEFT OUTER JOIN [UCAS].FeeYears FY1 ON FY1.FeeYearId = F1.FeeYearId
																	WHERE F1.CourseIndexId = F.CourseIndexId
																	ORDER BY F1.CourseIndexId
																	FOR XML PATH ('')
																) FeeText
															FROM [UCAS].Fees F
																LEFT OUTER JOIN [UCAS].Currencies C ON C.CurrencyId = F.CurrencyId
																LEFT OUTER JOIN [UCAS].FeeYears FY ON FY.FeeYearId = F.FeeYearId
															GROUP BY CourseIndexId
														) FT ON FT.CourseIndexId = F.CourseIndexId
									GROUP BY F.CourseIndexId
								) F ON F.CourseIndexId = CI.CourseIndexId
				LEFT OUTER JOIN [dbo].UcasStudyModeMapping UCASMapping ON UCASMapping.UcasStudyModeId = CI.StudyModeId
				LEFT OUTER JOIN [dbo].StudyMode SM ON SM.StudyModeId = UCASMapping.MapsToStudyModeId
				LEFT OUTER JOIN [dbo].AttendanceType AT ON AT.AttendanceTypeId = UCASMapping.MapsToAttendanceTypeId
				LEFT OUTER JOIN [dbo].AttendancePattern AP ON AP.AttendancePatternId = UCASMapping.MapsToAttendancePattern
				LEFT OUTER JOIN [dbo].DurationUnit DU ON DU.DurationUnitId = CASE WHEN CI.DurationId = 1 THEN 3 -- Weeks
																				  WHEN CI.DurationId = 2 THEN 4 -- Months
																				  WHEN CI.DurationId = 3 THEN 7 -- Years
																				  WHEN CI.DurationId = 5 THEN 6 -- Semesters
																				  WHEN CI.DurationId = 6 THEN 5 -- Terms
																				  WHEN CI.DurationId = 7 THEN 7 -- Calendar Years
																				  ELSE Null END;
		END;

		-- Include UCAS PG Data
		IF (@IncludeUCAS_PG_Data = 1)
		BEGIN
			INSERT INTO [Search].[CourseInstance]
				   ([CourseInstanceId]
				   ,[ProviderOwnCourseInstanceRef]
				   ,[Price]
				   ,[PriceAsText]
				   ,[DurationUnitId]
				   ,[DurationUnitBulkUploadRef]
				   ,[DurationUnitName]
				   ,[DurationAsText]
				   ,[StartDateDescription]
				   ,[EndDate]
				   ,[StudyModeBulkUploadRef]
				   ,[StudyModeName]
				   ,[AttendanceModeBulkUploadRef]
				   ,[AttendanceModeName]
				   ,[AttendancePatternBulkUploadRef]
				   ,[AttendancePatternName]
				   ,[LanguageOfInstruction]
				   ,[LanguageOfAssessment]
				   ,[PlacesAvailable]
				   ,[EnquiryTo]
				   ,[ApplyTo]
				   ,[ApplyFromDate]
				   ,[ApplyUntilDate]
				   ,[ApplyUntilText]
				   ,[Url]
				   ,[TimeTable]
				   ,[CourseId]
				   ,[VenueId]
				   ,[CanApplyAllYear]
				   ,[RegionName]
				   ,[CreatedDateTimeUtc]
				   ,[ModifiedDateTimeUtc]
				   ,[RecordStatusId]
				   ,[CreatedByUserId]
				   ,[ModifiedByUserId]
				   ,[CourseInstanceSummary]
				   ,[ProviderRegionId]
				   ,[ApplicationId]
				   ,[OfferedByOrganisationId]
				   ,[OfferedByProviderId] 
				   ,[StartDate]
				   ,[A10Codes]
				   ,[DfEFunded])
			SELECT CO.CourseOptionId,
				Null AS ProviderOwnCourseInstanceRef,
				F.Price,
				F.PriceDescription AS PriceAsText,
				CO.DurationValue, 
				DU.BulkUploadRef AS DurationBulkUploadRef,
				DU.DurationUnitName,
				Null AS DurationAsText,
				Null AS StartDescription,
				Null AS EndDate,
				COALESCE(SM.BulkUploadRef, '') AS StudyModeBulkUploadRef,
				SM.StudyModeName,
				COALESCE(AT.BulkUploadRef, '') AS AttendanceModeBulkUploadRef,
				AT.AttendanceTypeName AS AttendanceModeName,
				COALESCE(AP.BulkUploadRef, '') AS AttendancePatternBulkUploadRef,
				AP.AttendancePatternName,
				Null AS LanguageOfInstruction,
				Null AS LanguageOfAssessment,
				Null, -- C.NoOfPlaces AS PlacesAvailable,  -- TODO: Get Number of Places
				Null AS EnquiryTo,
				Null AS ApplyTo,
				Null AS ApplyFromDate,
				Null AS ApplyUntilDate,
				Null AS ApplyUntilText,
				Null AS Url,
				Null AS TimeTable,
				CId.CourseId AS CourseId,
				CO.LocationId,
				0 AS CanApplyAllYear,
				'' AS RegionName,
				C.CreatedDateTimeUtc,
				Null AS ModifiedDateTimeUtc,
				2 AS RecordStatusId,
				C.CreatedByUserId,
				Null AS ModifiedByUserId,
				dbo.GetCourseInstanceSummaryText
					(
						DU.DurationUnitId, 
						DU.DurationUnitName, 
						'', 
						SM.StudyModeName, 
						F.Price, 
						F.PriceDescription, 
						CO.StartDate,
						'',  --S.StartDescription, 
						L.LocationName, 
						'', 
						''
					) AS CourseInstanceSummary,
				Null AS ProviderRegionId,
				3 AS ApplicationId,
				Null AS OfferedByOrganisationId,
				Null AS OfferedByProviderId,
				CO.StartDate,
				Null AS A10Codes,
				0 AS DFEFunded
			FROM [UCAS_PG].Course C
				INNER JOIN [UCAS_PG].[CourseOption] CO ON CO.CourseId = C.CourseId
				INNER JOIN [UCAS_PG].[Location] L ON L.LocationId = CO.LocationId
				INNER JOIN (
								SELECT Max(CO.CourseOptionId) AS CourseId,
									C.ProviderId,
									C.CourseTitle,
									C.CourseSummary,
									CO.EntryRequirements,
									CO.AssessmentMethods,
									CO.Qualification,
									CO.QualificationLevel
								FROM [UCAS_PG].[Course] C
									INNER JOIN [UCAS_PG].[CourseOption] CO ON CO.CourseId = C.CourseId
								GROUP BY C.ProviderId,
									C.CourseTitle,
									C.CourseSummary,
									CO.EntryRequirements,
									CO.AssessmentMethods,
									CO.Qualification,
									CO.QualificationLevel							
							) CId ON CId.ProviderId = C.ProviderId 
										AND CId.CourseTitle = C.CourseTitle 
										AND CId.CourseSummary = C.CourseSummary 
										AND CId.EntryRequirements = CO.EntryRequirements 
										AND CId.AssessmentMethods = CO.AssessmentMethods
										AND CId.Qualification = CO.Qualification
										AND CId.QualificationLevel = CO.QualificationLevel
				LEFT OUTER JOIN [dbo].UcasStudyModeMapping UCASMapping ON UCASMapping.UcasStudyMode = CO.StudyMode
				LEFT OUTER JOIN [dbo].StudyMode SM ON SM.StudyModeId = UCASMapping.MapsToStudyModeId
				LEFT OUTER JOIN [dbo].AttendanceType AT ON AT.AttendanceTypeId = UCASMapping.MapsToAttendanceTypeId
				LEFT OUTER JOIN [dbo].AttendancePattern AP ON AP.AttendancePatternId = UCASMapping.MapsToAttendancePattern
				LEFT OUTER JOIN [dbo].DurationUnit DU ON DU.DurationUnitId = CASE WHEN CO.DurationType = 'Weeks' THEN 3 -- Weeks
																				  WHEN CO.DurationType = 'Months' THEN 4 -- Months
																				  WHEN CO.DurationType = 'Years' THEN 7 -- Years
																				  WHEN CO.DurationType = 'Semesters' THEN 6 -- Semesters
																				  WHEN CO.DurationType = 'Terms' THEN 5 -- Terms
																				  WHEN CO.DurationType = 'Calendar Years' THEN 7 -- Calendar Years
																				  ELSE Null END
				LEFT OUTER JOIN (
									SELECT DISTINCT F.CourseOptionId,
										CASE WHEN Count(DISTINCT Fee) = 1 THEN Min(Fee) ELSE Null END AS Price,
										REPLACE(Min(FeeText), '~', Char(13) + Char(10)) AS PriceDescription
									FROM [UCAS_PG].CourseOptionFee F
										LEFT OUTER JOIN (
															SELECT CourseOptionId,
																(
																	SELECT Locale + ' ' + FeeDurationPeriod + ' ' + CAST(Fee AS VARCHAR(10)) + ' ' + Currency + '~' AS [text()] 
																	FROM [UCAS_PG].[CourseOptionFee] F1
																	WHERE F1.CourseOptionId = F.CourseOptionId
																	ORDER BY F1.CourseOptionId
																	FOR XML PATH ('')
																) FeeText
															FROM [UCAS_PG].[CourseOptionFee] F
															GROUP BY CourseOptionId
														) FT ON FT.CourseOptionId = F.CourseOptionId
									GROUP BY F.CourseOptionId
								) F ON F.CourseOptionId = CO.CourseOptionId;
		END;

		/** Venue List**/
		SELECT @OldVenueCount = Count(*) FROM [Search].[Venue] WHERE (@IncludeUCASData = 1 AND VenueId < 0 AND ApplicationId = 3) OR (@IncludeUCAS_PG_Data = 1 AND VenueId > 0 AND ApplicationId = 3) OR ApplicationId <> 3;
		TRUNCATE TABLE [Search].[Venue];
		INSERT INTO [Search].[Venue]
			   ([VenueId]
			   ,[ProviderId]
			   ,[VenueName]
			   ,[ProviderOwnVenueRef]
			   ,[Telephone]
			   ,[AddressLine1]
			   ,[AddressLine2]
			   ,[Town]
			   ,[County]
			   ,[Postcode]
			   ,[Email]
			   ,[Website]
			   ,[Fax]
			   ,[Facilities]
			   ,[CreatedDateTimeUtc]
			   ,[ModifiedDateTimeUtc]
			   ,[RecordStatusId]
			   ,[CreatedByUserId]
			   ,[ModifiedByUserId]
			   ,[Easting]
			   ,[Northing]
			   ,[SearchRegion]
			   ,[ApplicationId]
			   ,[Latitude]
			   ,[Longitude]
			   ,[PostTown]
			   ,[DependentLocality]
			   ,[DoubleDependentLocality]
			   ,[VenueLocationId]
			   ,[EuropeanElectoralRegion]
			   ,[LocalAuthorityDistrict]
			   ,[CurrentElectoralWard]
			   ,[OnsCounty]
			   ,[CountyAliasId]
			   ,[ParishCommunity]
			   ,[CensusBuiltUpAreaSubDivision]
				,RegionLevelPenalty
			   )
				EXEC [Search].[VenueList];

		-- Insert UCAS Data
		IF (@IncludeUCASData = 1)
		BEGIN
			INSERT INTO [Search].[Venue]
				   ([VenueId]
				   ,[ProviderId]
				   ,[VenueName]
				   ,[ProviderOwnVenueRef]
				   ,[Telephone]
				   ,[AddressLine1]
				   ,[AddressLine2]
				   ,[Town]
				   ,[County]
				   ,[Postcode]
				   ,[Email]
				   ,[Website]
				   ,[Fax]
				   ,[Facilities]
				   ,[CreatedDateTimeUtc]
				   ,[ModifiedDateTimeUtc]
				   ,[RecordStatusId]
				   ,[CreatedByUserId]
				   ,[ModifiedByUserId]
				   ,[Easting]
				   ,[Northing]
				   ,[SearchRegion]
				   ,[ApplicationId]
				   ,[Latitude]
				   ,[Longitude]
				   ,[PostTown]
				   ,[DependentLocality]
				   ,[DoubleDependentLocality]
				   ,[VenueLocationId]
				   ,[EuropeanElectoralRegion]
				   ,[LocalAuthorityDistrict]
				   ,[CurrentElectoralWard]
				   ,[OnsCounty]
				   ,[CountyAliasId]
				   ,[ParishCommunity]
				   ,[CensusBuiltUpAreaSubDivision]
				   ,RegionLevelPenalty)
			SELECT PlaceOfStudyId * (-1),
				OrgId * (-1),
				PlaceOfStudy,
				Null,
				Null,
				Address1,
				Address2,
				COALESCE(T.Town, ''),
				Null,
				POS.Postcode,
				Null,
				Null,
				Null,
				Null,
				CreatedDateTimeUtc,
				Null,
				2,
				CreatedByUserId,
				Null,
				Geo.Easting,
				Geo.Northing,
				COALESCE(SubString(POS.Postcode, 0, CharIndex(' ', POS.Postcode, 1)), PlaceOfStudy),
				3,
				Geo.Lat,
				Geo.Lng,
				-- Address Base
				ab.Town AS PostTown,
				ab.DependentLocality,
				ab.DoubleDependentLocality,
				-- Venue Location Hierarchy (excl. World)
				vl.VenueLocationId,
				-- ONS_PostcodeDirectory
				ons.EuropeanElectoralRegion,
				ons.LocalAuthorityDistrict,
				ons.CurrentElectoralWard,
				ons.County AS OnsCounty,
				ca.CountyAliasId,
				ons.ParishCommunity,
				ons.CensusBuiltUpAreaSubDivision,
				0
			FROM [UCAS].[PlacesOfStudy] POS
				LEFT OUTER JOIN [UCAS].[Towns] T ON T.TownId = POS.TownId
				LEFT OUTER JOIN [dbo].[GeoLocation] Geo ON Geo.Postcode = POS.Postcode
				OUTER APPLY (
					SELECT TOP 1 *
					FROM dbo.AddressBase abi
					WHERE abi.PostCode = POS.Postcode
				) AS ab
				LEFT OUTER JOIN onspd.VenueEnhancement ons ON ons.Postcode = POS.Postcode
				LEFT OUTER JOIN (
					SELECT vli.LocationName, Count(*) LocationCount
					FROM VenueLocation vli
					GROUP BY LocationName
				) vlc ON ab.Town = vlc.LocationName
				LEFT OUTER JOIN venuelocation vl ON ab.town = vl.locationname AND vlc.LocationCount = 1
				LEFT OUTER JOIN v_CountyAlias ca ON ca.CountyOnsName = ons.County;
		END;

		-- Insert UCAS PG Data
		IF (@IncludeUCAS_PG_Data = 1)
		BEGIN
			INSERT INTO [Search].[Venue]
				   ([VenueId]
				   ,[ProviderId]
				   ,[VenueName]
				   ,[ProviderOwnVenueRef]
				   ,[Telephone]
				   ,[AddressLine1]
				   ,[AddressLine2]
				   ,[Town]
				   ,[County]
				   ,[Postcode]
				   ,[Email]
				   ,[Website]
				   ,[Fax]
				   ,[Facilities]
				   ,[CreatedDateTimeUtc]
				   ,[ModifiedDateTimeUtc]
				   ,[RecordStatusId]
				   ,[CreatedByUserId]
				   ,[ModifiedByUserId]
				   ,[Easting]
				   ,[Northing]
				   ,[SearchRegion]
				   ,[ApplicationId]
				   ,[Latitude]
				   ,[Longitude]
				   ,[PostTown]
				   ,[DependentLocality]
				   ,[DoubleDependentLocality]
				   ,[VenueLocationId]
				   ,[EuropeanElectoralRegion]
				   ,[LocalAuthorityDistrict]
				   ,[CurrentElectoralWard]
				   ,[OnsCounty]
				   ,[CountyAliasId]
				   ,[ParishCommunity]
				   ,[CensusBuiltUpAreaSubDivision]
				   ,RegionLevelPenalty)
			SELECT LocationId,
				ProviderId,
				L.LocationName,
				Null,
				Null,
				Address1,
				Address2,
				Address4,
				NULL,
				L.Postcode,
				Null,
				Null,
				Null,
				Null,
				CreatedDateTimeUtc,
				Null,
				2,
				CreatedByUserId,
				Null,
				Geo.Easting,
				Geo.Northing,
				COALESCE(SubString(L.Postcode, 0, CharIndex(' ', L.Postcode, 1)), L.LocationName),
				3,
				Geo.Lat,
				Geo.Lng,
				-- Address Base
				ab.Town AS PostTown,
				ab.DependentLocality,
				ab.DoubleDependentLocality,
				-- Venue Location Hierarchy (excl. World)
				vl.VenueLocationId,
				-- ONS_PostcodeDirectory
				ons.EuropeanElectoralRegion,
				ons.LocalAuthorityDistrict,
				ons.CurrentElectoralWard,
				ons.County AS OnsCounty,
				ca.CountyAliasId,
				ons.ParishCommunity,
				ons.CensusBuiltUpAreaSubDivision,
				0
			FROM [UCAS_PG].[Location] L
				LEFT OUTER JOIN [dbo].[GeoLocation] Geo ON Geo.Postcode = L.Postcode
				OUTER APPLY (
					SELECT TOP 1 *
					FROM dbo.AddressBase abi
					WHERE abi.PostCode = L.Postcode
				) AS ab
				LEFT OUTER JOIN onspd.VenueEnhancement ons ON ons.Postcode = L.Postcode
				LEFT OUTER JOIN (
					SELECT vli.LocationName, Count(*) LocationCount
					FROM VenueLocation vli
					GROUP BY LocationName
				) vlc ON ab.Town = vlc.LocationName
				LEFT OUTER JOIN venuelocation vl ON ab.town = vl.locationname AND vlc.LocationCount = 1
				LEFT OUTER JOIN v_CountyAlias ca ON ca.CountyOnsName = ons.County;			
		END;

		-- Add phantom venues
		DECLARE @LiveRecordStatusId int = (SELECT RecordStatusId FROM RecordStatus WHERE IsArchived = 0 AND IsDeleted = 0 AND IsPublished = 1);

		DECLARE @PhantomVenue TABLE (
			CourseInstanceId	INT				NOT NULL PRIMARY KEY,
			ProviderId          INT				NOT NULL,
			VenueName           NVARCHAR (255)  NOT NULL,
			ProviderOwnVenueRef NVARCHAR (255)  NULL,
			Telephone           NVARCHAR (30)   NULL,
			AddressLine1        NVARCHAR (100)  NOT NULL,
			AddressLine2        NVARCHAR (100)  NULL,
			Town                NVARCHAR (75)   NOT NULL,
			County              NVARCHAR (75)   NULL,
			Postcode            NVARCHAR (30)   NOT NULL,
			Email               NVARCHAR (255)  NULL,
			Website             NVARCHAR (511)  NULL,
			Fax                 NVARCHAR (35)   NULL,
			Facilities          NVARCHAR (2000) NULL,
			CreatedDateTimeUtc  DATETIME        NOT NULL,
			ModifiedDateTimeUtc DATETIME        NULL,
			RecordStatusId      INT             NOT NULL,
			CreatedByUserId     NVARCHAR (128)  NOT NULL,
			ModifiedByUserId    NVARCHAR (128)  NULL,
			Easting             FLOAT (53)      NULL,
			Northing            FLOAT (53)      NULL,
			SearchRegion        NVARCHAR (30)   NOT NULL,
			ApplicationId       INT             NOT NULL, 
			Latitude			FLOAT			NULL, 
			Longitude		    FLOAT			NULL, 
			--Geography as geography::STGeomFromText('Point(' + CAST(Latitude AS VARCHAR(32)) + ' ' + CAST(Longitude AS VARCHAR(32)) + ')',4326),
			-- Address Base
			PostTown			NVARCHAR(30)	NULL,
			DependentLocality   NVARCHAR(35)	NULL,
			DoubleDependentLocality   NVARCHAR(35)	NULL,
			-- Venue Location Hierarchy
			VenueLocationId		INT				NULL,
			-- ONS_PostcodeDirectory
			EuropeanElectoralRegion	NVARCHAR(24)	NULL,
			LocalAuthorityDistrict	NVARCHAR(36)	NULL,
			CurrentElectoralWard	NVARCHAR(56)	NULL,
			OnsCounty				NVARCHAR(27)	NULL,
			CountyAliasId			INT				NULL,
			ParishCommunity			NVARCHAR(70)	NULL,
			CensusBuiltUpAreaSubDivision	NVARCHAR(47)	NULL,
			RegionLevelPenalty		FLOAT			NOT NULL
		);

		INSERT INTO @PhantomVenue
		SELECT ci.CourseInstanceId,
			c.ProviderId,
			'' AS VenueName,
			'' AS ProviderOwnVenueRef,
			'' AS Telephone,
			'' AS AddressLine1,
			'' AS AddressLine2,
			COALESCE(vl.LocationName, ab.Town) Town,
			'' AS County,
			CASE WHEN vl.NearestPostCode IS NOT NULL THEN vl.NearestPostCode ELSE '' END AS Postcode,
			'' AS Email,
			'' AS Website,
			'' AS Fax,
			'' AS Facilities,
			ci.CreatedDateTimeUtc,
			ci.ModifiedDateTimeUtc,
			@LiveRecordStatusId RecordStatusId,
			ci.CreatedByUserId,
			ci.ModifiedByUserId,
			vl.Easting,
			vl.Northing,
			CASE WHEN vl.NearestPostcode IS NOT NULL THEN Left(vl.NearestPostCode, CharIndex(' ', NearestPostCode) - 1) ELSE '' END AS SearchRegion,
			ci.ApplicationId,
			vl.Latitude,
			vl.Longitude,
			ab.Town PostTown,
			ab.DependentLocality,
			ab.DoubleDependentLocality,
			vl.VenueLocationId,
			ons.EuropeanElectoralRegion,
			ons.LocalAuthorityDistrict,
			ons.CurrentElectoralWard,
			ons.County OnsCounty,
			ca.CountyAliasId,
			ons.ParishCommunity,
			ons.CensusBuiltUpAreaSubDivision,
			CASE WHEN dbo.GetRegionLevel(vl.VenueLocationId) * @RegionLevelPenalty > @MaxRegionLevelPenalty THEN @MaxRegionLevelPenalty ELSE dbo.GetRegionLevel(vl.VenueLocationId) * @RegionLevelPenalty END AS RegionLevelPenalty
		FROM Search.CourseInstance ci
			INNER JOIN Search.Course c ON c.CourseId = ci.CourseId
			INNER JOIN dbo.venueLocation vl on vl.VenueLocationId = ci.VenueLocationId
			LEFT JOIN ONS_PostcodeDirectory.dbo.VenueEnhancement ons ON ons.Postcode = vl.NearestPostCode
			LEFT JOIN Search.v_CountyAlias ca on ca.CountyOnsName = ons.County
			OUTER APPLY (
				SELECT TOP 1 *
				FROM AddressBase a
				WHERE a.Postcode = vl.NearestPostCode
			) ab
		WHERE ci.VenueLocationId IS NOT NULL AND VenueId IS NULL;

		DECLARE @MyCursor CURSOR;
		DECLARE @CourseInstanceId INT;
		BEGIN
			SET NOCOUNT ON;

			SET @MyCursor = CURSOR FOR
				SELECT CourseInstanceId
				FROM @PhantomVenue;

			OPEN @MyCursor 
			FETCH NEXT FROM @MyCursor 
				INTO @CourseInstanceId;

			WHILE @@FETCH_STATUS = 0
			BEGIN
		
				DECLARE @VenueId INT = (SELECT Min(VenueId)-1 FROM Search.Venue);

				INSERT INTO Search.Venue (
					VenueId
					, ProviderId
					, VenueName
					, ProviderOwnVenueRef
					, Telephone
					, AddressLine1
					, AddressLine2
					, Town
					, County
					, Postcode
					, Email
					, Website
					, Fax
					, Facilities
					, CreatedDateTimeUtc
					, ModifiedDateTimeUtc
					, RecordStatusId
					, CreatedByUserId
					, ModifiedByUserId
					, Easting
					, Northing
					, SearchRegion
					, ApplicationId
					, Latitude
					, Longitude
					, PostTown
					, DependentLocality
					, DoubleDependentLocality
					, VenueLocationId
					, EuropeanElectoralRegion
					, LocalAuthorityDistrict
					, CurrentElectoralWard
					, OnsCounty
					, CountyAliasId
					, ParishCommunity
					, CensusBuiltUpAreaSubDivision
					, RegionLevelPenalty
				)
				SELECT
					@VenueId
					, ProviderId
					, VenueName
					, ProviderOwnVenueRef
					, Telephone
					, AddressLine1
					, AddressLine2
					, Town
					, County
					, Postcode
					, Email
					, Website
					, Fax
					, Facilities
					, CreatedDateTimeUtc
					, ModifiedDateTimeUtc
					, RecordStatusId
					, CreatedByUserId
					, ModifiedByUserId
					, Easting
					, Northing
					, SearchRegion
					, ApplicationId
					, Latitude
					, Longitude
					, PostTown
					, DependentLocality
					, DoubleDependentLocality
					, VenueLocationId
					, EuropeanElectoralRegion
					, LocalAuthorityDistrict
					, CurrentElectoralWard
					, OnsCounty
					, CountyAliasId
					, ParishCommunity
					, CensusBuiltUpAreaSubDivision
					, RegionLevelPenalty
				FROM @PhantomVenue
				WHERE CourseInstanceId = @CourseInstanceId;

				UPDATE Search.CourseInstance
					SET VenueId = @VenueId
				WHERE CourseInstanceId = @CourseInstanceId;

				FETCH NEXT FROM @MyCursor 
					INTO @CourseInstanceId;
			END; 

			CLOSE @MyCursor;
			DEALLOCATE @MyCursor;

			SET NOCOUNT OFF;
		END;
		-- End phantom venues

		SELECT @NewVenueCount = Count(*) FROM [Search].[Venue];

		/* Now that venues are populated try and populate missing County Aliases where possible */
		UPDATE search.Venue
		SET CountyAliasId = la.LocationAliasId
		FROM search.Venue v 
			OUTER APPLY (
				SELECT TOP 1 *
				FROM dbo.LocationAlias la
					LEFT JOIN search.v_VenueLocation vl ON v.VenueLocationId = vl.VenueLocationId
				WHERE
					la.LocationAliasName = v.Town
					OR la.LocationAliasName = v.County
					OR la.LocationAliasName = v.SearchRegion
					OR la.LocationAliasName = v.PostTown
					OR la.LocationAliasName = v.DependentLocality
					OR la.LocationAliasName = v.DoubleDependentLocality
					OR la.LocationAliasName = v.EuropeanElectoralRegion
					OR la.LocationAliasName = v.LocalAuthorityDistrict
					OR la.LocationAliasName = v.CurrentElectoralWard
					OR la.LocationAliasName = v.OnsCounty
					OR la.LocationAliasName = v.ParishCommunity
					OR la.LocationAliasName = v.CensusBuiltUpAreaSubDivision
					OR la.LocationAliasName = vl.LocationName1
					OR la.LocationAliasName = vl.LocationName2
					OR la.LocationAliasName = vl.LocationName3
					OR la.LocationAliasName = vl.LocationName4
					OR la.LocationAliasName = vl.LocationName5
					OR la.LocationAliasName = vl.LocationName6
					OR la.LocationAliasName = vl.LocationName7
					OR la.LocationAliasName = vl.LocationName8
					OR la.LocationAliasName = vl.LocationName9
				) la
		WHERE la.LocationAliasTypeId = 1 AND v.CountyAliasId IS NULL;

		/* Venue Text (This must be done AFTER all venues have been populated)*/
		TRUNCATE TABLE [Search].[VenueText];
		DBCC CHECKIDENT([Search.VenueText], RESEED, 1);  -- Reseed the identity column because we are adding approx 22 million rows per night and therefore the int column runs out of space in around 100 days.
		INSERT INTO [Search].[VenueText]
			([VenueId], [ProviderId], [SearchText])
			EXEC [Search].[VenueSearchText];


		-- Remove anything where the start date is more than 1 year ago
		DELETE FROM [Search].CourseInstance WHERE StartDate < @OneYearAgo;
		DELETE FROM [Search].CourseInstanceStartDate WHERE StartDate < @OneYearAgo OR CourseInstanceId NOT IN (SELECT CourseInstanceId FROM [Search].CourseInstance);
		DELETE FROM [Search].Course WHERE CourseId NOT IN (SELECT DISTINCT CourseId FROM [Search].CourseInstance);
		DELETE FROM [Search].Venue WHERE VenueId NOT IN (SELECT DISTINCT VenueId FROM [Search].CourseInstance);

		SELECT @NewCourseInstanceCount = Count(*) FROM [Search].[CourseInstance];


		/** Course Text **/
		-- We populate this table and move it across to the SFA_SearchAPI database but we don't use it anywhere
		SELECT @OldCourseTextCount = Count(*) FROM [Search].[CourseText] CT INNER JOIN [Search].CourseInstance CI ON CI.CourseInstanceId = CT.CourseInstanceId WHERE (@IncludeUCASData = 1 AND CI.CourseInstanceId < 0 AND ApplicationId = 3) OR (@IncludeUCAS_PG_Data = 1 AND CI.CourseInstanceId > 0 AND ApplicationId = 3) OR ApplicationId <> 3;
		TRUNCATE TABLE [Search].[CourseText];
		INSERT INTO [Search].[CourseText]
			   ([CourseId]
			   ,[CourseInstanceId]
			   ,[StudyModeBulkUploadRef]
			   ,[AttendanceModeBulkUploadRef]
			   ,[AttendancePatternBulkUploadRef]
			   ,[QualificationTypeRef]
			   ,[EastingMin]
			   ,[EastingMax]
			   ,[NorthingMin]
			   ,[NorthingMax]
			   ,[Easting]
			   ,[Northing]
			   ,[SearchText]
			   ,[NumberOfCourseInstances]
			   ,[ModifiedDateTimeUtc]
			   ,[ProviderId]
			   ,[LdcsCodes]
			   ,[ProviderName]
			   --,[Loans24Plus]
			   ,[ApplyUntilDate]
			   ,[ApplicationFundingStatus]
			   --,[Loans24PlusFundingStatus]
			   --,[AdultLearnerFundingStatus]
			   --,[OtherFundingStatus]
			   --,[IndependentLivingSkills]
			   ,[CanApplyAllYear]
			   ,[SearchRegion]
			   ,[SearchTown]
			   ,[Postcode])
			EXEC [Search].[CourseSearchText];

		-- INSERT UCAS Data
		IF (@IncludeUCASData = 1 OR @IncludeUCAS_PG_Data = 1)
		BEGIN
			INSERT INTO [Search].[CourseText]
				   ([CourseId]
				   ,[CourseInstanceId]
				   ,[StudyModeBulkUploadRef]
				   ,[AttendanceModeBulkUploadRef]
				   ,[AttendancePatternBulkUploadRef]
				   ,[QualificationTypeRef]
				   ,[EastingMin]
				   ,[EastingMax]
				   ,[NorthingMin]
				   ,[NorthingMax]
				   ,[Easting]
				   ,[Northing]
				   ,[SearchText]
				   ,[NumberOfCourseInstances]
				   ,[ModifiedDateTimeUtc]
				   ,[ProviderId]
				   ,[LdcsCodes]
				   ,[ProviderName]
				   --,[Loans24Plus]
				   ,[ApplyUntilDate]
				   ,[ApplicationFundingStatus]
				   --,[Loans24PlusFundingStatus]
				   --,[AdultLearnerFundingStatus]
				   --,[OtherFundingStatus]
				   --,[IndependentLivingSkills]
				   ,[CanApplyAllYear]
				   ,[SearchRegion]
				   ,[SearchTown]
				   ,[Postcode])
			SELECT C.CourseId,
				CI.CourseInstanceId,
				StudyModeBulkUploadRef,
				AttendanceModeBulkUploadRef,
				AttendancePatternBulkUploadRef,
				Null,
				Null,
				Null,
				Null,
				Null,
				V.Easting,
				V.Northing,
				C.CourseTitle 
					+ '<C>' + IsNull(LDCS1 + ' ', '') + IsNull(LDCS2 + ' ', '') + IsNull(LDCS3, '') + '</C>'		 
					+ '<S>' + IsNull(StudyModeBulkUploadRef, '') + '</S>'
					+ '<M>' + IsNull(AttendanceModeBulkUploadRef, '') + '</M>'
					+ '<P>' + IsNull(AttendancePatternBulkUploadRef, '') + '</P>'
					+ '<Q></Q>'
					+ '<PI>' + CAST(C.ProviderId AS VARCHAR(10)) + '</PI>'
					+ '<PN>' + P.ProviderName + '<PN>'
					+ '<ON></ON>'
					+ '<QL></QL>'
					+ '<A></A>' AS SearchText,
				(SELECT COUNT(*) FROM [Search].[CourseInstance] WHERE CourseId = C.CourseId) AS NumberOfCourseInstances,
				C.CreatedDateTimeUtc AS ModifiedDateTimeUtc,
				P.ProviderId,
				IsNull(LDCS1 + ' ', '') + IsNull(LDCS2 + ' ', '') + IsNull(LDCS3, '') AS LDCSCodes,
				P.ProviderName,
				Null,
				Null,
				0,
				Null,
				V.Town,
				V.Postcode
			FROM [Search].[Course] C
				INNER JOIN [Search].[CourseInstance] CI ON CI.CourseId = C.CourseId
				INNER JOIN [Search].[Venue] V ON V.VenueId = CI.VenueId
				INNER JOIN [Search].[Provider] P ON P.ProviderId = C.ProviderId
			WHERE C.ApplicationId = 3;
		END;

		SELECT @NewCourseTextCount = Count(*) FROM [Search].[CourseText];


		/*Category Code*/
		SELECT @OldCategoryCodeCount = Count(*) FROM [Search].[CategoryCode];
		TRUNCATE TABLE [Search].[CategoryCode];
		INSERT INTO [Search].[CategoryCode]
			   ([CategoryCode]
			   ,[ParentCategoryCode]
			   ,[Description]
			   ,[IsSearchable]
			   ,[TotalCourses]
			   ,[TotalUCASCourses])
			EXEC [Search].[CourseBrowseList] @IncludeUCASData = @IncludeUCASData;

		SELECT @NewCategoryCodeCount = Count(*) FROM [Search].[CategoryCode];

		/*Section for ensuring valid data import*/
		DECLARE @ThresholdPercent int = 100;
		DECLARE @OverrideThreshold bit = 0;
		DECLARE @TableName NVARCHAR(100);

		SELECT @ThresholdPercent = IsNull(Value, ValueDefault) FROM [dbo].ConfigurationSettings WHERE Name = 'NCSExportThresholdCheckPercent';
		SELECT @OverrideThreshold = IsNull(Value, ValueDefault) FROM [dbo].ConfigurationSettings WHERE Name = 'NCSOverrideThresholdCheck';

		IF (@OldProviderCount <> 0 AND @NewProviderCount / @OldProviderCount * 100 < @ThresholdPercent)
			BEGIN
				SET @IsValidInput = 0;
				SET @TableName = 'Provider';
			END;
		ELSE IF (@OldProviderTextCount <> 0 AND @NewProviderTextCount / @OldProviderTextCount * 100 < @ThresholdPercent)
			BEGIN
				SET @IsValidInput = 0;
				SET @TableName = 'ProviderText';
			END;
		ELSE IF (@OldVenueCount <> 0 AND @NewVenueCount / @OldVenueCount * 100 < @ThresholdPercent)
			BEGIN
				SET @IsValidInput = 0;
				SET @TableName = 'Venue';
			END;
		ELSE IF (@OldCourseCount <> 0 AND @NewCourseCount / @OldCourseCount * 100 < @ThresholdPercent)
			BEGIN
				SET @IsValidInput = 0;
				SET @TableName = 'Course';
			END;
		ELSE IF (@OldCourseTextCount <> 0 AND @NewCourseTextCount - @OldCourseTextCount / @OldCourseTextCount * 100 < @ThresholdPercent)
			BEGIN
				SET @IsValidInput = 0;
				SET @TableName = 'CourseText';
			END;
		ELSE IF (@OldCourseInstanceA10Count <> 0 AND @NewCourseInstanceA10Count / @OldCourseInstanceA10Count * 100 < @ThresholdPercent)
			BEGIN
				SET @IsValidInput = 0;
				SET @TableName = 'CourseInstanceA10';
			END;
		ELSE IF (@OldCourseInstanceStartDateCount <> 0 AND @NewCourseInstanceStartDateCount / @OldCourseInstanceStartDateCount * 100 < @ThresholdPercent)
			BEGIN
				SET @IsValidInput = 0;
				SET @TableName = 'CourseInstanceStartDate';
			END;
		ELSE IF (@OldCourseInstanceCount <> 0 AND @NewCourseInstanceCount / @OldCourseInstanceCount * 100 < @ThresholdPercent)
			BEGIN
				SET @IsValidInput = 0;
				SET @TableName = 'CourseInstance';
			END;
		ELSE IF (@OldCategoryCodeCount <> 0 AND @NewCategoryCodeCount / @OldCategoryCodeCount * 100 < @ThresholdPercent)
			BEGIN
				SET @IsValidInput = 0;
				SET @TableName = 'CategoryCode';
			END;
		ELSE
			SET @IsValidInput = 1;

		/*Either is valid data, or is been overridden, commit the changes*/
		IF (@IsValidInput = 1 OR @OverrideThreshold = 1) 
			BEGIN
				COMMIT TRANSACTION;

				/*Audit table entries */
				INSERT INTO [Search].[DataExportLog] ([ExportType], [ExecutedOn], [IsSuccessful], [IsValidDataImport], [ImportUCASData], [Import_UCAS_PG_Data]) VALUES ('Courses', GetUtcDate(), 1, @IsValidInput, @IncludeUCASData, @IncludeUCAS_PG_Data);
				SET @LogId = SCOPE_IDENTITY();

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'Provider', @OldProviderCount, @NewProviderCount);
 
				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'Provider Text', @OldProviderTextCount, @NewProviderTextCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'Venue', @OldVenueCount, @NewVenueCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'Course', @OldCourseCount, @NewCourseCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'Course Text', @OldCourseTextCount, @NewCourseTextCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'Course Instance A10 Code', @OldCourseInstanceA10Count, @NewCourseInstanceA10Count);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'Course Instance Start Date', @OldCourseInstanceStartDateCount, @NewCourseInstanceStartDateCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'Course Instance', @OldCourseInstanceCount, @NewCourseInstanceCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'Category Code', @OldCategoryCodeCount, @NewCategoryCodeCount);

				IF (@OverrideThreshold = 1) /* Override needs to be set back once overridden*/
				BEGIN
					UPDATE [dbo].[ConfigurationSettings] SET Value = 'false' WHERE Name = 'NCSOverrideThresholdCheck';
					SET @IsValidInput = @OverrideThreshold;
				END;
			END;
		ELSE
			BEGIN
				ROLLBACK;

				/*Audit table entries */
				INSERT INTO [Search].[DataExportLog] ([ExportType], [ExecutedOn], [IsSuccessful], [IsValidDataImport], [ImportUCASData], [Import_UCAS_PG_Data]) VALUES ('Courses', GetUtcDate(), 0, @IsValidInput, @IncludeUCASData, @IncludeUCAS_PG_Data);
				SET @LogId = SCOPE_IDENTITY();

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
						VALUES (@LogId, 'Provider', @OldProviderCount, @NewProviderCount);
 
				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
						VALUES (@LogId, 'Provider Text', @OldProviderTextCount, @NewProviderTextCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
						VALUES (@LogId, 'Venue', @OldVenueCount, @NewVenueCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
						VALUES (@LogId, 'Course', @OldCourseCount, @NewCourseCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
						VALUES (@LogId, 'Course Text', @OldCourseTextCount, @NewCourseTextCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
						VALUES (@LogId, 'Course Instance A10 Code', @OldCourseInstanceA10Count, @NewCourseInstanceA10Count);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
						VALUES (@LogId, 'Course Instance Start Date', @OldCourseInstanceStartDateCount, @NewCourseInstanceStartDateCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
						VALUES (@LogId, 'Course Instance', @OldCourseInstanceCount, @NewCourseInstanceCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
						VALUES (@LogId, 'Category Code', @OldCategoryCodeCount, @NewCategoryCodeCount);

				DECLARE @ErrorMessage NVARCHAR(1000) = 'Threshold of ' + CAST(@ThresholdPercent AS NVARCHAR(3)) + '%% Failed for Table ' + @TableName;
				THROW 50000, @ErrorMessage, 1;
			END;
	END TRY
	BEGIN CATCH
	
			IF (@@TRANCOUNT = 1)
				ROLLBACK;

			DECLARE @message VARCHAR(4000);
			SET @message= '[Error Number] ' + CONVERT(NVARCHAR(100), ERROR_NUMBER()) + ' [Procedure] ' + CONVERT(NVARCHAR(100), ERROR_PROCEDURE()) + ' [Error Message] ' + ERROR_MESSAGE();
	
			INSERT INTO [Search].[DataExportLog] ([ExportType], [ExecutedOn], [IsSuccessful], [IsValidDataImport], [ImportUCASData], [Import_UCAS_PG_Data]) VALUES ('Courses', GetUtcDate(), 0, @IsValidInput, @IncludeUCASData, @IncludeUCAS_PG_Data);
			SET @LogId = SCOPE_IDENTITY(); 	 
 
			INSERT INTO [Search].[DataExportException] ([DataExportLogId],[ExceptionDetails]) VALUES (@LogId, @message);

			THROW;

	END CATCH

END;