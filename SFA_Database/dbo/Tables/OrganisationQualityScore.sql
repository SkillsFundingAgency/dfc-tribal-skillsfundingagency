CREATE TABLE [dbo].[OrganisationQualityScore]
(
	[OrganisationId] INT NOT NULL,
	-- Stats
	[EarliestModifiedDateTimeUtc] DATETIME NULL,
	[CalculatedDateTimeUtc] DATETIME NOT NULL,
    CONSTRAINT [PK_OrganisationQualityScore] PRIMARY KEY CLUSTERED ([OrganisationId] ASC),
	CONSTRAINT [FK_OrganisationQualityScore_Organisation] FOREIGN KEY([OrganisationId]) REFERENCES [Organisation] ([OrganisationId])
)
