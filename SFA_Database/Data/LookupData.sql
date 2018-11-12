IF NOT EXISTS(SELECT * FROM RecordStatus)
BEGIN
	INSERT [dbo].[RecordStatus] ([RecordStatusId], [RecordStatusName], [IsPublished], [IsArchived], [IsDeleted]) VALUES (1, N'Pending', 0, 0, 0);
	INSERT [dbo].[RecordStatus] ([RecordStatusId], [RecordStatusName], [IsPublished], [IsArchived], [IsDeleted]) VALUES (2, N'Live', 1, 0, 0);
	INSERT [dbo].[RecordStatus] ([RecordStatusId], [RecordStatusName], [IsPublished], [IsArchived], [IsDeleted]) VALUES (3, N'Archived', 0, 1, 0);
	INSERT [dbo].[RecordStatus] ([RecordStatusId], [RecordStatusName], [IsPublished], [IsArchived], [IsDeleted]) VALUES (4, N'Deleted', 0, 0, 1);
END;
GO

IF NOT EXISTS (SELECT * FROM A10FundingCode)
BEGIN
	INSERT [dbo].[A10FundingCode] ([A10FundingCodeId], [A10FundingCodeName], [RecordStatusId]) VALUES (10, N'Community Learning', 2);
	INSERT [dbo].[A10FundingCode] ([A10FundingCodeId], [A10FundingCodeName], [RecordStatusId]) VALUES (21, N'16-18 Learner Responsive', 1);
	INSERT [dbo].[A10FundingCode] ([A10FundingCodeId], [A10FundingCodeName], [RecordStatusId]) VALUES (22, N'Adult Learner Responsive', 1);
	INSERT [dbo].[A10FundingCode] ([A10FundingCodeId], [A10FundingCodeName], [RecordStatusId]) VALUES (45, N'Employer Responsive', 1);
	INSERT [dbo].[A10FundingCode] ([A10FundingCodeId], [A10FundingCodeName], [RecordStatusId]) VALUES (46, N'Employer Responsive main aim which is part of an apprenticeship programme', 2);
	INSERT [dbo].[A10FundingCode] ([A10FundingCodeId], [A10FundingCodeName], [RecordStatusId]) VALUES (70, N'ESF funded (co-financed by the SFA)', 2);
	INSERT [dbo].[A10FundingCode] ([A10FundingCodeId], [A10FundingCodeName], [RecordStatusId]) VALUES (80, N'Other LSC funding (valid only for learning status starting before 1 August 2010)', 1);
	INSERT [dbo].[A10FundingCode] ([A10FundingCodeId], [A10FundingCodeName], [RecordStatusId]) VALUES (81, N'Other SFA funding model - In', 2);
	INSERT [dbo].[A10FundingCode] ([A10FundingCodeId], [A10FundingCodeName], [RecordStatusId]) VALUES (82, N'Other YPLA funding model', 2);
	INSERT [dbo].[A10FundingCode] ([A10FundingCodeId], [A10FundingCodeName], [RecordStatusId]) VALUES (99, N'No SPA or YPLA funding for this learning aim', 2);
END;
GO

IF NOT EXISTS (SELECT * FROM A10FundingCode WHERE A10FundingCodeId = 25)
BEGIN
	-- These were left out from original batch so added as a separate NOT EXISTS check to upgrade any existing deployed DBs
	INSERT [dbo].[A10FundingCode] ([A10FundingCodeId], [A10FundingCodeName], [RecordStatusId]) VALUES (25, N'DfE 16-19 Funded', 2);
	INSERT [dbo].[A10FundingCode] ([A10FundingCodeId], [A10FundingCodeName], [RecordStatusId]) VALUES (35, N'Adult Skills Funding', 2);
END;
GO

IF NOT EXISTS(SELECT * FROM [Application])
BEGIN
	INSERT [dbo].[Application] ([ApplicationId], [ApplicationName]) VALUES (1, N'Portal');
	INSERT [dbo].[Application] ([ApplicationId], [ApplicationName]) VALUES (2, N'Bulk upload');
	INSERT [dbo].[Application] ([ApplicationId], [ApplicationName]) VALUES (3, N'Ucas import');
END;
GO

IF NOT EXISTS(SELECT * FROM AttendancePattern)
BEGIN
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [DisplayOrder], [BulkUploadRef]) VALUES (1, N'Daytime/working hours', 20, N'AP1');
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [DisplayOrder], [BulkUploadRef]) VALUES (2, N'Day/Block release', 30, N'AP2');
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [DisplayOrder], [BulkUploadRef]) VALUES (3, N'Evening', 40, N'AP3');
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [DisplayOrder], [BulkUploadRef]) VALUES (4, N'Twilight', 50, N'AP4');
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [DisplayOrder], [BulkUploadRef]) VALUES (5, N'Weekend', 60, N'AP5');
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [DisplayOrder], [BulkUploadRef]) VALUES (6, N'Customised', 10, N'AP6');
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [DisplayOrder], [BulkUploadRef]) VALUES (7, N'Not known', 70, N'AP7');
	INSERT [dbo].[AttendancePattern] ([AttendancePatternId], [AttendancePatternName], [DisplayOrder], [BulkUploadRef]) VALUES (8, N'Not applicable', 80, N'NA');
END;
GO

IF NOT EXISTS(SELECT * FROM AttendanceType)
BEGIN
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef], [DisplayOrder]) VALUES (1, N'Location/Campus', 'AM1', 10);
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef], [DisplayOrder]) VALUES (2, N'Face-to-face (non-campus)', 'AM2', 20);
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef], [DisplayOrder]) VALUES (3, N'Work-based', 'AM3', 30);
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef], [DisplayOrder]) VALUES (4, N'Mixed mode', 'AM4', 40);
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef], [DisplayOrder]) VALUES (5, N'Distance with attendance', 'AM5', 50);
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef], [DisplayOrder]) VALUES (6, N'Distance without attendance', 'AM6', 60);
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef], [DisplayOrder]) VALUES (7, N'Online (without attendance)', 'AM7', 70);
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef], [DisplayOrder]) VALUES (8, N'Online (with attendance)', 'AM8', 80);
	INSERT [dbo].[AttendanceType] ([AttendanceTypeId], [AttendanceTypeName], [BulkUploadRef], [DisplayOrder]) VALUES (9, N'Not known', 'AM9', 90);
END;
GO

IF NOT EXISTS(SELECT * FROM DurationUnit)
BEGIN
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (1, N'Hour(s)', N'DU1', 10, 0.025);
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (2, N'Day(s)', N'DU2', 20, 0.2);
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (3, N'Week(s)', N'DU3', 30, 1);
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (4, N'Month(s)', N'DU4', 40, 4);
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (5, N'Term(s)', N'DU5', 50, 6);
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (6, N'Semester(s)', N'DU6', 60, 13);
	INSERT [dbo].[DurationUnit] ([DurationUnitId], [DurationUnitName], [BulkUploadRef], [DisplayOrder], [WeekEquivalent]) VALUES (7, N'Year(s)', N'DU7', 70, 52);
END;
GO

IF NOT EXISTS(SELECT * FROM OrganisationType)
BEGIN
	INSERT [dbo].[OrganisationType] ([OrganisationTypeId], [OrganisationTypeName]) VALUES (1, N'Partnership');
	INSERT [dbo].[OrganisationType] ([OrganisationTypeId], [OrganisationTypeName]) VALUES (2, N'Shared back office');
	INSERT [dbo].[OrganisationType] ([OrganisationTypeId], [OrganisationTypeName]) VALUES (3, N'MIS Vendor');
	INSERT [dbo].[OrganisationType] ([OrganisationTypeId], [OrganisationTypeName]) VALUES (4, N'Consortium');
	INSERT [dbo].[OrganisationType] ([OrganisationTypeId], [OrganisationTypeName]) VALUES (5, N'Other');
END;
GO

IF NOT EXISTS(SELECT * FROM ProviderType)
BEGIN
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (1, N'Schools');
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (2, N'Further Education (FE)');
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (3, N'Higher Education (HE)');
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (4, N'Private class-based');
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (5, N'Private self study');
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (6, N'Public sector community education');
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (7, N'Public sector other');
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (8, N'Voluntary sector education');
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (9, N'Private work-based');
END;
GO

IF NOT EXISTS(SELECT * FROM ProviderType WHERE [ProviderTypeName] = N'DfE 16-19')
BEGIN
	INSERT [dbo].[ProviderType] ([ProviderTypeId], [ProviderTypeName]) VALUES (10, N'DfE 16-19');
END;
GO

IF NOT EXISTS(SELECT * FROM QualificationLevel)
BEGIN
	INSERT [dbo].[QualificationLevel] ([QualificationLevelId], [QualificationLevelName], [BulkUploadRef], [DisplayOrder]) VALUES (1, N'Level 1', N'LV1', 20);
	INSERT [dbo].[QualificationLevel] ([QualificationLevelId], [QualificationLevelName], [BulkUploadRef], [DisplayOrder]) VALUES (2, N'Level 2', N'LV2', 30);
	INSERT [dbo].[QualificationLevel] ([QualificationLevelId], [QualificationLevelName], [BulkUploadRef], [DisplayOrder]) VALUES (3, N'Level 3', N'LV3', 40);
	INSERT [dbo].[QualificationLevel] ([QualificationLevelId], [QualificationLevelName], [BulkUploadRef], [DisplayOrder]) VALUES (4, N'Level 4', N'LV4', 50);
	INSERT [dbo].[QualificationLevel] ([QualificationLevelId], [QualificationLevelName], [BulkUploadRef], [DisplayOrder]) VALUES (5, N'Level 5', N'LV5', 60);
	INSERT [dbo].[QualificationLevel] ([QualificationLevelId], [QualificationLevelName], [BulkUploadRef], [DisplayOrder]) VALUES (6, N'Level 6', N'LV6', 70);
	INSERT [dbo].[QualificationLevel] ([QualificationLevelId], [QualificationLevelName], [BulkUploadRef], [DisplayOrder]) VALUES (7, N'Level 7', N'LV7', 80);
	INSERT [dbo].[QualificationLevel] ([QualificationLevelId], [QualificationLevelName], [BulkUploadRef], [DisplayOrder]) VALUES (8, N'Level 8', N'LV8', 90);
	INSERT [dbo].[QualificationLevel] ([QualificationLevelId], [QualificationLevelName], [BulkUploadRef], [DisplayOrder]) VALUES (9, N'Higher level', N'LV9', 100);
	INSERT [dbo].[QualificationLevel] ([QualificationLevelId], [QualificationLevelName], [BulkUploadRef], [DisplayOrder]) VALUES (10, N'Entry level', N'LV0', 10);
	INSERT [dbo].[QualificationLevel] ([QualificationLevelId], [QualificationLevelName], [BulkUploadRef], [DisplayOrder]) VALUES (11, N'Unknown/not applicable', N'LVNA', 110);
END;
GO

IF NOT EXISTS(SELECT * FROM ProviderRegion)
BEGIN
	INSERT [dbo].[ProviderRegion] ([ProviderRegionId], [RegionName]) VALUES (1, N'North East');
	INSERT [dbo].[ProviderRegion] ([ProviderRegionId], [RegionName]) VALUES (2, N'North West');
	INSERT [dbo].[ProviderRegion] ([ProviderRegionId], [RegionName]) VALUES (3, N'The Midlands');
	INSERT [dbo].[ProviderRegion] ([ProviderRegionId], [RegionName]) VALUES (4, N'South East');
	INSERT [dbo].[ProviderRegion] ([ProviderRegionId], [RegionName]) VALUES (5, N'South West');
	INSERT [dbo].[ProviderRegion] ([ProviderRegionId], [RegionName]) VALUES (6, N'London');
END;
GO

IF NOT EXISTS(SELECT * FROM StatisticAction)
BEGIN
	INSERT [dbo].[StatisticAction] ([StatisticActionId], [StatisticActionName]) VALUES (1, N'Add');
	INSERT [dbo].[StatisticAction] ([StatisticActionId], [StatisticActionName]) VALUES (2, N'Update');
	INSERT [dbo].[StatisticAction] ([StatisticActionId], [StatisticActionName]) VALUES (3, N'Archive');
	INSERT [dbo].[StatisticAction] ([StatisticActionId], [StatisticActionName]) VALUES (4, N'Delete');
END;
GO

IF NOT EXISTS(SELECT * FROM StatisticType)
BEGIN
	INSERT [dbo].[StatisticType] ([StatisticTypeId], [StatisticTypeName]) VALUES (1, N'Provider');
	INSERT [dbo].[StatisticType] ([StatisticTypeId], [StatisticTypeName]) VALUES (2, N'Venue');
	INSERT [dbo].[StatisticType] ([StatisticTypeId], [StatisticTypeName]) VALUES (3, N'Course');
	INSERT [dbo].[StatisticType] ([StatisticTypeId], [StatisticTypeName]) VALUES (4, N'CourseInstance');
END;
GO

IF NOT EXISTS(SELECT * FROM StudyMode)
BEGIN
	INSERT [dbo].[StudyMode] ([StudyModeId], [StudyModeName], [BulkUploadRef], [DisplayOrder]) VALUES (1, N'Full time', 'SM1', 10);
	INSERT [dbo].[StudyMode] ([StudyModeId], [StudyModeName], [BulkUploadRef], [DisplayOrder]) VALUES (2, N'Part time', 'SM2', 20);
	INSERT [dbo].[StudyMode] ([StudyModeId], [StudyModeName], [BulkUploadRef], [DisplayOrder]) VALUES (3, N'Part of a full time program', 'SM3', 30);
	INSERT [dbo].[StudyMode] ([StudyModeId], [StudyModeName], [BulkUploadRef], [DisplayOrder]) VALUES (4, N'Flexible', 'SM4', 40);
	INSERT [dbo].[StudyMode] ([StudyModeId], [StudyModeName], [BulkUploadRef], [DisplayOrder]) VALUES (5, N'Not known', 'SM5', 50);
END;
GO

IF NOT EXISTS(SELECT * FROM Prison)
BEGIN
	SET IDENTITY_INSERT [dbo].[Prison] ON ;
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (1, N'BIRMINGHAM', N'B18 4AS');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (2, N'HEWELL Grange ', N'B97 6QQ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (3, N'HEWELL', N'B97 6QS ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (4, N'SHEPTON MALLET', N'BA4 5LU');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (5, N'FORD', N'BN18 0BX');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (6, N'LEWES', N'BN7 1EA');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (7, N'BRISTOL', N'BS7 8PS');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (8, N'HIGHPOINT', N'CB8 9YG');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (9, N'EDMUNDS HILL', N'CB8 9YN');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (10, N'CHELMSFORD', N'CM2 6LQ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (11, N'CANTERBURY', N'CT1 1PJ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (12, N'ONLEY', N'CV23 8AP');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (13, N'SUDBURY', N'DE6 5HW');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (14, N'FOSTON HALL', N'DE65 5DN');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (15, N'DURHAM', N'DH1 3HU');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (16, N'LOW NEWTON', N'DH1 5YA');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (17, N'FRANKLAND', N'DH1 5YD');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (18, N'DEERBOLT', N'DL12 9BG');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (19, N'NORTHALLERTON', N'DL6 1NW');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (20, N'RANBY', N'DN22 8EU');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (21, N'MOORLAND CLOSED', N'DN7 6BW');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (22, N'LINDHOLME', N'DN7 6EE');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (23, N'MOORLAND OPEN', N'DN7 6EL');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (24, N'DORCHESTER', N'DT1 1JD');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (25, N'PORTLAND', N'DT5 1DL');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (26, N'THE VERNE', N'DT5 1EQ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (27, N'EXETER', N'EX4 4EX');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (28, N'GLOUCESTER', N'GL1 2JN');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (29, N'LEYHILL', N'GL12 8BT');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (30, N'EASTWOOD PARK', N'GL12 8DB');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (31, N'SEND', N'GU23 7LJ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (32, N'COLDINGLEY', N'GU24 9EX');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (33, N'GRENDON', N'HP18 0TL');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (34, N'SPRING HILL', N'HP18 0TH');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (35, N'AYLESBURY', N'HP20 1EH');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (36, N'THE MOUNT', N'HP3 0NZ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (37, N'EVERTHORPE', N'HU15 1RB');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (38, N'HULL', N'HU9 5LS');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (39, N'HOLLESLEY BAY', N'IP12 3JW');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (40, N'WARREN HILL', N'IP12 3JW');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (41, N'WAYLAND', N'IP25 6RL');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (42, N'KENNET', N'L31 1HX');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (43, N'LIVERPOOL', N'L9 3DF');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (44, N'LANCASTER CASTLE', N'LA1 1YL');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (45, N'LANCASTER FARMS', N'LA1 3QZ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (46, N'HAVERIGG', N'LA18 4NA');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (47, N'ASHWELL', N'LE15 7LF');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (48, N'STOCKEN', N'LE15 7RD');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (49, N'GARTREE', N'LE16 7RP');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (50, N'GLEN PARVA', N'LE18 4TN');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (51, N'LEICESTER', N'LE2 7AJ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (52, N'LINCOLN', N'LN2 4BD');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (53, N'MORTON HALL', N'LN6 9PT');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (54, N'LEEDS', N'LS12 2TJ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (55, N'WETHERBY', N'LS22 5ED');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (56, N'WEALSTUN', N'LS23 7AZ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (57, N'MANCHESTER SLA', N'M60 9AH');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (58, N'COOKHAM WOOD', N'ME1 3LU');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (59, N'ROCHESTER', N'ME1 3QS');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (60, N'STANDFORD HILL', N'ME12 4AA');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (61, N'SWALESIDE', N'ME12 4AX');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (62, N'ELMLEY', N'ME12 4DZ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (63, N'MAIDSTONE', N'ME14 1UZ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (64, N'EAST SUTTON PARK', N'ME17 3DF');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (65, N'WOODHILL', N'MK4 4DA');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (66, N'BEDFORD', N'MK40 1HG');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (67, N'HOLLOWAY', N'N7 0NU');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (68, N'PENTONVILLE', N'N7 8TT');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (69, N'ACKLINGTON', N'NE65 9XF');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (70, N'CASTINGTON', N'NE65 9XG');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (71, N'WHATTON', N'NG13 9FQ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (72, N'NOTTINGHAM', N'NG5 3AG');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (73, N'WELLINGBOROUGH', N'NN8 2NH');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (74, N'NORWICH', N'NR1 4LU');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (75, N'BURE', N'NR10 5GB');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (76, N'BLUNDESTON', N'NR32 5BG');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (77, N'BUCKLEY HALL', N'OL12 9DP');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (78, N'BULLINGDON', N'OX25 1PZ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (79, N'WHITEMOOR', N'PE15 0PR');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (80, N'NORTH SEA CAMP', N'PE22 0QX');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (81, N'LITTLEHEY', N'PE28 0SR');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (82, N'LITTLEHEY 2', N'PE28 0SR');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (83, N'DARTMOOR', N'PL20 6RR');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (84, N'KINGSTON', N'PO3 6AS');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (85, N'Parkhurst', N'PO30 5NX');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (86, N'CampHill', N'PO30 5PB');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (87, N'Albany', N'PO30 5RS');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (88, N'PRESTON', N'PR1 5AB');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (89, N'WYMOTT', N'PR26 8LW');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (90, N'GARTH', N'PR26 8NE');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (91, N'KIRKHAM', N'PR4 2RN');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (92, N'READING', N'RG1 3HY');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (93, N'HUNTERCOMBE', N'RG9 5SB');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (94, N'BELMARSH', N'SE28 0EB');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (95, N'ISIS', N'SE28 0NZ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (96, N'STYAL', N'SK9 4HR');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (97, N'DOWNVIEW', N'SM2 5PD');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (98, N'HIGH DOWN', N'SM2 5PJ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (99, N'ERLESTOKE', N'SN10 5TU');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (100, N'WINCHESTER', N'SO22 5DF');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (101, N'GUYS MARSH', N'SP7 0AH');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (102, N'BULLWOOD HALL', N'SS5 4TE');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (103, N'STAFFORD', N'ST16 3AW');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (104, N'DRAKE HALL', N'ST21 6LQ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (105, N'WERRINGTON', N'ST9 0DX');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (106, N'WANDSWORTH', N'SW18 3HS');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (107, N'BRIXTON SLA', N'SW2 5XF');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (108, N'SHREWSBURY', N'SY1 2HR');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (109, N'STOKE HEATH', N'TF9 2JL');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (110, N'BLANTYRE HOUSE', N'TN17 2NH');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (111, N'CHANNINGS WOOD', N'TQ12 6DW');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (112, N'KIRKLEVINGTON GRANGE', N'TS15 9PA');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (113, N'HOLME HOUSE', N'TS18 2QU');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (114, N'LATCHMERE HOUSE', N'TW10 5HH');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (115, N'FELTHAM', N'TW13 4ND');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (116, N'WORMWOOD SCRUBS', N'W12 0AE');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (117, N'RISLEY', N'WA3 6BP');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (118, N'THORN CROSS', N'WA4 4RL');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (119, N'WAKEFIELD', N'WF2 9AG');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (120, N'NEW HALL', N'WF4 4XX');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (121, N'HINDLEY', N'WN2 5TH');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (122, N'LONG LARTIN', N'WR11 8TZ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (123, N'SWINFEN HALL', N'WS14 9QS');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (124, N'FEATHERSTONE', N'WV10 7PU');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (125, N'BRINSFORD', N'WV10 7PY');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (126, N'ASKHAM GRANGE', N'YO23 3FT');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (127, N'FULL SUTTON', N'YO41 1PS');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (128, N'HEWELL LANE', N'B97 6QS');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (129, N'BROCKHILL ', N'B97 6RD');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (130, N'ASHFIELD', N'BS16 9QJ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (131, N'CARDIFF', N'CF24 0UG');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (132, N'BRIDGEND', N'CF35 6AP');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (133, N'DOVER', N'CT17 9DR');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (134, N'DONCASTER', N'DN5 8UX');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (135, N'WOLDS', N'HU15 2JZ');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (136, N'ALTCOURSE', N'L9 7LH');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (137, N'FOREST BANK', N'M27 8FB');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (138, N'LOWDHAM GRANGE', N'NG14 7DA');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (139, N'USK', N'NP15 1XP');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (140, N'PRESCOED', N'NP4 0TB');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (141, N'PETERBOROUGH', N'PE3 7PD');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (142, N'HASLAR', N'PO12 2AW');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (143, N'SWANSEA', N'SA1 3SR');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (144, N'DOVEGATE', N'ST14 8XR');
	INSERT [dbo].[Prison] ([PrisonId], [PrisonName], [PrisonPostcode]) VALUES (145, N'BRONZEFIELD', N'TW15 3JZ');
	SET IDENTITY_INSERT [dbo].[Prison] OFF;
END;
GO

IF NOT EXISTS (SELECT * FROM QualificationType)
BEGIN	
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (1, N'No qualification', 10, N'QT1');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (2, N'Certificate of attendance', 20, N'QT2');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (3, N'Functional skill', 30, N'QT3');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (4, N'Basic/key skill', 40, N'QT4');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (5, N'Course provider certificate (this must include an assessed element)', 50, N'QT5');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (6, N'External awarded qualification - Non-accredited', 60, N'QT6');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (7, N'Other regulated/accredited qualification', 70, N'QT7');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (8, N'GCSE or equivalent', 80, N'QT8');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (9, N'14-19 Diplomoa and relevant components', 90, N'QT9');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (10, N'Apprenticeship', 100, N'QT10');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (11, N'NVQ  and relevant components', 110, N'QT11');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (12, N'International Baccalaureate diploma', 120, N'QT12');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (13, N'GCE A/AS Level or equivalent', 130, N'QT13');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (14, N'Access to higher education', 140, N'QT14');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (15, N'HNC/HND/Higher education awards', 150, N'QT15');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (16, N'Foundation degree', 160, N'QT16');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (17, N'Undergraduate qualification', 170, N'QT17');
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (18, N'Postgraduate qualification', 180, N'QT18');
END;
GO

if not exists (select 1 from [dbo].[QualificationType] where [BulkUploadRef] = 'QT19')
BEGIN
	INSERT [dbo].[QualificationType] ([QualificationTypeId], [QualificationTypeName], [DisplayOrder], [BulkUploadRef]) VALUES (19, N'Professional or Industry Specific Qualification', 190, N'QT19');
END;
GO

IF NOT EXISTS(SELECT * FROM QualificationTitle)
BEGIN
	SET IDENTITY_INSERT [dbo].[QualificationTitle] ON;
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (1, N'No Qualification');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (2, N'Certificate of Attendance');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (3, N'Pre-Apprenticeship at Level 1');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (4, N'NVQ Level 1');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (5, N'SVQ Level 1');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (6, N'The Diploma - Foundation');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (7, N'Scottish National Qualification - Standard Grade');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (8, N'Scottish National Qualification - Access 3');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (9, N'Scottish National Qualification - Intermediate 1-2');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (10, N'GCSE');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (11, N'Welsh Baccalaureate Diploma');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (12, N'Vocational Related Qualification (VRQ)');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (13, N'The Diploma - Higher');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (14, N'Young Apprenticeship/Apprenticeship at Level 2');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (15, N'First Certificate/Diploma');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (16, N'NVQ Level 2');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (17, N'SVQ Level 2');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (18, N'Advanced Apprenticeship at Level 3');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (19, N'Advanced Extension Award');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (20, N'GCE AS Level');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (21, N'GCE A Level');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (22, N'International Baccalaureate Diploma');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (23, N'National Certificate/Diploma');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (24, N'NVQ Level 3');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (25, N'SVQ Level 3');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (26, N'Scottish National Qualification - Higher');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (27, N'Scottish National Qualification - Advanced Higher');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (28, N'The Diploma - Progression');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (29, N'The Diploma - Advanced');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (30, N'Access to Higher Education');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (31, N'Foundation Art and Design');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (32, N'NVQ Level 4');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (33, N'SVQ Level 4');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (34, N'NVQ Level 5');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (35, N'SVQ Level 5');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (36, N'External Awarding Body Qualifications (Certificate/Diploma/Award)');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (37, N'Course Provider Diploma/Certificate/Award (this must include an assessed element)');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (38, N'Foundation Degree');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (39, N'HNC - Higher National Certificate');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (40, N'HND - Higher National Diploma');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (41, N'Cert HE - Certificate of Higher Education');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (42, N'Dip HE - Diploma of Higher Education');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (43, N'Higher Professional Diplomas');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (44, N'Master Professional Diplomas');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (45, N'BA - Bachelor of Arts');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (46, N'BAcc - Bachelor of Accounting');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (47, N'BAE - Bachelor of Arts and Economics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (48, N'BArch - Bachelor of Architecture');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (49, N'BASc/BAS - Bachelor of Applied Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (50, N'BCL - Bachelor of Civil Law');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (51, N'BCoun - Bachelor of Counseling');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (52, N'BD - Bachelor of Divinity');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (53, N'BDes - Bachelor of Design');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (54, N'BDS or BChD - Bachelor of Dental Surgery');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (55, N'BEcon - Bachelor of Economics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (56, N'BEcon&Fin - Bachelor of Economics and Finance');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (57, N'BEd or EdB - Bachelor of Education');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (58, N'BEng or BE - Bachelor of Engineering');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (59, N'BFA - Bachelor of Fine Art');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (60, N'BFin - Bachelor of Finance');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (61, N'BHSc - Bachelor of Health Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (62, N'BLitt or LittB - Bachelor of Literature or Bachelor of Letters');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (63, N'BMedSc or BMSc - Bachelor of Biomedical science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (64, N'BMid - Bachelor of Midwifery');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (65, N'BMin - Bachelor of Ministry');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (66, N'BMus or MusB - Bachelor of Music');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (67, N'BNurs or BN - Bachelor of Nursing');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (68, N'BPharm - Bachelor of Pharmacy');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (69, N'BPhil - Bachelor of Philosophy');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (70, N'BSc(Psych) - Bachelor of Science in Psychology');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (71, N'BSc - Bachelor of Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (72, N'BSc(Econ) - Bachelor of Science in Economics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (73, N'BSc(Eng) - Bachelor of Science in Engineering');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (74, N'BSocSc - Bachelor of Social Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (75, N'BTchg- Bachelor of Teaching');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (76, N'BTech - Bachelor of Technology (not to be confused with BTEC)');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (77, N'BTh, ThB or BTheol - Bachelor of Theology');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (78, N'LLB - Bachelor of Laws');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (79, N'MB or BM - Bachelor of Medicine');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (80, N'BS, ChB, BChir or BCh - Bachelor of Surgery');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (81, N'BVetMed / BVMS (Bachelors in Veterinary Medicine)/ & Surgery');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (82, N'VetMB - Bachelor of Veterinary Medicine (Cambridge)');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (83, N'MA - with or without honours in Scotland or from Oxbridge');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (84, N'MBiochem, MBiolSci - Master of Biochemistry');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (85, N'MBiol - Master of Biology');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (86, N'MChem - Master of Chemistry');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (87, N'MComp - Master of Computing');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (88, N'MDiv - Master of Divinity');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (89, N'MEcon - Master of Economics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (90, N'MEng - Master of Engineering');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (91, N'MEnvSci - Master of Environmental Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (92, N'MESci - Master of Earth Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (93, N'MGeog - Master of Geography');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (94, N'MGeol - Master of Geology');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (95, N'MGeophys - Master of Geophysics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (96, N'MInf- Master of Informatics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (97, N'MMath - Master of Mathematics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (98, N'MMathComp - Master of Computational Mathematics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (99, N'MMathPhys - Master of Mathematics and Physics, Master of Mathematical Physics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (100, N'MMathStat - Master of Mathematics and Statistics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (101, N'MMORSE - Master of Mathematics, Operational Research, Statistics and Economics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (102, N'MNatSc, MNatSci - Master of Natural Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (103, N'MNursSci - Master of Nursing Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (104, N'MOcean - Master of Oceanography');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (105, N'MPharm - Master of Pharmacy');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (106, N'MPhys - Master of Physics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (107, N'MPlan - Master of Planning');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (108, N'MSci - Master in Science (Master of Natural Science at Cambridge University)');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (109, N'MStat - Master of Statistics');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (110, N'MTheol - Master of Theology');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (111, N'Postgraduate Certificate');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (112, N'Postgraduate Diploma');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (113, N'Graduate Certificate');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (114, N'Graduate Diploma');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (115, N'PGCE - Postgraduate Certificate of Education');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (116, N'PCET - Post Compulsory Education and Training');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (117, N'Postgraduate Higher Diploma');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (118, N'PQSW - Post Qualifying Award in Social Work');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (119, N'LLM - Master of Laws');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (120, N'MA - Master of Arts');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (121, N'MArch - Master of Architecture');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (122, N'MBA - Master of Business Administration');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (123, N'MBiolSci - Master of Biological Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (124, N'MCA - Master of Customs Administration');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (125, N'MClinDent - Master of Clinical Dentistry');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (126, N'MDrama - Master of Drama');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (127, N'MeB - Master of Electronic Business (eBusiness)');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (128, N'MEd - Master of Education');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (129, N'MFA - Master of Fine Art');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (130, N'MJur - Master of Jurisprudence (Law) (Magister Juris at Oxford)');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (131, N'MLib - Master of Librarianship');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (132, N'MLitt - Master of Letters');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (133, N'MMus or MusM - Master of Music');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (134, N'MPH - Master of Public Health');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (135, N'MPhil - Master of Philosophy');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (136, N'MRes - Master of Research');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (137, N'MSc - Master of Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (138, N'MSSc/MSocSc - Master of Social Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (139, N'MSt - Master of Studies');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (140, N'MTL - Master of Teaching and Learning');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (141, N'MTh or MTheol - Master of Theology');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (142, N'MUniv - Master of the University');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (143, N'PhD or DPhil (University of Oxford and a few others) - Doctor of Philosophy');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (144, N'DBA - Doctor of Business Administration');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (145, N'DClinPsych - Doctor of Clinical Psychology');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (146, N'EdD - Doctor of Education');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (147, N'EdPsychD - Doctor of Educational Psychology');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (148, N'EngD - Doctor of Engineering');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (149, N'HScD/DHSci - Doctor of Health Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (150, N'MD or DM - Doctor of Medicine');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (151, N'DMin - Doctor of Ministry');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (152, N'DNursSci - Doctor of Nursing Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (153, N'DProf - Doctor of Professional Studies');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (154, N'SocSciD - Doctor of Social Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (155, N'ThD - Doctor of Theology');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (156, N'DPT - Doctor of Practical Theology');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (157, N'DD - Doctor of Divinity');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (158, N'DCL - Doctor of Civil Law especially at Oxford');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (159, N'LLD - Doctor of Laws');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (160, N'DM or MD - Doctor of Medicine not to be confused with the American MD');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (161, N'DLitt or LittD - Doctor of Letters');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (162, N'DLit - Doctor of Literature');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (163, N'DSc or ScD - Doctor of Science');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (164, N'DMus or MusD - Doctor of Music');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (165, N'DDS - Doctor of Dental Surgery');
	INSERT [dbo].[QualificationTitle] ([QualificationTitleId], [QualficationTitle]) VALUES (166, N'DUniv - Doctor of the University');
	SET IDENTITY_INSERT [dbo].[QualificationTitle] OFF;
END;
GO

IF NOT EXISTS (SELECT * FROM CourseLanguage)
BEGIN
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (1, N'Abkhazian', N'AB', 10);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (2, N'Afan (Oromo)', N'OM', 20);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (3, N'Afar', N'AA', 30);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (4, N'Afrikaans', N'AF', 40);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (5, N'Albanian', N'SQ', 50);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (6, N'Amharic', N'AM', 60);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (7, N'Arabic', N'AR', 70);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (8, N'Armenian', N'HY', 80);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (9, N'Assamese', N'AS', 90);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (10, N'Aymara', N'AY', 100);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (11, N'Azerbaijani', N'AZ', 110);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (12, N'Bashkir', N'BA', 120);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (13, N'Basque', N'EU', 130);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (14, N'Bengali', N'BL', 140);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (15, N'Bangla', N'BN', 150);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (16, N'Bhutani', N'DZ', 160);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (17, N'Bihari', N'BH', 170);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (18, N'Bislama', N'BI', 180);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (19, N'Breton', N'BR', 190);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (20, N'Bulgarian', N'BG', 200);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (21, N'Burmese', N'MY', 210);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (22, N'Byelorussian', N'BE', 220);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (23, N'Cambodian', N'KM', 230);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (24, N'Catalan', N'CA', 240);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (25, N'Chinese(Traditional)', N'ZH', 250);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (26, N'Corsican', N'CO', 260);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (27, N'Croatian', N'HR', 270);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (28, N'Czech', N'CS', 280);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (29, N'Danish', N'DA', 290);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (30, N'Dutch', N'NL', 300);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (31, N'English', N'EN', 310);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (32, N'Esperanto', N'EO', 320);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (33, N'Estonian', N'ET', 330);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (34, N'Faroese', N'FO', 340);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (35, N'Fiji', N'FJ', 350);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (36, N'Finnish', N'FI', 360);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (37, N'French', N'FR', 370);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (38, N'Frisian', N'FY', 380);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (39, N'Galician', N'GL', 390);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (40, N'Georgian', N'KA', 400);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (41, N'German', N'DE', 410);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (42, N'Greek', N'EL', 420);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (43, N'Greenlandic', N'KL', 430);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (44, N'Guarani', N'GN', 440);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (45, N'Gujarati', N'GU', 450);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (46, N'Hausa', N'HA', 460);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (47, N'Hebrew', N'HE', 470);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (48, N'Hindi', N'HI', 480);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (49, N'Hungarian', N'HU', 490);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (50, N'Icelandic', N'IS', 500);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (51, N'Indonesian', N'ID', 510);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (52, N'Interlingua', N'IA', 520);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (53, N'Interlingue', N'IE', 530);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (54, N'Inuktitut', N'IU', 540);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (55, N'Inupiak', N'IK', 550);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (56, N'Irish', N'GA', 560);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (57, N'Italian', N'IT', 570);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (58, N'Japanese', N'JA', 580);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (59, N'Javanese', N'JV', 590);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (60, N'Kannada', N'KN', 600);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (61, N'Kashmiri', N'KS', 610);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (62, N'Kazakh', N'KK', 620);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (63, N'Kinyarwanda', N'RW', 630);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (64, N'Kirghiz', N'KY', 640);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (65, N'Kurundi', N'RN', 650);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (66, N'Korean', N'KO', 660);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (67, N'Kurdish', N'KU', 670);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (68, N'Laothian', N'LO', 680);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (69, N'Latin', N'LA', 690);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (70, N'Latvian', N'LV', 700);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (71, N'Lettish', N'LV', 710);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (72, N'Lingala', N'LN', 720);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (73, N'Lithuanian', N'LT', 730);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (74, N'Macedonian', N'MK', 740);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (75, N'Malagasy', N'MG', 750);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (76, N'Malay', N'MS', 760);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (77, N'Malayalam', N'ML', 770);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (78, N'Maltese', N'MT', 780);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (79, N'Maori', N'MI', 790);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (80, N'Marathi', N'MR', 800);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (81, N'Moldavian', N'MO', 810);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (82, N'Monlian', N'MN', 820);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (83, N'Nauru', N'NA', 830);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (84, N'Nepali', N'NE', 840);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (85, N'Norwegian', N'NO', 850);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (86, N'Occitan', N'OC', 860);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (87, N'Oriya', N'OR', 870);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (88, N'Pashto', N'PS', 880);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (89, N'Pushto', N'PS', 890);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (90, N'Persian', N'FA', 900);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (91, N'Farsi', N'FA', 910);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (92, N'Polish', N'PL', 920);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (93, N'Portuguese', N'PT', 930);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (94, N'Punjabi', N'PA', 940);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (95, N'Quechua', N'QU', 950);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (96, N'Rhaeto-Romance', N'RM', 960);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (97, N'Romanian', N'RO', 970);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (98, N'Russian', N'RU', 980);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (99, N'Samoan', N'SM', 990);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (100, N'Sangho', N'SG', 1000);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (101, N'Sanskrit', N'SA', 1010);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (102, N'Scots Gaelic', N'GD', 1020);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (103, N'Serbian', N'SR', 1030);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (104, N'Serbo-Croatian', N'SH', 1040);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (105, N'Sesotho', N'ST', 1050);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (106, N'Setswana', N'TN', 1060);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (107, N'Shona', N'SN', 1070);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (108, N'Sindhi', N'SD', 1080);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (109, N'Singhalese', N'SI', 1090);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (110, N'Siswati', N'SS', 1100);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (111, N'Slovak', N'SK', 1110);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (112, N'Slovenian', N'SL', 1120);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (113, N'Somali', N'SO', 1130);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (114, N'Spanish', N'ES', 1140);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (115, N'Sundanese', N'SU', 1150);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (116, N'Swahili', N'SW', 1160);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (117, N'Swedish', N'SV', 1170);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (118, N'Tagalog', N'TL', 1180);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (119, N'Tajik', N'TG', 1190);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (120, N'Tamil', N'TA', 1200);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (121, N'Tatar', N'TT', 1210);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (122, N'Telugu', N'TE', 1220);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (123, N'Thai', N'TH', 1230);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (124, N'Tibetan', N'BO', 1240);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (125, N'Tigrinya', N'TI', 1250);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (126, N'Tonga', N'TO', 1260);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (127, N'Tsonga', N'TS', 1270);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (128, N'Turkish', N'TR', 1280);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (129, N'Turkmen', N'TK', 1290);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (130, N'Twi', N'TW', 1300);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (131, N'Uigur', N'UG', 1310);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (132, N'Ukranian', N'UK', 1320);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (133, N'Urdu', N'UR', 1330);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (134, N'Uzbek', N'UZ', 1340);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (135, N'Vietnamese', N'VI', 1350);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (136, N'Volapuk', N'VO', 1360);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (137, N'Welsh', N'CY', 1370);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (138, N'Wolof', N'WO', 1380);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (139, N'Xhosa', N'XH', 1390);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (140, N'Yiddish', N'YI', 1400);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (141, N'Yoruba', N'YO', 1410);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (142, N'Zhuang', N'ZA', 1420);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (143, N'Zulu', N'ZU', 1430);
	INSERT [dbo].[CourseLanguage] ([LanguageId], [Language], [LanguageCode], [DisplayOrder]) VALUES (144, N'Chinese(Simplified)', N'ZS', 1440);
END;
GO

IF NOT EXISTS(SELECT * FROM QualityEmailStatus)
BEGIN
	INSERT [dbo].[QualityEmailStatus] ([QualityEmailStatusId], [QualityEmailStatusDesc]) VALUES (1, N'PROVIDER_EMAIL_1');
	INSERT [dbo].[QualityEmailStatus] ([QualityEmailStatusId], [QualityEmailStatusDesc]) VALUES (2, N'PROVIDER_EMAIL_PAUSE');
	INSERT [dbo].[QualityEmailStatus] ([QualityEmailStatusId], [QualityEmailStatusDesc]) VALUES (3, N'PROVIDER_EMAIL_2');
	INSERT [dbo].[QualityEmailStatus] ([QualityEmailStatusId], [QualityEmailStatusDesc]) VALUES (4, N'RM_EMAIL_1');
	INSERT [dbo].[QualityEmailStatus] ([QualityEmailStatusId], [QualityEmailStatusDesc]) VALUES (5, N'RM_EMAIL_PAUSE');
	INSERT [dbo].[QualityEmailStatus] ([QualityEmailStatusId], [QualityEmailStatusDesc]) VALUES (6, N'CDL_EMAIL_1');
	INSERT [dbo].[QualityEmailStatus] ([QualityEmailStatusId], [QualityEmailStatusDesc]) VALUES (7, N'CDL_EMAIL_PAUSE');
	INSERT [dbo].[QualityEmailStatus] ([QualityEmailStatusId], [QualityEmailStatusDesc]) VALUES (8, N'DART_EMAIL_1');
END;
GO

IF NOT EXISTS(select 1 from [dbo].BulkUploadErrorType where [BulkUploadErrorTypeId]=1)
	INSERT INTO [dbo].BulkUploadErrorType (BulkUploadErrorTypeId, [BulkUploadErrorTypeName])  VALUES  (1, 'Error');
	
IF NOT EXISTS(select 1 from [dbo].BulkUploadErrorType where [BulkUploadErrorTypeId]=2)
	INSERT INTO [dbo].BulkUploadErrorType (BulkUploadErrorTypeId,[BulkUploadErrorTypeName])  VALUES  (2, 'Warning');
 
 if not exists (select 1 from [dbo].[BulkUploadStatus] where [BulkUploadStatusId]=1 )
	 INSERT INTO [dbo].[BulkUploadStatus] ([BulkUploadStatusId],[BulkUploadStatusName],[BulkUploadStatusText])
     VALUES ( 1, 'Failed_Stage_1_of_4','Failed Validation at Stage 1 of 4');

 if not exists (select 1 from [dbo].[BulkUploadStatus] where [BulkUploadStatusId]=2 )
	 INSERT INTO [dbo].[BulkUploadStatus] ([BulkUploadStatusId],[BulkUploadStatusName],[BulkUploadStatusText])
     VALUES ( 2, 'Failed_Stage_2_of_4','Failed Validation at Stage 2 of 4');

if not exists (select 1 from [dbo].[BulkUploadStatus] where [BulkUploadStatusId]=3 )
	 INSERT INTO [dbo].[BulkUploadStatus] ([BulkUploadStatusId],[BulkUploadStatusName],[BulkUploadStatusText])
     VALUES ( 3, 'Failed_Stage_3_of_4','Failed Validation at Stage 3 of 4');

 if not exists (select 1 from [dbo].[BulkUploadStatus] where [BulkUploadStatusId]=4 )
	 INSERT INTO [dbo].[BulkUploadStatus] ([BulkUploadStatusId],[BulkUploadStatusName],[BulkUploadStatusText])
     VALUES ( 4, 'Failed_Stage_4_of_4','Failed Validation at Stage 4 of 4');

if not exists (select 1 from [dbo].[BulkUploadStatus] where [BulkUploadStatusId]=5 )
	 INSERT INTO [dbo].[BulkUploadStatus] ([BulkUploadStatusId],[BulkUploadStatusName],[BulkUploadStatusText])
     VALUES ( 5, 'Needs_Confirmation','Needs Confirmation');

if not exists (select 1 from [dbo].[BulkUploadStatus] where [BulkUploadStatusId]=6 )
	 INSERT INTO [dbo].[BulkUploadStatus] ([BulkUploadStatusId],[BulkUploadStatusName],[BulkUploadStatusText])
     VALUES ( 6, 'Aborted','Aborted');

if not exists (select 1 from [dbo].[BulkUploadStatus] where [BulkUploadStatusId]=7 )
	 INSERT INTO [dbo].[BulkUploadStatus] ([BulkUploadStatusId],[BulkUploadStatusName],[BulkUploadStatusText])
     VALUES ( 7, 'Published','Published');

if not exists (select 1 from [dbo].[BulkUploadStatus] where [BulkUploadStatusId]=8 )
	 INSERT INTO [dbo].[BulkUploadStatus] ([BulkUploadStatusId],[BulkUploadStatusName],[BulkUploadStatusText])
     VALUES ( 8, 'UnknownException','Unknown Exception');

if not exists (select 1 from [dbo].[BulkUploadStatus] where [BulkUploadStatusId]=9 )
	 INSERT INTO [dbo].[BulkUploadStatus] ([BulkUploadStatusId],[BulkUploadStatusName],[BulkUploadStatusText])
     VALUES ( 9, 'NoValidRecords','No Valid Records');

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '16AA903F-DDBE-47B7-9723-B03907DD0D43')
BEGIN
	PRINT '[Updating Study Mode Lookup Data]';
	SET NOCOUNT ON;
	
	UPDATE [dbo].[StudyMode] SET [StudyModeName] = 'Full time' WHERE [StudyModeId] = 1;
	UPDATE [dbo].[StudyMode] SET [StudyModeName] = 'Part of a full-time program' WHERE [StudyModeId] = 3;
	
	INSERT INTO __RefactorLog (OperationKey) VALUES ('16AA903F-DDBE-47B7-9723-B03907DD0D43');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '85A0D3D7-B42A-4B1B-B04E-91DDDC61F348')
BEGIN
	PRINT '[Updating Attendance Type Lookup Data]';
	SET NOCOUNT ON;
	
	UPDATE [dbo].AttendanceType SET AttendanceTypeName = 'Location / campus' WHERE AttendanceTypeId = 1;
	UPDATE [dbo].AttendanceType SET AttendanceTypeName = 'Online with attendance' WHERE AttendanceTypeId = 8;
	UPDATE [dbo].AttendanceType SET AttendanceTypeName = 'Online without attendance' WHERE AttendanceTypeId = 7;

 	INSERT INTO __RefactorLog (OperationKey) VALUES ('85A0D3D7-B42A-4B1B-B04E-91DDDC61F348');
END;
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[DfEProviderType])
BEGIN
	INSERT INTO [dbo].[DfEProviderType] ([DfEProviderTypeId], [DfEProviderTypeCode], [DfEProviderTypeName]) VALUES (1, '001', 'Establishment');
	INSERT INTO [dbo].[DfEProviderType] ([DfEProviderTypeId], [DfEProviderTypeCode], [DfEProviderTypeName]) VALUES (2, '002', 'Local authority');
	INSERT INTO [dbo].[DfEProviderType] ([DfEProviderTypeId], [DfEProviderTypeCode], [DfEProviderTypeName]) VALUES (3, '003', 'Other types');
	INSERT INTO [dbo].[DfEProviderType] ([DfEProviderTypeId], [DfEProviderTypeCode], [DfEProviderTypeName]) VALUES (4, '004', 'Early Year Setting');
	INSERT INTO [dbo].[DfEProviderType] ([DfEProviderTypeId], [DfEProviderTypeCode], [DfEProviderTypeName]) VALUES (5, '005', 'Additional Type 1');
	INSERT INTO [dbo].[DfEProviderType] ([DfEProviderTypeId], [DfEProviderTypeCode], [DfEProviderTypeName]) VALUES (6, '006', 'Additional Type 2');
	INSERT INTO [dbo].[DfEProviderType] ([DfEProviderTypeId], [DfEProviderTypeCode], [DfEProviderTypeName]) VALUES (7, '007', 'Additional Type 3');
	INSERT INTO [dbo].[DfEProviderType] ([DfEProviderTypeId], [DfEProviderTypeCode], [DfEProviderTypeName]) VALUES (8, '008', 'Other Stakeholders');
	INSERT INTO [dbo].[DfEProviderType] ([DfEProviderTypeId], [DfEProviderTypeCode], [DfEProviderTypeName]) VALUES (9, '009', 'Training Providers');
	INSERT INTO [dbo].[DfEProviderType] ([DfEProviderTypeId], [DfEProviderTypeCode], [DfEProviderTypeName]) VALUES (10, '010', 'Multi-Academy Trust');
END;
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[DfERegion])
BEGIN
	INSERT INTO [dbo].[DfERegion] ([DfERegionId], [DfERegionCode], [DfERegionName]) VALUES (1, 'A', 'North East');
	INSERT INTO [dbo].[DfERegion] ([DfERegionId], [DfERegionCode], [DfERegionName]) VALUES (2, 'B', 'North West');
	INSERT INTO [dbo].[DfERegion] ([DfERegionId], [DfERegionCode], [DfERegionName]) VALUES (4, 'D', 'Yorkshire and the Humber');
	INSERT INTO [dbo].[DfERegion] ([DfERegionId], [DfERegionCode], [DfERegionName]) VALUES (5, 'E', 'East Midlands');
	INSERT INTO [dbo].[DfERegion] ([DfERegionId], [DfERegionCode], [DfERegionName]) VALUES (6, 'F', 'West Midlands');
	INSERT INTO [dbo].[DfERegion] ([DfERegionId], [DfERegionCode], [DfERegionName]) VALUES (7, 'G', 'East of England');
	INSERT INTO [dbo].[DfERegion] ([DfERegionId], [DfERegionCode], [DfERegionName]) VALUES (8, 'H', 'London');
	INSERT INTO [dbo].[DfERegion] ([DfERegionId], [DfERegionCode], [DfERegionName]) VALUES (10, 'J', 'South East');
	INSERT INTO [dbo].[DfERegion] ([DfERegionId], [DfERegionCode], [DfERegionName]) VALUES (11, 'K', 'South West');
	INSERT INTO [dbo].[DfERegion] ([DfERegionId], [DfERegionCode], [DfERegionName]) VALUES (23, 'W', 'Wales (pseudo)');
	INSERT INTO [dbo].[DfERegion] ([DfERegionId], [DfERegionCode], [DfERegionName]) VALUES (26, 'Z', 'Not Applicable');
END;
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[DfEWsProviderStatus])
BEGIN
	INSERT INTO [dbo].[DfEWsProviderStatus] ([DfEWsProviderStatusId], [DfEWsProviderStatusCode], [DfEWsProviderStatusName]) VALUES (1, '1', 'Active');
	INSERT INTO [dbo].[DfEWsProviderStatus] ([DfEWsProviderStatusId], [DfEWsProviderStatusCode], [DfEWsProviderStatusName]) VALUES (2, '2', 'Closed');
END;
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[DfEProviderStatus])
BEGIN
	INSERT INTO [dbo].[DfEProviderStatus] ([DfEProviderStatusId], [DfEProviderStatusCode], [DfEProviderStatusName], [DfEWsProviderStatusId]) VALUES (1, '1', 'Open', 1);
	INSERT INTO [dbo].[DfEProviderStatus] ([DfEProviderStatusId], [DfEProviderStatusCode], [DfEProviderStatusName], [DfEWsProviderStatusId]) VALUES (2, '2', 'Closed', 2);
	INSERT INTO [dbo].[DfEProviderStatus] ([DfEProviderStatusId], [DfEProviderStatusCode], [DfEProviderStatusName], [DfEWsProviderStatusId]) VALUES (3, '3', 'Closed but active', 1);
	INSERT INTO [dbo].[DfEProviderStatus] ([DfEProviderStatusId], [DfEProviderStatusCode], [DfEProviderStatusName], [DfEWsProviderStatusId]) VALUES (4, '4', 'Proposed to open', 1);
END;
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[DfEEstablishmentStatus])
BEGIN
	INSERT INTO [dbo].[DfEEstablishmentStatus] ([DfEEstablishmentStatusId], [DfEEstablishmentStatusCode], [DfEEstablishmentStatusName], [DfEProviderStatusId]) VALUES (10, '10', 'ARCHIVED', 2);
	INSERT INTO [dbo].[DfEEstablishmentStatus] ([DfEEstablishmentStatusId], [DfEEstablishmentStatusCode], [DfEEstablishmentStatusName], [DfEProviderStatusId]) VALUES (2, '2', 'Closed', 3);
	INSERT INTO [dbo].[DfEEstablishmentStatus] ([DfEEstablishmentStatusId], [DfEEstablishmentStatusCode], [DfEEstablishmentStatusName], [DfEProviderStatusId]) VALUES (9, '9', 'Created in Error', NULL);
	INSERT INTO [dbo].[DfEEstablishmentStatus] ([DfEEstablishmentStatusId], [DfEEstablishmentStatusCode], [DfEEstablishmentStatusName], [DfEProviderStatusId]) VALUES (5, '5', 'De-Registered as EY Setting', NULL);
	INSERT INTO [dbo].[DfEEstablishmentStatus] ([DfEEstablishmentStatusId], [DfEEstablishmentStatusCode], [DfEEstablishmentStatusName], [DfEProviderStatusId]) VALUES (1, '1', 'Open', 1);
	INSERT INTO [dbo].[DfEEstablishmentStatus] ([DfEEstablishmentStatusId], [DfEEstablishmentStatusCode], [DfEEstablishmentStatusName], [DfEProviderStatusId]) VALUES (3, '3', 'Open, but proposed to close', 1);
	INSERT INTO [dbo].[DfEEstablishmentStatus] ([DfEEstablishmentStatusId], [DfEEstablishmentStatusCode], [DfEEstablishmentStatusName], [DfEProviderStatusId]) VALUES (7, '7', 'Pending Approval', NULL);
	INSERT INTO [dbo].[DfEEstablishmentStatus] ([DfEEstablishmentStatusId], [DfEEstablishmentStatusCode], [DfEEstablishmentStatusName], [DfEProviderStatusId]) VALUES (4, '4', 'Proposed to open', NULL);
	INSERT INTO [dbo].[DfEEstablishmentStatus] ([DfEEstablishmentStatusId], [DfEEstablishmentStatusCode], [DfEEstablishmentStatusName], [DfEProviderStatusId]) VALUES (6, '6', 'Quarantine', NULL);
	INSERT INTO [dbo].[DfEEstablishmentStatus] ([DfEEstablishmentStatusId], [DfEEstablishmentStatusCode], [DfEEstablishmentStatusName], [DfEProviderStatusId]) VALUES (8, '8', 'Rejected Opening', NULL);
END;
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[DfEEstablishmentType])
BEGIN
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (1, '1', 'Community School');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (2, '2', 'Voluntary Aided School');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (3, '3', 'Voluntary Controlled School');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (5, '5', 'Foundation School');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (6, '6', 'City Technology College');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (7, '7', 'Community Special School');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (8, '8', 'Non-Maintained Special School');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (9, '9', 'Independent School Approved for SEN Pupils');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (10, '10', 'Other Independent Special School');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (11, '11', 'Other Independent School');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (12, '12', 'Foundation Special School');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (14, '14', 'Pupil Referral Unit');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (15, '15', 'LA Nursery School');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (17, '17', 'European Schools');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (18, '18', 'Further Education');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (22, '22', 'EY Setting');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (23, '23', 'Playing for Success Centres');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (24, '24', 'Secure Units');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (25, '25', 'Offshore Schools');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (26, '26', 'Service Childrens Education');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (27, '27', 'Miscellaneous');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (28, '28', 'Academy Sponsor Led');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (29, '29', 'Higher Education Institutions');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (30, '30', 'Welsh Establishment');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (31, '31', 'Sixth Form Centres');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (32, '32', 'Special Post 16 Institution');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (33, '33', 'Academy Special Sponsor Led');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (34, '34', 'Academy Converter');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (35, '35', 'Free Schools');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (36, '36', 'Free Schools Special');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (37, '37', 'British Schools Overseas');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (38, '38', 'Free Schools - Alternative Provision');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (39, '39', 'Free Schools - 16-19');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (40, '40', 'University Technical College');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (41, '41', 'Studio Schools');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (42, '42', 'Academy Alternative Provision Converter');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (43, '43', 'Academy Alternative Provision Sponsor Led');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (44, '44', 'Academy Special Converter');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (45, '45', 'Academy 16-19 Converter');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (46, '46', 'Academy 16-19 Sponsor Led');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (56, '56', 'Institution funded by other Government Department');
	INSERT INTO [dbo].[DfEEstablishmentType] ([DfEEstablishmentTypeId], [DfEEstablishmentTypeCode], [DfEEstablishmentTypeName]) VALUES (98, '98', 'Legacy types');
END;
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[DfEEstablishmentPhase])
BEGIN
	INSERT INTO [dbo].[DfEEstablishmentPhase] ([DfEEstablishmentPhaseId], [DfEEstablishmentPhaseCode], [DfEEstablishmentPhaseName]) VALUES (0, '0', 'Not applicable');
	INSERT INTO [dbo].[DfEEstablishmentPhase] ([DfEEstablishmentPhaseId], [DfEEstablishmentPhaseCode], [DfEEstablishmentPhaseName]) VALUES (1, '1', 'Nursery');
	INSERT INTO [dbo].[DfEEstablishmentPhase] ([DfEEstablishmentPhaseId], [DfEEstablishmentPhaseCode], [DfEEstablishmentPhaseName]) VALUES (2, '2', 'Primary');
	INSERT INTO [dbo].[DfEEstablishmentPhase] ([DfEEstablishmentPhaseId], [DfEEstablishmentPhaseCode], [DfEEstablishmentPhaseName]) VALUES (3, '3', 'Middle Deemed Primary');
	INSERT INTO [dbo].[DfEEstablishmentPhase] ([DfEEstablishmentPhaseId], [DfEEstablishmentPhaseCode], [DfEEstablishmentPhaseName]) VALUES (4, '4', 'Secondary');
	INSERT INTO [dbo].[DfEEstablishmentPhase] ([DfEEstablishmentPhaseId], [DfEEstablishmentPhaseCode], [DfEEstablishmentPhaseName]) VALUES (5, '5', 'Middle Deemed Secondary');
	INSERT INTO [dbo].[DfEEstablishmentPhase] ([DfEEstablishmentPhaseId], [DfEEstablishmentPhaseCode], [DfEEstablishmentPhaseName]) VALUES (6, '6', '16 Plus');
	INSERT INTO [dbo].[DfEEstablishmentPhase] ([DfEEstablishmentPhaseId], [DfEEstablishmentPhaseCode], [DfEEstablishmentPhaseName]) VALUES (7, '7', 'All Through');
	INSERT INTO [dbo].[DfEEstablishmentPhase] ([DfEEstablishmentPhaseId], [DfEEstablishmentPhaseCode], [DfEEstablishmentPhaseName]) VALUES (9, '9', 'Unknown');
END;
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[DfELocalAuthority])
BEGIN
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (000, '000', 'Does not apply');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (001, '001', 'DfE');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (201, '201', 'City of London');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (202, '202', 'Camden');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (203, '203', 'Greenwich');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (204, '204', 'Hackney');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (205, '205', 'Hammersmith and Fulham');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (206, '206', 'Islington');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (207, '207', 'Kensington and Chelsea');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (208, '208', 'Lambeth');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (209, '209', 'Lewisham');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (210, '210', 'Southwark');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (211, '211', 'Tower Hamlets');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (212, '212', 'Wandsworth');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (213, '213', 'Westminster');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (301, '301', 'Barking and Dagenham');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (302, '302', 'Barnet');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (303, '303', 'Bexley');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (304, '304', 'Brent');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (305, '305', 'Bromley');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (306, '306', 'Croydon');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (307, '307', 'Ealing');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (308, '308', 'Enfield');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (309, '309', 'Haringey');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (310, '310', 'Harrow');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (311, '311', 'Havering');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (312, '312', 'Hillingdon');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (313, '313', 'Hounslow');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (314, '314', 'Kingston upon Thames');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (315, '315', 'Merton');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (316, '316', 'Newham');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (317, '317', 'Redbridge');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (318, '318', 'Richmond upon Thames');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (319, '319', 'Sutton');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (320, '320', 'Waltham Forest');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (330, '330', 'Birmingham');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (331, '331', 'Coventry');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (332, '332', 'Dudley');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (333, '333', 'Sandwell');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (334, '334', 'Solihull');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (335, '335', 'Walsall');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (336, '336', 'Wolverhampton');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (340, '340', 'Knowsley');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (341, '341', 'Liverpool');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (342, '342', 'St. Helens');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (343, '343', 'Sefton');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (344, '344', 'Wirral');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (350, '350', 'Bolton');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (351, '351', 'Bury');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (352, '352', 'Manchester');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (353, '353', 'Oldham');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (354, '354', 'Rochdale');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (355, '355', 'Salford');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (356, '356', 'Stockport');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (357, '357', 'Tameside');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (358, '358', 'Trafford');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (359, '359', 'Wigan');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (370, '370', 'Barnsley');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (371, '371', 'Doncaster');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (372, '372', 'Rotherham');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (373, '373', 'Sheffield');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (380, '380', 'Bradford');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (381, '381', 'Calderdale');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (382, '382', 'Kirklees');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (383, '383', 'Leeds');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (384, '384', 'Wakefield');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (390, '390', 'Gateshead');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (391, '391', 'Newcastle upon Tyne');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (392, '392', 'North Tyneside');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (393, '393', 'South Tyneside');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (394, '394', 'Sunderland');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (420, '420', 'Isles Of Scilly');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (660, '660', 'Isle of Anglesey');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (661, '661', 'Gwynedd');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (662, '662', 'Conwy');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (663, '663', 'Denbighshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (664, '664', 'Flintshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (665, '665', 'Wrexham');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (666, '666', 'Powys');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (667, '667', 'Ceredigion');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (668, '668', 'Pembrokeshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (669, '669', 'Carmarthenshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (670, '670', 'Swansea');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (671, '671', 'Neath Port Talbot');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (672, '672', 'Bridgend');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (673, '673', 'The Vale of Glamorgan');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (674, '674', 'Rhondda, Cynon, Taff');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (675, '675', 'Merthyr Tydfil');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (676, '676', 'Caerphilly');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (677, '677', 'Blaenau Gwent');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (678, '678', 'Torfaen');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (679, '679', 'Monmouthshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (680, '680', 'Newport');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (681, '681', 'Cardiff');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (702, '702', 'BFPO Overseas Establishments');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (705, '705', 'Isle of Man Offshore Establishments');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (706, '706', 'Guernsey Offshore Establishments');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (707, '707', 'Jersey Offshore Establishments');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (800, '800', 'Bath and North East Somerset');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (801, '801', 'Bristol City of');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (802, '802', 'North Somerset');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (803, '803', 'South Gloucestershire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (805, '805', 'Hartlepool');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (806, '806', 'Middlesbrough');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (807, '807', 'Redcar and Cleveland');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (808, '808', 'Stockton-on-Tees');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (810, '810', 'Kingston upon Hull City of');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (811, '811', 'East Riding of Yorkshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (812, '812', 'North East Lincolnshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (813, '813', 'North Lincolnshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (815, '815', 'North Yorkshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (816, '816', 'York');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (821, '821', 'Luton');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (822, '822', 'Bedford');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (823, '823', 'Central Bedfordshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (825, '825', 'Buckinghamshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (826, '826', 'Milton Keynes');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (830, '830', 'Derbyshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (831, '831', 'Derby');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (835, '835', 'Dorset');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (836, '836', 'Poole');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (837, '837', 'Bournemouth');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (840, '840', 'Durham');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (841, '841', 'Darlington');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (845, '845', 'East Sussex');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (846, '846', 'Brighton and Hove');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (850, '850', 'Hampshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (851, '851', 'Portsmouth');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (852, '852', 'Southampton');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (855, '855', 'Leicestershire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (856, '856', 'Leicester');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (857, '857', 'Rutland');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (860, '860', 'Staffordshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (861, '861', 'Stoke-on-Trent');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (865, '865', 'Wiltshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (866, '866', 'Swindon');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (867, '867', 'Bracknell Forest');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (868, '868', 'Windsor and Maidenhead');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (869, '869', 'West Berkshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (870, '870', 'Reading');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (871, '871', 'Slough');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (872, '872', 'Wokingham');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (873, '873', 'Cambridgeshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (874, '874', 'Peterborough');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (876, '876', 'Halton');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (877, '877', 'Warrington');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (878, '878', 'Devon');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (879, '879', 'Plymouth');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (880, '880', 'Torbay');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (881, '881', 'Essex');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (882, '882', 'Southend-on-Sea');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (883, '883', 'Thurrock');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (884, '884', 'Herefordshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (885, '885', 'Worcestershire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (886, '886', 'Kent');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (887, '887', 'Medway');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (888, '888', 'Lancashire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (889, '889', 'Blackburn with Darwen');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (890, '890', 'Blackpool');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (891, '891', 'Nottinghamshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (892, '892', 'Nottingham');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (893, '893', 'Shropshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (894, '894', 'Telford and Wrekin');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (895, '895', 'Cheshire East');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (896, '896', 'Cheshire West and Chester');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (908, '908', 'Cornwall');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (909, '909', 'Cumbria');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (916, '916', 'Gloucestershire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (919, '919', 'Hertfordshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (921, '921', 'Isle of Wight');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (925, '925', 'Lincolnshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (926, '926', 'Norfolk');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (928, '928', 'Northamptonshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (929, '929', 'Northumberland');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (931, '931', 'Oxfordshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (933, '933', 'Somerset');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (935, '935', 'Suffolk');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (936, '936', 'Surrey');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (937, '937', 'Warwickshire');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (938, '938', 'West Sussex');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (999, '999', 'Unknown');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (503, '503', 'SDA');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (500, '500', 'SMS LA');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (501, '501', 'Capita LA');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (502, '502', 'Serco LA');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (504, '504', 'Bromcom LA');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (505, '505', 'Wauton Samuel LA');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (506, '506', 'Ringwood LA');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (507, '507', 'Pearson Phoenix LA');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (508, '508', 'Centime Systems Development');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (509, '509', 'Accurate Solutions LA');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (510, '510', 'Pearson Phoenix Contractors LA');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (102, '102', 'Alliance Care & education');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (103, '103', 'Barford Care');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (104, '104', 'Care Today Childrens Services');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (105, '105', 'Castle Homes');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (106, '106', 'Cothill Educational trust');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (107, '107', 'Greencorns');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (108, '108', 'Meadows Care');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (109, '109', 'Education Youth Services');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (561, '561', 'SS Scholar LA');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (519, '519', 'WCBS LA');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (520, '520', 'Texunatech LA 1');
	INSERT INTO [dbo].[DfELocalAuthority] ([DfELocalAuthorityId], [DfELocalAuthorityCode], [DfELocalAuthorityName]) VALUES (521, '521', 'Texunatech LA 2');
END;
GO

IF NOT EXISTS(SELECT * FROM [Search].[DataExportConfiguration])
BEGIN
	INSERT INTO [Search].[DataExportConfiguration] (ThresholdPercent, OverrideThreshold, IsEnabled) VALUES (75, 0, 1);
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'CF98CE4E-59F8-48AE-AAD4-1F27DAB1C139')
BEGIN
	PRINT '[Updating Funding Code Names]'
	SET NOCOUNT ON;
	
	UPDATE [dbo].[A10FundingCode] SET A10FundingCodeName = N'DfE 16-19 Funded' WHERE A10FundingCodeId = 25;
	UPDATE [dbo].[A10FundingCode] SET A10FundingCodeName = N'No SPA or EFA funding for this learning aim' WHERE A10FundingCodeId = 99;
	
	INSERT INTO __RefactorLog (OperationKey) VALUES ('CF98CE4E-59F8-48AE-AAD4-1F27DAB1C139');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '786BDEBE-5E31-4713-9D61-05B2BEB7232F')
BEGIN
	PRINT '[Updating Funding Code Statuses]'
	SET NOCOUNT ON;
	
	UPDATE [dbo].[A10FundingCode] SET RecordStatusId = 4 WHERE A10FundingCodeId IN (21, 22, 45, 46, 80, 82);
	
	INSERT INTO __RefactorLog (OperationKey) VALUES ('786BDEBE-5E31-4713-9D61-05B2BEB7232F');
END;
GO

IF NOT EXISTS (SELECT * FROM BulkUploadErrorType WHERE BulkUploadErrorTypeId = 5)
BEGIN
	INSERT INTO BulkUploadErrorType (BulkUploadErrorTypeId, BulkUploadErrorTypeName) VALUES (5, 'Notice');
END;



IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '8EF04ECD-34A9-4DCB-A9EF-EC6E3FA396AE')
BEGIN
	PRINT '[Hiding 14-19 Diploma Qualification]'
	SET NOCOUNT ON;

	UPDATE [dbo].[QualificationType] SET IsHidden = 1, QualificationTypeName = '14-19 Diploma and relevant components' WHERE QualificationTypeId = 9;

	INSERT INTO __RefactorLog (OperationKey) VALUES ('8EF04ECD-34A9-4DCB-A9EF-EC6E3FA396AE');
END;

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'F053B4CF-0795-4371-AF20-37DFAD4454C4')
BEGIN
	PRINT '[Creating Unvalidated Bulk Upload Status]'
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT * FROM [dbo].[BulkUploadStatus] WHERE [BulkUploadStatusName] = 'Unvalidated')
	BEGIN
		INSERT INTO [dbo].[BulkUploadStatus]
			(BulkUploadStatusId, BulkUploadStatusName, BulkUploadStatusText)
		VALUES
			(10, 'Unvalidated', 'Not yet validated')
	END

	INSERT INTO __RefactorLog (OperationKey) VALUES ('F053B4CF-0795-4371-AF20-37DFAD4454C4');
END;
GO

IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = 'E8C192B9-6B19-40BD-9924-10F2B33C140F')
BEGIN
	PRINT '[Creating ConfirmationReceived Bulk Upload Status]'
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT * FROM [dbo].[BulkUploadStatus] WHERE [BulkUploadStatusName] = 'ConfirmationReceived')
	BEGIN
		INSERT INTO [dbo].[BulkUploadStatus]
			(BulkUploadStatusId, BulkUploadStatusName, BulkUploadStatusText)
		VALUES
			(11, 'ConfirmationReceived', 'Confirmation Received')
	END

	INSERT INTO __RefactorLog (OperationKey) VALUES ('E8C192B9-6B19-40BD-9924-10F2B33C140F');
END;
GO
IF NOT EXISTS (SELECT 1 FROM __RefactorLog WHERE OperationKey = '6DE8E139-FE5E-4608-8908-44113BFC4BC1')
BEGIN
	PRINT '[Creating ConfirmationReceived Bulk Upload Status]'
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT * FROM [dbo].[BulkUploadStatus] WHERE [BulkUploadStatusName] = 'Validated')
	BEGIN
		INSERT INTO [dbo].[BulkUploadStatus]
			(BulkUploadStatusId, BulkUploadStatusName, BulkUploadStatusText)
		VALUES
			(12, 'Validated', 'Validated')
	END

	INSERT INTO __RefactorLog (OperationKey) VALUES ('6DE8E139-FE5E-4608-8908-44113BFC4BC1');
END;
GO

IF NOT EXISTS (SELECT * FROM [dbo].[MetadataUploadType])
BEGIN
	INSERT INTO [dbo].[MetadataUploadType]
		(MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription)
	VALUES
		(1, 'AddressBase', 'UK Address Data');

	INSERT INTO [dbo].[MetadataUploadType]
		(MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription)
	VALUES
		(2, 'LearnDirectClassification', 'Learndirect Classifications');

	INSERT INTO [dbo].[MetadataUploadType]
		(MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription)
	VALUES
		(3, 'LearningAimAwardOrg', 'Learning Aim Award Organisations');

	INSERT INTO [dbo].[MetadataUploadType]
		(MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription)
	VALUES
		(4, 'LearningAim', 'Learning Aims');

	INSERT INTO [dbo].[MetadataUploadType]
		(MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription)
	VALUES
		(5, 'LearningAimValidity', 'Learning Aim Validity');

	INSERT INTO [dbo].[MetadataUploadType]
		(MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription)
	VALUES
		(6, 'FEChoices', 'FE Choices');

	INSERT INTO [dbo].[MetadataUploadType]
		(MetadataUploadTypeId, MetadataUploadTypeName, MetadataUploadTypeDescription)
	VALUES
		(7, 'UKRLP', 'UK Register of Learning Providers');
END
GO

IF NOT EXISTS (SELECT * FROM DeliveryMode)
BEGIN
	INSERT [dbo].[DeliveryMode] ([DeliveryModeId], [DeliveryModeName], [DeliveryModeDescription], [RecordStatusId], [BulkUploadRef], [DASRef]) VALUES (1, N'100% Employer Based','Entirely delivered at employer''s premises', 2, 'DM1', '100PercentEmployer');
    INSERT [dbo].[DeliveryMode] ([DeliveryModeId], [DeliveryModeName], [DeliveryModeDescription], [RecordStatusId], [BulkUploadRef], [DASRef]) VALUES (2, N'Day Release','Apprentices given days off to study', 2, 'DM2', 'DayRelease');
	INSERT [dbo].[DeliveryMode] ([DeliveryModeId], [DeliveryModeName], [DeliveryModeDescription], [RecordStatusId], [BulkUploadRef], [DASRef]) VALUES (3, N'Block Release', 'Apprentices given blocks of time off to study', 2, 'DM3', 'BlockRelease');
END
GO

IF NOT EXISTS (SELECT * FROM BulkUploadSection)
BEGIN
	INSERT [dbo].[BulkUploadSection] ([BulkUploadSectionId], [BulkUploadSectionName]) VALUES (1, N'Providers');
	INSERT [dbo].[BulkUploadSection] ([BulkUploadSectionId], [BulkUploadSectionName]) VALUES (2, N'Venues');
	INSERT [dbo].[BulkUploadSection] ([BulkUploadSectionId], [BulkUploadSectionName]) VALUES (3, N'Courses');
	INSERT [dbo].[BulkUploadSection] ([BulkUploadSectionId], [BulkUploadSectionName]) VALUES (4, N'Opportunities');
	INSERT [dbo].[BulkUploadSection] ([BulkUploadSectionId], [BulkUploadSectionName]) VALUES (5, N'Locations');
	INSERT [dbo].[BulkUploadSection] ([BulkUploadSectionId], [BulkUploadSectionName]) VALUES (6, N'Apprenticeships');
	INSERT [dbo].[BulkUploadSection] ([BulkUploadSectionId], [BulkUploadSectionName]) VALUES (7, N'Delivery Locations');
END
GO

IF NOT EXISTS (SELECT * FROM LocationAliasType)
BEGIN
	INSERT INTO [dbo].[LocationAliasType] ([LocationAliasTypeId], [LocationAliasTypeName]) VALUES (1, N'County');
END

IF NOT EXISTS (SELECT * FROM LocationAlias)
BEGIN
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (1, NULL, N'Avon', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (2, NULL, N'Bedfordshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (3, 2, N'Beds', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (4, NULL, N'Berkshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (5, 4, N'Berks', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (6, NULL, N'Buckinghamshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (7, 6, N'Bucks', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (8, NULL, N'Cambridgeshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (9, 8, N'Cambs', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (10, NULL, N'Cheshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (11, NULL, N'Cleveland', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (12, NULL, N'Cornwall and Isles of Scilly', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (13, 12, N'Cornwall', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (14, NULL, N'Cumbria', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (15, NULL, N'Derbyshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (16, NULL, N'Devon', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (17, NULL, N'Dorset', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (18, NULL, N'Durham', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (19, 18, N'County Durham', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (20, 18, N'C Durham', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (21, 18, N'C. Durham', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (22, NULL, N'East Sussex', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (23, 22, N'E Sussex', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (24, 22, N'E. Sussex', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (25, NULL, N'Essex', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (26, NULL, N'Gloucestershire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (27, 26, N'Gloucs', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (28, NULL, N'Greater London', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (29, 28, N'London', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (30, NULL, N'Greater Manchester', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (31, 30, N'Manchester', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (32, 30, N'Mancs', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (33, 30, N'Greater Mancs', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (34, NULL, N'Hampshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (35, 34, N'Hants', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (36, NULL, N'Hereford and Worcester', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (37, NULL, N'Hertfordshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (38, 37, N'Herts', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (39, NULL, N'Humberside', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (40, NULL, N'Isle of Wight', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (41, 40, N'IoW', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (42, NULL, N'Kent', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (43, NULL, N'Lancashire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (44, 43, N'Lancs', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (45, NULL, N'Leicestershire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (46, 45, N'Leics', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (47, NULL, N'Lincolnshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (48, 47, N'Lincs', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (49, NULL, N'Merseyside', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (50, NULL, N'Norfolk', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (51, NULL, N'North Yorkshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (52, 51, N'N Yorkshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (53, 51, N'N Yorks', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (54, 51, N'North Yorks', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (55, 51, N'N. Yorkshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (56, 51, N'N. Yorks', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (57, NULL, N'Northamptonshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (58, 57, N'Northants', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (59, NULL, N'Northumberland', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (60, NULL, N'Nottinghamshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (61, 60, N'Notts', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (62, NULL, N'Oxfordshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (63, 62, N'Oxon', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (64, NULL, N'Shropshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (65, 64, N'Salop', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (66, NULL, N'Somerset', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (67, NULL, N'South Yorkshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (68, 67, N'S Yorkshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (69, 67, N'S Yorks', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (70, 67, N'South Yorks', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (71, 67, N'S. Yorkshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (72, 67, N'S. Yorks', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (73, NULL, N'Staffordshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (74, 73, N'Staffs', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (75, NULL, N'Suffolk', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (76, NULL, N'Surrey', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (77, NULL, N'Tyne and Wear', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (78, NULL, N'Warwickshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (79, NULL, N'West Midlands', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (80, 79, N'W Midlands', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (81, 79, N'West Mids', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (82, 79, N'W. Mids', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (83, 79, N'W. Midlands', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (84, 79, N'W Mids', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (85, NULL, N'West Sussex', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (86, 85, N'W Sussex', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (87, 85, N'W. Sussex', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (88, NULL, N'West Yorkshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (89, 88, N'W Yorkshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (90, 88, N'W Yorks', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (91, 88, N'West Yorks', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (92, 88, N'W. Yorks', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (93, 88, N'W. Yorkshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (94, NULL, N'Wiltshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (95, 94, N'Wilts', 1);
END

--IF NOT EXISTS (SELECT * FROM [dbo].[LocationAlias] WHERE [LocationAliasId] = 96)
--BEGIN
--	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (96, 22, N'Sussex', 1);
--	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (97, 85, N'Sussex', 1);
--	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (98, 88, N'Yorks', 1);
--	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (99, 67, N'Yorks', 1);
--	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (100, 88, N'Yorkshire', 1);
--	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (101, 67, N'Yorkshire', 1);
--	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (102, 79, N'Midlands', 1);
--	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (103, 79, N'Mids', 1);
--END

IF EXISTS (SELECT * FROM [dbo].[LocationAlias] WHERE [LocationAliasId] = 96 AND [LocationAliasName] = 'Sussex')
BEGIN
	DELETE FROM [dbo].[LocationAlias] WHERE [LocationAliasId] IN (96, 97, 98, 99, 100, 101, 102, 103);
END;

IF NOT EXISTS (SELECT * FROM [dbo].[LocationAlias] WHERE [ParentLocationAliasId] = 15)
BEGIN
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (96, 15, N'Derbys', 1);
END;

IF NOT EXISTS (SELECT * FROM [dbo].[LocationAlias] WHERE [LocationAliasId] = 97)
BEGIN
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (97, NULL, N'Middlesex', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (98, 97, N'Middx', 1);
END;

IF NOT EXISTS (SELECT * FROM [dbo].[LocationAlias] WHERE [LocationAliasId] = 99)
BEGIN
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (99, NULL, N'North Humberside', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (100, 99, N'N Humbs', 1);
END;

IF NOT EXISTS (SELECT * FROM [dbo].[LocationAlias] WHERE [LocationAliasId] = 101)
BEGIN
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (101, 59, N'Northd', 1);
END;

IF NOT EXISTS (SELECT * FROM [dbo].[LocationAlias] WHERE [LocationAliasId] = 102)
BEGIN
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (102, NULL, N'South Humberside', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (103, 102, N's Humbs', 1);
END;

IF NOT EXISTS (SELECT * FROM [dbo].[LocationAlias] WHERE [LocationAliasId] = 104)
BEGIN
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (104, 77, N'Tyne & Wear', 1);
END;

IF NOT EXISTS (SELECT * FROM [dbo].[LocationAlias] WHERE [LocationAliasId] = 105)
BEGIN
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (105, 78, N'Warks', 1);
END;

IF NOT EXISTS (SELECT * FROM [dbo].[LocationAlias] WHERE [LocationAliasId] = 106)
BEGIN
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (106, 36, N'Hereford & Worcester', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (107, 36, N'Herefordshire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (108, 36, N'Worcestershire', 1);
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (109, 36, N'Worcs', 1);
END;

IF NOT EXISTS (SELECT * FROM [dbo].[LocationAlias] WHERE [LocationAliasId] = 110)
BEGIN
	INSERT INTO [dbo].[LocationAlias] ([LocationAliasId], [ParentLocationAliasId], [LocationAliasName], [LocationAliasTypeId]) VALUES (110, 26, N'Glos', 1);
END;

IF NOT EXISTS (SELECT * FROM [dbo].[RoATPProviderType])
BEGIN
	INSERT INTO RoATPProviderType (RoATPProviderTypeId, Code, [Description]) VALUES (1, 'MainProvider', 'Main Provider');
	INSERT INTO RoATPProviderType (RoATPProviderTypeId, Code, [Description]) VALUES (2, 'SupportingProvider', 'Supporting Provider');
	INSERT INTO RoATPProviderType (RoATPProviderTypeId, Code, [Description]) VALUES (3, 'EmployerProvider', 'Employer Provider');
END;

IF NOT EXISTS (SELECT * FROM [dbo].[ImportBatch] WHERE ImportBatchId IN (1, 2, 3, 4, 5))
BEGIN
	SET IDENTITY_INSERT [dbo].[ImportBatch] ON;

	INSERT INTO [dbo].[ImportBatch] (ImportBatchId, ImportBatchName, [Current]) VALUES (1, '--- Default Batch ---', 0);
	INSERT INTO [dbo].[ImportBatch] (ImportBatchId, ImportBatchName, [Current]) VALUES (2, 'Beta', 0);
	INSERT INTO [dbo].[ImportBatch] (ImportBatchId, ImportBatchName, [Current]) VALUES (3, 'Batch 1', 0);
	INSERT INTO [dbo].[ImportBatch] (ImportBatchId, ImportBatchName, [Current]) VALUES (4, 'Batch 2', 1);
	INSERT INTO [dbo].[ImportBatch] (ImportBatchId, ImportBatchName, [Current]) VALUES (5, 'Batch 3', 0);

	SET IDENTITY_INSERT [dbo].[ImportBatch] OFF;
END;

IF NOT EXISTS (SELECT * FROM [Search].[SearchPhrase])
BEGIN

	INSERT INTO [Search].[SearchPhrase] (Phrase, CreatedByUserId, Ordinal, RecordStatusId) VALUES ('GCSE', '24314672-f766-47f1-98cb-ad9fc49f6e9d', 1, 2);
	INSERT INTO [Search].[SearchPhrase_QualificationLevel] (SearchPhraseId, QualificationLevelId) VALUES (@@IDENTITY, 2);

	INSERT INTO [Search].[SearchPhrase] (Phrase, CreatedByUserId, Ordinal, RecordStatusId) VALUES ('GCE', '24314672-f766-47f1-98cb-ad9fc49f6e9d', 1, 2);
	INSERT INTO [Search].[SearchPhrase_QualificationLevel] (SearchPhraseId, QualificationLevelId) VALUES (@@IDENTITY, 3);

	INSERT INTO [Search].[SearchPhrase] (Phrase, CreatedByUserId, Ordinal, RecordStatusId) VALUES ('A Level', '24314672-f766-47f1-98cb-ad9fc49f6e9d', 1, 2);
	INSERT INTO [Search].[SearchPhrase_QualificationLevel] (SearchPhraseId, QualificationLevelId) VALUES (@@IDENTITY, 3);

	INSERT INTO [Search].[SearchPhrase] (Phrase, CreatedByUserId, Ordinal, RecordStatusId) VALUES ('A-Level', '24314672-f766-47f1-98cb-ad9fc49f6e9d', 1, 2);
	INSERT INTO [Search].[SearchPhrase_QualificationLevel] (SearchPhraseId, QualificationLevelId) VALUES (@@IDENTITY, 3);

	INSERT INTO [Search].[SearchPhrase] (Phrase, CreatedByUserId, Ordinal, RecordStatusId) VALUES ('Level 1', '24314672-f766-47f1-98cb-ad9fc49f6e9d', 1, 2);
	INSERT INTO [Search].[SearchPhrase_QualificationLevel] (SearchPhraseId, QualificationLevelId) VALUES (@@IDENTITY, 1);

	INSERT INTO [Search].[SearchPhrase] (Phrase, CreatedByUserId, Ordinal, RecordStatusId) VALUES ('Level 2', '24314672-f766-47f1-98cb-ad9fc49f6e9d', 1, 2);
	INSERT INTO [Search].[SearchPhrase_QualificationLevel] (SearchPhraseId, QualificationLevelId) VALUES (@@IDENTITY, 2);

	INSERT INTO [Search].[SearchPhrase] (Phrase, CreatedByUserId, Ordinal, RecordStatusId) VALUES ('Level 3', '24314672-f766-47f1-98cb-ad9fc49f6e9d', 1, 2);
	INSERT INTO [Search].[SearchPhrase_QualificationLevel] (SearchPhraseId, QualificationLevelId) VALUES (@@IDENTITY, 3);

	INSERT INTO [Search].[SearchPhrase] (Phrase, CreatedByUserId, Ordinal, RecordStatusId) VALUES ('Level 4', '24314672-f766-47f1-98cb-ad9fc49f6e9d', 1, 2);
	INSERT INTO [Search].[SearchPhrase_QualificationLevel] (SearchPhraseId, QualificationLevelId) VALUES (@@IDENTITY, 4);

	INSERT INTO [Search].[SearchPhrase] (Phrase, CreatedByUserId, Ordinal, RecordStatusId) VALUES ('Level 5', '24314672-f766-47f1-98cb-ad9fc49f6e9d', 1, 2);
	INSERT INTO [Search].[SearchPhrase_QualificationLevel] (SearchPhraseId, QualificationLevelId) VALUES (@@IDENTITY, 5);

	INSERT INTO [Search].[SearchPhrase] (Phrase, CreatedByUserId, Ordinal, RecordStatusId) VALUES ('Level 6', '24314672-f766-47f1-98cb-ad9fc49f6e9d', 1, 2);
	INSERT INTO [Search].[SearchPhrase_QualificationLevel] (SearchPhraseId, QualificationLevelId) VALUES (@@IDENTITY, 6);

	INSERT INTO [Search].[SearchPhrase] (Phrase, CreatedByUserId, Ordinal, RecordStatusId) VALUES ('Level 7', '24314672-f766-47f1-98cb-ad9fc49f6e9d', 1, 2);
	INSERT INTO [Search].[SearchPhrase_QualificationLevel] (SearchPhraseId, QualificationLevelId) VALUES (@@IDENTITY, 7);

	INSERT INTO [Search].[SearchPhrase] (Phrase, CreatedByUserId, Ordinal, RecordStatusId) VALUES ('Level 8', '24314672-f766-47f1-98cb-ad9fc49f6e9d', 1, 2);
	INSERT INTO [Search].[SearchPhrase_QualificationLevel] (SearchPhraseId, QualificationLevelId) VALUES (@@IDENTITY, 8);

END;