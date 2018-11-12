using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IMS.NCS.CourseSearchService.Common.Unity
{
    /// <summary>
    /// 
    /// </summary>
    public class StringToListConverter : TypeConverter
    {
        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value != null)
            {
                string configValues = value.ToString();
                if (!string.IsNullOrEmpty(configValues))
                {
                    string[] categories = configValues.Split(',');
                    return categories.ToList();
                }
            }
            return new List<string>();
        }
    }
}