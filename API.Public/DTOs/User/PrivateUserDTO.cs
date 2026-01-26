using API.Public.DTOs._Base;
using UserEntity = Domain.Data.Entities.User;
using Domain.Data.Models;
using System.Globalization;

namespace API.Public.DTOs.User;

public class PrivateUserDTO : PrivateBaseDTO<UserEntity>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Cellphone { get; set; }
    public string SupportCellphone { get; set; }
    public string Password { get; set; }
    public UserCompanyDTO CompanyInformation { get; set; }
    public UserAddressDTO AddressInformation { get; set; }

    public PrivateUserDTO(UserEntity o) : base(o)
    {
        if (o == null)
            throw new ArgumentNullException(nameof(o));

        Name = o.Name;
        Email = o.Email;
        Cellphone = o.Cellphone;
        SupportCellphone = o.SupportCellphone;
        Password = o.Password;
        CompanyInformation = o.CompanyInformation != null
            ? new UserCompanyDTO
            {
                CompanyName = o.CompanyInformation.CompanyName ?? string.Empty,
                CompanyDocument = o.CompanyInformation.CompanyDocument ?? string.Empty,
                MonthlyRevenue = o.CompanyInformation.MonthlyRevenue,
                CompanyDomain = o.CompanyInformation.CompanyDomain ?? string.Empty,
                BusinessSegment = o.CompanyInformation.BusinessSegment,
                BusinessDescription = o.CompanyInformation.BusinessDescription
            }
            : null!;
        AddressInformation = o.AddressInformation != null
            ? new UserAddressDTO
            {
                Address = o.AddressInformation.Address ?? string.Empty,
                Number = o.AddressInformation.Number ?? string.Empty,
                Complement = o.AddressInformation.Complement ?? string.Empty,
                Neighborhood = o.AddressInformation.Neighborhood ?? string.Empty,
                Zipcode = o.AddressInformation.Zipcode ?? string.Empty,
                State = o.AddressInformation.State ?? string.Empty,
                City = o.AddressInformation.City ?? string.Empty,
            }
            : null!;
    }

    public PrivateUserDTO()
    {
        Name = string.Empty;
        Email = string.Empty;
        Cellphone = string.Empty;
        SupportCellphone = string.Empty;
        Password = string.Empty;
        CompanyInformation = new UserCompanyDTO();
        AddressInformation = new UserAddressDTO();
    }

    public static PrivateUserDTO? ModelToDto(UserEntity o) => o == null ? null : new PrivateUserDTO(o);

    public static UserEntity? DtoToModel(PrivateUserDTO o)
    {
        if (o == null)
            return null;

        var model = new UserEntity
        {
            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(o.Name?.ToLower().Trim() ?? string.Empty),
            Email = o.Email.ToLower().Trim(),
            Cellphone = o.Cellphone,
            SupportCellphone = o.SupportCellphone,
            Password = (o.Password ?? string.Empty).Trim(),
            CompanyInformation = o.CompanyInformation != null ? new UserCompanyInformation
            {
                CompanyName = o.CompanyInformation.CompanyName?.Trim() ?? string.Empty,
                CompanyDocument = o.CompanyInformation.CompanyDocument?.Trim() ?? string.Empty,
                MonthlyRevenue = o.CompanyInformation.MonthlyRevenue,
                CompanyDomain = o.CompanyInformation.CompanyDomain?.Trim() ?? string.Empty,
                BusinessSegment = o.CompanyInformation.BusinessSegment,
                BusinessDescription = o.CompanyInformation.BusinessDescription?.Trim()
            } : null,
            AddressInformation = o.AddressInformation != null ? new UserAddressInformation
            {
                Address = o.AddressInformation.Address?.Trim() ?? string.Empty,
                Number = o.AddressInformation.Number?.Trim() ?? string.Empty,
                Complement = o.AddressInformation.Complement?.Trim(),
                Neighborhood = o.AddressInformation.Neighborhood?.Trim() ?? string.Empty,
                Zipcode = o.AddressInformation.Zipcode?.Trim() ?? string.Empty,
                State = o.AddressInformation.State?.Trim() ?? string.Empty,
                City = o.AddressInformation.City?.Trim() ?? string.Empty,
            } : null
        };

        return o.InitializeInstance(model);
    }

    public static List<PrivateUserDTO>? UserListToPrivateUserDTOList(List<UserEntity> userList)
    {
        if (userList is null || userList.Count == 0)
            return null;

        List<PrivateUserDTO> privateUserList = new();

        foreach (UserEntity user in userList)
        {
            if (user != null)
            {
                var dto = ModelToDto(user);
                if (dto != null)
                    privateUserList.Add(dto);
            }
        }

        return privateUserList;
    }

    public static PrivateUserDTO? ModelToDTOWithoutBase(UserEntity o)
    {
        if (o == null) return null;

        var dto = new PrivateUserDTO
        {
            Name = o.Name,
            Email = o.Email,
            Cellphone = o.Cellphone,
            SupportCellphone = o.SupportCellphone,
            Password = o.Password,
            CompanyInformation = o.CompanyInformation != null ? new UserCompanyDTO
            {
                CompanyName = o.CompanyInformation.CompanyName ?? string.Empty,
                CompanyDocument = o.CompanyInformation.CompanyDocument ?? string.Empty,
                MonthlyRevenue = o.CompanyInformation.MonthlyRevenue,
                CompanyDomain = o.CompanyInformation.CompanyDomain ?? string.Empty,
                BusinessSegment = o.CompanyInformation.BusinessSegment,
                BusinessDescription = o.CompanyInformation.BusinessDescription
            } : null!,
            AddressInformation = o.AddressInformation != null ? new UserAddressDTO
            {
                Address = o.AddressInformation.Address ?? string.Empty,
                Number = o.AddressInformation.Number ?? string.Empty,
                Complement = o.AddressInformation.Complement ?? string.Empty,
                Neighborhood = o.AddressInformation.Neighborhood ?? string.Empty,
                Zipcode = o.AddressInformation.Zipcode ?? string.Empty,
                State = o.AddressInformation.State ?? string.Empty,
                City = o.AddressInformation.City ?? string.Empty,
            } : null!,
        };

        return dto;
    }
}
