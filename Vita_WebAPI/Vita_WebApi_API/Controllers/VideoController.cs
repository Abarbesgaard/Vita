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

    [HttpGet]
    public async Task<IActionResult> GetAllVideos()
    {
        var videos = await _service.GetAllVideos();
        return Ok(videos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetVideoById(int id)
    {
        var video = await _service.GetVideoById(id);
        if (video == null)
        {
            return NotFound();
        }
        return Ok(video);
    }

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