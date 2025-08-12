export interface ApiException {
    type: string;
    title: string; 
    status: number;
    exceptionType: string;
    traceId?: string;
    errors: Record<string, string[]>;
}