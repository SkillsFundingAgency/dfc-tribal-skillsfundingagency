CREATE TABLE [dbo].[UserProvisionHistory]
(
	[UserProvisionHistoryId]	int				IDENTITY (1, 1) NOT NULL,
	[UserId]					nvarchar(128)	NOT NULL,
	[ProviderId]				int				NULL,
	[OrganisationId]			int				NULL,
	[DisplayOrder]				int				NOT NULL,
	CONSTRAINT [PK_UserProvisionHistory] PRIMARY KEY CLUSTERED ( [UserProvisionHistoryId] ASC ) ,
    CONSTRAINT [FK_UserProvisionHistory_Organisation] FOREIGN KEY ([OrganisationId]) REFERENCES [dbo].[Organisation] ([OrganisationId]),
    CONSTRAINT [FK_UserProvisionHistory_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[Provider] ([ProviderId]),
    CONSTRAINT [FK_UserProvisionHistory_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
)
