namespace SnapToTable.Domain.Common;

public record PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public long TotalCount { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public PagedResult(IReadOnlyList<T> items, long totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }
}