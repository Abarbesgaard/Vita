using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vita_WebApi_API.Dto;
using Vita_WebAPI_Services;
using Vita_WebApi_Shared;
using Serilog;

namespace Vita_WebApi_API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class VideoController : Controller
{ 
    private readonly IVideoService _service;
    private readonly ILogger<VideoController> _logger; 
    public VideoController(IVideoService service, ILogger<VideoController> logger)
    {
        _service = service;
        _logger = logger;
    }


    /// <summary>
    /// Retrieves a list of all videos.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/video
    /// </remarks>
    /// <response code="200">Returns a list of all videos.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    /// <returns>A list of video objects.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Video>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllVideos()
    {
        try
        {
            _logger.LogInformation("Getting all videos");
            var videos = await _service.GetAllVideos();
            if (videos == null)
            {
                _logger.LogWarning("No videos found");
                return NotFound("No videos found");
            }
            return Ok(videos);
        } 
        catch (Exception e)
        {
            _logger.LogError($"An error occurred: {e.Message}");
            return StatusCode(500, "Internal server error");
        }
    }   
    
    /// <summary>
    /// Get video by id
    /// </summary>
    /// <param name="id">The id of the video</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/video/1
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
    /// <response code="200">Returns the video object.</response>
    /// <response code="404">If no video is found with the given ID.</response>
    /// <returns>Returns a video object</returns>
    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> GetVideoById(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Invalid video ID");
            return BadRequest("Invalid video ID");
        }
        
        var video = await _service.GetVideoById(id);
        if (video != null) return Ok(video);
        _logger.LogWarning($"No video found with ID: {id}");
        return NotFound();
    }

    /// <summary>
    /// Creates a new video record.
    /// </summary>
    /// <param name="videoDto">The video data to be created.</param>
    /// <returns>Confirmation message indicating the success of the operation.</returns>
    /// <response code="201">Returns a success message with the created video details.
    /// </response>
    /// <response code="400">If the provided data is invalid.
    /// </response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPost]
    public async Task<IActionResult> CreateVideo([FromBody] VideoDto videoDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model object");
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
            await _service.CreateVideo(video);
            _logger.LogInformation("Video created successfully");
            return CreatedAtAction(nameof(GetVideoById), new { id = video.Id }, video);
        }
        catch (Exception)
        {
            _logger.LogError("An error occurred while creating the video");
            return StatusCode(500, "Internal server error");
        }
    }
}