namespace Vita_WebAPI_Repository;

public class VideoRepository(DataContext context) : IGenericRepository<Video>(context), IVideoRepo
{
    
}