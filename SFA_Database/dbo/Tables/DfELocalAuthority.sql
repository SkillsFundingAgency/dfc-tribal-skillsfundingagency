CREATE TABLE [dbo].[DfELocalAuthority]
(
	[DfELocalAuthorityId]		INT				NOT NULL
	, [DfELocalAuthorityCode]	NVARCHAR(3)		NOT NULL
	, [DfELocalAuthorityName]	NVARCHAR(50)	NOT NULL
	, CONSTRAINT [PK_DfELocalAuthority] PRIMARY KEY CLUSTERED ([DfELocalAuthorityId] ASC)
)
