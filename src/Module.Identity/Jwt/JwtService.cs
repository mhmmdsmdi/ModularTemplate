using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Module.Identity.Core;
using Module.Identity.Core.Dtos;
using Module.Identity.Infrastructure.Repositories;
using Shared.Domain.Users;

namespace Module.Identity.Jwt;

public interface IJwtService : IService
{
    Task<AccessToken> GenerateAsync(User user, CancellationToken cancellationToken);

    Task<AccessToken> RefreshToken(Guid refreshTokenId, CancellationToken cancellationToken);
}

public class JwtService : IJwtService
{
    private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
    private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;
    private readonly IdentitySettings _identitySettings;

    public JwtService(IOptions<IdentitySettings> identitySettings,
        IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
        IUserRefreshTokenRepository userRefreshTokenRepository)
    {
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _userRefreshTokenRepository = userRefreshTokenRepository;
        _identitySettings = identitySettings.Value;
    }

    public async Task<AccessToken> GenerateAsync(User user, CancellationToken cancellationToken)
    {
        var secretKey = Encoding.UTF8.GetBytes(_identitySettings.SecretKey); // longer that 16 character
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

        var encryptionKey = Encoding.UTF8.GetBytes(_identitySettings.EncryptKey); //must be 16 character
        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var claims = await GetUserClaims(user);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _identitySettings.Issuer,
            Audience = _identitySettings.Audience,
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now.AddMinutes(0),
            Expires = DateTime.Now.AddMinutes(_identitySettings.ExpirationMinutes),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

        var refreshToken = await _userRefreshTokenRepository.CreateToken(user.Id, cancellationToken);

        return new AccessToken(securityToken, refreshToken.ToString());
    }

    public async Task<AccessToken> RefreshToken(Guid refreshTokenId, CancellationToken cancellationToken)
    {
        var refreshToken = await _userRefreshTokenRepository.GetTokenWithInvalidation(refreshTokenId, cancellationToken);

        if (refreshToken is null)
            return null;

        refreshToken.IsValid = false;

        await _userRefreshTokenRepository.SaveAsync(cancellationToken);

        var user = await _userRefreshTokenRepository.GetUserByRefreshToken(refreshTokenId, cancellationToken);

        if (user is null)
            return null;

        return await GenerateAsync(user, cancellationToken);
    }

    private async Task<IEnumerable<Claim>> GetUserClaims(User user)
    {
        var result = await _userClaimsPrincipalFactory.CreateAsync(user);
        return result.Claims;
    }
}