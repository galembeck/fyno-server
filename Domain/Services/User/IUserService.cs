using Domain.Repository;
using Domain.SearchParameter;
using Domain.Services._Base;
using UserEntity = Domain.Data.Entities.User;

namespace Domain.Services.User;

public abstract class IUserService : IService<UserEntity, IUserRepository, UserSearchParameter>
{
    public IUserService(IUserRepository repository) : base(repository)
    {
    }

    public abstract Task<UserEntity> CreateAsync(UserEntity user);
    public abstract Task<UserEntity> UpdateUserAsync(UserEntity user, string userId);
    public abstract Task<UserEntity> GetUserAsync(string id, CancellationToken cancellationToken = default);
}
