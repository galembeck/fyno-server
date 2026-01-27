using UserEntity = Domain.Data.Entities.User;
using ProductEntity = Domain.Data.Entities.Product;
using Domain.Services._Base;
using Domain.Repository;
using Domain.SearchParameter;

namespace Domain.Services.Product;

public abstract class IProductService : IService<ProductEntity, IProductRepository, ProductSearchParameter>
{
    public IProductService(IProductRepository repository) : base(repository) { }

    public abstract Task<ProductEntity> CreateAsync(ProductEntity product, string actorId, CancellationToken cancellationToken = default);
    public abstract Task<IEnumerable<ProductEntity>> ListProductsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    public abstract Task<ProductEntity> UpdateProductAsync(ProductEntity input, string productId, string actorId);
    public abstract Task DeleteProductAsync(string productId, string actorId, CancellationToken cancellationToken = default);
}
