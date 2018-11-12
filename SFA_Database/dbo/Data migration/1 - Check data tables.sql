-- Checks all the necessary tables required from the Oracle Db have been imported
-- Check we have the tables
DECLARE @HaveAllTables BIT = 1
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'u_Users')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called u_Users, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'w_Address')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called w_Address, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'c_providers')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called c_providers, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'c_provider_tracking_codes')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called c_provider_tracking_codes, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'c_provider_users')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called c_provider_users, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 's_regions')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called s_regions, import can not start'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'w_provider_address')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called w_provider_address, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'c_provider_attribute_values')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called c_provider_attribute_values, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'c_org_providers')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called c_org_providers, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'c_venues')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called c_venues, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'c_venue_attribute_values')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called c_venue_attribute_values, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'o_courses')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called o_courses, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'o_course_attribute_values')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called o_course_attribute_values, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'o_learning_aims')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called o_learning_aims, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ld_awarding_organisations')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called ld_awarding_organisations, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ld_learning_aim')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called ld_learning_aim, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ld_lars_lrnaimreftype')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called ld_lars_lrnaimreftype, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'o_opportunities')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called o_opportunities, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'o_opp_attribute_values')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called o_opp_attribute_values, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'o_opp_start_dates')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called o_opp_start_dates, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'o_opp_locations')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called o_opp_locations, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 's_lad_qt_map')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called s_lad_qt_map, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 's_postcode_locations')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called s_postcode_locations, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ld_validity_details')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called ld_validity_details, import can not start'
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'S_LEARNING_AIMS')
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'Missing the import table called S_LEARNING_AIMS, import can not start'
END

-- Get required fixed data values
DECLARE @DefaultRoleId VARCHAR(128) = (SELECT Id FROM AspNetRoles ANR WHERE ANR.Name = 'Provider user')
IF(@@ROWCOUNT = 0)
BEGIN
	SET @HaveAllTables = 0
	PRINT 'Could not retrieve role id for role with name Provider user, import can not start'
END

-- Get if we have GeoLocation data imported, should have well over a million and half records
IF ((SELECT COUNT(-1) FROM GeoLocation) < 1500000)
BEGIN
    SET @HaveAllTables = 0 
	PRINT 'The GeoLocation data has not been loaded or is incomplete in the database, the import requires this data'
END

PRINT '************************************************************************'

IF (@HaveAllTables = 0)
BEGIN
	PRINT 'One or more required tables are missing, the import should not be started until all required tables have been imported'
END
ELSE
BEGIN
	PRINT 'All required tables were found'
END

-- Check if this database appears to have records already
IF EXISTS(SELECT * FROM Course)
BEGIN
	PRINT '---------------------------------------------------------------------------------'
	PRINT 'WARNING: This database isn''t empty and contains course data already, data migration should only be carried out to an empty database'
	PRINT '---------------------------------------------------------------------------------'
	
END