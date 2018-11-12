CREATE TABLE [dbo].[PermissionInRole]
(
	[RoleId] NVARCHAR(128) NOT NULL , 
    [PermissionId] INT NOT NULL, 
    PRIMARY KEY ([PermissionId], [RoleId]), 
    CONSTRAINT [FK_PermissionInRole_Permission] FOREIGN KEY (PermissionId) REFERENCES [Permission]([PermissionId]), 
	CONSTRAINT [FK_PermissionInRole_AspNetRoles] FOREIGN KEY([RoleId]) REFERENCES [AspNetRoles] ([Id])    
)



