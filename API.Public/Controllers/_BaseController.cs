using API.Public.Resources;
using Domain.Constants;
using Domain.Data.Models.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace API.Public.Controllers;

[ApiController]
[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
public class _BaseController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public _BaseController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public _BaseController() { }

    public static IdentityPrincipal Authenticated
        => (IdentityPrincipal)Thread.CurrentPrincipal;

    protected void GenerateAuthCookie(Domain.Data.Models.Tokens model)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = model.AccessTokenExpiresAt,
            HttpOnly = true,
            Secure = true,
            Domain = Constants.Settings.Domain
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Append(
            "accessToken",
            model.AccessToken,
            cookieOptions
        );

        cookieOptions.Expires = model.RefreshTokenExpiresAt;

        _httpContextAccessor.HttpContext.Response.Cookies.Append(
            "refreshToken",
            model.RefreshToken,
            cookieOptions
        );
    }

    protected void RemoveAuthCookie(Domain.Data.Models.Tokens model)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("accessToken");
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
    }
}
