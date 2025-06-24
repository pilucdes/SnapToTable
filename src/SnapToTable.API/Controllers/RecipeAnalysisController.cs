using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapToTable.API.DTOs;
using SnapToTable.Application.DTOs;
using SnapToTable.Application.Features.RecipeAnalysisRequest.CreateRecipeAnalysisRequest;

namespace SnapToTable.API.Controllers;

public class RecipeAnalysisController : ApiBaseController
{
    public RecipeAnalysisController(ISender mediator) : base(mediator)
    {
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateAnalysis([FromForm] CreateRecipeAnalysisRequest request)
    {
        var command = new CreateRecipeAnalysisRequestCommand(
            request.Images.Select(img => new ImageInput(
                img.OpenReadStream(),
                img.ContentType
            )).ToList()
        );

        var result = await Mediator.Send(command);
        
        return Ok(result);
        
        // return CreatedAtAction(
        //     nameof(GetAnalysisById),         // 1. The name of the GET action
        //     new { id = resultDto.Id },        // 2. The route values for the GET action
        //     resultDto                       // 3. The object to return in the response body
        // );
    }

}