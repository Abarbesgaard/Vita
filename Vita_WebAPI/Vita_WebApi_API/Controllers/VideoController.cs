using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Vita_WebApi_API.Dto;
using Vita_WebApi_API.Extensions;
using Vita_WebAPI_Services;
using Vita_WebApi_Shared;

namespace Vita_WebApi_API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class VideoController(IVideoService service, ILogger<VideoController> logger) : ControllerBase
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
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VideoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VideoDto>> GetVideoByIdAsync(Guid id)
    {
        var video = await service.GetVideoById(id);
        if (video == null)
        {
            logger.LogWarning("No video found with ID: {id}", id);
            return NotFound("No video found with the provided ID");
        }

        return video.AsDto();
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
    public async Task<ActionResult<VideoDto>> PostVideoAsync(VideoDto videoDto)
    {
        if (videoDto == null)
        {
            return BadRequest("VideoDto is null.");
        } 
        var video = new Video
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.Now,
            Title = videoDto.Title,
            Description = videoDto.Description,
            Url = videoDto.Url,
            UpdatedAt = videoDto.UpdatedAt,
        };
        await service.CreateVideo(video);
        return CreatedAtAction(nameof(GetVideoByIdAsync), new { id = video.Id }, video.AsDto());
    
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
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateVideoDto videoDto)
    {
        var existingVideo = await service.GetVideoById(id);
        if (existingVideo == null)
        {
            return NotFound();
        }

        existingVideo.Title = videoDto.Title;
        existingVideo.Description = videoDto.Description;
        existingVideo.Url = videoDto.Url;
        await service.UpdateVideo(existingVideo);
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
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var video = await service.GetVideoById(id);
        if (video == null)
        {
            return NotFound();
        }
        await service.DeleteVideo(id);
        return NoContent();
    }
}