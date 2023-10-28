using System.Globalization;

namespace Scrapper.Web.Helpers;

public class Utilities
{
    /// <summary>
    /// Add build version to the URL.
    /// </summary>
    public static string UrlVersioning(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return url;

        url = url.Trim();

        var version = typeof(WebApplication).Assembly.GetName().Version?.ToString();
        if (string.IsNullOrWhiteSpace(version))
            return url;

        if (url.IndexOf("?", StringComparison.Ordinal) > 0)
            url += "&v=" + version;
        else
            url += "?v=" + version;

        return url;
    }

    public static void SetAppCulture()
    {
        const string culture = "en-US";
        var cultureInfo = CultureInfo.GetCultureInfo(culture);

        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
    }
}