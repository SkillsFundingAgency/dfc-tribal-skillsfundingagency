CREATE TABLE [Search].[VenueText] (
    [VenueTextId]	 INT            IDENTITY (1, 1) NOT NULL,
    [VenueId]		 INT            NULL,
    [ProviderId]     INT            NULL,
    [SearchText]     NVARCHAR (2000) NOT NULL,
    CONSTRAINT [PK__venueText__47BDF094CAA57201] PRIMARY KEY NONCLUSTERED ([VenueTextId] ASC)
);



