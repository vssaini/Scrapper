using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrapper.Application.Abstractions.Clock;
using Scrapper.Application.Abstractions.Data;
using Scrapper.Application.Abstractions.Email;
using Scrapper.Application.Abstractions.Logger;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Royalties;
using Scrapper.Infrastructure.Clock;
using Scrapper.Infrastructure.Data;
using Scrapper.Infrastructure.Email;
using Scrapper.Infrastructure.Logger;
using Scrapper.Infrastructure.Repositories;
using Serilog;

namespace Scrapper.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<ILogService, LogService>();

        AddPersistence(services, configuration);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        var conString = configuration.GetConnectionString("UmgConString") ?? throw new ArgumentNullException(nameof(configuration));

        Log.Warning("Using connection string {ConnectionString}", conString);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(conString);
        });

        services.AddScoped<IScrapeRepository, ScrapeRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(conString));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }
}