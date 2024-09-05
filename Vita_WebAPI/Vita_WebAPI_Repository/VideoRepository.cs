using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Vita_WebAPI_Data;
using Vita_WebApi_Shared;

namespace Vita_WebAPI_Repository;
/// <summary>
/// Repository for managing video operations
/// </summary>
public class VideoRepository : IVideoRepository
{
    /// <summary>
    /// The name of the collection
    /// </summary>
    private const string CollectionName = "Videos";
    /// <summary>
    /// The database collection
    /// </summary>
    private readonly IMongoCollection<Video> _dbCollection;
    /// <summary>
    /// The filter builder
    /// </summary>
    private readonly FilterDefinitionBuilder<Video> _filterBuilder = Builders<Video>.Filter;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<VideoRepository> _logger;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"> The logger</param>
    /// <param name="videoDatabaseSetting"> The video database settings</param>
    public VideoRepository(ILogger<VideoRepository> logger, IOptions<VideoDatabaseSetting> videoDatabaseSetting)
    {
        _logger = logger;
        var mongoClient = new MongoClient(videoDatabaseSetting.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(videoDatabaseSetting.Value.DatabaseName);
        _dbCollection = mongoDatabase.GetCollection<Video>(CollectionName);
    } 
    /// <summary>
    /// Retrieves a video by its unique identifier
    /// </summary>
    /// <param name="id"> The unique identifier of the video to retrieve</param>
    /// <returns> A task representing the asynchronous operation</returns>
    /// <exception cref="ArgumentNullException">Thrown when the id is empty</exception>
    public async Task<Video?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id), "Id is empty - cannot get entity");
        }

        try
        {
            var filter = _filterBuilder.Eq(entity => entity.Id, id);
            var video = await _dbCollection.Find(filter).FirstOrDefaultAsync();
            if (video != null) return video;
            _logger.LogWarning("No entity found with id {Id}", id);
            return null;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while getting entity with id {Id}", id);
            throw;
        }
    }
    /// <summary>
    /// Retrieves all videos
    /// </summary>
    /// <returns> A task representing the asynchronous operation</returns>
    public async Task<IReadOnlyCollection<Video>> GetAllAsync()
    {
        try
        {
            var videos =  await _dbCollection.Find(_filterBuilder.Empty).ToListAsync(); 
            
            return videos.AsReadOnly();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while getting all entities of type {T}", nameof(Video));
            throw;
        }
    }

    /// <summary>
    /// Creates a new video
    /// </summary>
    /// <param name="entity"> The video object to create</param>
    /// <exception cref="ArgumentNullException"> Thrown when the entity is null</exception>
    public async Task CreateAsync(Video entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity is null - cannot create");
        }

        try
        {
            await _dbCollection.InsertOneAsync(entity);
        }
        catch (MongoWriteException exception ) when (exception.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            _logger.LogWarning("Entity with id {Id} already exists", entity.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while creating the entity: {@Entity}", entity);
            throw;
        }
    }
    /// <summary>
    /// Updates an existing video
    /// </summary>
    /// <param name="entity"> The video object to update</param>
    /// <exception cref="ArgumentNullException"> Thrown when the entity is null</exception>
    /// <exception cref="KeyNotFoundException"> Thrown when the entity is not found</exception>
    /// <exception cref="ApplicationException"> Thrown when an error occurs</exception>
    /// <exception cref="MongoWriteException"> Thrown when a duplicate key error occurs</exception>
    /// <exception cref="Exception"> Thrown when an error occurs</exception>
    /// <returns> A task representing the asynchronous operation</returns>
    public async Task UpdateAsync(Video entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity is null - cannot update");
        }

        try
        {
            var filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            var result = await _dbCollection.ReplaceOneAsync(filter, entity);
            if (result.MatchedCount == 0)
            {
                _logger.LogWarning("No entity found with id {Id}", entity.Id);
                throw new KeyNotFoundException($"No entity found with id {entity.Id}");
            }
        }
        catch (MongoWriteException exception ) when (exception.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            _logger.LogWarning("Entity with id {Id} already exists", entity.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while updating the entity: {@Entity}", entity);
            throw;
        }
    }
    
    /// <summary>
    /// Deletes a video
    /// </summary>
    /// <param name="id"> The unique identifier of the video to delete</param>
    /// <exception cref="ArgumentNullException"> Thrown when the id is empty</exception>
    /// <exception cref="KeyNotFoundException"> Thrown when the entity is not found</exception>
    /// <exception cref="ApplicationException"> Thrown when an error occurs</exception>
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id), "Id is empty - cannot delete entity");
        }

        try
        {
            var filter = _filterBuilder.Eq(entity => entity.Id, id);
            var deleteResult = await _dbCollection.DeleteOneAsync(filter);
            if (deleteResult.DeletedCount == 0)
            {
                _logger.LogWarning("No entity found with id {Id}", id);
                throw new KeyNotFoundException($"No entity found with id {id}");
            }
        }
        catch (MongoException ex)
        {
            _logger.LogWarning(ex, "An error occurred while deleting entity with id {Id}", id);
            throw new ApplicationException("An error occurred while deleting entity");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while deleting entity with id {Id}", id);
            throw new ApplicationException("An error occurred while deleting entity");
            
        }
    }

}