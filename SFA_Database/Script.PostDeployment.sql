/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
:r .\Data\LookupData.sql

/* Sample Ukrlp data for dev */
:r .\Data\SampleUkrlpData.sql

/* Add sample LearnDirectClassification */
:r .\Data\SampleLearnDirectClassificationData.sql

/* Add sample venue location data */
:r .\Data\SampleVenueLocationData.sql

/* Add default roles and permissions */
:r .\Data\DefaultRolePermissions.sql

/* Add default configuration settings */
:r .\Data\DefaultConfigurationSettings.sql

/* Add default site content */
:r .\Data\DefaultContent.sql

/* Add default email templates and groups */
:r .\Data\DefaultEmailTemplates.sql

/* Add default users for initial access */
:r .\Data\DefaultUsers.sql

/* Add the legacy course lookups */
:r .\Data\LegacyCourseCategory.sql

/* Add the Qualification Type map lookup from LAD */
:r .\Data\QualificationTypeMap.sql

/* Run any other manually created refactor scripts */
:r .\PostDeployment\Custom.sql