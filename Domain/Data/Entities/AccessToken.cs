using Domain.Data.Entities._Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBAccessToken")]
public class AccessToken : BaseEntity, IBaseEntity<AccessToken>
{
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public DateTimeOffset? ExpiresAt { get; set; }

    #region methods
    
    public AccessToken WithoutRelations(AccessToken entity)
    {
        if (entity == null)
            return null;

        var newEntity = new AccessToken()
        {
            UserId = entity.UserId,
            ExpiresAt = entity.ExpiresAt,
        };
        newEntity.InitializeInstance(entity);
        return newEntity;
    }
    
    #endregion methods
}
