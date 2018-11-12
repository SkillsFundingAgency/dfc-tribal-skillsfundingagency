CREATE TABLE [dbo].[A10FundingCode] (
    [A10FundingCodeId]   INT            NOT NULL,
    [A10FundingCodeName] NVARCHAR (100) NOT NULL,
    [RecordStatusId]	 INT			NOT NULL, 
    CONSTRAINT [PK_A10FundingCode] PRIMARY KEY CLUSTERED ([A10FundingCodeId] ASC), 
    CONSTRAINT [FK_A10FundingCode_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [RecordStatus]([RecordStatusId])
);

