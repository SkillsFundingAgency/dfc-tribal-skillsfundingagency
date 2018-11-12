CREATE TABLE [search].[CourseInstanceA10FundingCode] (
    [CourseInstanceA10FundingCodeId] INT           IDENTITY (1, 1) NOT NULL,
    [CourseInstanceId]               INT           NOT NULL,
    [A10FundingCode]                 NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK__CourseIn__31B5AC09755C9202] PRIMARY KEY NONCLUSTERED ([CourseInstanceA10FundingCodeId] ASC)
);






GO
CREATE NONCLUSTERED INDEX [IX_CourseInstanceA10FundingCode]
    ON [search].[CourseInstanceA10FundingCode]([CourseInstanceId] ASC);

