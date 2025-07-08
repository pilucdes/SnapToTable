using MediatR;
using SnapToTable.Application.DTOs;

namespace SnapToTable.Application.Features.RecipeAnalysis.Create;

public record CreateRecipeAnalysisCommand(IReadOnlyList<ImageInputDto> Images) : IRequest<Guid>;