CREATE TABLE [dbo].[Audit_AspNetUsers]
(
	[AuditSeq]				INT				NOT NULL PRIMARY KEY IDENTITY, 
    [AuditOperation]		NVARCHAR		NOT NULL, 
    [AuditDateUtc]			DATETIME		NOT NULL DEFAULT GetUtcDate(), 
    [Id]					NVARCHAR(128)	NOT NULL, 
    [Email]					NVARCHAR(256)	NULL, 
    [EmailConfirmed]		BIT				NOT NULL, 
    [PasswordHash]			NVARCHAR(MAX)	NULL, 
    [SecurityStamp]			NVARCHAR(MAX)	NULL, 
    [PhoneNumber]			NVARCHAR(MAX)	NULL, 
    [PhoneNumberConfirmed]	BIT				NOT NULL, 
    [TwoFactorEnabled]		BIT				NOT NULL, 
    [LockoutEndDateUtc]		DATETIME		NULL, 
    [LockoutEnabled]		BIT				NOT NULL, 
    [AccessFailedCount]		INT				NOT NULL, 
    [UserName]				NVARCHAR(256)	NOT NULL, 
    [Name]					NVARCHAR(MAX)	NULL, 
    [AddressId]				INT				NULL, 
    [PasswordResetRequired]	BIT				NOT NULL, 
    [ProviderUserTypeId]	INT				NOT NULL, 
    [CreatedByUserId]		NVARCHAR(128)	NULL, 
    [CreatedDateTimeUtc]	DATETIME		NOT NULL, 
    [ModifiedByUserId]		NVARCHAR(128)	NULL, 
    [ModifiedDateTimeUtc]	DATETIME		NULL, 
    [IsDeleted]				BIT				NOT NULL, 
    [LegacyUserId]			INT				NOT NULL, 
    [LastLoginDateTimeUtc]	DATETIME		NULL,
	[IsSecureAccessUser]	BIT				NOT NULL DEFAULT 0,
	[SecureAccessUserId]	INT				NULL
)

GO

CREATE INDEX [IX_Audit_AspNetUsers_Id] ON [dbo].[Audit_AspNetUsers] ([Id])
