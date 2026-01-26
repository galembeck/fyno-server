using UserEntity = Domain.Data.Entities.User;
using Domain.Repository;
using Domain.Services.User;
using BCrypt.Net;
using Domain.Exceptions;
using Domain.Enumerators;
using Domain.Data.Models;

namespace Domain.Services.User;

public class UserService(
    IUserRepository repository,
    IUserRepository userRepository) : IUserService(repository)
{
    public override async Task<UserEntity> CreateAsync(UserEntity user)
    {
        if (await userRepository.ExistsByEmailAsync(user.Email))
        {
            throw new Exception("EMAIL_ALREADY_REGISTERED");
        }

        user.Id = Guid.NewGuid().ToString();
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        user.CreatedAt = DateTimeOffset.UtcNow;
        user.LastAccessAt = DateTimeOffset.UtcNow;

        return await userRepository.InsertAsync(user);
    }

    public override async Task<UserEntity> UpdateUserAsync(UserEntity input, string userId)
    {
        var existingUser = await _Repository.GetUserAsync(userId);

        if (existingUser is null)
            throw new BusinessException(BusinessErrorMessage.USER_NOT_FOUND);

        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(input.Name))
            hasChanges |= SetIfDifferent(existingUser, input, u => u.Name, v => existingUser.Name = v);

        if (!string.IsNullOrWhiteSpace(input.Email))
            hasChanges |= SetIfDifferent(existingUser, input, u => u.Email, v => existingUser.Email = v);

        if (!string.IsNullOrWhiteSpace(input.Cellphone))
            hasChanges |= SetIfDifferent(existingUser, input, u => u.Cellphone, v => existingUser.Cellphone = v);

        if (!string.IsNullOrWhiteSpace(input.SupportCellphone))
            hasChanges |= SetIfDifferent(existingUser, input, u => u.SupportCellphone, v => existingUser.SupportCellphone = v);

        if (input.CompanyInformation != null)
        {
            if (existingUser.CompanyInformation == null)
            {
                existingUser.CompanyInformation = new UserCompanyInformation();
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(input.CompanyInformation.CompanyName))
                hasChanges |= SetIfDifferent(existingUser.CompanyInformation, input.CompanyInformation,
                    x => x.CompanyName, v => existingUser.CompanyInformation.CompanyName = v);

            if (!string.IsNullOrWhiteSpace(input.CompanyInformation.CompanyDocument))
                hasChanges |= SetIfDifferent(existingUser.CompanyInformation, input.CompanyInformation,
                    x => x.CompanyDocument, v => existingUser.CompanyInformation.CompanyDocument = v);

            if (!string.IsNullOrWhiteSpace(input.CompanyInformation.CompanyDomain))
                hasChanges |= SetIfDifferent(existingUser.CompanyInformation, input.CompanyInformation,
                    x => x.CompanyDomain, v => existingUser.CompanyInformation.CompanyDomain = v);

            if (input.CompanyInformation.BusinessDescription != null)
                hasChanges |= SetIfDifferent(existingUser.CompanyInformation, input.CompanyInformation,
                    x => x.BusinessDescription, v => existingUser.CompanyInformation.BusinessDescription = v);

            if (input.CompanyInformation.MonthlyRevenue != default)
                hasChanges |= SetIfDifferent(existingUser.CompanyInformation, input.CompanyInformation,
                    x => x.MonthlyRevenue, v => existingUser.CompanyInformation.MonthlyRevenue = v);

            if (input.CompanyInformation.BusinessSegment != default)
                hasChanges |= SetIfDifferent(existingUser.CompanyInformation, input.CompanyInformation,
                    x => x.BusinessSegment, v => existingUser.CompanyInformation.BusinessSegment = v);
        }

        if (input.AddressInformation != null)
        {
            if (existingUser.AddressInformation == null)
            {
                existingUser.AddressInformation = new UserAddressInformation();
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(input.AddressInformation.Address))
                hasChanges |= SetIfDifferent(existingUser.AddressInformation, input.AddressInformation,
                    x => x.Address, v => existingUser.AddressInformation.Address = v);

            if (!string.IsNullOrWhiteSpace(input.AddressInformation.Number))
                hasChanges |= SetIfDifferent(existingUser.AddressInformation, input.AddressInformation,
                    x => x.Number, v => existingUser.AddressInformation.Number = v);

            if (!string.IsNullOrWhiteSpace(input.AddressInformation.Zipcode))
                hasChanges |= SetIfDifferent(existingUser.AddressInformation, input.AddressInformation,
                    x => x.Zipcode, v => existingUser.AddressInformation.Zipcode = v);

            if (!string.IsNullOrWhiteSpace(input.AddressInformation.Neighborhood))
                hasChanges |= SetIfDifferent(existingUser.AddressInformation, input.AddressInformation,
                    x => x.Neighborhood, v => existingUser.AddressInformation.Neighborhood = v);

            if (input.AddressInformation.Complement != null)
                hasChanges |= SetIfDifferent(existingUser.AddressInformation, input.AddressInformation,
                    x => x.Complement, v => existingUser.AddressInformation.Complement = v);

            if (!string.IsNullOrWhiteSpace(input.AddressInformation.State))
                hasChanges |= SetIfDifferent(existingUser.AddressInformation, input.AddressInformation,
                    x => x.State, v => existingUser.AddressInformation.State = v);

            if (!string.IsNullOrWhiteSpace(input.AddressInformation.City))
                hasChanges |= SetIfDifferent(existingUser.AddressInformation, input.AddressInformation,
                    x => x.City, v => existingUser.AddressInformation.City = v);
        }

        if (!hasChanges)
            return existingUser;

        existingUser.UpdatedAt = DateTimeOffset.UtcNow;

        return await _Repository.UpdateUserWithOwnedEntitiesAsync(existingUser, userId);
    }

    public override async Task<UserEntity> GetUserAsync(string id, CancellationToken cancellationToken = default)
    {
        var response = await _Repository.GetUserAsync(id, cancellationToken);

        if (response is null)
            throw new BusinessException(BusinessErrorMessage.USER_NOT_FOUND);

        return response;
    }



    private bool SetIfDifferent<TObj, TProp>(
        TObj current,
        TObj updated,
        Func<TObj, TProp> getter,
        Action<TProp> setter
    )
    {
        var currentValue = getter(current);
        var newValue = getter(updated);

        if (!EqualityComparer<TProp>.Default.Equals(currentValue, newValue))
        {
            setter(newValue);
            return true;
        }

        return false;
    }

}