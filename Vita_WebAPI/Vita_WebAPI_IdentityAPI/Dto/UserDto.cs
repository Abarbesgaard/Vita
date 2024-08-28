namespace Vita_WebApi_API.Dto;

public class UserDto
{ 
    public string Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public IList<string> Roles { get; set; }
}