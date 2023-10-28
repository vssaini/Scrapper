using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Scrapper.Web.Models;

namespace Scrapper.Web.Extensions;

public static class DependencyInjection
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.BindSettings(configuration);
        services.ConfigureAuthentication();
    }

    private static void ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                //options.Cookie.SecurePolicy = _environment.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;

                options.Cookie.Name = "Scrapper.Web.AuthCookieAspNetCore";
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Home/Logout";
            });

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Strict;
            options.HttpOnly = HttpOnlyPolicy.None;
            //options.Secure = _environment.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
            options.Secure = CookieSecurePolicy.Always;
        });
    }

    private static void BindSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Models.Settings.Firebase>(options => configuration.GetSection(WebConstants.FirebaseSectionName).Bind(options));
    }
}