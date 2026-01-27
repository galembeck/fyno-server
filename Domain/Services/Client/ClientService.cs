using Domain.Constants;
using Domain.Data.Entities;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Repository;
using ClientEntity = Domain.Data.Entities.Client;

namespace Domain.Services.Client;

public class ClientService(
    IClientRepository repository,
    IClientRepository clientRepository) : IClientService(repository)
{
    public override async Task<ClientEntity> CreateAsync(ClientEntity client, string actorId, CancellationToken cancellationToken = default)
    {
        if (await clientRepository.ExistsByEmailAsync(client.Email))
        {
            throw new BusinessException(BusinessErrorMessage.ALREADY_REGISTERED);
        }

        client.Id = Guid.NewGuid().ToString();
        client.UserId = actorId;
        client.CreatedAt = DateTime.UtcNow;
        client.CreatedBy = actorId ?? Constants.Constants.Settings.SystemId;
        client.UpdatedAt = DateTime.UtcNow;
        client.UpdatedBy = actorId ?? Constants.Constants.Settings.SystemId;

        return await clientRepository.InsertAsync(client);
    }

    public override async Task<IEnumerable<ClientEntity>> ListClientsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _Repository.GetByUserIdAsync(userId, cancellationToken);
    }

    public override async Task<ClientEntity> UpdateClientAsync(ClientEntity input, string clientId, string actorId)
    {
        var existingClient = await clientRepository.GetForUpdateAsync(clientId);

        if (existingClient is null)
            throw new BusinessException(BusinessErrorMessage.NOT_FOUND);

        if (existingClient.UserId != actorId)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(input.Name))
        {
            if (existingClient.Name != input.Name)
            {
                existingClient.Name = input.Name;
                hasChanges = true;
            }
        }

        if (!string.IsNullOrWhiteSpace(input.Email))
        {
            if (existingClient.Email != input.Email)
            {
                existingClient.Email = input.Email;
                hasChanges = true;
            }
        }

        if (!string.IsNullOrWhiteSpace(input.PrimaryDocument))
        {
            if (existingClient.PrimaryDocument != input.PrimaryDocument)
            {
                existingClient.PrimaryDocument = input.PrimaryDocument;
                hasChanges = true;
            }
        }

        if (!string.IsNullOrWhiteSpace(input.Cellphone))
        {
            if (existingClient.Cellphone != input.Cellphone)
            {
                existingClient.Cellphone = input.Cellphone;
                hasChanges = true;
            }
        }

        if (!string.IsNullOrWhiteSpace(input.Address))
        {
            if (existingClient.Address != input.Address)
            {
                existingClient.Address = input.Address;
                hasChanges = true;
            }
        }

        if (!hasChanges)
            return existingClient;

        existingClient.UpdatedAt = DateTimeOffset.UtcNow;
        existingClient.UpdatedBy = actorId;

        await clientRepository.SaveChangesAsync();

        return existingClient;
    }

    public override async Task DeleteClientAsync(string clientId, string actorId, CancellationToken cancellationToken = default)
    {
        var existingClient = await clientRepository.GetForUpdateAsync(clientId, cancellationToken);

        if (existingClient is null)
            throw new BusinessException(BusinessErrorMessage.NOT_FOUND);

        if (existingClient.UserId != actorId)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        existingClient.DeletedAt = DateTimeOffset.UtcNow;
        existingClient.UpdatedAt = DateTimeOffset.UtcNow;
        existingClient.UpdatedBy = actorId;

        await clientRepository.SaveChangesAsync(cancellationToken);

    }
}
