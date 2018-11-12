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

/* Each refactor should use a unique GUID */
IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '0E5037C0-354E-4A0C-B0E8-8B3EE76401A7')
BEGIN
	ALTER FULLTEXT INDEX ON [search].[Course] SET STOPLIST = OFF;
	INSERT INTO __RefactorLog (OperationKey) VALUES ('0E5037C0-354E-4A0C-B0E8-8B3EE76401A7');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '1530095D-E038-42F2-89BC-85EBBC796609')
BEGIN
	CREATE FULLTEXT STOPLIST CourseSearch FROM SYSTEM STOPLIST; 
	INSERT INTO __RefactorLog (OperationKey) VALUES ('1530095D-E038-42F2-89BC-85EBBC796609');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '576573AB-A55E-4CBC-8CF6-3C570BBD5B95')
BEGIN
	ALTER FULLTEXT INDEX ON [dbo].[CourseFreeText] SET STOPLIST CourseSearch;
	INSERT INTO __RefactorLog (OperationKey) VALUES ('576573AB-A55E-4CBC-8CF6-3C570BBD5B95');
END;
GO
