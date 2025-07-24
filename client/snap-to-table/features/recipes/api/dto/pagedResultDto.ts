export default interface PagedResultDto<T> {
    items: readonly T[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
}