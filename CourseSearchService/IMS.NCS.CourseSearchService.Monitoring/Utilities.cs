using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using Ims.Schemas.Alse.CourseSearch.Contract;

namespace IMS.NCS.CourseSearchService.Monitoring
{
    public class Utilities
    {
        /// <summary>
        /// Returns the selected A10Flags values from the collection
        /// </summary>
        /// <param name="values">The collection containing the A10Flags checkbox values.</param>
        /// <param name="key">The A10Flags key of the values to find</param>
        /// <returns>The found A10Flags checkboxes selected values.</returns>
        public static A10InputType GetQueryStringA10Codes(NameValueCollection values, string key)
        {
            A10InputType result = new A10InputType();

            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(values[key])))
            {
                string commaSepFile = HttpUtility.HtmlDecode(values[key]);
                result.A10Code = commaSepFile.Split(new[] { ',' });
            }

            return result;
        }


        /// <summary>
        /// Returns the selected value from the collection
        /// </summary>
        /// <param name="values">The collection containing the checkbox value.</param>
        /// <param name="key">The key of the value to find</param>
        /// <returns>The found checkbox selected value.</returns>
        public static string GetCheckboxValue(NameValueCollection values, string key)
        {
            string result = null;

            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(values[key])))
            {
                result = HttpUtility.HtmlDecode(values[key]);
            }

            // checkboxes return 'on' for selected so convert to 'yes' for our
            // search
            return result == "on" ? "Y" : string.Empty;
        }


        /// <summary>
        /// Returns the selected values from the collection
        /// </summary>
        /// <param name="values">The collection containing the checkboxes values.</param>
        /// <param name="key">The key of the valuse to find</param>
        /// <returns>The found checkboxes selected values.</returns>
        public static string[] GetCheckboxValues(NameValueCollection values, string key)
        {
            string[] result = null;

            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(values[key])))
            {
                string commaSepFile = HttpUtility.HtmlDecode(values[key]);
                result = commaSepFile.Split(new[] { ',' });
            }

            return result;
        }


        /// <summary>
        /// Returns a string from the collection
        /// </summary>
        /// <param name="values">The collection containing the value.</param>
        /// <param name="key">The key of the value to find</param>
        /// <returns>The found string value.</returns>
        public static string GetQueryStringValue(NameValueCollection values, string key)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(values[key])))
            {
                result = HttpUtility.HtmlDecode(values[key]);
            }

            return result;
        }


        /// <summary>
        /// Returns a float from the collection
        /// </summary>
        /// <param name="values">The collection containing the value.</param>
        /// <param name="key">The key of the value to find</param>
        /// <returns>The found float value.</returns>
        public static float GetQueryStringValueAsFloat(NameValueCollection values, string key)
        {
            float result = 0;

            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(values[key])))
            {
                string value = HttpUtility.HtmlDecode(values[key]);
                float.TryParse(value, out result);
            }

            return result;
        }


        /// <summary>
        /// Returns an array of strings from the collection
        /// </summary>
        /// <param name="values">The collection containing the values.</param>
        /// <param name="key">The key of the value to find</param>
        /// <returns>An array of found string values.</returns>
        public static string[] GetQueryStringValues(NameValueCollection values, string key)
        {
            string[] result = null;

            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(values[key])))
            {
                string commaSepFile = HttpUtility.HtmlDecode(values[key]);
                result = commaSepFile.Split(new[] { ',' });
            }

            return result;
        }


        /// <summary>
        /// Returns the QualificationTypes from the collection
        /// </summary>
        /// <param name="values">The collection containing the QualificationTypes values.</param>
        /// <param name="key">The key of the value to find</param>
        /// <returns>The found QualificationTypes values.</returns>
        public static QualificationTypes GetQueryStringQualificationTypes(NameValueCollection values, string key)
        {
            QualificationTypes result = new QualificationTypes();

            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(values[key])))
            {
                string commaSepFile = HttpUtility.HtmlDecode(values[key]);
                result.QualificationType = commaSepFile.Split(new[] { ',' });
            }

            return result;
        }


        /// <summary>
        /// Returns the QualificationLevels from the collection
        /// </summary>
        /// <param name="values">The collection containing the QualificationLevels values.</param>
        /// <param name="key">The key of the value to find</param>
        /// <returns>The found QualificationLevels values.</returns>
        public static QualificationLevels GetQueryStringQualificationLevels(NameValueCollection values, string key)
        {
            QualificationLevels result = new QualificationLevels();

            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(values[key])))
            {
                string commaSepFile = HttpUtility.HtmlDecode(values[key]);
                result.QualificationLevel = commaSepFile.Split(new[] { ',' });
            }

            return result;
        }


        /// <summary>
        /// Returns the StudyModeType from the collection
        /// </summary>
        /// <param name="values">The collection containing the StudyModeType values.</param>
        /// <param name="key">The key of the value to find</param>
        /// <returns>The found StudyModeType values.</returns>
        public static StudyModeType GetQueryStringStudyModeType(NameValueCollection values, string key)
        {
            StudyModeType result = new StudyModeType();

            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(values[key])))
            {
                string commaSepFile = HttpUtility.HtmlDecode(values[key]);

                result.StudyMode = commaSepFile.Split(new[] { ',' });
            }

            return result;
        }


        /// <summary>
        /// Returns the AttendaceModeType from the collection
        /// </summary>
        /// <param name="values">The collection containing the AttendaceModeType values.</param>
        /// <param name="key">The key of the value to find</param>
        /// <returns>The found AttendaceModeType values.</returns>
        public static AttendaceModeType GetQueryStringAttendanceModeType(NameValueCollection values, string key)
        {
            AttendaceModeType result = new AttendaceModeType();

            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(values[key])))
            {
                string commaSepFile = HttpUtility.HtmlDecode(values[key]);

                result.AttendanceMode = commaSepFile.Split(new[] { ',' });
            }

            return result;
        }


        /// <summary>
        /// Returns the AttendancePatternType from the collection
        /// </summary>
        /// <param name="values">The collection containing the AttendancePatternType values.</param>
        /// <param name="key">The key of the value to find</param>
        /// <returns>The found AttendancePatternType values.</returns>
        public static AttendancePatternType GetQueryStringAttendancePatternType(NameValueCollection values, string key)
        {
            AttendancePatternType result = new AttendancePatternType();

            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(values[key])))
            {
                string commaSepFile = HttpUtility.HtmlDecode(values[key]);

                result.AttendancePattern = commaSepFile.Split(new[] { ',' });
            }

            return result;
        }


        /// <summary>
        /// Creates a javascript snippet line that will populate the control
        /// with the value.
        /// </summary>
        /// <param name="id">The id of the control.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>The snippet line that will populate the control.</returns>
        public static string GetJavascriptSetValueSnippet(string id, string value)
        {
            StringBuilder snippet = new StringBuilder();

            snippet.Append("document.getElementById(\"");
            snippet.Append(id + "\").value = \"");
            snippet.Append(value + "\";");
            snippet.Append(Environment.NewLine);
            return snippet.ToString();
        }


        /// <summary>
        /// Creates a javascript snippet line that will populate the control
        /// with the values in the array.
        /// </summary>
        /// <param name="id">The id of the control.</param>
        /// <param name="value">The values to set.</param>
        /// <returns>The snippet line that will populate the control.</returns>
        public static string GetJavascriptSetValueSnippet(string id, string[] value)
        {
            StringBuilder snippet = new StringBuilder();

            if (value != null)
            {
                string commaResults = String.Join(",", value);

                snippet.Append("document.getElementById(\"");
                snippet.Append(id + "\").value = \"");
                snippet.Append(commaResults + "\";");
                snippet.Append(Environment.NewLine);
            }
            return snippet.ToString();
        }


        /// <summary>
        /// Creates a javascript snippet line that will populate the checkbox control
        /// from the values in the array.
        /// </summary>
        /// <param name="id">The id of the checkbox control.</param>
        /// <param name="value">The values to set.</param>
        /// <returns>The snippet line that will populate the checkbox control.</returns>
        public static string GetJavascriptSetCheckboxValueSnippet(string id, string value)
        {
            StringBuilder snippet = new StringBuilder();

            if (!string.IsNullOrEmpty(value))
            {
                // grouped checkboxes are named 'id_value'
                snippet.Append("document.getElementById(\"");
                snippet.Append(id + "\").checked = 'yes';");
                snippet.Append(Environment.NewLine);
            }
            return snippet.ToString();
        }

        /// <summary>
        /// Creates a javascript snippet line that will populate the checkbox control
        /// from the values in the array.
        /// </summary>
        /// <param name="id">The id of the checkbox control.</param>
        /// <param name="value">The values to set.</param>
        /// <returns>The snippet line that will populate the checkbox control.</returns>
        public static string GetJavascriptSetCheckboxValueSnippet(string id, Boolean value)
        {
            StringBuilder snippet = new StringBuilder();

            if (value)
            {
                // grouped checkboxes are named 'id_value'
                snippet.Append("document.getElementById(\"");
                snippet.Append(id + "\").checked = 'yes';");
                snippet.Append(Environment.NewLine);
            }
            return snippet.ToString();
        }

        /// <summary>
        /// Creates a javascript snippet line that will populate the checkbox control
        /// from the values in the array.
        /// </summary>
        /// <param name="id">The id of the checkbox control.</param>
        /// <param name="value">The values to set.</param>
        /// <returns>The snippet line that will populate the checkbox control.</returns>
        public static string GetJavascriptSetCheckboxValueSnippet(string id, string[] value)
        {
            StringBuilder snippet = new StringBuilder();

            if (value != null)
            {
                foreach (string selected in value)
                {
                    // grouped checkboxes are named 'id_value'
                    snippet.Append("document.getElementById(\"");
                    snippet.Append(id + "_" + selected + "\").checked = 'yes';");
                    snippet.Append(Environment.NewLine);
                }
            }
            return snippet.ToString();
        }



        /// <summary>
        /// Creates a new QueryString type without the __VIEWSTATE key.  This enables
        /// us to redirect without an invalid MAC error.
        /// </summary>
        /// <param name="queryString">The current QueryString.</param>
        /// <returns>A new QueryString with any __VIEWSTATE key / value removed.</returns>
        public static NameValueCollection RemoveViewState(NameValueCollection queryString)
        {
            NameValueCollection newQueryString = new NameValueCollection();

            for (int index = 0; index < queryString.Count; index++)
            {
                if (queryString.GetKey(index).ToLower() != "__viewstate")
                {
                    newQueryString.Add(queryString.GetKey(index), queryString.Get(index));
                }
            }

            return newQueryString;
        }

    }
}