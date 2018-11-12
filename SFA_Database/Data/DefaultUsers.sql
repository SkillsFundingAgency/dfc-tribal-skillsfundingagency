-- A default user is added to allow logging in for a newly created database
-- Username: admin@tribalgroup.com
-- Password: Adm!n*log7in


-- INSERT default Tribal users for initial access
IF NOT EXISTS(SELECT * FROM AspNetUsers)
BEGIN
INSERT INTO [dbo].[Address]
           ([AddressLine1]
           ,[AddressLine2]
           ,[Town]
           ,[County]
           ,[Postcode]
           ,[ProviderRegionId]
           ,[Latitude]
           ,[Longitude])
     VALUES
           ('St Mary''s Court'
           ,'55 St. Mary''s Road'
           ,'Sheffield'
           ,'South Yorkshire'
           ,'S2 4AN'
           ,1
           ,null
           ,null)
	DECLARE @AddressIdForUser INT = SCOPE_IDENTITY()
	
	INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Name], [AddressId], [PasswordResetRequired], [ProviderUserTypeId], [CreatedByUserId], [CreatedDateTimeUtc], [ModifiedByUserId], [ModifiedDateTimeUtc], [IsDeleted]) VALUES (N'cdddf5b6-bd30-4459-8b49-ad6c844bfe96', N'jon.ripley@tribaldefault.com', 1, N'APhkGGs1pMgm9rsnlnl4DAyTiMShfPOtcwuz4W5X/kjFtggt+4UIsfM0f1UFtOL8rw==', N'2f181aa2-8e43-48ea-9c50-66e6d54d6abf', NULL, 0, 0, NULL, 0, 1, N'jon.ripley@tribaldefault.com', N'Jon Ripley', @AddressIdForUser, 0, 1, N'24314672-f766-47f1-98cb-ad9fc49f6e9d', CAST(N'2014-10-22 11:08:51.360' AS DateTime), N'cdddf5b6-bd30-4459-8b49-ad6c844bfe96', CAST(N'2014-11-13 10:46:10.527' AS DateTime), 0)
	INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Name], [AddressId], [PasswordResetRequired], [ProviderUserTypeId], [CreatedByUserId], [CreatedDateTimeUtc], [ModifiedByUserId], [ModifiedDateTimeUtc], [IsDeleted]) VALUES (N'ae74c28b-f577-41bc-bd40-f3c41971b21a', N'steven.fulleylove@tribaldefault.com', 1, N'ANg1/icjIDZQ9Z1uvIF6RZZhImw2CuWk5oeCLCRjfEg1Ywd+3qN7gkV61pULQGHbDg==', N'997fe624-5be6-4004-bf1c-b819932887f4', NULL, 0, 0, NULL, 0, 0, N'steven.fulleylove@tribaldefault.com', N'Steven Fulleylove', @AddressIdForUser, 0, 2, N'24314672-f766-47f1-98cb-ad9fc49f6e9d', CAST(N'2014-10-22 11:08:51.360' AS DateTime), N'24314672-f766-47f1-98cb-ad9fc49f6e9d', CAST(N'2014-11-07 10:47:26.030' AS DateTime), 0)
	INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Name], [AddressId], [PasswordResetRequired], [ProviderUserTypeId], [CreatedByUserId], [CreatedDateTimeUtc], [ModifiedByUserId], [ModifiedDateTimeUtc], [IsDeleted]) VALUES (N'9ac50fb5-95f0-4dd1-b67b-e6e8b5fbd993', N'admin@tribalgroup.com', 1, N'AI0I1dquL8hhODgZ4GllVBkGbgKVKMday3fU7VosUuslwXN1NP+ENv02F1qn/PWyww==', N'9d4bb8d0-da5c-4f7e-bf0e-9ce3c12868cc', NULL, 0, 0, NULL, 0, 0, N'admin@tribaldefault.com', N'Admin User', @AddressIdForUser, 0, 2, N'24314672-f766-47f1-98cb-ad9fc49f6e9d', CAST(N'2014-10-22 11:08:51.360' AS DateTime), N'24314672-f766-47f1-98cb-ad9fc49f6e9d', CAST(N'2014-11-07 10:47:26.030' AS DateTime), 0)

	-- Put these all in the developer role
	INSERT INTO [AspNetUserRoles](RoleId, UserId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 'cdddf5b6-bd30-4459-8b49-ad6c844bfe96')
	INSERT INTO [AspNetUserRoles](RoleId, UserId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', 'ae74c28b-f577-41bc-bd40-f3c41971b21a')
	INSERT INTO [AspNetUserRoles](RoleId, UserId) VALUES ('947CD027-FD8B-494D-97B3-FA512A20650A', '9ac50fb5-95f0-4dd1-b67b-e6e8b5fbd993')

END