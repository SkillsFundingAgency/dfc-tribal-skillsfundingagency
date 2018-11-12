CREATE TABLE [Search].[DataExportException] (
    [DataExportExceptionId] INT             IDENTITY (1, 1) NOT NULL,
    [DataExportLogId]       INT             NOT NULL,
    [ExceptionDetails]      NVARCHAR (2000) NOT NULL,
    [CreatedOn]             DATETIME        CONSTRAINT [DF_DataExportException_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_DataExportException] PRIMARY KEY CLUSTERED ([DataExportExceptionId] ASC),
    CONSTRAINT [FK_DataExportException_DataExportLog] FOREIGN KEY ([DataExportLogId]) REFERENCES [Search].[DataExportLog] ([DataExportLogId])
);



