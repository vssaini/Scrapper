using Scrapper.Application;
using Scrapper.Infrastructure;
using Scrapper.Web.Extensions;
using Scrapper.Web.Helpers;
using Scrapper.Web.Models;
using Serilog;
using Serilog.Debugging;
using System.Diagnostics;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.AzureApp()
    .CreateBootstrapLogger();

Log.Information("Starting application {ApplicationName}", WebConstants.AppName);

try
{
    EnableSerilogSelfLogging();

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddControllersWithViews();

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddDependencies(builder.Configuration);

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseSerilogRequestLogging();

    app.UseRouting();

    app.UseSession();
    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    Utilities.SetAppCulture();

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException" && ex.GetType().Name is not "HostAbortedException")
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

static void EnableSerilogSelfLogging()
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