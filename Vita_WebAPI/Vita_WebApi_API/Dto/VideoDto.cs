using System.ComponentModel.DataAnnotations;

namespace Vita_WebApi_API.Dto;

public record UpdateVideoDto(
    string? Title, 
    string? Description, 
    string? Url);
public record CreateVideoDto(
    string? CreatedBy,
    string? UpdatedBy,
    DateTimeOffset CreatedAt, 
    DateTimeOffset UpdatedAt, 
    string? Title, 
    string? Description,
    [Required] string? Url);

public record GetVideoDto(
    Guid Id, 
    DateTimeOffset CreatedAt, 
    DateTimeOffset UpdatedAt, 
    string? Title, 
    string? Description,
    string? Url);
public record VideoDto(
    Guid Id, 
    DateTimeOffset CreatedAt, 
    DateTimeOffset UpdatedAt, 
    string? Title, 
    string? Description,
    string? Url);
public class VideoAuditLogDto
{
    public string? Title { get; set; }
    public string? Url { get; set; }
}
 