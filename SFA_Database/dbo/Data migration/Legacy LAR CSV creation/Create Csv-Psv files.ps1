$savepath = "c:\users\leigh.carpenter\desktop\CsvOutput"
$server = "localhost"
$database = "Lars_November"


if ( (Get-PSSnapin -Name SqlServerCmdletSnapin100 -ErrorAction SilentlyContinue) -eq $null )
{
   Add-PSSnapin SqlServerCmdletSnapin100
  Add-PSSnapin SqlServerProviderSnapin100
}

$qry="select BasicSkills, BasicSkillsDesc, BasicSkillsDesc2
FROM CoreReference_LARS_BasicSkills_Lookup"
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_BasicSkills.psv -Delimiter "|" -Encoding UTF8 | select -skip 1  
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_BasicSkills.csv -Delimiter "," -Encoding UTF8 | select -skip 1 

$qry="select BasicSkillsType, BasicSkillsTypeDesc, BasicSkillsTypeDesc2,
convert(nvarchar(MAX), EffectiveFrom, 23) AS EffectiveFrom, convert(nvarchar(MAX), EffectiveTo, 23) AS EffectiveTo
FROM CoreReference_LARS_BasicSkillsType_Lookup"
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_BasicSkillsType.psv -Delimiter "|" -Encoding UTF8 | select -skip 1 
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_BasicSkillsType.csv -Delimiter "," -Encoding UTF8 | select -skip 1  

$qry="SELECT CreditBasedFwkType, CreditBasedFwkTypeDesc, CreditBasedFwkTypeDesc2
FROM CoreReference_LARS_CreditBasedFwkType_Lookup"
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_CreditBasedFwkType.psv -delimiter "|" -Encoding UTF8 | select -skip 1  
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_CreditBasedFwkType.csv -delimiter "," -Encoding UTF8 | select -skip 1 

$qry="SELECT FullLevel2EntitlementCategory AS FullLevel2EntitCat,
FullLevel2EntitlementCategoryDesc AS FullLevel2EntitCatDesc,
FullLevel2EntitlementCategoryDesc2 AS FullLevel2EntitCatDesc2,
convert(nvarchar(MAX), EffectiveFrom, 23) AS EffectiveFrom, convert(nvarchar(MAX), EffectiveTo, 23) AS EffectiveTo
from CoreReference_LARS_FullLevel2EntitlementCategory_Lookup"
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_FullLevel2EntitCat.psv -delimiter "|" -Encoding UTF8 | select -skip 1  
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_FullLevel2EntitCat.csv -delimiter "," -Encoding UTF8 | select -skip 1  

$qry="SELECT FullLevel3EntitlementCategory AS FullLevel3EntitCat,
FullLevel3EntitlementCategoryDesc AS FullLevel3EntitCatDesc,
FullLevel3EntitlementCategoryDesc2 AS FullLevel3EntitCatDesc2,
convert(nvarchar(MAX), EffectiveFrom, 23) AS EffectiveFrom, convert(nvarchar(MAX), EffectiveTo, 23) AS EffectiveTo
from CoreReference_LARS_FullLevel3EntitlementCategory_Lookup"
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_FullLevel3EntitCat.psv -delimiter "|" -Encoding UTF8 | select -skip 1  
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_FullLevel3EntitCat.csv -delimiter "," -Encoding UTF8 | select -skip 1

$qry="SELECT LearnAimRefType AS LrnAimRefType,
LearnAimRefTypeDesc AS LrnAimRefTypeDesc,
LearnAimRefTypeDesc2 AS LrnAimRefTypeDesc2,
convert(nvarchar(MAX), EffectiveFrom, 23) AS EffectiveFrom, convert(nvarchar(MAX), EffectiveTo, 23) AS EffectiveTo
FROM CoreReference_LARS_LearnAimRefType_Lookup"
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_LrnAimRefType.psv -delimiter "|" -Encoding UTF8 | select -skip 1  
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_LrnAimRefType.csv -delimiter "," -Encoding UTF8 | select -skip 1 

$qry="SELECT MI_NotionalNVQLevelv2 AS NotionalNVQLevel_V2,
MI_NotionalNVQLevelv2Desc AS NotionalNvqLevel_v2Desc,
MI_NotionalNVQLevelv2Desc2 AS NotionalNvqLevel_v2Desc2,
convert(nvarchar(MAX), EffectiveFrom, 23) AS EffectiveFrom, convert(nvarchar(MAX), EffectiveTo, 23) AS EffectiveTo
FROM CoreReference_LARS_MI_NotionalNVQLevelv2_Lookup"
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_NotionalNVQLevel_v2.psv -delimiter "|" -Encoding UTF8 | select -skip 1 
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_NotionalNVQLevel_v2.csv -delimiter "," -Encoding UTF8 | select -skip 1 

$qry="SELECT Section96ApprovalStatus AS Sec96ApprovalStatus,
Section96ApprovalStatusDesc AS Sec96ApprovalStatusDesc,
Section96ApprovalStatusDesc2 AS Sec96ApprovalStatusDesc2,
convert(nvarchar(MAX), EffectiveFrom, 23) AS EffectiveFrom, convert(nvarchar(MAX), EffectiveTo, 23) AS EffectiveTo
FROM CoreReference_LARS_Section96ApprovalStatus_Lookup"
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_Sec96ApprovalStatus.psv -delimiter "|" -Encoding UTF8 | select -skip 1 
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_Sec96ApprovalStatus.csv -delimiter "," -Encoding UTF8 | select -skip 1  

$qry="SELECT SectorSubjectAreaTier1 AS SecSubjAreaTier1,
SectorSubjectAreaTier1Desc AS SecSubjAreaTier1Desc,
SectorSubjectAreaTier1Desc2 AS SecSubjAreaTier1Desc2,
null as SecSubjAreaTier1Code, -- Appears not to be used in the import and missing
null as SecSubjAreaTier1Code2 -- Appears not to be used in the import and missing
FROM CoreReference_LARS_SectorSubjectAreaTier1_Lookup"
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_SecSubjAreaTier1.psv -delimiter "|" -Encoding UTF8 | select -skip 1  
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_SecSubjAreaTier1.csv -delimiter "," -Encoding UTF8 | select -skip 1 

$qry="
SELECT SectorSubjectAreaTier2 AS SecSubjAreaTier2,
SectorSubjectAreaTier2Desc AS SecSubjAreaTier2Desc,
SectorSubjectAreaTier2Desc2 AS SecSubjAreaTier2Desc2,
null as SecSubjAreaTier2Code, -- Appears not to be used in the import and missing
null as SecSubjAreaTier2Code2 -- Appears not to be used in the import and missing
FROM CoreReference_LARS_SectorSubjectAreaTier2_Lookup"
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_SecSubjAreaTier2.psv -delimiter "|" -Encoding UTF8 | select -skip 1  
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_SecSubjAreaTier2.csv -delimiter "," -Encoding UTF8 | select -skip 1  

$qry = "
SELECT SFAApprovalStatus AS SFA_ApprovalStatus, 
SFAApprovalStatusDesc AS SFA_ApprovalStatusDesc,
SFAApprovalStatusDesc2 AS SFA_ApprovalStatusDesc2,
convert(nvarchar(MAX), EffectiveFrom, 23) AS EffectiveFrom, convert(nvarchar(MAX), EffectiveTo, 23) AS EffectiveTo
FROM CoreReference_LARS_SFAApprovalStatus_Lookup"
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_SFA_ApprovalStatus.psv -delimiter "|" -Encoding UTF8 | select -skip 1  
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_SFA_ApprovalStatus.csv -delimiter "," -Encoding UTF8 | select -skip 1  

$qry="SELECT EntrySubLevel AS SubEntryLevel,
EntrySubLevelDesc AS SubEntryLevelDesc,
EntrySubLevelDesc2 AS SubEntryLevelDesc2,
convert(nvarchar(MAX), EffectiveFrom, 23) AS EffectiveFrom, convert(nvarchar(MAX), EffectiveTo, 23) AS EffectiveTo
FROM CoreReference_LARS_EntrySubLevel_Lookup"
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_SubEntryLevel.psv -delimiter "|" -Encoding UTF8 | select -skip 1 
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qry | Export-CSV -notype -Path $savepath\LARS_SubEntryLevel.csv -delimiter "," -Encoding UTF8 | select -skip 1  






echo "Lookup CSVs and PSVs created"
echo "Please wait, now creating the main CSV and PSV file, this will take a few minutes..."
$qryStringBuilder = New-Object -TypeName System.Text.StringBuilder
$qryStringBuilder.Append("SELECT 
CRLAL.AcademicYear,					--A				
CLLD.LearnAimRef,					--B
LearnAimRefTitle,					--C
LearnAimRefType,					--D
NotionalNVQLevel,					--E
NotionalNVQLevelv2,					--F
AwardOrgAimRef,						--G
'' AS AccredStrtDte,				--H Do not think this is used, can't find in new database and PSV appears to contain nulls
'' AS AccredEndDte,					--I Do not think this is used, can't find in the new database and PSV appears to contain nulls
'' AS LrnAimRefCertDte,				--J TODO can not see what this maps to, appears unused in the import
'' AS DteLrnAimRefLstChange,		--K TODO can not see what this maps to, appears unused in the import
'' AS LrnAimRefDiscontDate,				--L Appears unused in the csv file
CASE WHEN CLAV.BasicSkills = 0 THEN 'N' ELSE 'Y' END AS BasicSkills, --M
CASE  CRLBSTL.BasicSkillsTypeDesc
WHEN 'Not Applicable' THEN 'X'
WHEN 'Unknown' THEN 'U'
ELSE CAST(CRLBSTL.BasicSkillsType AS VARCHAR(10)) END AS BasicSkillsType, --N Looks to be correct but different results to PSV file
SectorSubjectAreaTier1 AS SecSubjAreaTier1, --O
SectorSubjectAreaTier2 AS SecSubjAreaTier2, --P
CASE ISNULL(CRLFL2ECL.FullLevel2EntitlementCategoryDesc, 'Unknown')
WHEN 'Not Applicable' THEN 'X'
WHEN 'Unknown' THEN 'U'
ELSE CAST(CLAV.FullLevel2EntitlementCategory AS VARCHAR(10)) END AS FullLevel2EntitCat, --Q
CLAV.FullLevel2Percent AS FullLevel2Pct,     --R
CASE ISNULL(CRLFL3ECL.FullLevel3EntitlementCategoryDesc, 'Unknown')
WHEN 'Not Applicable' THEN 'X'
WHEN 'Unknown' THEN 'U'
ELSE CAST(CLAV.FullLevel3EntitlementCategory AS VARCHAR(10)) END AS FullLevel3EntitCat, --S
CLAV.FullLevel3Percent AS FullLevel3Pct,    --T if no percent or match left blank
LearnDirectClassSystemCode1 AS LrnDrctClassSystemCode_1, --U
LearnDirectClassSystemCode2 AS LrnDrctClassSystemCode_2, --V
LearnDirectClassSystemCode3 AS LrnDrctClassSystemCode_3, --W
EnglandFEHEStatus AS EngFEHEStatus,						--X
CASE Section96Valid16to18 WHEN 0 THEN 'N' ELSE 'Y' END AS ValidSec96_16To18,     --Y
CASE Section96Valid18plus WHEN 0 THEN 'N' ELSE 'Y' END AS Sec96Valid_18Plus,      --Z
CASE S96AS.Section96ApprovalStatusDesc
WHEN 'Unknown' THEN 'X'
WHEN 'Not Applicable' THEN 'U'
ELSE CAST(S96AS.Section96ApprovalStatus AS VARCHAR(10)) END AS Sec96ApprovalStatus, --AA
CAST(CLAV.Section96ApprovalStartDate AS DATE) AS Sec96ApprovalStartDate,			--AB LC: Looks like different dates on the spreadsheet, appears unused anyway in the import
CAST(Section96ReviewDate AS DATE) AS Sec96ApprovalEndDate,							--AC LC: Not sure if this field is correct, provides different dates, appears unused anyway in the import
CASE CLAV.Section96Valid19plus WHEN 0 THEN 'N'										 
WHEN 1 THEN 'Y'
WHEN -1 THEN 'U'
WHEN -2 THEN 'X'
END	AS Sec96Valid_19Plus,															--AD
CASE CreditBasedFwkType                                                                  
WHEN -1 THEN 'U'
WHEN -2 THEN 'X'
ELSE CAST(CreditBasedFwkType AS VARCHAR(10)) END AS CreditBasedFwkType,				--AE
RegulatedCreditValue AS RgltdCreditVal,												--AF
CASE QltyAssAgencyType 
WHEN -2 THEN 'X'
WHEN -1 THEN 'U'
ELSE CAST(QltyAssAgencyType AS VARCHAR(10)) END AS QltyAssAgencyType,				--AG
OfQualGlhMin,																		--AH
OfQualGlhMax,																		--AI
DiplomaLinesOfLearning AS LinesLrningDipSplq,										--AJ
FrameworkCommonComponent AS FwkCmnCmpn,												--AK
CASE LTRCPWithProviderUpliftFactor													
WHEN -1 THEN 'U'
WHEN 0 THEN 'N'
WHEN 1 THEN 'Y' END AS LtrcPrvnWithProvUpLftFctr,									--AL
EntrySubLevel AS SubEntryLevel,														--AM
SuccessRateMapCode AS SuccRateMapCode,												--AN
CASE AccreditiedNotDfESApproved		
WHEN -2 THEN 'X'
WHEN -1 THEN 'U'
WHEN 0 THEN 'N'
WHEN 1 THEN 'Y' END AS AccredNotDfESApprvd,											--AO
CASE AccreditiedMayPossDfESApproved		
WHEN -2 THEN 'X'
WHEN -1 THEN 'U'
WHEN 0 THEN 'N'
WHEN 1 THEN 'Y' END AS AccredMayPossDfESApprvd,									--AP	
'' AS Flag14,																	--AQ
'' AS Flag15,																	--AR
'' AS Flag16,																	--AS
'' AS Flag17,																	--AT
'' AS Flag18,																	--AU
'' AS Flag19,																	--AV
'' AS Flag20,																	--AW
'' AS Flag21,																	--AX
'' AS Flag22,																	--AY
'' AS Flag23,																	--AZ
'' AS Flag24,																	--BA
'' AS Flag25,																	--BB 
'' AS Flag26,																	--BC
'' AS Flag27,																	--BD
'' AS Flag28,																	--BE
'' AS Flag29,																	--BF
'' AS Flag30,																	--BG
'' AS LastDateForNewStart,														--BH Appears not to be used and can't be found in new file							
CASE IndependentLivingSkills														
WHEN 0 THEN 'N'
WHEN 1 THEN 'Y' END AS IndepLvgSklls,											--BI
CASE AdditionalOrSpecialistLearning
WHEN 0 THEN 'N'
WHEN 1 THEN 'Y' END AS AddSpecLrn,												--BJ
CASE WHEN EnglPrscID > 0 THEN 'Y'ELSE 'N' END AS EnglPrscId,					--BK
0 AS AwardOrgNotOperational,													--BL Appears as all zeros in lars lite, no equivalent in Lars, not used in import scripts so default to zero
'' AS SprsdLearnAimRef,															--BM Appears all blanks in lars lite, no equivalent in Lars, not used in import so default to empty
'' AS SectorSkillsCouncil,														--BN Appears all blanks in Lars lite, no equivalent in Lars, not used in import so default to empty														
'N' AS CurrPrvsnIndic,															--BO Appears as all N, lookup in new database doesn't appear to link back, default to N, appears unused when importing
'N' AS Loanable,																--BP Has values in the spreadsheet but unable to find anything similar in the new lars DB and this value appears unused in the import so default to all to N
convert(nvarchar(MAX), CLLD.EffectiveFrom, 23) AS EffectiveFrom, --BQ
convert(nvarchar(MAX), CLLD.EffectiveTo, 23) AS EffectiveTo,	 -- BR
CASE Vocational
WHEN 0 THEN 'N'
WHEN 1 THEN 'Y' END AS Vocational,												--BS
AOCL.AwardOrgCode AS AwardOrgResp,												--BT
AOCL.AwardOrgUKPRN,																--BU
AOCL.AwardOrgName,																--BV
UnitType,																		--BW		
CASE SfaApprovalStatus
WHEN -2 THEN 'X'	
WHEN -1 THEN 'U'
WHEN 0 THEN ''    															
ELSE CAST(SfaApprovalStatus AS VARCHAR(10)) END AS Sfa_ApprovalStatus			--BX
FROM Core_LARS_LearningDelivery CLLD
LEFT OUTER JOIN CoreReference_LARS_AcademicYear_Lookup CRLAL ON CLLD.EffectiveFrom >= CRLAL.StartDate AND CLLD.EffectiveFrom <= CRLAL.EndDate
LEFT OUTER JOIN Core_LARS_AnnualValue CLAV ON CLLD.LearnAimRef = CLAV.LearnAimRef AND CLAV.EffectiveTo IS NULL
LEFT OUTER JOIN CoreReference_LARS_BasicSkillsType_Lookup CRLBSTL ON CLAV.BasicSkillsType = CRLBSTL.BasicSkillsType
LEFT OUTER JOIN CoreReference_LARS_FullLevel2EntitlementCategory_Lookup CRLFL2ECL ON CLAV.FullLevel2EntitlementCategory = CRLFL2ECL.FullLevel2EntitlementCategory
LEFT OUTER JOIN CoreReference_LARS_FullLevel3EntitlementCategory_Lookup CRLFL3ECL ON CLAV.FullLevel3EntitlementCategory = CRLFL3ECL.FullLevel3EntitlementCategory
LEFT OUTER JOIN CoreReference_LARS_Section96ApprovalStatus_Lookup S96AS ON CLAV.Section96ApprovalStatus = S96AS.Section96ApprovalStatus
LEFT OUTER JOIN CoreReference_LARS_AwardOrgCode_Lookup AOCL ON CLLD.AwardOrgCode = AOCL.AwardOrgCode
")

Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qryStringBuilder.ToString() | Export-CSV -notype -Path $savepath\LARS_1314.psv -delimiter "|" -Encoding UTF8 | select -skip 1 
Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $qryStringBuilder.ToString() | Export-CSV -notype -Path $savepath\LARS_1314.csv -delimiter "," -Encoding UTF8 | select -skip 1  




































































