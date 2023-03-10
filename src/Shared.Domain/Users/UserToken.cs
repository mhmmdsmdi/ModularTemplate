using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.Domain.Users;

public class UserToken : IdentityUserToken<Guid>, IEntity
{
    public UserToken()
    {
        GeneratedTime = DateTime.Now;
    }

    public DateTime GeneratedTime { get; set; }
    public virtual User User { get; set; }
}

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable(nameof(UserToken), "Identity");
    }
}