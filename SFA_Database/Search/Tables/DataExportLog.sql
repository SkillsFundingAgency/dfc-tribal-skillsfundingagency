CREATE TABLE [Search].[DataExportLog] (
    [DataExportLogId]   INT           IDENTITY (1, 1) NOT NULL,
	[ExportType]        NVARCHAR (15) NULL,
    [ExecutedOn]        DATETIME2 (7) CONSTRAINT [DF_DataExport_ExecutedOn] DEFAULT (getutcdate()) NOT NULL,
    [IsSuccessful]      BIT           CONSTRAINT [DF_DataExport_IsSuccessful] DEFAULT 0 NOT NULL,
    [IsValidDataImport] BIT           CONSTRAINT [DF_DataExport_IsValidDataImport] DEFAULT 0 NOT NULL,
    [ImportUCASData] BIT NOT NULL DEFAULT 0, 
    [Import_UCAS_PG_Data] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_DataExport] PRIMARY KEY CLUSTERED ([DataExportLogId] ASC)
);

