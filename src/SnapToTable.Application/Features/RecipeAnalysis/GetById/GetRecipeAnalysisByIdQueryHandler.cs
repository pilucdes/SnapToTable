using MediatR;
using SnapToTable.Application.Contracts;
using SnapToTable.Application.DTOs;
using SnapToTable.Application.Exceptions;
using SnapToTable.Domain.Repositories;

namespace SnapToTable.Application.Features.RecipeAnalysis.GetById;

public class
    GetRecipeAnalysisByIdQueryHandler : IRequestHandler<GetRecipeAnalysisByIdQuery,
    RecipeAnalysisDto>
{
    private readonly IRecipeAnalysisRepository _repository;

    public GetRecipeAnalysisByIdQueryHandler(IRecipeAnalysisRepository repository,
        IAiRecipeExtractionService aiRecipeExtractionService)
    {
        _repository = repository;
    }

    public async Task<RecipeAnalysisDto> Handle(GetRecipeAnalysisByIdQuery byId,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(byId.Id);

        if (result == null)
            throw new NotFoundException(nameof(Domain.Entities.RecipeAnalysis), byId.Id);

        return new RecipeAnalysisDto(result.Id, result.CreatedAt,
            result.Recipes.Select(r => new RecipeDto(r.Name, r.Category, r.PrepTime, r.CookTime, r.AdditionalTime,
                r.Servings, r.Ingredients, r.Directions, r.Notes)).ToList());
    }
}