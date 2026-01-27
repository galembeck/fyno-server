using API.Public.DTOs._Base;
using Domain.Data.Models;
using UserEntity = Domain.Data.Entities.User;

namespace API.Public.DTOs.User;

public class PublicUserDTO : PublicBaseDTO<UserEntity>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cellphone { get; set; } = string.Empty;
    public string SupportCellphone { get; set; } = string.Empty;
    public UserCompanyInformation CompanyInformation { get; set; } = new UserCompanyInformation();
    public UserAddressInformation AddressInformation { get; set; } = new UserAddressInformation();
    public DateTimeOffset? LastAccessAt { get; set; }

    public PublicUserDTO(UserEntity o) : base(o)
    {
        if (o == null) return;

        Id = o.Id;
        Name = o.Name;
        Email = o.Email;
        Cellphone = o.Cellphone;
        SupportCellphone = o.SupportCellphone;
        CompanyInformation = o.CompanyInformation ?? new UserCompanyInformation();
        AddressInformation = o.AddressInformation ?? new UserAddressInformation();
        LastAccessAt = o.LastAccessAt;
    }

    public static PublicUserDTO? ModelToDTO(UserEntity o)
    {
        return o == null ? null : new PublicUserDTO(o);
    }

    public static List<PublicUserDTO> ModelToDTO(IEnumerable<UserEntity> users) => 
        users.Select(user => new PublicUserDTO(user)).ToList();

    public static UserEntity? DtoToModel(PublicUserDTO o)
    {
        if (o == null) return null;

        var model = new UserEntity()
        {
            Name = o.Name,
            Email = o.Email,
            Cellphone = o.Cellphone,
            SupportCellphone = o.SupportCellphone,
            CompanyInformation = o.CompanyInformation,
            AddressInformation = o.AddressInformation,
            LastAccessAt = o.LastAccessAt
        };

        return o.InitializeInstance(model);
    }
}
