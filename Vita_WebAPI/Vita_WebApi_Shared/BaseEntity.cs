using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Vita_WebApi_Shared;

public abstract class BaseEntity : IBaseEntity
{ 
    [BsonId]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("UserId")]
    public Guid UserId { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
   
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; init; }
    
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; init; }
    
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; init; }
    
    [JsonPropertyName("updatedBy")]
    public string? UpdatedBy { get; set; }
    
    

}