using Domain.Data.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context, context.Products) { }

    public async Task<bool> ExistsByIdOrNameAsync(
        string productId,
        string productName,
        CancellationToken cancellationToken = default
    ) {
        return await _context.Products.AnyAsync(
            p => p.Id == productId || p.Name == productName,
            cancellationToken
        );
    }

    public async Task<IEnumerable<Product>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Where(p => p.UserId == userId && p.DeletedAt == null)
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetForUpdateAsync(string productId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Where(p => p.Id == productId && p.DeletedAt == null)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
