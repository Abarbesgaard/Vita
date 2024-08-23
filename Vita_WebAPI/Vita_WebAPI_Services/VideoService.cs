using ClassLibrary1Vita_WebAPI_Repository;
using Vita_WebApi_Shared;

namespace Vita_WebAPI_Services;
public interface IVideoService
{
}
public class VideoService(IVideoRepository repository) : IVideoService 
{
    private readonly IVideoRepository _repository = repository;

    public async Task CreateVideo(Video video)
    {
        await  _repository.AddAsync(video);
    }
    
    public async Task<IEnumerable<Video>> GetAllVideos()
    {
        return await _repository.GetAllAsync();
    }
    
    public async Task<Video?> GetVideoById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}

