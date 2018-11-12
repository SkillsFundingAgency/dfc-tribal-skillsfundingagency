CREATE PROCEDURE [dbo].[usp_PeriodEnd]
	@DateToRun		DATE = NULL,
	@ReRun			BIT = 0,
	@PeriodType		NVARCHAR(1) = 'Z'
AS

BEGIN

	DECLARE @LogTypeInformation			NVARCHAR(25) = 'Information';
	DECLARE @LogTypeWarning				NVARCHAR(25) = 'Warning';
	DECLARE @LogTypeError				NVARCHAR(25) = 'Error';
	
	DECLARE @Message					NVARCHAR(1000);

	DECLARE @ErrorNumber				INT;
	DECLARE @ErrorMessage				NVARCHAR(1000);

	BEGIN TRY

		IF (@DateToRun IS NULL)
		BEGIN
			SET @DateToRun = GetDate();
		END;

		IF (@ReRun IS NULL)
		BEGIN
			SET @ReRun = 0;
		END;

		SET @PeriodType = UPPER(@PeriodType);

		SET @Message = '**** Started with run date ' + CAST(@DateToRun AS VARCHAR(10)) + ', ReRun = ' + CAST(@ReRun AS VARCHAR) + ', PeriodType = ' + @PeriodType;
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = @Message;

		IF (@PeriodType = 'W' OR @PeriodType = 'Z')		
		BEGIN
			EXEC usp_CreateDataForPeriod @PeriodType = 'W', @DateToRun = @DateToRun, @ReRun = @ReRun;
		END;

		IF (@PeriodType = 'M' OR @PeriodType = 'Z')
		BEGIN
			EXEC usp_CreateDataForPeriod @PeriodType = 'M', @DateToRun = @DateToRun, @ReRun = @ReRun;
		END;

		SET @Message = '**** Completed with run date ' + CAST(@DateToRun AS VARCHAR(10)) + ', ReRun = ' + CAST(@ReRun AS VARCHAR) + ', PeriodType = ' + @PeriodType;
		EXEC usp_WriteDWHLog @LogType = @LogTypeInformation, @Message = @Message;

	END TRY

	BEGIN CATCH
		SELECT @ErrorNumber = ERROR_NUMBER(), @ErrorMessage = ERROR_MESSAGE();		
		EXEC usp_WriteDWHLog @LogType = 'Error', @Message = @ErrorMessage;
		THROW;
		RETURN -1;
	END CATCH;

END;

RETURN 0;
