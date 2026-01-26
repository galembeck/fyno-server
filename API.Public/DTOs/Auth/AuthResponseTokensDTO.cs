using Domain.Data.Models;

namespace API.Public.DTOs.Auth;

public class AuthResponseTokensDTO
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;

    public AuthResponseTokensDTO(Tokens o)
    {
        if (o is null)
            return;

        AccessToken = o.AccessToken;
        RefreshToken = o.RefreshToken;
    }

    public static AuthResponseTokensDTO? ModelToDTO(Tokens o)
    {
        return o is null ? null : new AuthResponseTokensDTO(o);
    }
}
