using Domain.Data.Entities;
using Domain.Exceptions;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context, context.Users) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);

    }

    public async Task<User> GetUserAsync(string id, CancellationToken cancellationToken = default)
    {
        User response;

        try
        {
            response = await _entity
                .Where(x => x.Id == id && x.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }

        return response;
    }

    public async Task<User> UpdateUserWithOwnedEntitiesAsync(User user, string actorId = null)
    {
        try
        {
            var existingUser = await _entity
                .Where(e => e.Id == user.Id)
                .FirstOrDefaultAsync();

            if (existingUser == null)
                throw new PersistenceException($"User with ID {user.Id} not found.");

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Cellphone = user.Cellphone;
            existingUser.SupportCellphone = user.SupportCellphone;

            if (user.CompanyInformation != null)
            {
                if (existingUser.CompanyInformation == null)
                {
                    existingUser.CompanyInformation = user.CompanyInformation;
                }
                else
                {
                    existingUser.CompanyInformation.CompanyName = user.CompanyInformation.CompanyName;
                    existingUser.CompanyInformation.CompanyDocument = user.CompanyInformation.CompanyDocument;
                    existingUser.CompanyInformation.MonthlyRevenue = user.CompanyInformation.MonthlyRevenue;
                    existingUser.CompanyInformation.CompanyDomain = user.CompanyInformation.CompanyDomain;
                    existingUser.CompanyInformation.BusinessSegment = user.CompanyInformation.BusinessSegment;
                    existingUser.CompanyInformation.BusinessDescription = user.CompanyInformation.BusinessDescription;
                }
            }

            if (user.AddressInformation != null)
            {
                if (existingUser.AddressInformation == null)
                {
                    existingUser.AddressInformation = user.AddressInformation;
                }
                else
                {
                    existingUser.AddressInformation.Address = user.AddressInformation.Address;
                    existingUser.AddressInformation.Number = user.AddressInformation.Number;
                    existingUser.AddressInformation.Complement = user.AddressInformation.Complement;
                    existingUser.AddressInformation.Neighborhood = user.AddressInformation.Neighborhood;
                    existingUser.AddressInformation.Zipcode = user.AddressInformation.Zipcode;
                    existingUser.AddressInformation.State = user.AddressInformation.State;
                    existingUser.AddressInformation.City = user.AddressInformation.City;
                }
            }

            existingUser.UpdatedAt = DateTimeOffset.UtcNow;
            existingUser.UpdatedBy = actorId ?? Domain.Constants.Constants.Settings.SystemId;

            await _context.SaveChangesAsync();

            return existingUser;
        }
        catch (System.Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task InvalidateUserTokensAsync(string userId, CancellationToken cancellationToken = default)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.ExpiresAt > DateTimeOffset.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(-1);
            token.DeletedAt = DateTimeOffset.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
