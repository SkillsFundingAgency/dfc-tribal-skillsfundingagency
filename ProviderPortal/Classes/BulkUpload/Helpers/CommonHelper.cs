using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Tribal.SkillsFundingAgency.ProviderPortal.BulkUpload.Helpers
{
    public static class CommonHelper
    {

        public static T GetSafe<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(T);

            T result;
            try
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                result = default(T);
            }
            return result;
        }

        public static string GetSafeString<T>(T value)
        {
            return value == null ? string.Empty : value.ToString();
        }

        internal static string GetSafeDateString(DateTime? value)
        {
            return value.HasValue ? value.Value.ToString(Constants.ConfigSettings.ShortDateFormat) : string.Empty;
        }
    }
}