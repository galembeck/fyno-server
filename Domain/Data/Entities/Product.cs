using Domain.Data.Entities._Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBProduct")]
public class Product : BaseEntity, IBaseEntity<Product>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }

    #region methods

    public Product WithoutRelations(Product entity)
    {
        if (entity == null)
            return null;

        var newEntity = new Product()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            UserId = entity.UserId,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion methods
}
