using Domain.Data.Entities._Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBClient")]
public class Client : BaseEntity, IBaseEntity<Client>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PrimaryDocument { get; set; } = string.Empty;
    public string Cellphone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public User? User { get; set; }
    public string UserId { get; set; } = string.Empty;

    #region methods

    public Client WithoutRelations(Client entity)
    {
        if (entity == null)
            return null;

        var newEntity = new Client()
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            PrimaryDocument = entity.PrimaryDocument,
            Cellphone = entity.Cellphone,
            Address = entity.Address,
            UserId = entity.UserId,
        };
        newEntity.InitializeInstance(entity);
        return newEntity;
    }
    
    #endregion methods
}
