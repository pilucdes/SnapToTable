import paginatedRequestDto from "@/features/common/dto/paginatedRequestDto";
export interface GetAllRecipesRequestDto extends paginatedRequestDto {
    recipeAnalysisId:string;
    filter:string;
} 