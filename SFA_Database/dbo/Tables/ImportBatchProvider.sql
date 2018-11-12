CREATE TABLE [dbo].[ImportBatchProvider](
	[ImportBatchId] INT NOT NULL,
	[ProviderId]	INT NOT NULL,
	[HasProviderLevelData] BIT NOT NULL DEFAULT 0, 
    [HasApprenticeshipLevelData] BIT NOT NULL DEFAULT 0, 
    [ExistingProvider] BIT NOT NULL DEFAULT 0, 
    [ImportDateTimeUtc] DATETIME NOT NULL DEFAULT GetUtcDate(), 
    [ManuallyAddedByUserId] NVARCHAR(128) NULL, 
    PRIMARY KEY CLUSTERED 
(
	[ImportBatchId] ASC,
	[ProviderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
    CONSTRAINT [FK_ImportBatchProvider_AspNetUsers] FOREIGN KEY ([ManuallyAddedByUserId]) REFERENCES [AspNetUsers]([Id])
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ImportBatchProvider]  WITH CHECK ADD  CONSTRAINT [FK_ImportBatchProvider_Provider] FOREIGN KEY([ProviderId]) REFERENCES [dbo].[Provider] ([ProviderId])
GO

ALTER TABLE [dbo].[ImportBatchProvider] CHECK CONSTRAINT [FK_ImportBatchProvider_Provider]
GO

ALTER TABLE [dbo].[ImportBatchProvider]  WITH CHECK ADD  CONSTRAINT [FK_ImportBatchProvider_Batch] FOREIGN KEY([ImportBatchId]) REFERENCES [dbo].[ImportBatch] ([ImportBatchId])
GO

ALTER TABLE [dbo].[ImportBatchProvider] CHECK CONSTRAINT [FK_ImportBatchProvider_Batch]
GO


