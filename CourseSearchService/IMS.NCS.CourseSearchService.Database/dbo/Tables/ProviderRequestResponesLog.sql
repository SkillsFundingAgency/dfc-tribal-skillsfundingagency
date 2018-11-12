CREATE TABLE [dbo].[ProviderRequestResponesLog] (
    [ProviderRequestResponseId] INT            IDENTITY (1, 1) NOT NULL,
    [ServiceMethod]             NVARCHAR (100) NOT NULL,
    [Request]                   NVARCHAR (MAX) NULL,
    [Response]                  NVARCHAR (MAX) NULL,
    [TimeInMilliseconds]        INT            NULL,
    [DateTimeUtc]               DATETIME2 (7)  CONSTRAINT [DF_ProviderRequestResponesLog_DateTimeUtc] DEFAULT (getutcdate()) NOT NULL,
    [PublicAPI] BIT NOT NULL DEFAULT 0, 
    [APIKey] NVARCHAR(50) NULL, 
    [RecordCount] INT NULL, 
    CONSTRAINT [PK_ProviderRequestResponesLog] PRIMARY KEY CLUSTERED ([ProviderRequestResponseId] ASC)
);

