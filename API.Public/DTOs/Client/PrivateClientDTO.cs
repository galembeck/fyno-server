using API.Public.DTOs._Base;
using ClientEntity = Domain.Data.Entities.Client;

namespace API.Public.DTOs.Client;

public class PrivateClientDTO : PrivateBaseDTO<ClientEntity>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PrimaryDocument { get; set; } = string.Empty;
    public string Cellphone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public PrivateClientDTO() : base(null) { }

    public PrivateClientDTO(ClientEntity o) : base(o)
    {
        if (o == null) return;

        Name = o.Name;
        Email = o.Email;
        PrimaryDocument = o.PrimaryDocument;
        Cellphone = o.Cellphone;
        Address = o.Address;
        UserId = o.UserId;
    }

    public static PrivateClientDTO? ModelToDTO(ClientEntity o)
    {
        return o == null ? null : new PrivateClientDTO(o);
    }

    public static List<PrivateClientDTO> ModelToDTO(IEnumerable<ClientEntity> clients) =>
        clients.Select(client => new PrivateClientDTO(client)).ToList();

    public static ClientEntity? DtoToModel(PrivateClientDTO o)
    {
        if (o == null) return null;

        var model = new ClientEntity()
        {
            Name = o.Name,
            Email = o.Email,
            PrimaryDocument = o.PrimaryDocument,
            Cellphone = o.Cellphone,
            Address = o.Address,
            UserId = o.UserId,
        };

        return o.InitializeInstance(model);
    }
}
