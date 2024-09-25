using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Vita_WebApi_Shared;

public interface IBaseEntity
{
    Guid Id { get; }
    string CreatedBy { get; } 
}
public abstract class BaseEntity : IBaseEntity
{ 
    [BsonId]
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
    [JsonPropertyName("title")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
    public string? Title { get; set; }
    [JsonPropertyName("description")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 500 characters")]
    public string? Description { get; set; }
    
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; init; }
    
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; init; }
    
    [JsonPropertyName("createdBy")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = " UpdatedBy must be between 1 and 200 characters")]
    public string? CreatedBy { get; set; }
    
    [JsonPropertyName("updatedBy")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = " UpdatedBy must be between 1 and 200 characters")]
    public string? UpdatedBy { get; set; }
    
    

}