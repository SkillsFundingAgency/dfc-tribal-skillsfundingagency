CREATE PROCEDURE [dbo].[API_LearningAim_GetById]
	@LearningAimReference	nvarchar(10)
 AS

 BEGIN

	SELECT [LearningAimId],
		[LearningAimRef],
		[AccreditationStartDate],
		[AccreditationEndDate],
		[CertificationEndDate],
		[ErAppStatus],
		[ErTtgStatus],
		[IndepLivingSkills],
		[Level2EntitlementCatDesc],
		[Level3EntitlementCatDesc],
		[OtherfundingNonFundedStatus],	
		[QcaGlh],	
		[SectorLeadBodyDesc],
		[SkillsForLife],
		[SkillsForLifeType_Desc],	
		[AdultlrStatus] 	
	FROM [dbo].[LearningAim]
	WHERE [LearningAimRef]=@LearningAimReference;

END;
