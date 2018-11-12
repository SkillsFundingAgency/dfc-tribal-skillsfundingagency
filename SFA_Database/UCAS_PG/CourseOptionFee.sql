CREATE TABLE [UCAS_PG].[CourseOptionFee]
(
	[CourseOptionFeeId] INT NOT NULL PRIMARY KEY, 
    [CourseOptionId] INT NOT NULL, 
    [Fee] FLOAT NOT NULL, 
    [Currency] NVARCHAR(10) NOT NULL, 
    [FeeDurationPeriod] NVARCHAR(50) NOT NULL, 
    [Locale] NVARCHAR(50) NOT NULL
)

GO

CREATE INDEX [IX_CourseOptionFee_CourseOptionId] ON [UCAS_PG].[CourseOptionFee] ([CourseOptionId])
