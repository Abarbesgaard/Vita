using System.Diagnostics;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Vita_WebAPI_Repository;

public class GenericRepository<T>(IMongoDatabase database, ILogger<GenericRepository<T>> logger)
    : IGenericRepository<T>
    where T : class
{
    private readonly IMongoCollection<T> _collection = database.GetCollection<T>(
        typeof(T).Name + "s"
    );

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        const int maxResults = 100;
        var stopwatch = Stopwatch.StartNew();

        var timeoutTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        try
        {
            logger.LogInformation("Getting all entities from the collection");
            var results = await _collection
                .Find(new BsonDocument())
                .Limit(maxResults)
                .ToListAsync(timeoutTokenSource.Token);

            logger.LogInformation("Retrieved {Count} entities from the collection", results.Count);

            return results;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting all entities from the collection");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation(
                "GetAllAsync took {ElapsedMilliseconds} ms ",
                stopwatch.ElapsedMilliseconds
            );
        }
    }

    public async Task? CreateAsync(T entity)
    {
        var stopwatch = Stopwatch.StartNew();
        if (entity == null)
        {
            logger.LogError($"Entity is null - cannot create {nameof(entity)}");
            throw new ArgumentNullException(
                nameof(entity),
                $"{nameof(entity)} is null - cannot create {nameof(entity)}"
            );
        }

        try
        {
            await _collection.InsertOneAsync(entity);
        }
        catch (MongoWriteException e)
        {
            logger.LogError(e, "Error writing entity to the collection");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation(
                "CreateAsync took {ElapsedMilliseconds} ms ",
                stopwatch.ElapsedMilliseconds
            );
        }
    }

    public async Task UpdateAsync(Guid id, T entity)
    {
        var stopwatch = Stopwatch.StartNew();
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id), "Id is empty - cannot update entity");
        }

        if (entity == null)
        {
            throw new ArgumentNullException(
                nameof(entity),
                "Entity is null - cannot update entity"
            );
        }

        try
        {
            var existingEntity = await _collection
                .Find(Builders<T>.Filter.Eq("_id", id))
                .FirstOrDefaultAsync();
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"{nameof(entity)} with ID {id} not found.");
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting existing entity from the collection");
            throw;
        }

        try
        {
            var result = await _collection.ReplaceOneAsync(
                Builders<T>.Filter.Eq("_id", id),
                entity
            );
            if (result.MatchedCount == 0)
            {
                logger.LogError("No entity found to update");
                throw new KeyNotFoundException($"{nameof(entity)} with ID {id} not found.");
            }
        }
        catch (MongoWriteException e)
        {
            logger.LogError(e, "Error writing entity to the collection");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation(
                "UpdateAsync took {ElapsedMilliseconds} ms ",
                stopwatch.ElapsedMilliseconds
            );
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var stopwatch = Stopwatch.StartNew();
        if (id == Guid.Empty)
        {
            logger.LogError("Id is empty - cannot delete entity");
            throw new ArgumentNullException(nameof(id), "Id is empty - cannot delete entity");
        }

        try
        {
            // Use the id parameter directly in the filter to match the document you want to delete
            var result = await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));

            // Optional: You can throw an exception or log if nothing was deleted
            if (result.DeletedCount == 0)
            {
                throw new Exception("No video found to delete.");
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting entity from the collection");
            throw new Exception("Error deleting entity from the collection", e);
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation(
                "DeleteAsync took {ElapsedMilliseconds} ms ",
                stopwatch.ElapsedMilliseconds
            );
        }
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var stopwatch = Stopwatch.StartNew();
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id), "Id is empty - cannot get entity");
        }

        try
        {
            var filter = Builders<T>.Filter.Eq("_id", id); // Use the property name as a string

            var entity = await _collection.Find(filter).FirstOrDefaultAsync();
            if (entity == null)
            {
                logger.LogError("No entity found to get");
                return null;
            }

            return entity; // This will return null if no matching entity is found
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting entity from the collection");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation(
                "GetByIdAsync took {ElapsedMilliseconds} ms ",
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}

