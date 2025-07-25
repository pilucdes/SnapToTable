import apiClient from "@/lib/apiClient";
import {mapPagedResultDtoToPagedResult, mapRecipeDtoToRecipe,mapRecipeSummaryDtoToRecipeSummary} from "./mappers";
import { CreateRecipeAnalysisRequestDto, GetAllRecipesRequestDto, PagedResultDto, RecipeDto, RecipeSummaryDto } from "./dto";
import { PagedResult, Recipe, RecipeSummary } from "../types";

export const postRecipeAnalysis = async (payload: CreateRecipeAnalysisRequestDto): Promise<string> => {
    const response = await apiClient.postForm('/recipeanalysis', payload, {formSerializer: {indexes: null}})
    return response.data;
}
export const getRecipes = async (params: GetAllRecipesRequestDto): Promise<PagedResult<RecipeSummary>> => {
    const response = await apiClient.get<PagedResultDto<RecipeSummaryDto>>('/recipes', {
        params: params
    })

    return mapPagedResultDtoToPagedResult(response.data, mapRecipeSummaryDtoToRecipeSummary);
}

export const getRecipeById = async (id:string): Promise<Recipe> => {
    const response = await apiClient.get<RecipeDto>(`/recipes/${id}`)

    return mapRecipeDtoToRecipe(response.data);
}