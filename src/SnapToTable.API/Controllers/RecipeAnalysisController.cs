using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapToTable.API.DTOs;
using SnapToTable.Application.DTOs;
using SnapToTable.Application.Features.RecipeAnalysisRequest.CreateRecipeAnalysisRequest;
using SnapToTable.Application.Features.RecipeAnalysisRequest.GetRecipeAnalysisRequestDetails;

namespace SnapToTable.API.Controllers;

public class RecipeAnalysisController : ApiBaseController
{
    public RecipeAnalysisController(ISender mediator) : base(mediator)
    {
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateAnalysis([FromForm] CreateRecipeAnalysisRequestDto requestDto)
    {
        var command = new CreateRecipeAnalysisRequestCommand(
            requestDto.Images.Select(img => new ImageInputDto(
                img.OpenReadStream(),
                img.ContentType
            )).ToList()
        );

        var result = await Mediator.Send(command);

        return Ok(result);

        // return CreatedAtAction(
        //     nameof(GetAnalysisById),
        //     new { id = result },
        //     result
        // );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAnalysisById(Guid id)
    {
        var query = new GetRecipeAnalysisDetailsRequestQuery(id);

        var result = await Mediator.Send(query);

        return Ok(result);
        
    }
}