
CREATE PROCEDURE [Search].[DAS_PopulateSearchData]
AS
BEGIN

	DECLARE @LogId int;

	DECLARE @OldDeliveryModeCount FLOAT = 0;
	DECLARE @NewDeliveryModeCount FLOAT = 0;
	
    DECLARE @OldProviderCount FLOAT = 0;
	DECLARE @NewProviderCount FLOAT = 0;

	DECLARE @OldLocationCount FLOAT = 0;
	DECLARE @NewLocationCount FLOAT = 0;

	DECLARE @OldApprenticeshipCount FLOAT = 0;
	DECLARE @NewApprenticeshipCount FLOAT = 0;

    DECLARE @OldApprenticeshipLocationCount FLOAT = 0;
	DECLARE @NewApprenticeshipLocationCount FLOAT = 0;

    DECLARE @OldApprenticeshipLocationDeliveryModeCount FLOAT = 0;
	DECLARE @NewApprenticeshipLocationDeliveryModeCount FLOAT = 0;

	DECLARE @IsValidInput BIT = 0;
 
	BEGIN TRY
 	 
		BEGIN TRANSACTION;

		/** Delivery Mode List **/
		SELECT @OldDeliveryModeCount = Count(*) FROM [Search].[DAS_DeliveryMode];
		TRUNCATE TABLE [Search].[DAS_DeliveryMode];
		INSERT INTO [Search].[DAS_DeliveryMode]
			   (DeliveryModeId,
			    DASRef)
			EXEC [search].[DAS_DeliveryModeList];

		SELECT @NewDeliveryModeCount = Count(*) FROM [Search].[DAS_DeliveryMode];


		/** Provider List **/
		SELECT @OldProviderCount = Count(*) FROM [Search].[DAS_Provider];
		TRUNCATE TABLE [Search].[DAS_Provider];
		INSERT INTO [Search].[DAS_Provider]
			   ([ProviderId]
			   ,[ProviderName]
			   ,[Ukprn]
			   ,[Email]
			   ,[Website]
			   ,[Telephone]
			   ,[MarketingInformation]
			   ,[LearnerSatisfaction]
			   ,[EmployerSatisfaction]
			   ,[National]
			   ,[TradingName])
			EXEC [search].[DAS_ProviderList];

		SELECT @NewProviderCount = Count(*) FROM [Search].[DAS_Provider];

		/** Location List**/
		SELECT @OldLocationCount = Count(*) FROM [Search].[DAS_Location];
		TRUNCATE TABLE [Search].[DAS_Location];
		INSERT INTO [Search].[DAS_Location]
			   ([LocationId]
			   ,[ProviderId]
			   ,[LocationName]
			   ,[AddressLine1]
			   ,[AddressLine2]
			   ,[Town]
			   ,[County]
			   ,[Postcode]
			   ,[Latitude]
			   ,[Longitude]
			   ,[Telephone]
			   ,[Email]
			   ,[Website])
				EXEC [Search].[DAS_LocationList];

		SELECT @NewLocationCount = Count(*) FROM [Search].[DAS_Location];


		/** Apprenticeships **/
		SELECT @OldApprenticeshipCount = Count(*) FROM [Search].[DAS_Apprenticeship];
		TRUNCATE TABLE [Search].[DAS_Apprenticeship];
		INSERT INTO [Search].[DAS_Apprenticeship]
			   ([ApprenticeshipId]
			   ,[ProviderId]
			   ,[StandardCode]
			   ,[FrameworkCode]
			   ,[ProgType]
			   ,[PathwayCode]
			   ,[MarketingInformation]
			   ,[Url]
			   ,[ContactTelephone]
			   ,[ContactEmail]
			   ,[ContactWebsite])
			EXEC [Search].[DAS_ApprenticeshipList];
	
		SELECT @NewApprenticeshipCount = Count(*) FROM [Search].[DAS_Apprenticeship];


		/** Apprenticeship Locations **/
		SELECT @OldApprenticeshipLocationCount = Count(*) FROM [Search].[DAS_ApprenticeshipLocation];
		TRUNCATE TABLE [Search].[DAS_ApprenticeshipLocation];
		INSERT INTO [Search].[DAS_ApprenticeshipLocation]
			   ([ApprenticeshipLocationId]
			   ,[ApprenticeshipId]
			   ,[LocationId]
			   ,[Radius])
			EXEC [Search].[DAS_ApprenticeshipLocationList];
	
		SELECT @NewApprenticeshipLocationCount = Count(*) FROM [Search].[DAS_ApprenticeshipLocation];


		/** Apprenticeship Location Delivery Modes **/
		SELECT @OldApprenticeshipLocationDeliveryModeCount = Count(*) FROM [Search].[DAS_ApprenticeshipLocationDeliveryMode];
		TRUNCATE TABLE [Search].[DAS_ApprenticeshipLocationDeliveryMode];
		INSERT INTO [Search].[DAS_ApprenticeshipLocationDeliveryMode]
			   ([ApprenticeshipLocationId]
			   ,[DeliveryModeId])
			EXEC [Search].[DAS_ApprenticeshipLocationDeliveryModeList];
	
		SELECT @NewApprenticeshipLocationDeliveryModeCount = Count(*) FROM [Search].[DAS_ApprenticeshipLocationDeliveryMode];


		/*Section for ensuring valid data import*/
		DECLARE @ThresholdChecksEnabled bit = 1;
		Select @ThresholdChecksEnabled = isNull(Value,ValueDefault) from [dbo].ConfigurationSettings where name = 'DASExportThresholdChecksEnabled'

		/* Are we doing any threshold checks? */
		IF (@ThresholdChecksEnabled = 1)
		BEGIN
		    /* Don't need to do threshold checks on the DeliveryMode reference data */
			DECLARE @ThresholdPercent int = 100;
			DECLARE @OverrideThreshold bit = 0;
			DECLARE @TableName NVARCHAR(100);

			Select @ThresholdPercent = isNull(Value,ValueDefault) from [dbo].ConfigurationSettings where name = 'DASExportThresholdCheckPercent'
			Select @OverrideThreshold = isNull(Value,ValueDefault) from [dbo].ConfigurationSettings where name = 'DASOverrideThresholdCheck'

			IF (@OldProviderCount <> 0 AND @NewProviderCount / @OldProviderCount * 100 < @ThresholdPercent)
				BEGIN
					SET @IsValidInput = 0;
					SET @TableName = 'Provider';
				END;
			ELSE IF (@OldLocationCount <> 0 AND @NewLocationCount / @OldLocationCount * 100 < @ThresholdPercent)
				BEGIN
					SET @IsValidInput = 0;
					SET @TableName = 'Location';
				END;
			ELSE IF (@OldApprenticeshipCount <> 0 AND @NewApprenticeshipCount / @OldApprenticeshipCount * 100 < @ThresholdPercent)
				BEGIN
					SET @IsValidInput = 0;
					SET @TableName = 'Apprenticeship';
				END;
			ELSE IF (@OldApprenticeshipLocationCount <> 0 AND @NewApprenticeshipLocationCount / @OldApprenticeshipLocationCount * 100 < @ThresholdPercent)
				BEGIN
					SET @IsValidInput = 0;
					SET @TableName = 'ApprenticeshipLocation';
				END;
			ELSE IF (@OldApprenticeshipLocationDeliveryModeCount <> 0 AND @NewApprenticeshipLocationDeliveryModeCount / @OldApprenticeshipLocationDeliveryModeCount * 100 < @ThresholdPercent)
				BEGIN
					SET @IsValidInput = 0;
					SET @TableName = 'ApprenticeshipLocationDeliveryMode';
				END;
			ELSE
				SET @IsValidInput = 1;
		END
		ELSE /* We're not doing any threshold checks so the import is valid */
		BEGIN
			SET @IsValidInput = 1;
		END

		/*If either is valid data, or has been overridden, commit the changes*/
		IF (@IsValidInput = 1 OR @OverrideThreshold = 1) 
			BEGIN
				COMMIT TRANSACTION;

				/*Audit table entries */
				INSERT INTO [Search].[DataExportLog] ([ExportType], [ExecutedOn] ,[IsSuccessful], [IsValidDataImport]) VALUES ('Apprenticeships', GetUtcDate(), 1, @IsValidInput);
				SET @LogId = SCOPE_IDENTITY();

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'DAS_DeliveryMode', @OldDeliveryModeCount, @NewDeliveryModeCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'DAS_Provider', @OldProviderCount, @NewProviderCount);
 
				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'DAS_Location', @OldLocationCount, @NewLocationCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'DAS_Apprenticeship', @OldApprenticeshipCount, @NewApprenticeshipCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'DAS_ApprenticeshipLocation', @OldApprenticeshipLocationCount, @NewApprenticeshipLocationCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'DAS_ApprenticeshipLocationDeliveryMode', @OldApprenticeshipLocationDeliveryModeCount, @NewApprenticeshipLocationDeliveryModeCount);

				IF (@OverrideThreshold = 1) /* Override needs to be set back once overridden*/
				BEGIN
					UPDATE [dbo].[ConfigurationSettings] SET Value = 'false' WHERE Name = 'DASOverrideThresholdCheck';
					SET @IsValidInput = @OverrideThreshold;
				END;
			END;
		ELSE /*The data is invalid and has not been overidden */
			BEGIN
				ROLLBACK;

				/*Audit table entries */
				INSERT INTO [Search].[DataExportLog] ([ExportType], [ExecutedOn], [IsSuccessful], [IsValidDataImport]) VALUES ('Apprenticeships', GetUtcDate(), 0, @IsValidInput);
				SET @LogId = SCOPE_IDENTITY();

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'DAS_DeliveryMode', @OldDeliveryModeCount, @NewDeliveryModeCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'DAS_Provider', @OldProviderCount, @NewProviderCount);
 
				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'DAS_Location', @OldLocationCount, @NewLocationCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'DAS_Apprenticeship', @OldApprenticeshipCount, @NewApprenticeshipCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'DAS_ApprenticeshipLocation', @OldApprenticeshipLocationCount, @NewApprenticeshipLocationCount);

				INSERT INTO [Search].[DataExportLogDetail] ([DataExportLogId], [Name], [ExistingRowCount], [NewRowCount])
					 VALUES (@LogId, 'DAS_ApprenticeshipLocationDeliveryMode', @OldApprenticeshipLocationDeliveryModeCount, @NewApprenticeshipLocationDeliveryModeCount);

				DECLARE @ErrorMessage NVARCHAR(1000) = 'Threshold of ' + CAST(@ThresholdPercent AS NVARCHAR(3)) + '%% Failed for Table ' + @TableName;
				THROW 50000, @ErrorMessage, 1;
			END;
	END TRY
	BEGIN CATCH
	
			IF (@@TRANCOUNT = 1)
				ROLLBACK;

			DECLARE @message VARCHAR(4000);
			SET @message= '[Error Number] ' + CONVERT(NVARCHAR(100), ERROR_NUMBER()) + ' [Procedure] ' + CONVERT(NVARCHAR(100), ERROR_PROCEDURE()) + ' [Error Message] ' + ERROR_MESSAGE();
	
			INSERT INTO [Search].[DataExportLog] ([ExportType], [ExecutedOn], [IsSuccessful], [IsValidDataImport]) VALUES ('Apprenticeships', GetUtcDate(), 0, @IsValidInput);
			SET @LogId = SCOPE_IDENTITY(); 	 
 
			INSERT INTO [Search].[DataExportException] ([DataExportLogId],[ExceptionDetails]) VALUES (@LogId, @message);

			THROW;

	END CATCH

END;
GO


