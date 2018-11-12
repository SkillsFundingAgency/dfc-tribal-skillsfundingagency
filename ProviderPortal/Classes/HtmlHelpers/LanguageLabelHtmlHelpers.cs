using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    // Based on LabelFor
    // Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
    public static class LanguageLabelHtmlHelpers
    {
        public static MvcHtmlString LanguageLabel(this HtmlHelper html, string expression)
        {
            return LanguageLabel(html,
                expression,
                labelText: null);
        }

        public static MvcHtmlString LanguageLabel(this HtmlHelper html, string expression, string labelText)
        {
            return LanguageLabel(html, expression, labelText, htmlAttributes: null, metadataProvider: null);
        }

        public static MvcHtmlString LanguageLabel(this HtmlHelper html, string expression, object htmlAttributes)
        {
            return LanguageLabel(html, expression, labelText: null, htmlAttributes: htmlAttributes,
                metadataProvider: null);
        }

        public static MvcHtmlString LanguageLabel(this HtmlHelper html, string expression,
            IDictionary<string, object> htmlAttributes)
        {
            return LanguageLabel(html, expression, labelText: null, htmlAttributes: htmlAttributes,
                metadataProvider: null);
        }

        public static MvcHtmlString LanguageLabel(this HtmlHelper html, string expression, string labelText,
            object htmlAttributes)
        {
            return LanguageLabel(html, expression, labelText, htmlAttributes, metadataProvider: null);
        }

        public static MvcHtmlString LanguageLabel(this HtmlHelper html, string expression, string labelText,
            IDictionary<string, object> htmlAttributes)
        {
            return LanguageLabel(html, expression, labelText, htmlAttributes, metadataProvider: null);
        }

        internal static MvcHtmlString LanguageLabel(this HtmlHelper html, string expression, string labelText,
            object htmlAttributes, ModelMetadataProvider metadataProvider)
        {
            return LanguageLabel(html,
                expression,
                labelText,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes),
                metadataProvider);
        }

        internal static MvcHtmlString LanguageLabel(this HtmlHelper html, string expression, string labelText,
            IDictionary<string, object> htmlAttributes, ModelMetadataProvider metadataProvider)
        {
            return LanguageLabelHelper(html,
                ModelMetadata.FromStringExpression(expression, html.ViewData /*, metadataProvider*/),
                expression,
                labelText,
                htmlAttributes);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString LanguageLabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            return LanguageLabelFor<TModel, TValue>(html, expression, labelText: null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString LanguageLabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string labelText)
        {
            return LanguageLabelFor(html, expression, labelText, htmlAttributes: null, metadataProvider: null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString LanguageLabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return LanguageLabelFor(html, expression, labelText: null, htmlAttributes: htmlAttributes,
                metadataProvider: null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString LanguageLabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            return LanguageLabelFor(html, expression, labelText: null, htmlAttributes: htmlAttributes,
                metadataProvider: null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString LanguageLabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes)
        {
            return LanguageLabelFor(html, expression, labelText, htmlAttributes, metadataProvider: null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString LanguageLabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string labelText, IDictionary<string, object> htmlAttributes)
        {
            return LanguageLabelFor(html, expression, labelText, htmlAttributes, metadataProvider: null);
        }

        internal static MvcHtmlString LanguageLabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes,
            ModelMetadataProvider metadataProvider)
        {
            return LanguageLabelFor(html,
                expression,
                labelText,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes),
                metadataProvider);
        }

        internal static MvcHtmlString LanguageLabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string labelText, IDictionary<string, object> htmlAttributes,
            ModelMetadataProvider metadataProvider)
        {
            return LanguageLabelHelper(html,
                ModelMetadata.FromLambdaExpression(expression, html.ViewData /*, metadataProvider*/),
                ExpressionHelper.GetExpressionText(expression),
                labelText,
                htmlAttributes);
        }

        public static MvcHtmlString LangaugeLabelForModel(this HtmlHelper html)
        {
            return LangaugeLabelForModel(html, labelText: null);
        }

        public static MvcHtmlString LangaugeLabelForModel(this HtmlHelper html, string labelText)
        {
            return LanguageLabelHelper(html, html.ViewData.ModelMetadata, String.Empty, labelText);
        }

        public static MvcHtmlString LangaugeLabelForModel(this HtmlHelper html, object htmlAttributes)
        {
            return LanguageLabelHelper(html, html.ViewData.ModelMetadata, String.Empty, labelText: null,
                htmlAttributes: HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString LangaugeLabelForModel(this HtmlHelper html,
            IDictionary<string, object> htmlAttributes)
        {
            return LanguageLabelHelper(html, html.ViewData.ModelMetadata, String.Empty, labelText: null,
                htmlAttributes: htmlAttributes);
        }

        public static MvcHtmlString LangaugeLabelForModel(this HtmlHelper html, string labelText, object htmlAttributes)
        {
            return LanguageLabelHelper(html, html.ViewData.ModelMetadata, String.Empty, labelText,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString LangaugeLabelForModel(this HtmlHelper html, string labelText,
            IDictionary<string, object> htmlAttributes)
        {
            return LanguageLabelHelper(html, html.ViewData.ModelMetadata, String.Empty, labelText, htmlAttributes);
        }

        internal static MvcHtmlString LanguageLabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName,
            string labelText = null, IDictionary<string, object> htmlAttributes = null)
        {
            string resolvedLabelText = labelText ??
                                       metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            if (String.IsNullOrEmpty(resolvedLabelText))
            {
                return MvcHtmlString.Empty;
            }

            TagBuilder label = new TagBuilder("label");
            label.Attributes.Add("for",
                TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            //label.MergeAttributes(htmlAttributes, replaceExisting: true);
            label.InnerHtml = resolvedLabelText;
            if (metadata.IsRequired && metadata.ModelType.FullName != "System.Boolean")
            {
                label.InnerHtml += " <span class=\"required\">*</span>";
            }

            string descriptionField = string.Concat(metadata.ContainerType.Name, "_Description_", metadata.PropertyName);
            string resolvedDescription = AppGlobal.Language.GetText(descriptionField, metadata.Description ?? String.Empty);
            TagBuilder hint = null;
            if (resolvedDescription != String.Empty)
            {
                hint = new TagBuilder("span");
                hint.Attributes.Add("title", resolvedDescription ?? metadata.Description ?? String.Empty);
                hint.Attributes.Add("tabindex", "0");
                //hint.MergeAttributes(htmlAttributes, replaceExisting: true);
                hint.AddCssClass("glyphicon");
                hint.AddCssClass("glyphicon-question-sign");
            }

            TagBuilder tag = new TagBuilder("div");
            tag.MergeAttributes(htmlAttributes, replaceExisting: true);
            tag.AddCssClass("hinted-control-label");
            tag.InnerHtml = resolvedDescription == String.Empty
                ? label.ToString(TagRenderMode.Normal)
                : String.Concat(label.ToString(TagRenderMode.Normal), hint.ToString(TagRenderMode.Normal));

            
            return tag.ToMvcHtmlString(TagRenderMode.Normal);
        }
    }
}