using Serilog;

namespace Scrapper.Web.Extensions;

public static class AppExtensions
{
    public static void UseVariousMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error/500");
            //app.UseHsts(); // Disable for IIS testing
        }

        app.Use(async (context, next) =>
        {
            await next();
            if (context.Response.StatusCode == 404)
            {
                context.Request.Path = "/Error/404";
                await next();
            }
        });

        app.UseRequestLocalization("en-US");

        //app.UseHttpsRedirection(); // Disable for IIS testing
        app.UseStaticFiles();

        app.UseSerilogRequestLogging();

        app.UseRouting();

        app.UseSession();
        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers(); // For attribute routing
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    }
}