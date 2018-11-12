CREATE TABLE [UCAS].[Fees]
(
	[FeeId] INT NOT NULL PRIMARY KEY, 
    [CourseIndexId] INT NOT NULL, 
    [CurrencyId] INT NOT NULL, 
    [StudyPeriodId] INT NOT NULL, 
    [FeeYearId] INT NULL, 
    [Fee] SMALLMONEY NOT NULL
)

GO

CREATE INDEX [IX_Fees_CourseIndexId] ON [UCAS].[Fees] ([CourseIndexId])

GO

CREATE INDEX [IX_Fees_CurrencyId] ON [UCAS].[Fees] ([CurrencyId])

GO

CREATE INDEX [IX_Fees_StudyPeriodId] ON [UCAS].[Fees] ([StudyPeriodId])

GO

CREATE INDEX [IX_Fees_FeeYearId] ON [UCAS].[Fees] ([FeeYearId])
