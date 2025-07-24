import PagedResult from "../types/pagedResult";
import Recipe from "../types/recipe";
import PagedResultDto from "./dto/pagedResultDto";
import RecipeDto from "./dto/recipeDto";

const parseDurationToMinutes = (duration: string | null): number | null => {
    if (!duration) return null;
    // e.g., "PT30M" -> 30, "PT1H15M" -> 75
    const matches = duration.match(/PT(?:(\d+)H)?(?:(\d+)M)?/);
    if (!matches) return null;
    const hours = parseInt(matches[1] || '0', 10);
    const minutes = parseInt(matches[2] || '0', 10);
    return hours * 60 + minutes;
};

const getTotalTimeInMinutes = (prepTimeInMinutes: number | null, cookTimeInMinutes: number | null, additionalTimeInMinutes: number | null) => {
    return [prepTimeInMinutes, cookTimeInMinutes, additionalTimeInMinutes]
        .reduce((sum, time) => sum || 0 + (time || 0), 0);
}
export const mapRecipeDtoToRecipe = (dto: RecipeDto): Recipe => {

    const prepTimeInMinutes = parseDurationToMinutes(dto.prepTime);
    const cookTimeInMinutes = parseDurationToMinutes(dto.cookTime);
    const additionalTimeInMinutes = parseDurationToMinutes(dto.additionalTime);
    const totalTimeInMinutes = getTotalTimeInMinutes(prepTimeInMinutes, cookTimeInMinutes, additionalTimeInMinutes);

    return {
        id: dto.id,
        createdAt: new Date(dto.createdAt),
        recipeAnalysisId: dto.recipeAnalysisId,
        name: dto.name,
        category: dto.category,
        servings: dto.servings,
        ingredients: dto.ingredients,
        directions: dto.directions,
        notes: dto.notes,

        prepTimeInMinutes,
        cookTimeInMinutes,
        additionalTimeInMinutes,
        totalTimeInMinutes
    };
};

export const mapPagedResultDtoToPagedResult = <TDto, TModel>(dtoToMap: PagedResultDto<TDto>, itemMapper: (dto: TDto) => TModel) => {
    const mappedItems = dtoToMap.items.map(itemMapper);
    return new PagedResult<TModel>(mappedItems, dtoToMap.totalCount, dtoToMap.page, dtoToMap.pageSize);
};
