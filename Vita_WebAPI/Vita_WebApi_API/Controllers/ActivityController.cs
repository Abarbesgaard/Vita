using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
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

		var tokenValidationResult = ValidateToken();
		if (tokenValidationResult.Result != null)
		{
			return await tokenValidationResult; 
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
	public async Task<IActionResult?> GetActivityByIdAsync(Guid id)
	{
		var tokenValidationResult = ValidateToken();
		if (tokenValidationResult.Result != null)
		{
			return await tokenValidationResult; 
		}	
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
	public async Task<IActionResult?> CreateActivityAsync([FromBody] ActivityDto? activityDto)
	{
		var tokenValidationResult = ValidateToken();
		if (tokenValidationResult.Result != null)
		{
			return await tokenValidationResult; 
		}	
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
	public async Task<IActionResult?> UpdateActivityAsync(Guid id, [FromBody] ActivityDto? activityDto)
	{
		var tokenValidationResult = ValidateToken();
		if (tokenValidationResult.Result != null)
		{
			return await tokenValidationResult; 
		}		
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
		
		// Check for Authorization header
		if (!Request.Headers.ContainsKey("Authorization"))
		{
			logger.LogWarning("No Authorization header present");
			return Unauthorized("No Authorization header present");
		}

		var token = Request.Headers.Authorization;
		if (!token.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
		{
			logger.LogWarning("Invalid token format");
			return Unauthorized("Invalid token format");
		}
		var tokenString = token.ToString()["Bearer ".Length..].Trim();
		logger.LogInformation($"tokenString received: {tokenString}");
		var tokenHandler = new JwtSecurityTokenHandler();
		var client = new HttpClient();
		
		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidAudience = "authenticated",
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = "https://vhomzkchzmeaxpjfjmvd.supabase.co/auth/v1",
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Jc4ZvGS+REA18iW9pKsEDGEOXKTF5EGrttm7aTgKvQRGKL/EcI60b3PvdbMdNYnQWLDbxoqYy0uDkvh+/E4Gew=="))
		};

		try
		{
			var claimsPrincipal = tokenHandler.ValidateToken(tokenString, tokenValidationParameters, out _);

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

			return null;
		}
		catch (SecurityTokenException ex)
		{
			logger.LogError($"Token validation failed: {ex.Message}");
			return Unauthorized("Invalid token");
		}
	}
}