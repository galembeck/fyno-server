using Domain.Data.Entities;
using Domain.Data.Models.DTOs;
using Domain.Exceptions;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository;

public class AccessTokenRepository : BaseRepository<AccessToken>, IAccessTokenRepository
{
    public AccessTokenRepository(AppDbContext context) : base(context, context.AccessTokens) { }

    public async Task<AccessTokenDTO> GetByToken(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _entity
                .Where(x => x.Id == token && x.DeletedAt == null)
                .Select(x => new AccessTokenDTO(x))
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            return response;
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task<AccessToken?> GetAccessTokenAsync(string tokenId, CancellationToken cancellationToken = default)
    {
        return await _context.AccessTokens
            .Include(at => at.User)
            .FirstOrDefaultAsync(at => at.Id == tokenId && at.ExpiresAt > DateTimeOffset.UtcNow, cancellationToken);
    }
}
