using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using Vita_WebAPI_Data;
using Vita_WebAPI_Repository;
using Vita_WebApi_Shared;

namespace Vita_Test;

[TestClass]
public class VideoRepositoryIntegrationTests
{
    private IVideoRepository? _videoRepository;
    private IMongoDatabase? _testDatabase;
    private const string TestDatabaseName = "VideoRepositoryTestDatabase";
    
    [TestInitialize]
    public void Setup()
    {
        // Set up MongoDB connection
        const string connectionString = "mongodb://localhost:27017"; // Your MongoDB connection string
        var client = new MongoClient(connectionString);
        _testDatabase = client.GetDatabase(TestDatabaseName);

        // Initialize VideoRepository with real database connection
        var videoDatabaseSetting = new VideoDatabaseSetting
        {
            Host = "localhost",
            Port = 27017,
            DatabaseName = TestDatabaseName
        };
        
        var logger = new LoggerFactory().CreateLogger<VideoRepository>();
        _videoRepository = new VideoRepository(logger, Options.Create(videoDatabaseSetting));

        // Prepare test data
        InitializeTestData();
    }
    [TestCleanup]
    public void Cleanup()
    {
        // Clean up test data
        var videoCollection = _testDatabase.GetCollection<Video>("Videos");
        videoCollection.DeleteMany(FilterDefinition<Video>.Empty);
    }
    private void InitializeTestData()
    {
        var videoCollection = _testDatabase.GetCollection<Video>("Videos");

        // Clear existing data
        videoCollection.DeleteMany(FilterDefinition<Video>.Empty);

        // Insert sample data
        var testVideo = new Video
        {
            Id = Guid.NewGuid(),
            Title = "Test Video",
            Url = "https://example.com"
        };

        videoCollection.InsertOne(testVideo);
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnVideo_WhenVideoExists()
    {
        // Arrange
        var videoCollection = _testDatabase.GetCollection<Video>("Videos");
        var testVideo = videoCollection.Find(FilterDefinition<Video>.Empty).FirstOrDefault();
        
        if (testVideo == null)
        {
            Assert.Fail("No test video found in the test database.");
        }
        
        // Act
        var result = await _videoRepository.GetByIdAsync(testVideo.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(testVideo);
    }
    [TestMethod]
    public async Task GetByIdAsync_ShouldThrowArgumentNullException_WhenIdIsEmpty()
    {
        // Arrange
        var emptyId = Guid.Empty;

        // Act & Assert
        var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _videoRepository.GetByIdAsync(emptyId));

        // Check that the exception message is as expected
        Assert.AreEqual("Id is empty - cannot get entity (Parameter 'id')", exception.Message);
    }
    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnNull_WhenVideoDoesNotExist()
    {
        // Arrange
        var nonExistentVideoId = Guid.NewGuid();

        // Act
        var result = await _videoRepository.GetByIdAsync(nonExistentVideoId);

        // Assert
        Assert.IsNull(result); // Ensure the result is null
    } 

    
    [TestMethod]
    public async Task GetAllAsync_ShouldReturnVideos_WhenVideosExist()
    {
        // Act
        var result = await _videoRepository.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count); // Expecting 1 video due to initialization
        Assert.AreEqual("Test Video", result.First().Title);
    } 
    [TestMethod]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoVideosExist()
    {
        // Cleanup to ensure no videos exist
        Cleanup();

        // Act
        var result = await _videoRepository.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count); // Expecting 0 videos since cleanup was called
    } 
    [TestMethod]
    public async Task CreateAsync_ShouldInsertVideo_WhenVideoIsValid()
    {
        // Arrange
        var video = new Video
        {
            Id = Guid.NewGuid(),
            Title = "Test Video",
            Url = "https://example.com"
        };

        // Act
        await _videoRepository?.CreateAsync(video)!;

        // Assert
        var videoCollection = _testDatabase?.GetCollection<Video>("Videos");
        var insertedVideo = await videoCollection.Find(v => v.Id == video.Id).FirstOrDefaultAsync();

        insertedVideo.Should().NotBeNull();
        insertedVideo.Should().BeEquivalentTo(video);
    }
    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateVideo_WhenVideoExists()
    {
        // Arrange
        var originalVideo = new Video
        {
            Id = Guid.NewGuid(),
            Title = "Original Title",
            Url = "https://example.com"
        };

        var updatedVideo = new Video
        {
            Id = originalVideo.Id,
            Title = "Updated Title",
            Url = "https://example.com/updated"
        };

        var videoCollection = _testDatabase?.GetCollection<Video>("Videos");
        await videoCollection?.InsertOneAsync(originalVideo)!;

        // Act
        await _videoRepository?.UpdateAsync(updatedVideo)!;

        // Assert
        var videoFromDb = await videoCollection.Find(v => v.Id == originalVideo.Id).FirstOrDefaultAsync();
        videoFromDb.Should().NotBeNull();
        videoFromDb.Title.Should().Be(updatedVideo.Title);
        videoFromDb.Url.Should().Be(updatedVideo.Url);
    }
    
    [TestMethod]
    public async Task DeleteAsync_ShouldDeleteVideo_WhenVideoExists()
    {
        // Arrange
        var video = new Video
        {
            Id = Guid.NewGuid(),
            Title = "Test Video",
            Url = "https://example.com"
        };

        var videoCollection = _testDatabase?.GetCollection<Video>("Videos");
        await videoCollection?.InsertOneAsync(video)!;

        // Act
        await _videoRepository?.DeleteAsync(video.Id)!;

        // Assert
        var videoFromDb = await videoCollection.Find(v => v.Id == video.Id).FirstOrDefaultAsync();
        videoFromDb.Should().BeNull();
    } 
    
} 
   

   
   