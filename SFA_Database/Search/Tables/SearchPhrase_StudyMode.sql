CREATE TABLE [Search].[SearchPhrase_StudyMode]
(
	[SearchPhraseId] INT NOT NULL , 
    [StudyModeId] INT NOT NULL, 
    PRIMARY KEY ([SearchPhraseId], [StudyModeId]), 
    CONSTRAINT [FK_SearchPhrase_StudyMode_SearchPhrase] FOREIGN KEY ([SearchPhraseId]) REFERENCES [Search].[SearchPhrase]([SearchPhraseId]), 
    CONSTRAINT [FK_SearchPhrase_StudyMode_StudyMode] FOREIGN KEY ([StudyModeId]) REFERENCES [dbo].[StudyMode]([StudyModeId])
)
