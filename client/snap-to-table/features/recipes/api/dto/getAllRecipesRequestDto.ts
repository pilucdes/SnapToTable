import paginatedRequestDto from "@/features/common/dto/paginatedRequestDto";

export default interface GetAllRecipesRequestDto extends paginatedRequestDto {
    recipeAnalysisId:string;
    filter:string;
} 