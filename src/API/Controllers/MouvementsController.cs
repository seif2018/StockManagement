using Application.Features.Mouvements.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MouvementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MouvementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var mouvements = await _mediator.Send(new GetMouvementsQuery());
        return Ok(mouvements);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "Date",
        [FromQuery] bool descending = true,
        [FromQuery] string? search = null)
    {
        var query = new GetMouvementsPagedQuery(page, pageSize, sortBy, descending, search);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}