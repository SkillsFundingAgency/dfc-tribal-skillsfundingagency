CREATE TABLE [dbo].[MetadataUploadType]
(
	[MetadataUploadTypeId]		[int]				NOT NULL
	, [MetadataUploadTypeName]	[nvarchar](100)		NOT NULL
	, [MetadataUploadTypeDescription]				[nvarchar](100)		NOT NULL
	, CONSTRAINT [PK_MetadataUploadType] PRIMARY KEY CLUSTERED ( [MetadataUploadTypeId] ASC )  
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
