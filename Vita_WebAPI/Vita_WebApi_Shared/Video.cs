using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Vita_WebApi_Shared;
/// <summary>
/// Defines the properties of a video.
/// </summary>
public class Video : BaseEntity
{

    [JsonPropertyName("url")]
    [StringLength(2083, MinimumLength = 1, ErrorMessage = "Url must be between 1 and 2083 characters")]
    public required string? Url { get; set; }
}