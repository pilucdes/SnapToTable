export interface Recipe {
    id: string;
    createdAt: Date;
    recipeAnalysisId: string;
    name: string;
    category: string;
    prepTimeInMinutes: number | null;
    cookTimeInMinutes: number | null;
    additionalTimeInMinutes: number | null;
    totalTimeInMinutes: number | null;
    servings: number | null;
    ingredients: readonly string[];
    directions: readonly string[];
    notes: readonly string[];
}