namespace SnapToTable.Application.Features.Common;

public interface IPaginatedQuery
{
    int Page { get; }
    int PageSize { get; }
}