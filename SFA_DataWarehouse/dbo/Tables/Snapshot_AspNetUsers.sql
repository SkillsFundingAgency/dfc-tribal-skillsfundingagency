CREATE TABLE [dbo].[Snapshot_AspNetUsers]
(
	[Period]				VARCHAR(7)			NOT NULL,
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
    [LegacyUserId]			INT					NOT NULL, -- LC: Legacy User Id contains the user Id they had in the Oracle database, we don't use this Id in the new schema
    [LastLoginDateTimeUtc]	DATETIME			NULL,
	[IsSecureAccessUser]	BIT					NOT NULL DEFAULT 0,
	[SecureAccessUserId]	INT					NULL,
    CONSTRAINT [PK_Snapshot_AspNetUsers] PRIMARY KEY ([Period], [Id])
);
GO

CREATE INDEX [IX_AspNetUsers_Email] ON [dbo].[Snapshot_AspNetUsers] ([Period], [Email]);
GO

CREATE INDEX [IX_AspNetUsers_UserName] ON [dbo].[Snapshot_AspNetUsers] ([Period], [UserName]);
GO
