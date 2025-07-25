import {useQuery, useMutation} from '@tanstack/react-query';
import {getRecipes, getRecipeById, postRecipeAnalysis} from '../api/recipeApi';
import {router} from 'expo-router';
import {CreateRecipeAnalysisRequestDto, GetAllRecipesRequestDto } from '../api/dto';

export const useGetAllRecipes = (params: GetAllRecipesRequestDto) => {
    return useQuery({
        queryKey: ['getAllRecipes', params],
        queryFn: () => getRecipes(params)
    })
}
export const useGetRecipeById = (id: string) => {
    return useQuery({
        queryKey: ['getRecipeById', id],
        queryFn: () => getRecipeById(id)
    })
}
export const useCreateRecipeAnalysis = () => {
    return useMutation({
        mutationFn: (payload: CreateRecipeAnalysisRequestDto) => postRecipeAnalysis(payload),
        onSuccess: (analysisId) => {
            console.log('Post created successfully:', analysisId);

            router.push(`/recipes?recipeAnalysisId=${analysisId}`);
        },
        onError: (error: Error) => {
            console.error('Failed to create post:', error.message);
        }
    })
}