CREATE TABLE [Search].[VenueText] (
    [VenueTextId]	 INT            IDENTITY (1, 1) NOT NULL,
    [VenueId]		 INT            NULL,
    [ProviderId]     INT            NULL,
    [SearchText]     NVARCHAR (400) NOT NULL,
    CONSTRAINT [PK_VenueText] PRIMARY KEY NONCLUSTERED ([VenueTextId] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_VenueText_SearchText]
ON [search].[VenueText] ([SearchText])
INCLUDE ([VenueId])