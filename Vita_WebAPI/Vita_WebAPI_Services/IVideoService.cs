using Vita_WebApi_Shared;

namespace Vita_WebAPI_Services;
/// <summary>
/// Interface for the Video Service
/// </summary>
public interface IVideoService 
{
    
    /// <summary>
    /// Asynchronously creates a new video record in the system.
    /// </summary>
    /// <param name="video">The video object containing details to be created.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateVideo(Video video);
    /// <summary>
    /// Asynchronously retrieves all video records in the system.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<IEnumerable<Video>?> GetAllVideos();
    /// <summary>
    /// Asynchronously retrieves a video record by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the video record to retrieve.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Video?> GetVideoById(Guid id);

    /// <summary>
    /// Asynchronously updates an existing video record in the system.
    /// </summary>
    /// <param name="video">The video object containing details to be updated.</param>
    /// <returns> A task representing the asynchronous operation.</returns> 
    Task UpdateVideo(Video? video);
    
    /// <summary>
    /// Asynchronously deletes a video record from the system.
    /// </summary>
    /// <param name="id"> The unique identifier of the video record to delete.</param>
    /// <returns> A task representing the asynchronous operation.</returns>
    Task DeleteVideo(Guid id);
}