using MediatR;
using SnapToTable.Application.DTOs;

namespace SnapToTable.Application.Features.RecipeAnalysis.GetById;

public record GetRecipeAnalysisByIdQuery(Guid Id) : IRequest<RecipeAnalysisDto>;