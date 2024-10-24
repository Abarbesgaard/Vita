namespace Vita_WebApi_API.Dto;

public record ActivityDto
{
    public Guid Id { get; init; }
    
    public Guid UserId { get; init; }
    
    public DateTimeOffset Start { get; init; }
    
    public string Title { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;

    public DateTimeOffset End { get; init; }

    public Guid HostId { get; init; } // Den der har oprettet aktiviteten

    public ICollection<UserDto>? Attendee { get; init; } // Dem der kan deltager i aktiviteten

    public ICollection<UserDto>? VerifiedAttendee { get; init; } // Dem der har accepteret invitationen

    public bool Cancelled { get; init; } = false;

    public bool AllDayEvent { get; init; } = false;
}

