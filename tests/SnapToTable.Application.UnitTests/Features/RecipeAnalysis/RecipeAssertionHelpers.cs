using Shouldly;
using SnapToTable.Application.DTOs;
using SnapToTable.Domain.Entities;

namespace SnapToTable.Application.UnitTests.Features.RecipeAnalysis;

public static class RecipeAssertionHelpers
{
    public static void ShouldMatch(this Recipe entity, RecipeExtractionResultDto dto)
    {
        entity.ShouldNotBeNull();
        dto.ShouldNotBeNull();

        entity.Name.ShouldBe(dto.Name);
        entity.Category.ShouldBe(dto.Category);
        entity.PrepTime.ShouldBe(dto.PrepTime);
        entity.CookTime.ShouldBe(dto.CookTime);
        entity.AdditionalTime.ShouldBe(dto.AdditionalTime);
        entity.Servings.ShouldBe(dto.Servings);
        
        entity.Ingredients.ShouldBe(dto.Ingredients);
        entity.Directions.ShouldBe(dto.Directions);
        entity.Notes.ShouldBe(dto.Notes);
    }
}