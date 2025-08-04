export interface RecipeSummaryDto {
    id: string;
    createdAt: string;
    recipeAnalysisId: string;
    name: string;
    category: string;
    url: string | null;
    readonly ingredients: readonly string[];
}