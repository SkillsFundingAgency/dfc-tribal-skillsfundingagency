﻿IF NOT EXISTS (SELECT* FROM [RecordStatus])
BEGIN
	INSERT [dbo].[RecordStatus] ([RecordStatusId], [RecordStatusName], [IsPublished], [IsArchived], [IsDeleted]) VALUES (1, N'Pending', 0, 0, 0)
	INSERT [dbo].[RecordStatus] ([RecordStatusId], [RecordStatusName], [IsPublished], [IsArchived], [IsDeleted]) VALUES (2, N'Live', 1, 0, 0)
	INSERT [dbo].[RecordStatus] ([RecordStatusId], [RecordStatusName], [IsPublished], [IsArchived], [IsDeleted]) VALUES (3, N'Archived', 0, 1, 0)
	INSERT [dbo].[RecordStatus] ([RecordStatusId], [RecordStatusName], [IsPublished], [IsArchived], [IsDeleted]) VALUES (4, N'Deleted', 0, 0, 1)
END

IF NOT EXISTS (SELECT * FROM [Application])
BEGIN
	INSERT [dbo].[Application] ([ApplicationId], [ApplicationName], [PrimaryApplicationName]) VALUES (1, N'NDLPP', 'NDLPP')
	INSERT [dbo].[Application] ([ApplicationId], [ApplicationName], [PrimaryApplicationName]) VALUES (2, N'BU', 'NDLPP')
	INSERT [dbo].[Application] ([ApplicationId], [ApplicationName], [PrimaryApplicationName]) VALUES (3, N'UCAS', 'UCAS')
END

IF NOT EXISTS(SELECT * FROM [ProviderType])
BEGIN
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (1, N'Schools')
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (2, N'Further Education (FE)')
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (3, N'Higher Education (HE)')
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (4, N'Private class-based')
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (5, N'Private self study')
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (6, N'Public sector community education')
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (7, N'Public sector other')
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (8, N'Voluntary sector education')
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (9, N'Private work-based')
END

IF NOT EXISTS(SELECT * FROM AttendancePattern)
BEGIN
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [BulkUploadRef]) VALUES (1, N'Daytime/working hours', N'AP1')
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [BulkUploadRef]) VALUES (2, N'Day/Block release', N'AP2')
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [BulkUploadRef]) VALUES (3, N'Evening', N'AP3')
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [BulkUploadRef]) VALUES (4, N'Twilight', N'AP4')
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [BulkUploadRef]) VALUES (5, N'Weekend', N'AP5')
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [BulkUploadRef]) VALUES (6, N'Customised', N'AP6')
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [BulkUploadRef]) VALUES (7, N'Not known', N'AP7')
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [BulkUploadRef]) VALUES (8, N'Not applicable', N'NA')
END 

IF NOT EXISTS(SELECT * FROM AttendanceType)
BEGIN
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef]) VALUES (1, N'Location / campus', 'AM1')
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef]) VALUES (2, N'Face-to-face (non-campus)', 'AM2')
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef]) VALUES (3, N'Work-based', 'AM3')
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef]) VALUES (4, N'Mixed mode', 'AM4')
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef]) VALUES (5, N'Distance with attendance', 'AM5')
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef]) VALUES (6, N'Distance without attendance', 'AM6')
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef]) VALUES (7, N'Online without attendance', 'AM7')
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef]) VALUES (8, N'Online with attendance', 'AM8')
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef]) VALUES (9, N'Not known', 'AM9')

END

IF NOT EXISTS(SELECT * FROM DurationUnit)
BEGIN
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (1, N'Hour(s)', N'DU1', 10, 0.025)
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (2, N'Day(s)', N'DU2', 20, 0.2)
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (3, N'Week(s)', N'DU3', 30, 1)
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (4, N'Month(s)', N'DU4', 40, 4)
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (5, N'Term(s)', N'DU5', 50, 6)
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (6, N'Semester(s)', N'DU6', 60, 13)
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (7, N'Year(s)', N'DU7', 70, 52)
END

IF NOT EXISTS(SELECT * FROM StudyMode)
BEGIN
	INSERT [dbo].[StudyMode] ([StudyModeId], [StudyModeName], [BulkUploadRef]) VALUES (1, N'Full time', 'SM1')
	INSERT [dbo].[StudyMode] ([StudyModeId], [StudyModeName], [BulkUploadRef]) VALUES (2, N'Part time', 'SM2')
	INSERT [dbo].[StudyMode] ([StudyModeId], [StudyModeName], [BulkUploadRef]) VALUES (3, N'Part of a full-time program', 'SM3')
	INSERT [dbo].[StudyMode] ([StudyModeId], [StudyModeName], [BulkUploadRef]) VALUES (4, N'Flexible', 'SM4')
	INSERT [dbo].[StudyMode] ([StudyModeId], [StudyModeName], [BulkUploadRef]) VALUES (5, N'Not known', 'SM5')
END

IF NOT EXISTS(SELECT * FROM StopWord)
BEGIN
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (1, 'A')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (2, 'ABOUT')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (3, 'AFTER')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (4, 'ALL')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (5, 'ALSO')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (6, 'AN')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (7, 'AND')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (8, 'ANY')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (9, 'ARE')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (10, 'AS')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (11, 'AT')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (12, 'BE')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (13, 'BECAUSE')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (14, 'BEEN')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (15, 'BUT')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (16, 'BY')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (17, 'CAN')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (18, 'CO')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (19, 'CORP')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (20, 'COULD')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (21, 'FOR')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (22, 'FROM')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (23, 'HAD')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (24, 'HAS')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (25, 'HAVE')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (26, 'HE')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (27, 'HER')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (28, 'HIS')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (29, 'IF')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (30, 'IN')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (31, 'INC')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (32, 'INTO')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (33, 'IS')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (34, 'ITS')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (35, 'LAST')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (36, 'MORE')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (37, 'MOST')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (38, 'MR')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (39, 'MRS')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (40, 'MS')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (41, 'MZ')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (42, 'NO')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (43, 'NOT')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (44, 'OF')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (45, 'ON')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (46, 'ONE')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (47, 'ONLY')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (48, 'OR')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (49, 'OTHER')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (50, 'OUT')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (51, 'OVER')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (52, 'S')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (53, 'SAYS')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (54, 'SHE')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (55, 'SO')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (56, 'SOME')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (57, 'SUCH')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (58, 'THAN')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (59, 'THAT')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (60, 'THE')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (61, 'THEIR')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (62, 'THERE')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (63, 'THEY')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (64, 'THIS')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (65, 'TO')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (66, 'UP')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (67, 'WAS')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (68, 'WE')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (69, 'WERE')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (70, 'WHEN')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (71, 'WHICH')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (72, 'WHO')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (73, 'WITH')
	INSERT INTO [StopWord] ([StopWordId], [StopWordName]) VALUES (74, 'WOULD')
END

IF NOT EXISTS(SELECT 1 FROM [dbo].[GeoLocation])
BEGIN
	INSERT INTO [dbo].[GeoLocation] ([Postcode],[Lat],[Lng],[Northing],[Easting])  
		SELECT [Postcode],[Lat],[Lng],[Northing],[Easting] FROM  [SFA_CourseDirectory].[dbo].[GeoLocation]
END