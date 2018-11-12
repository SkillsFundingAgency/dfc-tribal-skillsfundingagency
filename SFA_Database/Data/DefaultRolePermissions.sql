IF NOT EXISTS(SELECT * FROM AspNetRoles WHERE Id = '947CD027-FD8B-494D-97B3-FA512A20650A')
BEGIN
	INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [LanguageFieldName], [UserContextId]) VALUES (N'947CD027-FD8B-494D-97B3-FA512A20650A', 'Developer', 'A user who has all system permissions.', 'Account_RoleDescription_Developer', 4)
END

/* Make sure the developer role has everything */
DECLARE @PermissionId INT
DECLARE db_Cursor CURSOR FOR SELECT PermissionId FROM Permission
OPEN db_Cursor FETCH NEXT FROM db_Cursor INTO @PermissionId
WHILE @@FETCH_STATUS = 0
BEGIN
	IF NOT EXISTS(SELECT * FROM PermissionInRole PIR WHERE PIR.PermissionId = @PermissionId AND RoleId = '947CD027-FD8B-494D-97B3-FA512A20650A')
	BEGIN
		INSERT INTO PermissionInRole (PermissionId, RoleId) VALUES (@PermissionId, '947CD027-FD8B-494D-97B3-FA512A20650A')
	END
	FETCH NEXT FROM db_Cursor INTO @PermissionId
END
CLOSE db_cursor
DEALLOCATE db_Cursor

IF NOT EXISTS(SELECT * FROM AspNetRoles WHERE Id = '9E51B185-6FA5-4474-95A1-CF02DD523203')
BEGIN
	INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [LanguageFieldName], [UserContextId]) VALUES (N'9E51B185-6FA5-4474-95A1-CF02DD523203', 'Admin Superuser', 'A user who can see and change all Course Directory data.', 'Account_RoleDescription_AdminSuperuser', 4)
END

IF NOT EXISTS(SELECT * FROM AspNetRoles WHERE Id = 'D9B32EC6-4FC1-4685-98B5-606124924BDF')
BEGIN
	INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [LanguageFieldName], [UserContextId]) VALUES (N'D9B32EC6-4FC1-4685-98B5-606124924BDF', 'Admin User', 'A user who can see all Course Directory data but cannot change any of it.', 'Account_RoleDescription_AdminUser', 4)
END

IF NOT EXISTS(SELECT * FROM AspNetRoles WHERE Id = '5394B20B-1668-4D4C-AEE4-0FA057AC12B8')
BEGIN
	INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [LanguageFieldName], [UserContextId]) VALUES (N'5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 'Provider Superuser', 'A user who can see and change all data for their Provider.', 'Account_RoleDescription_ProviderSuperuser', 1)
END

IF NOT EXISTS(SELECT * FROM AspNetRoles WHERE Id = '1D0E704A-05CF-4CD9-9FE6-3DA453709306')
BEGIN
	INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [LanguageFieldName], [UserContextId]) VALUES (N'1D0E704A-05CF-4CD9-9FE6-3DA453709306', 'Provider User', 'A user who can see and change only provision data for their Provider. Provider Users cannot amend main provider details or manage users other than themselves.', 'Account_RoleDescription_ProviderUser', 1)
END

IF NOT EXISTS(SELECT * FROM AspNetRoles WHERE Id = '9176659E-1A37-4C74-A7E5-1A3B455DEDBB')
BEGIN
	INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [LanguageFieldName], [UserContextId]) VALUES (N'9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 'Organisation Superuser', 'A user who can see and change all data for their Organisation and can additionally see and change all data for any Providers who are members of their Organisation, unless member Providers have specifically forbidden this.', 'Account_RoleDescription_OrganisationSuperuser', 2)
END

IF NOT EXISTS(SELECT * FROM AspNetRoles WHERE Id = 'CE236751-D3B4-48A7-BADE-553EE12F39DA')
BEGIN
	INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [LanguageFieldName], [UserContextId]) VALUES (N'CE236751-D3B4-48A7-BADE-553EE12F39DA', 'Organisation User', 'A user who can see and change only provision data for their Organisation and can additionally see and change only provision data for any Providers who are members of their Organisation, unless the member Organisation has forbidden this. Organisation Users cannot amend main provider details or manage users other than themselves.', 'Account_RoleDescription_OrganisationUser', 2)
END


IF NOT EXISTS (SELECT * FROM [ProviderUserType])
BEGIN
	INSERT [dbo].[ProviderUserType] ([ProviderUserTypeId], [ProviderUserTypeName], [IsRelationshipManager], [IsInformationOfficer]) VALUES (1, N'Normal User', 0, 0)
	INSERT [dbo].[ProviderUserType] ([ProviderUserTypeId], [ProviderUserTypeName], [IsRelationshipManager], [IsInformationOfficer]) VALUES (2, N'Information Officer', 0, 1)
	INSERT [dbo].[ProviderUserType] ([ProviderUserTypeId], [ProviderUserTypeName], [IsRelationshipManager], [IsInformationOfficer]) VALUES (3, N'Relationship Manager', 1, 0)
	INSERT [dbo].[ProviderUserType] ([ProviderUserTypeId], [ProviderUserTypeName], [IsRelationshipManager], [IsInformationOfficer]) VALUES (4, N'NCS Course Lead', 0, 0)
	INSERT [dbo].[ProviderUserType] ([ProviderUserTypeId], [ProviderUserTypeName], [IsRelationshipManager], [IsInformationOfficer]) VALUES (5, N'DART', 0 ,0)
END

IF NOT EXISTS (SELECT * FROM [ProviderUserTypeInRole])
BEGIN
	INSERT INTO [ProviderUserTypeInRole] ([ProviderUserTypeId], [RoleId]) VALUES (1, '1D0E704A-05CF-4CD9-9FE6-3DA453709306')
	INSERT INTO [ProviderUserTypeInRole] ([ProviderUserTypeId], [RoleId]) VALUES (1, '5394B20B-1668-4D4C-AEE4-0FA057AC12B8')
	INSERT INTO [ProviderUserTypeInRole] ([ProviderUserTypeId], [RoleId]) VALUES (1, '9176659E-1A37-4C74-A7E5-1A3B455DEDBB')
	INSERT INTO [ProviderUserTypeInRole] ([ProviderUserTypeId], [RoleId]) VALUES (1, '947CD027-FD8B-494D-97B3-FA512A20650A')
	INSERT INTO [ProviderUserTypeInRole] ([ProviderUserTypeId], [RoleId]) VALUES (1, '9E51B185-6FA5-4474-95A1-CF02DD523203')
	INSERT INTO [ProviderUserTypeInRole] ([ProviderUserTypeId], [RoleId]) VALUES (1, 'CE236751-D3B4-48A7-BADE-553EE12F39DA')
	INSERT INTO [ProviderUserTypeInRole] ([ProviderUserTypeId], [RoleId]) VALUES (1, 'D9B32EC6-4FC1-4685-98B5-606124924BDF')
	INSERT INTO [ProviderUserTypeInRole] ([ProviderUserTypeId], [RoleId]) VALUES (2, '9E51B185-6FA5-4474-95A1-CF02DD523203')
	INSERT INTO [ProviderUserTypeInRole] ([ProviderUserTypeId], [RoleId]) VALUES (3, 'D9B32EC6-4FC1-4685-98B5-606124924BDF')
	INSERT INTO [ProviderUserTypeInRole] ([ProviderUserTypeId], [RoleId]) VALUES (4, 'D9B32EC6-4FC1-4685-98B5-606124924BDF')
	INSERT INTO [ProviderUserTypeInRole] ([ProviderUserTypeId], [RoleId]) VALUES (5, 'D9B32EC6-4FC1-4685-98B5-606124924BDF')
END

IF NOT EXISTS (SELECT * FROM Permission)
BEGIN	
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (1, N'CanViewHomePage', N'With this permission a user can view the standard home page')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (2, N'CanViewAdministratorHomePage', N'With this permission a user can view the administrator home page')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (3, N'CanViewProviderHomePage', N'With this permission a user can view the provider home page')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (4, N'CanViewOrganisationHomePage', N'With this permission a user can view the organisation home page')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (5, N'CanAddEditRoles', N'With this permission a user may add or edit roles and assign or remove permissions from roles')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (6, N'CanAddProvider', N'With this permission a user may add a new provider')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (7, N'CanEditProvider', N'With this permission a user may edit a provider')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (8, N'CanAddEditAdminUsers', N'With this permission a user can add and edit user accounts at the system level')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (9, N'CanViewAdminUsers', N'With this permission a user can view user accounts at the system level')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (10, N'CanAddEditProviderUsers', N'With this permission a user can add and edit user accounts at the provider level')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (11, N'CanViewProviderUsers', N'With this permission a user can view user accounts at the provider level')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (12, N'CanAddEditOrganisationUsers', N'With this permission a user can add and edit user accounts at the organisation level')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (13, N'CanViewOrganisationUsers', N'With this permission a user can view user accounts at the organisation level')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (14, N'CanEditEmailTemplates', N'With this permission a user can edit email templates')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (15, N'CanAddOrganisation', N'With this permission a user may add a new organisation')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (16, N'CanEditOrganisation', N'With this permission a user may edit an organisation')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (17, N'CanViewAdminReports', N'With this permission a user may view all admin reports')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (18, N'CanManuallyAuditProviders', N'With this permission a user may manually audit providers')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (19, N'CanViewOrganisationReports', N'With this permission a user may view organisation reports')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (20, N'CanViewProviderReports', N'With this permission a user may view provider reports')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (21, N'CanBulkUploadOrganisationFiles', N'With this permission a user may access the bulk upload screens at the organisation level')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (22, N'CanBulkUploadProviderFiles', N'With this permission a user may access the bulk upload screens at the provider level')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (23, N'CanManuallyAuditCourses', N'With this permission a user may manually audit courses')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (24, N'CanAddProviderVenue', N'With this permission a user may add a new venue')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (25, N'CanEditProviderVenue', N'With this permission a user may edit a venue')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (26, N'CanViewProviderVenue', N'With this permission a user may view a venue')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (27, N'CanAddProviderCourse', N'With this permission a user may add a new course')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (28, N'CanEditProviderCourse', N'With this permission a user may edit a course')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (29, N'CanViewProviderCourse', N'With this permission a user may view a course')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (30, N'CanManageCache', N'With this permission a user may manage the application cache')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (31, N'CanManageConfiguration', N'With this permission a user may manage the application configuration')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (32, N'CanAddProviderOpportunity', N'With this permission a user may add a new opportunity')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (33, N'CanEditProviderOpportunity', N'With this permission a user may edit an opportunity')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (34, N'CanViewProviderOpportunity', N'With this permission a user may view an opportunity')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (35, N'CanManageOrganisationProviderMembership', N'With this permission a user can manage provider memberships for an organisation')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (36, N'CanViewProvider', N'With this permission a user may view provider details')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (37, N'CanViewOrganisation', N'With this permission a user may view organisation details')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (38, N'IsPrimaryContact', N'With this permission a user is listed as a primary contact')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (39, N'CanManageProviderOrganisationMembership', N'With this permission a user can view and manage organisation memberships for a provider')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (40, N'CanDeleteProviderOpportunity', N'With this permission a user can delete opportunities for a provider')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (41, N'CanDeleteProviderCourse', N'With this permission a user can delete courses for a provider')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (42, N'CanDeleteProviderVenue', N'With this permission a user can delete venues for a provider')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (43, N'CanEditProviderSpecialFields', N'With this permission a user can edit special fields against a provider such as UKPRN and Contracting Body')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (44, N'CanEditOrganisationSpecialFields', N'With this permission a user can edit special fields against an organisation such as UKPRN and Contracting Body')
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (45, N'CanDeleteOrganisation', N'With this permission a user may delete an organisation')
END

IF NOT EXISTS (SELECT * FROM [PermissionInRole])
BEGIN
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 3)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 20)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 22)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 24)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 25)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 26)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 27)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 28)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 29)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 32)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 33)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 34)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 36)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 40)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 41)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('1D0E704A-05CF-4CD9-9FE6-3DA453709306', 42)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 3)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 6)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 7)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 10)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 11)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 15)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 20)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 22)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 24)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 25)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 26)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 27)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 28)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 29)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 32)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 33)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 34)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 36)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 37)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 38)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 39)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 40)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 41)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 42)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 3)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 4)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 6)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 7)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 10)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 11)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 12)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 13)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 16)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 19)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 20)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 21)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 22)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 24)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 25)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 26)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 27)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 28)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 29)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 32)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 33)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 34)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 35)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 36)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 37)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 38)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 40)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 41)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 42)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 45)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 1)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 2)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 3)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 4)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 5)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 6)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 7)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 8)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 9)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 10)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 11)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 12)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 13)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 14)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 15)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 16)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 17)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 18)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 19)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 20)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 21)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 22)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 23)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 24)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 25)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 26)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 27)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 28)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 29)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 30)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 31)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 32)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 33)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 34)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 35)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 36)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 37)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 38)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 39)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 40)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 41)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 42)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 43)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 44)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 45)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 1)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 2)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 3)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 4)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 6)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 7)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 8)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 9)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 10)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 11)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 12)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 13)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 14)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 15)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 16)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 17)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 18)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 19)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 20)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 21)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 22)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 23)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 24)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 25)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 26)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 27)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 28)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 29)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 32)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 33)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 34)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 35)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 36)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 37)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 38)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 39)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 40)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 41)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 42)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 43)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 44)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 45)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 3)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 4)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 11)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 13)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 19)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 20)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 21)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 22)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 24)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 25)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 26)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 27)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 28)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 29)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 32)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 33)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 34)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 36)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 37)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 40)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 41)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('CE236751-D3B4-48A7-BADE-553EE12F39DA', 42)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 1)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 2)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 3)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 4)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 9)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 11)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 13)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 17)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 19)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 20)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 23)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 26)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 29)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 34)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 35)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 36)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 37)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 39)
END

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanRecalculateQualityScores')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (46, N'CanRecalculateQualityScores', N'With this permission a user may recalculate provider or organisation quality scores')
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 46)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 46)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 46)
END

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanEditSecureAccessUsers')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (47, N'CanEditSecureAccessUsers', N'With this permission a user may edit Secure Access users')
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 47)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 47)
END

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanEditCourseSearchStats')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (48, N'CanEditCourseSearchStats', N'With this permission a user may upload and administer Course Search Usage Statistics files')
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 48)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 48)
END

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanManageLanguages')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (49, N'CanManageLanguages', N'With this permission a user may manage language resources')
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 49)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 49)
END

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanManageContent')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (55, N'CanManageContent', N'With this permission a user may manage site content')
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 55)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 55)
END

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanManageFiles')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (56, N'CanManageFiles', N'With this permission a user may manage content files')
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 56)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 56)
END

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanManageA10Codes')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) VALUES (57, N'CanManageA10Codes', N'With this permission a user may manage A10 codes')
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 57)
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 57)
END

IF NOT EXISTS (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanUploadUCASData')
BEGIN
	INSERT INTO Permission (PermissionId, PermissionName, PermissionDescription) VALUES (58, 'CanUploadUCASData', 'With this permission a user may view upload UCAS data');
	INSERT INTO PermissionInRole (PermissionId, RoleId) VALUES ((SELECT PermissionId FROM Permission WHERE PermissionName = 'CanUploadUCASData'), '947CD027-FD8B-494D-97B3-FA512A20650A');
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permission] where PermissionName=N'CanAddProviderLocation')
BEGIN
    INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription])
    VALUES (59, N'CanAddProviderLocation', N'With this permission a user may add locations.')
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (59, '9E51B185-6FA5-4474-95A1-CF02DD523203') -- Admin Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (59, 'D9B32EC6-4FC1-4685-98B5-606124924BDF') -- Admin User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (59, '947CD027-FD8B-494D-97B3-FA512A20650A') -- Developer
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (59, '9176659E-1A37-4C74-A7E5-1A3B455DEDBB') -- Organisation Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (59, 'CE236751-D3B4-48A7-BADE-553EE12F39DA') -- Organisation User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (59, '5394B20B-1668-4D4C-AEE4-0FA057AC12B8') -- Provider Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (59, '1D0E704A-05CF-4CD9-9FE6-3DA453709306') -- Provider User
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permission] where PermissionName=N'CanEditProviderLocation')
BEGIN
    INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription])
    VALUES (60, N'CanEditProviderLocation', N'With this permission a user may edit locations.')
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (60, '9E51B185-6FA5-4474-95A1-CF02DD523203') -- Admin Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (60, 'D9B32EC6-4FC1-4685-98B5-606124924BDF') -- Admin User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (60, '947CD027-FD8B-494D-97B3-FA512A20650A') -- Developer
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (60, '9176659E-1A37-4C74-A7E5-1A3B455DEDBB') -- Organisation Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (60, 'CE236751-D3B4-48A7-BADE-553EE12F39DA') -- Organisation User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (60, '5394B20B-1668-4D4C-AEE4-0FA057AC12B8') -- Provider Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (60, '1D0E704A-05CF-4CD9-9FE6-3DA453709306') -- Provider User
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permission] where PermissionName=N'CanViewProviderLocation')
BEGIN
    INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription])
    VALUES (61, N'CanViewProviderLocation', N'With this permission a user may view locations.')
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (61, '9E51B185-6FA5-4474-95A1-CF02DD523203') -- Admin Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (61, 'D9B32EC6-4FC1-4685-98B5-606124924BDF') -- Admin User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (61, '947CD027-FD8B-494D-97B3-FA512A20650A') -- Developer
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (61, '9176659E-1A37-4C74-A7E5-1A3B455DEDBB') -- Organisation Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (61, 'CE236751-D3B4-48A7-BADE-553EE12F39DA') -- Organisation User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (61, '5394B20B-1668-4D4C-AEE4-0FA057AC12B8') -- Provider Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (61, '1D0E704A-05CF-4CD9-9FE6-3DA453709306') -- Provider User
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permission] where PermissionName=N'CanDeleteProviderLocation')
BEGIN
    INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription])
    VALUES (62, N'CanDeleteProviderLocation', N'With this permission a user may delete locations.')
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (62, '9E51B185-6FA5-4474-95A1-CF02DD523203') -- Admin Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (62, 'D9B32EC6-4FC1-4685-98B5-606124924BDF') -- Admin User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (62, '947CD027-FD8B-494D-97B3-FA512A20650A') -- Developer
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (62, '9176659E-1A37-4C74-A7E5-1A3B455DEDBB') -- Organisation Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (62, 'CE236751-D3B4-48A7-BADE-553EE12F39DA') -- Organisation User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (62, '5394B20B-1668-4D4C-AEE4-0FA057AC12B8') -- Provider Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (62, '1D0E704A-05CF-4CD9-9FE6-3DA453709306') -- Provider User
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permission] where PermissionName=N'CanAddProviderApprenticeship')
BEGIN
    INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription])
    VALUES (63, N'CanAddProviderApprenticeship', N'With this permission a user may add apprenticeships.')
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (63, '9E51B185-6FA5-4474-95A1-CF02DD523203') -- Admin Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (63, 'D9B32EC6-4FC1-4685-98B5-606124924BDF') -- Admin User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (63, '947CD027-FD8B-494D-97B3-FA512A20650A') -- Developer
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (63, '9176659E-1A37-4C74-A7E5-1A3B455DEDBB') -- Organisation Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (63, 'CE236751-D3B4-48A7-BADE-553EE12F39DA') -- Organisation User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (63, '5394B20B-1668-4D4C-AEE4-0FA057AC12B8') -- Provider Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (63, '1D0E704A-05CF-4CD9-9FE6-3DA453709306') -- Provider User
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permission] where PermissionName=N'CanEditProviderApprenticeship')
BEGIN
    INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription])
    VALUES (64, N'CanEditProviderApprenticeship', N'With this permission a user may edit apprenticeships.')
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (64, '9E51B185-6FA5-4474-95A1-CF02DD523203') -- Admin Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (64, 'D9B32EC6-4FC1-4685-98B5-606124924BDF') -- Admin User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (64, '947CD027-FD8B-494D-97B3-FA512A20650A') -- Developer
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (64, '9176659E-1A37-4C74-A7E5-1A3B455DEDBB') -- Organisation Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (64, 'CE236751-D3B4-48A7-BADE-553EE12F39DA') -- Organisation User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (64, '5394B20B-1668-4D4C-AEE4-0FA057AC12B8') -- Provider Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (64, '1D0E704A-05CF-4CD9-9FE6-3DA453709306') -- Provider User
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permission] where PermissionName=N'CanViewProviderApprenticeship')
BEGIN
    INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription])
    VALUES (65, N'CanViewProviderApprenticeship', N'With this permission a user may view apprenticeships.')
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (65, '9E51B185-6FA5-4474-95A1-CF02DD523203') -- Admin Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (65, 'D9B32EC6-4FC1-4685-98B5-606124924BDF') -- Admin User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (65, '947CD027-FD8B-494D-97B3-FA512A20650A') -- Developer
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (65, '9176659E-1A37-4C74-A7E5-1A3B455DEDBB') -- Organisation Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (65, 'CE236751-D3B4-48A7-BADE-553EE12F39DA') -- Organisation User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (65, '5394B20B-1668-4D4C-AEE4-0FA057AC12B8') -- Provider Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (65, '1D0E704A-05CF-4CD9-9FE6-3DA453709306') -- Provider User
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permission] where PermissionName=N'CanDeleteProviderApprenticeship')
BEGIN
    INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription])
    VALUES (66, N'CanDeleteProviderApprenticeship', N'With this permission a user may delete apprenticeships.')
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (66, '9E51B185-6FA5-4474-95A1-CF02DD523203') -- Admin Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (66, 'D9B32EC6-4FC1-4685-98B5-606124924BDF') -- Admin User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (66, '947CD027-FD8B-494D-97B3-FA512A20650A') -- Developer
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (66, '9176659E-1A37-4C74-A7E5-1A3B455DEDBB') -- Organisation Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (66, 'CE236751-D3B4-48A7-BADE-553EE12F39DA') -- Organisation User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (66, '5394B20B-1668-4D4C-AEE4-0FA057AC12B8') -- Provider Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (66, '1D0E704A-05CF-4CD9-9FE6-3DA453709306') -- Provider User
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permission] where PermissionName=N'CanBulkUploadOrganisationApprenticeshipFiles')
BEGIN
    INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription])
    VALUES (67, N'CanBulkUploadOrganisationApprenticeshipFiles', N'With this permission a user may access the apprenticeship bulk upload screens at the organisation level')
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (67, '9E51B185-6FA5-4474-95A1-CF02DD523203') -- Admin Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (67, 'D9B32EC6-4FC1-4685-98B5-606124924BDF') -- Admin User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (67, '947CD027-FD8B-494D-97B3-FA512A20650A') -- Developer
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (67, '9176659E-1A37-4C74-A7E5-1A3B455DEDBB') -- Organisation Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (67, 'CE236751-D3B4-48A7-BADE-553EE12F39DA') -- Organisation User
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permission] where PermissionName=N'CanBulkUploadProviderApprenticeshipFiles')
BEGIN
    INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription])
    VALUES (68, N'CanBulkUploadProviderApprenticeshipFiles', N'With this permission a user may access the apprenticeship bulk upload screens at the provider level')
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (68, '9E51B185-6FA5-4474-95A1-CF02DD523203') -- Admin Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (68, 'D9B32EC6-4FC1-4685-98B5-606124924BDF') -- Admin User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (68, '947CD027-FD8B-494D-97B3-FA512A20650A') -- Developer
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (68, '9176659E-1A37-4C74-A7E5-1A3B455DEDBB') -- Organisation Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (68, 'CE236751-D3B4-48A7-BADE-553EE12F39DA') -- Organisation User
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (68, '5394B20B-1668-4D4C-AEE4-0FA057AC12B8') -- Provider Superuser
    INSERT INTO [PermissionInRole] ([PermissionId], [RoleId]) VALUES (68, '1D0E704A-05CF-4CD9-9FE6-3DA453709306') -- Provider User
END

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanManageDeliveryModes')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) 
	VALUES (69, N'CanManageDeliveryModes', N'With this permission a user may manage the list of apprenticeship location delivery modes')
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 69) --Developer
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 69) --Admin supervisor
END

DECLARE @NewRoleId UNIQUEIDENTIFIER = 'FE1CD530-C317-4DE5-B608-0CB1E4419305';
IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Id = @NewRoleId)
BEGIN
	INSERT INTO AspNetRoles (Id, Name, Description, LanguageFieldName, UserContextId) VALUES (@NewRoleId, 'Helpdesk', 'Role for helpdesk users with slightly higher permissions than Admin Superusers', 'Account_RoleDescription_Helpdesk', 4);
	INSERT INTO ProviderUserTypeInRole (ProviderUserTypeId, RoleId) VALUES (1, @NewRoleId);
	INSERT INTO ProviderUserTypeInRole (ProviderUserTypeId, RoleId) VALUES (2, @NewRoleId);

	-- Add the permissions
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 1);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 2);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 3);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 4);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 6);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 7);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 8);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 9);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 10);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 11);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 12);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 13);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 14);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 15);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 16);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 17);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 18);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 19);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 20);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 21);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 22);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 23);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 24);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 25);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 26);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 27);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 28);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 29);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 32);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 33);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 34);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 35);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 36);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 37);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 38);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 39);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 40);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 41);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 42);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 43);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 44);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 45);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 46);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 47);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 48);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 49);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 50);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 51);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 52);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 53);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 54);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 55);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 56);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 57);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 58);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 59);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 60);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 61);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 62);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 63);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 64);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 65);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 66);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 67);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 68);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 69);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES (@NewRoleId, 70);

	UPDATE ConfigurationSettings SET Value = 'Developer;Helpdesk;Admin User;Admin Superuser;Provider Superuser;Provider User;Organisation Superuser;Organisation User' WHERE Name = 'AdminUserCanAddRoles';
	UPDATE ConfigurationSettings SET Value = 'Developer;Helpdesk;Admin User;Admin Superuser;Provider Superuser;Provider User;Organisation Superuser;Organisation User' WHERE Name = 'AdminContextCanAddRoles';
END;

IF NOT EXISTS (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanUploadProviderData')
BEGIN
	INSERT INTO Permission (PermissionId, PermissionName, PermissionDescription) VALUES (71, 'CanUploadProviderData', 'With this permission a user may view upload provider data');
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 71); --Developer
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 71); --Admin supervisor
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('FE1CD530-C317-4DE5-B608-0CB1E4419305', 71); --Helpdesk
END;

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanQAApprenticeships')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) 
	VALUES (72, N'CanQAApprenticeships', N'With this permission a user may quality assure apprenticeships');
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 72); --Developer
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 72); --Admin supervisor
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 72); --Admin user
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('FE1CD530-C317-4DE5-B608-0CB1E4419305', 72); --Helpdesk
END;

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanViewApprenticeshipQA')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) 
	VALUES (73, N'CanViewApprenticeshipQA', N'With this permission a user may view apprenticeship quality assurance data');
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 73); --Developer
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 73); --Admin supervisor
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('FE1CD530-C317-4DE5-B608-0CB1E4419305', 73); --Helpdesk
END;

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanQAProviders')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) 
	VALUES (74, N'CanQAProviders', N'With this permission a user may quality assure providers');
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 74); --Developer
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 74); --Admin supervisor
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('FE1CD530-C317-4DE5-B608-0CB1E4419305', 74); --Helpdesk
END;

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanViewProviderQA')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) 
	VALUES (75, N'CanViewProviderQA', N'With this permission a user may view provider quality assurance data');
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 75); --Developer
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 75); --Admin supervisor
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 75); --Admin user
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('FE1CD530-C317-4DE5-B608-0CB1E4419305', 75); --Helpdesk
END;

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanViewApprenticeshipReports')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) 
	VALUES (76, N'CanViewApprenticeshipReports', N'With this permission a user may view quality assurance reports');
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 76); --Developer
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 76); --Admin supervisor
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 76); --Admin user
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('FE1CD530-C317-4DE5-B608-0CB1E4419305', 76); --Helpdesk
END;

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanSetAllCoursesUpToDate')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) 
	VALUES (77, N'CanSetAllCoursesUpToDate', N'With this permission a user can click the "All Courses up to Date" button');
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 77); --Developer
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 77); --Admin supervisor
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('D9B32EC6-4FC1-4685-98B5-606124924BDF', 77); --Admin user
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('FE1CD530-C317-4DE5-B608-0CB1E4419305', 77); --Helpdesk
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9176659E-1A37-4C74-A7E5-1A3B455DEDBB', 77); --Organisation Superuser
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('5394B20B-1668-4D4C-AEE4-0FA057AC12B8', 77); --Provider Superuser
END;

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanManageImportBatches')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) 
	VALUES (78, N'CanManageImportBatches', N'With this permission a user may manage import batch names');
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 78); --Developer
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 78); --Admin supervisor
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('FE1CD530-C317-4DE5-B608-0CB1E4419305', 78); --Helpdesk
END;

IF NOT EXISTS (SELECT 1 FROM Permission WHERE PermissionName = 'CanManuallyAssignImportBatches')
BEGIN
	INSERT [dbo].[Permission] ([PermissionId], [PermissionName], [PermissionDescription]) 
	VALUES (80, N'CanManuallyAssignImportBatches', N'With this permission a user may manually assign an import batch to a provider');
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 80); --Developer
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('9E51B185-6FA5-4474-95A1-CF02DD523203', 80); --Admin supervisor
    INSERT INTO [PermissionInRole] (RoleId, PermissionId) VALUES ('FE1CD530-C317-4DE5-B608-0CB1E4419305', 80); --Helpdesk
END;

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'EB34D8E1-D7C4-4D76-9CE3-C9319146C439')
BEGIN
	PRINT '[Adding Public API User Permissions]'
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT * FROM Permission WHERE PermissionId = 50)
	BEGIN
		INSERT INTO Permission (PermissionId, PermissionName, PermissionDescription) VALUES (50, 'CanViewPublicAPIUsers', 'With this permission a user can view the Public API users are their API Key');
		INSERT INTO Permission (PermissionId, PermissionName, PermissionDescription) VALUES (51, 'CanAddEditPublicAPIUsers', 'With this permission a user can add and edit the Public API users are their API Key');
	END;

	DELETE FROM PermissionInRole WHERE RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Developer') AND PermissionId = 50;
	DELETE FROM PermissionInRole WHERE RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Admin Superuser') AND PermissionId = 50;
	DELETE FROM PermissionInRole WHERE RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Helpdesk') AND PermissionId = 50;

	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES ((SELECT Id FROM AspNetRoles WHERE Name = 'Developer'), (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewPublicAPIUsers'));
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES ((SELECT Id FROM AspNetRoles WHERE Name = 'Admin Superuser'), (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewPublicAPIUsers'));
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES ((SELECT Id FROM AspNetRoles WHERE Name = 'Admin User'), (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanViewPublicAPIUsers'));
	
	DELETE FROM PermissionInRole WHERE RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Developer') AND PermissionId = 51;
	DELETE FROM PermissionInRole WHERE RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Admin Superuser') AND PermissionId = 51;
	DELETE FROM PermissionInRole WHERE RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Helpdesk') AND PermissionId = 51;

	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES ((SELECT Id FROM AspNetRoles WHERE Name = 'Developer'), (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddEditPublicAPIUsers'));
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES ((SELECT Id FROM AspNetRoles WHERE Name = 'Admin Superuser'), (SELECT PermissionId FROM Permission WHERE PermissionName = 'CanAddEditPublicAPIUsers'));

	INSERT INTO __RefactorLog (OperationKey) VALUES ('EB34D8E1-D7C4-4D76-9CE3-C9319146C439');
END;
GO


IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'EB66AC53-21A0-4638-B46F-2B92007189CD')
BEGIN
	PRINT '[Adding Override Maximum Number of Locations Permissions]'
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT * FROM Permission WHERE PermissionId = 81)
	BEGIN
		INSERT INTO Permission (PermissionId, PermissionName, PermissionDescription) VALUES (81, 'CanOverrideMaxLocations', 'With this permission a user can override the maximum number of locations for a specific provider');
	END;

	DELETE FROM PermissionInRole WHERE RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Developer') AND PermissionId = 81;
	DELETE FROM PermissionInRole WHERE RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Admin Superuser') AND PermissionId = 81;
	DELETE FROM PermissionInRole WHERE RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Helpdesk') AND PermissionId = 81;

	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES ((SELECT Id FROM AspNetRoles WHERE Name = 'Developer'), 81);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES ((SELECT Id FROM AspNetRoles WHERE Name = 'Admin Superuser'), 81);
	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES ((SELECT Id FROM AspNetRoles WHERE Name = 'Helpdesk'), 81);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('EB66AC53-21A0-4638-B46F-2B92007189CD');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '275565C7-353C-44C6-8E8E-EFB4CF6D76FB')
BEGIN
	PRINT '[Adding Search API Phrases Permissions]'
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT * FROM Permission WHERE PermissionId = 82)
	BEGIN
		INSERT INTO Permission (PermissionId, PermissionName, PermissionDescription) VALUES (82, 'CanEditAPISearchPhrases', 'With this permission a user can edit the course search API search phrases');
	END;

	DELETE FROM PermissionInRole WHERE RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Developer') AND PermissionId = 82;

	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES ((SELECT Id FROM AspNetRoles WHERE Name = 'Developer'), 82);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('275565C7-353C-44C6-8E8E-EFB4CF6D76FB');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '8C309F55-6D2D-47D4-AA83-C999D6F297A9')
BEGIN
	PRINT '[Adding Search API Stop Word Permissions]'
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT * FROM Permission WHERE PermissionId = 83)
	BEGIN
		INSERT INTO Permission (PermissionId, PermissionName, PermissionDescription) VALUES (83, 'CanEditStopWords', 'With this permission a user can edit the course search API stop words');
	END;

	DELETE FROM PermissionInRole WHERE RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Developer') AND PermissionId = 83;

	INSERT INTO PermissionInRole (RoleId, PermissionId) VALUES ((SELECT Id FROM AspNetRoles WHERE Name = 'Developer'), 83);

	INSERT INTO __RefactorLog (OperationKey) VALUES ('8C309F55-6D2D-47D4-AA83-C999D6F297A9');
END;
GO

