using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public static class Markdown
    {
        private static readonly Dictionary<String, String> replaceChars = new Dictionary<String, String>
        {
            {"–", "-"},     // Elongated hyphen
            {"“", "\""},    // Opening double quote
            {"”", "\""},    // Closing double quote
            {"‘", "'"},     // Opening single quote
            {"’", "'"},     // Closing single quote
            {" ", " "},     // Non-breaking space (&#160)
            {"—", "--"},    // Double hyphen
            {"©", "(C)"},   // Copyright
            {"®", "(R)"},   // Registered
            {"…", "..."},   // Ellipsis
            {"™", "(TM)"}   // Trademark
        };

        public static String Sanitize(string markdown)
        {
            // Remove HTML using HtmlAgilityPack
            var pageDoc = new HtmlDocument();
            pageDoc.LoadHtml("<html><body>" + markdown + "</body></html>");

            // Perform any replacements
            String mdown = pageDoc.DocumentNode.InnerText;
            foreach (KeyValuePair<String, String> kvp in replaceChars)
            {
                mdown = mdown.Replace(kvp.Key, kvp.Value);
            }

            return mdown;
        }

        // Everything below this point cribbed from NCETM
        private static readonly Hashtable AllowedTags = new Hashtable();

        static Markdown()
        {
            const string allowedTagNameString = "a,abbr,acronym,address,area,b,big,blockquote,br,caption,center,cite,code,col,colgroup,dd,del,div,dfn,di,dt,em,font,h1,h2,h3,h4,h5,h6,hr,i,img,ins,kbd,li,map,ol,p,pre,q,s,samp,small,span,strike,strong,style,sub,sup,table,tbody,td,tfoot,th,thead,tr,tt,u,ul";

            string[] allowedTagNames = allowedTagNameString.Split(',');

            foreach (string s in allowedTagNames.Where(s => !string.IsNullOrEmpty(s)))
            {
                AllowedTags[s.Trim().ToLower()] = null;
            }
        }

        private static string Html2SaferHtml(string s)
        {
            if (s == null) return null;

            MatchEvaluator myEvaluator = Html2SafeHtmlMatchEvaluator;

            string oldS = s + "PADDING";
            while (s != oldS)
            {
                oldS = s;
                s = FixLessThan(s);

                // Remove <script>...</script> and <script />
                s = Regex.Replace(s, @"<script[^>]*>((.*?)<?\/script)?[^>]*>", "",
                    RegexOptions.Multiline | RegexOptions.IgnoreCase);

                // The next two don't guard against spaces around = or symbols before = or the javascript: element
                // having extra characters or whitespace inserted within it.
                // The latter really only applies to the second RegEx
                // Remove on events in the format onXXXX=[|'|"]...[|'|"]
                s = Regex.Replace(s,
                    @"(\s(\bon[a-zA-Z][a-z]+)\s?\=\s?[`\'\""]?((java|vb|live)script\:)?[\w\(\),\' ]*;?[`\'\""]?)+", "",
                    RegexOptions.Multiline | RegexOptions.IgnoreCase);
                // Remove attributes in the format XXXX=[|'|"]javascript[|:]...[|'|"]
                s = Regex.Replace(s,
                    @"(\s(\b[a-zA-Z][a-z]+)\s?\=\s?[`\'\""]?((java|vb|live)script\:)[\w\(\),\' ]*;?[`\'\""]?)+", "",
                    RegexOptions.Multiline | RegexOptions.IgnoreCase);

                s = Regex.Replace(s, @"<(\s*/?\s*([a-zA-Z][a-zA-Z0-9]*)\s*.*?/?\s*)>", myEvaluator,
                    RegexOptions.Multiline);
            }

            return s;
        }

        private static string Html2SafeHtmlMatchEvaluator(Match match)
        {
            return AllowedTags.ContainsKey(match.Groups[2].Value.ToLower())
                ? match.Value
                : "";
        }

        /// <summary>
        /// Remove html tags from a string
        /// </summary>
        private static string Html2NoHtml(string s)
        {
            if (s == null) return null;

            s = FixLessThan(s);

            //strip style and script tags
            //and their content            
            s = Regex.Replace(s, @"<\s*\s*(?:style|script)\s*.*?\s*>.*?<\s*/?\s*(?:style|script)\s*>", String.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            //remove all other html tags 
            //but leave the content
            s = Regex.Replace(s, @"<(\s*/?\s*(?:[a-zA-Z][a-zA-Z0-9]*)\s*.*?/?\s*)>", String.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            //remove the comments
            s = Regex.Replace(s, @"<!--.*?-->", String.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            //remove entity references
            s = System.Web.HttpUtility.HtmlDecode(s);
            return s;
        }

        /// <summary>
        ///     Decodes encoded less than symbols (not including "&lt;?")
        /// </summary>
        private static string FixLessThan(string s)
        {
            return Regex.Replace(s, @"(%3c|&#x?[0]*(60|3c);?|\\(u|x)[0]*3c)", "<", RegexOptions.IgnoreCase);
        }
    }
}