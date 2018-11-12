using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web;
using Microsoft.SqlServer.Server;

// ReSharper disable once CheckNamespace
public partial class QualityIndicator
{
    /// <summary>
    /// Traffic lights.
    /// </summary>
    public enum TrafficLight
    {
        Red = 1,
        Amber = 2,
        Green = 3
    }

    /// <summary>
    /// Get the current traffic light status for a provider.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="isSfaFunded"></param>
    /// <param name="isDfeFunded"></param>
    /// <returns></returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static int GetTrafficStatus(DateTime? date, bool isSfaFunded, bool isDfeFunded)
    {
        if (date == null) return (int)TrafficLight.Red;
        if (isSfaFunded)
        {
            var freshnessPeriod = GetGreenDuration(date.Value.Month);
            var months = GetMonthsBetween(date.Value, DateTime.UtcNow);
            return months > freshnessPeriod
                ? (int)TrafficLight.Red
                : months == freshnessPeriod
                    ? (int)TrafficLight.Amber
                    : (int)TrafficLight.Green;
        }
        if (isDfeFunded)
        {
            return (int)DfeProviderDateToIndex(date.Value);
        }
        return (int)TrafficLight.Red;
    }

    /// <summary>
    /// Gets the number of months a provider's traffic light stays green
    /// based on the month of the last update. The provider is assumed to be 
    /// amber for one month after.
    /// </summary>
    /// <param name="month">The month.</param>
    /// <returns></returns>
    private static int GetGreenDuration(int month)
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
    private static TrafficLight DfeProviderDateToIndex(DateTime date)
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
    private static DateTime GetDfeProviderGreenEndDate(DateTime date)
    {
        return date.Month < 5
            ? new DateTime(date.Year, 9, 30)
            : new DateTime(date.Year + 1, 9, 30);
    }

    /// <summary>
    /// Gets the months between two dates.
    /// </summary>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    /// <returns></returns>
    [Microsoft.SqlServer.Server.SqlFunction]
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

        Int32 monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));
        return from.AddMonths(monthDiff) > to || to.Day < from.Day
            ? monthDiff - 1
            : monthDiff;
    }
}
