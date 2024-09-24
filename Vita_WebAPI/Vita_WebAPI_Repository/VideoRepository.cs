using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using Vita_WebAPI_Data;
using Vita_WebApi_Shared;

namespace Vita_WebAPI_Repository;
/// <summary>
/// Repository for managing video operations
/// </summary>
///
/*
public class VideoRepository : GenericRepository<Video>, IVideoRepository
{
    private const string? CollectionName = "Videos";

   public VideoRepository(IMongoClient client, IOptions<VideoDatabaseSetting> setting) 
        : base(client, setting.Value.DatabaseName ?? throw new ArgumentNullException(nameof(setting.Value.DatabaseName)), CollectionName)
    
    {
        Console.WriteLine($"VideoDatabaseSetting: {setting.Value.DatabaseName}"); 
    }
}*/