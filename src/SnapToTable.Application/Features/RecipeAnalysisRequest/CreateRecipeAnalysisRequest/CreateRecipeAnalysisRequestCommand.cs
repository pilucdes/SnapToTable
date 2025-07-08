using MediatR;
using SnapToTable.Application.DTOs;

namespace SnapToTable.Application.Features.RecipeAnalysisRequest.CreateRecipeAnalysisRequest;

public record CreateRecipeAnalysisRequestCommand(IReadOnlyList<ImageInputDto> Images) : IRequest<Guid>;