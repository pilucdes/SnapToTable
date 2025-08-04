export interface RecipeDto {
    id: string;
    createdAt: string;
    recipeAnalysisId: string;
    name: string;
    category: string;
    url: string | null;
    prepTime: string | null;
    cookTime: string | null;
    additionalTime: string | null;
    servings: number | null;
    
    readonly ingredients: readonly string[];
    readonly directions: readonly string[];
    readonly notes: readonly string[];
}