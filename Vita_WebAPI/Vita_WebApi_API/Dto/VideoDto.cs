namespace Vita_WebApi_API.Dto;

public class VideoDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
}