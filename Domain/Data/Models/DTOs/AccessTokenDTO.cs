using Domain.Data.Entities;
using Domain.Data.Models.DTOs._Base;

namespace Domain.Data.Models.DTOs;

public class AccessTokenDTO : BaseDTO
{
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public DateTimeOffset? ExpiresAt { get; set; }

    public AccessTokenDTO(string id, DateTimeOffset createdAt, DateTimeOffset? expiresAt)
    {
        Id = id;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
    }

    public AccessTokenDTO(AccessToken accessToken)
    {
        Id = accessToken.Id;
        CreatedAt = accessToken.CreatedAt;
        CreatedBy = accessToken.CreatedBy;
        UpdatedAt = accessToken.UpdatedAt;
        UpdatedBy = accessToken.UpdatedBy;
        DeletedAt = accessToken.DeletedAt;
        UserId = accessToken.UserId;
        User = accessToken.User;
        ExpiresAt = accessToken.ExpiresAt;
    }
}
