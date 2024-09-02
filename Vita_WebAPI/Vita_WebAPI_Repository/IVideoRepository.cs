using Vita_WebApi_Shared;

namespace Vita_WebAPI_Repository;
/// <summary>
/// Interface for the Video Repository
/// </summary>
public interface IVideoRepository
{
    /// <summary>
    /// Asynchronously deletes a video record from the system.
    /// </summary>
    /// <param name="id"> The unique identifier of the video record to delete.</param>
    /// <returns> A task representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid id);
    /// <summary>
    /// Asynchronously retrieves a video record by its unique identifier.
    /// </summary>
    /// <param name="id"> The unique identifier of the video record to retrieve.</param>
    /// <returns> A task representing the asynchronous operation.</returns>
    Task<Video?> GetByIdAsync(Guid id);
    /// <summary>
    /// Asynchronously retrieves all video records in the system.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    Task<IReadOnlyCollection<Video>> GetAllAsync();
    /// <summary>
    /// Asynchronously creates a new video record in the system.
    /// </summary>
    /// <param name="entity"> The video object containing details to be created.</param>
    /// <returns> A task representing the asynchronous operation.</returns>
    Task CreateAsync(Video entity);
    /// <summary>
    /// Asynchronously updates an existing video record in the system.
    /// </summary>
    /// <param name="entity"> The video object containing details to be updated.</param>
    /// <returns> A task representing the asynchronous operation.</returns>
    Task UpdateAsync(Video entity);


}