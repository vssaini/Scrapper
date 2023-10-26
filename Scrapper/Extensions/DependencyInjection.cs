using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Scrapper.Contracts;
using Scrapper.Services;

namespace Scrapper.Extensions;

public static class DependencyInjection
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        //services.BindSettings(configuration);

        //services.ConfigureClient(configuration);
        //services.ConfigureSession();

        services.AddClassDependencies();
        // services.AddAuthentication();
    }

    //private static void ConfigureClient(this IServiceCollection services, IConfiguration configuration)
    //{
    //    var umgApi = configuration.GetSection(WebConstants.UmgApiSectionName).Get<UmgApi>();

    //    services.AddHttpClient(Constants.ApiClientName, client =>
    //    {
    //        client.BaseAddress = new Uri(umgApi.BaseUrl ?? throw new Exception("UMG API BaseUrl is not set in appsettings.json."));
    //        client.DefaultRequestHeaders.Add("X-Api-Key", umgApi.ApiKey ?? throw new Exception("UMG API Key is not set in appsettings.json."));
    //    });
    //}

    private static void ConfigureSession(this IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
    }

    private static void AddClassDependencies(this IServiceCollection services)
    {
        services.AddScoped<IWescoService, WescoService>();
    }

    private static void AddAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                //options.Cookie.SecurePolicy = _environment.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;

                options.Cookie.Name = "UMG.Web.AuthCookieAspNetCore";
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

    //private static void BindSettings(this IServiceCollection services, IConfiguration configuration)
    //{
    //    services.Configure<UmgApi>(options => configuration.GetSection(WebConstants.UmgApiSectionName).Bind(options));
    //    services.Configure<Models.Settings.Firebase>(options => configuration.GetSection(WebConstants.FirebaseSectionName).Bind(options));
    //}
}