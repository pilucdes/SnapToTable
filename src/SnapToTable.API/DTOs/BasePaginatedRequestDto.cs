namespace SnapToTable.API.DTOs;

public record BasePaginatedRequestDto(
    int Page = 1,
    int PageSize = 20);