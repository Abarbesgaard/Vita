using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Vita_WebApi_Shared;

public class Users
{
    [BsonId]
    [JsonPropertyName( "id_User" )]
    public Guid Id { get; set; }
    
    [JsonPropertyName( "name_User" )]
    [StringLength( 200, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 200 characters" )]
    public string? Name { get; set; }
    
    [JsonPropertyName( "role_User" )]
    [Required(ErrorMessage = "Role is required")]
    public string? Role { get; set; }
}