namespace Vita_WebApi_Shared;

public class Meeting : BaseEntity
{
    public Guid? HostId { get; set; }
    public Guid? GuestId { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public bool Accepted { get; set; }
    public bool Declined { get; set; }
    public bool Tentative { get; set; }
    public bool Cancelled { get; set; }
    public bool Rescheduled { get; set; } 
}