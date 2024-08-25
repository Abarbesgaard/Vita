using Microsoft.AspNetCore.Mvc;
using Vita_WebApi_API.Dto;
using Vita_WebAPI_Services;
using Vita_WebApi_Shared;

namespace Vita_WebApi_API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class VideoController : Controller
{ 
    private readonly IVideoService _service;

    public VideoController(IVideoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves a list of all videos.
    /// </summary>
    /// <returns>A list of video objects.</returns>
    /// <response code="200">Returns the list of all videos.</response>
    [HttpGet]
    public async Task<IActionResult> GetAllVideos()
    {
        var videos = await _service.GetAllVideos();
        return Ok(videos);
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> GetVideoById(int id)
    {
        var video = await _service.GetVideoById(id);
        if (video == null)
        {
            return NotFound();
        }
        return Ok(video);
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
    public async Task<IActionResult> CreateVideo(VideoDto videoDto)
    {
        var video = new Video
        {
            Title = videoDto.Title,
            Description = videoDto.Description,
            Url = videoDto.Url
        };
        try
        {
            await _service.CreateVideo(video);
            return Ok("Video created successfully");
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}