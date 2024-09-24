using FluentAssertions;
using MongoDB.Driver;
using Vita_WebAPI_Repository;
using Vita_WebApi_Shared;

namespace Vita_Test;

[TestClass]
public class RepositoryTests
{
    private IGenericRepository<Video>? _videoRepository;
    private IMongoDatabase? _testDatabase;
    private IMongoClient? _mongoClient;
    private const string TestDatabaseName = "VideoRepositoryTestDatabase";

    [TestInitialize]
    public void Setup()
    {
        const string connectionString = "mongodb://localhost:27017";
        _mongoClient = new MongoClient(connectionString); 
        _testDatabase = _mongoClient.GetDatabase(TestDatabaseName);
       
        _videoRepository = new GenericRepository<Video>(_testDatabase);
         
        InitializeTestData();
    }
    [TestCleanup]
    public void Cleanup()
    {
        // Drop the test database after each test
        _testDatabase?.Client.DropDatabase(TestDatabaseName);
    }
    private void InitializeTestData()
    {
        var videoCollection = _testDatabase?.GetCollection<Video>("Videos");

        // Clear existing data
        videoCollection?.DeleteMany(FilterDefinition<Video>.Empty);

        // Insert sample data
        var testVideo = new Video
        {
            Id = Guid.NewGuid(),
            Title = "Test Video",
            Url = "https://example.com"
        };

        videoCollection?.InsertOne(testVideo);
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnVideo_WhenVideoExists()
    {
        // Arrange
        var videoCollection = _testDatabase?.GetCollection<Video>("Videos");
        var testVideo = videoCollection.Find(FilterDefinition<Video>.Empty).FirstOrDefault();
        Console.WriteLine(testVideo.Id); 
        if (testVideo == null)
        {
            Assert.Fail("No test video found in the test database.");
        }
        
        // Act
        var result = await _videoRepository?.GetByIdAsync(testVideo.Id)!;
        // Assert
        result.Should().BeEquivalentTo(testVideo);
    }
    [TestMethod]
    public async Task GetByIdAsync_ShouldThrowArgumentNullException_WhenIdIsEmpty()
    {
        // Arrange
        var emptyId = Guid.Empty;

        // Act & Assert
        var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _videoRepository!.GetByIdAsync(emptyId));

        // Check that the exception message is as expected
        Assert.AreEqual("Id is empty - cannot get entity (Parameter 'id')", exception.Message);
    }
    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnNull_WhenVideoDoesNotExist()
    {
        // Arrange
        var nonExistentVideoId = Guid.NewGuid();

        // Act
        var result = await _videoRepository?.GetByIdAsync(nonExistentVideoId)!;

        // Assert
        Assert.IsNull(result); // Ensure the result is null
    } 

    
    [TestMethod]
    public async Task GetAllAsync_ShouldReturnVideos_WhenVideosExist()
    {
        // Act
        var result = await _videoRepository?.GetAllAsync()!;

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
        var result = await _videoRepository?.GetAllAsync()!;

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

        insertedVideo.Should().NotBeNull( "Video should be inserted into the database");
        insertedVideo.Should().BeEquivalentTo(video);
    }
    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateVideo_WhenVideoExists()
    {
        var videoCollection = _testDatabase?.GetCollection<Video>("Videos");
        var testVideo = videoCollection.Find(FilterDefinition<Video>.Empty).FirstOrDefault(); 
        // Arrange
        var updatedVideo = new Video
        {
            Id = testVideo.Id, // Ensure we are updating the same video
            Title = "Updated Title",
            Url = "https://example.com/updated"
        };

        // Act
        await _videoRepository?.UpdateAsync(testVideo.Id, updatedVideo)!;

        // Assert
        var videoFromDb = await _videoRepository.GetByIdAsync(testVideo.Id);
        videoFromDb.Should().NotBeNull();
        videoFromDb.Title.Should().Be("Updated Title");
        videoFromDb.Url.Should().Be("https://example.com/updated");
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
   

   
   