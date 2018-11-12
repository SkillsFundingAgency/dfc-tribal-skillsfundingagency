using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public static class TooltipForHtmlHelpers
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString TooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            return TooltipFor<TModel, TValue>(html, expression, labelText: null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString TooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string labelText)
        {
            return TooltipFor(html, expression, labelText, htmlAttributes: null, metadataProvider: null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString TooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return TooltipFor(html, expression, labelText: null, htmlAttributes: htmlAttributes,
                metadataProvider: null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString TooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            return TooltipFor(html, expression, labelText: null, htmlAttributes: htmlAttributes,
                metadataProvider: null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString TooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes)
        {
            return TooltipFor(html, expression, labelText, htmlAttributes, metadataProvider: null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString TooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string labelText, IDictionary<string, object> htmlAttributes)
        {
            return TooltipFor(html, expression, labelText, htmlAttributes, metadataProvider: null);
        }

        internal static MvcHtmlString TooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes,
            ModelMetadataProvider metadataProvider)
        {
            return TooltipFor(html,
                expression,
                labelText,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes),
                metadataProvider);
        }

        internal static MvcHtmlString TooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string labelText, IDictionary<string, object> htmlAttributes,
            ModelMetadataProvider metadataProvider)
        {
            return TooltipForHelper(html,
                ModelMetadata.FromLambdaExpression(expression, html.ViewData /*, metadataProvider*/),
                ExpressionHelper.GetExpressionText(expression),
                labelText,
                htmlAttributes);
        }

        public static MvcHtmlString TooltipForModel(this HtmlHelper html)
        {
            return TooltipForModel(html, labelText: null);
        }

        public static MvcHtmlString TooltipForModel(this HtmlHelper html, string labelText)
        {
            return TooltipForHelper(html, html.ViewData.ModelMetadata, String.Empty, labelText);
        }

        public static MvcHtmlString TooltipForModel(this HtmlHelper html, object htmlAttributes)
        {
            return TooltipForHelper(html, html.ViewData.ModelMetadata, String.Empty, labelText: null,
                htmlAttributes: HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString TooltipForModel(this HtmlHelper html,
            IDictionary<string, object> htmlAttributes)
        {
            return TooltipForHelper(html, html.ViewData.ModelMetadata, String.Empty, labelText: null,
                htmlAttributes: htmlAttributes);
        }

        public static MvcHtmlString TooltipForModel(this HtmlHelper html, string labelText, object htmlAttributes)
        {
            return TooltipForHelper(html, html.ViewData.ModelMetadata, String.Empty, labelText,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString TooltipForModel(this HtmlHelper html, string labelText,
            IDictionary<string, object> htmlAttributes)
        {
            return TooltipForHelper(html, html.ViewData.ModelMetadata, String.Empty, labelText, htmlAttributes);
        }

        internal static MvcHtmlString TooltipForHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName,
            string labelText = null, IDictionary<string, object> htmlAttributes = null)
        {
            string descriptionField = string.Concat(metadata.ContainerType.Name, "_Description_", metadata.PropertyName);
            string resolvedDescription = AppGlobal.Language.GetText(descriptionField, metadata.Description ?? String.Empty);
            
            TagBuilder tag = null;
            if (resolvedDescription != String.Empty)
            {
                tag = new TagBuilder("span");
                tag.Attributes.Add("title", metadata.Description ?? String.Empty);
                tag.Attributes.Add("tabindex", "0");
                tag.MergeAttributes(htmlAttributes, replaceExisting: true);
                tag.AddCssClass("glyphicon");
                tag.AddCssClass("glyphicon-question-sign");
            }

            return tag.ToMvcHtmlString(TagRenderMode.Normal);
        }
    }
}