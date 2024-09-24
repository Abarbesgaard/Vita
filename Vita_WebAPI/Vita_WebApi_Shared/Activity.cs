namespace Vita_WebApi_Shared;


public class Activity : BaseEntity 
{ 
    public bool Accepted { get; set; }
    public bool Declined { get; set; }
    public bool Tentative { get; set; }
    public bool Cancelled { get; set; }
    public bool Rescheduled { get; set; }
    
}