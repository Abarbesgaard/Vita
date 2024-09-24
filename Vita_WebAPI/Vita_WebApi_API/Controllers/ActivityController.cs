using System.Net;
using System.Net.Http.Headers;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Vita_WebAPI_Services;

namespace Vita_WebApi_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivityController(IActivityService service, IWebHostEnvironment env) : ControllerBase
{
    private static readonly string[] Secret = ["secret1"];
    
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        if (!env.IsDevelopment())
        {
            var a = Request.Headers.TryGetValue("Authorization", out var token);
            if (a == false)
            {
                throw new Exception("Unauthorized - no token");
            }

            var tokenString = token.ToString();
            if (!tokenString.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized("Invalid token format");
            }

            tokenString = tokenString["Bearer ".Length..].Trim();

            var decodedString = JwtBuilder
                .Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Secret).MustVerifySignature().Decode<IDictionary<string, string>>(tokenString);
            var isValid = decodedString.TryGetValue("sub", out var sub);

            if (!isValid) return Unauthorized("Invalid token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            try
            {
                var response = await client.GetAsync($"https://localhost:5226/auth/getuser/{sub}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }

                if (response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "User is authorized");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseContent);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        try
        {
            var result = await service.GetAll();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}