import apiClient from "@/lib/apiClient";
import CreateRecipeAnalysisRequestDto from "./dto/createRecipeAnalysisRequestDto";
import GetAllRecipesRequestDto from "./dto/getAllRecipesRequestDto";

export const postRecipeAnalysis = async (payload: CreateRecipeAnalysisRequestDto): Promise<string> => {
    const response = await apiClient.postForm('/recipeanalysis', payload)
    return response.data;
}
export const getRecipes = async (params: GetAllRecipesRequestDto) => {
    const response = await apiClient.get('/recipes', {params: params})
    return response.data;
}