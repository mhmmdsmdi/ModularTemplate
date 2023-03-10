using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.Domain.Users;

public class User : IdentityUser<Guid>, IEntity<Guid>
{
    public User()
    {
        GeneratedCode = Guid.NewGuid().ToString().Substring(0, 8);
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string GeneratedCode { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User), "Identity");

        builder.Property(e => e.PhoneNumber).HasMaxLength(15);

        builder.Property(e => e.UserName)
            .IsRequired()
            .HasMaxLength(256);
    }
}