using System;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class LearnDirectClassificationExtensions
    {
        public static String GetDescription(this LearnDirectClassification classification)
        {
            return classification.LearnDirectClassSystemCodeDesc + " (" + classification.LearnDirectClassificationRef + ")";
        }
    }
}