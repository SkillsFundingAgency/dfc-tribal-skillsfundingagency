CREATE TABLE [dbo].[AspNetUsers](
	[Id]					[nvarchar](128)		NOT NULL,
	[Email]					[nvarchar](256)		NULL,
	[EmailConfirmed]		[bit]				NOT NULL,
	[PasswordHash]			[nvarchar](max)		NULL,
	[SecurityStamp]			[nvarchar](max)		NULL,
	[PhoneNumber]			[nvarchar](max)		NULL,
	[PhoneNumberConfirmed]	[bit]				NOT NULL,
	[TwoFactorEnabled]		[bit]				NOT NULL,
	[LockoutEndDateUtc]		[datetime]			NULL,
	[LockoutEnabled]		[bit]				NOT NULL,
	[AccessFailedCount]		[int]				NOT NULL,
	[UserName]				[nvarchar](256)		NOT NULL,
    [Name]					NVARCHAR(MAX)		NULL, 
    [AddressId]				INT					NULL, 
	[PasswordResetRequired] [bit]				NOT NULL DEFAULT 0,
	[ProviderUserTypeId]	INT					NOT NULL DEFAULT 1,
    [CreatedByUserId]		nvarchar(128)		NULL, -- This has to be nullable for ASP.Net Identity to initially create the user
    [CreatedDateTimeUtc]	DATETIME			NOT NULL DEFAULT GetUTCDate(),
    [ModifiedByUserId]		nvarchar(128)		NULL,
    [ModifiedDateTimeUtc]	DATETIME			NULL,
	[IsDeleted]				BIT					NOT NULL DEFAULT 0, 
    [LegacyUserId]			INT					NOT NULL IDENTITY (1,1), -- LC: Legacy User Id contains the user Id they had in the Oracle database, we don't use this Id in the new schema
    [LastLoginDateTimeUtc]	DATETIME			NULL,
	-- DfE Secure Access fields
	[IsSecureAccessUser]	BIT					NOT NULL DEFAULT 0,
	[SecureAccessUserId]	INT					NULL,	
	[ShowUserWizard] BIT NULL, 
    -- End of DfE Secure Access fields
	CONSTRAINT [FK_AspNetUsers_Address] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Address] ([AddressId]),
	CONSTRAINT [FK_AspNetUsers_ProviderUserType] FOREIGN KEY ([ProviderUserTypeId]) REFERENCES [dbo].[ProviderUserType] ([ProviderUserTypeId]),
    CONSTRAINT [FK_AspNetUsers_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]), 
    CONSTRAINT [FK_AspNetUsers_ModifiedByUserId] FOREIGN KEY ([ModifiedByUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TRIGGER [dbo].[Trigger_AspNetUsers_InsertUpdate]
    ON [dbo].[AspNetUsers]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_AspNetUsers (AuditOperation, Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, LockoutEndDateUtc, AccessFailedCount, UserName, Name, AddressId, PasswordResetRequired, ProviderUserTypeId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, IsDeleted, LegacyUserId, LastLoginDateTimeUtc, IsSecureAccessUser, SecureAccessUserId)
		(SELECT CASE WHEN EXISTS(SELECT * FROM DELETED) THEN 'U' ELSE 'I' END, Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, LockoutEndDateUtc, AccessFailedCount, UserName, Name, AddressId, PasswordResetRequired, ProviderUserTypeId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, IsDeleted, LegacyUserId, LastLoginDateTimeUtc, IsSecureAccessUser, SecureAccessUserId FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[Trigger_AspNetUsers_Delete]
    ON [dbo].[AspNetUsers]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON;
		INSERT INTO Audit_AspNetUsers (AuditOperation, Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, LockoutEndDateUtc, AccessFailedCount, UserName, Name, AddressId, PasswordResetRequired, ProviderUserTypeId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, IsDeleted, LegacyUserId, LastLoginDateTimeUtc, IsSecureAccessUser, SecureAccessUserId)
		(SELECT 'D', Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, LockoutEndDateUtc, AccessFailedCount, UserName, Name, AddressId, PasswordResetRequired, ProviderUserTypeId, CreatedByUserId, CreatedDateTimeUtc, ModifiedByUserId, ModifiedDateTimeUtc, IsDeleted, LegacyUserId, LastLoginDateTimeUtc, IsSecureAccessUser, SecureAccessUserId FROM deleted);
    END
GO

CREATE INDEX [IX_AspNetUsers_EmailConfirmed_IsDeleted] ON [dbo].[AspNetUsers] ([EmailConfirmed], [IsDeleted]) INCLUDE ([Id], [Email], [PhoneNumber], [Name])
GO

CREATE INDEX [IX_AspNetUsers_Email] ON [dbo].[AspNetUsers] ([Email])
GO

CREATE INDEX [IX_AspNetUsers_UserName] ON [dbo].[AspNetUsers] ([UserName])
GO
