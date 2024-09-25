using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Vita_WebAPI_Data;
using Vita_WebAPI_Repository;
using Vita_WebAPI_Services;
using Vita_WebApi_Shared;

namespace Vita_Test;
[TestClass]
public class VideoServiceTests
{
   
    private IGenericRepository<Video>? _videoRepository;
    private VideoService? _videoService; // No nullable now
    private IMongoDatabase? _testDatabase; 
    private ServiceProvider? _serviceProvider;

    private const string TestDatabaseName = "VideoRepositoryTestDatabase";

    [TestInitialize]
    public void Setup()
    {
        // Configure services to match the application DI container
        var serviceCollection = new ServiceCollection();

        // Register MongoDB settings
        var settings = new VideoDatabaseSetting
        {
            Host = "localhost",
            Port = 27017,
            DatabaseName = TestDatabaseName
        };
        var options = Options.Create(settings);
        serviceCollection.AddSingleton<IOptions<VideoDatabaseSetting>>(options);

        // Register MongoClient and IMongoDatabase
        serviceCollection.AddSingleton<IMongoClient>(provider =>
        {
            var mongoDbSettings = provider.GetRequiredService<IOptions<VideoDatabaseSetting>>().Value;
            return new MongoClient($"mongodb://{mongoDbSettings.Host}:{mongoDbSettings.Port}");
        });

        serviceCollection.AddSingleton<IMongoDatabase>(provider =>
        {
            var mongoDbSettings = provider.GetRequiredService<IOptions<VideoDatabaseSetting>>().Value;
            var client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoDbSettings.DatabaseName);
        });

        // Register GenericRepository for Video
        serviceCollection.AddScoped<IGenericRepository<Video>, GenericRepository<Video>>();

        // Register logger and other services
        serviceCollection.AddLogging();
        serviceCollection.AddScoped<IAuditLogService, AuditLogService>();

        // Build the service provider
        _serviceProvider = serviceCollection.BuildServiceProvider();

        // Resolve dependencies from the service provider
        _videoRepository = _serviceProvider.GetRequiredService<IGenericRepository<Video>>();
        var logger = _serviceProvider.GetRequiredService<ILogger<VideoService>>();
        var auditLogService = _serviceProvider.GetRequiredService<IAuditLogService>();

        // Initialize VideoService with resolved dependencies
        _videoService = new VideoService(_videoRepository, logger, auditLogService);

        // Get the test database for direct operations
        _testDatabase = _serviceProvider.GetRequiredService<IMongoDatabase>();

        // Prepare test data
        InitializeTestData();
    }

    [TestCleanup]
    public void Cleanup()
    {
        // Drop the test database after each test to ensure a clean slate
        _testDatabase?.Client.DropDatabase(TestDatabaseName);
    }
    private void InitializeTestData()
    {
        var videoCollection = _testDatabase?.GetCollection<Video>("Videos");

        // Clear existing data
        videoCollection!.DeleteMany(FilterDefinition<Video>.Empty);

        // Insert sample data
        var testVideo = new Video
        {
            Id = Guid.NewGuid(),
            Title = "Test Video 1",
            Url = "https://example.com/"
        };

        videoCollection.InsertOne(testVideo);
    } 
    
    [TestMethod]
    public async Task CreateVideo_ShouldCreateVideo_WhenValidVideoIsProvided()
    {
        // Arrange
        var video = new Video
        {
            Id = Guid.NewGuid(),
            Title = "Test Video",
            Url = "https://example.com/"
        };

        // Act
        await _videoService?.CreateVideo(video)!;

        // Assert
        var videoCollection = _testDatabase?.GetCollection<Video>("Videos");
        var videoFromDb = await videoCollection.Find(v => v.Id == video.Id).FirstOrDefaultAsync();

        // Debugging information
        if (videoFromDb == null)
        {
            Assert.Fail("Video not found in the database. Check database connectivity and initialization.");
        }

        videoFromDb.Should().NotBeNull();
        videoFromDb.Title.Should().Be(video.Title);
        videoFromDb.Url.Should().Be(video.Url);
    }
    [TestMethod]
    public async Task GetAllVideos_ShouldReturnVideos_WhenVideosExist()
    {
        // Act
        var videos = await _videoService?.GetAllVideos()!;

        // Assert
        if (videos != null)
        {
            var enumerable = videos.ToList();
            enumerable.Should().NotBeNull();
            enumerable.Should().HaveCount(1); // Adjust based on the number of videos inserted in InitializeTestData
        }
    } 
    [TestMethod]
    public async Task GetVideoById_ShouldReturnVideo_WhenVideoExists()
    {
        // Arrange
        var videoCollection = _testDatabase?.GetCollection<Video>("Videos");
        var testVideo = videoCollection.Find(FilterDefinition<Video>.Empty).First();
        var videoId = testVideo.Id;
    
        // Act
        var video = await _videoService?.GetVideoById(videoId)!;

        // Assert
        video.Should().NotBeNull();
        video.Id.Should().Be(videoId);
        video.Title.Should().Be("Test Video 1"); // Ensure this matches the initialized data
        video.Url.Should().Be("https://example.com/");
    }
    [TestMethod]
    public async Task GetVideoById_ShouldReturnNull_WhenVideoDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        
        // Act
        var video = await _videoService?.GetVideoById(nonExistentId)!;

        // Assert
        video.Should().BeNull();
    } 
    [TestMethod]
    public async Task UpdateVideo_ShouldUpdateVideo_WhenValidVideoIsProvided()
    {
        // Arrange
        var videoCollection = _testDatabase?.GetCollection<Video>("Videos");
        var existingVideo = new Video
        {
            Id = Guid.NewGuid(),
            Title = "Old Title",
            Url = "https://example.com/old"
        };
        await videoCollection?.InsertOneAsync(existingVideo)!;

        var updatedVideo = new Video
        {
            Id = existingVideo.Id, // Use the same ID to update
            Title = "New Title",
            Url = "https://example.com/new"
        };

        // Act
        await _videoService?.UpdateVideo(updatedVideo)!;

        // Assert
        var videoFromDb = await videoCollection.Find(v => v.Id == updatedVideo.Id).FirstOrDefaultAsync();
    
        videoFromDb.Should().NotBeNull();
        videoFromDb.Title.Should().Be(updatedVideo.Title, "the title should have been updated to the new value");
        videoFromDb.Url.Should().Be(updatedVideo.Url, "the URL should have been updated to the new value");
    }
    [TestMethod] 
    public async Task DeleteVideo_ShouldDeleteVideo_WhenValidIdIsProvided()
    {
        // Arrange
        var videoCollection = _testDatabase?.GetCollection<Video>("Videos");
        var existingVideo = new Video
        {
            Id = Guid.NewGuid(),
            Title = "To Be Deleted",
            Url = "https://example.com/to-be-deleted"
        };
        await videoCollection?.InsertOneAsync(existingVideo)!;

        // Act
        await _videoService?.DeleteVideo(existingVideo.Id)!;

        // Assert
        var videoFromDb = await videoCollection.Find(v => v.Id == existingVideo.Id).FirstOrDefaultAsync();
        videoFromDb.Should().BeNull(); // The video should no longer exist
    }
 
 
}