using Domain.Services.Email;
using Domain.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Public.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : _BaseController
{
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;

    public EmailController(
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor
,
        IUserService userService) : base(httpContextAccessor)
    {
        _emailService = emailService ?? throw new ArgumentNullException();
        _userService = userService;
    }

    [HttpGet("already-registered")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
    {
        var exists = await _emailService.EmailExistsAsync(email);

        return Ok(new { exists });
    }

}
