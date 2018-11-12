IF NOT EXISTS(SELECT * FROM ConfigurationSettings)
BEGIN	
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'SMTPServer', N'mail.tribalgroup.com', NULL, N'System.String', N'The SMTP server name', NULL, NULL, 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'SMTPServerPort', N'25', N'25', N'System.Int32', N'The port to connect to on the SMTP server', NULL, NULL, 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'SMTPUserName', N'', N'', N'System.String', N'The user name if the SMTP server requires authentication', NULL, NULL, 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'SMTPPassword', N'', N'', N'System.String', N'The password if the SMTP server requires authentication', NULL, NULL, 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'SMTPIsSecure', N'false', N'false', N'System.Boolean', N'When true the connection is via SSL sockets', NULL, NULL, 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'SiteName', N'Provider Portal', N'Provider Portal', N'System.String', N'The sites name', NULL, NULL, 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'AutomatedFromEmailName', N'Support Course Directory Provider Portal', N'Support Course Directory Provider Portal', N'System.String', N'The from email name used when emails are sent from the Provider Portal system.', NULL, N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'AuditLoginFailures', N'True', N'false', N'System.Boolean', N'When true failed logins by users will be logged to the event log', CAST(N'2014-06-09 12:25:32.017' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'AuditLoginSuccesses', N'True', N'false', N'System.Boolean', N'When true successful logins by users will be logged to the event log', CAST(N'2012-02-02 13:01:35.330' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'AutomatedFromEmailAddress', N'support@coursedirectoryproviderportal.org.uk', N'support@coursedirectoryproviderportal.org.uk', N'System.String', N'The from email address used when emails are sent from the Provider Portal system.', CAST(N'2012-01-29 16:46:28.100' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'AutoSiteLoginAllow', N'True', N'true', N'System.Boolean', N'When true the user is able at login to tick an option so they are remembered on future visits', CAST(N'2013-04-26 12:09:24.260' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'AutoSiteLoginValidPeriod', N'24', N'24', N'System.Int32', N'How long in hours before a user must log in again if they have asked the site to remember them.  This setting only has an effect when AutoSiteLoginAllow is true.', CAST(N'2013-04-22 09:52:43.447' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'CacheDataSets', N'False', N'true', N'System.Boolean', N'Set to true for best performance, when true data is cached by the site.  Set to false if data is routinely modified directly in the database.', CAST(N'2013-08-27 11:05:02.103' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'DisplayTimeZone', N'GMT Standard Time', N'GMT Standard Time', N'System.String', N'The time zone used when displaying dates and time', CAST(N'2012-03-23 11:49:52.917' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EventLoggingLevel', N'Informational', N'Informational', N'System.String', N'The type of errors that should be logged, the possible values are: Debugging, Informational, Warnings or Errors', CAST(N'2013-08-08 08:34:00.813' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EventLogPurgeAuditFailure', N'8760', N'8760', N'System.Int32', N'The age in hours of events to purge from the Event log.  Events older than the time in hours are deleted.', CAST(N'2012-01-01 00:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EventLogPurgeAuditSuccess', N'8760', N'8760', N'System.Int32', N'The age in hours of events to purge from the Event log.  Events older than the time in hours are deleted.', CAST(N'2012-01-01 00:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EventLogPurgeDebug', N'1', N'1', N'System.Int32', N'The age in hours of events to purge from the Event log.  Events older than the time in hours are deleted.', CAST(N'2012-01-01 00:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EventLoggingBufferSize', N'5', N'5', N'System.Int32', N'The number of events to cache before committing them to the database, the higher the number the less performance impact on the database, to have events written without caching (useful for debugging purposes) set to zero.', CAST(N'2012-01-27 14:05:58.680' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EventLogPurgeError', N'365', N'336', N'System.Int32', N'The age in hours of events to purge from the Event log.  Events older than the time in hours are deleted.', CAST(N'2013-07-30 15:07:01.720' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EventLogPurgeInformation', N'72', N'72', N'System.Int32', N'The age in hours of events to purge from the Event log.  Events older than the time in hours are deleted.', CAST(N'2012-01-01 00:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EventLogPurgeWarning', N'200', N'243', N'System.Int32', N'The age in hours of events to purge from the Event log.  Events older than the time in hours are deleted.', CAST(N'2013-07-30 15:07:12.170' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'FileUploadMaximumFileSize', N'1048576', N'1048756', N'System.Int32', N'The maximum file size accepted for storage in the file store system', CAST(N'2012-01-01 00:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'VirusScanType', N'InstalledOnAccessVirusScanner', N'InstalledOnAccessVirusScaner', N'System.String', N'Sets the type of virus scanner.  Options are ClamWin, Sophos, InstalledOnAccessVirusScanner, or None.  When set to ClamWin or Sophos the EXE path must be entered to the EXE of the program to scan the file and clamWin requires a path to the definitions file.  When virus scanner type is None no virus scanning takes place.', CAST(N'2013-04-16 13:55:34.380' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'VirusScanPath', N'C:\Program Files (x86)\ClamWin\bin\clamscan.exe', N'', N'System.String', N'The path to the virus scan EXE, needs setting when VirusCanType is Sophos or ClamWin', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'VirusScanDefinitionPath', N'C:\Program Files (x86)\ClamWin\db', N'', N'System.String', N'The path to the ClamWin definition files, only needs setting when VirusScanType is ClamWin', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'LongDateFormat', N'd MMMM yyyy', N'd MMMM yyyy', N'System.String', N'The format used for long dates, for example: 10 December 2008', CAST(N'2009-11-06 13:58:51.940' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EventLogPurgingEnabled', N'true', N'true', N'System.Boolean', N'When true the event log is purged of older entries as per the EventLogPurge times', NULL, NULL, 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'LoginValidPeriod', N'20', N'20', N'System.Int32', N'The number of minutes the credentials are valid for before they timeout for inactivity meaning the user needs to log in again.  This figure only applies when a user has logged in without using the ''remember me'' option', CAST(N'2013-08-07 15:19:05.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'LongDateTimeFormat', N'd MMMM yyyy HH:mm', N'd MMMM yyyy HH:mm', N'System.String', N'The format used for long dates with time, for example: 10 December 2008 10:33', CAST(N'2008-04-29 00:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'OffLine', N'false', N'false', N'System.Boolean', N'When true the site is off line. ', CAST(N'2012-04-27 07:05:53.670' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'OffLineButStillAllowIPs', N'127.0.0.1', NULL, N'System.String', N'A comma delimited list of IP addresses that may still access the system even if offline', CAST(N'2012-03-29 14:07:22.933' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'OffLinePageToShow', N'siteoffline.htm', N'siteoffline.htm', N'System.String', N'The web page to show to the user when the site is set off line.', CAST(N'2008-04-30 00:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'SMTPIsRetryPolicyEnabled', N'true', N'true', N'System.Boolean', N'When true emails that fail to be sent are persisted to storage and later will attempt to be sent again', NULL, NULL, 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'PageServingTimeWarningThreshold', N'200', N'200', N'System.Int32', N'In milliseconds, if a page takes longer to serve than the value in milliseconds details of the page are logged for investigation.', CAST(N'2013-07-30 15:06:20.930' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'PasswordComplexity', N'^.*(?=.{6,})(?=.*\d)(?=.*[a-zA-Z]).*$', N'^.*(?=.{6,})(?=.*\d)(?=.*[a-zA-Z]).*$', N'System.String', N'A regular expression that defines the complexity of the password required.', CAST(N'2008-04-29 00:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'PasswordEnableResetRequest', N'True', N'true', N'System.Boolean', N'If true users may request to reset their password.  An email is sent with instructions on how their password maybe reset.', CAST(N'2012-01-22 16:05:16.397' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'PasswordShowLockedOutWarning', N'true', N'true', N'System.Boolean', N'When true and an account is locked out a validation message is shown to the user telling them the account is locked.  When false no message is shown, this is better security as it doesn''t reveal the username must be correct.', CAST(N'2012-02-02 00:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'PasswordValidDays', N'0', N'0', N'System.Int32', N'The number of days a password is valid for before the user is forced to change it.  Entering 0 days will mean the password never expires', CAST(N'2010-01-12 10:27:44.207' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'RecipientsForErrorEmail', N'jon.ripley@tribalgroup.com', N'support.technology@tribalgroup.co.uk', N'System.String', N'The email address where error notifications should sent, if more than one email address required separate by a comma.', CAST(N'2014-06-20 09:02:35.063' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'ServerFarmListIPAddresses', N'192.168.0.100:8080,192.168.0.5:8080', NULL, N'System.String', N'A comma delimited list of all local IP addresses for each web server in a farm for this site, used for clearing caches across the farm', CAST(N'2012-03-31 16:09:33.040' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'ShortDateFormat', N'dd/MM/yyyy', N'dd/MM/yyyy', N'System.String', N'The format used for short date strings, for example: 10/10/2008', CAST(N'2008-04-29 00:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'ShortDateTimeFormat', N'dd/MM/yyyy HH:mm', N'dd/MM/yyyy HH:mm', N'System.String', N'The format used for short date with time strings, for example: 10/10/2008 10:23', CAST(N'2012-01-29 16:03:00.730' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'NameDisplayFormat', N'%FORENAME% %SURNAME%', N'%FORENAME% %SURNAME%', N'System.String', N'The format used to display the user name, valid placeholders are %FORENAME% %SURNAME%', CAST(N'2012-02-05 09:50:51.313' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'AllowSelfRegistration', N'False', N'False', N'System.Boolean', N'When true the site will show a link and allow new users to self-register.', CAST(N'2014-09-30 12:00:00.000' AS DateTime), NULL, 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'NameDisplayFormatWithTitle', N'%TITLE% %FORENAME% %SURNAME%', N'%TITLE% %FORENAME% %SURNAME%', N'System.String', N'The format used to display the user name, valid placeholders are %TITLE% %FORENAME% %SURNAME%', CAST(N'2012-02-05 09:50:51.313' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'SiteUrlForBackgroundTasks', N'http://localhost:8103', N'not set', N'System.String', N'The site Url, usually this is known automatically, however any background tasks such as emailing will not be able to retrieve the Url so will use this setting.', CAST(N'2012-09-18 10:19:40.167' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EnableAllBackgroundTasks', N'True', N'false', N'System.Boolean', N'When true the background task scheduled will run and process back ground tasks, set to false when debugging or for development if background tasks should be suppressed.', CAST(N'2012-09-18 10:23:28.550' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'LanguageXxxxIetf', N'af', N'', N'System.String', N'The language Ietf that when a browser visits with that language renders all fields as Xxxx.  Useful when developing to check all fields are language driven.', CAST(N'2013-04-12 09:51:39.810' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'LanguageKeyNamesIetf', N'ab', N'', N'System.String', N'The language Ietf that when a browser visits with that language renders all fields as the field key name as used in the database.  Useful when developing to check what the language fields are called.', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'AdminUserCanAddRoles', N'Developer;Admin User;Admin Superuser;Provider Superuser;Provider User;Organisation Superuser;Organisation User', N'', N'System.String', N'A semi-colon separated list of roles a user with the CanAddEditAdminUsers permission can create.', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'ProviderUserCanAddRoles', N'Provider User', N'', N'System.String', N'A semi-colon separated list of roles a user with the CanAddEditProviderUsers permission can create.', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'OrganisationUserCanAddRoles', N'Organisation User;Provider Superuser;Provider User', N'', N'System.String', N'A semi-colon separated list of roles a user with the CanAddEditOrganisationUsers permission can create.', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'OrganisationContextCanAddRoles', N'Organisation Superuser;Organisation User', N'', N'System.String', N'A semi-colon separated list of roles available in the Organisation context.', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'ProviderContextCanAddRoles', N'Provider Superuser;Provider User', N'', N'System.String', N'A semi-colon separated list of roles available in the Provider context.', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'AdminContextCanAddRoles', N'Developer;Admin User;Admin Superuser;Provider Superuser;Provider User;Organisation Superuser;Organisation User', N'', N'System.String', N'A semi-colon separated list of roles available in the Admin context.', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EmailOverrideEnabled', N'False', N'False', N'System.Boolean', N'When true all emails sent by the system will be redirected to the configured EmailOverrideRecipients unless the recipient is at the EmailOverrideSafeDomain.', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EmailOverrideSafeDomain', N'tribalgroup.com', N'tribalgroup.com', N'System.String', N'When EmailOverrideEnabled is true all emails to this domain will be delivered normally.', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'EmailOverrideRecipients', N'Development.ProviderPortal@tribalgroup.com', N'Development.ProviderPortal@tribalgroup.com', N'System.String', N'A semi-colon separated list of email addresses that system will redirect email to when EmailOverrideEnabled is true. These addresses must be at the configured EmailOverrideSafeDomain.', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES (N'LoginRedirectsToHomePage', N'True', N'False', N'System.Boolean', N'When set this option forces all users to the home page after they log in, this overrides the default behaviour where an unauthenticated user can visit a page that requires authentication, log in and be automatically taken to that page.', CAST(N'2013-01-03 12:21:40.300' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
END

if not exists (select 1 from [dbo].[ConfigurationSettings] where Name=N'VirtualDirectoryNameForStoringBulkUploadFiles')
Begin
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) 
	VALUES (N'VirtualDirectoryNameForStoringBulkUploadFiles', N'\\vm-sfancd-app03\UploadedFiles', N'\\vm-sfancd-app03\UploadedFiles', N'System.String', N'When user will perform Bulk upload operation, the source csv file will be stored in a virtual directory which may be mapped to a network/local drive.  This would be used for maintain the history of all the files user has uploaded.  Bulk upload history page will list all these file on user basis.', CAST(N'2014-12-17 10:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
End

if not exists (select 1 from [dbo].[ConfigurationSettings] where Name=N'NightlyCsvFilesLocation')
Begin
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) 
	VALUES (N'NightlyCsvFilesLocation', N'c:\Test', N'c:\Test', N'System.String',	 N'As part of night csv export, all 10 csv files would be created in this directory.', CAST(N'2015-01-02 10:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
End

if not exists (select 1 from [dbo].[ConfigurationSettings] where Name=N'ShortDateFormatFileName')
Begin
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) VALUES 
	(N'ShortDateFormatFileName', N'yyyyMMdd', N'yyyyMMdd', N'System.String', N'The format used for short date strings mainly to be used in filename, for example: 20081010', CAST(N'2008-04-29 00:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 0)
End


if not exists (select 1 from [dbo].[ConfigurationSettings] where Name=N'BulkUploadThresholdAcceptablePercent')
Begin
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) 
	VALUES (N'BulkUploadThresholdAcceptablePercent', '90', '90', N'System.Int32', N'During bulk upload if newly inserted data goes below this limit, need to have stop the upload process.', CAST(N'2015-01-06 10:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
End

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'UsageStatisticsVirtualDirectory')
BEGIN
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) 
	VALUES (N'UsageStatisticsVirtualDirectory', '/Content/Reports', '/Content/Reports', N'System.String', N'The URL path to the virtual/real directory where the usage statistics reports are held, omit any trailing slash.', CAST(N'2015-01-13 10:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) 
	VALUES (N'UsageStatisticsFilesLocation', '%BASE%\Content\Reports', '%BASE%\Content\Reports', N'System.String', N'A fully specified path to the directory where the usage stastics reports are held, use %BASE% to reference the application root directory.', CAST(N'2015-01-13 10:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SupportTeamEmailRecipients')
BEGIN
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) 
	VALUES (N'SupportTeamEmailRecipients', N'support@coursedirectoryproviderportal.org.uk', N'support@coursedirectoryproviderportal.org.uk', N'System.String', N'A semi-colon separated list of email addresses to send support team alerts to.', CAST(N'2015-01-13 10:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SupportTeamEmailName')
BEGIN
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) 
	VALUES (N'SupportTeamEmailName', N'Course Directory Provider Portal Support Team', N'Course Directory Provider Portal Support Team', N'System.String', N'The name to address support team alerts to.', CAST(N'2015-01-13 10:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'ProviderDashboardPdfRenderDelay')
BEGIN
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) 
	VALUES (N'ProviderDashboardPdfRenderDelay', '500', '500', N'System.Int32', N'The number of milliseconds to wait after the provider dashboard has rendered to begin the PDF export.', CAST(N'2015-01-19 10:00:00.000' AS DateTime), N'33e86032-9663-4831-984c-b46af51d3781', 1)
END

/* Configure default lanuage options */
IF NOT EXISTS(SELECT * FROM Language)
BEGIN	
	SET IDENTITY_INSERT [dbo].[Language] ON 
	INSERT [dbo].[Language] ([LanguageID], [IETF], [DefaultText], [LanguageFieldName], [SqlLanguageId], [IsDefaultLanguage]) VALUES (1, N'en', N'English', N'Table_Language_1', 1033, 1)
	SET IDENTITY_INSERT [dbo].[Language] OFF
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'DailyReportDecimalPlaces')
BEGIN
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) 
	VALUES (N'DailyReportDecimalPlaces', '0', '0', N'System.Int32', N'The number of decimal places to use for numeric columns on the Daily Report', CAST(N'2015-03-02 10:00:00.000' AS DateTime), N'99953842-6156-46DF-8C3B-D374D0BD4993', 0)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'NumberOfNightlyCsvFilesToRetain')
BEGIN
	INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart]) 
	VALUES (N'NumberOfNightlyCsvFilesToRetain', '4', '4', N'System.Int32', N'The number of weekly CSV files to retain', CAST(N'2015-05-08 10:00:00.000' AS DateTime), N'382685FE-0EA2-4C5E-87DB-6C78C85AE5F1', 0)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SADestination')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SADestination', N'https://sa.education.gov.uk/idp/profile/SAML2/POST/SSO', N'https://sa.education.gov.uk/idp/profile/SAML2/POST/SSO', N'System.String', N'The URI to post authentication requests to.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SAAssertionConsumerServiceUrl')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SAAssertionConsumerServiceUrl', N'https://coursedirectoryproviderportal.org.uk/SA/SAML/Accept', N'https://coursedirectoryproviderportal.org.uk/SA/SAML/Accept', N'System.String', N'The URI of the page that receives the SAML2 response from the SSO server.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SALandingPage')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SALandingPage', N'https://coursedirectoryproviderportal.org.uk/SA', N'https://coursedirectoryproviderportal.org.uk/SA', N'System.String', N'The URI of the landing page for SSO users.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SAServiceProviderEntityId')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SAServiceProviderEntityId', N'https://coursedirectoryproviderportal.org.uk/SA/SFANCD', N'https://coursedirectoryproviderportal.org.uk/SA/SFANCD', N'System.String', N'The URI of the service identifier.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SAX509Certificate')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SAX509Certificate', N'~\App_Data\X.509 Certificates\sa-prod-live.txt', N'~\App_Data\X.509 Certificates\sa-prod-live.txt', N'System.String', N'The location of the X.509 certificate for decoding SAML2 responses.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SAEnabled')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SAEnabled', N'true', N'true', N'System.Boolean', N'Whether SSO is enabled.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SALoggedOutNotification')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SALoggedOutNotification', N'https://sa.education.gov.uk/idp/endOfSession?SystemId=Post16CoursePortal&Status=LoggedOut', N'https://sa.education.gov.uk/idp/endOfSession?SystemId=Post16CoursePortal&Status=LoggedOut', N'System.String', N'The URI of the page to show when a SSO user logs out.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SATimedOutNotification')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SATimedOutNotification', N'https://sa.education.gov.uk/idp/endOfSession?SystemId=Post16CoursePortal&Status=TimedOut', N'https://sa.education.gov.uk/idp/endOfSession?SystemId=Post16CoursePortal&Status=TimedOut', N'System.String', N'The URI of the page to show when a SSO user times out.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SALoginValidPeriod')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SALoginValidPeriod', N'20', N'20', N'System.Int32', N'The number of minutes the credentials are valid for before they timeout for inactivity meaning the user needs to log in again.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SAUserAccountManagement')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SAUserAccountManagement', N'https://sa.education.gov.uk/ui/myAccount', N'https://sa.education.gov.uk/ui/myAccount', N'System.String', N'The URI of the page where a user can view and manage their account details.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SAUserChangePassword')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SAUserChangePassword', N'https://sa.education.gov.uk/ui/passwordChange', N'https://sa.education.gov.uk/ui/passwordChange', N'System.String', N'The URI of the page where a user can manage their account password.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SAUserRolePrimary')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SAUserRolePrimary', N'Provider Superuser', N'Provider Superuser', N'System.String', N'The role granted to the first user account at a Secure Access provider.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SAUserRoleSecondary')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SAUserRoleSecondary', N'Provider Superuser', N'Provider Superuser', N'System.String', N'The role granted to any subsequent user accounts for a Secure Access provider.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SAHomePage')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SAHomePage', N'https://sa.education.gov.uk', N'https://sa.education.gov.uk', N'System.String', N'The URI of the Secure Access home page.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'RequireUniqueProvisionNames')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'RequireUniqueProvisionNames', N'true', N'true', N'System.Boolean', N'When true this option requires all providers and organisations to have unique names.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'BulkUploadPollingSeconds')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'BulkUploadPollingSeconds', N'20', N'20', N'System.Int32', N'The frequency in seconds of the Bulk Upload service polling the upload queue for new uploaded files to process.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 0)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'BulkUploadErrorEventDelaySeconds')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'BulkUploadErrorEventDelaySeconds', N'300', N'300', N'System.Int32', N'The time period that the Bulk Upload system should wait after a non terminal error before retrying.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'BulkUploadErrorEventMaxRetries')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'BulkUploadErrorEventMaxRetries', N'10', N'10', N'System.Int32', N'The maximum number of times the Bulk Upload system should retry before quitting when a non-terminal error occurs.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'BulkUploadLargeFileSize')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'BulkUploadLargeFileSize', N'512000', N'512000', N'System.Int32', N'The size in bytes of a file that the bulk upload system should treat as a Large File', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'DASExportThresholdChecksEnabled')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'DASExportThresholdChecksEnabled', N'true', N'true', N'System.Boolean', N'When building data for export to the Apprenticeship API, do we sanity check the scale of the changes since the last export.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'DASExportThresholdCheckPercent')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'DASExportThresholdCheckPercent', N'75', N'75', N'System.Int32', N'Used when building data for export to the Apprenticeship API. The number of records in the export tables is compared to the number generated during the previous export. If the number falls by more than the specified percentage, the export is rolled back.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'DASOverrideThresholdCheck')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'DASOverrideThresholdCheck', N'false', N'false', N'System.Boolean', N'Used when building data for export to the Apprenticeship API. Setting the value to True forces through an export which has failed the threshold validation checks. The system resets the value to False after the export.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'NCSExportEnabled')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'NCSExportEnabled', N'true', N'true', N'System.Boolean', N'Enable export of data to the NCS and Public APIs?', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'NCSExportThresholdCheckPercent')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'NCSExportThresholdCheckPercent', N'75', N'75', N'System.Int32', N'Used when building data for export to the NCS and Public APIs. The number of records in the export tables is compared to the number generated during the previous export. If the number falls by more than the specified percentage, the export is rolled back.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'NCSOverrideThresholdCheck')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'NCSOverrideThresholdCheck', N'false', N'false', N'System.Boolean', N'Used when building data for export to the NCS and Public APIs. Setting the value to True forces through an export which has failed the threshold validation checks. The system resets the value to False after the export.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'NCSExportIncludeUCASData')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'NCSExportIncludeUCASData', N'false', N'false', N'System.Boolean', N'Used when building data for export to the NCS API. Setting the value to True includes UCAS data.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'NCSExportIncludeUCAS-PG-Data')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'NCSExportIncludeUCAS-PG-Data', N'false', N'false', N'System.Boolean', N'Used when building data for export to the NCS API. Setting the value to True includes UCAS Post Graduate data.', GetDate(), N'99953842-6156-46DF-8C3B-D374D0BD4993', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'VirtualDirectoryNameForStoringBulkUploadFiles')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'VirtualDirectoryNameForStoringBulkUploadFiles', N'\\SFA-TEST-S-VI\BulkUpload', N'\\SFA-TEST-S-VI\BulkUpload', N'System.String', N'When user will perform Bulk upload operation, the source csv file will be stored in a virtual directory which may be mapped to a network/local drive.  This would be used for maintain the history of all the files user has uploaded.  Bulk upload history page will list all these file on user basis.', GetDate(), N'33e86032-9663-4831-984c-b46af51d3781', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'VirtualDirectoryNameForStoringProviderImportFiles')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'VirtualDirectoryNameForStoringProviderImportFiles', N'\\SFA-TEST-S-VI\ProviderImport', N'\\SFA-TEST-S-VI\ProviderImport', N'System.String', N'When user will perform provider import operation, the source csv file will be stored in a virtual directory which may be mapped to a network/local drive.  This would be used for maintain the history of all the files user has uploaded.', GetDate(), N'33e86032-9663-4831-984c-b46af51d5789', 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'LARSImportTime')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'LARSImportTime', N'21:00', N'21:00', N'System.String', N'Time to check for new LARS file in the format HH:MM', GetDate(), N'33e86032-9663-4831-984c-b46af51d5789', 1);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'LARSImportUserId')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'LARSImportUserId', N'24314672-f766-47f1-98cb-ad9fc49f6e9d', N'24314672-f766-47f1-98cb-ad9fc49f6e9d', N'System.String', N'Id of user to use when importing LARS file', GetDate(), N'33e86032-9663-4831-984c-b46af51d5789', 1);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'LARSUrlAndFileName')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'LARSUrlAndFileName', N'https://hub.fasst.org.uk/Learning%20Aims/Downloads/Documents/{date}_LARS_{year}_MDB.zip', N'https://hub.fasst.org.uk/Learning%20Aims/Downloads/Documents/{date}_LARS_{year}_MDB.zip', N'System.String', N'Url to check for new LARS data file.  The placeholders {date} and {year} will be replaced during execution', GetDate(), N'33e86032-9663-4831-984c-b46af51d5789', 1);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'LARSLongTimeSinceImportEmailAddress')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'LARSLongTimeSinceImportEmailAddress', N'development.SFA@tribalgroup.com', N'development.SFA@tribalgroup.com', N'System.String', N'Email address to send warning email to when it has been too long since the last LARS file import.', GetDate(), N'33e86032-9663-4831-984c-b46af51d5789', 1);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'LARSDaysSinceLastImportBeforeSendingEmail')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'LARSDaysSinceLastImportBeforeSendingEmail', N'21', N'21', N'System.Int32', N'The number of days after last import to send a warning email', GetDate(), N'33e86032-9663-4831-984c-b46af51d5789', 1);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'LARSImportErrorEmailAddress')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'LARSImportErrorEmailAddress', N'development.SFA@tribalgroup.com', N'development.SFA@tribalgroup.com', N'System.String', N'The email address to send the automated LARS importer threw an exception email', GetDate(), N'33e86032-9663-4831-984c-b46af51d5789', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'LongCourseMinDurationWeeks')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'LongCourseMinDurationWeeks', N'12', N'12', N'System.Int32', N'The number of weeks a course needs to be in duration before being considered a long course', GetDate(), N'33e86032-9663-4831-984c-b46af51d5789', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'LongCourseMaxStartDateInPastDays')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'LongCourseMaxStartDateInPastDays', N'28', N'28', N'System.Int32', N'The number of days a long course can start prior to today', GetDate(), N'33e86032-9663-4831-984c-b46af51d5789', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'RoATPAPIImportTime')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'RoATPAPIImportTime', N'23:00', N'23:00', N'System.String', N'Time to import RoATP API data (in the format HH:MM)', GetDate(), N'33e86032-9663-4831-984c-b46af51d5789', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'MonthlyReportExcelTemplateFilename')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'MonthlyReportExcelTemplateFilename', N'\\vfiler-th\SFA_CDPP\ReportTemplates\MonthlyReportTemplate.xls', N'\\vfiler-th\SFA_CDPP\ReportTemplates\MonthlyReportTemplate.xls', N'System.String', N'Monthly report excel template filename', GetDate(), N'FC9D7130-E093-4C7F-84A1-B7470EBA0F6E', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'DFEStartDateReportExcelTemplateFilename')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'DFEStartDateReportExcelTemplateFilename', N'\\vfiler-th\SFA_CDPP\ReportTemplates\DFEStartDateReportTemplate.xlsx', N'\\vfiler-th\SFA_CDPP\ReportTemplates\DFEStartDateReportTemplate.xlsx', N'System.String', N'DFE Start Date report excel template filename', GetDate(), N'FC9D7130-E093-4C7F-84A1-B7470EBA0F6E', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SearchAPIRegionLevelPenalty')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SearchAPIRegionLevelPenalty', N'0.05', N'0.05', N'System.Decimal', N'Penalty applied to search API ranking per level of region', GetDate(), N'FC9D7130-E093-4C7F-84A1-B7470EBA0F6E', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'SearchAPIMaxRegionLevelPenalty')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SearchAPIMaxRegionLevelPenalty', N'50', N'50', N'System.Decimal', N'Maximum penalty applied to search API ranking when using region rather than venue', GetDate(), N'FC9D7130-E093-4C7F-84A1-B7470EBA0F6E', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name=N'MaxLocations')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'MaxLocations', N'250', N'250', N'System.Int32', N'Maximum number of apprenticeship locations per provider (this can be overridden by individual provider)', GetDate(), N'FC9D7130-E093-4C7F-84A1-B7470EBA0F6E', 1);
END;

IF EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name IN ('SearchAPICourseTitleMulitplier', 'SearchAPICourseSummaryMulitplier', 'SearchAPIQualificationTitleMulitplier'))
BEGIN
	DELETE FROM dbo.ConfigurationSettings WHERE Name IN ('SearchAPICourseTitleMulitplier', 'SearchAPICourseSummaryMulitplier', 'SearchAPIQualificationTitleMulitplier');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name = N'SearchAPICourseTitleMultiplier')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SearchAPICourseTitleMultiplier', N'1000', N'1000', N'System.Int32', N'Multiplier to be used by course search ranking for course title', GetDate(), N'FC9D7130-E093-4C7F-84A1-B7470EBA0F6E', 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name = N'SearchAPICourseSummaryMultiplier')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SearchAPICourseSummaryMultiplier', N'100', N'100', N'System.Int32', N'Multiplier to be used by course search ranking for course summary', GetDate(), N'FC9D7130-E093-4C7F-84A1-B7470EBA0F6E', 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationSettings] where Name = N'SearchAPIQualificationTitleMultiplier')
BEGIN
    INSERT [dbo].[ConfigurationSettings] ([Name], [Value], [ValueDefault], [DataType], [Description], [LastUpdated], [LastUpdatedBy], [RequiresSiteRestart])
    VALUES (N'SearchAPIQualificationTitleMultiplier', N'500', N'500', N'System.Int32', N'Multiplier to be used by course search ranking for qualification title', GetDate(), N'FC9D7130-E093-4C7F-84A1-B7470EBA0F6E', 0);
END;

