export default class PagedResult<T> {
    public readonly items: readonly T[];
    public readonly totalCount: number;
    public readonly page: number;
    public readonly pageSize: number;

    constructor(items: readonly T[], totalCount: number, page: number, pageSize: number) {
        this.items = items;
        this.totalCount = totalCount;
        this.page = page;
        this.pageSize = pageSize;
    }

    public get totalPages(): number {
        return Math.ceil(this.totalCount / this.pageSize);
    }

    public get hasNextPage(): boolean {
        return this.page < this.totalPages;
    }

    public get hasPreviousPage(): boolean {
        return this.page > 1;
    }
}