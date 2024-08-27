using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vita_WebApi_API.Dto;
using Vita_WebAPI_Services;
using Vita_WebApi_Shared;

namespace Vita_WebApi_API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class VideoController(IVideoService service, ILogger<VideoController> logger) : Controller
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
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VideoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllVideos()
    {
        try
        {
            logger.LogInformation("Getting all videos");
            var videos = await service.GetAllVideos();
            if (videos == null)
            {
                logger.LogWarning("No videos found");
                return NotFound("No videos found");
            }
            return Ok(videos);
        } 
        catch (Exception e)
        {
            logger.LogError($"An error occurred: {e.Message}");
            return StatusCode(500, "Internal server error");
        }
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
    ///         "isDeleted": false,
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
    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(VideoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetVideoById(int id)
    {
        if (id <= 0)
        {
            logger.LogWarning("Invalid video ID");
            return BadRequest("Invalid video ID");
        }
        
        var video = await service.GetVideoById(id);
        if (video != null) return Ok(video);
        logger.LogWarning($"No video found with ID: {id}");
        return NotFound();
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
    [HttpPost]
    public async Task<IActionResult> CreateVideo([FromBody] VideoDto videoDto)
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid model object");
            return BadRequest("Invalid model object");
        }
        var video = new Video
        {
            Title = videoDto.Title,
            Description = videoDto.Description,
            Url = videoDto.Url
        };
        try
        {
            await service.CreateVideo(video);
            logger.LogInformation("Video created successfully");
            return CreatedAtAction(nameof(GetVideoById), new { id = video.Id }, video);
        }
        catch (Exception)
        {
            logger.LogError("An error occurred while creating the video");
            return StatusCode(500, "Internal server error");
        }
    }
}