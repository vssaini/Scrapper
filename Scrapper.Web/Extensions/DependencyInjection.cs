using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Scrapper.Infrastructure;
using Scrapper.Web.Contracts;
using Scrapper.Web.Models;
using Scrapper.Web.Services;

namespace Scrapper.Web.Extensions;

public static class DependencyInjection
{
    public static void AddWebDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.BindSettings(configuration);

        services.ConfigureDataProtection();
        services.ConfigureSession();
        services.ConfigureAuthentication();
        services.ConfigureDependencies();
    }

    private static void BindSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Models.Settings.Firebase>(options => configuration.GetSection(WebConstants.FirebaseSectionName).Bind(options));
    }

    public static void ConfigureDataProtection(this IServiceCollection services)
    {
        services.AddDataProtection()
            .PersistKeysToDbContext<ApplicationDbContext>();
    }

    private static void ConfigureSession(this IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
    }

    private static void ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
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
            options.Secure = CookieSecurePolicy.Always;
        });
    }

    private static void ConfigureDependencies(this IServiceCollection services)
    {
        services.AddScoped<IHomeService, HomeService>();
        services.AddScoped<IProductService, ProductService>();
    }
}