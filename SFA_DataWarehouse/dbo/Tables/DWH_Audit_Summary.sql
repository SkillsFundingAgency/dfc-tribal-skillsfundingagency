CREATE TABLE [dbo].[DWH_Audit_Summary]
(
	[TableName] NVARCHAR(100) NOT NULL, 
    [Id] INT NULL, 
    [Id2] INT NULL, 
	[Id3] VARCHAR(50) NULL,
    [AuditSeq] INT NOT NULL
    CONSTRAINT [PK_DWH_Audit_Summary] PRIMARY KEY ([TableName], [AuditSeq])
);

GO

CREATE INDEX [IX_DWH_Audit_Summary_TableName_Id_Id2] ON [dbo].[DWH_Audit_Summary] ([TableName], [Id], [Id2]);
GO

CREATE INDEX [IX_DWH_Audit_Summary_TableName_Id_Id3] ON [dbo].[DWH_Audit_Summary] ([TableName], [Id], [Id3]);
