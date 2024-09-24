using MongoDB.Bson;
using MongoDB.Driver;

namespace Vita_WebAPI_Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly IMongoCollection<T> _collection;

    public GenericRepository(IMongoDatabase database, string collectionName)
    {
        // Assign the collection from the provided database
        _collection = database.GetCollection<T>(collectionName);
    }


    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await _collection.Find(new BsonDocument()).ToListAsync();
    }
    
    public async Task CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }
    
    public async Task UpdateAsync(Guid id, T entity)
    {
        await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), entity);
    }

    
    public async Task DeleteAsync(Guid id)
    {
        // Use the id parameter directly in the filter to match the document you want to delete
        var result = await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));

        // Optional: You can throw an exception or log if nothing was deleted
        if (result.DeletedCount == 0)
        {
            throw new Exception("No video found to delete.");
        }
    }
    
    public async Task<T> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id), "Id is empty - cannot get entity");
        }
 
        var filter = Builders<T>.Filter.Eq("Id", id); // Use the property name as a string
    
        var entity = await _collection.Find(filter).FirstOrDefaultAsync();
    
        return entity; // This will return null if no matching entity is found
    }
}