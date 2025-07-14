using MediatR;
using SnapToTable.Application.DTOs;

namespace SnapToTable.Application.Features.Recipe.GetAll;

public record GetAllRecipesQuery(
    Guid? RecipeAnalysisId,
    string? Filter,
    int PageNumber, int PageSize = 20)
    : IRequest<PagedResultDto<RecipeDto>>;