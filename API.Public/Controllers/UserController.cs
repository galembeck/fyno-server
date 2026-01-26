using API.Public.DTOs.User;
using API.Public.DTOs.User.Payloads;
using API.Public.Filters;
using API.Public.Validators.User;
using Domain.Data.Entities;
using Domain.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Public.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : _BaseController
{
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PublicUserDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] PrivateUserDTO body)
    {
        var model = await _userService.CreateAsync(PrivateUserDTO.DtoToModel(body)!);
        var response = PublicUserDTO.ModelToDTO(model);

        return CreatedAtAction(nameof(Register), new { id = response.Id }, response);
    }

    [AuthAttribute]
    [HttpPatch]
    [ProducesResponseType(typeof(PublicUserDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUserInformation([FromBody] UpdateUserDTO body, CancellationToken cancellationToken = default)
    {
        await new UserUpdateValidator().ValidateAndThrowAsync(body);

        var user = await _userService.GetByIdAsync(Authenticated.User.Id, cancellationToken);
        var model = await _userService.UpdateUserAsync(UpdateUserDTO.DTOToModel(body), Authenticated.User.Id);

        return Ok(PublicUserDTO.ModelToDTO(model));
    }

    [AuthAttribute]
    [HttpGet("me")]
    [ProducesResponseType(typeof(PublicUserDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserInformation(CancellationToken cancellationToken = default)
    { 
        User response = await _userService.GetUserAsync(Authenticated.User.Id, cancellationToken);

        return Ok(PublicUserDTO.ModelToDTO(response));
    }
}
