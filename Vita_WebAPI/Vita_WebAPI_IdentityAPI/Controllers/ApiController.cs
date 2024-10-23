using Microsoft.AspNetCore.Mvc;
using Vita_WebAPI_Services;

namespace Vita_WebAPI_IdentityAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly Auth0Service _auth0Service;

    public AuthController(Auth0Service auth0Service)
    {
        _auth0Service = auth0Service;
    }

    [HttpPost("add-user")]
    public async Task<IActionResult> AddUser()
    {
        var bearerToken = HttpContext
            .Request.Headers.Authorization.ToString()
            .Replace("Bearer ", string.Empty);
        await _auth0Service.AddUser(bearerToken);
        return Ok();
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetUser(string id)
    {
        var bearerToken = HttpContext
            .Request.Headers.Authorization.ToString()
            .Replace("Bearer ", string.Empty);
        await _auth0Service.GetUser(id, bearerToken);
        return Ok();
    }
}
