using System.Text.Json.Serialization;

namespace Vita_WebApi_Shared;

public abstract class BaseEntity
{ 
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

}