import {useQuery, useMutation, useInfiniteQuery} from '@tanstack/react-query';
import {getRecipes, getRecipeById, postRecipeAnalysis} from '../api/recipeApi';
import {router} from 'expo-router';
import {CreateRecipeAnalysisRequestDto, GetAllRecipesRequestDto} from '../api/dto';

type UseGetAllRecipesParams = Omit<GetAllRecipesRequestDto, 'page'>;

export const useGetAllRecipes = ({recipeAnalysisId, filter, pageSize}: UseGetAllRecipesParams) => {
    return useInfiniteQuery({
        queryKey: ['getAllRecipes', {recipeAnalysisId, filter, pageSize}],
        queryFn: ({pageParam}) => getRecipes({
            page: pageParam,
            pageSize,
            recipeAnalysisId,
            filter
        }),
        getNextPageParam: (params) => {
            if (params.hasNextPage) {
                return params.page + 1;
            }
        },
        initialPageParam: 1
    });
}
export const useGetRecipeById = (id: string) => {
    return useQuery({
        queryKey: ['getRecipeById', id],
        queryFn: () => getRecipeById(id)
    });
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