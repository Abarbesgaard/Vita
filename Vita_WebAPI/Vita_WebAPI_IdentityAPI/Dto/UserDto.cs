namespace Vita_WebAPI_IdentityAPI.Dto;

public class UserDto
{ 
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public IList<string>? Roles { get; set; }
}