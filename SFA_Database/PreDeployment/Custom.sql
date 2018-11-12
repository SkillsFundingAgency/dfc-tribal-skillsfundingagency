/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------

All custom refactor scripts should run only once, the best way to do this is to use
one of the following templates. You should replace the X's with a GUID, print a sensible
description and add your SQL where indicated.

The Pre-Deployment script should be used sparingly, most custom refactors belong in the
Post-Deployment section.

1. General changes not related to the language text fields

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX')
BEGIN
	PRINT '[Updating Widget Catalogue Information]'
	SET NOCOUNT ON

	-- SQL Code goes here

	INSERT INTO __RefactorLog (OperationKey) VALUES ('XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX')
END
GO

2. Changes to language text fields

These go in the Post-Deployment Custom.sql file

*/

-- If the [dbo].[Apprenticeship] table doesn't exist yet (e.g. this is a new database).  We never need to run this
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Apprenticeship]') AND type in (N'U'))
BEGIN
	IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'CDC741F4-45C8-4E01-B97B-EE9AFAC3681E')
	BEGIN
		PRINT '[Removing Tooltop for Provider Marketing Information Field]';
		SET NOCOUNT ON;

		UPDATE dbo.Apprenticeship SET MarketingInformation = LEFT(MarketingInformation, 900) WHERE LEN(MarketingInformation) > 900;

		INSERT INTO __RefactorLog (OperationKey) VALUES ('CDC741F4-45C8-4E01-B97B-EE9AFAC3681E');
	END;
ELSE
	IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'CDC741F4-45C8-4E01-B97B-EE9AFAC3681E')
	BEGIN
		INSERT INTO __RefactorLog (OperationKey) VALUES ('CDC741F4-45C8-4E01-B97B-EE9AFAC3681E');
	END;
END;
GO

-- If the [Search].[DAS_Apprenticeship] table doesn't exist yet (e.g. this is a new database).  We never need to run this
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Search].[DAS_Apprenticeship]') AND type in (N'U'))
BEGIN
	IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'E5CD74BD-6CAC-4E1D-81ED-CEF6BC5DCDC4')
	BEGIN
		PRINT '[Truncating Apprentiecship Search Marketing Information Field]';
		SET NOCOUNT ON;

		UPDATE Search.DAS_Apprenticeship SET MarketingInformation = LEFT(MarketingInformation, 900) WHERE LEN(MarketingInformation) > 900;

		INSERT INTO __RefactorLog (OperationKey) VALUES ('E5CD74BD-6CAC-4E1D-81ED-CEF6BC5DCDC4');
	END;
ELSE
	IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'E5CD74BD-6CAC-4E1D-81ED-CEF6BC5DCDC4')
	BEGIN
		INSERT INTO __RefactorLog (OperationKey) VALUES ('E5CD74BD-6CAC-4E1D-81ED-CEF6BC5DCDC4');
	END;
END;
GO
