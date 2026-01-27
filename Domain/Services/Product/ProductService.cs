using ProductEntity = Domain.Data.Entities.Product;
using Domain.Repository;
using Domain.Exceptions;
using Domain.Enumerators;

namespace Domain.Services.Product;

public class ProductService(
    IProductRepository repository,
    IProductRepository productRepository) : IProductService(repository)
{
    public override async Task<ProductEntity> CreateAsync(ProductEntity product, string actorId)
    {
        if (await productRepository.ExistsByIdOrNameAsync(product.Id, product.Name))
        {
            throw new BusinessException(BusinessErrorMessage.ALREADY_REGISTERED);
        }

        product.UserId = actorId;
        product.CreatedAt = DateTime.UtcNow;
        product.CreatedBy = actorId ?? Constants.Constants.Settings.SystemId;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy = actorId ?? Constants.Constants.Settings.SystemId;

        return await productRepository.InsertAsync(product);
    }

    public override async Task<IEnumerable<ProductEntity>> ListProductsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _Repository.GetByUserIdAsync(userId, cancellationToken);
    }

    public override async Task<ProductEntity> UpdateProductAsync(ProductEntity input, string productId, string actorId)
    {
        var existingProduct = await productRepository.GetForUpdateAsync(productId);

        if (existingProduct is null)
            throw new BusinessException(BusinessErrorMessage.NOT_FOUND);

        if (existingProduct.UserId != actorId)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(input.Name))
        {
            if (existingProduct.Name != input.Name)
            {
                existingProduct.Name = input.Name;
                hasChanges = true;
            }
        }

        if (!string.IsNullOrWhiteSpace(input.Description))
        {
            if (existingProduct.Description != input.Description)
            {
                existingProduct.Description = input.Description;
                hasChanges = true;
            }
        }

        if (input.Price > 0 && existingProduct.Price != input.Price)
        {
            existingProduct.Price = input.Price;
            hasChanges = true;
        }

        if (!hasChanges)
            return existingProduct;

        existingProduct.UpdatedAt = DateTimeOffset.UtcNow;
        existingProduct.UpdatedBy = actorId;

        await productRepository.SaveChangesAsync();

        return existingProduct;
    }

    public override async Task DeleteProductAsync(string productId, string actorId, CancellationToken cancellationToken = default)
    {
        var existingProduct = await productRepository.GetForUpdateAsync(productId, cancellationToken);

        if (existingProduct is null)
            throw new BusinessException(BusinessErrorMessage.NOT_FOUND);

        if (existingProduct.UserId != actorId)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        existingProduct.DeletedAt = DateTimeOffset.UtcNow;
        existingProduct.UpdatedAt = DateTimeOffset.UtcNow;
        existingProduct.UpdatedBy = actorId;

        await productRepository.SaveChangesAsync(cancellationToken);
    }
}
