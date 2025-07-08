using MediatR;
using SnapToTable.Application.Contracts;
using SnapToTable.Application.DTOs;
using SnapToTable.Application.Exceptions;
using SnapToTable.Domain.Repositories;

namespace SnapToTable.Application.Features.RecipeAnalysisRequest.GetRecipeAnalysisRequestDetails;

public class
    GetRecipeAnalysisDetailsRequestQueryHandler : IRequestHandler<GetRecipeAnalysisDetailsRequestQuery,
    RecipeAnalysisRequestDto>
{
    private readonly IRecipeAnalysisRequestRepository _repository;

    public GetRecipeAnalysisDetailsRequestQueryHandler(IRecipeAnalysisRequestRepository repository,
        IAiRecipeExtractionService aiRecipeExtractionService)
    {
        _repository = repository;
    }

    public async Task<RecipeAnalysisRequestDto> Handle(GetRecipeAnalysisDetailsRequestQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id);

        if (result == null)
            throw new NotFoundException(nameof(Domain.Entities.RecipeAnalysisRequest), request.Id);

        return new RecipeAnalysisRequestDto(result.Id, result.CreatedAt,
            result.Recipes.Select(r => new RecipeDto(r.Name, r.Category, r.PrepTime, r.CookTime, r.AdditionalTime,
                r.Servings, r.Ingredients, r.Directions, r.Notes)).ToList());
    }
}