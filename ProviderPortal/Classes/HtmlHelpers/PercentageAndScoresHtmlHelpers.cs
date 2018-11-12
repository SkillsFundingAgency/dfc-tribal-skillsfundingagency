using System;
using System.Web;
using System.Web.Mvc;

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public static class PercentageAndScoresHtmlHelpers
    {
        /// <summary>
        /// Displays a bracketed percentage.
        /// </summary>
        /// <param name="html">The HTMLHelper.</param>
        /// <param name="items">The number of items.</param>
        /// <param name="total">The total number of items.</param>
        /// <param name="parenthesis">if set to <c>true</c> adds parenthesis.</param>
        /// <returns></returns>
        public static MvcHtmlString DashboardPercentage(this HtmlHelper html, int? items, int? total,
            bool parenthesis = true)
        {
            if (items == null || total == null || total == 0)
                return new MvcHtmlString("-");
            return new MvcHtmlString(
                parenthesis
                    ? String.Format("({0:0%})", (decimal)items.Value / total.Value)
                    : String.Format("{0:0%}", (decimal)items.Value/total.Value));
        }

        //public static MvcHtmlString DashboardRatedPercentage(this HtmlHelper html, int? items, int? total)
        //{
        //    if (items == null || total == null || total == 0)
        //        return new MvcHtmlString("-");
        //    return DashboardRatedPercentageHelper(html, (decimal)items.Value/total.Value);
        //}

        //public static MvcHtmlString DashboardRatedPercentage(this HtmlHelper html, decimal percentage)
        //{
        //    return DashboardRatedPercentageHelper(html, percentage);
        //}

        //private static MvcHtmlString DashboardRatedPercentageHelper(HtmlHelper html, decimal percentage)
        //{
        //    return new MvcHtmlString("TODO");
        //}

        /// <summary>
        /// Dashboards the quality cell.
        /// </summary>
        /// <param name="html">The HTMLHelper.</param>
        /// <param name="items">The items.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        public static MvcHtmlString DashboardQualityCell(this HtmlHelper html, decimal? items, int? total, string helpIndex = "", string courseFilter = "")
        {

            string helpLink = string.Empty;
            if (!String.IsNullOrWhiteSpace(helpIndex))
            {
                TagBuilder helpTag = new TagBuilder("a");
                helpTag.MergeAttribute("href", string.Format("/Help/DataQuality#{0}", helpIndex));
                helpTag.MergeAttribute("target", "_blank");
                helpTag.AddCssClass("text-white pull-right");
                helpTag.SetInnerText(AppGlobal.Language.GetText("Dashboard_DataQuality_CellHelp", "(help)"));
                helpLink = helpTag.ToString();
            }

            var tag = new TagBuilder("td");
            if (items == null || total == null || total == 0)
            {
                tag.SetInnerText("-");
            }
            else
            {
                var val = (decimal) items.Value/total.Value;
                string cellBackground = QualityIndicator.GetQualityBackground(val);
                tag.AddCssClass(cellBackground);
                var text = String.Format("{0:0} ({1:0.0%})", QualityIndicator.GetQualityText(val, false), val);

                if (!String.IsNullOrWhiteSpace(courseFilter))
                {
                    TagBuilder linkTag = new TagBuilder("a");
                    linkTag.MergeAttribute("href", string.Format("/Course/List?qualitySearchMode={0}", courseFilter));
                    //helpTag.AddCssClass("text-white pull-right");
                    linkTag.SetInnerText(text);
                    tag.InnerHtml = linkTag.ToString() + (cellBackground == "bg-quality-poor" ? helpLink : "");
                }
                else
                { 
                    tag.InnerHtml = text + (cellBackground == "bg-quality-poor" ? helpLink : "");
                }
            }
            return tag.ToMvcHtmlString(TagRenderMode.Normal);
        }

        /// <summary>
        /// Dashboards quality cell.
        /// </summary>
        /// <param name="html">The HTMLHelper.</param>
        /// <param name="value">The decimal value betweeb 0.0% and 100.0%.</param>
        /// <returns></returns>
        public static MvcHtmlString DashboardQualityCell(this HtmlHelper html, decimal? value, string helpIndex = "")
        {
            string helpLink = string.Empty;
            if (!String.IsNullOrWhiteSpace(helpIndex))
            {
                TagBuilder helpTag = new TagBuilder("a");
                helpTag.MergeAttribute("href", string.Format("/Help/DataQuality#{0}", helpIndex));
                helpTag.MergeAttribute("target", "_blank");
                helpTag.AddCssClass("text-white pull-right");
                helpTag.SetInnerText(AppGlobal.Language.GetText("Dashboard_DataQuality_CellHelp", "(help)"));
                helpLink = helpTag.ToString();
            }

            var tag = new TagBuilder("td");
            if (value == null)
            {
                tag.SetInnerText("-");
            }
            else
            {
                var val = (decimal) value/100;
                string cellBackground = QualityIndicator.GetQualityBackground(val);
                tag.AddCssClass(cellBackground);
                var text = String.Format("{0} ({1:##0.#%})",
                    QualityIndicator.GetQualityText(val, false), val);
                tag.InnerHtml = text + (cellBackground == "bg-quality-poor" ? helpLink : "");
            }
            return tag.ToMvcHtmlString(TagRenderMode.Normal);
        }

        /// <summary>
        /// Dashboards quality cell.
        /// </summary>
        /// <param name="html">The HTMLHelper.</param>
        /// <param name="qualityScore">The quality score between 1 and 4.</param>
        /// <returns></returns>
        public static MvcHtmlString DashboardQualityCell(this HtmlHelper html, int? qualityScore, string helpIndex = "")
        {
            string helpLink = string.Empty;
            if (!String.IsNullOrWhiteSpace(helpIndex))
            {
                TagBuilder helpTag = new TagBuilder("a");
                helpTag.MergeAttribute("href", string.Format("/Help/DataQuality#{0}", helpIndex));
                helpTag.MergeAttribute("target", "_blank");
                helpTag.AddCssClass("text-white pull-right");
                helpTag.SetInnerText(AppGlobal.Language.GetText("Dashboard_DataQuality_CellHelp", "(help)"));
                helpLink = helpTag.ToString();
            }


            var tag = new TagBuilder("td");
            if (qualityScore == null)
            {
                tag.SetInnerText("-");
            }
            else
            {
                string cellBackground = QualityIndicator.GetQualityBackground(qualityScore.Value);
                tag.AddCssClass(cellBackground);
                var text = String.Format("{0}", QualityIndicator.GetQualityText(qualityScore.Value, false));
                tag.InnerHtml = text + (cellBackground == "bg-quality-poor" ? helpLink : "");
            }
            return tag.ToMvcHtmlString(TagRenderMode.Normal);
        }
    }
}