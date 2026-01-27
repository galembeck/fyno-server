using API.Public.DTOs._Base;
using ProductEntity = Domain.Data.Entities.Product;

namespace API.Public.DTOs.Product.Payloads;

public class UpdateProductDTO : PublicBaseDTO<ProductEntity>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }

    public UpdateProductDTO() : base(null) { }

    public static ProductEntity? DTOToModel(UpdateProductDTO? o)
    {
        if (o == null)
            return null;

        var model = new ProductEntity()
        {
            Name = o.Name,
            Description = o.Description,
            Price = o.Price ?? 0
        };

        return o.InitializeInstance(model);
    }
}
