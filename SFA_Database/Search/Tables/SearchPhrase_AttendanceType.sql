CREATE TABLE [Search].[SearchPhrase_AttendanceType]
(
	[SearchPhraseId] INT NOT NULL , 
    [AttendanceTypeId] INT NOT NULL, 
    PRIMARY KEY ([SearchPhraseId], [AttendanceTypeId]), 
    CONSTRAINT [FK_SearchPhrase_AttendanceMode_SearchPhrase] FOREIGN KEY ([SearchPhraseId]) REFERENCES [Search].[SearchPhrase]([SearchPhraseId]), 
    CONSTRAINT [FK_SearchPhrase_AttendanceMode_AttendanceType] FOREIGN KEY ([AttendanceTypeId]) REFERENCES [dbo].[AttendanceType]([AttendanceTypeId])
)
