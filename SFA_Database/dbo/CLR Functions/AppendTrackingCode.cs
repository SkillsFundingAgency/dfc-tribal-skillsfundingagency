using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web;
using Microsoft.SqlServer.Server;

// ReSharper disable once CheckNamespace
public partial class UserDefinedFunctions
{
    private static readonly char[] RemoveMe = {'?', '&', ' ', '\t', '\n', '\r'};

    /// <summary>
    /// Append a tracking code to a base URL.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="trackingUrl"></param>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static string AppendTrackingUrl(string url, string trackingUrl)
    {
        url = (url ?? String.Empty).Trim().TrimEnd(RemoveMe);
        if (String.IsNullOrEmpty(url)) return null;
        trackingUrl = (trackingUrl ?? String.Empty).Trim().TrimStart(RemoveMe);
        if (String.IsNullOrEmpty(trackingUrl)) return url;
        return url.Contains("?")
            ? url + "&" + trackingUrl
            : url + "?" + trackingUrl;
    }
}
