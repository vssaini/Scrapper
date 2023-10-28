using Microsoft.EntityFrameworkCore;
using Scrapper.Application.Abstractions.Clock;
using Scrapper.Application.Exceptions;
using Scrapper.Domain.Abstractions;

namespace Scrapper.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    //private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    //{
    //    TypeNameHandling = TypeNameHandling.All
    //};

    private readonly IDateTimeProvider _dateTimeProvider;

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }
}