CREATE TABLE [dbo].[ApprenticeshipQAStyleFailureReason]
(
	[ApprenticeshipQAStyleId] INT NOT NULL , 
    [QAStyleFailureReasonId] INT NOT NULL, 
    PRIMARY KEY ([ApprenticeshipQAStyleId], [QAStyleFailureReasonId]), 
    CONSTRAINT [FK_ApprenticeshipQAStyleFailureReason_ApprenticeshipQAStyle] FOREIGN KEY ([ApprenticeshipQAStyleId]) REFERENCES [ApprenticeshipQAStyle]([ApprenticeshipQAStyleId]), 
    CONSTRAINT [FK_ApprenticeshipQAStyleFailureReason_QAComplianceFailureReason] FOREIGN KEY ([QAStyleFailureReasonId]) REFERENCES [QAStyleFailureReason]([QAStyleFailureReasonId])
)