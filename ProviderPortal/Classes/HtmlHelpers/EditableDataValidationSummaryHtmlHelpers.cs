using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    // Based on System.Web.Mvc.Html.ValidationExtensions
    public static class EditableDataValidationSummaryHtmlHelpers
    {
        /// <summary>
        ///     Returns an unordered list (ul element) of validation messages that are in the
        ///     <see cref="T:System.Web.Mvc.ModelStateDictionary" /> object.
        /// </summary>
        /// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper)
        {
            return htmlHelper.EditableDataValidationSummary(false);
        }

        /// <summary>
        ///     Returns an unordered list (ul element) of validation messages that are in the
        ///     <see cref="T:System.Web.Mvc.ModelStateDictionary" /> object and optionally displays only model-level errors.
        /// </summary>
        /// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="excludePropertyErrors">
        ///     true to have the summary display model-level errors only, or false to have the
        ///     summary display all errors.
        /// </param>
        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, bool excludePropertyErrors)
        {
            return htmlHelper.EditableDataValidationSummary(excludePropertyErrors, null);
        }

        /// <summary>
        ///     Returns an unordered list (ul element) of validation messages that are in the
        ///     <see cref="T:System.Web.Mvc.ModelStateDictionary" /> object.
        /// </summary>
        /// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
        /// <param name="htmlHelper">The HMTL helper instance that this method extends.</param>
        /// <param name="message">The message to display if the specified field contains an error.</param>
        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, string message)
        {
            return htmlHelper.EditableDataValidationSummary(false, message, (object) null, null);
        }

        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, string message,
            string headingTag)
        {
            return htmlHelper.EditableDataValidationSummary(false, message, (object) null, headingTag);
        }

        /// <summary>
        ///     Returns an unordered list (ul element) of validation messages that are in the
        ///     <see cref="T:System.Web.Mvc.ModelStateDictionary" /> object and optionally displays only model-level errors.
        /// </summary>
        /// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="excludePropertyErrors">
        ///     true to have the summary display model-level errors only, or false to have the
        ///     summary display all errors.
        /// </param>
        /// <param name="message">The message to display with the validation summary.</param>
        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, bool excludePropertyErrors,
            string message)
        {
            return htmlHelper.EditableDataValidationSummary(excludePropertyErrors, message, (object) null, null);
        }

        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, bool excludePropertyErrors,
            string message, string headingTag)
        {
            return htmlHelper.EditableDataValidationSummary(excludePropertyErrors, message, (object) null, headingTag);
        }

        /// <summary>
        ///     Returns an unordered list (ul element) of validation messages in the
        ///     <see cref="T:System.Web.Mvc.ModelStateDictionary" /> object.
        /// </summary>
        /// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="message">The message to display if the specified field contains an error.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes for the element. </param>
        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, string message,
            object htmlAttributes)
        {
            return EditableDataValidationSummary(htmlHelper, false, message,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), null);
        }

        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, string message,
            object htmlAttributes,
            string headingTag)
        {
            return EditableDataValidationSummary(htmlHelper, false, message,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), headingTag);
        }

        /// <summary>
        ///     Returns an unordered list (ul element) of validation messages that are in the
        ///     <see cref="T:System.Web.Mvc.ModelStateDictionary" /> object and optionally displays only model-level errors.
        /// </summary>
        /// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="excludePropertyErrors">
        ///     true to have the summary display model-level errors only, or false to have the
        ///     summary display all errors.
        /// </param>
        /// <param name="message">The message to display with the validation summary.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes for the element.</param>
        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, bool excludePropertyErrors,
            string message, object htmlAttributes)
        {
            return EditableDataValidationSummary(htmlHelper, excludePropertyErrors, message,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), null);
        }

        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, bool excludePropertyErrors,
            string message, object htmlAttributes, string headingTag)
        {
            return EditableDataValidationSummary(htmlHelper, excludePropertyErrors, message,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), headingTag);
        }

        /// <summary>
        ///     Returns an unordered list (ul element) of validation messages that are in the
        ///     <see cref="T:System.Web.Mvc.ModelStateDictionary" /> object.
        /// </summary>
        /// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="message">The message to display if the specified field contains an error.</param>
        /// <param name="htmlAttributes">A dictionary that contains the HTML attributes for the element.</param>
        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, string message,
            IDictionary<string, object> htmlAttributes)
        {
            return EditableDataValidationSummary(htmlHelper, false, message, htmlAttributes, null);
        }

        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, string message,
            IDictionary<string, object> htmlAttributes, string headingTag)
        {
            return EditableDataValidationSummary(htmlHelper, false, message, htmlAttributes, headingTag);
        }

        /// <summary>
        ///     Returns an unordered list (ul element) of validation messages that are in the
        ///     <see cref="T:System.Web.Mvc.ModelStateDictionary" /> object and optionally displays only model-level errors.
        /// </summary>
        /// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="excludePropertyErrors">
        ///     true to have the summary display model-level errors only, or false to have the
        ///     summary display all errors.
        /// </param>
        /// <param name="message">The message to display with the validation summary.</param>
        /// <param name="htmlAttributes">A dictionary that contains the HTML attributes for the element.</param>
        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, bool excludePropertyErrors,
            string message, IDictionary<string, object> htmlAttributes)
        {
            return EditableDataValidationSummary(htmlHelper, excludePropertyErrors, message, htmlAttributes, null);
        }

        public static MvcHtmlString EditableDataValidationSummary(this HtmlHelper htmlHelper, bool excludePropertyErrors,
            string message, IDictionary<string, object> htmlAttributes, string headingTag)
        {
            var tag = new TagBuilder("div");
            tag.AddCssClass("validation-summary-valid");
            tag.AddCssClass("alert");
            tag.AddCssClass("alert-danger");
            tag.AddCssClass("alert-dismissible");
            tag.Attributes["role"] = "alert";
            tag.InnerHtml = "<span class=\"glyphicon glyphicon-exclamation-sign\" aria-hidden=\"true\"></span>"
                            + "<span class=\"sr-only\">Error:</span> "
                            + "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>"
                            + AppGlobal.Language.GetText("Validation_Error_DataNotSaved",
                                "Your changes were not saved, please correct the errors below and try again.");
            var modelStateList = GetModelStateList(htmlHelper, excludePropertyErrors);
            if (!modelStateList.Any() || modelStateList.All(x => x.Errors.Count() == 0))
            {
                tag.Attributes["style"] = "display:none";
            }
            var ret1 = tag.ToMvcHtmlString(TagRenderMode.Normal);
            var ret2 = htmlHelper.ValidationSummary(excludePropertyErrors, message, htmlAttributes,
                headingTag);
            return Concat(ret1, ret2);
        }

        private static MvcHtmlString Concat(params MvcHtmlString[] items)
        {
            var sb = new StringBuilder();
            foreach (var item in items.Where(i => i != null))
                sb.Append(item.ToHtmlString());
            return MvcHtmlString.Create(sb.ToString());
        }


        // From System.Web.Mvc.Html.ValidationExtensions
        private static IEnumerable<ModelState> GetModelStateList(HtmlHelper htmlHelper, bool excludePropertyErrors)
        {
            if (excludePropertyErrors)
            {
                ModelState modelState;
                htmlHelper.ViewData.ModelState.TryGetValue(htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix,
                    out modelState);
                if (modelState == null)
                    return (IEnumerable<ModelState>) new ModelState[0];
                return (IEnumerable<ModelState>) new ModelState[1]
                {
                    modelState
                };
            }
            Dictionary<string, int> ordering = new Dictionary<string, int>();
            ModelMetadata modelMetadata = htmlHelper.ViewData.ModelMetadata;
            if (modelMetadata != null)
            {
                foreach (ModelMetadata property in modelMetadata.Properties)
                    ordering[property.PropertyName] = property.Order;
            }
            return
                htmlHelper.ViewData.ModelState.Select(kv => new {kv = kv, name = kv.Key})
                    .OrderBy(param0 => ordering.GetOrDefault<string, int>(param0.name, 10000))
                    .Select(param0 => param0.kv.Value);
        }

        // From System.Web.Mvc.DictionaryHelpers
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue @default)
        {
            TValue obj;
            if (dict.TryGetValue(key, out obj))
                return obj;
            return @default;
        }
    }
}