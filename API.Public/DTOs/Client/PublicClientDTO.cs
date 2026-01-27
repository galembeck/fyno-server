using API.Public.DTOs._Base;
using ClientEntity = Domain.Data.Entities.Client;

namespace API.Public.DTOs.Client;

public class PublicClientDTO : PublicBaseDTO<ClientEntity>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PrimaryDocument { get; set; } = string.Empty;
    public string Cellphone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public PublicClientDTO() : base(null) { }

    public PublicClientDTO(ClientEntity o) : base(o)
    {
        if (o == null) return;

        Id = o.Id;
        Name = o.Name;
        Email = o.Email;
        PrimaryDocument = o.PrimaryDocument;
        Cellphone = o.Cellphone;
        Address = o.Address;
        CreatedAt = o.CreatedAt;
        UpdatedAt = o.UpdatedAt;
    }

    public static PublicClientDTO? ModelToDTO(ClientEntity o)
    {
        return o == null ? null : new PublicClientDTO(o);
    }

    public static List<PublicClientDTO> ModelToDTO(IEnumerable<ClientEntity> clients) =>
        clients.Select(client => new PublicClientDTO(client)).ToList();
}
