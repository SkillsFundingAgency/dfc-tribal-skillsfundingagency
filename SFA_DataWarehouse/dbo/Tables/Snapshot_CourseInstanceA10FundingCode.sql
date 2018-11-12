CREATE TABLE [dbo].[Snapshot_CourseInstanceA10FundingCode]
(
	[Period]			VARCHAR(7) NOT NULL,
    [CourseInstanceId]  INT NOT NULL,
    [A10FundingCode]    INT NOT NULL, 
    CONSTRAINT [PK_Snapshot_CourseInstanceA10FundingCode] PRIMARY KEY ([Period], [CourseInstanceId], [A10FundingCode])
);
