CREATE FUNCTION [dbo].[GetProviderIsTASOnly]
(
	@ProviderId int,
	@RoATPFFlag bit
)
RETURNS BIT
AS
BEGIN
	--Provider is TAS Only if the RoATPFFlag is set and they have no courses
	Declare @IsTASOnly bit = (
		CASE 
			WHEN @RoATPFFlag = 0 THEN 0
		ELSE 
			(select case when count(CourseId) > 0 then 0 else 1 end  from Course where ProviderId = @ProviderId)
		END)
	
	Return @IsTASOnly;
END
