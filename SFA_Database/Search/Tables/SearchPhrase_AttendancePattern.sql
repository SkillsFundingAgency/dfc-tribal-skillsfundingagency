CREATE TABLE [Search].[SearchPhrase_AttendancePattern]
(
	[SearchPhraseId] INT NOT NULL , 
    [AttendancePatternId] INT NOT NULL, 
    PRIMARY KEY ([SearchPhraseId], [AttendancePatternId]), 
    CONSTRAINT [FK_SearchPhrase_AttendancePattern_SearchPhrase] FOREIGN KEY ([SearchPhraseId]) REFERENCES [Search].[SearchPhrase]([SearchPhraseId]), 
    CONSTRAINT [FK_SearchPhrase_AttendancePattern_AttendancePattern] FOREIGN KEY ([AttendancePatternId]) REFERENCES [dbo].[AttendancePattern]([AttendancePatternId])
)
