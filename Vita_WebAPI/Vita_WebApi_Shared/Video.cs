namespace Vita_WebApi_Shared;

public class Video : BaseEntity
{
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
}