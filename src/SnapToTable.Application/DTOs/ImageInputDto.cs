namespace SnapToTable.Application.DTOs;

public record ImageInputDto(
    Stream Content,
    string ContentType
);