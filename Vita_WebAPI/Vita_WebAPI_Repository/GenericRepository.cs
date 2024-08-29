using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Vita_WebAPI_Data;
using Vita_WebApi_Shared;

namespace Vita_WebAPI_Repository;

public class GenericRepository<T>
    : IGenericRepository<T> where T : BaseEntity
{
    private readonly IMongoCollection<T> _dbCollection;
    private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;
    private readonly ILogger<GenericRepository<T>> _logger;


    public GenericRepository(IMongoDatabase database, string collectionName, ILogger<GenericRepository<T>> logger)
    {
        _dbCollection = database.GetCollection<T>(collectionName);
        
    }
     public async Task<T> GetByIdAsync(Guid id)
       {
           var filter = _filterBuilder.Eq(entity => entity.Id, id);
           return await _dbCollection.Find(filter).FirstOrDefaultAsync();
       }
       public async Task<IReadOnlyCollection<T>> GetAllAsync()
       {
           try
           {
               return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync(); 
           }
           catch (Exception e)
           {
               _logger.LogError(e, "An error occurred while getting all entities of type {T}", nameof(Video));
               throw;
           }
       }
   
       public async Task CreateAsync(T entity)
       {
           if (entity == null)
           {
               throw new ArgumentNullException(nameof(entity), "Entity is null - cannot create");
           }
   
           try
           {
               await _dbCollection.InsertOneAsync(entity);
           }
           catch (Exception e)
           {
               _logger.LogError(e, "An error occurred while creating the entity: {@Entity}", entity);
               throw;
           }
       }
       
       public async Task UpdateAsync(T entity)
       {
           if (entity == null)
           {
               throw new ArgumentNullException(nameof(entity), "Entity is null - cannot update");
           }
   
           try
           {
               var filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
               await _dbCollection.ReplaceOneAsync(filter, entity);
           }
           catch (Exception e)
           {
               _logger.LogError(e, "An error occurred while updating the entity: {@Entity}", entity);
               throw;
           }
       }
       
       public async Task DeleteAsync(Guid id)
       {
           var filter = _filterBuilder.Eq(entity => entity.Id, id);
           await _dbCollection.DeleteOneAsync(filter);
       }
}
    
