-- Clear functions and indexes added for the import
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'up_GetDateFromString')
DROP PROCEDURE up_GetDateFromString
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetParsedUcasTariffPoints]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetParsedUcasTariffPoints]
END
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetParsedPrice]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetParsedPrice]
END
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetParsedInt]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetParsedInt]
END
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetA10Code]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetA10Code]
END
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetQualificationLevelId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetQualificationTypeId]
END
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAttendanceTypeId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetAttendanceTypeId]
END
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAttendancePatternId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetAttendancePatternId]
END
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDurationUnitId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetDurationUnitId]
END
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBitFromFlag]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetBitFromFlag]
END
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDateOrNull]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetDateOrNull]
END
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRecordStatus]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetRecordStatus]
END
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetQualificationLevelId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetQualificationLevelId]
END
GO
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplicationId]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GetApplicationId]
END
GO


-- Drop all the indexes that were created to speed up the import
IF EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_learning_aims' AND object_id = OBJECT_ID('o_learning_aims'))
BEGIN		
	ALTER TABLE [dbo].[o_learning_aims] DROP CONSTRAINT [PK_o_learning_aims] 
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='IX_o_opp_attribute_values' AND object_id = OBJECT_ID('o_opp_attribute_values'))
BEGIN	
	DROP INDEX [IX_o_opp_attribute_values] ON [dbo].[o_opp_attribute_values]
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_opp_attribute_values' AND object_id = OBJECT_ID('o_opp_attribute_values'))
BEGIN
	ALTER TABLE [dbo].[o_opp_attribute_values] DROP CONSTRAINT [PK_o_opp_attribute_values] 
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_opp_start_dates' AND object_id = OBJECT_ID('[o_opp_start_dates]'))
BEGIN	
	ALTER TABLE [dbo].[o_opp_start_dates] DROP CONSTRAINT [PK_o_opp_start_dates] 
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_course_attribute_values' AND object_id = OBJECT_ID('o_course_attribute_values'))
BEGIN	
	ALTER TABLE [dbo].[o_course_attribute_values] DROP CONSTRAINT [PK_o_course_attribute_values] 
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='PK_s_users' AND object_id = OBJECT_ID('s_users'))
BEGIN	
	ALTER TABLE [dbo].[s_users] DROP CONSTRAINT [PK_s_users]
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_opportunity' AND object_id = OBJECT_ID('o_opportunities'))
BEGIN	
	ALTER TABLE [dbo].[o_opportunities] DROP CONSTRAINT [PK_o_opportunity] 
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='IX_c_provider_attribute_values' AND object_id = OBJECT_ID('c_provider_attribute_values'))
BEGIN	
	DROP INDEX [IX_c_provider_attribute_values] ON [dbo].[c_provider_attribute_values]			
END
GO

IF  EXISTS(SELECT * FROM sys.indexes WHERE name='IX_AspNetUsers_OriginalUserId' AND object_id = OBJECT_ID('AspNetUsers'))
BEGIN	
	DROP INDEX [IX_AspNetUsers_OriginalUserId] ON [dbo].[AspNetUsers]		
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='PK_c_providers' AND object_id = OBJECT_ID('c_providers'))
BEGIN	
	ALTER TABLE [dbo].[c_providers] DROP CONSTRAINT [PK_c_providers] 
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='IX_Course_OriginalCourseId' AND object_id = OBJECT_ID('Course'))
BEGIN	
	DROP INDEX [IX_Course_OriginalCourseId] ON [dbo].[Course]	
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='IX_CourseInstance_OriginalOpportunityId' AND object_id = OBJECT_ID('CourseInstance'))
BEGIN
	DROP INDEX [IX_CourseInstance_OriginalOpportunityId] ON [dbo].[CourseInstance]	
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='IX_Venue_OriginalVenue' AND object_id = OBJECT_ID('Venue'))
BEGIN	
	DROP INDEX [IX_Venue_OriginalVenue] ON [dbo].[Venue]	
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='IX_Provider_OriginalProviderId' AND object_id = OBJECT_ID('Provider'))
BEGIN	
	DROP INDEX [IX_Provider_OriginalProviderId] ON [dbo].[Provider]	
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='PK_w_address' AND object_id = OBJECT_ID('w_address'))
BEGIN
	ALTER TABLE [dbo].[w_address] DROP CONSTRAINT [PK_w_address] 
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_opp_locations' AND object_id = OBJECT_ID('o_opp_locations'))
BEGIN
	ALTER TABLE [dbo].[o_opp_locations] DROP CONSTRAINT [PK_o_opp_locations] 
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_learning_aims' AND object_id = OBJECT_ID('o_learning_aims'))
BEGIN	
	ALTER TABLE [dbo].[o_learning_aims] DROP CONSTRAINT [PK_o_learning_aims]
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE name='PK_o_courses' AND object_id = OBJECT_ID('o_courses'))
BEGIN
	ALTER TABLE [dbo].[o_courses] DROP CONSTRAINT [PK_o_courses] 
END
GO


IF EXISTS(SELECT * FROM sys.indexes WHERE name='PK_c_venue_attribute_values' AND object_id = OBJECT_ID('c_venue_attribute_values'))
BEGIN	
	ALTER TABLE [dbo].[c_venue_attribute_values] DROP CONSTRAINT [PK_c_venue_attribute_values] 	
END
GO

-- Remove temporary columns
IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'OriginalUserId' AND object_id = Object_ID(N'AspNetUsers'))
BEGIN
	ALTER TABLE AspNetUsers DROP COLUMN OriginalUserId  
	ALTER TABLE AspNetUsers DROP COLUMN OriginalAddressId
	ALTER TABLE AspNetUsers DROP COLUMN OriginalModifiedByUserId 
	ALTER TABLE AspNetUsers DROP COLUMN OriginalCreatedByUserId 
	ALTER TABLE Provider DROP COLUMN OriginalProviderId 
	ALTER TABLE Organisation DROP COLUMN OriginalOrganisationId 
	ALTER TABLE OrganisationType DROP COLUMN OriginalOrganisationTypeID 
	ALTER TABLE Course DROP COLUMN OriginalCourseId 
	ALTER TABLE CourseInstance DROP COLUMN OriginalOpportunityId 
	ALTER TABLE Venue DROP COLUMN OriginalVenueId 
END
GO

-- Drop all the import tables
DROP TABLE c_org_providers
DROP TABLE c_provider_attribute_values
DROP TABLE c_provider_tracking_codes
DROP TABLE c_provider_users
DROP TABLE c_providers
DROP TABLE c_venue_attribute_values
DROP TABLE c_venues
DROP TABLE ld_awarding_organisations
DROP TABLE ld_lars_lrnaimreftype
DROP TABLE ld_learning_aim
DROP TABLE ld_validity_details
DROP TABLE o_course_attribute_values
DROP TABLE o_courses
DROP TABLE o_learning_aims
DROP TABLE o_opp_attribute_values
DROP TABLE o_opp_locations
DROP TABLE o_opp_start_dates
DROP TABLE o_opportunities
DROP TABLE s_regions
DROP TABLE u_users
DROP TABLE w_address
DROP TABLE w_provider_address
DROP TABLE s_lad_qt_map
DROP TABLE s_postcode_locations
DROP TABLE s_learning_aims