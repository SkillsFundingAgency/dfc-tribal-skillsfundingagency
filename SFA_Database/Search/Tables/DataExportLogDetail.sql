CREATE TABLE [Search].[DataExportLogDetail] (
    [DataExportLogDetailId] INT           IDENTITY (1, 1) NOT NULL,
    [DataExportLogId]       INT           NOT NULL,
    [Name]                  NVARCHAR (50) NOT NULL,
    [ExistingRowCount]      INT           NOT NULL,
    [NewRowCount]           INT           NOT NULL,
    CONSTRAINT [PK_DataExportLogDetail] PRIMARY KEY CLUSTERED ([DataExportLogDetailId] ASC),
    CONSTRAINT [FK_DataExportLogDetail_DataExportLog] FOREIGN KEY ([DataExportLogId]) REFERENCES [Search].[DataExportLog] ([DataExportLogId])
);

