import {useQuery, useMutation} from '@tanstack/react-query';
import {getRecipes, postRecipeAnalysis} from '../api/recipeApi';
import GetAllRecipesRequestDto from '../api/dto/getAllRecipesRequestDto';
import CreateRecipeAnalysisRequestDto from '../api/dto/createRecipeAnalysisRequestDto';
import {router} from 'expo-router';

export const useGetAllRecipes = (params: GetAllRecipesRequestDto) => {
    return useQuery({
        queryKey: ['getAllRecipes', params],
        queryFn: () => getRecipes(params)
    })
}

export const useCreateRecipeAnalysis = () => {
    return useMutation({
        mutationFn: (payload: CreateRecipeAnalysisRequestDto) => postRecipeAnalysis(payload),
        onSuccess: (analysisId) => {
            console.log('Post created successfully:', analysisId);

            router.push(`/recipes/recipes?recipeAnalysisId=${analysisId}`);
        },
        onError: (error: Error) => {
            console.error('Failed to create post:', error.message);
        }
    })
} 