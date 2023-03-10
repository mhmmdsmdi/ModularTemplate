using Common.Entities;
using Infrastructure.General.SqlServer.Contexts;
using Infrastructure.General.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Module.Identity.Core.BadUser;
using Shared.Domain.Users;

namespace Module.Identity.Infrastructure.Contexts;

public class BloggingContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=ModularV2;Integrated Security=true;Encrypt=False");

        return new IdentityDbContext(optionsBuilder.Options);
    }
}

public class IdentityDbContext : BaseDbContext
{
    public IdentityDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entitiesAssembly = typeof(BadUser).Assembly;
        modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
        modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
    }
}