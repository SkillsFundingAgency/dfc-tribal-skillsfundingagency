using System.Diagnostics;
using System.Text;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode renderMode)
        {
            Debug.Assert(tagBuilder != null);
            return new MvcHtmlString(tagBuilder.ToString(renderMode));
        }
    }
}