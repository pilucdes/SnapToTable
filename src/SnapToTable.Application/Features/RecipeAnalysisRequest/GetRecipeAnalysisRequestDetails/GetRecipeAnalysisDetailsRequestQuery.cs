using MediatR;
using SnapToTable.Application.DTOs;

namespace SnapToTable.Application.Features.RecipeAnalysisRequest.GetRecipeAnalysisRequestDetails;

public record GetRecipeAnalysisDetailsRequestQuery(Guid Id) : IRequest<RecipeAnalysisRequestDto>;