using UserEntity = Domain.Data.Entities.User;
using Domain.Data.Models;

namespace Domain.Services.Auth;

public interface IAuthService
{
    Task<Tokens> AuthenticateAsync(string email, string password);
    Task<Tokens> RefreshTokenAsync(string refreshTokenId);
    Task<Tokens> RevokeAccessTokenAsync(string accessTokenId, string refreshTokenId, UserEntity actor);
}
