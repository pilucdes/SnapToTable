export default interface RecipeDto {
    
    id: string;
    createdAt: string;
    recipeAnalysisId: string;
    name: string;
    category: string;
    prepTime: string | null;
    cookTime: string | null;
    additionalTime: string | null;
    servings: number | null;
    
    readonly ingredients: readonly string[];
    readonly directions: readonly string[];
    readonly notes: readonly string[];
}