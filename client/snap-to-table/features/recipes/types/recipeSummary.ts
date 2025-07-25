export interface RecipeSummary {
    id: string;
    createdAt: Date;
    recipeAnalysisId: string;
    name: string;
    category: string;
    ingredients: readonly string[];
}