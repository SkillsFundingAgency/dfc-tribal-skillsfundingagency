CREATE TABLE [dbo].[ProviderTASRefresh](
    [TASRefreshId]   INT  IDENTITY(1,1) NOT NULL,
	[ImportBatchId] INT NULL,
	[ProviderId]	INT NOT NULL,
    [RefreshTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [RefreshUserId] NVARCHAR(128) NULL, 
    CONSTRAINT [PK_TASRefreshId] PRIMARY KEY CLUSTERED ([TASRefreshId] ASC), 
    CONSTRAINT [FK_TASRefresh_AspNetUsers] FOREIGN KEY ([RefreshUserId]) REFERENCES [AspNetUsers]([Id])
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProviderTASRefresh]  WITH CHECK ADD  CONSTRAINT [FK_TASRefresh_Provider] FOREIGN KEY([ProviderId]) REFERENCES [dbo].[Provider] ([ProviderId])
GO

ALTER TABLE [dbo].[ProviderTASRefresh] CHECK CONSTRAINT [FK_TASRefresh_Provider]
GO

ALTER TABLE [dbo].[ProviderTASRefresh]  WITH CHECK ADD  CONSTRAINT [FK_TASRefresh_Batch] FOREIGN KEY([ImportBatchId]) REFERENCES [dbo].[ImportBatch] ([ImportBatchId])
GO

ALTER TABLE [dbo].[ProviderTASRefresh] CHECK CONSTRAINT [FK_TASRefresh_Batch]
GO


