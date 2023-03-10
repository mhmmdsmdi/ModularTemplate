using Common.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Shared.Domain.Users;

public class UserRefreshToken : BaseEntity<Guid>
{
    public UserRefreshToken()
    {
        CreatedAt = DateTime.Now;
    }

    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsValid { get; set; }
    public virtual User User { get; set; }
}

public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.ToTable(nameof(UserRefreshToken), "Identity");
    }
}