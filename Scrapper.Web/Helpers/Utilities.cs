using Serilog.Debugging;
using System.Diagnostics;
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

    public static void EnableSerilogSelfLogging()
    {
        try
        {
            var errorFilePath = $"{Directory.GetCurrentDirectory()}\\Logs\\Scrapper.SerilogInternalErrors.log";

            // Ref - https://arefinblog.wordpress.com/2011/06/20/thread-safe-streamwriter/
            SelfLog.Enable(TextWriter.Synchronized(File.AppendText(errorFilePath)));
        }
        catch (Exception exc)
        {
            // Error can be seen from Azure Log Stream
            Debug.Write(exc.Message);
            Trace.TraceError(exc.Message);
        }
    }
}