using System.ComponentModel.DataAnnotations;

namespace Domain.Data.Entities._Base;

public static class BaseEntityExtension
{
    public static BaseEntity InitializeInstance(this BaseEntity current, BaseEntity i)
    {
        current.Id = i.Id;

        current.CreatedBy = i.CreatedBy;
        current.UpdatedBy = i.UpdatedBy;
        current.CreatedAt = i.CreatedAt;
        current.UpdatedAt = i.UpdatedAt;
        current.DeletedAt = i.DeletedAt;

        return current;
    }
}

public class BaseEntity
{
    [Key]
    public string Id { get; set; }

    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
