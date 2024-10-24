using System.Net;
using System.Net.Http.Headers;
using AutoMapper;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using Vita_WebApi_API.Dto;
using Vita_WebAPI_Services;
using Vita_WebApi_Shared;

namespace Vita_WebApi_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivityController(
    IGenericService<Activity> service,
    ILogger<ActivityController> logger, 
    IWebHostEnvironment env,
    IMapper? mapper) 
    : ControllerBase
{
    private static readonly string[] Secret = ["secret1"];
    
    [HttpGet("GetAll")]
    public async Task<IActionResult?> GetAllAsync()
    {
        if (!env.IsDevelopment())
        {
            var tokenValidationResult = ValidateToken();
            return await tokenValidationResult; // Return unauthorized response if any issue occurs
        } 
        try
        {
            var activities = await service
                .GetAllAsync();
            var activityDtos = mapper?
                .Map<List<ActivityDto>>(activities);
            if (activityDtos is null)
            {
                logger.LogWarning("No activities found");
                return NotFound();
            }
            
            return Ok(activityDtos);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("Get/{id:guid}")]
    public async Task<ActionResult<ActivityDto>> GetActivityByIdAsync(Guid id)
    {
        try
        {
            var activity = await service.GetByIdAsync(id);
            var activityDto = mapper?.Map<ActivityDto>(activity);
            return Ok(activityDto);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("Create")]
    public async Task<ActionResult<ActivityDto>> CreateActivityAsync([FromBody] ActivityDto? activityDto)
    {
        if (activityDto is null)
        {
            logger.LogWarning("ActivityDto is null");
            return BadRequest();
        }

        // Map the ActivityDto to an Activity entity
        var activity = mapper?.Map<Activity>(activityDto);
        if (activity is null)
        {
            logger.LogWarning("Activity not created");
            return BadRequest();
        }

        // Create the Activity entity using the service
        await service.CreateAsync(activity)!;

        // Map the newly created Activity entity back to ActivityDto
        var createdActivityDto = mapper?.Map<ActivityDto>(activity);
        if (createdActivityDto is null)
        {
            logger.LogWarning("ActivityDto could not be mapped from Activity");
            return StatusCode(500, "Internal server error during mapping.");
        }

        // Return the CreatedAtAction response, pointing to the new Activity by ID
        return CreatedAtAction(nameof(GetActivityByIdAsync), new { id = createdActivityDto.Id }, createdActivityDto);
    }
   
    [HttpPut("Update/{id:guid}")]
    public async Task<ActionResult<ActivityDto>> UpdateActivityAsync(Guid id, [FromBody] ActivityDto? activityDto)
    {
        if (activityDto is null)
        {
            logger.LogWarning("ActivityDto is null");
            return BadRequest();
        }

        // Map the ActivityDto to an Activity entity
        var activity = mapper?.Map<Activity>(activityDto);
        if (activity is null)
        {
            logger.LogWarning("Activity not created");
            return BadRequest();
        }

        // Update the Activity entity using the service
        await service.UpdateAsync(id, activity);

        // Map the updated Activity entity back to ActivityDto
        var updatedActivityDto = mapper?.Map<ActivityDto>(activity);
        if (updatedActivityDto is null)
        {
            logger.LogWarning("ActivityDto could not be mapped from Activity");
            return StatusCode(500, "Internal server error during mapping.");
        }

        // Return the CreatedAtAction response, pointing to the updated Activity by ID
        return CreatedAtAction(nameof(GetActivityByIdAsync), new { id = updatedActivityDto.Id }, updatedActivityDto);
    }
    
    
    private async Task<IActionResult?> ValidateToken()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            return Unauthorized("Unauthorized - no token");
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
            .WithSecret(Secret)
            .MustVerifySignature()
            .Decode<IDictionary<string, string>>(tokenString);

        if (!decodedString.TryGetValue("sub", out var sub))
        {
            return Unauthorized("Invalid token");
        }

        // Call your external user validation service
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

        try
        {
            var response = await client.GetAsync($"https://localhost:5226/auth/getuser/{sub}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                logger.LogWarning($"User validation failed: {responseContent}");
                return Unauthorized("User validation failed");
            }

            return null; // Valid token and user
        }
        catch (HttpRequestException ex)
        {
            logger.LogError($"Request error: {ex.Message}");
            return StatusCode(500, $"{ex.Message}");
        }
    }  
}