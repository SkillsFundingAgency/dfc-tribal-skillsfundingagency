using System.Collections.Generic;
using System.Web.Mvc;

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public static class RequiredFieldHtmlHelpers
    {
        public static MvcHtmlString RequiredFields(this HtmlHelper html)
        {
            return RequiredFieldsHelper(html, null);
        }

        public static MvcHtmlString RequiredFields(this HtmlHelper html, object htmlAttributes)
        {
            return RequiredFieldsHelper(html, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        internal static MvcHtmlString RequiredFieldsHelper(HtmlHelper html, IDictionary<string, object> htmlAttributes)
        {
            const string defaultText = "Fields marked <span class=\"required\">*</span> are required.";
            TagBuilder tag = new TagBuilder("p");
            tag.MergeAttributes(htmlAttributes, replaceExisting: true);
            tag.AddCssClass("required-message");
            tag.InnerHtml = AppGlobal.Language.GetText("Global_Forms_RequiredMessage", defaultText);
            return tag.ToMvcHtmlString(TagRenderMode.Normal);
        }
    }
}