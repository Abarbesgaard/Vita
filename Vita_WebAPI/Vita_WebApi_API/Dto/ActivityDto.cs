namespace Vita_WebApi_API.Dto;

public record ActivityDto
(
	 Guid Id,

	 Guid UserId,

	 string Title,

	 string Description,

	 DateTimeOffset Start,

	 DateTimeOffset End,

	 Guid HostId, // Den der har oprettet aktiviteten

	 ICollection<UserDto>? Attendee, // Dem der kan deltager i aktiviteten

	 ICollection<UserDto>? VerifiedAttendee, // Dem der har accepteret invitationen

	 bool Cancelled,

	 bool AllDayEvent
);

