CREATE TABLE [dbo].[BulkUpload]
(
	[BulkUploadId]				INT					NOT NULL IDENTITY,
	[ProcessingToken]			UNIQUEIDENTIFIER	NULL,
	[FileSize]					INT					NOT NULL DEFAULT 0,
	[FileContentType]			INT                 NULL,
    [UserProviderId]			INT					NULL,
    [UserOrganisationId]		INT					NULL,
    [FileName]					NVARCHAR (2000)		NOT NULL,
    [FilePath]					NVARCHAR (2000)		NOT NULL,
    [ExistingCourses]			INT					NULL,
    [NewCourses]				INT					NULL,
    [InvalidCourses]			INT					NULL,
    [ExistingVenues]			INT					NULL,
    [NewVenues]					INT					NULL,
    [InvalidVenues]				INT					NULL,
    [ExistingOpportunities]		INT					NULL,
    [NewOpportunities]			INT					NULL,
    [InvalidOpportunities]		INT					NULL,
	[ExistingApprenticeships]	INT					NULL,
    [NewApprenticeships]		INT					NULL,
    [InvalidApprenticeships]	INT					NULL,
    [ExistingLocations]			INT					NULL,
    [NewLocations]				INT					NULL,
    [InvalidLocations]			INT					NULL,
    [ExistingDeliveryLocations] INT					NULL,
    [NewDeliveryLocations]      INT					NULL,
    [InvalidDeliveryLocations]  INT					NULL,
    CONSTRAINT [PK_BulkUpload] PRIMARY KEY CLUSTERED ([BulkUploadId] ASC),
	CONSTRAINT [FK_AspNetUsers_UserproviderId] FOREIGN KEY ([UserProviderId]) REFERENCES [dbo].[Provider] ([ProviderId]),
    CONSTRAINT [FK_AspNetUsers_UserOrganisationId] FOREIGN KEY ([UserOrganisationId]) REFERENCES [dbo].[Organisation] ([OrganisationId]),   
)
GO

CREATE INDEX [IX_BulkUpload_ProcessingToken_FileSize] ON [dbo].[BulkUpload] ([ProcessingToken],[FileSize]) INCLUDE ([BulkUploadId])
GO
