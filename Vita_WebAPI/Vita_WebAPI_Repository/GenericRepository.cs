using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Vita_WebAPI_Data;

namespace Vita_WebAPI_Repository;

public class GenericRepository<T>(DataContext context, ILogger<GenericRepository<T>> logger) 
    : IGenericRepository<T> where T : class
{
    public async Task DeleteAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity is null - cannot delete");
        }

        try
        {
            logger.LogInformation($"Deleting entity of type {entity.GetType().Name}");
        context.Remove(entity);
        await context.SaveChangesAsync();
            
        }
        catch (DbUpdateConcurrencyException e)
        {
            logger.LogError(e, "Concurrency error occurred while deleting the entity: {@Entity}", entity);
            throw;
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Error occurred while deleting the entity: {@Entity}", entity);
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while deleting the entity: {@Entity}", entity);
            throw;
        }
    }

    public async Task<List<T>> GetAllAsync()
    {
        try
        {
            
            if (context == null)
            {
                logger.LogError("Context is null");
                throw new ArgumentNullException(nameof(context), "Context is null");
            }
            logger.LogInformation("returning all entities of type {T}", typeof(T).Name);
            var entities = await context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
            logger.LogInformation("Returning {Count} entities of type {T}", entities.Count, typeof(T).Name);
            return entities;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting all entities of type {T}", typeof(T).Name);
            throw;
        }
    }
    public async Task AddAsync(T entity)
    {
        if (entity == null)
        {
            logger.LogError("Entity is null - cannot add");
            throw new ArgumentNullException(nameof(entity), "Entity is null - cannot add");
        }
        logger.LogInformation($"Adding entity of type {entity.GetType().Name}");
        try
        {
            logger.LogInformation("Try: Adding entity of type {T}", typeof(T).Name);
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            logger.LogInformation(" Success: Entity of type {T} added", typeof(T).Name);
        }
        catch (DbUpdateException dbex)
        {
            logger.LogError(dbex," Error: An error occurred while adding entity of type {T}", typeof(T).Name);
            throw new InvalidOperationException("Error occurred while adding entity", dbex);
        }
        catch (Exception e)
        {
            logger.LogError(e, " Error: An error occurred while adding entity of type {T}", typeof(T).Name);
            throw;
        } 
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            logger.LogError("Id is less than or equal to 0");
            throw new ArgumentOutOfRangeException(nameof(id), "Id is less than or equal to 0");
        }

        try
        {
            logger.LogInformation("fetching entity of type {T} with id {Id}", typeof(T).Name, id);
        var entity = await context.FindAsync<T>(id);
        if (entity == null)
        {
            logger.LogWarning("No entity found with id {Id}", id);
            return null;
        }
        return entity;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while fetching entity of type {T} with id {Id}", typeof(T).Name, id);
            throw;
        }
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity == null)
        {
            logger.LogError("Entity is null - cannot update");
            throw new ArgumentNullException(nameof(entity), "Entity is null - cannot update");
        }

        try
        {
            logger.LogInformation("Updating entity of type {T}", typeof(T).Name);
        context.Update(entity);
        await context.SaveChangesAsync();
        logger.LogInformation(" Entity of type {T} updated", typeof(T).Name);
        }
        catch (DbUpdateConcurrencyException e)
        {
            logger.LogWarning(e, "Concurrency error occurred while updating the entity: {@Entity}", entity);
            throw;
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Error occurred while updating the entity: {@Entity}", entity);
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while updating the entity: {@Entity}", entity);
            throw;
        }
    }
}