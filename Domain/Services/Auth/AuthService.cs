using Domain.Data.Entities;
using UserEntity = Domain.Data.Entities.User;
using Domain.Data.Models;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Repository;
using Constant = Domain.Constants.Constants;
using Domain.Data.Models.DTOs;

namespace Domain.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccessTokenRepository _accessTokenRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(
        IUserRepository userRepository,
        IAccessTokenRepository accessTokenRepository,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _accessTokenRepository = accessTokenRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Tokens> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            throw new AuthenticationException(AuthenticationErrorMessage.INVALID_TOKEN);
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            throw new AuthenticationException(AuthenticationErrorMessage.INVALID_TOKEN);
        }

        var response = await GenerateTokensAsync(user.Id);

        await _userRepository.UpdatePartialAsync(
            new UserEntity { Id = user.Id },
            u => u.LastAccessAt = DateTimeOffset.UtcNow,
            user.Id
        );

        return response;
    }

    public async Task<Tokens> RefreshTokenAsync(string refreshTokenId)
    {
        var refreshToken = await _refreshTokenRepository.GetAsync(refreshTokenId);

        if (refreshToken == null)
        {
            throw new AuthenticationException(AuthenticationErrorMessage.TOKEN_EXPIRED);
        }

        var user = await _userRepository.GetAsync(refreshToken.UserId);

        if (user == null)
        {
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);
        }

        refreshToken.ExpiresAt = DateTimeOffset.UtcNow.AddHours(Constant.Settings.AuthSettings.RefreshTokenExpiration);
        refreshToken.DeletedAt = DateTimeOffset.UtcNow;

        var response = await GenerateTokensAsync(user.Id);

        await _userRepository.UpdatePartialAsync(
            new UserEntity { Id = user.Id },
            u => u.LastAccessAt = DateTimeOffset.UtcNow,
            user.Id
        );

        return response;
    }

    public async Task<Tokens> RevokeAccessTokenAsync(string accessTokenId, string refreshTokenId, UserEntity actor)
    {
        AccessTokenDTO accessTokenDTO = await _accessTokenRepository.GetByToken(accessTokenId);
        RefreshToken refreshToken = await _refreshTokenRepository.GetAsync(refreshTokenId);

        if (accessTokenDTO != null)
        {
            accessTokenDTO.UpdatedBy = actor.Id;
            accessTokenDTO.DeletedAt = DateTimeOffset.UtcNow;
            accessTokenDTO.ExpiresAt = DateTimeOffset.MinValue;

            _ = await _accessTokenRepository.UpdatePartialAsync(
                new AccessToken
                {
                    Id = accessTokenDTO.Id
                },
                accessToken =>
                {
                    accessToken.UpdatedBy = accessTokenDTO.UpdatedBy;
                    accessToken.DeletedAt = accessTokenDTO.DeletedAt;
                    accessToken.ExpiresAt = accessTokenDTO.ExpiresAt;
                },
                actor.Id);
        }
        else
            throw new AuthenticationException(AuthenticationErrorMessage.ACCESSTOKEN_NOT_FOUND);

        if (refreshToken != null)
        {
            refreshToken.UpdatedBy = actor.Id;
            refreshToken.DeletedAt = DateTimeOffset.UtcNow;
            refreshToken.ExpiresAt = DateTimeOffset.MinValue;

            refreshToken = await _refreshTokenRepository.UpdateAsync(refreshToken, actor.Id);
        }
        else
        {
            throw new AuthenticationException(AuthenticationErrorMessage.REFRESHTOKEN_NOT_FOUND);
        }

        return new Tokens
        {
            AccessToken = accessTokenDTO.Id,
            AccessTokenExpiresAt = accessTokenDTO.ExpiresAt,
            RefreshToken = refreshToken.Id,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt,
        };
    }

    #region PrivateMethods

    private async Task<Tokens> GenerateTokensAsync(string userId)
    {
        AccessToken accessToken = await _accessTokenRepository.InsertAsync(new AccessToken()
        {
            UserId = userId,
            ExpiresAt = DateTimeOffset.UtcNow.AddHours(Constant.Settings.AuthSettings.AccessTokenExpiration)
        });

        RefreshToken refreshToken = await _refreshTokenRepository.InsertAsync(new RefreshToken()
        {
            UserId = userId,
            ExpiresAt = DateTimeOffset.UtcNow.AddHours(Constant.Settings.AuthSettings.RefreshTokenExpiration)
        });

        return new Tokens
        {
            AccessToken = accessToken.Id,
            AccessTokenExpiresAt = accessToken.ExpiresAt,
            RefreshToken = refreshToken.Id,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt
        };
    }

    #endregion PrivateMethods
}
