CREATE TABLE [dbo].[VenueText] (
    [VenueTextId]	 INT			NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 32768),
    [VenueId]		 INT            NULL,
    [ProviderId]     INT            NULL,
    [SearchText]     NVARCHAR (400) COLLATE Latin1_General_100_BIN2 NOT NULL,
	INDEX [IX_dbo_VenueText_SearchText] NONCLUSTERED ([SearchText])
)  WITH (MEMORY_OPTIMIZED = ON);
GO
