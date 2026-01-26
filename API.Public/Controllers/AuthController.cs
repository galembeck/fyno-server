using API.Public.DTOs.Auth;
using API.Public.Filters;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Public.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : _BaseController
{
    private readonly IAuthService _authService;

    public AuthController(
        IAuthService authService,
        IHttpContextAccessor httpContextAccessor
        ) : base(httpContextAccessor)
    {
        _authService = authService ?? throw new ArgumentNullException();
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseTokensDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] AuthenticateDTO body)
    {
        var model = await _authService.AuthenticateAsync(body.Email, body.Password);
        
        return Ok(AuthResponseTokensDTO.ModelToDTO(model));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseTokensDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO body)
    {
        var model = await _authService.RefreshTokenAsync(body.RefreshToken);

        GenerateAuthCookie(model);

        return Ok(AuthResponseTokensDTO.ModelToDTO(model));
    }

    [AuthAttribute]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RevokeAccessToken()
    {
        var accessToken = Request.Cookies["accessToken"];
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        var model = await _authService.RevokeAccessTokenAsync(accessToken, refreshToken, Authenticated.User);

        GenerateAuthCookie(model);

        return NoContent();
    }
}
