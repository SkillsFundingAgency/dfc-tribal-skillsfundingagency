using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    /// <summary>
    /// Various utility methods for quality indicator scores
    /// </summary>
    public static class QualityIndicator
    {
        /// <summary>
        /// The quality background colour CSS classes.
        /// </summary>
        private static readonly List<string> CssClasses = new List<string>
        {
            "bg-quality-poor",
            "bg-quality-average",
            "bg-quality-good",
            "bg-quality-very-good"
        };

        /// <summary>
        /// The quality orb CSS classes
        /// </summary>
        private static readonly List<string> OrbCssClasses = new List<string>
        {
            "quality-poor",
            "quality-average",
            "quality-good",
            "quality-very-good"
        };

        private static readonly List<string> Terms;
        private static readonly List<string> TermsParenthesis;
        private static readonly List<string> TrafficLights;

        /// <summary>
        /// Traffic lights.
        /// </summary>
        public enum TrafficLight
        {
            Red = 1,
            Amber = 2,
            Green = 3
        }

        static QualityIndicator()
        {
            Terms = new List<string>
            {
                AppGlobal.Language.GetText("QualityIndicator_Status_Poor", "Poor"),
                AppGlobal.Language.GetText("QualityIndicator_Status_Average", "Average"),
                AppGlobal.Language.GetText("QualityIndicator_Status_Good", "Good"),
                AppGlobal.Language.GetText("QualityIndicator_Status_VeryGood", "Very Good"),
            };
            TermsParenthesis = new List<string>
            {
                "(" + Terms[0] + ")",
                "(" + Terms[1] + ")",
                "(" + Terms[2] + ")",
                "(" + Terms[3] + ")",
            };
            TrafficLights = new List<string>
            {
                AppGlobal.Language.GetText("QualityIndicator_TrafficLight_Red", "Red"),
                AppGlobal.Language.GetText("QualityIndicator_TrafficLight_Amber", "Amber"),
                AppGlobal.Language.GetText("QualityIndicator_TrafficLight_Green", "Green"),
            };
        }

        /// <summary>
        /// Gets the quality text for a recent date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="parenthesis">if set to <c>true</c> [parenthesis].</param>
        /// <returns></returns>
        public static string GetQualityText(DateTime? date, bool parenthesis = true)
        {
            var pool = parenthesis ? TermsParenthesis : Terms;
            if (date == null) return String.Empty;
            var months = GetMonthsBetween(date.Value, DateTime.UtcNow);
            return months > 12
                ? pool[0]
                : months >= 4
                    ? pool[1]
                    : months > 1
                        ? pool[2]
                        : pool[3];
        }

        /// <summary>
        /// Gets the quality background for a number in the range .00 to 1.00.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static string GetQualityBackground(decimal number)
        {
            return number >= (decimal) .91
                ? CssClasses[3]
                : number >= (decimal) .71
                    ? CssClasses[2]
                    : number >= (decimal) .51
                        ? CssClasses[1]
                        : CssClasses[0];
        }

        /// <summary>
        /// Gets the quality background for a quality score in the range 1 to 4.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static string GetQualityBackground(int number)
        {
            if (number < 1 || number > 4)
                throw new ArgumentOutOfRangeException();
            return CssClasses[number - 1];
        }

        /// <summary>
        /// Gets the traffic light quality orb for a traffic light date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="isSfaFunded"></param>
        /// <param name="isDfeFunded"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static string GetTrafficOrb(DateTime? date, bool isSfaFunded, bool isDfeFunded)
        {
            if (date == null) return OrbCssClasses[0];
            if (isSfaFunded)
            {
                var freshnessPeriod = GetGreenDuration(date.Value.Month);
                var months = GetMonthsBetween(date.Value, DateTime.UtcNow);
                return months > freshnessPeriod
                    ? OrbCssClasses[0]
                    : months == freshnessPeriod
                        ? OrbCssClasses[1]
                        : OrbCssClasses[3];
            }
            if (isDfeFunded)
            {
                switch (DfeProviderDateToIndex(date.Value))
                {
                    case TrafficLight.Green:
                        return OrbCssClasses[3];
                    case TrafficLight.Amber:
                        return OrbCssClasses[1];
                    default:
                        return OrbCssClasses[0];
                }
            }
            return OrbCssClasses[0];
        }

        /// <summary>
        /// Gets the quality orb CSS class for a quality score in the range 1 to 4.
        /// </summary>
        /// <param name="number">The score.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static string GetQualityOrb(int number)
        {
            if (number < 1 || number > 4)
                throw new ArgumentOutOfRangeException();
            return OrbCssClasses[number - 1];
        }

        /// <summary>
        /// Gets the quality background for a traffic light date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="isSfaFunded"></param>
        /// <param name="isDfeFunded"></param>
        /// <returns></returns>
        public static string GetTrafficBackground(DateTime? date, bool isSfaFunded, bool isDfeFunded)
        {
            if (date == null) return CssClasses[0];
            if (isSfaFunded)
            {
                var freshnessPeriod = GetGreenDuration(date.Value.Month);
                var months = GetMonthsBetween(date.Value, DateTime.UtcNow);
                return months > freshnessPeriod
                    ? CssClasses[0]
                    : months == freshnessPeriod
                        ? CssClasses[1]
                        : CssClasses[3];
            }
            if (isDfeFunded)
            {
                switch (DfeProviderDateToIndex(date.Value))
                {
                    case TrafficLight.Green:
                        return CssClasses[3];
                    case TrafficLight.Amber:
                        return CssClasses[1];
                    default:
                        return CssClasses[0];
                }
            }

            return CssClasses[0];
        }

        /// <summary>
        /// Gets the quality background for a traffic light number in the range 1 to 3.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static string GetTrafficBackground(int number)
        {
            if (number < 1 || number > 3)
                throw new ArgumentOutOfRangeException();
            return number < 3
                ? CssClasses[number - 1]
                : CssClasses[3];
        }
        
        /// <summary>
        /// Gets the quality text for a number in the range .00 to 1.00.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="parenthesis">if set to <c>true</c> [parenthesis].</param>
        /// <returns></returns>
        public static string GetQualityText(decimal number, bool parenthesis = true)
        {
            var pool = parenthesis ? TermsParenthesis : Terms;
            return number >= (decimal) .91
                ? pool[3]
                : number >= (decimal) .71
                    ? pool[2]
                    : number >= (decimal) .51
                        ? pool[1]
                        : pool[0];
        }

        /// <summary>
        /// Gets the quality score in the range 1 to 4 for a number in the range 0.00 to 1.00.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static int GetQualityScore(decimal? number)
        {
            if (number == null) return 1;
            return number >= (decimal).91
                ? 4
                : number >= (decimal).71
                    ? 3
                    : number >= (decimal).51
                        ? 2
                        : 1;
        }

        /// <summary>
        /// Gets the quality text for a quality score in the range 1 to 4.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="parenthesis">if set to <c>true</c> [parenthesis].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static string GetQualityText(int number, bool parenthesis = true)
        {
            if (number < 1 || number > 4)
                throw new ArgumentOutOfRangeException();
            var pool = parenthesis ? TermsParenthesis : Terms;
            return pool[number - 1];
        }

        /// <summary>
        /// Gets the quality text for a traffic light date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="isSfaFunded"></param>
        /// <param name="isDfeFunded"></param>
        /// <returns></returns>
        public static string GetTrafficText(DateTime? date, bool isSfaFunded, bool isDfeFunded)
        {
            if (date == null) return TrafficLights[0];
            if (isSfaFunded)
            {
                var freshnessPeriod = GetGreenDuration(date.Value.Month);
                var months = GetMonthsBetween(date.Value, DateTime.UtcNow);
                return months > freshnessPeriod
                    ? TrafficLights[0]
                    : months == freshnessPeriod
                        ? TrafficLights[1]
                        : TrafficLights[2];
            }
            if (isDfeFunded)
            {
                var idx = (int)DfeProviderDateToIndex(date.Value);
                return GetTrafficText(idx);
            }
            return TrafficLights[0];
        }

        /// <summary>
        /// Gets the quality text for a traffic light int in the range 1 to 3.
        /// </summary>
        /// <param name="number">The date.</param>
        /// <returns></returns>
        public static string GetTrafficText(int number)
        {
            return TrafficLights[number - 1];
        }

        public static TrafficLight GetTrafficStatus(DateTime? date, bool isSfaFunded, bool isDfeFunded)
        {
            if (date == null) return TrafficLight.Red;
            if (isSfaFunded)
            {
                var freshnessPeriod = GetGreenDuration(date.Value.Month);
                var months = GetMonthsBetween(date.Value, DateTime.UtcNow);
                return months > freshnessPeriod
                    ? TrafficLight.Red
                    : months == freshnessPeriod
                        ? TrafficLight.Amber
                        : TrafficLight.Green;
            }
            if (isDfeFunded)
            {
                return DfeProviderDateToIndex(date.Value);
            }
            return TrafficLight.Red;
        }

        /// <summary>
        /// Gets the number of months a provider's traffic light stays green
        /// based on the month of the last update. The provider is assumed to be 
        /// amber for one month after.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns></returns>
        public static int GetGreenDuration(int month)
        {
            switch (month)
            {
                case 6:
                    return 5;
                case 7:
                    return 4;
                case 8:
                    return 3;
                default:
                    return 2;
            }
        }

        /// <summary>
        ///     Converts a DfE provider update date into a traffic light index.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static TrafficLight DfeProviderDateToIndex(DateTime date)
        {
            var today = DateTime.UtcNow.Date;

            var endDate = GetDfeProviderGreenEndDate(date);

            if (today.Date <= endDate.Date)
                return TrafficLight.Green;

            if (today.Year == endDate.Year && today.Month == 10)
                return TrafficLight.Amber;

            return TrafficLight.Red;
        }

        /// <summary>
        /// Gets the last day a DfE provider would be considered green.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime GetDfeProviderGreenEndDate(DateTime date)
        {
            return date.Month < 5
                ? new DateTime(date.Year, 9, 30)
                : new DateTime(date.Year + 1, 9, 30);
        }

        public static DateTime GetDfeProviderNextUpdateDueDate(DateTime date)
        {
            return date.Month < 5
                ? new DateTime(date.Year, 10, 31)
                : new DateTime(date.Year + 1, 10, 31);
        }


        /// <summary>
        /// Gets the months between two dates.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static int GetMonthsBetween(DateTime from, DateTime to)
        {
            // Adapted from: http://stackoverflow.com/a/9874527

            // Ignore the time element
            from = from.Date;
            to = to.Date;

            if (from > to)
            {
                return GetMonthsBetween(to, from);
            }

            Int32 monthDiff = Math.Abs((to.Year*12 + (to.Month - 1)) - (from.Year*12 + (from.Month - 1)));
            return from.AddMonths(monthDiff) > to || to.Day < from.Day
                ? monthDiff - 1
                : monthDiff;
        }

        #region Session Quality Information

        /// <summary>
        /// Set quality session information for the current context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        public static void SetSessionInformation(UserContext.UserContextInfo context)
        {
            if (context == null || context.ItemId == null)
            {
                ClearSessionInformation();
                return;
            }

            var db = new ProviderPortalEntities();
            switch (context.ContextName)
            {
                case UserContext.UserContextName.Provider:
                {
                    // Update the score if it doesn't exist
                    if (db.QualityScores.All(x => x.ProviderId != context.ItemId.Value))
                    {
                        db.up_ProviderUpdateQualityScore(context.ItemId.Value, true);
                    }
                        var info = db.QualityScores
                            .Where(x => x.ProviderId == context.ItemId.Value)
                            .Select(x => new
                            {
                                Score = x.AutoAggregateQualityRating,
                                UpdatedDateTimeUtc = x.ModifiedDateTimeUtc,
                                SfaFunded = x.Provider.SFAFunded,
                                DfeFunded = x.Provider.DFE1619Funded,
                                ProviderType = x.Provider.ProviderType != null ? x.Provider.ProviderType.ProviderTypeName : String.Empty,
                                LastCalculated = x.CalculatedDateTimeUtc
                        }).FirstOrDefault();
                    DateTime? lastAllDataUpToDateTimeUtc = null;
                    Provider provider = db.Providers.Find(context.ItemId.Value);
                    if (provider != null)
                    {
                        lastAllDataUpToDateTimeUtc = provider.LastAllDataUpToDateTimeUtc;
                    }
                    HttpContext.Current.Session[Constants.SessionFieldNames.ProviderLastActivity] =
                        info == null ? lastAllDataUpToDateTimeUtc : lastAllDataUpToDateTimeUtc.HasValue ? ProvisionDataCurrent.GetLatestDate(lastAllDataUpToDateTimeUtc.Value, info.UpdatedDateTimeUtc.HasValue ? info.UpdatedDateTimeUtc.Value : DateTime.MinValue) : info.UpdatedDateTimeUtc;
                    HttpContext.Current.Session[Constants.SessionFieldNames.ProviderLastProvisionUpdate] =
                        info == null ? null : info.UpdatedDateTimeUtc;
                    HttpContext.Current.Session[Constants.SessionFieldNames.ProviderQualityScore] =
                        info == null ? 0.0m : info.Score.Value/100m;
                    HttpContext.Current.Session[Constants.SessionFieldNames.ProviderIsSfaFunded] =
                        info != null && info.SfaFunded;
                    HttpContext.Current.Session[Constants.SessionFieldNames.ProviderIsDfe1619Funded] =
                        info != null && info.DfeFunded;
                    HttpContext.Current.Session[Constants.SessionFieldNames.ProviderType] = info != null ? info.ProviderType : String.Empty;
                    HttpContext.Current.Session[Constants.SessionFieldNames.ProviderQALastCalculated] =  info == null ? (DateTime?)null : info.LastCalculated;
                        break;
                }
                case UserContext.UserContextName.Organisation:
                {
                    // Update the score if it doesn't exist
                    if (db.OrganisationQualityScores.All(x => x.OrganisationId != context.ItemId.Value))
                    {
                        db.up_OrganisationUpdateQualityScore(context.ItemId.Value);
                    }
                    var info = db.OrganisationQualityScores
                        .Where(x => x.OrganisationId == context.ItemId.Value)
                        .Select(x => new
                        {
                            EarliestModifiedDateTimeUtc = x.EarliestModifiedDateTimeUtc,
                            SfaFunded =
                                x.Organisation.OrganisationProviders.Any(
                                    y => y.IsAccepted && !y.IsRejected && y.Provider.SFAFunded),
                            DfeFunded =
                                x.Organisation.OrganisationProviders.Any(
                                    y => y.IsAccepted && !y.IsRejected && y.Provider.DFE1619Funded)
                        }).FirstOrDefault();
                    HttpContext.Current.Session[Constants.SessionFieldNames.OrganisationLastActivity] =
                        info == null ? null : info.EarliestModifiedDateTimeUtc;
                    HttpContext.Current.Session[Constants.SessionFieldNames.OrganisationIsSfaFunded] =
                        info != null && info.SfaFunded;
                    HttpContext.Current.Session[Constants.SessionFieldNames.OrganisationIsDfe1619Funded] =
                        info != null && info.DfeFunded;
                    break;
                }
                default:
                {
                    ClearSessionInformation();
                    break;
                }
            }
        }

        /// <summary>
        /// Clear quality indicator information from the current session.
        /// </summary>
        public static void ClearSessionInformation()
        {
            HttpContext.Current.Session[Constants.SessionFieldNames.ProviderLastActivity] = null;
            HttpContext.Current.Session[Constants.SessionFieldNames.ProviderLastProvisionUpdate] = null;
            HttpContext.Current.Session[Constants.SessionFieldNames.ProviderQualityScore] = null;
            HttpContext.Current.Session[Constants.SessionFieldNames.ProviderIsSfaFunded] = null;
            HttpContext.Current.Session[Constants.SessionFieldNames.ProviderIsDfe1619Funded] = null;
            HttpContext.Current.Session[Constants.SessionFieldNames.OrganisationIsSfaFunded] = null;
            HttpContext.Current.Session[Constants.SessionFieldNames.OrganisationIsDfe1619Funded] = null;
            HttpContext.Current.Session[Constants.SessionFieldNames.OrganisationLastActivity] = null;
        }

        /// <summary>
        /// Update the masthead updating the quality information when a new course or course instance is created or edited.
        /// </summary>
        /// <param name="lastActivity">Last activity</param>
        /// <param name="lastUpdated">Last provision update date.</param>
        /// <param name="qualityScore">Quality score in the range 0 to 100.</param>
        public static void UpdateSessionQualityInformation(DateTime? lastActivity, DateTime? lastUpdated, decimal qualityScore)
        {
            HttpContext.Current.Session[Constants.SessionFieldNames.ProviderLastActivity] = lastActivity; 
            HttpContext.Current.Session[Constants.SessionFieldNames.ProviderLastProvisionUpdate] = lastUpdated;
            HttpContext.Current.Session[Constants.SessionFieldNames.ProviderQualityScore] = qualityScore / 100m;
        }


        public static void CheckQAUpToDate()
        {
            var providerQALastCalculated = (DateTime?)HttpContext.Current.Session[Constants.SessionFieldNames.ProviderQALastCalculated];
            if (!providerQALastCalculated.HasValue) return;

            var context = UserContext.GetUserContext();
            if (context == null) return;

            using (var db = new ProviderPortalEntities())
            {
                var currentLastCalculated = db.QualityScores.Where(s => s.ProviderId == context.ItemId).Select(x => x.CalculatedDateTimeUtc).FirstOrDefault();
                if (currentLastCalculated != null && currentLastCalculated > providerQALastCalculated.Value)
                {
                    //Quality score values in session are out of date and require updating with current values
                    SetSessionInformation(context);
                }
            }

        }
        #endregion
    }
}