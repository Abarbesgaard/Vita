using Microsoft.Extensions.Logging;
using Vita_WebAPI_Repository;
using Vita_WebApi_Shared;
namespace Vita_WebAPI_Services;

/// <summary>
/// Service for managing video operations
/// </summary>
public class VideoService: IVideoService 
{
    /// <summary>
    /// The video repository
    /// </summary>
    private readonly IVideoRepository? _repository;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<VideoService> _logger;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="repository"> The video repository</param>
    /// <param name="logger"> The logger</param>
    public VideoService(IVideoRepository? repository, ILogger<VideoService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
   
    /// <summary>
    /// Creates a new video
    /// </summary>
    /// <param name="video">The video object containing details to be created.</param>
    /// <returns> A task representing the asynchronous operation.</returns>
    public async Task CreateVideo(Video video)
    {
        try
        {
         _logger.LogInformation("Creating a new video");   
        await  _repository.CreateAsync(video);
        _logger.LogInformation("Video created");
        }
        catch (Exception e)
        {
            _logger.LogError("An error occurred: {0}", e.Message);
            throw;
        }
    }
   /// <summary>
   /// Retrieves a list of all videos
   /// </summary>
   /// <exception cref="Exception">Thrown when an error occurs</exception>
   /// <returns> A List of all videos</returns>
    public async Task<IEnumerable<Video>?> GetAllVideos()
    {
        try
        {
            _logger.LogInformation("Getting all videos");
            var videos = await _repository.GetAllAsync();
            return videos;
        }
        catch (Exception e)
        {
            _logger.LogError($"An error occurred: {e.Message}");
            throw;
        }
    }
    /// <summary>
    /// Retrieves a video by its unique identifier
    /// </summary>
    /// <exception cref="Exception"> Thrown when an error occurs</exception>
    /// <param name="id"> The unique identifier of the video record to retrieve.</param>
    /// <returns> A task representing the asynchronous operation.</returns>
    public async Task<Video?> GetVideoById(Guid id)
    {
        try
        {
         _logger.LogInformation($"Getting video with id: {id}");   
        return await _repository.GetByIdAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError($"An error occurred: {e.Message}");
            throw;
        }
    }
    /// <summary>
    /// Updates a video
    /// </summary>
    /// <param name="video">The video object containing details to be updated.</param>
    public async Task UpdateVideo(Video? video)
    {
        if (video != null) await _repository.UpdateAsync(video);
    }

    /// <summary>
    ///  Deletes a video
    /// </summary>
    /// <param name="id"> The unique identifier of the video record to delete.</param>
    /// <returns> A task representing the asynchronous operation.</returns>
    public Task DeleteVideo(Guid id)
    {
        return _repository.DeleteAsync(id);
    }
}

