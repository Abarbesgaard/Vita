using FluentAssertions;
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
    private static IVideoRepository? _videoRepository;
    private VideoService? _videoService; // Remove nullable
    private IMongoDatabase? _testDatabase; 
    private const string TestDatabaseName = "VideoRepositoryTestDatabase";

    [TestInitialize]
    public void Setup()
    {
        // Set up MongoDB connection
        const string connectionString = "mongodb://localhost:27017"; // Your MongoDB connection string
        var client = new MongoClient(connectionString);
        _testDatabase = client.GetDatabase(TestDatabaseName);

        // Initialize logger
        var loggerFactory = new LoggerFactory();
        var logger = loggerFactory.CreateLogger<VideoRepository>();

        // Initialize VideoRepository with real database connection
        var videoDatabaseSetting = new VideoDatabaseSetting
        {
            Host = "localhost",
            Port = 27017,
            DatabaseName = TestDatabaseName
        };
        
        _videoRepository = new VideoRepository(logger, Options.Create(videoDatabaseSetting));

        // Initialize VideoService after VideoRepository is set up
        _videoService = new VideoService(_videoRepository, loggerFactory.CreateLogger<VideoService>());

        // Prepare test data
        InitializeTestData();
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
    [TestCleanup]
    public void Cleanup()
    {
        var videoCollection = _testDatabase?.GetCollection<Video>("Videos");
        videoCollection?.DeleteMany(FilterDefinition<Video>.Empty);
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
        videoFromDb.Title.Should().Be(updatedVideo.Title);
        videoFromDb.Url.Should().Be(updatedVideo.Url);
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