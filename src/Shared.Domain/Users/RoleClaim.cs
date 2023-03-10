using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Shared.Domain.Users;

public class RoleClaim : IdentityRoleClaim<Guid>, IEntity
{
    public RoleClaim()
    {
        CreatedClaim = DateTime.Now;
    }

    public DateTime CreatedClaim { get; set; }
    public virtual Role Role { get; set; }
}

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable(nameof(RoleClaim), "Identity");
    }
}