using Microsoft.EntityFrameworkCore;

namespace Infrastructure.General.SqlServer.Contexts;

public abstract class BaseGeneralDbContext : DbContext
{
    protected abstract string Schema { get; }

    protected BaseGeneralDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}