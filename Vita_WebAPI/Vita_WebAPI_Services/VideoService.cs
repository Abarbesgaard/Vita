﻿using ClassLibrary1Vita_WebAPI_Repository;
using Microsoft.Extensions.Logging;
using Vita_WebApi_Shared;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Vita_WebAPI_Services;
public interface IVideoService
{
    Task CreateVideo(Video video);
    Task<IEnumerable<Video>?> GetAllVideos();
    Task<Video?> GetVideoById(int id);
}
public class VideoService: IVideoService 
{
    private readonly IVideoRepository _repository;

    private readonly ILogger<VideoService> _logger;
    // Constructor
    public VideoService(IVideoRepository repository, ILogger<VideoService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
   
    /// <summary>
    /// Creates a new video
    /// </summary>
    /// <param name="video"></param>
    public async Task CreateVideo(Video video)
    {
        try
        {
         _logger.LogInformation("Creating a new video");   
        await  _repository.AddAsync(video);
        _logger.LogInformation("Video created");
        }
        catch (Exception e)
        {
            _logger.LogError($"An error occurred: {e.Message}");
            throw;
        }
    }
   /// <summary>
   /// Retrieves a list of all videos
   /// </summary>
   /// <returns>List of Videos</returns> 
    public async Task<IEnumerable<Video>?> GetAllVideos()
    {
        try
        {
            _logger.LogInformation("Getting all videos");
            var videos = await _repository.GetAllAsync();
            if (videos == null)
            {
                _logger.LogWarning("No videos found");
                return null;
            }
            return videos;
        }
        catch (Exception e)
        {
            _logger.LogError($"An error occurred: {e.Message}");
            throw;
        }
    }
    
    public async Task<Video?> GetVideoById(int id)
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
}
