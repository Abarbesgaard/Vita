using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vita_WebAPI_Services;


namespace Vita_WebAPI_IdentityAPI.Controllers;

[Route("api")]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly Auth0Service _auth0Service = new();

    [HttpPost("add-user")]
    [Authorize("read:messages")]
    public async Task AddUser()
    {
      await _auth0Service.AddUser(); 
    }

    [HttpGet("id")]
    public async Task GetUser(Guid id)
    {
        await _auth0Service.GetUser(id);
    }
}