import apiClient from "@/lib/apiClient";
import {mapPagedResultDtoToPagedResult, mapRecipeDtoToRecipe, mapRecipeSummaryDtoToRecipeSummary} from "./mappers";
import {GetAllRecipesRequestDto, PagedResultDto, RecipeDto, RecipeSummaryDto} from "./dto";
import {PagedResult, Recipe, RecipeSummary} from "../types";
import {ImagePickerAsset} from "expo-image-picker";
import {Platform} from "react-native";

const createUploadFormData = async (assets: ImagePickerAsset[]) => {

    const formData = new FormData();

    for (const asset of assets) {
        if (Platform.OS === 'web') {
            const response = await fetch(asset.uri);
            const blob = await response.blob();

            formData.append('images', blob);
        } else {
            const uriParts = asset.uri.split('.');
            const fileType = uriParts[uriParts.length - 1];

            formData.append('images', {
                uri: asset.uri,
                name: asset.fileName || `photo.${fileType}`,
                type: asset.mimeType || `image/${fileType}`,
            } as any); 
        }
    }

    return formData;
};

export const postRecipeAnalysis = async (assets: ImagePickerAsset[]): Promise<string> => {

    const formData = await createUploadFormData(assets);

    const response = await apiClient.post('/recipeanalysis', formData, {
        formSerializer: {indexes: null}, headers: {
            'Content-Type': 'multipart/form-data',
        }
    })
    return response.data;
}
export const getRecipes = async (params: GetAllRecipesRequestDto): Promise<PagedResult<RecipeSummary>> => {
    const response = await apiClient.get<PagedResultDto<RecipeSummaryDto>>('/recipes', {
        params: params
    })

    return mapPagedResultDtoToPagedResult(response.data, mapRecipeSummaryDtoToRecipeSummary);
}

export const getRecipeById = async (id: string): Promise<Recipe> => {
    const response = await apiClient.get<RecipeDto>(`/recipes/${id}`)

    return mapRecipeDtoToRecipe(response.data);
}