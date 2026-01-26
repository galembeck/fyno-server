using Domain.Data.Entities._Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBRefreshToken")]
public class RefreshToken : BaseEntity, IBaseEntity<RefreshToken>
{
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public DateTimeOffset? ExpiresAt { get; set; }

    #region methods

    public RefreshToken WithoutRelations(RefreshToken entity)
    {
        if (entity == null)
            return null;

        var newEntity = new RefreshToken()
        {
            UserId = entity.UserId,
            ExpiresAt = entity.ExpiresAt,
        };

        newEntity.InitializeInstance(entity);
        return newEntity;
    }

    #endregion methods
}
