-- Create functions required for lookups from old to new 
-- and create indexes required to speed up the import
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetParsedUcasTariffPoints]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetParsedUcasTariffPoints]
END
GO
CREATE FUNCTION [dbo].[GetParsedUcasTariffPoints](@UcasEnteredTariff NVARCHAR(50))
RETURNS INT
AS 
BEGIN
	-- Ucas Tariff points have been misused as the old website site had the label 'Tariff Required' this
	-- has been misread in many cases and has money entered!  Ucas Tariff points must be an integer,
	-- so we parse it best we can and anything entered as £'s is return as null for no Ucas Tariff points.

	SET @UcasEnteredTariff = LOWER(@UcasEnteredTariff)
	-- Remove white space
	SET @UcasEnteredTariff = REPLACE(@UcasEnteredTariff, ' ', '')
	-- Remove credits often entered, we need just a number
	SET @UcasEnteredTariff = REPLACE(@UcasEnteredTariff, 'credits', '')

	DECLARE @ReturnValue INT
	IF (@UcasEnteredTariff like '%[^0-9]%') SET @ReturnValue = NULL ELSE SET @ReturnValue = CAST(@UcasEnteredTariff AS INT)

	RETURN @ReturnValue
	
END	
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetParsedPrice]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetParsedPrice]
END
GO
CREATE FUNCTION [dbo].[GetParsedPrice](@Price NVARCHAR(50))
RETURNS DECIMAL(10,2)
AS 
BEGIN	

	DECLARE @ReturnValue DECIMAL(10,2)
	IF (@Price like '%[^0-9.]%') SET @ReturnValue = NULL ELSE SET @ReturnValue = CAST(@Price AS DECIMAL(10,2))

	RETURN @ReturnValue
	
END	
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetParsedInt]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetParsedInt]
END
GO
CREATE FUNCTION [dbo].[GetParsedInt](@Number NVARCHAR(50))
RETURNS INT
AS 
BEGIN	
	DECLARE @ReturnValue INT
	IF (@Number like '%[^0-9]%') SET @ReturnValue = NULL ELSE SET @ReturnValue = CAST(@Number AS INT)

	RETURN @ReturnValue
	
END	
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetA10Code]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetA10Code]
END
GO
CREATE FUNCTION [dbo].[GetA10Code](@OptionValue INT)
RETURNS INT
AS 
BEGIN
	DECLARE @ReturnValue INT =	
		CASE ISNULL(@OptionValue, 0)
			WHEN 56 THEN  10
			WHEN 57 THEN  21
			WHEN 58 THEN  22
			WHEN 59 THEN  45
			WHEN 60 THEN  46
			WHEN 61 THEN  70
			WHEN 62 THEN  80
			WHEN 63 THEN  81
			WHEN 64 THEN  82
			WHEN 65 THEN  99
			WHEN 69 THEN  25
			WHEN 70 THEN  35				
			ELSE NULL
		END
	RETURN @ReturnValue	
END	
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetQualificationLevelId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetQualificationTypeId]
END
GO
CREATE FUNCTION [dbo].[GetQualificationTypeId](@OriginalQualificationType INT)
RETURNS INT
AS 
BEGIN
	DECLARE @ReturnValue INT =	
		CASE ISNULL(@OriginalQualificationType, 0)
			WHEN 10 THEN  1
			WHEN 11 THEN  2
			WHEN 36 THEN  3
			WHEN 37 THEN  4
			WHEN 13 THEN  5
			WHEN 12 THEN  6
			WHEN 14 THEN  7
			WHEN 15 THEN  8
			WHEN 16 THEN  9
			WHEN 18 THEN  10
			WHEN 17 THEN  11
			WHEN 20 THEN  12
			WHEN 22 THEN  13
			WHEN 23 THEN  14
			WHEN 19 THEN  15
			WHEN 21 THEN  16
			WHEN 24 THEN  17
			WHEN 25 THEN  18			
			ELSE NULL
		END
	RETURN @ReturnValue	
END	
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAttendanceTypeId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetAttendanceTypeId]
END
GO
CREATE FUNCTION [dbo].[GetAttendanceTypeId](@OriginalAttendanceModeId INT)
RETURNS INT
AS 
BEGIN
	DECLARE @ReturnValue INT =	
		CASE ISNULL(@OriginalAttendanceModeId, 0)
			WHEN 6 THEN  9
			WHEN 7 THEN  1
			WHEN 8 THEN  2
			WHEN 9 THEN  3
			WHEN 10 THEN  4
			WHEN 11 THEN  5
			WHEN 12 THEN  6
			WHEN 13 THEN  7
			WHEN 67 THEN  8		
			ELSE NULL
		END
	RETURN @ReturnValue	
END	
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAttendancePatternId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetAttendancePatternId]
END
GO
CREATE FUNCTION [dbo].[GetAttendancePatternId](@OriginalAttendancePatternId INT)
RETURNS INT
AS 
BEGIN
	DECLARE @ReturnValue INT =	
		CASE ISNULL(@OriginalAttendancePatternId, 0)
			WHEN 14 THEN  7
			WHEN 15 THEN  6
			WHEN 16 THEN  1
			WHEN 17 THEN  2
			WHEN 18 THEN  3
			WHEN 19 THEN  4
			WHEN 20 THEN  5
			WHEN 68 THEN  8		
			ELSE NULL
		END
	RETURN @ReturnValue	
END	
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDurationUnitId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetDurationUnitId]
END
GO
CREATE FUNCTION [dbo].[GetDurationUnitId](@OriginalDurationUnitId INT)
RETURNS INT
AS 
BEGIN	
	DECLARE @ReturnValue INT =
		CASE ISNULL(@OriginalDurationUnitId, 0)
			WHEN 41 THEN  1
			WHEN 42 THEN  2
			WHEN 43 THEN  3
			WHEN 44 THEN  4
			WHEN 45 THEN  5
			WHEN 46 THEN  6
			WHEN 47 THEN  7		
			ELSE NULL
		END
	RETURN @ReturnValue	
END	
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBitFromFlag]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetBitFromFlag]
END
GO
CREATE FUNCTION [dbo].[GetBitFromFlag](
	@Flag NVARCHAR(10), 
	@SetNullAsFalse BIT)
RETURNS BIT
AS 
BEGIN
	DECLARE @ReturnValue BIT = NULL

	IF(@Flag IS NULL)
	BEGIN
		IF(@SetNullAsFalse = 1) SET @ReturnValue = 0 
	END
	ELSE
	BEGIN
		SET @ReturnValue =
		CASE @Flag
			WHEN 'Y' THEN 1
			WHEN 'y' THEN 1
			WHEN '1' THEN 1
			WHEN 'OK' THEN 1
			ELSE 0 END
	END
	
	RETURN @ReturnValue	
END	
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDateOrNull]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetDateOrNull]
END
GO
CREATE FUNCTION [dbo].[GetDateOrNull](@DateAsText NVARCHAR(50))
RETURNS DATE
AS 
BEGIN
	DECLARE @ReturnValue DATE = NULL
	IF(@DateAsText IS NOT NULL)
	BEGIN
		SELECT @ReturnValue = TRY_PARSE(@DateAsText AS date USING 'en-gb')		
	END
	RETURN @ReturnValue	
END	
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'up_GetDateFromString')
DROP PROCEDURE up_GetDateFromString
GO
CREATE PROCEDURE [dbo].[up_GetDateFromString] 
	@DateString NVARCHAR(20),
	@DateOutput DATE OUTPUT
AS
BEGIN
	
	SET @DateOutput = NULL
	BEGIN TRY
		SET @DateOutput = CAST(@DateString AS DATE)
	END TRY
	BEGIN CATCH
		PRINT 'Failed to convert date ' + @DateString 
	END CATCH

	RETURN 0
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRecordStatus]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetRecordStatus]
END
GO
CREATE FUNCTION [dbo].[GetRecordStatus](@OriginalRecordStatus NVARCHAR(50))
RETURNS INT
AS 
BEGIN
	DECLARE @ReturnValue INT =	
		CASE @OriginalRecordStatus
			WHEN 'PENDING' THEN  1
			WHEN 'LIVE' THEN  2
			WHEN 'ARCHIVED' THEN  3
			WHEN 'DELETED' THEN  4
			WHEN 'TEST' THEN 4  -- If flagged as test we put at 4 for deleted
		END
	RETURN @ReturnValue	
END	
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetQualificationLevelId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetQualificationLevelId]
END
GO
CREATE FUNCTION [dbo].[GetQualificationLevelId](@OriginalQualificationLevel INT)
RETURNS INT
AS 
BEGIN
	DECLARE @ReturnValue INT =	
		CASE ISNULL(@OriginalQualificationLevel, 0)
			WHEN 38 THEN  11
			WHEN 39 THEN  10
			WHEN 40 THEN  1
			WHEN 41 THEN  2
			WHEN 42 THEN  3
			WHEN 43 THEN  4
			WHEN 44 THEN  5
			WHEN 45 THEN  6
			WHEN 46 THEN  7
			WHEN 47 THEN  8
			WHEN 48 THEN  9
			ELSE 11
		END
	RETURN @ReturnValue	
END	
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplicationId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetApplicationId]
END
GO
CREATE FUNCTION [dbo].[GetApplicationId](@OriginalApplication NVARCHAR(50))
RETURNS INT
AS 
BEGIN
	DECLARE @ReturnValue INT =	
		CASE @OriginalApplication
			WHEN 'NDLPP' THEN  1
			WHEN 'BU' THEN  2
			ELSE 3						
		END
	RETURN @ReturnValue	
END	
GO

-- Modify the tables to temporarily hold original Ids for use during the import
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'OriginalUserId' AND object_id = Object_ID(N'AspNetUsers'))
BEGIN
	ALTER TABLE AspNetUsers ADD OriginalUserId INT 
	ALTER TABLE AspNetUsers ADD OriginalAddressId INT
	ALTER TABLE AspNetUsers ADD OriginalModifiedByUserId INT
	ALTER TABLE AspNetUsers ADD OriginalCreatedByUserId INT
	ALTER TABLE Provider ADD OriginalProviderId INT
	ALTER TABLE Organisation ADD OriginalOrganisationId INT
	ALTER TABLE OrganisationType ADD OriginalOrganisationTypeID INT
	ALTER TABLE Course ADD OriginalCourseId INT
	ALTER TABLE CourseInstance ADD OriginalOpportunityId INT
	ALTER TABLE Venue ADD OriginalVenueId INT
END
GO

-- Set primary keys to not null ready  for keys to be added
ALTER TABLE o_opp_attribute_values ALTER COLUMN OPPORTUNITY_ID INT NOT NULL
ALTER TABLE o_opp_attribute_values ALTER COLUMN ATTRIBUTE_ID INT NOT NULL
ALTER TABLE o_learning_aims ALTER COLUMN COURSE_ID INT NOT NULL
ALTER TABLE o_opp_attribute_values ALTER COLUMN ATTRIBUTE_VALUE_ID INT NOT NULL
ALTER TABLE o_learning_aims ALTER COLUMN COURSE_ID INT NOT NULL
ALTER TABLE o_opp_attribute_values ALTER COLUMN ATTRIBUTE_VALUE_ID INT NOT NULL
ALTER TABLE u_users ALTER COLUMN [USER_ID] INT NOT NULL
ALTER TABLE c_provider_attribute_values ALTER COLUMN [PROVIDER_ID] INT NOT NULL
ALTER TABLE c_provider_attribute_values ALTER COLUMN [ATTRIBUTE_ID] INT NOT NULL
ALTER TABLE c_providers ALTER COLUMN [PROVIDER_ID] INT NOT NULL
ALTER TABLE w_address ALTER COLUMN ADDRESS_ID INT NOT NULL
ALTER TABLE o_opportunities ALTER COLUMN OPPORTUNITY_ID INT NOT NULL
ALTER TABLE o_course_attribute_values ALTER COLUMN COURSE_ID INT NOT NULL
ALTER TABLE o_course_attribute_values ALTER COLUMN ATTRIBUTE_ID INT NOT NULL
ALTER TABLE o_courses ALTER COLUMN COURSE_ID INT NOT NULL
ALTER TABLE o_opp_locations ALTER COLUMN OPPORTUNITY_ID INT NOT NULL
ALTER TABLE o_learning_aims ALTER COLUMN COURSE_ID INT NOT NULL
ALTER TABLE c_venue_attribute_values ALTER COLUMN VENUE_ID INT NOT NULL
ALTER TABLE c_venue_attribute_values ALTER COLUMN ATTRIBUTE_ID INT NOT NULL
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_learning_aims' AND object_id = OBJECT_ID('o_learning_aims'))
BEGIN		
	ALTER TABLE [dbo].[o_learning_aims] ADD  CONSTRAINT [PK_o_learning_aims] PRIMARY KEY CLUSTERED 
	(
		[COURSE_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO


IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='IX_o_opp_attribute_values' AND object_id = OBJECT_ID('o_opp_attribute_values'))
BEGIN	
	CREATE NONCLUSTERED INDEX [IX_o_opp_attribute_values] ON [dbo].[o_opp_attribute_values]
	(
		[OPPORTUNITY_ID] ASC,
		[ATTRIBUTE_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO


IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_opp_attribute_values' AND object_id = OBJECT_ID('o_opp_attribute_values'))
BEGIN
	ALTER TABLE [dbo].[o_opp_attribute_values] ADD CONSTRAINT [PK_o_opp_attribute_values] PRIMARY KEY CLUSTERED 
	(
		[ATTRIBUTE_VALUE_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF EXISTS(SELECT * FROM sys.columns WHERE object_id = Object_ID(N'c_org_providers'))
BEGIN
	IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'ImportId' AND object_id = Object_ID(N'c_org_providers'))
	BEGIN
		ALTER TABLE c_org_providers ADD ImportId INT NOT NULL IDENTITY (1,1)
	END
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_opp_start_dates' AND object_id = OBJECT_ID('o_opp_start_dates'))
BEGIN	
	
	IF NOT EXISTS(SELECT * FROM sys.columns WHERE name='ImportId' AND object_id = OBJECT_ID('o_opp_start_dates'))
	BEGIN
		ALTER TABLE [dbo].[o_opp_start_dates] ADD [ImportId]  INT IDENTITY (1,1)
	END
	ALTER TABLE [dbo].[o_opp_start_dates] ADD  CONSTRAINT [PK_o_opp_start_dates] PRIMARY KEY CLUSTERED 
	(
		[ImportId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_course_attribute_values' AND object_id = OBJECT_ID('o_course_attribute_values'))
BEGIN	
	ALTER TABLE [dbo].[o_course_attribute_values] ADD  CONSTRAINT [PK_o_course_attribute_values] PRIMARY KEY CLUSTERED 
	(
		[COURSE_ID] ASC,
		[ATTRIBUTE_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='PK_u_users' AND object_id = OBJECT_ID('u_users'))
BEGIN	
	ALTER TABLE [dbo].[u_users] ADD  CONSTRAINT [PK_u_users] PRIMARY KEY CLUSTERED 
	(
		[USER_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO


IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_opportunity' AND object_id = OBJECT_ID('o_opportunities'))
BEGIN	
	ALTER TABLE [dbo].[o_opportunities] ADD  CONSTRAINT [PK_o_opportunity] PRIMARY KEY CLUSTERED 
	(
		[OPPORTUNITY_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='IX_c_provider_attribute_values' AND object_id = OBJECT_ID('c_provider_attribute_values'))
BEGIN	
	CREATE NONCLUSTERED INDEX [IX_c_provider_attribute_values] ON [dbo].[c_provider_attribute_values]
	(
		[PROVIDER_ID] ASC,
		[ATTRIBUTE_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
		
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='IX_AspNetUsers_OriginalUserId' AND object_id = OBJECT_ID('AspNetUsers'))
BEGIN	
	CREATE NONCLUSTERED INDEX [IX_AspNetUsers_OriginalUserId] ON [dbo].[AspNetUsers]
	(
		[OriginalUserId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
		
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='PK_c_providers' AND object_id = OBJECT_ID('c_providers'))
BEGIN	
	ALTER TABLE [dbo].[c_providers] ADD  CONSTRAINT [PK_c_providers] PRIMARY KEY CLUSTERED 
	(
		[PROVIDER_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='IX_Course_OriginalCourseId' AND object_id = OBJECT_ID('Course'))
BEGIN	

	CREATE UNIQUE NONCLUSTERED INDEX [IX_Course_OriginalCourseId] ON [dbo].[Course]
	(
		[OriginalCourseId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='IX_CourseInstance_OriginalOpportunityId' AND object_id = OBJECT_ID('CourseInstance'))
BEGIN	

	CREATE UNIQUE NONCLUSTERED INDEX [IX_CourseInstance_OriginalOpportunityId] ON [dbo].[CourseInstance]
	(
		[OriginalOpportunityId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='IX_Venue_OriginalVenue' AND object_id = OBJECT_ID('Venue'))
BEGIN	
	CREATE NONCLUSTERED INDEX [IX_Venue_OriginalVenue] ON [dbo].[Venue]
	(
		[OriginalVenueId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='IX_Provider_OriginalProviderId' AND object_id = OBJECT_ID('Provider'))
BEGIN	
	CREATE NONCLUSTERED INDEX [IX_Provider_OriginalProviderId] ON [dbo].[Provider]
	(
		[OriginalProviderId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='PK_w_address' AND object_id = OBJECT_ID('w_address'))
BEGIN
	ALTER TABLE [dbo].[w_address] ADD CONSTRAINT [PK_w_address] PRIMARY KEY CLUSTERED 
	(
		[ADDRESS_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_opp_locations' AND object_id = OBJECT_ID('o_opp_locations'))
BEGIN
	ALTER TABLE [dbo].[o_opp_locations] ADD CONSTRAINT [PK_o_opp_locations] PRIMARY KEY CLUSTERED 
	(
		[OPPORTUNITY_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_learning_aims' AND object_id = OBJECT_ID('o_learning_aims'))
BEGIN	
	ALTER TABLE [dbo].[o_learning_aims] ADD  CONSTRAINT [PK_o_learning_aims] PRIMARY KEY CLUSTERED 
	(
		[COURSE_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_courses' AND object_id = OBJECT_ID('o_courses'))
BEGIN
	ALTER TABLE [dbo].[o_courses] ADD  CONSTRAINT [PK_o_courses] PRIMARY KEY CLUSTERED 
	(
		[COURSE_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO


IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='PK_c_venue_attribute_values' AND object_id = OBJECT_ID('c_venue_attribute_values'))
BEGIN	
	ALTER TABLE [dbo].[c_venue_attribute_values] ADD  CONSTRAINT [PK_c_venue_attribute_values] PRIMARY KEY CLUSTERED 
	(
		[VENUE_ID] ASC,
		[ATTRIBUTE_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

-- Add region mappings to the s_region import table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MapsToProviderRegionId' AND object_id = Object_ID(N's_regions'))
BEGIN
	ALTER TABLE s_regions ADD MapsToProviderRegionId INT
END
GO

-- Add a table for writing import logs for warnings
IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = Object_ID(N'ImportLog'))
BEGIN
	CREATE TABLE [dbo].[ImportLog](
		[ImportLogId] [int] IDENTITY(1,1) NOT NULL,
		[ImportLogText] [nvarchar](max) NOT NULL,
		[DateTimeUtc] DATETIME NOT NULL
	 CONSTRAINT [PK_ImportLog] PRIMARY KEY CLUSTERED 
	(
		[ImportLogId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
ELSE
BEGIN
	-- TODO Take out when dev over
	DELETE FROM ImportLog
END
GO