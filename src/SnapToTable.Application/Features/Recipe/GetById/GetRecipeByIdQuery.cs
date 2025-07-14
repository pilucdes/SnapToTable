using MediatR;
using SnapToTable.Application.DTOs;

namespace SnapToTable.Application.Features.Recipe.GetById;

public record GetRecipeByIdQuery(Guid Id) : IRequest<RecipeDto>;