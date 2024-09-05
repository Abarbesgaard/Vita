using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Vita_WebApi_Shared;
/// <summary>
/// Defines the properties of a video.
/// </summary>
public class Video : BaseEntity
{
    [JsonPropertyName("updatedBy")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = " UpdatedBy must be between 1 and 200 characters")]
    public string? UpdatedBy { get; set; }
    
    [JsonPropertyName("createdBy")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = " UpdatedBy must be between 1 and 200 characters")]
    public string? CreatedBy { get; set; }
    
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; init; }
    
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; init; }
    
    [JsonPropertyName("title")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
    public string? Title { get; set; }
    
    [JsonPropertyName("description")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 500 characters")]
    public string? Description { get; set; }
    
    [JsonPropertyName("url")]
    [StringLength(2083, MinimumLength = 1, ErrorMessage = "Url must be between 1 and 2083 characters")]
    public required string? Url { get; set; }
}