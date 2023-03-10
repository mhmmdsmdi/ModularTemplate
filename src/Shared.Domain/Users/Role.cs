using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Shared.Domain.Users;

public class Role : IdentityRole<Guid>, IEntity<Guid>
{
    public Role()
    {
        CreatedDate = DateTime.Now;
    }

    public string DisplayName { get; set; }
    public DateTime CreatedDate { get; set; }

    public virtual ICollection<RoleClaim> Claims { get; set; }
    public virtual ICollection<UserRole> Users { get; set; }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role), "Identity");
    }
}