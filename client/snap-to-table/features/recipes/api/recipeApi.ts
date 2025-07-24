import apiClient from "@/lib/apiClient";
import CreateRecipeAnalysisRequestDto from "./dto/createRecipeAnalysisRequestDto";
import GetAllRecipesRequestDto from "./dto/getAllRecipesRequestDto";
import RecipeDto from "./dto/recipeDto";
import PagedResultDto from "./dto/pagedResultDto";
import PagedResult from "../types/pagedResult";
import Recipe from "../types/recipe";
import {mapPagedResultDtoToPagedResult, mapRecipeDtoToRecipe} from "./mappers";

export const postRecipeAnalysis = async (payload: CreateRecipeAnalysisRequestDto): Promise<string> => {
    const response = await apiClient.postForm('/recipeanalysis', payload, {formSerializer: {indexes: null}})
    return response.data;
}
export const getRecipes = async (params: GetAllRecipesRequestDto): Promise<PagedResult<Recipe>> => {
    const response = await apiClient.get<PagedResultDto<RecipeDto>>('/recipes', {
        params: params
    })

    return mapPagedResultDtoToPagedResult(response.data, mapRecipeDtoToRecipe);
}