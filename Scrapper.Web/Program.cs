using Scrapper.Application;
using Scrapper.Infrastructure;
using Serilog;
using Serilog.Debugging;
using System.Diagnostics;
using Scrapper.Web.Extensions;

const string appName = "Scrapper";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.AzureApp()
    .CreateBootstrapLogger();

Log.Information("Starting application {ApplicationName}", appName);

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

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

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