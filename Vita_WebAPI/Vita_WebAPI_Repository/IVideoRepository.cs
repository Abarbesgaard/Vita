using Vita_WebApi_Shared;

namespace Vita_WebAPI_Repository;

public interface IVideoRepository
{
    Task DeleteAsync(Guid id);
    Task<Video?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Video>> GetAllAsync();
    Task CreateAsync(Video entity);
    Task UpdateAsync(Video entity);


}