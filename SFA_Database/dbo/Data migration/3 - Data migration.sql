/**************************************************************************************/
/* Set to 0 to have the transaction committed when happy to complete the import       */
DECLARE @TestImport BIT = 1
/**************************************************************************************/

DECLARE @DefaultRoleId VARCHAR(128) = (SELECT Id FROM AspNetRoles ANR WHERE ANR.Name = 'Provider user')
DECLARE @DateString NVARCHAR(20)

-- Add special system import user, we can use this to default any unknown user Ids to, this allows the import to complete
DECLARE @ImportSystemUser NVARCHAR(128) = LOWER('24314672-F766-47F1-98CB-AD9FC49F6E9D')
IF NOT EXISTS(SELECT * FROM AspNetUsers WHERE Id = @ImportSystemUser)
BEGIN
 INSERT INTO [dbo].[AspNetUsers]
           ([Id]
           ,[EmailConfirmed]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[LockoutEnabled]
           ,[AccessFailedCount]
           ,[UserName] 
		   ,[Email]          
           ,[PasswordResetRequired]
           ,[ProviderUserTypeId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]           
           ,[IsDeleted]
		   ,[SecurityStamp])
     VALUES
           (@ImportSystemUser
           ,0
           ,0
           ,0           
           ,0
           ,0
           ,'importer@tribalgroup.com'
		   ,'importer@tribalgroup.com'
           ,0
           ,1
           ,''
           ,GETUTCDATE()
           ,1
		   ,NEWID())
		   
	UPDATE AspNetUsers SET CreatedByUserId = @ImportSystemUser WHERE Id = @ImportSystemUser
END

-- Add 0 UKPRN entry to allow providers to be imported even if their UKPRN is invalid or wrong, these cases will need to be checked separately
IF NOT EXISTS(SELECT * FROM Ukrlp WHERE Ukprn = 0)
BEGIN
	INSERT INTO Ukrlp (Ukprn, LegalName, UkrlpStatus)
	VALUES (0, 'UKPRN was not recognised during import', 3) -- Record status 3 means archived, we don't want this showing as available for new records
END

-- Add temp mappings to Organisation type to map old ids to new lookup table 
UPDATE OrganisationType SET OriginalOrganisationTypeID = 11 WHERE OrganisationTypeId = 4
UPDATE OrganisationType SET OriginalOrganisationTypeID = 12 WHERE OrganisationTypeId = 1
UPDATE OrganisationType SET OriginalOrganisationTypeID = 13 WHERE OrganisationTypeId = 2
UPDATE OrganisationType SET OriginalOrganisationTypeID = 14 WHERE OrganisationTypeId = 3
UPDATE OrganisationType SET OriginalOrganisationTypeID = 15 WHERE OrganisationTypeId = 5

-- Add mappings to the s_region table that map old regions to new ones
UPDATE s_regions SET MapsToProviderRegionId = 6 WHERE REGION_ID = 1		--London North and East maps to London
UPDATE s_regions SET MapsToProviderRegionId = 6 WHERE REGION_ID = 2		--London South and West maps to London
UPDATE s_regions SET MapsToProviderRegionId = 4 WHERE REGION_ID = 3		--South Central maps to South East
UPDATE s_regions SET MapsToProviderRegionId = 5 WHERE REGION_ID = 4		--South West maps to South West
UPDATE s_regions SET MapsToProviderRegionId = 4 WHERE REGION_ID = 5		--South East maps to South East
UPDATE s_regions SET MapsToProviderRegionId = 5 WHERE REGION_ID = 6		--Thames Valley maps to South West
UPDATE s_regions SET MapsToProviderRegionId = 4 WHERE REGION_ID = 7		--Central Eastern maps to South East
UPDATE s_regions SET MapsToProviderRegionId = 3 WHERE REGION_ID = 8		--West Midlands maps to The Midlands
UPDATE s_regions SET MapsToProviderRegionId = 3 WHERE REGION_ID = 9		--East Midlands maps to The Midlands
UPDATE s_regions SET MapsToProviderRegionId = 2 WHERE REGION_ID = 10	--Man, Cheshire, Warrington and Staffs maps to North West
UPDATE s_regions SET MapsToProviderRegionId = 2 WHERE REGION_ID = 11	--Liverpool City Region, Cumbria and Lancashire maps to North West
UPDATE s_regions SET MapsToProviderRegionId = 1 WHERE REGION_ID = 12	--Yorkshire and the Humber maps to North East
UPDATE s_regions SET MapsToProviderRegionId = 1 WHERE REGION_ID = 13	--North East maps to North East

-- Deal with Nulls, these are imported as empty spaces, put them back to Null
UPDATE c_providers SET PROVIDER_ALIAS = NULL WHERE PROVIDER_ALIAS = ''
UPDATE c_providers SET UKPRN = NULL WHERE UKPRN = ''
UPDATE c_providers SET LSC_SUPPLIER_NO = NULL WHERE LSC_SUPPLIER_NO = 0
UPDATE c_provider_tracking_codes SET PROVIDER_URL_TC_TXT = NULL WHERE PROVIDER_URL_TC_TXT = ''
UPDATE c_provider_tracking_codes SET COURSE_URL_TC_TXT = NULL WHERE COURSE_URL_TC_TXT = ''
UPDATE c_provider_tracking_codes SET BOOKING_URL_TC_TXT = NULL WHERE BOOKING_URL_TC_TXT = ''
UPDATE c_provider_tracking_codes SET VENUE_URL_TC_TXT = NULL WHERE VENUE_URL_TC_TXT = ''
UPDATE c_venues SET PROV_VENUE_ID = NULL WHERE PROV_VENUE_ID = ''
UPDATE u_users SET EMAIL = NULL WHERE EMAIL = ''
UPDATE u_users SET LAST_LOGIN_DATE = NULL WHERE LAST_LOGIN_DATE = ''
UPDATE u_users SET DATE_UPDATED = NULL WHERE DATE_UPDATED = ''
UPDATE u_users SET PHONE = NULL WHERE PHONE = ''
UPDATE o_opportunities SET PROV_OPPORTUNITY_ID = NULL WHERE PROV_OPPORTUNITY_ID = ''
UPDATE o_courses SET PROV_COURSE_ID = NULL WHERE PROV_COURSE_ID = '' 
UPDATE w_address SET ADDRESS_2 = NULL WHERE ADDRESS_2 = ''
UPDATE w_address SET COUNTY = NULL WHERE COUNTY = ''
UPDATE w_address SET COUNTRY = NULL WHERE COUNTRY = ''
UPDATE ld_learning_aim SET ENTRY_SUB_LEVEL_CODE = null WHERE ENTRY_SUB_LEVEL_CODE = ''
UPDATE ld_learning_aim SET NOTIONAL_LEVEL_V2_CODE = null WHERE NOTIONAL_LEVEL_V2_CODE = ''
UPDATE ld_learning_aim SET NOTIONAL_NVQ_LEVEL_CODE = null WHERE NOTIONAL_NVQ_LEVEL_CODE = ''
UPDATE ld_learning_aim SET CREDIT_BASED_TYPE_CODE = null WHERE CREDIT_BASED_TYPE_CODE = ''
UPDATE ld_learning_aim SET OFQUAL_GLH_MAX = NULL WHERE OFQUAL_GLH_MAX = 0
UPDATE ld_learning_aim SET OFQUAL_GLH_MIN = NULL WHERE OFQUAL_GLH_MIN = 0
UPDATE ld_validity_details SET VALIDITY_START_DATE = NULL WHERE VALIDITY_START_DATE = ''
UPDATE ld_validity_details SET VALIDITY_END_DATE = NULL WHERE VALIDITY_END_DATE = ''
UPDATE ld_validity_details SET LAST_DATE_FOR_NEW_STARTS = NULL WHERE LAST_DATE_FOR_NEW_STARTS = ''
UPDATE s_learning_aims SET INDEP_LIVING_SKILLS = NULL WHERE INDEP_LIVING_SKILLS = ''
UPDATE o_opp_start_dates SET START_DATE = NULL WHERE DATEPART(YEAR, START_DATE) < 2000

/************************************************************************************************************************/
-- Checks complete, import the users first
/************************************************************************************************************************/
BEGIN TRANSACTION

-- Jump to Course as rest is now done and committed for dev testing
GOTO COURSE_IMPORT_START
COURSE_IMPORT_START:

SET IDENTITY_INSERT [AspNetUsers] ON
DECLARE @ImportUserId INT
DECLARE db_UserCursor CURSOR FOR SELECT [User_ID] FROM u_users ORDER BY [User_ID]
OPEN db_UserCursor 
FETCH NEXT FROM db_UserCursor INTO @ImportUserId
WHILE @@FETCH_STATUS = 0
BEGIN
	-- Import the user
	DECLARE @Email NVARCHAR(256)
	DECLARE @PasswordHash NVARCHAR(MAX)
	DECLARE @SecurityStamp NVARCHAR(MAX)
	DECLARE @PhoneNumber NVARCHAR(MAX)
	DECLARE @PhoneNumberConfirmed BIT
	DECLARE @UserName NVARCHAR(256)
	DECLARE @Name NVARCHAR(MAX)
	DECLARE @CreatedDateTimeUtc DATETIME
	DECLARE @AddressId INT
	DECLARE @IsDeleted BIT
	DECLARE @StatusString VARCHAR(7)
	DECLARE @NewProviderUserTypeId INT
	DECLARE @ModifiedDateTimeUtc DATETIME
	DECLARE @OldModifiedUserId INT
	DECLARE @OldCreatedUserId INT
	DECLARE @CreatedByUserId NVARCHAR(128)
	DECLARE @ModifiedByUserId NVARCHAR(128)
	DECLARE @ImportCreatedByUserId INT
	DECLARE @ImportModifiedByUserId INT	
	DECLARE @RoleId INT
	DECLARE @LastLoginDateTime DATETIME

	-- Fetch data from import table
	SELECT 
		@Email = LOWER(EMAIL), 
		@PhoneNumber = PHONE,
		@UserName = LOWER(EMAIL),
		@Name = NAME,
		@CreatedDateTimeUtc = DATE_CREATED,
		@StatusString = [STATUS],
		@AddressId = ADDRESS_ID,
		@ImportCreatedByUserId = CREATED_BY,
		@ImportModifiedByUserId = UPDATED_BY,
		@NewProviderUserTypeId = USER_TYPE_ID,  -- Same id's used no need to map
		@RoleId = ROLE_ID,
		@LastLoginDateTime = LAST_LOGIN_DATE
	FROM u_users WHERE [USER_ID] = @ImportUserId

	-- Check the status and convert to the status used in new system
	IF(@StatusString = 'LIVE') SET @IsDeleted = 0 ELSE SET @IsDeleted = 1 
	
	-- Set the address and get FK to assign to the user
	DECLARE @NewAddressId INT = NULL
	IF(@AddressId IS NOT NULL)
	BEGIN
		INSERT INTO [Address] (AddressLine1, AddressLine2, Town, County, Postcode, Latitude, Longitude)
			SELECT ISNULL(ADDRESS_1, ''), ADDRESS_2, TOWN, COUNTY, POSTCODE, null, null 
			FROM w_address WHERE ADDRESS_ID = @AddressId
		SET @NewAddressId = SCOPE_IDENTITY()		
	END
	ELSE
	BEGIN
		SET @NewAddressId = NULL
	END

	PRINT 'Inserting record with UserId ' + CAST(@ImportUserId AS VARCHAR(10))
	-- Insert the user into the new user table
	DECLARE @NewUserId VARCHAR(128) = LOWER(NEWID())

	INSERT INTO [dbo].[AspNetUsers]
			   ([Id]
			   ,[Email]
			   ,[EmailConfirmed]
			   ,[PasswordHash]
			   ,[SecurityStamp]
			   ,[PhoneNumber]
			   ,[PhoneNumberConfirmed]
			   ,[TwoFactorEnabled]
			   ,[LockoutEndDateUtc]
			   ,[LockoutEnabled]
			   ,[AccessFailedCount]
			   ,[UserName]
			   ,[Name]
			   ,[AddressId]
			   ,[PasswordResetRequired]
			   ,[ProviderUserTypeId]
			   ,[CreatedByUserId]
			   ,[CreatedDateTimeUtc]
			   ,[ModifiedByUserId]
			   ,[ModifiedDateTimeUtc]
			   ,[IsDeleted]
			   ,[OriginalUserId]
			   ,[OriginalAddressId]
			   ,[OriginalModifiedByUserId]
			   ,[OriginalCreatedByUserId]
			   ,[LegacyUserId]
			   ,[LastLoginDateTimeUtc]
			   )
		 VALUES
			   (@NewUserId
			   ,@Email
			   ,1
			   ,null
			   ,NEWID()
			   ,@PhoneNumber
			   ,0
			   ,0
			   ,null
			   ,0
			   ,0
			   ,@Email
			   ,@Name
			   ,@NewAddressId
			   ,1
			   ,@NewProviderUserTypeId
			   ,'Updated later'
			   ,@CreatedDateTimeUtc
			   ,null
			   ,@ModifiedDateTimeUtc
			   ,@IsDeleted
			   ,@ImportUserId
			   ,@AddressId
			   ,@ImportCreatedByUserId
			   ,@ImportModifiedByUserId
			   ,@ImportUserId
			   ,@LastLoginDateTime)		
	
	-- Add the role based on the user type
	DECLARE @RoleByProviderUserType VARCHAR(128) =
	CASE @RoleId
		WHEN 1 THEN '9E51B185-6FA5-4474-95A1-CF02DD523203' -- Admin super user
		WHEN 2 THEN 'D9B32EC6-4FC1-4685-98B5-606124924BDF'  -- Admin user
		WHEN 11 THEN '5394B20B-1668-4D4C-AEE4-0FA057AC12B8'  -- Provider super user
		ELSE @DefaultRoleId  -- Provider user 
	END	
	
	INSERT INTO AspNetUserRoles (RoleId, UserId) VALUES (@RoleByProviderUserType, @NewUserId)	
	
	-- Fetch the next user to import	  	
	FETCH NEXT FROM db_UserCursor INTO @ImportUserId
END
CLOSE db_UserCursor
DEALLOCATE db_UserCursor
SET IDENTITY_INSERT [AspNetUsers] OFF

-- Now that all users are imported update the created and modified by ids to the new Ids
UPDATE AspNetUsers SET CreatedByUserId = ISNULL((SELECT ID FROM AspNetUsers ANU WHERE ANU.OriginalUserId = AspNetUsers.OriginalCreatedByUserId), @ImportSystemUser)
UPDATE AspNetUsers SET ModifiedByUserId = ISNULL((SELECT ID FROM AspNetUsers ANU WHERE ANU.OriginalUserId = AspNetUsers.OriginalModifiedByUserId), @ImportSystemUser)

/************************************************************************************************/
-- Import Lars
/************************************************************************************************/
PRINT 'Importing LARS, this may take a few minutes...'
INSERT INTO LearningAimAwardOrg (LearningAimAwardOrgCode, AwardOrgName)
SELECT O.AWARDING_ORGANISATION_CODE, O.AWARDING_ORGANISATION_DESC FROM ld_awarding_organisations O 

INSERT INTO LearningAim (LearningAimRefId, LearningAimAwardOrgCode, LearningAimTitle, Qualification, QualificationTypeId, IndependentLivingSkills)
SELECT DISTINCT LA.LEARNING_AIM_REF, LA.AWARDING_ORGANISATION_CODE, LA.LEARNING_AIM_TITLE, LRT.LARS_LRNAIMREFTYPEDESC, QT.QualificationTypeId, dbo.GetBitFromFlag (SLA.INDEP_LIVING_SKILLS, 0)
FROM ld_learning_aim LA
LEFT OUTER JOIN S_LEARNING_AIMS SLA ON LA.LEARNING_AIM_REF = SLA.LEARNING_AIM_REF
LEFT OUTER JOIN ld_lars_lrnaimreftype LRT ON LA.LEARNING_AIM_TYPE_CODE = LRT.LARS_LRNAIMREFTYPE
LEFT OUTER JOIN s_lad_qt_map QTMap ON LA.LEARNING_AIM_REF = QTMap.LAD_ID
LEFT OUTER JOIN QualificationType QT ON QTMap.CORRECT_QT_CODE = QT.BulkUploadRef

-- Insert the validity
INSERT INTO LearningAimValidity (LearningAimRefId, ValidityCategory, StartDate, LastNewStartDate, EndDate)
SELECT VD.LEARNING_AIM_REF, VD.FUND_MODEL_ILR_SUBSET_CODE, dbo.GetDateOrNull(VD.VALIDITY_START_DATE), dbo.GetDateOrNull(VD.LAST_DATE_FOR_NEW_STARTS), dbo.GetDateOrNull(VD.VALIDITY_END_DATE)
FROM ld_validity_details VD


/************************************************************************************************************************/
-- Import providers and organisations
/************************************************************************************************************************/

DECLARE @ImportProviderId INT
DECLARE db_ProviderCursor CURSOR FOR SELECT [Provider_ID] FROM c_providers ORDER BY [Provider_ID]
OPEN db_ProviderCursor 
FETCH NEXT FROM db_ProviderCursor INTO @ImportProviderId
WHILE @@FETCH_STATUS = 0
BEGIN
	-- Required values to import for provider
	DECLARE @ProviderName NVARCHAR(100)
	DECLARE @ProviderAlias NVARCHAR(100)
	DECLARE @Loans24Plus BIT
	DECLARE @Ukprn INT
	DECLARE @ProviderTypeId INT
	DECLARE @OrganisationTypeId INT
	DECLARE @RecordStatusId INT
	DECLARE @ProviderRegionId INT
	DECLARE @IsContractingBody BIT
	DECLARE @ProviderTrackingUrl NVARCHAR(255)
	DECLARE @VenueTrackingUrl NVARCHAR(255)
	DECLARE @CourseTrackingUrl NVARCHAR(255)
	DECLARE @BookingTrackingUrl NVARCHAR(255)
	DECLARE @RelationshipManagerUserId NVARCHAR(128)
	DECLARE @InformationOfficerUserId NVARCHAR(128)
	DECLARE @ProviderAddressId INT
	DECLARE @ProviderEmail NVARCHAR(255)
	DECLARE @Website NVARCHAR(255)
	DECLARE @Telephone NVARCHAR(255)
	DECLARE @Fax NVARCHAR(30)
	DECLARE @Upin INT
	

	SELECT 
		@Upin = P.LSC_SUPPLIER_NO,
		@OrganisationTypeId = ISNULL(P.ORG_TYPE_ID, 0),
		@ProviderName = P.PROVIDER_NAME,
		@ProviderAlias = P.PROVIDER_ALIAS,
		@Loans24Plus = CASE WHEN TTG_FLAG = 'Y' THEN 1 ELSE 0 END,
		@Ukprn = P.UKPRN,
		@ProviderTypeId = P.PROVIDER_TYPE_ID, -- Direct Id mappings between new and old
		@RecordStatusId = dbo.GetRecordStatus(STATUS),
		@CreatedDateTimeUtc = DATE_CREATED,
		@ModifiedDateTimeUtc = DATE_UPDATED,
		@OldCreatedUserId = CREATED_BY,
		@OldModifiedUserId = UPDATED_BY,
		@ProviderRegionId = R.MapsToProviderRegionId,
		@IsContractingBody = CASE WHEN CONTRACTING_BODY = 'Y' THEN 1 ELSE 0 END,
		@ProviderTrackingUrl = PTC.PROVIDER_URL_TC_TXT,
		@VenueTrackingUrl = PTC.VENUE_URL_TC_TXT,
		@CourseTrackingUrl = PTC.COURSE_URL_TC_TXT,
		@BookingTrackingUrl = PTC.BOOKING_URL_TC_TXT,
		@ProviderEmail = PAVEmail.ATTRIBUTE_VALUE,
		@Website = PAVWebsite.ATTRIBUTE_VALUE,
		@Telephone = PAVTel.ATTRIBUTE_VALUE,
		@Fax = PAVFax.ATTRIBUTE_VALUE,
		@ImportCreatedByUserId = P.CREATED_BY,
		@ImportModifiedByUserId = P.UPDATED_BY			
	FROM c_providers P 
	LEFT OUTER JOIN c_provider_tracking_codes PTC ON P.PROVIDER_ID = PTC.PROVIDER_ID
	LEFT OUTER JOIN s_regions R ON P.REGION_ID = R.REGION_ID
	-- Join to get Email address
	LEFT OUTER JOIN c_provider_attribute_values PAVEmail ON P.PROVIDER_ID = PAVEmail.PROVIDER_ID AND PAVEmail.ATTRIBUTE_ID = 1
	-- Join to get website address
	LEFT OUTER JOIN c_provider_attribute_values PAVWebsite ON P.PROVIDER_ID = PAVWebsite.PROVIDER_ID AND PAVWebsite.ATTRIBUTE_ID = 2
	-- Join to get telephone number
	LEFT OUTER JOIN c_provider_attribute_values PAVTel ON P.PROVIDER_ID = PAVTel.PROVIDER_ID AND PAVTel.ATTRIBUTE_ID = 3
	-- Join to get fax number
	LEFT OUTER JOIN c_provider_attribute_values PAVFax ON P.PROVIDER_ID = PAVFax.PROVIDER_ID AND PAVFax.ATTRIBUTE_ID = 4
	WHERE P.PROVIDER_ID = @ImportProviderId

	-- If UK Prn doesn't exist or is null then flag a warning and default it to allow the import to complete, these cases will need to be checked later
	IF NOT EXISTS(SELECT Ukprn FROM Ukrlp WHERE Ukprn = @Ukprn AND @Ukprn <> 0)
	BEGIN
		PRINT 'Warning: Provider with name ' + @ProviderName + ' (Id ' + CAST(@ImportProviderId AS VARCHAR(10)) + ') has an invalid UKPRN, it will be imported with UKPRN set to 0, this should be manually investigated and corrected'
		INSERT INTO ImportLog (ImportLogText, DateTimeUtc)
		VALUES ('Warning: Provider with name ' + @ProviderName + ' (Id ' + CAST(@ImportProviderId AS VARCHAR(10)) + ') has an invalid UKPRN, it will be imported with UKPRN set to 0, this should be manually investigated and corrected', GETUTCDATE())
		SET @Ukprn = 0
	END
	
	-- Check provider type is valid
	IF(@OrganisationTypeId = 0) -- Importing provider so check provider type
	BEGIN
		IF NOT EXISTS(SELECT * FROM ProviderType WHERE ProviderTypeId = @ProviderTypeId)
		BEGIN
			PRINT 'Warning: Provider with name ' + @ProviderName + ' (Id ' + CAST(@ImportProviderId AS VARCHAR(10)) + ') has an invalid Provider type value and will not be imported'
			INSERT INTO ImportLog (ImportLogText, DateTimeUtc)
			VALUES ('Warning: Provider with name ' + @ProviderName + ' (Id ' + CAST(@ImportProviderId AS VARCHAR(10)) + ') has an invalid Provider type value and will not be imported', GETUTCDATE())
	 		GOTO SKIP_IMPORT
		END
	END
	ELSE
	BEGIN
		-- Importing organisation, check Org type is found in lookup
		IF NOT EXISTS(SELECT * FROM OrganisationType WHERE OriginalOrganisationTypeID = @OrganisationTypeId)
		BEGIN
			PRINT 'Warning: Organisation with name ' + @ProviderName + ' (Id ' + CAST(@ImportProviderId AS VARCHAR(10)) + ') has an invalid Organisation type value, it will be imported set to other'
			INSERT INTO ImportLog (ImportLogText, DateTimeUtc)
			VALUES ('Warning: Organisation with name ' + @ProviderName + ' (Id ' + CAST(@ImportProviderId AS VARCHAR(10)) + ') has an invalid Organisation type value, it will be imported set to other', GETUTCDATE())
	 		SET @OrganisationTypeId = 15 -- 15 is other
		END
	END

	-- Look up associated user that is the Information manager
	SELECT TOP 1 @InformationOfficerUserId = ANU.Id
	FROM u_users U 
	INNER JOIN c_provider_users PU ON U.[USER_ID] = PU.USER_ID
	INNER JOIN AspNetUsers ANU ON PU.[USER_ID] = ANU.OriginalUserId
	WHERE PU.PROVIDER_ID = @ImportProviderId AND U.USER_TYPE_ID = 2 -- 2 is Information officer

	-- Look up associated user that is the relationship manager for this provider
	SELECT TOP 1 @RelationshipManagerUserId = ANU.Id
	FROM u_users U 
	INNER JOIN c_provider_users PU ON U.[USER_ID] = PU.USER_ID
	INNER JOIN AspNetUsers ANU ON PU.[USER_ID] = ANU.OriginalUserId
	WHERE PU.PROVIDER_ID = @ImportProviderId AND U.USER_TYPE_ID = 3 -- 3 is Relationship manager

	-- Fetch the primary address, this is the provider main address
	SELECT TOP 1 @AddressId = A.ADDRESS_ID 
	FROM w_address A		
	INNER JOIN w_provider_address PA ON A.ADDRESS_ID = PA.ADDRESS_ID
	WHERE PA.PROVIDER_ID = @ImportProviderId AND (A.ADDRESS_TYPE_ID = 4 OR A.ADDRESS_TYPE_ID = 1) ORDER BY A.ADDRESS_TYPE_ID ASC -- 4 is the provider primary address, 1 is provider address, which seems to take priority in the CSV outputs
		
	-- Get and add the address	
	IF(@AddressId IS NOT NULL)
	BEGIN
		INSERT INTO [Address] (AddressLine1, AddressLine2, Town, County, Postcode, Latitude, Longitude)
			SELECT ISNULL(ADDRESS_1, ''), ADDRESS_2, TOWN, COUNTY, POSTCODE, null, null 
			FROM w_address WHERE ADDRESS_ID = @AddressId		
		SET @NewAddressId = SCOPE_IDENTITY()
		SET @AddressId = NULL		
	END
	ELSE
	BEGIN
		SET @NewAddressId = NULL
		-- Can't insert with null address Id, add log
		INSERT INTO ImportLog (DateTimeUtc, ImportLogText)
		VALUES (GETUTCDATE(), 'Import for original Provider Id ' + CAST(@ImportProviderId AS VARCHAR(10)) + ' can not be imported as no address record found.')
		GOTO SKIP_IMPORT
	END

	-- Fetch the created by user id
	SET @CreatedByUserId = (SELECT Id FROM AspNetUsers WHERE OriginalUserId = @ImportCreatedByUserId)

	-- Fetch the modified by user id
	SET @ModifiedByUserId = (SELECT Id FROM AspNetUsers WHERE OriginalUserId = @ImportModifiedByUserId)	
	
	PRINT 'Importing provider with old id ' + CAST(@ImportProviderId AS VARCHAR(10))
	
	IF(@OrganisationTypeId = 0)
	BEGIN
		SET IDENTITY_INSERT Provider ON
		INSERT INTO [dbo].[Provider]
				   ([ProviderId]
				   ,[ProviderName]
				   ,[ProviderNameAlias]
				   ,[Loans24Plus]
				   ,[Ukprn]
				   ,[ProviderTypeId]
				   ,[RecordStatusId]
				   ,[CreatedByUserId]
				   ,[CreatedDateTimeUtc]
				   ,[ModifiedByUserId]
				   ,[ModifiedDateTimeUtc]
				   ,[ProviderRegionId]
				   ,[IsContractingBody]
				   ,[ProviderTrackingUrl]
				   ,[VenueTrackingUrl]
				   ,[CourseTrackingUrl]
				   ,[BookingTrackingUrl]
				   ,[RelationshipManagerUserId]
				   ,[InformationOfficerUserId]
				   ,[AddressId]
				   ,[Email]
				   ,[Website]
				   ,[Telephone]
				   ,[Fax]
				   ,[OriginalProviderId]
				   ,[UPIN])
			VALUES
			   (
			    @ImportProviderId
			   ,@ProviderName
			   ,@ProviderAlias
			   ,@Loans24Plus
			   ,@Ukprn
			   ,@ProviderTypeId
			   ,@RecordStatusId
			   ,@CreatedByUserId
			   ,@CreatedDateTimeUtc
			   ,@ModifiedByUserId
			   ,@ModifiedDateTimeUtc
			   ,@ProviderRegionId
			   ,@IsContractingBody
			   ,@ProviderTrackingUrl
			   ,@VenueTrackingUrl
			   ,@CourseTrackingUrl
			   ,@BookingTrackingUrl
			   ,@RelationshipManagerUserId
			   ,@InformationOfficerUserId
			   ,@NewAddressId
			   ,@ProviderEmail
			   ,@Website
			   ,LEFT(@Telephone, 30)  -- Some records have long telephone numbers
			   ,@Fax
			   ,@ImportProviderId
			   ,@Upin)
		SET IDENTITY_INSERT Provider OFF
	END
	ELSE
	BEGIN
		SET IDENTITY_INSERT Organisation ON
		INSERT INTO [dbo].[Organisation]
           ([OrganisationId]
		   ,[UKPRN]
           ,[OrganisationName]
           ,[OrganisationAlias]
           ,[UPIN]
           ,[OrganisationTypeId]
           ,[Email]
           ,[Website]
           ,[Phone]
           ,[Fax]
           ,[IsContractingBody]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc]
           ,[RecordStatusId]
           ,[RelationshipManagerUserId]
           ,[InformationOfficerUserId]
           ,[AddressId]
		   ,[OriginalOrganisationId]
		   ,[Loans24Plus])
		VALUES
           (
			@ImportProviderId
		   ,@Ukprn
           ,@ProviderName
           ,@ProviderAlias
           ,@Upin
           ,(SELECT OrganisationTypeId FROM OrganisationType OT WHERE OT.OriginalOrganisationTypeID = @OrganisationTypeId)
           ,@Email
           ,@Website
           ,@PhoneNumber
		   ,@Fax
           ,@IsContractingBody
           ,@CreatedByUserId
           ,@CreatedDateTimeUtc
           ,@ModifiedByUserId
           ,@ModifiedDateTimeUtc
           ,@RecordStatusId
           ,@RelationshipManagerUserId
           ,@InformationOfficerUserId
           ,@NewAddressId
		   ,@ImportProviderId
		   ,@Loans24Plus)
		 SET IDENTITY_INSERT Organisation OFF
	END	

	-- Associate the Provider/Organisation with a user, we are excluding Admin User and Admin Superuser roles from being added
	IF(@OrganisationTypeId = 0)
	BEGIN
		INSERT INTO ProviderUser (ProviderId, UserId)
		SELECT DISTINCT ProviderId, ANU.Id
		FROM Provider P
		INNER JOIN c_provider_users CPU ON P.OriginalProviderId = CPU.PROVIDER_ID
		INNER JOIN AspNetUsers ANU ON CPU.USER_ID = ANU.OriginalUserId 
		INNER JOIN AspNetUserRoles ANUR ON ANU.Id = ANUR.UserId
		WHERE CPU.PROVIDER_ID = @ImportProviderId AND ANUR.RoleId <> 'D9B32EC6-4FC1-4685-98B5-606124924BDF' AND ANUR.RoleId <> '9E51B185-6FA5-4474-95A1-CF02DD523203'
	END
	ELSE
	BEGIN
		INSERT INTO OrganisationUser (OrganisationId, UserId)
		SELECT DISTINCT O.OrganisationId, ANU.Id
		FROM Organisation O
		INNER JOIN c_provider_users CPU ON O.OriginalOrganisationId = CPU.PROVIDER_ID
		INNER JOIN AspNetUsers ANU ON CPU.USER_ID = ANU.OriginalUserId 
		INNER JOIN AspNetUserRoles ANUR ON ANU.Id = ANUR.UserId
		WHERE CPU.PROVIDER_ID = @ImportProviderId AND ANUR.RoleId <> 'D9B32EC6-4FC1-4685-98B5-606124924BDF' AND ANUR.RoleId <> '9E51B185-6FA5-4474-95A1-CF02DD523203'
	END


SKIP_IMPORT:	
	-- Fetch the next user to import	  	
	FETCH NEXT FROM db_ProviderCursor INTO @ImportProviderId
END
CLOSE db_ProviderCursor
DEALLOCATE db_ProviderCursor


/*******************************************************************************************************************/
-- Create relationships between organisations and providers
/*******************************************************************************************************************/
DECLARE @ImportId INT
DECLARE db_ImportOrganisationProvider CURSOR FOR SELECT ImportId FROM c_org_providers ORDER BY DATE_UPDATED DESC -- Newest records type priority, any older ones that are duplicates are rejected due to PK contraints which is okay 
OPEN db_ImportOrganisationProvider 
FETCH NEXT FROM db_ImportOrganisationProvider INTO @ImportId
WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @ProviderId INT
	DECLARE @OrganisationId INT
	DECLARE @HasPermission BIT
	DECLARE @IsRejected BIT
	DECLARE @IsAccepted BIT
	DECLARE @RespondedByDateTimeUtc DATETIME
	DECLARE @Reason NVARCHAR(200)
	DECLARE @OriginalStatus NVARCHAR(100)

	SET @OriginalStatus = NULL
	SELECT @ProviderId = P.ProviderId, @OrganisationId = O.OrganisationId,
	@HasPermission = dbo.GetBitFromFlag(COP.PROV_PRIVILEGES, 1),
	@RespondedByDateTimeUtc = COP.DATE_CREATED,
	@IsRejected = CASE COP.STATUS WHEN 'REJECTED' THEN 1 WHEN 'HAS LEFT' THEN 1 ELSE 0 END,
	@IsAccepted = CASE COP.STATUS WHEN 'LIVE' THEN 1 WHEN 'HAS LEFT' THEN 1 ELSE 0 END,
	@Reason = COP.REASON,
	@OriginalStatus = COP.[STATUS]
	FROM c_org_providers COP
	INNER JOIN Provider P ON P.OriginalProviderId = COP.PROVIDER_ID
	INNER JOIN Organisation O ON O.OriginalOrganisationId = COP.ORG_ID
	WHERE COP.ImportId = @ImportId

	IF(@OriginalStatus IS NULL)
	BEGIN
		PRINT 'Import for Organisation/Provider invite link failed on import id as the provider or organisation does not exist ' + CAST(@ImportId AS VARCHAR(10))
		INSERT INTO ImportLog(DateTimeUtc, ImportLogText)
		VALUES (GETUTCDATE(), 'Import for Organisation/Provider invite link failed on import id as the provider or organisation does not exist ' + CAST(@ImportId AS VARCHAR(10)))
		GOTO SKIP_IMPORT_ORG_PROV
	END

	-- Don't import link if status is removed or withdrawn
	IF(@OriginalStatus <> 'WITHDRAWN' AND @OriginalStatus <> 'REMOVED')
	BEGIN
		IF NOT EXISTS (SELECT * FROM OrganisationProvider WHERE ProviderId = @ProviderId AND OrganisationId = @OrganisationId)
		BEGIN		
			DECLARE @OriginalOrganisation INT = (SELECT COP.ORG_ID FROM c_org_providers COP WHERE ImportId = @ImportId)
			PRINT 'Importing for original org id ' + CAST(@OriginalOrganisation AS VARCHAR(10))
			INSERT INTO [dbo].[OrganisationProvider]
				   ([OrganisationId]
				   ,[ProviderId]
				   ,[IsRejected]
				   ,[IsAccepted]
				   ,[RespondedByUserId]
				   ,[RespondedByDateTimeUtc]
				   ,[CanOrganisationEditProvider]
				   ,[Reason])
			 VALUES
				   (@OrganisationId
				   ,@ProviderId
				   ,@IsRejected
				   ,@IsAccepted
				   ,@ImportUserId
				   ,@RespondedByDateTimeUtc
				   ,@HasPermission
				   ,@Reason)
		END
		ELSE
		BEGIN
			INSERT INTO ImportLog(DateTimeUtc, ImportLogText)
			VALUES (GETUTCDATE(), 'Import for OrganisationProvider failed due to duplicate invites being found, the newest record with reject/accept status has taken priority ' +
				'New Provider Id is ' + CAST(@ProviderId AS VARCHAR(10)) + ' and new Org Id ' + CAST(@OrganisationId AS VARCHAR(10)))
		END
	END
SKIP_IMPORT_ORG_PROV:
	FETCH NEXT FROM db_ImportOrganisationProvider INTO @ImportId
END
CLOSE db_ImportOrganisationProvider
DEALLOCATE db_ImportOrganisationProvider



/*******************************************************************************************************************/
-- Add venues
/*******************************************************************************************************************/
SET IDENTITY_INSERT Venue ON
DECLARE @ImportVenueId INT
DECLARE db_ImportVenue CURSOR FOR SELECT VENUE_ID FROM c_venues
OPEN db_ImportVenue 
FETCH NEXT FROM db_ImportVenue INTO @ImportVenueId
WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @VenueName NVARCHAR(255)
	DECLARE @ProviderOwnVenueRef NVARCHAR(255)
	DECLARE @OriginalProviderId INT
	DECLARE @Facilities NVARCHAR(2000)
		
	SELECT 	
	@VenueName = CV.VENUE_NAME, 
	@ProviderOwnVenueRef = CV.PROV_VENUE_ID,
	@OriginalProviderId = CV.PROVIDER_ID,
	@RecordStatusId = dbo.GetRecordStatus(STATUS),
	@AddressId = CV.ADDRESS_ID,
	@Email = (SELECT ATTRIBUTE_VALUE FROM c_venue_attribute_values CAV WHERE CAV.VENUE_ID = @ImportVenueId AND CAV.ATTRIBUTE_ID = 1),
	@Telephone = CV.PHONE,
	@Website = (SELECT ATTRIBUTE_VALUE FROM c_venue_attribute_values CAV WHERE CAV.VENUE_ID = @ImportVenueId AND CAV.ATTRIBUTE_ID = 2),
	@Facilities = (SELECT ATTRIBUTE_VALUE FROM c_venue_attribute_values CAV WHERE CAV.VENUE_ID = @ImportVenueId AND CAV.ATTRIBUTE_ID = 4),
	@Fax = (SELECT ATTRIBUTE_VALUE FROM c_venue_attribute_values CAV WHERE CAV.VENUE_ID = @ImportVenueId AND CAV.ATTRIBUTE_ID = 3),
	@ImportCreatedByUserId = CV.CREATED_BY,
	@CreatedDateTimeUtc = CV.DATE_CREATED,
	@ImportModifiedByUserId = CV.UPDATED_BY,
	@ModifiedDateTimeUtc = CV.DATE_UPDATED	
	FROM c_venues CV
	WHERE VENUE_ID = @ImportVenueId

	SET @ProviderId = (SELECT ProviderId FROM Provider P WHERE P.OriginalProviderId = @OriginalProviderId)
	IF(@ProviderId IS NULL)
	BEGIN
		INSERT INTO ImportLog (DateTimeUtc, ImportLogText)
		VALUES(GETUTCDATE(), 'Venue with original venue Id ' + CAST(@ImportVenueId AS VARCHAR(10)) + ' for original provider Id ' + CAST(@OriginalProviderId AS VARCHAR(10)) +
			' the provider was not found in the new imported provider list. Venue will not be imported.')
		GOTO SKIP_VENUE_IMPORT
	END

	-- Get and add the address	
	IF(@AddressId IS NOT NULL)
	BEGIN		
		INSERT INTO [Address] (AddressLine1, AddressLine2, Town, County, Postcode, Latitude, Longitude)
			SELECT ISNULL(ADDRESS_1, ''), ADDRESS_2, TOWN, COUNTY, POSTCODE, null, null 
			FROM w_address WHERE ADDRESS_ID = @AddressId
		SET @NewAddressId = SCOPE_IDENTITY()
		SET @AddressId = NULL		
	END
	ELSE
	BEGIN
		SET @NewAddressId = NULL
	END

	-- Fetch the created by user id
	SET @CreatedByUserId = (SELECT Id FROM AspNetUsers WHERE OriginalUserId = @ImportCreatedByUserId)

	-- Fetch the modified by user id
	SET @ModifiedByUserId = (SELECT Id FROM AspNetUsers WHERE OriginalUserId = @ImportModifiedByUserId)		

	PRINT 'Inserting venue with original venue Id ' + CAST(@ImportVenueId AS VARCHAR(10))

INSERT INTO [Venue]
           (
		    [VenueId]
		   ,[ProviderId]
           ,[ProviderOwnVenueRef]
           ,[VenueName]
           ,[Email]
           ,[Website]
           ,[Fax]
           ,[Facilities]
           ,[RecordStatusId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc]
           ,[AddressId]
           ,[Telephone]
		   ,[OriginalVenueId]
		   )
     VALUES
           (
			@ImportVenueId
		   ,@ProviderId
           ,@ProviderOwnVenueRef
           ,@VenueName
           ,@Email
           ,@Website
           ,@Fax
           ,@Facilities
           ,@RecordStatusId
           ,@CreatedByUserId
           ,@CreatedDateTimeUtc
           ,@ModifiedByUserId
           ,@ModifiedDateTimeUtc
           ,@NewAddressId
           ,@Telephone
		   ,@ImportVenueId)


SKIP_VENUE_IMPORT:
	FETCH NEXT FROM db_ImportVenue INTO @ImportVenueId
END
CLOSE db_ImportVenue
DEALLOCATE db_ImportVenue
SET IDENTITY_INSERT Venue OFF


-- Do some tidying up, set fields to null if now ''
UPDATE Venue SET Telephone = NULL WHERE Telephone = ''


/*******************************************************************************************************************/
-- Add Course
/*******************************************************************************************************************/
SET IDENTITY_INSERT Course ON
DECLARE @ImportCourseId INT   -- Only fetch courses where we did import the provider, so check using a join
DECLARE db_ImportCourse CURSOR FOR SELECT COURSE_ID FROM o_courses C INNER JOIN Provider P ON C.PROVIDER_ID = P.OriginalProviderId
OPEN db_ImportCourse 
FETCH NEXT FROM db_ImportCourse INTO @ImportCourseId
WHILE @@FETCH_STATUS = 0
BEGIN	
	DECLARE @CourseTitle NVARCHAR(255)
	DECLARE @CourseSummary NVARCHAR(2000)
	DECLARE @AddedByApplication INT
	DECLARE @LearningAimRefId NVARCHAR(10)
	DECLARE @QualificationLevelId INT
	DECLARE @EntryRequirements NVARCHAR(4000)
	DECLARE @ProviderOwnCourseRef NVARCHAR(255)
	DECLARE @BookingUrl NVARCHAR(255)
	DECLARE @AssessmentMethod NVARCHAR(4000)
	DECLARE @EquipmentRequired NVARCHAR(4000)
	DECLARE @QualificationTitle NVARCHAR(255)
	DECLARE @QualificationTypeId INT
	DECLARE @AwardingOrganisationName NVARCHAR(150)
	DECLARE @UcasTariffPoints INT
	

	SET @CourseSummary = (SELECT ATTRIBUTE_VALUE FROM o_course_attribute_values WHERE COURSE_ID = @ImportCourseId AND ATTRIBUTE_ID = 1)
	SET @QualificationLevelId = dbo.GetQualificationLevelId((SELECT OPTION_ID FROM o_course_attribute_values WHERE COURSE_ID = @ImportCourseId AND ATTRIBUTE_ID = 23))
	SET @EntryRequirements = ISNULL((SELECT ATTRIBUTE_VALUE FROM o_course_attribute_values WHERE COURSE_ID = @ImportCourseId AND ATTRIBUTE_ID = 10), '')
	SET @Website = (SELECT ATTRIBUTE_VALUE FROM o_course_attribute_values WHERE COURSE_ID = @ImportCourseId AND ATTRIBUTE_ID = 8)
	SET @BookingUrl = (SELECT ATTRIBUTE_VALUE FROM o_course_attribute_values WHERE COURSE_ID = @ImportCourseId AND ATTRIBUTE_ID = 9)
	SET @AssessmentMethod = (SELECT ATTRIBUTE_VALUE FROM o_course_attribute_values WHERE COURSE_ID = @ImportCourseId AND ATTRIBUTE_ID = 11)
	SET @EquipmentRequired = (SELECT ATTRIBUTE_VALUE FROM o_course_attribute_values WHERE COURSE_ID = @ImportCourseId AND ATTRIBUTE_ID = 12)
	SET @QualificationTitle = (SELECT ATTRIBUTE_VALUE FROM o_course_attribute_values WHERE COURSE_ID = @ImportCourseId AND ATTRIBUTE_ID = 3)
	SET @QualificationTypeId = dbo.GetQualificationTypeId((SELECT OPTION_ID FROM o_course_attribute_values WHERE COURSE_ID = @ImportCourseId AND ATTRIBUTE_ID = 2))
	SET @AwardingOrganisationName = (SELECT ATTRIBUTE_VALUE FROM o_course_attribute_values WHERE COURSE_ID = @ImportCourseId AND ATTRIBUTE_ID = 4)
	SET @UcasTariffPoints = dbo.GetParsedUcasTariffPoints((SELECT ATTRIBUTE_VALUE FROM o_course_attribute_values WHERE COURSE_ID = @ImportCourseId AND ATTRIBUTE_ID = 22))
	SET @LearningAimRefId = NULL
	SET @LearningAimRefId = (SELECT LAD_ID FROM O_Learning_aims WHERE Course_Id = @ImportCourseId)

	-- Check we have the learning aim, if not flag a warning and import as null
	IF(@LearningAimRefId IS NOT NULL AND NOT EXISTS (SELECT * FROM LearningAim LA WHERE LA.LearningAimRefId = @LearningAimRefId))
	BEGIN
		PRINT 'Warning, LearningAimRef of ' + @LearningAimRefId + ' was not found, import for original course Id ' +
		CAST(@ImportCourseId AS VARCHAR(10)) + ' will take place but with no Learning Aim Ref assigned to it'
		INSERT INTO ImportLog(DateTimeUtc, ImportLogText)
		VALUES (GETUTCDATE(),  'Warning, LearningAimRef of ' + @LearningAimRefId + ' was not found, import for original course Id ' +
		CAST(@ImportCourseId AS VARCHAR(10)) + ' will take place but with no Learning Aim Ref assigned to it')
		SET @LearningAimRefId = null
	END
	
	SELECT 
		@ProviderId = (SELECT ProviderId FROM Provider P WHERE P.OriginalProviderId = C.PROVIDER_ID),
		@CourseTitle = PROV_COURSE_TITLE,
		@AddedByApplication = dbo.GetApplicationId(APPLICATION_ID),
		@RecordStatusId = dbo.GetRecordStatus([STATUS]),
		@ProviderOwnCourseRef = C.PROV_COURSE_ID,
		@ImportCreatedByUserId = C.CREATED_BY,
		@ImportModifiedByUserId = C.UPDATED_BY,
		@CreatedDateTimeUtc = C.DATE_CREATED,
		@ModifiedDateTimeUtc = C.DATE_UPDATED
	FROM o_courses C WHERE COURSE_ID = @ImportCourseId

	-- Fetch the created by user id
	SET @CreatedByUserId = (SELECT Id FROM AspNetUsers WHERE OriginalUserId = @ImportCreatedByUserId)

	-- Fetch the modified by user id
	SET @ModifiedByUserId = (SELECT Id FROM AspNetUsers	WHERE OriginalUserId = @ImportModifiedByUserId)	

	PRINT 'Inserting Course record with Original Course Id ' + CAST(@ImportCourseId AS VARCHAR(100))
	INSERT INTO [dbo].[Course]
			   (
			    [CourseId]
			   ,[ProviderId]
			   ,[CourseTitle]
			   ,[CourseSummary]
			   ,[CreatedByUserId]
			   ,[CreatedDateTimeUtc]
			   ,[ModifiedByUserId]
			   ,[ModifiedDateTimeUtc]
			   ,[AddedByApplicationId]
			   ,[RecordStatusId]
			   ,[LearningAimRefId]
			   ,[QualificationLevelId]
			   ,[EntryRequirements]
			   ,[ProviderOwnCourseRef]
			   ,[Url]
			   ,[BookingUrl]
			   ,[AssessmentMethod]
			   ,[EquipmentRequired]
			   ,[WhenNoLarQualificationTypeId]
			   ,[WhenNoLarQualificationTitle]
			   ,[AwardingOrganisationName]
			   ,[UcasTariffPoints]
			   ,[OriginalCourseId])
		 VALUES
			   (
			    @ImportCourseId
			   ,@ProviderId
			   ,@CourseTitle
			   ,@CourseSummary
			   ,@CreatedByUserId
			   ,@CreatedDateTimeUtc
			   ,@ModifiedByUserId
			   ,@ModifiedDateTimeUtc
			   ,@AddedByApplication
			   ,@RecordStatusId
			   ,@LearningAimRefId
			   ,@QualificationLevelId
			   ,@EntryRequirements
			   ,@ProviderOwnCourseRef
			   ,@Website
			   ,@BookingUrl
			   ,@AssessmentMethod
			   ,@EquipmentRequired
			   ,@QualificationTypeId
			   ,@QualificationTitle
			   ,@AwardingOrganisationName
			   ,@UcasTariffPoints
			   ,@ImportCourseId)

	
	DECLARE @CourseId INT = SCOPE_IDENTITY()
	
	-- Add learn direct classifications
	DECLARE @RefLevel INT = 1
	DECLARE @LearnDirectRef NVARCHAR(12)
	DECLARE @NewOrder INT = 1
	WHILE @RefLevel <= 5
	BEGIN
		SET @LearnDirectRef = (SELECT ATTRIBUTE_VALUE FROM o_course_attribute_values CAV
		WHERE CAV.COURSE_ID = @ImportCourseId AND CAV.ATTRIBUTE_ID = 13 + @RefLevel)
		IF(ISNULL(@LearnDirectRef, '') <> '')
		BEGIN
			INSERT INTO CourseLearnDirectClassification (ClassificationOrder, LearnDirectClassificationRef, CourseId)
			VALUES (@NewOrder, @LearnDirectRef, @CourseId)
			SET @NewOrder = @NewOrder + 1
		END
		SET @RefLevel = @RefLevel + 1
	END

SKIP_COURSE_IMPORT:
	FETCH NEXT FROM db_ImportCourse INTO @ImportCourseId
END
CLOSE db_ImportCourse
DEALLOCATE db_ImportCourse
SET IDENTITY_INSERT Course OFF


/***********************************************************************************************************/
-- Import course instance (aka Opportunity)
/***********************************************************************************************************/
SET IDENTITY_INSERT CourseInstance ON
DECLARE @ImportCourseInstanceId INT   -- Only fetch course instances that we did import a course for by using join
DECLARE db_ImportCourseInstance CURSOR FOR SELECT OPPORTUNITY_ID FROM o_opportunities O INNER JOIN Course C ON O.COURSE_ID = C.OriginalCourseId
OPEN db_ImportCourseInstance
FETCH NEXT FROM db_ImportCourseInstance INTO @ImportCourseInstanceId
WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @OriginalCourseId INT
	DECLARE @OfferedByProviderId INT
	DECLARE @OriginalOfferedByProviderId INT
	DECLARE @OriginalDisplayByProviderId INT
	DECLARE @DisplayByProviderId INT
	DECLARE @StudyModeId INT
	DECLARE @AttendanceTypeId INT
	DECLARE @AttendancePatternId INT
	DECLARE @DurationUnit INT
	DECLARE @DurationUnitId INT
	DECLARE @DurationAsText NVARCHAR(150)
	DECLARE @StartDateDesc NVARCHAR(150)	
	DECLARE @EndDate DATE
	DECLARE @TimeTable NVARCHAR(200)
	DECLARE @Price DECIMAL(10,2)
	DECLARE @PriceAsText NVARCHAR(150)
	DECLARE @LanguageOfInstruction NVARCHAR(100)
	DECLARE @LanguageOfAssessment NVARCHAR(100)
	DECLARE @ApplyFromDate DATE
	DECLARE @ApplyUntilDate DATE
	DECLARE @ApplyUntilText NVARCHAR(100)
	DECLARE @EnquiryTo NVARCHAR(255)
	DECLARE @ApplyTo NVARCHAR(255)
	DECLARE @CanApplyAllYear BIT
	DECLARE @PlacesAvailable INT
	DECLARE @BothOfferedBy BIT
	DECLARE @DisplayedByOrganisationId INT
	DECLARE @OfferedByOrganisationId INT
		
	-- Get offered by provider Id
	
	PRINT 'Inserting new CourseInstance for original id ' + CAST(@ImportCourseInstanceId AS VARCHAR(10))
	
	SET @OriginalDisplayByProviderId = (SELECT OPTION_ID FROM o_opp_attribute_values OAV WHERE OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 32)
	SET @OriginalOfferedByProviderId = (SELECT OPTION_ID FROM o_opp_attribute_values OAV WHERE OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 31)
	SET @StudyModeId = (SELECT OPTION_ID FROM o_opp_attribute_values OAV WHERE OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 1) -- One to one mapping with new lookup
	SET @AttendanceTypeId = dbo.GetAttendanceTypeId((SELECT OPTION_ID FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 2))
	SET @AttendancePatternId = dbo.GetAttendancePatternId((SELECT OPTION_ID FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 3))
	SET @DurationUnit = dbo.GetParsedInt((SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 23))
	SET @DurationUnitId = dbo.GetDurationUnitId((SELECT OPTION_ID FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 19))
	SET @DurationAsText = (SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 18)
	SET @EndDate = dbo.GetDateOrNull((SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 5))
	SET @TimeTable = (SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 7)
	SET @Price = dbo.GetParsedPrice((SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 8))
	SET @PriceAsText = (SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 9)
	SET @LanguageOfInstruction = (SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 10)
	SET @LanguageOfAssessment = (SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 11)
	SET @ApplyFromDate = dbo.GetDateOrNull((SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 13))
	SET @ApplyUntilDate = dbo.GetDateOrNull((SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 14))
	SET @ApplyUntilText = (SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 29)
	SET @EnquiryTo = (SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 15)
	SET @ApplyTo = (SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 16)
	SET @Website = (SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 17)
	SET @CanApplyAllYear = dbo.GetBitFromFlag((SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 28), 1)
	SET @PlacesAvailable = (SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 12)
	SET @BothOfferedBy = dbo.GetBitFromFlag((SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 33), 1)
	SET @StartDateDesc = (SELECT ATTRIBUTE_VALUE FROM o_opp_attribute_values OAV WHERE OAV.OPPORTUNITY_ID = @ImportCourseInstanceId AND ATTRIBUTE_ID = 6)

	SET @DisplayByProviderId = NULL
	SET @DisplayedByOrganisationId = NULL
	SET @OfferedByOrganisationId = NULL
	SET @OfferedByProviderId = NULL

	-- Do we need to fetch a display by Id to save to this course
	IF(@OriginalDisplayByProviderId IS NOT NULL)  -- We have a displayed by provider
	BEGIN
		-- Try and fetch the display provider id by doing a lookup, it ensures we have the provider record imported and we have the new Id set
		SET @DisplayByProviderId = (SELECT ProviderId FROM Provider P WHERE P.OriginalProviderId = @OriginalDisplayByProviderId)
		IF(@DisplayByProviderId IS NULL)
		BEGIN
			-- Is this a display by organisation perhaps, hence not finding the provider
			SET @DisplayedByOrganisationId = (SELECT OrganisationId FROM Organisation O WHERE O.OriginalOrganisationId = @OriginalDisplayByProviderId)
			IF(@DisplayedByOrganisationId IS NULL)
			BEGIN
				PRINT 'Warning, could not find the display provider so leaving as null for original Course instance Id ' + CAST(@ImportCourseInstanceId AS VARCHAR(10))
				INSERT INTO ImportLog (DateTimeUtc, ImportLogText)
				VALUES(GETUTCDATE(), 'Warning, could not find the display provider so leaving as null for original Course instance Id ' + CAST(@ImportCourseInstanceId AS VARCHAR(10)))				
			END
		END	
	END

	-- Do we need to fetch a offered by Ids to save to this course
	IF(@OriginalOfferedByProviderId IS NOT NULL)  -- We have an offered by provider
	BEGIN
		-- Try and fetch the offered provider id by doing a lookup, it ensures we have the provider record imported and we have the new Id set
		SET @OfferedByProviderId = (SELECT ProviderId FROM Provider P WHERE P.OriginalProviderId = @OriginalOfferedByProviderId)
		IF(@OfferedByProviderId IS NULL)
		BEGIN
			-- Is this a offered by organisation perhaps, hence not finding the provider
			SET @OfferedByOrganisationId = (SELECT OrganisationId FROM Organisation O WHERE O.OriginalOrganisationId = @OriginalOfferedByProviderId)
			IF(@OfferedByOrganisationId IS NULL)
			BEGIN
				PRINT 'Warning, could not find the offered provider so leaving as null for original Course instance Id ' + CAST(@ImportCourseInstanceId AS VARCHAR(10))
				INSERT INTO ImportLog (DateTimeUtc, ImportLogText)
				VALUES(GETUTCDATE(), 'Warning, could not find the offered provider so leaving as null for original Course instance Id ' + CAST(@ImportCourseInstanceId AS VARCHAR(10)))				
			END
		END	
	END

	SELECT
		@OriginalCourseId = O.COURSE_ID,
		@RecordStatusId = dbo.GetRecordStatus(O.[STATUS]),
		@ProviderOwnCourseRef = O.PROV_OPPORTUNITY_ID,
		@AddedByApplication = dbo.GetApplicationId(O.APPLICATION_ID),
		@CreatedByUserId = O.CREATED_BY,
		@CreatedDateTimeUtc = O.DATE_CREATED,
		@ModifiedByUserId = O.UPDATED_BY,
		@ModifiedDateTimeUtc = O.DATE_UPDATED
	FROM o_opportunities O WHERE O.OPPORTUNITY_ID = @ImportCourseInstanceId

	-- Get the new course Id
	SET @CourseId = (SELECT CourseId FROM Course C WHERE C.OriginalCourseId = @OriginalCourseId)

	INSERT INTO [dbo].[CourseInstance]
			   (
			    [CourseInstanceId]
			   ,[CourseId]
			   ,[RecordStatusId]
			   ,[ProviderOwnCourseInstanceRef]
			   ,[OfferedByProviderId]
			   ,[DisplayProviderId]
			   ,[StudyModeId]
			   ,[AttendanceTypeId]
			   ,[AttendancePatternId]
			   ,[DurationUnit]
			   ,[DurationUnitId]
			   ,[DurationAsText]
			   ,[StartDateDescription]
			   ,[EndDate]
			   ,[TimeTable]
			   ,[Price]
			   ,[PriceAsText]
			   ,[AddedByApplicationId]
			   ,[LanguageOfInstruction]
			   ,[LanguageOfAssessment]
			   ,[ApplyFromDate]
			   ,[ApplyUntilDate]
			   ,[ApplyUntilText]
			   ,[EnquiryTo]
			   ,[ApplyTo]
			   ,[Url]
			   ,[CanApplyAllYear]
			   ,[CreatedByUserId]
			   ,[CreatedDateTimeUtc]
			   ,[ModifiedByUserId]
			   ,[ModifiedDateTimeUtc]
			   ,[PlacesAvailable]
			   ,[BothOfferedByDisplayBySearched]
			   ,[VenueLocationId]
			   ,[OfferedByOrganisationId]
			   ,[DisplayedByOrganisationId]
			   ,[OriginalOpportunityId])
		 VALUES
			   (@ImportCourseInstanceId
			   ,@CourseId
			   ,@RecordStatusId
			   ,@ProviderOwnCourseRef
			   ,@OfferedByProviderId 
			   ,@DisplayByProviderId   
			   ,@StudyModeId
			   ,@AttendanceTypeId
			   ,@AttendancePatternId
			   ,@DurationUnit
			   ,@DurationUnitId
			   ,@DurationAsText
			   ,@StartDateDesc
			   ,@EndDate
			   ,@TimeTable
			   ,@Price
			   ,@PriceAsText
			   ,@AddedByApplication
			   ,@LanguageOfInstruction
			   ,@LanguageOfAssessment
			   ,@ApplyFromDate
			   ,@ApplyUntilDate
			   ,@ApplyUntilText
			   ,@EnquiryTo
			   ,@ApplyTo
			   ,@Website
			   ,@CanApplyAllYear
			   ,@CreatedByUserId
			   ,@CreatedDateTimeUtc
			   ,@ModifiedByUserId
			   ,@ModifiedDateTimeUtc
			   ,@PlacesAvailable
			   ,@BothOfferedBy
			   ,null  -- Course locations added later
			   ,@OfferedByOrganisationId
			   ,@DisplayedByOrganisationId
			   ,@ImportCourseInstanceId)

SKIP_COURSEINSTANCE_IMPORT:
	FETCH NEXT FROM db_ImportCourseInstance INTO @ImportCourseInstanceId
END
CLOSE db_ImportCourseInstance
DEALLOCATE db_ImportCourseInstance
SET IDENTITY_INSERT CourseInstance OFF


/***********************************************************************************************************/
-- Import course dates
/***********************************************************************************************************/
DECLARE @ImportCourseDateId INT   -- Join against Course Instance so we only import to Course instances that have been imported okay
DECLARE db_ImportCourseDate CURSOR FOR SELECT ImportId FROM o_opp_start_dates OD INNER JOIN CourseInstance CI ON OD.OPPORTUNITY_ID = CI.OriginalOpportunityId WHERE DATEPART(YEAR, OD.START_DATE) > 2000
OPEN db_ImportCourseDate
FETCH NEXT FROM db_ImportCourseDate INTO @ImportCourseDateId
WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @StartDate DATE 
	DECLARE @IsMonthOnlyStartDate BIT
	DECLARE @OriginalCourseInstanceId INT
	DECLARE @CourseInstanceId INT

	SELECT 
		@StartDate = OD.[START_DATE],	-- Implicit cast okay as this field is a real date field	
		@IsMonthOnlyStartDate = CASE WHEN OD.DATE_FORMAT = 'Mon-RRRR' THEN 1 ELSE 0 END,
		@OriginalCourseInstanceId = OD.OPPORTUNITY_ID,
		@PlacesAvailable = OD.PLACES_AVAILABLE
	FROM o_opp_start_dates OD 
	WHERE OD.ImportId = @ImportCourseDateId
		
	IF(@StartDate IS NULL)
	BEGIN
		PRINT 'Warning: Start date could not be converted to a date for Original Course instance Id ' + CAST(@OriginalCourseInstanceId AS VARCHAR(10))
		INSERT INTO ImportLog (DateTimeUtc, ImportLogText)
		VALUES (GETUTCDATE(), 'Warning: Start date could not be converted to date for Original Course instance Id ' + CAST(@OriginalCourseInstanceId AS VARCHAR(10)))
		GOTO SKIP_IMPORT_COURSEDATE
	END
	
	-- Get new course instance Id
	SET @CourseInstanceId = (SELECT CI.CourseInstanceId FROM CourseInstance CI WHERE CI.OriginalOpportunityId = @OriginalCourseInstanceId)
		
	-- Insert the course date
	PRINT 'Inserting into CourseInstanceStartDate for original course instance Id ' + CAST(@OriginalCourseInstanceId AS VARCHAR(10))
	INSERT INTO CourseInstanceStartDate (CourseInstanceId, StartDate, IsMonthOnlyStartDate)
	VALUES (@CourseInstanceId, @StartDate, @IsMonthOnlyStartDate)


SKIP_IMPORT_COURSEDATE:
	FETCH NEXT FROM db_ImportCourseDate INTO @ImportCourseDateId
END
CLOSE db_ImportCourseDate
DEALLOCATE db_ImportCourseDate


/***********************************************************************************************************/
-- Import A10 Codes
/***********************************************************************************************************/
DECLARE @ImportAttributeValueId INT   -- Only import where we have the course instant by using a join
DECLARE db_ImportA10Code CURSOR FOR SELECT Attribute_Value_Id FROM o_opp_attribute_values OAV INNER JOIN CourseInstance CI ON OAV.OPPORTUNITY_ID = CI.OriginalOpportunityId AND ATTRIBUTE_ID = 24 ORDER BY OAV.OPPORTUNITY_ID
OPEN db_ImportA10Code
FETCH NEXT FROM db_ImportA10Code INTO @ImportAttributeValueId
WHILE @@FETCH_STATUS = 0
BEGIN
	
	DECLARE @A10FundingCode INT
	DECLARE @OptionValue INT
	SELECT 
		@A10FundingCode = dbo.GetA10Code(OAV.OPTION_ID),
		@OptionValue = OAV.OPTION_ID,
		@CourseInstanceId = (SELECT CourseInstanceId FROM CourseInstance CI WHERE CI.OriginalOpportunityId = OAV.OPPORTUNITY_ID)
	FROM o_opp_attribute_values OAV
	WHERE OAV.ATTRIBUTE_VALUE_ID = @ImportAttributeValueId
	
	IF(@A10FundingCode IS NULL AND @OptionValue <> 66)
	BEGIN
		PRINT 'Warning: Funding code was not found for new Course instance Id ' + CAST(@CourseInstanceId AS VARCHAR(10))
		INSERT INTO ImportLog (DateTimeUtc, ImportLogText)
		VALUES (GETUTCDATE(), 'Warning: Funding code was not found for new Course instance Id ' + CAST(@CourseInstanceId AS VARCHAR(10)))
		GOTO SKIP_IMPORT_A10
	END

	IF(@A10FundingCode IS NOT NULL)
	BEGIN
		PRINT 'Inserting A10 funding code for new Course instance Id ' + CAST(@CourseInstanceId AS VARCHAR(10))
		INSERT INTO CourseInstanceA10FundingCode(CourseInstanceId, A10FundingCode)
		VALUES(@CourseInstanceId, @A10FundingCode)
	END
	ELSE PRINT 'A10 funding code is N/A no record needs inserting ' + CAST(@CourseInstanceId AS VARCHAR(10))

SKIP_IMPORT_A10:
	FETCH NEXT FROM db_ImportA10Code INTO @ImportAttributeValueId
END
CLOSE db_ImportA10Code
DEALLOCATE db_ImportA10Code

/***********************************************************************************************************/
-- Import Course instance to venue or venue location
/***********************************************************************************************************/
DECLARE @ImportOpportunityId INT -- Only import where we have the course instant by using a join
DECLARE db_ImportCourseVenue CURSOR FOR SELECT OPPORTUNITY_ID FROM o_opp_locations OL INNER JOIN CourseInstance CI ON OL.OPPORTUNITY_ID = CI.OriginalOpportunityId
OPEN db_ImportCourseVenue
FETCH NEXT FROM db_ImportCourseVenue INTO @ImportOpportunityId
WHILE @@FETCH_STATUS = 0
BEGIN
	-- Get the new course instance id, this should be the same as the original but check anyway
	SET @CourseInstanceId = (SELECT CourseInstanceId FROM CourseInstance CI WHERE CI.OriginalOpportunityId = @ImportOpportunityId)

	DECLARE @IsRegionLocation INT
	DECLARE @OriginalLocationId INT
	DECLARE @VenueId INT
	SELECT 
		@IsRegionLocation = CASE WHEN LOCATION_TYPE_ID = 2 THEN 1 ELSE 0 END,
		@OriginalLocationId = LOCATION_ID		
	FROM o_opp_locations OL WHERE OL.OPPORTUNITY_ID = @ImportOpportunityId

	-- Is this a Venue or venue location
	IF(@IsRegionLocation = 1)
	BEGIN
		PRINT 'Inserting venue location for new course instance id ' + CAST(@CourseInstanceId AS VARCHAR(10))
		-- One to one mapping
		UPDATE CourseInstance SET VenueLocationId = @OriginalLocationId WHERE CourseInstanceId = @CourseInstanceId
	END
	ELSE
	BEGIN
		-- Lookup the venue 
		SET @VenueId = (SELECT VenueId FROM Venue V WHERE V.OriginalVenueId = @OriginalLocationId)
		IF(@VenueId IS NULL)
		BEGIN
			PRINT 'Warning: Venue with original venue Id ' + CAST(@OriginalLocationId AS VARCHAR(10)) + ' was not found in the imported venue table, can''t venue to course instance'
			INSERT INTO ImportLog (DateTimeUtc, ImportLogText)
			VALUES (GETUTCDATE(), 'Venue with original venue Id ' + CAST(@OriginalLocationId AS VARCHAR(10)) + ' was not found in the imported venue table')
		END
		ELSE
		BEGIN
			-- Insert the venue
			PRINT 'Inserting venue id for new course instance id ' + CAST(@CourseInstanceId AS VARCHAR(10))
			INSERT INTO CourseInstanceVenue(CourseInstanceId, VenueId)
			VALUES(@CourseInstanceId, @VenueId)
		END
	END
	
SKIP_IMPORT_COURSEVENUE:
	FETCH NEXT FROM db_ImportCourseVenue INTO @ImportOpportunityId
END
CLOSE db_ImportCourseVenue
DEALLOCATE db_ImportCourseVenue

/***********************************************************************************************************/
-- Do lat long lookups for addresses
/***********************************************************************************************************/
DECLARE @ImportAddressId INT 
DECLARE db_ImportAddressLatLong CURSOR FOR SELECT AddressId FROM Address
OPEN db_ImportAddressLatLong
FETCH NEXT FROM db_ImportAddressLatLong INTO @ImportAddressId
WHILE @@FETCH_STATUS = 0
BEGIN
	
	DECLARE @Lat FLOAT
	DECLARE @Long FLOAT
	DECLARE @LookupPostcode NVARCHAR(30)
	SET @LookupPostcode = (SELECT Postcode FROM Address WHERE AddressId = @ImportAddressId)
	
	-- Try and fetch lat/long
	SET @Lat = NULL
	SELECT @Lat = Lat, @Long = Lng FROM GeoLocation WHERE Postcode = @LookupPostcode

	IF(@Lat IS NULL)
	BEGIN
		PRINT 'Could not lookup new AddressId ' + CAST(@ImportAddressId AS VARCHAR(10)) + ' with postcode ' + @LookupPostcode
		INSERT INTO ImportLog(DateTimeUtc, ImportLogText)
		VALUES (GETUTCDATE(), 'Could not lookup new AddressId ' + CAST(@ImportAddressId AS VARCHAR(10)) + ' with postcode ' + @LookupPostcode)
	END
	ELSE
	BEGIN
		PRINT 'Updating lat/long to new address Id ' + CAST(@ImportAddressId AS VARCHAR(10))
		UPDATE [Address] SET Latitude = @Lat, Longitude = @Long WHERE AddressId = @ImportAddressId
	END

	FETCH NEXT FROM db_ImportAddressLatLong INTO @ImportAddressId
END
CLOSE db_ImportAddressLatLong
DEALLOCATE db_ImportAddressLatLong

SET IDENTITY_INSERT [Address] OFF

SELECT * FROM ImportLog

IF(@TestImport = 1)
BEGIN
	ROLLBACK TRANSACTION
	PRINT 'Import is in test mode and the transaction has been rolled back'
END
ELSE
BEGIN
	COMMIT TRANSACTION
	PRINT 'Import complete, now run tidy up script to remove temporary indexes, tables and modifications that were made to support the import.'
END


