namespace Vita_WebApi_API.Dto;

public record ActivityDto(
    Guid Id,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    string? Title,
    string? Description,
    bool Accepted,
    bool Tentative,
    bool Cancelled,
    bool Rescheduled
);

