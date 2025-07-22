import {useQuery, useMutation} from '@tanstack/react-query';
import {getRecipes, postRecipeAnalysis} from '../api/api';
import GetAllRecipesRequestDto from '../api/dto/getAllRecipesRequestDto';
import CreateRecipeAnalysisRequestDto from '../api/dto/createRecipeAnalysisRequestDto';

export const useGetAllRecipes = (params: GetAllRecipesRequestDto) => {
    return useQuery({
        queryKey: ['getAllRecipes', params],
        queryFn: () => getRecipes(params)
    })
}

export const usePostRecipeAnalysis = () => {
    return useMutation({
        mutationFn: (payload: CreateRecipeAnalysisRequestDto) => postRecipeAnalysis(payload),
        onSuccess: (data) => {
            console.log('Post created successfully:', data);
        },
        onError: (error: Error) => {
            console.error('Failed to create post:', error.message);
        }
    })
} 