using MediatR;
using SnapToTable.Application.Contracts;
using SnapToTable.Domain.Repositories;

namespace SnapToTable.Application.Features.RecipeAnalysis.Create;

public class CreateRecipeAnalysisCommandHandler : IRequestHandler<CreateRecipeAnalysisCommand, Guid>
{
    private readonly IRecipeAnalysisRepository _analysisRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IAiRecipeExtractionService _aiRecipeExtractionService;

    public CreateRecipeAnalysisCommandHandler(IRecipeAnalysisRepository analysisRepository,
        IRecipeRepository recipeRepository,
        IAiRecipeExtractionService aiRecipeExtractionService)
    {
        _analysisRepository = analysisRepository;
        _recipeRepository = recipeRepository;
        _aiRecipeExtractionService = aiRecipeExtractionService;
    }

    public async Task<Guid> Handle(CreateRecipeAnalysisCommand request, CancellationToken cancellationToken)
    {
        var extractedRecipes =
            await _aiRecipeExtractionService.GetRecipeFromImagesAsync(request.Images, cancellationToken);

        var newAnalysis = new Domain.Entities.RecipeAnalysis();
        await _analysisRepository.AddAsync(newAnalysis);

        var recipes = extractedRecipes.Select(recipe =>
            new Domain.Entities.Recipe(
                newAnalysis.Id,
                recipe.Name,
                recipe.Category,
                recipe.PrepTime,
                recipe.CookTime,
                recipe.AdditionalTime,
                recipe.Servings,
                recipe.Ingredients,
                recipe.Directions,
                recipe.Notes)).ToList();

        await _recipeRepository.AddRangeAsync(recipes);

        return newAnalysis.Id;
    }
}