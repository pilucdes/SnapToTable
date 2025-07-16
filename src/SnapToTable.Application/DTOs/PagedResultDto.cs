namespace SnapToTable.Application.DTOs;

public record PagedResultDto<T>
{
    public IReadOnlyList<T> Items { get; }
    public long TotalCount { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public PagedResultDto(IReadOnlyList<T> items, long totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }
}