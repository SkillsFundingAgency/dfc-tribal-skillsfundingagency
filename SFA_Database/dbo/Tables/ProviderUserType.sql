CREATE TABLE [dbo].[ProviderUserType] (
    [ProviderUserTypeId]   INT           NOT NULL,
    [ProviderUserTypeName] NVARCHAR (50) NOT NULL,
	[IsRelationshipManager] bit NOT NULL,
	[IsInformationOfficer] bit NOT NULL,
    CONSTRAINT [PK_ProviderUserType] PRIMARY KEY CLUSTERED ([ProviderUserTypeId] ASC)
);

