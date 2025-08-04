using Moq;
using Shouldly;
using SnapToTable.Application.Exceptions;
using SnapToTable.Application.Features.Recipe.GetById;
using SnapToTable.Domain.Repositories;
using SnapToTable.Tests.Common.Builders;
using Xunit;

namespace SnapToTable.Application.UnitTests.Features.Recipe.GetById;

public class GetRecipeByIdQueryHandlerTests
{
    private readonly Mock<IRecipeRepository> _mockRepo;
    private readonly GetRecipeByIdQueryHandler _handler;

    public GetRecipeByIdQueryHandlerTests()
    {
        _mockRepo = new Mock<IRecipeRepository>();
        _handler = new GetRecipeByIdQueryHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_WhenIsFound_ShouldReturnCorrectRecipeDto()
    {
        // Arrange
        var query = new GetRecipeByIdQuery(Guid.NewGuid());

        var recipeEntity = new RecipeBuilder()
            .WithId(query.Id)
            .WithCreatedAt(DateTime.UtcNow)
            .WithName("Test Recipe 1")
            .WithUrl("http://www.test.com")
            .WithCategory("Test Category")
            .WithServings(4)
            .WithIngredients(["Ingredient A", "Ingredient B"])
            .Build();

        _mockRepo.Setup(r => r.GetByIdAsync(query.Id))
            .ReturnsAsync(recipeEntity);

        // Act
        var resultDto = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultDto.ShouldNotBeNull();
        resultDto.Id.ShouldBe(recipeEntity.Id);
        resultDto.CreatedAt.ShouldBe(recipeEntity.CreatedAt, tolerance: TimeSpan.FromSeconds(1));
        resultDto.Name.ShouldBe("Test Recipe 1");
        resultDto.Category.ShouldBe("Test Category");
        resultDto.Url.ShouldBe("http://www.test.com");
        resultDto.Servings.ShouldBe(4);
        resultDto.Ingredients.ShouldBe(["Ingredient A", "Ingredient B"]);
    }

    [Fact]
    public async Task Handle_WhenIsNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var query = new GetRecipeByIdQuery(Guid.NewGuid());

        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.Recipe)null!);

        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));
        
        exception.Message.ShouldContain(nameof(Recipe));
        exception.Message.ShouldContain(query.Id.ToString());
    }
}