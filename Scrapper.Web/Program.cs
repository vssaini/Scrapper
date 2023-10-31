using Scrapper.Application;
using Scrapper.Infrastructure;
using Scrapper.Web.Extensions;
using Scrapper.Web.Helpers;
using Scrapper.Web.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.AzureApp()
    .CreateBootstrapLogger();

Log.Information("Starting application {ApplicationName}", WebConstants.AppName);

try
{
    Utilities.EnableSerilogSelfLogging();

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
    
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddWebDependencies(builder.Configuration);
    builder.Services.AddApplication();

    builder.Services.AddControllersWithViews();

    var app = builder.Build();
    app.UseVariousMiddlewares();
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