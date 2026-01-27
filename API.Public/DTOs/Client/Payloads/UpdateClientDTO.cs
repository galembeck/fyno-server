using API.Public.DTOs._Base;
using ClientEntity = Domain.Data.Entities.Client;

namespace API.Public.DTOs.Client.Payloads;

public class UpdateClientDTO : PublicBaseDTO<ClientEntity>
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PrimaryDocument { get; set; }
    public string? Cellphone { get; set; }
    public string? Address { get; set; }

    public UpdateClientDTO() : base(null) { }

    public static ClientEntity? DTOToModel(UpdateClientDTO? o)
    {
        if (o == null)
            return null;
        var model = new ClientEntity()
        {
            Name = o.Name,
            Email = o.Email,
            PrimaryDocument = o.PrimaryDocument,
            Cellphone = o.Cellphone,
            Address = o.Address
        };
        return o.InitializeInstance(model);
    }
}
