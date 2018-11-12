CREATE PROCEDURE [dbo].[usp_CreateDataForPeriod]
	@DateToRun		DATE = NULL,
	@ReRun			BIT = 0,
	@PeriodType		NVARCHAR(1)
AS

BEGIN

	SET @PeriodType = UPPER(@PeriodType);

	DECLARE @PeriodToRun				VARCHAR(7);
	DECLARE @PreviousPeriod				VARCHAR(7);
	DECLARE @CurrentPeriod				VARCHAR(7);
	DECLARE @PeriodStartDate			DATE;
	DECLARE @NextPeriodStartDate		DATE;

	DECLARE @ErrorNumber				INT;
	DECLARE @ErrorMessage				NVARCHAR(1000);

	DECLARE @LogTypeInformation			NVARCHAR(25) = 'Information';
	DECLARE @LogTypeWarning				NVARCHAR(25) = 'Warning';
	DECLARE @LogTypeError				NVARCHAR(25) = 'Error';

	DECLARE @PeriodTypeDescription		NVARCHAR(15) = dbo.GetPeriodTypeDescription(@PeriodType);

	DECLARE @LiveStatusId INT = (SELECT RecordStatusId FROM [Remote].RecordStatus WHERE IsPublished = 1);
	DECLARE @PortalApplicationId INT = 1;
	DECLARE @BulkUploadApplicationId INT = 2;

	IF (@PeriodTypeDescription = '')
	BEGIN
		SET @ErrorMessage = 'Invalid period type supplied - ' + @PeriodType;
		THROW 50000, @ErrorMessage, 1;
	END;

	-- Get Period to run
	SELECT TOP 1 @PeriodToRun = [Period], 
		@PreviousPeriod = [PreviousPeriod],
		@PeriodStartDate = [PeriodStartDate], 
		@NextPeriodStartDate = [NextPeriodStartDate] 
	FROM DWH_Period 
	WHERE NextPeriodStartDate <= @DateToRun 
		AND PeriodType = @PeriodType
	ORDER BY NextPeriodStartDate DESC;
	
	IF (@PeriodToRun IS NULL)
		BEGIN
			-- Can't find the period
			SET @ErrorMessage = 'Unable to determine which ' + Lower(@PeriodTypeDescription) + ' to run';
			EXEC usp_WriteDWHLog @LogType = @LogTypeWarning, @Message = @ErrorMessage;
			RETURN 0;
		END;

	SELECT @CurrentPeriod = [Period] FROM DWH_Period_Latest WHERE PeriodType = @PeriodType;

	-- If re-run is not 1 then make it 0.
	IF (@ReRun <> 1)
		SET @ReRun = 0;

	IF (@ReRun = 0)
		BEGIN
			IF (@CurrentPeriod = @PeriodToRun)
				BEGIN
					-- Nothing to do - Log it?
					PRINT @PeriodTypeDescription + ' end for ' + @PeriodToRun + ' already run.';
					RETURN 0;
				END;
			ELSE IF (@CurrentPeriod <> @PreviousPeriod)
				BEGIN
					-- Can't run period
					SET @ErrorMessage = 'Cannot run period for ' + @PeriodToRun + ' because current period is ' + @CurrentPeriod + ' and ReRun = 0';
					THROW 50000, @ErrorMessage, 1;
				END;
		END;
	ELSE IF (@CurrentPeriod < @PreviousPeriod)
		BEGIN
			-- Can't run period
			SET @ErrorMessage = 'Cannot run period for ' + @PeriodToRun + ' because current period is ' + @CurrentPeriod;
			THROW 50000, @ErrorMessage, 1;
		END;

	SET @ErrorMessage = 'Running ' + Lower(@PeriodTypeDescription) + ' for period ' + @PeriodToRun;
	EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = @ErrorMessage;

	IF (@NextPeriodStartDate > GetDate())
	BEGIN
		SET @ErrorMessage = 'Period end date (' + CAST(@NextPeriodStartDate AS VARCHAR(10)) + ') is greater than system date.  Previous period could be incomplete.';
		EXEC usp_WriteDWHLog @LogType = @LogTypeWarning, @Message = @ErrorMessage;
	END;

	BEGIN TRY

		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Truncating DWH_Audit_Summary';
		TRUNCATE TABLE DWH_Audit_Summary;

		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Address data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id, 
			AuditSeq
		)
		SELECT 'Address',
			AddressId,
			Max(AuditSeq)
		FROM [Remote].Audit_Address
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY AddressId;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Apprenticeship data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id, 
			AuditSeq
		)
		SELECT 'Apprenticeship',
			ApprenticeshipId,
			Max(AuditSeq)
		FROM [Remote].Audit_Apprenticeship
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY ApprenticeshipId;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting ApprenticeshipLocation data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id,
			AuditSeq
		)
		SELECT 'ApprenticeshipLocation',
			ApprenticeshipLocationId,
			Max(AuditSeq)
		FROM [Remote].Audit_ApprenticeshipLocation
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY ApprenticeshipLocationId;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting ApprenticeshipLocationDeliveryMode data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id,
			Id2,
			AuditSeq
		)
		SELECT 'ApprenticeshipLocationDeliveryMode',
			ApprenticeshipLocationId,
			DeliveryModeId,
			Max(AuditSeq)
		FROM [Remote].Audit_ApprenticeshipLocationDeliveryMode
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY ApprenticeshipLocationId,
			DeliveryModeId;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting AspNetUsers data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id3,
			AuditSeq
		)
		SELECT 'AspNetUsers',
			Id,
			Max(AuditSeq)
		FROM [Remote].Audit_AspNetUsers
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY Id;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Course data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id,
			AuditSeq
		)
		SELECT 'Course',
			CourseId,
			Max(AuditSeq)
		FROM [Remote].Audit_Course
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY CourseId;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting CourseInstance data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id,
			AuditSeq
		)
		SELECT 'CourseInstance',
			CourseInstanceId,
			Max(AuditSeq)
		FROM [Remote].Audit_CourseInstance
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY CourseInstanceId;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting CourseInstanceA10FundingCode data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id,
			Id2,
			AuditSeq
		)
		SELECT 'CourseInstanceA10FundingCode',
			CourseInstanceId,
			A10FundingCode,
			Max(AuditSeq)
		FROM [Remote].Audit_CourseInstanceA10FundingCode
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY CourseInstanceId,
			A10FundingCode;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting CourseInstanceStartDate data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id,
			AuditSeq
		)
		SELECT 'CourseInstanceStartDate',
			CourseInstanceStartDateId,
			Max(AuditSeq)
		FROM [Remote].Audit_CourseInstanceStartDate
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY CourseInstanceStartDateId;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting CourseInstanceVenue data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id,
			Id2,
			AuditSeq
		)
		SELECT 'CourseInstanceVenue',
			CourseInstanceId,
			VenueId,
			Max(AuditSeq)
		FROM [Remote].Audit_CourseInstanceVenue
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY CourseInstanceId,
			VenueId;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting CourseLeanDirectClassification data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id,
			Id3,
			AuditSeq
		)
		SELECT 'CourseLearnDirectClassification',
			CourseId,
			LearnDirectClassificationRef,
			Max(AuditSeq)
		FROM [Remote].Audit_CourseLearnDirectClassification
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY CourseId,
			LearnDirectClassificationRef;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Location data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id,
			AuditSeq
		)
		SELECT 'Location',
			LocationId,
			Max(AuditSeq)
		FROM [Remote].Audit_Location
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY LocationId;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Provider data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id,
			AuditSeq
		)
		SELECT 'Provider',
			ProviderId,
			Max(AuditSeq)
		FROM [Remote].Audit_Provider
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY ProviderId;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting ProviderUser data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id,
			Id3,
			AuditSeq
		)
		SELECT 'ProviderUser',
			ProviderId,
			UserId,
			Max(AuditSeq)
		FROM [Remote].Audit_ProviderUser
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY ProviderId,
			UserId;


		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Venue data into DWH_Audit_Summary';
		INSERT INTO DWH_Audit_Summary 
		(
			TableName, 
			Id,
			AuditSeq
		)
		SELECT 'Venue',
			VenueId,
			Max(AuditSeq)
		FROM [Remote].Audit_Venue
		WHERE AuditDateUtc BETWEEN @PeriodStartDate AND @NextPeriodStartDate
		GROUP BY VenueId;

		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_Address 
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_Address data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_Address] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_Address]
		(
			[Period],
			[AddressId],
			[AddressLine1],
			[AddressLine2],
			[Town],
			[County],
			[Postcode],
			[ProviderRegionId],
			[Latitude],
			[Longitude]
		)
		SELECT @PeriodToRun,
			[AddressId],
			[AddressLine1],
			[AddressLine2],
			[Town],
			[County],
			[Postcode],
			[ProviderRegionId],
			[Latitude],
			[Longitude]
		FROM [Snapshot_Address]
		WHERE [Period] = @PreviousPeriod
			AND AddressId NOT IN (
									SELECT Id
									FROM DWH_Audit_Summary 
									WHERE TableName = 'Address'
								 );

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_Address]
		(
			[Period],
			[AddressId],
			[AddressLine1],
			[AddressLine2],
			[Town],
			[County],
			[Postcode],
			[ProviderRegionId],
			[Latitude],
			[Longitude]
		)
		SELECT @PeriodToRun,
			[AddressId],
			[AddressLine1],
			[AddressLine2],
			[Town],
			[County],
			[Postcode],
			[ProviderRegionId],
			[Latitude],
			[Longitude]
		FROM [Remote].[Audit_Address]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'Address'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_Address 
		-------------------------------------------------------------------------------------------------------


		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_AspNetUsers
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_AspNetUsers data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_AspNetUsers] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_AspNetUsers]
		(
			[Period],
			[Id],
			[Email],
			[EmailConfirmed],
			[PasswordHash],
			[SecurityStamp],
			[PhoneNumber],
			[PhoneNumberConfirmed],
			[TwoFactorEnabled],
			[LockoutEndDateUtc],
			[LockoutEnabled],
			[AccessFailedCount],
			[UserName],
			[Name],
			[AddressId],
			[PasswordResetRequired],
			[ProviderUserTypeId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[IsDeleted],
			[LegacyUserId],
			[LastLoginDateTimeUtc],
			[IsSecureAccessUser],
			[SecureAccessUserId]
		)
		SELECT @PeriodToRun,
			[Id],
			[Email],
			[EmailConfirmed],
			[PasswordHash],
			[SecurityStamp],
			[PhoneNumber],
			[PhoneNumberConfirmed],
			[TwoFactorEnabled],
			[LockoutEndDateUtc],
			[LockoutEnabled],
			[AccessFailedCount],
			[UserName],
			[Name],
			[AddressId],
			[PasswordResetRequired],
			[ProviderUserTypeId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[IsDeleted],
			[LegacyUserId],
			[LastLoginDateTimeUtc],
			[IsSecureAccessUser],
			[SecureAccessUserId]
		FROM [Snapshot_AspNetUsers]
		WHERE [Period] = @PreviousPeriod
			AND Id NOT IN (
									SELECT Id3
									FROM DWH_Audit_Summary 
									WHERE TableName = 'AspNetUsers'
								 );

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_AspNetUsers]
		(
			[Period],
			[Id],
			[Email],
			[EmailConfirmed],
			[PasswordHash],
			[SecurityStamp],
			[PhoneNumber],
			[PhoneNumberConfirmed],
			[TwoFactorEnabled],
			[LockoutEndDateUtc],
			[LockoutEnabled],
			[AccessFailedCount],
			[UserName],
			[Name],
			[AddressId],
			[PasswordResetRequired],
			[ProviderUserTypeId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[IsDeleted],
			[LegacyUserId],
			[LastLoginDateTimeUtc],
			[IsSecureAccessUser],
			[SecureAccessUserId]
		)
		SELECT @PeriodToRun,
			[Id],
			[Email],
			[EmailConfirmed],
			[PasswordHash],
			[SecurityStamp],
			[PhoneNumber],
			[PhoneNumberConfirmed],
			[TwoFactorEnabled],
			[LockoutEndDateUtc],
			[LockoutEnabled],
			[AccessFailedCount],
			[UserName],
			[Name],
			[AddressId],
			[PasswordResetRequired],
			[ProviderUserTypeId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[IsDeleted],
			[LegacyUserId],
			[LastLoginDateTimeUtc],
			[IsSecureAccessUser],
			[SecureAccessUserId]
		FROM [Remote].[Audit_AspNetUsers]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'AspNetUsers'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_AspNetUsers
		-------------------------------------------------------------------------------------------------------


		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_Apprenticeship 
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_Apprenticeship data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_Apprenticeship] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_Apprenticeship]
		(
			[Period],
			[ApprenticeshipId],
			[ProviderId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[AddedByApplicationId],
			[RecordStatusId],
			[StandardCode],
			[Version],
			[FrameworkCode],
			[ProgType],
			[PathwayCode],
			[MarketingInformation],
			[Url],
			[ContactTelephone],
			[ContactEmail],
			[ContactWebsite]
		)
		SELECT @PeriodToRun,
			[ApprenticeshipId],
			[ProviderId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[AddedByApplicationId],
			[RecordStatusId],
			[StandardCode],
			[Version],
			[FrameworkCode],
			[ProgType],
			[PathwayCode],
			[MarketingInformation],
			[Url],
			[ContactTelephone],
			[ContactEmail],
			[ContactWebsite]
		FROM [Snapshot_Apprenticeship]
		WHERE [Period] = @PreviousPeriod
			AND ApprenticeshipId NOT IN (
											SELECT Id
											FROM DWH_Audit_Summary 
											WHERE TableName = 'Apprenticeship'
										);

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_Apprenticeship]
		(
			[Period],
			[ApprenticeshipId],
			[ProviderId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[AddedByApplicationId],
			[RecordStatusId],
			[StandardCode],
			[Version],
			[FrameworkCode],
			[ProgType],
			[PathwayCode],
			[MarketingInformation],
			[Url],
			[ContactTelephone],
			[ContactEmail],
			[ContactWebsite]
		)
		SELECT @PeriodToRun,
			[ApprenticeshipId],
			[ProviderId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[AddedByApplicationId],
			[RecordStatusId],
			[StandardCode],
			[Version],
			[FrameworkCode],
			[ProgType],
			[PathwayCode],
			[MarketingInformation],
			[Url],
			[ContactTelephone],
			[ContactEmail],
			[ContactWebsite]
		FROM [Remote].[Audit_Apprenticeship]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'Apprenticeship'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_Apprenticeship 
		-------------------------------------------------------------------------------------------------------


		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_ApprenticeshipLocation 
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_ApprenticeshipLocation data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_ApprenticeshipLocation] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_ApprenticeshipLocation]
		(
			[Period],
			[ApprenticeshipLocationId],
			[ApprenticeshipId],
			[LocationId],
			[Radius],
			[RecordStatusId],
			[AddedByApplicationId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc]
		)
		SELECT @PeriodToRun,
			[ApprenticeshipLocationId],
			[ApprenticeshipId],
			[LocationId],
			[Radius],
			[RecordStatusId],
			[AddedByApplicationId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc]
		FROM [Snapshot_ApprenticeshipLocation]
		WHERE [Period] = @PreviousPeriod
			AND ApprenticeshipLocationId NOT IN (
													SELECT Id
													FROM DWH_Audit_Summary 
													WHERE TableName = 'ApprenticeshipLocation'
												);

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_ApprenticeshipLocation]
		(
			[Period],
			[ApprenticeshipLocationId],
			[ApprenticeshipId],
			[LocationId],
			[Radius],
			[RecordStatusId],
			[AddedByApplicationId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc]
		)
		SELECT @PeriodToRun,
			[ApprenticeshipLocationId],
			[ApprenticeshipId],
			[LocationId],
			[Radius],
			[RecordStatusId],
			[AddedByApplicationId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc]
		FROM [Remote].[Audit_ApprenticeshipLocation]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'ApprenticeshipLocation'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_ApprenticeshipLocation 
		-------------------------------------------------------------------------------------------------------


		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_ApprenticeshipLocationDeliveryMode
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_ApprenticeshipLocationDeliveryMode data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_ApprenticeshipLocationDeliveryMode] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_ApprenticeshipLocationDeliveryMode]
		(
			[Period],
			[ApprenticeshipLocationId],
			[DeliveryModeId]
		)
		SELECT @PeriodToRun,
			[ApprenticeshipLocationId],
			[DeliveryModeId]
		FROM [Snapshot_ApprenticeshipLocationDeliveryMode] ALDM
		WHERE [Period] = @PreviousPeriod
			AND NOT EXISTS (
								SELECT Id
								FROM DWH_Audit_Summary
								WHERE TableName = 'ApprenticeshipLocationDeliveryMode'
									AND Id = ALDM.ApprenticeshipLocationId
									AND Id2 = ALDM.DeliveryModeId
						   );

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_ApprenticeshipLocationDeliveryMode]
		(
			[Period],
			[ApprenticeshipLocationId],
			[DeliveryModeId]
		)
		SELECT @PeriodToRun,
			[ApprenticeshipLocationId],
			[DeliveryModeId]
		FROM [Remote].[Audit_ApprenticeshipLocationDeliveryMode]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'ApprenticeshipLocationDeliveryMode'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_ApprenticeshipLocationDeliveryMode 
		-------------------------------------------------------------------------------------------------------

		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_Course 
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_Course data';
		
		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_Course] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_Course]
		(
			[Period],
			[CourseId],
			[ProviderId],
			[CourseTitle],
			[CourseSummary],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[AddedByApplicationId],
			[RecordStatusId],
			[LearningAimRefId],
			[QualificationLevelId],
			[EntryRequirements],
			[ProviderOwnCourseRef],
			[Url],
			[BookingUrl],
			[AssessmentMethod],
			[EquipmentRequired],
			[WhenNoLarQualificationTypeId],
			[WhenNoLarQualificationTitle],
			[AwardingOrganisationName],
			[UcasTariffPoints]
		)
		SELECT @PeriodToRun,
			[CourseId],
			[ProviderId],
			[CourseTitle],
			[CourseSummary],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[AddedByApplicationId],
			[RecordStatusId],
			[LearningAimRefId],
			[QualificationLevelId],
			[EntryRequirements],
			[ProviderOwnCourseRef],
			[Url],
			[BookingUrl],
			[AssessmentMethod],
			[EquipmentRequired],
			[WhenNoLarQualificationTypeId],
			[WhenNoLarQualificationTitle],
			[AwardingOrganisationName],
			[UcasTariffPoints]
		FROM [Snapshot_Course]
		WHERE [Period] = @PreviousPeriod
			AND CourseId NOT IN (
									SELECT Id
									FROM DWH_Audit_Summary 
									WHERE TableName = 'Course'
								);

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_Course]
		(
			[Period],
			[CourseId],
			[ProviderId],
			[CourseTitle],
			[CourseSummary],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[AddedByApplicationId],
			[RecordStatusId],
			[LearningAimRefId],
			[QualificationLevelId],
			[EntryRequirements],
			[ProviderOwnCourseRef],
			[Url],
			[BookingUrl],
			[AssessmentMethod],
			[EquipmentRequired],
			[WhenNoLarQualificationTypeId],
			[WhenNoLarQualificationTitle],
			[AwardingOrganisationName],
			[UcasTariffPoints]
		)
		SELECT @PeriodToRun,
			[CourseId],
			[ProviderId],
			[CourseTitle],
			[CourseSummary],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[AddedByApplicationId],
			[RecordStatusId],
			[LearningAimRefId],
			[QualificationLevelId],
			[EntryRequirements],
			[ProviderOwnCourseRef],
			[Url],
			[BookingUrl],
			[AssessmentMethod],
			[EquipmentRequired],
			[WhenNoLarQualificationTypeId],
			[WhenNoLarQualificationTitle],
			[AwardingOrganisationName],
			[UcasTariffPoints]
		FROM [Remote].[Audit_Course]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'Course'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_Course 
		-------------------------------------------------------------------------------------------------------

		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_CourseInstance 
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_CourseInstance data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_CourseInstance] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_CourseInstance]
		(
			[Period],
			[CourseInstanceId],
			[CourseId],
			[RecordStatusId],
			[ProviderOwnCourseInstanceRef],
			[OfferedByProviderId],
			[DisplayProviderId],
			[StudyModeId],
			[AttendanceTypeId],
			[AttendancePatternId],
			[DurationUnit],
			[DurationUnitId],
			[DurationAsText],
			[StartDateDescription],
			[EndDate],
			[TimeTable],
			[Price],
			[PriceAsText],
			[AddedByApplicationId],
			[LanguageOfInstruction],
			[LanguageOfAssessment],
			[ApplyFromDate],
			[ApplyUntilDate],
			[ApplyUntilText],
			[EnquiryTo],
			[ApplyTo],
			[Url],
			[CanApplyAllYear],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[PlacesAvailable],
			[BothOfferedByDisplayBySearched],
			[VenueLocationId],
			[OfferedByOrganisationId],
			[DisplayedByOrganisationId]	
		)
		SELECT @PeriodToRun,
			[CourseInstanceId],
			[CourseId],
			[RecordStatusId],
			[ProviderOwnCourseInstanceRef],
			[OfferedByProviderId],
			[DisplayProviderId],
			[StudyModeId],
			[AttendanceTypeId],
			[AttendancePatternId],
			[DurationUnit],
			[DurationUnitId],
			[DurationAsText],
			[StartDateDescription],
			[EndDate],
			[TimeTable],
			[Price],
			[PriceAsText],
			[AddedByApplicationId],
			[LanguageOfInstruction],
			[LanguageOfAssessment],
			[ApplyFromDate],
			[ApplyUntilDate],
			[ApplyUntilText],
			[EnquiryTo],
			[ApplyTo],
			[Url],
			[CanApplyAllYear],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[PlacesAvailable],
			[BothOfferedByDisplayBySearched],
			[VenueLocationId],
			[OfferedByOrganisationId],
			[DisplayedByOrganisationId]	
		FROM [Snapshot_CourseInstance]
		WHERE [Period] = @PreviousPeriod
			AND CourseInstanceId NOT IN (
									SELECT Id
									FROM DWH_Audit_Summary 
									WHERE TableName = 'CourseInstance'
								);

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_CourseInstance]
		(
			[Period],
			[CourseInstanceId],
			[CourseId],
			[RecordStatusId],
			[ProviderOwnCourseInstanceRef],
			[OfferedByProviderId],
			[DisplayProviderId],
			[StudyModeId],
			[AttendanceTypeId],
			[AttendancePatternId],
			[DurationUnit],
			[DurationUnitId],
			[DurationAsText],
			[StartDateDescription],
			[EndDate],
			[TimeTable],
			[Price],
			[PriceAsText],
			[AddedByApplicationId],
			[LanguageOfInstruction],
			[LanguageOfAssessment],
			[ApplyFromDate],
			[ApplyUntilDate],
			[ApplyUntilText],
			[EnquiryTo],
			[ApplyTo],
			[Url],
			[CanApplyAllYear],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[PlacesAvailable],
			[BothOfferedByDisplayBySearched],
			[VenueLocationId],
			[OfferedByOrganisationId],
			[DisplayedByOrganisationId]	
		)
		SELECT @PeriodToRun,
			[CourseInstanceId],
			[CourseId],
			[RecordStatusId],
			[ProviderOwnCourseInstanceRef],
			[OfferedByProviderId],
			[DisplayProviderId],
			[StudyModeId],
			[AttendanceTypeId],
			[AttendancePatternId],
			[DurationUnit],
			[DurationUnitId],
			[DurationAsText],
			[StartDateDescription],
			[EndDate],
			[TimeTable],
			[Price],
			[PriceAsText],
			[AddedByApplicationId],
			[LanguageOfInstruction],
			[LanguageOfAssessment],
			[ApplyFromDate],
			[ApplyUntilDate],
			[ApplyUntilText],
			[EnquiryTo],
			[ApplyTo],
			[Url],
			[CanApplyAllYear],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[PlacesAvailable],
			[BothOfferedByDisplayBySearched],
			[VenueLocationId],
			[OfferedByOrganisationId],
			[DisplayedByOrganisationId]	
		FROM [Remote].[Audit_CourseInstance]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'CourseInstance'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_CourseInstance 
		-------------------------------------------------------------------------------------------------------

		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_CourseInstanceA10FundingCode 
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_CourseInstanceA10FundingCode data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_CourseInstanceA10FundingCode] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_CourseInstanceA10FundingCode]
		(
			[Period],
			[CourseInstanceId],
			[A10FundingCode]
		)
		SELECT @PeriodToRun,
			[CourseInstanceId],
			[A10FundingCode]
		FROM [Snapshot_CourseInstanceA10FundingCode] CIFC
		WHERE [Period] = @PreviousPeriod
			AND NOT EXISTS (
								SELECT Id
								FROM DWH_Audit_Summary
								WHERE TableName = 'CourseInstanceA10FundingCode'
									AND Id = CIFC.CourseInstanceId
									AND Id2 = CIFC.A10FundingCode
						   );

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_CourseInstanceA10FundingCode]
		(
			[Period],
			[CourseInstanceId],
			[A10FundingCode]
		)
		SELECT @PeriodToRun,
			[CourseInstanceId],
			[A10FundingCode]
		FROM [Remote].[Audit_CourseInstanceA10FundingCode]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'CourseInstanceA10FundingCode'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_CourseInstanceA10FundingCode 
		-------------------------------------------------------------------------------------------------------

		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_CourseInstanceStartDate 
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_CourseInstanceStartDate data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_CourseInstanceStartDate] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_CourseInstanceStartDate]
		(
			[Period],
			[CourseInstanceStartDateId],
			[CourseInstanceId],
			[StartDate],
			[IsMonthOnlyStartDate],
			[PlacesAvailable]	
		)
		SELECT @PeriodToRun,
			[CourseInstanceStartDateId],
			[CourseInstanceId],
			[StartDate],
			[IsMonthOnlyStartDate],
			[PlacesAvailable]	
		FROM [Snapshot_CourseInstanceStartDate]
		WHERE [Period] = @PreviousPeriod
			AND CourseInstanceStartDateId NOT IN (
									SELECT Id
									FROM DWH_Audit_Summary 
									WHERE TableName = 'CourseInstanceStartDate'
								);

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_CourseInstanceStartDate]
		(
			[Period],
			[CourseInstanceStartDateId],
			[CourseInstanceId],
			[StartDate],
			[IsMonthOnlyStartDate],
			[PlacesAvailable]	
		)
		SELECT @PeriodToRun,
			[CourseInstanceStartDateId],
			[CourseInstanceId],
			[StartDate],
			[IsMonthOnlyStartDate],
			[PlacesAvailable]	
		FROM [Remote].[Audit_CourseInstanceStartDate]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'CourseInstanceStartDate'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_CourseInstanceStartDate 
		-------------------------------------------------------------------------------------------------------

		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_CourseInstanceVenue 
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_CourseInstanceVenue data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_CourseInstanceVenue] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_CourseInstanceVenue]
		(
			[Period],
			[CourseInstanceId],
			[VenueId]
		)
		SELECT @PeriodToRun,
			[CourseInstanceId],
			[VenueId]
		FROM [Snapshot_CourseInstanceVenue] CIV
		WHERE [Period] = @PreviousPeriod
			AND NOT EXISTS (
								SELECT Id
								FROM DWH_Audit_Summary
								WHERE TableName = 'CourseInstanceVenue'
									AND Id = CIV.CourseInstanceId
									AND Id2 = CIV.VenueId
						   );

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_CourseInstanceVenue]
		(
			[Period],
			[CourseInstanceId],
			[VenueId]
		)
		SELECT @PeriodToRun,
			[CourseInstanceId],
			[VenueId]
		FROM [Remote].[Audit_CourseInstanceVenue]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'CourseInstanceVenue'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_CourseInstanceVenue 
		-------------------------------------------------------------------------------------------------------

		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_CourseLearnDirectClassification 
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_CourseLearnDirectClassification data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_CourseLearnDirectClassification] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_CourseLearnDirectClassification]
		(
			[Period],
			[CourseId],
			[LearnDirectClassificationRef],
			[ClassificationOrder]
		)
		SELECT @PeriodToRun,
			[CourseId],
			[LearnDirectClassificationRef],
			[ClassificationOrder]
		FROM [Snapshot_CourseLearnDirectClassification] CLDC
		WHERE [Period] = @PreviousPeriod
			AND NOT EXISTS (
								SELECT Id
								FROM DWH_Audit_Summary
								WHERE TableName = 'CourseLearnDirectClassification'
									AND Id = CLDC.CourseId
									AND Id3 = CLDC.LearnDirectClassificationRef
						   );

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_CourseLearnDirectClassification]
		(
			[Period],
			[CourseId],
			[LearnDirectClassificationRef],
			[ClassificationOrder]
		)
		SELECT @PeriodToRun,
			[CourseId],
			[LearnDirectClassificationRef],
			[ClassificationOrder]
		FROM [Remote].[Audit_CourseLearnDirectClassification]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'CourseLearnDirectClassification'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_CourseLearnDirectClassification 
		-------------------------------------------------------------------------------------------------------

		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_Location 
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_Location data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_Location] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_Location]
		(
			[Period],
			[LocationId],
			[ProviderId],
			[ProviderOwnLocationRef],
			[LocationName],
			[AddressId],
			[Telephone],
			[Email],
			[Website],
			[RecordStatusId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[BulkUploadLocationId]
		)
		SELECT @PeriodToRun,
			[LocationId],
			[ProviderId],
			[ProviderOwnLocationRef],
			[LocationName],
			[AddressId],
			[Telephone],
			[Email],
			[Website],
			[RecordStatusId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[BulkUploadLocationId]
		FROM [Snapshot_Location]
		WHERE [Period] = @PreviousPeriod
			AND LocationId NOT IN (
									SELECT Id
									FROM DWH_Audit_Summary 
									WHERE TableName = 'Location'
								);

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_Location]
		(
			[Period],
			[LocationId],
			[ProviderId],
			[ProviderOwnLocationRef],
			[LocationName],
			[AddressId],
			[Telephone],
			[Email],
			[Website],
			[RecordStatusId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[BulkUploadLocationId]
		)
		SELECT @PeriodToRun,
			[LocationId],
			[ProviderId],
			[ProviderOwnLocationRef],
			[LocationName],
			[AddressId],
			[Telephone],
			[Email],
			[Website],
			[RecordStatusId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[BulkUploadLocationId]
		FROM [Remote].[Audit_Location]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'Location'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_Location 
		-------------------------------------------------------------------------------------------------------

		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_Provider 
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_Provider data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_Provider] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_Provider]
		(
			[Period],
			[ProviderId],
			[ProviderName],
			[ProviderNameAlias],
			[Loans24Plus],
			[Ukprn],
			[UPIN],
			[ProviderTypeId],
			[RecordStatusId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[ProviderRegionId],
			[IsContractingBody],
			[ProviderTrackingUrl],
			[VenueTrackingUrl],
			[CourseTrackingUrl],
			[BookingTrackingUrl],
			[RelationshipManagerUserId],
			[InformationOfficerUserId],
			[AddressId],
			[Email],
			[Website],
			[Telephone],
			[Fax],
			[FeChoicesLearner],
			[FeChoicesEmployer],
			[FeChoicesDestination],
			[FeChoicesUpdatedDateTimeUtc],
			[QualityEmailsPaused],
			[QualityEmailStatusId],
			[TrafficLightEmailDateTimeUtc],
			[DFE1619Funded],
			[SFAFunded],
			[DfENumber],
			[DfEUrn],
			[DfEProviderTypeId],
			[DfEProviderStatusId],
			[DfELocalAuthorityId],
			[DfERegionId],
			[DfEEstablishmentTypeId],
			[DfEEstablishmentNumber],
			[StatutoryLowestAge],
			[StatutoryHighestAge],
			[AgeRange],
			[AnnualSchoolCensusLowestAge],
			[AnnualSchoolCensusHighestAge],
			[CompanyRegistrationNumber],
			[Uid],
			[SecureAccessId],
			[BulkUploadPending],
			[PublishData],
			[MarketingInformation],
			[NationalApprenticeshipProvider],
			[ApprenticeshipContract],
			[PassedOverallQAChecks],
			[DataReadyToQA],
			[RoATPFFlag],
			[LastAllDataUpToDateTimeUtc],
			[RoATPProviderTypeId],
			[RoATPStartDate],
			[MarketingInformationUpdatedDateUtc],
			[IsTASOnly]
		)
		SELECT @PeriodToRun,
			[ProviderId],
			[ProviderName],
			[ProviderNameAlias],
			[Loans24Plus],
			[Ukprn],
			[UPIN],
			[ProviderTypeId],
			[RecordStatusId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[ProviderRegionId],
			[IsContractingBody],
			[ProviderTrackingUrl],
			[VenueTrackingUrl],
			[CourseTrackingUrl],
			[BookingTrackingUrl],
			[RelationshipManagerUserId],
			[InformationOfficerUserId],
			[AddressId],
			[Email],
			[Website],
			[Telephone],
			[Fax],
			[FeChoicesLearner],
			[FeChoicesEmployer],
			[FeChoicesDestination],
			[FeChoicesUpdatedDateTimeUtc],
			[QualityEmailsPaused],
			[QualityEmailStatusId],
			[TrafficLightEmailDateTimeUtc],
			[DFE1619Funded],
			[SFAFunded],
			[DfENumber],
			[DfEUrn],
			[DfEProviderTypeId],
			[DfEProviderStatusId],
			[DfELocalAuthorityId],
			[DfERegionId],
			[DfEEstablishmentTypeId],
			[DfEEstablishmentNumber],
			[StatutoryLowestAge],
			[StatutoryHighestAge],
			[AgeRange],
			[AnnualSchoolCensusLowestAge],
			[AnnualSchoolCensusHighestAge],
			[CompanyRegistrationNumber],
			[Uid],
			[SecureAccessId],
			[BulkUploadPending],
			[PublishData],
			[MarketingInformation],
			[NationalApprenticeshipProvider],
			[ApprenticeshipContract],
			[PassedOverallQAChecks],
			[DataReadyToQA],
			[RoATPFFlag],
			[LastAllDataUpToDateTimeUtc],
			[RoATPProviderTypeId],
			[RoATPStartDate],
			[MarketingInformationUpdatedDateUtc],
			[IsTASOnly]
		FROM [Snapshot_Provider]
		WHERE [Period] = @PreviousPeriod
			AND ProviderId NOT IN (
									SELECT Id
									FROM DWH_Audit_Summary 
									WHERE TableName = 'Provider'
								);

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_Provider]
		(
			[Period],
			[ProviderId],
			[ProviderName],
			[ProviderNameAlias],
			[Loans24Plus],
			[Ukprn],
			[UPIN],
			[ProviderTypeId],
			[RecordStatusId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[ProviderRegionId],
			[IsContractingBody],
			[ProviderTrackingUrl],
			[VenueTrackingUrl],
			[CourseTrackingUrl],
			[BookingTrackingUrl],
			[RelationshipManagerUserId],
			[InformationOfficerUserId],
			[AddressId],
			[Email],
			[Website],
			[Telephone],
			[Fax],
			[FeChoicesLearner],
			[FeChoicesEmployer],
			[FeChoicesDestination],
			[FeChoicesUpdatedDateTimeUtc],
			[QualityEmailsPaused],
			[QualityEmailStatusId],
			[TrafficLightEmailDateTimeUtc],
			[DFE1619Funded],
			[SFAFunded],
			[DfENumber],
			[DfEUrn],
			[DfEProviderTypeId],
			[DfEProviderStatusId],
			[DfELocalAuthorityId],
			[DfERegionId],
			[DfEEstablishmentTypeId],
			[DfEEstablishmentNumber],
			[StatutoryLowestAge],
			[StatutoryHighestAge],
			[AgeRange],
			[AnnualSchoolCensusLowestAge],
			[AnnualSchoolCensusHighestAge],
			[CompanyRegistrationNumber],
			[Uid],
			[SecureAccessId],
			[BulkUploadPending],
			[PublishData],
			[MarketingInformation],
			[NationalApprenticeshipProvider],
			[ApprenticeshipContract],
			[PassedOverallQAChecks],
			[DataReadyToQA],
			[RoATPFFlag],
			[LastAllDataUpToDateTimeUtc],
			[RoATPProviderTypeId],
			[RoATPStartDate],
			[MarketingInformationUpdatedDateUtc]
		)
		SELECT @PeriodToRun,
			[ProviderId],
			[ProviderName],
			[ProviderNameAlias],
			[Loans24Plus],
			[Ukprn],
			[UPIN],
			[ProviderTypeId],
			[RecordStatusId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[ProviderRegionId],
			[IsContractingBody],
			[ProviderTrackingUrl],
			[VenueTrackingUrl],
			[CourseTrackingUrl],
			[BookingTrackingUrl],
			[RelationshipManagerUserId],
			[InformationOfficerUserId],
			[AddressId],
			[Email],
			[Website],
			[Telephone],
			[Fax],
			[FeChoicesLearner],
			[FeChoicesEmployer],
			[FeChoicesDestination],
			[FeChoicesUpdatedDateTimeUtc],
			[QualityEmailsPaused],
			[QualityEmailStatusId],
			[TrafficLightEmailDateTimeUtc],
			COALESCE([DFE1619Funded], 0),
			COALESCE([SFAFunded], 0),
			[DfENumber],
			[DfEUrn],
			[DfEProviderTypeId],
			[DfEProviderStatusId],
			[DfELocalAuthorityId],
			[DfERegionId],
			[DfEEstablishmentTypeId],
			[DfEEstablishmentNumber],
			[StatutoryLowestAge],
			[StatutoryHighestAge],
			[AgeRange],
			[AnnualSchoolCensusLowestAge],
			[AnnualSchoolCensusHighestAge],
			[CompanyRegistrationNumber],
			[Uid],
			[SecureAccessId],
			COALESCE([BulkUploadPending], 0),
			COALESCE([PublishData], 0),
			[MarketingInformation],
			COALESCE([NationalApprenticeshipProvider], 0),
			COALESCE([ApprenticeshipContract], 0),
			COALESCE([PassedOverallQAChecks], 0),
			COALESCE([DataReadyToQA], 0),
			COALESCE([RoATPFFlag], 0),
			[LastAllDataUpToDateTimeUtc],
			[RoATPProviderTypeId],
			[RoATPStartDate],
			[MarketingInformationUpdatedDateUtc]
		FROM [Remote].[Audit_Provider]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'Provider'
			    			  );

	
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Setting IsTASOnly Flag';

		UPDATE Snapshot_Provider
		SET IsTASOnly = 1 
		WHERE [Period] = @PeriodToRun
			AND ProviderId IN (
									SELECT ProviderId
									FROM Snapshot_Provider SP
									WHERE [Period] = @PeriodToRun	
										AND RoATPFFlag = 1
										AND ProviderId NOT IN (SELECT ProviderId FROM Snapshot_Course WHERE [Period] = @PeriodToRun)
							  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_Provider 
		-------------------------------------------------------------------------------------------------------

		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_ProviderUser
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_ProviderUser data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_ProviderUser] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_ProviderUser]
		(
			[Period],
			[ProviderId],
			[UserId]
		)
		SELECT @PeriodToRun,
			[ProviderId],
			[UserId]
		FROM [Snapshot_ProviderUser] SPU
		WHERE [Period] = @PreviousPeriod
			AND NOT EXISTS (
								SELECT Id
								FROM DWH_Audit_Summary
								WHERE TableName = 'ProviderUser'
									AND Id = SPU.ProviderId
									AND Id3 = SPU.UserId
						   );

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_ProviderUser]
		(
			[Period],
			[ProviderId],
			[UserId]
		)
		SELECT @PeriodToRun,
			[ProviderId],
			[UserId]
		FROM [Remote].[Audit_ProviderUser]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'ProviderUser'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_ProviderUser
		-------------------------------------------------------------------------------------------------------


		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_QualityScore
		-------------------------------------------------------------------------------------------------------

		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_QualityScore data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_QualityScore] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		EXEC [up_ProviderUpdateAllQualityScores] @PeriodToRun = @PeriodToRun;

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_QualityScore 
		-------------------------------------------------------------------------------------------------------


		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_Venue 
		-------------------------------------------------------------------------------------------------------
		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting Snapshot_Venue data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [Snapshot_Venue] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		-- If record has not been updated in this period, move across from previous period
		INSERT INTO [Snapshot_Venue]
		(
			[Period],
			[VenueId],
			[ProviderId],
			[ProviderOwnVenueRef],
			[VenueName],
			[Email],
			[Website],
			[Fax],
			[Facilities],
			[RecordStatusId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[AddressId],
			[Telephone],
			[BulkUploadVenueId]
		)
		SELECT @PeriodToRun,
			[VenueId],
			[ProviderId],
			[ProviderOwnVenueRef],
			[VenueName],
			[Email],
			[Website],
			[Fax],
			[Facilities],
			[RecordStatusId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[AddressId],
			[Telephone],
			[BulkUploadVenueId]
		FROM [Snapshot_Venue]
		WHERE [Period] = @PreviousPeriod
			AND VenueId NOT IN (
									SELECT Id
									FROM DWH_Audit_Summary 
									WHERE TableName = 'Venue'
								);

		-- If record has been updated (but not deleted) in the period, get latest version of the record in this period
		INSERT INTO [Snapshot_Venue]
		(
			[Period],
			[VenueId],
			[ProviderId],
			[ProviderOwnVenueRef],
			[VenueName],
			[Email],
			[Website],
			[Fax],
			[Facilities],
			[RecordStatusId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[AddressId],
			[Telephone],
			[BulkUploadVenueId]
		)
		SELECT @PeriodToRun,
			[VenueId],
			[ProviderId],
			[ProviderOwnVenueRef],
			[VenueName],
			[Email],
			[Website],
			[Fax],
			[Facilities],
			[RecordStatusId],
			[CreatedByUserId],
			[CreatedDateTimeUtc],
			[ModifiedByUserId],
			[ModifiedDateTimeUtc],
			[AddressId],
			[Telephone],
			[BulkUploadVenueId]
		FROM [Remote].[Audit_Venue]
		WHERE [AuditOperation] <> 'D'
			AND [AuditSeq] IN (
								SELECT AuditSeq
								FROM DWH_Audit_Summary 
								WHERE TableName = 'Venue'
			    			  );

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_Venue 
		-------------------------------------------------------------------------------------------------------

		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_CourseInstance IsDfE and IsSFA Flags 
		-------------------------------------------------------------------------------------------------------

		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Populate Snapshot_CourseInstance IsDfE and IsSFA Flags';

		UPDATE Snapshot_CourseInstance
		SET IsDfE = 1
		WHERE [Period] = @PeriodToRun
			AND CourseInstanceId IN (
										SELECT DISTINCT SCI.CourseInstanceId
										FROM Snapshot_CourseInstanceA10FundingCode SCIFC
											INNER JOIN Snapshot_CourseInstance SCI ON SCI.CourseInstanceId = SCIFC.CourseInstanceId AND SCI.[Period] = SCIFC.[Period]
											INNER JOIN Snapshot_Course SC ON SC.CourseId = SCI.CourseId AND SC.[Period] = SCi.[Period]
											INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SC.ProviderId AND SP.[Period] = SC.[Period]
										WHERE SCIFC.[Period] = @PeriodToRun
											AND (
													SCIFC.A10FundingCode = 25
													OR
													SP.SFAFunded = 0
												)
											AND SP.DFE1619Funded = 1
									);

		UPDATE Snapshot_CourseInstance
		SET IsSFA = 1
		WHERE [Period] = @PeriodToRun
			AND CourseInstanceId NOT IN (
											SELECT DISTINCT SCI.CourseInstanceId
											FROM Snapshot_CourseInstanceA10FundingCode SCIFC
												INNER JOIN Snapshot_CourseInstance SCI ON SCI.CourseInstanceId = SCIFC.CourseInstanceId AND SCI.[Period] = SCIFC.[Period]
												INNER JOIN Snapshot_Course SC ON SC.CourseId = SCI.CourseId AND SC.[Period] = SCi.[Period]
												INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SC.ProviderId AND SP.[Period] = SC.[Period]
											WHERE SCIFC.[Period] = @PeriodToRun
												AND SP.DFE1619Funded = 1
												AND (
														SCIFC.A10FundingCode = 25
														OR
														SP.SFAFunded = 0
													)
										);


		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_CourseInstance IsDfE and IsSFA Flags 
		-------------------------------------------------------------------------------------------------------

		-------------------------------------------------------------------------------------------------------
		-- Populate Snapshot_Course IsDfE and IsSFA Flags 
		-------------------------------------------------------------------------------------------------------

		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Populate Snapshot_Course IsDfE and IsSFA Flags';

		UPDATE Snapshot_Course
		SET IsDfE = 1
		WHERE [Period] = @PeriodToRun
			AND CourseId IN (
								SELECT DISTINCT CourseId
								FROM Snapshot_CourseInstance
								WHERE RecordStatusId = @LiveStatusId
									AND IsDfE = 1
									AND [Period] = @PeriodToRun
							);


		UPDATE Snapshot_Course
		SET IsSFA = 1
		WHERE [Period] = @PeriodToRun
			AND CourseId IN (
								SELECT DISTINCT CourseId
								FROM Snapshot_CourseInstance
								WHERE RecordStatusId = @LiveStatusId
									AND IsSFA = 1
									AND [Period] = @PeriodToRun
							);

		-------------------------------------------------------------------------------------------------------
		-- End of Populate Snapshot_Course IsDfE and IsSFA Flags 
		-------------------------------------------------------------------------------------------------------


		-------------------------------------------------------------------------------------------------------
		-- Generate aggregated data for Monthly Report
		-------------------------------------------------------------------------------------------------------

		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting MonthlyReport_Usage data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [MonthlyReport_Usage] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		INSERT [MonthlyReport_Usage]
		(
			[Period], 
			[NumberOfProvidersWithValidSuperUser], 
			[NumberOfProviders], 
			[NumberOfProvidersUpdatedOpportunityInPeriod], 
			[TotalBulkUploadOpportunities], 
			[NumberOfBulkUploadOpportunitiesInPeriod], 
			[TotalManuallyUpdatedOpportunities],
			[NumberOfManuallyUpdatedOpportunitiesInPeriod], 
			[NumberOfProvidersNotUpdatedOpportunityInPastYear], 
			[NumberOfProvidersUpdatedDuringPeriod], 
			[NumberOfProvidersUpdated1to2PeriodsAgo], 
			[NumberOfProvidersUpdated2to3PeriodsAgo], 
			[NumberOfProvidersUpdatedMoreThan3PeriodsAgo]
		)
		SELECT @PeriodToRun,
			COALESCE(P.SuperUserCount, 0),
			COALESCE(P.ProviderCount, 0),
			COALESCE(CI.NumberOfOpportunitiesUpdatedInPeriod, 0),
			COALESCE(CI.BulkUploadOpportunities, 0),
			COALESCE(CI.BulkUploadOpportunitiesInPeriod, 0),
			COALESCE(CI.TotalManuallyUpdatedOpportunities, 0),
			COALESCE(CI.ManualOpportunitiesInPeriod, 0),
		 	COALESCE(P.ProvidersNotUpdatedInLastYear, 0),
			COALESCE(P.ProvidersUpdatedInLastMonth, 0),
			COALESCE(P.ProvidersUpdatedBetween1and2Months, 0),
			COALESCE(P.ProvidersUpdatedBetween2and3Months, 0),
			COALESCE(P.ProvidersUpdatedMoreThan3Months, 0)
		FROM (
				SELECT Sum(CASE WHEN A.SuperUserCount > 0 THEN 1 ELSE 0 END) As SuperUserCount,
					Count(*) AS ProviderCount,
					Sum(CASE WHEN A.LastUpdatedOpportunity BETWEEN @PeriodStartDate AND @NextPeriodStartDate THEN 1 ELSE 0 END) AS ProvidersUpdatedInLastMonth,
					Sum(CASE WHEN A.LastUpdatedOpportunity BETWEEN DateAdd(MONTH, -1, @PeriodStartDate) AND @PeriodStartDate THEN 1 ELSE 0 END) AS ProvidersUpdatedBetween1and2Months,
					Sum(CASE WHEN A.LastUpdatedOpportunity BETWEEN DateAdd(MONTH, -2, @PeriodStartDate) AND DateAdd(MONTH, -1, @PeriodStartDate) THEN 1 ELSE 0 END) AS ProvidersUpdatedBetween2and3Months,
					Sum(CASE WHEN COALESCE(A.LastUpdatedOpportunity, CAST('01 Jan 2000' AS DATE)) < DateAdd(MONTH, -2, @PeriodStartDate) THEN 1 ELSE 0 END) AS ProvidersUpdatedMoreThan3Months,
					Sum(CASE WHEN COALESCE(A.LastUpdatedOpportunity, CAST('01 Jan 2000' AS DATE)) < DateAdd(YEAR, -1, @NextPeriodStartDate) THEN 1 ELSE 0 END) AS ProvidersNotUpdatedInLastYear
				FROM (
						SELECT SPU.ProviderId,
							SUM(CASE WHEN P.PermissionName = 'CanAddEditProviderUsers' THEN 1 ELSE 0 END) AS SuperUserCount,
							Max(SCI.LastUpdated) AS LastUpdatedOpportunity,
							Max(SU.LastLoginDateTimeUtc) AS LastLoginDateTime
						FROM Snapshot_Provider SP
							INNER JOIN Snapshot_ProviderUser SPU ON SPU.ProviderId = SP.ProviderId AND SPU.[Period] = SP.[Period]
							LEFT OUTER JOIN Snapshot_AspNetUsers SU ON SU.Id = SPU.UserId AND SU.[Period] = SPU.[Period]
							LEFT OUTER JOIN [Remote].AspNetUserRoles UR ON UR.UserId = SPU.UserId
							LEFT OUTER JOIN [Remote].PermissionInRole PIR ON PIR.RoleId = UR.RoleId
							LEFT OUTER JOIN [Remote].Permission P ON P.PermissionId = PIR.PermissionId
							LEFT OUTER JOIN (
												SELECT SC.ProviderId,
													Max(COALESCE(SCI.ModifiedDateTimeUtc, SCI.CreatedDateTimeUtc)) As LastUpdated
												FROM Snapshot_Course SC
													INNER JOIN Snapshot_CourseInstance SCI ON SCI.CourseId = SC.CourseId AND SCI.[Period] = SC.[Period]
												GROUP BY SC.ProviderId
											) SCI ON SCI.ProviderId = SPU.ProviderId
						WHERE SP.[Period] = @PeriodToRun
							AND SU.IsDeleted = 0
							AND SP.SFAFunded = 1
							AND SP.RecordStatusId = @LiveStatusId
							AND SP.IsTASOnly = 0
						GROUP BY SPU.ProviderId
					) A			
				) P,
				(
					SELECT Sum(CASE WHEN COALESCE(SCI.ModifiedDateTimeUtc, SCI.CreatedDateTimeUtc) BETWEEN @PeriodStartDate AND @NextPeriodStartDate THEN 1 ELSE 0 END) As NumberOfOpportunitiesUpdatedInPeriod,
						Sum(CASE WHEN SCI.AddedByApplicationId = @BulkUploadApplicationId THEN 1 ELSE 0 END) AS BulkUploadOpportunities,
						Sum(CASE WHEN SCI.AddedByApplicationId = @BulkUploadApplicationId AND COALESCE(SCI.ModifiedDateTimeUtc, SCI.CreatedDateTimeUtc) BETWEEN @PeriodStartDate AND @NextPeriodStartDate THEN 1 ELSE 0 END) AS BulkUploadOpportunitiesInPeriod,
						Sum(CASE WHEN SCI.AddedByApplicationId = @PortalApplicationId THEN 1 ELSE 0 END) AS TotalManuallyUpdatedOpportunities,
						Sum(CASE WHEN SCI.AddedByApplicationId = @PortalApplicationId AND COALESCE(SCI.ModifiedDateTimeUtc, SCI.CreatedDateTimeUtc) BETWEEN @PeriodStartDate AND @NextPeriodStartDate THEN 1 ELSE 0 END) AS ManualOpportunitiesInPeriod,
						Count(*) AS NumberOfLiveOpportunities
					FROM Snapshot_CourseInstance SCI
						INNER JOIN Snapshot_Course SC ON SC.CourseId = SCI.CourseId AND SC.[Period] = SCI.[Period]
						INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SC.ProviderId AND SP.[Period] = SC.[Period]
					WHERE SCI.RecordStatusId = @LiveStatusId
						AND SP.SFAFunded = 1						
						AND SP.RecordStatusId = @LiveStatusId
						AND SP.IsTASOnly = 0
						AND SCI.[Period] = @PeriodToRun
				) CI;


		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting MonthlyReport_Provision data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [MonthlyReport_Provision] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		INSERT [MonthlyReport_Provision]
		(
			[Period], 
			[NumberOfCourses],
			[NumberOfLiveCourses],
			[NumberOfOpportunities],
			[NumberOfLiveOpportunities],
			[ProvidersWithNoCourses],
			[SM_Flexible],
			[SM_FullTime],
			[SM_NotKnown],
			[SM_PartOfFullTimeProgramme],
			[SM_PartTime],
			[AM_DistanceWithAttendance],
			[AM_DistanceWithoutAttendance],
			[AM_FaceToFace],
			[AM_LocationCampus],
			[AM_MixedMode],
			[AM_NotKnown],
			[AM_OnlineWithAttendance],
			[AM_OnlineWithoutAttendance],
			[AM_WorkBased],
			[AP_Customised],
			[AP_DayBlockRelease],
			[AP_Daytime],
			[AP_Evening],
			[AP_NotApplicable],
			[AP_NotKnown],
			[AP_Twilight],
			[AP_Weekend],
			[DU_1WeekOrLess],
			[DU_1To4Weeks],
			[DU_1To3Months],
			[DU_3To6Months],
			[DU_6To12Months],
			[DU_1To2Years],
			[DU_NotKnown],
			[QT_14To19Diploma],
			[QT_AccessToHigherEducation],
			[QT_Apprenticeship],
			[QT_BasicKeySkill],
			[QT_CertificateOfAttendance],
			[QT_CourseProviderCertificate],
			[QT_ExternalAwardedQualification],
			[QT_FoundationalDegree],
			[QT_FunctionalSkill],
			[QT_GCEOrEquivalent],
			[QT_GCSEOrEquivalent],
			[QT_HncHnd],
			[QT_InternationalBacculaureate],
			[QT_NoQualification],
			[QT_NVQ],
			[QT_OtherAccreditedQualification],
			[QT_Postgraduate],
			[QT_Undergraduate],
			[QT_IndustrySpecificQualification],
			[QL_EntryLevel],
			[QL_HigherLevel],
			[QL_Level1],
			[QL_Level2],
			[QL_Level3],
			[QL_Level4],
			[QL_Level5],
			[QL_Level6],
			[QL_Level7],
			[QL_Level8],
			[QL_NotKnown]
		)
		SELECT @PeriodToRun,
			COALESCE(C.NumberOfCourses, 0),
			COALESCE(C.NumberOfLiveCourses, 0),
			COALESCE(C.NumberOfOpportunities, 0),
			COALESCE(C.NumberOfLiveOpportunities, 0),
			COALESCE(P.NumberOfProvidersWithNoCourses, 0),
			COALESCE(SM.Flexible, 0),
			COALESCE(SM.FullTime, 0),
			COALESCE(SM.NotKnown, 0),
			COALESCE(SM.PartOfFullTimeProgramme, 0),
			COALESCE(SM.PartTime, 0),
			COALESCE(AM.DistanceWithAttendance, 0),
			COALESCE(AM.DistanceWithoutAttendance, 0),
			COALESCE(AM.FaceToFace, 0),
			COALESCE(AM.LocationCampus, 0),
			COALESCE(AM.MixedMode, 0),
			COALESCE(AM.NotKnown, 0),
			COALESCE(AM.OnlineWithAttendance, 0),
			COALESCE(AM.OnlineWithoutAttendance, 0),
			COALESCE(AM.WorkBased, 0),
			COALESCE(AP.Customised, 0),
			COALESCE(AP.DayRelease, 0),
			COALESCE(AP.Daytime, 0),
			COALESCE(AP.Evening, 0),
			COALESCE(AP.NotApplicable, 0),
			COALESCE(AP.NotKnown, 0),
			COALESCE(AP.Twilight, 0),
			COALESCE(AP.Weekend, 0),
			COALESCE(DU.OneWeekOrLess, 0),
			COALESCE(DU.OneToFourWeeks, 0),
			COALESCE(DU.OneToThreeMonths, 0),
			COALESCE(DU.ThreeToSixMonths, 0),
			COALESCE(DU.SixToTwelveMonths, 0),
			COALESCE(DU.OneToTwoYears, 0),
			COALESCE(DU.NotKnown, 0),
			COALESCE(QT.Diploma, 0),
			COALESCE(QT.AccessToHigherEducation, 0),
			COALESCE(QT.Apprenticeship, 0),
			COALESCE(QT.BasicSkill, 0),
			COALESCE(QT.CertificateOfAttendance, 0),
			COALESCE(QT.CourseProviderCertificate, 0),
			COALESCE(QT.ExternalAward, 0),
			COALESCE(QT.FoundationDegree, 0),
			COALESCE(QT.FunctionalSkill, 0),
			COALESCE(QT.GCEOrEquivalent, 0),
			COALESCE(QT.GCSEOrEquivalent, 0),
			COALESCE(QT.HncHnd, 0),
			COALESCE(QT.InternationalBacculaureate, 0),
			COALESCE(QT.NoQualification, 0),
			COALESCE(QT.NVQ, 0),
			COALESCE(QT.AccreditedQualification, 0),
			COALESCE(QT.Postgraduate, 0),
			COALESCE(QT.Undergraduate, 0),
			COALESCE(QT.ProfessionalQualification, 0),
			COALESCE(QL.EntryLevel, 0),
			COALESCE(QL.HigherLevel, 0),
			COALESCE(QL.Level1, 0),
			COALESCE(QL.Level2, 0),
			COALESCE(QL.Level3, 0),
			COALESCE(QL.Level4, 0),
			COALESCE(QL.Level5, 0),
			COALESCE(QL.Level6, 0),
			COALESCE(QL.Level7, 0),
			COALESCE(QL.Level8, 0),
			COALESCE(QL.NotKnown, 0)
		FROM (
				SELECT Count(*) AS NumberOfCourses,
					Sum(CASE WHEN SC.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumberOfLiveCourses,
					Sum(COALESCE(NumOpportunities, 0)) AS NumberOfOpportunities,
					Sum(COALESCE(NumLiveOpportunities, 0)) AS NumberOfLiveOpportunities
				FROM Snapshot_Course SC					
					LEFT OUTER JOIN (
						SELECT CI.CourseId, Count(*) AS NumOpportunities, 
						Sum(CASE WHEN CI.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumLiveOpportunities 
						FROM Snapshot_CourseInstance CI
						Join Snapshot_Course C on CI.CourseId = C.CourseID and C.Period = @PeriodToRun
						JOIN Snapshot_Provider P on C.ProviderId = P.ProviderId and P.Period = @PeriodToRun
						WHERE CI.[Period] = @PeriodToRun and P.SFAFunded = 1
						GROUP BY CI.CourseId) SCI ON SCI.CourseId = SC.CourseId
				JOIN Snapshot_Provider P on SC.ProviderId = P.ProviderId and P.Period = @PeriodToRun
				WHERE SC.[Period] = @PeriodToRun and P.SFAFunded = 1
					
			 ) C,
			 (
				SELECT Count(*) AS NumberOfProvidersWithNoCourses
				FROM Snapshot_Provider SP
				WHERE SP.[Period] = @PeriodToRun
					AND SP.ProviderId NOT IN (SELECT DISTINCT ProviderId FROM Snapshot_Course WHERE [Period] = @PeriodToRun AND RecordStatusId = @LiveStatusId)
					AND SP.IsTASOnly = 0
					AND SP.RecordStatusId = @LiveStatusId
					AND SP.SFAFunded = 1
			 ) P,
			 (
				SELECT Sum(CASE WHEN StudyModeId = 4 THEN 1 ELSE 0 END) AS Flexible,
					Sum(CASE WHEN StudyModeId = 1 THEN 1 ELSE 0 END) AS FullTime,
					Sum(CASE WHEN StudyModeId = 5 THEN 1 ELSE 0 END) AS NotKnown,
					Sum(CASE WHEN StudyModeId = 3 THEN 1 ELSE 0 END) AS PartOfFullTimeProgramme,
					Sum(CASE WHEN StudyModeId = 2 THEN 1 ELSE 0 END) AS PartTime
				FROM Snapshot_CourseInstance SCI
					Join Snapshot_Course C on SCI.CourseId = C.CourseID and C.Period = @PeriodToRun
					JOIN Snapshot_Provider P on C.ProviderId = P.ProviderId and P.Period = @PeriodToRun
				WHERE SCI.[Period] = @PeriodToRun
					AND SCI.RecordStatusId = @LiveStatusId
					AND P.SFAFunded = 1
			 ) SM,
			 (
				SELECT Sum(CASE WHEN AttendanceTypeId = 5 THEN 1 ELSE 0 END) AS DistanceWithAttendance,
					Sum(CASE WHEN AttendanceTypeId = 6 THEN 1 ELSE 0 END) AS DistanceWithoutAttendance,
					Sum(CASE WHEN AttendanceTypeId = 2 THEN 1 ELSE 0 END) AS FaceToFace,
					Sum(CASE WHEN AttendanceTypeId = 1 THEN 1 ELSE 0 END) AS LocationCampus,
					Sum(CASE WHEN AttendanceTypeId = 4 THEN 1 ELSE 0 END) AS MixedMode,
					Sum(CASE WHEN AttendanceTypeId = 9 THEN 1 ELSE 0 END) AS NotKnown,
					Sum(CASE WHEN AttendanceTypeId = 2 THEN 1 ELSE 0 END) AS OnlineWithAttendance,
					Sum(CASE WHEN AttendanceTypeId = 8 THEN 1 ELSE 0 END) AS OnlineWithoutAttendance,
					Sum(CASE WHEN AttendanceTypeId = 3 THEN 1 ELSE 0 END) AS WorkBased
				FROM Snapshot_CourseInstance SCI
				Join Snapshot_Course C on SCI.CourseId = C.CourseID and C.Period = @PeriodToRun
				JOIN Snapshot_Provider P on C.ProviderId = P.ProviderId and P.Period = @PeriodToRun
				WHERE SCI.[Period] = @PeriodToRun
					AND SCI.RecordStatusId = @LiveStatusId
					AND P.SFAFunded = 1
			 ) AM,
			 (
				SELECT Sum(CASE WHEN AttendancePatternId = 6 THEN 1 ELSE 0 END) AS Customised,
					Sum(CASE WHEN AttendancePatternId = 2 THEN 1 ELSE 0 END) AS DayRelease,
					Sum(CASE WHEN AttendancePatternId = 1 THEN 1 ELSE 0 END) AS Daytime,
					Sum(CASE WHEN AttendancePatternId = 3 THEN 1 ELSE 0 END) AS Evening,
					Sum(CASE WHEN AttendancePatternId = 8 THEN 1 ELSE 0 END) AS NotApplicable,
					Sum(CASE WHEN AttendancePatternId = 7 THEN 1 ELSE 0 END) AS NotKnown,
					Sum(CASE WHEN AttendancePatternId = 4 THEN 1 ELSE 0 END) AS Twilight,
					Sum(CASE WHEN AttendancePatternId = 5 THEN 1 ELSE 0 END) AS Weekend
				FROM Snapshot_CourseInstance SCI
				Join Snapshot_Course C on SCI.CourseId = C.CourseID and C.Period = @PeriodToRun
				JOIN Snapshot_Provider P on C.ProviderId = P.ProviderId and P.Period = @PeriodToRun
				WHERE SCI.[Period] = @PeriodToRun
					AND SCI.RecordStatusId = @LiveStatusId
					AND P.SFAFunded = 1
			 ) AP,
			 (
				SELECT Sum(CASE WHEN DU.DurationUnitId IS NOT NULL AND SCI.DurationUnit * DU.WeekEquivalent < 1 THEN 1 ELSE 0 END) AS OneWeekOrLess,
					Sum(CASE WHEN DU.DurationUnitId IS NOT NULL AND SCI.DurationUnit * DU.WeekEquivalent BETWEEN 1 AND 4 THEN 1 ELSE 0 END) AS OneToFourWeeks,
					Sum(CASE WHEN DU.DurationUnitId IS NOT NULL AND SCI.DurationUnit * DU.WeekEquivalent BETWEEN 4 AND 13 THEN 1 ELSE 0 END) AS OneToThreeMonths,
					Sum(CASE WHEN DU.DurationUnitId IS NOT NULL AND SCI.DurationUnit * DU.WeekEquivalent BETWEEN 13 AND 26 THEN 1 ELSE 0 END) AS ThreeToSixMonths,
					Sum(CASE WHEN DU.DurationUnitId IS NOT NULL AND SCI.DurationUnit * DU.WeekEquivalent BETWEEN 26 AND 52 THEN 1 ELSE 0 END) AS SixToTwelveMonths,
					Sum(CASE WHEN DU.DurationUnitId IS NOT NULL AND SCI.DurationUnit * DU.WeekEquivalent BETWEEN 52 AND 104 THEN 1 ELSE 0 END) AS OneToTwoYears,
					Sum(CASE WHEN DU.DurationUnitId IS NULL THEN 1 ELSE 0 END) AS NotKnown
				FROM Snapshot_CourseInstance SCI
					Join Snapshot_Course C on SCI.CourseId = C.CourseID and C.Period = @PeriodToRun
					JOIN Snapshot_Provider P on C.ProviderId = P.ProviderId and P.Period = @PeriodToRun
					LEFT OUTER JOIN [Remote].DurationUnit DU ON DU.DurationUnitId = SCI.DurationUnitId
				WHERE SCI.[Period] = @PeriodToRun
					AND SCI.RecordStatusId = @LiveStatusId
					AND P.SFAFunded = 1
			 ) DU,
			 (
				SELECT Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 9 THEN 1 ELSE 0 END) AS Diploma,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 14 THEN 1 ELSE 0 END) AS AccessToHigherEducation,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 10 THEN 1 ELSE 0 END) AS Apprenticeship,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 4 THEN 1 ELSE 0 END) AS BasicSkill,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 2 THEN 1 ELSE 0 END) AS CertificateOfAttendance,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 5 THEN 1 ELSE 0 END) AS CourseProviderCertificate,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 6 THEN 1 ELSE 0 END) AS ExternalAward,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 16 THEN 1 ELSE 0 END) AS FoundationDegree,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 3 THEN 1 ELSE 0 END) AS FunctionalSkill,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 13 THEN 1 ELSE 0 END) AS GCEOrEquivalent,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 8 THEN 1 ELSE 0 END) AS GCSEOrEquivalent,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 15 THEN 1 ELSE 0 END) AS InternationalBacculaureate,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 12 THEN 1 ELSE 0 END) AS NoQualification,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 1 THEN 1 ELSE 0 END) AS HncHnd,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 11 THEN 1 ELSE 0 END) AS NVQ,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 7 THEN 1 ELSE 0 END) AS AccreditedQualification,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 18 THEN 1 ELSE 0 END) AS Postgraduate,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 17 THEN 1 ELSE 0 END) AS UnderGraduate,
					Sum(CASE WHEN COALESCE(SC.WhenNoLarQualificationTypeId, LA.QualificationTypeId) = 19 THEN 1 ELSE 0 END) AS ProfessionalQualification
				FROM Snapshot_Course SC
					JOIN Snapshot_Provider P on SC.ProviderId = P.ProviderId and P.Period = @PeriodToRun
					LEFT OUTER JOIN [Remote].LearningAim LA ON LA.LearningAimRefId = SC.LearningAimRefId
				WHERE SC.[Period] = @PeriodToRun
					AND SC.RecordStatusId = @LiveStatusId
					AND P.SFAFunded = 1
			 ) QT,
			 (
				SELECT Sum(CASE WHEN QualificationLevelId = 10 THEN 1 ELSE 0 END) AS EntryLevel,
					Sum(CASE WHEN QualificationLevelId = 9 THEN 1 ELSE 0 END) AS HigherLevel,
					Sum(CASE WHEN QualificationLevelId = 1 THEN 1 ELSE 0 END) AS Level1,
					Sum(CASE WHEN QualificationLevelId = 2 THEN 1 ELSE 0 END) AS Level2,
					Sum(CASE WHEN QualificationLevelId = 3 THEN 1 ELSE 0 END) AS Level3,
					Sum(CASE WHEN QualificationLevelId = 4 THEN 1 ELSE 0 END) AS Level4,
					Sum(CASE WHEN QualificationLevelId = 5 THEN 1 ELSE 0 END) AS Level5,
					Sum(CASE WHEN QualificationLevelId = 6 THEN 1 ELSE 0 END) AS Level6,
					Sum(CASE WHEN QualificationLevelId = 7 THEN 1 ELSE 0 END) AS Level7,
					Sum(CASE WHEN QualificationLevelId = 8 THEN 1 ELSE 0 END) AS Level8,
					Sum(CASE WHEN QualificationLevelId = 11 THEN 1 ELSE 0 END) AS NotKnown
				FROM Snapshot_Course SC
					JOIN Snapshot_Provider P on SC.ProviderId = P.ProviderId and P.Period = @PeriodToRun
				WHERE SC.[Period] = @PeriodToRun
					AND SC.RecordStatusId = @LiveStatusId
					AND P.SFAFunded = 1
			 ) QL;


		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting MonthlyReport_Quality data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [MonthlyReport_Quality] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		--Spreadsheet includes note that Quality Ratings are for providers with live courses only
		--up_ProviderUpdateQualityScore which populates the Snapshot_QualityScore table populates the table with data from live providers only
		INSERT [MonthlyReport_Quality]
		(
			[Period], 
			[Poor], 
			[Average], 
			[Good], 
			[VeryGood]
		)
		SELECT @PeriodToRun,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating < 51 THEN 1 ELSE 0 END), 0) AS Poor,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 51 AND AutoAggregateQualityRating < 71 THEN 1 ELSE 0 END), 0) AS Average,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 71 AND AutoAggregateQualityRating < 91 THEN 1 ELSE 0 END), 0) AS Good,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 91 THEN 1 ELSE 0 END), 0) AS VeryGood
		FROM Snapshot_QualityScore SQ
			INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SQ.ProviderId AND SP.[Period] = SQ.[Period]
		WHERE SQ.[Period] = @PeriodToRun
			AND SP.IsTASOnly = 0
			AND SP.SFAFunded = 1
			AND SP.RecordStatusId = @LiveStatusId;

		-------------------------------------------------------------------------------------------------------
		-- End of Generate aggregated data for Monthly Report
		-------------------------------------------------------------------------------------------------------


		-------------------------------------------------------------------------------------------------------
		-- Generate aggregated data for DFE Report
		-------------------------------------------------------------------------------------------------------

		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting DFEReport_Usage data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [DFEReport_Usage] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		INSERT [DFEReport_Usage]
		(
			[Period], 
			[NumberOfProvidersWithValidSuperUser], 
			[NumberOfProviders], 
			[NumberOfProvidersUpdatedOpportunityInPeriod], 
			[TotalBulkUploadOpportunities], 
			[NumberOfBulkUploadOpportunitiesInPeriod], 
			[TotalManuallyUpdatedOpportunities],
			[NumberOfManuallyUpdatedOpportunitiesInPeriod], 
			[NumberOfProvidersNotUpdatedOpportunityInPastYear], 
			[NumberOfProvidersUpdatedDuringPeriod], 
			[NumberOfProvidersUpdated1to2PeriodsAgo], 
			[NumberOfProvidersUpdated2to3PeriodsAgo], 
			[NumberOfProvidersUpdatedMoreThan3PeriodsAgo]
		)
		SELECT @PeriodToRun,
			COALESCE(P.SuperUserCount, 0),
			COALESCE(P.ProviderCount, 0),
			COALESCE(CI.NumberOfOpportunitiesUpdatedInPeriod, 0),
			COALESCE(CI.BulkUploadOpportunities, 0),
			COALESCE(CI.BulkUploadOpportunitiesInPeriod, 0),
			COALESCE(CI.TotalManuallyUpdatedOpportunities, 0),
			COALESCE(CI.ManualOpportunitiesInPeriod, 0),
		 	COALESCE(P.ProvidersNotUpdatedInLastYear, 0),
			COALESCE(P.ProvidersUpdatedInLastMonth, 0),
			COALESCE(P.ProvidersUpdatedBetween1and2Months, 0),
			COALESCE(P.ProvidersUpdatedBetween2and3Months, 0),
			COALESCE(P.ProvidersUpdatedMoreThan3Months, 0)
		FROM (
				SELECT Sum(CASE WHEN A.SuperUserCount > 0 THEN 1 ELSE 0 END) As SuperUserCount,
					Count(*) AS ProviderCount,
					Sum(CASE WHEN A.LastUpdatedOpportunity BETWEEN @PeriodStartDate AND @NextPeriodStartDate THEN 1 ELSE 0 END) AS ProvidersUpdatedInLastMonth,
					Sum(CASE WHEN A.LastUpdatedOpportunity BETWEEN DateAdd(MONTH, -1, @PeriodStartDate) AND @PeriodStartDate THEN 1 ELSE 0 END) AS ProvidersUpdatedBetween1and2Months,
					Sum(CASE WHEN A.LastUpdatedOpportunity BETWEEN DateAdd(MONTH, -2, @PeriodStartDate) AND DateAdd(MONTH, -1, @PeriodStartDate) THEN 1 ELSE 0 END) AS ProvidersUpdatedBetween2and3Months,
					Sum(CASE WHEN COALESCE(A.LastUpdatedOpportunity, CAST('01 Jan 2000' AS DATE)) < DateAdd(MONTH, -2, @PeriodStartDate) THEN 1 ELSE 0 END) AS ProvidersUpdatedMoreThan3Months,
					Sum(CASE WHEN COALESCE(A.LastUpdatedOpportunity, CAST('01 Jan 2000' AS DATE)) < DateAdd(YEAR, -1, @NextPeriodStartDate) THEN 1 ELSE 0 END) AS ProvidersNotUpdatedInLastYear
				FROM (
						SELECT SPU.ProviderId,
							SUM(CASE WHEN P.PermissionName = 'CanAddEditProviderUsers' THEN 1 ELSE 0 END) AS SuperUserCount,
							Max(SCI.LastUpdated) AS LastUpdatedOpportunity,
							Max(SU.LastLoginDateTimeUtc) AS LastLoginDateTime
						FROM Snapshot_Provider SP
							INNER JOIN Snapshot_ProviderUser SPU ON SPU.ProviderId = SP.ProviderId AND SPU.[Period] = SP.[Period]
							LEFT OUTER JOIN Snapshot_AspNetUsers SU ON SU.Id = SPU.UserId AND SU.[Period] = SPU.[Period]
							LEFT OUTER JOIN [Remote].AspNetUserRoles UR ON UR.UserId = SPU.UserId
							LEFT OUTER JOIN [Remote].PermissionInRole PIR ON PIR.RoleId = UR.RoleId
							LEFT OUTER JOIN [Remote].Permission P ON P.PermissionId = PIR.PermissionId
							LEFT OUTER JOIN (
												SELECT SC.ProviderId,
													Max(COALESCE(SCI.ModifiedDateTimeUtc, SCI.CreatedDateTimeUtc)) As LastUpdated
												FROM Snapshot_Course SC
													INNER JOIN Snapshot_CourseInstance SCI ON SCI.CourseId = SC.CourseId AND SCI.[Period] = SC.[Period]
												GROUP BY SC.ProviderId
											) SCI ON SCI.ProviderId = SPU.ProviderId
						WHERE SP.[Period] = @PeriodToRun
							AND SU.IsDeleted = 0
							AND SP.DFE1619Funded = 1
							AND SP.RecordStatusId = @LiveStatusId
							AND SP.IsTASOnly = 0
						GROUP BY SPU.ProviderId
					) A			
				) P,
				(
					SELECT Sum(CASE WHEN COALESCE(SCI.ModifiedDateTimeUtc, SCI.CreatedDateTimeUtc) BETWEEN @PeriodStartDate AND @NextPeriodStartDate THEN 1 ELSE 0 END) As NumberOfOpportunitiesUpdatedInPeriod,
						Sum(CASE WHEN SCI.AddedByApplicationId = @BulkUploadApplicationId THEN 1 ELSE 0 END) AS BulkUploadOpportunities,
						Sum(CASE WHEN SCI.AddedByApplicationId = @BulkUploadApplicationId AND COALESCE(SCI.ModifiedDateTimeUtc, SCI.CreatedDateTimeUtc) BETWEEN @PeriodStartDate AND @NextPeriodStartDate THEN 1 ELSE 0 END) AS BulkUploadOpportunitiesInPeriod,
						Sum(CASE WHEN SCI.AddedByApplicationId = @PortalApplicationId THEN 1 ELSE 0 END) AS TotalManuallyUpdatedOpportunities,
						Sum(CASE WHEN SCI.AddedByApplicationId = @PortalApplicationId AND COALESCE(SCI.ModifiedDateTimeUtc, SCI.CreatedDateTimeUtc) BETWEEN @PeriodStartDate AND @NextPeriodStartDate THEN 1 ELSE 0 END) AS ManualOpportunitiesInPeriod,
						Count(*) AS NumberOfLiveOpportunities
					FROM Snapshot_CourseInstance SCI
						INNER JOIN Snapshot_Course SC ON SC.CourseId = SCI.CourseId AND SC.[Period] = SCI.[Period]
						INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SC.ProviderId AND SP.[Period] = SC.[Period]
					WHERE SCI.RecordStatusId = @LiveStatusId
						AND SP.DFE1619Funded = 1						
						AND SP.RecordStatusId = @LiveStatusId
						AND SCI.[Period] = @PeriodToRun
				) CI;


		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting DFEReport_Provision data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [DFEReport_Provision] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		INSERT [DFEReport_Provision]
		(
			[Period], 
			[NumberOfCourses],
			[NumberOfLiveCourses],
			[NumberOfOpportunities],
			[NumberOfLiveOpportunities],
			[ProvidersWithNoCourses]			
		)
		SELECT @PeriodToRun,
			COALESCE(C.NumberOfCourses, 0) AS NumberOfCourses,
			COALESCE(C.NumberOfLiveCourses, 0) AS NumberOfLiveCourses,
			COALESCE(C.NumberOfOpportunities, 0) AS NumberOfOpportunities,
			COALESCE(C.NumberOfLiveOpportunities, 0) AS NumberOfLiveOpportunities,
			COALESCE(P.NumberOfProvidersWithNoCourses, 0) AS NumberOfProvidersWithNoCourses			
		FROM (
				SELECT Count(*) AS NumberOfCourses,
					Sum(CASE WHEN SC.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumberOfLiveCourses,
					Sum(COALESCE(NumOpportunities, 0)) AS NumberOfOpportunities,
					Sum(COALESCE(NumLiveOpportunities, 0)) AS NumberOfLiveOpportunities
				FROM Snapshot_Course SC
					LEFT JOIN Snapshot_Provider SP ON SP.ProviderId = SC.ProviderId AND SP.Period = SC.Period
					LEFT OUTER JOIN (
										SELECT SCI.CourseId, 
											CASE WHEN Sum(CASE WHEN A10.CourseInstanceId IS NOT NULL THEN 1 ELSE 0 END) = 0 THEN 0 ELSE 1 END AS IsDFE, 
											Count(*) AS NumOpportunities, 
											Sum(CASE WHEN SCI.RecordStatusId = @LiveStatusId THEN 1 ELSE 0 END) AS NumLiveOpportunities 
										FROM Snapshot_CourseInstance SCI 
											LEFT OUTER JOIN (
																SELECT CourseInstanceId 
																FROM Snapshot_CourseInstanceA10FundingCode 
																WHERE Period = @PeriodToRun
																	AND A10FundingCode = 25 															
																GROUP BY CourseInstanceId
															) A10 ON A10.CourseInstanceId = SCI.CourseInstanceId
										WHERE SCI.[Period] = @PeriodToRun 
										GROUP BY SCI.CourseId
									) SCI ON SC.CourseId = SCI.CourseId
				WHERE SC.[Period] = @PeriodToRun
					AND SP.RecordStatusId = @LiveStatusId
					AND SP.DFE1619Funded = 1
					AND (SP.SFAFunded = 0 OR SCI.IsDFE = 1)
			) C,
			(
				SELECT Count(*) AS NumberOfProvidersWithNoCourses
				FROM Snapshot_Provider SP
					LEFT OUTER JOIN Snapshot_Course SC ON SC.ProviderId = SP.ProviderId AND SC.[Period] = SP.[Period]
				WHERE SP.[Period] = @PeriodToRun
					AND SC.ProviderId IS NULL
					AND SP.DFE1619Funded = 1
					AND SP.RecordStatusId = @LiveStatusId
			) P;


		-- Write the log
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = 'Inserting DFEReport_Quality data';

		-- Delete any existing records for this period (and any future periods)
		DELETE FROM [DFEReport_Quality] WHERE [Period] >= @PeriodToRun AND LEFT([Period], 1) = LEFT(@PeriodToRun, 1);

		INSERT [DFEReport_Quality]
		(
			[Period], 
			[Poor], 
			[Average], 
			[Good], 
			[VeryGood]
		)
		SELECT @PeriodToRun,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating < 51 THEN 1 ELSE 0 END), 0) AS Poor,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 51 AND AutoAggregateQualityRating < 71 THEN 1 ELSE 0 END), 0) AS Average,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 71 AND AutoAggregateQualityRating < 91 THEN 1 ELSE 0 END), 0) AS Good,
			COALESCE(Sum(CASE WHEN AutoAggregateQualityRating >= 91 THEN 1 ELSE 0 END), 0) AS VeryGood
		FROM Snapshot_QualityScore SQ
			INNER JOIN Snapshot_Provider SP ON SP.ProviderId = SQ.ProviderId AND SP.[Period] = SQ.[Period]
		WHERE SQ.[Period] = @PeriodToRun
			AND SP.DFE1619Funded = 1
			AND SP.RecordStatusId = @LiveStatusId;

		-------------------------------------------------------------------------------------------------------
		-- End of Generate aggregated data for DFE Report
		-------------------------------------------------------------------------------------------------------


		-- Update latest period
		DELETE FROM DWH_Period_Latest WHERE PeriodType = @PeriodType;
		INSERT INTO DWH_Period_Latest ([Period], [PeriodType], [PeriodName], [PeriodStartDate], [PreviousPeriod], [NextPeriod], [NextPeriodStartDate]) SELECT [Period], [PeriodType], [PeriodName], [PeriodStartDate], [PreviousPeriod], [NextPeriod], [NextPeriodStartDate] FROM DWH_Period WHERE [Period] = @PeriodToRun AND [PeriodType] = @PeriodType;

		SET @ErrorMessage = 'Completed ' + Lower(@PeriodTypeDescription) + ' end for period ' + @PeriodToRun;
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = @ErrorMessage;

	END TRY

	BEGIN CATCH
		THROW;
	END CATCH;

END;
