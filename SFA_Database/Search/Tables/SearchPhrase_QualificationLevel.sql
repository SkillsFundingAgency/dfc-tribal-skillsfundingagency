CREATE TABLE [Search].[SearchPhrase_QualificationLevel]
(
	[SearchPhraseId] INT NOT NULL , 
    [QualificationLevelId] INT NOT NULL, 
    PRIMARY KEY ([SearchPhraseId], [QualificationLevelId]), 
    CONSTRAINT [FK_SearchPhrase_QualificationLevel_SearchPhrase] FOREIGN KEY ([SearchPhraseId]) REFERENCES [Search].[SearchPhrase]([SearchPhraseId]), 
    CONSTRAINT [FK_SearchPhrase_QualificationLevel_QualificationLevel] FOREIGN KEY ([QualificationLevelId]) REFERENCES [dbo].[QualificationLevel]([QualificationLevelId])
)
