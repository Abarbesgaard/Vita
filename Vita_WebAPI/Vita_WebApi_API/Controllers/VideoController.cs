using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Vita_WebApi_API.Dto;
using AutoMapper;
using Vita_WebAPI_Services;
using Vita_WebApi_Shared;

namespace Vita_WebApi_API.Controllers;
/// <summary>
/// The controller responsible for handling video-related operations.
/// </summary>
/// <param name="service">The service that provides video-related operations.</param>
/// <param name="logger">The logger used to log information and errors.</param>
/// <param name="env">The hosting environment information.</param>
[ApiController]
[Route("api/[controller]")]
public class VideoController(
    IGenericService<Video> service, 
    ILogger<VideoController> logger, 
    IWebHostEnvironment env, 
    IMapper? mapper) : ControllerBase
{

    /// <summary>
    /// Retrieves a list of all videos from the system.
    /// </summary>
    /// <remarks>
    /// This endpoint retrieves a collection of video objects available in the system.
    /// 
    /// Sample request:
    /// 
    ///     GET /api/video
    /// 
    /// Sample response:
    /// 
    ///     HTTP/1.1 200 OK
    ///     Content-Type: application/json
    ///     
    ///     [
    ///       {
    ///         "id": 1,
    ///         "title": "Sample Video",
    ///         "description": "This is a sample video description.",
    ///         "url": "http://example.com/video.mp4"
    ///       },
    ///       ...
    ///     ]
    /// </remarks>
    /// <response code="200">Returns a list of video objects.</response>
    /// <response code="404">No videos were found in the system.</response>
    /// <response code="500">Internal server error if something goes wrong while retrieving videos.</response>
    /// <returns>A list of video objects or a message indicating no videos were found.</returns>
    [HttpGet("GetAll")] 
    [ProducesResponseType(typeof(IEnumerable<GetVideoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult?> GetAllVideos()
    {
        if (env.IsDevelopment())
        {
            try
            {
                logger.LogInformation("Getting all videos (Development mode)");
                var videos = await service.GetAllAsync();
                var videoDtos = mapper?.Map<IEnumerable<VideoDto>>(videos);
                if (videoDtos == null || !videoDtos.Any())
                {
                    logger.LogWarning("No videos found");
                    return NotFound("No videos found");
                }
                return Ok(videoDtos);
            }
            catch (Exception e)
            {
                logger.LogError($"An error occurred: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        
        //--------------------------------------------------------------------------
        // Production mode
        // In production, validate the token and return the videos if the token is valid.
        // Proper error handling is included to log issues and return correct status codes.
        //--------------------------------------------------------------------------
        if (!env.IsDevelopment())
        {

            var tokenValidationResult = ValidateToken();
            if (tokenValidationResult.Result != null)
            {
                return await tokenValidationResult; 
            }



            try
            {
                logger.LogInformation("Getting all videos");
                var videos = await service.GetAllAsync();
                var videoDtos = mapper?.Map<IEnumerable<VideoDto>>(videos);
                if (videoDtos == null || !videoDtos.Any())
                {
                    logger.LogWarning("No videos found");
                    return NotFound("No videos found");
                }
                return Ok(videoDtos);
            }
            catch (Exception e)
            {
                logger.LogError($"An error occurred: {e.Message}");
                return StatusCode(500, "Internal server error");
            }    }

        return null;
    }
    
    /// <summary>
    /// Retrieves a video by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the video to retrieve.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/video/1
    ///
    /// Sample response:
    ///
    ///     HTTP/1.1 200 OK
    ///     Content-Type: application/json
    ///     
    ///     {
    ///         "id": 14,
    ///         "createdAt": "2024-08-24T05:08:44.377Z",
    ///         "updatedAt": "2024-08-24T05:08:44.377Z",
    ///         "title": "Hjælp til at indberette regnskab",
    ///         "description": "En udførlig video til at hjælpe dig med at indberette regnskab",
    ///         "url": "https://link.til.youtube.com"
    ///     }
    /// </remarks>
    /// <response code="200">Returns the video object with the specified ID.</response>
    /// <response code="400">Invalid ID provided in the request.</response>
    /// <response code="404">No video found with the provided ID.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    /// <returns>A video object if found, otherwise a 404 Not Found response.</returns>
    [HttpGet("Get/{id:guid}")]
    [ProducesResponseType(typeof(GetVideoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult?> GetVideoByIdAsync(Guid id)
    {
        var tokenValidationResult = ValidateToken();
        if (tokenValidationResult.Result != null)
        {
            return await tokenValidationResult; 
        }
        
        if (id == Guid.Empty)
        {
            logger.LogWarning("Invalid ID provided");
            return BadRequest("Invalid ID provided");
        }
 
        var video = await service.GetByIdAsync(id);
        var videoDto = mapper?.Map<IEnumerable<VideoDto>>(video);
        return Ok(videoDto ?? throw new InvalidOperationException("Videos not found"));
    }
    


    /// <summary>
    /// Creates a new video record.
    /// </summary>
    /// <param name="videoDto">The data for the video to be created, including title, description, and URL.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/video
    ///     Content-Type: application/json
    ///     
    ///     {
    ///         "title": "New Video Title",
    ///         "description": "A detailed description of the new video.",
    ///         "url": "https://link.to/newvideo"
    ///     }
    /// 
    /// Sample response:
    /// 
    ///     HTTP/1.1 201 Created
    ///     Content-Type: application/json
    ///     
    ///     {
    ///         "id": 123,
    ///         "title": "New Video Title",
    ///         "description": "A detailed description of the new video.",
    ///         "url": "https://link.to/newvideo",
    ///         "createdAt": "2024-08-27T10:00:00.000Z",
    ///         "updatedAt": "2024-08-27T10:00:00.000Z",
    ///         "isDeleted": false
    ///     }
    /// </remarks>
    /// <response code="201">Returns the created video object with its ID and details.</response>
    /// <response code="400">If the provided data is invalid or does not meet validation requirements.</response>
    /// <response code="500">If an internal server error occurs during the creation process.</response>
    /// <returns>A confirmation message with the created video details.</returns>
    [HttpPost("Create")]
    public async Task<IActionResult?> PostVideoAsync([FromBody] CreateVideoDto? videoDto)
    {
        if (videoDto == null)
        {
            return BadRequest("VideoDto is null.");
        }

        // Handle token validation for non-development environments
        if (!env.IsDevelopment())
        {
            // Reuse the ValidateToken method
            var tokenValidationResult = ValidateToken();
            if (tokenValidationResult.Result != null)
            {
                return await tokenValidationResult; 
            }
            var token = Request.Headers["Authorization"].ToString();
            var tokenString = token["Bearer ".Length..].Trim();
            var claimsPrincipal = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
            var subClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(subClaim))
            {
                return Unauthorized("Invalid token - missing 'sub' claim");
            }
            var video = new Video
            {
                CreatedBy = subClaim,
                UpdatedBy = subClaim,
                CreatedAt = DateTimeOffset.Now,
                Title = videoDto.Title,
                Description = videoDto.Description,
                Url = videoDto.Url,
                UpdatedAt = videoDto.UpdatedAt
            };
        
            await service
                .CreateAsync(video)!;
            return CreatedAtAction(nameof(GetVideoByIdAsync), new { id = video.Id }, video);
        }
        
        if (env.IsDevelopment())
        {
            var videoTest = new Video
            {
                CreatedBy = "TestUser",
                UpdatedBy = "TestUser",
                CreatedAt = DateTimeOffset.Now,
                Title = videoDto.Title,
                Description = videoDto.Description,
                Url = videoDto.Url,
                UpdatedAt = videoDto.UpdatedAt
            };

            await service.CreateAsync(videoTest)!;
            return CreatedAtAction(nameof(GetVideoByIdAsync), new { id = videoTest.Id }, videoTest);
        }

		return null!;
	}
	/// <summary>
	/// Updates an existing video record in the system.
	/// This operation replaces the current video record with the updated data provided in the request body.
	/// </summary>
	/// <param name="id">The unique identifier (GUID) of the video to be updated.</param>
	/// <param name="videoDto">An object containing the updated video details. It must include the title, description, and URL of the video.</param>
	/// <returns>
	/// A <see cref="IActionResult"/> that represents the result of the update operation:
	/// - <see cref="NoContent"/> (HTTP 204) if the video was successfully updated.
	/// - <see cref="NotFound"/> (HTTP 404) if no video with the specified ID was found.
	/// </returns>
	/// <remarks>
	/// If the video with the specified ID does not exist, the method returns a 404 Not Found status code.
	/// The request body must be a JSON object with the following structure:
	/// 
	///     {
	///         "title": "The new title of the video",
	///         "description": "The new description of the video",
	///         "url": "http://example.com/the-new-video-url"
	///     }
	/// 
	/// Example request:
	/// 
	///     PUT /api/videos/123e4567-e89b-12d3-a456-426614174000
	///     Content-Type: application/json
	///     
	///     {
	///         "title": "Updated Video Title",
	///         "description": "Updated Video Description",
	///         "url": "http://example.com/updated-video-url"
	///     }
	/// 
	/// Example response (successful update):
	/// 
	///     HTTP/1.1 204 No Content
	/// 
	/// Example response (video not found):
	/// 
	///     HTTP/1.1 404 Not Found
	/// </remarks>
	[HttpPut("Put/{id:guid}")]
	public async Task<IActionResult?> PutAsync(Guid id, UpdateVideoDto? videoDto)
	{
        if (videoDto == null)
        {
            logger.LogWarning("UpdateVideoDto is null");
            return BadRequest("Invalid video data");
        }

        var tokenValidationResult = ValidateToken();
        if (tokenValidationResult.Result != null)
        {
            return await tokenValidationResult; 
        }

 
        var existingVideo = await service.GetByIdAsync(id);

        existingVideo.Title = videoDto.Title;
        existingVideo.Description = videoDto.Description;
        existingVideo.Url = videoDto.Url;
        await service.UpdateAsync(existingVideo.Id, existingVideo);
        return NoContent();
            
    }
    /// <summary>
    /// Deletes a video record from the system by its unique identifier.
    /// This operation removes the video with the specified ID from the database. 
    /// </summary>
    /// <param name="id">The unique identifier (GUID) of the video to be deleted.</param>
    /// <returns>
    /// A <see cref="IActionResult"/> that represents the result of the delete operation:
    /// - <see cref="NoContent"/> (HTTP 204) if the video was successfully deleted.
    /// - <see cref="NotFound"/> (HTTP 404) if no video with the specified ID was found.
    /// </returns>
    /// <remarks>
    /// If the video with the specified ID does not exist, the method returns a 404 Not Found status code.
    /// 
    /// Example request:
    /// 
    ///     DELETE /api/videos/123e4567-e89b-12d3-a456-426614174000
    /// 
    /// Example response (successful deletion):
    /// 
    ///     HTTP/1.1 204 No Content
    /// 
    /// Example response (video not found):
    /// 
    ///     HTTP/1.1 404 Not Found
    /// </remarks> 
    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult?> DeleteAsync(Guid id)
    {
        var tokenValidationResult = ValidateToken();
        if (tokenValidationResult.Result != null)
        {
            return await tokenValidationResult; 
        }

        // Check if the user has permission to delete the video
        var userId = User.FindFirst("sub")?.Value;
        if (userId == null)
        {
            logger.LogWarning("User ID not found in token");
            return Unauthorized("User ID not found in token");
        }

        var video = await service.GetByIdAsync(id);

        if (video.CreatedBy != userId)
        {
            logger.LogWarning($"User {userId} does not have permission to delete video with ID {id}");
            return Forbid("You do not have permission to delete this video");
        }

        await service.DeleteAsync(id);
        return NoContent();
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