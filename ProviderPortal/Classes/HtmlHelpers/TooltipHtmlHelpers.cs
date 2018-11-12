using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public static class TooltipHtmlHelpers
    {
        public static MvcHtmlString Tooltip(this HtmlHelper html, string tooltipText)
        {
            return TooltipHelper(html, tooltipText, null);
        }

        public static MvcHtmlString Tooltip(this HtmlHelper html, string tooltipText, object htmlAttributes)
        {
            return TooltipHelper(html, tooltipText, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        internal static MvcHtmlString TooltipHelper(HtmlHelper html, string tooltipText, IDictionary<string, object> htmlAttributes)
        {
            var tag = new TagBuilder("span");
            tag.Attributes.Add("title", tooltipText ?? String.Empty);
            tag.Attributes.Add("tabindex", "0");
            tag.MergeAttributes(htmlAttributes, replaceExisting: true);
            tag.AddCssClass("glyphicon");
            tag.AddCssClass("glyphicon-question-sign");

            return tag.ToMvcHtmlString(TagRenderMode.Normal);
        }
    }
}