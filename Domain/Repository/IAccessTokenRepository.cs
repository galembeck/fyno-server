using Domain.Data.Entities;
using Domain.Data.Models.DTOs;
using Domain.Repository._Base;

namespace Domain.Repository;

public interface IAccessTokenRepository : IRepository<AccessToken>
{
    Task<AccessTokenDTO> GetByToken(string token, CancellationToken cancellationToken = default);
    Task<AccessToken?> GetAccessTokenAsync(string tokenId, CancellationToken cancellationToken = default);
}
