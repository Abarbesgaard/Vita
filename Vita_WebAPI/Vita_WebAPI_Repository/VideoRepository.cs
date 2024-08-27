using ClassLibrary1Vita_WebAPI_Repository;
using Microsoft.Extensions.Logging;
using Vita_WebAPI_Data;
using Vita_WebApi_Shared;

namespace Vita_WebAPI_Repository;

public class VideoRepository(DataContext context, ILogger<VideoRepository> logger) : GenericRepository<Video>(context, logger), IVideoRepository
{
    
}