using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Vita_WebApi_Shared;


public class Activity : BaseEntity 
{ 
    [JsonPropertyName("Time_start")]
    [DataType(DataType.DateTime)]
    public DateTimeOffset Start { get; set; }
    
    [JsonPropertyName("Time_end")]
    [DataType(DataType.DateTime)]
    public DateTimeOffset End { get; set; }
    
    [JsonPropertyName("Title")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
    public string? Title { get; set; }
    
    [JsonPropertyName("Description")]
    public string Description { get; set; }
    [StringLength(3000, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 3000 characters")]
    
    [JsonPropertyName("HostId")]
    [Required(ErrorMessage = "HostId is required")]
    public Guid HostId { get; set; } // Den der har oprettet aktiviteten
    
    [JsonPropertyName("Attendees")]
    public ICollection<Users>? Attendee { get; set; } // Dem der kan deltager i aktiviteten
   
    [JsonPropertyName("VerifiedAttendees")]
    public ICollection<Users>? VerifiedAttendee { get; set; } // Dem der har accepteret invitationen
    
    [JsonPropertyName("Cancelled")] 
    public bool Cancelled { get; set; } = false;
   
    [JsonPropertyName("AllDayEvent")]
    public bool AllDayEvent { get; set; } = false;
}
