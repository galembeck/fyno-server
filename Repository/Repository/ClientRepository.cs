using Domain.Data.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository;

public class ClientRepository : BaseRepository<Client>, IClientRepository
{
    public ClientRepository(AppDbContext context) : base(context, context.Clients) { }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Clients.AnyAsync(
            c => c.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<Client>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Clients
            .Where(c => c.UserId == userId &&
                c.DeletedAt == null)
            .OrderByDescending(c => c.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Client?> GetForUpdateAsync(string clientId, CancellationToken cancellationToken = default)
    {
        return await _context.Clients
            .Where(c => c.Id == clientId &&
                c.DeletedAt == null)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
