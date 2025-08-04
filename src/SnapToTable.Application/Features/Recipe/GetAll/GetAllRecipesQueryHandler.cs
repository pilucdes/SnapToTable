using MediatR;
using SnapToTable.Application.DTOs;
using SnapToTable.Domain.Entities;
using SnapToTable.Domain.Repositories;

namespace SnapToTable.Application.Features.Recipe.GetAll;

public class GetAllRecipesQueryHandler : IRequestHandler<GetAllRecipesQuery,
    PagedResultDto<RecipeSummaryDto>>
{
    private readonly IRecipeRepository _repository;

    public GetAllRecipesQueryHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDto<RecipeSummaryDto>> Handle(GetAllRecipesQuery request,
        CancellationToken cancellationToken)
    {
        var pagedResult =
            await _repository.GetPagedAsync(request.Filter, request.RecipeAnalysisId,
                p => new RecipeSummary(p.RecipeAnalysisId, p.Name, p.Category, p.Url, p.Ingredients)
                    { Id = p.Id, CreatedAt = p.CreatedAt }, request.Page,
                request.PageSize);

        var dtoItems = pagedResult.Items.Select(r => new RecipeSummaryDto(
            r.Id, r.CreatedAt, r.RecipeAnalysisId,
            r.Name, r.Category, r.Url, r.Ingredients)).ToList();

        return new PagedResultDto<RecipeSummaryDto>(dtoItems, pagedResult.TotalCount, request.Page,
            request.PageSize);
    }
}