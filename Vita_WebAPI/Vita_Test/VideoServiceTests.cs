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
    private IGenericRepository<Video> _videoRepository;
    private IGenericService<Video> _videoService; // No nullable now
    private IMongoDatabase? _testDatabase;
    private IMongoClient? _mongoClient;
    private ILogger<GenericService<Video>>? _logger;
    private ILogger<GenericRepository<Video>>? _log;
    private IAuditLogService? _auditLogService;
    private const string TestDatabaseName = "VideoRepositoryTestDatabase";

    [TestInitialize]
    public void Setup()
    {
       const string connectionString = "mongodb://localhost:27017";
        _mongoClient = new MongoClient(connectionString);
        _testDatabase = _mongoClient.GetDatabase(TestDatabaseName);
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();  // Adds console logging
            builder.SetMinimumLevel(LogLevel.Debug);  // Set the logging level
        });

        _logger = loggerFactory.CreateLogger<GenericService<Video>>();
        _log = loggerFactory.CreateLogger<GenericRepository<Video>>();
        _auditLogService = new AuditLogService(_testDatabase);
        _videoRepository = new GenericRepository<Video>(_testDatabase, _log);
        _videoService = new GenericService<Video>(_videoRepository, _logger, _auditLogService);
        
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
        await _videoService?.CreateAsync(video)!;

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
        var videos = await _videoService?.GetAllAsync()!;

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
        var video = await _videoService?.GetByIdAsync(videoId)!;

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
        var nonExistentId = Guid.NewGuid(); // Generate a new GUID that does not exist in the database

        // Act
        Func<Task> act = async () => await _videoService.GetByIdAsync(nonExistentId);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Entity with ID: {nonExistentId} not found");
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
        await _videoService?.UpdateAsync(updatedVideo.Id, updatedVideo)!;

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
        await _videoService?.DeleteAsync(existingVideo.Id)!;

        // Assert
        var videoFromDb = await videoCollection.Find(v => v.Id == existingVideo.Id).FirstOrDefaultAsync();
        videoFromDb.Should().BeNull(); // The video should no longer exist
    }
 
 
}