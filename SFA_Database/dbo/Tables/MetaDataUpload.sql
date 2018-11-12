CREATE TABLE [dbo].[MetadataUpload]
(
	[MetadataUploadId]			[int]				IDENTITY(1,1) NOT NULL
	, [MetadataUploadTypeId]	[int]				NOT NULL
	, [RowsBefore]				[int]				NOT NULL
	, [RowsAfter]				[int]				NOT NULL
	, [CreatedByUserId]			[nvarchar](128)		NOT NULL
	, [CreatedDateTimeUtc]		[datetime]			NOT NULL
	, [DurationInMilliseconds]	[int]				NOT NULL
	, [FileName]				[nvarchar](4000)	NULL
	, [FileSizeInBytes]			[int]				NULL
	, CONSTRAINT [FK_MetadataUpload_MetadataUploadType] FOREIGN KEY ([MetadataUploadTypeId]) REFERENCES [dbo].[MetadataUploadType] ([MetaDataUploadTypeId])
	, CONSTRAINT [FK_MetadataUpload_CreatedByUser] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
	, CONSTRAINT [PK_MetadataUpload] PRIMARY KEY CLUSTERED ( [MetadataUploadId] ASC )  
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
