using MediatR;
using SnapToTable.Application.DTOs;
using SnapToTable.Application.Exceptions;
using SnapToTable.Domain.Repositories;

namespace SnapToTable.Application.Features.Recipe.GetById;

public class
    GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery,
    RecipeDto>
{
    private readonly IRecipeRepository _repository;

    public GetRecipeByIdQueryHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<RecipeDto> Handle(GetRecipeByIdQuery byId,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(byId.Id);

        if (result == null)
            throw new NotFoundException(nameof(Domain.Entities.RecipeAnalysis), byId.Id);

        return new RecipeDto(result.Id, result.CreatedAt, result.RecipeAnalysisId,
            result.Name, result.Category,
            result.Url, result.PrepTime,
            result.CookTime, result.AdditionalTime,
            result.Servings, result.Ingredients, result.Directions, result.Notes);
    }
}