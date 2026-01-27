using Domain.Data.Entities;
using Domain.Repository._Base;

namespace Domain.Repository;

public interface IProductRepository : IRepository<Product>
{
    Task<bool> ExistsByIdOrNameAsync(string productId, string productName, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Product?> GetForUpdateAsync(string productId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
