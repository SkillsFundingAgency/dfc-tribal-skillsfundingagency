CREATE TABLE [dbo].[ProviderQAStyleFailureReason]
(
	[ProviderQAStyleId] INT NOT NULL , 
    [QAStyleFailureReasonId] INT NOT NULL, 
    PRIMARY KEY ([ProviderQAStyleId], [QAStyleFailureReasonId]), 
    CONSTRAINT [FK_ProviderQAStyleFailureReason_ProviderQAStyle] FOREIGN KEY ([ProviderQAStyleId]) REFERENCES [ProviderQAStyle]([ProviderQAStyleId]), 
    CONSTRAINT [FK_ProviderQAStyleFailureReason_QAComplianceFailureReason] FOREIGN KEY ([QAStyleFailureReasonId]) REFERENCES [QAStyleFailureReason]([QAStyleFailureReasonId])
)
