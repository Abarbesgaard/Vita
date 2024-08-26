using Vita_WebAPI_Data;
using Vita_WebAPI_Repository;
using Vita_WebApi_Shared;

namespace ClassLibrary1Vita_WebAPI_Repository;

public class VideoRepository(DataContext context) : GenericRepository<Video>(context), IVideoRepository
{
    
}