using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vita_WebAPI_Services;


namespace Vita_WebAPI_IdentityAPI.Controllers;

[Route("users")]
public class ApiController : Controller
{
   private readonly Auth0Service _auth0Service;
    [HttpGet("private")]
    [Authorize]
    public IActionResult Private()
    {
        return Ok(new
        {
            Message = "Hello from a private endpoint!"
        });
    }

    [HttpGet("private-scoped")]
    [Authorize("read:messages")]
    public async Task Scoped()
    {
      await _auth0Service.Scoped(); 
    }

    [HttpGet("id")]
    public async Task GetUser(Guid id)
    {
        await _auth0Service.GetUser(id);
    }
}