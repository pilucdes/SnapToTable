using MediatR;
using SnapToTable.Application.Contracts;
using SnapToTable.Domain.Entities;
using SnapToTable.Domain.Repositories;

namespace SnapToTable.Application.Features.RecipeAnalysis.Create;

public class CreateRecipeAnalysisCommandHandler : IRequestHandler<CreateRecipeAnalysisCommand, Guid>
{
    private readonly IRecipeAnalysisRepository _repository;
    private readonly IAiRecipeExtractionService _aiRecipeExtractionService;

    public CreateRecipeAnalysisCommandHandler(IRecipeAnalysisRepository repository,
        IAiRecipeExtractionService aiRecipeExtractionService)
    {
        _repository = repository;
        _aiRecipeExtractionService = aiRecipeExtractionService;
    }

    public async Task<Guid> Handle(CreateRecipeAnalysisCommand request, CancellationToken cancellationToken)
    {
        var extractedRecipes = await _aiRecipeExtractionService.GetRecipeFromImagesAsync(request.Images, cancellationToken);

        var newAnalysis = new Domain.Entities.RecipeAnalysis(extractedRecipes.Select(recipe =>
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