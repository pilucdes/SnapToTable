using MediatR;
using SnapToTable.Application.DTOs;
using SnapToTable.Application.Features.Common;

namespace SnapToTable.Application.Features.Recipe.GetAll;

public record GetAllRecipesQuery(
    Guid? RecipeAnalysisId,
    string? Filter,
    int Page,
    int PageSize = 20)
    : IPaginatedQuery, IRequest<PagedResultDto<RecipeSummaryDto>>;