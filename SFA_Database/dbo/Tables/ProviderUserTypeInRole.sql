CREATE TABLE [dbo].[ProviderUserTypeInRole]
(
	[ProviderUserTypeId] INT NOT NULL, 
    [RoleId] NVARCHAR(128) NOT NULL,
    PRIMARY KEY ([ProviderUserTypeId], [RoleId]), 
    CONSTRAINT [FK_ProviderUserTypeInRole_ProviderUserType] FOREIGN KEY ([ProviderUserTypeId]) REFERENCES [ProviderUserType]([ProviderUserTypeId]), 
	CONSTRAINT [FK_ProviderUserTypeInRole_AspNetRoles] FOREIGN KEY([RoleId]) REFERENCES [AspNetRoles] ([Id])    
)

