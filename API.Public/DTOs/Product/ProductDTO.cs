using API.Public.DTOs._Base;
using ProductEntity = Domain.Data.Entities.Product;

namespace API.Public.DTOs.Product;

public class ProductDTO : PublicBaseDTO<ProductEntity>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get;set; }

    public ProductDTO() : base(null) { }

    public ProductDTO(ProductEntity o) : base(o)
    {
        if (o == null) return;

        Id = o.Id;
        Name = o.Name;
        Description = o.Description;
        Price = o.Price;
        UserId = o.UserId;
        CreatedAt = o.CreatedAt;
        UpdatedAt = o.UpdatedAt;
    }

    public static ProductDTO? ModelToDTO(ProductEntity o)
    {
        return o == null ? null : new ProductDTO(o);
    }

    public static List<ProductDTO> ModelToDTO(IEnumerable<ProductEntity> products) =>
        products.Select(product => new ProductDTO(product)).ToList();

    public static ProductEntity? DtoToModel(ProductDTO o)
    {
        if (o == null) return null;

        var model = new ProductEntity()
        {
            Id = o.Id,
            Name = o.Name,
            Description = o.Description,
            Price = o.Price,
            UserId = o.UserId,
        };

        return o.InitializeInstance(model);
    }
}
