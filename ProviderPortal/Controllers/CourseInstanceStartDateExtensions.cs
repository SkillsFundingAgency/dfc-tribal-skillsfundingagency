using System;

using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public static class CourseInstanceStartDateExtensions
    {
        public static String ToFormattedString(this CourseInstanceStartDate sd)
        {
            return sd.StartDate.ToString(sd.IsMonthOnlyStartDate ? OpportunityController.StartMonthFormat : Constants.ConfigSettings.ShortDateFormat);
        }
    }
}