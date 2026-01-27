using ClientEntity = Domain.Data.Entities.Client;
using Domain.Services._Base;
using Domain.Repository;
using Domain.SearchParameter;

namespace Domain.Services.Client;

public abstract class IClientService : IService<ClientEntity, IClientRepository, ClientSearchParameter>
{
    public IClientService(IClientRepository repository) : base(repository) { }

    public abstract Task<ClientEntity> CreateAsync(ClientEntity client, string actorId, CancellationToken cancellationToken = default);
    public abstract Task<IEnumerable<ClientEntity>> ListClientsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    public abstract Task<ClientEntity> UpdateClientAsync(ClientEntity input, string clientId, string actorId);
    public abstract Task DeleteClientAsync(string clientId, string actorId, CancellationToken cancellationToken = default);
}
