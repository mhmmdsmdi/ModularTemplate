using Common.Repositories;
using Infrastructure.General.SqlServer;
using Infrastructure.General.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Users;

namespace Module.Identity.Infrastructure.Repositories;

public interface IUserRefreshTokenRepository : IRepository<UserRefreshToken>
{
    Task<Guid> CreateToken(Guid userId, CancellationToken cancellationToken);

    Task<UserRefreshToken> GetTokenWithInvalidation(Guid id, CancellationToken cancellationToken);

    Task<User> GetUserByRefreshToken(Guid tokenId, CancellationToken cancellationToken);

    Task RemoveUserOldTokens(Guid userId, CancellationToken cancellationToken);
}

public class UserRefreshTokenRepository : Repository<UserRefreshToken>, IUserRefreshTokenRepository
{
    public UserRefreshTokenRepository(BaseDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Guid> CreateToken(Guid userId, CancellationToken cancellationToken)
    {
        var userRefreshToken = new UserRefreshToken { IsValid = true, UserId = userId };
        await AddAsync(userRefreshToken, cancellationToken);
        return userRefreshToken.Id;
    }

    public async Task<UserRefreshToken> GetTokenWithInvalidation(Guid id, CancellationToken cancellationToken)
    {
        return await Table
            .Where(t => t.IsValid && t.Id.Equals(id))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User> GetUserByRefreshToken(Guid tokenId, CancellationToken cancellationToken)
    {
        var user = await TableNoTracking
            .Include(t => t.User)
            .Where(c => c.Id.Equals(tokenId))
            .Select(c => c.User)
            .FirstOrDefaultAsync(cancellationToken);

        return user;
    }

    public async Task RemoveUserOldTokens(Guid userId, CancellationToken cancellationToken)
    {
        await TableNoTracking
            .Where(x => x.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}