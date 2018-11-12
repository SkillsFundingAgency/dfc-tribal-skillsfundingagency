CREATE TABLE [dbo].[ApprenticeshipQAComplianceFailureReason]
(
	[ApprenticeshipQAComplianceId] INT NOT NULL , 
    [QAComplianceFailureReasonId] INT NOT NULL, 
    PRIMARY KEY ([ApprenticeshipQAComplianceId], [QAComplianceFailureReasonId]), 
    CONSTRAINT [FK_ApprenticeshipQAComplianceFailureReason_ApprenticeshipQACompliance] FOREIGN KEY ([ApprenticeshipQAComplianceId]) REFERENCES [ApprenticeshipQACompliance]([ApprenticeshipQAComplianceId]), 
    CONSTRAINT [FK_ApprenticeshipQAComplianceFailureReason_QAComplianceFailureReason] FOREIGN KEY ([QAComplianceFailureReasonId]) REFERENCES [QAComplianceFailureReason]([QAComplianceFailureReasonId])
)
