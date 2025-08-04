export interface RecipeSummary {
    id: string;
    createdAt: Date;
    recipeAnalysisId: string;
    name: string;
    category: string;
    url: string | null;
    ingredients: readonly string[];
}