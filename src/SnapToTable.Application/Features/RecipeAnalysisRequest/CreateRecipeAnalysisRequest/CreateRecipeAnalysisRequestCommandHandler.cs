using MediatR;
using SnapToTable.Application.Contracts;
using SnapToTable.Domain.Entities;
using SnapToTable.Domain.Repositories;

// using SnapToTable.Domain.Repositories;

namespace SnapToTable.Application.Features.RecipeAnalysisRequest.CreateRecipeAnalysisRequest;

public class CreateRecipeAnalysisRequestCommandHandler : IRequestHandler<CreateRecipeAnalysisRequestCommand, Guid>
{
    private readonly IRecipeAnalysisRequestRepository _repository;
    private readonly IAiRecipeExtractionService _aiRecipeExtractionService;

    public CreateRecipeAnalysisRequestCommandHandler(IRecipeAnalysisRequestRepository repository,
        IAiRecipeExtractionService aiRecipeExtractionService)
    {
        _repository = repository;
        _aiRecipeExtractionService = aiRecipeExtractionService;
    }

    public async Task<Guid> Handle(CreateRecipeAnalysisRequestCommand request, CancellationToken cancellationToken)
    {
        var extractedRecipes = await _aiRecipeExtractionService.GetRecipeFromImagesAsync(request.Images, cancellationToken);

        var newAnalysis = new Domain.Entities.RecipeAnalysisRequest(extractedRecipes.Select(recipe =>
            new Recipe(recipe.Name,
                recipe.Category,
                recipe.PrepTime,
                recipe.CookTime,
                recipe.AdditionalTime,
                recipe.Servings,
                recipe.Ingredients,
                recipe.Directions,
                recipe.Notes)).ToList());

        await _repository.AddAsync(newAnalysis);
        return newAnalysis.Id;
    }
}