export interface RecipeSummaryDto {
    id: string;
    createdAt: string;
    recipeAnalysisId: string;
    name: string;
    category: string;
    readonly ingredients: readonly string[];
}