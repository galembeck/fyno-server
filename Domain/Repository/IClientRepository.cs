using Domain.Data.Entities;
using Domain.Repository._Base;

namespace Domain.Repository;

public interface IClientRepository : IRepository<Client>
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Client>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Client?> GetForUpdateAsync(string clientId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
