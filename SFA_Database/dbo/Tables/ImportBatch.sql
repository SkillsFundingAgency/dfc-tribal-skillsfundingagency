CREATE TABLE [dbo].[ImportBatch] (
    [ImportBatchId]   INT  IDENTITY(1,1) NOT NULL,
    [ImportBatchName] NVARCHAR (50) NOT NULL,
	[Current]		  BIT			NOT NULL DEFAULT 0,
    CONSTRAINT [PK_ImportBatch] PRIMARY KEY CLUSTERED ([ImportBatchId] ASC)
)
GO
