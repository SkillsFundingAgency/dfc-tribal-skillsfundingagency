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

All custom refactor scripts should run only once, the best way to do this is to use
one of the following templates. You should replace the X's with a GUID, print a sensible
description and add your SQL where indicated.

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

To get the fully qualified field names:
	a. Go to the Portal Admin > Languages and download the current language or,
	b. Run the following SQL: EXEC [dbo].[up_LanguageTextListByLanguageId] 1, null
	                      or: EXEC [dbo].[up_LanguageTextListByKeyGroupId] 1, null, null, null, 1

Ensure you make corresponding changes to the call to AppGlobal.Language.GetText().

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX')
BEGIN
	PRINT '[Updating Widget Catalogue Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'The new text for the field'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Fully_Qualified_FieldName1', @NewText, @NewText

	SET @NewText = 'The new text for the field'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Fully_Qualified_FieldName2', @NewText, @NewText

	-- etc

	INSERT INTO __RefactorLog (OperationKey) VALUES ('XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX')
END
GO
*/

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '4B3D9BE2-8E5C-422F-91ED-E1BF850DDB61')
BEGIN
	PRINT '[Updating Address GeoLocation Information]';
	SET NOCOUNT ON;

	UPDATE Address
		SET Latitude = null
			, Longitude = null;

	UPDATE Address
		SET Latitude = g.Lat
			, Longitude = g.Lng
	FROM GeoLocation g
	WHERE g.Postcode = Address.Postcode;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('4B3D9BE2-8E5C-422F-91ED-E1BF850DDB61');
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'E7362423-A81B-4D46-8550-D8212AC017FC')
BEGIN
	PRINT '[Updating A10FundingCode Record Statuses]';
	SET NOCOUNT ON;

	UPDATE A10FundingCode SET RecordStatusId = 2;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('E7362423-A81B-4D46-8550-D8212AC017FC');
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'ADCEE51F-233F-4C30-AAEB-D123137AD4DD')
BEGIN
	PRINT '[Updating Provider Funding Status]';
	SET NOCOUNT ON;

	UPDATE Provider SET SFAFunded = 1;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('ADCEE51F-233F-4C30-AAEB-D123137AD4DD');
END
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '7FA23A8A-1EAD-4C71-B82B-9B2DF4BFCF29')
BEGIN
	PRINT '[Updating A10 Language Text]';
	SET NOCOUNT ON;

	UPDATE LanguageText
	SET LanguageText = 'Please select the appropriate Funding code(s) for this course opportunity. If this is not relevant, please use ''Not Applicable'''
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId FROM LanguageKeyChild LKC INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId WHERE LKG.KeyGroupName = 'BulkUpload' AND LKC.KeyChildName = 'Constants')
									AND LanguageFieldName = 'MandatoryA10Code'
								)
		AND LanguageText = 'Please select the appropriate A10 code(s) for this course opportunity. If this is not relevant, please use ''Not applicable.'''
		AND LanguageId = 1;

	UPDATE LanguageText
	SET LanguageText = 'Invalid Funding Code  {0}, section : {1}, provider : {2}.'
	WHERE LanguageFieldId IN (
							SELECT LanguageFieldId 
							FROM LanguageField
							WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId FROM LanguageKeyChild LKC INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId WHERE LKG.KeyGroupName = 'BulkUpload' AND LKC.KeyChildName = 'Constants')
							AND LanguageFieldName = 'InvalidA10Code'
						)
		AND LanguageText = 'Invalid A10 Code  {0}, section : {1}, provider : {2}.'
		AND LanguageId = 1;

	UPDATE LanguageText
	SET LanguageText = 'Funding Code'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId FROM LanguageKeyChild LKC INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId WHERE LKG.KeyGroupName = 'AddEditOpportunityModel' AND LKC.KeyChildName = 'DisplayName')
									AND LanguageFieldName = 'A10FundingCodes'
							)
		AND LanguageText = 'A10 Funding Code'
		AND LanguageId = 1;

	UPDATE LanguageText
	SET LanguageText = 'Funding Code'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId FROM LanguageKeyChild LKC INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId WHERE LKG.KeyGroupName = 'Opportunity' AND LKC.KeyChildName = 'Create')
									AND LanguageFieldName = 'A10Funding'
								)
		AND LanguageText = 'A10 Funding'
		AND LanguageId = 1;

	UPDATE LanguageText
	SET LanguageText = 'Funding Code'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId FROM LanguageKeyChild LKC INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId WHERE LKG.KeyGroupName = 'Opportunity' AND LKC.KeyChildName = 'Edit')
									AND LanguageFieldName = 'A10Funding'
								)
		AND LanguageText = 'A10 Funding'
		AND LanguageId = 1;


	UPDATE LanguageText
	SET LanguageText = 'Please enter any extra information about the price including concessions, tuition, examination fees and materials cost if appropriate. Only the standard price should be placed in the price field. Providers should provide details of any financial support they offer in terms of bursaries etc.'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId FROM LanguageKeyChild LKC INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId WHERE LKG.KeyGroupName = 'AddEditOpportunityModel' AND LKC.KeyChildName = 'Description')
									AND LanguageFieldName = 'PriceDescription'
								)
		AND LanguageText = 'Please enter any extra information about the price including concessions, tuition, examination fees and materials cost if appropriate. Only the standard price should be placed in the price field.'
		AND LanguageId = 1;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('7FA23A8A-1EAD-4C71-B82B-9B2DF4BFCF29');
END
GO



IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'FECA0904-DC85-49B5-9597-1AC6A3DD4A42')
BEGIN
	PRINT '[Updating Provider Status Based On UKRLP]';
	SET NOCOUNT ON;

	UPDATE Provider
	SET RecordStatusId = 4
	WHERE UKPRN IN (SELECT UKPRN FROM Ukrlp WHERE UkrlpStatus = 4)
		AND RecordStatusId <> 4;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('FECA0904-DC85-49B5-9597-1AC6A3DD4A42');
END
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '16EA924E-329E-4B0C-84BD-A0FCE9C8A16B')
BEGIN
	PRINT '[Updating Report Headings]';
	SET NOCOUNT ON;

	UPDATE LanguageText
	SET LanguageText = 'SFA Daily Report'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId FROM LanguageKeyChild LKC INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId WHERE LKG.KeyGroupName = 'Report' AND LKC.KeyChildName = 'DailyReport')
									AND LanguageFieldName = 'Header'
								)
		AND LanguageId = 1;


	UPDATE LanguageText
	SET LanguageText = 'SFA Weekly Report'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId FROM LanguageKeyChild LKC INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId WHERE LKG.KeyGroupName = 'Report' AND LKC.KeyChildName = 'WeeklyReport')
									AND LanguageFieldName = 'Header'
								)
		AND LanguageId = 1;

	UPDATE LanguageText
	SET LanguageText = 'SFA Additional Report'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId FROM LanguageKeyChild LKC INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId WHERE LKG.KeyGroupName = 'Report' AND LKC.KeyChildName = 'SFAWeeklyReport')
									AND LanguageFieldName = 'Header'
								)
		AND LanguageId = 1;

	UPDATE LanguageText
	SET LanguageText = 'DFE Additional Report'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId FROM LanguageKeyChild LKC INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId WHERE LKG.KeyGroupName = 'Report' AND LKC.KeyChildName = 'DFESFAWeeklyReport')
									AND LanguageFieldName = 'Header'
								)
		AND LanguageId = 1;

	UPDATE LanguageText
	SET LanguageText = 'A10 25 DfE 16-19 Funding'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId FROM LanguageKeyChild LKC INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId WHERE LKG.KeyGroupName = 'SFAWeeklyReportViewModelItem' AND LKC.KeyChildName = 'DisplayName')
									AND LanguageFieldName = 'A1025'
								)
		AND LanguageId = 1;

	UPDATE LanguageText
	SET LanguageText = 'Roles and Permissions'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId FROM LanguageKeyChild LKC INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId WHERE LKG.KeyGroupName = 'PortalAdmin' AND LKC.KeyChildName = 'PermissionRoles')
									AND LanguageFieldName = 'Title'
								)
		AND LanguageId = 1;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('16EA924E-329E-4B0C-84BD-A0FCE9C8A16B');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '2CA71E03-FDA6-4436-AFF7-C14C2EADC479')
BEGIN
	PRINT '[Updating Roles]'
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT 1 FROM PermissionInRole WHERE RoleId = '9E51B185-6FA5-4474-95A1-CF02DD523203' AND PermissionId = 48)
	BEGIN
		INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 48)
	END

	INSERT INTO __RefactorLog (OperationKey) VALUES ('2CA71E03-FDA6-4436-AFF7-C14C2EADC479')
END
GO    

	
-- ** Craig Whale 21-5-15 **
--The below statements are to change text based items already populated into Dev/Test databases and any post deployemnt deployments.
IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'FADE3185-7DA8-45D1-9863-F48DC410FC12')
BEGIN
	PRINT '[Updating Report Titles, Headers and Table Headers for report: DFE Daily, DFE Weekly DFE Addional, SFA Weekly SFA Daily, SFA Additional]';
	SET NOCOUNT ON;

	-- Update the KeyChildName from 'DFESFAWeeklyReport' to 'DFEAdditionalReport'
	UPDATE LanguageKeyChild
	SET KeyChildName = 'DFEAdditionalReport'
	WHERE KeyChildName = 'DFESFAWeeklyReport'

	-- Update the KeyChildName from 'SFAWeeklyReport' to 'SFAAdditionalReport'
	UPDATE LanguageKeyChild
	SET KeyChildName = 'SFAAdditionalReport'
	WHERE KeyChildName = 'SFAWeeklyReport'

	-- DFE Daily Report

		-- Header
		UPDATE LanguageText
		SET LanguageText = 'DFE Daily Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEDailyReport')
										AND LanguageFieldName = 'Header'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = 'DFE Daily Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEDailyReport')
										AND LanguageFieldName = 'Header'
									)
			

		-- Title
		UPDATE LanguageText
		SET LanguageText = 'DFE Daily Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEDailyReport')
										AND LanguageFieldName = 'Title'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = 'DFE Daily Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEDailyReport')
										AND LanguageFieldName = 'Title'
									)

		-- TableHeader
		UPDATE LanguageText
		SET LanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEDailyReport')
										AND LanguageFieldName = 'TableHeader'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEDailyReport')
										AND LanguageFieldName = 'TableHeader'
									)

	-- DFE Weekly Report

		-- Header
		UPDATE LanguageText
		SET LanguageText = 'DFE Weekly Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEWeeklyReport')
										AND LanguageFieldName = 'Header'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = 'DFE Weekly Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEWeeklyReport')
										AND LanguageFieldName = 'Header'
									)

		-- Title
		UPDATE LanguageText
		SET LanguageText = 'DFE Weekly Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEWeeklyReport')
										AND LanguageFieldName = 'Title'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = 'DFE Weekly Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEWeeklyReport')
										AND LanguageFieldName = 'Title'
									)

		-- TableHeader
		UPDATE LanguageText
		SET LanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEWeeklyReport')
										AND LanguageFieldName = 'TableHeader'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEWeeklyReport')
										AND LanguageFieldName = 'Header'
									)

	 --DFE Additional

		-- Header
		UPDATE LanguageText
		SET LanguageText = 'DFE Additional Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEAdditionalReport')
										AND LanguageFieldName = 'Header'
									) 
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = 'DFE Additional Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEAdditionalReport')
										AND LanguageFieldName = 'Header'
									)

		-- Title
		UPDATE LanguageText
		SET LanguageText = 'DFE Additional Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEAdditionalReport')
										AND LanguageFieldName = 'Title'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = 'DFE Additional Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEAdditionalReport')
										AND LanguageFieldName = 'Title'
									)

		-- TableHeader
		UPDATE LanguageText
		SET LanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEAdditionalReport')
										AND LanguageFieldName = 'TableHeader'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DFEAdditionalReport')
										AND LanguageFieldName = 'TableHeader'
									)

	-- SFA Daily Report

		-- Header
		UPDATE LanguageText
		SET LanguageText = 'SFA Daily Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DailyReport')
										AND LanguageFieldName = 'Header'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = 'SFA Daily Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DailyReport')
										AND LanguageFieldName = 'Header'
									)

		-- Title
		UPDATE LanguageText
		SET LanguageText = 'SFA Daily Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DailyReport')
										AND LanguageFieldName = 'Title'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = 'SFA Daily Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DailyReport')
										AND LanguageFieldName = 'Title'
									)

		-- TableHeader
		UPDATE LanguageText
		SET LanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DailyReport')
										AND LanguageFieldName = 'TableHeader'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'DailyReport')
										AND LanguageFieldName = 'TableHeader'
									)

	-- SFA Weekly Report

		-- Header
		UPDATE LanguageText
		SET LanguageText = 'SFA Weekly Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'WeeklyReport')
										AND LanguageFieldName = 'Header'
									)
			AND LanguageId = 1;

		
		UPDATE LanguageField
		SET DefaultLanguageText ='SFA Weekly Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'WeeklyReport')
										AND LanguageFieldName = 'Header'
									)

		-- Title
		UPDATE LanguageText
		SET LanguageText = 'SFA Daily Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'WeeklyReport')
										AND LanguageFieldName = 'Title'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = 'SFA Daily Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'WeeklyReport')
										AND LanguageFieldName = 'Title'
									)

		-- TableHeader
		UPDATE LanguageText
		SET LanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'WeeklyReport')
										AND LanguageFieldName = 'TableHeader'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'WeeklyReport')
										AND LanguageFieldName = 'TableHeader'
									)

	 --SFA Additional

		-- Header
		UPDATE LanguageText
		SET LanguageText = 'SFA Additional Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'SFAAdditionalReport')
										AND LanguageFieldName = 'Header'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = 'SFA Additional Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'SFAAdditionalReport')
										AND LanguageFieldName = 'Header'
									)

		-- Title
		UPDATE LanguageText
		SET LanguageText = 'SFA Additional Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'SFAAdditionalReport')
										AND LanguageFieldName = 'Title'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = 'SFA Additional Report'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'SFAAdditionalReport')
										AND LanguageFieldName = 'Title'
									)


		-- TableHeader
		UPDATE LanguageText
		SET LanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'SFAAdditionalReport')
										AND LanguageFieldName = 'TableHeader'
									)
			AND LanguageId = 1;

		UPDATE LanguageField
		SET DefaultLanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
		WHERE LanguageFieldId IN (
									SELECT LanguageFieldId 
									FROM LanguageField
									WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
																FROM LanguageKeyChild LKC 
																INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
																WHERE LKG.KeyGroupName = 'Report' 
																AND LKC.KeyChildName = 'SFAAdditionalReport')
										AND LanguageFieldName = 'TableHeader'
									)

	--Venues
	UPDATE LanguageText
	SET LanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
															FROM LanguageKeyChild LKC 
															INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
															WHERE LKG.KeyGroupName = 'Report' 
															AND LKC.KeyChildName = 'Venues')
									AND LanguageFieldName = 'TableHeader'
							)
			AND LanguageId = 1;

	UPDATE LanguageField
	SET DefaultLanguageText = '<p>Using the table below you can filter and order your venues. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print.<p>'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
															FROM LanguageKeyChild LKC 
															INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
															WHERE LKG.KeyGroupName = 'Report' 
															AND LKC.KeyChildName = 'Venues')
									AND LanguageFieldName = 'TableHeader'
							)

	--Courses
	UPDATE LanguageText
	SET LanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
															FROM LanguageKeyChild LKC 
															INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
															WHERE LKG.KeyGroupName = 'Report' 
															AND LKC.KeyChildName = 'Courses')
									AND LanguageFieldName = 'TableHeader'
							)
			AND LanguageId = 1;

	UPDATE LanguageField
	SET DefaultLanguageText = '<p>Using the table below you can filter and order your venues. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print.<p>'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
															FROM LanguageKeyChild LKC 
															INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
															WHERE LKG.KeyGroupName = 'Report' 
															AND LKC.KeyChildName = 'Courses')
									AND LanguageFieldName = 'TableHeader'
							)

	--oportunities
	UPDATE LanguageText
	SET LanguageText = '<p>The table below is part of the MI reporting. Use the ''Search'' box to filter the table, and the arrows at the top of the columns to sort it. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print. <p>'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
															FROM LanguageKeyChild LKC 
															INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
															WHERE LKG.KeyGroupName = 'Report' 
															AND LKC.KeyChildName = 'Opportunities')
									AND LanguageFieldName = 'TableHeader'
							)
			AND LanguageId = 1;

	UPDATE LanguageField
	SET DefaultLanguageText = '<p>Using the table below you can filter and order your venues. Use the buttons on the top right of the table to copy to your clipboard, save in comma separated values (CSV) format, save in Microsoft Excel format or print.<p>'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
															FROM LanguageKeyChild LKC 
															INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
															WHERE LKG.KeyGroupName = 'Report' 
															AND LKC.KeyChildName = 'Opportunities')
									AND LanguageFieldName = 'TableHeader'
							)

	INSERT INTO __RefactorLog (OperationKey) VALUES ('FADE3185-7DA8-45D1-9863-F48DC410FC12');
END
GO

-- ** Matt 28/05/2015 **
-- Update Bulk Upload language definition(s), per Bug #111588
IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'B668F07E-F9FF-4A2A-9401-369A36AE8C81')
BEGIN
	PRINT '[Updating Bulk Upload language definition(s)]';
	SET NOCOUNT ON;

 	UPDATE LanguageText
	SET LanguageText = 'In order to save your valid data to the Course Directory, we need you to confirm that you understand that you will be uploading fewer valid records than are currently held.'
	FROM LanguageText
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
															FROM LanguageKeyChild LKC 
															INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
															WHERE LKG.KeyGroupName = 'Confirm' 
															AND LKC.KeyChildName = 'PartialUpload')
									AND LanguageFieldName = 'WarningForLowRecordCount3'
							)
			AND LanguageId = 1;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('B668F07E-F9FF-4A2A-9401-369A36AE8C81');
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '2EE4A15E-AEF3-441C-868F-7B34562DC655')
BEGIN
	PRINT '[Updating Email Configuration Settings]'
	SET NOCOUNT ON

	UPDATE ConfigurationSettings
	SET Value = 'support@coursedirectoryproviderportal.org.uk'
	WHERE Name = 'AutomatedFromEmailAddress' AND Value = 'automated@tribalgroup.com'

	UPDATE ConfigurationSettings
	SET ValueDefault = 'support@coursedirectoryproviderportal.org.uk'
	WHERE Name = 'AutomatedFromEmailAddress' AND ValueDefault = 'automated@tribalgroup.com'

	INSERT INTO __RefactorLog (OperationKey) VALUES ('2EE4A15E-AEF3-441C-868F-7B34562DC655')
END
GO


-- Craig Whale - further fix relating to the above with a type in the insert - had to put it into another update to fix here - TFS Item: 115665
IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '339e8d20-8ec6-4e27-ae3a-2699e4715f13')
BEGIN
	PRINT '[Updating Report Titles, Headers and Table Headers for report: SFA Weekly';
	SET NOCOUNT ON;

	-- Title
	UPDATE LanguageText
	SET LanguageText = 'SFA Weekly Report'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
															FROM LanguageKeyChild LKC 
															INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
															WHERE LKG.KeyGroupName = 'Report' 
															AND LKC.KeyChildName = 'WeeklyReport')
									AND LanguageFieldName = 'Title'
								)
		AND LanguageId = 1;

	UPDATE LanguageField
	SET DefaultLanguageText = 'SFA Weekly Report'
	WHERE LanguageFieldId IN (
								SELECT LanguageFieldId 
								FROM LanguageField
								WHERE LanguageKeyChildId = (SELECT LanguageKeyChildId 
															FROM LanguageKeyChild LKC 
															INNER JOIN LanguageKeyGroup LKG ON LKG.LanguageKeyGroupId = LKC.LanguageKeyGroupId 
															WHERE LKG.KeyGroupName = 'Report' 
															AND LKC.KeyChildName = 'WeeklyReport')
									AND LanguageFieldName = 'Title'
								)

	INSERT INTO __RefactorLog (OperationKey) VALUES ('339e8d20-8ec6-4e27-ae3a-2699e4715f13');
END
GO

-- Matt : 02/06/2015
-- Change references to "DfE Funded" to "DfE EFA Funded"
IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'F3C74407-B91E-4581-A9C2-DD5937DDD02F')
BEGIN
	PRINT '[Updating "DfE Funded" language items]';
	SET NOCOUNT ON;

	UPDATE [lt]
	   SET [LanguageText] = 'DfE EFA Funded'
	  FROM [dbo].[LanguageText] [lt] 
	  JOIN [dbo].[LanguageField] [lf]
		ON [lf].[LanguageFieldId] = [lt].[LanguageFieldId]
	  JOIN [dbo].[LanguageKeyChild] [lkc]
		ON [lf].[LanguageKeyChildId] = [lkc].[LanguageKeyChildId]
	 WHERE [lf].[LanguageFieldName] = 'DFE1619Funded'
	   AND [lkc].[KeyChildName] = 'DisplayName'

   
	UPDATE [lf]
	   SET [DefaultLanguageText] = 'DfE EFA Funded'
	  FROM [dbo].[LanguageField] [lf]
	  JOIN [dbo].[LanguageKeyChild] [lkc]
		ON [lf].[LanguageKeyChildId] = [lkc].[LanguageKeyChildId]
	 WHERE [lf].[LanguageFieldName] = 'DFE1619Funded'
	   AND [lkc].[KeyChildName] = 'DisplayName'

	UPDATE [lt]
	   SET [LanguageText] = 'Please select whether this provider is funded by the SFA, DfE EFA or both.'
	  FROM [dbo].[LanguageText] [lt]
	  JOIN [dbo].[LanguageField] [lf]
		ON [lf].[LanguageFieldId] = [lt].[LanguageFieldId]
	  JOIN [dbo].[LanguageKeyChild] [lkc]
		ON [lkc].[LanguageKeyChildId] = [lf].[LanguageKeyChildId]
	  JOIN [dbo].[LanguageKeyGroup] [lkg]
		ON [lkg].[LanguageKeyGroupId] = [lkc].[LanguageKeyGroupId]
	 WHERE [lkg].[KeyGroupName] = 'Provider'
	   AND [lf].[LanguageFieldName] = 'FundingSourceRequired'

	UPDATE [lf]
	   SET [DefaultLanguageText] = 'Please select whether this provider is funded by the SFA, DfE EFA or both.'
	  FROM [dbo].[LanguageField] [lf]
	  JOIN [dbo].[LanguageKeyChild] [lkc]
		ON [lkc].[LanguageKeyChildId] = [lf].[LanguageKeyChildId]
	  JOIN [dbo].[LanguageKeyGroup] [lkg]
		ON [lkg].[LanguageKeyGroupId] = [lkc].[LanguageKeyGroupId]
	 WHERE [lkg].[KeyGroupName] = 'Provider'
	   AND [lf].[LanguageFieldName] = 'FundingSourceRequired'

	UPDATE [lt]
	   SET [LanguageText] = 'A10 25 DfE EFA Funding'
	  FROM [dbo].[LanguageText] [lt]
	  JOIN [dbo].[LanguageField] [lf]
		ON [lf].[LanguageFieldId] = [lt].[LanguageFieldId]
	  JOIN [dbo].[LanguageKeyChild] [lkc]
		ON [lkc].[LanguageKeyChildId] = [lf].[LanguageKeyChildId]
	  JOIN [dbo].[LanguageKeyGroup] [lkg]
		ON [lkg].[LanguageKeyGroupId] = [lkc].[LanguageKeyGroupId]
	 WHERE [lkg].[KeyGroupName] = 'SFAWeeklyReportViewModelItem'
	   AND [lf].[LanguageFieldName] = 'A1025'

	UPDATE [lf]
	   SET [DefaultLanguageText] = 'A10 25 DfE EFA Funding'
	  FROM [dbo].[LanguageField] [lf]
	  JOIN [dbo].[LanguageKeyChild] [lkc]
		ON [lkc].[LanguageKeyChildId] = [lf].[LanguageKeyChildId]
	  JOIN [dbo].[LanguageKeyGroup] [lkg]
		ON [lkg].[LanguageKeyGroupId] = [lkc].[LanguageKeyGroupId]
	 WHERE [lkg].[KeyGroupName] = 'SFAWeeklyReportViewModelItem'
	   AND [lf].[LanguageFieldName] = 'A1025'

	UPDATE [lt]
	   SET [LanguageText] = 'This provider receives DfE EFA funding.'	   
	  FROM [dbo].[LanguageText] [lt]
	  JOIN [dbo].[LanguageField] [lf]
		ON [lf].[LanguageFieldId] = [lt].[LanguageFieldId]
	  JOIN [dbo].[LanguageKeyChild] [lkc]
		ON [lkc].[LanguageKeyChildId] = [lf].[LanguageKeyChildId]
	 WHERE [lkc].[KeyChildName] = 'ContextProviderHeaderPartial'
	   AND [lf].[LanguageFieldName] = 'DfE'

	UPDATE [lf]
	   SET [DefaultLanguageText] = 'This provider receives DfE EFA funding.'	   
	  FROM [dbo].[LanguageField] [lf]
	  JOIN [dbo].[LanguageKeyChild] [lkc]
		ON [lkc].[LanguageKeyChildId] = [lf].[LanguageKeyChildId]
	 WHERE [lkc].[KeyChildName] = 'ContextProviderHeaderPartial'
	   AND [lf].[LanguageFieldName] = 'DfE'

	UPDATE [lt]
	   SET [LanguageText] = 'This organisation has provider(s) receiving DfE EFA funding.'
	  FROM [dbo].[LanguageText] [lt]
	  JOIN [dbo].[LanguageField] [lf]
		ON [lf].[LanguageFieldId] = [lt].[LanguageFieldId]
	  JOIN [dbo].[LanguageKeyChild] [lkc]
		ON [lkc].[LanguageKeyChildId] = [lf].[LanguageKeyChildId]
	 WHERE [lkc].[KeyChildName] = 'ContextOrganisationHeaderPartial'
	   AND [lf].[LanguageFieldName] = 'DfE'

	UPDATE [lf]
	   SET [DefaultLanguageText] = 'This organisation has provider(s) receiving DfE EFA funding.'
	  FROM [dbo].[LanguageField] [lf]
	  JOIN [dbo].[LanguageKeyChild] [lkc]
		ON [lkc].[LanguageKeyChildId] = [lf].[LanguageKeyChildId]
	 WHERE [lkc].[KeyChildName] = 'ContextOrganisationHeaderPartial'
	   AND [lf].[LanguageFieldName] = 'DfE'

	UPDATE [dbo].[A10FundingCode]
	   SET [A10FundingCodeName] = 'DfE EFA Funded'
	 WHERE [A10FundingCodeName] = 'DfE 16-19 Funded'

	INSERT INTO __RefactorLog (OperationKey) VALUES ('F3C74407-B91E-4581-A9C2-DD5937DDD02F');
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '8816FCBD-94EA-4CDD-B672-DC9013E8AA9B')
BEGIN
	PRINT '[Updating Secure Access Support Error Messages]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Log in failed for DfE Secure Access. If you believe you should have access to the Post 16 Provider Portal please contact the Post 16 Provider Portal Support Team on <a href=''tel:08448115073''>0844 811 5073</a> or <a href=''mailto:support@coursedirectoryproviderportal.org.uk''>support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_AssertionConsumerService_SSOLogInFailed', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. Invalid log in - error code 2327. If you believe you should have access to the Post 16 Provider Portal please contact the Post 16 Provider Portal Support Team on <a href=''tel:08448115073''>0844 811 5073</a> or <a href=''mailto:support@coursedirectoryproviderportal.org.uk''>support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_UserResponseError_SecureAccessCreateError', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. Invalid log in - error code 2386. If you believe you should have access to the Post 16 Provider Portal please contact the Post 16 Provider Portal Support Team on <a href=''tel:08448115073''>0844 811 5073</a> or <a href=''mailto:support@coursedirectoryproviderportal.org.uk''>support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_UserResponseError_SecureAccessCreateFailed', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. Invalid log in - error code 2181. If you believe you should have access to the Post 16 Provider Portal please contact the Post 16 Provider Portal Support Team on <a href=''tel:08448115073''>0844 811 5073</a> or <a href=''mailto:support@coursedirectoryproviderportal.org.uk''>support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_UserResponseError_SecureAccessEmailInUse', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. Invalid log in - error code 2493. If you believe you should have access to the Post 16 Provider Portal please contact the Post 16 Provider Portal Support Team on <a href=''tel:08448115073''>0844 811 5073</a> or <a href=''mailto:support@coursedirectoryproviderportal.org.uk''>support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_UserResponseError_SecureAccessUserNameInUse', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. We are unable to uniquely identify your organisation and cannot log you in. If you believe you should have access to the Post 16 Provider Portal please contact the Post 16 Provider Portal Support Team on <a href=''tel:08448115073''>0844 811 5073</a> or <a href=''mailto:support@coursedirectoryproviderportal.org.uk''>support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_GetProviderAsync_SecureAccessIdNotUnique', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. Your organisation does not have a UKPRN record which is required to access the Post 16 Provider Portal. If you believe you should have access to the Post 16 Provider Portal please contact the Post 16 Provider Portal Support Team on <a href=''tel:08448115073''>0844 811 5073</a> or <a href=''mailto:support@coursedirectoryproviderportal.org.uk''>support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_GetProviderAsync_NoUkprn', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. We are unable to uniquely identify your organisation and cannot log you in. If you believe you should have access to the Post 16 Provider Portal please contact the Post 16 Provider Portal Support Team on <a href=''tel:08448115073''>0844 811 5073</a> or <a href=''mailto:support@coursedirectoryproviderportal.org.uk''>support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_GetProviderAsync_CannotDetermineProvider', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. An error occurred setting up your organisation as a new DFE 16-19 provider. If you believe you should have access to the Post 16 Provider Portal please contact the Post 16 Provider Portal Support Team on <a href=''tel:08448115073''>0844 811 5073</a> or <a href=''mailto:support@coursedirectoryproviderportal.org.uk''>support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_GetProviderAsync_ErrorSavingProvider', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('8816FCBD-94EA-4CDD-B672-DC9013E8AA9B')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '78B7FDC9-51B1-4665-953B-0466C9C83305')
BEGIN
	PRINT '[Updating Address GeoLocation Information]';
	SET NOCOUNT ON;

	UPDATE Address
		SET Latitude = null
			, Longitude = null;

	UPDATE Address
		SET Latitude = g.Lat
			, Longitude = g.Lng
	FROM GeoLocation g
	WHERE g.Postcode = Address.Postcode;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('78B7FDC9-51B1-4665-953B-0466C9C83305');
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'DDFA342C-9818-4544-8A42-4986BA3D782B')
BEGIN
	PRINT '[Updating DfE EFA Error message originally specified in Provider Controller]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'DfE Provider Type is not valid for providers without DfE EFA Funding'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Create_DfEProviderTypeNotAllowed', @NewText, @NewText

	SET @NewText = 'DfE Provider Status is not valid for providers without DfE EFA Funding'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Create_DfEProviderStatusNotAllowed', @NewText, @NewText

	SET @NewText = 'DfE Local Authority is not valid for providers without DfE EFA Funding'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Create_DfELocalAuthorityNotAllowed', @NewText, @NewText

	SET @NewText = 'DfE Region is not valid for providers without DfE EFA Funding'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Create_DfERegionNotAllowed', @NewText, @NewText

	SET @NewText = 'DfE Establishment Type is not valid for providers without DfE EFA Funding'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Create_DfEEstablishmentTypeNotAllowed', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('DDFA342C-9818-4544-8A42-4986BA3D782B')
END
GO



IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'E81F43F6-04BE-4F75-8643-371792154A8F')
BEGIN
	PRINT '[Fixing data migration issue where users are linked to multiple providers]';
	SET NOCOUNT ON;

	DELETE FROM ProviderUser WHERE UserId IN (SELECT UserId FROM ProviderUser GROUP BY UserId HAVING Count(*) > 1);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('E81F43F6-04BE-4F75-8643-371792154A8F');
END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '15EE8EC1-623A-41AE-8B71-77DDB08E8C7D')
BEGIN
	PRINT '[Updating Bulk Upload Language Text]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'One or more sections of the bulk upload file are empty.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_partialUploadSummary_NoValidRecordsAlert', @NewText, @NewText

	SET @NewText = 'Publishing this file could result in all courses and opportunities for your provision being deleted, please correct any errors, ensure you have valid records in all sections and upload the file again.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_partialUploadSummary_NoValidRecordsMessage', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('15EE8EC1-623A-41AE-8B71-77DDB08E8C7D')
END
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '70BA0F40-CEBD-43F1-9CB2-D7D4C7834AA6')
BEGIN
	PRINT '[Updating Bulk Upload Language Text]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Bulk Upload Summary'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_HistoryDetails_Title', @NewText, @NewText
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_HistoryDetails_Header', @NewText, @NewText

	SET @NewText = 'Bulk Upload'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_Index_Title', @NewText, @NewText
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_Index_Header', @NewText, @NewText


	SET @NewText = 'Latest Files Uploaded'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_History_Title', @NewText, @NewText
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_History_Header', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('70BA0F40-CEBD-43F1-9CB2-D7D4C7834AA6')
END
GO

-- Removed refactor EE0A6775-AE21-4023-9D09-AC713D76C639

-- Removed refactor 4DAEA80A-3EB1-4FE3-825C-D3CDFC5AFD57

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '6811D954-788F-46E5-ADEC-1885930F6B14')
BEGIN
	PRINT '[Add permission to upload FE Choices data for developer role]';
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT PermissionId FROM Permission WHERE PermissionId = 52)
	BEGIN
		INSERT INTO Permission (PermissionId, PermissionName, PermissionDescription) VALUES (52, 'CanUploadFEChoicesData', 'With this permission a user may view upload FE Choices data');
	END;

	INSERT INTO PermissionInRole (PermissionId, RoleId) VALUES (52, '947CD027-FD8B-494D-97B3-FA512A20650A');

	INSERT INTO __RefactorLog (OperationKey) VALUES ('6811D954-788F-46E5-ADEC-1885930F6B14');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'C9470814-17B1-4DBB-8348-CD6EA3DC44D7')
BEGIN
	PRINT '[Add permission to upload FE Choices data for developer role]';
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT PermissionId FROM Permission WHERE PermissionId = 53)
	BEGIN
		INSERT INTO Permission (PermissionId, PermissionName, PermissionDescription) VALUES (53, 'CanUploadLARSData', 'With this permission a user may view upload LARS data');
	END;

	INSERT INTO PermissionInRole (PermissionId, RoleId) VALUES (53, '947CD027-FD8B-494D-97B3-FA512A20650A');

	INSERT INTO __RefactorLog (OperationKey) VALUES ('C9470814-17B1-4DBB-8348-CD6EA3DC44D7');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'FA060AB8-14FA-4E16-ABAE-E230256D226D')
BEGIN
	PRINT '[Add configuration setting for LARS Upload file]';
	SET NOCOUNT ON;

	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) VALUES ('VirtualDirectoryNameForStoringLARSFiles', '\\vfiler-th\SFA_CDPP\LARSUpload', '\\vfiler-th\SFA_CDPP\BulkUpload', 'System.String', 'Directory to temporarily store LARS import file.', 0);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('FA060AB8-14FA-4E16-ABAE-E230256D226D');
END;
GO

-- Removed refactor 6AB65F9B-5FF7-4B2C-999F-CF32AC53AA25

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'FD74B062-3706-4977-AA0E-78F97AD96C0D')
BEGIN
	PRINT '[Add permission to upload Address Base data for developer role]';
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT PermissionId FROM Permission WHERE PermissionId = 54)
	BEGIN
		INSERT INTO Permission (PermissionId, PermissionName, PermissionDescription) VALUES (54, 'CanUploadAddressBaseData', 'With this permission a user may view upload Address Base data');
	END;

	INSERT INTO PermissionInRole (PermissionId, RoleId) VALUES (54, '947CD027-FD8B-494D-97B3-FA512A20650A');

	INSERT INTO __RefactorLog (OperationKey) VALUES ('FD74B062-3706-4977-AA0E-78F97AD96C0D');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '0AD21BD8-F6C3-4B06-8483-1F2A14EAC838')
BEGIN
	PRINT '[Add configuration setting for Address Base Upload file]';
	SET NOCOUNT ON;

	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) VALUES ('VirtualDirectoryNameForStoringAddressBaseFiles', '\\vfiler-th\SFA_CDPP\AddressBaseUpload', '\\vfiler-th\SFA_CDPP\AddressBaseUpload', 'System.String', 'Directory to temporarily store Address Base import file.', 0);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('0AD21BD8-F6C3-4B06-8483-1F2A14EAC838');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '30D7DB8E-BF7A-4958-BCB5-A9AB7143F94C')
BEGIN
	PRINT '[Updating password reset submit button text]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Set Password'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Account_ResetPassword_Submit', @NewText, @NewText

	UPDATE EmailTemplate
	SET HtmlBody = '<p>Please <a href="%URL%">confirm your account by clicking here</a>.</p>'
	WHERE EmailTemplateId = 2

	INSERT INTO __RefactorLog (OperationKey) VALUES ('30D7DB8E-BF7A-4958-BCB5-A9AB7143F94C')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '3ACBFACE-48EA-4D91-BFAD-A69225898651')
BEGIN
	PRINT '[Updating Secure Access Support Error Messages]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Log in failed for DfE Secure Access. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href=''tel:08448115028''>0844 811 5028</a> or <a href=''mailto:dfe.support@coursedirectoryproviderportal.org.uk''>dfe.support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_AssertionConsumerService_SSOLogInFailed', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. Invalid log in - error code 2327. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href=''tel:08448115028''>0844 811 5028</a> or <a href=''mailto:dfe.support@coursedirectoryproviderportal.org.uk''>dfe.support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_UserResponseError_SecureAccessCreateError', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. Invalid log in - error code 2386. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href=''tel:08448115028''>0844 811 5028</a> or <a href=''mailto:dfe.support@coursedirectoryproviderportal.org.uk''>dfe.support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_UserResponseError_SecureAccessCreateFailed', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. Invalid log in - error code 2181. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href=''tel:08448115028''>0844 811 5028</a> or <a href=''mailto:dfe.support@coursedirectoryproviderportal.org.uk''>dfe.support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_UserResponseError_SecureAccessEmailInUse', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. Invalid log in - error code 2493. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href=''tel:08448115028''>0844 811 5028</a> or <a href=''mailto:dfe.support@coursedirectoryproviderportal.org.uk''>dfe.support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_UserResponseError_SecureAccessUserNameInUse', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. We are unable to uniquely identify your organisation and cannot log you in. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href=''tel:08448115028''>0844 811 5028</a> or <a href=''mailto:dfe.support@coursedirectoryproviderportal.org.uk''>dfe.support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_GetProviderAsync_SecureAccessIdNotUnique', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. Your organisation does not have a UKPRN record which is required to access the Post 16 Provider Portal. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href=''tel:08448115028''>0844 811 5028</a> or <a href=''mailto:dfe.support@coursedirectoryproviderportal.org.uk''>dfe.support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_GetProviderAsync_NoUkprn', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. We are unable to uniquely identify your organisation and cannot log you in. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href=''tel:08448115028''>0844 811 5028</a> or <a href=''mailto:dfe.support@coursedirectoryproviderportal.org.uk''>dfe.support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_GetProviderAsync_CannotDetermineProvider', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. An error occurred setting up your organisation as a new DFE 16-19 provider. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href=''tel:08448115028''>0844 811 5028</a> or <a href=''mailto:dfe.support@coursedirectoryproviderportal.org.uk''>dfe.support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_GetProviderAsync_ErrorSavingProvider', @NewText, @NewText

	SET @NewText = 'Log in failed for DfE Secure Access. Your organisation is no longer available. If you believe you should have access to the Post 16 Provider Portal please contact the DfE Support Team on <a href=''tel:08448115028''>0844 811 5028</a> or <a href=''mailto:dfe.support@coursedirectoryproviderportal.org.uk''>dfe.support@coursedirectoryproviderportal.org.uk</a>.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'SA_GetValidatedProviderAsync_ProviderNotAvailable', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('3ACBFACE-48EA-4D91-BFAD-A69225898651')
END
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '00AD2EEF-8972-4E05-BA60-CA1739ADC41C')
BEGIN
	PRINT '[Deleting last logged in date for users with a 1899 date]';
	SET NOCOUNT ON;

	UPDATE AspNetUsers SET LastLoginDateTimeUtc = NULL WHERE LastLoginDateTimeUtc = CAST('30 Dec 1899' AS DATETIME);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('00AD2EEF-8972-4E05-BA60-CA1739ADC41C');
END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '88479EFE-4E80-4E19-B93D-750E26BAAED3')
BEGIN
	PRINT '[Fixing Referential Integrity for Courses and CourseInstances]'
	SET NOCOUNT ON

	UPDATE CourseInstance
		SET DisplayedByOrganisationId = null
	WHERE DisplayedByOrganisationId = 0

	UPDATE CourseInstance
		SET OfferedByOrganisationId = null
	WHERE OfferedByOrganisationId = 0

	INSERT INTO __RefactorLog (OperationKey) VALUES ('88479EFE-4E80-4E19-B93D-750E26BAAED3')
END
GO

-- JR: TFS Item 117713 (Undo previous fixes)
IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '67DA6DEC-7923-4AE4-85B5-731D7F168B80')
BEGIN
	PRINT '[Undoing changes to user roles]'
	SET NOCOUNT ON

	IF EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'EE0A6775-AE21-4023-9D09-AC713D76C639')
	BEGIN
		-- Remove Organisation Superuser : CanEditProviderSpecialFields
		IF EXISTS (SELECT 1 FROM PermissionInRole WHERE RoleId = '9176659E-1A37-4C74-A7E5-1A3B455DEDBB' AND PermissionId = 43)
		BEGIN
			DELETE FROM PermissionInRole WHERE RoleId = '9176659E-1A37-4C74-A7E5-1A3B455DEDBB' AND PermissionId = 43
		END
	END

	IF EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '6AB65F9B-5FF7-4B2C-999F-CF32AC53AA25')
	BEGIN
		-- Remove Organisation Superuser : CanEditOrganisationSpecialFields
		IF EXISTS (SELECT 1 FROM PermissionInRole WHERE RoleId = '9176659E-1A37-4C74-A7E5-1A3B455DEDBB' AND PermissionId = 44)
		BEGIN
			DELETE FROM PermissionInRole WHERE RoleId = '9176659E-1A37-4C74-A7E5-1A3B455DEDBB' AND PermissionId = 44
		END

		-- Remove Provider Superuser : CanEditProviderSpecialFields
		IF EXISTS (SELECT 1 FROM PermissionInRole WHERE RoleId = '5394B20B-1668-4D4C-AEE4-0FA057AC12B8' AND PermissionId = 43)
		BEGIN
			DELETE FROM PermissionInRole WHERE RoleId = '5394B20B-1668-4D4C-AEE4-0FA057AC12B8' AND PermissionId = 43
		END
	END
	
	INSERT INTO __RefactorLog (OperationKey) VALUES ('67DA6DEC-7923-4AE4-85B5-731D7F168B80')
END
GO

-- JR: TFS Item 117657 (Undo previous fixes and update configuration)
IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'D825CACA-F2BF-46C7-A84E-3CB85DC14254')
BEGIN
	SET NOCOUNT ON

	IF EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '4DAEA80A-3EB1-4FE3-825C-D3CDFC5AFD57')
	BEGIN
		DELETE FROM ConfigurationSettings WHERE Name in ('ProviderSuperUserCanAddRoles', 'OrganisationSuperUserCanAddRoles')
	END

	-- Allow superusers to create superusers
	UPDATE ConfigurationSettings
		SET Value = 'Provider Superuser;Provider User'
	WHERE Name = 'ProviderUserCanAddRoles'

	UPDATE ConfigurationSettings
		SET Value = 'Organisation Superuser;Organisation User;Provider Superuser;Provider User'
	WHERE Name = 'OrganisationUserCanAddRoles'

	INSERT INTO __RefactorLog (OperationKey) VALUES ('D825CACA-F2BF-46C7-A84E-3CB85DC14254')
END
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '7A129923-5BA4-41A3-B612-05F82E75EFE3')
BEGIN
	PRINT '[Updating Price Description]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'You must enter a Price in pounds or a Price Description, or both. Please also use the Price Description field to provide details of any financial support you offer e.g. bursaries';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Opportunity_Create_PriceSummary', @NewText, @NewText;
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Opportunity_Edit_PriceSummary', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('7A129923-5BA4-41A3-B612-05F82E75EFE3');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'B603DD15-C1D3-4483-89B0-591BD8E0AE28')
BEGIN
	PRINT '[Updating A10 Funding Code Description]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Please select the appropriate Funding code(s) for this course opportunity. If this is not relevant, please use ''Not Applicable''. EFA funded Providers should select funding code 25 for all courses in addition to any other funding codes that may apply.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditOpportunityModel_Description_A10FundingCodes', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('B603DD15-C1D3-4483-89B0-591BD8E0AE28');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'D72CA62E-D3AC-4CBD-AD9B-382B5253EAE4')
BEGIN
	PRINT '[Migrating legacy Bulk Upload data]'
	SET NOCOUNT ON

	EXEC [dbo].[up_BulkUploadMigration]

	INSERT INTO __RefactorLog (OperationKey) VALUES ('D72CA62E-D3AC-4CBD-AD9B-382B5253EAE4');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '99A6724E-9237-4CE4-9165-CFD6EF6E0DC8')
BEGIN
	PRINT '[Deleting Home_Index_XXX Language Fields]'
	SET NOCOUNT ON

	DECLARE @LanguageFieldId as INT
	DECLARE @cursor as CURSOR
 
	SET @cursor = CURSOR FOR
		SELECT LanguageFieldId
		FROM LanguageKeyGroup g
			join LanguageKeyChild c on c.LanguageKeyGroupId = g.LanguageKeyGroupId
			join LanguageField f on f.LanguageKeyChildId = c.LanguageKeyChildId
		WHERE KeyGroupName = 'Home' AND KeyChildName = 'Index'

	OPEN @cursor
	FETCH NEXT FROM @cursor INTO @LanguageFieldId
 
	WHILE @@FETCH_STATUS = 0
	BEGIN
	 EXEC up_LanguageFieldDelete @LanguageFieldId
	 FETCH NEXT FROM @cursor INTO @LanguageFieldId
	END
 
	CLOSE @cursor
	DEALLOCATE @cursor

	INSERT INTO __RefactorLog (OperationKey) VALUES ('99A6724E-9237-4CE4-9165-CFD6EF6E0DC8')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'D2D985AC-8E50-455B-A068-B92D1E2960D6')
BEGIN
	PRINT '[Updating Bulk Upload error text]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Blank Course Id for provider {0} section {1}';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_Constants_BlankOpportunityId', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('D2D985AC-8E50-455B-A068-B92D1E2960D6');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '2A28F9E3-8B96-4C1C-8470-7A4A1F46D1A4')
BEGIN
	PRINT '[Updating Home Page for Users with Invalid Accounts]'
	SET NOCOUNT ON

	UPDATE Content
		SET Body = '<div class="row">
<div class="col-md-12">
<h1>Welcome to the Course Directory Provider Portal</h1>
<p>Your account is not currently linked to a Provider or an Organisation, please contact the Course Directory Support Team on <a href="tel:08448115073">0844 811 5073</a> or <a href="mailto:support@coursedirectoryproviderportal.org.uk" target="_blank">support@coursedirectoryproviderportal.org.uk</a>.</p>
</div>
</div>'
	WHERE Path = 'HomeInvalidUser';

	INSERT INTO __RefactorLog (OperationKey) VALUES ('2A28F9E3-8B96-4C1C-8470-7A4A1F46D1A4')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'D445CB8A-1D4F-4756-8B47-EFA36CFAB142')
BEGIN
	PRINT '[Updating SFA Provider Traffic Light Email Templates]'
	SET NOCOUNT ON

	DECLARE @EmailTemplateGroupId int = 
	(
		SELECT EmailTemplateGroupId
		FROM EmailTemplateGroup
		WHERE Name = 'Traffic Light Status (SFA)'
	)
	
	UPDATE EmailTemplate
		SET Params = '%PROVIDERNAME%=Provider Name,%LASTUPDATEDATE%=Last Update Date,%MONTHSSINCEUPDATE%=Months Since the Last Update'
	WHERE EmailTemplateGroupId = @EmailTemplateGroupId

	UPDATE EmailTemplate
		SET
			HtmlBody = '<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. This means it is %MONTHSSINCEUPDATE% months since you last updated and you are now at Amber status.</p>'
			, TextBody = 'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. This means it is %MONTHSSINCEUPDATE% months since you last updated and you are now at Amber status.'
	WHERE Name = 'Provider Traffic Light is Now Amber'
		AND EmailTemplateGroupId = @EmailTemplateGroupId

	UPDATE EmailTemplate
		SET
			HtmlBody = '<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are now at Red status, since it has been over %MONTHSSINCEUPDATE% months since you updated.</p>'
			, TextBody = 'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are now at Red status, since it has been over %MONTHSSINCEUPDATE% months since you updated.'
	WHERE Name = 'Provider Traffic Light is Now Red'
		AND EmailTemplateGroupId = @EmailTemplateGroupId

	UPDATE EmailTemplate
		SET
			HtmlBody = '<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are still at Red status, since it has been over %MONTHSSINCEUPDATE% months since you updated.</p>'
			, TextBody = 'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are still at Red status, since it has been over %MONTHSSINCEUPDATE% months since you updated.'
	WHERE Name = 'Provider Traffic Light is Still Red'
		AND EmailTemplateGroupId = @EmailTemplateGroupId

	INSERT INTO __RefactorLog (OperationKey) VALUES ('D445CB8A-1D4F-4756-8B47-EFA36CFAB142')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '9A00AC93-BE42-4EE7-B27D-2A9E3B2D4180')
BEGIN
	PRINT '[Updating Opportunity Price Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Price (&pound;)'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditOpportunityModel_DisplayName_Price', @NewText, @NewText

	SET @NewText = 'You must enter a Price in pounds or a Price Description, or both. Please also use the Price Description field to provide details of any financial support you offer e.g. bursaries.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditOpportunityModel_Description_Price', @NewText, @NewText
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Opportunity_Create_PriceSummary', @NewText, @NewText
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Opportunity_Edit_PriceSummary', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('9A00AC93-BE42-4EE7-B27D-2A9E3B2D4180')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '9A00AC93-BE42-4EE7-B27D-2A9E3B2D4180')
BEGIN
	PRINT '[Updating Opportunity Duration Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Price (&pound;)'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditOpportunityModel_DisplayName_Price', @NewText, @NewText

	SET @NewText = 'You must enter a Price in pounds or a Price Description, or both. Please also use the Price Description field to provide details of any financial support you offer e.g. bursaries.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditOpportunityModel_Description_Price', @NewText, @NewText
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Opportunity_Create_PriceSummary', @NewText, @NewText
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Opportunity_Edit_PriceSummary', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('9A00AC93-BE42-4EE7-B27D-2A9E3B2D4180')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '7CA20C2A-0E23-4290-87CC-A893055185AD')
BEGIN
	PRINT '[Updating Opportunity Duration Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Duration is mandatory unless you enter both a Start Date and End Date (see below). For duration, you may enter a number and unit (e.g. \"6 months\"0 or \"2 semesters\"), or a description (e.g. \"Different durations available.\"), or both. Take care your duration and duration description don''t conflict – if they do, you may need to create a separate opportunity instead.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Opportunity_Create_DurationSummary', @NewText, @NewText
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Opportunity_Edit_DurationSummary', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('7CA20C2A-0E23-4290-87CC-A893055185AD')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '1D7CFF36-7C24-424E-97C3-7D6C534655C1')
BEGIN
	PRINT '[Updating Bulk Upload Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Blank Provider Opportunity Id for provider {0} section {1}'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_Constants_BlankOpportunityId', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('1D7CFF36-7C24-424E-97C3-7D6C534655C1')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'F85EE7BE-21AB-4E3B-BC03-467AB2011FE1')
BEGIN
	PRINT '[Updating Language Fields for the Bulk Upload Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'A provider already exists with this name.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_Constants_ProviderAlreadyExistsWithName', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('F85EE7BE-21AB-4E3B-BC03-467AB2011FE1')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '08F3B0EF-1B98-4AA6-85DF-7B228B27B54B')
BEGIN
	PRINT '[Adding WORLD as a Location]'
	SET NOCOUNT ON

	INSERT INTO VenueLocation
		(LocationName, ParentVenueLocationId, Latitude, Longitude, Region, EastingMin, EastingMax, NorthingMin, NorthingMax)
	SELECT 'WORLD', null, Latitude, Longitude, 'WORLD', EastingMin, EastingMax, NorthingMin, NorthingMax
	FROM VenueLocation
	WHERE LocationName = 'UNITED KINGDOM'

	UPDATE VenueLocation
		SET ParentVenueLocationId = (SELECT VenueLocationId FROM VenueLocation WHERE LocationName = 'WORLD')
	WHERE LocationName = 'UNITED KINGDOM'

	INSERT INTO __RefactorLog (OperationKey) VALUES ('08F3B0EF-1B98-4AA6-85DF-7B228B27B54B')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '08F3B0EF-1B98-4AA6-85DF-7B228B27B54B')
BEGIN
	PRINT '[Updating Quality Email Log]'
	SET NOCOUNT ON

	UPDATE QualityEmailLog
		SET
			CreatedDateTimeUtc = EmailDateTimeUtc
			, TrafficLightStatusId =
				CASE TrafficLightStatusId
					WHEN 0 THEN 1
					WHEN 1 THEN 2
					WHEN 3 THEN 3
					ELSE TrafficLightStatusId
				END

	DELETE FROM QualityEmailLog
		WHERE EmailTemplateId IS NULL AND NextEmailTemplateId IS NULL

	INSERT INTO __RefactorLog (OperationKey) VALUES ('08F3B0EF-1B98-4AA6-85DF-7B228B27B54B')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '9876FD9A-E15C-49B8-AEA3-9DFE0F94406D')
BEGIN
	PRINT '[Disallow remembered logins]'
	SET NOCOUNT ON

	UPDATE ConfigurationSettings
		SET Value = 'false'
	WHERE Name = 'AutoSiteLoginAllow'

	INSERT INTO __RefactorLog (OperationKey) VALUES ('9876FD9A-E15C-49B8-AEA3-9DFE0F94406D')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '7936149D-77A5-45C9-8AAB-A7A9FABB446B')
BEGIN
	PRINT '[Allow everyone to recalculate quality scores]'
	SET NOCOUNT ON

	DECLARE @PermissionId int = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanRecalculateQualityScores')

	INSERT INTO PermissionInRole
		(RoleId, PermissionId)
	SELECT Id, @PermissionId
	FROM AspNetRoles
	WHERE Id NOT IN (
		SELECT RoleId
		FROM PermissionInRole
		WHERE PermissionId = @PermissionId
	)

	INSERT INTO __RefactorLog (OperationKey) VALUES ('7936149D-77A5-45C9-8AAB-A7A9FABB446B')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '28BBB835-137A-4718-9ECC-90E2FF877288')
BEGIN
	PRINT '[Updating Address Download Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = '<hr /><p>Use this page to upload new Address Base data.  The files should be a zipped copies of the CSV version.  The latest version can be ordered from <a href="https://orders.ordnancesurvey.co.uk/orders/index.html" target="blank">this page</a>.</p><p>You can upload many ZIP files each containing CSV files.  Once all the ZIP files have been uploaded, click <strong>Import Data</strong></p><hr />'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Address_Index_SubHeader', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('28BBB835-137A-4718-9ECC-90E2FF877288')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '81DB3E8E-131D-40EF-88D1-CED96B7CB90C')
BEGIN
	PRINT '[Updating Bulk Upload Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Column count does not match for {0} section, Provider Id {1}.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_Constants_ColumnCountNotMatch', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('81DB3E8E-131D-40EF-88D1-CED96B7CB90C')
END
GO

GO
-- Migrated 4A2823C9-6EC7-4E74-B6D3-E7C18BC29CBC to DefaultRolePermissions.sql

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'E59740B6-1649-4669-9AD8-3A8F8AA165A7')
BEGIN
	PRINT '[Add configuration setting for UCAS Upload file]';
	SET NOCOUNT ON;

	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) VALUES ('VirtualDirectoryNameForStoringUCASImportFiles', '\\vfiler-th\SFA_CDPP\UCASUpload', '\\vfiler-th\SFA_CDPP\UCASUpload', 'System.String', 'Directory to temporarily store UCAS import file.', 0);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('E59740B6-1649-4669-9AD8-3A8F8AA165A7');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '20201A4B-90A0-4804-8A72-F9F5F9CC6976')
BEGIN
	PRINT '[Updating Report Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Bulk Upload History Report'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AdminReportsBulkUploadHistoryReportMenuItem', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('20201A4B-90A0-4804-8A72-F9F5F9CC6976')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'BF76D49C-5369-415E-B951-3B13CD5EE4DF')
BEGIN
	PRINT '[Adding Meta Data Upload Types]';
	SET NOCOUNT ON;

	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (8, 'UCAS', 'UCAS Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (9, 'Standards', 'Aprrenticeship Standards');

	INSERT INTO __RefactorLog (OperationKey) VALUES ('BF76D49C-5369-415E-B951-3B13CD5EE4DF');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '0601DB86-A022-4F6E-842D-E209ADBC8E00')
BEGIN
	PRINT '[Adding Meta Data Upload Types]';
	SET NOCOUNT ON;

	UPDATE [dbo].[MetadataUploadType] SET MetadataUploadTypeName = 'UCASCourseEntry', MetadataUploadTypeDescription = 'UCAS Course Entry Data' WHERE MetadataUploadTypeId = 8;
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (10, 'UCASCourses', 'UCAS Course Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (11, 'UCASCoursesIndex', 'UCAS Course Index Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (12, 'UCASCurrencies', 'UCAS Currency Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (13, 'UCASDurations', 'UCAS Duration Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (14, 'UCASFees', 'UCAS Fee Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (15, 'UCASFeeYears', 'UCAS Fee Year Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (16, 'UCASOrgs', 'UCAS Org Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (17, 'UCASPlacesOfStudy', 'UCAS Place Of Study Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (18, 'UCASStarts', 'UCAS Start Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (19, 'UCASStartsIndex', 'UCAS Start Index Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (20, 'UCASTowns', 'UCAS Town Data');

	INSERT INTO __RefactorLog (OperationKey) VALUES ('0601DB86-A022-4F6E-842D-E209ADBC8E00');
END;
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tmp_LearnDirectClassification' AND TABLE_SCHEMA = 'dbo')
BEGIN
	PRINT '[Dropping Table tmp_LearnDirectClassification]';
	SET NOCOUNT ON;

	DROP TABLE [dbo].[tmp_LearnDirectClassification];
END;
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tmp_LearningAim' AND TABLE_SCHEMA = 'dbo')
BEGIN
	PRINT '[Dropping Table tmp_LearningAim]';
	SET NOCOUNT ON;

	DROP TABLE [dbo].[tmp_LearningAim];
END;
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tmp_LearningAimAwardOrg' AND TABLE_SCHEMA = 'dbo')
BEGIN
	PRINT '[Dropping Table tmp_LearningAimAwardOrg]';
	SET NOCOUNT ON;

	DROP TABLE [dbo].[tmp_LearningAimAwardOrg];
END;
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tmp_LearningAimValidity' AND TABLE_SCHEMA = 'dbo')
BEGIN
	PRINT '[Dropping Table tmp_LearningAimValidity]';
	SET NOCOUNT ON;

	DROP TABLE [dbo].[tmp_LearningAimValidity];
END;
GO

IF NOT EXISTS (SELECT * FROM [dbo].[UcasStudyModeMapping])
BEGIN
	PRINT '[Adding UCAS Study Mode Mapping]';
	SET NOCOUNT ON;

	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (27, N'Block-Release', NULL, NULL, 2)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (28, N'Distance Learning', NULL, 5, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (29, N'Full-time', 1, NULL, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (30, N'Full-time including placement abroad', 1, NULL, 4)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (31, N'Full-time with time abroad', 1, NULL, 4)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (32, N'Full-time with time abroad and foundation year', 1, NULL, 4)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (33, N'Mixed mode', 4, 4, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (34, N'Open Learning', NULL, NULL, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (35, N'Part-time', 2, NULL, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (36, N'Part-tim block release', 2, NULL, 2)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (37, N'Part-time day', 2, NULL, 1)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (38, N'Part-time day and evening', 2, NULL, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (39, N'Part-time day release', 2, NULL, 2)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (40, N'Part-time day release and evening', 2, NULL, 3)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (41, N'Part-time day/evening', 2, NULL, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (42, N'Part-time evening', 2, NULL, 3)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (43, N'Part-time evening only', 2, NULL, 3)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (45, N'Sandwich', 1, NULL, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (46, N'Sandwich including foundation year', 1, NULL, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (47, N'Sandwich including industrial placement', 1, NULL, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (48, N'Sandwich with time abroad', 1, NULL, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (49, N'Sandwich with time abroad and foundation year', 1, NULL, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (50, N'Full-time including foundation year', 1, NULL, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (51, N'Part-time day and block release', 1, NULL, 2)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (52, N'Part-time weekend', 2, NULL, 5)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (53, N'Work-based learning', 1, 3, 1)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (79, N'Variable', NULL, NULL, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (80, N'Online study', NULL, 7, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (81, N'Distance learning (Full-time)', 1, 7, NULL)
	INSERT [dbo].[UcasStudyModeMapping] ([UcasStudyModeId], [UcasStudyMode], [MapsToStudyModeId], [MapsToAttendanceTypeId], [MapsToAttendancePattern]) VALUES (82, N'Distance learning (with some attendance)', NULL, 8, NULL)

END;
GO

IF NOT EXISTS (SELECT * FROM [dbo].[LearnDirectClassificationToJACS3Mapping])
BEGIN
	PRINT '[Adding JACS 3 to LDCS Mapping]';
	SET NOCOUNT ON;

	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N900', 'A');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N100', 'AA.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N120', 'AA.12');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N200', 'AB.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N700', 'AB.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N220', 'AB.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N290', 'AB.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L231', 'AC.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N110', 'AD.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N215', 'AE.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N210', 'AF.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N690', 'AF.42');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N860', 'AF.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N211', 'AG.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N214', 'AG.13');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G310', 'AG.42');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N213', 'AG.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N600', 'AJ.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N611', 'AJ.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N614', 'AJ.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N612', 'AJ.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N990', 'AK.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N340', 'AK.12');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N400', 'AK.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N421', 'AK.62');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N490', 'AK.63');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N411', 'AK.64');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N412', 'AK.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N413', 'AK.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Z990', 'AK.9');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N300', 'AL.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N310', 'AL.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N390', 'AL.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N322', 'AL.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N323', 'AL.51');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N321', 'AL.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N590', 'AM.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N720', 'AZ.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N722', 'AZ.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N721', 'AZ.51');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N500', 'BA.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N550', 'BA.13');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N510', 'BA.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N560', 'BA.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P210', 'BA.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N561', 'BA.72');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N710', 'BB.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N790', 'BB.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N530', 'BC.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N240', 'BC.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M990', 'BC.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N520', 'BD.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N190', 'BF.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I900', 'CA.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I100', 'CB.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I110', 'CB.12');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I112', 'CB.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I111', 'CB.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I120', 'CB.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G200', 'CB.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I400', 'CB.31');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I530', 'CB.311');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I420', 'CB.312');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I150', 'CB.32');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I260', 'CB.33');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I450', 'CB.34');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J930', 'CB.36');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I200', 'CB.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I240', 'CB.42');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P110', 'CB.43');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F846', 'CB.4322');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I190', 'CB.44');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I322', 'CB.4461');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I320', 'CB.4463');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I230', 'CB.45');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H610', 'CB.51');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I321', 'CB.6112');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I300', 'CB.66');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I140', 'CB.663');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('I310', 'CB.664');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P100', 'CD.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P120', 'CE.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P121', 'CE.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V990', 'DA.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V900', 'DB.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V590', 'DB.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V100', 'DB.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V160', 'DB.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V150', 'DB.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V140', 'DB.24');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V420', 'DB.31');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V430', 'DB.32');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V440', 'DB.33');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V200', 'DB.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V271', 'DB.41');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V210', 'DB.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V214', 'DB.521');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V211', 'DB.522');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V212', 'DB.523');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V213', 'DB.524');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V220', 'DB.62');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V250', 'DB.63');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V230', 'DB.64');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V240', 'DB.65');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V260', 'DB.66');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V290', 'DB.67');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V320', 'DB.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V380', 'DB.71');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V390', 'DB.711');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V330', 'DB.73');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V310', 'DB.741');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V391', 'DB.743');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V322', 'DB.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V323', 'DB.9');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V400', 'DC.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V460', 'DC.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F420', 'DC.27');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V490', 'DC.272');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V450', 'DC.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V244', 'DC.732');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V410', 'DC.733');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V600', 'DD.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V620', 'DD.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V610', 'DD.13');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V627', 'DD.15');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V621', 'DD.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V641', 'DD.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V622', 'DD.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V623', 'DD.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V626', 'DD.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V625', 'DD.51');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V624', 'DD.52');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V690', 'DD.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V650', 'DD.9');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V500', 'DE.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V520', 'DE.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V510', 'DE.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V511', 'DE.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V540', 'DE.42');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q990', 'DF.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L290', 'EA.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L230', 'EA.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L200', 'EA.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L210', 'EA.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L220', 'EA.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L240', 'EA.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L251', 'EA.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L252', 'EA.71');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L250', 'EA.74');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L243', 'EA.9');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L241', 'EA.91');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L100', 'EB.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L113', 'EB.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L190', 'EB.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L130', 'EB.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L120', 'EB.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L140', 'EB.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L110', 'EB.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L160', 'EB.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L360', 'EB.79');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M000', 'EC.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M240', 'EC.12');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M140', 'EC.13');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M200', 'EC.15');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M130', 'EC.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M100', 'EC.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M120', 'EC.31');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M110', 'EC.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M111', 'EC.41');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M112', 'EC.44');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M210', 'EC.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M211', 'EC.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M220', 'EC.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M222', 'EC.71');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M224', 'EC.72');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M223', 'EC.74');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M250', 'EC.742');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M221', 'EC.75');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('M290', 'EC.771');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L390', 'ED.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L300', 'EE.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L350', 'EE.225');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L391', 'EE.226');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L600', 'EE.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L620', 'EE.231');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L610', 'EE.232');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F641', 'EE.234');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L370', 'EE.25');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L320', 'FB.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L321', 'FB.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L322', 'FB.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q200', 'FC.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q290', 'FC.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q320', 'FC.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q321', 'FC.41');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q322', 'FC.48');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R000', 'FC.62');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T920', 'FC.63');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q100', 'FJ.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q120', 'FJ.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q130', 'FJ.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q150', 'FJ.27');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q140', 'FJ.28');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q910', 'FJ.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R711', 'FK.376');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Z900', 'FM.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L242', 'FM.12');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R700', 'FM.13');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L330', 'FM.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q500', 'FM.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q300', 'FM.31');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R600', 'FM.414');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R100', 'FM.421');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R200', 'FM.422');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R900', 'FM.423');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R300', 'FM.424');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R400', 'FM.426');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R730', 'FM.495');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T500', 'FM.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T700', 'FM.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T400', 'FM.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T100', 'FM.732');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T200', 'FM.733');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T300', 'FM.74');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T800', 'FM.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T600', 'FM.9');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q400', 'FN.15');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q310', 'FN.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q610', 'FN.323');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q710', 'FN.325');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q711', 'FN.326');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R910', 'FN.33');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R911', 'FN.332');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T510', 'FN.3323');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R614', 'FN.333');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R210', 'FN.335');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R612', 'FN.3363');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R611', 'FN.337');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R990', 'FN.338');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R410', 'FN.342');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R110', 'FN.343');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R310', 'FN.344');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R510', 'FN.346');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q520', 'FN.35');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q580', 'FN.352');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q570', 'FN.353');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q540', 'FN.354');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q550', 'FN.355');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q530', 'FN.356');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q560', 'FN.357');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R713', 'FN.362');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R712', 'FN.363');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R613', 'FN.3922');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T710', 'FN.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R511', 'FN.55');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('R411', 'FN.56');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T810', 'FN.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T910', 'FN.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T310', 'FN.811');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T610', 'FN.82');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q420', 'FN.8211');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q470', 'FN.822');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q480', 'FN.823');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T110', 'FN.8352');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('T210', 'FN.836');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X000', 'G');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X300', 'GA.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X342', 'GA.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X341', 'GA.12');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X350', 'GA.13');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X330', 'GA.14');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X320', 'GA.15');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X310', 'GA.16');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X390', 'GA.17');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X360', 'GA.19');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L433', 'GA.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X100', 'GA.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X900', 'GA.9');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X190', 'GB.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X140', 'GB.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X141', 'GB.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X142', 'GB.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X150', 'GB.24');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X130', 'GB.31');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X120', 'GB.32');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X110', 'GB.34');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X160', 'GB.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X161', 'GB.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N613', 'GB.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X151', 'GB.723');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X162', 'GC.521');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N224', 'GD.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L550', 'GF.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Y000', 'HC.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('X220', 'HC.73');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q390', 'HD.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G190', 'HD.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L500', 'HF.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W900', 'HJ.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C600', 'HJ.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B300', 'HK.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B344', 'HK.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B346', 'HK.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B390', 'HK.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B341', 'HK.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W000', 'J');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W190', 'JA.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V370', 'JA.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W100', 'JA.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V350', 'JA.331');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W790', 'JA.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W120', 'JB.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W110', 'JB.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W130', 'JB.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W210', 'JB.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W220', 'JB.63');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W615', 'JB.64');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W150', 'JB.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W140', 'JB.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J524', 'JB.81');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W200', 'JC.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W290', 'JC.41');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W250', 'JC.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P130', 'JD.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P131', 'JD.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W160', 'JD.41');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J512', 'JD.42');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J990', 'JD.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W700', 'JF.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W780', 'JG.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W782', 'JG.75');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W721', 'JH.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W720', 'JH.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W722', 'JH.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J530', 'JH.62');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W230', 'JK.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W231', 'JK.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J443', 'JK.13');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W710', 'JK.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W714', 'JK.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W712', 'JK.62');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J441', 'JK.72');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W711', 'JL.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W713', 'JL.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W715', 'JL.9');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W730', 'JP.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W733', 'JP.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W734', 'JP.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W260', 'JP.41');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W761', 'JP.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W751', 'JR.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W270', 'JR.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W770', 'JR.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W753', 'JR.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P900', 'KA.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W213', 'KA.14');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P300', 'KA.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P302', 'KA.221');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P301', 'KA.222');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P303', 'KA.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W212', 'KA.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P390', 'KB.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('Q190', 'KB.621');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W800', 'KC.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P990', 'KC.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W830', 'KC.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W810', 'KC.32');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W820', 'KC.33');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P500', 'KC.35');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P400', 'KH.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P410', 'KH.15');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P413', 'KH.152');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J520', 'KH.31');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W211', 'KH.32');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J523', 'KH.58');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W781', 'KH.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W600', 'KJ.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W640', 'KJ.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W612', 'KJ.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W611', 'KJ.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W690', 'KJ.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J900', 'KJ.25');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P310', 'KJ.26');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P311', 'KJ.261');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('P312', 'KJ.262');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W400', 'LA.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W500', 'LB.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W540', 'LB.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W510', 'LB.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W520', 'LB.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W410', 'LC.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W440', 'LC.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W420', 'LC.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W430', 'LE.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W450', 'LE.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W460', 'LE.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W461', 'LE.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W451', 'LE.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W452', 'LE.52');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W300', 'LF.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W350', 'LF.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W310', 'LF.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W340', 'LG.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J950', 'LJ.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W390', 'LK.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N870', 'MA.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B990', 'MD.9');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H330', 'ME.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N800', 'N');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N861', 'NA.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D630', 'NA.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N862', 'NA.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N890', 'NE.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D632', 'NE.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D690', 'NE.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D610', 'NH.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B400', 'NH.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B410', 'NH.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D620', 'NH.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J700', 'NH.62');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N830', 'NK.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N831', 'NK.13');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N832', 'NK.18');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N850', 'NK.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D445', 'NN.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('A300', 'PB.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B610', 'PB.461');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B130', 'PB.62');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C550', 'PB.621');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B820', 'PB.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B821', 'PB.71');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B822', 'PB.72');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B900', 'PB.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C847', 'PB.81');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B320', 'PC.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B310', 'PC.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B343', 'PC.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B342', 'PC.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B950', 'PD.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B800', 'PE.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B810', 'PE.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B890', 'PE.13');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B830', 'PE.51');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B210', 'PE.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B230', 'PE.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B120', 'PE.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('A400', 'PF.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B790', 'PF.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B840', 'PF.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B500', 'PG.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B510', 'PG.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B520', 'PG.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B590', 'PG.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B700', 'PH.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B710', 'PH.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B714', 'PH.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B712', 'PH.25');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B713', 'PH.26');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B760', 'PH.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B741', 'PH.51');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B720', 'PH.53');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B730', 'PH.55');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B340', 'PJ.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B930', 'PJ.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B160', 'PJ.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B170', 'PJ.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C844', 'PJ.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C800', 'PK.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C842', 'PK.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C810', 'PK.12');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C820', 'PK.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C821', 'PK.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C812', 'PK.32');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C870', 'PK.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C871', 'PK.51');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B345', 'PK.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C850', 'PK.81');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C890', 'PK.82');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C811', 'PK.83');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C815', 'PK.831');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C814', 'PK.832');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C880', 'PK.84');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B940', 'PK.861');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N620', 'PL.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B920', 'PL.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L400', 'PR.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L432', 'PR.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L540', 'PR.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L590', 'PR.31');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L560', 'PR.471');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N223', 'PR.481');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L530', 'PR.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L520', 'PT.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F751', 'QA.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K320', 'QA.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D447', 'QA.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F750', 'QA.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C150', 'QA.32');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F140', 'QA.33');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H221', 'QB.12');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F753', 'QC.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B910', 'QD.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L435', 'QH.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F410', 'QH.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H121', 'QJ.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F000', 'RA.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C000', 'RA.12');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V550', 'RA.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G100', 'RB.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G110', 'RB.14');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G120', 'RB.16');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G140', 'RB.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G300', 'RB.71');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G390', 'RB.716');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G340', 'RB.717');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G320', 'RB.73');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G290', 'RB.743');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G150', 'RB.744');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('G160', 'RB.91');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F300', 'RC.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F343', 'RC.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F310', 'RC.12');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F311', 'RC.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H142', 'RC.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H141', 'RC.24');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H440', 'RC.241');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F390', 'RC.25');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H341', 'RC.252');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F360', 'RC.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F341', 'RC.53');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F340', 'RC.54');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F320', 'RC.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F342', 'RC.62');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F370', 'RC.63');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F351', 'RC.67');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F510', 'RC.72');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F331', 'RC.83');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F350', 'RC.85');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F100', 'RD.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F190', 'RD.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F170', 'RD.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F160', 'RD.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F120', 'RD.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F200', 'RD.51');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F131', 'RD.52');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F110', 'RD.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F180', 'RD.61');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F111', 'RD.62');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F112', 'RD.622');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F150', 'RD.63');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F520', 'RE.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F500', 'RE.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F521', 'RE.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F990', 'RE.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F600', 'RF.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F610', 'RF.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F612', 'RF.221');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F842', 'RF.222');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F621', 'RF.223');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F690', 'RF.232');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F650', 'RF.233');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F643', 'RF.235');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F800', 'RF.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F840', 'RF.42');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L721', 'RF.43');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L700', 'RF.44');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L725', 'RF.441');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('L710', 'RF.45');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F841', 'RF.46');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F761', 'RF.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F760', 'RF.51');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F752', 'RF.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F770', 'RF.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F670', 'RF.9');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H240', 'RG.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F843', 'RG.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F845', 'RG.231');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F844', 'RG.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F890', 'RG.41');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H242', 'RG.71');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C180', 'RH.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C170', 'RH.14');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C190', 'RH.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C100', 'RH.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C191', 'RH.311');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C910', 'RH.3111');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C120', 'RH.3121');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C182', 'RH.313');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C160', 'RH.321');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C162', 'RH.3211');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C161', 'RH.3212');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C400', 'RH.33');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C432', 'RH.331');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C490', 'RH.332');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C420', 'RH.333');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C130', 'RH.34');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C142', 'RH.35');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C720', 'RH.36');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C770', 'RH.361');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B140', 'RH.364');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C111', 'RH.37');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C500', 'RH.38');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C540', 'RH.383');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B110', 'RH.42');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C741', 'RH.463');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C521', 'RH.464');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('B220', 'RH.465');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C200', 'RH.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C750', 'RH.612');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C250', 'RH.62');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C220', 'RH.66');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C300', 'RH.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D322', 'RH.71');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D323', 'RH.72');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C310', 'RH.73');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C110', 'RH.81');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J710', 'RH.822');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J740', 'RH.823');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C510', 'RH.83');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C710', 'RH.861');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H160', 'RH.862');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('C460', 'RH.87');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F162', 'RJ.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D700', 'RK.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D710', 'RK.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D730', 'RK.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D400', 'SA.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D490', 'SA.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D410', 'SC.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D414', 'SC.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D411', 'SC.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D415', 'SD.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D416', 'SD.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K300', 'SE.9');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K310', 'SE.911');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D900', 'SF.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D500', 'SG.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D516', 'SG.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D540', 'SG.13');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D510', 'SG.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D590', 'SG.41');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D515', 'SG.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D421', 'SH.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D462', 'SH.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D423', 'SH.64');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D422', 'SH.71');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D425', 'SH.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D430', 'SJ.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D470', 'SK.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D471', 'SK.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D441', 'SM.15');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D200', 'SN.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D210', 'SN.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D310', 'SN.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D300', 'SN.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D390', 'SN.7');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D990', 'SP.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D434', 'SP.26');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D328', 'SP.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D440', 'SQ.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K990', 'TA.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K290', 'TC.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K400', 'TC.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K410', 'TC.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K440', 'TC.231');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K460', 'TC.24');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K240', 'TC.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K230', 'TC.33');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N234', 'TC.43');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N232', 'TC.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N231', 'TC.612');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H223', 'TC.613');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K100', 'TD.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('V360', 'TD.13');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K110', 'TD.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K190', 'TD.22');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K250', 'TD.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K200', 'TE.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('K220', 'TF.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H200', 'TL.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H290', 'TL.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H220', 'TL.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H123', 'TL.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H230', 'TL.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H231', 'TL.51');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H250', 'TL.63');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F631', 'TL.631');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H210', 'TM.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H660', 'VE.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H661', 'VE.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H131', 'VE.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H672', 'VE.41');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H671', 'VE.42');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H130', 'VE.44');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H190', 'VE.46');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W240', 'VF.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W280', 'VF.12');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J920', 'VF.24');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H150', 'VF.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H300', 'VG.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J940', 'VG.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H700', 'WA.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H711', 'WA.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H890', 'WA.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H790', 'WD.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J590', 'WE.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J310', 'WF.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W750', 'WF.13');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J320', 'WF.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J400', 'WG.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J410', 'WG.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J420', 'WH.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J440', 'WH.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J430', 'WJ.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J490', 'WJ.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J445', 'WJ.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W731', 'WK.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('W732', 'WK.25');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D631', 'WM.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D633', 'WM.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D641', 'WM.26');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D635', 'WM.32');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('D634', 'WM.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H100', 'XA.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H390', 'XA.15');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J511', 'XA.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H311', 'XH.32');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H321', 'XH.33');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H340', 'XH.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J510', 'XH.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H320', 'XH.81');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H620', 'XJ.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H600', 'XJ.14');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H630', 'XK.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H631', 'XK.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H632', 'XK.4');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H690', 'XK.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H611', 'XL.15');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H612', 'XL.53');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H730', 'XL.9');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H641', 'XM.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H644', 'XM.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H642', 'XM.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H400', 'XP.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H410', 'XP.11');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H430', 'XP.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H460', 'XP.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H333', 'XQ.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H500', 'XQ.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H520', 'XQ.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H510', 'XQ.3');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H514', 'XQ.8');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H350', 'XQ.9');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H331', 'XR.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H120', 'XR.213');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H332', 'XT.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J110', 'YA.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('F620', 'YA.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J120', 'YA.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H810', 'YC.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H830', 'YC.2');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H990', 'YC.21');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H831', 'YC.24');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J500', 'YC.33');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J140', 'YC.5');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J100', 'YC.54');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J160', 'YC.621');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H840', 'YC.64');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J200', 'YD.1');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J220', 'YD.23');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J221', 'YD.32');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J960', 'ZM.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N853', 'ZR.6');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('H420', 'ZR.741');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N852', 'ZS.');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('N851', 'ZS.26');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J690', 'ZS.31');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J611', 'ZS.33');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J612', 'ZS.333');
	INSERT INTO [dbo].[LearnDirectClassificationToJACS3Mapping] (JACS3, LearnDirectClassificationRef) VALUES ('J613', 'ZS.334');

END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'BE557F09-A84A-44FF-919C-AB4201DBCB9A')
BEGIN
	PRINT '[Updating Report Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Bulk Upload History: {0:dd/MM/yyyy} to {1:dd/MM/yyyy}'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Report_BulkUploadHistory_Header', @NewText, @NewText
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Report_BulkUploadHistory_Title', @NewText, @NewText
	
	INSERT INTO __RefactorLog (OperationKey) VALUES ('BE557F09-A84A-44FF-919C-AB4201DBCB9A')
END
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '0BFFD861-5802-4DE1-8424-FCFEC32AE42A')
BEGIN
	PRINT '[Updating Bulk Upload Language Fields]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Blank Course Id for provider {0} section {1}';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_Constants_BlankOpportunityId', @NewText, @NewText;
	
	INSERT INTO __RefactorLog (OperationKey) VALUES ('0BFFD861-5802-4DE1-8424-FCFEC32AE42A');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '7FF8B5F5-9834-4800-B562-F04355680743')
BEGIN
	PRINT '[Adding Meta Data Upload Types]';
	SET NOCOUNT ON;

	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (21, 'Frameworks', 'LARS Framework Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (22, 'ProgTypes', 'LARS ProgType Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (23, 'SectorSubjectAreaTier1', 'LARS Sector Subject Tier 1 Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (24, 'SectorSubjectAreaTier2', 'LARS Sector Subject Tier 1 Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (25, 'StandardSectors', 'LARS Standard Sector Code Data');

	INSERT INTO __RefactorLog (OperationKey) VALUES ('7FF8B5F5-9834-4800-B562-F04355680743');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'B145A750-816B-4467-B54F-0BFAA22B6073')
BEGIN
	PRINT '[Removing excess spaces from LARS data]'
	SET NOCOUNT ON

	UPDATE SectorSubjectAreaTier1
	SET SectorSubjectAreaTier1Desc = LTrim(RTrim(SectorSubjectAreaTier1Desc))
		, SectorSubjectAreaTier1Desc2 = LTrim(RTrim(SectorSubjectAreaTier1Desc2))

	UPDATE SectorSubjectAreaTier2
	SET SectorSubjectAreaTier2Desc = LTrim(RTrim(SectorSubjectAreaTier2Desc))
		, SectorSubjectAreaTier2Desc2 = LTrim(RTrim(SectorSubjectAreaTier2Desc2))

	UPDATE ProgType
	SET ProgTypeDesc = LTrim(RTrim(ProgTypeDesc))
		, ProgTypeDesc2 = LTrim(RTrim(ProgTypeDesc2))

	UPDATE Framework
	SET PathwayName = LTrim(RTrim(PathwayName))
		, NasTitle = LTrim(RTrim(NasTitle))

	UPDATE StandardSectorCode
	SET StandardSectorCodeDesc = LTrim(RTrim(StandardSectorCodeDesc))
		, StandardSectorCodeDesc2 = LTrim(RTrim(StandardSectorCodeDesc2))

	UPDATE Standard
	SET StandardName = LTrim(RTrim(StandardName))
		, StandardSectorCode = LTrim(RTrim(StandardSectorCode))
		, URLLink = LTrim(RTrim(URLLink))

	INSERT INTO __RefactorLog (OperationKey) VALUES ('B145A750-816B-4467-B54F-0BFAA22B6073')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'A22FD131-059F-4C83-AFA0-1B4C0150928A')
BEGIN
	PRINT '[Change user permissions for the apprenticeships BETAs]'
	SET NOCOUNT ON

	DECLARE
		@AOU uniqueidentifier = NewId()
		, @AOSU uniqueidentifier = NewId()
		, @APU uniqueidentifier = NewId()
		, @APSU uniqueidentifier = NewId()
		, @OSU uniqueidentifier = (SELECT Id FROM AspNetRoles WHERE Name = 'Organisation Superuser')
		, @OU uniqueidentifier = (SELECT Id FROM AspNetRoles WHERE Name = 'Organisation User')
		, @PSU uniqueidentifier = (SELECT Id FROM AspNetRoles WHERE Name = 'Provider Superuser')
		, @PU uniqueidentifier = (SELECT Id FROM AspNetRoles WHERE Name = 'Provider User')
	

	-- Clone existing roles (which currently include apprenticeship permissions)

	INSERT INTO AspNetRoles
		(Id, Name, Description, LanguageFieldName, UserContextId)
	VALUES
		(@APU,'Provider User (Apprenticeships)','A user who can see and change only provision data for their Provider. Provider Users cannot amend main provider details or manage users other than themselves. This role has access to Apprenticeships.','Account_RoleDescription_ProviderUserApprenticeships',1)

	INSERT INTO AspNetRoles
		(Id, Name, Description, LanguageFieldName, UserContextId)
	VALUES
		(@APSU,'Provider Superuser (Apprenticeships)','A user who can see and change all data for their Provider. This role has access to Apprenticeships','Account_RoleDescription_ProviderSuperuser',1)

	INSERT INTO AspNetRoles
		(Id, Name, Description, LanguageFieldName, UserContextId)
	VALUES
		(@AOSU,'Organisation Superuser (Apprenticeships)','A user who can see and change all data for their Organisation and can additionally see and change all data for any Providers who are members of their Organisation, unless member Providers have specifically forbidden this. This role has access to Apprenticeships','Account_RoleDescription_OrganisationSuperuser',2)

	INSERT INTO AspNetRoles
		(Id, Name, Description, LanguageFieldName, UserContextId)
	VALUES
		(@AOU,'Organisation User (Apprenticeships)','A user who can see and change only provision data for their Organisation and can additionally see and change only provision data for any Providers who are members of their Organisation, unless the member Organisation has forbidden this. Organisation Users cannot amend main provider details or manage users other than themselves. This role has access to Apprenticeships','Account_RoleDescription_OrganisationUser',2)

	-- Clone user permissions for those roles

	INSERT INTO PermissionInRole
		(RoleId, PermissionId)
	SELECT @APU, PermissionId
	FROM PermissionInRole
	WHERE RoleId = @PU

	INSERT INTO PermissionInRole
		(RoleId, PermissionId)
	SELECT @APSU, PermissionId
	FROM PermissionInRole
	WHERE RoleId = @PSU

	INSERT INTO PermissionInRole
		(RoleId, PermissionId)
	SELECT @AOU, PermissionId
	FROM PermissionInRole
	WHERE RoleId = @OU

	INSERT INTO PermissionInRole
		(RoleId, PermissionId)
	SELECT @AOSU, PermissionId
	FROM PermissionInRole
	WHERE RoleId = @OSU

    -- Clone user type in role

	INSERT INTO ProviderUserTypeInRole
		(ProviderUserTypeId, RoleId)
	SELECT ProviderUserTypeId, @APSU
	FROM ProviderUserTypeInRole
	WHERE RoleId = @PSU

	INSERT INTO ProviderUserTypeInRole
		(ProviderUserTypeId, RoleId)
	SELECT ProviderUserTypeId, @APU
	FROM ProviderUserTypeInRole
	WHERE RoleId = @PU

	INSERT INTO ProviderUserTypeInRole
		(ProviderUserTypeId, RoleId)
	SELECT ProviderUserTypeId, @AOSU
	FROM ProviderUserTypeInRole
	WHERE RoleId = @OSU

	INSERT INTO ProviderUserTypeInRole
		(ProviderUserTypeId, RoleId)
	SELECT ProviderUserTypeId, @AOU
	FROM ProviderUserTypeInRole
	WHERE RoleId = @OU

	-- Remove apprenticeship permissions from existing roles

	DELETE FROM PermissionInRole
	WHERE
		PermissionId in (59, 60, 61, 62, 63, 64, 65, 66, 67, 68)
		AND RoleId IN (@PSU, @PU, @OSU, @OU)

	-- Update add/edit user configuration

	UPDATE ConfigurationSettings
		SET Value = Value + ';Provider Superuser (Apprenticeships);Provider User (Apprenticeships);Organisation Superuser (Apprenticeships);Organisation User (Apprenticeships)'
	WHERE
		Name IN (
			'AdminUserCanAddRoles'
			, 'ProviderContextCanAddRoles'
			, 'OrganisationContextCanAddRoles'
			, 'AdminContextCanAddRoles'
			)

	INSERT INTO __RefactorLog (OperationKey) VALUES ('A22FD131-059F-4C83-AFA0-1B4C0150928A')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '252E89AA-4A12-4837-8D0E-FB81BF295EA2')
BEGIN
	PRINT '[Updating Widget Catalogue Information]'
	SET NOCOUNT ON

	 UPDATE ConfigurationSettings
	 SET Value = 'Provider Superuser;Provider User;Provider Superuser (Apprenticeships);Provider User (Apprenticeships)'
	 WHERE Name = 'ProviderContextCanAddRoles'

	 UPDATE ConfigurationSettings
	 SET Value = 'Organisation Superuser;Organisation User;Organisation Superuser (Apprenticeships);Organisation User (Apprenticeships)'
	 WHERE Name = 'OrganisationContextCanAddRoles'

	INSERT INTO __RefactorLog (OperationKey) VALUES ('252E89AA-4A12-4837-8D0E-FB81BF295EA2')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '8A998259-641D-469D-B927-735BEBF3CB53')
BEGIN
	PRINT '[Updating Delivery Mode Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Create a New Delivery Mode'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'DeliveryMode_Create_Header', @NewText, @NewText

	SET @NewText = 'Edit Delivery Mode'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'DeliveryMode_Edit_Header', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('8A998259-641D-469D-B927-735BEBF3CB53')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '6EEC217E-40DD-4B77-B694-77CDC33A90CD')
BEGIN
	PRINT '[Upgrading DAS Private Beta Users]'
	SET NOCOUNT ON

	DECLARE
		@APSU uniqueidentifier = (SELECT Id FROM AspNetRoles WHERE Name = 'Provider Superuser (Apprenticeships)')
		, @APU uniqueidentifier = (SELECT Id FROM AspNetRoles WHERE Name = 'Provider User (Apprenticeships)')
		, @PSU uniqueidentifier = (SELECT Id FROM AspNetRoles WHERE Name = 'Provider Superuser')
		, @PU uniqueidentifier = (SELECT Id FROM AspNetRoles WHERE Name = 'Provider User')

	DECLARE @Users TABLE (UserId uniqueidentifier)

	INSERT INTO @Users
	SELECT UserId
	FROM AspNetUsers u 
		JOIN ProviderUser pu ON pu.UserId = u.Id
		JOIN Provider p ON p.ProviderId = pu.ProviderId
	WHERE p.UKPRN IN (
		10031241 -- Aspire Achieve Advance Ltd
		, 10006442 -- Birmingham Metropolitan College
		, 10000488 -- B-Skill
		, 10001004 -- Burton and South Derbyshire College
		, 10001602 -- CTS Training
		, 10007924 -- Dudley College
		, 10025384 -- EEF
		, 10044028 -- EQL Solutions
		, 10002370 -- Exeter College
		, 10007938 -- Grimsby Institute of Further and Higher Education
		, 10007949 -- Huntingdon Regional College
		, 10034309 -- Interserve Learning and Employment (ESG Skills)
		, 10004601 -- Newcastle City Learning
		, 10004723 -- North West Training Council 
		, 10003375 -- QA Ltd
		, 10006463 -- Swindon College
		, 10007872 -- SWRAC
		, 10005998 -- Trafford College
		, 10001282 -- University of Northumbria at Newcastle
		, 10007159 -- University of Sunderland
		, 10007859 -- Warwickshire College Group
		, 10007407 -- West Cheshire College
		, 10007405 -- YMCA
	)

	UPDATE AspnetUserRoles
	SET RoleId = @APSU
	WHERE UserId IN (
			SELECT UserId FROM @Users
		)
		AND RoleId = @PSU

	UPDATE AspnetUserRoles
	SET RoleId = @APU
	WHERE UserId IN (
			SELECT UserId FROM @Users
		)
		AND RoleId = @PU

	INSERT INTO __RefactorLog (OperationKey) VALUES ('6EEC217E-40DD-4B77-B694-77CDC33A90CD')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'AB9C9238-9A24-4011-B169-BD10BF0D14C3')
BEGIN
	PRINT '[Updating Language Fields]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Catchment Radius (miles)';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditDeliveryLocationViewModel_DisplayName_Radius', @NewText, @NewText;

	SET @NewText = 'Create and Add Another Delivery Location';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'DeliveryLocation_EditDeliveryLocation_CreateAndAddDeliveryLocation', @NewText, @NewText;
	
	INSERT INTO __RefactorLog (OperationKey) VALUES ('AB9C9238-9A24-4011-B169-BD10BF0D14C3');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'FA158CF5-FD58-4E44-9FA2-771033CBC0C4')
BEGIN
	PRINT '[Updating Language Fields]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Are you sure you would like to delete this Location?  Any apprenticeship delivery locations attached to this Location will be deleted.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Location_Edit_DeleteLocationWarning', @NewText, @NewText;
	
	INSERT INTO __RefactorLog (OperationKey) VALUES ('FA158CF5-FD58-4E44-9FA2-771033CBC0C4');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '3BA6E465-C1FB-4040-A7A2-DB392698F76E')
BEGIN
	PRINT '[Updating Language Fields]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Apprenticeship Delivery';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Create_MarketingInformationHeader', @NewText, @NewText;
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Edit_MarketingInformationHeader', @NewText, @NewText;
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditProviderModel_DisplayName_MarketingInformation', @NewText, @NewText;
	
	INSERT INTO __RefactorLog (OperationKey) VALUES ('3BA6E465-C1FB-4040-A7A2-DB392698F76E');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'D8C8DADB-A733-47F3-8CDC-EFE9D0B34B79')
BEGIN
	PRINT '[Setting NCS and Public API Configuration Settings]';
	SET NOCOUNT ON;

	UPDATE [dbo].[ConfigurationSettings] SET Value = (SELECT CASE WHEN IsEnabled = 1 THEN 'true' ELSE 'false' END FROM [Search].[DataExportConfiguration]) WHERE Name = 'NCSExportEnabled';
	UPDATE [dbo].[ConfigurationSettings] SET Value = (SELECT ThresholdPercent FROM [Search].[DataExportConfiguration]) WHERE Name = 'NCSExportThresholdCheckPercent';
	UPDATE [dbo].[ConfigurationSettings] SET Value = (SELECT CASE WHEN OverrideThreshold = 1 THEN 'true' ELSE 'false' END FROM [Search].[DataExportConfiguration]) WHERE Name = 'NCSOverrideThresholdCheck';
	UPDATE [dbo].[ConfigurationSettings] SET Value = (SELECT CASE WHEN IncludeUCASData = 1 THEN 'true' ELSE 'false' END FROM [Search].[DataExportConfiguration]) WHERE Name = 'NCSExportIncludeUCASData';
	
	INSERT INTO __RefactorLog (OperationKey) VALUES ('D8C8DADB-A733-47F3-8CDC-EFE9D0B34B79');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '75400A4C-1BC2-4E95-AFC7-DBEA6B13520D')
BEGIN
	PRINT '[Adding Meta Data Upload Types]';
	SET NOCOUNT ON;

	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (26, 'UCASQualifications', 'UCAS Qualifications Data');

	INSERT INTO __RefactorLog (OperationKey) VALUES ('75400A4C-1BC2-4E95-AFC7-DBEA6B13520D');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '5853C789-6057-4550-909F-ABBA96580E34')
BEGIN
	PRINT '[Delete reundant Bulk Upload email templates]';
	SET NOCOUNT ON;

	Delete from EmailTemplate where EmailTemplateId in (7,8,9);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('5853C789-6057-4550-909F-ABBA96580E34');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '95289E7F-C4A7-4967-8D0A-DCD528EA7EBE')
BEGIN
	PRINT '[Updating Bulk Upload help page to make it course specific and adding Bulk Upload help page for apprenticeships]'
	SET NOCOUNT ON

	--If the bulk upload help content has never been altered from the initial release, create a new version indicating that it is now 
	--obsolete and has been divided into Course and Apprenticeship versions. If it has been altered by the content team. we should leave it alone.
	if (select count(*) from content where path = 'Help/BulkUpload') = 1
	begin
		update content set RecordStatusId = 3 where path = 'Help/BulkUpload'

		insert into content ([version],[path],title,body,scripts,styles,summary,UserContext,Embed,RecordStatusId,LanguageId,CreatedByUserId,CreatedDateTimeUtc,ModifiedByUserId,ModifiedDateTimeUtc)
		select 2,path,'Detailed Instructions for Bulk Upload - now obsolete, divided into separate content items for Courses and Apprenticeships',body,scripts,styles,null,UserContext,Embed,2,LanguageId,'24314672-f766-47f1-98cb-ad9fc49f6e9d',GETUTCDATE(),'24314672-f766-47f1-98cb-ad9fc49f6e9d',GETUTCDATE()
		from content where path = 'Help/BulkUpload'
	end


	--Insert new help content for courses
	INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'Help/BulkUploadCourses'
           , 'Detailed Instructions for Bulk Upload of Course Data'
           , '<div id="divBulkUploadHelp">
    <p>Bulk upload allows you to easily send all your provision in a single file. The Comma Separated Values (CSV) format used is simply a series of rows and columns, and can be exported from many information management systems, or simply prepared using spreadsheet programs such as Microsoft Excel. Preparing your first bulk upload will take some work, but once you’ve set it up, it is a simple way to keep all your information up to date.</p>
    <p>If you’re a provider only uploading provision run by you, please note that each time you upload, <strong><i>all existing provision will be deleted and replaced with what you’ve uploaded</i></strong>. If you’re uncertain, you can always back up your existing provision using the “Download CSV” function on the Bulk Upload page. This gives you file containing all your provision in bulk upload format that can be uploaded at any time.</p>
    <p><strong>Organisations</strong></p>
    <p>If you’re a provider who is a member of one or more Organisations, you can upload provision that you run for that Organisation or promote under that Organisation’s brand. The Opportunity fields OFFERED_BY and DISPLAY_NAME are used for this. OFFERED_BY should be set to the ID of who has the contract with the Agency for this opportunity, and DISPLAY_NAME should be set to what name you wish learners to see on the National Careers Service site. If they are different, you can choose to have both searchable if you wish using BOTH_SEARCHABLE. You can see all Organisation IDs on the Organisations screen.</p>
    <p>Note if you do not include any sections for a particular Provider in an upload file, any existing provision you run for that Provider will be preserved. Only if you include sections for the Provider will that Provider’s provision be affected. This also applies if you only upload provision offered by yourself directly &ndash; any existing provision for Providers will be preserved. If you include sections for a Provider which contain no records, all that Provider’s data will be deleted, although you will be asked to confirm the deletion.</p>
    <p>If you’re an Organisation superuser, you can upload provision for your member providers in the same way they do (if they have allowed you to do so), except you must preface each bulk upload file with the Provider ID. <a target="_blank" href="/Content/SampleData/multi_provider_template.csv">Click here to download an example</a>. All member Provider IDs can be viewed on the Organisation home screen. You can put one, some or all of your providers in a single file. </p>
    <h2>Getting started</h2>
    <p><strong>Step 1: Read the Data Standards &amp; Help Guide</strong></p>
    <p>The <a target="_blank" href="/Content/Help/Course Directory Provider Portal Data Standards.pdf">Data Standards</a> list the name and format of every piece of data used in the Course Directory. Not all fields are mandatory, but the more you supply, the more useful your data will be to learners. The <a target="_blank" href="/Content/Help/Course Directory Provider Portal Help Guide 2015.pdf">Help Guide</a> contains instructions on how to use Bulk Upload. For fields such as Study Mode, you will need to supply a code rather than the value itself. For example, instead of "Full Time", your file will contain "SM1", A complete list of fields requiring codes and the codes themselves can be found in Appendix 2 of the <a target="_blank" href="/Content/Help/Course Directory Provider Portal Help Guide 2015.pdf">Help Guide</a>.</p>
    <p><strong>Step 2: Understand the file format and create your file</strong></p>
    <p>Bulk upload files are in Comma Separated Values (CSV) format with a defined column and section order. You can review a <a target="_blank" href="/Content/SampleData/blank_file.csv">blank example with headers only</a> and an <a target="_blank" href="/Content/SampleData/csv_template_with_values.csv">example with test data</a>. This page always has the latest versions. Previous versions may not work. Take careful note of the following:</p>
    <div>
        <ul>
            <li>Your file must have four sections in the expected order - Provider, then Venues, then Courses and then Opportunities. If you’re an Organisation superuser, you simply repeat this format prefacing each Provider with their ID.</li>
            <li>Headers for each section should read exactly as they are in the template, including the * where it is shown.</li>
            <li>Each section should have every item of that type: Venues section has ALL venues, Courses section has ALL courses, and so on.</li>
            <li>Each Venue and Course should have a unique identifier, stored in the VENUE_ID* and COURSE_ID* columns in the relevant section. Neither field is in the Data Standards, and will be discarded after the file is loaded - they’re simply to allow successful processing of your file. This may be any identifier you choose. If you’ve already got a unique identifier in your own system, use this; if not, simple numbers will do.</li>
            <li>To link an OPPORTUNITY to a venue, please put the venue’s ID in the VENUE_ID/REGION_NAME column. Alternatively, you may put a town, county or region name in this column. Please see the Help Guide for more information.</li>
            <li>The column OFFERED_BY is used to indicate who has the contract with the Agency the opportunity is run under &ndash; this can be the provider themselves or an Organisation of which the Provider is a member. If this column is left blank, the opportunity is assumed to be offered under the Provider’s direct contract with the Agency.</li>
            <li>The column DISPLAY_NAME is used to indicate which name the learner will see for this Opportunity. This can be the Provider themselves or an Organisation of which the Provider is a member. If this ID is different to the one in OFFERED_BY, you can further choose whether you want both names searchable by the learners using BOTH_SEARCHABLE.</li>
            <li><strong>All columns except OFFERED_BY, DISPLAY_NAME and BOTH_SEARCHABLE need to be present in the file (even if they have no data in them) and in the same order as the example files.</strong></li>
        </ul>
    </div>
    <p><strong>Step 3: Save &amp; upload your file</strong></p>
    <p>Your file may be named anything, but must have the extension .csv. Once uploaded it will be queued up for processing. The time to process the file will depend on the size of the file, complexity of the information and the time of day.</p>
    <p><strong>Step 4: Review errors and re-upload if necessary</strong></p>
    <p>If your file contains errors, the entire file will be rejected and no data will be published. You will likely have errors the first time you try an upload, and will likely need several attempts to get it exactly right. The errors are reported on the Provider Portal. The error list is broken down into Provider, Venue, Course and Opportunity errors and where possible a line number is given. You will be sent an email to confirm the status of the upload..</p>

    <p><a target="_parent" href="/BulkUpload/Courses">Return to the main bulk upload page.</a></p>
    </div>'
           , null
           , '#divBulkUploadHelp a {color: red; font-weight: bold;}'
           , 'Initial content'
           , 127 /* All user contexts */
           , 0 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )



	--Insert new help content for apprenticeships
	INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'Help/BulkUploadApprenticeships'
           , 'Detailed Instructions for Bulk Upload of Apprenticeship Data'
           , '<div id="divBulkUploadHelp">
    <p>Bulk upload allows you to easily send all your provision in a single file. The Comma Separated Values (CSV) format used is simply a series of rows and columns, and can be exported from many information management systems, or simply prepared using spreadsheet programs such as Microsoft Excel. Preparing your first bulk upload will take some work, but once you’ve set it up, it is a simple way to keep all your information up to date.</p>
    <p>If you’re a provider only uploading provision run by you, please note that each time you upload, <strong><i>all existing provision will be deleted and replaced with what you’ve uploaded</i></strong>. If you’re uncertain, you can always back up your existing provision using the “Download CSV” function on the Bulk Upload page. This gives you file containing all your provision in bulk upload format that can be uploaded at any time.</p>
    <p><strong>Organisations</strong></p>
    <p>If you do not include any sections for a particular Provider in an upload file, any existing provision you run for that Provider will be preserved. Only if you include sections for the Provider will that Provider’s provision be affected. This also applies if you only upload provision offered by yourself directly &ndash; any existing provision for Providers will be preserved. If you include sections for a Provider which contain no records, all that Provider’s data will be deleted, although you will be asked to confirm the deletion.</p>
    <p>If you’re an Organisation superuser, you can upload provision for your member providers in the same way they do (if they have allowed you to do so), except you must preface each bulk upload file with the Provider ID. <a target="_blank" href="/Content/SampleData/multi_provider_apprenticeship_template.csv">Click here to download an example</a>. All member Provider IDs can be viewed on the Organisation home screen. You can put one, some or all of your providers in a single file. </p>
    <h2>Getting started</h2>
    <p><strong>Step 1: Read the Data Standards &amp; Help Guide</strong></p>
    <p>The <a target="_blank" href="/Content/Help/Course Directory Provider Portal Data Standards.pdf">Data Standards</a> list the name and format of every piece of data used in the Course Directory. Not all fields are mandatory, but the more you supply, the more useful your data will be to learners. The <a target="_blank" href="/Content/Help/Course Directory Provider Portal Help Guide 2015.pdf">Help Guide</a> contains instructions on how to use Bulk Upload. For the Delivery Mode field, you will need to supply a code rather than the value itself. For example, instead of "100% Employer Based", your file will contain "DM1", A complete list of fields requiring codes and the codes themselves can be found in Appendix 2 of the <a target="_blank" href="/Content/Help/Course Directory Provider Portal Help Guide 2015.pdf">Help Guide</a>.</p>
    <p><strong>Step 2: Understand the file format and create your file</strong></p>
    <p>Bulk upload files are in Comma Separated Values (CSV) format with a defined column and section order. You can review a <a target="_blank" href="/Content/SampleData/blank_apprenticeships_file.csv">blank example with headers only</a> and an <a target="_blank" href="/Content/SampleData/csv_apprenticeship_template_with_values.csv">example with test data</a>. This page always has the latest versions. Previous versions may not work. Take careful note of the following:</p>
    <div>
        <ul>
            <li>Your file must have four sections in the expected order - Provider, then Location, then Apprenticeships and then Delivery Locations. If you’re an Organisation superuser, you simply repeat this format prefacing each Provider with their ID.</li>
            <li>Headers for each section should read exactly as they are in the template, including the * where it is shown.</li>
            <li>Each section should have every item of that type: Locations section has ALL locations, Apprenticeships section has ALL apprenticeships, and so on.</li>
            <li>Each Location and Apprenticeship should have a unique identifier, stored in the LOCATION_ID* and APPRENTICESHIP_ID* columns in the relevant section. Neither field is in the Data Standards, and will be discarded after the file is loaded - they’re simply to allow successful processing of your file. This may be any identifier you choose. If you’ve already got a unique identifier in your own system, use this; if not, simple numbers will do.</li>
            <li>To link a DELIVERY LOCATION to an apprenticeship, please put the apprenticeships’s ID in the APPRENTICESHIP_ID* column.</li>
			<li>To link a DELIVERY LOCATION to a location, please put the location’s ID in the LOCATION_ID* column.</li>
            <li><strong>All columns need to be present in the file (even if they have no data in them) and in the same order as the example files.</strong></li>
        </ul>
    </div>
    <p><strong>Step 3: Save &amp; upload your file</strong></p>
    <p>Your file may be named anything, but must have the extension .csv. Once uploaded it will be queued up for processing. The time to process the file will depend on the size of the file, complexity of the information and the time of day.</p>
    <p><strong>Step 4: Review errors and re-upload if necessary</strong></p>
    <p>If your file contains errors, the entire file will be rejected and no data will be published. You will likely have errors the first time you try an upload, and will likely need several attempts to get it exactly right. The errors are reported on the Provider Portal. The error list is broken down into Provider, Location, Apprenticeship and Delivery Location errors and where possible a line number is given. You will be sent an email to confirm the status of the upload.</p>

    <p><a target="_parent" href="/BulkUpload/Apprenticeships">Return to the main bulk upload page.</a></p>
    </div>'
           , null
           , '#divBulkUploadHelp a {color: red; font-weight: bold;}'
           , 'Initial content'
           , 127 /* All user contexts */
           , 0 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )



	INSERT INTO __RefactorLog (OperationKey) VALUES ('95289E7F-C4A7-4967-8D0A-DCD528EA7EBE')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '2ABDACDD-7D96-46D4-B6F4-9B72701BA4A5')
BEGIN
	PRINT '[Updating Language Fields As Requested By DAS]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Live Framework / Standard Name on LARS';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_DisplayName_FrameworkOrStandard', @NewText, @NewText;
	
	SET @NewText = 'Your Apprenticeship Information for Employers';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_DisplayName_MarketingInformation', @NewText, @NewText;

	SET @NewText = 'Your Website Page About the Apprenticeship';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_DisplayName_Url', @NewText, @NewText;

	SET @NewText = 'Your Contact Email for Apprenticeship Information';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_DisplayName_ContactEmail', @NewText, @NewText;

	SET @NewText = 'Your Contact Telephone for Apprenticeship Information';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_DisplayName_ContactTelephone', @NewText, @NewText;

	SET @NewText = 'Your Website "Contact Us" Page for Apprenticeship Information';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_DisplayName_ContactWebsite', @NewText, @NewText;

	SET @NewText = '<strong>Warning:</strong> this apprenticeship is currently PENDING as it does not have any delivery locations. It will not be viewable to employers on the website.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_EditApprenticeship_ApprenticeshipPendingWarning', @NewText, @NewText;

	SET @NewText = 'Enter information about how your organisation delivers this apprenticeship that employers would find useful (max 1500 characters).';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_Description_MarketingInformation', @NewText, @NewText;

	SET @NewText = 'Enter a link to an employer focused page on your organisation''s website: ideally about: 1) this specific apprenticeship or 2) your apprenticeships in general or 3) your home page.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_Description_Url', @NewText, @NewText;

	SET @NewText = 'Enter your organisation''s email address (not named individuals) for handling employer queries about this apprenticeship.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_Description_ContactEmail', @NewText, @NewText;

	SET @NewText = 'Enter your organisation''s telephone number for handling employer queries about this apprenticeship.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_Description_ContactTelephone', @NewText, @NewText;

	SET @NewText = 'Enter a link to a "Contact us" page on your organisation''s website where an employer can enquire about this apprenticeship.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_Description_ContactWebsite', @NewText, @NewText;

	SET @NewText = 'Please include the full URL including http://www';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Location_CreateFromDialog_WebsiteNotValid', @NewText, @NewText;

	SET @NewText = 'Please include the full URL including http://www';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Location_Create_WebsiteNotValid', @NewText, @NewText;

	SET @NewText = 'Please include the full URL including http://www';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Location_Edit_WebsiteNotValid', @NewText, @NewText;

	SET @NewText = 'Enter a unique name of the location where training takes place.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditLocationModel_Description_LocationName', @NewText, @NewText;

	SET @NewText = 'Enter a phone number for general enquiries about this location.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditLocationModel_Description_Telephone', @NewText, @NewText;

	SET @NewText = 'Enter one main email address for enquiries about this location.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditLocationModel_Description_Email', @NewText, @NewText;

	SET @NewText = 'Enter a link for general enquiries about this location if available.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditLocationModel_Description_Website', @NewText, @NewText;

	SET @NewText = 'Your Generic Apprenticeship Information for Employers';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditProviderModel_DisplayName_MarketingInformation', @NewText, @NewText;

	SET @NewText = 'Enter general information about how your organisation delivers apprenticeships that employers would find useful (max 500 characters).';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditProviderModel_Description_MarketingInformation', @NewText, @NewText;

	SET @NewText = 'Enter a postcode and click Find Address, then select your address from the drop down. You can also just enter a postcode and leave the rest blank.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'LocationAddressViewModel_Description_AddressLine1', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('2ABDACDD-7D96-46D4-B6F4-9B72701BA4A5');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'CF994FE4-2268-4AAD-8EFE-C904235F477A')
BEGIN
	PRINT '[Updating Language Fields For Marketing Information]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'The maximum length of {0} is 500 characters.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditProviderModel_StringLength_MarketingInformation', @NewText, @NewText;
	
	SET @NewText = 'The maximum length of {0} is 1500 characters.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_StringLength_MarketingInformation', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('CF994FE4-2268-4AAD-8EFE-C904235F477A');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'F4BE66FA-002C-478A-9210-40AF48F5CB41')
BEGIN
	PRINT '[Update Apprenticeship Bulk Upload Help Content]';
	SET NOCOUNT ON;

	UPDATE [dbo].[Content]
	SET Body = '<div id="divBulkUploadHelp">
    <p>Bulk upload allows you to easily send all your provision in a single file. The Comma Separated Values (CSV) format used is simply a series of rows and columns, and can be exported from many information management systems, or simply prepared using spreadsheet programs such as Microsoft Excel. Preparing your first bulk upload will take some work, but once you’ve set it up, it is a simple way to keep all your information up to date.</p>
	<p>A bulk upload of your apprenticeship data will not change any course data you have in the Course Directory. There is a separate set of detailed instructions on how to bulk upload course data.</p>
    <p>Please note that each time you upload, <strong><i>all existing provision will be deleted and replaced with what you’ve uploaded</i></strong>. If you’re uncertain, you can always back up your existing provision using the “Download CSV” function on the Bulk Upload page. This gives you file containing all your provision in bulk upload format that can be uploaded at any time.</p>
    <p><strong>Organisations</strong></p>
    <p>If you do not include any sections for a particular Provider in an upload file, any existing provision you run for that Provider will be preserved. Only if you include sections for the Provider will that Provider’s provision be affected. This also applies if you only upload provision offered by yourself directly &ndash; any existing provision for Providers will be preserved. If you include sections for a Provider which contain no records, all that Provider’s data will be deleted, although you will be asked to confirm the deletion.</p>
    <p>If you’re an Organisation superuser, you can upload provision for your member providers in the same way they do (if they have allowed you to do so), except you must preface each bulk upload file with the Provider ID. <a target="_blank" href="/Content/SampleData/multi_provider_apprenticeship_template.csv">Click here to download an example</a>. All member Provider IDs can be viewed on the Organisation home screen. You can put one, some or all of your providers in a single file. </p>
    <h2>Getting started</h2>
    <p><strong>Step 1: Read the Data Standards &amp; Help Guide</strong></p>
    <p>The <a target="_blank" href="/Content/Help/Course Directory Provider Portal Data Standards.pdf">Data Standards</a> list the name and format of every piece of data used in the Course Directory. Not all fields are mandatory, but the more you supply, the more useful your data will be to employers. The <a target="_blank" href="/Content/Help/Course Directory Provider Portal Help Guide 2015.pdf">Help Guide</a> contains instructions on how to use Bulk Upload. For the Delivery Mode field, you will need to supply a code rather than the value itself. For example, instead of "100% Employer Based", your file will contain "DM1", A complete list of fields requiring codes and the codes themselves can be found in Appendix 2 of the <a target="_blank" href="/Content/Help/Course Directory Provider Portal Help Guide 2015.pdf">Help Guide</a>.</p>
    <p><strong>Step 2: Understand the file format and create your file</strong></p>
    <p>Bulk upload files are in Comma Separated Values (CSV) format with a defined column and section order. You can review a <a target="_blank" href="/Content/SampleData/blank_apprenticeships_file.csv">blank example with headers only</a> and an <a target="_blank" href="/Content/SampleData/csv_apprenticeship_template_with_values.csv">example with test data</a>. This page always has the latest versions. Previous versions may not work. Take careful note of the following:</p>
    <div>
        <ul>
            <li>Your file must have four sections in the expected order - Provider, then Location, then Apprenticeships and then Delivery Locations. If you’re an Organisation superuser, you simply repeat this format prefacing each Provider with their ID.</li>
            <li>Headers for each section should read exactly as they are in the template, including the * where it is shown.</li>
            <li>Each section should have every item of that type: Locations section has ALL locations, Apprenticeships section has ALL apprenticeships, and so on.</li>
            <li>Each Location and Apprenticeship should have a unique identifier, stored in the LOCATION_ID* and APPRENTICESHIP_ID* columns in the relevant section. Neither field is in the Data Standards, and will be discarded after the file is loaded - they’re simply to allow successful processing of your file. This may be any identifier you choose. If you’ve already got a unique identifier in your own system, use this; if not, simple numbers will do.</li>
            <li>To link a DELIVERY LOCATION to an apprenticeship, please put the apprenticeship’s ID in the APPRENTICESHIP_ID* column.</li>
			<li>To link a DELIVERY LOCATION to a location, please put the location’s ID in the LOCATION_ID* column.</li>
            <li><strong>All columns need to be present in the file (even if they have no data in them) and in the same order as the example files.</strong></li>
        </ul>
    </div>
    <p><strong>Step 3: Save &amp; upload your file</strong></p>
    <p>Your file may be named anything, but must have the extension .csv. Once uploaded it will be queued up for processing. The time to process the file will depend on the size of the file, complexity of the information and the time of day.</p>
    <p><strong>Step 4: Review errors and re-upload if necessary</strong></p>
    <p>If your file contains errors, the entire file will be rejected and no data will be published. You will likely have errors the first time you try an upload, and will likely need several attempts to get it exactly right. The errors are reported on the Provider Portal. The error list is broken down into Provider, Location, Apprenticeship and Delivery Location errors and where possible a line number is given. You will be sent an email to confirm the status of the upload.</p>

    <p><a target="_parent" href="/BulkUpload/Apprenticeships">Return to the main bulk upload page.</a></p>
    </div>',
		[ModifiedDateTimeUtc] = GetUtcDate()		
	WHERE [Path] = 'Help/BulkUploadApprenticeships'
		AND [Version] = 1;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('F4BE66FA-002C-478A-9210-40AF48F5CB41');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '4CA701F7-5BE4-4947-A8C7-A80D3E8987B1')
BEGIN
	PRINT '[Updating Apprenticeship Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'Your Website "Contact Us" Page for Apprenticeship Information'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_Description_ContactWebsite', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('4CA701F7-5BE4-4947-A8C7-A80D3E8987B1')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'A84D396A-22C7-4776-A620-90A104083293')
BEGIN
	PRINT '[Updating Course Instance Creating User IDs]'
	SET NOCOUNT ON

	UPDATE CourseInstance
	SET CreatedByUserId = u.Id
	FROM AspNetUsers u
	WHERE Convert(nvarchar(32), u.LegacyUserId) = CourseInstance.CreatedByUserId

	INSERT INTO __RefactorLog (OperationKey) VALUES ('A84D396A-22C7-4776-A620-90A104083293')
END
GO
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'A68D482A-5F29-400C-863F-189479234069')
BEGIN
	PRINT '[Adding Bulk Upload Threshold for Apprenticeships]'
	SET NOCOUNT ON

	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) VALUES ('BulkUploadThresholdApprenticeshipAcceptablePercent', (SELECT Value FROM ConfigurationSettings WHERE Name = 'BulkUploadThresholdAcceptablePercent'), '90', 'System.Int32', 'During bulk upload of apprenticeships if newly inserted data goes below this limit, need to have stop the upload process.', 0);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('A68D482A-5F29-400C-863F-189479234069')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '3096C0D6-9DBE-4222-883E-80D22376C2BA')
BEGIN
	PRINT '[Updating Language Fields As Requested By DAS]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Your Apprenticeship Website Page';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_DisplayName_Url', @NewText, @NewText;

	SET @NewText = 'Your Apprenticeship Contact Email';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_DisplayName_ContactEmail', @NewText, @NewText;

	SET @NewText = 'Your Apprenticeship Contact Telephone';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_DisplayName_ContactTelephone', @NewText, @NewText;

	SET @NewText = 'Your Apprenticeship Website "Contact Us" Page';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_DisplayName_ContactWebsite', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('3096C0D6-9DBE-4222-883E-80D22376C2BA');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '50FC8A79-2A8E-4928-9D27-C2BF7B97B4F8')
BEGIN
	PRINT '[Updating Bulk Upload Radius Error Message]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Invalid Radius. Radius must be a whole number of miles between {2} and {3}. Section : {0}, provider : {1}.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_Constants_InvalidRadius', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('50FC8A79-2A8E-4928-9D27-C2BF7B97B4F8')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '3E66C804-266D-4486-9D69-9FDF65EEEFE9')
BEGIN
	PRINT '[Updating Create Location Title]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Create New Location';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Shared_AddLocationScript_AddLocation', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('3E66C804-266D-4486-9D69-9FDF65EEEFE9')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'B5E9CAD4-B882-4614-91E7-166E3AB57459')
BEGIN
	PRINT '[Removing AddEdit... Permissions from Admin User]';
	SET NOCOUNT ON;

	DELETE FROM dbo.PermissionInRole 
	WHERE RoleId = (SELECT Id FROM dbo.AspNetRoles WHERE Name = 'Admin user')
		AND PermissionId IN (SELECT PermissionId FROM dbo.Permission WHERE PermissionName IN ('CanAddOrganisation', 'CanManageOrganisationProviderMembership', 'CanManageProviderOrganisationMembership', 'CanManuallyAuditCourses', 'CanAddProviderLocation', 'CanEditProviderLocation', 'CanDeleteProviderLocation', 'CanAddProviderApprenticeship', 'CanEditProviderApprenticeship', 'CanDeleteProviderApprenticeship', 'CanBulkUploadOrganisationApprenticeshipFiles', 'CanBulkUploadProviderApprenticeshipFiles'));

	INSERT INTO __RefactorLog (OperationKey) VALUES ('B5E9CAD4-B882-4614-91E7-166E3AB57459');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '2D595D1B-750E-4D02-B78C-77CAB379DEA8')
BEGIN
	PRINT '[Removing Tooltop for Provider Marketing Information Field]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = '';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditProviderModel_Description_MarketingInformation', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('2D595D1B-750E-4D02-B78C-77CAB379DEA8');
END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'E322767F-3F2B-4791-BEBC-B69CFA388868')
BEGIN
	PRINT '[Changing Marketing Information Field Lengths]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Enter information about how your organisation delivers this apprenticeship that employers would find useful (max 750 characters).';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_Description_MarketingInformation', @NewText, @NewText;

	SET @NewText = 'The maximum length of {0} is 750 characters.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditApprenticeshipViewModel_StringLength_MarketingInformation', @NewText, @NewText;
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditProviderModel_StringLength_MarketingInformation', @NewText, @NewText;

	SET @NewText = '(max 750 characters)';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_Create_MarketingInformationMaxCharacterCount', @NewText, @NewText;
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_Edit_MarketingInformationMaxCharacterCount', @NewText, @NewText;
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Create_MarketingInformationMaxCharacterCount', @NewText, @NewText;
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Edit_MarketingInformationMaxCharacterCount', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('E322767F-3F2B-4791-BEBC-B69CFA388868');
END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '47543D37-EB07-4B04-9044-177D377A0AF0')
BEGIN
	PRINT '[Updating UCAS Meta Data Types]';
	SET NOCOUNT ON;

	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (27, 'UCAS_PG_Courses', 'UCAS PG Course Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (28, 'UCAS_PG_Providers', 'UCAS PG Provider Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (29, 'UCAS_PG_Locations', 'UCAS PG Location Data');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (30, 'UCAS_PG_CourseOptions', 'UCAS PG Course Options');
	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (31, 'UCAS_PG_CourseOptionFees', 'UCAS PG Course Option Fee Data');

	UPDATE [dbo].[UcasStudyModeMapping] SET UcasStudyMode = 'Part-time block-release' WHERE UcasStudyModeId = 36;
	UPDATE [dbo].[UcasStudyModeMapping] SET UcasStudyMode = 'Part-time day-release' WHERE UcasStudyModeId = 39;
	UPDATE [dbo].[UcasStudyModeMapping] SET UcasStudyMode = 'Part-time day and block-release' WHERE UcasStudyModeId = 51;
	UPDATE [dbo].[UcasStudyModeMapping] SET UcasStudyMode = 'On-line study' WHERE UcasStudyModeId = 80;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = '<hr /><p>Use this page to upload new UCAS data.  You can upload one of: <ul><li>A full UCAS Under Graduate data file</li><li>An incremental UCAS Under Graduate data file</li><li>A UCAS Post Graduate data file.</li></ul>The file must be a .ZIP file containing CSV files.</p><hr />';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'UCASImport_Index_SubHeader', @NewText, @NewText;	

	INSERT INTO __RefactorLog (OperationKey) VALUES ('47543D37-EB07-4B04-9044-177D377A0AF0');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'DDD82FE7-76EE-4210-87EC-C86A1068A861')
BEGIN
	PRINT '[Adding Code Point Meta Data Type]';
	SET NOCOUNT ON;

	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (32, 'CodePoint', 'Code Point Data (GeoLocation)');
	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) VALUES ('VirtualDirectoryNameForStoringCodePointFiles', '\\vfiler-th\SFA_CDPP\CodePointUpload', '\\vfiler-th\SFA_CDPP\CodePointUpload', 'System.String', 'Directory to temporarily store Code Point import file.', 0);
	INSERT INTO [dbo].[Permission] (PermissionId, PermissionName, PermissionDescription) VALUES (70, 'CanUploadCodePointData', 'With this permission the user can upload Code Point (GeoLocation) data'); 
	INSERT INTO [dbo].[PermissionInRole] (RoleId, PermissionId) VALUES ((SELECT Id FROM AspNetRoles WHERE Name = 'Developer'), 70);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('DDD82FE7-76EE-4210-87EC-C86A1068A861');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'BE214055-BA24-42FA-BB69-FCD52581C727')
BEGIN
	PRINT '[Changing Tooltips]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = '';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Create_MarketingInformationHeaderTooltip', @NewText, @NewText;
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Edit_MarketingInformationHeaderTooltip', @NewText, @NewText;

	SET @NewText = 'Enter a brief introductory overview of your organisation and how it provides apprenticeships training. This must be information that employers will find useful e.g. what type of training organisation you are, how long you have been providing apprenticeship training, etc. Employers will view this information on all search results you are returned in (max 750 characters).';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditProviderModel_Description_MarketingInformation', @NewText, @NewText;

	SET @NewText = 'Apprenticeships';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Create_MarketingInformationHeader', @NewText, @NewText;
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Edit_MarketingInformationHeader', @NewText, @NewText;

	UPDATE Provider SET ApprenticeshipContract = 1 WHERE ProviderId IN (SELECT DISTINCT ProviderId FROM Apprenticeship WHERE RecordStatusId = 2);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('BE214055-BA24-42FA-BB69-FCD52581C727');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '79D1CC95-4F9E-4F9E-A1CC-B586D1F79397')
BEGIN
	PRINT '[Dropping UCAS.Durations Table]';
	SET NOCOUNT ON;

	DROP TABLE [UCAS].[Durations];
	DELETE FROM [dbo].[MetadataUpload] WHERE MetadataUploadTypeId = 13;
	DELETE FROM [dbo].[MetadataUploadType] WHERE MetadataUploadTypeId = 13;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('79D1CC95-4F9E-4F9E-A1CC-B586D1F79397');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '3D17FF8C-3CB7-4E22-997C-769B26A23FFC')
BEGIN
	PRINT '[Removing Capitals from Digital Apprenticeship Service]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Information entered here will appear on the digital apprenticeship service website after an overnight refresh.<hr />';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_EditApprenticeship_Summary', @NewText, @NewText;

	SET @NewText = 'Apprenticeship information will not be displayed on the digital apprenticeship service website as ''Current contract with the SFA'' is not checked. Click <a href="/Provider/Edit#ApprenticeshipDelivery">Edit Provider</a> to enter it.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_EditApprenticeship_NoProviderApprenticeshipContractWithEditLink', @NewText, @NewText;

	SET @NewText = 'Apprenticeship information will not be displayed on the digital apprenticeship service website as ''Current contract with the SFA'' is not checked.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_EditApprenticeship_NoProviderApprenticeshipContractWithoutEditLink', @NewText, @NewText;

	SET @NewText = 'Apprenticeship information will not be displayed on the digital apprenticeship service website as ''Current contract with the SFA'' is not checked. Click <a href="/Provider/Edit#ApprenticeshipDelivery">Edit Provider</a> to enter it.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_List_NoProviderApprenticeshipContractWithEditLink', @NewText, @NewText;

	SET @NewText = 'Apprenticeship information will not be displayed on the digital apprenticeship service website as ''Current contract with the SFA'' is not checked.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_List_NoProviderApprenticeshipContractWithoutEditLink', @NewText, @NewText;

	SET @NewText = 'Apprenticeship information will not be displayed on the digital apprenticeship service website as ''Current contract with the SFA'' is not checked. Click <a href="/Provider/Edit#ApprenticeshipDelivery">Edit Provider</a> to enter it.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_View_NoProviderApprenticeshipContractWithEditLink', @NewText, @NewText;

	SET @NewText = 'Apprenticeship information will not be displayed on the digital apprenticeship service website as ''Current contract with the SFA'' is not checked.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_View_NoProviderApprenticeshipContractWithoutEditLink', @NewText, @NewText;

	SET @NewText = '<br /><span style=\"font-size: 0.8em;\">Apprenticeship information will only be displayed on the digital apprenticeship service website if ''<strong>Current contract with the SFA</strong>'' is checked</span>';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Create_ApprenticeshipContractExplanation', @NewText, @NewText;
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Edit_ApprenticeshipContractExplanation', @NewText, @NewText;

	SET @NewText = '<strong>Warning:</strong> this apprenticeship is currently <strong>ARCHIVED</strong> and neither it nor any of its delivery locations will be published to the digital apprenticeship service.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_EditApprenticeship_ApprenticeshipArchivedWarning', @NewText, @NewText;
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_View_ApprenticeshipArchivedWarning', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('3D17FF8C-3CB7-4E22-997C-769B26A23FFC');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '8B027E0B-820D-48F3-954F-2A2DDAD175F0')
BEGIN
	PRINT '[Fixing Spaces in Postcodes]';
	SET NOCOUNT ON;

	UPDATE Address
	SET Postcode = LEFT(Postcode, LEN(Postcode) - 3) + ' ' + RIGHT(Postcode, 3)
	WHERE Postcode IS NOT NULL
		AND Len(Postcode) >= 5
		AND CHARINDEX(' ', Postcode) <= 0;

	UPDATE Address SET Postcode = REPLACE(Postcode, '  ', ' ') WHERE CHARINDEX('  ', Postcode) > 0;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('8B027E0B-820D-48F3-954F-2A2DDAD175F0');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '129F80E5-79FE-464D-A6AA-097E59F4B2CF')
BEGIN
	PRINT '[Clear Summary on Edit Apprenticeship]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = '';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_EditApprenticeship_Summary', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('129F80E5-79FE-464D-A6AA-097E59F4B2CF');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '48791222-375D-42E7-98C4-2468FFA1BB82')
BEGIN
	PRINT '[Fix Postcode Not Found Message]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'We could not find the postcode you have entered. Postcodes must be entered with a space - e.g. SW1A 1AA. If you believe your postcode is correct and are still receiving this error, please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Shared_EditorTemplates_PostcodeNotFound', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('48791222-375D-42E7-98C4-2468FFA1BB82');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '6F58883E-4AAB-4AA2-98DB-515C3570D4B8')
BEGIN
	PRINT '[Fixing Spaces in Postcodes Part 2]';
	SET NOCOUNT ON;

	UPDATE Address SET Postcode = RTRIM(LTRIM(Postcode)) WHERE Postcode <> RTRIM(LTRIM(Postcode));

	UPDATE Address
	SET Postcode = LEFT(Postcode, LEN(Postcode) - 3) + ' ' + RIGHT(Postcode, 3)
	WHERE Postcode IS NOT NULL
		AND Len(Postcode) >= 5
		AND CHARINDEX(' ', Postcode) <= 0;

	UPDATE Address SET Postcode = REPLACE(Postcode, '  ', ' ') WHERE CHARINDEX('  ', Postcode) > 0;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('6F58883E-4AAB-4AA2-98DB-515C3570D4B8');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'B5E5C6DF-5E23-46ED-83BB-C77AB643B4C7')
BEGIN
	PRINT '[Adding configuration options for UCAS API]';
	SET NOCOUNT ON;

	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) VALUES ('UCASAPIClientId', 'f8abba1d331b48d890efb846b78c9e4e', 'f8abba1d331b48d890efb846b78c9e4e', 'System.String', 'Client Id for UCAS API', 0);
	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) VALUES ('UCASAPIClientSecret', '331f5d11ed9e46bf8A0941CAD4090A0A', '331f5d11ed9e46bf8A0941CAD4090A0A', 'System.String', 'Client Secret for UCAS API', 0);
	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) VALUES ('UCASAPIURL', 'https://commercial-extract-experience.eu.cloudhub.io/api/v1/courses/postgraduate?dataType=CSV', 'https://commercial-extract-experience.eu.cloudhub.io/api/v1/courses/postgraduate?dataType=CSV', 'System.String', 'URL for UCAS API', 0);
	
	INSERT INTO __RefactorLog (OperationKey) VALUES ('B5E5C6DF-5E23-46ED-83BB-C77AB643B4C7');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'FD948986-19E5-453F-95F4-04AB662AEEC3')
BEGIN
	PRINT '[Add Apprenticeship Permissions to Standard Roles]';
	SET NOCOUNT ON;

	DECLARE	@RoleId UNIQUEIDENTIFIER;
	
	SELECT @RoleId = Id FROM AspNetRoles WHERE Name = 'Provider User';
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanBulkUploadProviderApprenticeshipFiles')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanBulkUploadProviderApprenticeshipFiles'));
	END;

	SELECT @RoleId = Id FROM AspNetRoles WHERE Name = 'Provider Superuser';
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanBulkUploadProviderApprenticeshipFiles')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanBulkUploadProviderApprenticeshipFiles'));
	END;

	SELECT @RoleId = Id FROM AspNetRoles WHERE Name = 'Organisation User';
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanBulkUploadOrganisationApprenticeshipFiles')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanBulkUploadOrganisationApprenticeshipFiles'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanBulkUploadProviderApprenticeshipFiles')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanBulkUploadProviderApprenticeshipFiles'));
	END;

	SELECT @RoleId = Id FROM AspNetRoles WHERE Name = 'Organisation Superuser';
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderLocation')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderLocation'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanEditProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderApprenticeship')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanDeleteProviderApprenticeship'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanBulkUploadOrganisationApprenticeshipFiles')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanBulkUploadOrganisationApprenticeshipFiles'));
	END;
	IF NOT EXISTS (SELECT * FROM PermissionInRole WHERE RoleId = @RoleId AND PermissionId = (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanBulkUploadProviderApprenticeshipFiles')) 
	BEGIN
		INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@RoleId, (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanBulkUploadProviderApprenticeshipFiles'));
	END;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('FD948986-19E5-453F-95F4-04AB662AEEC3');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '7855EBA8-082E-438E-9FF3-D234C739E02D')
BEGIN
	PRINT '[Remove Apprenticeship Roles]';
	SET NOCOUNT ON;

	DECLARE	@OldRoleId UNIQUEIDENTIFIER;
	DECLARE @NewRoleId UNIQUEIDENTIFIER;

	SELECT @OldRoleId = Id FROM AspNetRoles WHERE Name = 'Provider User (Apprenticeships)';
	SELECT @NewRoleId = Id FROM AspNetRoles WHERE Name = 'Provider User';

	UPDATE AspNetUserRoles SET RoleId = @NewRoleId WHERE RoleId = @OldRoleId;
	DELETE FROM PermissionInRole WHERE RoleId = @OldRoleId;
	DELETE FROM ProviderUserTypeInRole WHERE RoleId = @OldRoleId;
	DELETE FROM AspNetRoles WHERE Id = @OldRoleId;

	SELECT @OldRoleId = Id FROM AspNetRoles WHERE Name = 'Provider Superuser (Apprenticeships)';
	SELECT @NewRoleId = Id FROM AspNetRoles WHERE Name = 'Provider Superuser';

	UPDATE AspNetUserRoles SET RoleId = @NewRoleId WHERE RoleId = @OldRoleId;
	DELETE FROM ProviderUserTypeInRole WHERE RoleId = @OldRoleId;
	DELETE FROM PermissionInRole WHERE RoleId = @OldRoleId;
	DELETE FROM AspNetRoles WHERE Id = @OldRoleId;

	SELECT @OldRoleId = Id FROM AspNetRoles WHERE Name = 'Organisation User (Apprenticeships)';
	SELECT @NewRoleId = Id FROM AspNetRoles WHERE Name = 'Organisation User';

	UPDATE AspNetUserRoles SET RoleId = @NewRoleId WHERE RoleId = @OldRoleId;
	DELETE FROM ProviderUserTypeInRole WHERE RoleId = @OldRoleId;
	DELETE FROM PermissionInRole WHERE RoleId = @OldRoleId;
	DELETE FROM AspNetRoles WHERE Id = @OldRoleId;

	SELECT @OldRoleId = Id FROM AspNetRoles WHERE Name = 'Organisation Superuser (Apprenticeships)';
	SELECT @NewRoleId = Id FROM AspNetRoles WHERE Name = 'Organisation Superuser';

	UPDATE AspNetUserRoles SET RoleId = @NewRoleId WHERE RoleId = @OldRoleId;
	DELETE FROM ProviderUserTypeInRole WHERE RoleId = @OldRoleId;
	DELETE FROM PermissionInRole WHERE RoleId = @OldRoleId;
	DELETE FROM AspNetRoles WHERE Id = @OldRoleId;

	UPDATE ConfigurationSettings SET Value = 'Developer;Admin User;Admin Superuser;Provider Superuser;Provider User;Organisation Superuser;Organisation User' WHERE Name = 'AdminUserCanAddRoles';
	UPDATE ConfigurationSettings SET Value = 'Developer;Admin User;Admin Superuser;Provider Superuser;Provider User;Organisation Superuser;Organisation User' WHERE Name = 'AdminContextCanAddRoles';
	UPDATE ConfigurationSettings SET Value = 'Organisation Superuser;Organisation User' WHERE Name = 'OrganisationContextCanAddRoles';
	UPDATE ConfigurationSettings SET Value = 'Provider Superuser;Provider User' WHERE Name = 'ProviderContextCanAddRoles';

	INSERT INTO __RefactorLog (OperationKey) VALUES ('7855EBA8-082E-438E-9FF3-D234C739E02D');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '2EFD4DF8-A885-4E4F-BEE0-F20DC765D78F')
BEGIN
	PRINT '[Adding New QA Compliance & Style Failure Reasons]';
	SET NOCOUNT ON;

	INSERT INTO QAComplianceFailureReason (QAComplianceFailureReasonId, Description, Ordinal, RecordStatusId) VALUES (1, 'Specific employer named', 1, 2);
	INSERT INTO QAComplianceFailureReason (QAComplianceFailureReasonId, Description, Ordinal, RecordStatusId) VALUES (2, 'Unverifiable claim', 2, 2);

	INSERT INTO QAStyleFailureReason (QAStyleFailureReasonId, Description, Ordinal, RecordStatusId) VALUES (1, 'Specific apprenticeship details given', 1, 2);
	INSERT INTO QAStyleFailureReason (QAStyleFailureReasonId, Description, Ordinal, RecordStatusId) VALUES (2, 'Job roles included', 2, 2);
	INSERT INTO QAStyleFailureReason (QAStyleFailureReasonId, Description, Ordinal, RecordStatusId) VALUES (3, 'Term ''standard'' used', 3, 2);
	INSERT INTO QAStyleFailureReason (QAStyleFailureReasonId, Description, Ordinal, RecordStatusId) VALUES (4, 'Term ''framework'' used', 4, 2);
	INSERT INTO QAStyleFailureReason (QAStyleFailureReasonId, Description, Ordinal, RecordStatusId) VALUES (5, 'Term ''learner'' used', 5, 2);
	INSERT INTO QAStyleFailureReason (QAStyleFailureReasonId, Description, Ordinal, RecordStatusId) VALUES (6, 'Term ''course'' used', 6, 2);
		
	INSERT INTO __RefactorLog (OperationKey) VALUES ('2EFD4DF8-A885-4E4F-BEE0-F20DC765D78F');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'B520713D-06B6-46BC-9170-13B830EFB12A')
BEGIN
	PRINT '[Add configuration setting for Apprenticeship QA bands]';
	SET NOCOUNT ON;

	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) VALUES ('ApprenticeshipQABands', '5~3,10~4,50~8,100~15,9999999~20', '5~3,10~4,50~8,100~15,9999999~20', 'System.String', 'Apprenticeship bands for QA. This should be comma and tilde (~) separated values.  For example the default value of 5~3,10~4,50~8,100~15,9999999~20 means up to 5 apprenticeships then QA 3, up to 10 apprenticeships then QA 4 etc. ', 0);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('B520713D-06B6-46BC-9170-13B830EFB12A');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'CE1C454C-B5C0-4791-991D-3398EFF1F5DA')
BEGIN
	PRINT '[Adding Ofsted QA Compliance Failure Reasons]';
	SET NOCOUNT ON;

	INSERT INTO QAComplianceFailureReason (QAComplianceFailureReasonId, Description, Ordinal, RecordStatusId) VALUES (3, 'Incorrect Ofsted grade used', 3, 2);
		
	INSERT INTO __RefactorLog (OperationKey) VALUES ('CE1C454C-B5C0-4791-991D-3398EFF1F5DA');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'EA0149D3-0B52-4383-9D5B-EEA7F7EA5CD1')
BEGIN
	PRINT '[Add configuration setting for Email Address for Data Ready for QA]';
	SET NOCOUNT ON;

	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) VALUES ('DataReadyForQAEmailAddress', 'ROATP@coursedirectoryproviderportal.org.uk', 'ROATP@coursedirectoryproviderportal.org.uk', 'System.String', 'Email address to send ''Data Ready for QA'' email to', 0);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('EA0149D3-0B52-4383-9D5B-EEA7F7EA5CD1');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'BF76D49C-5978-415B-B951-3B13CF5EE4DF')
BEGIN
	PRINT '[Adding Meta Data Upload Types]';
	SET NOCOUNT ON;

	INSERT INTO [dbo].[MetadataUploadType] (MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription) VALUES (33, 'ProviderImport', 'Provider Import');

	INSERT INTO __RefactorLog (OperationKey) VALUES ('BF76D49C-5978-415B-B951-3B13CF5EE4DF');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'ABEA9C6D-39FA-4996-B9D7-6D69CBC390FB')
BEGIN
	PRINT '[Adding Full Descriptions to QA Failure Reasons]';
	SET NOCOUNT ON;

	UPDATE QAComplianceFailureReason SET FullDescription = 'Name specific employers' WHERE QAComplianceFailureReasonId = 1;
	UPDATE QAComplianceFailureReason SET FullDescription = 'Make unverifiable claims - for example, ''we are the best in the country''' WHERE QAComplianceFailureReasonId = 2;
	UPDATE QAComplianceFailureReason SET FullDescription = 'Include Ofsted grades other than the main rating for the whole organisation' WHERE QAComplianceFailureReasonId = 3;

	UPDATE QAStyleFailureReason SET FullDescription = 'List the content of the apprenticeship, for example, learning aim titles or level. This information is already contained in the standards and framework pathways summary pages' WHERE QAStyleFailureReasonId = 1;
	UPDATE QAStyleFailureReason SET FullDescription = 'Include job roles apprentices could do as these are included elsewhere' WHERE QAStyleFailureReasonId = 2;
	UPDATE QAStyleFailureReason SET FullDescription = 'Use the term ''standard'' or ''framework''; use ''apprenticeship'' instead' WHERE QAStyleFailureReasonId IN (3, 4);
	UPDATE QAStyleFailureReason SET FullDescription = 'Use the term ''learner'' or ''student''; use the term ''apprentice'' instead' WHERE QAStyleFailureReasonId = 5;
	UPDATE QAStyleFailureReason SET FullDescription = 'Use the term ''course''; use ''apprenticeship'' or ''training'' instead' WHERE QAStyleFailureReasonId = 6;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('ABEA9C6D-39FA-4996-B9D7-6D69CBC390FB');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'EB97D12F-7E85-43DC-A17F-E0CA7833BFE9')
BEGIN
	PRINT '[Fix configuration setting for Provider Import file]';
	SET NOCOUNT ON;

	UPDATE ConfigurationSettings
		SET Name = 'VirtualDirectoryNameForStoringProviderImportFiles'
	WHERE Name = 'VirtualDirectoryNameForStoringproviderImportFiles';

	INSERT INTO __RefactorLog (OperationKey) VALUES ('EB97D12F-7E85-43DC-A17F-E0CA7833BFE9');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '42ACFB31-38AE-413A-A086-E26EF52CDA73')
BEGIN
	PRINT '[Updating Email Template]';
	SET NOCOUNT ON;

	UPDATE EmailTemplate SET Params = '%PROVIDERNAME%=Provider Name,%USERNAME%=User''s Name,%REASON%=Reason(s) for failure' WHERE EmailTemplateId = 42;
	UPDATE QAComplianceFailureReason SET FullDescription = 'Make unverifiable claims - for example, ''we are the best in the country''' WHERE QAComplianceFailureReasonId = 2;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('42ACFB31-38AE-413A-A086-E26EF52CDA73');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'e7efd3e7-94ec-4a83-a499-594265e8e59d')
BEGIN
	PRINT '[Add configuration setting for Email Address for Submit New Text for QA]';
	SET NOCOUNT ON;

	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) VALUES ('SubmitNewTextForQAAddress', 'ROATP@coursedirectoryproviderportal.org.uk', 'ROATP@coursedirectoryproviderportal.org.uk', 'System.String', 'Email address to send ''Submit New Text For QA'' email to', 0);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('e7efd3e7-94ec-4a83-a499-594265e8e59d');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '5839703D-167C-4EEC-93B8-66601E691943')
BEGIN
	PRINT '[Adding UKPRN & Postcode to Email Template Parameters]';
	SET NOCOUNT ON;

	UPDATE EmailTemplate SET Params = '%PROVIDERNAME%=Provider Name,%UKPRN%=Provider''s UKPRN,%POSTCODE%=Provider''s Postcode' WHERE EmailTemplateId = 43;
	UPDATE EmailTemplate SET Params = '%PROVIDERNAME%=Provider Name,%UKPRN%=Provider''s UKPRN,%POSTCODE%=Provider''s Postcode,%NEWTEXT%=New text for QA' WHERE EmailTemplateId = 44;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('5839703D-167C-4EEC-93B8-66601E691943');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '4C5CB81A-DDAF-498D-AD77-65997C9FA076')
BEGIN
	PRINT '[Change QA Fail Email]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = '<table><thead><tr><th>Provider / Apprenticeship</th><th>Reason(s) for Failure</th><th>Further Details of Unverifiable Claim</th><th>Style Failure Reason(s)</th></tr></thead><tbody>';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_SendQAFailEmail_QAFailEmailHeaderRowHTML', @NewText, @NewText;
	SET @NewText = '<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_SendQAFailEmail_QAFailEmailRowHTML', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('4C5CB81A-DDAF-498D-AD77-65997C9FA076');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'CB607BCB-B0E0-4542-81AE-083ADBA64BE5')
BEGIN
	PRINT '[Change report heading]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Passed Overall?';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'QAdProvidersReportViewModelItem_DisplayName_Passed', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('CB607BCB-B0E0-4542-81AE-083ADBA64BE5');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '0B38B971-91CE-4603-B2BD-06EDE9199175')
BEGIN
	PRINT '[Set Tooltip for RoATP Field]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Has this provider applied to be on the Register of Apprenticeship Training Providers?';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'AddEditProviderModel_Description_RoATP', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('0B38B971-91CE-4603-B2BD-06EDE9199175');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'BDFB537A-21C9-40B5-A4D2-BFB637CC5885')
BEGIN
	PRINT '[Fix Failure Reason Descriptions]';
	SET NOCOUNT ON;

	UPDATE QAStyleFailureReason SET FullDescription = 'Use of the term ''standard'' or ''framework''; use ''apprenticeship'' instead' WHERE QAStyleFailureReasonId IN (3, 4);
	UPDATE QAStyleFailureReason SET FullDescription = 'Use of the term ''learner'' or ''student''; use the term ''apprentice'' instead' WHERE QAStyleFailureReasonId = 5;
	UPDATE QAStyleFailureReason SET FullDescription = 'Use of the term ''course''; use ''apprenticeship'' or ''training'' instead' WHERE QAStyleFailureReasonId = 6;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('BDFB537A-21C9-40B5-A4D2-BFB637CC5885');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'F145A6F2-859C-43BF-9D73-D361EB5620FC')
BEGIN
	PRINT '[Fix Term Learner Used Name]';
	SET NOCOUNT ON;

	UPDATE QAStyleFailureReason SET Description = 'Term ''learner'' or ''student'' used' WHERE QAStyleFailureReasonId = 5;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('F145A6F2-859C-43BF-9D73-D361EB5620FC');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '0379EFA8-BCD5-4E9E-89C1-B51C3563779B')
BEGIN
	PRINT '[Add Name to Automation Import User]';
	SET NOCOUNT ON;

	UPDATE AspNetUsers SET Name = 'Automation Import User' WHERE Id = '24314672-f766-47f1-98cb-ad9fc49f6e9d';

	INSERT INTO __RefactorLog (OperationKey) VALUES ('0379EFA8-BCD5-4E9E-89C1-B51C3563779B');
END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '0BAAD861-5802-4DE1-8424-FCFEC32AE42B')
BEGIN
       PRINT '[Updating Bulk Upload Language Fields]';
       SET NOCOUNT ON;

       DECLARE @NewText nvarchar(2000);
       SET @NewText = 'Blank Opportunity Id for provider {0} section {1}';
       EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_Constants_BlankOpportunityId', @NewText, @NewText;
       
       SET @NewText = 'Blank Course Id for provider {0} section {1}';
       EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_Constants_BlankCourseId', @NewText, @NewText;
       
       INSERT INTO __RefactorLog (OperationKey) VALUES ('0BAAD861-5802-4DE1-8424-FCFEC32AE42B');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'D089DF06-0200-46CB-A656-D33C828DE28A')
BEGIN
	PRINT '[Updating Provider Create/Edit Page Heading]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'The details below are for display on the National Careers Service. For those fields not filled in, contact information from UKRLP will be used. Providers can update these Course Directory fields at any time and they will then be used in preference to their UKRLP information.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Edit_PageSummary', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('D089DF06-0200-46CB-A656-D33C828DE28A')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'D089DF06-0200-46AA-A656-D33C828DE28A')
BEGIN
	PRINT '[Updating DeliveryInformation Page Heading]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)
	SET @NewText = 'The details below are for display on the Find an Apprenticeship Service. They are not mandatory.'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_DeliveryInformation_PageSummary', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('D089DF06-0200-46AA-A656-D33C828DE28A')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '102B7220-7B4F-4F25-B238-BF79A8520576')
BEGIN
	PRINT '[Replacing invalid characters in Marketing Information fields]';
	SET NOCOUNT ON;

	UPDATE Provider SET MarketingInformation = REPLACE(MarketingInformation, '–', '-') WHERE MarketingInformation LIKE '%–%';
	UPDATE Provider SET MarketingInformation = REPLACE(MarketingInformation, '“', '"') WHERE MarketingInformation LIKE '%“%';
	UPDATE Provider SET MarketingInformation = REPLACE(MarketingInformation, '”', '"') WHERE MarketingInformation LIKE '%”%';
	UPDATE Provider SET MarketingInformation = REPLACE(MarketingInformation, '‘', '''') WHERE MarketingInformation LIKE '%‘%';
	UPDATE Provider SET MarketingInformation = REPLACE(MarketingInformation, '’', '''') WHERE MarketingInformation LIKE '%’%';
	UPDATE Provider SET MarketingInformation = REPLACE(MarketingInformation, ' ', ' ') WHERE MarketingInformation LIKE '% %';
	UPDATE Provider SET MarketingInformation = REPLACE(MarketingInformation, '—', '-') WHERE MarketingInformation LIKE '%—%';
	UPDATE Provider SET MarketingInformation = REPLACE(MarketingInformation, '©', '(C)') WHERE MarketingInformation LIKE '%©%';
	UPDATE Provider SET MarketingInformation = REPLACE(MarketingInformation, '®', '(R)') WHERE MarketingInformation LIKE '%®%';
	UPDATE Provider SET MarketingInformation = REPLACE(MarketingInformation, '…', '...') WHERE MarketingInformation LIKE '%…%';
	UPDATE Provider SET MarketingInformation = REPLACE(MarketingInformation, '™', '(TM)') WHERE MarketingInformation LIKE '%™%';

	UPDATE Apprenticeship SET MarketingInformation = REPLACE(MarketingInformation, '–', '-') WHERE MarketingInformation LIKE '%–%';
	UPDATE Apprenticeship SET MarketingInformation = REPLACE(MarketingInformation, '“', '"') WHERE MarketingInformation LIKE '%“%';
	UPDATE Apprenticeship SET MarketingInformation = REPLACE(MarketingInformation, '”', '"') WHERE MarketingInformation LIKE '%”%';
	UPDATE Apprenticeship SET MarketingInformation = REPLACE(MarketingInformation, '‘', '''') WHERE MarketingInformation LIKE '%‘%';
	UPDATE Apprenticeship SET MarketingInformation = REPLACE(MarketingInformation, '’', '''') WHERE MarketingInformation LIKE '%’%';
	UPDATE Apprenticeship SET MarketingInformation = REPLACE(MarketingInformation, ' ', ' ') WHERE MarketingInformation LIKE '% %';
	UPDATE Apprenticeship SET MarketingInformation = REPLACE(MarketingInformation, '—', '-') WHERE MarketingInformation LIKE '%—%';
	UPDATE Apprenticeship SET MarketingInformation = REPLACE(MarketingInformation, '©', '(C)') WHERE MarketingInformation LIKE '%©%';
	UPDATE Apprenticeship SET MarketingInformation = REPLACE(MarketingInformation, '®', '(R)') WHERE MarketingInformation LIKE '%®%';
	UPDATE Apprenticeship SET MarketingInformation = REPLACE(MarketingInformation, '…', '...') WHERE MarketingInformation LIKE '%…%';
	UPDATE Apprenticeship SET MarketingInformation = REPLACE(MarketingInformation, '™', '(TM)') WHERE MarketingInformation LIKE '%™%';

	INSERT INTO __RefactorLog (OperationKey) VALUES ('102B7220-7B4F-4F25-B238-BF79A8520576');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'FCE435E0-D8E8-48C6-B1D6-146159CE87A6')
BEGIN
	PRINT '[Correct Spelling on MetadataUploadType]';
	SET NOCOUNT ON;

	UPDATE MetadataUploadType SET MetadataUploadTypeDescription = 'Apprenticeship Standards' WHERE MetadataUploadTypeId = 9;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('FCE435E0-D8E8-48C6-B1D6-146159CE87A6');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '58B496E5-375B-4099-9C2B-5718A350EDA1')
BEGIN
	PRINT '[Updating Widget Catalogue Language Fields]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'The details below are for display on the Find an Apprenticeship Service.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_DeliveryInformation_PageSummary', @NewText, @NewText;

	SET @NewText = 'Edit delivery information';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Shared_BootstrapMenuPartial_ApprenticeshipsViewMarketingInformationMenuItem', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('58B496E5-375B-4099-9C2B-5718A350EDA1');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '8EA22BF6-EE02-4971-9638-D91DBE4BC09B')
BEGIN
	PRINT '[Updating Import New Provider TableHeader Text]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Use this page to import the details of new providers. <br /><br /> The file should be a zipped copy of the CSV file downloaded from <a href="/Content/RoATP%20Provider%20CSV%20File%20Format.docx">here.</a>';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderImport_Import_TableHeader', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('8EA22BF6-EE02-4971-9638-D91DBE4BC09B');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '2B4B8A3C-DEE2-4260-B6A0-E8C593F24E7B')
BEGIN
	PRINT '[Adding new can manage import batches permission]';
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanManageImportBatches')
	BEGIN
		INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) 
		VALUES (78, N'CanManageImportBatches', N'With this permission a user may manage import batch names');
		INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 78); --Developer
		INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 78); --Admin supervisor
		INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('FE1CD530-C317-4DE5-B608-0CB1E4419305', 78); --Helpdesk
	END;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('2B4B8A3C-DEE2-4260-B6A0-E8C593F24E7B');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '02FF3109-2AE0-4D57-A8DE-52321AD1471E')
BEGIN
	PRINT '[Setting default import batches]';
	SET NOCOUNT ON;

	-- Beta
	INSERT INTO ImportBatchProvider (ImportBatchId, ProviderId, ImportDateTimeUtc) (SELECT 2, ProviderId, CreatedDateTimeUtc FROM dbo.Provider WHERE RoATPFFlag = 1 AND CreatedDateTimeUtc < CAST('01 DEC 2016' AS DATE));

	-- Batch 1
	INSERT INTO ImportBatchProvider (ImportBatchId, ProviderId, ImportDateTimeUtc) (SELECT 3, ProviderId, CreatedDateTimeUtc FROM dbo.Provider WHERE RoATPFFlag = 1 AND CreatedDateTimeUtc BETWEEN CAST('02 DEC 2016' AS DATE) AND CAST('31 DEC 2016' AS DATE));
	
	-- Batch 2
	INSERT INTO ImportBatchProvider (ImportBatchId, ProviderId, ImportDateTimeUtc) (SELECT 4, ProviderId, CreatedDateTimeUtc FROM dbo.Provider WHERE RoATPFFlag = 1 AND CreatedDateTimeUtc > CAST('01 JAN 2017' AS DATE));

	INSERT INTO __RefactorLog (OperationKey) VALUES ('02FF3109-2AE0-4D57-A8DE-52321AD1471E');
END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'CE9C3EAC-DA31-42E5-A232-66771B8FD7D0')
BEGIN
	PRINT '[Setting last time marketing information changed]';
	SET NOCOUNT ON;

	UPDATE Provider
	SET MarketingInformationUpdatedDateUtc = (
												SELECT Min(AuditDateUtc) 
												FROM Audit_Provider 
												WHERE AuditSeq > (
																	SELECT Max(AuditSeq)
																	FROM Audit_Provider
																	WHERE ProviderId = Provider.ProviderId
																		AND MarketingInformation <> Provider.MarketingInformation
																 )
													AND ProviderId = Provider.ProviderId
											 );

	INSERT INTO __RefactorLog (OperationKey) VALUES ('CE9C3EAC-DA31-42E5-A232-66771B8FD7D0');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'BED9C329-4033-4d95-8F60-82D753FCF07F')
BEGIN
	PRINT '[Adding new QA Compliance Failure Reasons]';
	SET NOCOUNT ON;

	INSERT INTO QAComplianceFailureReason (QAComplianceFailureReasonId, Description, FullDescription, Ordinal, RecordStatusId)
	VALUES (4, 'Insufficient detail','Information for employers is too short or not informative enough',4,2);

	INSERT INTO QAComplianceFailureReason (QAComplianceFailureReasonId, Description, FullDescription, Ordinal, RecordStatusId)
	VALUES (5, 'Not aimed at employer','Information for employers should be aimed at a potential employers.',5,2);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('BED9C329-4033-4d95-8F60-82D753FCF07F');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'DA2FC2D9-A10E-4288-936E-9E371F17D25C')
BEGIN
	PRINT '[Updating QA Failed Email Row Templates]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = '<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_SendQAFailEmail_QAFailEmailRowHTML', @NewText, @NewText;

	SET @NewText = '<table><thead><tr><th>Provider / Apprenticeship</th><th>Reason(s) for Failure</th><th>Further Details of Unverifiable Claim</th><th>Further details about compliance fails</th><th>Style Failure Reason(s)</th></tr></thead><tbody>';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_SendQAFailEmail_QAFailEmailHeaderRowHTML', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('DA2FC2D9-A10E-4288-936E-9E371F17D25C');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'C1B6E809-9F98-467A-B5A8-65BDC093167B')
BEGIN
	PRINT '[Adding Unable To Complete Failure Reasons]';
	SET NOCOUNT ON;
INSERT INTO [dbo].[UnableToCompleteFailureReason] ([UnableToCompleteFailureReasonId],[Description],[FullDescription],[Ordinal],[RecordStatusId]) VALUES (1,'Are developing their provision','Are developing their provision',1,2);
INSERT INTO [dbo].[UnableToCompleteFailureReason] ([UnableToCompleteFailureReasonId],[Description],[FullDescription],[Ordinal],[RecordStatusId]) VALUES (2,'Do not have a current available approved Standard to deliver','Do not have a current available approved Standard to deliver',2,2);
INSERT INTO [dbo].[UnableToCompleteFailureReason] ([UnableToCompleteFailureReasonId],[Description],[FullDescription],[Ordinal],[RecordStatusId]) VALUES (3,'Have used a deactivated/incorrect UKPRN','Have used a deactivated/incorrect UKPRN',3,2);
INSERT INTO [dbo].[UnableToCompleteFailureReason] ([UnableToCompleteFailureReasonId],[Description],[FullDescription],[Ordinal],[RecordStatusId]) VALUES (4,'Have applied to the wrong route i.e. employer providers or subcontractors','Have applied to the wrong route i.e. employer providers or subcontractors',4,2);
INSERT INTO [dbo].[UnableToCompleteFailureReason] ([UnableToCompleteFailureReasonId],[Description],[FullDescription],[Ordinal],[RecordStatusId]) VALUES (5,'Are subcontractors who were forced to apply to the Main Route as they deliver >£500k of funded learning p.a.','Are subcontractors who were forced to apply to the Main Route as they deliver >£500k of funded learning p.a.',5,2);
INSERT INTO [dbo].[UnableToCompleteFailureReason] ([UnableToCompleteFailureReasonId],[Description],[FullDescription],[Ordinal],[RecordStatusId]) VALUES (6,'Decided to withdraw application','Decided to withdraw application',6,2);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('C1B6E809-9F98-467A-B5A8-65BDC093167B');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '0031277F-3C43-47C2-A4C4-695DD4C55BA3')
BEGIN
	PRINT '[Adding new can manage unable to complete permission]';
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanManageUnabletoComplete')
	BEGIN
		INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) 
		VALUES (79, N'CanManageUnabletoComplete', N'With this permission a user may manage unable to complete information');
		INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 79); --Developer
		INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 79); --Admin supervisor
		INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 79); --Admin User
		INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('FE1CD530-C317-4DE5-B608-0CB1E4419305', 79); --Helpdesk
		
	END;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('0031277F-3C43-47C2-A4C4-695DD4C55BA3');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '359B99A0-6491-4591-B842-88C36E718093')
BEGIN
	PRINT '[Updating Unable To Complete When/By Text]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Reported inability to complete on <b>{0}</b> by <b>{1}</b>.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_Edit_UnableToCompleteWhenBy', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('359B99A0-6491-4591-B842-88C36E718093');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '41A75D48-92CB-4D59-94E8-F20E078E06A2')
BEGIN
	PRINT '[Updating Edit Delivery Information Page Heading]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'The details below are for display on the Find an Apprenticeship Service. ';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_DeliveryInformation_PageSummary', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('41A75D48-92CB-4D59-94E8-F20E078E06A2');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '9AEEEC26-6DA9-4050-BBDE-E038C22E4D8E')
BEGIN
	PRINT '[Fix Bulk Upload Blank Course Id Error Message]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = 'Blank Course Id for provider {0}';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'BulkUpload_Constants_BlankCourseId', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('9AEEEC26-6DA9-4050-BBDE-E038C22E4D8E');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'F6928F89-D023-49CA-8199-F8CC7235DFF6')
BEGIN
	PRINT '[Update configuration setting for LARS File Name]';
	SET NOCOUNT ON;

	UPDATE ConfigurationSettings SET Value = 'https://hub.fasst.org.uk/Learning%20Aims/Downloads/Documents/{date}_LARS_V005_MDB.Zip' WHERE Name = 'LARSUrlAndFileName';

	INSERT INTO __RefactorLog (OperationKey) VALUES ('F6928F89-D023-49CA-8199-F8CC7235DFF6');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '22F93C54-6776-4666-9919-BC21FCE88F4A')
BEGIN
	SET NOCOUNT ON;

	-- For some reason (maybe due to original data migration) some records never created insert operations into the
	-- audit_* tables.  As the data warehouse relies on those we need to create them.

	PRINT '[Adding Missing Address Audit Records]';
	SET IDENTITY_INSERT Audit_Address ON;

	INSERT INTO Audit_Address
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
		[AddressId],
		[AddressLine1],
		[AddressLine2],
		[Town],
		[County],
		[Postcode],
		[ProviderRegionId],
		[Latitude],
		[Longitude]
	FROM Audit_Address
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT AddressId,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_Address
								WHERE AddressId NOT IN (SELECT AddressId FROM Audit_Address WHERE AuditOperation = 'I')
								GROUP BY AddressId
								) A
						);

	SET IDENTITY_INSERT Audit_Address OFF;

	INSERT INTO Audit_Address 
	(
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT 'I',
		COALESCE(P.[CreatedDateTimeUtc], L.[CreatedDateTimeUtc], V.[CreatedDateTimeUtc], U.[CreatedDateTimeUtc]),
		A.[AddressId],
		A.[AddressLine1],
		A.[AddressLine2],
		A.[Town],
		A.[County],
		A.[Postcode],
		A.[ProviderRegionId],
		A.[Latitude],
		A.[Longitude]
	FROM [Address] A
		LEFT OUTER JOIN (SELECT AddressId, Min(CreatedDateTimeUtc) AS CreatedDateTimeUtc FROM Provider GROUP BY AddressId) P ON P.AddressId = A.AddressId
		LEFT OUTER JOIN (SELECT AddressId, Min(CreatedDateTimeUtc) AS CreatedDateTimeUtc FROM Location GROUP BY AddressId) L ON L.AddressId = A.AddressId
		LEFT OUTER JOIN (SELECT AddressId, Min(CreatedDateTimeUtc) AS CreatedDateTimeUtc FROM Venue GROUP BY AddressId) V ON V.AddressId = A.AddressId
		LEFT OUTER JOIN (SELECT AddressId, Min(CreatedDateTimeUtc) AS CreatedDateTimeUtc FROM AspNetUsers GROUP BY AddressId) U ON V.AddressId = A.AddressId
	WHERE (P.AddressId IS NOT NULL OR L.AddressId IS NOT NULL OR V.AddressId IS NOT NULL OR U.AddressId IS NOT NULL)
		AND	A.AddressId NOT IN (SELECT DISTINCT AddressId FROM Audit_Address);
			

	PRINT '[Adding Missing Apprenticeship Audit Records]';
	SET IDENTITY_INSERT Audit_Apprenticeship ON;

	INSERT INTO Audit_Apprenticeship
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
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
	FROM Audit_Apprenticeship
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT ApprenticeshipId,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_Apprenticeship
								WHERE ApprenticeshipId NOT IN (SELECT ApprenticeshipId FROM Audit_Apprenticeship WHERE AuditOperation = 'I')
								GROUP BY ApprenticeshipId
								) A
						);

	SET IDENTITY_INSERT Audit_Apprenticeship OFF;

	INSERT INTO Audit_Apprenticeship 
	(
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT 'I',
		[CreatedDateTimeUtc],
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
	FROM [Apprenticeship]
	WHERE ApprenticeshipId NOT IN (SELECT DISTINCT ApprenticeshipId FROM Audit_Apprenticeship);


	PRINT '[Adding Missing ApprenticeshipLocation Audit Records]';
	SET IDENTITY_INSERT Audit_ApprenticeshipLocation ON;

	INSERT INTO Audit_ApprenticeshipLocation
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
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
	FROM Audit_ApprenticeshipLocation
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT ApprenticeshipLocationId,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_ApprenticeshipLocation
								WHERE ApprenticeshipLocationId NOT IN (SELECT ApprenticeshipLocationId FROM Audit_ApprenticeshipLocation WHERE AuditOperation = 'I')
								GROUP BY ApprenticeshipLocationId
								) A
						);

	SET IDENTITY_INSERT Audit_ApprenticeshipLocation OFF;

	INSERT INTO Audit_ApprenticeshipLocation 
	(
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT 'I',
		[CreatedDateTimeUtc],
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
	FROM [ApprenticeshipLocation]
	WHERE ApprenticeshipLocationId NOT IN (SELECT DISTINCT ApprenticeshipLocationId FROM Audit_ApprenticeshipLocation);


	PRINT '[Adding Missing ApprenticeshipLocationDeliveryMode Audit Records]';
	SET IDENTITY_INSERT Audit_ApprenticeshipLocationDeliveryMode ON;

	INSERT INTO Audit_ApprenticeshipLocationDeliveryMode
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
		[ApprenticeshipLocationId],
		[DeliveryModeId]
	)
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
		[ApprenticeshipLocationId],
		[DeliveryModeId]
	FROM Audit_ApprenticeshipLocationDeliveryMode
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT ApprenticeshipLocationId,
										DeliveryModeId,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_ApprenticeshipLocationDeliveryMode A
								WHERE NOT EXISTS (
													SELECT ApprenticeshipLocationId
													FROM Audit_ApprenticeshipLocationDeliveryMode
													WHERE AuditOperation = 'I'
														AND ApprenticeshipLocationId = A.ApprenticeshipLocationId
														AND DeliveryModeId = A.DeliveryModeId
												 )
								GROUP BY ApprenticeshipLocationId,
									DeliveryModeId
								) A
						);

	SET IDENTITY_INSERT Audit_ApprenticeshipLocationDeliveryMode OFF;

	INSERT INTO Audit_ApprenticeshipLocationDeliveryMode
	(
		[AuditOperation],
		[AuditDateUtc],
		[ApprenticeshipLocationId],
		[DeliveryModeId]
	)
	SELECT 'I',
		AL.[CreatedDateTimeUtc],
		A.[ApprenticeshipLocationId],
		A.[DeliveryModeId]
	FROM ApprenticeshipLocationDeliveryMode A
		INNER JOIN ApprenticeshipLocation AL ON AL.ApprenticeshipLocationId = A.ApprenticeshipLocationId
	WHERE NOT EXISTS (
						SELECT ApprenticeshipLocationId
						FROM Audit_ApprenticeshipLocationDeliveryMode
						WHERE AuditOperation = 'I'
							AND ApprenticeshipLocationId = A.ApprenticeshipLocationId
							AND DeliveryModeId = A.DeliveryModeId
					 );


	PRINT '[Adding Missing AspNetUsers Audit Records]';
	SET IDENTITY_INSERT Audit_AspNetUsers ON;

	INSERT INTO Audit_AspNetUsers
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
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
	FROM Audit_AspNetUsers 
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT Id,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_AspNetUsers
								WHERE Id NOT IN (SELECT Id FROM Audit_AspNetUsers WHERE AuditOperation = 'I')
								GROUP BY Id
								) A
						);

	SET IDENTITY_INSERT Audit_AspNetUsers OFF;

	INSERT INTO Audit_AspNetUsers 
	(
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT 'I',
		[CreatedDateTimeUtc],
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
	FROM AspNetUsers
	WHERE Id NOT IN (SELECT DISTINCT Id FROM Audit_AspNetUsers);


	PRINT '[Adding Missing Course Audit Records]';
	SET IDENTITY_INSERT Audit_Course ON;

	INSERT INTO Audit_Course
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
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
	FROM Audit_Course 
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT CourseId,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_Course
								WHERE CourseId NOT IN (SELECT CourseId FROM Audit_Course WHERE AuditOperation = 'I')
								GROUP BY CourseId
								) A
						);

	SET IDENTITY_INSERT Audit_Course OFF;

	INSERT INTO Audit_Course 
	(
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT 'I',
		[CreatedDateTimeUtc],
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
	FROM Course
	WHERE CourseId NOT IN (SELECT DISTINCT CourseId FROM Audit_Course);


	PRINT '[Adding Missing CourseInstance Audit Records]';
	SET IDENTITY_INSERT Audit_CourseInstance ON;

	INSERT INTO Audit_CourseInstance
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
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
	FROM Audit_CourseInstance
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT CourseInstanceId,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_CourseInstance
								WHERE CourseInstanceId NOT IN (SELECT CourseInstanceId FROM Audit_CourseInstance WHERE AuditOperation = 'I')
								GROUP BY CourseInstanceId
								) A
						);

	SET IDENTITY_INSERT Audit_CourseInstance OFF;

	INSERT INTO Audit_CourseInstance 
	(
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT 'I',
		[CreatedDateTimeUtc],
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
	FROM [CourseInstance]
	WHERE CourseInstanceId NOT IN (SELECT DISTINCT CourseInstanceId FROM Audit_CourseInstance);


	PRINT '[Adding Missing CourseInstanceA10FundingCode Audit Records]';
	SET IDENTITY_INSERT Audit_CourseInstanceA10FundingCode ON;

	INSERT INTO Audit_CourseInstanceA10FundingCode
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
		[CourseInstanceId],
		[A10FundingCode]
	)
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
		[CourseInstanceId],
		[A10FundingCode]
	FROM Audit_CourseInstanceA10FundingCode
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT CourseInstanceId,
										A10FundingCode,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_CourseInstanceA10FundingCode CIFC
								WHERE NOT EXISTS (
													SELECT CourseInstanceId
													FROM Audit_CourseInstanceA10FundingCode
													WHERE AuditOperation = 'I'
														AND CourseInstanceId = CIFC.CourseInstanceId
														AND A10FundingCode = CIFC.A10FundingCode
												 )
								GROUP BY CourseInstanceId,
									A10FundingCode
								) A
						);

	SET IDENTITY_INSERT Audit_CourseInstanceA10FundingCode OFF;

	INSERT INTO Audit_CourseInstanceA10FundingCode 
	(
		[AuditOperation],
		[AuditDateUtc],
		[CourseInstanceId],
		[A10FundingCode]
	)
	SELECT 'I',
		CI.[CreatedDateTimeUtc],
		CIFC.[CourseInstanceId],
		CIFC.[A10FundingCode]
	FROM [CourseInstanceA10FundingCode] CIFC
		INNER JOIN CourseInstance CI ON CI.CourseInstanceId = CIFC.CourseInstanceId
	WHERE NOT EXISTS (
						SELECT CourseInstanceId
						FROM Audit_CourseInstanceA10FundingCode
						WHERE AuditOperation = 'I'
							AND CourseInstanceId = CIFC.CourseInstanceId
							AND A10FundingCode = CIFC.A10FundingCode
					 );


	PRINT '[Adding Missing CourseInstanceStartDate Audit Records]';
	SET IDENTITY_INSERT Audit_CourseInstanceStartDate ON;

	INSERT INTO Audit_CourseInstanceStartDate
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
		[CourseInstanceStartDateId],
		[CourseInstanceId],
		[StartDate],
		[IsMonthOnlyStartDate],
		[PlacesAvailable]
	)
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
		[CourseInstanceStartDateId],
		[CourseInstanceId],
		[StartDate],
		[IsMonthOnlyStartDate],
		[PlacesAvailable]
	FROM Audit_CourseInstanceStartDate
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT CourseInstanceStartDateId,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_CourseInstanceStartDate
								WHERE CourseInstanceStartDateId NOT IN (SELECT CourseInstanceStartDateId FROM Audit_CourseInstanceStartDate WHERE AuditOperation = 'I')
								GROUP BY CourseInstanceStartDateId
								) A
						);

	SET IDENTITY_INSERT Audit_CourseInstanceStartDate OFF;

	INSERT INTO Audit_CourseInstanceStartDate
	(
		[AuditOperation],
		[AuditDateUtc],
		[CourseInstanceStartDateId],
		[CourseInstanceId],
		[StartDate],
		[IsMonthOnlyStartDate],
		[PlacesAvailable]
	)
	SELECT 'I',
		CI.[CreatedDateTimeUtc],
		CISD.[CourseInstanceStartDateId],
		CISD.[CourseInstanceId],
		CISD.[StartDate],
		CISD.[IsMonthOnlyStartDate],
		CISD.[PlacesAvailable]
	FROM CourseInstanceStartDate CISD
		INNER JOIN CourseInstance CI ON CI.CourseInstanceId = CISD.CourseInstanceId
	WHERE CourseInstanceStartDateId NOT IN (SELECT CourseInstanceStartDateId FROM Audit_CourseInstanceStartDate WHERE AuditOperation = 'I');


	PRINT '[Adding Missing CourseInstanceVenue Audit Records]';
	SET IDENTITY_INSERT Audit_CourseInstanceVenue ON;

	INSERT INTO Audit_CourseInstanceVenue
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
		[CourseInstanceId],
		[VenueId]
	)
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
		[CourseInstanceId],
		[VenueId]
	FROM Audit_CourseInstanceVenue
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT CourseInstanceId,
										VenueId,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_CourseInstanceVenue CIV
								WHERE NOT EXISTS (
													SELECT CourseInstanceId
													FROM Audit_CourseInstanceVenue
													WHERE AuditOperation = 'I'
														AND CourseInstanceId = CIV.CourseInstanceId
														AND VenueId = CIV.VenueId
												 )
								GROUP BY CourseInstanceId,
									VenueId
								) A
						);

	SET IDENTITY_INSERT Audit_CourseInstanceVenue OFF;

	INSERT INTO Audit_CourseInstanceVenue
	(
		[AuditOperation],
		[AuditDateUtc],
		[CourseInstanceId],
		[VenueId]
	)
	SELECT 'I',
		CI.[CreatedDateTimeUtc],
		CIV.[CourseInstanceId],
		CIV.[VenueId]
	FROM CourseInstanceVenue CIV
		INNER JOIN CourseInstance CI ON CI.CourseInstanceId = CIV.CourseInstanceId
	WHERE NOT EXISTS (
						SELECT CourseInstanceId
						FROM Audit_CourseInstanceVenue
						WHERE AuditOperation = 'I'
							AND CourseInstanceId = CIV.CourseInstanceId
							AND VenueId = CIV.VenueId
					 );
					 

	PRINT '[Adding Missing CourseLearnDirectClassification Audit Records]';
	SET IDENTITY_INSERT Audit_CourseLearnDirectClassification ON;

	INSERT INTO Audit_CourseLearnDirectClassification
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
		[CourseId],
		[LearnDirectClassificationRef],
		[ClassificationOrder]
	)
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
		[CourseId],
		[LearnDirectClassificationRef],
		[ClassificationOrder]
	FROM Audit_CourseLearnDirectClassification
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT CourseId,
									LearnDirectClassificationRef,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_CourseLearnDirectClassification CLDC
								WHERE NOT EXISTS (
													SELECT CourseId
													FROM Audit_CourseLearnDirectClassification 
													WHERE AuditOperation = 'I'					
														AND CourseID = CLDC.CourseId
														AND LearnDirectClassificationRef = CLDC.LearnDirectClassificationRef
												 )
								GROUP BY CourseId,
									LearnDirectClassificationRef
								) A
						);

	SET IDENTITY_INSERT Audit_CourseLearnDirectClassification OFF;

	INSERT INTO Audit_CourseLearnDirectClassification
	(
		[AuditOperation],
		[AuditDateUtc],
		[CourseId],
		[LearnDirectClassificationRef],
		[ClassificationOrder]
	)
	SELECT 'I',
		C.[CreatedDateTimeUtc],
		CLDC.[CourseId],
		CLDC.[LearnDirectClassificationRef],
		CLDC.[ClassificationOrder]
	FROM CourseLearnDirectClassification CLDC
		INNER JOIN Course C ON C.CourseId = CLDC.CourseId
	WHERE NOT EXISTS (
						SELECT CourseId
						FROM Audit_CourseLearnDirectClassification
						WHERE AuditOperation = 'I'
							AND CourseID = CLDC.CourseId
							AND LearnDirectClassificationRef = CLDC.LearnDirectClassificationRef
					 );


	PRINT '[Adding Missing Location Audit Records]';
	SET IDENTITY_INSERT Audit_Location ON;

	INSERT INTO Audit_Location
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
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
	FROM Audit_Location 
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT LocationId,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_Location
								WHERE LocationId NOT IN (SELECT LocationId FROM Audit_Location WHERE AuditOperation = 'I')
								GROUP BY LocationId
								) A
						);

	SET IDENTITY_INSERT Audit_Location OFF;

	INSERT INTO Audit_Location 
	(
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT 'I',
		[CreatedDateTimeUtc],
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
	FROM Location
	WHERE LocationId NOT IN (SELECT DISTINCT LocationId FROM Audit_Location);


	PRINT '[Adding Missing Provider Audit Records]';
	SET IDENTITY_INSERT Audit_Provider ON;

	INSERT INTO Audit_Provider
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
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
		[FEChoicesLearner],
		[FEChoicesEmployer],
		[FEChoicesDestination],
		[FEChoicesUpdatedDateTimeUtc],
		[QualityEmailsPaused],
		[QualityEmailStatusId],
		[DFE1619Funded],
		[SFAFunded],
		[TrafficLightEmailDateTimeUtc],
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
		[RoATPFFLag],
		[LastAllDataUpToDateTimeUtc],
		[RoATPProviderTypeId],
		[RoATPStartDate],
		[MarketingInformationUpdatedDateUtc]
	)
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
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
		[FEChoicesLearner],
		[FEChoicesEmployer],
		[FEChoicesDestination],
		[FEChoicesUpdatedDateTimeUtc],
		[QualityEmailsPaused],
		[QualityEmailStatusId],
		[DFE1619Funded],
		[SFAFunded],
		[TrafficLightEmailDateTimeUtc],
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
		[RoATPFFLag],
		[LastAllDataUpToDateTimeUtc],
		[RoATPProviderTypeId],
		[RoATPStartDate],
		[MarketingInformationUpdatedDateUtc]
	FROM Audit_Provider 
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT ProviderId,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_Provider
								WHERE ProviderId NOT IN (SELECT ProviderId FROM Audit_Provider WHERE AuditOperation = 'I')
								GROUP BY ProviderId
								) A
						);

	SET IDENTITY_INSERT Audit_Provider OFF;

	INSERT INTO Audit_Provider 
	(
		[AuditOperation],
		[AuditDateUtc],
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
		[FEChoicesLearner],
		[FEChoicesEmployer],
		[FEChoicesDestination],
		[FEChoicesUpdatedDateTimeUtc],
		[QualityEmailsPaused],
		[QualityEmailStatusId],
		[DFE1619Funded],
		[SFAFunded],
		[TrafficLightEmailDateTimeUtc],
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
		[RoATPFFLag],
		[LastAllDataUpToDateTimeUtc],
		[RoATPProviderTypeId],
		[RoATPStartDate],
		[MarketingInformationUpdatedDateUtc]
	)
	SELECT 'I',
		[CreatedDateTimeUtc],
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
		[FEChoicesLearner],
		[FEChoicesEmployer],
		[FEChoicesDestination],
		[FEChoicesUpdatedDateTimeUtc],
		[QualityEmailsPaused],
		[QualityEmailStatusId],
		[DFE1619Funded],
		[SFAFunded],
		[TrafficLightEmailDateTimeUtc],
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
		[RoATPFFLag],
		[LastAllDataUpToDateTimeUtc],
		[RoATPProviderTypeId],
		[RoATPStartDate],
		[MarketingInformationUpdatedDateUtc]
	FROM Provider
	WHERE ProviderId NOT IN (SELECT DISTINCT ProviderId FROM Audit_Provider);


	PRINT '[Adding Missing ProviderUser Audit Records]';
	SET IDENTITY_INSERT Audit_ProviderUser ON;

	INSERT INTO Audit_ProviderUser
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
		[ProviderId],
		[UserId]
	)
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
		[ProviderId],
		[UserId]
	FROM Audit_ProviderUser 
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT ProviderId,
									UserId,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_ProviderUser APU
								WHERE NOT EXISTS (
													SELECT ProviderId 
													FROM Audit_ProviderUser 
													WHERE AuditOperation = 'I'
														AND ProviderId = APU.ProviderId 
														AND UserId = APU.UserId
												  )
								GROUP BY ProviderId,
									UserId
								) A
						);

	SET IDENTITY_INSERT Audit_ProviderUser OFF;

	INSERT INTO Audit_ProviderUser 
	(
		[AuditOperation],
		[AuditDateUtc],
		[ProviderId],
		[UserId]
	)
	SELECT 'I',
		COALESCE(U.ModifiedDateTimeUtc, U.CreatedDateTimeUtc),
		[ProviderId],
		[UserId]
	FROM ProviderUser PU
		INNER JOIN AspNetUsers U ON U.Id = PU.UserId
	WHERE NOT EXISTS (
						SELECT ProviderId 
						FROM Audit_ProviderUser
						WHERE AuditOperation = 'I'
							AND ProviderId = PU.ProviderId
							AND UserId = PU.UserId
					 );


	PRINT '[Adding Missing Venue Audit Records]';
	SET IDENTITY_INSERT Audit_Venue ON;

	INSERT INTO Audit_Venue
	(
		[AuditSeq],
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT [AuditSeq] * (-1),
		'I',
		[AuditDateUtc],
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
	FROM Audit_Venue 
	WHERE AuditSeq IN (
						SELECT AuditSeq 
						FROM (
								SELECT VenueId,
									Min(AuditSeq) AS AuditSeq
								FROM Audit_Venue
								WHERE VenueId NOT IN (SELECT VenueId FROM Audit_Venue WHERE AuditOperation = 'I')
								GROUP BY VenueId
								) A
						);

	SET IDENTITY_INSERT Audit_Venue OFF;

	INSERT INTO Audit_Venue 
	(
		[AuditOperation],
		[AuditDateUtc],
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
	SELECT 'I',
		[CreatedDateTimeUtc],
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
	FROM Venue
	WHERE VenueId NOT IN (SELECT DISTINCT VenueId FROM Audit_Venue);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('22F93C54-6776-4666-9919-BC21FCE88F4A');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '5C8464DB-6608-473E-8B7F-B1A03BF32400')
BEGIN
	PRINT '[Removing Link from Course Search Usage Statistics]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = '';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Shared_HeaderMenuPartial_SearchStatisticsMenuItem', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('5C8464DB-6608-473E-8B7F-B1A03BF32400');
END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '14A61688-72C3-4943-8216-EB76EBD5F70B')
BEGIN
	PRINT '[Adding blank site news alert message for providers]'
	SET NOCOUNT ON

	--Insert new blank site alert news message for providers
	INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'Alert/Provider'
           , null
           , ''
           , null
           , null
           , 'Populate to display site alert message for providers'
           , 127 /* All user contexts */
           , 1 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )

	INSERT INTO __RefactorLog (OperationKey) VALUES ('14A61688-72C3-4943-8216-EB76EBD5F70B')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '0225F96E-684C-409c-B88C-E058D9C3EFC9')
BEGIN
	PRINT '[Change QA Fail Email - Add new style fail details]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);
	SET @NewText = '<table><thead><tr><th>Provider / Apprenticeship</th><th>Reason(s) for Failure</th><th>Further Details of Unverifiable Claim</th><th>Further details about compliance fails</th><th>Style Failure Reason(s)</th><th>Further style fail details</th></tr></thead><tbody>'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_SendQAFailEmail_QAFailEmailHeaderRowHTML', @NewText, @NewText;
	SET @NewText = '<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Provider_SendQAFailEmail_QAFailEmailRowHTML', @NewText, @NewText;
	INSERT INTO __RefactorLog (OperationKey) VALUES ('0225F96E-684C-409c-B88C-E058D9C3EFC9');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '9E707291-5EFD-408b-9B3A-0D173FB419F4')
BEGIN
	PRINT '[Add new passed QA with failed style email template]';
	SET NOCOUNT ON;

	INSERT INTO [dbo].[EmailTemplate]
		([EmailTemplateId]
		,[EmailTemplateGroupId]
		,[Name]
		,[Description]
		,[Params]
		,[Subject]
		,[HtmlBody]
		,[TextBody]
		,[Priority]
		,[UserDescription])
	VALUES
		(48
		,9
		,'Provider Passed QA Checks with Style Failures'
		,'Provider notification that their data has passed QA checks with some styling failures.'
		,'%PROVIDERNAME%=Provider Name,%USERNAME%=User''s Name'
		,'Your apprenticeship data has passed quality assurance'
		,'<p>Your apprenticeship data has passed quality assurance. The following style issues still require attention: </p><p>%REASON%</p>'
		,'Your apprenticeship data has passed quality assurance. The following style issues still require attention: %REASON%'
		,1
		,'Sent to a provider when their apprenticeship data has passed quality assurance checks but still has style failures');

	INSERT INTO __RefactorLog (OperationKey) VALUES ('9E707291-5EFD-408b-9B3A-0D173FB419F4');

END;
GO



IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '4C5BEDCA-F3B4-44b7-89F4-20C94E98C981')
BEGIN
	PRINT '[Populate new provider trading name field from ukrlp table]';
	SET NOCOUNT ON;

	Update p
	set p.TradingName = isnull(u.TradingName,'')
	from Provider p 
	join ukrlp u on p.Ukprn = u.Ukprn

	INSERT INTO __RefactorLog (OperationKey) VALUES ('4C5BEDCA-F3B4-44b7-89F4-20C94E98C981');

END;
GO




IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '8868A8DE-181C-4b96-95DB-8247FF3FA259')
BEGIN
	PRINT '[Adding new help page on how to improve your data quality]'
	SET NOCOUNT ON

	--Insert new blank site alert news message for providers
	INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'Help/DataQuality'
           , 'Guidance for improving your data quality score'
           , '<div id="shortSummary">
<p><strong>Summaries:</strong> To achieve the highest quality score each course must have a summary of at least 200 characters in length, and each course must have a unique summary. The summary should provide an overview of the course content which will be useful for prospective students when making their learning choices.</p>
<p><strong>Top Tip:</strong> <em>make sure your course summary is at least 200 characters long</em></p>
<p><strong>Unique summaries:</strong> To achieve the highest quality score each course must be unique, as well as being at least 200 characters long.</p>
</div>
<div id="nonDistinctSummary">
<p><strong>Top Tips:</strong> <em>Make sure your course summaries are different for each course. Where courses are very similar e.g. the same subject at different levels, it only needs to be a very minor difference in the summary.</em></p>
</div>
<div id="learningAims">
<p><strong>Learning Aims/QAN Codes:</strong> You can achieve a maximum score in this area by providing a Learning Aim or QAN code for each course you upload to the Provider Portal. Using a Learning Aim not only provides accurate information to the user, but also simplifies the upload process as you have fewer fields to complete.</p>
<p><strong>Top Tip:</strong> <em>Adding a Learning Aim/QAN code will pre-populate some fields on the Provider Portal to save you time.</em></p>
</div>
<div id="futureStartDates">
<p><strong>Start dates:</strong> You are encouraged to set a specific start date for each appropriate course on the portal. This will allow users to find a course which matches their availability. In order to ensure that out-of-date courses are not displayed on the Course Directory, your quality score is penalised for courses which do not have a start date which is in the future. If a course is delivered on a roll on / roll off basis, you can use the description field instead of a specific date.</p>
<p><strong>Top Tip:</strong> <em>If your short courses run very frequently, it may be better to add a start date description rather than specific start dates. For example, &ldquo;Courses start every four weeks during term time&rdquo;. This will save you time and will mean that your courses don&rsquo;t go out of date.</em></p>
</div>
<div id="CourseUrls">
<p><strong>Course URLs:</strong> To help prospective students find out more information about your courses, you should include a URL which provides more detail for each course. To achieve the maximum quality score, each URL should unique to the course.</p>
<p><strong>Top Tip:</strong> <em>Use this field to direct prospective learners to the enrolment or information page specific to each course if you have one.</em></p>
</div>
<div id="EntryReq">
<p><strong>Entry requirements:</strong> We encourage you to include entry requirements for each course so that students can assess whether they are suitably qualified to take the course. To achieve the highest score in this section, each course must have a description of the entry requirements.</p>
</div>'
           , null
           , null
           , ''
           , 127 /* All user contexts */
           , 0 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )

	INSERT INTO __RefactorLog (OperationKey) VALUES ('8868A8DE-181C-4b96-95DB-8247FF3FA259')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'D86FE1A9-A6A4-4a10-AC72-0A4D5BFC4AC6')
BEGIN
	PRINT '[Give CanViewProviderUsers permission to ProviderUserRole ]';
	SET NOCOUNT ON;

	declare @CanViewProviderUsersPermission int = (Select PermissionId from Permission where PermissionName='CanViewProviderUsers')
	declare @ProviderUserRole nvarchar(128) = (Select id from AspNetRoles where Name='Provider User')
	insert into PermissionInRole (RoleId, PermissionId) values (@ProviderUserRole, @CanViewProviderUsersPermission)

	INSERT INTO __RefactorLog (OperationKey) VALUES ('D86FE1A9-A6A4-4a10-AC72-0A4D5BFC4AC6');

END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '0AC5E38C-F144-4aeb-B72C-331EE45DC605')
BEGIN
	PRINT '[Adding settings for long and short course due for update periods]'
	SET NOCOUNT ON

	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) 
		VALUES ('ShortCourseExpiringDays', '14', '14', 'System.Int32', 'Number of days beyond start date which a short course is flagged as due for update', 0);

	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) 
		VALUES ('LongCourseExpiringDays', '28', '28', 'System.Int32', 'Number of days beyond start date which a long course is flagged as due for update', 0);


	INSERT INTO __RefactorLog (OperationKey) VALUES ('0AC5E38C-F144-4aeb-B72C-331EE45DC605')
END
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '4DE4F9AD-23E0-4143-9F86-5F59BCB849A9')
BEGIN
	PRINT '[Updating Dashboard Language Fields]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)

	--Field names and help text for Provider Dashboard Report View Model
	SET @NewText='% overall quality score for your entry on the Provider Portal' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_Description_AutoAggregateQualityRating', @NewText, @NewText

	SET @NewText='% courses which have a unique overview of course content to attract prospective students (200+ characters).' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_Description_CoursesWithDistinctLongSummary', @NewText, @NewText

	SET @NewText='% opportunities with a start date in the future or with the start date description updated with any roll on / roll off options. This means the NCS is always up to date for the user and meets with their availability.' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_Description_CoursesWithFutureStartDates', @NewText, @NewText

	SET @NewText='% courses which have a ESFA-assigned Learning Aim/QAN code. This provides accurate Information to the user and simplifies the upload process meaning fewer fields to complete.' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_Description_CoursesWithLearningAims', @NewText, @NewText

	SET @NewText='% courses which have an overview of course content to attract prospective students (200+ characters).' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_Description_CoursesWithLongSummary', @NewText, @NewText

	SET @NewText='Date by which you must update your provision.' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_Description_DateNextUpdateDue', @NewText, @NewText

	SET @NewText='Date of most recent provider user login.' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_Description_LastProviderLoginDateTimeUtc', @NewText, @NewText

	SET @NewText='Name of most recent provider user.' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_Description_LastProviderLoginUserDisplayName', @NewText, @NewText

	SET @NewText='Date of most recently updated opportunity or course.' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_Description_LastUpdatingDateTimeUtc', @NewText, @NewText

	SET @NewText='User who made the most recent update.' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_Description_LastUpdatingUserDisplayName', @NewText, @NewText

	SET @NewText='The parent organisation(s) linked to this provider'
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_Description_ParentOrganisations', @NewText, @NewText

	SET @NewText='Date provider added to the portal.' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_Description_ProviderCreatedDateTimeUTC', @NewText, @NewText

	SET @NewText='Data Quality Score' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_AutoAggregateQualityRating', @NewText, @NewText

	SET @NewText='Courses by start date' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_CoursesChart', @NewText, @NewText

	SET @NewText='Unique Summaries' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_CoursesWithDistinctLongSummary', @NewText, @NewText

	SET @NewText='Start Dates' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_CoursesWithFutureStartDates', @NewText, @NewText

	SET @NewText='Learning Aims / QAN' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_CoursesWithLearningAims', @NewText, @NewText

	SET @NewText='Course Summaries' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_CoursesWithLongSummary', @NewText, @NewText

	SET @NewText='Next update due' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_DateNextUpdateDue', @NewText, @NewText

	SET @NewText='Last log in (date)' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_LastProviderLoginDateTimeUtc', @NewText, @NewText

	SET @NewText='Last log in (user)' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_LastProviderLoginUserDisplayName', @NewText, @NewText

	SET @NewText='Last updated (date)' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_LastUpdatingDateTimeUtc', @NewText, @NewText

	SET @NewText='Last updated (user)' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_LastUpdatingUserDisplayName', @NewText, @NewText

	SET @NewText='Opportunities by start date' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_OpportunitiesChart', @NewText, @NewText

	SET @NewText='Linked Parent Organisation' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_ParentOrganisations', @NewText, @NewText

	SET @NewText='Date provider added to portal' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'ProviderDashboardReportViewModel_DisplayName_ProviderCreatedDateTimeUTC', @NewText, @NewText

	--Label for recalculate score button on dashboard
	SET @NewText='Update Quality Scoring' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Report_Dashboard_RecalculateScore', @NewText, @NewText

	--Update SFA / DFE mouseover in provider header to show provider type, updated both DFE hint and label as spotted possible issue - hint and label were wrong way round in my DB.
	SET @NewText='This {0} provider receives SFA funding.' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Shared_ContextOrganisationHeaderPartial_SFAHint', @NewText, @NewText

	SET @NewText='DfE' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Shared_ContextProviderHeaderPartial_DfE', @NewText, @NewText

	SET @NewText='This {0} provider receives DfE EFA funding.' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Shared_ContextProviderHeaderPartial_DfEHint', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('4DE4F9AD-23E0-4143-9F86-5F59BCB849A9')
END
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'B0597981-04D9-4178-863E-F8B22C314ECC')
BEGIN
	PRINT '[Updating Language Data]'
	SET NOCOUNT ON

	DECLARE @NewText nvarchar(2000)

	--Field names and help text for Provider Dashboard Report View Model
	SET @NewText='<span style=\"font-size: smaller;\">Last uploaded by {0} on {1}.  Filename(s): {2}</span>' 
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Address_Index_LastUploadDetails', @NewText, @NewText

	INSERT INTO __RefactorLog (OperationKey) VALUES ('B0597981-04D9-4178-863E-F8B22C314ECC')
END
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'A3A86BE1-6ADC-418E-B6DB-7802D70BE6E8')
BEGIN
	PRINT '[Fixing Location Hierarchy]';
	SET NOCOUNT ON;

	UPDATE VenueLocation SET ParentVenueLocationId = 2397 WHERE VenueLocationId = 9093;
	UPDATE VenueLocation SET ParentVenueLocationId = 9 WHERE VenueLocationId = 2397;
	UPDATE VenueLocation SET ParentVenueLocationId = 16087 WHERE VenueLocationId IN (10947, 11658, 12924, 21027, 23348, 23538, 100162, 100163, 100164, 100165);
	UPDATE VenueLocation SET ParentVenueLocationId = 30038 WHERE VenueLocationId = 21036;
	UPDATE VenueLocation SET LocationName = 'GLASGOW', ParentVenueLocationId = 30041 WHERE VenueLocationId = 21037;
	UPDATE VenueLocation SET ParentVenueLocationId = 2397 WHERE VenueLocationId IN (21038, 100112, 100113);
	UPDATE VenueLocation SET ParentVenueLocationId = 23318 WHERE VenueLocationId = 21039;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('A3A86BE1-6ADC-418E-B6DB-7802D70BE6E8');
END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'AC2C6C3A-C568-4923-8352-F1DAD95BAC61')
BEGIN
	PRINT '[Setting MustHaveFullLocation Flag]';
	SET NOCOUNT ON;

	UPDATE DeliveryMode SET MustHaveFullLocation = 1 WHERE DeliveryModeId <> 1;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('AC2C6C3A-C568-4923-8352-F1DAD95BAC61');
END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'E13390C8-82B3-4DEF-8BBF-DC9882334E6A')
BEGIN
	PRINT '[Fix Issue With Config Settings Set to Double]';
	SET NOCOUNT ON;

	UPDATE ConfigurationSettings SET DataType = 'System.Decimal' WHERE DataType = 'System.Double';

	INSERT INTO __RefactorLog (OperationKey) VALUES ('E13390C8-82B3-4DEF-8BBF-DC9882334E6A');
END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'F5D37EDF-A650-4EF7-8670-8A50B47A6B3C')
BEGIN
	-- Set NCS Override Threshold Check Flag as we are changing what gets moved across and this may
	-- cause the data to fall below the threshold.  This flag is automatically reset after the overnight
	-- transfer is run
	PRINT '[Set NCS Override Threshold Check Flag]';
	SET NOCOUNT ON;

	UPDATE [dbo].ConfigurationSettings SET Value = 'true'  WHERE Name = 'NCSOverrideThresholdCheck';

	INSERT INTO __RefactorLog (OperationKey) VALUES ('F5D37EDF-A650-4EF7-8670-8A50B47A6B3C');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'A37640AB-7AAC-41AC-80E8-FB7A35642370')
BEGIN
	PRINT '[Updating Language Data]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);

	SET @NewText='Click on each bar to view content';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Report_Dashboard_ChartFooterOpportunities', @NewText, @NewText;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('A37640AB-7AAC-41AC-80E8-FB7A35642370');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'D5A3F117-6400-4BD1-AEE9-0236B8417D92')
BEGIN
	PRINT '[Adding New QA Style Failure Reason]';
	SET NOCOUNT ON;

	INSERT INTO QAStyleFailureReason (QAStyleFailureReasonId, Description, FullDescription, Ordinal, RecordStatusId) VALUES (7, 'Other', 'Other', 99, 2);
		
	INSERT INTO __RefactorLog (OperationKey) VALUES ('D5A3F117-6400-4BD1-AEE9-0236B8417D92');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'EC89DAA4-6058-4521-A1AB-38F299DC317C')
BEGIN
	PRINT '[Set RoATP Refresh Start and End Dates]';
	SET NOCOUNT ON;

	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) 
		VALUES ('RoATPRefreshStartDate', '01/04/2019', '01/09/2018', 'System.DateTime', 'The start date dd/mm/yyyy when we require providers to confirm they have refreshed their provision.', 0);

	INSERT INTO ConfigurationSettings (Name, Value, ValueDefault, DataType, Description, RequiresSiteRestart) 
		VALUES ('RoATPRefreshEndDate', '31/10/2019', '30/04/2019', 'System.DateTime', 'The end date dd/mm/yyyy by which we require providers to confirm they have refreshed their provision.', 0);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('EC89DAA4-6058-4521-A1AB-38F299DC317C');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'AD000974-3123-4465-9F1C-E000E8E44452')
BEGIN
	PRINT '[Set Regulated Standard Warning Text]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);

	SET @NewText = 'This standard (or part of this standard) requires training providers to be approved by the associated approval body. Please check the <a href="https://www.instituteforapprenticeships.org/apprenticeship-standards/" target="_blank">Institute for Apprenticeships</a> website for more information.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_EditApprenticeship_RegulatedStandard', @NewText, @NewText;
		
	INSERT INTO __RefactorLog (OperationKey) VALUES ('AD000974-3123-4465-9F1C-E000E8E44452');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '63D5FC81-7A4C-408E-9171-76B0AD8461F2')
BEGIN
	PRINT '[Set Regulated Standard Warning Text - New Url]';
	SET NOCOUNT ON;

	DECLARE @NewText nvarchar(2000);

	SET @NewText = 'This standard (or part of this standard) requires training providers to be approved by the associated approval body. Please check the <a href="https://www.instituteforapprenticeships.org/developing-new-apprenticeships/resources/regulated-occupations/" target="_blank">Institute for Apprenticeships</a> website for more information.';
	EXEC up_LanguageTextSetByQualifiedFieldName 1, 'Apprenticeship_EditApprenticeship_RegulatedStandard', @NewText, @NewText;
		
	INSERT INTO __RefactorLog (OperationKey) VALUES ('63D5FC81-7A4C-408E-9171-76B0AD8461F2');
END;
GO

