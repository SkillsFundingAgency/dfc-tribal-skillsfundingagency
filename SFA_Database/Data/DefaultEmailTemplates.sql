IF NOT EXISTS(SELECT * FROM [EmailTemplateGroup])
BEGIN	
	INSERT INTO [dbo].[EmailTemplateGroup]
			   ([EmailTemplateGroupId]
			   ,[Name]
			   ,[Description])
		 VALUES
			   (1
			   ,'Template'
			   ,'Master templates for all emails.')

	INSERT INTO [dbo].[EmailTemplateGroup]
			   ([EmailTemplateGroupId]
			   ,[Name]
			   ,[Description])
		 VALUES
			   (2
			   ,'User Accounts'
			   ,'User account emails.')
END
GO

IF NOT EXISTS(SELECT 1 FROM [EmailTemplateGroup] WHERE [EmailTemplateGroupId] = 3)
BEGIN
	INSERT INTO [dbo].[EmailTemplateGroup]
			   ([EmailTemplateGroupId]
			   ,[Name]
			   ,[Description])
		 VALUES
			   (3
			   ,'Bulk Upload'
			   ,'Bulk upload emails.')
END

IF NOT EXISTS(SELECT 1 FROM [EmailTemplateGroup] WHERE [EmailTemplateGroupId] = 4)
BEGIN
	INSERT INTO [dbo].[EmailTemplateGroup]
			   ([EmailTemplateGroupId]
			   ,[Name]
			   ,[Description])
		 VALUES
			   (4
			   ,'Provider'
			   ,'Provider emails.')
END

IF NOT EXISTS(SELECT 1 FROM [EmailTemplateGroup] WHERE [EmailTemplateGroupId] = 5)
BEGIN
	INSERT INTO [dbo].[EmailTemplateGroup]
			   ([EmailTemplateGroupId]
			   ,[Name]
			   ,[Description])
		 VALUES
			   (5
			   ,'Organisation'
			   ,'Organisation emails.')
END

IF NOT EXISTS(SELECT * FROM [EmailTemplate])
BEGIN	
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
			   (1
			   ,1
			   ,'Master Template'
			   ,'Master template for all emails any standard chrome, headers and footers should be placed here.'
			   ,'%SUBJECT%=Email subject, %NAME%=Recipient name, %HTMLBODY%=HTML formatted email body, %TEXTBODY%=Plain text formatted email body'
			   ,'[ProviderPortal-Test] %SUBJECT%'
			   ,'<p>Dear %NAME%,</p>
%HTMLBODY%
<p>Providers are contractually required to update their provision at least every three calendar months. To update your data, please visit <a href="https://www.coursedirectoryproviderportal.org.uk/">https://www.coursedirectoryproviderportal.org.uk</a>.</p>
<p>Please do not reply to this email. If you believe you have received this email erroneously or have further enquiries please contact the service desk on <a href="tel:0844 811 5073">0844 811 5073</a> or email <a href="mailto:support@coursedirectoryproviderportal.org.uk">support@coursedirectoryproviderportal.org.uk</a>.</p>
<p>Regards,<br/>
The Course Directory Provider Portal team</p>'
			   ,'Dear %NAME%,

%TEXTBODY%

Providers are contractually required to update their provision at least every three calendar months. To update your data, please visit https://www.coursedirectoryproviderportal.org.uk.

Please do not reply to this email. If you believe you have received this email erroneously or have further enquiries please contact the Course Directory Support Team on 0844 811 5073 or email support@coursedirectoryproviderportal.org.uk.

Regards,
The Course Directory Provider Portal team'
			   ,1
			   ,'Master template for all emails.')

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
			   (2
			   ,2
			   ,'Account Confirmation'
			   ,'New user account confirmation email.'
			   ,'%URL%=Account confirmation URL, %RESENT%=Additional text for subsequent sendings of this email'
			   ,'Confirm your account %RESENT%'
			   ,'<p>Please <a href="%URL%">confirm your account by clicking here</a>.</p>'
			   ,'Please confirm your account by visiting this link: %URL%'
			   ,1
			   ,'Sent when you need to confirm your email address.')

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
			   (3
			   ,2
			   ,'Password Reset'
			   ,'Password reset request.'
			   ,'%URL%=Password reset callback URL'
			   ,'Reset password'
			   ,'<p>A password reset was requested for your account. <a href="%URL%">Reset your password by clicking here</a>.</p>'
			   ,'To reset your password please visit this link: %URL%'
			   ,1
			   ,'Sent when you forget your password and request a password reset.')

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
			   (4
			   ,2
			   ,'New User Welcome'
			   ,'New user account welcome and account confirmation email.'
			   ,'%URL%=Account confirmation URL'
			   ,'Welcome to the Provider Portal'
			   ,'<p>Welcome to the Provider Portal.</p><p>Please <a href="%URL%">confirm your account by clicking here</a>.</p>'
			   ,'Welcome to the Provider Portal. Please confirm your account by visiting this link: %URL%'
			   ,1
			   ,'Sent when your account is first created and you need to confirm your email address and create a password.')

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
			   (5
			   ,2
			   ,'Enforced Password Reset'
			   ,'User must reset their password before they can log in.'
			   ,'%URL%=Password reset callback URL'
			   ,'Create Password'
			   ,'<p>Before you can log in to your account you must <a href="%URL%">create a password by clicking here</a>.</p>'
			   ,'Before you can log in to your account you must create a password by visiting this link: %URL%'
			   ,1
			   ,'Sent when you must reset your password in order to log in.')

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
			   (6
			   ,2
			   ,'Login Details Change Notification'
			   ,'User notification sent when login details have changed.'
			   ,'%OLDEMAIL%=Old email address, %NEWEMAIL%=New email address'
			   ,'Your login details have changed'
			   ,'<p>The email address you use to log in and receive notifications has changed from <a href="mailto:%OLDEMAIL%">%OLDEMAIL%</a> to <a href="mailto:%NEWEMAIL%">%NEWEMAIL%</a>. Before you can log in again you must confirm that you have access to this email address.</p>
<p>An account confirmation email has been sent to <a href="mailto:%NEWEMAIL%">%NEWEMAIL%</a>, please confirm your account by clicking the link in that email.</p>
<p>If you did not request this change to your account please contact the support team.</p>'
			   ,'The email address you use to log in and receive notifications has changed from %OLDEMAIL% to %NEWEMAIL%. Before you can log in again you must confirm that you have access to this email address.

An account confirmation email has been sent to %NEWEMAIL%, please confirm your account by clicking the link in that email.

If you did not request this change to your account please contact the support team.'
			   ,1
			   ,'Sent when you need to confirm your email address.')
END			   

--This bulk upload confirmation temple has been split into three separate templates,
--templateIds 38, 39 and 40

--if not exists (select 1 from [EmailTemplate] where EmailTemplateId=7)
--begin
--	INSERT INTO [dbo].[EmailTemplate]
--				([EmailTemplateId]
--				,[EmailTemplateGroupId]
--				,[Name]
--				,[Description]
--				,[Params]
--				,[Subject]
--				,[HtmlBody]
--				,[TextBody]
--				,[Priority]
--				,[UserDescription])
--			VALUES
--				(7
--				,3
--				,'Bulk Upload Failure'
--				,'User should be notified about the bulk upload summary after the process is successful.'
--				,'%TABLEDATA%=Bulk Upload'
--				,'Course Directory: CSV uploaded at %UPLOADEDDATETIME%, REQUIRES YOUR URGENT ATTENTION'
--				, '<p>Unfortunately your recent bulk upload %FILENAME% contains errors and the contents will <strong><span style="text-decoration: underline;">NOT</span></strong> be saved to the Course Directory.</p>'+
--						'<p>To view the list of errors and warnings please follow the instructions below:</p>'+
--						'<p>'+
--						'1. Login to the National Careers Service Course Directory Provider Portal (http://www.coursedirectoryproviderportal.org.uk).<br/>'+
--						'2. Click on the Bulk upload link at the top of the page.<br/>'+
--						'3. Click on the link titled "View previous upload status".<br/>'+
--						'4. Click on the file Copy of %FILENAME%.<br/>'+
--						'5. Review the errors/warnings and rectify as appropriate.<br/>'+
--						'</p>'+
--						'<p>If you need any assistance please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk.</p>'
--				,''
--				,1
--				,'Sent when you do a manual upload fails on any of stage 1,2 or 3.')
--end


--This bulk upload confirmation temple has been split into four separate templates,
--templateIds 32,33 and 34 and 35

--if not exists (select 1 from [EmailTemplate] where EmailTemplateId=8)
--begin	   	   
--	INSERT INTO [dbo].[EmailTemplate]
--			([EmailTemplateId]
--			,[EmailTemplateGroupId]
--			,[Name]
--			,[Description]
--			,[Params]
--			,[Subject]
--			,[HtmlBody]
--			,[TextBody]
--			,[Priority]
--			,[UserDescription])
--		VALUES
--			(8
--			,3
--			,'Bulk Upload Confirmation Required'
--			,'User should be notified about the bulk upload summary after in case a confirmation is required from the users side.'
--			,'%TABLEDATA%=Bulk Upload'
--			,'Course Directory: CSV uploaded at %UPLOADEDDATETIME% needs your confirmation to proceed.'
--			,'<p>Your recent bulk uploaded file %FILENAME% contains fewer valid records than you currently have on our database.</p>'+
--			'<p>You have %EXISTINGCOURSECOUNT% number courses and %EXISTINGOPPORTUNITYCOUNT% number opportunities on the existing database. Your newly uploaded file contains %NEWCOURSECOUNT% number courses (of which %INVALIDCOURSECOUNT% number contain errors) and %NEWOPPORTUNITYCOUNT% number opportunities (of which %INVALIDOPPORTUNITYCOUNT% number contain errors).</p>'+
--			'<p>In order to save your valid data to the Course Directory, you will need to confirm that you are aware that any errors within your file will <b><u>NOT</u></b> be published.</p>'+
--			'<p>To publish your file please follow the instructions below:</p>'+
--			'<p>'+
--			'1. Login to the Course Directory Provider Portal (http://www.coursedirectoryproviderportal.org.uk). <br/>'+
--			'2. Click on the Bulk upload link at the top of the page.  <br/>'+
--			'3. Click on the link titled "View previous upload status". <br/> '+
--			'4. Click on the file %FILENAME%.  <br/>'+
--			'5. Press the "Confirm" button.  <br/>'+
--			'</p>'+
--			'<p>Alternatively, if you would prefer to fix your errors before publishing this data, please do so within the CSV document and re-upload the file.</p>'+
--			'<p>If this file was incorrect then upload the correct file to replace it.</p>'+
--			'<p>If you require any assistance with this please contact the Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk.</p>'
--			,''
--			,1
--			,'Sent when manual bulk upload is fails on stage 4.')
--end

--This bulk upload confirmation temple has been split into three separate templates,
--templateIds 36 and 37

--if not exists (select 1 from [EmailTemplate] where EmailTemplateId=9)
--begin
--	INSERT INTO [dbo].[EmailTemplate]
--		([EmailTemplateId]
--		,[EmailTemplateGroupId]
--		,[Name]
--		,[Description]
--		,[Params]
--		,[Subject]
--		,[HtmlBody]
--		,[TextBody]
--		,[Priority]
--		,[UserDescription])
--	VALUES
--		(9
--		,3
--		,'Bulk Upload Success'
--		,'User should be notified about the bulk upload summary after the process is successful.'
--		,'%TABLEDATA%=Bulk Upload'
--		,'Course Directory: %FILENAME% uploaded at %UPLOADEDDATETIME% has been processed successfully'
--		,'<p>Your file %FILENAME% was uploaded at %UPLOADEDDATETIME% and processed successfully. %VALIDCOURSECOUNT% number out of %TOTALCOURSECOUNT% number courses have been validated and %VALIDOPPORTUNITYCOUNT% number out of %TOTALOPPORTUNITYCOUNT% number opportunities have also been validated.</p>' +
--		'<p>This data is now available to view at the Course Directory Provider Portal (http://www.coursedirectoryproviderportal.org.uk) and will be published to National Careers Service website shortly.</p>' +
--		'<p>We advise that you investigate and correct any errors in your upload. To do this please:</p>' +
--		'<p>1. Login to the National Careers Service Course Directory Provider Portal (http://www.coursedirectoryproviderportal.org.uk).<br />' +
--		'2. Click on the Bulk upload link at the top of the page.<br />' +
--		'3. Click on the link titled "View previous upload status.<br />' +
--		'4. Click on the file %FILENAME%.</p>' +
--		'<p>If you need any assistance please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk.</p>',''
--		,1
--		,'Sent when manual bulk upload is successful.')
--end

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 10)
BEGIN
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
		(10
		,4
		,'New Provider Notification'
		,'Admin notification that a new provider has been added to the system.'
		,'%PROVIDERNAME%=Provider Name,%ADDRESS%=Address'
		,'New Course Directory Provider Added'
		,'<p>A new Provider with the name %PROVIDERNAME% has been created in the Provider Portal. Their address details are: %ADDRESS.</p>'
		,'A new Provider with the name %PROVIDERNAME% has been created in the Provider Portal. Their address details are: %ADDRESS%.'
		,1
		,'Sent when a new provider is added to the portal.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 11)
BEGIN
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
		(11
		,5
		,'New Organisation Notification'
		,'Admin notification that a new organisation has been added to the system.'
		,'%ORGANISATIONNAME%=Organisation Name,%ADDRESS%=Address'
		,'New Course Directory Organisation Added'
		,'<p>A new Organisation with the name %ORGANISATIONNAME% has been created in the Provider Portal. Their address details are: %ADDRESS.</p>'
		,'A new Organisation with the name %ORGANISATIONNAME% has been created in the Provider Portal. Their address details are: %ADDRESS%.'
		,1
		,'Sent when a new organisation is added to the portal.')
END

-----------------------------------------------------------------------------

IF NOT EXISTS(SELECT 1 FROM [EmailTemplateGroup] WHERE [EmailTemplateGroupId] = 6)
BEGIN
	INSERT INTO [dbo].[EmailTemplateGroup]
			   ([EmailTemplateGroupId]
			   ,[Name]
			   ,[Description])
		 VALUES
			   (6
			   ,'Traffic Light Status'
			   ,'Traffic light status emails.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 12)
BEGIN
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
		(12
		,6
		,'Provider Traffic Light is Now Amber'
		,'Provider notification that their traffic light status has changed to amber.'
		,'%PROVIDERNAME%=Provider Name,%LASTUPDATEDATE%=Last Update Date'
		,'Your provision is now at Amber status'
		,'<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. This means it is two months since you last updated and you are now at Amber status.</p>'
		,'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. This means it is two months since you last updated and you are now at Amber status.'
		,1
		,'Sent on the day a provider''s traffic light status changes to amber.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 13)
BEGIN
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
		(13
		,6
		,'Provider Traffic Light Has Been Amber for One Week'
		,'Provider notification that their traffic light status has changed to amber.'
		,'%PROVIDERNAME%=Provider Name,%LASTUPDATEDATE%=Last Update Date'
		,'Your provision has been at Amber status for one week'
		,'<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You went into Amber status one week ago.</p>'
		,'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You went into Amber status one week ago.'
		,1
		,'Sent one week after a provider''s traffic light status changes to amber.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 14)
BEGIN
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
		(14
		,6
		,'Provider Traffic Light Will Be Red in One Week'
		,'Provider notification that their traffic light status will change to red in one week.'
		,'%PROVIDERNAME%=Provider Name,%LASTUPDATEDATE%=Last Update Date'
		,'Your provision will change to Red status in one week'
		,'<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are now at Amber status, and will change to Red status in one week.</p>'
		,'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are now at Amber status, and will change to Red status in one week.'
		,1
		,'Sent one week before a provider''s traffic light status changes to red.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 15)
BEGIN
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
		(15
		,6
		,'Provider Traffic Light is Now Red'
		,'Provider notification that their traffic light status has changed to red.'
		,'%PROVIDERNAME%=Provider Name,%LASTUPDATEDATE%=Last Update Date'
		,'Your provision is now at Red status'
		,'<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are now at Red status, since it has been over 3 months since you updated.</p>'
		,'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are now at Red status, since it has been over 3 months since you updated.'
		,1
		,'Sent on the day a provider''s traffic light status changes to red.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 16)
BEGIN
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
		(16
		,6
		,'Provider Traffic Light is Still Red'
		,'Provider notification that their traffic light status is still red.'
		,'%PROVIDERNAME%=Provider Name,%LASTUPDATEDATE%=Last Update Date'
		,'Your provision is still at Red status'
		,'<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are still at Red status, since it has been over 3 months since you updated.</p>'
		,'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are still at Red status, since it has been over 3 months since you updated.'
		,1
		,'Sent each week after a provider''s traffic light status changes to red.')
END

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '619E3ADD-85AD-4EA6-BC41-91919F7EB130')
BEGIN
	PRINT '[Updating SFA Provider Traffic Light Email Group]'
	SET NOCOUNT ON

	UPDATE [EmailTemplateGroup]
		SET [Name] = 'Traffic Light Status (SFA)'
			, [Description] = 'Traffic light status emails for SFA and SFA/DfE funded providers.'
	WHERE [EmailTemplateGroupId] = 6;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('619E3ADD-85AD-4EA6-BC41-91919F7EB130')
END
GO

-----------------------------------------------------------------------------

IF NOT EXISTS(SELECT 1 FROM [EmailTemplateGroup] WHERE [EmailTemplateGroupId] = 7)
BEGIN
	INSERT INTO [dbo].[EmailTemplateGroup]
			   ([EmailTemplateGroupId]
			   ,[Name]
			   ,[Description])
		 VALUES
			   (7
			   ,'Traffic Light Status (DfE)'
			   ,'Traffic light status emails for DfE funded only providers.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 17)
BEGIN
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
		(17
		,7
		,'Provider Traffic Light is Now Amber'
		,'Provider notification that their traffic light status has changed to amber.'
		,'%PROVIDERNAME%=Provider Name,%LASTUPDATEDATE%=Last Update Date'
		,'Your provision is now at Amber status'
		,'<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. This means you have not updated since May you and are now at Amber status.</p>'
		,'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. This means you have not updated since May and you are now at Amber status.'
		,1
		,'Sent on the day a provider''s traffic light status changes to amber.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 18)
BEGIN
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
		(18
		,7
		,'Provider Traffic Light Has Been Amber for One Week'
		,'Provider notification that their traffic light status has changed to amber.'
		,'%PROVIDERNAME%=Provider Name,%LASTUPDATEDATE%=Last Update Date'
		,'Your provision has been at Amber status for one week'
		,'<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You went into Amber status one week ago.</p>'
		,'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You went into Amber status one week ago.'
		,1
		,'Sent one week after a provider''s traffic light status changes to amber.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 19)
BEGIN
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
		(19
		,7
		,'Provider Traffic Light Will Be Red in One Week'
		,'Provider notification that their traffic light status will change to red in one week.'
		,'%PROVIDERNAME%=Provider Name,%LASTUPDATEDATE%=Last Update Date'
		,'Your provision will change to Red status in one week'
		,'<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are now at Amber status, and will change to Red status in one week.</p>'
		,'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are now at Amber status, and will change to Red status in one week.'
		,1
		,'Sent one week before a provider''s traffic light status changes to red.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 20)
BEGIN
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
		(20
		,7
		,'Provider Traffic Light is Now Red'
		,'Provider notification that their traffic light status has changed to red.'
		,'%PROVIDERNAME%=Provider Name,%LASTUPDATEDATE%=Last Update Date'
		,'Your provision is now at Red status'
		,'<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are now at Red status as you have not updated since May.</p>'
		,'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are now at Red status as you have not updated since May.'
		,1
		,'Sent on the day a provider''s traffic light status changes to red.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 21)
BEGIN
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
		(21
		,7
		,'Provider Traffic Light is Still Red'
		,'Provider notification that their traffic light status is still red.'
		,'%PROVIDERNAME%=Provider Name,%LASTUPDATEDATE%=Last Update Date'
		,'Your provision is still at Red status'
		,'<p>The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are still at Red status as you have not updated your provision.</p>'
		,'The last date you updated an opportunity on the Course Directory Provider Portal was %LASTUPDATEDATE%. You are still at Red status as you have not updated your provision.'
		,1
		,'Sent each week after a provider''s traffic light status changes to red.')
END

-----------------------------------------------------------------------------

IF NOT EXISTS(SELECT 1 FROM [EmailTemplateGroup] WHERE [EmailTemplateGroupId] = 8)
BEGIN
	INSERT INTO [dbo].[EmailTemplateGroup]
			   ([EmailTemplateGroupId]
			   ,[Name]
			   ,[Description])
		 VALUES
			   (8
			   ,'Organisation / Provider Relationship'
			   ,'Organisation and Provider relationship emails.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 22)
BEGIN
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
    (22
    ,8
    ,'Provider Invite Notification'
    ,'Provider notification sent when they are invited to join an organisation.'
    ,'%PROVIDERNAME%=Provider Name,%ORGANISATIONNAME%=Organisation Name'
    ,'You have been invited to join %ORGANISATIONNAME%'
    ,'<p>%ORGANISATIONNAME% has invited %PROVIDERNAME% to join their organisation, you can accept or decline this invitation by logging into the Course Directory Provider Portal and going to the Organisation pages. You can also control whether %ORGANISATIONNAME% is allowed to manage your provision data on your behalf.</p>'
    ,'%ORGANISATIONNAME% has invited %PROVIDERNAME% to join their organisation, you can accept or decline this invitation by logging into the Course Directory Provider Portal and going to the Organisation pages. You can also control whether %ORGANISATIONNAME% is allowed to manage your provision data on your behalf.'
    ,1
    ,'Sent when an organisation invites a provider to join their organisation.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 23)
BEGIN
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
    (23
    ,8
    ,'Provider Invite Withdrawn'
    ,'Provider notification sent when their invite to join an organisation is withdrawn.'
    ,'%PROVIDERNAME%=Provider Name,%ORGANISATIONNAME%=Organisation Name'
    ,'%ORGANISATIONNAME% has withdrawn their invitation'
    ,'<p>%ORGANISATIONNAME% has withdrawn their invitation for %PROVIDERNAME% to join their organisation.</p>'
    ,'%ORGANISATIONNAME% has withdrawn their invitation for %PROVIDERNAME% to join their organisation.'
    ,1
    ,'Sent when an organisation withdraws an invitation for a provider to join their organisation.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 24)
BEGIN
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
    (24
    ,8
    ,'Provider Removed From Organisation'
    ,'Provider notification sent when they are removed from an organisation.'
    ,'%PROVIDERNAME%=Provider Name,%ORGANISATIONNAME%=Organisation Name'
    ,'%ORGANISATIONNAME% has removed you from their organisation'
    ,'<p>%ORGANISATIONNAME% has removed %PROVIDERNAME% as a member of their organisation. If you had opted to allow then to manage your provision data they will no longer be able to do this.</p>'
    ,'%ORGANISATIONNAME% has removed %PROVIDERNAME% as a member of their organisation. If you had opted to allow then to manage your provision data they will no longer be able to do this.'
    ,1
    ,'Sent when an organisation removes a provider from their organisation.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 25)
BEGIN
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
    (25
    ,8
    ,'Provider Invite Accepted Can Edit'
    ,'Organisation notification sent when a provider has accepted an invitation and opted to allow the organisation to manage their data.'
    ,'%PROVIDERNAME%=Provider Name,%ORGANISATIONNAME%=Organisation Name'
    ,'%PROVIDERNAME% has accepted your invitation'
    ,'<p>%PROVIDERNAME% has accepted the invitation to join %ORGANISATIONNAME%. They have opted to allow you to view and edit their content.</p>'
    ,'%PROVIDERNAME% has accepted the invitation to join %ORGANISATIONNAME%. They have opted to allow you to view and edit their content.'
    ,1
    ,'Sent when a provider accepts an invitation to join an organisation and allows the organisation to manage their data.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 26)
BEGIN
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
    (26
    ,8
    ,'Provider Invite Accepted Cannot Edit'
    ,'Organisation notification sent when a provider has accepted an invitation and opted to not allow the organisation to manage their data.'
    ,'%PROVIDERNAME%=Provider Name,%ORGANISATIONNAME%=Organisation Name'
    ,'%PROVIDERNAME% has accepted your invitation'
    ,'<p>%PROVIDERNAME% has accepted the invitation to join %ORGANISATIONNAME%. They have opted not to allow you to view or edit their content. They may still run courses for your organisation.</p>'
    ,'%PROVIDERNAME% has accepted the invitation to join %ORGANISATIONNAME%. They have opted not to allow you to view or edit their content. They may still run courses for your organisation.'
    ,1
    ,'Sent when a provider accepts an invitation to join an organisation and does not allow the organisation to manage their data.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 27)
BEGIN
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
    (27
    ,8
    ,'Provider Invite Rejected'
    ,'Organisation notification sent when a provider invite is rejected.'
    ,'%PROVIDERNAME%=Provider Name,%ORGANISATIONNAME%=Organisation Name,%REASONS%=Reasons For Rejecting The Invitation'
    ,'%PROVIDERNAME% has declined your invitation'
    ,'<p>%PROVIDERNAME% has declined the invitation to join %ORGANISATIONNAME%. They gave the following reasons for declining your invitation: %REASONS%</p>'
    ,'%PROVIDERNAME% has declined the invitation to join %ORGANISATIONNAME%. They gave the following reasons for declining your invitation: %REASONS%'
    ,1
    ,'Sent when a provider declines an invite to join an organisation')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 28)
BEGIN
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
    (28
    ,8
    ,'Organisation Provider Deleted'
    ,'Organisation notification sent when a provider is deleted.'
    ,'%PROVIDERNAME%=Provider Name,%ORGANISATIONNAME%=Organisation Name'
    ,'%PROVIDERNAME% has been deleted'
    ,'<p>%PROVIDERNAME% has been deleted and is no longer active on the Course Directory Provider Portal.</p>'
    ,'%PROVIDERNAME% has been deleted and is no longer active on the Course Directory Provider Portal.'
    ,1
    ,'Sent to an organisation when a member provider is deleted')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 29)
BEGIN
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
    (29
    ,8
    ,'Provider Left Organisation'
    ,'Organisation notification sent when a provider leaves the organisation.'
    ,'%PROVIDERNAME%=Provider Name,%ORGANISATIONNAME%=Organisation Name,%REASONS%=Reasons For Leaving The Organisation'
    ,'%PROVIDERNAME% has left your organisation'
    ,'<p>%PROVIDERNAME% has left %ORGANISATIONNAME%. They gave the following reasons for leaving: %REASONS%</p>'
    ,'%PROVIDERNAME% has left %ORGANISATIONNAME%. They gave the following reasons for leaving: %REASONS%'
    ,1
    ,'Sent when a provider leaves an organisation.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 30)
BEGIN
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
    (30
    ,8
    ,'Provider Allowed Organisation To Manage Data'
    ,'Organisation notification sent when a provider allows the organisation to manage their data.'
    ,'%PROVIDERNAME%=Provider Name,%ORGANISATIONNAME%=Organisation Name'
    ,'%PROVIDERNAME% has allowed you to manage their data'
    ,'<p>%PROVIDERNAME% has opted to allow %ORGANISATIONNAME% to view and edit their content.</p>'
    ,'%PROVIDERNAME% has opted to allow %ORGANISATIONNAME% to view and edit their content.'
    ,1
    ,'Sent when a provider allows an organisation to manage their data.')
END

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 31)
BEGIN
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
    (31
    ,8
    ,'Provider Disallowed Organisation To Manage Data'
    ,'Organisation notification sent when a provider disallows the organisation to manage their data.'
    ,'%PROVIDERNAME%=Provider Name,%ORGANISATIONNAME%=Organisation Name'
    ,'%PROVIDERNAME% has declined to allow you to manage their data'
    ,'<p>%PROVIDERNAME% has opted out of allowing %ORGANISATIONNAME% to view and edit their content. They may still run courses for your organisation.</p>'
    ,'%PROVIDERNAME% has opted out of allowing %ORGANISATIONNAME% to view and edit their content. They may still run courses for your organisation.'
    ,1
    ,'Sent when a provider disallows an organisation to manage their data.')
END

if not exists (select 1 from [EmailTemplate] where EmailTemplateId=32)
begin	   	   
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
			(32
			,3
			,'Bulk Upload Course Threshold exceeded'
			,'User asked to confirm bulk upload of a Course data file where the number of records is significantly lower than the number currently held.'
			,'%TABLEDATA%=Bulk Upload'
			,'Course Directory: Course data upload at %UPLOADEDDATETIME% needs your confirmation to proceed.'
			,'<p>Your recent bulk uploaded file %FILENAME% contains fewer valid records than you currently have on our database.</p>'+
			'<p>You have %EXISTINGCOURSECOUNT% courses and %EXISTINGOPPORTUNITYCOUNT% opportunities on the existing database. Your newly uploaded file contains %NEWCOURSECOUNT% courses and %NEWOPPORTUNITYCOUNT% opportunities.</p>'+
			'<p>In order to save your data to the Course Directory you will need to confirm that you wish to upload the file.</p>'+
			'<p>To publish your file please follow the instructions below:</p>'+
			'<p>'+
			'1. Login to the Course Directory Provider Portal (http://www.coursedirectoryproviderportal.org.uk). <br/>'+
			'2. Click on the Bulk upload link at the top of the page.  <br/>'+
			'3. Click on the link titled "Course data". <br/> '+
			'4. Click on the link titled "View previous upload status". <br/> '+
			'5. Click on the file name %FILENAME%.  <br/>'+
			'6. Review any warnings against the file. <br/>'+
			'7. Press the "Confirm" button.  <br/>'+
			'</p>'+
			'<p>Alternatively, if you would prefer to fix the highlighted issues before publishing this data, please do so within the CSV document and re-upload the file.</p>'+
			'<p>If this file was incorrect then upload the correct file to replace it.</p>'+
			'<p>If you require any assistance with this please contact the Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk.</p>'
			,''
			,1
			,'Sent when the manual bulk upload file contains significantly less course data than is currently held in the system.')
end

if not exists (select 1 from [EmailTemplate] where EmailTemplateId=33)
begin	   	   
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
			(33
			,3
			,'Bulk Upload Apprenticeship Threshold Exceeded'
			,'User asked to confirm bulk upload of an Apprenticeship data file where the number of records is significantly lower than the number currently held.'
			,'%TABLEDATA%=Bulk Upload'
			,'Course Directory: Apprenticeship upload at %UPLOADEDDATETIME% needs your confirmation to proceed.'
			,'<p>Your recent bulk uploaded file %FILENAME% contains fewer valid records than you currently have on our database.</p>'+
			'<p>You have %EXISTINGAPPRENTICESHIPCOUNT% apprenticeships and %EXISTINGDELIVERYLOCATIONCOUNT% delivery locations on the existing database. Your newly uploaded file contains %NEWAPPRENTICESHIPCOUNT% apprenticeships and %NEWDELIVERYLOCATIONCOUNT% delivery locations.</p>'+
			'<p>In order to save your data to the Course Directory you will need to confirm that you wish to upload the file.</p>'+
			'<p>To publish your file please follow the instructions below:</p>'+
			'<p>'+
			'1. Login to the Course Directory Provider Portal (http://www.coursedirectoryproviderportal.org.uk). <br/>'+
			'2. Click on the Bulk upload link at the top of the page.  <br/>'+
			'3. Click on the link titled "Apprenticeship data". <br/> '+
			'4. Click on the link titled "View previous upload status". <br/> '+
			'5. Click on the file name %FILENAME%.  <br/>'+
			'6. Review any warnings against the file. <br/>'+
			'7. Press the "Confirm" button.  <br/>'+
			'</p>'+
			'<p>Alternatively,  if you would prefer to fix the highlighted issues before publishing the data, please do so within the CSV document and re-upload the file.</p>'+
			'<p>If this file was incorrect then upload the correct file to replace it.</p>'+
			'<p>If you require any assistance with this please contact the Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk.</p>'
			,''
			,1
			,'Sent when the manual bulk upload file contains significantly less apprenticeship data than is currently held in the system.')
end

if not exists (select 1 from [EmailTemplate] where EmailTemplateId=34)
begin	   	   
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
			(34
			,3
			,'Course Bulk Upload warnings'
			,'User asked to confirm the import of a bulk upload file of course data when warning messages have been generated during validation of the data.'
			,'%TABLEDATA%=Bulk Upload'
			,'Course Directory: Course data upload at %UPLOADEDDATETIME% needs your confirmation to proceed.'
			,'<p>The bulk upload process has generated warning message(s) for your recently uploaded file of course data: %FILENAME%.</p>'+
			'<p>In order to save your data to the Course Directory, you will need to confirm that you are aware that any data within your file will be published.</p>'+
			'<p>To publish your file please follow the instructions below:</p>'+
			'<p>'+
			'1. Login to the Course Directory Provider Portal (http://www.coursedirectoryproviderportal.org.uk). <br/>'+
			'2. Click on the Bulk upload link at the top of the page.  <br/>'+
			'3. Click on the link titled "Course data". <br/> '+
		    '4. Click on the link titled "View previous upload status". <br/> '+
			'5. Click on the file name %FILENAME%.  <br/>'+
			'6. Review the warnings against the file. <br/>'+
			'7. Press the "Confirm" button.  <br/>'+
			'</p>'+
			'<p>Alternatively, if you would prefer to fix the highlighted issues before publishing this data, please do so within the CSV document and re-upload the file.</p>'+
			'<p>If this file was incorrect then upload the correct file to replace it.</p>'+
			'<p>If you require any assistance with this please contact the Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk.</p>'
			,''
			,1
			,'Sent when the manual bulk upload of course data generated warning messages for the uploaded file.')
end

if not exists (select 1 from [EmailTemplate] where EmailTemplateId=35)
begin	   	   
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
			(35
			,3
			,'Apprenticeship Bulk Upload warnings'
			,'User asked to confirm the import of a bulk upload file of apprenticeship data when warning messages have been generated during validation of the data.'
			,'%TABLEDATA%=Bulk Upload'
			,'Course Directory: Apprenticeship upload at %UPLOADEDDATETIME% needs your confirmation to proceed.'
			,'<p>The bulk upload process has generated warning message(s) for your recently uploaded file of apprenticeship data: %FILENAME%.</p>'+
			'<p>In order to save your data to the Course Directory, you will need to confirm that you are aware that any data within your file will be published.</p>'+
			'<p>To publish your file please follow the instructions below:</p>'+
			'<p>'+
			'1. Login to the Course Directory Provider Portal (http://www.coursedirectoryproviderportal.org.uk). <br/>'+
			'2. Click on the Bulk upload link at the top of the page.  <br/>'+
			'3. Click on the link titled "Apprenticeship data". <br/> '+
		    '4. Click on the link titled "View previous upload status". <br/> '+
			'5. Click on the file name  %FILENAME%.  <br/>'+
			'6. Review the warnings against the file. <br/>'+
			'7. Press the "Confirm" button.  <br/>'+
			'</p>'+
			'<p>Alternatively, if you would prefer to fix the highlighted issues before publishing this data, please do so within the CSV document and re-upload the file.</p>'+
			'<p>If this file was incorrect then upload the correct file to replace it.</p>'+
			'<p>If you require any assistance with this please contact the Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk.</p>'
			,''
			,1
			,'Sent when the manual bulk upload of apprenticeship data generated warning messages for the uploaded file.')
end


if not exists (select 1 from [EmailTemplate] where EmailTemplateId=36)
begin
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
		(36
		,3
		,'Bulk Upload Course Success'
		,'User should be notified about the course bulk upload summary after the process is successful.'
		,'%TABLEDATA%=Bulk Upload'
		,'Course Directory: Course data uploaded at %UPLOADEDDATETIME% has been processed successfully'
		,'<p>Your file %FILENAME% was uploaded at %UPLOADEDDATETIME% and processed successfully. %TOTALCOURSECOUNT% courses and %TOTALOPPORTUNITYCOUNT% opportunities have been validated.</p>' +
		 '<p>If you need any assistance please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk.</p>'
		,''
		,1
		,'Sent when manual bulk upload of a file containing course data is successful.')
end

if not exists (select 1 from [EmailTemplate] where EmailTemplateId=37)
begin
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
		(37
		,3
		,'Bulk Upload Apprenticeship Success'
		,'User should be notified about the apprenticeship bulk upload summary after the process is successful.'
		,'%TABLEDATA%=Bulk Upload'
		,'Course Directory: Apprenticeship data uploaded at %UPLOADEDDATETIME% has been processed successfully'
		,'<p>Your file %FILENAME% was uploaded at %UPLOADEDDATETIME% and processed successfully. %TOTALAPPRENTICESHIPCOUNT% apprenticeships and %TOTALDELIVERYLOCATIONCOUNT% delivery locations have been validated.</p>' +
		 '<p>If you need any assistance please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk.</p>'
		,''
		,1
		,'Sent when manual bulk upload of a file containing course data is successful.')
end


if not exists (select 1 from [EmailTemplate] where EmailTemplateId=38)
begin
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
				(38
				,3
				,'Course Bulk Upload Failure'
				,'User should be notified about the bulk upload of course data after the process generates errors.'
				,'%TABLEDATA%=Bulk Upload'
				,'Course Directory: Course data upload at %UPLOADEDDATETIME%, REQUIRES YOUR URGENT ATTENTION'
				, '<p>Unfortunately your recent bulk upload of course data %FILENAME% contains errors and the contents will <strong><span style="text-decoration: underline;">NOT</span></strong> be saved to the Course Directory.</p>'+
						'<p>To view the list of errors and warnings please follow the instructions below:</p>'+
						'<p>'+
						'1. Login to the National Careers Service Course Directory Provider Portal (http://www.coursedirectoryproviderportal.org.uk).<br/>'+
						'2. Click on the Bulk upload link at the top of the page.<br/>'+
						'3. Click on the link titled "Course data".<br/>'+
						'4. Click on the link titled "View previous upload status".<br/>'+
						'5. Click on the file name %FILENAME%.<br/>'+
						'6. Review the errors/warnings and rectify as appropriate.<br/>'+
						'</p>'+
						'<p>If you need any assistance please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk.</p>'
				,''
				,1
				,'Sent when a manual upload of course data fails on stage 1,2,3 or 4')
end

if not exists (select 1 from [EmailTemplate] where EmailTemplateId=39)
begin
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
				(39
				,3
				,'Apprenticeship Bulk Upload Failure'
				,'User should be notified about the bulk upload of apprenticeship data after the process generates errors.'
				,'%TABLEDATA%=Bulk Upload'
				,'Course Directory: Apprenticeship data upload at %UPLOADEDDATETIME%, REQUIRES YOUR URGENT ATTENTION'
				, '<p>Unfortunately your recent bulk upload of apprenticeship data %FILENAME% contains errors and the contents will <strong><span style="text-decoration: underline;">NOT</span></strong> be saved to the Course Directory.</p>'+
						'<p>To view the list of errors and warnings please follow the instructions below:</p>'+
						'<p>'+
						'1. Login to the National Careers Service Course Directory Provider Portal (http://www.coursedirectoryproviderportal.org.uk).<br/>'+
						'2. Click on the Bulk upload link at the top of the page.<br/>'+
						'3. Click on the link titled "Apprenticeship data".<br/>'+
						'4. Click on the link titled "View previous upload status".<br/>'+
						'5. Click on the file name %FILENAME%.<br/>'+
						'6. Review the errors/warnings and rectify as appropriate.<br/>'+
						'</p>'+
						'<p>If you need any assistance please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk.</p>'
				,''
				,1
				,'Sent when a manual upload of apprenticeship data fails on stage 1,2,3 or 4')
end

if not exists (select 1 from [EmailTemplate] where EmailTemplateId=40)
begin
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
				(40
				,3
				,'Bulk Upload Failure - unknown file contents' 
				,'Used to notify the user of a bulk upload error when the system is unable to determine whether the file contains course or apprenticeship data'
				,'%TABLEDATA%=Bulk Upload'
				,'Course Directory: Data upload at %UPLOADEDDATETIME%, REQUIRES YOUR URGENT ATTENTION'
				, '<p>Unfortunately your recent bulk upload of data %FILENAME% contains errors and the contents will <strong><span style="text-decoration: underline;">NOT</span></strong> be saved to the Course Directory.</p>'+
						'<p>To view the list of errors and warnings please follow the instructions below:</p>'+
						'<p>'+
						'1. Login to the National Careers Service Course Directory Provider Portal (http://www.coursedirectoryproviderportal.org.uk).<br/>'+
						'2. Click on the Bulk upload link at the top of the page.<br/>'+
						'3. Click on either of the "Course data" or "Apprenticeship data" links.<br/>'+
						'4. Click on the link titled "View previous upload status".<br/>'+
						'5. Click on the file name %FILENAME%.<br/>'+
						'6. Review the errors/warnings and rectify as appropriate.<br/>'+
						'</p>'+
						'<p>If you need any assistance please contact the Course Directory Support Team on 0844 811 5073 or support@coursedirectoryproviderportal.org.uk.</p>'
				,''
				,1
				,'Sent when a manual upload of data fails and the system is unable to determine whether the file holds course or apprenticeship data')
end


IF NOT EXISTS(SELECT 1 FROM [EmailTemplateGroup] WHERE [EmailTemplateGroupId] = 9)
BEGIN
	INSERT INTO [dbo].[EmailTemplateGroup]
			   ([EmailTemplateGroupId]
			   ,[Name]
			   ,[Description])
		 VALUES
			   (9
			   ,'Quality Assurance'
			   ,'Quality assurance emails.');
END;


IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 41)
BEGIN
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
		(41
		,9
		,'Provider has Passed Quality Assurance Checks'
		,'Provider notification that their data has passed QA checks.'
		,'%PROVIDERNAME%=Provider Name,%USERNAME%=User''s Name'
		,'Your apprenticeship data has passed quality assurance'
		,'<p>Your apprenticeship data has passed quality assurance</p>'
		,'Your apprenticeship data has passed quality assurance.'
		,1
		,'Sent to a provider when their apprenticeship data has passed quality assurance checks');
END;

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 42)
BEGIN
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
		(42
		,9
		,'Provider has Failed Quality Assurance Checks'
		,'Provider notification that their data has failed QA checks.'
		,'%PROVIDERNAME%=Provider Name,%USERNAME%=User''s Name,%REASON%=Reason(s) for failure,%DETAILSOFUNVERIFIABLECLAIM%=Details of the unverifiable claim'
		,'Your apprenticeship data has failed quality assurance'
		,'<p>Your apprenticeship data has failed quality assurance</p><p>Reason(s) for failure<p><p>%REASON%</p>'
		,'Your apprenticeship data has failed quality assurance.  Reason(s) for failure: %REASON%'
		,1
		,'Sent to a provider when their apprenticeship data has failed quality assurance checks');
END;

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 43)
BEGIN
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
		(43
		,9
		,'Provider''s data is ready for QA'
		,'Notification to support when provider indicates that their data is ready for QA'
		,'%PROVIDERNAME%=Provider Name'
		,'Provider''s Apprenticeship Data Ready for QA'
		,'<p>The apprenticeship data for %PROVIDERNAME% is ready for Quality Assurance</p>'
		,'The apprenticeship data for %PROVIDERNAME% is ready for Quality Assurance'
		,1
		,'Sent to a support when a provider indicates that their data is ready for QA');
END;

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 44)
BEGIN
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
		(44
		,9
		,'Provider has submitted new text for QA'
		,'Notification to support that provider has submitted new text for QA'
		,'%PROVIDERNAME%=Provider Name, %NEWTEXT%=New text for QA'
		,'Provider has submitted new text for QA'
		,'<p>%PROVIDERNAME% has submittted new text ready for Quality Assurance:</p><p>%NEWTEXT%</p>'
		,'%PROVIDERNAME% has submittted new text ready for Quality Assurance: %NEWTEXT%'
		,1
		,'Sent to a support when a provider submits new text ready for QA');
END;


IF NOT EXISTS(SELECT 1 FROM [EmailTemplateGroup] WHERE [EmailTemplateGroupId] = 10)
BEGIN
	INSERT INTO [dbo].[EmailTemplateGroup]
			   ([EmailTemplateGroupId]
			   ,[Name]
			   ,[Description])
		 VALUES
			   (10
			   ,'Automation'
			   ,'Automated emails.');
END;


IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 45)
BEGIN
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
		(45
		,10
		,'Last LARS File Import x Days Ago'
		,'Notification to development that last LARS import was too long ago'
		,'%PROVIDERNAME%=Provider Name, %NEWTEXT%=New text for QA'
		,'Last LARS File Import %NUMBEROFDAYSSINCELASTIMPORT% Days Ago'
		,'<p>The LARS file has not been imported since %LASTIMPORT% (%NUMBEROFDAYSSINCELASTIMPORT% days).</p><p>The Provider Portal is configured to send an email after %CONFIGUREDNUMBEROFDAYS% days</p>'
		,'The LARS file has not been imported since %LASTIMPORT% (%NUMBEROFDAYSSINCELASTIMPORT% days). The Provider Portal is configured to send an email after %CONFIGUREDNUMBEROFDAYS% days'
		,1
		,'Notification to development that last LARS import was too long ago');
END;

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 46)
BEGIN
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
		(46
		,10
		,'Automated LARS Importer Threw an Exception'
		,'Notification to development that automated LARS importer threw an exception'
		,'%EXCEPTION%=The Exception Thrown, %STACKTRACE%=The stack trace, %FILENAME%=The name of the LARS file being imported'
		,'Automated LARS Importer Threw an Exception'
		,'<p>The automated LARS importer threw the exception: "%EXCEPTION%" whilst importing file %FILENAME%</p><p>Stack trace: %STACKTRACE%</p>'
		,'The automated LARS importer threw the exception: "%EXCEPTION%" whilst importing file %FILENAME%.   Stack trace: %STACKTRACE%.'
		,1
		,'Notification to development that automated LARS importer threw an exception');
END;

IF NOT EXISTS (SELECT 1 FROM [EmailTemplate] WHERE EmailTemplateId = 47)
BEGIN
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
		(47
		,10
		,'Automated RoATP Importer Threw an Exception'
		,'Notification to development that automated RoATP importer threw an exception'
		,'%EXCEPTION%=The Exception Thrown, %STACKTRACE%=The stack trace'
		,'Automated RoATP Importer Threw an Exception'
		,'<p>The automated RoATP importer threw the exception: "%EXCEPTION%"</p><p>Stack trace: %STACKTRACE%</p>'
		,'The automated RoATP importer threw the exception: "%EXCEPTION%".  Stack trace: %STACKTRACE%.'
		,1
		,'Notification to development that automated RoATP importer threw an exception');
END;
