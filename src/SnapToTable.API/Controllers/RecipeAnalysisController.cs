using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapToTable.API.DTOs;
using SnapToTable.Application.DTOs;
using SnapToTable.Application.Features.RecipeAnalysis.Create;

namespace SnapToTable.API.Controllers;

public class RecipeAnalysisController : ApiBaseController
{
    public RecipeAnalysisController(ISender mediator) : base(mediator)
    {
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateRecipeAnalysis([FromForm] CreateRecipeAnalysisRequestDto requestDto)
    {
        var command = new CreateRecipeAnalysisCommand(
            requestDto.Images.Select(img => new ImageInputDto(
                img.OpenReadStream(),
                img.ContentType
            )).ToList()
        );

        var result = await Mediator.Send(command);
        
        return Ok(result);
    }
    
}