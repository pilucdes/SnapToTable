using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapToTable.API.DTOs;
using SnapToTable.Application.Features.Recipe.GetAll;
using SnapToTable.Application.Features.Recipe.GetById;

namespace SnapToTable.API.Controllers;

public class RecipesController : ApiBaseController
{
    public RecipesController(ISender mediator) : base(mediator)
    {
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRecipeById(Guid id)
    {
        var query = new GetRecipeByIdQuery(id);

        var result = await Mediator.Send(query);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRecipes([FromQuery] GetAllRecipesRequestDto request)
    {
        var query = new GetAllRecipesQuery(request.RecipeAnalysisId, request.Filter, request.Page, request.PageSize);

        var result = await Mediator.Send(query);

        return Ok(result);
    }
}