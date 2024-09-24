namespace Vita_WebApi_Shared;

public class Meeting : BaseEntity
{
    public Guid? HostId { get; set; }
    public Guid? GuestId { get; set; }
    
}