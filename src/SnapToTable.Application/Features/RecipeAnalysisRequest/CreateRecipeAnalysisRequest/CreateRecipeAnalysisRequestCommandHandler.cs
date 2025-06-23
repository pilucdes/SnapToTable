using MediatR;
using SnapToTable.Application.Contracts;
using SnapToTable.Domain.Repositories;

// using SnapToTable.Domain.Repositories;

namespace SnapToTable.Application.Features.RecipeAnalysisRequest.CreateRecipeAnalysisRequest;

public class CreateRecipeAnalysisRequestCommandHandler : IRequestHandler<CreateRecipeAnalysisRequestCommand, Guid>
{
    private readonly IRecipeAnalysisRequestRepository _repository;
    private readonly IRecipeExtractionService _extractionService;

    public CreateRecipeAnalysisRequestCommandHandler(IRecipeAnalysisRequestRepository repository,
        IRecipeExtractionService extractionService)
    {
        _repository = repository;
        _extractionService = extractionService;
    }

    public async Task<Guid> Handle(CreateRecipeAnalysisRequestCommand request, CancellationToken cancellationToken)
    {
        var extractedRecipes = await _extractionService.GetRecipeFromImagesAsync(request.Images, cancellationToken);

        foreach (var recipe in extractedRecipes)
        {
            var newAnalysis = new Domain.Entities.RecipeAnalysisRequest(
                recipe.Name,
                recipe.Category,
                recipe.PrepTime,
                recipe.CookTime,
                recipe.AdditionalTime,
                recipe.Servings,
                recipe.Ingredients,
                recipe.Directions,
                recipe.Notes
            );

            await _repository.AddAsync(newAnalysis);
        }

        return Guid.CreateVersion7();
    }
}