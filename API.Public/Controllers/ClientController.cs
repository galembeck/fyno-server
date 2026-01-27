using API.Public.DTOs.Client;
using API.Public.DTOs.Client.Payloads;
using API.Public.Filters;
using API.Public.Validators.Client;
using Domain.Services.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Public.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController(IClientService clientService) : _BaseController
{
    private readonly IClientService _clientService = clientService ??
        throw new ArgumentNullException(nameof(clientService));

    [AuthAttribute]
    [HttpPost]
    [ProducesResponseType(typeof(PublicClientDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateNewClient([FromBody] PrivateClientDTO body, CancellationToken cancellationToken = default)
    {
        var userId = Authenticated.User.Id;

        var model = await _clientService.CreateAsync(PrivateClientDTO.DtoToModel(body)!, userId, cancellationToken);
        var response = PublicClientDTO.ModelToDTO(model);

        return CreatedAtAction(nameof(CreateNewClient), new { id = response.Id }, response);
    }

    [AuthAttribute]
    [HttpGet]
    [ProducesResponseType(typeof(PublicClientDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserClients(CancellationToken cancellationToken = default)
    {
        var userId = Authenticated.User.Id;

        var clients = await _clientService.ListClientsByUserIdAsync(userId, cancellationToken);
        var response = PublicClientDTO.ModelToDTO(clients);

        return Ok(response);
    }

    [AuthAttribute]
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(PublicClientDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetClientByUserId(string userId, CancellationToken cancellationToken = default)
    {
        var clients = await _clientService.ListClientsByUserIdAsync(userId, cancellationToken);
        var response = PublicClientDTO.ModelToDTO(clients);

        return Ok(response);
    }

    [AuthAttribute]
    [HttpPatch("{clientId}")]
    [ProducesResponseType(typeof(PublicClientDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateClientInformation(string clientId, [FromBody] UpdateClientDTO body, CancellationToken cancellationToken = default)
    {
        await new ClientUpdateValidator().ValidateAndThrowAsync(body);

        var userId = Authenticated.User.Id;

        var clientModel = UpdateClientDTO.DTOToModel(body);
        if (clientModel is null)
        {
            return BadRequest("INVALID_REQUEST_BODY");
        }

        var model = await _clientService.UpdateClientAsync(clientModel, clientId, userId);

        return Ok(PublicClientDTO.ModelToDTO(model));
    }

    [AuthAttribute]
    [HttpDelete("{clientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteClient(string clientId, CancellationToken cancellationToken = default)
    {
        var userId = Authenticated.User.Id;

        await _clientService.DeleteClientAsync(clientId, userId, cancellationToken);

        return Ok();
    }
}
