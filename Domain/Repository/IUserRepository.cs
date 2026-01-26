using Domain.Data.Entities;
using Domain.Repository._Base;

namespace Domain.Repository;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email);
    Task<User> GetUserAsync(string id, CancellationToken cancellationToken = default);
    Task<User> UpdateUserWithOwnedEntitiesAsync(User user, string actorId = null);

    Task InvalidateUserTokensAsync(string userId, CancellationToken cancellationToken = default);
}
