using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Shared.Domain.Users;

public class UserLogin : IdentityUserLogin<Guid>
{
    public UserLogin()
    {
        LoggedOn = DateTime.Now;
    }

    public DateTime LoggedOn { get; set; }

    public virtual User User { get; set; }
}

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable(nameof(UserLogin), "Identity");
    }
}