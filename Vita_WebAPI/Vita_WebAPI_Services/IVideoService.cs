using Vita_WebApi_Shared;

namespace Vita_WebAPI_Services;

public interface IVideoService 
{
    Task CreateVideo(Video? video);
    Task<IEnumerable<Video>?> GetAllVideos();
    Task<Video?> GetVideoById(Guid id);

    
    Task UpdateVideo(Video? video);
    Task DeleteVideo(Guid id);
    
}