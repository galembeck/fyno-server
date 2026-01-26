using API.Public.DTOs._Base;
using Domain.Data.Models;
using UserEntity = Domain.Data.Entities.User;

namespace API.Public.DTOs.User.Payloads;

public class UpdateUserDTO : PublicBaseDTO<UserEntity>
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Cellphone { get; set; }
    public string? SupportCellphone { get; set; }
    public UserCompanyInformation? CompanyInformation { get; set; }
    public UserAddressInformation? AddressInformation { get; set; }

    public static UserEntity DTOToModel(UpdateUserDTO o)
    {
        if (o == null)
            return null;

        var model = new UserEntity()
        {
            Name = o.Name,
            Email = o.Email,
            Cellphone = o.Cellphone,
            SupportCellphone = o.SupportCellphone,
            CompanyInformation = o.CompanyInformation,
            AddressInformation = o.AddressInformation
        };

        return o.InitializeInstance(model);
    }
}
