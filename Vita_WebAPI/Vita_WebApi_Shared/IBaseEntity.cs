namespace Vita_WebApi_Shared;

public interface IBaseEntity
{
    Guid Id { get; }
    string? CreatedBy { get; } 
    string? UpdatedBy { get; }
    DateTimeOffset CreatedAt { get; }
    DateTimeOffset UpdatedAt { get; }
    string? Title { get; }
    string? Description { get; }
    
}