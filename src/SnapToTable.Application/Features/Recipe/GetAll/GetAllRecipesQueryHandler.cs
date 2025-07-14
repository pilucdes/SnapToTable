using MediatR;
using SnapToTable.Application.DTOs;
using SnapToTable.Domain.Repositories;

namespace SnapToTable.Application.Features.Recipe.GetAll;

public class GetAllRecipesQueryHandler : IRequestHandler<GetAllRecipesQuery,
    PagedResultDto<RecipeDto>>
{
    private readonly IRecipeRepository _repository;

    public GetAllRecipesQueryHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDto<RecipeDto>> Handle(GetAllRecipesQuery request,
        CancellationToken cancellationToken)
    {
        var pagedResult =
            await _repository.GetPagedAsync(request.Filter, request.RecipeAnalysisId, p => p, request.PageNumber,
                request.PageSize);

        var dtoItems = pagedResult.Items.Select(r => new RecipeDto(r.Name, r.Category, r.PrepTime, r.CookTime,
            r.AdditionalTime,
            r.Servings, r.Ingredients, r.Directions, r.Notes)).ToList();

        return new PagedResultDto<RecipeDto>(dtoItems, pagedResult.TotalCount, request.PageNumber,
            request.PageSize);
    }
}