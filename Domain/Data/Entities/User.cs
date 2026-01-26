using Domain.Data.Entities._Base;
using Domain.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBUser")]
public class User : BaseEntity, IBaseEntity<User>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cellphone { get; set; } = string.Empty;
    public string SupportCellphone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public UserCompanyInformation? CompanyInformation { get; set; }
    public UserAddressInformation? AddressInformation { get; set; }

    public DateTimeOffset? LastAccessAt { get; set; }


    #region methods

    public User WithoutRelations(User entity)
    {
        if (entity == null)
        {
            #pragma warning disable CS8603 // Possible null reference return.
            return null;
            #pragma warning restore CS8603 // Possible null reference return.
        }

        var newEntity = new User()
        {
            Name = entity.Name,
            Email = entity.Email,
            Cellphone = entity.Cellphone,
            SupportCellphone = entity.SupportCellphone,
            Password = entity.Password,
            CompanyInformation = entity.CompanyInformation,
            AddressInformation = entity.AddressInformation,
            LastAccessAt = entity.LastAccessAt,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion methods
}
