using Microsoft.AspNetCore.Mvc;
using Vita_WebAPI_Services;


namespace Vita_WebAPI_IdentityAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    private readonly Auth0Service _auth0Service = new();

    [HttpPost("add-user")]
    public async Task AddUser()
    {
      await _auth0Service.AddUser(); 
    }

    [HttpGet("id")]
    public async Task GetUser(string id)
    {
        await _auth0Service.GetUser(id);
    }
}