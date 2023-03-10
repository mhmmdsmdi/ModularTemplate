using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Shared.Domain.Users;

public class UserRole : IdentityUserRole<Guid>, IEntity
{
    public DateTime CreatedUserRoleDate { get; set; }
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable(nameof(UserRole), "Identity");
    }
}