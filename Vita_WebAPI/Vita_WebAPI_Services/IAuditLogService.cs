using MongoDB.Driver;

namespace Vita_WebAPI_Services;

public interface IAuditLogService
{
   Task LogAsync(AuditLog auditLog);
}

public abstract class AuditData
{
    public Guid? Id { get; set; }
}
public class AuditLog
{
      public Guid UserId { get; set; }
      public string? Operation { get; set; }
      public string? Collection { get; set; }
      public Guid? DocumentId { get; set; }
      public DateTimeOffset Timestamp { get; set; }
      
}

public class AuditLogService: IAuditLogService
{
    private readonly IMongoCollection<AuditLog> _auditLogsCollection;

    
    public AuditLogService(IMongoDatabase database)
    {
        _auditLogsCollection = database.GetCollection<AuditLog>("AuditLogs") 
                               ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task LogAsync(AuditLog auditLog)
    {
        ArgumentNullException.ThrowIfNull(auditLog);
        
        await _auditLogsCollection.InsertOneAsync(auditLog);
    }
}