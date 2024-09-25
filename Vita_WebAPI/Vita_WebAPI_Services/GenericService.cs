using Microsoft.Extensions.Logging;
using Vita_WebAPI_Repository;
using Vita_WebApi_Shared;

namespace Vita_WebAPI_Services;

public class GenericService<T>(
    IGenericRepository<T>? repository,
    ILogger<GenericService<T>> logger,
    IAuditLogService auditLogService)
    : IGenericService<T>
    where T : class, IBaseEntity
{

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        logger.LogInformation("Getting all entities");

        var result = await repository?.GetAllAsync()!;

        // Log the audit, but only if there are entities
        if (result.Any())
        {
            await LogAuditLogAsync(result, "GetAll"); // Consider changing this method to handle collections
        }
        else
        {
            logger.LogInformation("No entities found during GetAll");
        }

        return result;
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        try
        {
            logger.LogInformation($" Getting entity with id: {id}");
            var entity = await repository?.GetByIdAsync(id)!;
        
            if (entity == null)
            {
                logger.LogWarning("Entity with ID: {Id} not found", id);
                throw new KeyNotFoundException($"Entity with ID: {id} not found");
            } 
            await LogAuditLogAsync(entity, "GetById");
            return entity;
            
        }
        catch (Exception e)
        {
            logger.LogError("An error occurred: {0}", e.Message);
            throw;
        }
    }

    public async Task? CreateAsync(T entity)
    {
        try
        {
            logger.LogInformation("Creating a new entity");
            if (repository != null) await repository.CreateAsync(entity)!;
            logger.LogInformation("Entity created");
            await LogAuditLogAsync( entity, "Create");
           
        }
        catch (Exception e)
        {
            logger.LogError("An error occurred: {0}", e.Message);
            throw;
        } 
    }
    
    public async Task UpdateAsync(Guid id, T entity)
    {
        logger.LogInformation("Updating entity");
        await repository?.UpdateAsync(id, entity)!;
        await LogAuditLogAsync( entity, "Update");
    }

    public async Task DeleteAsync(Guid id)
    {
        logger.LogInformation("Deleting entity");
        var entityToDelete = await repository?.GetByIdAsync(id)!;
        if (entityToDelete == null)
        {
            logger.LogWarning("Entity with ID: {Id} not found", id);
            throw new KeyNotFoundException($"Entity with ID: {id} not found");
        } 
        await repository.DeleteAsync(id);
        await LogAuditLogAsync( entityToDelete, "Delete");
    }

    private async Task LogAuditLogAsync(T entity, string operation)
    {
        var auditLog = new AuditLog
        {
            UserId = entity.Id,
            Operation = operation,
            Collection = typeof(T).Name + "s",
            DocumentId = entity.Id,
            Timestamp = DateTimeOffset.UtcNow
        };
        try
        {
            await auditLogService.LogAsync(auditLog);
        }
        catch (Exception e)
        {
            logger.LogError("An error occurred: {0}", e.Message);
            throw;
        }
    }
    private async Task LogAuditLogAsync(IReadOnlyCollection<T> entities, string operation)
    {
        var auditLog = new AuditLog
        {
            UserId = entities.First().Id, 
            Operation = operation,
            Collection = typeof(T).Name + "s",
            DocumentId = Guid.Empty, 
            Timestamp = DateTimeOffset.UtcNow
        };
        try
        {
            await auditLogService.LogAsync(auditLog);
        }
        catch (Exception e)
        {
            logger.LogError("An error occurred while logging audit: {0}", e.Message);
            throw;
        }
    }
 
}