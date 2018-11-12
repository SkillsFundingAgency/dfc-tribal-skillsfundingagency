CREATE TABLE [dbo].[QAComplianceFailureReason]
(
	[QAComplianceFailureReasonId] INT NOT NULL PRIMARY KEY, 
    [Description] NVARCHAR(100) NOT NULL, 
	[FullDescription] NVARCHAR(1000) NULL,
    [Ordinal] INT NOT NULL, 
    [RecordStatusId] INT NOT NULL, 
    CONSTRAINT [FK_QAComplianceFailureReason_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [RecordStatus]([RecordStatusId])
)
