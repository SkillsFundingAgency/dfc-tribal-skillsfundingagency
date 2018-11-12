using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Classes.Content
{
    public class ContentPathAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is string && ContentManager.IsPathValid((string)value);
        }
    }
}